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
using System.Threading;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class PayuController : BaseCCController
    {
           
        public PayuController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;            
           
        }

        public PayuController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, System.Web.HttpRequestBase iRequest, System.Web.HttpServerUtilityBase iServer, System.Web.HttpSessionStateBase iSession, decimal? Config_id)
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
          http://localhost:4091/Payu/PayuRequest?Guid=268bc35d-dc43-43f1-a39a-20e3139c8042&Email=febermejo@gmail.com&Amount=10000&CurrencyISOCODE=MXN&Description=fasdfasddfa&UTCDate=231200111217&Culture=ES&Hash=32423
        */


        [HttpPost]
        public ActionResult PayuRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return PayuRequest(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),
                                _request["CurrencyISOCODE"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],
                                _request["Hash"]);
        }


        [HttpGet]
        public ActionResult PayuRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture, string ReturnURL,  string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";

            try
            {

                if (_session == null)
                {
                    _server = Server;
                    _session = Session;
                    _request = Request;
                }

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["PayuGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["description"] = null;
                _session["lang"] = null;
                _session["name"] = null;
                _session["DocumentId"] = null;
                _session["ReturnURL"] = null;


                Logger_AddLogMessage(string.Format("PayuRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; Culture={6}; ReturnURL={7}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        Culture,
                                        ReturnURL), LogLevels.logINFO);

                _session["ReturnURL"] = ReturnURL;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value < 0) ||
                    (string.IsNullOrEmpty(CurrencyISOCODE)) ||
                    (string.IsNullOrEmpty(Description)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Culture)) ||
                    (string.IsNullOrEmpty(Hash) && bAvoidHashCheck == false))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {
                    PAYU_CONFIGURATION oPayuConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null) 
                    {
                        Configuration_OK = infraestructureRepository.GetPayuConfigurationById(Configuration_Id, out oPayuConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetPayuConfiguration(Guid, out oPayuConfiguration);    
                    }

                    if (Configuration_OK)
                    {
                        if (oPayuConfiguration != null)
                        {
                            _session["HashSeed"] = oPayuConfiguration.PAYUCON_HASH_SEED;

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, Culture, ReturnURL, oPayuConfiguration.PAYUCON_HASH_SEED);

                            if ((oPayuConfiguration.PAYUCON_CHECK_DATE_AND_HASH == 0) ||
                                (strCalcHash == Hash) ||
                                (bAvoidHashCheck == true))
                            {
                                DateTime dtUTC = DateTime.Now; ;
                                try
                                {
                                    dtUTC = DateTime.ParseExact(UTCDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("PayuRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oPayuConfiguration.PAYUCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oPayuConfiguration.PAYUCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        string strLang= ((Culture.ToLower() ?? "").Length >= 2) ? Culture.Substring(0, 2).ToLower() : "es";
                                        string strCulture = "es-ES";
                                        if (PayuPayments.Language(strLang) == "en")
                                        {
                                            strCulture = "en-US";
                                        }

                                        CultureInfo ci = new CultureInfo(strCulture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.Properties.Resources.Culture = ci;

                                        ViewData["api_url"] = oPayuConfiguration.PAYUCON_TOKEN_URL; 
                                        ViewData["public_key"] = oPayuConfiguration.PAYUCON_PUBLIC_KEY;
                                        ViewData["account_id"] = oPayuConfiguration.PAYUCON_ACCOUNT_ID;
                                        ViewData["payer_id"] = Email + "_" + DateTime.UtcNow.ToString("HHmmssddMMyy");
                                        ViewData["lang"] = PayuPayments.Language(strLang);
                                        ViewData["month"] = dtUTC.Month;
                                        ViewData["year"] = dtUTC.Year;
                                        var uri = new Uri(_request.Url.AbsoluteUri);
                                        string strURLPath = _request.Url.AbsoluteUri.Substring(0, _request.Url.AbsoluteUri.LastIndexOf("/"));
                                        string sSuffix = string.Empty;
                                        if (_session["Suffix"] != null) 
                                        {
                                            sSuffix = _session["Suffix"].ToString();
                                        }
                                        string urlReturn = strURLPath + "/PayuResponse" + sSuffix;
                                        ViewData["response_url"] = urlReturn;
                                        string strCheck = DateTime.UtcNow.ToString("HHmmssddMMyy") + "======" + strURLPath;
                                        ViewData["check"] = CalculateCryptResult(strCheck, _session["HashSeed"].ToString());
                                        _session["utcdate"] = dtUTC;
                                        _session["PayuGuid"] = Guid;
                                        _session["email"] = Email;
                                        _session["amount"] = Amount;
                                        _session["currency"] = CurrencyISOCODE;
                                        _session["description"] = Description;
                                        _session["lang"] = strLang;                                        

                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("PayuRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("PayuRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Payu configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Payu configuration not found";
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "PayuRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

                string sSuffix = string.Empty;
                if (_session["Suffix"] != null)
                {
                    sSuffix = _session["Suffix"].ToString();
                }

                string strRedirectionURLLog = string.Format("PayuResult" + sSuffix + "?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("PayuResult" + sSuffix + "?r={0}", PayuResultCalc());
                Logger_AddLogMessage(string.Format("PayuRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {
                Logger_AddLogMessage(string.Format("PayuRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                return View();
            }

        }



        [HttpPost]
        public ActionResult PayuResponse()
        {
                string result = "";
                string errorMessage = "";
                string errorCode = "";
                string strRedirectionURLLog = "";

                if (_session == null)
                {
                    _server = Server;
                    _session = Session;
                    _request = Request;
                }

                string sSuffix = string.Empty;
                if (_session["Suffix"] != null)
                {
                    sSuffix = _session["Suffix"].ToString();
                }

            string strRedirectionURL = "PayuResult" + sSuffix;
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strPayuDateTime = "";
            string strPAN = "";
            string strAuthCode = "";
            string strName = "";
            string strIdNumber = "";
            string strSecurityCode = "";
           
            try
            {
               

                string strToken = _request["token"];
                string strPayerID = _request["payer_id"];
                string strCheckCrypt = _request["check"];
                string strMethod = _request["method"];
                strName = _request["name"];
                strIdNumber = _request["document"];
                strPAN = _request["pan"];
                string strCancel = _request["cancel"];
                strSecurityCode = _request["cvc"];

                Logger_AddLogMessage(string.Format("PayResponse Begin: Email={0} ; Amount={1} ; Currency={2}; Token={3}; Check={4}; PayerID ={5}; Method={6}; Cancel={7}; Name={8}; Document={9}; PAN={10}; CVC={11}",
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strToken,
                                        strCheckCrypt,
                                        strPayerID,
                                        strMethod,
                                        strCancel,
                                        strName,
                                        strIdNumber,
                                        strPAN,
                                        strSecurityCode),
                                        LogLevels.logINFO);

                string strGuid = "";

                if (_session["PayuGuid"] != null)
                {
                    strGuid = _session["PayuGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "Payu Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    PAYU_CONFIGURATION oPayuConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetPayuConfigurationById(Configuration_Id, out oPayuConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetPayuConfiguration(strGuid, out oPayuConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oPayuConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Payu configuration not found";
                        }
                        else
                        {

                            string strCheck = DecryptCryptResult(strCheckCrypt, _session["HashSeed"].ToString());

                            string[] strParams = strCheck.Split(new string[]{"======"},StringSplitOptions.None);

                            if (strParams.Count() != 2)
                            {
                                result = "error";
                                errorCode = "security_check_not_passed";
                                errorMessage = "Bad Check Value";
                            }
                            else
                            {
                                DateTime? dtUTCInt = null;
                                try
                                {
                                    dtUTCInt = DateTime.ParseExact(strParams[0], "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";

                                }


                                if ((!dtUTCInt.HasValue) || ((oPayuConfiguration.PAYUCON_CHECK_DATE_AND_HASH != 0) &&
                                    (Math.Abs((DateTime.UtcNow - dtUTCInt.Value).TotalSeconds) > oPayuConfiguration.PAYUCON_CONFIRMATION_TIME)))
                                {

                                    result = "error";
                                    errorMessage = "Expired operation";
                                    errorCode = "security_check_not_passed";

                                }
                                else
                                {

                                    if (!_request.UrlReferrer.AbsoluteUri.StartsWith(strParams[1]))
                                    {
                                        result = "error";
                                        errorCode = "security_check_not_passed";
                                        errorMessage = "Bad URL Referral";
                                    }
                                    else
                                    {


                                        /*DateTime dtUTC = (DateTime)_session["utcdate"];


                                        if ((oPayuConfiguration.PAYUCON_CHECK_DATE_AND_HASH == 1) &&
                                            (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oPayuConfiguration.PAYUCON_CONFIRMATION_TIME))
                                        {
                                            result = "error";
                                            errorMessage = "Invalid DateTime";
                                            errorCode = "invalid_datetime";
                                            Logger_AddLogMessage(string.Format("PayuResponse : BeginningDate={0} ; CurrentDate={1}",
                                            dtUTC, DateTime.UtcNow), LogLevels.logINFO);


                                        }
                                        else
                                        {*/

                                            int iCancel = 1;

                                            try
                                            {
                                                if (!string.IsNullOrEmpty(strCancel))
                                                {
                                                    iCancel = Convert.ToInt32(strCancel);
                                                }

                                            }
                                            catch
                                            {

                                            }

                                            if (iCancel == 1)
                                            {
                                                result = "error";
                                                errorMessage = "Transaction cancelled By user";
                                                errorCode = "transaction_cancelled";

                                            }
                                            else
                                            {


                                                if (string.IsNullOrEmpty(strToken) || string.IsNullOrEmpty(strPayerID))
                                                {
                                                    result = "error";
                                                    errorCode = "null_token";
                                                    errorMessage = "Null token";
                                                }
                                                else
                                                {


                                                    PayuPayments.PayuErrorCode eErrorCode = PayuPayments.PayuErrorCode.InternalError;

                                                    PayuPayments oPayments = new PayuPayments();




                                                    int iAmount = (int)_session["amount"];

                                                    if (iAmount > 0)
                                                    {
                                                        strPayuDateTime = DateTime.UtcNow.ToString("HHmmssddMMyy");
                                                        DateTime? dtTransaction = null;


                                                        decimal dAmount = (iAmount / Convert.ToDecimal(infraestructureRepository.GetCurrencyDivisorFromIsoCode(_session["currency"].ToString())));

                                                        oPayments.AutomaticTransaction(oPayuConfiguration.PAYUCON_API_URL,
                                                                oPayuConfiguration.PAYUCON_API_KEY,
                                                                oPayuConfiguration.PAYUCON_API_LOGIN,
                                                                oPayuConfiguration.PAYUCON_ACCOUNT_ID,
                                                                oPayuConfiguration.PAYUCON_MERCHANT_ID,
                                                                oPayuConfiguration.PAYUCON_SERVICE_TIMEOUT,
                                                                oPayuConfiguration.PAYUCON_COUNTRY,
                                                                oPayuConfiguration.PAYUCON_IS_TEST != 1 ? false : true,
                                                                strToken,
                                                                strPayerID,
                                                                PayuPayments.Language(_session["lang"].ToString()),
                                                                _session["email"].ToString(),
                                                                dAmount,
                                                                _session["currency"].ToString(),
                                                                _session["description"].ToString(),
                                                                _request.UserAgent,
                                                                strMethod,
                                                                strName,
                                                                strIdNumber,
                                                                strSecurityCode,
                                                                out eErrorCode,
                                                                out errorMessage,
                                                                out strTransactionId,
                                                                out strReference,
                                                                out strAuthCode,
                                                                out dtTransaction);
                                                        
                                                        if (PayuPayments.IsError(eErrorCode))
                                                        {
                                                            result = "error";
                                                            errorCode = "transaction_failed";

                                                            PayuPayments.PayuErrorCode eErrorCode2 = PayuPayments.PayuErrorCode.InternalError;
                                                            string errorMessage2 = "";

                                                            oPayments.DeleteToken(oPayuConfiguration.PAYUCON_API_URL,
                                                                        oPayuConfiguration.PAYUCON_API_KEY,
                                                                        oPayuConfiguration.PAYUCON_API_LOGIN,
                                                                        oPayuConfiguration.PAYUCON_SERVICE_TIMEOUT,
                                                                        strToken,
                                                                        strPayerID,
                                                                        PayuPayments.Language(_session["lang"].ToString()),
                                                                        out eErrorCode2,
                                                                        out errorMessage2);
                                                        }
                                                        else
                                                        {

                                                            strPayuDateTime = dtTransaction.Value.ToString("HHmmssddMMyy");
                                                            /*string strTransaction2 = "";
                                                            oPayments.RefundTransaction(oPayuConfiguration.PAYUCON_API_URL,
                                                                        oPayuConfiguration.PAYUCON_API_KEY,
                                                                        oPayuConfiguration.PAYUCON_API_LOGIN,
                                                                        oPayuConfiguration.PAYUCON_SERVICE_TIMEOUT,
                                                                        strAuthCode,
                                                                        strTransactionId,
                                                                        "Test",
                                                                        PayuPayments.Language(_session["lang"].ToString()),                                                                                           
                                                                        out eErrorCode,
                                                                        out errorMessage,
                                                                        out strTransaction2);*/


                                                            DateTime dtNow = DateTime.Now;
                                                            DateTime dtUTCNow = DateTime.UtcNow;
                                                            customersRepository.StartRecharge(oPayuConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                                                                    _session["email"].ToString(),
                                                                                                                    dtUTCNow,
                                                                                                                    dtNow,
                                                                                                                    iAmount,
                                                                                                                    infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                                                                                    "",
                                                                                                                    strReference,
                                                                                                                    strTransactionId,
                                                                                                                    "",
                                                                                                                    strPayuDateTime,
                                                                                                                    strAuthCode,
                                                                                                                    PaymentMeanRechargeStatus.Committed);


                                                            result = "succeeded";
                                                            errorCode = eErrorCode.ToString();
                                                            strCardReference = strToken;
                                                            strCardHash = strPayerID;
                                                            strCardScheme = strMethod;

                                                        }

                                                    }
                                                    else
                                                    {
                                                        strAuthCode = "";
                                                        result = "succeeded";
                                                        errorCode = eErrorCode.ToString();
                                                    }
                                                }

                                            }
                                        }
                                    //}
                                }
                            }
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Payu configuration not found";
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
                _session["result"] = result; ;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;
                _session["cardReference"] = strCardReference;
                _session["cardHash"] = strCardHash;
                _session["cardScheme"] = strCardScheme;
                _session["cardPAN"] = strPAN;
                _session["chargeDateTime"] = strPayuDateTime;
                _session["reference"] = strReference;
                _session["transactionID"] = strTransactionId;
                _session["authCode"] = strAuthCode;
                _session["name"] = strName;
                _session["DocumentId"] = strIdNumber;
                _session["cvc"] = strSecurityCode;

                strRedirectionURLLog = string.Format("PayuResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardPAN={6}" +
                                                    "&chargeDateTime={7}" +
                                                    "&reference={8}" +
                                                    "&transactionID={9}" +
                                                    "&authCode={10}" +
                                                    "&name={11}" +
                                                    "&cvc={12}",
                                                    "&DocumentId={13}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(strCardReference),
                    _server.UrlEncode(strCardHash),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strPayuDateTime),
                    _server.UrlEncode(strReference),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(strAuthCode),
                    _server.UrlEncode(strName),
                    _server.UrlEncode(strSecurityCode),
                    _server.UrlEncode(strIdNumber));


                Logger_AddLogMessage(string.Format("PayResponse End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("PayuResult" + sSuffix + "?r={0}", PayuResultCalc());

            }

            return Redirect(strRedirectionURL);
        }




        [HttpGet]
        public ActionResult PayuResult(string r)
        {
            if (string.IsNullOrEmpty(r))
            {
                return new HttpNotFoundResult("");
            }
            else
            {
                if (_session == null)
                {
                    _server = Server;
                    _session = Session;
                    _request = Request;
                }

                string strResultDec = DecryptCryptResult(r, _session["HashSeed"].ToString());
                ViewData["Result"] = strResultDec;
                if (_session["PayerQuantity"] != null) ViewData["PayerQuantity"] = _session["PayerQuantity"];
                if (_session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = _session["PayerCurrencyISOCode"];
                if (_session["UserBalance"] != null) ViewData["UserBalance"] = _session["UserBalance"];

                if (_session["ReturnURL"] != null && !string.IsNullOrEmpty(_session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    postData.Add("r", r);
                    _session["cvc"] = null;

                    RedirectWithData(_session["ReturnURL"].ToString(), postData);
                    
                }
                else
                {


                    _session["result"] = null;
                    _session["errorCode"] = null;
                    _session["errorMessage"] = null;
                    _session["cardReference"] = null;
                    _session["cardHash"] = null;
                    _session["cardScheme"] = null;
                    _session["cardPAN"] = null;
                    _session["chargeDateTime"] = null;
                    _session["reference"] = null;
                    _session["transactionID"] = null;
                    _session["authCode"] = null;
                    _session["email"] = null;
                    _session["amount"] = null;
                    _session["currency"] = null;
                    _session["utcdate"] = null;
                    _session["PayuGuid"] = null;
                    _session["description"] = null;
                    _session["lang"] = null;
                    _session["name"] = null;
                    _session["DocumentId"] = null;
                    _session["ResultURL"] = null;
                    
                }

                return View();
            }

        }


        [HttpPost]
        public ActionResult PayuCallback()
        {

            if (_session == null)
            {
                _server = Server;
                _session = Session;
                _request = Request;
            }

            try
            {
                string strEmailBuyer = _request["email_buyer"];
                string strTransactionId = Request["transaction_id"];
                string strNewValue = Request["value"];
                string strCurrency = Request["currency"];
                string strResponseCodePol = _request["response_code_pol"];
                string strStatePol = _request["state_pol"];
                string strResponseMessagePol = _request["response_message_pol"];
                string strPaymentMethodType = _request["payment_method_type"];
                string strMerchantId = Request["merchant_id"];
                string strReferenceSale = Request["reference_sale"];
                string strSign = Request["sign"];



                Logger_AddLogMessage(string.Format("PayuCallback : email_buyer={0} ; transaction_id={1} ; value={2}; currency={3}; response_message_pol={4}; response_code_pol={5}; state_pol ={6}; payment_method_type={7}; merchant_id={8}; reference_sale={9}; sign={10}",
                                       strEmailBuyer,
                                       strTransactionId,
                                       strNewValue,
                                       strCurrency,
                                       strResponseMessagePol,
                                       strResponseCodePol,
                                       strStatePol,
                                       strPaymentMethodType,
                                       strMerchantId,
                                       strReferenceSale,
                                       strSign),
                                       LogLevels.logINFO);
                int iStatePol = int.Parse(strStatePol, CultureInfo.InvariantCulture);


                CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = null;
                if (customersRepository.GetRechargeByTransactionId(strTransactionId, PaymentMeanCreditCardProviderType.pmccpPayu, out oRecharge))
                {
                    switch ((PaymentMeanRechargeType)oRecharge.CUSPMR_TYPE)
                    {
                        case PaymentMeanRechargeType.Oxxo:
                            {
                                CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oConfig = oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG;
                                string strAPIKey = oConfig.PAYU_CONFIGURATION.PAYUCON_API_KEY;
                                PayuPayments oPayments = new PayuPayments();

                                decimal dAmount = decimal.Parse(strNewValue, CultureInfo.InvariantCulture);

                                string strCalcSign = oPayments.CalculateMD5Hash(strAPIKey + "~" + strMerchantId + "~" + strReferenceSale + "~" +
                                                     dAmount.ToString("#.0#", CultureInfo.InvariantCulture) + "~" + strCurrency + "~" + strStatePol).ToLower();

                                if ((strCalcSign != strSign)&&(oConfig.PAYU_CONFIGURATION.PAYUCON_CHECK_DATE_AND_HASH!=0))
                                {
                                    Logger_AddLogMessage(string.Format("PayuCallback: Error Signs differs -->  sign={0} ; calcSign={1}",
                                                           strSign,
                                                           strCalcSign),
                                                           LogLevels.logERROR);

                                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                                }


                                switch ((PayuPayments.PayuConfStatePol)iStatePol)
                                {

                                    case PayuPayments.PayuConfStatePol.Declined:
                                    case PayuPayments.PayuConfStatePol.Expired:
                                        {
                                            if (customersRepository.CancelOxxoTransaction(ref oRecharge))
                                            {
                                                return new HttpStatusCodeResult(HttpStatusCode.OK);
                                            }
                                            else
                                            {
                                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                                            }
                                        }
                                    case PayuPayments.PayuConfStatePol.Approved:
                                        {

                                            if (oRecharge.CUSPMR_TRANS_STATUS == (int)PaymentMeanRechargeStatus.Waiting_External_Approval)
                                            {
                                                USER oUser = oRecharge.USER;

                                                string culture = oUser.USR_CULTURE_LANG;
                                                if (culture == "es-MX")
                                                {
                                                    culture = "es-ES";
                                                }

                                                if (customersRepository.CommitOxxoTransaction(ref oRecharge))
                                                {
                                                    oUser = oRecharge.USER;

                                                    CultureInfo ci = new CultureInfo(culture);
                                                    Thread.CurrentThread.CurrentUICulture = ci;
                                                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                                    integraMobile.Properties.Resources.Culture = ci;


                                                    int iQuantity = oRecharge.CUSPMR_AMOUNT;
                                                    decimal dPercVAT1 = oRecharge.CUSPMR_PERC_VAT1 ?? 0;
                                                    decimal dPercVAT2 = oRecharge.CUSPMR_PERC_VAT2 ?? 0;
                                                    decimal dPercFEE = oRecharge.CUSPMR_PERC_FEE ?? 0;
                                                    int iPercFEETopped = (int)(oRecharge.CUSPMR_PERC_FEE_TOPPED ?? 0);
                                                    int iFixedFEE = (int)(oRecharge.CUSPMR_FIXED_FEE ?? 0);

                                                    int iPartialVAT1;
                                                    int iPartialPercFEE;
                                                    int iPartialFixedFEE;
                                                    int iPartialPercFEEVAT;
                                                    int iPartialFixedFEEVAT;

                                                    int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                                                    int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                                                    if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
                                                    iQFEE += iFixedFEE;
                                                    int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                                                    int iQSubTotal = iQuantity + iQFEE;

                                                    int iLayout = 0;
                                                    if (iQFEE != 0 || iQVAT != 0)
                                                    {
                                                        OPERATOR oOperator = customersRepository.GetDefaultOperator();
                                                        if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                                                    }


                                                    string sLayoutSubtotal = "";
                                                    string sLayoutTotal = "";

                                                    string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID));

                                                    if (iLayout == 2)
                                                    {
                                                        sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutSubtotal"),
                                                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                                                        (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                                        (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode));
                                                    }
                                                    else if (iLayout == 1)
                                                    {
                                                        sLayoutTotal = string.Format(ResourceExtension.GetLiteral("Email_LayoutTotal"),
                                                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                                                     (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                                     (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode));
                                                    }

                                                    string strRechargeEmailSubject = ResourceExtension.GetLiteral("ConfirmNoAutomaticRecharge_EmailHeader");
                                                    /*
                                                        ID: {0}<br>
                                                     *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                                                     *  Cantidad Recargada: {2} 
                                                     */
                                                    string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral("ConfirmRecharge_EmailBody"),
                                                        oRecharge.CUSPMR_ID,
                                                        oRecharge.CUSPMR_DATE,
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                                      infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}", Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                                            infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),

                                                        "", "",
                                                        sLayoutSubtotal, sLayoutTotal,
                                                        GetEmailFooter(ref oUser));


                                                    SendEmail(ref oUser, strRechargeEmailSubject, strRechargeEmailBody);

                                                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                                                }
                                                else
                                                {
                                                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                                                }


                                                
                                            }
                                            else if (oRecharge.CUSPMR_TRANS_STATUS == (int)PaymentMeanRechargeStatus.Committed)
                                            {
                                                return new HttpStatusCodeResult(HttpStatusCode.OK);
                                            }
                                            else
                                            {
                                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                                            }
                                        }
                                    default:
                                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                                }
                            }
                        default:
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);  
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);                      
                }
                
            }
            catch (Exception e)
            {
                Logger_AddLogException(e,string.Format("PayuCallback Exception"), LogLevels.logERROR);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);  
            }                       
        }

        private string GetEmailFooter(ref USER oUser)
        {
            string strFooter = "";

            try
            {
                strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}_{1}", infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)), oUser.COUNTRy.COU_CODE));
                if (string.IsNullOrEmpty(strFooter))
                {
                    strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}", infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))));
                }
            }
            catch
            {

            }

            return strFooter;
        }

        private bool SendEmail(ref USER oUser, string strEmailSubject, string strEmailBody)
        {
            bool bRes = true;
            try
            {
                decimal dSourceApp = oUser.USR_LAST_SOAPP_ID.Value;

                long lSenderId = infraestructureRepository.SendEmailTo(oUser.USR_EMAIL, strEmailSubject, strEmailBody, dSourceApp);

                if (lSenderId > 0)
                {
                    customersRepository.InsertUserEmail(ref oUser, oUser.USR_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                }

            }
            catch
            {
                bRes = false;
            }

            return bRes;
        }

        private string PayuResultCalc()
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
            if (_session["PayerQuantity"] != null) oDataDict["PayerQuantity"] = _session["PayerQuantity"];
            if (_session["PayerCurrencyISOCode"] != null) oDataDict["PayerCurrencyISOCode"] = _session["PayerCurrencyISOCode"];
            if (_session["UserBalance"] != null) oDataDict["UserBalance"] = _session["UserBalance"];

            if (_session["result"] != null && _session["result"].ToString() == "succeeded")
            {
                oDataDict["payu_card_reference"] = _session["cardReference"];
                oDataDict["payu_card_hash"] = _session["cardHash"];
                oDataDict["payu_card_scheme"] = _session["cardScheme"];
                oDataDict["payu_masked_card_number"] = _session["cardPAN"];
                oDataDict["payu_date_time_local_fmt"] = _session["chargeDateTime"];
                oDataDict["payu_reference"] = _session["reference"];
                oDataDict["payu_transaction_id"] = _session["transactionID"];
                oDataDict["payu_auth_code"] = _session["authCode"];
                oDataDict["payu_name"] = _session["name"];
                oDataDict["payu_document_id"] = ""; ;
                oDataDict["payu_cvc"] = _session["cvc"];
            }

            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("PayuResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, _session["HashSeed"].ToString());

            return strRes;


        }

        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture,string ReturnURL, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + Culture + ReturnURL;

            return CalculateHash(strHashString, strHashSeed);

        }

    }
}
