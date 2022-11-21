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
using System.Configuration;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class PayPalController : BaseCCController
    {


        private bool bMaintainReturnURLS=false;

        public PayPalController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;            
           
        }

        public PayPalController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, System.Web.HttpRequestBase iRequest, System.Web.HttpServerUtilityBase iServer, System.Web.HttpSessionStateBase iSession, decimal? Config_id, System.Web.HttpResponseBase iResponse)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;            
            _request = iRequest;
            _server = iServer;
            _session = iSession;
            _response = iResponse;
            bAvoidHashCheck = true;
            bMaintainReturnURLS = true;
            Configuration_Id = Config_id;
            
        }

        /*
          http://localhost:4091/PayPal/PaypalRequest?Guid=84350577-cadc-49d3-a0b4-fdb93f29a53c&Email=sb-lltnw6251325@personal.example.com&Amount=1000&CurrencyISOCODE=CAD&Description=fasdfasddfa&UTCDate=231200111217&Culture=es-ES&QFEE=0&QVAT=0&Total=1000&Hash=32423
        */


        [HttpPost]
        public ActionResult PayPalRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;
            if (_response == null) _response = Response;

            return PayPalRequest(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),
                                _request["CurrencyISOCODE"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],
                                _request["CancelURL"],
                                (_request["QFEE"] != null ? Convert.ToInt32(_request["QFEE"]) : (int?)null),
                                (_request["QVAT"] != null ? Convert.ToInt32(_request["QVAT"]) : (int?)null),
                                (_request["Total"] != null ? Convert.ToInt32(_request["Total"]) : (int?)null),
                                (_request["ExternalId"] != null ? _request["ExternalId"].ToString() : ""),
                                _request["Hash"]
                                );
        }


        [HttpGet]
        public ActionResult PayPalRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture, 
                                          string returnURL, string cancelURL, int? QFEE, int? QVAT, int? Total, string ExternalId, string Hash)
        {
            string result = string.Empty;
            string errorMessage = string.Empty;
            string errorCode = string.Empty; ;
            string sToken = string.Empty;
            string strRedirectionURLLog = "";
            string strRedirectionURL = "";
            string sSuffix = string.Empty;
          

            try
            {

                if (_session == null)
                {
                    _server = Server;
                    _session = Session;
                    _request = Request;
                    _response = Response;
                }

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["PayPalGuid"] = null;
                _session["cardToken"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["ExternalId"] = null;


                Logger_AddLogMessage(string.Format("PayPalRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; Culture={6}; ReturnURL={7}; CancelURL={8}; ExternalId={9}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        Culture,
                                        returnURL, 
                                        cancelURL,
                                        ExternalId
                                       ), LogLevels.logINFO);

                _session["ReturnURL"] = returnURL;
                _session["ExternalId"] = ExternalId;

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
                    if (_session["Suffix"] != null)
                    {
                        sSuffix = _session["Suffix"].ToString();
                    }

                    PAYPAL_CONFIGURATION oPayPalConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetPaypalConfigurationById(Configuration_Id, out oPayPalConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetPaypalConfiguration(Guid, out oPayPalConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oPayPalConfiguration != null)
                        {
                            _session["HashSeed"] = oPayPalConfiguration.PPCON_HASH_SEED;

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, Culture, returnURL,
                                                               cancelURL, QFEE, QVAT, Total, ExternalId, oPayPalConfiguration.PPCON_HASH_SEED);
                            Hash = strCalcHash;

                            if ((oPayPalConfiguration.PPCON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("PayPalRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {
                                    if ((oPayPalConfiguration.PPCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oPayPalConfiguration.PPCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {
                                        if (string.IsNullOrEmpty(Culture))
                                        {
                                            Culture = "en-US";
                                        }
                                        CultureInfo ci = new CultureInfo(Culture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.Properties.Resources.Culture = ci;

                                    

                                        string urlReturn = returnURL;
                                        string urlCancel = cancelURL;

                                        var uri = new Uri(_request.Url.AbsoluteUri);
                                        string strURLPath = _request.Url.AbsoluteUri.Substring(0, _request.Url.AbsoluteUri.LastIndexOf("/"));



                                        if (!bMaintainReturnURLS)
                                        {

                                            urlReturn = strURLPath + "/PayPalResponse" + sSuffix;
                                            urlCancel = strURLPath + "/PayPalCancel" + sSuffix;
                                        }
                                        else
                                        {
                                            urlReturn = returnURL;
                                            urlCancel = cancelURL;
                                        }


                                        _session["utcdate"] = dtUTC;
                                        _session["PayPalGuid"] = Guid;
                                        _session["email"] = Email;
                                        _session["amount"] = Amount;
                                        _session["currency"] = CurrencyISOCODE;
                                        _session["description"] = Description;
                                        _session["lang"] = ci.Name;
                                        _session["FixedFEE"] = QFEE;
                                        _session["QVAT"] = QVAT;
                                        _session["QuantityToRechargeBase"] = Total;
                                        String sLinkRedirect = string.Empty;
                                        int iDivisor = infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE);
                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        string strAmount = (Amount.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);
                                        string strQFEE = (QFEE.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);
                                        string strQVAT = (QVAT.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);
                                        //string strQVAT = ((QVAT.Value + QFEE.Value) / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);
                                        string strTotal = (Total.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);



                                        string strToken = "";

                                        string modePaypal = Enum.GetName(typeof(enumPayPalMode), (object)oPayPalConfiguration.PPCON_RESTAPI_ENVIRONMENT.Value);
                                        if (PaypalPayments.ExpressCheckoutPassOne(strAmount,
                                                                                   CurrencyISOCODE,
                                                                                   urlCancel,
                                                                                   urlReturn,
                                                                                   Description,
                                                                                   strQFEE,
                                                                                   strQVAT,
                                                                                   strTotal,
                                                                                   oPayPalConfiguration.PPCON_RESTAPI_CLIENT_ID,
                                                                                   oPayPalConfiguration.PPCON_RESTAPI_CLIENT_SECRET,
                                                                                   oPayPalConfiguration.PPCON_RESTAPI_URL_PREFIX,
                                                                                   modePaypal,
                                                                                   oPayPalConfiguration.PPCON_SERVICE_TIMEOUT,
                                                                                   out sLinkRedirect,
                                                                                   out strToken))
                                        {
                                            _session["sessionid"] = strToken;
                                            SaveSession(strToken);
                                            Logger_AddLogMessage("PayPalRequest::LinkRedirect= " + sLinkRedirect, LogLevels.logINFO);
                                            return Redirect(sLinkRedirect);
                                        }
                                        else
                                        {
                                            result = "error";
                                            errorCode = "invalid_hash";
                                            errorMessage = "Invalid Hash";

                                            Logger_AddLogMessage(string.Format("PayPalRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                                   Hash, strCalcHash), LogLevels.logINFO);
                                        }
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("PayPalRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("PayPalRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }
                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "PayPal configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "PayPal configuration not found";
                    }
                }
            }
            catch (Exception e)
            {
                Logger_AddLogMessage("ERROR:: PayPalRequest" + e.Message, LogLevels.logERROR);
                Console.Write(e.Message);
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "PayPalRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;                               
            }


            strRedirectionURLLog = string.Format("PayPalResult" + sSuffix + "?result={0}" +
                                                "&errorCode={1}" +
                                                "&errorMessage={2}",
                                                _server.UrlEncode(result),
                                                _server.UrlEncode(errorCode),
                                                _server.UrlEncode(errorMessage));


            strRedirectionURL = string.Format("PayPalResult" + sSuffix + "?r={0}", PayPalResultCalc());


            return Redirect(strRedirectionURL);

        }



        [HttpGet]
        public ActionResult PayPalResponse()
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "";


            string token = "";
            string paymentId = "";
            string PayerId = "";


            if (_session == null)
            {
                _server = Server;
                _session = Session;
                _request = Request;
                _response = Response;
            }

            string sSuffix = string.Empty;
                      
           
            try
            {
                token = _request.QueryString["token"];

                if (_session["sessionid"] == null)
                {
                    LoadSession(token);
                }


                if (_session["Suffix"] != null)
                {
                    sSuffix = _session["Suffix"].ToString();
                }
                
                paymentId = _request.Params["paymentId"];
                PayerId = _request.Params["PayerID"];


                Logger_AddLogMessage(string.Format("PayPalResponse Begin: Email={0} ; Amount={1} ; Currency={2}; Token={3}; Paymentid={4}; PayerID ={5}",
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        token,
                                        paymentId,
                                        PayerId),
                                        LogLevels.logINFO);

                string strGuid = "";

                if (_session["PayPalGuid"] != null)
                {
                    strGuid = _session["PayPalGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "PayPal Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {
                    result = "succeeded";
                    errorMessage = "";
                    errorCode = "succeeded";
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
                _session["token"] = token;
                _session["paymentId"] = paymentId;
                _session["PayerID"] = PayerId;




                strRedirectionURLLog = string.Format("PayPalResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&token={3}" +
                                                    "&paymentId={4}" +
                                                    "&PayerId={5}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(token),
                    _server.UrlEncode(paymentId),
                    _server.UrlEncode(PayerId));


                Logger_AddLogMessage(string.Format("PayPalResponse End: Email={0} ; Amount={1} ; Currency={2} ; ResultURL={3}",
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("PayPalResult" + sSuffix + "?r={0}", PayPalResultCalc());

            }

            return Redirect(strRedirectionURL);
        }




        [HttpGet]
        public ActionResult PayPalCancel()
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "";


            string token = "";
          


            if (_session == null)
            {
                _server = Server;
                _session = Session;
                _request = Request;
                _response = Response;
            }

            string sSuffix = string.Empty;
           


            try
            {

                token = _request.QueryString["token"];

                if (_session["sessionid"] == null)
                {
                    LoadSession(token);
                }


                if (_session["Suffix"] != null)
                {
                    sSuffix = _session["Suffix"].ToString();
                }

                Logger_AddLogMessage(string.Format("PayPalCancel Begin: Email={0} ; Amount={1} ; Currency={2}; Token={3}; ",
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        token),
                                        LogLevels.logINFO);

                string strGuid = "";

                if (_session["PayPalGuid"] != null)
                {
                    strGuid = _session["PayPalGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "PayPal Guid not found";
                    errorCode = "invalid_configuration";
                }
                else
                {
                    result = "cancel";
                    errorMessage = "Transaction cancelled By user";
                    errorCode = "transaction_cancelled";

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
                _session["token"] = token;




                strRedirectionURLLog = string.Format("PayPalResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&token={3}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(token));


                Logger_AddLogMessage(string.Format("PayPalResponse End: Email={0} ; Amount={1} ; Currency={2} ; ResultURL={3}",
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("PayPalResult" + sSuffix + "?r={0}", PayPalResultCalc());

            }

            return Redirect(strRedirectionURL);
        }



        [HttpGet]
        public ActionResult PayPalResult(string r)
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
                    _response = Response;
                }

                if (_session["ReturnURL"] != null && !string.IsNullOrEmpty(_session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    
                    if (_session["ExternalId"] != null && !string.IsNullOrEmpty(_session["ExternalId"].ToString()))
                    {
                        postData.Add("ExternalId", _session["ExternalId"].ToString());
                    }
                    postData.Add("r", r);

                    RedirectWithData(_session["ReturnURL"].ToString(), postData);
                    
                }
                else
                {

                    _session["result"] = null;
                    _session["errorCode"] = null;
                    _session["errorMessage"] = null;
                    _session["email"] = null;
                    _session["amount"] = null;
                    _session["currency"] = null;
                    _session["utcdate"] = null;
                    _session["PayPalGuid"] = null;
                    _session["cardToken"] = null;
                    _session["HashSeed"] = null;
                    _session["ReturnURL"] = null;
                    _session["ExternalId"] = null;
                }

                return View();
            }

        }


     

        private string PayPalResultCalc()
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

            if (_session["token"] != null)  oDataDict["paypal_token"] = _session["token"];
            if (_session["result"] != null && _session["result"].ToString() == "succeeded")
            {                            
                oDataDict["paypal_paymentId"] = _session["paymentId"];
                oDataDict["paypal_PayerID"] = _session["PayerID"];
            }

            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("PaypalResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, _session["HashSeed"].ToString());

            return strRes;


        }

        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture,string ReturnURL,
                                    string cancelURL, int? QFEE, int? QVAT, int? Total, string ExternalId, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + Culture + ReturnURL +
                cancelURL + (QFEE.HasValue ? QFEE.ToString() : "") + (QVAT.HasValue ? QVAT.ToString() : "") + (Total.HasValue ? Total.ToString() : "") + ExternalId;

            return CalculateHash(strHashString, strHashSeed);

        }

    }
}
