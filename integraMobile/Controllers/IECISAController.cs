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
    public class IECISAController : BaseCCController
    {

        public IECISAController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

        /*
        http://localhost:4091/iecisa/iecisaRequest?Guid=a18bdd08-5219-493a-9e7c-035a24d5f2ca&Email=febermejo@gmail.com&Amount=100&CurrencyISOCODE=EUR&Description=fasdfasddfa&Culture=ES&UTCDate=231200111217&Hash=32423
        */


        [HttpPost]
        public ActionResult iecisaRequest()
        {



            return iecisaRequest(Request["Guid"],
                                Request["Email"],
                                (Request["Amount"] != null ? Convert.ToInt32(Request["Amount"]) : (int?)null),
                                Request["CurrencyISOCODE"],
                                Request["Description"],
                                Request["UTCDate"],
                                Request["Culture"],
                                Request["ReturnURL"],
                                Request["Hash"]);
        }

        [HttpGet]
        public ActionResult iecisaRequest(string Guid, 
                                          string Email, 
                                          int? Amount, 
                                          string CurrencyISOCODE, 
                                          string Description, 
                                          string UTCDate,
                                          string Culture,
                                          string ReturnURL,
                                          string Hash)
        {           
            string result = "";
            string errorMessage = "";
            string errorCode = "";



            try
            {

                IECISA_CONFIGURATION oIECISAConfiguration = null;
                Session["result"] = null;
                Session["errorCode"] = null;
                Session["errorMessage"] = null;
                Session["email"] = null;
                Session["amount"] = null;
                Session["currency"] = null;
                Session["utcdate"] = null;
                Session["IECISAGuid"] = null;
                Session["cardToken"] = null;
                Session["cardScheme"] = null;
                Session["cardPAN"] = null;
                Session["cardExpirationDate"] = null;
                Session["chargeID"] = null;
                Session["chargeDateTime"] = null;
                Session["HashSeed"] = null;
                Session["customerID"] = null;
                Session["ReturnURL"] = null;


                Logger_AddLogMessage(string.Format("IECISARequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5} ; ReturnURL={6}",
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
                    (string.IsNullOrEmpty(Culture)) ||
                    (string.IsNullOrEmpty(Hash)))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {

                    if (infraestructureRepository.GetIECISAConfiguration(Guid, out oIECISAConfiguration))
                    {
                        if (oIECISAConfiguration != null)
                        {
                            Session["HashSeed"] = oIECISAConfiguration.IECCON_HASH_SEED;

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, Culture, ReturnURL, oIECISAConfiguration.IECCON_HASH_SEED);


                            if ((oIECISAConfiguration.IECCON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("IECISARequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oIECISAConfiguration.IECCON_CHECK_DATE_AND_HASH == 0) ||
                                         (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oIECISAConfiguration.IECCON_CONFIRMATION_TIME))
                                    {
                                        string strTransactionId = null;
                                        string strOpReference = "";
                                        string strCardHash = "";
                                        DateTime dtNow = DateTime.Now;
                                        DateTime dtUTCNow = DateTime.UtcNow;

                                        IECISAPayments cardPayment = new IECISAPayments();

                                        var uri = new Uri(Request.Url.AbsoluteUri);
                                        string strURLPath = Request.Url.AbsoluteUri.Substring(0,Request.Url.AbsoluteUri.LastIndexOf("/"));
                                        string strLang = ((Culture ?? "").Length >= 2) ? Culture.Substring(0, 2) : "ES";

                                        IECISAPayments.IECISAErrorCode eErrorCode;

                                        cardPayment.StartWebTransaction(oIECISAConfiguration.IECCON_FORMAT_ID,
                                                                        oIECISAConfiguration.IECCON_CF_USER,
                                                                        oIECISAConfiguration.IECCON_CF_MERCHANT_ID,
                                                                        oIECISAConfiguration.IECCON_CF_INSTANCE,
                                                                        oIECISAConfiguration.IECCON_CF_CENTRE_ID,
                                                                        oIECISAConfiguration.IECCON_CF_POS_ID,
                                                                        oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                        oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                        oIECISAConfiguration.IECCON_MAC_KEY,
                                                                        oIECISAConfiguration.IECCON_CF_TEMPLATE,
                                                                        oIECISAConfiguration.IECCON_CUSTOMER_ID,
                                                                        strURLPath + "/iecisaResponse",
                                                                        strURLPath + "/iecisaResponse",
                                                                        Email,
                                                                        strLang,
                                                                        Amount.Value,
                                                                        CurrencyISOCODE,
                                                                        infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(CurrencyISOCODE),
                                                                        true,
                                                                        dtNow,
                                                                        out eErrorCode,
                                                                        out errorMessage,
                                                                        out strTransactionId,
                                                                        out strOpReference,
                                                                        out strCardHash);

                                        if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                        {
                                            result = "error";
                                            errorCode = eErrorCode.ToString();

                                            Logger_AddLogMessage(string.Format("IECISARequest.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                                                      errorCode, errorMessage), LogLevels.logINFO);


                                        }
                                        else
                                        {
                                            string strRedirectURL = "";
                                            cardPayment.GetWebTransactionPaymentTypes(oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                                    oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                                    strTransactionId,
                                                                                    out eErrorCode,
                                                                                    out errorMessage,
                                                                                    out strRedirectURL);
                                            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                            {
                                                result = "error";
                                                errorCode = eErrorCode.ToString();

                                                Logger_AddLogMessage(string.Format("IECISARequest.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                                          errorCode, errorMessage), LogLevels.logINFO);


                                            }
                                            else
                                            {
                                                customersRepository.StartRecharge(oIECISAConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                          Email,
                                                          dtUTCNow,
                                                          dtNow,
                                                          Amount.Value,
                                                          infraestructureRepository.GetCurrencyFromIsoCode(CurrencyISOCODE),
                                                          "",
                                                          strOpReference,
                                                          strTransactionId,
                                                          "",
                                                          "",
                                                          "",
                                                          PaymentMeanRechargeStatus.Committed);
                                             
                                                result = "succeeded";
                                                errorCode = eErrorCode.ToString();

                                                Session["email"] = Email;
                                                Session["amount"] = Amount;
                                                Session["utcdate"] = dtUTC;
                                                Session["IECISAGuid"] = Guid;
                                                Session["currency"] = CurrencyISOCODE;
                                                Session["cardHash"] = strCardHash;
                                               


                                                return Redirect(strRedirectURL);


                                            }

                                        }
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("IECISARequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("IECISARequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "IECISA configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "IECISA configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage="IECISARequest Method Exception";
            }
        
            if (!string.IsNullOrEmpty(errorCode))
            {

                Session["result"] = result;
                Session["errorCode"] = errorCode;
                Session["errorMessage"] = errorMessage;


                string strRedirectionURLLog = string.Format("iecisaResult?result={0}&errorCode={1}&errorMessage={2}", Server.UrlEncode(result), Server.UrlEncode(errorCode), Server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("iecisaResult?r={0}",IECISAResultCalc());
                Logger_AddLogMessage(string.Format("IECISARequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",     
                                        Guid,               
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {


                Logger_AddLogMessage(string.Format("IECISARequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
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
        public ActionResult iecisaResponse(string transactionId)
        {            
            string result="";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "iecisaResult";
            string strCardToken = "";
            string strCardHash = "";
            string strCardScheme = "";
            string strIECISADateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strOpReference = "";
            string strAuthCode = "";
            string strCFTransactionID = "";



            try
            {
                Logger_AddLogMessage(string.Format("iecisaResponse Begin: Transaction={0} ; Email={1} ; Amount={2} ; Currency={3}",
                                        transactionId,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString()
                                        ), LogLevels.logINFO);


                string strGuid = "";

                if (Session["IECISAGuid"] != null)
                {
                    strGuid = Session["IECISAGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid))
                {
                    result = "error";
                    errorMessage = "IECISA Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    IECISA_CONFIGURATION oIECISAConfiguration = null;

                    if (infraestructureRepository.GetIECISAConfiguration(strGuid, out oIECISAConfiguration))
                    {
                        if (oIECISAConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "IECISA configuration not found";
                        }
                        else
                        {

                            DateTime dtUTC = (DateTime)Session["utcdate"];


                            if ((oIECISAConfiguration.IECCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oIECISAConfiguration.IECCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("iecisaResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);

                            }
                            else
                            {
                                IECISAPayments cardPayment = new IECISAPayments();
                                IECISAPayments.IECISAErrorCode eErrorCode;
                                string strMaskedCardNumber="";
                                string strCardReference="";
                                strCardHash = Session["cardHash"].ToString();
                                DateTime? dtExpDate=null;
                                DateTime? dtTransactionDate=null;
                                string strExpMonth = "";
                                string strExpYear = "";


                                cardPayment.GetTransactionStatus(oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                                    oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                                    oIECISAConfiguration.IECCON_MAC_KEY,
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
                                    result = "error";
                                    errorCode = eErrorCode.ToString();

                                    Logger_AddLogMessage(string.Format("iecisaResponse.GetTransactionStatus : errorCode={0} ; errorMessage={1}",
                                              errorCode, errorMessage), LogLevels.logINFO);

                                    customersRepository.FailedRecharge(oIECISAConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                       Session["email"].ToString(),
                                                                       transactionId,
                                                                       PaymentMeanRechargeStatus.Cancelled);

                                }
                                else
                                {
                                    result = "succeeded";
                                    customersRepository.CompleteStartRecharge(oIECISAConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                      Session["email"].ToString(),
                                                      transactionId,
                                                      result,
                                                      strCFTransactionID,
                                                      dtTransactionDate.Value.ToString("HHmmssddMMyyyy"),
                                                      strAuthCode,
                                                      PaymentMeanRechargeStatus.Committed);

                                    
                                    errorCode = eErrorCode.ToString();
                                    strCardToken = strCardReference;
                                    strCardScheme = "";
                                    strPAN = strMaskedCardNumber;
                                    strExpirationDateMonth = strExpMonth;
                                    strExpirationDateYear = strExpYear;
                                    strIECISADateTime = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                                   



          /**-------*
                                    string strErrorMessage;
                                    string strTransactionId2;
                                    string strCFTransactionID2;
                                    string strAuthCode2;
                                    DateTime? dtTransactionDate2;
                                    string strUserReference;
                                    cardPayment.StartAutomaticTransaction(oIECISAConfiguration.IECCON_FORMAT_ID, 
                                                                                  oIECISAConfiguration.IECCON_CF_USER,
                                                                                  oIECISAConfiguration.IECCON_CF_MERCHANT_ID,
                                                                                  oIECISAConfiguration.IECCON_CF_INSTANCE,
                                                                                  oIECISAConfiguration.IECCON_CF_CENTRE_ID,
                                                                                  oIECISAConfiguration.IECCON_CF_POS_ID,
                                                                                  oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                                  oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                                  oIECISAConfiguration.IECCON_MAC_KEY,
                                                                                  oIECISAConfiguration.IECCON_CUSTOMER_ID,
                                                                                  oIECISAConfiguration.IECCON_CF_TEMPLATE,
                                                                                  strCardReference,
                                                                                  Session["email"].ToString(),
                                                                                  Convert.ToInt32(Session["amount"]),
                                                                                  Session["currency"].ToString(),
                                                                                  infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(Session["currency"].ToString()),
                                                                                  DateTime.Now,
                                                                                  out eErrorCode,
                                                                                  out strErrorMessage,
                                                                                  out strTransactionId2,
                                                                                  out strUserReference);

                                    string strRedirectURL = "";
                                    cardPayment.GetWebTransactionPaymentTypes(oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                            oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                            strTransactionId2,
                                                                            out eErrorCode,
                                                                            out strErrorMessage,
                                                                            out strRedirectURL);

                                    cardPayment.CompleteAutomaticTransaction(oIECISAConfiguration.IECCON_SERVICE_URL,
                                                            oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                            oIECISAConfiguration.IECCON_MAC_KEY,
                                                            strTransactionId2,
                                                            out eErrorCode,
                                                            out strErrorMessage,
                                                            out dtTransactionDate2,
                                                            out strCFTransactionID2,
                                                            out strAuthCode2);


                                    string strRefundTransactionId1;
                                    string strRefundTransactionId2;
                                    cardPayment.RefundTransaction(oIECISAConfiguration.IECCON_FORMAT_ID,
                                            oIECISAConfiguration.IECCON_CF_USER,
                                            oIECISAConfiguration.IECCON_CF_MERCHANT_ID,
                                            oIECISAConfiguration.IECCON_CF_INSTANCE,
                                            oIECISAConfiguration.IECCON_CF_CENTRE_ID,
                                            oIECISAConfiguration.IECCON_CF_POS_ID,
                                            oIECISAConfiguration.IECCON_REFUNDSERVICE_URL,
                                            oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                            oIECISAConfiguration.IECCON_MAC_KEY,
                                            oIECISAConfiguration.IECCON_CUSTOMER_ID,
                                            oIECISAConfiguration.IECCON_CF_TEMPLATE,
                                            transactionId,
                                            strCFTransactionID,
                                            dtTransactionDate.Value,
                                            strAuthCode,
                                            Convert.ToInt32(Session["amount"]),
                                            Session["currency"].ToString(),
                                            infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(Session["currency"].ToString()),
                                            DateTime.Now,
                                            out eErrorCode,
                                            out errorMessage,
                                            out strRefundTransactionId1);


                                    cardPayment.RefundTransaction(oIECISAConfiguration.IECCON_FORMAT_ID,
                                            oIECISAConfiguration.IECCON_CF_USER,
                                            oIECISAConfiguration.IECCON_CF_MERCHANT_ID,
                                            oIECISAConfiguration.IECCON_CF_INSTANCE,
                                            oIECISAConfiguration.IECCON_CF_CENTRE_ID,
                                            oIECISAConfiguration.IECCON_CF_POS_ID,
                                            oIECISAConfiguration.IECCON_REFUNDSERVICE_URL,
                                            oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                            oIECISAConfiguration.IECCON_MAC_KEY,
                                            oIECISAConfiguration.IECCON_CUSTOMER_ID,
                                            oIECISAConfiguration.IECCON_CF_TEMPLATE,
                                            strTransactionId2,
                                            strCFTransactionID2,
                                            dtTransactionDate2.Value,
                                            strAuthCode2,
                                            Convert.ToInt32(Session["amount"]),
                                            Session["currency"].ToString(),
                                            infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(Session["currency"].ToString()),
                                            DateTime.Now,
                                            out eErrorCode,
                                            out errorMessage,
                                            out strRefundTransactionId2);



                                    string strTokenDeletionTrans = "";
                                    cardPayment.StartTokenDeletion(oIECISAConfiguration.IECCON_FORMAT_ID,
                                            oIECISAConfiguration.IECCON_CF_USER,
                                            oIECISAConfiguration.IECCON_CF_MERCHANT_ID,
                                            oIECISAConfiguration.IECCON_CF_INSTANCE,
                                            oIECISAConfiguration.IECCON_CF_CENTRE_ID,
                                            oIECISAConfiguration.IECCON_CF_POS_ID,
                                            oIECISAConfiguration.IECCON_SERVICE_URL,
                                            oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                            oIECISAConfiguration.IECCON_MAC_KEY,
                                            oIECISAConfiguration.IECCON_CUSTOMER_ID,
                                            oIECISAConfiguration.IECCON_CF_TEMPLATE,
                                            strCardReference,
                                            strCardHash,
                                            DateTime.Now,
                                            out eErrorCode,
                                            out strErrorMessage,
                                            out strTokenDeletionTrans);

                                  
                                    cardPayment.GetWebTransactionPaymentTypes(oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                            oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                            strTokenDeletionTrans,
                                                                            out eErrorCode,
                                                                            out strErrorMessage,
                                                                            out strRedirectURL);
                                       
                                    cardPayment.CompleteTokenDeletion(oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                      oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                      oIECISAConfiguration.IECCON_MAC_KEY,
                                                                      strTokenDeletionTrans,
                                                                      1,
                                                                      5,
                                                                    out eErrorCode,
                                                                    out strErrorMessage,
                                                                    out dtTransactionDate,
                                                                    out strCFTransactionID,
                                                                    out strAuthCode);


          *-------*/





                                }


                            }
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "IECISA configuration not found";
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
                Session["cardHash"] = strCardHash;
                Session["cardScheme"] = strCardScheme;
                Session["cardPAN"] = strPAN;
                Session["cardExpirationDate"] = strExpirationDateMonth+strExpirationDateYear;
                Session["chargeDateTime"] = strIECISADateTime;
                Session["cardCFTicketNumber"] = strOpReference;
                Session["cardCFAuthCode"] = strAuthCode;
                Session["cardCFTransactionID"] = strCFTransactionID;
                Session["cardTransactionID"] = transactionId;


                strRedirectionURLLog = string.Format("iecisaResult?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardToken={3}" +
                                                    "&cardHash={12}" +
                                                    "&cardScheme={4}" +
                                                    "&cardPan={5}" +
                                                    "&cardExpirationDate={6}" +
                                                    "&chargeDateTime={7}" +
                                                    "&opReference={8}" +
                                                    "&AuthCode={9}" +
                                                    "&CFTransaction={10}" +
                                                    "&Transaction={11}",
                    Server.UrlEncode(result),
                    Server.UrlEncode(errorCode),
                    Server.UrlEncode(errorMessage),
                    Server.UrlEncode(strCardToken),
                    Server.UrlEncode(strCardScheme),
                    Server.UrlEncode(strPAN),
                    Server.UrlEncode(strExpirationDateMonth + strExpirationDateYear),
                    Server.UrlEncode(strIECISADateTime),
                    Server.UrlEncode(strOpReference),
                    Server.UrlEncode(strAuthCode),
                    Server.UrlEncode(strCFTransactionID),
                    Server.UrlEncode(transactionId),
                    Server.UrlEncode(strCardHash));

                Logger_AddLogMessage(string.Format("iecisaResponse End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardToken,
                                        Session["email"].ToString(),
                                        Session["amount"].ToString(),
                                        Session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("iecisaResult?r={0}", IECISAResultCalc());
     
            }
            
            return Redirect(strRedirectionURL);
        }



        [HttpGet]
        public ActionResult iecisaResult(string r)
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
                    Session["email"] = null;
                    Session["amount"] = null;
                    Session["currency"] = null;
                    Session["utcdate"] = null;
                    Session["IECISAGuid"] = null;
                    Session["cardToken"] = null;
                    Session["cardScheme"] = null;
                    Session["cardPAN"] = null;
                    Session["cardExpirationDate"] = null;
                    Session["chargeDateTime"] = null;
                    Session["HashSeed"] = null;
                    Session["cardCFTicketNumber"] = null;
                    Session["cardCFAuthCode"] = null;
                    Session["cardCFTransactionID"] = null;
                    Session["cardTransactionID"] = null;
                    Session["cardHash"] = null;
                    Session["ResultURL"] = null;
                }

                return View();
            }
            
        }


       
        private string IECISAResultCalc()
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
                oDataDict["cardToken"] = Session["cardToken"];
                oDataDict["cardHash"] = Session["cardHash"];
                oDataDict["cardScheme"] = Session["cardScheme"];
                oDataDict["cardPAN"] = Session["cardPAN"];
                oDataDict["cardExpirationDate"] = Session["cardExpirationDate"];
                oDataDict["chargeDateTime"] = Session["chargeDateTime"];
                oDataDict["cardCFAuthCode"] = Session["cardCFAuthCode"];
                oDataDict["cardCFTicketNumber"] = Session["cardCFTicketNumber"];
                oDataDict["cardCFTransactionID"] = Session["cardCFTransactionID"];
                oDataDict["cardTransactionID"] = Session["cardTransactionID"];
            }


            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("IECISAResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes=CalculateCryptResult(json, Session["HashSeed"].ToString());

            return strRes;
            

        }
       
        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture,string ReturnURL, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + Culture + ReturnURL;

            return CalculateHash(strHashString,strHashSeed);

        }

    }
}
