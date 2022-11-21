using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using integraMobile.Infrastructure.RedsysAPI;
using System.Threading;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class PaysafeController : BaseCCController
    {

        public PaysafeController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

        public PaysafeController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, System.Web.HttpRequestBase iRequest, System.Web.HttpServerUtilityBase iServer, System.Web.HttpSessionStateBase iSession, decimal? Config_id)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
            _request = iRequest;
            _server = iServer;
            _session = iSession;
            bAvoidHashCheck = true;
            Configuration_Id = Config_id;
        }

        /*
          http://localhost:4091/Moneris/MonerisRequest?Guid=3db29743-d641-4bf7-a23f-d9ecfdf6481f&Email=febermejo@gmail.com&Amount=100&CurrencyISOCODE=CAD&Description=fasdfasddfa&UTCDate=231200111217&ReturnURL=http://localhost:4091/Moneris/test&Hash=32423
          https://www.iparkme.com/PreProd/integraMobile/Moneris/MonerisRequest?Guid=3db29743-d641-4bf7-a23f-d9ecfdf6481f&Email=febermejo@gmail.com&Amount=100&CurrencyISOCODE=CAD&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
          https://www.iparkme.com/Dev/integraMobile/Moneris/MonerisRequest?Guid=9ff9dba3-8a3a-4db5-aa64-39dd421b80a0&Email=febermejo@gmail.com&Amount=100&CurrencyISOCODE=CAD&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
         */


        [HttpPost]
        public ActionResult PaysafeRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return PaysafeRequest(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),
                                _request["CurrencyISOCode"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],
                                _request["Hash"]);
        }

        [HttpGet]
        public ActionResult PaysafeRequest(string Guid, string Email, int? Amount, string CurrencyISOCode, string Description, string UTCDate, string Culture, string ReturnURL, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";

            try
            {
                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["paysafeGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["cardExpMonth"] = null;
                _session["cardExpYear"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["locale"] = null;


                Logger_AddLogMessage(string.Format("PaysafeRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}; culture= {7}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCode,
                                        UTCDate,
                                        Description,
                                        ReturnURL,
                                        Culture), LogLevels.logINFO);


                _session["ReturnURL"] = ReturnURL;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value <= 0) ||
                    (string.IsNullOrEmpty(CurrencyISOCode)) ||
                    (string.IsNullOrEmpty(Description)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Hash) && bAvoidHashCheck == false))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {
                    /*string sLocale = "en_US";
                    if (!string.IsNullOrEmpty(Culture) && Culture.Length >= 2)
                    {
                        if ((Culture.ToLower().Substring(0, 2)) == "fr")                        
                            sLocale = "fr_CA";                        
                    }*/

                    PAYSAFE_CONFIGURATION oConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetPaysafeConfigurationById(Configuration_Id, out oConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetPaysafeConfiguration(Guid, out oConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oConfiguration != null)
                        {
                            _session["HashSeed"] = oConfiguration.PYSCON_HASH_SEED;

                            string strCalcHash = CalculateHash(oConfiguration.PYSCON_GUID, Email, Amount.Value, CurrencyISOCode, Description, UTCDate, ReturnURL, Culture, oConfiguration.PYSCON_HASH_SEED);

                            if ((oConfiguration.PYSCON_CHECK_DATE_AND_HASH == 0) ||
                                (strCalcHash == Hash) ||
                                (bAvoidHashCheck == true))
                            {

                                DateTime dtUTC = DateTime.UtcNow; ;
                                try
                                {
                                    dtUTC = DateTime.ParseExact(UTCDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("PaysafeRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                         UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {
                                    if ((oConfiguration.PYSCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oConfiguration.PYSCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {                                        
                                        CultureInfo ci = new CultureInfo(Culture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.Properties.Resources.Culture = ci;


                                        var uri = new Uri(_request.Url.AbsoluteUri);
                                        string strURLPath = _request.Url.AbsoluteUri.Substring(0, _request.Url.AbsoluteUri.LastIndexOf("/"));

                                        if (!strURLPath.ToLower().EndsWith("/paysafe"))
                                            strURLPath = strURLPath.Substring(0, strURLPath.LastIndexOf("/")) + "/Paysafe";

                                        var sCurrencyISONum = infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(CurrencyISOCode);

                                        ViewData["email"] = Email;
                                        ViewData["paysafe_include_js_url"] = oConfiguration.PYSCON_INCLUDE_JS_URL;
                                        ViewData["paysafe_api_key"] = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(oConfiguration.PYSCON_JS_API_KEY + ":" + oConfiguration.PYSCON_JS_API_SECRET));
                                        //ViewData["paysafe_api_key"] = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("SUT-320260" + ":" + "B-qa2-0-5cdaeecd-0-302c02147425a6a4d95fe19b2b34c776fc007bdafd3973eb02146bff5950465c2d59c6517c62cad86dd1667c012d"));
                                        ViewData["paysafe_environment"] = oConfiguration.PYSCON_ENVIRONMENT;
                                        ViewData["paysafe_currency"] = CurrencyISOCode;
                                        ViewData["paysafe_amount"] = Amount;
                                        ViewData["paysafe_company_name"] = "Blinkay";
                                        //ViewData["paysafe_locale"] = sLocale;

                                        ViewData["paysafe_url_success"] = strURLPath + "/PaysafeSuccess";
                                        ViewData["paysafe_url_failure"] = strURLPath + "/PaysafeFailure";

                                        _session["email"] = Email;
                                        _session["amount"] = Amount;
                                        _session["currency"] = CurrencyISOCode;
                                        _session["utcdate"] = dtUTC;
                                        Guid = oConfiguration.PYSCON_GUID;
                                        _session["PaysafeGuid"] = Guid;
                                        _session["locale"] = Culture;
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("PaysafeRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }

                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("PaysafeRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);
                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Paysafe configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "PaysafeRequest configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "PaysafeRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;


                string strRedirectionURLLog = string.Format("PaysafeResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("PaysafeResult?r={0}", PaysafeResultCalc());
                Logger_AddLogMessage(string.Format("PaysafeRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCode,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {
                Logger_AddLogMessage(string.Format("PaysafeRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCode,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                return View();
            }

        }

        [HttpGet]
        public ActionResult PaysafeSuccess()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return PaysafeReturn("succeeded",
                                  _request["token"],
                                  _request["paymentMethod"],
                                  _request["zip"]);
        }

        [HttpGet]
        public ActionResult PaysafeFailure()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return PaysafeReturn((_request["cancel"] == "1" ? "cancel" : "error"), "", "", "");
        }

        [HttpGet]
        public ActionResult PaysafeReturn(string result, string token, string paymentMethod, string zip)
        {

            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "PaysafeResult";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";            
            DateTime? dtPaysafeDateTime = null;
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";
            string strAuthResult = "";            

            try
            {

                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                Logger_AddLogMessage(string.Format("PaysafeSuccess Begin: Result={0} ; Email={1} ; Amount={2} ; Currency={3}",
                                        result,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString()
                                        ), LogLevels.logINFO);

                string strGuid = "";

                if (_session["PaysafeGuid"] != null)
                {
                    strGuid = _session["PaysafeGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "Paysafe Guid not found";
                    errorCode = "invalid_configuration";
                }
                else
                {

                    PAYSAFE_CONFIGURATION oConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetPaysafeConfigurationById(Configuration_Id, out oConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetPaysafeConfiguration(strGuid, out oConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Paysafe configuration not found";
                        }
                        else
                        {
                            //BSRedsysPayments.BSRedsysErrorCode eErrorCode = BSRedsysPayments.BSRedsysErrorCode.ERROR;
                            errorCode = "-1";

                            if (result == "succeeded")
                            {

                                var oPaysafeConfig = new PaysafePayments.PaysafeMerchantInfo(oConfiguration.PYSCON_ACCOUNT_NUMBER, oConfiguration.PYSCON_API_KEY, oConfiguration.PYSCON_API_SECRET, oConfiguration.PYSCON_ENVIRONMENT);

                                PaysafePayments oPaysafePayments = new PaysafePayments();

                                string sCustomerId = string.Format("{0}_{1:yyyyMMddHHmmss}", _session["email"].ToString(), DateTime.UtcNow);

                                string sProfileId = "";
                                Paysafe.CustomerVault.Profile oProfile = null;

                                bool bRes = oPaysafePayments.VerifyPaymentToken(oPaysafeConfig, sCustomerId, token, zip, out sProfileId, out errorMessage);
                                if (bRes)
                                {
                                    string sLocalePaysafe = "en_US";
                                    string sLocale = _session["locale"].ToString();
                                    if (!string.IsNullOrEmpty(sLocale) && sLocale.Length >= 2)
                                    {
                                        if ((sLocale.ToLower().Substring(0, 2)) == "fr")
                                            sLocalePaysafe = "fr_CA";
                                    }

                                    bRes = oPaysafePayments.CreateProfileFromSingleUseToken(oPaysafeConfig, sCustomerId, token, sLocalePaysafe, out oProfile, out errorMessage);
                                }
                                /*if (!bRes)
                                {
                                    //bRes = oPaysafePayments.GetProfileFromId(oPaysafeConfig, sProfileId, out oProfile);
                                    bRes = oPaysafePayments.GetProfileFromMerchantCustomerId(oPaysafeConfig, sCustomerId, out oProfile, out errorMessage);
                                    if (bRes && oProfile != null)
                                    {
                                        bRes = oPaysafePayments.DeleteProfile(oPaysafeConfig, oProfile, out errorMessage);
                                        if (bRes) oProfile = null;
                                    }
                                    if (oProfile == null)
                                    {
                                        bRes = oPaysafePayments.CreateProfileFromSingleUseToken(oPaysafeConfig, sCustomerId, token, _session["locale"].ToString(), out oProfile, out errorMessage);
                                    }
                                }
                                else
                                {
                                    bRes = oPaysafePayments.GetProfileFromId(oPaysafeConfig, sProfileId, out oProfile);
                                }*/

                                if (oProfile != null)
                                {
                                    var oCards = oProfile.cards();
                                    if (oCards != null && oCards.Any())
                                    {
                                        strCardReference = oCards.First().paymentToken();
                                        strCardHash = sCustomerId;
                                        strCardScheme = "";
                                        strPAN = "***";
                                    }
                                }

                                bRes = (!string.IsNullOrEmpty(strCardHash));

                                if (bRes)
                                {
                                    bRes = oPaysafePayments.Authorize(oPaysafeConfig, strCardReference, Convert.ToInt32(_session["amount"]), zip,
                                                                      out strTransactionId, out strReference, out dtPaysafeDateTime, 
                                                                      out strExpirationDateYear, out strExpirationDateMonth, out strPAN,
                                                                      out errorMessage);
                                }

                                if (!bRes)
                                {
                                    result = "error";
                                }
                            }
                            
                            if (result == "succeeded")
                            {
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;

                                DateTime dtUTC = (DateTime)_session["utcdate"];

                                if ((oConfiguration.PYSCON_CHECK_DATE_AND_HASH == 1) &&
                                    (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oConfiguration.PYSCON_CONFIRMATION_TIME))
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("PaysafeReturn : BeginningDate={0} ; CurrentDate={1}",
                                                                       dtUTC, DateTime.UtcNow), LogLevels.logINFO);


                                }
                                else
                                {


                                    int Amount = (int)_session["amount"];
                                    customersRepository.StartRecharge(oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                              _session["email"].ToString(),
                                                              dtUTCNow,
                                                              dtNow,
                                                              Amount,
                                                              infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                              "",
                                                              strReference,
                                                              strTransactionId,
                                                              "",
                                                              dtPaysafeDateTime.Value.ToString("HHmmssddMMyy"),
                                                              strTransactionId,
                                                              PaymentMeanRechargeStatus.Committed);
                                }
                            }
                            else if (result == "cancel")
                            {
                                result = "cancel";
                                errorMessage = "Transaction cancelled By user";
                                errorCode = "transaction_cancelled";
                            }

                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Paysafe configuration not found";
                    }


                }
            }
            catch (Exception e)
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = e.Message;
            }
            finally
            {
                string sPaysafeDateTime = "";
                if (dtPaysafeDateTime.HasValue)
                    sPaysafeDateTime = dtPaysafeDateTime.Value.ToString("HHmmssddMMyy");

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;
                _session["cardReference"] = strCardReference;
                _session["cardHash"] = strCardHash;
                _session["cardScheme"] = strCardScheme;
                _session["cardPAN"] = strPAN;
                _session["cardExpMonth"] = strExpirationDateMonth;
                _session["cardExpYear"] = strExpirationDateYear;
                _session["chargeDateTime"] = sPaysafeDateTime;
                _session["reference"] = strReference;
                _session["transactionID"] = strTransactionId;
                _session["authCode"] = strAuthCode;
                _session["authResult"] = strAuthResult;
                _session["zip"] = zip;

                strRedirectionURLLog = string.Format("PaysafeResult?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardPAN={6}" +
                                                    "&cardExpMonth={7}" +
                                                    "&cardExpYear={8}" +
                                                    "&chargeDateTime={9}" +
                                                    "&reference={10}" +
                                                    "&transactionID={11}" +
                                                    "&authCode={12}" +
                                                    "&authResult={13}" +
                                                    "&zip={14}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(strCardReference),
                    _server.UrlEncode(strCardHash),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strExpirationDateMonth),
                    _server.UrlEncode(strExpirationDateYear),
                    _server.UrlEncode(sPaysafeDateTime),
                    _server.UrlEncode(strReference),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(strAuthCode),
                    _server.UrlEncode(strAuthResult),
                    _server.UrlEncode(zip));

                Logger_AddLogMessage(string.Format("PaysafeSuccess End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("PaysafeResult?r={0}", PaysafeResultCalc());

            }

            return Redirect(strRedirectionURL);
        }

        [HttpGet]
        public ActionResult PaysafeResult(string r)
        {
            if (string.IsNullOrEmpty(r))
            {
                return new HttpNotFoundResult("");
            }
            else
            {
                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                string strResultDec = DecryptCryptResult(r, _session["HashSeed"].ToString());
                ViewData["Result"] = strResultDec;
                if (_session["PayerQuantity"] != null) ViewData["PayerQuantity"] = _session["PayerQuantity"];
                if (_session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = _session["PayerCurrencyISOCode"];
                if (_session["UserBalance"] != null) ViewData["UserBalance"] = _session["UserBalance"];



                if (_session["ReturnURL"] != null && !string.IsNullOrEmpty(_session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    postData.Add("r", r);

                    RedirectWithData(_session["ReturnURL"].ToString(), postData);
                }
                else
                {

                    if (_session["PAYMENT_ORIGIN"] != null && _session["PAYMENT_ORIGIN"].ToString() == "AccountController")
                    {
                        _session["result"] = null;
                        _session["errorCode"] = null;
                        _session["errorMessage"] = null;
                        _session["cardReference"] = null;
                        _session["cardHash"] = null;
                        _session["cardScheme"] = null;
                        _session["cardPAN"] = null;
                        _session["cardExpMonth"] = null;
                        _session["cardExpYear"] = null;
                        _session["chargeDateTime"] = null;
                        _session["reference"] = null;
                        _session["transactionID"] = null;
                        _session["authCode"] = null;
                        _session["authResult"] = null;
                        _session["email"] = null;
                        _session["amount"] = null;
                        _session["currency"] = null;
                        _session["utcdate"] = null;
                        _session["PaysafeGuid"] = null;
                        _session["ResultURL"] = null;
                        _session["zip"] = null;

                        string sSuffix = string.Empty;
                        if (_session["Suffix"] != null)
                        {
                            sSuffix = _session["Suffix"].ToString();
                        }
                        return RedirectToAction("PaysafeResult" + sSuffix, "Account", new { r = r });
                    }
                    else if (_session["PAYMENT_ORIGIN"] != null && _session["PAYMENT_ORIGIN"].ToString() == "FineController")
                    {
                        return RedirectToAction("PaysafeResult", "Fine", new { r = r });
                    }

                }

                return View();
            }

        }

        private string PaysafeResultCalc()
        {

            string strRes = "";

            Dictionary<string, object> oDataDict = new Dictionary<string, object>();

            oDataDict["email"] = _session["email"];
            oDataDict["amount"] = _session["amount"];
            oDataDict["currency"] = _session["currency"];
            oDataDict["result"] = _session["result"];
            oDataDict["errorCode"] = _session["errorCode"];
            oDataDict["errorMessage"] = _session["errorMessage"];

            if (_session["AmountCurrencyIsoCode"] != null) oDataDict["PayTickets_AmountCurrencyIsoCode"] = _session["AmountCurrencyIsoCode"];
            if (_session["Total"] != null) oDataDict["PayTickets_Total"] = _session["Total"];
            if (_session["Plate"] != null) oDataDict["PayTickets_Plate"] = _session["Plate"];
            if (_session["QFEE"] != null) oDataDict["PayTickets_QFEE"] = _session["QFEE"];
            if (_session["QVAT"] != null) oDataDict["PayTickets_QVAT"] = _session["QVAT"];
            if (_session["TotalQuantity"] != null) oDataDict["PayTickets_TotalQuantity"] = _session["TotalQuantity"];

            if (_session["result"] != null && _session["result"].ToString() == "succeeded")
            {
                oDataDict["paysafe_card_reference"] = _session["cardReference"];
                oDataDict["paysafe_card_hash"] = _session["cardHash"];
                oDataDict["paysafe_card_scheme"] = _session["cardScheme"];
                oDataDict["paysafe_masked_card_number"] = _session["cardPAN"];
                oDataDict["paysafe_expires_end_month"] = _session["cardExpMonth"];
                oDataDict["paysafe_expires_end_year"] = _session["cardExpYear"];
                oDataDict["paysafe_date_time_local_fmt"] = _session["chargeDateTime"];
                oDataDict["paysafe_reference"] = _session["reference"];
                oDataDict["paysafe_transaction_id"] = _session["transactionID"];
                oDataDict["paysafe_auth_code"] = _session["authCode"];
                oDataDict["paysafe_auth_result"] = _session["authResult"];
                oDataDict["paysafe_zip"] = _session["zip"];
            }

            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("PaysafeResultCalc: {0}",
                                               PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, _session["HashSeed"].ToString());

            return strRes;


        }



        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string lang, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL + lang;

            return CalculateHash(strHashString, strHashSeed);

        }

    }
}
