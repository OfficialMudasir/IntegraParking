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
    public class IECISAController : Controller
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(IECISAController));

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;


        private const long BIG_PRIME_NUMBER = 472189635;
        private const long BIG_PRIME_NUMBER2 = 624159837;
        private const string DEFAULT_IMAGE_URL = "https://IECISA.com/img/documentation/checkout/marketplace.png";


        public IECISAController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

        public ActionResult iecisaRequest(string Guid, 
                                          string Email, 
                                          int? Amount, 
                                          string CurrencyISOCODE, 
                                          string Description, 
                                          string UTCDate,
                                          string Culture,
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


                Logger_AddLogMessage(string.Format("IECISARequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description), LogLevels.logINFO);



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

                            string strCalcHash = CalculateHash(Guid, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, Culture, oIECISAConfiguration.IECCON_HASH_SEED);


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
                                        DateTime dtNow = DateTime.Now;
                                        DateTime dtUTCNow = DateTime.UtcNow;

                                        IECISAPayments cardPayment = new IECISAPayments();

                                        var uri = new Uri(Request.Url.AbsoluteUri);
                                        string strURLPath = Request.Url.AbsoluteUri.Substring(0,Request.Url.AbsoluteUri.LastIndexOf("/"));
                                        string strLang = ((Culture ?? "").Length >= 2) ? Culture.Substring(0, 2) : "ES";

                                        IECISAPayments.IECISAErrorCode eErrorCode;

                                        cardPayment.StartWebTransaction(oIECISAConfiguration.IECCON_CF_USER,
                                                                        oIECISAConfiguration.IECCON_CF_MERCHANT_ID,
                                                                        oIECISAConfiguration.IECCON_CF_INSTANCE,
                                                                        oIECISAConfiguration.IECCON_CF_CENTRE_ID,
                                                                        oIECISAConfiguration.IECCON_CF_POS_ID,
                                                                        oIECISAConfiguration.IECCON_SERVICE_URL,
                                                                        oIECISAConfiguration.IECCON_SERVICE_TIMEOUT,
                                                                        oIECISAConfiguration.IECCON_MAC_KEY,
                                                                        oIECISAConfiguration.IECCON_CF_TEMPLATE,
                                                                        strURLPath+"/iecisaResponse",
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
                                                                        out strOpReference);

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
                    Server.UrlEncode(transactionId));

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
       
        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + Culture;

            return CalculateHash(strHashString,strHashSeed);

        }



        private string CalculateHash(string strInput, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;
                HMACSHA256 _hmacsha256 = null;
                int iKeyLength = 64;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }

                _hmacsha256 = new HMACSHA256(_normKey);

                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = null;


                hash = _hmacsha256.ComputeHash(inputBytes);


                if (hash.Length >= 8)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = hash.Length - 8; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    strRes = sb.ToString();
                }


            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateHash::Exception", LogLevels.logERROR);

            }


            return strRes;
        }


        private string CalculateCryptResult(string strInput, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;
                
                int iKeyLength = 32;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }


                byte[] _iv = null;

                int iIVLength = 16;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _iv = new byte[iIVLength];
                iSum = 0;

                for (int i = 0; i < iIVLength; i++)
                {
                    if (i < ivBytes.Length)
                    {
                        iSum += ivBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _iv[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER2) % (Byte.MaxValue + 1));

                }

                strRes = ByteArrayToString(EncryptStringToBytes_Aes(strInput, _normKey, _iv));



            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateHash::Exception", LogLevels.logERROR);

            }


            return strRes;
        }


        private string DecryptCryptResult(string strHexByteArray, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;

                int iKeyLength = 32;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }


                byte[] _iv = null;

                int iIVLength = 16;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _iv = new byte[iIVLength];
                iSum = 0;

                for (int i = 0; i < iIVLength; i++)
                {
                    if (i < ivBytes.Length)
                    {
                        iSum += ivBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _iv[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER2) % (Byte.MaxValue + 1));

                }

                strRes = DecryptStringFromBytes_Aes(StringToByteArray(strHexByteArray), _normKey, _iv);



            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateHash::Exception", LogLevels.logERROR);

            }


            return strRes;
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        static string PrettyJSON(string json)
        {

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string strRes = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }
      

        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }


        private static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }
    }
}
