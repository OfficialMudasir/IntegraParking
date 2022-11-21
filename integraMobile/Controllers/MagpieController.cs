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


    //http://localhost:1050/Magpie/MagpieRequest?Guid=e53f878f-a15b-4343-9545-2fd6c69b171f&Email=febermejo@gmail.com&Amount=5000&CurrencyISOCODE=EUR&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423


    public class MAGPIE_CONFIGURATION
    {
        public string MPIECON_CHECKOUT_URL = "https://pay.magpie.im/api/v2";
        public string MPIECON_API_URL = "https://api.magpie.im/v2";
        public string MPIECON_HASH_SEED = "";
        public int MPIECON_CHECK_DATE_AND_HASH = 0;
        public int MPIECON_CONFIRMATION_TIME = 5000;
        public string MPIECON_IMAGE_URL = "";
        public string MPIECON_DATA_KEY = "";
        public string MPIECON_COMMERCE_NAME = "";
        public string MPIECON_PANEL_LABEL = "";
        public string MPIECON_API_KEY = "pk_live_k8TeDsR10en1yGqaG8BoBs";
        public string MPIECON_SECRET_KEY = "sk_live_lXlTWqRMZnlY47vOWSlAVU";
        //public string MPIECON_API_KEY = "pk_test_ZZvWxebdswcxOtFVjiIMdd";
        //public string MPIECON_SECRET_KEY = "sk_test_EEJnedFbFBdVBrdZV5rdlp";
        public int MPIECON_WS_API_TIMEOUT = 20000;


        
    }



    [HandleError]
    [NoCache]
    public class MagpieController : BaseCCController
    {
        private const string DEFAULT_IMAGE_URL = "https://Magpie.com/img/documentation/checkout/marketplace.png";


        public MagpieController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

        protected bool GetMagpieConfiguration(string Guid, out MAGPIE_CONFIGURATION oMagpieConfiguration)
        {
            oMagpieConfiguration = new MAGPIE_CONFIGURATION();
            return true;
        }

        [HttpPost]
        public ActionResult MagpieRequest()
        {


            return MagpieRequest(Request["Guid"],
                                Request["Email"],
                                (Request["Amount"] != null ? Convert.ToInt32(Request["Amount"]) : (int?)null),
                                Request["CurrencyISOCODE"],
                                Request["Description"],
                                Request["UTCDate"],
                                Request["ReturnURL"],
                                Request["Hash"]);
        }


        public ActionResult MagpieRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";

            try
            {



                if (_session == null) _session = Session;
                MagpiePayments oPayments = new MagpiePayments();

                if (_server == null) _server = Server;
                if (_request == null) _request = Request;
                
                
                Session["result"] = null;
                Session["errorCode"] = null;
                Session["errorMessage"] = null;
                Session["email"] = null;
                Session["amount"] = null;
                Session["currency"] = null;
                Session["utcdate"] = null;
                Session["MagpieGuid"] = null;
                Session["cardToken"] = null;
                Session["cardScheme"] = null;
                Session["cardPAN"] = null;
                Session["cardExpMonth"] = null;
                Session["cardExpYear"] = null;
                Session["chargeID"] = null;
                Session["chargeDateTime"] = null;
                Session["HashSeed"] = null;
                Session["customerID"] = null;
                Session["ReturnURL"] = null;


                Logger_AddLogMessage(string.Format("MagpieRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        ReturnURL), LogLevels.logINFO);


                Session["ReturnURL"] = ReturnURL;

                if ((string.IsNullOrEmpty(Guid)) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value <= 0) ||
                    (string.IsNullOrEmpty(CurrencyISOCODE)) ||
                    (string.IsNullOrEmpty(Description)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Hash)))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {


                    MAGPIE_CONFIGURATION oMagpieConfiguration = null;

                    if (/*infraestructureRepository.*/GetMagpieConfiguration(Guid, out oMagpieConfiguration))
                    {
                        if (oMagpieConfiguration != null)
                        {

                            MagpiePayments.MagpieErrorCode oErrorCode;
                            string strErrorMessage = "";
                            /*
                            string strSID = "cs_iA5dLjCNm1d3m7AY";



                            string strChargeId = "";


                            if (oPayments.GetCheckoutSession(oMagpieConfiguration.MPIECON_CHECKOUT_URL,
                                            oMagpieConfiguration.MPIECON_SECRET_KEY,
                                            oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                            strSID,
                                            out oErrorCode,
                                            out strErrorMessage,
                                            out strChargeId))
                            {

                                oPayments.RefundCharge(oMagpieConfiguration.MPIECON_API_URL,
                                            oMagpieConfiguration.MPIECON_SECRET_KEY,
                                            oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                            strChargeId,
                                            5000,
                                            out oErrorCode,
                                            out strErrorMessage);

                            }*/

                           
                            Session["HashSeed"] = oMagpieConfiguration.MPIECON_HASH_SEED;

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, ReturnURL, oMagpieConfiguration.MPIECON_HASH_SEED);


                            if ((oMagpieConfiguration.MPIECON_CHECK_DATE_AND_HASH == 0) ||
                                (strCalcHash == Hash))
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
                                    Logger_AddLogMessage(string.Format("MagpieRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oMagpieConfiguration.MPIECON_CHECK_DATE_AND_HASH == 0) ||
                                         (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oMagpieConfiguration.MPIECON_CONFIRMATION_TIME))
                                    {
                                        string strUserReference = MagpiePayments.UserReference();
                                        string strURLPath = _request.Url.AbsoluteUri.Substring(0, _request.Url.AbsoluteUri.LastIndexOf("/"));
                                        string sSuffix = string.Empty;
                                        if (_session["Suffix"] != null)
                                        {
                                            sSuffix = _session["Suffix"].ToString();
                                        }
                                        string urlSuccess = strURLPath + "/MagpieSuccess" + sSuffix;
                                        string urlCancel = strURLPath + "/MagpieCancel" + sSuffix;
                                        string strSessionId;
                                        string strPaymentUrl;


                                        string strCustomerId = "";
                                        if (!oPayments.RetrieveCustomerByEmail(oMagpieConfiguration.MPIECON_API_URL,
                                                                        oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                                        oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                                        Email,
                                                                        out oErrorCode,
                                                                        out strErrorMessage,
                                                                        out strCustomerId))
                                        {
                                            oPayments.CreateCustomer(oMagpieConfiguration.MPIECON_API_URL,
                                                                                oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                                                oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                                                Email,
                                                                                out oErrorCode,
                                                                                out strErrorMessage,
                                                                                out strCustomerId);
                                        }

                                        if (oPayments.CreateCheckoutSession(oMagpieConfiguration.MPIECON_CHECKOUT_URL,
                                                                            oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                                            Description,
                                                                            strCustomerId,
                                                                            Amount.Value,
                                                                            urlSuccess,
                                                                            urlCancel,
                                                                            oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                                            strUserReference,
                                                                            out oErrorCode,
                                                                            out strErrorMessage,
                                                                            out strSessionId,
                                                                            out strPaymentUrl))
                                        {
                                            Session["email"] = Email;
                                            Session["amount"] = Amount;
                                            Session["currency"] = CurrencyISOCODE;
                                            Session["utcdate"] = dtUTC;
                                            Session["MagpieGuid"] = Guid;
                                            return Redirect(strPaymentUrl);

                                        }
                                        else
                                        {

                                            result = "error";
                                            errorMessage = "Error Getting Session";
                                            errorCode = "error_getting_session";
                                        }

                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("MagpieRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("MagpieRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Magpie configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Magpie configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "MagpieRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                Session["result"] = result;
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;


                string strRedirectionURLLog = string.Format("MagpieResult?result={0}&errorCode={1}&errorMessage={2}", Server.UrlEncode(result), Server.UrlEncode(errorCode), Server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("MagpieResult?r={0}", MagpieResultCalc());
                Logger_AddLogMessage(string.Format("MagpieRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {
                Logger_AddLogMessage(string.Format("MagpieRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                return View();
            }

        }


        [HttpGet]
        public ActionResult MagpieSuccess(string session_id)
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "MagpieResult";
            string strCustomerId = "";
            string strCardToken = "";
            string strChargeID = "";
            string strCardScheme = "";
            string strMagpieDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";


            try
            {

                string Guid = Session["MagpieGuid"].ToString();
                string Email = Session["email"].ToString();

                MAGPIE_CONFIGURATION oMagpieConfiguration = null;

                if (/*infraestructureRepository.*/GetMagpieConfiguration(Guid, out oMagpieConfiguration))
                {
                    if (oMagpieConfiguration != null)
                    {


                        MagpiePayments.MagpieErrorCode oErrorCode;
                        string strErrorMessage = "";
                        string strChargeId = "";
                        string strSourceId = "";


                        MagpiePayments oPayments = new MagpiePayments();


                        oPayments.GetCheckoutSession(oMagpieConfiguration.MPIECON_CHECKOUT_URL,
                                                    oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                    oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                    session_id,
                                                    out oErrorCode,
                                                    out strErrorMessage,
                                                    out strSourceId,
                                                    out strChargeId);

                     

                        if (!oPayments.RetrieveCustomerByEmail(oMagpieConfiguration.MPIECON_API_URL,
                                                            oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                            oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                            Email,
                                                            out oErrorCode,
                                                            out strErrorMessage,
                                                            out strCustomerId))
                        {
                            oPayments.CreateCustomer(oMagpieConfiguration.MPIECON_API_URL,
                                                                oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                                oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                                Email,
                                                                out oErrorCode,
                                                                out strErrorMessage,
                                                                out strCustomerId);
                        }


                        oPayments.AttachSourceToCustomer(oMagpieConfiguration.MPIECON_API_URL,
                                                                oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                                oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                                strCustomerId,
                                                                strSourceId,
                                                                out oErrorCode,
                                                                out strErrorMessage);




                       /* oPayments.RefundCharge(oMagpieConfiguration.MPIECON_API_URL,
                                           oMagpieConfiguration.MPIECON_SECRET_KEY,
                                           oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                           strChargeId,
                                           Convert.ToInt32(Session["amount"]),
                                           out oErrorCode,
                                           out errorMessage);*/


                        string strChargeId2 = "";
                        oPayments.AutomaticTransaction(oMagpieConfiguration.MPIECON_API_URL,
                                                                            oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                                            oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                                            "test",
                                                                            strSourceId,
                                                                            Convert.ToInt32(Session["amount"]),
                                                                            out oErrorCode,
                                                                            out strErrorMessage,
                                                                            out strChargeId2);

                        oPayments.RefundCharge(oMagpieConfiguration.MPIECON_API_URL,
                                               oMagpieConfiguration.MPIECON_SECRET_KEY,
                                               oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                               strChargeId2,
                                               Convert.ToInt32(Session["amount"]),
                                               out oErrorCode,
                                               out errorMessage);
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Magpie configuration not found";
                    }
                }
                else
                {
                    result = "error";
                    errorCode = "configuration_not_found";
                    errorMessage = "Magpie configuration not found";
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
                Session["result"] = result;
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;
                Session["cardToken"] = strCardToken;
                Session["cardScheme"] = strCardScheme;
                Session["cardPAN"] = strPAN;
                Session["cardExpMonth"] = strExpirationDateMonth;
                Session["cardExpYear"] = strExpirationDateYear;
                Session["chargeID"] = strChargeID;
                Session["chargeDateTime"] = strMagpieDateTime;
                Session["customerID"] = strCustomerId;

                strRedirectionURLLog = string.Format("MagpieResult?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardToken={3}" +
                                                    "&cardScheme={4}" +
                                                    "&cardPan={5}" +
                                                    "&cardExpMonth={6}" +
                                                    "&cardExpYear={7}" +
                                                    "&chargeID={8}" +
                                                    "&chargeDateTime={9}" +
                                                    "&customerID={10}",
                    Server.UrlEncode(result),
                    Server.UrlEncode(errorCode),
                    Server.UrlEncode(errorMessage),
                    Server.UrlEncode(strCardToken),
                    Server.UrlEncode(strCardScheme),
                    Server.UrlEncode(strPAN),
                    Server.UrlEncode(strExpirationDateMonth),
                    Server.UrlEncode(strExpirationDateYear),
                    Server.UrlEncode(strChargeID),
                    Server.UrlEncode(strMagpieDateTime),
                    Server.UrlEncode(strCustomerId));

                Logger_AddLogMessage(string.Format("MagpieResponse End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardToken,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MagpieResult?r={0}", MagpieResultCalc());

            }

            return Redirect(strRedirectionURL);
        }



        [HttpGet]
        public ActionResult MagpieCancel(string session_id)
        {

            if (_session == null) _session = Session;

            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "MagpieResult";
            string strCustomerId = "";
            string strCardToken = "";
            string strChargeID = "";
            string strCardScheme = "";
            string strMagpieDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";


            try
            {

                string Guid = Session["MagpieGuid"].ToString(); ;

                MAGPIE_CONFIGURATION oMagpieConfiguration = null;

                if (/*infraestructureRepository.*/GetMagpieConfiguration(Guid, out oMagpieConfiguration))
                {
                    if (oMagpieConfiguration != null)
                    {


                        MagpiePayments.MagpieErrorCode oErrorCode;
                        string strErrorMessage = "";
                        string strChargeId = "";
                        string strSourceId = "";



                        MagpiePayments oPayments = new MagpiePayments();


                        oPayments.GetCheckoutSession(oMagpieConfiguration.MPIECON_API_URL,
                                                    oMagpieConfiguration.MPIECON_SECRET_KEY,
                                                    oMagpieConfiguration.MPIECON_WS_API_TIMEOUT,
                                                    session_id,
                                                    out oErrorCode,
                                                    out strErrorMessage,
                                                    out strSourceId,
                                                    out strChargeId);
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Magpie configuration not found";
                    }
                }
                else
                {
                    result = "error";
                    errorCode = "configuration_not_found";
                    errorMessage = "Magpie configuration not found";
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
                Session["result"] = result;
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;
                Session["cardToken"] = strCardToken;
                Session["cardScheme"] = strCardScheme;
                Session["cardPAN"] = strPAN;
                Session["cardExpMonth"] = strExpirationDateMonth;
                Session["cardExpYear"] = strExpirationDateYear;
                Session["chargeID"] = strChargeID;
                Session["chargeDateTime"] = strMagpieDateTime;
                Session["customerID"] = strCustomerId;

                strRedirectionURLLog = string.Format("MagpieResult?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardToken={3}" +
                                                    "&cardScheme={4}" +
                                                    "&cardPan={5}" +
                                                    "&cardExpMonth={6}" +
                                                    "&cardExpYear={7}" +
                                                    "&chargeID={8}" +
                                                    "&chargeDateTime={9}" +
                                                    "&customerID={10}",
                    Server.UrlEncode(result),
                    Server.UrlEncode(errorCode),
                    Server.UrlEncode(errorMessage),
                    Server.UrlEncode(strCardToken),
                    Server.UrlEncode(strCardScheme),
                    Server.UrlEncode(strPAN),
                    Server.UrlEncode(strExpirationDateMonth),
                    Server.UrlEncode(strExpirationDateYear),
                    Server.UrlEncode(strChargeID),
                    Server.UrlEncode(strMagpieDateTime),
                    Server.UrlEncode(strCustomerId));

                Logger_AddLogMessage(string.Format("MagpieResponse End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardToken,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MagpieResult?r={0}", MagpieResultCalc());

            }

            return Redirect(strRedirectionURL);
        }


        [HttpGet]
        public ActionResult MagpieResult(string r)
        {


            if (string.IsNullOrEmpty(r))
            {
                return new HttpNotFoundResult("");
            }
            else
            {

                //string strResultDec = DecryptCryptResult(r, Session["HashSeed"].ToString());
                //ViewData["Result"] = strResultDec;
                if (Session["ReturnURL"] != null && !string.IsNullOrEmpty(Session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    postData.Add("r", r);

                    RedirectWithData(Session["ReturnURL"].ToString(), postData);
                }
                else
                {

                    Session["result"] = null;
                    Session["errorCode"] = null;
                    Session["errorMessage"] = null;
                    Session["email"] = null;
                    Session["amount"] = null;
                    Session["currency"] = null;
                    Session["utcdate"] = null;
                    Session["MagpieGuid"] = null;
                    Session["cardToken"] = null;
                    Session["cardScheme"] = null;
                    Session["cardPAN"] = null;
                    Session["cardExpMonth"] = null;
                    Session["cardExpYear"] = null;
                    Session["chargeID"] = null;
                    Session["chargeDateTime"] = null;
                    Session["HashSeed"] = null;
                    Session["customerID"] = null;
                    Session["ResultURL"] = null;
                }

                return View();
            }

        }



        private string MagpieResultCalc()
        {

            string strRes = "";

            Dictionary<string, object> oDataDict = new Dictionary<string, object>();

            oDataDict["email"] = Session["email"];
            oDataDict["amount"] = Session["amount"];
            oDataDict["currency"] = Session["currency"];
            oDataDict["result"] = Session["result"];
            oDataDict["errorCode"] = Session["errorCode"];
            oDataDict["errorMessage"] = Session["errorMessage"];

            if (Session["result"] != null && Session["result"].ToString() == "succeeded")
            {
                oDataDict["customerID"] = Session["customerID"];
                oDataDict["cardToken"] = Session["cardToken"];
                oDataDict["cardScheme"] = Session["cardScheme"];
                oDataDict["cardPAN"] = Session["cardPAN"];
                oDataDict["cardExpMonth"] = Session["cardExpMonth"];
                oDataDict["cardExpYear"] = Session["cardExpYear"];
                oDataDict["chargeID"] = Session["chargeID"];
                oDataDict["chargeDateTime"] = Session["chargeDateTime"];
            }


            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("MagpieResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, Session["HashSeed"].ToString());

            return strRes;


        }

        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL;

            return CalculateHash(strHashString, strHashSeed);

        }



    }
}
