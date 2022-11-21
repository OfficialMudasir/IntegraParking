#region Directives
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Models;
using integraMobile.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
#endregion
#region Namespace
namespace integraMobile.Controllers
{
    #region Class
    public class AppealController : Controller
    {
        #region Properties
        /// <summary>
        /// Properties
        /// </summary>
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(FineController));
        private IFineRepository fineRepository;
        private IAppealRepository appealRepository;
        private IInfraestructureRepository infraestructureRepository;
        private ICustomersRepository customersRepository;
        private IGeograficAndTariffsRepository geograficAndTariffsRepository;
        private integraMobile.Services.ExternalService external = new Services.ExternalService();
        static string _xmlTagName = "ipark";
        private const string OUT_SUFIX = "_out";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_appealRepository"></param>
        /// <param name="_fineRepository"></param>
        /// <param name="_infraestructureRepository"></param>
        /// <param name="_customersRepository"></param>
        /// <param name="_geograficAndTariffsRepository"></param>
        public AppealController(IAppealRepository _appealRepository, IFineRepository _fineRepository, IInfraestructureRepository _infraestructureRepository, ICustomersRepository _customersRepository, IGeograficAndTariffsRepository _geograficAndTariffsRepository)
        {
            this.fineRepository = _fineRepository;
            this.infraestructureRepository = _infraestructureRepository;
            this.customersRepository = _customersRepository;
            this.geograficAndTariffsRepository = _geograficAndTariffsRepository;
            this.appealRepository = _appealRepository;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// GET: /Appeal/
        /// </summary>
        /// <param name="init"></param>
        /// <param name="OpId"></param>
        /// <param name="InstallationId"></param>
        /// <param name="Culture"></param>
        /// <param name="TicketNumber"></param>
        /// <returns></returns>
        public ActionResult Appeal(bool? init, decimal? OpId, decimal? InstallationId, string Culture, string TicketNumber = "")
        {
            m_Log.LogMessage(LogLevels.logINFO, string.Format("Fine={0} {1} {2} {3} {4}", init, OpId, InstallationId, Culture, TicketNumber));

            // New Changes

            TempData.Remove("AppealModelData");
            TempData.Remove("AppealTicketSearchData");
            TempData.Remove("AppealPersonalInfoData");
            TempData.Remove("AppealPITicketSearchData");
            TempData.Remove("AppealAllTicketData");
           // End

            Session["Theme"] = "theme-Blinkay";
            Session["FineModel"] = null;
            Session["CityName"] = null;

            Response.Cache.SetCacheability(HttpCacheability.Private, "Community=DEV");
            RewriteReturnURL(init, OpId, InstallationId, Culture);

            if ((OpId.HasValue || InstallationId.HasValue) && !String.IsNullOrEmpty(this.Request.Url.AbsoluteUri.ToString()) && Session["ReturnFine"] == null)
            {
                int iInicio = this.Request.Url.AbsoluteUri.ToString().LastIndexOf("?");
                if (iInicio != -1)
                {
                    string path = this.Request.Url.AbsoluteUri.Substring(iInicio, this.Request.Url.AbsoluteUri.Length - iInicio);
                    Session["ReturnFine"] = path;
                }
            }
            else if (Session["ReturnFine"] != null && !OpId.HasValue && !InstallationId.HasValue)
            {
                Session["ReturnFine"] = null;
            }

            ViewData["OpId"] = OpId;
            ViewData["InstallationId"] = InstallationId;
            Session["PayPal_Enabled"] = false;
            decimal? dInsOpId = null;

            INSTALLATION[] inss = null;

            if (InstallationId != null)
            {
                inss = infraestructureRepository.Installations
                                        .Where(r => r.INS_WEB_PORTAL_PAYMENT_ENABLED == 1 &&
                                                    r.INS_OPT_TICKET == 1 &&
                                                    r.INS_ID == InstallationId).ToArray();

                ViewData["InstallationsOptionList"] = inss;
                if (inss.Length == 1)
                {
                    Session["CityName"] = inss[0].INS_DESCRIPTION;


                    dInsOpId = infraestructureRepository.FinanDistOperatorsInstallation
                                                           .Where(r => r.INSTALLATION.INS_WEB_PORTAL_PAYMENT_ENABLED == 1 &&
                                                                       r.INSTALLATION.INS_OPT_TICKET == 1 &&
                                                                       DateTime.UtcNow >= r.FDOI_INI_APPLY_DATE &&
                                                                       DateTime.UtcNow <= r.FDOI_END_APPLY_DATE &&
                                                                       r.FDOI_INS_ID == InstallationId)
                                                           .Select(r => r.FDOI_FDO_ID).FirstOrDefault();



                }
                else
                {
                    ViewData["InstallationId"] = null;
                    InstallationId = null;
                    inss = null;

                }
            }
            else
            {

                if (Request.Params["OpId"] != null)
                {
                    inss = infraestructureRepository.FinanDistOperatorsInstallation
                                                            .Where(r => r.INSTALLATION.INS_WEB_PORTAL_PAYMENT_ENABLED == 1 &&
                                                                        r.INSTALLATION.INS_OPT_TICKET == 1 &&
                                                                        DateTime.UtcNow >= r.FDOI_INI_APPLY_DATE &&
                                                                        DateTime.UtcNow <= r.FDOI_END_APPLY_DATE &&
                                                                        r.FDOI_FDO_ID == OpId)
                                                            .Select(r => r.INSTALLATION).ToArray();

                    if (inss.Length == 0)
                    {
                        OpId = null;
                        inss = null;
                    }
                    else
                    {
                        dInsOpId = OpId;
                    }


                }
                else if (Session["RequestOpId"] != null && OpId.HasValue)
                {
                    inss = infraestructureRepository.FinanDistOperatorsInstallation
                                                            .Where(r => r.INSTALLATION.INS_WEB_PORTAL_PAYMENT_ENABLED == 1 &&
                                                                        r.INSTALLATION.INS_OPT_TICKET == 1 &&
                                                                        DateTime.UtcNow >= r.FDOI_INI_APPLY_DATE &&
                                                                        DateTime.UtcNow <= r.FDOI_END_APPLY_DATE &&
                                                                        r.FDOI_FDO_ID == OpId)
                                                            .Select(r => r.INSTALLATION).ToArray();
                    if (inss.Length == 0)
                    {
                        inss = null;
                    }

                }

            }


            if (inss == null)
            {
                inss = infraestructureRepository.Installations
                                                           .Where(r => r.INS_WEB_PORTAL_PAYMENT_ENABLED == 1 && r.INS_OPT_TICKET == 1).ToArray();
            }

            ViewData["InstallationsOptionList"] = inss;
            foreach (INSTALLATION i in inss.ToList())
            {
                if (i.INS_PAYPAL_CPTGC_ID != null)
                {
                    CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG cptgc = infraestructureRepository.PaymentGateways.Where(r => r.CPTGC_ID == i.INS_PAYPAL_CPTGC_ID).FirstOrDefault();
                    if (cptgc != null)
                    {
                        if (cptgc.CPTGC_ENABLED == 1)
                        {
                            Session["PayPal_Enabled"] = true;
                            break;
                        }
                    }
                }
            }


            ViewData["ShowCultureSelector"] = true;
            ViewData["Show_en-US"] = true;
            ViewData["Show_fr-CA"] = true;
            ViewData["Show_es-ES"] = true;
            ViewData["Show_ca-ES"] = true;

            switch (dInsOpId.ToString())
            {
                case "400004":
                    Session["Theme"] = "theme-CWP";
                    break;
                case "400005":
                    Session["Theme"] = "theme-Dube";
                    break;
                case "400006":
                    Session["Theme"] = "theme-Spark";
                    break;
                case "400007":
                    Session["Theme"] = "theme-ParkSafe";
                    break;
                case "400008":
                    Session["Theme"] = "theme-SafeWay";
                    break;
                case "400009":
                    Session["Theme"] = "theme-Gestac";
                    break;
                case "400010":
                    Session["Theme"] = "theme-SocServStatQuebec";
                    break;
                case "400015":
                    Session["Theme"] = "theme-Parkeo";
                    break;
            }

            if (dInsOpId.HasValue)
            {
                if (dInsOpId >= 400000 && dInsOpId < 500000)
                {
                    ViewData["Show_es-ES"] = false;
                    ViewData["Show_ca-ES"] = false;
                }
            }

            AppealModel model = new AppealModel();
            model.Init(appealRepository);

            string InstallationList = string.Empty;
            string StandardInstallationList = string.Empty;
            foreach (INSTALLATION i in (Array)ViewData["InstallationsOptionList"])
            {
                InstallationList += string.Format(",{0}", i.INS_ID);
                StandardInstallationList += string.Format(",{0}", i.INS_STANDARD_CITY_ID);
            }
            if (!string.IsNullOrEmpty(InstallationList))
            {
                InstallationList = InstallationList.Substring(1);
                StandardInstallationList = StandardInstallationList.Substring(1);
            }
            model.InstallationList = InstallationList;
            model.StandardInstallationList = StandardInstallationList;

            Session["FineModel"] = model;

            if (InstallationId != null)
            {
                foreach (INSTALLATION i in (Array)ViewData["InstallationsOptionList"])
                {
                    if (i.INS_ID == InstallationId)
                    {
                        model.InstallationId = (int)InstallationId;
                        model.ForceInstallationId = InstallationId;
                        model.BlockInstallationCombo = true;
                        if (OpId != null)
                        {
                            model.BlockInstallationCombo = false;
                        }
                        break;
                    }
                }
            }
            model.ForceTicketNumber = TicketNumber;

            /*if (Session["ChangeCulture"] == null || (Session["ChangeCulture"] != null && (bool)Session["ChangeCulture"]))
            {*/
            if (Culture != null)
            {
                if (Session["Culture"] != null &&
                    (((CultureInfo)Session["Culture"]).Name != Culture))
                {
                    try
                    {
                        Session["Culture"] = new CultureInfo(Culture);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        Session["Culture"] = new CultureInfo("en-US");
                        Culture = "en-US";
                    }

                    //return RedirectToAction("Fine", new { init = init, OpId = OpId, InstallationId = InstallationId, Culture = Culture });
                }
            }
            else
            {
                if (Session["Culture"] == null)
                {
                    Session["Culture"] = new CultureInfo("en-US");
                    Culture = "en-US";
                }

            }
            /*Session["ChangeCulture"] = false;
        }*/


            //SetCulture();

            /* START IPT-260 */
            //if (OpId.HasValue)
            //{
            //Session["CustomLogo"] = GetCustomConfigurationForOpId(OpId, "CUSTOM_LOGOS_PER_OPID");
            //Session["CustomInfo"] = GetCustomConfigurationForOpId(OpId, "CUSTOM_INFO_PER_OPID", (Session["Culture"] != null ? Session["Culture"].ToString() : Culture));
            //Session["CustomElementsToHide"] = GetCustomConfigurationForOpId(OpId, "CUSTOM_ELEMENTS_TO_HIDE");
            /* START IPS-198 */
            //Session["CustomNavbarBackgroundColor"] = GetCustomConfigurationForOpId(OpId, "CUSTOM_NAVBAR_BACKGROUND_COLOR");
            //}
            //else
            //{
            //Session["CustomLogo"] = null;
            //Session["CustomInfo"] = null;
            //Session["CustomElementsToHide"] = null;
            //Session["CustomNavbarBackgroundColor"] = null;
            //}
            /* END IPT-260 */

            return View(model);
            //return View();
        }

        
        //save appeal information
        //[HttpPost]
        public ActionResult SaveAppealInformation(string response, string Back)
        {
            return View();
        }

        public ActionResult TicketPaymentSummary(AppealReturnModel model, string submitButton)
        {
            return View(model);
        }

        /// <summary>
        /// RedirectToView
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RedirectToView(string data)
        {
            return Json(new { success = true });
        }


        /// <summary>
        /// SaveAppealInformation
        /// <see cref="ActionResult"/>
        /// </summary>
        /// <param name="appealReturnModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAppealInformation(AppealReturnModel appealReturnModel)
        {
            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "Enter Save Appeal Information");
                if (!String.IsNullOrEmpty(appealReturnModel.AppealData.AppealImageLists))
                {
                    Tools.QuickLog("Random Log");

                    m_Log.LogMessage(LogLevels.logINFO, "Entering into condition");
                    Tools.QuickLog("Entering into condition : Line  351");


                    List<AppealReturnModel.TicketAppealData.AppealImages> ImageList = JsonConvert.DeserializeObject<List<AppealReturnModel.TicketAppealData.AppealImages>>(appealReturnModel.AppealData.AppealImageLists);
                    m_Log.LogMessage(LogLevels.logINFO,"Parsed ImageList success");
                    Tools.QuickLog(String.Format("Parsed ImageList success"));


                    appealReturnModel.AppealData.AppealImageList = ImageList;
                    if (appealReturnModel != null)
                    {
                        if (String.IsNullOrEmpty(appealReturnModel.Plate))
                        {
                            appealReturnModel.Plate = appealReturnModel.SearchedTicket != null &&
                                !String.IsNullOrEmpty(appealReturnModel.SearchedTicket.ticketlprplate) ?
                                appealReturnModel.SearchedTicket.ticketlprplate :
                                appealReturnModel.SearchedTicket.ticketqueryplate;
                        }

                        appealReturnModel.Init(appealRepository);
                        AppealResponse response = SaveAppeal(ref appealReturnModel);

                        if (response != null)
                        {
                            appealReturnModel.AppealData.AppealResponse = response;
                            Tools.QuickLog(String.Format("Appeal Response Updated"));
                            m_Log.LogMessage(LogLevels.logINFO, "Appeal Response Updated");

                        }
                    }
                }
                else
                {
                    Tools.QuickLog(String.Format("Appeal Images String is null or empty"));
                    m_Log.LogMessage(LogLevels.logINFO,"Appeal Images String is null or empty");
                }
                
                return View(appealReturnModel);
            }
            catch (Exception ex)
            {
                Tools.QuickLog(String.Format("Message : {0} and Stack Trace : {1}",ex.Message, ex.StackTrace));
                m_Log.LogMessage(LogLevels.logERROR, ex.Message);
                m_Log.LogMessage(LogLevels.logERROR, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// uploadData Appeal Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="appealData"></param>
        /// <param name="InstallationDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult uploadData(string data, string appealData, string InstallationDetails)
        {
            try
            {
                if(!(string.IsNullOrEmpty(data) && string.IsNullOrEmpty(appealData) && string.IsNullOrEmpty(InstallationDetails)))
                {
                    // Convert the json data into model properties 

                    AppealReturnModel returnModel = new AppealReturnModel();
                    AppealReturnModel.TicketAppealData model = new AppealReturnModel.TicketAppealData();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    returnModel = serializer.Deserialize<AppealReturnModel>(InstallationDetails);
                    model = serializer.Deserialize<AppealReturnModel.TicketAppealData>(appealData);
                    List<AppealReturnModel.TicketAppealData.AppealImages> images = serializer.Deserialize<List<AppealReturnModel.TicketAppealData.AppealImages>>(data);
                    model.AppealImageList = images;
                    returnModel.StandardInstallationList = "40069";
                    returnModel.InstallationList = Convert.ToString(returnModel.InstallationId);
                    returnModel.AppealData = model;

                    if(returnModel != null)
                    {
                        returnModel.Init(appealRepository);
                        AppealResponse response = SaveAppeal(ref returnModel);
                        if(response != null)
                        {
                            if(response.Result == (int?)ExternalWS.ResultType.Result_OK)
                            {
                                return Json(new { success = true, message = response.Message, resultType = (int?)ExternalWS.ResultType.Result_OK });
                            }
                            if(response.Result == (int?)ExternalWS.ResultType.Result_Error_Email_Already_Exist)
                            {
                                return Json(new { success = true, message = response.Message, resultType = (int ?)ExternalWS.ResultType.Result_Error_Email_Already_Exist });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = response.Message, resultType = response.Result });
                        }
                    }
                    return Json(new { success = false, message = "fail", resultType = -999 });
                }
                else
                {
                    return Json(new { success = false, message = "UnSuccessful", resultType = -999 });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "UnSuccessful", resultType = -999 });
            }
        }

        /// <summary>
        /// PersonalInformation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="submitButton"></param>
        /// <param name="TicketID"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult PersonalInformation(AppealReturnModel model, string submitButton, long? TicketID)
        {
            long? TicketId = null;

            // New Changes
            
            bool _isTicketCheckChanged = false;
            if (model != null && model.SearchedTicket != null && model.TicketId > 0 && !string.IsNullOrEmpty(model.SearchedTicket.ticketnumber))
            {
                TempData["AppealPersonalInfoData"] = JsonConvert.SerializeObject(model);
                TempData.Keep("AppealPersonalInfoData");
                _isTicketCheckChanged = true;
            }
            else if (TempData["AppealPersonalInfoData"] != null)
            {
                var _modeldata = TempData["AppealPersonalInfoData"];
                model = JsonConvert.DeserializeObject<AppealReturnModel>(_modeldata.ToString());
                TempData.Keep("AppealPersonalInfoData");
            }
            // End

            try
            {
                if (!String.IsNullOrEmpty(submitButton))
                {
                    TicketId = Convert.ToInt32(submitButton);
                }
                else
                {
                    TicketId = (long)(model.SearchedTicket.ticketid != 0 ? model.SearchedTicket.ticketid : model.TicketId);
                }

            }
            catch (Exception)
            { }
           if(TicketId != null)
            {
                if (model.SearchedTicket.ticketlprplate == null || model.InstallationList == null)
                {
                    model.SearchedTicket.ticketlprplate = model.SearchedTicket.ticketqueryplate;
                    
                    model.InstallationList = Convert.ToString(model.InstallationId) != null ? Convert.ToString(model.InstallationId) : Convert.ToString(model.SearchedTicket.InstallationID);
                    model.StandardInstallationList = "40069";
                }
                model.Init(appealRepository);
                GetTicketFines(ref model, TicketId);

                // Converting the longititude and latitude into actual address

                // New Chnages
                if (TempData["AppealPITicketSearchData"] != null && model.SearchedTicket != null && !_isTicketCheckChanged)
                {
                    var _modeldata = TempData["AppealPITicketSearchData"];
                    model = JsonConvert.DeserializeObject<AppealReturnModel>(_modeldata.ToString());
                    TempData.Keep("AppealPITicketSearchData");
                } // END
                else if (model.SearchedTicket != null)
                {
                    // calculating amount
                    model.SearchedTicket.TickBalance = (Convert.ToDouble(model.SearchedTicket.TickBalance) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode));
                    model.SearchedTicket.Address = getAddress((double)model.SearchedTicket.ticketlatitude, (double)model.SearchedTicket.ticketlongitude);

                    // New Chnages

                    TempData["AppealPITicketSearchData"] = JsonConvert.SerializeObject(model);
                    TempData.Keep("AppealPITicketSearchData");

                    // END
                }

                DateTime? dt = null;
                INSTALLATION oInstallation = null;
                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dt);
                if (oInstallation != null && oInstallation.INS_PAYPAL_CPTGC_ID != null)
                {
                    CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetGatewayConfig(oInstallation.INS_PAYPAL_CPTGC_ID.Value);
                    if (oGatewayConfig != null && oGatewayConfig.CPTGC_ENABLED == 1 && oGatewayConfig.CPTGC_IS_INTERNAL == 0)
                    {
                        Session["PaymentPayPalButton"] = 1;
                    }
                }
                else
                {
                    Session["PaymentPayPalButton"] = null;
                }

                if (oInstallation != null)
                {
                    model.CurrencyId = oInstallation.CURRENCy.CUR_ID;
                    model.InstallationShortDesc = oInstallation.INS_SHORTDESC;
                    model.CountryCode = oInstallation.COUNTRy.COU_CODE;
                }

                if (model.Email == null) model.Email = ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString();

                if(model.SearchedTicket != null)
                {
                    ViewData["AppealReturnModel"] = model.SearchedTicket;
                    Session["AppealReturnModel"] = model.SearchedTicket;

                    model.PriceCalculationDate = System.DateTime.Now;
                    Session["TicketNumber"] = model.SearchedTicket.ticketnumber;
                    ViewData["TicketNumber"] = model.SearchedTicket.ticketnumber;
                }
                else
                {
                    ViewData["AppealReturnModel"] = model;
                    Session["AppealReturnModel"] = model;

                    model.PriceCalculationDate = System.DateTime.Now;
                    Session["TicketNumber"] = model.ticketnumber;
                }
            }
            return View(model);
        }

        /// <summary>
        /// Details
        /// </summary>
        /// <param name="model"></param>
        /// <param name="submitButton"></param>
        /// <param name="Ticketid"></param>
        /// <param name="btnBackJson"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult Details(AppealReturnModel model, string submitButton, long? Ticketid, string btnBackJson)
        {

            //SetCulture();

            // Back Button functionality
            // New Changes
            if (model != null && model.ForceInstallationId > 0 && !string.IsNullOrEmpty(model.ticketnumber) && !string.IsNullOrEmpty(model.Plate))
            {
                TempData["AppealModelData"] = JsonConvert.SerializeObject(model);
                TempData.Keep("AppealModelData");
            }
            else if (TempData["AppealModelData"] != null)
            {
                var _modeldata = TempData["AppealModelData"];
                model = JsonConvert.DeserializeObject<AppealReturnModel>(_modeldata.ToString());
                TempData.Keep("AppealModelData");
            }
            //End
            if (!String.IsNullOrEmpty(btnBackJson) && !String.IsNullOrEmpty(submitButton))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                model = serializer.Deserialize<AppealReturnModel>(btnBackJson);
            }
            if (!String.IsNullOrEmpty(submitButton))
            {
                if (String.IsNullOrEmpty(model.Plate)) model.Plate = model.SearchedTicket.ticketlprplate;
                if (model.ForceInstallationId == null) model.ForceInstallationId = model.SearchedTicket.InstallationID;

            }
            
            if (!string.IsNullOrEmpty(model.Plate))
            {
                model.Plate = model.Plate.Trim().ToUpper();
                model.Plate = Regex.Replace(model.Plate, "[^a-zA-Z0-9]", string.Empty);
            }

            if (
                (model.InstallationId > 0 && !string.IsNullOrEmpty(model.ticketnumber) && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.ConfirmEmail) && model.Email == model.ConfirmEmail)
                ||
                (!string.IsNullOrEmpty(model.Plate) && !string.IsNullOrEmpty(model.ticketnumber))
            )
            {
                if (model.ForceInstallationId != null && model.InstallationId == 0)
                {
                    model.InstallationId = (int)model.ForceInstallationId;
                }

                model.Init(appealRepository);
                //GetFineDetails(ref model);
                GetTicketFines(ref model,  Ticketid);

                
                // Converting the longititude and latitude into actual address
                List<ExternalWS.TicketReturnModel> selectedtickets = new List<ExternalWS.TicketReturnModel>();

                // New Changes

                if (TempData["AppealTicketSearchData"] != null && model.SearchedTicket != null)
                {
                    var _modeldata = TempData["AppealTicketSearchData"];
                    model = JsonConvert.DeserializeObject<AppealReturnModel>(_modeldata.ToString());
                    TempData.Keep("AppealTicketSearchData");
                } //END
                else if(model.SearchedTicket != null)
                {
                    // Calculate the Amount
                    model.SearchedTicket.TickBalance = (Convert.ToDouble(model.SearchedTicket.TickBalance) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode));
                    model.SearchedTicket.Address = getAddress((double)model.SearchedTicket.ticketlatitude, (double)model.SearchedTicket.ticketlongitude);
                    
                    //model.SelectedTickets.Add(returnModel);
                    selectedtickets.Add(model.SearchedTicket);
                    model.isChecked = true;

                    //New Changes
                    TempData["AppealTicketSearchData"] = JsonConvert.SerializeObject(model);
                    TempData.Keep("AppealTicketSearchData");

                    //End
                }
                if (model.AllTickets != null)
                {
                    // New Changes
                    if (TempData["AppealAllTicketData"] != null && model.AllTickets.Count > 0)
                    {
                        var _modeldata = TempData["AppealAllTicketData"];
                        model = JsonConvert.DeserializeObject<AppealReturnModel>(_modeldata.ToString());
                        TempData.Keep("AppealAllTicketData");
                    }// End
                    else if (model.AllTickets.Count > 0)
                    {
                        model.isChecked = true;
                        foreach (var ticket in model.AllTickets)
                        {
                            if (ticket != null)
                            {
                                ticket.TickBalance =  (Convert.ToDouble(ticket.TickBalance) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode));
                                ticket.Address = getAddress(Convert.ToDouble(ticket.ticketlatitude), Convert.ToDouble(ticket.ticketlongitude));
                            }
                            selectedtickets.Add(ticket);
                        }
                        // New Changes
                        TempData["AppealAllTicketData"] = JsonConvert.SerializeObject(model);
                        TempData.Keep("AppealAllTicketData");
                        //End
                    }
                }
                // Assign all tickets as selscted tickets by default
                if(selectedtickets != null)
                {
                    model.SelectedTickets = selectedtickets;
                }
                

                DateTime? dt = null;
                INSTALLATION oInstallation = null;
                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dt);
                if (oInstallation != null && oInstallation.INS_PAYPAL_CPTGC_ID != null)
                {
                    CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetGatewayConfig(oInstallation.INS_PAYPAL_CPTGC_ID.Value);
                    if (oGatewayConfig != null && oGatewayConfig.CPTGC_ENABLED == 1 && oGatewayConfig.CPTGC_IS_INTERNAL == 0)
                    {
                        Session["PaymentPayPalButton"] = 1;
                    }
                }
                else
                {
                    Session["PaymentPayPalButton"] = null;
                }

                if (oInstallation != null)
                {
                    model.CurrencyId = oInstallation.CURRENCy.CUR_ID;
                    model.InstallationShortDesc = oInstallation.INS_SHORTDESC;
                    model.CountryCode = oInstallation.COUNTRy.COU_CODE;
                }

                if (model.Email == null) model.Email = ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString();

                ViewData["AppealReturnModel"] = model;
                Session["AppealReturnModel"] = model;

                model.PriceCalculationDate = System.DateTime.Now;
                Session["TicketNumber"] = model.ticketnumber;

                return View(model);
            }
            else
            {
                
                if (!string.IsNullOrEmpty(submitButton))
                {
                    if (Session["ReturnFine"] != null)
                    {
                        return RedirecToactionBySession();
                    }
                }
                model.Init(appealRepository);

                // New Changes
                Session["searchtext"] = model;
                return View(model);
                //End
                //return Redirect("Details");
            }
            //return View();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Get Address
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static ExternalWS.TicketAddress getAddress1(double lat, double lon)
        {
            ExternalWS.TicketAddress completeAddress = null;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            var jsonData = webClient.DownloadData("http://nominatim.openstreetmap.org/reverse?format=json&lat=" + lat + "&lon=" + lon);
            Dictionary<String, Object> Addresses = parse(jsonData);

            foreach(var address in Addresses)
            {
                if(address.Key == "address")
                {
                    completeAddress = JsonConvert.DeserializeObject<ExternalWS.TicketAddress>(address.Value.ToString());
                }
            }

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            //RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            return completeAddress;
        }
        
        //New changes
        public static ExternalWS.TicketAddress getAddress(double lat, double lon)
        {

            ExternalWS.TicketAddress completeAddress = null;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            // webClient.Headers.Add("Content-Type","text/xml");            
            webClient.Encoding = Encoding.UTF8;
            string url = lat + "&lon = " + lon;
            url = url.Replace(",", ".");
            url = url.Replace(" ", "");
            var jsonData = webClient.DownloadData("http://nominatim.openstreetmap.org/reverse?format=json&lat=" + url);
            Dictionary<String, Object> Addresses = parse(jsonData);

            foreach (var address in Addresses)
            {
                if (address.Key == "address")
                {
                    completeAddress = JsonConvert.DeserializeObject<ExternalWS.TicketAddress>(address.Value.ToString());
                }
            }

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            //RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            return completeAddress;
        }

        //End

        /// <summary>
        /// RedirecToactionBySession
        /// </summary>
        /// <returns></returns>
        private ActionResult RedirecToactionBySession()
        {
            bool? binit = null;
            decimal? dOpId = null;
            decimal? dInstallationId = null;
            string sCulture = string.Empty;
            if (Session["ReturnFine"] != null)
            {
                string linkRedirect = Session["ReturnFine"].ToString();
                linkRedirect = linkRedirect.Replace("?", "");
                foreach (string cadena in linkRedirect.Split('&'))
                {
                    if (cadena.Contains("OpId"))
                    {
                        dOpId = Convert.ToDecimal(find(cadena, "OpId"));
                    }
                    else if (cadena.Contains("InstallationId"))
                    {
                        dInstallationId = Convert.ToDecimal(find(cadena, "InstallationId"));
                    }
                    else if (cadena.Contains("Culture"))
                    {
                        sCulture = find(cadena, "Culture");
                    }
                    else if (cadena.Contains("init"))
                    {
                        binit = Convert.ToBoolean(find(cadena, "init"));
                    }
                }
            }
            Session["ReturnFine"] = null;
            return RedirectToAction("Appeal", new { init = binit, OpId = dOpId, InstallationId = dInstallationId, Culture = sCulture });
        }


        #region My new chanegs in methods

        public static Dictionary<String, Object> parse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonStr);
        }
        public void GetTicketFines(ref AppealReturnModel model, long? Ticketid)
        {

            decimal? InstallationIdFound = null;
            XmlReturnModel xmlReturnModel = null;
            string Plate = model.Plate;
            if (model.SearchedTicket != null) Plate = model.SearchedTicket.ticketqueryplate;
            if (Ticketid != null)
            {
                xmlReturnModel = external.GetTicketFine(model.ticketnumber, Ticketid, model.InstallationId, appealRepository, fineRepository, customersRepository, infraestructureRepository, geograficAndTariffsRepository, out InstallationIdFound, null, Plate, model.InstallationList, model.StandardInstallationList);
                model.SearchedTicket = xmlReturnModel.SearchedTicket;
            }
            else
            {
                xmlReturnModel = external.GetTicketFine(model.ticketnumber, Ticketid, model.InstallationId, appealRepository, fineRepository, customersRepository, infraestructureRepository, geograficAndTariffsRepository, out InstallationIdFound, null, Plate, model.InstallationList, model.StandardInstallationList);
                model.AllTickets = xmlReturnModel.AllTickets;
                model.SearchedTicket = xmlReturnModel.SearchedTicket;
            }
            string fineDetails = xmlReturnModel.XML;
            if (fineDetails != null)
            {
                model.InstallationId = Convert.ToInt32(InstallationIdFound);
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    xmlInDoc.LoadXml(fineDetails);
                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        if (Node.Name != "ah")
                        {
                            if (Node.HasChildNodes)
                            {
                                if (Node.ChildNodes[0].HasChildNodes)
                                {
                                    foreach (XmlNode ChildNode in Node.ChildNodes)
                                    {
                                        switch (Node.Name + "_" + ChildNode.Name)
                                        {
                                            case "d":
                                                DateTime dt = DateTime.ParseExact(ChildNode.InnerText.Trim(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                                model.TicketDate = dt;
                                                break;
                                            case "cur_minor_unit":
                                                model.NumDecimals = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_partial_vat1":
                                                model.PartialVAT1 = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_fee":
                                                model.QFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_total":
                                                model.TotalQuantity = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_vat":
                                                model.QVAT = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "dPercVAT1":
                                                model.PercVAT1 = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                break;
                                            case "dPercVAT2":
                                                model.PercVAT2 = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                break;
                                            case "dPercFEE":
                                                model.PercFEE = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                break;
                                            case "iPercFEETopped":
                                                model.PercFEETopped = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "iPartialPercFEE":
                                                model.PartialPercFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "iFixedFEE":
                                                model.FixedFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "iPartialFixedFEE":
                                                model.PartialFixedFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "eTaxMode":
                                                model.TaxMode = (IsTAXMode)Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch (Node.Name)
                                    {
                                        case "d":
                                            DateTime dt = DateTime.ParseExact(Node.InnerText.Trim(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                            model.TicketDate = dt;
                                            break;
                                        case "cur":
                                            model.AmountCurrencyIsoCode = Node.InnerText.Trim();
                                            break;
                                        case "q":
                                            model.Total = Convert.ToInt32(Node.InnerText.Trim());
                                            model.Quantity = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "r":
                                            model.Result = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "lp":
                                            model.Plate = Node.InnerText.Trim();
                                            break;
                                        case "cur_minor_unit":
                                            model.NumDecimals = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_partial_vat1":
                                            model.PartialVAT1 = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_vat":
                                            model.QVAT = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_fee":
                                            model.QFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_total":
                                            model.TotalQuantity = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_vat_percent":
                                            model.VAT_Percent = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "dPercVAT1":
                                            model.PercVAT1 = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "dPercVAT2":
                                            model.PercVAT2 = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "dPercFEE":
                                            model.PercFEE = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "iPercFEETopped":
                                            model.PercFEETopped = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "iPartialPercFEE":
                                            model.PartialPercFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "iFixedFEE":
                                            model.FixedFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "iPartialFixedFEE":
                                            model.PartialFixedFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "eTaxMode":
                                            model.TaxMode = (IsTAXMode)Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "GrpId":
                                            model.GrpId = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "AuthId":
                                            model.AuthId = Node.InnerText.Trim();
                                            break;


                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }


        public AppealResponse SaveAppeal(ref AppealReturnModel model)
        {
            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "Entering into SaveAppeal 1 Method");
                Tools.QuickLog(String.Format("Entering into SaveAppeal 1 Method"));

                decimal? InstallationIdFound = null;
                string fineDetails = "";
                AppealResponse response = external.SaveAppeal(model, model.SearchedTicket.ticketnumber, model.InstallationId, appealRepository, fineRepository, customersRepository, infraestructureRepository, geograficAndTariffsRepository, out InstallationIdFound, null, model.Plate, model.InstallationList, model.StandardInstallationList);
                if (response != null)
                {

                    fineDetails = response.xmlOut;
                }
                if (fineDetails != null)
                {
                    model.InstallationId = Convert.ToInt32(InstallationIdFound);
                    XmlDocument xmlInDoc = new XmlDocument();
                    try
                    {
                        xmlInDoc.LoadXml(fineDetails);
                        XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                        foreach (XmlNode Node in Nodes)
                        {
                            if (Node.Name != "ah")
                            {
                                if (Node.HasChildNodes)
                                {
                                    if (Node.ChildNodes[0].HasChildNodes)
                                    {
                                        foreach (XmlNode ChildNode in Node.ChildNodes)
                                        {
                                            switch (Node.Name + "_" + ChildNode.Name)
                                            {
                                                case "d":
                                                    DateTime dt = DateTime.ParseExact(ChildNode.InnerText.Trim(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                                    model.TicketDate = dt;
                                                    break;
                                                case "cur_minor_unit":
                                                    model.NumDecimals = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "q_partial_vat1":
                                                    model.PartialVAT1 = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "q_fee":
                                                    model.QFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "q_total":
                                                    model.TotalQuantity = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "q_vat":
                                                    model.QVAT = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "dPercVAT1":
                                                    model.PercVAT1 = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                    break;
                                                case "dPercVAT2":
                                                    model.PercVAT2 = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                    break;
                                                case "dPercFEE":
                                                    model.PercFEE = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                    break;
                                                case "iPercFEETopped":
                                                    model.PercFEETopped = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "iPartialPercFEE":
                                                    model.PartialPercFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "iFixedFEE":
                                                    model.FixedFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "iPartialFixedFEE":
                                                    model.PartialFixedFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                                case "eTaxMode":
                                                    model.TaxMode = (IsTAXMode)Convert.ToInt32(ChildNode.InnerText.Trim());
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        switch (Node.Name)
                                        {
                                            case "d":
                                                DateTime dt = DateTime.ParseExact(Node.InnerText.Trim(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                                model.TicketDate = dt;
                                                break;
                                            case "cur":
                                                model.AmountCurrencyIsoCode = Node.InnerText.Trim();
                                                break;
                                            case "q":
                                                model.Total = Convert.ToInt32(Node.InnerText.Trim());
                                                model.Quantity = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "r":
                                                model.Result = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "lp":
                                                model.Plate = Node.InnerText.Trim();
                                                break;
                                            case "cur_minor_unit":
                                                model.NumDecimals = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "q_partial_vat1":
                                                model.PartialVAT1 = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "q_vat":
                                                model.QVAT = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "q_fee":
                                                model.QFEE = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "q_total":
                                                model.TotalQuantity = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "q_vat_percent":
                                                model.VAT_Percent = Convert.ToDecimal(Node.InnerText.Trim());
                                                break;
                                            case "dPercVAT1":
                                                model.PercVAT1 = Convert.ToDecimal(Node.InnerText.Trim());
                                                break;
                                            case "dPercVAT2":
                                                model.PercVAT2 = Convert.ToDecimal(Node.InnerText.Trim());
                                                break;
                                            case "dPercFEE":
                                                model.PercFEE = Convert.ToDecimal(Node.InnerText.Trim());
                                                break;
                                            case "iPercFEETopped":
                                                model.PercFEETopped = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "iPartialPercFEE":
                                                model.PartialPercFEE = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "iFixedFEE":
                                                model.FixedFEE = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "iPartialFixedFEE":
                                                model.PartialFixedFEE = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "eTaxMode":
                                                model.TaxMode = (IsTAXMode)Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "GrpId":
                                                model.GrpId = Convert.ToInt32(Node.InnerText.Trim());
                                                break;
                                            case "AuthId":
                                                model.AuthId = Node.InnerText.Trim();
                                                break;


                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        Tools.QuickLog(String.Format("Error from Save Appeal Method 1 : {0}",e.Message));

                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                Tools.QuickLog(String.Format("Error from Save Appeal Method 1 : {0}", ex.Message));

                throw;
            }
        }


        #endregion

        public void GetFineDetails(ref AppealModel model)
        {

            decimal? InstallationIdFound = null;
            string fineDetails = external.GetFine(model.TicketNumber, model.InstallationId, fineRepository, customersRepository, infraestructureRepository, geograficAndTariffsRepository, out InstallationIdFound, null, model.Plate, model.InstallationList, model.StandardInstallationList);
            if (fineDetails != null)
            {
                model.InstallationId = Convert.ToInt32(InstallationIdFound);
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    xmlInDoc.LoadXml(fineDetails);
                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        if (Node.Name != "ah")
                        {
                            if (Node.HasChildNodes)
                            {
                                if (Node.ChildNodes[0].HasChildNodes)
                                {
                                    foreach (XmlNode ChildNode in Node.ChildNodes)
                                    {
                                        switch (Node.Name + "_" + ChildNode.Name)
                                        {
                                            case "d":
                                                DateTime dt = DateTime.ParseExact(ChildNode.InnerText.Trim(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                                model.TicketDate = dt;
                                                break;
                                            case "cur_minor_unit":
                                                model.NumDecimals = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_partial_vat1":
                                                model.PartialVAT1 = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_fee":
                                                model.QFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_total":
                                                model.TotalQuantity = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "q_vat":
                                                model.QVAT = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "dPercVAT1":
                                                model.PercVAT1 = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                break;
                                            case "dPercVAT2":
                                                model.PercVAT2 = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                break;
                                            case "dPercFEE":
                                                model.PercFEE = Convert.ToDecimal(ChildNode.InnerText.Trim());
                                                break;
                                            case "iPercFEETopped":
                                                model.PercFEETopped = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "iPartialPercFEE":
                                                model.PartialPercFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "iFixedFEE":
                                                model.FixedFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "iPartialFixedFEE":
                                                model.PartialFixedFEE = Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                            case "eTaxMode":
                                                model.TaxMode = (IsTAXMode)Convert.ToInt32(ChildNode.InnerText.Trim());
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch (Node.Name)
                                    {
                                        case "d":
                                            DateTime dt = DateTime.ParseExact(Node.InnerText.Trim(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                            model.TicketDate = dt;
                                            break;
                                        case "cur":
                                            model.AmountCurrencyIsoCode = Node.InnerText.Trim();
                                            break;
                                        case "q":
                                            model.Total = Convert.ToInt32(Node.InnerText.Trim());
                                            model.Quantity = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "r":
                                            model.Result = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "lp":
                                            model.Plate = Node.InnerText.Trim();
                                            break;
                                        case "cur_minor_unit":
                                            model.NumDecimals = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_partial_vat1":
                                            model.PartialVAT1 = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_vat":
                                            model.QVAT = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_fee":
                                            model.QFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_total":
                                            model.TotalQuantity = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "q_vat_percent":
                                            model.VAT_Percent = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "dPercVAT1":
                                            model.PercVAT1 = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "dPercVAT2":
                                            model.PercVAT2 = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "dPercFEE":
                                            model.PercFEE = Convert.ToDecimal(Node.InnerText.Trim());
                                            break;
                                        case "iPercFEETopped":
                                            model.PercFEETopped = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "iPartialPercFEE":
                                            model.PartialPercFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "iFixedFEE":
                                            model.FixedFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "iPartialFixedFEE":
                                            model.PartialFixedFEE = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "eTaxMode":
                                            model.TaxMode = (IsTAXMode)Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "GrpId":
                                            model.GrpId = Convert.ToInt32(Node.InnerText.Trim());
                                            break;
                                        case "AuthId":
                                            model.AuthId = Node.InnerText.Trim();
                                            break;


                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }
        private void RewriteReturnURL(bool? binit, decimal? dOpId, decimal? dInstallationId, string sCulture)
        {
            if (Session["ReturnFine"] != null)
            {
                bool bIsDifferent = false;
                string linkRedirect = Session["ReturnFine"].ToString();
                linkRedirect = linkRedirect.Replace("?", "");
                foreach (string cadena in linkRedirect.Split('&'))
                {
                    if (cadena.Contains("OpId"))
                    {
                        bIsDifferent = (!dOpId.HasValue || Convert.ToDecimal(find(cadena, "OpId")) != dOpId.Value);
                    }
                    else if (cadena.Contains("InstallationId"))
                    {
                        bIsDifferent = (!dInstallationId.HasValue || Convert.ToDecimal(find(cadena, "InstallationId")) != dInstallationId.Value);
                    }
                    else if (cadena.Contains("Culture"))
                    {
                        bIsDifferent = (string.IsNullOrEmpty(sCulture) || find(cadena, "Culture") != sCulture);
                    }
                    else if (cadena.Contains("init"))
                    {
                        bIsDifferent = (!binit.HasValue || Convert.ToBoolean(find(cadena, "init")) != binit.Value);
                    }

                    if (bIsDifferent)
                        break;
                }

                if (bIsDifferent)
                {
                    string stringURL = "?";

                    if (binit.HasValue)
                    {
                        stringURL += string.Format("{0}={1}", "init", binit);
                    }

                    if (dOpId.HasValue)
                    {
                        if (stringURL != "?")
                            stringURL += "&";
                        stringURL += string.Format("{0}={1}", "OpId", dOpId);
                    }

                    if (dInstallationId.HasValue)
                    {
                        if (stringURL != "?")
                            stringURL += "&";
                        stringURL += string.Format("{0}={1}", "InstallationId", dInstallationId);
                    }

                    if (!string.IsNullOrEmpty(sCulture))
                    {
                        if (stringURL != "?")
                            stringURL += "&";
                        stringURL += string.Format("{0}={1}", "Culture", sCulture);
                    }

                    Session["ReturnFine"] = stringURL;

                }


            }

            //return RedirectToAction("Fine", new { init = binit, OpId = dOpId, InstallationId = dInstallationId, Culture = sCulture });
        }

        private string find(string cadena, string find)
        {
            string sValue = string.Empty;
            if (!string.IsNullOrEmpty(cadena))
            {
                if (cadena.Contains(find))
                {
                    sValue = cadena.Remove(0, cadena.IndexOf("=") + 1);
                }
            }
            return sValue;
        }
        #endregion
    }
    #endregion
}

#endregion