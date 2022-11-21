using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using integraMobile.Models;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.WS;
using integraMobile.Infrastructure.Logging.Tools;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Threading;
using Newtonsoft.Json;
using integraMobile.Properties;
using System.Resources;
using integraMobile.Infrastructure;
using integraMobile.Domain.Concrete;
using integraMobile.ExternalWS;

using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using System.Globalization;
using integraMobile.Web.Resources;
using NPOI.HSSF.UserModel;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace integraMobile.Controllers
{
    public class PermitsController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;
        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infrastructureRepository;
        private SQLGeograficAndTariffsRepository geograficRepository;
        private const string RequiredRole = "IPKMINSCRIPTIONS_WRITE";
        private const char NonBreakableSpace = (char)160;
        private const int DEFAULT_MAX_GRID_NUM_OPS = 20;

        private integraMobile.ExternalWS.WSintegraMobile wswi = null;
        ResourceManager resBundle = Resources.ResourceManager;
        protected static CLogWrapper m_Log;
        private string cs = ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ToString();

        #region Constructor        

        public PermitsController()
        {
            IBackOfficeRepository boR = new SQLBackOfficeRepository(cs);
            ICustomersRepository cuR = new SQLCustomersRepository(cs);
            IInfraestructureRepository isR = new SQLInfraestructureRepository(cs);
            SQLGeograficAndTariffsRepository gR = new SQLGeograficAndTariffsRepository(cs);
            this.backOfficeRepository = boR;
            this.customersRepository = cuR;
            this.infrastructureRepository = isR;
            this.geograficRepository = gR;

            wswi = new integraMobile.ExternalWS.WSintegraMobile(boR, cuR, isR, gR);
            m_Log = new CLogWrapper(typeof(PermitsController));
        }

        #endregion

        #region Aux

        public void UpdatePaymentParamsDate()
        {
            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            if (Model != null)
            {
                Model.PaymentParams.UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
                Model.PaymentParams.Hash = wswi.CalculateHash(Model.PaymentParams.Guid, Model.PaymentParams.Email, Model.PaymentParams.Amount, Model.PaymentParams.CurrencyISOCODE, Model.PaymentParams.Description, Model.PaymentParams.UTCDate, Model.PaymentParams.Culture, Model.PaymentParams.ReturnURL, Model.HashSeed);
                Session["CurrentModel"] = Model;
            }
        }

        private string GetStringOrDefault(string Key)
        {
            string value = resBundle.GetString(Key);
            if (string.IsNullOrEmpty(value))
            {
                value = Key.Replace('_', ' ');
            }
            return value;
        }

        #endregion

        #region Login

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    PermitsModel oModel = new PermitsModel();

        //    if (Session["CurrentModel"] != null)
        //    {
        //        if (!string.IsNullOrEmpty(((PermitsModel)Session["CurrentModel"]).Error))
        //        {
        //            oModel.Error = ((PermitsModel)Session["CurrentModel"]).Error;
        //            oModel.InFrame = ((PermitsModel)Session["CurrentModel"]).InFrame;
        //        }
        //    }

        //    USER WebUser = account.GetUserFromSession();
        //    oModel.BackofficeUserCountry = WebUser.USR_COU_ID;

        //    if (oModel.InFrame)
        //    {
        //        oModel.InFrame = false;
        //        Session["CurrentModel"] = oModel.Clone();
        //    }
        //    else
        //    {
        //        Session["CurrentModel"] = oModel.Clone();
        //        ((PermitsModel)Session["CurrentModel"]).Error = string.Empty;
        //    }

        //    return View(oModel);
        //}

        public ActionResult Login(decimal User, string UTC_Offset, string ReturnTo = "PayForPermit")
        {
            SetCulture();
            PermitsModel Model = new PermitsModel();

            try
            {
                USER u = backOfficeRepository.GetUsers(PredicateBuilder.True<USER>())
                                                .Where(t => t.USR_ID == User && t.USR_ENABLED == 1)
                                                .FirstOrDefault();
                Model.User = u.USR_ID;
                Model.Email = u.USR_EMAIL;
                Model.UserName = u.USR_USERNAME;
                Model.CellNumber = u.USR_MAIN_TEL;
                Model.UserCurrency = u.CURRENCy.CUR_ISO_CODE;
                Model.FromLogin = true;
                Model.UTC_Offset = UTC_Offset;
                if (u.USR_USERNAME.ToLower().Contains("@telephone.iparkme.com"))
                {
                    Model.UserNameForSummary = string.Format("({0}) {1}", u.COUNTRy.COU_TEL_PREFIX.Trim().Replace(NonBreakableSpace.ToString(), string.Empty), u.USR_MAIN_TEL);
                }
                else
                {
                    Model.UserNameForSummary = u.USR_USERNAME;
                }
                /*
                List<int> oOptions;
                int oCCProvider;
                string oChargeCurrency;
                ResultType res = wswi.GetSubPayOptions(Convert.ToInt32(u.USR_COU_ID), out oOptions, out oCCProvider, out oChargeCurrency);
                if (res == ResultType.Result_OK)
                {
                    if (string.IsNullOrEmpty(oChargeCurrency))
                    {
                        oChargeCurrency = u.CURRENCy.CUR_ISO_CODE;
                    }
                    Model.CCProvider = oCCProvider.ToString();
                    if (oOptions.Count == 2)
                    {
                        Model.PrepaymentAvailable = oOptions[0];
                        Model.PaypertransactionAvailable = oOptions[1];
                    }
                }

                PaymentParams pp = new PaymentParams();

                pp.Email = u.USR_EMAIL;
                
                int iPayPerTransactionAmount = 0;
                List<string> oPerTransactionParameters = new List<string>();
                oPerTransactionParameters = ConfigurationManager.AppSettings["PermitsSuscriptionType_AddPayMethChargeValue"].ToString().Split(';').ToList();

                for (int i = 0; i < oPerTransactionParameters.Count; i++)
                {
                    if (oPerTransactionParameters[i] == oChargeCurrency)
                    {
                        iPayPerTransactionAmount = Convert.ToInt32(oPerTransactionParameters[i + 1]);
                        break;
                    }
                }
               
                pp.Amount = Convert.ToInt32(iPayPerTransactionAmount.ToString());
                pp.CurrencyISOCODE = oChargeCurrency;
                pp.Description = "Alta tarjeta bancaria";
                pp.UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
                pp.Culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                pp.ReturnURL = ConfigurationManager.AppSettings["PermitsReturnURL2"].ToString();

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG cptgc = u.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard)).FirstOrDefault();
                pp.RequestURL = cptgc.CPTGC_FORM_URL;

                switch (oCCProvider)
                {
                    case 1: // CreditCall                    
                        pp.Guid = cptgc.CREDIT_CALL_CONFIGURATION.CCCON_GUID;
                        pp.Culture = string.Empty;
                        pp.Hash = wswi.CalculateHash(pp.Guid, pp.Email, pp.Amount, pp.CurrencyISOCODE, pp.Description, pp.UTCDate, pp.Culture, pp.ReturnURL, cptgc.CREDIT_CALL_CONFIGURATION.CCCON_HASH_SEED);
                        Model.HashSeed = cptgc.CREDIT_CALL_CONFIGURATION.CCCON_HASH_SEED;
                        break;
                    case 2: // IECISA
                        pp.Guid = cptgc.IECISA_CONFIGURATION.IECCON_GUID;
                        pp.Hash = wswi.CalculateHash(pp.Guid, pp.Email, pp.Amount, pp.CurrencyISOCODE, pp.Description, pp.UTCDate, pp.Culture, pp.ReturnURL, cptgc.IECISA_CONFIGURATION.IECCON_HASH_SEED);
                        Model.HashSeed = cptgc.IECISA_CONFIGURATION.IECCON_HASH_SEED;
                        break;
                    case 4: // Stripe
                        pp.Guid = cptgc.STRIPE_CONFIGURATION.STRCON_GUID;
                        pp.Culture = string.Empty;
                        pp.Hash = wswi.CalculateHash(pp.Guid, pp.Email, pp.Amount, pp.CurrencyISOCODE, pp.Description, pp.UTCDate, pp.Culture, pp.ReturnURL, cptgc.STRIPE_CONFIGURATION.STRCON_HASH_SEED);
                        Model.HashSeed = cptgc.STRIPE_CONFIGURATION.STRCON_HASH_SEED;
                        break;
                    case 5: // Moneris
                        pp.Guid = cptgc.MONERIS_CONFIGURATION.MONCON_GUID;
                        pp.Culture = string.Empty;
                        pp.Hash = wswi.CalculateHash(pp.Guid, pp.Email, pp.Amount, pp.CurrencyISOCODE, pp.Description, pp.UTCDate, pp.Culture, pp.ReturnURL, cptgc.MONERIS_CONFIGURATION.MONCON_HASH_SEED);
                        Model.HashSeed = cptgc.MONERIS_CONFIGURATION.MONCON_HASH_SEED;
                        break;
                    case 6: // Transbank
                        pp.Guid = cptgc.TRANSBANK_CONFIGURATION.TRBACON_GUID;
                        pp.Culture = string.Empty;
                        pp.Hash = wswi.CalculateHash(pp.Guid, pp.Email, pp.Amount, pp.CurrencyISOCODE, pp.Description, pp.UTCDate, pp.Culture, pp.ReturnURL, cptgc.TRANSBANK_CONFIGURATION.TRBACON_HASH_SEED);
                        Model.HashSeed = cptgc.TRANSBANK_CONFIGURATION.TRBACON_HASH_SEED;
                        break;
                    case 7: // Payu
                        pp.RequestURL = cptgc.CPTGC_FORM_URL;
                        pp.Guid = cptgc.PAYU_CONFIGURATION.PAYUCON_GUID;
                        pp.Hash = wswi.CalculateHash(pp.Guid, pp.Email, pp.Amount, pp.CurrencyISOCODE, pp.Description, pp.UTCDate, pp.Culture, pp.ReturnURL, cptgc.PAYU_CONFIGURATION.PAYUCON_HASH_SEED);
                        Model.HashSeed = cptgc.PAYU_CONFIGURATION.PAYUCON_HASH_SEED;
                        break;
                }

                Model.PaymentParams = pp;

                if (u.USR_SUSCRIPTION_TYPE == null)
                {
                    // User without subscription type [paymeth = -1]               
                    /*
                        Solo una opción -> método de pago
                        Varias opciones -> ChangeSubscriptionTypeInternal -> método de pago
                    
                    if (Model.PrepaymentAvailable == 1 && Model.PaypertransactionAvailable == 1)
                    {
                        Session["CurrentModel"] = Model;
                        return RedirectToAction("SelectSuscriptionType", "Account");
                    }
                    else if (Model.PrepaymentAvailable == 1)
                    {
                        Model.SubscriptionType = 1;                        
                        Session["CurrentModel"] = Model;
                        return RedirectToAction("PaymentMethod", "Permits");
                    }
                    else if (Model.PaypertransactionAvailable == 1)
                    {
                        Model.SubscriptionType = 2;                        
                        Session["CurrentModel"] = Model;
                        return RedirectToAction("PaymentMethod", "Permits");
                    }
                }
                else
                {
                    if (u.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                    {
                        Model.SubscriptionType = 1;                        

                        if (u.CUSTOMER_PAYMENT_MEAN != null)
                        {
                            if ((u.CUSTOMER_PAYMENT_MEAN.CUSPM_ENABLED == 1) && (u.CUSTOMER_PAYMENT_MEAN.CUSPM_VALID == 1))
                            {
                                // Prepay credit card [paymeth = 1 || 2 || 3 || 4]                            
                            }
                            else
                            {
                                // Prepay without paymethod [paymeth = 0]
                                Session["CurrentModel"] = Model;
                                return RedirectToAction("PaymentMethod", "Permits");
                            }
                        }
                        else
                        {
                            // Prepay without paymethod [paymeth = 0 || 8]
                            Session["CurrentModel"] = Model;
                            return RedirectToAction("PaymentMethod", "Permits");
                        }
                    }
                    else if (u.USR_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction)
                    {
                        Model.SubscriptionType = 2;                        

                        if (u.CUSTOMER_PAYMENT_MEAN != null)
                        {
                            if ((u.CUSTOMER_PAYMENT_MEAN.CUSPM_ENABLED == 1) && (u.CUSTOMER_PAYMENT_MEAN.CUSPM_VALID == 1))
                            {
                                // Pay per transaction [paymeth = 6 || 7]
                            }
                            else
                            {
                                // Pay per transaction without paymethod [paymeth = 5 || 9]
                                Session["CurrentModel"] = Model;
                                return RedirectToAction("PaymentMethod", "Permits");
                            }
                        }
                        else
                        {
                            // Pay per transaction without paymethod [paymeth = 5 || 9]
                            Session["CurrentModel"] = Model;
                            return RedirectToAction("PaymentMethod", "Permits");
                        }
                    }
                }
                */
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("PermitsController > Login > {0}", ex.Message));                
            }
            Session["CurrentModel"] = Model;
            return RedirectToAction(ReturnTo, "Permits", new { LoginSuccessful = true });
        }

        public JsonResult GetUsers(string text)
        {
            object[] users_emails = backOfficeRepository.GetUsers(PredicateBuilder.True<USER>())
                                            .Select(u => new { id = u.USR_ID, Name = u.USR_USERNAME, Enabled = u.USR_ENABLED })
                                            .Where(u => u.Name.Contains(text) && text != string.Empty && u.Enabled == 1 && !u.Name.ToLower().Contains("@telephone.iparkme.com"))
                                            .ToList().AsQueryable<object>()
                                            .ToArray<object>();

            object[] users_phones = backOfficeRepository.GetUsers(PredicateBuilder.True<USER>())
                                            .Select(u => new { id = u.USR_ID, Name = "(" + u.COUNTRy.COU_TEL_PREFIX.ToString().Trim().Replace(NonBreakableSpace.ToString(), string.Empty) + ") " + u.USR_MAIN_TEL.ToString(), Enabled = u.USR_ENABLED })
                                            .Where(u => u.Name.Contains(text) && text != string.Empty && u.Enabled == 1)
                                            .ToList().AsQueryable<object>()
                                            .ToArray<object>();

            object[] users = users_emails.Concat(users_phones).ToArray();

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PaymentMethod

        [HttpGet]
        public ActionResult PaymentMethod()
        {
            SetCulture();

            if (Session["CurrentModel"] == null)
            {
                m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > PaymentMethod > Forced LogOff > Session[CurrentModel] is null"));
                return RedirectToAction("LogOff", "Account");
            }

            USER oUser = GetUserFromSession();
            ViewData["oUser"] = oUser;

            decimal? dInstallationId = Session["InstallationID"] != null ? Convert.ToDecimal(Session["InstallationID"]) : (decimal?)null;

            ViewData["InstallationID"]= dInstallationId;

            m_Log.LogMessage(LogLevels.logINFO, string.Format("InstallationId={0}", dInstallationId));


            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            return View(Model);
        }

        #endregion        

        #region Manage

        [HttpGet]
        public ActionResult Manage(decimal? OperationId = null, decimal? City = null, decimal? Zone = null, decimal? Tariff = null, string P1 = null, string P2 = null, string P3 = null, string P4 = null, string P5 = null, string P6 = null, string P7 = null, string P8 = null, string P9 = null, string P10 = null, bool LoginSuccessful = false)
        {
            SetCulture();

            try
            {
                if (Session["USER_ID"] == null)
                {
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > Manage [GET] > Forced LogOff > Session[USER_ID] is null"));
                    return RedirectToAction("LogOff", "Account");
                }

                if (!LoginSuccessful)
                {
                    Session["OperationId"] = OperationId;
                    Session["SelectedCity"] = City;
                    Session["SelectedZone"] = Zone;
                    Session["SelectedTariff"] = Tariff;
                    Session["SelectedPlate1"] = P1;
                    Session["SelectedPlate2"] = P2;
                    Session["SelectedPlate3"] = P3;
                    Session["SelectedPlate4"] = P4;
                    Session["SelectedPlate5"] = P5;
                    Session["SelectedPlate6"] = P6;
                    Session["SelectedPlate7"] = P7;
                    Session["SelectedPlate8"] = P8;
                    Session["SelectedPlate9"] = P9;
                    Session["SelectedPlate10"] = P10;
                }

                if (LoginSuccessful && Session["CurrentModel"] != null)
                {
                    PermitsModel Model = (PermitsModel)Session["CurrentModel"];
                    if (Model.User == Convert.ToDecimal(Session["USER_ID"]))
                    {
                        Model.OperationId = (decimal)Session["OperationId"];
                        Model.SelectedCity = (decimal?)Session["SelectedCity"];
                        Model.SelectedZone = (decimal?)Session["SelectedZone"];
                        Model.SelectedTariff = (decimal?)Session["SelectedTariff"];
                        Model.SelectedPlate1 = (Session["SelectedPlate1"] != null ? Session["SelectedPlate1"].ToString() : null);
                        Model.SelectedPlate2 = (Session["SelectedPlate2"] != null ? Session["SelectedPlate2"].ToString() : null);
                        Model.SelectedPlate3 = (Session["SelectedPlate3"] != null ? Session["SelectedPlate3"].ToString() : null);
                        Model.SelectedPlate4 = (Session["SelectedPlate4"] != null ? Session["SelectedPlate4"].ToString() : null);
                        Model.SelectedPlate5 = (Session["SelectedPlate5"] != null ? Session["SelectedPlate5"].ToString() : null);
                        Model.SelectedPlate6 = (Session["SelectedPlate6"] != null ? Session["SelectedPlate6"].ToString() : null);
                        Model.SelectedPlate7 = (Session["SelectedPlate7"] != null ? Session["SelectedPlate7"].ToString() : null);
                        Model.SelectedPlate8 = (Session["SelectedPlate8"] != null ? Session["SelectedPlate8"].ToString() : null);
                        Model.SelectedPlate9 = (Session["SelectedPlate9"] != null ? Session["SelectedPlate9"].ToString() : null);
                        Model.SelectedPlate10 = (Session["SelectedPlate10"] != null ? Session["SelectedPlate10"].ToString() : null);

                        Session["CurrentModel"] = Model;

                        USER oUser = GetUserFromSession();
                        ViewData["oUser"] = oUser;

                        // Needed to get SessionId
                        GetZones(Model.SelectedCity);
                        GetTariffs((decimal)Model.SelectedZone);                        

                        Model = (PermitsModel)Session["CurrentModel"];

                        foreach (Tariff t in Model.TariffList)
                        {
                            if (t.id == Model.SelectedTariff)
                            {
                                Model.MaxLicensePlates = t.MaxLicensePlates;
                            }
                        }

                        Session["CurrentModel"] = Model;

                        return View(Model);
                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > Manage [GET] > Forced LogOff > Model.User ({0}) != Session[USER_ID] ({1})", Model.User, Session["USER_ID"]));
                        return RedirectToAction("LogOff", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Permits", new { User = Session["USER_ID"], UTC_Offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes.ToString(), ReturnTo = "Manage" });
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("PermitsController > Manage [GET] > Forced LogOff > {0}", ex.Message));
                return RedirectToAction("LogOff", "Account");
            }
        }

        [HttpPost]
        public ActionResult Manage(string LicensePlate1 = null, string LicensePlate2 = null, string LicensePlate3 = null, string LicensePlate4 = null, string LicensePlate5 = null, string LicensePlate6 = null, string LicensePlate7 = null, string LicensePlate8 = null, string LicensePlate9 = null, string LicensePlate10 = null)
        {
            SetCulture();

            if (Session["CurrentModel"] == null)
            {
                m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > Manage [POST] > Forced LogOff > Session[CurrentModel] is null"));
                return RedirectToAction("LogOff", "Account");
            }

            PermitsModel Model = (PermitsModel)Session["CurrentModel"];

            try
            {
                Model.Error = null;
                string AllLicensePlates = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", LicensePlate1, LicensePlate2, LicensePlate3, LicensePlate4, LicensePlate5, LicensePlate6, LicensePlate7, LicensePlate8, LicensePlate9, LicensePlate10).Trim();

                decimal n;
                if (Model.OperationId != null && !string.IsNullOrEmpty(AllLicensePlates))
                {
                    List<string> P = ArrangePlates(LicensePlate1, LicensePlate2, LicensePlate3, LicensePlate4, LicensePlate5, LicensePlate6, LicensePlate7, LicensePlate8, LicensePlate9, LicensePlate10);

                    Model.LicensePlate1 = P[0];
                    Model.LicensePlate2 = P[1];
                    Model.LicensePlate3 = P[2];
                    Model.LicensePlate4 = P[3];
                    Model.LicensePlate5 = P[4];
                    Model.LicensePlate6 = P[5];
                    Model.LicensePlate7 = P[6];
                    Model.LicensePlate8 = P[7];
                    Model.LicensePlate9 = P[8];
                    Model.LicensePlate10 = P[9];

                    Session["CurrentModel"] = Model;


                    Model.Error = string.Empty;
                    ResultType res = wswi.ModifyOperationPlates(
                        (decimal)Model.OperationId,
                        Model.SessionId,
                        Model.UserName,
                        P[0],
                        P[1],
                        P[2],
                        P[3],
                        P[4],
                        P[5],
                        P[6],
                        P[7],
                        P[8],
                        P[9]);

                    if (res != ResultType.Result_OK)
                    {
                        Model.Error = GetStringOrDefault(string.Format("Permits_Error_{0}", res.ToString()));
                        Session["CurrentModel"] = Model;
                        return RedirectToAction("ManageError", "Permits");
                    }
                }
                else
                {
                    Model.Error = GetStringOrDefault("Permits_Signup_Required");
                    Session["CurrentModel"] = Model;
                    return RedirectToAction("ManageError", "Permits");
                }
                return RedirectToAction("ActivePermits", "Permits");
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("PermitsController > Manage [POST] > {0}", ex.Message));
                Model.Error = ex.Message;
                Session["CurrentModel"] = Model;
                return RedirectToAction("ManageError", "Permits");
            }
        }

        [HttpGet]
        public ActionResult ManageError()
        {
            SetCulture();

            if (Session["CurrentModel"] == null)
            {
                m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > ManageError > Forced LogOff > Session[CurrentModel] is null"));
                return RedirectToAction("LogOff", "Account");
            }

            USER oUser = GetUserFromSession();
            ViewData["oUser"] = oUser;

            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            return View(Model);
        }

        
        #endregion

        #region PayForPermit

        public JsonResult GetCities(string text)
        {
            if (text == null)
            {
                text = string.Empty;
            }

            USER oUser = GetUserFromSession();
            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            Model.Cities.Clear();

            List<WSintegraMobile.City> installationsAndSuperInstallations = wswi.GetCities(text, oUser.CURRENCy.CUR_ID);

            foreach (WSintegraMobile.City c in installationsAndSuperInstallations)
            {
                Model.Cities[c.id] = c.Name;
            }

            foreach (WSintegraMobile.City c in installationsAndSuperInstallations)
            {
                c.Installation = null; // if not, serialization fails
            }

            return Json(installationsAndSuperInstallations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClearError()
        {
            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            if (Model != null)
            {
                Model.Error = string.Empty;
                Session["CurrentModel"] = Model;
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetZones(decimal? CityId)
        {
            List<Zone> Zones = new List<Zone>();
            List<Plate> Plates = new List<Plate>();
            string SessionId = string.Empty;
            int oPaymentMethod = 0;
            
            string text = null;
            if (!string.IsNullOrEmpty(Request.Params["filter[filters][0][field]"]) && Request.Params["filter[filters][0][field]"] == "Name")
            {
                if (!string.IsNullOrEmpty(Request.Params["filter[filters][0][value]"]))
                {
                    text = Request.Params["filter[filters][0][value]"];
                }
            }

            if (CityId != null)
            {
                PermitsModel Model = (PermitsModel)Session["CurrentModel"];

                SortedList parametersOut = new SortedList();

                ResultType res = wswi.QueryLoginCityInternal(CityId, Model.UserName, text, Model.UTC_Offset, out parametersOut);

                if (res == ResultType.Result_OK)
                {
                    Session["InstallationID"] = CityId;
                    if (parametersOut["moneris_hash_seed_key"]!=null)
                        Session["moneris_hash_seed_key"] = parametersOut["moneris_hash_seed_key"].ToString();

                    if (parametersOut.ContainsKey("SessionID"))
                    {
                        SessionId = parametersOut["SessionID"].ToString();
                    }

                    oPaymentMethod = Convert.ToInt32(parametersOut["userDATA_0_paymeth"]);

                    if ((oPaymentMethod == (int)payMethods.PayPerTransaction_New_User_Without_Paymethod_Currently) ||
                        (oPaymentMethod == (int)payMethods.PayPerTransaction_Without_Paymethod_Currently) ||
                        (oPaymentMethod == (int)payMethods.Prepay_New_User_Without_Paymethod_Currently) ||
                        (oPaymentMethod == (int)payMethods.Invalidated_Paymethod))
                    {

                        Zones.Add(new Zone
                        {
                            id = -1, //Without payment mean
                            Name = ""
                        });
                    }
                    else
                    {
                        List<decimal> PermitTypeTariffs = new List<decimal>();
                        int hNum = Convert.ToInt32(parametersOut["InfoTAR_0_ad_num"]);
                        if (hNum > 0)
                        {
                            for (int i = 0; i < hNum; i++)
                            {
                                if (Convert.ToInt32(parametersOut[string.Format("InfoTAR_0_ad_{0}_type", i)]) == 1)
                                {
                                    int kNum = Convert.ToInt32(parametersOut[string.Format("InfoTAR_0_ad_{0}_szs_0_sz_num", i)]);
                                    for (int k = 0; k < kNum; k++)
                                    {
                                        PermitTypeTariffs.Add(Convert.ToDecimal(parametersOut[string.Format("InfoTAR_0_ad_{0}_szs_0_sz_{1}_id", i, k)]));
                                    }
                                }
                            }
                        }

                        int iNum = Convert.ToInt32(parametersOut["ZoneTar_0_zone_num"]);
                        if (iNum > 0)
                        {
                            if (string.IsNullOrEmpty(text))
                            {
                                Zones.Add(new Zone
                                {
                                    id = 0,
                                    Name = Resources.Permits_SelectZone
                                });

                            }
                        }
                        for (int i = 0; i < iNum; i++)
                        {
                            int jNum = Convert.ToInt32(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_num", i)]);
                            if (jNum > 0)
                            {
                                for (int j = 0; j < jNum; j++)
                                {
                                    if (string.IsNullOrEmpty(text) || string.Format("[{0}] {1} - {2}", 
                                                                            parametersOut[string.Format("ZoneTar_0_zone_{0}_numdesc", i)], 
                                                                            parametersOut[string.Format("ZoneTar_0_zone_{0}_desc", i)], 
                                                                            parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_desc", i, j)]).ToString().ToLower().Contains(text.ToLower()))
                                    {
                                        if (PermitTypeTariffs.Contains(Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_id", i, j)])))
                                        {
                                            Zones.Add(new Zone
                                            {
                                                id = Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_id", i, j)]),
                                                Name = string.Format("[{0}] {1} - {2}", 
                                                                parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_numdesc", i, j)], 
                                                                parametersOut[string.Format("ZoneTar_0_zone_{0}_desc", i)], 
                                                                parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_desc", i, j)]).Replace("[] ", ""),
                                                MaxMonths = parametersOut.ContainsKey(string.Format("ZoneTar_0_zone_{0}_subzone_{1}_permitmaxmonths", i, j)) ? 
                                                                Convert.ToInt32(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_permitmaxmonths", i, j)]) : 
                                                                1,
                                                MaxBuyDay = parametersOut.ContainsKey(string.Format("ZoneTar_0_zone_{0}_subzone_{1}_permitmonthmaxbuyday", i, j)) ? 
                                                                (parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_permitmonthmaxbuyday", i, j)].ToString() != string.Empty ? 
                                                                    Convert.ToInt32(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_permitmonthmaxbuyday", i, j)]) : 
                                                                    0) : 
                                                                0,
                                            });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(text) || string.Format("[{0}] {1}", 
                                                                        parametersOut[string.Format("ZoneTar_0_zone_{0}_numdesc", i)], 
                                                                        parametersOut[string.Format("ZoneTar_0_zone_{0}_desc", i)]).ToString().ToLower().Contains(text.ToLower()))

                                    if (PermitTypeTariffs.Contains(Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_id", i)])))
                                    {
                                        Zones.Add(new Zone
                                        {
                                            id = Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_id", i)]),
                                            Name = string.Format("[{0}] {1}", 
                                                        parametersOut[string.Format("ZoneTar_0_zone_{0}_numdesc", i)], 
                                                        parametersOut[string.Format("ZoneTar_0_zone_{0}_desc", i)]).Replace("[] ", ""),
                                            MaxMonths = parametersOut.ContainsKey(string.Format("ZoneTar_0_zone_{0}_permitmaxmonths", i)) ? 
                                                                                        Convert.ToInt32(parametersOut[string.Format("ZoneTar_0_zone_{0}_permitmaxmonths", i)]) : 
                                                                                        1,
                                            MaxBuyDay = parametersOut.ContainsKey(string.Format("ZoneTar_0_zone_{0}_permitmonthmaxbuyday", i)) ? 
                                                                                    (parametersOut[string.Format("ZoneTar_0_zone_{0}_permitmonthmaxbuyday", i)].ToString() != string.Empty ? 
                                                                                        Convert.ToInt32(parametersOut[string.Format("ZoneTar_0_zone_permitmonthmaxbuyday", i)]) : 
                                                                                        0) : 
                                                                                    0,
                                        });
                                    }
                            }
                        }

                        iNum = Convert.ToInt32(parametersOut["userDATA_0_userlp_0_lp_num"]);

                        for (int i = 0; i < iNum; i++)
                        {
                            if (parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString().Contains("~"))
                            {
                                foreach (string p in parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString().Split('~'))
                                {
                                    if (string.IsNullOrEmpty(text) || p.ToLower().Contains(text.ToLower()))
                                    {
                                        Plates.Add(new Plate { id = p, LicensePlate = p });
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(text) || parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString().ToLower().Contains(text.ToLower()))
                                {
                                    Plates.Add(new Plate { 
                                        id = parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString(), 
                                        LicensePlate = parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString() });
                                }
                            }
                        }
                    }
                }
                Model.SessionId = SessionId;
                Model.Plates = Plates;
                Model.PaymentMethod = oPaymentMethod;

                Model.Zones.Clear();
                foreach (Zone z in Zones)
                {
                    Model.Zones[z.id] = z.Name;
                }

                Session["CurrentModel"] = Model;

                return Json(Zones, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPlates(string PlateToAdd)
        {
            List<Plate> matchingPlates = new List<Plate>() { };

            try
            {
                PermitsModel Model = (PermitsModel)Session["CurrentModel"];

                string text = Convert.ToString(Request.Params["filter[filters][0][value]"]);

                if (!string.IsNullOrEmpty(PlateToAdd))
                {
                    if (Model.Plates.Where<Plate>(t => t.id.ToLower() == PlateToAdd.ToLower()).Count<Plate>() == 0)
                    {
                        Model.Plates.Add(new Plate() { id = PlateToAdd, LicensePlate = PlateToAdd });
                        Session["CurrentModel"] = Model;
                    }
                }

                matchingPlates = Model.Plates.Where<Plate>(t => !string.IsNullOrEmpty(text) && t.id.ToLower().Contains(text.ToLower()) || string.IsNullOrEmpty(text)).OrderBy(t => t.LicensePlate).ToList<Plate>();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            if (matchingPlates.Count > 0)
            {
                Plate p = new Plate();
                p.id = string.Empty;
                p.LicensePlate = string.Empty;
                matchingPlates.Insert(0, p);
            }
            return Json(matchingPlates, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddLicensePlate(string LicensePlate, int CityId)
        {
            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            ResultType res = wswi.AddLicensePlate(Model.UserName, Model.SessionId, LicensePlate, CityId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMonths(decimal CityId, decimal ZoneId)
        {
            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            List<Month> Months = new List<Month>() { };
            INSTALLATION ins = backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>())
                    .Where(i => i.INS_ID == CityId)
                    .FirstOrDefault();

            GROUP grp = backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>())
                    .Where(g => g.GRP_ID == ZoneId)
                    .FirstOrDefault();

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(ins.INS_TIMEZONE_ID);
            DateTime dtServerTime = DateTime.Now;
            DateTime dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);

            int? MaxMonths = grp.GRP_PERMIT_MAX_MONTHS;
            if (MaxMonths == null) { MaxMonths = 1; }

            int? MaxBuyDay = grp.GRP_PERMIT_MAX_CUR_MONTH_DAY;
            if (MaxBuyDay == null) { MaxBuyDay = 31; } // You can buy current month any day

            Month m = new Month();
            m.id = "0";
            m.Name = Resources.Permits_SelectMonth;
            Months.Add(m);

            for (int i = 0; i < MaxMonths; i++)
            {
                Month mm = new Month();
                if (dtInsDateTime.Day <= MaxBuyDay)
                {
                    // If you still can buy current month, we offer current month
                    if (i == 0)
                    {
                        mm.id = "CURRENT"; //dtInsDateTime.ToString("HHmmssddMMyy");
                        mm.Name = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dtInsDateTime.Month), dtInsDateTime.Year);
                    }
                    else
                    {
                        DateTime d = new DateTime(dtInsDateTime.Year, dtInsDateTime.Month, 1);
                        d = d.AddMonths(i);
                        mm.id = d.ToString("000000ddMMyy");
                        mm.Name = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Month), d.Year);
                    }
                }
                else 
                {
                    // ...else, you only can buy from next month on
                    DateTime d = new DateTime(dtInsDateTime.Year, dtInsDateTime.Month, 1);
                    d = d.AddMonths(i+1);
                    mm.id = d.ToString("000000ddMMyy");
                    mm.Name = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.Month), d.Year);               
                }
                Months.Add(mm);
            }

            return Json(Months, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTariffs(decimal ZoneId)
        {
            List<Tariff> Tariffs = new List<Tariff>() { };
            string text = null;
            if (!string.IsNullOrEmpty(Request.Params["filter[filters][1][field]"]) && Request.Params["filter[filters][1][field]"] == "Name")
            {
                if (!string.IsNullOrEmpty(Request.Params["filter[filters][1][value]"]))
                {
                    text = Request.Params["filter[filters][1][value]"];
                }
            }

            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            SortedList parametersOut = new SortedList();
            ResultType res = wswi.QueryParkingTariffs(Model.UserName, Model.SessionId, ZoneId, text, out parametersOut);

            if (res == ResultType.Result_OK)
            {

                int iNum = Convert.ToInt32(parametersOut["ltar_ad_num"]);
                if (iNum > 0)
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        Tariffs.Add(new Tariff
                        {
                            id = 0,
                            Name = Resources.Permits_SelectTariff,
                            MaxLicensePlates = 0,
                            MaxBuyOnce = 0
                        });
                    }
                }
                for (int i = 0; i < iNum; i++)
                {
                    if (string.IsNullOrEmpty(text) || parametersOut[string.Format("ltar_ad_{0}_desc_2", i)].ToString().ToLower().Contains(text.ToLower()))
                    {
                        if (Convert.ToDecimal(parametersOut[string.Format("ltar_ad_{0}_type_4", i)]) == 1)
                        {
                            Random r = new Random();
                            int rnd = r.Next(2, 10);
                            Tariffs.Add(new Tariff
                            {
                                id = Convert.ToDecimal(parametersOut[string.Format("ltar_ad_{0}_id_0", i)]),
                                Name = parametersOut[string.Format("ltar_ad_{0}_desc_2", i)].ToString(),
                                MaxLicensePlates = parametersOut.ContainsKey(string.Format("ltar_ad_{0}_maxplates_5", i)) ? Convert.ToInt32(parametersOut[string.Format("ltar_ad_{0}_maxplates_5", i)]) : 1,
                                MaxBuyOnce = parametersOut.ContainsKey(string.Format("ltar_ad_{0}_permitmaxbuyonce_8", i)) ? Convert.ToInt32(parametersOut[string.Format("ltar_ad_{0}_permitmaxbuyonce_8", i)]) : 1
                            });
                        }
                    }
                }
            }

            Model.Tariffs.Clear();
            foreach (Tariff t in Tariffs)
            {
                Model.Tariffs[t.id] = t.Name;
            }

            Model.TariffList = Tariffs;

            Session["CurrentModel"] = Model;

            return Json(Tariffs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNumPermits(decimal? TariffId)
        {
            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            Tariff t = null;
            if (TariffId != null)
            {
                t = Model.TariffList.Where(x => x.id == (decimal)TariffId).FirstOrDefault();
            }
            List<NumPermits> np = new List<NumPermits>();
            np.Add(new NumPermits(0, Resources.Permits_SelectNumPermits));
            if (t != null)
            {                
                for (int i = 1; i <= t.MaxBuyOnce; i++)
                {
                    np.Add(new NumPermits(i, i.ToString()));
                }
                return Json(np, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new NumPermits(1, "1"), JsonRequestBehavior.AllowGet);
            }
        }

        private List<string> ArrangePlates(string LP1, string LP2, string LP3, string LP4, string LP5, string LP6, string LP7, string LP8, string LP9, string LP10)
        {
            List<string> P = new List<string>();

            if (!string.IsNullOrEmpty(LP1)) { P.Add(LP1); }
            if (!string.IsNullOrEmpty(LP2)) { P.Add(LP2); }
            if (!string.IsNullOrEmpty(LP3)) { P.Add(LP3); }
            if (!string.IsNullOrEmpty(LP4)) { P.Add(LP4); }
            if (!string.IsNullOrEmpty(LP5)) { P.Add(LP5); }
            if (!string.IsNullOrEmpty(LP6)) { P.Add(LP6); }
            if (!string.IsNullOrEmpty(LP7)) { P.Add(LP7); }
            if (!string.IsNullOrEmpty(LP8)) { P.Add(LP8); }
            if (!string.IsNullOrEmpty(LP9)) { P.Add(LP9); }
            if (!string.IsNullOrEmpty(LP10)) { P.Add(LP10); }

            for (int i = P.Count; i < 10; i++)
            {
                P.Add(string.Empty);
            }

            return P;
        }

        [HttpGet]
        public JsonResult GetTimeSteps(decimal CityId, decimal ZoneId, decimal Tariff, string Month)
        {
            List<string> PlateCollection = new List<string>();
            List<TimeStep> TimeSteps = new List<TimeStep>() { };

            try
            {

                for (int i = 1; i > 0; i++)
                {
                    if (Request.Params.AllKeys.Contains(string.Format("LP{0}_1", i)))
                    {
                        string PlateBlock = string.Empty;
                        for (int j = 1; j <= 10; j++)
                        {
                            if (Request.Params.AllKeys.Contains(string.Format("LP{0}_{1}", i, j)) && !string.IsNullOrEmpty(Request.Params[string.Format("LP{0}_{1}", i, j)]))
                            {
                                PlateBlock += string.Format(",{0}", Request.Params[string.Format("LP{0}_{1}", i, j)]);
                            }
                        }
                        if (!string.IsNullOrEmpty(PlateBlock))
                        {
                            PlateCollection.Add(PlateBlock.Substring(1));
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (PlateCollection.Count > 0)
                {
                    string text = null;
                    if (!string.IsNullOrEmpty(Request.Params["filter[filters][1][field]"]) && Request.Params["filter[filters][1][field]"] == "text")
                    {
                        if (!string.IsNullOrEmpty(Request.Params["filter[filters][1][value]"]))
                        {
                            text = Request.Params["filter[filters][1][value]"];
                        }
                    }

                    PermitsModel Model = (PermitsModel)Session["CurrentModel"];
                    Model.Error = string.Empty;

                    SortedList parametersOut = new SortedList();
                    int failedPermit = 0;

                    ResultType res = wswi.QueryParkingOperationWithTimeSteps(Model.UserName, Model.SessionId, PlateCollection, CityId, ZoneId, Tariff, Month, out parametersOut, out failedPermit);

                    if (res != ResultType.Result_OK)
                    {
                        if (failedPermit > 0)
                        {
                            TimeSteps.Add(new TimeStep { EndDate = string.Format("{0} - {1} {2}", resBundle.GetString(string.Format("Permits_Error_{0}", res)), resBundle.GetString("Permits_Permit"), failedPermit), text = string.Empty });
                        }
                        else
                        {
                            TimeSteps.Add(new TimeStep { EndDate = resBundle.GetString(string.Format("Permits_Error_{0}", res)), text = string.Empty });
                        }
                    }
                    else
                    {
                        int iNum = Convert.ToInt32(parametersOut["steps_0_step_num"]);
                        if (iNum > 0)
                        {
                            if (string.IsNullOrEmpty(text))
                            {
                                TimeSteps.Add(new TimeStep
                                {
                                    EndDate = string.Empty,
                                    text = Resources.Permits_SelectTimeStep
                                });
                            }
                        }

                        bool ShowStartDate = false;

                        string StartDateStr = parametersOut["di"].ToString();
                        int sdHo = Convert.ToInt32(parametersOut["di"].ToString().Substring(0, 2));
                        int sdMi = Convert.ToInt32(parametersOut["di"].ToString().Substring(2, 2));
                        int sdSe = Convert.ToInt32(parametersOut["di"].ToString().Substring(4, 2));
                        int sdDa = Convert.ToInt32(parametersOut["di"].ToString().Substring(6, 2));
                        int sdMo = Convert.ToInt32(parametersOut["di"].ToString().Substring(8, 2));
                        int sdYe = 2000 + Convert.ToInt32(parametersOut["di"].ToString().Substring(10, 2));

                        DateTime StartDateTime = new DateTime(sdYe, sdMo, sdDa, sdHo, sdMi, sdSe);

                        GROUP CurrentGroup = null;
                        DateTime? CurrentGroupDateTime = null;
                        geograficRepository.getGroup(ZoneId, ref CurrentGroup, ref CurrentGroupDateTime);

                        if (CurrentGroupDateTime != null)
                        {
                            //if (StartDateTime > CurrentGroupDateTime)
                            //{
                            ShowStartDate = true;
                            //}
                        }

                        for (int i = 0; i < iNum; i++)
                        {
                            string _day = parametersOut[string.Format("steps_0_step_{0}_d", i)].ToString().Substring(6, 2);
                            string _mon = parametersOut[string.Format("steps_0_step_{0}_d", i)].ToString().Substring(8, 2);
                            string _yea = parametersOut[string.Format("steps_0_step_{0}_d", i)].ToString().Substring(10, 2);

                            DateTime date = new DateTime(Convert.ToInt32(_yea) + 2000, Convert.ToInt32(_mon), Convert.ToInt32(_day));

                            string StartDateForShowing = string.Empty;
                            if (ShowStartDate)
                            {
                                StartDateForShowing = string.Format("{0} 00:00 - ", StartDateTime.ToShortDateString());
                            }

                            String VisibleString = string.Format("{5}{4} {0}:{1} - {2} {3}",
                                parametersOut[string.Format("steps_0_step_{0}_d", i)].ToString().Substring(0, 2),
                                parametersOut[string.Format("steps_0_step_{0}_d", i)].ToString().Substring(2, 2),
                                Math.Round(Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_total", i)]) / 100, 2),
                                parametersOut["cur"],
                                date.ToShortDateString(),
                                StartDateForShowing);

                            if (string.IsNullOrEmpty(text) || VisibleString.ToString().ToLower().Contains(text.ToLower()))
                            {
                                string _d = parametersOut[string.Format("steps_0_step_{0}_d", i)].ToString();
                                decimal _per_bon = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_per_bon", i)]);
                                decimal _q = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q", i)]);
                                decimal _q_fee = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_fee", i)]);
                                decimal _q_fee_plus_vat = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_fee_plus_vat", i)]);
                                decimal _q_plus_vat = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_plus_vat", i)]);
                                decimal _q_subtotal = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_subtotal", i)]);
                                decimal _q_total = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_total", i)]);
                                decimal _q_vat = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_vat", i)]);
                                decimal _q_without_bon = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_without_bon", i)]);
                                decimal _real_q = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_real_q", i)]);
                                decimal _t = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_t", i)]);
                                decimal _time_bal_used = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_time_bal_used", i)]);
                                string _di = parametersOut["di"].ToString();
                                decimal? _qch_total = null;
                                if (parametersOut.ContainsKey(string.Format("steps_0_step_{0}_qch_total", i)))
                                {
                                    _qch_total = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_qch_total", i)]);
                                }


                                TimeSteps.Add(new TimeStep
                                {
                                    EndDate = _d,
                                    per_bon = _per_bon,
                                    Amount = _q,
                                    AmountFee = _q_fee,
                                    q_fee_plus_vat = _q_fee_plus_vat,
                                    q_plus_vat = _q_plus_vat,
                                    q_subtotal = _q_subtotal,
                                    AmountTotal = _q_total,
                                    AmountVat = _q_vat,
                                    AmountWithoutBon = _q_without_bon,
                                    RealAmount = _real_q,
                                    MinimumTime = _t,
                                    TimeBalanceUsed = _time_bal_used,
                                    InitialDate = _di,
                                    qch_total = _qch_total,
                                    text = (_qch_total == null)
                                                ? string.Format("{5}{4} {0}:{1} - {2} {3}",
                                                    _d.Substring(0, 2),
                                                    _d.Substring(2, 2),
                                                    Math.Round(_q_total / 100, 2),
                                                    parametersOut["cur"],
                                                    date.ToShortDateString(),
                                                    StartDateForShowing)
                                                : string.Format("{7}{6} {0}:{1} - {2} {3} ({4} {5})",
                                                    _d.Substring(0, 2),
                                                    _d.Substring(2, 2),
                                                    Math.Round(_q_total / 100, 2),
                                                    parametersOut["cur"],
                                                    Math.Round((decimal)_qch_total / 100, 2),
                                                    Model.UserCurrency,
                                                    date.ToShortDateString(),
                                                    StartDateForShowing)
                                });
                            }
                        }

                        Model.TimeSteps.Clear();

                        int totalTimeSteps = TimeSteps.Count;
                        foreach (TimeStep t in TimeSteps)
                        {
                            if (t.EndDate == TimeSteps.Last().EndDate)
                            {
                                Model.TimeSteps[t.EndDate] = new TimeStep
                                {
                                    EndDate = t.EndDate,
                                    per_bon = t.per_bon,
                                    Amount = t.Amount,
                                    AmountFee = t.AmountFee,
                                    q_fee_plus_vat = t.q_fee_plus_vat,
                                    q_plus_vat = t.q_plus_vat,
                                    q_subtotal = t.q_subtotal,
                                    AmountTotal = t.AmountTotal,
                                    AmountVat = t.AmountVat,
                                    AmountWithoutBon = t.AmountWithoutBon,
                                    RealAmount = t.RealAmount,
                                    MinimumTime = t.MinimumTime,
                                    InitialDate = t.InitialDate,
                                    qch_total = t.qch_total,
                                    TimeBalanceUsed = t.TimeBalanceUsed,
                                    text = t.text
                                };
                            }
                        }

                        Session["CurrentModel"] = Model;
                    }
                }
            }
            catch (Exception ex)
            {
                TimeSteps.Clear();
                TimeSteps.Add(new TimeStep { EndDate = string.Format("<p>{0}</p><p><em>{1}</em></p>", resBundle.GetString(string.Format("Permits_Error_{0}", ResultType.Result_Error_Generic)), ex.Message), text = string.Empty });
            }
            if (TimeSteps.Count > 0)
            {
                return Json(new List<TimeStep>() { TimeSteps.Last() }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(new List<TimeStep>() { }, JsonRequestBehavior.AllowGet);
            }
        }        

        [HttpGet]
        public ActionResult PayForPermit(decimal? SelectedCity = null, decimal? SelectedZone = null, string SelectedMonth = null, int? SelectedNumPermits = null, string SelectedPlate1 = null, string SelectedPlate2 = null, string SelectedPlate3 = null, string SelectedPlate4 = null, string SelectedPlate5 = null, string SelectedPlate6 = null, string SelectedPlate7 = null, string SelectedPlate8 = null, string SelectedPlate9 = null, string SelectedPlate10 = null, decimal? SelectedTariff = null, bool LoginSuccessful = false)
        {
            SetCulture();

            try
            {
                if (Session["USER_ID"] == null)
                {
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > PayForPermit [GET] > Forced LogOff > Session[USER_ID] is null"));
                    return RedirectToAction("LogOff", "Account");
                }
                else
                {
                    
                    USER oUser = GetUserFromSession();
                    if (oUser != null)
                    {
                        ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);
                    }
                    
                }

                if (!SelectedCity.HasValue)
                    Session["InstallationID"] = null;

                if (!LoginSuccessful)
                {
                    Session["SelectedCity"] = SelectedCity;
                    Session["SelectedZone"] = SelectedZone;
                    Session["SelectedMonth"] = SelectedMonth;
                    Session["SelectedPlate1"] = SelectedPlate1;
                    Session["SelectedPlate2"] = SelectedPlate2;
                    Session["SelectedPlate3"] = SelectedPlate3;
                    Session["SelectedPlate4"] = SelectedPlate4;
                    Session["SelectedPlate5"] = SelectedPlate5;
                    Session["SelectedPlate6"] = SelectedPlate6;
                    Session["SelectedPlate7"] = SelectedPlate7;
                    Session["SelectedPlate8"] = SelectedPlate8;
                    Session["SelectedPlate9"] = SelectedPlate9;
                    Session["SelectedPlate10"] = SelectedPlate10;
                    Session["SelectedTariff"] = SelectedTariff;
                    Session["SelectedNumPermits"] = SelectedNumPermits;
                }

                if (LoginSuccessful && Session["CurrentModel"] != null)
                {
                    PermitsModel Model = (PermitsModel)Session["CurrentModel"];
                    if (Model.User == Convert.ToDecimal(Session["USER_ID"]))
                    {
                        Model.SelectedCity = (decimal?)Session["SelectedCity"];
                        Model.SelectedZone = (decimal?)Session["SelectedZone"];
                        Model.SelectedMonth = Convert.ToString(Session["SelectedMonth"]);
                        Model.SelectedPlate1  = (Session["SelectedPlate1"] != null  ? Session["SelectedPlate1"].ToString()  : null);
                        Model.SelectedPlate2  = (Session["SelectedPlate2"] != null  ? Session["SelectedPlate2"].ToString()  : null);
                        Model.SelectedPlate3  = (Session["SelectedPlate3"] != null  ? Session["SelectedPlate3"].ToString()  : null);
                        Model.SelectedPlate4  = (Session["SelectedPlate4"] != null  ? Session["SelectedPlate4"].ToString()  : null);
                        Model.SelectedPlate5  = (Session["SelectedPlate5"] != null  ? Session["SelectedPlate5"].ToString()  : null);
                        Model.SelectedPlate6  = (Session["SelectedPlate6"] != null  ? Session["SelectedPlate6"].ToString()  : null);
                        Model.SelectedPlate7  = (Session["SelectedPlate7"] != null  ? Session["SelectedPlate7"].ToString()  : null);
                        Model.SelectedPlate8  = (Session["SelectedPlate8"] != null  ? Session["SelectedPlate8"].ToString()  : null);
                        Model.SelectedPlate9  = (Session["SelectedPlate9"] != null  ? Session["SelectedPlate9"].ToString()  : null);
                        Model.SelectedPlate10 = (Session["SelectedPlate10"] != null ? Session["SelectedPlate10"].ToString() : null);
                        Model.SelectedTariff = (decimal?)Session["SelectedTariff"];
                        Model.SelectedNumPermits = (int?)Session["SelectedNumPermits"];

                        if (Model.SelectedCity != null)
                        {
                            INSTALLATION ins = backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>())
                                .Where(i => i.INS_ID == Model.SelectedCity)
                                .FirstOrDefault();

                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(ins.INS_TIMEZONE_ID);
                            DateTime dtInsDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, tzi);

                            Model.CurrentMonth = dtInsDateTime.ToString("MMyy");
                        }
                        else {
                            Model.CurrentMonth = DateTime.Now.ToString("MMyy");
                        }

                        Session["InMonerisPayment"] = null;
                        Session["ReturnURL"] = null;
                        Session["PermitCity"] = null;
                        Session["PermitZone"] = null;
                        Session["PermitTariff"] = null;
                        Session["PermitTimeStep"] = null;
                        Session["PermitAutomatic_renewal"] = null;
                        Session["PlateCollection"] = null;

                        Session["CurrentModel"] = Model;
                        
                        USER oUser = GetUserFromSession();
                        ViewData["oUser"] = oUser;

                        return View(Model);
                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > PayForPermit [GET] > Forced LogOff > Model.User ({0}) != Session[USER_ID] ({1})", Model.User, Session["USER_ID"]));
                        return RedirectToAction("LogOff", "Account");
                    }
                }
                else 
                {
                    return RedirectToAction("Login", "Permits", new { User = Session["USER_ID"], UTC_Offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes.ToString() });
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("PermitsController > PayForPermit [GET] > Forced LogOff > {0}", ex.Message));
                return RedirectToAction("LogOff", "Account");
            }
        }




        [HttpPost]
        public ActionResult PayForPermit(decimal? City, decimal? Zone, decimal? Tariff, string TimeStep, bool? automatic_renewal, string r)
        {
            SetCulture();

            string strMonerisMD = "";
            string strMonerisCAVV = "";
            string strMonerisECI = "";

            if (Session["CurrentModel"] == null)
            {
                m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > PayForPermit [POST] > Forced LogOff > Session[CurrentModel] is null"));
                return RedirectToAction("LogOff", "Account");
            }

            PermitsModel Model = (PermitsModel)Session["CurrentModel"];

            List<string> PlateCollection = new List<string>();

            if (Convert.ToBoolean(Session["InMonerisPayment"]))
            {
                Session["InMonerisPayment"] = null;
                if (!string.IsNullOrEmpty(r))
                {

                    MonerisController moneris = new MonerisController(customersRepository, infrastructureRepository, Request, Server, Session, null);
                    string r_decrypted = moneris.DecryptCryptResult(r, Session["moneris_hash_seed_key"].ToString());
                    dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

                    if (j.result == "succeeded")
                    {
                        strMonerisMD = j.moneris_md;
                        strMonerisCAVV = j.moneris_cavv;
                        strMonerisECI = j.moneris_eci;
                    }
                    else
                    {
                        Model.Error = j.errorMessage;
                        Session["CurrentModel"] = Model;
                        return RedirectToAction("Summary", "Permits");
                    }
                }



                if ((!City.HasValue) && Session["PermitCity"] != null)
                {
                    City = Convert.ToDecimal(Session["PermitCity"]) != -1 ? City = Convert.ToDecimal(Session["PermitCity"]) : null;
                }

                if ((!Zone.HasValue) && Session["PermitZone"] != null)
                {
                    Zone = Convert.ToDecimal(Session["PermitZone"]) != -1 ? Zone = Convert.ToDecimal(Session["PermitZone"]) : null;
                }

                if ((!Tariff.HasValue) && Session["PermitTariff"] != null)
                {
                    Tariff = Convert.ToDecimal(Session["PermitTariff"]) != -1 ? Tariff = Convert.ToDecimal(Session["PermitTariff"]) : null;
                }

                if ((string.IsNullOrEmpty(TimeStep)) && Session["PermitTimeStep"] != null)
                {
                    TimeStep = Session["PermitTimeStep"].ToString();
                }

                if ((!automatic_renewal.HasValue) && Session["PermitAutomatic_renewal"] != null)
                {
                    automatic_renewal = Convert.ToBoolean(Session["PermitAutomatic_renewal"]);
                }

                if (Session["PlateCollection"] != null)
                {
                    PlateCollection = (List<string>)Session["PlateCollection"];
                }

            }


            

            for (int i = 1; i > 0; i++)
            {
                if (Request.Params.AllKeys.Contains(string.Format("LP{0}_1", i)))
                {
                    string PlateBlock = string.Empty;
                    for (int j = 1; j <= 10; j++)
                    {
                        if (Request.Params.AllKeys.Contains(string.Format("LP{0}_{1}", i, j)) && !string.IsNullOrEmpty(Request.Params[string.Format("LP{0}_{1}", i, j)]))
                        {
                            PlateBlock += string.Format(",{0}", Request.Params[string.Format("LP{0}_{1}", i, j)]);
                        }
                    }
                    if (!string.IsNullOrEmpty(PlateBlock))
                    {
                        PlateCollection.Add(PlateBlock.Substring(1));
                    }
                }
                else
                {
                    break;
                }
            }           

            try
            {
                Model.Error = null;                

                decimal n;
                if (City != null && Zone != null && Tariff != null && City > 0 && Zone > 0 && PlateCollection.Count > 0 && Tariff > 0 && !string.IsNullOrEmpty(TimeStep) && Decimal.TryParse(TimeStep, out n))
                {
                    Model.City = (decimal)City;
                    Model.Zone = (decimal)Zone;
                    Model.Tariff = (decimal)Tariff;
                    Model.TimeStep = TimeStep;

                    Model.PlateCollection = PlateCollection;

                    Session["CurrentModel"] = Model;
                    string str3DSUrl = "";
                    Model.Error = string.Empty;
                    ResultType res = wswi.ConfirmParkingOperation(
                        Model.TimeSteps[Model.TimeStep].Amount,
                        DateTime.Now.ToString("HHmmssddMMyy"),
                        Model.TimeSteps[Model.TimeStep].EndDate,
                        Model.TimeSteps[Model.TimeStep].InitialDate,
                        (decimal)Zone,
                        PlateCollection,
                        Model.TimeSteps[Model.TimeStep].AmountWithoutBon,
                        Model.TimeSteps[Model.TimeStep].RealAmount,
                        Model.TimeSteps[Model.TimeStep].AmountFee,
                        (decimal)Tariff,
                        Model.TimeSteps[Model.TimeStep].MinimumTime,
                        Model.TimeSteps[Model.TimeStep].TimeBalanceUsed,
                        Model.TimeSteps[Model.TimeStep].AmountTotal,
                        Model.TimeSteps[Model.TimeStep].AmountVat,
                        Model.SessionId,
                        Model.UserName,
                        Convert.ToInt32(automatic_renewal),
                        strMonerisMD,
                        strMonerisCAVV,
                        strMonerisECI,
                        true,
                        out str3DSUrl);

                    if (res != ResultType.Result_OK)
                    {
                        if (res == ResultType.Result_3DS_Validation_Needed)
                        {
                            Session["InMonerisPayment"] = true;
                            Session["CurrentModel"] = Model;
                            Session["PermitCity"] = City.HasValue?City.Value:-1;
                            Session["PermitZone"] = Zone.HasValue?Zone.Value:-1;
                            Session["PermitTariff"] = Tariff.HasValue?Tariff.Value:-1;
                            Session["PermitTimeStep"] = TimeStep;
                            Session["PermitAutomatic_renewal"] = automatic_renewal.HasValue? automatic_renewal.Value:false;
                            Session["PlateCollection"] = PlateCollection;
                            return Redirect(string.Format("{0}&ReturnURL={1}",str3DSUrl.Replace("&amp;","&"),HttpUtility.UrlEncode(Request.Url.ToString())));

                        }

                        Model.Error = GetStringOrDefault(string.Format("Permits_Error_{0}", res.ToString()));
                        Session["CurrentModel"] = Model;
                    }
                }
                else
                {
                    Model.Error = GetStringOrDefault("Permits_Signup_Required");
                    Session["CurrentModel"] = Model;
                }
               
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("PermitsController > PayForPermit [POST] > {0}", ex.Message));
                Model.Error = ex.Message;
                Session["CurrentModel"] = Model;
                return RedirectToAction("Summary", "Permits");
            }

            return RedirectToAction("Summary", "Permits");
        }

        #endregion

        #region Summary
        
        [HttpGet]
        public ActionResult Summary()
        {
            SetCulture();

            Session["ReturnURL"] = null;
            Session["PermitCity"] = null;
            Session["PermitZone"] = null;
            Session["PermitTariff"] = null;
            Session["PermitTimeStep"] = null;
            Session["PermitAutomatic_renewal"] = null;
            Session["PlateCollection"] = null;

            PermitsModel Model = (PermitsModel)Session["CurrentModel"];
            if (Model == null)
            {
                Model = new PermitsModel();
                Model.Error = GetStringOrDefault("Permits_Error_Session_Expired");
                Session["CurrentModel"] = Model;
                return RedirectToAction("PayForPermit", "Permits");
            }

            USER oUser = GetUserFromSession();
            ViewData["oUser"] = oUser;

            return View(Model);
        }

        #endregion    

        #region ActivePermits

        [Authorize]
        public ActionResult ActivePermits(int? Type,
                                  string DateIni,
                                  string DateEnd,
                                  int? Plate,
                                  GridSortOptions gridSortOptions,
                                  int? page)
        {
            SetCulture();

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {
                Session["USER_ID"] = oUser.USR_ID;
                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;
                ViewData["bTariffPermit"] = GetUserInstallationPermit(oUser.CURRENCy.CUR_ID);


                if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
                {
                    gridSortOptions.Column = "Date";
                    gridSortOptions.Direction = MvcContrib.Sorting.SortDirection.Descending;
                }

                DateTimeFormatInfo provider = ((CultureInfo)Session["Culture"]).DateTimeFormat;
                DateTime? dtDateIni = null;
                DateTime? dtDateEnd = null;

                if ((DateIni != null) && (DateIni.Length > 0))
                {
                    try
                    {
                        dtDateIni = Convert.ToDateTime(DateIni, provider);
                    }
                    catch {}
                }

                if ((DateEnd != null) && (DateEnd.Length > 0))
                {
                    try
                    {
                        dtDateEnd = Convert.ToDateTime(DateEnd, provider);
                    }
                    catch {}
                }

                var permitFilterViewModel = new PermitFilterModel();
                permitFilterViewModel.SelectedType = Type ?? -1;
                permitFilterViewModel.SelectedPlate = Plate ?? -1;
                permitFilterViewModel.CurrentDateIni = dtDateIni;
                permitFilterViewModel.CurrentDateEnd = dtDateEnd;
                permitFilterViewModel.CurrentGridSortOptions = gridSortOptions;
                permitFilterViewModel.Fill(oUser);

                int iNumRows = 0;
                int iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;

                try
                {
                    iGridMaxRows = Convert.ToInt32(ConfigurationManager.AppSettings["OperationsGridNumRows"]);
                    if (iGridMaxRows == 0)
                    {
                        iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;
                    }
                }
                catch
                {
                    iGridMaxRows = DEFAULT_MAX_GRID_NUM_OPS;
                }

                var permitList = GetActivePermits(ref oUser,
                                                    Type,
                                                    dtDateIni,
                                                    dtDateEnd,
                                                    Plate,
                                                    gridSortOptions,
                                                    page,
                                                    iGridMaxRows,
                                                    out iNumRows);



                var permitPagedList = new CustomPagination<PermitRowModel>(
                                    permitList,
                                    page.GetValueOrDefault(1),
                                    iGridMaxRows,
                                    iNumRows);

                var permitListContainer = new PermitListContainerViewModel
                {
                    PermitPagedList = permitPagedList,
                    PermitFilterViewModel = permitFilterViewModel,
                    GridSortOptions = gridSortOptions
                };

                return View(permitListContainer);
            }
            else
            {
                m_Log.LogMessage(LogLevels.logINFO, string.Format("PermitsController > ActivePermits > Forced LogOff > oUser is null"));
                return RedirectToAction("LogOff", "Account");
            }
        }

        private IQueryable<PermitRowModel> GetActivePermits(ref USER oUser,
                                                                int? Type,
                                                                DateTime? DateIni,
                                                                DateTime? DateEnd,
                                                                int? Plate,
                                                                GridSortOptions gridSortOptions,
                                                                int? page,
                                                                int pageSize,
                                                                out int iNumRows)
        {

            var predicate = PredicateBuilder.True<VW_ACTIVE_PERMIT_OPERATION>();


            if (Type.HasValue)
            {
                predicate = predicate.And(a => a.OPE_TAR_ID == Type);
            }

            if (DateIni.HasValue)
            {
                predicate = predicate.And(a => a.OPE_INIDATE <= DateIni);
            }

            if (DateEnd.HasValue)
            {
                predicate = predicate.And(a => a.OPE_ENDDATE >= DateEnd);
            }

            if (Plate.HasValue)
            {
                predicate = predicate.And(a => a.USRP_ID == Plate || a.OPE_PLATE2_USRP_ID == Plate || a.OPE_PLATE3_USRP_ID == Plate || a.OPE_PLATE4_USRP_ID == Plate || a.OPE_PLATE5_USRP_ID == Plate || a.OPE_PLATE6_USRP_ID == Plate || a.OPE_PLATE7_USRP_ID == Plate || a.OPE_PLATE8_USRP_ID == Plate || a.OPE_PLATE9_USRP_ID == Plate || a.OPE_PLATE10_USRP_ID == Plate);
            }

            string orderbyField = "";
            switch (gridSortOptions.Column)
            {
                case "TariffId":
                    orderbyField = "OPE_TAR_ID";
                    break;
                case "Tariff":
                    orderbyField = "TAR_DESCRIPTION";
                    break;
                case "Plate":
                    orderbyField = "USRP_PLATE";
                    break;
                case "Amount":
                    orderbyField = "OPE_TOTAL_AMOUNT";
                    break;
                case "DateIni":
                    orderbyField = "OPE_INIDATE";
                    break;
                case "DateEnd":
                    orderbyField = "OPE_ENDDATE";
                    break;
                case "Sector":
                    orderbyField = "GRP_DESCRIPTION";
                    break;
                case "GrpDescription":
                    orderbyField = "GRP_DESCRIPTION";
                    break;
                case "Zone":
                    orderbyField = "GRP_DESCRIPTION";
                    break;
                default:
                    orderbyField = "OPE_DATE";
                    break;
            }

            IQueryable<PermitRowModel> modelOps = null;

            if (!page.HasValue && pageSize == -1)
            {
                iNumRows = -1;
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                modelOps = from domOp in customersRepository.GetActivePermits(ref oUser, predicate,
                                   orderbyField, gridSortOptions.Direction.ToString())
                           select new PermitRowModel
                           {
                               Id = domOp.OPE_ID,
                               TariffId = Convert.ToInt32(domOp.OPE_TAR_ID),
                               Tariff = domOp.TAR_DESCRIPTION,
                               DateIni = domOp.OPE_INIDATE,
                               DateEnd = domOp.OPE_ENDDATE,
                               GrpDescription = domOp.GRP_DESCRIPTION,
                               Amount = Convert.ToDouble(domOp.OPE_TOTAL_AMOUNT),
                               CurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                               PlateId = Convert.ToInt32(domOp.USRP_ID),
                               PlateId2 = Convert.ToInt32(domOp.OPE_PLATE2_USRP_ID),
                               PlateId3 = Convert.ToInt32(domOp.OPE_PLATE3_USRP_ID),
                               PlateId4 = Convert.ToInt32(domOp.OPE_PLATE4_USRP_ID),
                               PlateId5 = Convert.ToInt32(domOp.OPE_PLATE5_USRP_ID),
                               PlateId6 = Convert.ToInt32(domOp.OPE_PLATE6_USRP_ID),
                               PlateId7 = Convert.ToInt32(domOp.OPE_PLATE7_USRP_ID),
                               PlateId8 = Convert.ToInt32(domOp.OPE_PLATE8_USRP_ID),
                               PlateId9 = Convert.ToInt32(domOp.OPE_PLATE9_USRP_ID),
                               PlateId10 = Convert.ToInt32(domOp.OPE_PLATE10_USRP_ID),
                               Plate = domOp.USRP_PLATE,
                               Plate2 = domOp.USRP_PLATE2,
                               Plate3 = domOp.USRP_PLATE3,
                               Plate4 = domOp.USRP_PLATE4,
                               Plate5 = domOp.USRP_PLATE5,
                               Plate6 = domOp.USRP_PLATE6,
                               Plate7 = domOp.USRP_PLATE7,
                               Plate8 = domOp.USRP_PLATE8,
                               Plate9 = domOp.USRP_PLATE9,
                               Plate10 = domOp.USRP_PLATE10,
                               Plates = GetPlateList(domOp.USRP_PLATE, domOp.USRP_PLATE2, domOp.USRP_PLATE3, domOp.USRP_PLATE4, domOp.USRP_PLATE5, domOp.USRP_PLATE6, domOp.USRP_PLATE7, domOp.USRP_PLATE8, domOp.USRP_PLATE9, domOp.USRP_PLATE10),
                               CurrencyMinorUnit = domOp.OPE_AMOUNT_CUR_MINOR_UNIT,
                               RenewAutomatically = (domOp.OPE_PERMIT_AUTO_RENEW == null ? false : Convert.ToBoolean(domOp.OPE_PERMIT_AUTO_RENEW)),
                               CityId = domOp.OPE_INS_ID,
                               GrpId = domOp.OPE_GRP_ID,
                               PaymentDisabled = false,
                               AutoRenewalDisabled = false,
                               MaxMonths = domOp.GRP_PERMIT_MAX_MONTHS != null ? (int)domOp.GRP_PERMIT_MAX_MONTHS : 1
                           };
            }
            else
            {
                modelOps = from domOp in customersRepository.GetActivePermits(ref oUser, predicate,
                                   orderbyField, gridSortOptions.Direction.ToString(),
                                   page.GetValueOrDefault(1), pageSize, out iNumRows)
                           select new PermitRowModel
                           {
                               Id = domOp.OPE_ID,
                               TariffId = Convert.ToInt32(domOp.OPE_TAR_ID),
                               Tariff = domOp.TAR_DESCRIPTION,
                               DateIni = domOp.OPE_INIDATE,
                               DateEnd = domOp.OPE_ENDDATE,
                               GrpDescription = domOp.GRP_DESCRIPTION,
                               Amount = Convert.ToDouble(domOp.OPE_TOTAL_AMOUNT),
                               CurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                               PlateId = Convert.ToInt32(domOp.USRP_ID),
                               PlateId2 = Convert.ToInt32(domOp.OPE_PLATE2_USRP_ID),
                               PlateId3 = Convert.ToInt32(domOp.OPE_PLATE3_USRP_ID),
                               PlateId4 = Convert.ToInt32(domOp.OPE_PLATE4_USRP_ID),
                               PlateId5 = Convert.ToInt32(domOp.OPE_PLATE5_USRP_ID),
                               PlateId6 = Convert.ToInt32(domOp.OPE_PLATE6_USRP_ID),
                               PlateId7 = Convert.ToInt32(domOp.OPE_PLATE7_USRP_ID),
                               PlateId8 = Convert.ToInt32(domOp.OPE_PLATE8_USRP_ID),
                               PlateId9 = Convert.ToInt32(domOp.OPE_PLATE9_USRP_ID),
                               PlateId10 = Convert.ToInt32(domOp.OPE_PLATE10_USRP_ID),
                               Plate = domOp.USRP_PLATE,
                               Plate2 = domOp.USRP_PLATE2,
                               Plate3 = domOp.USRP_PLATE3,
                               Plate4 = domOp.USRP_PLATE4,
                               Plate5 = domOp.USRP_PLATE5,
                               Plate6 = domOp.USRP_PLATE6,
                               Plate7 = domOp.USRP_PLATE7,
                               Plate8 = domOp.USRP_PLATE8,
                               Plate9 = domOp.USRP_PLATE9,
                               Plate10 = domOp.USRP_PLATE10,
                               Plates = GetPlateList(domOp.USRP_PLATE, domOp.USRP_PLATE2, domOp.USRP_PLATE3, domOp.USRP_PLATE4, domOp.USRP_PLATE5, domOp.USRP_PLATE6, domOp.USRP_PLATE7, domOp.USRP_PLATE8, domOp.USRP_PLATE9, domOp.USRP_PLATE10),
                               CurrencyMinorUnit = domOp.OPE_AMOUNT_CUR_MINOR_UNIT,
                               RenewAutomatically = (domOp.OPE_PERMIT_AUTO_RENEW == null ? false : Convert.ToBoolean(domOp.OPE_PERMIT_AUTO_RENEW)),
                               CityId = domOp.OPE_INS_ID,
                               GrpId = domOp.OPE_GRP_ID,
                               PaymentDisabled = false,
                               AutoRenewalDisabled = false,
                               MaxMonths = domOp.GRP_PERMIT_MAX_MONTHS != null ? (int)domOp.GRP_PERMIT_MAX_MONTHS : 1,
                               UTC_Offset = domOp.OPE_DATE_UTC_OFFSET
                           };
            }

            List<PermitRowHelperModel> AllRows = new List<PermitRowHelperModel>();

            foreach (PermitRowModel p in modelOps)
            {
                List<string> Plates = new List<string>();
                if (!string.IsNullOrEmpty(p.Plate)) { Plates.Add(p.Plate); }
                if (!string.IsNullOrEmpty(p.Plate2)) { Plates.Add(p.Plate2); }
                if (!string.IsNullOrEmpty(p.Plate3)) { Plates.Add(p.Plate3); }
                if (!string.IsNullOrEmpty(p.Plate4)) { Plates.Add(p.Plate4); }
                if (!string.IsNullOrEmpty(p.Plate5)) { Plates.Add(p.Plate5); }
                if (!string.IsNullOrEmpty(p.Plate6)) { Plates.Add(p.Plate6); }
                if (!string.IsNullOrEmpty(p.Plate7)) { Plates.Add(p.Plate7); }
                if (!string.IsNullOrEmpty(p.Plate8)) { Plates.Add(p.Plate8); }
                if (!string.IsNullOrEmpty(p.Plate9)) { Plates.Add(p.Plate9); }
                if (!string.IsNullOrEmpty(p.Plate10)) { Plates.Add(p.Plate10); }
                Plates = Plates.OrderBy(x => x).ToList();

                string ItemKey = string.Format("{0}_{1}", p.GrpId, p.TariffId);
                foreach (string pp in Plates)
                {
                    ItemKey += string.Format("_{0}", pp);
                }

                // Our key to identify uniquely one permit is GroupId_TariffId_Plate_PlateN... with plates ordered alphabetically

                PermitRowHelperModel prhm = new PermitRowHelperModel();
                prhm.Key = ItemKey;
                prhm.Id = p.Id;
                prhm.DateIni = (DateTime)p.DateIni;
                prhm.RenewAutomatically = p.RenewAutomatically;
                prhm.UTC_Offset = p.UTC_Offset;
                prhm.MaxMonths = p.MaxMonths;

                AllRows.Add(prhm);
            }

            foreach (PermitRowHelperModel prhm in AllRows)
            {
                // If one permit has automatical renewal disabled, we have to check if manual payment is possible
                //if (prhm.RenewAutomatically == false)
                //{
                DateTime NextDate = prhm.DateIni.AddMonths(1);
                string NextMonth = NextDate.ToString("MMyy");

                // Installation date is UTC - Offset from view
                DateTime InstallationDate = DateTime.UtcNow.AddMinutes(-prhm.UTC_Offset);
                // We go to day 1 in order to avoid problems with months having different amount of days (31/01/2018 + 1 month could go to march)
                DateTime FirstDayOfCurrentMonthInInstallation = new DateTime(InstallationDate.Year, InstallationDate.Month, 1);
                // Max month is current month + MaxMonths from view (-1 because 3 means current + 2)
                DateTime MaxMonth = FirstDayOfCurrentMonthInInstallation.AddMonths(prhm.MaxMonths-1);

                // If there is a permit for the same key (group, tariff and plates) for the next month, payment button and autorenewal are disabled
                if (AllRows.Where(p => p.Key == prhm.Key && p.DateIni.ToString("MMyy") == NextMonth).Count() > 0)
                {
                    List<PermitRowModel> tempList = modelOps.ToList<PermitRowModel>();

                    foreach (PermitRowModel prw in tempList)
                    {
                        if (prw.Id == prhm.Id)
                        {
                            prw.PaymentDisabled = true;
                            prw.AutoRenewalDisabled = true;
                        }
                    }
                    modelOps = tempList.AsQueryable<PermitRowModel>();
                }
                // If there is a permit for the same key (group, tariff and plates) for the next month, payment button is disabled                    
                if (prhm.DateIni.ToString("MMyy") == MaxMonth.ToString("MMyy"))
                {
                    List<PermitRowModel> tempList = modelOps.ToList<PermitRowModel>();

                    foreach (PermitRowModel prw in tempList)
                    {
                        if (prw.Id == prhm.Id)
                        {
                            prw.PaymentDisabled = true;
                        }
                    }
                    modelOps = tempList.AsQueryable<PermitRowModel>();
                }
                //}
            }

            return modelOps;
        }        

        private string GetPlateList(string P1, string P2, string P3, string P4, string P5, string P6, string P7, string P8, string P9, string P10)
        {
            string list = string.Empty;
            if (!string.IsNullOrEmpty(P1)) { list += string.Format(", {0}", P1); }
            if (!string.IsNullOrEmpty(P2)) { list += string.Format(", {0}", P2); }
            if (!string.IsNullOrEmpty(P3)) { list += string.Format(", {0}", P3); }
            if (!string.IsNullOrEmpty(P4)) { list += string.Format(", {0}", P4); }
            if (!string.IsNullOrEmpty(P5)) { list += string.Format(", {0}", P5); }
            if (!string.IsNullOrEmpty(P6)) { list += string.Format(", {0}", P6); }
            if (!string.IsNullOrEmpty(P7)) { list += string.Format(", {0}", P7); }
            if (!string.IsNullOrEmpty(P8)) { list += string.Format(", {0}", P8); }
            if (!string.IsNullOrEmpty(P9)) { list += string.Format(", {0}", P9); }
            if (!string.IsNullOrEmpty(P10)) { list += string.Format(", {0}", P10); }
            if (!string.IsNullOrEmpty(list)) { list = list.Substring(2); }
            return list;
        }

        private USER GetUserFromSession()
        {
            USER oUser = null;
            try
            {
                if (Session["USER_ID"] != null)
                {
                    decimal dUserId = (decimal)Session["USER_ID"];
                    if (!customersRepository.GetUserDataById(ref oUser, dUserId))
                    {
                        oUser = null;

                    }
                    else
                    {
                        ViewData["SuscriptionTypeGeneral"] = oUser.USR_SUSCRIPTION_TYPE;
                    }

                }

            }
            catch
            {
                oUser = null;
            }

            return oUser;
        }

        [Authorize]
        public FileResult ActivePermitsExport(int? Type, string DateIni, string DateEnd, int? Plate, GridSortOptions gridSortOptions, string format)
        {
            SetCulture();

            MemoryStream output = new MemoryStream();
            string sContentType = "";
            string sFileName = "";

            USER oUser = GetUserFromSession();

            if (oUser != null)
            {
                Session["USER_ID"] = oUser.USR_ID;
                ViewData["oUser"] = oUser;
                ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = oUser.CURRENCy.CUR_ISO_CODE;

                if (string.IsNullOrWhiteSpace(gridSortOptions.Column))
                {
                    gridSortOptions.Column = "Date";
                    gridSortOptions.Direction = MvcContrib.Sorting.SortDirection.Descending;
                }

                DateTimeFormatInfo provider = ((CultureInfo)Session["Culture"]).DateTimeFormat;
                DateTime? dtDateIni = null;
                DateTime? dtDateEnd = null;

                if ((DateIni != null) && (DateIni.Length > 0))
                {
                    try
                    {
                        dtDateIni = Convert.ToDateTime(DateIni, provider);
                    }
                    catch
                    {

                    }
                }

                if ((DateEnd != null) && (DateEnd.Length > 0))
                {
                    try
                    {
                        dtDateEnd = Convert.ToDateTime(DateEnd, provider);
                    }
                    catch
                    {

                    }

                }

                Type = (Type == -1 ? null : Type);
                Plate = (Plate == -1 ? null : Plate);

                int iNumRows = 0;

                var permitsList = GetActivePermits(ref oUser,
                                                        Type,
                                                        dtDateIni,
                                                        dtDateEnd,
                                                        Plate,
                                                        gridSortOptions,
                                                        null,
                                                        -1,
                                                        out iNumRows);
                string[] arrColumns = { "GrpDescription", "Tariff", "Plates", "Amount", "DateIni", "DateEnd" };
                string[] arrHeaders = { "PermitsDataModel_Zone", "PermitsDataModel_Tariff", "PermitsDataModel_Plates", "Account_Op_Amount", "Account_Op_Start_Date", "Account_Op_End_Date" };
                switch (format)
                {
                    case "xls":
                        ExportExcel(typeof(OperationRowModel), permitsList, arrColumns, arrHeaders, output);
                        sContentType = "application/vnd.ms-excel";
                        sFileName = ResourceExtension.GetLiteral("Permits_Export_XLSFilename") + ".xls";
                        break;
                    case "pdf":
                        ExportPdf(typeof(OperationRowModel), permitsList, arrColumns, arrHeaders, output);
                        sContentType = "application/pdf";
                        sFileName = ResourceExtension.GetLiteral("Permits_Export_PDFFilename") + ".pdf";
                        break;
                }
            }

            //Return the result to the end user
            return File(output.ToArray(), sContentType, sFileName);
        }

        public ActionResult UpdateRenewalStatus(decimal? OperationId, int? CheckedStatus)
        {
            if (!(OperationId == null || CheckedStatus == null || (CheckedStatus != 1 && CheckedStatus != 0)))
            {
                USER u = GetUserFromSession();
                customersRepository.UpdateOperationPermitAutoRenew(u, (decimal)OperationId, (int)CheckedStatus);
            }
            return RedirectToAction("ActivePermits", "Permits");
        }

        protected void SetCulture()
        {

            if (Session["Culture"] == null)
            {
                Session["Culture"] = new CultureInfo("en-US");
            }

            integraMobile.Properties.Resources.Culture = (CultureInfo)Session["Culture"];
            Thread.CurrentThread.CurrentUICulture = (CultureInfo)Session["Culture"];
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(((CultureInfo)Session["Culture"]).Name);
            integraMobile.Properties.Resources.Culture = (CultureInfo)Session["Culture"];
        }

        #endregion

        #region Export Methods

        private void ExportExcel(Type modelType, IQueryable rows, string[] columns, string[] headers, MemoryStream output)
        {
            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = ExportExcel_CreateSheet(workbook, columns, headers, modelType);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (object row in rows)
            {
                if (row.GetType() == typeof(OperationRowModel))
                {
                    ((OperationRowModel)row).RecalculateAmountStrings();
                }


                if (rowNumber >= 0xFFFF)
                {
                    sheet = ExportExcel_CreateSheet(workbook, columns, headers, modelType);
                    rowNumber = 1;
                }
                //Create a new row
                var sheetRow = sheet.CreateRow(rowNumber++);

                var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;

                //Set values for the cells
                int j = 0;
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != "")
                    {
                        string value = "";
                        string value2 = "";
                        PropertyInfo propInfo = row.GetType().GetProperty(columns[i] + "_FK");
                        if (propInfo == null) propInfo = row.GetType().GetProperty(columns[i]);
                        object obj = propInfo.GetValue(row, null);
                        if (obj != null)
                        {
                            if (propInfo.PropertyType != typeof(DateTime) && propInfo.PropertyType != typeof(DateTime?))
                                value = obj.ToString();
                            else
                            {
                                value = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortDatePattern);
                                value2 = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortTimePattern);
                            }
                        }
                        sheetRow.CreateCell(i + j).SetCellValue(value);
                        if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?))
                        {
                            j += 1;
                            sheetRow.CreateCell(i + j).SetCellValue(value2);
                        }
                    }
                }
            }

            //Write the workbook to a memory stream            
            workbook.Write(output);

        }

        private NPOI.SS.UserModel.ISheet ExportExcel_CreateSheet(HSSFWorkbook workbook, string[] columns, string[] headers, Type modelType)
        {
            var sheet = workbook.CreateSheet();

            /*for (int i = 0; i < columns.Length; i++)
            {
                sheet.SetColumnWidth(i, 10 * 256);
            }*/

            var headerRow = sheet.CreateRow(0);

            PropertyInfo propInfo = null;
            int j = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    headerRow.CreateCell(i + j).SetCellValue(ResourceExtension.GetLiteral(headers[i]));
                    propInfo = modelType.GetProperty(columns[i]);
                    if (propInfo != null && (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?)))
                    {
                        j += 1;
                        headerRow.CreateCell(i + j).SetCellValue(ResourceExtension.GetLiteral(headers[i] + "_Time")); ;
                    }
                }
            }

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            return sheet;
        }

        private void ExportPdf(Type modelType, IQueryable rows, string[] columns, string[] headers, MemoryStream output)
        {
            Rectangle pageSize = PageSize.A4;
            if (columns.Length > 5) pageSize = pageSize.Rotate();

            var document = new Document(pageSize, 10, 10, 10, 10);

            PdfWriter.GetInstance(document, output);

            document.Open();

            var numOfColumns = columns.Count(e => e != "");
            var dataTable = new PdfPTable(numOfColumns);

            dataTable.DefaultCell.Padding = 3;

            dataTable.DefaultCell.BorderWidth = 2;
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Font hFont = new Font(Font.FontFamily.COURIER, 8, Font.BOLD);
            Font rFont = new Font(Font.FontFamily.COURIER, 8, Font.NORMAL);

            // Adding headers
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    dataTable.AddCell(new PdfPCell(new Phrase(ResourceExtension.GetLiteral(headers[i]), hFont)));
                    //dataTable.AddCell(ResourceExtension.GetLiteral("ResourceManager.GetString("Account_Op_" + columns[i]));                
                }
            }

            dataTable.HeaderRows = 1;
            dataTable.DefaultCell.BorderWidth = 1;

            long iCount = 0;
            foreach (object row in rows)
            {
                if (row.GetType() == typeof(OperationRowModel))
                {
                    ((OperationRowModel)row).RecalculateAmountStrings();
                }

                foreach (string column in columns)
                {
                    if (column != "")
                    {
                        string value = "";
                        PropertyInfo propInfo = row.GetType().GetProperty(column + "_FK");
                        if (propInfo == null) propInfo = row.GetType().GetProperty(column);
                        object obj = propInfo.GetValue(row, null);
                        if (obj != null) value = obj.ToString();
                        dataTable.AddCell(new PdfPCell(new Phrase(value, rFont)));
                    }
                }
                iCount++;
            }

            if (iCount == 0)
            {
                for (int i = 0; i < columns.Length; i++)
                    if (columns[i] != "") dataTable.AddCell("");
            }

            document.Add(dataTable);

            document.Close();

        }

        private bool GetUserInstallationPermit(decimal currencyId)
        {
            bool bTariffPermit = false;

            List<WSintegraMobile.City> installationsAndSuperInstallations = wswi.GetCities(string.Empty, currencyId);
            if (installationsAndSuperInstallations.Count > 0)
            {
                bTariffPermit = true;
            }

            return bTariffPermit;
        }
        #endregion

    }
}