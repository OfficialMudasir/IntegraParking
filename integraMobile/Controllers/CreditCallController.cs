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
    public class CreditCallController : BaseCCController
    {

        public CreditCallController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

        [HttpPost]
        public ActionResult CreditCallRequest()
        {

        
            return CreditCallRequest(Request["Guid"],
                                Request["Email"],
                                (Request["Amount"] != null ? Convert.ToInt32(Request["Amount"]) : (int?)null),
                                Request["CurrencyISOCODE"],
                                Request["Description"],
                                Request["UTCDate"],
                                Request["ReturnURL"],
                                Request["Hash"]);
        }

     
        [HttpGet]
        public ActionResult CreditCallRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string Hash)
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
                Session["CreditCallGuid"] = null;
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


                Logger_AddLogMessage(string.Format("CreditCallRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}",
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


                    CREDIT_CALL_CONFIGURATION oCreditCallConfiguration = null;

                    if (infraestructureRepository.GetCreditCallConfiguration(Guid, out oCreditCallConfiguration))
                    {
                        if (oCreditCallConfiguration != null)
                        {

                            Session["HashSeed"] = oCreditCallConfiguration.CCCON_HASH_SEED;

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, ReturnURL, oCreditCallConfiguration.CCCON_HASH_SEED);


                            if ((oCreditCallConfiguration.CCCON_CHECK_DATE_AND_HASH == 0) ||
                                (strCalcHash == Hash))
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
                                    Logger_AddLogMessage(string.Format("CreditCallRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oCreditCallConfiguration.CCCON_CHECK_DATE_AND_HASH == 0) ||
                                         (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oCreditCallConfiguration.CCCON_CONFIRMATION_TIME))
                                    {

                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        string strAmount = (Amount.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);

                                        ViewData["email"] = Email;
                                        ViewData["ekashu_form_url"] = oCreditCallConfiguration.CCCON_EKASHU_FORM_URL;
                                        ViewData["ekashu_seller_id"] = oCreditCallConfiguration.CCCON_TERMINAL_ID;
                                        ViewData["ekashu_seller_key"] = oCreditCallConfiguration.CCCON_TRANSACTION_KEY.Substring(0, 8);
                                        ViewData["ekashu_amount"] = strAmount;
                                        ViewData["ekashu_currency"] = CurrencyISOCODE;
                                        ViewData["ekashu_reference"] = CardEasePayments.UserReference();
                                        ViewData["ekashu_hash_code"] = CardEasePayments.HashCode(oCreditCallConfiguration.CCCON_TERMINAL_ID,
                                                                                                    oCreditCallConfiguration.CCCON_HASH_KEY, 
                                                                                                    (string)ViewData["ekashu_reference"],
                                                                                                    strAmount); 
                                        ViewData["ekashu_description"] = Description;

                                        ViewData["css_url"] = oCreditCallConfiguration.CCCON_CSS_URL;

                                        string strSellerName = oCreditCallConfiguration.CCCON_SELLER_NAME;

                                        if (strSellerName.Length > 8)
                                            ViewData["ekashu_seller_name"] = strSellerName.Substring(0, 8);
                                        else
                                            ViewData["ekashu_seller_name"] = strSellerName;



                                        string strURLPath = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/"));


                                        ViewData["ekashu_failure_url"] = strURLPath + "/CreditCallFailure";
                                        ViewData["ekashu_return_url"] = strURLPath + "/CreditCallCancel";
                                        ViewData["ekashu_success_url"] = strURLPath + "/CreditCallSuccess";

                                        Session["email"] = Email;
                                        Session["amount"] = Amount;
                                        Session["currency"] = CurrencyISOCODE;
                                        Session["utcdate"] = dtUTC;
                                        Session["CreditCallGuid"] = Guid;
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("CreditCallRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("CreditCallRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "CreditCall configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "CreditCall configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "CreditCallRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                Session["result"] = result;
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;


                string strRedirectionURLLog = string.Format("CreditCallResult?result={0}&errorCode={1}&errorMessage={2}", Server.UrlEncode(result), Server.UrlEncode(errorCode), Server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("CreditCallResult?r={0}", CreditCallResultCalc());
                Logger_AddLogMessage(string.Format("CreditCallRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {
                Logger_AddLogMessage(string.Format("CreditCallRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                return View();
            }

        }


        public ActionResult CreditCallSuccess()
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "CreditCallResult";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strCreditCallDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";
            string strAuthResult = "";


            try
            {

                strReference = Request["ekashu_reference"];
                strAuthCode = Request["ekashu_auth_code"];
                strAuthResult = Request["ekashu_auth_result"];
                strCardHash = Request["ekashu_card_hash"];
                strCardReference = Request["ekashu_card_reference"];
                strCardScheme = Request["ekashu_card_scheme"];
                strCreditCallDateTime = Request["ekashu_date_time_local_fmt"];
                strPAN = Request["ekashu_masked_card_number"];
                strTransactionId = Request["ekashu_transaction_id"];
                strExpirationDateMonth = Request["ekashu_expires_end_month"];
                strExpirationDateYear = Request["ekashu_expires_end_year"];


                Logger_AddLogMessage(string.Format("CreditCallSuccess Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4}",
                                        "succeeded",
                                        strCardReference,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString()
                                        ), LogLevels.logINFO);


               

                string strGuid = "";

                if (Session["CreditCallGuid"] != null)
                {
                    strGuid = Session["CreditCallGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid))
                {
                    result = "error";
                    errorMessage = "CreditCall Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    CREDIT_CALL_CONFIGURATION oCreditCallConfiguration = null;

                    if (infraestructureRepository.GetCreditCallConfiguration(strGuid, out oCreditCallConfiguration))
                    {
                        if (oCreditCallConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "CreditCall configuration not found";
                        }
                        else
                        {

                            DateTime dtUTC = (DateTime)Session["utcdate"];


                            if ((oCreditCallConfiguration.CCCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oCreditCallConfiguration.CCCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("CreditCallResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);

                            }
                            else
                            {
                                if (string.IsNullOrEmpty(strCardReference))
                                {
                                    result = "error";
                                    errorCode = "null_token";
                                    errorMessage = "Returned Token is null";
                                }
                            }
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "CreditCall configuration not found";
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
                Session["result"] = "succeeded";
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;
                Session["cardReference"] = strCardReference;
                Session["cardHash"] = strCardHash;
                Session["cardScheme"] = strCardScheme;
                Session["cardPAN"] = strPAN;
                Session["cardExpMonth"] = strExpirationDateMonth;
                Session["cardExpYear"] = strExpirationDateYear;
                Session["chargeDateTime"] = strCreditCallDateTime;
                Session["reference"] = strReference;
                Session["transactionID"] = strTransactionId;
                Session["authCode"] = strAuthCode;
                Session["authResult"] = strAuthResult;



                strRedirectionURLLog = string.Format("CreditCallResult?result={0}" +
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
                                                    "&authCode={12}"+
                                                    "&authResult={13}",
                    Server.UrlEncode(result),
                    Server.UrlEncode(errorCode),
                    Server.UrlEncode(errorMessage),
                    Server.UrlEncode(strCardReference),
                    Server.UrlEncode(strCardHash),
                    Server.UrlEncode(strCardScheme),
                    Server.UrlEncode(strPAN),
                    Server.UrlEncode(strExpirationDateMonth),
                    Server.UrlEncode(strExpirationDateYear),
                    Server.UrlEncode(strCreditCallDateTime),
                    Server.UrlEncode(strReference),
                    Server.UrlEncode(strTransactionId),
                    Server.UrlEncode(strAuthCode),
                    Server.UrlEncode(strAuthResult));

                Logger_AddLogMessage(string.Format("CreditCallSuccess End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("CreditCallResult?r={0}", CreditCallResultCalc());

            }

            return Redirect(strRedirectionURL);
        }


        public ActionResult CreditCallCancel()
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "CreditCallResult";
            string strAuthResult = "";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strCreditCallDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";


            try
            {

                strAuthResult = Request["ekashu_auth_result"];


                Logger_AddLogMessage(string.Format("CreditCallCancel Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4}",
                                        "error",
                                        "",
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString()
                                        ), LogLevels.logINFO);




                string strGuid = "";

                if (Session["CreditCallGuid"] != null)
                {
                    strGuid = Session["CreditCallGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid))
                {
                    result = "error";
                    errorMessage = "CreditCall Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    CREDIT_CALL_CONFIGURATION oCreditCallConfiguration = null;

                    if (infraestructureRepository.GetCreditCallConfiguration(strGuid, out oCreditCallConfiguration))
                    {
                        if (oCreditCallConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "CreditCall configuration not found";
                        }
                        else
                        {

                            DateTime dtUTC = (DateTime)Session["utcdate"];


                            if ((oCreditCallConfiguration.CCCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oCreditCallConfiguration.CCCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("CreditCallResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);

                            }
                            else
                            {                                
                                result = "error";
                                errorCode = "transaction_cancelled";
                                errorMessage = "Transaction Cancelled by user";
                            }
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "CreditCall configuration not found";
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
                Session["cardReference"] = strCardReference;
                Session["cardHash"] = strCardHash;
                Session["cardScheme"] = strCardScheme;
                Session["cardPAN"] = strPAN;
                Session["cardExpMonth"] = strExpirationDateMonth;
                Session["cardExpYear"] = strExpirationDateYear;
                Session["chargeDateTime"] = strCreditCallDateTime;
                Session["reference"] = strReference;
                Session["transactionID"] = strTransactionId;
                Session["authCode"] = strAuthCode;
                Session["authResult"] = strAuthResult;



                strRedirectionURLLog = string.Format("CreditCallResult?result={0}" +
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
                                                    "&authCode={12}"+
                                                    "&authResult={13}",
                    Server.UrlEncode(result),
                    Server.UrlEncode(errorCode),
                    Server.UrlEncode(errorMessage),
                    Server.UrlEncode(strCardReference),
                    Server.UrlEncode(strCardHash),
                    Server.UrlEncode(strCardScheme),
                    Server.UrlEncode(strPAN),
                    Server.UrlEncode(strExpirationDateMonth),
                    Server.UrlEncode(strExpirationDateYear),
                    Server.UrlEncode(strCreditCallDateTime),
                    Server.UrlEncode(strReference),
                    Server.UrlEncode(strTransactionId),
                    Server.UrlEncode(strAuthCode),
                    Server.UrlEncode(strAuthResult));

                Logger_AddLogMessage(string.Format("CreditCallCancel End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("CreditCallResult?r={0}", CreditCallResultCalc());

            }

            return Redirect(strRedirectionURL);
        }



       



        public ActionResult CreditCallFailure()
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "CreditCallResult";
            string strAuthResult = "";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strCreditCallDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";


            try
            {

                strAuthResult = Request["ekashu_auth_result"];


                Logger_AddLogMessage(string.Format("CreditCallFailure Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4}",
                                        "error",
                                        "",
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString()
                                        ), LogLevels.logINFO);




                string strGuid = "";

                if (Session["CreditCallGuid"] != null)
                {
                    strGuid = Session["CreditCallGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid))
                {
                    result = "error";
                    errorMessage = "CreditCall Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    CREDIT_CALL_CONFIGURATION oCreditCallConfiguration = null;

                    if (infraestructureRepository.GetCreditCallConfiguration(strGuid, out oCreditCallConfiguration))
                    {
                        if (oCreditCallConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "CreditCall configuration not found";
                        }
                        else
                        {

                            DateTime dtUTC = (DateTime)Session["utcdate"];


                            if ((oCreditCallConfiguration.CCCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oCreditCallConfiguration.CCCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("CreditCallResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);

                            }
                            else
                            {
                                result = "error";
                                errorCode = "transaction_failed";
                                errorMessage = "Transaction Failed";
                            }
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "CreditCall configuration not found";
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
                Session["cardReference"] = strCardReference;
                Session["cardHash"] = strCardHash;
                Session["cardScheme"] = strCardScheme;
                Session["cardPAN"] = strPAN;
                Session["cardExpMonth"] = strExpirationDateMonth;
                Session["cardExpYear"] = strExpirationDateYear;
                Session["chargeDateTime"] = strCreditCallDateTime;
                Session["reference"] = strReference;
                Session["transactionID"] = strTransactionId;
                Session["authCode"] = strAuthCode;
                Session["authResult"] = strAuthResult;



                strRedirectionURLLog = string.Format("CreditCallResult?result={0}" +
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
                                                    "&authCode={12}",
                                                    "&authResult={13}",
                    Server.UrlEncode(result),
                    Server.UrlEncode(errorCode),
                    Server.UrlEncode(errorMessage),
                    Server.UrlEncode(strCardReference),
                    Server.UrlEncode(strCardHash),
                    Server.UrlEncode(strCardScheme),
                    Server.UrlEncode(strPAN),
                    Server.UrlEncode(strExpirationDateMonth),
                    Server.UrlEncode(strExpirationDateYear),
                    Server.UrlEncode(strCreditCallDateTime),
                    Server.UrlEncode(strReference),
                    Server.UrlEncode(strTransactionId),
                    Server.UrlEncode(strAuthCode),
                    Server.UrlEncode(strAuthResult));

                Logger_AddLogMessage(string.Format("CreditCallFailure End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("CreditCallResult?r={0}", CreditCallResultCalc());

            }

            return Redirect(strRedirectionURL);
        }


       
        [HttpGet]
        public ActionResult CreditCallResult(string r)
        {


            if (string.IsNullOrEmpty(r))
            {
                return new HttpNotFoundResult("");
            }
            else
            {

                if (Session["ReturnURL"] != null && !string.IsNullOrEmpty(Session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    postData.Add("r", r);

                    RedirectWithData(Session["ReturnURL"].ToString(), postData);
                }
                else
                {

                    //string strResultDec = DecryptCryptResult(r, Session["HashSeed"].ToString());
                    //ViewData["Result"] = strResultDec;

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
                    Session["CreditCallGuid"] = null;
                    Session["ResultURL"] = null;
                }
                return View();
            }

        }




        private string CreditCallResultCalc()
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
                oDataDict["ekashu_card_reference"] = Session["cardReference"];
                oDataDict["ekashu_card_hash"] = Session["cardHash"];
                oDataDict["ekashu_card_scheme"] = Session["cardScheme"];
                oDataDict["ekashu_masked_card_number"] = Session["cardPAN"];
                oDataDict["ekashu_expires_end_month"] = Session["cardExpMonth"];
                oDataDict["ekashu_expires_end_year"] = Session["cardExpYear"];
                oDataDict["ekashu_date_time_local_fmt"] = Session["chargeDateTime"];
                oDataDict["ekashu_reference"] = Session["reference"];
                oDataDict["ekashu_transaction_id"] = Session["transactionID"];
                oDataDict["ekashu_auth_code"] = Session["authCode"];
                oDataDict["ekashu_auth_result"] = Session["authResult"];
            }

            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("CreditCallResultCalc: {0}",
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
