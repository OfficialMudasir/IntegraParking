using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Xml;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using System.Text.RegularExpressions;
using integraMobile.ExternalWS;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Reflection;
using integraMobile.Helper;
//using System.EnterpriseServices;



namespace integraMobile.Controllers
{
    public class FineController : Controller
    {
        #region Constructor PayPal
        private readonly string TEXT_TOKEN = "token";
        private readonly string TEXT_PAYMENT_ID = "paymentId";
        private readonly string TEXT_PAYER_ID = "PayerID";
        private readonly string TEXT_PAYPAL_TOKEN_2 = "PaypalToken2";
        private readonly string TEXT_PAYPAL_TOKEN_3 = "PaypalToken3";
        private readonly string TEXT_PAYPAL_FAILURE = "PaypalFailure";
        private readonly string TEXT_IN_PAYPAL_PAYMENT = "InPaypalPayment";
        private readonly string TEXT_PAYPAL_PREAPPROVAL_KEY = "PaypalPreapprovalKey";
        private readonly string TEXT_PAYPAL_RESULT = "PaypalResult";
        private readonly string TEXT_PAYPAL_RETURN = "PaypalReturn";
        private readonly string TEXT_PAYPAL_CANCEL = "PaypalCancel";
        private readonly string TEXT_PAYPAL_MODEL = "PayPalModel";
        #endregion

        #region Constructor
        private readonly string TEXT_QUANTITYTORECHARGE = "QuantityToRecharge";
        private readonly string TEXT_QUANTITYTORECHARGEBASE = "QuantityToRechargeBase";
        private readonly string TEXT_PERC_VAT1 = "PercVAT1";
        private readonly string TEXT_PERC_VAT2 = "PercVAT2";
        private readonly string TEXT_PERC_FEE = "PercFEE";
        private readonly string TEXT_PERC_FEE_TOPPED = "PercFEETopped";
        private readonly string TEXT_FIXED_FEE = "FixedFEE";
        private readonly string TEXT_CURRENCY_TO_RECHARGE = "CurrencyToRecharge";
        private readonly string TEXT_RECHARGE_CREATIONT_YPE = "RechargeCreationType";
        private readonly string TEXT_CURRENCY_ISO_CODE = "CurrencyISOCode";
        private readonly string TEXT_CC_FAILURE = "CCFailure";
        private const string DEFAULT_STRIPE_IMAGE_URL = "https://stripe.com/img/documentation/checkout/marketplace.png";
        private const string TEXT_BD_ENGLISH_LANGUAGE = "en-US";
        private const string TEXT_ENGLISH_LANGUAGE = "en-EN";
        #endregion

        #region Properties
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(FineController));
        private IFineRepository fineRepository;
        private IInfraestructureRepository infraestructureRepository;
        private ICustomersRepository customersRepository;
        private IGeograficAndTariffsRepository geograficAndTariffsRepository;
        private integraMobile.Services.ExternalService external = new Services.ExternalService();
        static string _xmlTagName = "ipark";
        private const string OUT_SUFIX = "_out";
        #endregion

        #region Public Method

        public FineController(IFineRepository _fineRepository, IInfraestructureRepository _infraestructureRepository, ICustomersRepository _customersRepository, IGeograficAndTariffsRepository _geograficAndTariffsRepository)
        {
            this.fineRepository = _fineRepository;
            this.infraestructureRepository = _infraestructureRepository;
            this.customersRepository = _customersRepository;
            this.geograficAndTariffsRepository = _geograficAndTariffsRepository;
        }

        [HttpGet]
        public ActionResult SuccessMail()
        {
            Session["validEmail"] = 1;
            return View();
        }

        [HttpPost]
        public ActionResult SuccessMail(string email, string email_confirm)
        {
            EmailChecker.EmailCheckResult oCheckResult = EmailChecker.EmailCheckResult.Invalid;

            oCheckResult = EmailChecker.Check(email);

            Session["validEmail"] = Convert.ToInt32(oCheckResult);
            if (!string.IsNullOrEmpty(email) && oCheckResult==EmailChecker.EmailCheckResult.Valid && Session["strRechargeEmailSubject"] != null && Session["strRechargeEmailBody"] != null)
            {
                if (SendEmail(email, Session["strRechargeEmailSubject"].ToString(), Session["strRechargeEmailBody"].ToString()))
                {
                    Session["strRechargeEmailSubject"] = null;
                    Session["strRechargeEmailBody"] = null;
                }
                else if (oCheckResult != EmailChecker.EmailCheckResult.Valid)
                {
                    this.ModelState.AddModelError(" EmailChecker.Check", integraMobile.Properties.Resources.urs_error_email);
                }
            }
            return View();
        }

        public ActionResult Fine(bool? init, decimal? OpId, decimal? InstallationId, string Culture, string TicketNumber = "")
        {

            m_Log.LogMessage(LogLevels.logINFO, string.Format("Fine={0} {1} {2} {3} {4}", init, OpId, InstallationId, Culture, TicketNumber));

            Session["Theme"] = "theme-Blinkay"; 
            Session["FineModel"] = null;
            Session["CityName"] = null;


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
            decimal ? dInsOpId=null;

            INSTALLATION[] inss = null;

            if (InstallationId != null)
            {
                inss = infraestructureRepository.Installations
                                        .Where(r => r.INS_WEB_PORTAL_PAYMENT_ENABLED== 1 &&
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

            FineModel model = new FineModel();
            model.Init(fineRepository);

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


            SetCulture();
           
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
        }

        [HttpPost]
        public ActionResult Details(FineModel model, string submitButton)
        {
            SetCulture();

            if (!string.IsNullOrEmpty(model.Plate))
            {
                model.Plate = model.Plate.Trim().ToUpper();
                model.Plate = Regex.Replace(model.Plate, "[^a-zA-Z0-9]", string.Empty);
            }

            if (
                (model.InstallationId > 0 && !string.IsNullOrEmpty(model.TicketNumber) && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.ConfirmEmail) && model.Email == model.ConfirmEmail)
                ||
                (!string.IsNullOrEmpty(model.Plate) && !string.IsNullOrEmpty(model.TicketNumber))
            )
            {
                if (model.ForceInstallationId != null && model.InstallationId == 0)
                {
                    model.InstallationId = (int)model.ForceInstallationId;
                }

                model.Init(fineRepository);
                GetFineDetails(ref model);

                DateTime? dt = null;
                INSTALLATION oInstallation = null;
                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dt);
                if (oInstallation!=null && oInstallation.INS_PAYPAL_CPTGC_ID != null)
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

                ViewData["FineModel"] = model;
                Session["FineModel"] = model;

                model.PriceCalculationDate = System.DateTime.Now;
                Session["TicketNumber"] = model.TicketNumber;


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
                model.Init(fineRepository);
                return Redirect("Fine");
            }
        }

        public ActionResult Details(string submitButton)
        {
            SetCulture();

            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    return RedirecToactionBySession();
                }
            }
            return Redirect("Fine");
        }

        public ActionResult ChangeCulture(string submitButton, string Lang)
        {
            Session["Culture"] = new CultureInfo(Lang);

            SetCulture();

            //Session["ChangeCulture"] = true;
            
            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    ChangeReturnFineCulture(Lang);
                    return RedirecToactionBySession();
                }
            }
            return Redirect("Fine");
        }        

        public void GetFineDetails(ref FineModel model)
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

        [HttpPost]
        public ActionResult Payment(FineModel model, string submitButton)
        {
            SetCulture();

            int? iActionButton = null;
            if (!string.IsNullOrEmpty(submitButton))
            {
                switch (submitButton)
                {
                    case "PayPal":
                        iActionButton = 2;
                        break;
                    case "Credit":
                        iActionButton = 1;
                        break;
                    default:
                        if (Session["ReturnFine"] != null)
                        {
                            return RedirecToactionBySession();
                        }
                        return Redirect("Fine");
                }
            }
            model = (FineModel)Session["FineModel"];
            ViewData["FineModel"] = model;

            TimeSpan ts = System.DateTime.Now - model.PriceCalculationDate;

            if (ts.Minutes <= 4)
            {
                string strCreditCardProviderPrefix = "CC";
                string strCreditCardSessionName = "";

                if (model.AmountCurrencyIsoCode != null)
                {
                    model.Init(fineRepository, iActionButton);
                    
                    CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                    if (oGatewayConfig != null &&
                                !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                    {
                        oGatewayConfig = null;
                    }


                    if (oGatewayConfig == null)
                    {

                        oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard))
                                        .FirstOrDefault();
                    }

                    PaymentMeanCreditCardProviderType eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
                    try
                    {
                        eProviderType = (PaymentMeanCreditCardProviderType)oGatewayConfig.CPTGC_PROVIDER;
                    }
                    catch
                    {
                        eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
                    }

                    if (iActionButton.HasValue && iActionButton.Value == 2)
                    {
                        eProviderType = PaymentMeanCreditCardProviderType.pmccpPaypal;
                    }

                    // MICHEL DEV
                    //eProviderType = PaymentMeanCreditCardProviderType.pmccpPayu;
                    //eProviderType = PaymentMeanCreditCardProviderType.pmccpMoneris;
                    //eProviderType = PaymentMeanCreditCardProviderType.pmccpTransbank;

                    switch (eProviderType)
                    {
                        case PaymentMeanCreditCardProviderType.pmccpCreditCall:
                            strCreditCardProviderPrefix = "CC";
                            strCreditCardSessionName = "InCreditCallPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpIECISA:
                            strCreditCardProviderPrefix = "CC2";
                            strCreditCardSessionName = "InIECISAPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpStripe:
                            strCreditCardProviderPrefix = "CC3";
                            strCreditCardSessionName = "InStripePayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPayu:
                            strCreditCardProviderPrefix = "CC4";
                            strCreditCardSessionName = "InPayuPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpTransbank:
                            strCreditCardProviderPrefix = "CC5";
                            strCreditCardSessionName = "InTransBankPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpMoneris:
                            strCreditCardProviderPrefix = "CC6";
                            strCreditCardSessionName = "InMonerisPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPaypal:
                            strCreditCardProviderPrefix = "CC7";
                            strCreditCardSessionName = "InPayPalPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPaysafe:
                            strCreditCardProviderPrefix = "CC8";
                            strCreditCardSessionName = "InPaysafePayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpBSRedsys:
                            strCreditCardProviderPrefix = "CC9";
                            strCreditCardSessionName = "InBSRedsysPayment";
                            break;
                        default:
                            break;
                    }

                    Session[strCreditCardSessionName] = true;
                    Session["ProviderType"] = (int)eProviderType;

                    string actionName = "";
                    switch (strCreditCardProviderPrefix)
                    {
                        case "CC4":
                            actionName = "PayuRequest";
                            break;
                        case "CC5":
                            actionName = "TransBankRequest";
                            break;
                        case "CC6":
                            actionName = "MonerisRequest";
                            break;
                        case "CC7":
                            actionName = "PaypalRequest";
                            break;
                        case "CC8":
                            actionName = "PaysafeRequest";
                            break;
                        case "CC9":
                            actionName = "BSRedsysRequest";
                            break;
                        default:
                            actionName = strCreditCardProviderPrefix + "Redirect";
                            break;
                    }

                
                    return RedirectToAction(actionName, new System.Web.Routing.RouteValueDictionary(model.ToDictionary(Request.Params)));
                }
                else
                {
                    model.Init(fineRepository);
                    return Redirect("Fine");
                }
            }
            else
            {
                model.Init(fineRepository);
                return Redirect("Fine");
            }
        }

        public ActionResult Payment()
        {
            return Redirect("Fine");
        }
        #endregion

        #region Private Method
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

        /* START IPT-260 */
        private string GetCustomConfigurationForOpId(decimal? OpId, string Setting, string Culture = null)
        {
            if (Culture != null)
            {
                if (ConfigurationManager.AppSettings[string.Format("{0}_{1}", Setting, Culture)] != null)
                {
                    Setting = string.Format("{0}_{1}", Setting, Culture);
                }
            }

            string[] Configs = ConfigurationManager.AppSettings[Setting].Split('~');
            if (Configs.Contains(OpId.ToString()))
            {
                bool getNext = false;
                foreach (string Config in Configs)
                {
                    if (getNext == true)
                    {
                        return Config;
                    }
                    if (Config == OpId.ToString())
                    {
                        getNext = true;
                    }
                }
            }
            return null;
        }
        /* END IPT-260 */

        private string GetEmailFooter(string InstallationShortDesc, string CountryCode)
        {
            string strFooter = "";

            try
            {
                strFooter = ResourceExtension.GetLiteral(string.Format("footer_INS_{0}", InstallationShortDesc));
                if (string.IsNullOrEmpty(strFooter))
                {
                    strFooter = ResourceExtension.GetLiteral(string.Format("footer_COU_{0}", CountryCode));
                }

            }
            catch
            {

            }

            return strFooter;
        }

        private Int32 GetXMLNodeValue(string res, string nodeName)
        {
            XmlDocument xmlInDoc = new XmlDocument();
            try
            {
                xmlInDoc.LoadXml(res);
                XmlNodeList Nodes = xmlInDoc.SelectNodes(string.Format("//{0}", nodeName));
                foreach (XmlNode Node in Nodes)
                {
                    if (Node.Name == nodeName)
                    {
                        if (Node.HasChildNodes)
                        {
                            return Convert.ToInt32(Node.InnerText.Trim());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return -9;
            }
            return -9;
        }

        /// <summary>
        /// Método para eliminar la session de Paypal
        /// </summary>
        private void DeleteSessionPayPal()
        {
            Session[TEXT_PAYPAL_TOKEN_2] = null;
            Session[TEXT_PAYPAL_TOKEN_3] = null;
            Session[TEXT_PAYER_ID] = null;
            Session[TEXT_IN_PAYPAL_PAYMENT] = false;
            Session[TEXT_PAYPAL_PREAPPROVAL_KEY] = null;
            Session[TEXT_PAYPAL_MODEL] = null;
            DeleteSessionAmount();
        }

        /// <summary>
        /// Método para eliminar la session correspondientes a la información del monto
        /// </summary>
        private void DeleteSessionAmount()
        {
            Session[TEXT_QUANTITYTORECHARGE] = null;
            Session[TEXT_QUANTITYTORECHARGEBASE] = null;
            Session[TEXT_PERC_VAT1] = null;
            Session[TEXT_PERC_VAT2] = null;
            Session[TEXT_PERC_FEE] = null;
            Session[TEXT_PERC_FEE_TOPPED] = null;
            Session[TEXT_FIXED_FEE] = null;
            Session[TEXT_CURRENCY_TO_RECHARGE] = null;
            Session[TEXT_RECHARGE_CREATIONT_YPE] = null;
        }

        /// <summary>
        /// Método para convertir la informacion que tiene la session a un valor decimal
        /// </summary>
        private decimal ConvertSessionToDecimal(string sessionName, NumberFormatInfo provider)
        {
            m_Log.LogMessage(LogLevels.logERROR, string.Format("ConvertSessionToDecimal: sessionName={0}", sessionName));
            decimal dValue = 0;
            if (!String.IsNullOrEmpty(sessionName))
            {
                dValue = (Session[sessionName] != null ? Convert.ToDecimal(Session[sessionName].ToString(), provider) : 0);
            }
            return dValue;
        }

        /// <summary>
        /// Método para convertir la informacion que tiene la session a un valor int
        /// </summary>
        private int ConvertSessionToInt(string sessionName)
        {
            m_Log.LogMessage(LogLevels.logERROR, string.Format("ConvertSessionToInt: sessionName={0}", sessionName));
            int dValue = 0;
            if (!String.IsNullOrEmpty(sessionName))
            {
                dValue = (Session[sessionName] != null ? Convert.ToInt32(Session[sessionName].ToString()) : 0);
            }
            return dValue;
        }

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
            return RedirectToAction("Fine", new { init = binit, OpId = dOpId, InstallationId = dInstallationId, Culture = sCulture });
        }

        private void ChangeReturnFineCulture(string culture)
        {
            bool? binit = null;
            decimal? dOpId = null;
            decimal? dInstallationId = null;

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
                    else if (cadena.Contains("init"))
                    {
                        binit = Convert.ToBoolean(find(cadena, "init"));
                    }
                }

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

                if (!string.IsNullOrEmpty(culture))
                {
                    if (stringURL != "?")
                        stringURL += "&";
                    stringURL += string.Format("{0}={1}", "Culture", culture);
                }

                Session["ReturnFine"] = stringURL;
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
                        bIsDifferent= (!dOpId.HasValue || Convert.ToDecimal(find(cadena, "OpId"))!=dOpId.Value);
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

        #endregion

        #region Payment Methods

        #region CreditCall Payment

        public ActionResult CCRedirect(FineModel model)
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure");

            model = (FineModel)Session["FineModel"];

            //USER oUser = GetUserFromSession();

            //if (oUser == null)
            //{
            //    return LogOff();
            //}

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

            //if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            //{
            //    oGatewayConfig = oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG;
            //}

            // Set current operation
            Session["OperationChargeType"] = ChargeOperationsType.TicketPayment;
            Session["QuantityToRecharge"] = (Convert.ToDouble(model.TotalQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode)).ToString().Replace(",", ".");
            Session["QuantityToRechargeBase"] = (Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode)).ToString().Replace(",", ".");
            Session["CurrencyToRecharge"] = model.AmountCurrencyIsoCode;

            if (oGatewayConfig == null)
            {

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

                oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {


                    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpCreditCall))
                                            .FirstOrDefault();

                }

            }


            ViewData["email"] = model.Email;
            ViewData["ekashu_form_url"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_EKASHU_FORM_URL;
            ViewData["ekashu_seller_id"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID;
            ViewData["ekashu_seller_key"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY.Substring(0, 8);
            ViewData["ekashu_amount"] = Session["QuantityToRecharge"].ToString();
            ViewData["ekashu_currency"] = (string)Session["CurrencyToRecharge"];
            ViewData["ekashu_reference"] = CardEasePayments.UserReference();
            ViewData["ekashu_hash_code"] = CardEasePayments.HashCode(oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                     oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_HASH_KEY,
                                                                     ViewData["ekashu_reference"].ToString(),
                                                                     Session["QuantityToRecharge"].ToString());
            ViewData["ekashu_description"] = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");

            ViewData["css_url"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_CSS_URL;

            string strSellerName = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_SELLER_NAME;

            if (strSellerName.Length > 8)
                ViewData["ekashu_seller_name"] = strSellerName.Substring(0, 8);
            else
                ViewData["ekashu_seller_name"] = strSellerName;


            string requrl = "";
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
            {
                requrl = Request.Url.ToString();
            }
            else
            {
                requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Fine/";
            }

            ViewData["ekashu_failure_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCFailure";
            ViewData["ekashu_return_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCCancel";
            ViewData["ekashu_success_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCSuccess";

          

            return View();
        }

        public ActionResult CCFailure(string submitButton)
        {
            SetCulture();

            //USER oUser = GetUserFromSession();

            //if (oUser == null)
            //{
            //    return LogOff();
            //}
            m_Log.LogMessage(LogLevels.logINFO, string.Format("FineController::-->Entra por el ReturnFine: "));
            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Entra por el ReturnFine: "));
                    return RedirecToactionBySession();
                }
                else
                {
                    return RedirecToactionBySession();
                }
            }
            else
            {


                string strCardReference = "";
                string strMaskedCardNumber = "";

                try
                {
                    
                    strCardReference = (Request["ekashu_card_reference"]!=null ? Request["ekashu_card_reference"] : string.Empty);
                    strMaskedCardNumber = (Request["ekashu_masked_card_number"]!=null ? Request["ekashu_masked_card_number"]: string.Empty);
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("ekashu_card_reference Exception: {0}", strCardReference));
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("ekashu_masked_card_number: {0}", strMaskedCardNumber));
                }
                catch { }

                m_Log.LogMessage(LogLevels.logERROR, string.Format("CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

                try
                {
                    //Console.Write("Stop");
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Entra por el try: "));
                    //ViewData["oUser"] = oUser;
                    //ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                    //ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                    //ViewData["CurrencyISOCode"] = oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE;
                    ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
                    ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
                    if (Session["CurrencyToRecharge"] != null)
                    {
                        ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(Session["CurrencyToRecharge"].ToString()) + "}",
                            Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));
                    }
                    ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];

                    Session["QuantityToRecharge"] = null;
                    Session["QuantityToRechargeBase"] = null;
                    Session["PercVAT1"] = null;
                    Session["PercVAT2"] = null;
                    Session["PercFEE"] = null;
                    Session["PercFEETopped"] = null;
                    Session["FixedFEE"] = null;
                    Session["CurrencyToRecharge"] = null;
                    Session["InCreditCallPayment"] = null;
                    Session["OVERWRITE_CARD"] = false;
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("CCFailure Exception: {0}", e.Message));
                }
            }


            return View();
        }

        //[Authorize] 
        public ActionResult CCCancel()
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure");



            //USER oUser = GetUserFromSession();

            //if (oUser == null)
            //{
            //    return LogOff();
            //}

            Console.Write("Stop");
            //ViewData["oUser"] = oUser;
            //ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
            //ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
            //ViewData["CurrencyISOCode"] = oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE;
            ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
            ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(Session["CurrencyToRecharge"].ToString()) + "}",
                Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));
            ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];



            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InCreditCallPayment"] = null;
            Session["OVERWRITE_CARD"] = false;


            return View();
        }

        //[Authorize] 
        public ActionResult CCSuccess()
        {
            SetCulture();

            string strCardReference = "";
            string strMaskedCardNumber = "";

            try
            {
                if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                    return RedirectToAction("CCFailure");

                FineModel model = (FineModel)Session["FineModel"];

                string strReference = Request["ekashu_reference"];
                string strAuthCode = Request["ekashu_auth_code"];
                string strAuthResult = Request["ekashu_auth_result"];
                string strCardHash = Request["ekashu_card_hash"];
                strCardReference = (Request["ekashu_card_reference"] != null ? Request["ekashu_card_reference"] : string.Empty);
                string strCardScheme = Request["ekashu_card_scheme"];
                string strGatewayDate = Request["ekashu_date_time_local_fmt"];
                strMaskedCardNumber = (Request["ekashu_masked_card_number"] != null ? Request["ekashu_masked_card_number"] : string.Empty); 
                string strTransactionId = Request["ekashu_transaction_id"];
                string strExpMonth = Request["ekashu_expires_end_month"];
                string strExpYear = Request["ekashu_expires_end_year"];

                string strCFTransactionId = String.Empty;
                if (Session["Sess_strCFTransactionId"] != null)
                {
                    strCFTransactionId = Session["Sess_strCFTransactionId"].ToString();
                }

                DateTime? dtExpDate = null;

                if (Session["Sess_dtExpDate"] != null)
                {
                    dtExpDate = (DateTime?)Session["Sess_dtExpDate"];
                }

                if ((strExpMonth.Length == 2) && (strExpYear.Length == 4))
                {
                    dtExpDate = new DateTime(Convert.ToInt32(strExpYear), Convert.ToInt32(strExpMonth), 1).AddMonths(1).AddSeconds(-1);
                }

                PaymentMeanCreditCardProviderType eProviderType = (PaymentMeanCreditCardProviderType)Session["ProviderType"];

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                            .FirstOrDefault();
                }

                int iWSTimeout = infraestructureRepository.GetRateWSTimeout(model.InstallationId);

                string res = external.ConfirmFinePaymentNonUser(model.TicketNumber, model.Quantity,
                                                model.TotalQuantity, model.Plate, model.PercFEE, model.PercVAT1,
                                                model.PercVAT2, model.PercFEETopped, model.FixedFEE, model.PartialVAT1,
                                                model.PartialPercFEE, model.PartialFixedFEE, model.PartialPercFEEVAT,
                                                model.PartialFixedFEEVAT, model.TaxMode, model.Email, strReference, strTransactionId,
                                                strCFTransactionId, strGatewayDate, strAuthCode, strAuthResult,
                                                strCardHash, strCardReference, strCardScheme, strMaskedCardNumber, null, null,
                                                dtExpDate, eProviderType, model.InstallationId, geograficAndTariffsRepository,
                                                model.CurrencyId, model.AmountCurrencyIsoCode, oGatewayConfig.CPTGC_ID, fineRepository, model.GrpId, model.AuthId, iWSTimeout);

                // SUCCESS res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><autorecharged>0</autorecharged><r>1</r><utc_offset>-420</utc_offset></ipark_out>
                // ERROR   res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><r>-9</r></ipark_out>

                Session["FineModel"] = null;

                // Extraemos el valor de r
                int xmlNodeValue = GetXMLNodeValue(res, "r");
                if ((ExternalWS.ResultType)xmlNodeValue == ExternalWS.ResultType.Result_OK)
                {
                    // Capturamos pago
                    string strUserReference = null;
                    string strSecundaryTransactionId = null;
                    int iTransactionFee = 0;
                    string strTransactionFeeCurrencyIsocode = null;
                    string strTransactionURL = null;
                    string strRefundTransactionURL = null;


                    DateTime dt;
                    DateTime.TryParseExact(strGatewayDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);

                    if (eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA || CommitTransaction(model.InstallationId, eProviderType, strTransactionId, strCFTransactionId, model.CurrencyId, dt, strAuthCode, model.TotalQuantity, model.AmountCurrencyIsoCode, out strUserReference, out strAuthResult, out strGatewayDate, out strSecundaryTransactionId, out iTransactionFee, out strTransactionFeeCurrencyIsocode, out strTransactionURL, out strRefundTransactionURL))
                    {

                        string culture = Session["Culture"].ToString();
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                        integraMobile.Properties.Resources.Culture = ci;

                        int iQuantity = model.Quantity;
                        decimal dPercVAT1 = model.PercVAT1;
                        decimal dPercVAT2 = model.PercVAT2;
                        decimal dPercFEE = model.PercFEE;
                        int iPercFEETopped = (int)(model.PercFEETopped);
                        int iFixedFEE = (int)(model.FixedFEE);
                        int iPartialVAT1 = model.PartialVAT1;
                        int iPartialPercFEE = model.PartialPercFEE;
                        int iPartialFixedFEE = model.PartialFixedFEE;
                        int iPartialPercFEEVAT = model.PartialPercFEEVAT;
                        int iPartialFixedFEEVAT = model.PartialPercFEEVAT;
                        int iQFEE = model.QFEE;
                        int iQVAT = model.QVAT;

                        int iLayout = 0;
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            OPERATOR oOperator = customersRepository.GetDefaultOperator();
                            if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                        }

                        string sLayoutSubtotal = "";
                        string sLayoutTotal = "";
                        string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(model.CurrencyId));

                        if (iLayout == 2)
                        {
                            sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / 100,
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }
                        else if (iLayout == 1)
                        {
                            sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }

                        string strRechargeEmailSubject = ResourceExtension.GetLiteral("ConfirmFinePayment_EmailHeader");
                        /*
                        *  ID: {0}<br>
                        *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                        *  Total Pagado: {2} 
                        *  Matrícula: 3
                        */

                        string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmTicketPaymentNonUser_EmailBody"),
                            model.TicketNumber,
                            dt,
                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.TotalQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                            infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                            model.Plate,
                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                            sLayoutSubtotal, sLayoutTotal,
                            GetEmailFooter(model.InstallationShortDesc, model.CountryCode));

                        if (model.Email != ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString())
                        {
                            SendEmail(model.Email, strRechargeEmailSubject, strRechargeEmailBody);
                        }
                        else
                        {
                            Session["strRechargeEmailSubject"] = strRechargeEmailSubject;
                            Session["strRechargeEmailBody"] = strRechargeEmailBody;
                        }

                        if (CCSuccess(strReference,
                                       strTransactionId,
                                       null,
                                       strGatewayDate,
                                       strAuthCode,
                                       strAuthResult,
                                       "",
                                       strCardHash,
                                       strCardReference,
                                       strCardScheme,
                                       strMaskedCardNumber,
                                       PaymentMeanRechargeStatus.Waiting_Commit,
                                       dtExpDate))
                        {
                            return View(model);
                        }
                    }
                }
                else
                {
                    xmlNodeValue = GetXMLNodeValue(res, "ConfirmFinePayment");
                    /* Si ConfirmFinePayment = 1 quiere decir que falló después de hacerse el pago, es decir, 
                    *  durante la confirmación.
                    *  Si esto ocurre, y se trata de IECISA, hay que generar devolución.
                    */
                    if (xmlNodeValue == 1 && eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA)
                    {
                        string strUserReference = null;
                        string strRefundTransactionId = null;
                        string strAuthResultDesc = null;
                        RefundTransaction(model.InstallationId, eProviderType, strTransactionId, strCFTransactionId, model.CurrencyId, model.TotalQuantity, strAuthCode, model.AmountCurrencyIsoCode, out strUserReference, out strAuthCode, out strAuthResult, out strAuthResultDesc, out strGatewayDate, out strRefundTransactionId);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CCSuccess Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CCSuccess: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));


            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InCreditCallPayment"] = null;
            return RedirectToAction("CCFailure");
        }

        private bool CCSuccess(string strReference,
                                string strTransactionId,
                                string strCFTransactionId,
                                string strGatewayDate,
                                string strAuthCode,
                                string strAuthResult,
                                string strAuthResultDesc,
                                string strCardHash,
                                string strCardReference,
                                string strCardScheme,
                                string strMaskedCardNumber,
                                PaymentMeanRechargeStatus rechargeStatus,
                                DateTime? dtExpDate)
        {
            bool bRes = false;
            try
            {
                SetCulture();

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                ViewData["PayerQuantity"] = Session["QuantityToRecharge"].ToString();
                ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"].ToString();
                int iQuantity = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRechargeBase"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));

                ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);

                //decimal? dRechargeId = null;
                //bool bRefundServiceCharge = false;

                decimal dPercVAT1 = 0;
                decimal dPercVAT2 = 0;
                decimal dPercFEE = 0;
                int iPercFEETopped = 0;
                int iFixedFEE = 0;


                int iPartialVAT1 = 0;
                int iPartialPercFEE = 0;
                int iPartialFixedFEE = 0;

                int iTotalQuantity = iQuantity;

                if (chargeType == ChargeOperationsType.ServiceCharge)
                {
                    //bRefundServiceCharge = true;
                    rechargeStatus = rechargeStatus == PaymentMeanRechargeStatus.Waiting_Commit ? PaymentMeanRechargeStatus.Waiting_Cancellation : PaymentMeanRechargeStatus.Waiting_Refund;
                }
                else
                {

                    dPercVAT1 = (Session["PercVAT1"] != null ? Convert.ToDecimal(Session["PercVAT1"].ToString(), provider) : 0);
                    dPercVAT2 = (Session["PercVAT2"] != null ? Convert.ToDecimal(Session["PercVAT2"].ToString(), provider) : 0);
                    dPercFEE = (Session["PercFEE"] != null ? Convert.ToDecimal(Session["PercFEE"].ToString(), provider) : 0);
                    iPercFEETopped = (Session["PercFEETopped"] != null ? Convert.ToInt32(Session["PercFEETopped"], provider) : 0);
                    iFixedFEE = (Session["FixedFEE"] != null ? Convert.ToInt32(Session["FixedFEE"], provider) : 0);
                    iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);

                }
                PaymentMeanRechargeCreationType rechargeCreationType = PaymentMeanRechargeCreationType.pmrctRegularRecharge;

                if (Session["RechargeCreationType"] != null)
                {
                    rechargeCreationType = (PaymentMeanRechargeCreationType)Session["RechargeCreationType"];
                }

                bRes = true;
                Session["Sess_strReference"] = null;
                Session["Sess_strTransactionId"] = null;
                Session["Sess_strGatewayDate"] = null;
                Session["Sess_strAuthCode"] = null;
                Session["Sess_strAuthResult"] = null;
                Session["Sess_strAuthResultDesc"] = null;
                Session["Sess_strCardHash"] = null;
                Session["Sess_strCardReference"] = null;
                Session["Sess_strCardScheme"] = "";
                Session["Sess_strMaskedCardNumber"] = null;
                Session["Sess_dtExpDate"] = null;
                Session["QuantityToRecharge"] = null;
                Session["QuantityToRechargeBase"] = null;
                Session["PercVAT1"] = null;
                Session["PercVAT2"] = null;
                Session["PercFEE"] = null;
                Session["PercFEETopped"] = null;
                Session["FixedFEE"] = null;
                Session["CurrencyToRecharge"] = null;

                Session["InCreditCallPayment"] = null; // CC
                Session["InIECISAPayment"] = null; // CC2
                Session["InStripePayment"] = null; // CC3
                Session["InPayuPayment"] = null; // Payu
                Session["InTransBankPayment"] = null; // TransBank
                Session["InMonerisPayment"] = null; // Moneris

                Session["RechargeCreationType"] = null;

            }
            catch
            {
                bRes = false;
            }

            return bRes;

        }


        #endregion

        #region RedSys payment

        [HttpGet]
        public ActionResult BSRedsysRequest()
        {
            SetCulture();

            FineModel model = (FineModel)Session["FineModel"];

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
            INSTALLATION oInstallation = null;
            DateTime? dtinstDateTime = null;
            geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

            oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

            if (oGatewayConfig != null &&
                        !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {

                oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                     .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                 r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                 r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpBSRedsys))
                                     .FirstOrDefault();
            }

            Session["Redsys_Config_id"] = oGatewayConfig.CPTGC_BSRCON_ID;
            string Email = model.Email;
            int Amount = model.TotalQuantity;
            string CurrencyISOCODE = model.AmountCurrencyIsoCode;
            string Description = model.TicketNumber.ToString();
            string UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string Hash = string.Empty;
            string ReturnURL = "";
            string culture = ((CultureInfo)Session["Culture"]).Name;


            Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
            Session["Plate"] = model.Plate;
            Session["Total"] = model.Total;
            Session["QFEE"] = model.QFEE;
            Session["QVAT"] = model.QVAT;
            Session["TotalQuantity"] = model.TotalQuantity;

            Session["PAYMENT_ORIGIN"] = "FineController";

            BSRedsysController Redsys = new BSRedsysController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Redsys_Config_id"]));
            return Redsys.BSRedsysRequest(string.Empty, Email, Amount, CurrencyISOCODE, Description, UTCDate, culture, ReturnURL, Hash);
        }

        [HttpGet]
        public ActionResult BSRedsysFailure()
        { 
            SetCulture();

            BSRedsysController Redsys = new BSRedsysController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Redsys_Config_id"]));
            return Redsys.BSRedsysReturn("error",
                                  Request["Ds_SignatureVersion"],
                                  Request["Ds_MerchantParameters"],
                                  Request["Ds_Signature"]);
        }

        public ActionResult BSRedsysResult(string submitButton)
        { 
            SetCulture();

            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    return RedirecToactionBySession();
                }
                else if (Session["RedsysResult"] != null)
                {
                    Session["RedsysResult"] = null;
                    return RedirectToAction("Fine");
                }
                else if(Session["ReturnFine"]==null && Session["RedsysResult"]==null)
                {
                    return RedirectToAction("Fine");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult BSRedsysResult(string r, string submitButton)
        {
            SetCulture();

            BSRedsysController Redsys = new BSRedsysController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Redsys_Config_id"]));
            string r_decrypted = Redsys.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                if (Convert.ToBoolean(Session["InBSRedsysPayment"]))
                {
                    Session["InBSRedsysPayment"] = null;
                    ResultType res = ResultType.Result_OK;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("RedsysResult::-->ConfirmPayment: "));
                    res = ConfirmPayment(j);
                    Session["RedsysResult"] = j.result;
                    if (res != ResultType.Result_OK)
                    {
                        return RedirectToAction("CCFailure");
                    }
                }
            }

            Session["result"] = null;
            Session["errorCode"] = null;
            Session["errorMessage"] = null;
            Session["cardReference"] = null;
            Session["cardHash"] = null;
            Session["cardScheme"] = null;
            Session["cardPAN"] = null;
            Session["cardExpMonth"] = null;
            Session["cardExpYear"] = null;
            Session["chargeDateTime"] = null;
            Session["reference"] = null;
            Session["transactionID"] = null;
            Session["authCode"] = null;
            Session["authResult"] = null;
            Session["email"] = null;
            Session["amount"] = null;
            Session["currency"] = null;
            Session["utcdate"] = null;
            Session["MonerisGuid"] = null;

            ViewData["result"] = r_decrypted;

            return View();
        }

        [HttpPost]
        public ActionResult BSRedsysSuccess()
        { 
            SetCulture();

            BSRedsysController Redsys = new BSRedsysController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Redsys_Config_id"]));
            return Redsys.BSRedsysReturn("succeeded",
                                  Request["Ds_SignatureVersion"],
                                  Request["Ds_MerchantParameters"],
                                  Request["Ds_Signature"]);
        }        

        #endregion

        #region IECISA Payment

        public ActionResult CC2Redirect(FineModel model)
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure");

            model = (FineModel)Session["FineModel"];

            // Set current operation
            Session["OperationChargeType"] = ChargeOperationsType.TicketPayment;
            Session["QuantityToRecharge"] = model.TotalQuantity;
            Session["QuantityToRechargeBase"] = model.Total;
            Session["CurrencyToRecharge"] = model.AmountCurrencyIsoCode;

            ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider));
            DateTime dtNow = DateTime.Now;
            DateTime dtUTCNow = DateTime.UtcNow;

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

            if (oGatewayConfig == null)
            {
                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

                oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                            .FirstOrDefault();
                }
            }

            string strTransactionId = null;
            string strOpReference = null;
            string errorMessage = null;
            string strCardHash = null;

            IECISAPayments cardPayment = new IECISAPayments();

            var uri = new Uri(Request.Url.AbsoluteUri);
            string strURLPath = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/"));
            string strLang = ((CultureInfo)Session["Culture"]).Name.Substring(0, 2);
            IECISAPayments.IECISAErrorCode eErrorCode;

            cardPayment.StartWebTransaction(oGatewayConfig.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_USER,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                            oGatewayConfig.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                            strURLPath + "/CC2Reply",
                                            strURLPath + "/CC2Reply",
                                            model.Email,
                                            strLang,
                                            iQuantityToRecharge,
                                            (string)Session["CurrencyToRecharge"],
                                            infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode((string)Session["CurrencyToRecharge"]),
                                            false,
                                            DateTime.Now,
                                            out eErrorCode,
                                            out errorMessage,
                                            out strTransactionId,
                                            out strOpReference,
                                            out strCardHash);

            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
            {
                string errorCode = eErrorCode.ToString();

                m_Log.LogMessage(LogLevels.logERROR, string.Format("IECISARequest.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                          errorCode, errorMessage));

                return RedirectToAction("CC2Failure");


            }
            else
            {
                string strRedirectURL = "";
                cardPayment.GetWebTransactionPaymentTypes(oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                        oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                        strTransactionId,
                                                        out eErrorCode,
                                                        out errorMessage,
                                                        out strRedirectURL);
                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                {
                    string errorCode = eErrorCode.ToString();

                    m_Log.LogMessage(LogLevels.logERROR, string.Format("IECISARequest.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                              errorCode, errorMessage));

                    return RedirectToAction("CC2Failure");

                }
                else
                {
                    customersRepository.StartRecharge(oGatewayConfig.CPTGC_ID,
                                                           model.Email,
                                                           dtUTCNow,
                                                           dtNow,
                                                           iQuantityToRecharge,
                                                           infraestructureRepository.GetCurrencyFromIsoCode((string)Session["CurrencyToRecharge"]),
                                                           "",
                                                           strOpReference,
                                                           strTransactionId,
                                                           "",
                                                           "",
                                                           "",
                                                           PaymentMeanRechargeStatus.Committed);
                    Session["cardHash"] = strCardHash;
                    //Session["USER_ID"] = oUser.USR_ID;

                    return Redirect(strRedirectURL);
                }

            }

        }

        public ActionResult CC2Reply(string transactionId)
        {
            SetCulture();

            string strCardReference = "";
            string strMaskedCardNumber = "";

            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure");

            FineModel model = (FineModel)Session["FineModel"];

            try
            {
                if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                    return RedirectToAction("CC2Failure");

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

                if (oGatewayConfig == null)
                {

                    INSTALLATION oInstallation = null;
                    DateTime? dtinstDateTime = null;

                    geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);



                    oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                    if (oGatewayConfig != null &&
                               !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                    {
                        oGatewayConfig = null;
                    }

                    if (oGatewayConfig == null)
                    {

                        oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                                .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                            ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                                .FirstOrDefault();

                    }
                }


                IECISAPayments cardPayment = new IECISAPayments();
                IECISAPayments.IECISAErrorCode eErrorCode;
                DateTime? dtExpDate = null;
                DateTime? dtTransactionDate = null;
                string strExpMonth = "";
                string strExpYear = "";
                string errorMessage = "";
                string strOpReference = "";
                string strAuthCode = "";
                string strCFTransactionID = "";


                cardPayment.GetTransactionStatus(oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                 oGatewayConfig.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                 oGatewayConfig.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                 transactionId,
                                                out eErrorCode,
                                                out errorMessage,
                                                out strMaskedCardNumber,
                                                out strCardReference,
                                                out dtExpDate,
                                                out strExpMonth,
                                                out strExpYear,
                                                out dtTransactionDate,
                                                out strOpReference,
                                                out strCFTransactionID,
                                                out strAuthCode);

                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                {

                    INSTALLATION oInstallation = null;
                    DateTime? dtinstDateTime = null;

                    geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);
                    //model.InstallationObject = oInstallation;
                    model.CurrencyId = oInstallation.CURRENCy.CUR_ID;
                    model.InstallationShortDesc = oInstallation.INS_SHORTDESC;
                    model.CountryCode = oInstallation.COUNTRy.COU_CODE;
                    model.CountryName = oInstallation.COUNTRy.COU_DESCRIPTION;



                    oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                    if (oGatewayConfig != null &&
                               !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                    {
                        oGatewayConfig = null;
                    }

                    if (oGatewayConfig == null)
                    {


                        oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                                .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                            ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                                .FirstOrDefault();
                    }

                    customersRepository.FailedRecharge(oGatewayConfig.CPTGC_ID,
                                    model.Email,
                                    transactionId,
                                    PaymentMeanRechargeStatus.Cancelled);

                    RedirectToRouteResult res = null;

                    string errorCode = eErrorCode.ToString();
                    switch (eErrorCode)
                    {

                        case IECISAPayments.IECISAErrorCode.TransactionCancelled:
                        case IECISAPayments.IECISAErrorCode.TransactionCancelled2:
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("iecisaResponse.GetTransactionStatus : errorCode={0} ; errorMessage={1}", errorCode, errorMessage));
                                res = RedirectToAction("CC2Cancel");
                            }
                            break;

                        default:
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("iecisaResponse.GetTransactionStatus : errorCode={0} ; errorMessage={1}", errorCode, errorMessage));
                                res = RedirectToAction("CC2Failure");
                            }
                            break;
                    }
                    return res;
                }

                Session["Sess_strMaskedCardNumber"] = strMaskedCardNumber;

                if (strCardReference == null) strCardReference = string.Empty;
                Session["Sess_strCardReference"] = strCardReference;

                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Reply: Exiting with CC2Success: errorCode={0}; Card Reference={1}; AuthCode={2}", eErrorCode.ToString(), strCardReference, strAuthCode));

                Session["Sess_strReference"] = strOpReference;
                Session["Sess_strTransactionId"] = transactionId;
                Session["Sess_strCFTransactionId"] = strCFTransactionID;
                Session["Sess_strGatewayDate"] = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                Session["Sess_strAuthCode"] = strAuthCode;
                Session["Sess_strAuthResult"] = "succeeded";
                Session["Sess_strAuthResultDesc"] = errorMessage;
                Session["Sess_strCardHash"] = Session["cardHash"].ToString();
                Session["Sess_strCardReference"] = strCardReference;
                Session["Sess_strCardScheme"] = "";
                Session["Sess_dtExpDate"] = dtExpDate;



                return RedirectToAction("CC2Success");


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            return RedirectToAction("CC2Failure");
        }

        public ActionResult CC2Success()
        {
            SetCulture();

            string strMaskedCardNumber = "";
            string strCardReference = "";

            try
            {
                if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                    return RedirectToAction("CC2Failure");

                FineModel model = (FineModel)Session["FineModel"];

                string strReference = Session["Sess_strReference"].ToString();
                string strTransactionId = Session["Sess_strTransactionId"].ToString();
                string strCFTransactionId = Session["Sess_strCFTransactionId"].ToString();
                string strGatewayDate = Session["Sess_strGatewayDate"].ToString();
                string strAuthCode = Session["Sess_strAuthCode"].ToString();
                string strAuthResult = Session["Sess_strAuthResult"].ToString();
                string strAuthResultDesc = Session["Sess_strAuthResultDesc"].ToString();
                string strCardHash = Session["Sess_strCardHash"].ToString();
                strCardReference = Session["Sess_strCardReference"].ToString();
                string strCardScheme = Session["Sess_strCardScheme"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
                DateTime? dtExpDate = (DateTime?)Session["Sess_dtExpDate"];

                PaymentMeanCreditCardProviderType eProviderType = (PaymentMeanCreditCardProviderType)Session["ProviderType"];

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                            .FirstOrDefault();
                }

                int iWSTimeout = infraestructureRepository.GetRateWSTimeout(model.InstallationId);

                string res = external.ConfirmFinePaymentNonUser(model.TicketNumber, model.Quantity,
                                                model.TotalQuantity, model.Plate, model.PercFEE, model.PercVAT1,
                                                model.PercVAT2, model.PercFEETopped, model.FixedFEE, model.PartialVAT1,
                                                model.PartialPercFEE, model.PartialFixedFEE, model.PartialPercFEEVAT,
                                                model.PartialFixedFEEVAT, model.TaxMode, model.Email, strReference, strTransactionId,
                                                strCFTransactionId, strGatewayDate, strAuthCode, strAuthResult,
                                                strCardHash, strCardReference, strCardScheme, strMaskedCardNumber, null, null,
                                                dtExpDate, eProviderType, model.InstallationId, geograficAndTariffsRepository,
                                                model.CurrencyId, model.AmountCurrencyIsoCode, oGatewayConfig.CPTGC_ID, fineRepository, model.GrpId, model.AuthId, iWSTimeout);

                // SUCCESS res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><autorecharged>0</autorecharged><r>1</r><utc_offset>-420</utc_offset></ipark_out>
                // ERROR   res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><r>-9</r></ipark_out>

                // Extraemos el valor de r
                int xmlNodeValue = GetXMLNodeValue(res, "r");
                if ((ExternalWS.ResultType)xmlNodeValue == ExternalWS.ResultType.Result_OK)
                {
                    // Capturamos pago
                    string strUserReference = null;
                    string strSecundaryTransactionId = null;
                    int iTransactionFee = 0;
                    string strTransactionFeeCurrencyIsocode = null;
                    string strTransactionURL = null;
                    string strRefundTransactionURL = null;

                    DateTime dt;
                    DateTime.TryParseExact(strGatewayDate, "HHmmssddMMyyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);

                    if (eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA || CommitTransaction(model.InstallationId, eProviderType, strTransactionId, strCFTransactionId, model.CurrencyId, dt, strAuthCode, model.TotalQuantity, model.AmountCurrencyIsoCode, out strUserReference, out strAuthResult, out strGatewayDate, out strSecundaryTransactionId, out iTransactionFee, out strTransactionFeeCurrencyIsocode, out strTransactionURL, out strRefundTransactionURL))
                    {

                        string culture = Session["Culture"].ToString();
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                        integraMobile.Properties.Resources.Culture = ci;

                        int iQuantity = model.Quantity;
                        decimal dPercVAT1 = model.PercVAT1;
                        decimal dPercVAT2 = model.PercVAT2;
                        decimal dPercFEE = model.PercFEE;
                        int iPercFEETopped = (int)(model.PercFEETopped);
                        int iFixedFEE = (int)(model.FixedFEE);

                        int iPartialVAT1 = model.PartialVAT1;
                        int iPartialPercFEE = model.PartialPercFEE;
                        int iPartialFixedFEE = model.PartialFixedFEE;
                        int iPartialPercFEEVAT = model.PartialPercFEEVAT;
                        int iPartialFixedFEEVAT = model.PartialPercFEEVAT;
                        int iQFEE = model.QFEE;
                        int iQVAT = model.QVAT;

                        int iLayout = 0;
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            OPERATOR oOperator = customersRepository.GetDefaultOperator();
                            if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                        }

                        string sLayoutSubtotal = "";
                        string sLayoutTotal = "";

                        string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(model.CurrencyId));

                        if (iLayout == 2)
                        {
                            sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }
                        else if (iLayout == 1)
                        {
                            sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }

                        string strRechargeEmailSubject = ResourceExtension.GetLiteral("ConfirmFinePayment_EmailHeader");
                        /*
                            ID: {0}<br>
                            *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                            *  Total Pagado: {2} 
                            *  Matrícula: 3
                            */

                        string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmTicketPaymentNonUser_EmailBody"),
                            model.TicketNumber,
                            dt,
                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.TotalQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                            infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                            model.Plate,
                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                            sLayoutSubtotal, sLayoutTotal,
                            GetEmailFooter(model.InstallationShortDesc, model.CountryCode));

                        if (model.Email != ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString())
                        {
                            SendEmail(model.Email, strRechargeEmailSubject, strRechargeEmailBody);
                        }
                        else
                        {
                            Session["strRechargeEmailSubject"] = strRechargeEmailSubject;
                            Session["strRechargeEmailBody"] = strRechargeEmailBody;
                        }

                        customersRepository.CompleteStartRecharge(oGatewayConfig.CPTGC_ID,
                                                        model.Email,
                                                        strTransactionId,
                                                        strAuthResult,
                                                        strCFTransactionId,
                                                        strGatewayDate,
                                                        strAuthCode,
                                                        PaymentMeanRechargeStatus.Committed);

                        if (CCSuccess(strReference,
                                       strTransactionId,
                                       strCFTransactionId,
                                       strGatewayDate,
                                       strAuthCode,
                                       strAuthResult,
                                       strAuthResultDesc,
                                       strCardHash,
                                       strCardReference,
                                       strCardScheme,
                                       strMaskedCardNumber,
                                       PaymentMeanRechargeStatus.Committed,
                                       dtExpDate))
                        {
                            return View(model);
                        }
                        else
                        {



                            customersRepository.FailedRecharge(oGatewayConfig.CPTGC_ID,
                                                model.Email,
                                                strTransactionId,
                                                PaymentMeanRechargeStatus.Cancelled);
                        }
                    }
                }
                else
                {
                    xmlNodeValue = GetXMLNodeValue(res, "ConfirmFinePayment");
                    /* Si ConfirmFinePayment = 1 quiere decir que falló después de hacerse el pago, es decir, 
                    *  durante la confirmación.
                    *  Si esto ocurre, y se trata de IECISA, hay que generar devolución.
                    */
                    if (xmlNodeValue == 1 && eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA)
                    {
                        string strUserReference = null;
                        string strRefundTransactionId = null;
                        RefundTransaction(model.InstallationId, eProviderType, strTransactionId, strCFTransactionId, model.CurrencyId, model.TotalQuantity, strAuthCode, model.AmountCurrencyIsoCode, out strUserReference, out strAuthCode, out strAuthResult, out strAuthResultDesc, out strGatewayDate, out strRefundTransactionId);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Success: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            Session["Sess_strReference"] = null;
            Session["Sess_strTransactionId"] = null;
            Session["Sess_strCFTransactionId"] = null;
            Session["Sess_strGatewayDate"] = null;
            Session["Sess_strAuthCode"] = null;
            Session["Sess_strAuthResult"] = null;
            Session["Sess_strAuthResultDesc"] = null;
            Session["Sess_strCardHash"] = null;
            Session["Sess_strCardReference"] = null;
            Session["Sess_strCardScheme"] = "";
            Session["Sess_strMaskedCardNumber"] = null;
            Session["Sess_dtExpDate"] = null;

            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InIECISAPayment"] = null;
            Session["cardHash"] = null;
            return RedirectToAction("CC2Failure");
        }

        public ActionResult CC2Failure()
        {
            SetCulture();

            //USER oUser = GetUserFromSession();

            //if (oUser == null)
            //{
            //    return LogOff();
            //}

            string strCardReference = "";
            string strMaskedCardNumber = "";

            try
            {
                strCardReference = Session["Sess_strCardReference"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
            }
            catch { }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Failure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            try
            {
                Console.Write("stop");
                //ViewData["oUser"] = oUser;
                //ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                //ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
                //ViewData["CurrencyISOCode"] = oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE;
                ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
                ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
                ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(Session["CurrencyToRecharge"].ToString()) + "}",
                    Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));
                ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];


                Session["Sess_strCardReference"] = null;
                Session["Sess_strMaskedCardNumber"] = null;

                Session["QuantityToRecharge"] = null;
                Session["QuantityToRechargeBase"] = null;
                Session["PercVAT1"] = null;
                Session["PercVAT2"] = null;
                Session["PercFEE"] = null;
                Session["PercFEETopped"] = null;
                Session["FixedFEE"] = null;
                Session["CurrencyToRecharge"] = null;
                Session["InIECISAPayment"] = null;
                Session["OVERWRITE_CARD"] = false;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC2Failure Exception: {0}", e.Message));
            }

            return View();
        }

        public ActionResult CC2Cancel()
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2Failure");

            //USER oUser = GetUserFromSession();

            //if (oUser == null)
            //{
            //    return LogOff();
            //}

            Console.Write("Stop");
            //ViewData["oUser"] = oUser;
            //ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
            //ViewData["UserBalance"] = Convert.ToDouble(oUser.USR_BALANCE);
            //ViewData["CurrencyISOCode"] = oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE;
            ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
            ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(Session["CurrencyToRecharge"].ToString()) + "}",
                Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));
            ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];

            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InIECISAPayment"] = null;
            Session["OVERWRITE_CARD"] = false;


            return View();
        }

        #endregion

        #region Stripe Payment

        public ActionResult CC3Redirect(FineModel model)
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure");

            model = (FineModel)Session["FineModel"];

            // Set current operation
            Session["OperationChargeType"] = ChargeOperationsType.TicketPayment;
            Session["QuantityToRecharge"] = model.TotalQuantity;
            Session["QuantityToRechargeBase"] = model.Total;
            Session["CurrencyToRecharge"] = model.AmountCurrencyIsoCode;

            ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider));
            DateTime dtNow = DateTime.Now;

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

            if (oGatewayConfig == null)
            {

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

                oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                                .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                            ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                                                .FirstOrDefault();
                }
            }

            string strImageURL = DEFAULT_STRIPE_IMAGE_URL;

            if (!string.IsNullOrEmpty(oGatewayConfig.STRIPE_CONFIGURATION.STRCON_IMAGE_URL))
            {
                strImageURL = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_IMAGE_URL;
            }

            //ViewData["SuscriptionTypeGeneral"] = 1; // usually obtained via user
            ViewData["email"] = model.Email;
            ViewData["amount"] = iQuantityToRecharge.ToString(); ;
            ViewData["currency"] = (string)Session["CurrencyToRecharge"];
            ViewData["key"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_DATA_KEY;
            ViewData["commerceName"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_COMMERCE_NAME;
            ViewData["panelLabel"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_PANEL_LABEL;
            ViewData["description"] = ResourceExtension.GetLiteral("FineTicketPayment");
            ViewData["image"] = strImageURL;

            return View();
        }

        public ActionResult CC3Reply(StripeResponseModel oModel)
        {
            SetCulture();

            string strPAN = "";
            string strCardToken = "";

            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure");

            FineModel model = (FineModel)Session["FineModel"];

            /*
            // We don't have a logged user when paying fines through the website
            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }
            */
            try
            {
                if (!Convert.ToBoolean(Session["InStripePayment"]))
                    return RedirectToAction("CC3Failure");

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
                /*
                // We don't have a logged user when paying fines through the website 
                if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                {
                    oGatewayConfig = oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG;
                }
                */
                if (oGatewayConfig == null)
                {
                    INSTALLATION oInstallation = null;
                    DateTime? dtinstDateTime = null;

                    geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

                    oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                    if (oGatewayConfig != null &&
                               !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                    {
                        oGatewayConfig = null;
                    }

                    if (oGatewayConfig == null)
                    {

                        oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                                .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                            ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                                                .FirstOrDefault();
                    }
                }

                if (!string.IsNullOrEmpty(oModel.stripeErrorCode) && oModel.stripeErrorCode == "window_closed")
                {
                    return RedirectToAction("CC3Cancel");
                }
                else
                {
                    if (!string.IsNullOrEmpty(oModel.stripeToken))
                    {
                        strCardToken = oModel.stripeToken;

                        if (oModel.stripeEmail == model.Email)
                        {
                            NumberFormatInfo provider = new NumberFormatInfo();
                            provider.NumberDecimalSeparator = ".";
                            string strCustomerId = "";
                            int iQuantityToRecharge = Convert.ToInt32(Convert.ToDouble(Session["QuantityToRecharge"].ToString(), provider) * infraestructureRepository.GetCurrencyDivisorFromIsoCode((string)Session["CurrencyToRecharge"]));

                            string result = "";
                            string errorMessage = "";
                            string errorCode = "";
                            string strChargeID = "";
                            string strCardScheme = "";
                            string strStripeDateTime = "";
                            string strExpirationDateMonth = "";
                            string strExpirationDateYear = "";

                            if (StripePayments.PerformCharge(oGatewayConfig.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                            oModel.stripeEmail,
                                                            strCardToken,
                                                            ref strCustomerId,
                                                            iQuantityToRecharge,
                                                            Session["CurrencyToRecharge"].ToString(),
                                                            false,
                                                            out result,
                                                            out errorCode,
                                                            out errorMessage,
                                                            out strCardScheme,
                                                            out strPAN,
                                                            out strExpirationDateMonth,
                                                            out strExpirationDateYear,
                                                            out strChargeID,
                                                            out strStripeDateTime))
                            {

                                DateTime dtExpDate = DateTime.UtcNow;
                                if ((strExpirationDateMonth.Length == 2) && (strExpirationDateYear.Length == 4))
                                {
                                    dtExpDate = new DateTime(Convert.ToInt32(strExpirationDateYear), Convert.ToInt32(strExpirationDateMonth), 1).AddMonths(1).AddSeconds(-1);
                                }

                                Session["Sess_strMaskedCardNumber"] = strPAN;
                                Session["Sess_strCardReference"] = oModel.stripeToken;
                                Session["Sess_strReference"] = strChargeID;
                                Session["Sess_strTransactionId"] = strChargeID;
                                Session["Sess_strGatewayDate"] = DateTime.ParseExact(strStripeDateTime, "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                Session["Sess_strAuthCode"] = "";
                                Session["Sess_strAuthResult"] = "succeeded";
                                Session["Sess_strAuthResultDesc"] = "";
                                Session["Sess_strCardHash"] = strCustomerId;
                                Session["Sess_strCardScheme"] = strCardScheme;
                                Session["Sess_dtExpDate"] = dtExpDate;

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Success: Email={0}; ChargeID={1}; PAN={2}", oModel.stripeEmail, strChargeID, strPAN));
                                return RedirectToAction("CC3Success");

                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Failure: Error Performing charge Email={0}; ChargeID={1}; PAN={2}; Error={3}; ErrorMsg={4}", oModel.stripeEmail, strChargeID, strPAN, errorCode, errorMessage));
                            }

                        }
                        else
                        {
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Failure: Email not match Email={0} | UserEMail = {1}", oModel.stripeEmail, model.Email));
                        }

                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Reply: Exiting with CC3Failure: Empty Token Received Email={0} | UserEMail = {1}", oModel.stripeEmail, model.Email));
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Success Exception: {0}", e.Message));
            }

            return RedirectToAction("CC3Failure");
        }

        public ActionResult CC3Success()
        {
            SetCulture();

            string strMaskedCardNumber = "";
            string strCardReference = "";

            try
            {
                if (!Convert.ToBoolean(Session["InStripePayment"]))
                    return RedirectToAction("CC3Failure");

                FineModel model = (FineModel)Session["FineModel"];

                string strReference = Session["Sess_strReference"].ToString();
                string strTransactionId = Session["Sess_strTransactionId"].ToString();
                string strGatewayDate = Session["Sess_strGatewayDate"].ToString();
                string strAuthCode = Session["Sess_strAuthCode"].ToString();
                string strAuthResult = Session["Sess_strAuthResult"].ToString();
                string strAuthResultDesc = Session["Sess_strAuthResultDesc"].ToString();
                string strCardHash = Session["Sess_strCardHash"].ToString();
                strCardReference = Session["Sess_strCardReference"].ToString();
                string strCardScheme = Session["Sess_strCardScheme"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
                DateTime? dtExpDate = (DateTime?)Session["Sess_dtExpDate"];

                string strCFTransactionId = String.Empty;
                if (Session["Sess_strCFTransactionId"] != null)
                {
                    strCFTransactionId = Session["Sess_strCFTransactionId"].ToString();
                }

                PaymentMeanCreditCardProviderType eProviderType = (PaymentMeanCreditCardProviderType)Session["ProviderType"];


                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {

                    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                                            .FirstOrDefault();
                }

                int iWSTimeout = infraestructureRepository.GetRateWSTimeout(model.InstallationId);

                string res = external.ConfirmFinePaymentNonUser(model.TicketNumber, model.Quantity,
                                model.TotalQuantity, model.Plate, model.PercFEE, model.PercVAT1,
                                model.PercVAT2, model.PercFEETopped, model.FixedFEE, model.PartialVAT1,
                                model.PartialPercFEE, model.PartialFixedFEE, model.PartialPercFEEVAT,
                                model.PartialFixedFEEVAT, model.TaxMode, model.Email, strReference, strTransactionId,
                                strCFTransactionId, strGatewayDate, strAuthCode, strAuthResult,
                                strCardHash, strCardReference, strCardScheme, strMaskedCardNumber, null, null,
                                dtExpDate, eProviderType, model.InstallationId, geograficAndTariffsRepository,
                                model.CurrencyId, model.AmountCurrencyIsoCode, oGatewayConfig.CPTGC_ID, fineRepository, model.GrpId, model.AuthId, iWSTimeout);

                // SUCCESS res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><autorecharged>0</autorecharged><r>1</r><utc_offset>-420</utc_offset></ipark_out>
                // ERROR   res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><r>-9</r></ipark_out>

                // Extraemos el valor de r
                int xmlNodeValue = GetXMLNodeValue(res, "r");
                if ((ExternalWS.ResultType)xmlNodeValue == ExternalWS.ResultType.Result_OK)
                {
                    // Capturamos pago
                    string strUserReference = null;
                    string strSecundaryTransactionId = null;
                    int iTransactionFee = 0;
                    string strTransactionFeeCurrencyIsocode = null;
                    string strTransactionURL = null;
                    string strRefundTransactionURL = null;


                    DateTime dt;
                    DateTime.TryParseExact(strGatewayDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);

                    if (eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA || CommitTransaction(model.InstallationId, eProviderType, strTransactionId, strCFTransactionId, model.CurrencyId, dt, strAuthCode, model.TotalQuantity, model.AmountCurrencyIsoCode, out strUserReference, out strAuthResult, out strGatewayDate, out strSecundaryTransactionId, out iTransactionFee, out strTransactionFeeCurrencyIsocode, out strTransactionURL, out strRefundTransactionURL))
                    {
                        string culture = Session["Culture"].ToString();
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                        integraMobile.Properties.Resources.Culture = ci;

                        int iQuantity = model.Quantity;
                        decimal dPercVAT1 = model.PercVAT1;
                        decimal dPercVAT2 = model.PercVAT2;
                        decimal dPercFEE = model.PercFEE;
                        int iPercFEETopped = (int)(model.PercFEETopped);
                        int iFixedFEE = (int)(model.FixedFEE);

                        int iPartialVAT1 = model.PartialVAT1;
                        int iPartialPercFEE = model.PartialPercFEE;
                        int iPartialFixedFEE = model.PartialFixedFEE;
                        int iPartialPercFEEVAT = model.PartialPercFEEVAT;
                        int iPartialFixedFEEVAT = model.PartialPercFEEVAT;
                        int iQFEE = model.QFEE;
                        int iQVAT = model.QVAT;

                        int iLayout = 0;
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            OPERATOR oOperator = customersRepository.GetDefaultOperator();
                            if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                        }

                        string sLayoutSubtotal = "";
                        string sLayoutTotal = "";

                        string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(model.CurrencyId));

                        if (iLayout == 2)
                        {
                            sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }
                        else if (iLayout == 1)
                        {
                            sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }

                        string strRechargeEmailSubject = ResourceExtension.GetLiteral("ConfirmFinePayment_EmailHeader");
                        /*
                            ID: {0}<br>
                            *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                            *  Total Pagado: {2} 
                            *  Matrícula: 3
                            */

                        string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmTicketPaymentNonUser_EmailBody"),
                            model.TicketNumber,
                            dt,
                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.TotalQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                            infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                            model.Plate,
                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                            sLayoutSubtotal, sLayoutTotal,
                            GetEmailFooter(model.InstallationShortDesc, model.CountryCode));

                        if (model.Email != ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString())
                        {
                            SendEmail(model.Email, strRechargeEmailSubject, strRechargeEmailBody);
                        }
                        else 
                        {
                            Session["strRechargeEmailSubject"] = strRechargeEmailSubject;
                            Session["strRechargeEmailBody"] = strRechargeEmailBody;
                        }

                        if (CCSuccess(strReference,
                                       strTransactionId,
                                       null,
                                       strGatewayDate,
                                       strAuthCode,
                                       strAuthResult,
                                       strAuthResultDesc,
                                       strCardHash,
                                       strCardReference,
                                       strCardScheme,
                                       strMaskedCardNumber,
                                       PaymentMeanRechargeStatus.Waiting_Commit,
                                       dtExpDate))
                        {
                            return View(model);
                        }
                    }
                }
                else
                {
                    xmlNodeValue = GetXMLNodeValue(res, "ConfirmFinePayment");
                    /* Si ConfirmFinePayment = 1 quiere decir que falló después de hacerse el pago, es decir, 
                    *  durante la confirmación.
                    *  Si esto ocurre, y se trata de IECISA, hay que generar devolución.
                    */
                    if (xmlNodeValue == 1 && eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA)
                    {
                        string strUserReference = null;
                        string strRefundTransactionId = null;

                        RefundTransaction(model.InstallationId, eProviderType, strTransactionId, strCFTransactionId, model.CurrencyId, model.TotalQuantity, strAuthCode, model.AmountCurrencyIsoCode, out strUserReference, out strAuthCode, out strAuthResult, out strAuthResultDesc, out strGatewayDate, out strRefundTransactionId);
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Success Exception: {0}", e.Message));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Success: Exiting with CCFailure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            Session["Sess_strReference"] = null;
            Session["Sess_strTransactionId"] = null;
            Session["Sess_strGatewayDate"] = null;
            Session["Sess_strAuthCode"] = null;
            Session["Sess_strAuthResult"] = null;
            Session["Sess_strAuthResultDesc"] = null;
            Session["Sess_strCardHash"] = null;
            Session["Sess_strCardReference"] = null;
            Session["Sess_strCardScheme"] = "";
            Session["Sess_strMaskedCardNumber"] = null;
            Session["Sess_dtExpDate"] = null;

            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InStripePayment"] = null;
            return RedirectToAction("CC3Failure");
        }

        private bool CommitTransaction(decimal installationid,
                                        PaymentMeanCreditCardProviderType provider,
                                        string strTransactionId,
                                        string strCFTransactionId,
                                        decimal cur_id,
                                        DateTime dt,
                                        string strAuthCode,
                                        int TotalAmount,
                                        string AmountCurrencyIsoCode,
                                        out string strUserReference,
                                        out string strAuthResult,
                                        out string strGatewayDate,
                                        out string strCommitTransactionId,
                                        out int iTransactionFee,
                                        out string strTransactionFeeCurrencyIsocode,
                                        out string strTransactionURL,
                                        out string strRefundTransactionURL)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strCommitTransactionId = null;
            iTransactionFee = 0;
            strTransactionFeeCurrencyIsocode = "";
            strTransactionURL = "";
            strRefundTransactionURL = "";

            //CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG gwc = (CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG)Session["GatewayConfig"];

            try
            {
                SetCulture();

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallation(installationid, null, null, ref oInstallation, ref dtinstDateTime);

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG gwc = customersRepository.GetInstallationGatewayConfig(installationid);

                if (gwc != null &&
                           !(gwc.CPTGC_ENABLED != 0 && gwc.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    gwc = null;
                }

                if (gwc == null)
                {

                    gwc = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                               .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                           ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                           r.CURRENCy.CUR_ISO_CODE == AmountCurrencyIsoCode &&
                                                           r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard))
                                          .FirstOrDefault();
                }

                string result = "";
                string errorMessage = "";
                string errorCode = "";

                switch (provider)
                {
                    case PaymentMeanCreditCardProviderType.pmccpCreditCall:
                        CardEasePayments cardPayment_CC = new CardEasePayments();
                        bRes = cardPayment_CC.ConfirmUnCommitedPayment(gwc.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                    gwc.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                    gwc.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                    gwc.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                    strTransactionId,
                                                                    out strUserReference,
                                                                    out strAuthResult,
                                                                    out strGatewayDate,
                                                                    out strCommitTransactionId);
                        break;
                    case PaymentMeanCreditCardProviderType.pmccpStripe:

                        bRes = StripePayments.CaptureCharge(gwc.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                            strTransactionId,
                                                                out result,
                                                                out errorCode,
                                                                out errorMessage,
                                                                out strCommitTransactionId);

                        if (bRes)
                        {
                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::CommitTransaction: Exception {0}", e.Message));
                bRes = false;
            }
            return bRes;
        }

        private bool RefundTransaction(decimal installationid,
                                        PaymentMeanCreditCardProviderType provider,
                                        string strTransactionId,
                                        string strCFTransactionId,
                                        decimal cur_id,
                                        int TotalAmount,
                                        string iStrAuthCode,
                                        string AmountCurrencyIsoCode,
                                        out string strUserReference,
                                        out string strAuthCode,
                                        out string strAuthResult,
                                        out string strAuthResultDesc,
                                        out string strGatewayDate,
                                        out string strRefundTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthCode = null;
            strAuthResult = null;
            strAuthResultDesc = "";
            strGatewayDate = null;
            strRefundTransactionId = null;

            //CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG gwc = (CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG)Session["GatewayConfig"];



            try
            {
                SetCulture();


                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;

                geograficAndTariffsRepository.getInstallation(installationid, null, null, ref oInstallation, ref dtinstDateTime);

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG gwc = customersRepository.GetInstallationGatewayConfig(installationid);

                if (gwc != null &&
                           !(gwc.CPTGC_ENABLED != 0 && gwc.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    gwc = null;
                }

                if (gwc == null)
                {

                    gwc = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                               .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                           ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                           r.CURRENCy.CUR_ISO_CODE == AmountCurrencyIsoCode &&
                                                           r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard))
                                          .FirstOrDefault();
                }

                if (provider == PaymentMeanCreditCardProviderType.pmccpIECISA)
                {
                    DateTime? dtExpDate = null;
                    DateTime? dtTransactionDate = null;
                    string strExpMonth = "";
                    string strExpYear = "";
                    string strOpReference = "";
                    string strCFTransactionID = "";
                    string strMaskedCardNumber = "";
                    string strCardReference = "";
                    IECISAPayments cardPayment = new IECISAPayments();
                    IECISAPayments.IECISAErrorCode eErrorCode;
                    string errorMessage = "";

                    cardPayment.GetTransactionStatus(gwc.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                        gwc.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                        gwc.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                        strTransactionId,
                                                    out eErrorCode,
                                                    out errorMessage,
                                                    out strMaskedCardNumber,
                                                    out strCardReference,
                                                    out dtExpDate,
                                                    out strExpMonth,
                                                    out strExpYear,
                                                    out dtTransactionDate,
                                                    out strOpReference,
                                                    out strCFTransactionID,
                                                    out strAuthCode);

                    if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                    {
                        if (eErrorCode == IECISAPayments.IECISAErrorCode.TransactionNotCompleted)
                        {
                            strAuthResult = "cancelled";
                            bRes = true;
                        }
                    }
                    else
                    {
                        int iQuantityToRefund = TotalAmount;
                        DateTime dtNow = DateTime.Now;
                        DateTime dtUTCNow = DateTime.UtcNow;

                        string strErrorMessage = "";
                        string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)cur_id);

                        cardPayment.RefundTransaction(gwc.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                            gwc.IECISA_CONFIGURATION.IECCON_CF_USER,
                                            gwc.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                            gwc.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                            gwc.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                            gwc.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                            gwc.IECISA_CONFIGURATION.IECCON_REFUNDSERVICE_URL,
                                            gwc.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                            gwc.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                            gwc.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                            gwc.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                            strTransactionId,
                                            strCFTransactionId,
                                            dtTransactionDate.Value,
                                            iStrAuthCode,
                                            iQuantityToRefund,
                                            strCurISOCode,
                                            infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(strCurISOCode),
                                            dtNow,
                                            out eErrorCode,
                                            out errorMessage,
                                            out strRefundTransactionId);

                        if ((eErrorCode != IECISAPayments.IECISAErrorCode.OK) && (eErrorCode != IECISAPayments.IECISAErrorCode.OriginalAmountLessAmountReturned))
                        {
                            strAuthResult = eErrorCode.ToString();
                            strAuthResultDesc = errorMessage;

                            m_Log.LogMessage(LogLevels.logERROR, string.Format("RefundTransaction : errorCode={0} ; errorMessage={1}",
                                        strAuthResult, strErrorMessage));
                        }
                        else
                        {
                            strAuthResult = "succeeded";
                            bRes = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::Refund: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }



        //[Authorize] 
        public ActionResult CC3Failure()
        {
            SetCulture();

            string strCardReference = "";
            string strMaskedCardNumber = "";

            try
            {
                strCardReference = Session["Sess_strCardReference"].ToString();
                strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
            }
            catch { }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Failure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            try
            {
                // We don't have a logged user when paying fines through the website
                ViewData["oUser"] = null; // oUser; 
                ViewData["UserNameAndSurname"] = ""; // oUser.CUSTOMER.CUS_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
                ViewData["UserBalance"] = 0; // Convert.ToDouble(oUser.USR_BALANCE);
                ViewData["CurrencyISOCode"] = Session["CurrencyToRecharge"]; // oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE;
                ViewData["PayerQuantity"] = Session["QuantityToRecharge"]; // Session["QuantityToRecharge"];
                ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"]; // Session["CurrencyToRecharge"];
                ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(Session["CurrencyToRecharge"].ToString()) + "}",
                    Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));
                ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];

                Session["Sess_strCardReference"] = null;
                Session["Sess_strMaskedCardNumber"] = null;

                Session["QuantityToRecharge"] = null;
                Session["QuantityToRechargeBase"] = null;
                Session["PercVAT1"] = null;
                Session["PercVAT2"] = null;
                Session["PercFEE"] = null;
                Session["PercFEETopped"] = null;
                Session["FixedFEE"] = null;
                Session["CurrencyToRecharge"] = null;
                Session["InStripePayment"] = null;
                Session["OVERWRITE_CARD"] = false;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CC3Failure Exception: {0}", e.Message));
            }

            return View();
        }

        //[Authorize] 
        public ActionResult CC3Cancel()
        {
            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CC3Failure");

            /*
            // We don't have a logged user when paying fines through the website
            USER oUser = GetUserFromSession();

            if (oUser == null)
            {
                return LogOff();
            }
            */

            // We don't have a logged user when paying fines through the website
            ViewData["oUser"] = null; // oUser;
            ViewData["UserNameAndSurname"] = "John Doe"; // oUser.CUSTOMER.CUS_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;
            ViewData["UserBalance"] = 0; // Convert.ToDouble(oUser.USR_BALANCE);
            ViewData["CurrencyISOCode"] = Session["CurrencyToRecharge"]; // oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE;
            ViewData["PayerQuantity"] = Session["QuantityToRecharge"];
            ViewData["PayerCurrencyISOCode"] = Session["CurrencyToRecharge"];
            ViewData["DiscountValue"] = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(Session["CurrencyToRecharge"].ToString()) + "}",
                Convert.ToDouble(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(Session["CurrencyToRecharge"].ToString()));
            ViewData["DiscountCurrency"] = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];

            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InStripePayment"] = null;
            Session["OVERWRITE_CARD"] = false;

            return View();
        }

        #endregion

        #region Paypal Payment

        public ActionResult PaypalRequest(string strURLSufix)
        {
            SetCulture();

            Session["PayPalModel"] = null;
            FineModel model = (FineModel)Session["FineModel"];

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
            INSTALLATION oInstallation = null;
            DateTime? dtinstDateTime = null;
            geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);
            
            //oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);
            if (oInstallation.INS_PAYPAL_CPTGC_ID.HasValue)
            {
                oGatewayConfig = customersRepository.GetGatewayConfig(oInstallation.INS_PAYPAL_CPTGC_ID);
                if(oGatewayConfig!=null)
                {
                    m_Log.LogMessage(LogLevels.logINFO, "PaypalRequest::oGatewayConfig.CPTGC_ID= " + oGatewayConfig.CPTGC_ID);
                    m_Log.LogMessage(LogLevels.logINFO, "PaypalRequest::oGatewayConfig.CPTGC_PP_RESTAPI_CLIENT_ID= " + oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID);
                    m_Log.LogMessage(LogLevels.logINFO, "PaypalRequest::oGatewayConfig.CPTGC_PP_RESTAPI_CLIENT_SECRET= " + oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET);
                    m_Log.LogMessage(LogLevels.logINFO, "PaypalRequest::oGatewayConfig.CPTGC_PP_RESTAPI_URL_PREFIX= " + oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX);;
                }
            }

            //if (oGatewayConfig != null &&
            //            !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            //{
            //    oGatewayConfig = null;
            //}

            //if (oGatewayConfig == null)
            //{


            //    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
            //                                .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
            //                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
            //                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpPayu))
            //                                .FirstOrDefault();
            //}

            //TO DO: Mariu incluye 100 porque no existe tabla para PayPal
            Session["PayPal_Config_id"] = oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_ID;
            string email = model.Email;
            int amount = model.TotalQuantity;
            string isocode = model.AmountCurrencyIsoCode;
            string plate = (!string.IsNullOrEmpty(model.Plate) ? (" - " + model.Plate) : string.Empty);
            string description = model.TicketNumber.ToString() + plate;
            string utcdate = DateTime.UtcNow.ToString("HHmmssddMMyy");

            string culture = String.Empty; ;

            if (Session["Culture"] != null)
            {
                CultureInfo ci = (CultureInfo)Session["Culture"];
                if (ci.Name.Equals(TEXT_ENGLISH_LANGUAGE))
                {
                    culture = TEXT_BD_ENGLISH_LANGUAGE;
                }
                else
                {
                    culture = ci.Name;
                }
            }
            else
            {
                culture = TEXT_BD_ENGLISH_LANGUAGE;
            }

            string hash = string.Empty;
            string requrl = "";
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
            {
                requrl = Request.Url.ToString();
            }
            else
            {
                requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/Fine/";
            }
            string returnURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + TEXT_PAYPAL_RETURN + strURLSufix;
            string cancelURL = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + TEXT_PAYPAL_CANCEL + strURLSufix;

            PayPalController oPayPal = new PayPalController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["PayPal_Config_id"]), Response);
            return oPayPal.PayPalRequest(string.Empty, email, amount, isocode, description, utcdate, culture, returnURL, cancelURL,
                                         model.QFEE, model.QVAT, model.Total, "", hash);
        }

        [HttpGet]
        public ActionResult PaypalReturn(string strURLSufix)
        {
            SetCulture();


            if (!Convert.ToBoolean(Session[TEXT_IN_PAYPAL_PAYMENT]))
                return RedirectToAction(TEXT_PAYPAL_FAILURE + strURLSufix);

            string token = Request.QueryString[TEXT_TOKEN];
            string paymentId = Request.Params[TEXT_PAYMENT_ID];
            string PayerId = Request.Params[TEXT_PAYER_ID];

            if (token != null)
            {
                Session[TEXT_PAYPAL_TOKEN_2] = token;
                Session[TEXT_PAYMENT_ID] = paymentId;
                Session[TEXT_PAYER_ID] = PayerId;
                m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalReturn RedirectResul={0}", TEXT_PAYPAL_RESULT + strURLSufix));
                return RedirectToAction(TEXT_PAYPAL_RESULT + strURLSufix);
            }
            else
            {
                m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalReturn RedirectFailure={0}", TEXT_PAYPAL_FAILURE + strURLSufix));
                return RedirectToAction(TEXT_PAYPAL_FAILURE + strURLSufix);
            }
        }

        [HttpGet]
        public ActionResult PaypalFailure(string strURLSufix)
        {
            SetCulture();

            DeleteSessionPayPal();

            FineModel model = (FineModel)Session["FineModel"];
            if (model != null)
            {
                ViewData[TEXT_CURRENCY_ISO_CODE] = model.AmountCurrencyIsoCode;
            }
            return RedirectToAction(TEXT_CC_FAILURE);
        }

        public ActionResult PayPalResult(string submitButton)
        {
            SetCulture();

            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    return RedirecToactionBySession();
                }
                else if (Session["PayPalResult"] != null)
                {
                    Session["PayPalResult"] = null;
                    return RedirectToAction("Fine");
                }
                else if(Session["ReturnFine"]==null && Session["PayPalResult"]==null)
                {
                    return RedirectToAction("Fine");
                }
            }
            return View();
        }

        public ActionResult PayPalResultPT(string strURLSufix, string submitButton)
        {
            SetCulture();

            return RedirectToAction(TEXT_PAYPAL_RESULT + strURLSufix);
        }
        
        [HttpGet]
        public ActionResult PayPalResult(string strURLSufix, string submitButton)
        {
            SetCulture();


            if (!Convert.ToBoolean(Session[TEXT_IN_PAYPAL_PAYMENT]))
                return RedirectToAction(TEXT_PAYPAL_FAILURE + strURLSufix);

            if (Session[TEXT_PAYPAL_TOKEN_2] == null)
                return RedirectToAction(TEXT_PAYPAL_FAILURE + strURLSufix);

            FineModel model = (FineModel)Session["FineModel"];

            try
            {
                if (model != null)
                {
                    ViewData[TEXT_CURRENCY_ISO_CODE] = model.AmountCurrencyIsoCode;
                }

                string strTime = string.Empty;
                string strIntent = string.Empty;
                string strState = string.Empty;
                string apiClientId = string.Empty;
                string apiClientSecret = string.Empty;
                string apiClientUrl = string.Empty;



                DateTime? dt = null;
                INSTALLATION oInstallation = null;
                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dt);
                
                if (oInstallation != null && oInstallation.INS_PAYPAL_CPTGC_ID.HasValue)
                {
                    oGatewayConfig = customersRepository.GetGatewayConfig(oInstallation.INS_PAYPAL_CPTGC_ID.Value);
                    if (oGatewayConfig != null && oGatewayConfig.CPTGC_ENABLED == 1 && oGatewayConfig.CPTGC_IS_INTERNAL == 0)
                    {
                        m_Log.LogMessage(LogLevels.logINFO, "PayPalResult::oGatewayConfig.CPTGC_ID= " + oGatewayConfig.CPTGC_ID);
                        m_Log.LogMessage(LogLevels.logINFO, "PayPalResult::oGatewayConfig.CPTGC_PP_RESTAPI_CLIENT_ID= " + oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID);
                        m_Log.LogMessage(LogLevels.logINFO, "PayPalResult::oGatewayConfig.CPTGC_PP_RESTAPI_CLIENT_SECRET= " + oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET);
                        m_Log.LogMessage(LogLevels.logINFO, "PayPalResult::oGatewayConfig.CPTGC_PP_RESTAPI_URL_PREFIX= " + oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX);
                        m_Log.LogMessage(LogLevels.logINFO, "PayPalResult::oGatewayConfig.CPTGC_PP_RESTAPI_ENVIRONMENT= " + oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_ENVIRONMENT);
                    }
                }
                else
                {
                    Session["PaymentPayPalButton"] = null;
                }
                

                string strOpReference = null;
                DateTime dtNow = DateTime.Now;
                DateTime dtUTCNow = DateTime.UtcNow;
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                int iDivisor = infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode);
                string sIsoCode = infraestructureRepository.GetCurSymbolFromIsoCode(model.AmountCurrencyIsoCode);
                string AuthorizationCode = Session[TEXT_PAYMENT_ID].ToString();

                decimal dPercVAT1 = ConvertSessionToDecimal(TEXT_PERC_VAT1, provider);
                decimal dPercVAT2 = ConvertSessionToDecimal(TEXT_PERC_VAT2, provider);
                decimal dPercFEE = ConvertSessionToDecimal(TEXT_PERC_FEE, provider);
                int iPercFEETopped = ConvertSessionToInt(TEXT_PERC_FEE_TOPPED);
                int iFixedFEE = ConvertSessionToInt(TEXT_FIXED_FEE);
                int iQuantity = Session["QuantityToRechargeBase"] != null ? Convert.ToInt32(Convert.ToDouble(Session["QuantityToRechargeBase"].ToString(), provider) * iDivisor) : 0;

                int iPartialVAT1;
                int iPartialPercFEE;
                int iPartialFixedFEE;

                int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);
              
                string strAuthCode = string.Empty;
                string modePaypal = Enum.GetName(typeof(enumPayPalMode), (object)oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_ENVIRONMENT.Value);
                if (PaypalPayments.ExpressCheckoutPassTwo(Session[TEXT_PAYPAL_TOKEN_2].ToString(), 
                                                          Session[TEXT_PAYMENT_ID].ToString(), 
                                                          Session[TEXT_PAYER_ID].ToString(),
                                                          oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID, 
                                                          oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET, 
                                                          oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX, 
                                                          modePaypal,
                                                          oGatewayConfig.PAYPAL_CONFIGURATION.PPCON_SERVICE_TIMEOUT,
                                                          out strAuthCode, out strTime, out strIntent, out strState))
                {


                    m_Log.LogMessage(LogLevels.logINFO, "PaypalRequest::oGatewayConfig.CPTGC_ID= " + oGatewayConfig.CPTGC_ID);

                    customersRepository.StartRecharge(oGatewayConfig.CPTGC_ID,
                                                      model.Email,
                                                      dtUTCNow,
                                                      dtNow,
                                                      model.TotalQuantity,
                                                      infraestructureRepository.GetCurrencyFromIsoCode(model.AmountCurrencyIsoCode),
                                                      "",
                                                      strOpReference,
                                                      Session[TEXT_PAYMENT_ID].ToString(),
                                                      "",
                                                      strAuthCode,
                                                      "",
                                                      PaymentMeanRechargeStatus.Waiting_Commit);


                    DateTime TimeStamp = Convert.ToDateTime(strTime);

                    ViewData["TransactionID"] = Session[TEXT_PAYMENT_ID];
                    ViewData["TransactionDate"] = TimeStamp.ToString("HHmmssddMMyyyy");

                    ChargeOperationsType chargeType = (ChargeOperationsType)Convert.ToInt32(Session["OperationChargeType"]);

                    PaymentMeanRechargeCreationType rechargeCreationType = PaymentMeanRechargeCreationType.pmrctRegularRecharge;

                    if (Session[TEXT_RECHARGE_CREATIONT_YPE] != null)
                    {
                        rechargeCreationType = (PaymentMeanRechargeCreationType)Session[TEXT_RECHARGE_CREATIONT_YPE];
                    }

                    string strGatewayDate = string.Empty;
                    DateTime.TryParseExact(strGatewayDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtUTCNow);

                    int iWSTimeout = infraestructureRepository.GetRateWSTimeout(model.InstallationId);

                    string res = external.ConfirmFinePaymentNonUser(model.TicketNumber, model.Quantity,
                                model.TotalQuantity, model.Plate, model.PercFEE, model.PercVAT1,
                                model.PercVAT2, model.PercFEETopped, model.FixedFEE, model.PartialVAT1,
                                model.PartialPercFEE, model.PartialFixedFEE, model.PartialPercFEEVAT,
                                model.PartialFixedFEEVAT, model.TaxMode, model.Email, null, AuthorizationCode,
                                null, strGatewayDate, strAuthCode, null, null, null, null, null,
                                Session[TEXT_PAYER_ID].ToString(), Session[TEXT_PAYPAL_TOKEN_2].ToString(),
                                null, PaymentMeanCreditCardProviderType.pmccpPaypal, model.InstallationId, geograficAndTariffsRepository,
                                model.CurrencyId, model.AmountCurrencyIsoCode, Convert.ToDecimal(oInstallation.INS_PAYPAL_CPTGC_ID), fineRepository, model.GrpId, model.AuthId, iWSTimeout);
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Se guarda en base de datos: "));
                    int xmlNodeValue = GetXMLNodeValue(res, "r");

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("resul: {0} ", xmlNodeValue.ToString()));
                    if ((ExternalWS.ResultType)xmlNodeValue == ExternalWS.ResultType.Result_OK)
                    {
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("Entra external resul "));
                        if (String.IsNullOrEmpty(model.AmountCurrencyIsoCode))
                        {
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("Error:: model.AmountCurrencyIsoCode"));
                        }
                        else
                        {
                            ViewData[TEXT_CURRENCY_ISO_CODE] = model.AmountCurrencyIsoCode;
                            ViewData["PayerCurrencyISOCode"] = model.AmountCurrencyIsoCode;
                        }

                        if (model == null)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("Error:: PayPalResult: Model is null "));
                        }

                        int iQFEE = model.QFEE;
                        int iQVAT = model.QVAT;
                        int iLayout = 0;
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            OPERATOR oOperator = customersRepository.GetDefaultOperator();
                            if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                        }


                        string sLayoutSubtotal = "";
                        string sLayoutTotal = "";

                        string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(model.CurrencyId));

                        if (iLayout == 2)
                        {
                            sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }
                        else if (iLayout == 1)
                        {
                            sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }

                        string strRechargeEmailSubject = ResourceExtension.GetLiteral("ConfirmFinePayment_EmailHeader");

                        string emailDestination = string.Empty;
                        try
                        {
                            if (model != null && !string.IsNullOrEmpty(model.Email) && model.Email != ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString())
                            {                                
                                emailDestination = model.Email;
                                m_Log.LogMessage(LogLevels.logINFO, string.Format("###################### emailDestination - {0}", emailDestination));
                            }
                            
                            string strRechargeEmailBody;
                            string baseString = null;
                            string param0 = null;
                            DateTime? param1 = null;
                            string param2 = null;
                            string param3 = null;
                            string param6 = null;
                            string param7 = null;
                            string param8 = null;

                            try
                            {
                                baseString = ResourceExtension.GetLiteral("ConfirmTicketPaymentNonUser_EmailBody");
                                param0 = model.TicketNumber;
                                param1 = dt;
                                param2 = string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}",
                                    Convert.ToDouble(model.TotalQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                    infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode));
                                param3 = model.Plate;
                                param6 = sLayoutSubtotal;
                                param7 = sLayoutTotal;
                                param8 = GetEmailFooter(model.InstallationShortDesc, model.CountryCode);
                            }
                            catch (Exception ex)
                            {
                                m_Log.LogMessage(LogLevels.logINFO, string.Format("###################### exception: {0} BASE: {1}", ex.Message, baseString));
                            }
                            
                            try
                            {
                                strRechargeEmailBody = string.Format(baseString,
                                    param0,
                                    param1,
                                    param2,
                                    param3,
                                    ConfigurationManager.AppSettings["EmailSignatureURL"],
                                    ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                    param6,
                                    param7,
                                    param8);

                                m_Log.LogMessage(LogLevels.logINFO, "###################### cargado strRechargeEmailBody");
                            }
                            catch (Exception ex)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### exception: {0}", ex.Message));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### base: {0}", baseString));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### param0: {0}", param0));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### param1: {0}", param1));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### param2: {0}", param2));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### param3: {0}", param3));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### param6: {0}", param6));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### param7: {0}", param7));
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("###################### param8: {0}", param8));

                                strRechargeEmailBody = null;
                            }
                            if (!string.IsNullOrEmpty(emailDestination))
                            {
                                m_Log.LogMessage(LogLevels.logINFO, "###################### send email");
                                SendEmail(emailDestination, strRechargeEmailSubject, strRechargeEmailBody);
                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logINFO, string.Format("###################### load sessions for view - {0} - {0}", string.IsNullOrEmpty(strRechargeEmailSubject), string.IsNullOrEmpty(strRechargeEmailBody)));
                                Session["strRechargeEmailSubject"] = strRechargeEmailSubject;
                                Session["strRechargeEmailBody"] = strRechargeEmailBody;
                                Session["validEmail"] = 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = string.Format("Error:: Send Paypal Mail to {0} tansaction Id {1} - Exception: {2}",
                                emailDestination != string.Empty ? emailDestination : "UNKNOWN",
                                Session[TEXT_PAYER_ID].ToString(),
                                ex.Message);

                            m_Log.LogMessage(LogLevels.logERROR, string.Format("Error:: Send Mail - {0}", error ));
                        }
                        DeleteSessionPayPal();
                        return View(model);
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("Error en el Result: ", ex.Message));
                string mensaje = ex.Message;
            }
            return RedirectToAction(TEXT_PAYPAL_FAILURE + strURLSufix);
        }

        [HttpGet]
        public ActionResult PaypalCancel(string strURLSufix)
        {
            SetCulture();

            if (!Convert.ToBoolean(Session[TEXT_IN_PAYPAL_PAYMENT]))
                return RedirectToAction(TEXT_PAYPAL_FAILURE + strURLSufix);

            FineModel model = (FineModel)Session["FineModel"];

            ViewData[TEXT_CURRENCY_ISO_CODE] = model.AmountCurrencyIsoCode;

            return RedirectToAction(TEXT_CC_FAILURE);
        }

        #endregion

        #region Payu Payment

        [HttpGet]
        public ActionResult PayuRequest()
        {
            SetCulture();

            FineModel model = (FineModel)Session["FineModel"];

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
            INSTALLATION oInstallation = null;
            DateTime? dtinstDateTime = null;
            geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);


            oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

            if (oGatewayConfig != null &&
                        !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {


                oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpPayu))
                                            .FirstOrDefault();
            }

            Session["Payu_Config_id"] = oGatewayConfig.CPTGC_PAYUCON_ID;
            string email = model.Email;
            int amount = model.TotalQuantity;
            string isocode = model.AmountCurrencyIsoCode;
            string description = model.TicketNumber.ToString();
            string utcdate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string culture = model.CountryCode;
            string ReturnURL = "";

            string hash = string.Empty;
            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            return payu.PayuRequest(string.Empty, email, amount, isocode, description, utcdate, culture, ReturnURL, hash);
        }

        [HttpPost]
        public ActionResult PayuResponse()
        {
            SetCulture();

            FineModel model = (FineModel)Session["FineModel"];
            Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
            Session["Plate"] = model.Plate;
            Session["Total"] = model.Total;
            Session["QFEE"] = model.QFEE;
            Session["QVAT"] = model.QVAT;
            Session["TotalQuantity"] = model.TotalQuantity;
            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            return payu.PayuResponse();
        }

        public ActionResult PayuResult(string submitButton)
        {
            SetCulture();

            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    return RedirecToactionBySession();
                }
                else if (Session["PayuResult"] != null)
                {
                    Session["PayuResult"] = null;
                    return RedirectToAction("Fine");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult PayuResult(string r, string submitButton)
        {
            SetCulture();

            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            string r_decrypted = payu.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);
            
            if (j.result == "succeeded")
            {
                if (Convert.ToBoolean(Session["InPayuPayment"]))
                {
                    Session["InPayuPayment"] = null;
                    ResultType res = ResultType.Result_OK;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("PayuResult::-->ConfirmPayment: "));
                    res = ConfirmPayment(j);
                    Session["PayuResult"] = j.result;
                    if (res != ResultType.Result_OK)
                    {
                        return RedirectToAction("CCFailure");
                    }
                }
            }

            return payu.PayuResult(r);
        }

        #endregion

        #region TransBank Payment

        [HttpGet]
        public ActionResult TransBankRequest()
        {
            SetCulture();

            FineModel model = (FineModel)Session["FineModel"];
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
            INSTALLATION oInstallation = null;
            DateTime? dtinstDateTime = null;
            geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);

            oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

            if (oGatewayConfig != null &&
                        !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {


                oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                        .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                    ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                    r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpTransbank))
                                        .FirstOrDefault();
            }


            Session["Transbank_Config_id"] = oGatewayConfig.CPTGC_TRBACON_ID;
            string email = model.Email;
            int amount = model.TotalQuantity;
            string isocode = model.AmountCurrencyIsoCode;
            string description = model.TicketNumber.ToString();
            string utcdate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string culture = model.CountryCode;
            string ReturnURL = "";

            string hash = string.Empty;
            TransbankController transbank = new TransbankController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Transbank_Config_id"]));
            return transbank.TransbankRequest(string.Empty, email, amount, isocode, description, utcdate, ReturnURL, hash);
        }

        [HttpPost]
        public ActionResult TransbankResponse()
        {
            SetCulture();

            FineModel model = (FineModel)Session["FineModel"];
            Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
            Session["Plate"] = model.Plate;
            Session["Total"] = model.Total;
            Session["QFEE"] = model.QFEE;
            Session["QVAT"] = model.QVAT;
            Session["TotalQuantity"] = model.TotalQuantity;
            TransbankController transbank = new TransbankController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Transbank_Config_id"]));
            return transbank.TransbankResponse();
        }

        public ActionResult TransBankResult(string submitButton)
        {
            SetCulture();

            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    return RedirecToactionBySession();
                }
                else if (Session["TransBankResult"] != null)
                {
                    Session["TransBankResult"] = null;
                    return RedirectToAction("Fine");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult TransBankResult(string r, string submitButton)
        {
            SetCulture();

            TransbankController transbank = new TransbankController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Transbank_Config_id"]));
            string r_decrypted = transbank.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                if (Convert.ToBoolean(Session["InTransBankPayment"]))
                {
                    Session["InTransBankPayment"] = null;
                    ResultType res = ResultType.Result_OK;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("TransBankResult::-->ConfirmPayment: "));
                    res = ConfirmPayment(j);
                    Session["TransBankResult"] = j.result;
                    if (res != ResultType.Result_OK)
                    {
                        return RedirectToAction("CCFailure");
                    }
                }
            }

            return transbank.TransbankResult(r);
        }

        #endregion

        #region Moneris Payment

        [HttpGet]
        public ActionResult MonerisRequest()
        {
            SetCulture();

            FineModel model = (FineModel)Session["FineModel"];

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
            INSTALLATION oInstallation = null;
            DateTime? dtinstDateTime = null;
            geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);


            oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

            if (oGatewayConfig != null &&
                        !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {

                oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                     .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                 ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                 r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                 r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpMoneris))
                                     .FirstOrDefault();
            }

            Session["Moneris_Config_id"] = oGatewayConfig.CPTGC_MONCON_ID;
            string Email = model.Email;
            int Amount = model.TotalQuantity;
            string CurrencyISOCODE = model.AmountCurrencyIsoCode;
            string Description = model.TicketNumber.ToString();
            string UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string Hash = string.Empty;
            string ReturnURL = "";
            string culture = ((CultureInfo)Session["Culture"]).Name;


            Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
            Session["Plate"] = model.Plate;
            Session["Total"] = model.Total;
            Session["QFEE"] = model.QFEE;
            Session["QVAT"] = model.QVAT;
            Session["TotalQuantity"] = model.TotalQuantity;

            Session["PAYMENT_ORIGIN"] = "FineController";

            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));
            return moneris.MonerisRequest(string.Empty, Email, Amount, CurrencyISOCODE, Description, UTCDate, culture, ReturnURL, "", Hash);
        }

        [HttpPost]
        public ActionResult MonerisFailure()
        {
            SetCulture();

            return MonerisFailure(Request["response_order_id"],
                                Request["response_code"],
                                Request["date_stamp"],
                                Request["time_stamp"],
                                Request["message"]);
        }

        [HttpGet]
        public ActionResult MonerisFailure(string response_order_id,
                                           string response_code,
                                           string date_stamp,
                                           string time_stamp,
                                           string message)
        {
            SetCulture();

            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));
            return moneris.MonerisFailure(response_order_id, response_code, date_stamp, time_stamp, message);
        }

        public ActionResult MonerisResult(string submitButton)
        {
            SetCulture();

            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {
                    return RedirecToactionBySession();
                }
                else if (Session["MonerisResult"] != null)
                {
                    Session["MonerisResult"] = null;
                    return RedirectToAction("Fine");
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult MonerisResult(string r, string submitButton)
        {
            SetCulture();

            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));
            string r_decrypted = moneris.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                if (Convert.ToBoolean(Session["InMonerisPayment"]))
                {
                    Session["InMonerisPayment"] = null;
                    ResultType res = ResultType.Result_OK;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("MonerisResult::-->ConfirmPayment: "));
                    res = ConfirmPayment(j);
                    Session["MonerisResult"] = j.result;
                    if (res != ResultType.Result_OK)
                    {
                        return RedirectToAction("CCFailure");
                    }
                }
            }

            Session["result"] = null;
            Session["errorCode"] = null;
            Session["errorMessage"] = null;
            Session["cardReference"] = null;
            Session["cardHash"] = null;
            Session["cardScheme"] = null;
            Session["cardPAN"] = null;
            Session["cardExpMonth"] = null;
            Session["cardExpYear"] = null;
            Session["chargeDateTime"] = null;
            Session["reference"] = null;
            Session["transactionID"] = null;
            Session["authCode"] = null;
            Session["authResult"] = null;
            Session["email"] = null;
            Session["amount"] = null;
            Session["currency"] = null;
            Session["utcdate"] = null;
            Session["MonerisGuid"] = null;

            ViewData["result"] = r_decrypted;

            return View();
        }

        [HttpPost]
        public ActionResult MonerisSuccess()
        {
            SetCulture();

            return MonerisSuccess(Request["response_order_id"],
                                Request["response_code"],
                                Request["date_stamp"],
                                Request["time_stamp"],
                                Request["eci"],
                                Request["txn_num"],
                                Request["bank_approval_code"],
                                Request["result"],
                                Request["trans_name"],
                                Request["gcardholder"],
                                Request["charge_total"],
                                Request["card"],
                                Request["f4l4"],
                                Request["message"],
                                Request["iso_code"],
                                Request["bank_transaction_id"],
                                Request["expiry_date"],
                                Request["cvd_response_code"],
                                Request["email"],
                                Request["cust_id"],
                                Request["note"]);
        }

        [HttpGet]
        public ActionResult MonerisSuccess(string response_order_id,
                                           string response_code,
                                           string date_stamp,
                                           string time_stamp,
                                           string eci,
                                           string txn_num,
                                           string bank_approval_code,
                                           string result,
                                           string trans_name,
                                           string gcardholder,
                                           string charge_total,
                                           string card,
                                           string f4l4,
                                           string message,
                                           string iso_code,
                                           string bank_transaction_id,
                                           string expiry_date,
                                           string cvd_response_code,
                                           string email,
                                           string cust_id,
                                           string note)
        {

            SetCulture();

            MonerisController moneris = new MonerisController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Moneris_Config_id"]));

            if (Convert.ToBoolean(Session["InMonerisPayment"]))
            {
                FineModel model = (FineModel)Session["FineModel"];
                Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
                Session["Plate"] = model.Plate;
                Session["Total"] = model.Total;
                Session["QFEE"] = model.QFEE;
                Session["QVAT"] = model.QVAT;
                Session["TotalQuantity"] = model.TotalQuantity;

                return moneris.MonerisSuccess(response_order_id, response_code, date_stamp, time_stamp, eci, txn_num, bank_approval_code, result, trans_name, gcardholder, charge_total, card, f4l4, message, iso_code, bank_transaction_id, expiry_date, cvd_response_code, email, cust_id, note);
            }
            else
            {
                return moneris.MonerisFailure(response_order_id, response_code, date_stamp, time_stamp, "Error");
            }
        }

        #endregion

        #region Paysafe Payment

        [HttpGet]
        public ActionResult PaysafeRequest()
        {
            SetCulture();

            FineModel model = (FineModel)Session["FineModel"];

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;
            INSTALLATION oInstallation = null;
            DateTime? dtinstDateTime = null;
            geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);



            oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

            if (oGatewayConfig != null &&
                        !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
            {
                oGatewayConfig = null;
            }

            if (oGatewayConfig == null)
            {

                oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                     .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                 r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                 r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpPaysafe))
                                     .FirstOrDefault();
            }

            Session["Paysafe_Config_id"] = oGatewayConfig.CPTGC_PYSCON_ID;
            string Email = model.Email;
            int Amount = model.TotalQuantity;
            string CurrencyISOCODE = model.AmountCurrencyIsoCode;
            string Description = model.TicketNumber.ToString();
            string UTCDate = DateTime.UtcNow.ToString("HHmmssddMMyy");
            string Hash = string.Empty;
            string ReturnURL = "";
            string culture = ((CultureInfo)Session["Culture"]).Name;


            Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
            Session["Plate"] = model.Plate;
            Session["Total"] = model.Total;
            Session["QFEE"] = model.QFEE;
            Session["QVAT"] = model.QVAT;
            Session["TotalQuantity"] = model.TotalQuantity;

            Session["PAYMENT_ORIGIN"] = "FineController";

            PaysafeController paysafe = new PaysafeController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Paysafe_Config_id"]));
            return paysafe.PaysafeRequest(string.Empty, Email, Amount, CurrencyISOCODE, Description, UTCDate, culture, ReturnURL, Hash);
        }

        [HttpPost]
        public ActionResult PaysafeFailure()
        {
            SetCulture();

            return PaysafeFailure(Request["response_order_id"],
                                Request["response_code"],
                                Request["date_stamp"],
                                Request["time_stamp"],
                                Request["message"]);
        }

        [HttpGet]
        public ActionResult PaysafeFailure(string response_order_id,
                                           string response_code,
                                           string date_stamp,
                                           string time_stamp,
                                           string message)
        {
            SetCulture();

            PaysafeController paysafe = new PaysafeController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Paysafe_Config_id"]));
            return paysafe.PaysafeFailure();
            //return paysafe.PaysafeFailure(response_order_id, response_code, date_stamp, time_stamp, message);
        }

        public ActionResult PaysafeResult(string submitButton)
        {
            SetCulture();

            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {

                    return RedirecToactionBySession();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult PaysafeResult(string r, string submitButton)
        {
            SetCulture();

            PaysafeController paysafe = new PaysafeController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Paysafe_Config_id"]));
            string r_decrypted = paysafe.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);

            if (j.result == "succeeded")
            {
                if (Convert.ToBoolean(Session["InPaysafePayment"]))
                {
                    Session["InPaysafePayment"] = null;
                    ResultType res = ResultType.Result_OK;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("PaysafeResult::-->ConfirmPayment: "));
                    res = ConfirmPayment(j);
                    if (res != ResultType.Result_OK)
                    {
                        return RedirectToAction("CCFailure");
                    }
                }
            }

            Session["result"] = null;
            Session["errorCode"] = null;
            Session["errorMessage"] = null;
            Session["cardReference"] = null;
            Session["cardHash"] = null;
            Session["cardScheme"] = null;
            Session["cardPAN"] = null;
            Session["cardExpMonth"] = null;
            Session["cardExpYear"] = null;
            Session["chargeDateTime"] = null;
            Session["reference"] = null;
            Session["transactionID"] = null;
            Session["authCode"] = null;
            Session["authResult"] = null;
            Session["email"] = null;
            Session["amount"] = null;
            Session["currency"] = null;
            Session["utcdate"] = null;
            Session["MonerisGuid"] = null;


            ViewData["result"] = r_decrypted;
            if (!string.IsNullOrEmpty(submitButton))
            {
                if (Session["ReturnFine"] != null)
                {

                    return RedirecToactionBySession();
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult PaysafeSuccess()
        {
            SetCulture();

            return PaysafeSuccess();
            //return PaysafeSuccess(Request["response_order_id"],
            //                    Request["response_code"],
            //                    Request["date_stamp"],
            //                    Request["time_stamp"],
            //                    Request["eci"],
            //                    Request["txn_num"],
            //                    Request["bank_approval_code"],
            //                    Request["result"],
            //                    Request["trans_name"],
            //                    Request["gcardholder"],
            //                    Request["charge_total"],
            //                    Request["card"],
            //                    Request["f4l4"],
            //                    Request["message"],
            //                    Request["iso_code"],
            //                    Request["bank_transaction_id"],
            //                    Request["expiry_date"],
            //                    Request["cvd_response_code"],
            //                    Request["email"],
            //                    Request["cust_id"],
            //                    Request["note"]);
        }

        [HttpGet]
        public ActionResult PaysafeSuccess(string response_order_id,
                                           string response_code,
                                           string date_stamp,
                                           string time_stamp,
                                           string eci,
                                           string txn_num,
                                           string bank_approval_code,
                                           string result,
                                           string trans_name,
                                           string gcardholder,
                                           string charge_total,
                                           string card,
                                           string f4l4,
                                           string message,
                                           string iso_code,
                                           string bank_transaction_id,
                                           string expiry_date,
                                           string cvd_response_code,
                                           string email,
                                           string cust_id,
                                           string note)
        {
            SetCulture();

            PaysafeController paysafe = new PaysafeController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Paysafe_Config_id"]));

            if (Convert.ToBoolean(Session["InPaysafePayment"]))
            {
                FineModel model = (FineModel)Session["FineModel"];
                Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
                Session["Plate"] = model.Plate;
                Session["Total"] = model.Total;
                Session["QFEE"] = model.QFEE;
                Session["QVAT"] = model.QVAT;
                Session["TotalQuantity"] = model.TotalQuantity;

                return paysafe.PaysafeSuccess();
                //return paysafe.PaysafeSuccess(response_order_id, response_code, date_stamp, time_stamp, eci, txn_num, bank_approval_code, result, trans_name, gcardholder, charge_total, card, f4l4, message, iso_code, bank_transaction_id, expiry_date, cvd_response_code, email, cust_id, note);
            }
            else
            {
                return paysafe.PaysafeFailure();
                //return paysafe.PaysafeFailure(response_order_id, response_code, date_stamp, time_stamp, "Error");
            }
        }

        #endregion

        #region Common payment helpers

        private ExternalWS.ResultType ConfirmPayment(dynamic j)
        {
            SetCulture();

            string strReference = String.Empty;
            string strTransactionId = String.Empty;
            string strGatewayDate = String.Empty;
            string strAuthCode = String.Empty;
            string strAuthResult = String.Empty;
            string strAuthResultDesc = String.Empty;
            string strCardHash = String.Empty;
            string strCardReference = String.Empty;
            string strCardScheme = String.Empty;
            string strMaskedCardNumber = String.Empty;
            DateTime? dtExpDate = null;
            string strCFTransactionId = String.Empty;
            string ErrorInfo = "";

            ExternalWS.ResultType result = ExternalWS.ResultType.Result_OK;

            try
            {

                FineModel model = (FineModel)Session["FineModel"];

                PaymentMeanCreditCardProviderType eProviderType = (PaymentMeanCreditCardProviderType)Session["ProviderType"];
                string GatewayDateFormat = "HHmmssddMMyy";

                switch (eProviderType)
                {
                    case PaymentMeanCreditCardProviderType.pmccpPayu:
                        strReference = j.payu_reference.ToString();
                        strTransactionId = j.payu_transaction_id.ToString();
                        strGatewayDate = j.payu_date_time_local_fmt.ToString();
                        strAuthCode = j.payu_auth_code.ToString();
                        strAuthResult = j.errorCode.ToString();
                        strAuthResultDesc = j.errorMessage.ToString();
                        strCardHash = j.payu_card_hash.ToString();
                        strCardReference = j.payu_card_reference.ToString();
                        strCardScheme = j.payu_card_scheme.ToString();
                        strMaskedCardNumber = j.payu_masked_card_number.ToString();
                        dtExpDate = null;
                        if (j.payu_transaction_id != null) { strCFTransactionId = j.payu_transaction_id.ToString(); }
                        break;
                    case PaymentMeanCreditCardProviderType.pmccpMoneris:
                        strReference = j.moneris_reference.ToString();
                        strTransactionId = j.moneris_transaction_id.ToString();
                        strGatewayDate = j.moneris_date_time_local_fmt.ToString();
                        strAuthCode = j.moneris_auth_code.ToString();
                        strAuthResult = j.errorCode.ToString();
                        strAuthResultDesc = j.errorMessage.ToString();
                        strCardHash = j.moneris_card_hash.ToString();
                        strCardReference = j.moneris_card_reference.ToString();
                        strCardScheme = j.moneris_card_scheme.ToString();
                        strMaskedCardNumber = j.moneris_masked_card_number.ToString();
                        dtExpDate = null;
                        if (j.moneris_transaction_id != null) { strCFTransactionId = j.moneris_transaction_id.ToString(); }
                        GatewayDateFormat = "yyyy-MM-dd HH:mm:ss";
                        break;
                    case PaymentMeanCreditCardProviderType.pmccpPaysafe:
                        strReference = j.paysafe_reference.ToString();
                        strTransactionId = j.paysafe_transaction_id.ToString();
                        strGatewayDate = j.paysafe_date_time_local_fmt.ToString();
                        strAuthCode = j.paysafe_auth_code.ToString();
                        strAuthResult = j.errorCode.ToString();
                        strAuthResultDesc = j.errorMessage.ToString();
                        strCardHash = j.paysafe_card_hash.ToString();
                        strCardReference = j.paysafe_card_reference.ToString();
                        strCardScheme = j.paysafe_card_scheme.ToString();
                        strMaskedCardNumber = j.paysafe_masked_card_number.ToString();
                        dtExpDate = null;
                        if (j.paysafe_transaction_id != null) { strCFTransactionId = j.paysafe_transaction_id.ToString(); }
                        GatewayDateFormat = "yyyy-MM-dd HH:mm:ss";
                        break;
                    case PaymentMeanCreditCardProviderType.pmccpTransbank:
                        strReference = j.transbank_reference.ToString();
                        strTransactionId = j.transbank_transaction_id.ToString();
                        strGatewayDate = j.transbank_date_time_local_fmt.ToString();
                        strAuthCode = j.transbank_auth_code.ToString();
                        strAuthResult = j.errorCode.ToString();
                        strAuthResultDesc = j.errorMessage.ToString();
                        strCardHash = j.transbank_card_hash.ToString();
                        strCardReference = j.transbank_card_reference.ToString();
                        strCardScheme = j.transbank_card_scheme.ToString();
                        strMaskedCardNumber = j.transbank_masked_card_number.ToString();
                        dtExpDate = null;
                        if (j.transbank_transaction_id != null) { strCFTransactionId = j.transbank_transaction_id.ToString(); }
                        break;
                    case PaymentMeanCreditCardProviderType.pmccpBSRedsys:
                        strReference = j.bsredsys_reference.ToString();
                        strTransactionId = j.bsredsys_transaction_id.ToString();
                        strGatewayDate = j.bsredsys_date_time_local_fmt.ToString();
                        strAuthCode = j.bsredsys_auth_code.ToString();
                        strAuthResult = j.bsredsys_auth_result.ToString();
                        strAuthResultDesc = j.result.ToString();
                        strCardHash = j.bsredsys_card_hash.ToString();
                        strCardReference = j.bsredsys_card_reference.ToString();
                        strCardScheme = j.bsredsys_card_scheme.ToString();
                        strMaskedCardNumber = j.bsredsys_masked_card_number.ToString();
                        dtExpDate = null;
                        if (j.bsredsys_transaction_id != null) { strCFTransactionId = j.bsredsys_transaction_id.ToString(); }
                        GatewayDateFormat = "dd/MM/yyyy HH:mm";
                        break;
                }

                INSTALLATION oInstallation = null;
                DateTime? dtinstDateTime = null;
                geograficAndTariffsRepository.getInstallationById(model.InstallationId, ref oInstallation, ref dtinstDateTime);
                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = customersRepository.GetInstallationGatewayConfig(model.InstallationId);

                if (oGatewayConfig != null &&
                           !(oGatewayConfig.CPTGC_ENABLED != 0 && oGatewayConfig.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard))
                {
                    oGatewayConfig = null;
                }

                if (oGatewayConfig == null)
                {
                    oGatewayConfig = oInstallation.CURRENCy.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                            .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                        ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                        r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                        r.CPTGC_PROVIDER == Convert.ToInt32(eProviderType))
                                            .FirstOrDefault();
                }

                if (external == null) ErrorInfo = "external is null";
                if (model == null) ErrorInfo = "model is null";
                if (oGatewayConfig == null) ErrorInfo = "oGatewayConfig is null";

                int iWSTimeout = infraestructureRepository.GetRateWSTimeout(model.InstallationId);

                string res = external.ConfirmFinePaymentNonUser(model.TicketNumber, model.Quantity,
                                model.TotalQuantity, model.Plate, model.PercFEE, model.PercVAT1,
                                model.PercVAT2, model.PercFEETopped, model.FixedFEE, model.PartialVAT1,
                                model.PartialPercFEE, model.PartialFixedFEE, model.PartialPercFEEVAT,
                                model.PartialFixedFEEVAT, model.TaxMode, model.Email, strReference, strTransactionId,
                                strCFTransactionId, strGatewayDate, strAuthCode, strAuthResult,
                                strCardHash, strCardReference, strCardScheme, strMaskedCardNumber, null, null,
                                dtExpDate, eProviderType, model.InstallationId, geograficAndTariffsRepository,
                                model.CurrencyId, model.AmountCurrencyIsoCode, oGatewayConfig.CPTGC_ID, fineRepository, model.GrpId, model.AuthId, iWSTimeout);

                // SUCCESS res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><autorecharged>0</autorecharged><r>1</r><utc_offset>-420</utc_offset></ipark_out>
                // ERROR   res = <?xml version="1.0" encoding="UTF-8"?><ipark_out><r>-9</r></ipark_out>
                // Extraemos el valor de r
                int xmlNodeValue = GetXMLNodeValue(res, "r");
                result = (ExternalWS.ResultType)xmlNodeValue;

                if (result == ExternalWS.ResultType.Result_OK)
                {
                    // Capturamos pago
                    string strUserReference = null;
                    string strSecundaryTransactionId = null;
                    int iTransactionFee = 0;
                    string strTransactionFeeCurrencyIsocode = null;
                    string strTransactionURL = null;
                    string strRefundTransactionURL = null;

                    DateTime dt;
                    DateTime.TryParseExact(strGatewayDate, GatewayDateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
                    if (eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA ||
                        eProviderType == PaymentMeanCreditCardProviderType.pmccpPayu ||
                        eProviderType == PaymentMeanCreditCardProviderType.pmccpMoneris ||
                        eProviderType == PaymentMeanCreditCardProviderType.pmccpTransbank ||
                        eProviderType == PaymentMeanCreditCardProviderType.pmccpPaysafe ||
                        eProviderType == PaymentMeanCreditCardProviderType.pmccpBSRedsys ||
                        CommitTransaction(model.InstallationId, eProviderType, strTransactionId, strCFTransactionId, model.CurrencyId, dt, strAuthCode, model.TotalQuantity, model.AmountCurrencyIsoCode, out strUserReference, out strAuthResult, out strGatewayDate, out strSecundaryTransactionId, out iTransactionFee, out strTransactionFeeCurrencyIsocode, out strTransactionURL, out strRefundTransactionURL))
                    {
                        string culture = Session["culture"].ToString();
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                        integraMobile.Properties.Resources.Culture = ci;

                        int iQuantity = model.Quantity;
                        decimal dPercVAT1 = model.PercVAT1;
                        decimal dPercVAT2 = model.PercVAT2;
                        decimal dPercFEE = model.PercFEE;
                        int iPercFEETopped = (int)(model.PercFEETopped);
                        int iFixedFEE = (int)(model.FixedFEE);

                        int iPartialVAT1 = model.PartialVAT1;
                        int iPartialPercFEE = model.PartialPercFEE;
                        int iPartialFixedFEE = model.PartialFixedFEE;
                        int iPartialPercFEEVAT = model.PartialPercFEEVAT;
                        int iPartialFixedFEEVAT = model.PartialFixedFEEVAT;
                        int iQFEE = model.QFEE;
                        int iQVAT = model.QVAT;

                        int iLayout = 0;
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            OPERATOR oOperator = customersRepository.GetDefaultOperator();
                            if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                        }


                        string sLayoutSubtotal = "";
                        string sLayoutTotal = "";

                        string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(model.CurrencyId));

                        if (iLayout == 2)
                        {
                            sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }
                        else if (iLayout == 1)
                        {
                            sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.Total) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                            (model.VAT_Percent != 0 ? string.Format("{0:0.00#}% ", model.VAT_Percent * 100) : ""),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.QVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                        }

                        string strRechargeEmailSubject = ResourceExtension.GetLiteral("ConfirmFinePayment_EmailHeader");
                        /*
                            ID: {0}<br>
                            *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                            *  Total Pagado: {2} 
                            *  Matrícula: 3
                            */

                        string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmTicketPaymentNonUser_EmailBody"),
                            model.TicketNumber,
                            dt,
                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(model.TotalQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                            infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                            model.Plate,
                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                            sLayoutSubtotal, sLayoutTotal,
                            GetEmailFooter(model.InstallationShortDesc, model.CountryCode));

                        if (model.Email != ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString())
                        {
                            SendEmail(model.Email, strRechargeEmailSubject, strRechargeEmailBody);
                        }
                        else
                        {
                            Session["strRechargeEmailSubject"] = strRechargeEmailSubject;
                            Session["strRechargeEmailBody"] = strRechargeEmailBody;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("ConfirmPayment Exception: {0} [{1}]", e.Message, ErrorInfo));
            }

            m_Log.LogMessage(LogLevels.logERROR, string.Format("ConfirmPayment: Exiting with failure: PAN={0}; Card Reference={1}", strMaskedCardNumber, strCardReference));

            Session["Sess_strReference"] = null;
            Session["Sess_strTransactionId"] = null;
            Session["Sess_strGatewayDate"] = null;
            Session["Sess_strAuthCode"] = null;
            Session["Sess_strAuthResult"] = null;
            Session["Sess_strAuthResultDesc"] = null;
            Session["Sess_strCardHash"] = null;
            Session["Sess_strCardReference"] = null;
            Session["Sess_strCardScheme"] = "";
            Session["Sess_strMaskedCardNumber"] = null;
            Session["Sess_dtExpDate"] = null;

            Session["QuantityToRecharge"] = null;
            Session["QuantityToRechargeBase"] = null;
            Session["PercVAT1"] = null;
            Session["PercVAT2"] = null;
            Session["PercFEE"] = null;
            Session["PercFEETopped"] = null;
            Session["FixedFEE"] = null;
            Session["CurrencyToRecharge"] = null;
            Session["InStripePayment"] = null;

            return result;
        }

        public USER GetUserFromSession()
        {
            USER oUser = null;
            try
            {
                if (Session["USER_ID"] != null)
                {
                    decimal dUserId = Convert.ToDecimal(Session["USER_ID"]);
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

        #region Mailing

        private bool SendEmail(string strEmailRecipient, string strEmailSubject, string strEmailBody)
        {
            bool bRes = true;
            try
            {

                long lSenderId = infraestructureRepository.SendEmailTo(strEmailRecipient, strEmailSubject, strEmailBody, null);

            }
            catch
            {
                bRes = false;
            }

            return bRes;
        }

        #endregion

        #endregion

        #endregion                
    }
}
