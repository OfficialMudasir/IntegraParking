using System;
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
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.Controllers
{
    public class RetailerController : Controller
    {
        #region Properties
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(RetailerController));
        private const string DEFAULT_STRIPE_IMAGE_URL = "https://stripe.com/img/documentation/checkout/marketplace.png";
        public RetailerCouponsModel RetailerCouponsSession
        {
            get {
                   if (Session["RetailerCoupons"] == null)
                   {
                       Session["RetailerCoupons"] = new RetailerCouponsModel();
                   }
                   return (RetailerCouponsModel)Session["RetailerCoupons"];
                }
            set {Session["RetailerCoupons"] = value; }
        }
        private IRetailerRepository retailerRepository;
        private IInfraestructureRepository infraestructureRepository;
        private ICustomersRepository customersRepository;
        private IGeograficAndTariffsRepository geograficAndTariffsRepository;
        #endregion

        #region Constructor
        public RetailerController(IRetailerRepository _retailerRepository, IInfraestructureRepository _infraestructureRepository, ICustomersRepository _customersRepository)
        {
            this.retailerRepository = _retailerRepository;
            this.infraestructureRepository = _infraestructureRepository;
            this.customersRepository = _customersRepository;
        }
        #endregion

        #region Public Methods
        public ActionResult Retailer(bool? init)
        {
            SetCulture();

            if (ConfigurationManager.AppSettings["COUPON_ENABLED"] != null && ConfigurationManager.AppSettings["COUPON_ENABLED"].ToString().ToLower() == "true")
            {
                RetailerCouponsModel model = new RetailerCouponsModel();
                if (init.HasValue && init.Value)
                {
                    model = RetailerCouponsSession;
                    if (model == null) model = new RetailerCouponsModel();
                }
                model.Init(retailerRepository, customersRepository);
                model.CalculateTotals(customersRepository);
                return View(model);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Retailer(RetailerCouponsModel model)
        {
            SetCulture();

            if (ConfigurationManager.AppSettings["COUPON_ENABLED"] != null && ConfigurationManager.AppSettings["COUPON_ENABLED"].ToString().ToLower() == "true")
            {
                string strCreditCardProviderPrefix = "CC";
                string strCreditCardSessionName = "";
                //ViewData["RetailerCouponsModel"] = model;            
                if (ModelState.IsValid)
                {
                    //if((RetailerCouponsModel)Session["RetailerCoupons"]==null)
                    //{
                    //    Session["RetailerCoupons"] = model;
                    //}
                    //if (RetailerCouponsSession == null)
                    //{
                        RetailerCouponsSession = model;
                    //}
                    model.Init(retailerRepository, customersRepository);
                    model.CalculateTotals(customersRepository);

                    CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

                    oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                        .Where(r => r.CPTGC_ENABLED != 0 &&
                                                    r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                                    r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard))
                                        .FirstOrDefault();

                    PaymentMeanCreditCardProviderType eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
                    try
                    {
                        eProviderType = (PaymentMeanCreditCardProviderType)oGatewayConfig.CPTGC_PROVIDER;
                    }
                    catch
                    {
                        eProviderType = PaymentMeanCreditCardProviderType.pmccpCreditCall;
                    }


                    switch (eProviderType)
                    {
                        case PaymentMeanCreditCardProviderType.pmccpCreditCall:
                            strCreditCardProviderPrefix = "CC" + "Redirect";
                            strCreditCardSessionName = "InCreditCallPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpIECISA:
                            strCreditCardProviderPrefix = "CC2" + "Redirect";
                            strCreditCardSessionName = "InIECISAPayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpStripe:
                            strCreditCardProviderPrefix = "CC3" + "Redirect";
                            strCreditCardSessionName = "InStripePayment";
                            break;
                        case PaymentMeanCreditCardProviderType.pmccpPayu:
                            strCreditCardProviderPrefix = "PayuRequest";
                            strCreditCardSessionName = "InPayuPayment";
                            break;
                        default:
                            break;
                    }



                    Session[strCreditCardSessionName] = true;

                    return RedirectToAction(strCreditCardProviderPrefix, new System.Web.Routing.RouteValueDictionary(model.ToDictionary(Request.Params)));
                }
                else
                {
                    model.Init(retailerRepository, customersRepository);
                    model.CalculateTotals(customersRepository);
                    return View(model);
                }
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CalculateTotals(RetailerCouponsModel model)
        {
            SetCulture();

            model.Init(retailerRepository, customersRepository);
            model.CalculateTotals(customersRepository);

            return Json(model);
        }

        public ActionResult CCRedirect(RetailerCouponsModel model)
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure");

            model.Init(retailerRepository, customersRepository);
            model.CalculateTotals(customersRepository);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            string sDecimalFormat = "{0:0";
            if (model.NumDecimals > 0)
            {
                sDecimalFormat += ".";
                for (int i = 0; i < model.NumDecimals; sDecimalFormat += "0", i++) ;
                sDecimalFormat += "}";
            }

            model.CurrencyDivisorFromIsoCode = infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode);
            RetailerCouponsSession = model;

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

            oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                .Where(r => r.CPTGC_ENABLED != 0 && 
                                            r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpCreditCall))
                                .FirstOrDefault();

            ViewData["ekashu_form_url"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_EKASHU_FORM_URL;
            ViewData["ekashu_seller_id"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID;
            ViewData["ekashu_seller_key"] = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY.Substring(0, 8);
            string sAmount = string.Format(provider, sDecimalFormat, model.Total);
            ViewData["ekashu_amount"] = sAmount;
            ViewData["ekashu_currency"] = model.AmountCurrencyIsoCode;
            ViewData["ekashu_reference"] = CardEasePayments.UserReference();
            ViewData["ekashu_hash_code"] = CardEasePayments.HashCode(oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                     oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_HASH_KEY,
                                                                     (string)ViewData["ekashu_reference"], 
                                                                     sAmount);
            ViewData["ekashu_description"] = ResourceExtension.GetLiteral("Account_Recharge_ItemDescription");

            string strSellerName = oGatewayConfig.CREDIT_CALL_CONFIGURATION.CCCON_SELLER_NAME;

            if (strSellerName.Length > 8)
                ViewData["ekashu_seller_name"] = strSellerName.Substring(0, 8);
            else
                ViewData["ekashu_seller_name"] = strSellerName;

            string requrl = Request.Url.ToString();
            ViewData["ekashu_failure_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCFailure";
            ViewData["ekashu_return_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCCancel";
            ViewData["ekashu_success_url"] = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "CCSuccess";

            return View();
            // ***
            //return RedirectToAction("CCSuccess");
            // ***
        }

        public ActionResult CC2Redirect(RetailerCouponsModel model)
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CC2ailure");
            
            
            model.Init(retailerRepository, customersRepository);
            model.CalculateTotals(customersRepository);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            string sDecimalFormat = "{0:0";
            if (model.NumDecimals > 0)
            {
                sDecimalFormat += ".";
                for (int i = 0; i < model.NumDecimals; sDecimalFormat += "0", i++) ;
                sDecimalFormat += "}";
            }

            model.CurrencyDivisorFromIsoCode = infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode);
            RetailerCouponsSession = model;
            string sAmount = string.Format(provider, sDecimalFormat, model.Total);

            int iAmount = Convert.ToInt32(model.Total * model.CurrencyDivisorFromIsoCode, provider);
            DateTime dtNow = DateTime.Now;
            DateTime dtUTCNow = DateTime.UtcNow;

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

            oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                .Where(r => r.CPTGC_ENABLED != 0 &&
                                            r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                                .FirstOrDefault();

            string strTransactionId = null;
            string strOpReference = null;
            string errorMessage = null;
            string strCardHash = null;

            IECISAPayments cardPayment = new IECISAPayments();

            var uri = new Uri(Request.Url.AbsoluteUri);
            string strURLPath = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/"));
            string strLang = ((CultureInfo)Session["Culture"]).Name.Substring(0, 2);
            IECISAPayments.IECISAErrorCode eErrorCode;

            PayuPayments cardPayment2 = new PayuPayments();
            
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
                                            iAmount,
                                            model.AmountCurrencyIsoCode,
                                            infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(model.AmountCurrencyIsoCode),
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

                return RedirectToAction("CCFailure");


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

                    return RedirectToAction("CCFailure");

                }
                else
                {
                    customersRepository.StartRecharge(oGatewayConfig.CPTGC_ID,
                                                           model.Email,
                                                           dtUTCNow,
                                                           dtNow,
                                                           iAmount,
                                                           infraestructureRepository.GetCurrencyFromIsoCode(model.AmountCurrencyIsoCode),
                                                           "",
                                                           strOpReference,
                                                           strTransactionId,
                                                           "",
                                                           "",
                                                           "",
                                                           PaymentMeanRechargeStatus.Committed);
                    Session["cardHash"] = strCardHash;
                    return Redirect(strRedirectURL);
                }

            }

            return RedirectToAction("CCFailure");
        }

        public ActionResult CC2Reply(string transactionId)
        {
            SetCulture();


            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CCFailure");

           
            try
            {

                //RetailerCouponsModel model = (RetailerCouponsModel)Session["RetailerCoupons"];
                RetailerCouponsModel model = RetailerCouponsSession;
                
                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

                oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                              .Where(r => r.CPTGC_ENABLED != 0 &&
                                          r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                          r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                          r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                              .FirstOrDefault();


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
                string strCardReference = "";
                string strMaskedCardNumber = "";

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


                    customersRepository.FailedRecharge(oGatewayConfig.CPTGC_ID,
                                    model.Email,
                                    transactionId,
                                    PaymentMeanRechargeStatus.Cancelled);

                    string errorCode = eErrorCode.ToString();
                    switch (eErrorCode)
                    {

                        case IECISAPayments.IECISAErrorCode.TransactionCancelled:
                        case IECISAPayments.IECISAErrorCode.TransactionCancelled2:
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("iecisaResponse.GetTransactionStatus : errorCode={0} ; errorMessage={1}",
                               errorCode, errorMessage));



                                return RedirectToAction("CCCancel");
                            }
                            break;

                        default:
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("iecisaResponse.GetTransactionStatus : errorCode={0} ; errorMessage={1}",
                                errorCode, errorMessage));

                                return RedirectToAction("CCFailure");
                            }
                            break;
                    }

                }


                Session["Sess_strMaskedCardNumber"] = strMaskedCardNumber;
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

                /*Session["Sess_strReference"] = strOpReference;
                Session["Sess_strTransactionId"] = strTransactionID;
                Session["Sess_strGatewayDate"] = DateTime.Now.ToString("ddMMyyyyHHmmss");
                Session["Sess_strAuthCode"] = strNumAut;
                Session["Sess_strAuthResult"] = strResultCode;
                Session["Sess_strAuthResultDesc"] = strResultCodeDes;
                Session["Sess_strCardScheme"] = "";
                Session["Sess_dtExpDate"] = dtExpDate;



                return RedirectToAction("CC2Success");*/


            }
            catch (Exception e)
            {
            }           

            return RedirectToAction("CCFailure");
        }

        public ActionResult CC3Redirect(RetailerCouponsModel model)
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CCFailure");


            model.Init(retailerRepository, customersRepository);
            model.CalculateTotals(customersRepository);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            string sDecimalFormat = "{0:0";
            if (model.NumDecimals > 0)
            {
                sDecimalFormat += ".";
                for (int i = 0; i < model.NumDecimals; sDecimalFormat += "0", i++) ;
                sDecimalFormat += "}";
            }

            //Session["RetailerCoupons"] = model;
            model.CurrencyDivisorFromIsoCode = infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode);
            RetailerCouponsSession = model;
            string sAmount = string.Format(provider, sDecimalFormat, model.Total);

            int iAmount = Convert.ToInt32(model.Total * model.CurrencyDivisorFromIsoCode, provider);
            DateTime dtNow = DateTime.Now;
            DateTime dtUTCNow = DateTime.UtcNow;

            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

            oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                .Where(r => r.CPTGC_ENABLED != 0 &&
                                            r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                            r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                            r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                                .FirstOrDefault();

                  

            var uri = new Uri(Request.Url.AbsoluteUri);
            string strURLPath = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/"));
            string strLang = ((CultureInfo)Session["Culture"]).Name.Substring(0, 2);

            
            string strImageURL = DEFAULT_STRIPE_IMAGE_URL;

            if (!string.IsNullOrEmpty(oGatewayConfig.STRIPE_CONFIGURATION.STRCON_IMAGE_URL))
            {
                strImageURL = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_IMAGE_URL;
            }

            ViewData["email"] = model.Email;
            ViewData["amount"] = iAmount.ToString(); ;
            ViewData["currency"] = (string)Session["CurrencyToRecharge"];
            ViewData["key"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_DATA_KEY;
            ViewData["commerceName"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_COMMERCE_NAME;
            ViewData["panelLabel"] = oGatewayConfig.STRIPE_CONFIGURATION.STRCON_PANEL_LABEL;
            ViewData["description"] = ResourceExtension.GetLiteral("RetailerCoupons_BuyButton");
            ViewData["image"] = strImageURL;
            

            return View();
        }

        public ActionResult CC3Reply(StripeResponseModel oModel)
        {
            SetCulture();

            string strPAN = "";
            string strCardToken = "";

            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CCFailure");
         
            try
            {

                //RetailerCouponsModel model = (RetailerCouponsModel)Session["RetailerCoupons"];
                RetailerCouponsModel model = RetailerCouponsSession;

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

                oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                              .Where(r => r.CPTGC_ENABLED != 0 &&
                                          r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                          r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                          r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpStripe))
                              .FirstOrDefault();


                if (!string.IsNullOrEmpty(oModel.stripeErrorCode) && oModel.stripeErrorCode == "window_closed")
                {
                    return RedirectToAction("CCCancel");

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

                            int iAmount = Convert.ToInt32(model.Total * infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode), provider);

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
                                                            iAmount,
                                                            model.AmountCurrencyIsoCode,
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
                                Session["Sess_strGatewayDate"] = DateTime.ParseExact(strStripeDateTime, "HHmmssddMMyy",
                                                                CultureInfo.InvariantCulture);
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

            return RedirectToAction("CCFailure");
        }

        public ActionResult CCSuccess()
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure");

            //RetailerCouponsModel model = (RetailerCouponsModel)Session["RetailerCoupons"];
            RetailerCouponsModel model = RetailerCouponsSession;

            string strReference = Request["ekashu_reference"];
            string strAuthCode = Request["ekashu_auth_code"];
            string strAuthResult = Request["ekashu_auth_result"];
            string strCardHash = Request["ekashu_card_hash"];
            string strCardReference = Request["ekashu_card_reference"];
            string strCardScheme = Request["ekashu_card_scheme"];
            string strGatewayDate = Request["ekashu_date_time_local_fmt"];
            string strMaskedCardNumber = Request["ekashu_masked_card_number"];
            string strTransactionId = Request["ekashu_transaction_id"];                        
            string strExpMonth = Request["ekashu_expires_end_month"];
            string strExpYear = Request["ekashu_expires_end_year"];
            

            DateTime? dtExpDate = null;
            if ((strExpMonth.Length == 2) && (strExpYear.Length == 4))
            {
                dtExpDate = new DateTime(Convert.ToInt32(strExpYear), Convert.ToInt32(strExpMonth), 1).AddMonths(1).AddSeconds(-1);
            }

            if (CCSuccess(PaymentMeanCreditCardProviderType.pmccpCreditCall,
                         strReference,
                         strAuthCode,
                         strAuthResult,
                         "",
                         strCardHash,
                         strCardReference,
                         strCardScheme,
                         strGatewayDate,
                         strMaskedCardNumber,
                         strTransactionId,
                         dtExpDate.Value,
                         model))
            {

                ActionResult oRet = View("Invoice", model);
                //model.DeleteQRTmpFiles();

                return oRet;
            }
            else
            {
                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                return View("Retailer", model);
            }
        }

        public ActionResult CC2Success()
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InIECISAPayment"]))
                return RedirectToAction("CCFailure");

            RetailerCouponsModel model = RetailerCouponsSession;

            string strReference = Session["Sess_strReference"].ToString();
            string strTransactionId = Session["Sess_strTransactionId"].ToString();
            string strCFTransactionId = Session["Sess_strCFTransactionId"].ToString();
            string strGatewayDate = Session["Sess_strGatewayDate"].ToString();
            string strAuthCode = Session["Sess_strAuthCode"].ToString();
            string strAuthResult = Session["Sess_strAuthResult"].ToString();
            string strAuthResultDesc = Session["Sess_strAuthResultDesc"].ToString();
            string strCardHash = "";
            string strCardReference = "";
            string strCardScheme = Session["Sess_strCardScheme"].ToString();
            string strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
            DateTime? dtExpDate = (DateTime?)Session["Sess_dtExpDate"];
            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

            oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                          .Where(r => r.CPTGC_ENABLED != 0 &&
                                      r.CURRENCy.CUR_ISO_CODE == model.AmountCurrencyIsoCode &&
                                      r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                      r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpIECISA))
                          .FirstOrDefault();

            customersRepository.CompleteStartRecharge(oGatewayConfig.CPTGC_ID,
                                                         model.Email,
                                                         strTransactionId,
                                                         strAuthResult,
                                                         strCFTransactionId,
                                                         strGatewayDate,
                                                         strAuthCode,
                                                         PaymentMeanRechargeStatus.Committed);

            if (CCSuccess(PaymentMeanCreditCardProviderType.pmccpIECISA,
                         strReference,
                         strAuthCode,
                         strAuthResult,
                         strAuthResultDesc,
                         strCardHash,
                         strCardReference,
                         strCardScheme,
                         strGatewayDate,
                         strMaskedCardNumber,
                         strTransactionId,
                         dtExpDate.Value,
                         model))
            {
                ActionResult oRet = View("Invoice", model);
                //model.DeleteQRTmpFiles();

                return oRet;
            }
            else
            {
                
                customersRepository.FailedRecharge(oGatewayConfig.CPTGC_ID,
                                    model.Email,
                                    strTransactionId,
                                    PaymentMeanRechargeStatus.Cancelled);

                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                return View("Retailer", model);
            }
        }

        public ActionResult CC3Success()
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InStripePayment"]))
                return RedirectToAction("CCFailure");

            RetailerCouponsModel model = RetailerCouponsSession;

            string strReference = Session["Sess_strReference"].ToString();
            string strTransactionId = Session["Sess_strTransactionId"].ToString();
            string strGatewayDate = Session["Sess_strGatewayDate"].ToString();
            string strAuthCode = Session["Sess_strAuthCode"].ToString();
            string strAuthResult = Session["Sess_strAuthResult"].ToString();
            string strAuthResultDesc = Session["Sess_strAuthResultDesc"].ToString();
            string strCardHash = "";
            string strCardReference = "";
            string strCardScheme = Session["Sess_strCardScheme"].ToString();
            string strMaskedCardNumber = Session["Sess_strMaskedCardNumber"].ToString();
            DateTime? dtExpDate = (DateTime?)Session["Sess_dtExpDate"];

            if (CCSuccess(PaymentMeanCreditCardProviderType.pmccpStripe,
                         strReference,
                         strAuthCode,
                         strAuthResult,
                         strAuthResultDesc,
                         strCardHash,
                         strCardReference,
                         strCardScheme,
                         strGatewayDate,
                         strMaskedCardNumber,
                         strTransactionId,
                         dtExpDate.Value,
                         model))
            {
                ActionResult oRet = View("Invoice", model);
                //model.DeleteQRTmpFiles();

                return oRet;
            }
            else
            {               

                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                return View("Retailer", model);
            }
        }

        public ActionResult CCFailure()
        {
            SetCulture();

            Session["InCreditCallPayment"] = null;
            Session["InIECISAPayment"] = null;
            Session["InStripePayment"] = null;
            Session["InPayuPayment"] = null;
            Session["OVERWRITE_CARD"] = false;
            
            return RedirectToAction("Retailer", new { init = true });
        }

        public ActionResult CCCancel()
        {
            SetCulture();

            if (!Convert.ToBoolean(Session["InCreditCallPayment"]))
                return RedirectToAction("CCFailure");
            
            Session["InCreditCallPayment"] = null;
            Session["InIECISAPayment"] = null;
            Session["InStripePayment"] = null;
            Session["InPayuPayment"] = null;
            Session["OVERWRITE_CARD"] = false;
            
            return RedirectToAction("Retailer", new { init = true });            
        }

        [HttpPost]
        public ActionResult DeleteQRTmpFiles(decimal RetailerId)
        {
            SetCulture();

            RetailerCouponsModel.DeleteQRTmpFiles(retailerRepository, RetailerId);
            return Json(true);
        }
        #endregion

        #region Private Methods
        private bool CCSuccess(PaymentMeanCreditCardProviderType eProviderType,
                            string strReference ,
                            string strAuthCode,
                            string strAuthResult,
                            string strAuthResultDesc,
                            string strCardHash,
                            string strCardReference,
                            string strCardScheme,
                            string strGatewayDate,
                            string strMaskedCardNumber,
                            string strTransactionId,
                            DateTime dtExpDate,
                            RetailerCouponsModel model)
        {
            bool bRes = true;

            try
            {
                RETAILER oRetailer = null;

                int iPercFEETopped = Convert.ToInt32(Math.Round(model.PercFEETopped, MidpointRounding.AwayFromZero));
                int iFixedFEE = Convert.ToInt32(Math.Round(model.FixedFEE, MidpointRounding.AwayFromZero));

                if (retailerRepository.UpdateRetailerCoupons(ref oRetailer,
                                                             model.Name, model.Email, model.Address, model.DocId,
                                                             model.Coupons, model.CouponAmount,
                                                             model.AmountCurrencyIsoCode, model.TotalAmount,
                                                             model.PercVAT1, model.PercVAT2, model.PartialVAT1, model.PercFEE,iPercFEETopped , model.PartialPercFEE, iFixedFEE, model.PartialFixedFEE,                                                             
                                                             model.Total,
                                                             model.CurrencyDivisorFromIsoCode,
                                                             eProviderType,
                                                             strReference,
                                                             strTransactionId,
                                                             strGatewayDate,
                                                             strAuthCode,
                                                             strAuthResult,
                                                             strAuthResultDesc,
                                                             strCardHash,
                                                             strCardReference,
                                                             strCardScheme,
                                                             strMaskedCardNumber,
                                                             dtExpDate,
                                                             null,
                                                             null,
                                                             null))
                {

                    Session["OVERWRITE_CARD"] = false;
                    Session["InCreditCallPayment"] = null;
                    Session["InIECISAPayment"] = null;
                    Session["InStripePayment"] = null;
                    //Session["RetailerCoupons"] = null;
                    RetailerCouponsSession = null;

                    
                    model.RetailerId = oRetailer.RTL_ID;
                    model.LoadRechargeData(retailerRepository);

                    string sInvoicePdf = model.GenerateInvoicePdf(retailerRepository, HttpContext.Server.MapPath("~/Invoicing/"), HttpContext.Server.MapPath("~/Content/img/"));
                    m_Log.LogMessage(LogLevels.logDEBUG, string.Format("CCSuccess Invoice Path: {0}", sInvoicePdf));

                    List<FileAttachmentInfo> lstAttachments = new List<FileAttachmentInfo>();

                    FileAttachmentInfo file = new FileAttachmentInfo();
                    file.strName = Path.GetFileName(sInvoicePdf); //este es el que aparece como nombre del adjunto dentro del correo.
                    file.strMediaType = "application/pdf";
                    file.filePath = sInvoicePdf; //¿ sInvoicePdf es el path absoluto?. si no es así tiene que ser el absoluto
                    //como alternativa podríamos pasar el fichero como un array de bytes y entonces no haría falta pasar el filePath.
                    //file.fileContent = System.IO.File.ReadAllBytes(sInvoicePdf);
                    lstAttachments.Add(file);

                    infraestructureRepository.SendEmailWithAttachmentsTo(model.Email, ResourceExtension.GetLiteral("RetailerInvoice_NotificationEmail_Subject"),
                                                                        ResourceExtension.GetLiteral("RetailerInvoice_NotificationEmail_Body"), lstAttachments, null);
                    try
                    {
                        System.IO.File.Delete(sInvoicePdf);
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("CCSuccess Exception: {0}", e.Message));

                    }

                    bRes = true;
                }
                else
                    bRes = false;
                    
            }
            catch(Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CCSuccess Exception: {0}", e.Message));
                bRes=false;   
            };

            return bRes;
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

        #region Payu Payment
        [HttpGet]
        public ActionResult PayuRequest(RetailerCouponsModel model)
        {
            try
            {
                model.Init(retailerRepository, customersRepository);
                model.CalculateTotals(customersRepository);

                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = null;

                oGatewayConfig = customersRepository.GetCurrenciesPaymentTypeGatewayConfigs()
                                    .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_IS_INTERNAL != 0 &&
                                                ((r.CPTGC_INTERNAL_SOAPP_ID.HasValue && r.SOURCE_APP.SOAPP_DEFAULT == 1) || !r.CPTGC_INTERNAL_SOAPP_ID.HasValue) &&
                                                r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                                r.CPTGC_PROVIDER == Convert.ToInt32(PaymentMeanCreditCardProviderType.pmccpPayu)).FirstOrDefault();

                Session["Payu_Config_id"] = oGatewayConfig.CPTGC_PAYUCON_ID;
                string email = model.Email;
                
                string isocode = model.AmountCurrencyIsoCode;
                string description = model.DocId.ToString();
                string utcdate = DateTime.UtcNow.ToString("HHmmssddMMyy");
                string culture = ((CultureInfo)Session["Culture"]).Name.Substring(0, 2);
                string ReturnURL = "";

                model.CurrencyDivisorFromIsoCode = infraestructureRepository.GetCurrencyDivisorFromIsoCode(model.AmountCurrencyIsoCode);
                RetailerCouponsSession = model;

                int iAmount = Convert.ToInt32(model.Total * model.CurrencyDivisorFromIsoCode);

                string hash = string.Empty;
                PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
                m_Log.LogMessage(LogLevels.logINFO, string.Format("RetailerController::PayuRequest: isocode:{0}, description:{1}, utcdate:{2}, culture{3}", isocode, description, utcdate, culture));
                return payu.PayuRequest(string.Empty, email, iAmount, isocode, description, utcdate, culture, ReturnURL, hash);
                
            }
            catch(Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("PayuRequest Exception: {0}", e.Message));
                
            };
            return RedirectToAction("CCFailure");
        }

        [HttpPost]
        public ActionResult PayuResponse()
        {
            RetailerCouponsModel model = RetailerCouponsSession;
            Session["AmountCurrencyIsoCode"] = model.AmountCurrencyIsoCode;
            Session["Total"] = model.Total;
            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            return payu.PayuResponse();
        }

        [HttpGet]
        public ActionResult PayuResult(string r)
        {
            PayuController payu = new PayuController(customersRepository, infraestructureRepository, Request, Server, Session, Convert.ToDecimal(Session["Payu_Config_id"]));
            string r_decrypted = payu.DecryptCryptResult(r, Session["HashSeed"].ToString());
            dynamic j = System.Web.Helpers.Json.Decode(r_decrypted);
            RetailerCouponsModel model = RetailerCouponsSession;
            if (j.result == "succeeded")
            {
                if (Convert.ToBoolean(Session["InPayuPayment"]))
                {
                    Session["InPayuPayment"] = null;

                    string strReference = Session["reference"].ToString();
                    string strTransactionId = Session["transactionID"].ToString();
                    string strGatewayDate = Session["chargeDateTime"].ToString();
                    string strAuthCode = Session["authCode"].ToString();
                    string strAuthResult = Session["result"].ToString();
                    string strAuthResultDesc = Session["errorMessage"].ToString();
                    string strCardHash = "";
                    string strCardReference = "";
                    string strCardScheme = Session["cardScheme"].ToString();
                    string strMaskedCardNumber = Session["cardPAN"].ToString();
                    DateTime? dtExpDate = (DateTime?)Session["utcdate"];

                    if (CCSuccess(PaymentMeanCreditCardProviderType.pmccpPayu,
                                 strReference,
                                 strAuthCode,
                                 strAuthResult,
                                 strAuthResultDesc,
                                 strCardHash,
                                 strCardReference,
                                 strCardScheme,
                                 strGatewayDate,
                                 strMaskedCardNumber,
                                 strTransactionId,
                                 dtExpDate.Value,
                                 model))
                    {
                        ActionResult oRet = View("Invoice", model);

                        return oRet;
                    }
                    else
                    {
                        customersRepository.FailedRecharge((decimal)Session["Payu_Config_id"],
                                    model.Email,
                                    strTransactionId,
                                    PaymentMeanRechargeStatus.Cancelled);

                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                        return View("Retailer", model);

                    }
                }
            }
            else
            {
                return RedirectToAction("CCCancel");
            }
            return View("Retailer", model);
        }
        #endregion
    }
}
