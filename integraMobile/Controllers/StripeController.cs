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
    public class StripeController : BaseCCController
    {
        private const string DEFAULT_IMAGE_URL = "https://stripe.com/img/documentation/checkout/marketplace.png";


        public StripeController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }



        [HttpPost]
        public ActionResult StripeRequest()
        {


            return StripeRequest(Request["Guid"],
                                Request["Email"],
                                (Request["Amount"] != null ? Convert.ToInt32(Request["Amount"]) : (int?)null),
                                Request["CurrencyISOCODE"],
                                Request["Description"],
                                Request["UTCDate"],
                                Request["ReturnURL"],
                                Request["Hash"]);
        }


        public ActionResult StripeRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string Hash)
        {           
            string result = "";
            string errorMessage = "";
            string errorCode = "";

            try
            {

                Session["result"] = null;
                Session["errorCode"] = null;
                Session["errorMessage"] = null;
                Session["email"] = null;
                Session["amount"] = null;
                Session["currency"] = null;
                Session["utcdate"] = null;
                Session["StripeGuid"] = null;
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


                Logger_AddLogMessage(string.Format("StripeRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}",
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
                    (!Amount.HasValue)||(Amount.Value<=0) ||
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


                    STRIPE_CONFIGURATION oStripeConfiguration = null;

                    if (infraestructureRepository.GetStripeConfiguration(Guid, out oStripeConfiguration))
                    {
                        if (oStripeConfiguration != null)
                        {

                            Session["HashSeed"] = oStripeConfiguration.STRCON_HASH_SEED;

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, ReturnURL, oStripeConfiguration.STRCON_HASH_SEED);


                            if ((oStripeConfiguration.STRCON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("StripeRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oStripeConfiguration.STRCON_CHECK_DATE_AND_HASH == 0) ||
                                         (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oStripeConfiguration.STRCON_CONFIRMATION_TIME))
                                    {

                                        string strImageURL = DEFAULT_IMAGE_URL;

                                        if (!string.IsNullOrEmpty(oStripeConfiguration.STRCON_IMAGE_URL))
                                        {
                                            strImageURL = oStripeConfiguration.STRCON_IMAGE_URL;
                                        }

                                        ViewData["email"] = Email;
                                        ViewData["amount"] = Amount;
                                        ViewData["currency"] = CurrencyISOCODE;
                                        ViewData["key"] = oStripeConfiguration.STRCON_DATA_KEY;
                                        ViewData["commerceName"] = oStripeConfiguration.STRCON_COMMERCE_NAME;
                                        ViewData["panelLabel"] = oStripeConfiguration.STRCON_PANEL_LABEL;
                                        ViewData["description"] = Description;
                                        ViewData["image"] = strImageURL;
                                        Session["email"] = Email;
                                        Session["amount"] = Amount;
                                        Session["currency"] = CurrencyISOCODE;
                                        Session["utcdate"] = dtUTC;
                                        Session["StripeGuid"] = Guid;
                                       
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("StripeRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("StripeRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Stripe configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Stripe configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage="StripeRequest Method Exception";
            }
        
            if (!string.IsNullOrEmpty(errorCode))
            {

                Session["result"] = result;
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;


                string strRedirectionURLLog = string.Format("StripeResult?result={0}&errorCode={1}&errorMessage={2}", Server.UrlEncode(result), Server.UrlEncode(errorCode), Server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("StripeResult?r={0}",StripeResultCalc());
                Logger_AddLogMessage(string.Format("StripeRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",     
                                        Guid,               
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {
                Logger_AddLogMessage(string.Format("StripeRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
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
        public ActionResult StripeResponse(StripeResponseModel oModel)
        {            
            string result="";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "StripeResult";
            string strCustomerId = "";
            string strCardToken = "";
            string strChargeID = "";
            string strCardScheme = "";
            string strStripeDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";


            try
            {
                Logger_AddLogMessage(string.Format("StripeResponse Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4}",
                                        oModel.stripeErrorCode,
                                        oModel.stripeToken,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString()
                                        ), LogLevels.logINFO);


                if (!string.IsNullOrEmpty(oModel.stripeErrorCode))
                {
                    result = "error";
                    errorCode = oModel.stripeErrorCode;
                    errorMessage = "Request Error Code";
                }
                else
                {

                    string strGuid = "";

                    if (Session["StripeGuid"] != null)
                    {
                        strGuid = Session["StripeGuid"].ToString();
                    }

                    if (string.IsNullOrEmpty(strGuid))
                    {
                        result = "error";
                        errorMessage = "Stripe Guid not found";
                        errorCode = "invalid_configuration";

                    }
                    else
                    {

                        STRIPE_CONFIGURATION oStripeConfiguration = null;

                        if (infraestructureRepository.GetStripeConfiguration(strGuid, out oStripeConfiguration))
                        {
                            if (oStripeConfiguration == null)
                            {
                                result = "error";
                                errorCode = "configuration_not_found";
                                errorMessage = "Stripe configuration not found";
                            }
                            else
                            {

                                /*DateTime dtUTC = (DateTime)Session["utcdate"];


                                if ((oStripeConfiguration.STRCON_CHECK_DATE_AND_HASH == 1) &&
                                    (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oStripeConfiguration.STRCON_CONFIRMATION_TIME))
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("StripeResponse : BeginningDate={0} ; CurrentDate={1}",
                                    dtUTC, DateTime.UtcNow), LogLevels.logINFO);

                                }
                                else
                                {*/
                                    if (string.IsNullOrEmpty(oModel.stripeToken))
                                    {
                                        result = "error";
                                        errorCode = "null_token";
                                        errorMessage = "Returned Token is null";
                                    }
                                    else
                                    {
                                        if (oModel.stripeEmail != Session["email"].ToString())
                                        {
                                            result = "error";
                                            errorCode = "invalid_email";
                                            errorMessage = "Returned Email is not valid";
                                        }
                                        else
                                        {

                                            strCardToken = oModel.stripeToken;
                                            StripePayments.PerformCharge(oStripeConfiguration.STRCON_SECRET_KEY,
                                                                            oModel.stripeEmail,
                                                                            oModel.stripeToken,
                                                                            ref strCustomerId,
                                                                            (int)Session["amount"],
                                                                            Session["currency"].ToString(),
                                                                            false,
                                                                            out result,
                                                                            out errorCode,
                                                                            out errorMessage,
                                                                            out strCardScheme,
                                                                            out strPAN,
                                                                            out strExpirationDateMonth,
                                                                            out strExpirationDateYear,
                                                                            out strChargeID,
                                                                            out strStripeDateTime);                                              
                                            
                                        }
                                    }
                                //}
                            }
                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Stripe configuration not found";
                        }

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
                Session["result"] = result;
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;
                Session["cardToken"] = strCardToken;
                Session["cardScheme"] = strCardScheme;
                Session["cardPAN"] = strPAN;
                Session["cardExpMonth"] = strExpirationDateMonth;
                Session["cardExpYear"] = strExpirationDateYear;
                Session["chargeID"] = strChargeID;
                Session["chargeDateTime"] = strStripeDateTime;
                Session["customerID"] = strCustomerId;

                strRedirectionURLLog = string.Format("StripeResult?result={0}"+
                                                    "&errorCode={1}"+
                                                    "&errorMessage={2}"+
                                                    "&cardToken={3}"+
                                                    "&cardScheme={4}" +
                                                    "&cardPan={5}" +
                                                    "&cardExpMonth={6}" +
                                                    "&cardExpYear={7}" +
                                                    "&chargeID={8}" +
                                                    "&chargeDateTime={9}"+
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
                    Server.UrlEncode(strStripeDateTime),
                    Server.UrlEncode(strCustomerId));

                Logger_AddLogMessage(string.Format("StripeResponse End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardToken,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("StripeResult?r={0}", StripeResultCalc());
     
            }
            
            return Redirect(strRedirectionURL);
        }

        [HttpGet]
        public ActionResult StripeResult(string r)
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
                    Session["StripeGuid"] = null;
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



        private string StripeResultCalc()
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

            Logger_AddLogMessage(string.Format("StripeResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes=CalculateCryptResult(json, Session["HashSeed"].ToString());

            return strRes;
            

        }

        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate,string ReturnURL, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL;

            return CalculateHash(strHashString,strHashSeed);

        }



    }
}
