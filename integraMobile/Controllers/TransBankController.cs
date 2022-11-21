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

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class TransbankController : BaseCCController
    {

        public TransbankController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
           
        }

        public TransbankController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, System.Web.HttpRequestBase iRequest, System.Web.HttpServerUtilityBase iServer, System.Web.HttpSessionStateBase iSession, decimal? Config_id)
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
          http://localhost:4091/TransBank/TransBankRequest?Guid=268bc35d-dc43-43f1-a39a-20e3139c8042&Email=febermejo@gmail.com&Amount=100&CurrencyISOCODE=CAD&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
         */

        [HttpPost]
        public ActionResult TransbankRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return TransbankRequest(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),
                                _request["CurrencyISOCODE"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["ReturnURL"],
                                _request["Hash"]);
        }

        [HttpGet]
        public ActionResult TransbankRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string Hash)
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
                _session["TransBankGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;


                Logger_AddLogMessage(string.Format("TransBankRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        ReturnURL), LogLevels.logINFO);

                _session["ReturnURL"] = ReturnURL;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value < 0) ||
                    (string.IsNullOrEmpty(CurrencyISOCODE)) ||
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


                    TRANSBANK_CONFIGURATION oTransBankConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetTransBankConfigurationById(Configuration_Id, out oTransBankConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetTransBankConfiguration(Guid, out oTransBankConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oTransBankConfiguration != null)
                        {

                            _session["HashSeed"] = oTransBankConfiguration.TRBACON_HASH_SEED;

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, ReturnURL, oTransBankConfiguration.TRBACON_HASH_SEED);

                            // MICHEL
                            oTransBankConfiguration.TRBACON_CHECK_DATE_AND_HASH = 0;

                            if ((oTransBankConfiguration.TRBACON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("TransBankRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oTransBankConfiguration.TRBACON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oTransBankConfiguration.TRBACON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        var uri = new Uri(_request.Url.AbsoluteUri);
                                        string strURLPath = _request.Url.AbsoluteUri.Substring(0, _request.Url.AbsoluteUri.LastIndexOf("/"));

                                        string sSuffix = string.Empty;
                                        if (_session["Suffix"] != null)
                                        {
                                            sSuffix = _session["Suffix"].ToString();
                                        }

                                        string urlReturn = strURLPath + "/TransBankResponse" + sSuffix;
                                        TransBankPayments.TransBankErrorCode eErrorCode;
                                        string strToken = "";
                                        string strURLRedirect="";

                                        TransBankPayments oPayments = new TransBankPayments();
                                        oPayments.TokenInitInscription(oTransBankConfiguration.TRBACON_ENVIRONMENT,
                                                                    oTransBankConfiguration.TRBACON_COMMERCECODE,
                                                                    oTransBankConfiguration.TRBACON_PUBLICCERT_FILE,
                                                                    oTransBankConfiguration.TRBACON_WEBPAYCERT_FILE,
                                                                    oTransBankConfiguration.TRBACON_PASSWORD,
                                                                    Email,
                                                                    urlReturn,
                                                                    out eErrorCode,
                                                                    out errorMessage,
                                                                    out strToken,
                                                                    out strURLRedirect);
                                                                             
                                        if ((!string.IsNullOrEmpty(strToken)) && (!string.IsNullOrEmpty(strURLRedirect)))
                                        {                                      
                                            ViewData["transbank_form_url"] = strURLRedirect;
                                            ViewData["token"] = strToken;

                                            _session["email"] = Email;
                                            _session["amount"] = Amount;
                                            _session["currency"] = CurrencyISOCODE;
                                            _session["utcdate"] = dtUTC;
                                            _session["TransBankGuid"] = Guid;
                                        }
                                        else
                                        {
                                            result = "error";
                                            errorMessage = "Web Pay is not available";
                                            errorCode = "invalid_token";
                                            Logger_AddLogMessage(string.Format("TransBankRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                               UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                        }


                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("TransBankRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("TransBankRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "TransBank configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "TransBank configuration not found";
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "TransBankRequest Method Exception";
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

                string strRedirectionURLLog = string.Format("TransBankResult" + sSuffix + "?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("TransBankResult" + sSuffix + "?r={0}", TransbankResultCalc());
                Logger_AddLogMessage(string.Format("TransBankRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {
                Logger_AddLogMessage(string.Format("TransBankRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
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
        public ActionResult TransbankResponse()
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

            string strRedirectionURL = "TransBankResult" + sSuffix;
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strTransBankDateTime = "";
            string strPAN = "";
            string strAuthCode = "";


            try
            {
                string strToken = _request["TBK_TOKEN"];


                Logger_AddLogMessage(string.Format("TransBankSuccess Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4}; Token={5}",
                                        "succeeded",
                                        strCardReference,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strToken
                                        ), LogLevels.logINFO);

                string strGuid = "";

                if (_session["TransBankGuid"] != null)
                {
                    strGuid = _session["TransBankGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "TransBank Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    TRANSBANK_CONFIGURATION oTransBankConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetTransBankConfigurationById(Configuration_Id, out oTransBankConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetTransBankConfiguration(strGuid, out oTransBankConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oTransBankConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "TransBank configuration not found";
                        }
                        else
                        {

                            TransBankPayments.TransBankErrorCode eErrorCode = TransBankPayments.TransBankErrorCode.InternalError;

                            string strTokenUser = "";
                            strAuthCode = "";

                            TransBankPayments oPayments = new TransBankPayments();
                            oPayments.TokenEndInscription(oTransBankConfiguration.TRBACON_ENVIRONMENT,
                                                        oTransBankConfiguration.TRBACON_COMMERCECODE,
                                                        oTransBankConfiguration.TRBACON_PUBLICCERT_FILE,
                                                        oTransBankConfiguration.TRBACON_WEBPAYCERT_FILE,
                                                        oTransBankConfiguration.TRBACON_PASSWORD,
                                                        strToken,
                                                        out eErrorCode,
                                                        out errorMessage,
                                                        out strTokenUser,
                                                        out strPAN,
                                                        out strAuthCode,
                                                        out strCardScheme);


                            if (TransBankPayments.IsError(eErrorCode))
                            {
                                result = "error";
                                errorCode = "null_token";

                                Logger_AddLogMessage(string.Format("TransBankSuccess:TokenEndInscription : errorCode={0} ; errorMessage={1}",
                                            errorCode, errorMessage), LogLevels.logINFO);

                                if (string.IsNullOrEmpty(errorMessage))
                                {
                                    errorMessage = TransBankPayments.ErrorMessageDict[eErrorCode];
                                }
                            }
                            else
                            {

                                DateTime dtUTC = (DateTime)_session["utcdate"];


                                if ((oTransBankConfiguration.TRBACON_CHECK_DATE_AND_HASH == 1) &&
                                    (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oTransBankConfiguration.TRBACON_CONFIRMATION_TIME))
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("TransBankResponse : BeginningDate={0} ; CurrentDate={1}",
                                    dtUTC, DateTime.UtcNow), LogLevels.logINFO);


                                }
                                else
                                {

                                    int iAmount = (int)_session["amount"];

                                    if (iAmount > 0)
                                    {

                                        string strBuyOrder = TransBankPayments.UserReference();

                                        oPayments.AutomaticTransaction(oTransBankConfiguration.TRBACON_ENVIRONMENT,
                                                oTransBankConfiguration.TRBACON_COMMERCECODE,
                                                oTransBankConfiguration.TRBACON_PUBLICCERT_FILE,
                                                oTransBankConfiguration.TRBACON_WEBPAYCERT_FILE,
                                                oTransBankConfiguration.TRBACON_PASSWORD,
                                                _session["email"].ToString(),
                                                strTokenUser,
                                                strBuyOrder,
                                                iAmount.ToString(),
                                                out eErrorCode,
                                                out errorMessage,
                                                out strTransactionId,
                                                out strAuthCode,
                                                out strTransBankDateTime);

                                        if (TransBankPayments.IsError(eErrorCode))
                                        {
                                            result = "error";
                                            errorCode = "transaction_failed";
                                        }
                                        else
                                        {
                                            DateTime dtNow = DateTime.Now;
                                            DateTime dtUTCNow = DateTime.UtcNow;
                                            customersRepository.StartRecharge(oTransBankConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                                                      _session["email"].ToString(),
                                                                                                      dtUTCNow,
                                                                                                      dtNow,
                                                                                                      iAmount,
                                                                                                      infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                                                                      "",
                                                                                                      strBuyOrder,
                                                                                                      strTransactionId,
                                                                                                      "",
                                                                                                      strTransBankDateTime,
                                                                                                      strAuthCode,
                                                                                                      PaymentMeanRechargeStatus.Committed);    
                                            

                                            strReference = strBuyOrder;
                                            result = "succeeded";
                                            errorCode = eErrorCode.ToString();

                                        }

                                    }
                                    else
                                    {
                                        strAuthCode = "";
                                        result = "succeeded";
                                        errorCode = eErrorCode.ToString();
                                    }


                                    strCardReference = strTokenUser;
                                    strCardHash = strToken;


                                    /*
                                    string strTransaction2 = "";
                                    oPayments.RefundTransaction(oTransBankConfiguration.TRBACON_ENVIRONMENT,
                                                                                  oTransBankConfiguration.TRBACON_COMMERCECODE,
                                                                                  oTransBankConfiguration.TRBACON_PUBLICCERT_FILE,
                                                                                  oTransBankConfiguration.TRBACON_WEBPAYCERT_FILE,
                                                                                  oTransBankConfiguration.TRBACON_PASSWORD,
                                                                                  strReference,
                                                                                  out eErrorCode,
                                                                                  out errorMessage,
                                                                                  out strTransaction2);

                                    oPayments.DeleteToken(oTransBankConfiguration.TRBACON_ENVIRONMENT,
                                                       oTransBankConfiguration.TRBACON_COMMERCECODE,
                                                       oTransBankConfiguration.TRBACON_PUBLICCERT_FILE,
                                                       oTransBankConfiguration.TRBACON_WEBPAYCERT_FILE,
                                                       oTransBankConfiguration.TRBACON_PASSWORD,
                                                        _session["email"].ToString(),
                                                        strTokenUser,
                                                       out eErrorCode,
                                                       out errorMessage);
                                  
                                    */

                                }
                            }

                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "TransBank configuration not found";
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
                _session["chargeDateTime"] = strTransBankDateTime;
                _session["reference"] = strReference;
                _session["transactionID"] = strTransactionId;
                _session["authCode"] = strAuthCode;



                strRedirectionURLLog = string.Format("TransBankResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardPAN={6}" +
                                                    "&chargeDateTime={7}" +
                                                    "&reference={8}" +
                                                    "&transactionID={9}" +
                                                    "&authCode={10}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(strCardReference),
                    _server.UrlEncode(strCardHash),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strTransBankDateTime),
                    _server.UrlEncode(strReference),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(strAuthCode));

                Logger_AddLogMessage(string.Format("TransBankSuccess End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("TransBankResult" + sSuffix + "?r={0}", TransbankResultCalc());

            }

            return Redirect(strRedirectionURL);
        }

        [HttpGet]
        public ActionResult TransbankResult(string r)
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
                    _session["TransBankGuid"] = null;
                    _session["ResultURL"] = null;
                }

                return View();
            }

        }




        private string TransbankResultCalc()
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
                oDataDict["transbank_card_reference"] = _session["cardReference"];
                oDataDict["transbank_card_hash"] = _session["cardHash"];
                oDataDict["transbank_card_scheme"] = _session["cardScheme"];
                oDataDict["transbank_masked_card_number"] = _session["cardPAN"];
                oDataDict["transbank_date_time_local_fmt"] = _session["chargeDateTime"];
                oDataDict["transbank_reference"] = _session["reference"];
                oDataDict["transbank_transaction_id"] = _session["transactionID"];
                oDataDict["transbank_auth_code"] = _session["authCode"];
            }

            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("TransBankResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, _session["HashSeed"].ToString());

            return strRes;


        }

        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL;

            return CalculateHash(strHashString, strHashSeed);

        }

    }
}
