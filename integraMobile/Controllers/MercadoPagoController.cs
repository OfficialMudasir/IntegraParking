using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using integraMobile.Helper;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class MercadoPagoController : BaseCCController
    {
        //http://localhost/integraMobile/MercadoPago/MercadoPagoRequest?Guid=33631fe0-3221-4f44-9ea6-9c95973b0ab3&Email=febermejo2@gmail.com&Amount=5000&CurrencyISOCODE=ARS&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
        //http://localhost/integraMobile/MercadoPago/MercadoPagoCVVRequest?Guid=33631fe0-3221-4f44-9ea6-9c95973b0ab3&cardId=1646133122220&cvvLength=3&UTCDate=231200111217&Hash=32423
        //http://localhost/integraMobile/MercadoPago/MercadoPagoProRequest?Guid=ea4e072a-eac0-43ad-b44d-8c6a525ce919&Email=febermejo2@gmail.com&Amount=5000&CurrencyISOCODE=ARS&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
        //http://localhost/integraMobile/MercadoPago/MercadoPagoProSuccess?collection_id=23036429591&collection_status=approved&payment_id=23036429591&status=approved&external_reference=20220609154318038419&payment_type=credit_card&merchant_order_id=4937644495&preference_id=1102379214-7d05a861-d8bc-4121-a8d1-0aefb40e3ea9&site_id=MLA&processing_mode=aggregator&merchant_account_id=null
        //http://localhost/integraMobile/MercadoPago/MercadoPagoProFailure?collection_id=null&collection_status=null&payment_id=null&status=null&external_reference=20220609154318038419&payment_type=null&merchant_order_id=null&preference_id=1102379214-7d05a861-d8bc-4121-a8d1-0aefb40e3ea9&site_id=MLA&processing_mode=aggregator&merchant_account_id=null
        public MercadoPagoController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }


        public MercadoPagoController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, System.Web.HttpRequestBase iRequest, System.Web.HttpServerUtilityBase iServer, System.Web.HttpSessionStateBase iSession, decimal? Config_id)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
            _request = iRequest;
            _server = iServer;
            _session = iSession;
            bAvoidHashCheck = true;
            if (Config_id.HasValue)
                Configuration_Id = Config_id != 0 ? Config_id : null;
            else
                Configuration_Id = null;
        }

        [HttpPost]
        public ActionResult MercadoPagoRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return MercadoPagoRequest(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),
                                _request["CurrencyISOCODE"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],
                                "",
                                _request["Hash"]);
        }

        [HttpGet]
        public ActionResult MercadoPagoRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture, string ReturnURL, string ExternalId, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";


            try
            {
                if (_session == null) _session = Session;
                MercadoPagoPayments oPayments = new MercadoPagoPayments();
                string strOrderId = MercadoPagoPayments.UserReference();
                _session["sessionid"] = strOrderId;


                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                _session["cardid"] = null;
                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["MercadoPagoGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["cardExpMonth"] = null;
                _session["cardExpYear"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["sessionid"] = null;
                _session["orderid"] = null;
                _session["MD"] = null;
                _session["CAVV"] = null;
                _session["ECI"] = null;
                _session["ExternalId"] = null;
                _session["token"] = null;




                Logger_AddLogMessage(string.Format("MercadoPagoRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}; culture= {7}; ExternalId={8}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        ReturnURL,
                                        Culture,
                                        ExternalId), LogLevels.logINFO);


                _session["ReturnURL"] = ReturnURL;
                _session["ExternalId"] = ExternalId;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value <= 0) ||
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

                    MERCADOPAGO_CONFIGURATION oMercadoPagoConfiguration = null;
                    bool Configuration_OK = false;



                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfigurationById(Configuration_Id, out oMercadoPagoConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfiguration(Guid, out oMercadoPagoConfiguration);
                    }
                                       

                    if (Configuration_OK)
                    {
                        if (oMercadoPagoConfiguration != null)
                        {

                            _session["HashSeed"] = oMercadoPagoConfiguration.MEPACON_HASH_SEED;

                            string strCalcHash = CalculateHash(oMercadoPagoConfiguration.MEPACON_GUID, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, ReturnURL, Culture, ExternalId, oMercadoPagoConfiguration.MEPACON_HASH_SEED);

                            if ((oMercadoPagoConfiguration.MEPACON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("MercadoPagoRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oMercadoPagoConfiguration.MEPACON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oMercadoPagoConfiguration.MEPACON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        string strAmount = (Amount.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);




                                        MercadoPagoPayments.MercadoPagoErrorCode eErrorCode;
                                        _session["sessionid"] = strOrderId;



                                        //                                        oPayments.GetTicketFromDataPreload(oMercadoPagoConfiguration.MEPACON_MercadoPago_FORM_URL, oMercadoPagoConfiguration.MEPACON_PS_STORE_ID, oMercadoPagoConfiguration.MEPACON_HPP_KEY, strOrderId, strAmount, Email, MercadoPago_lang,
                                        //                                            oMercadoPagoConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_FORM_URL, out eErrorCode, out errorMessage, out strHPPID, out strTicket);

                                        eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.Approved;
                                        if (!MercadoPagoPayments.IsError(eErrorCode))
                                        {
                                            string strCulture = "es-AR";

                                            try
                                            {
                                                if (!string.IsNullOrEmpty(Culture))
                                                {
                                                    strCulture = Culture.Replace("_", "-");
                                                }

                                                CultureInfo ci = new CultureInfo(strCulture);
                                                Thread.CurrentThread.CurrentUICulture = ci;
                                                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                                integraMobile.Properties.Resources.Culture = ci;
                                            }
                                            catch
                                            {
                                                strCulture = "es-AR";
                                                CultureInfo ci = new CultureInfo(strCulture);
                                                Thread.CurrentThread.CurrentUICulture = ci;
                                                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                                integraMobile.Properties.Resources.Culture = ci;
                                            }


                                            string strFullURL = _request.Url.AbsoluteUri;
                                            string strBaseURL = strFullURL.Substring(0, strFullURL.LastIndexOf("/"));
                                            string strReturnURL = strBaseURL + "/MercadoPagoResponse";


                                            ViewData["MercadoPago_sdk_url"] = oMercadoPagoConfiguration.MEPACON_SDK_URL;
                                            ViewData["MercadoPago_public_key"] = oMercadoPagoConfiguration.MEPACON_PUBLIC_KEY;
                                            ViewData["MercadoPago_response_url"] = strReturnURL;
                                            ViewData["MercadoPago_amount"] = strAmount;
                                            ViewData["MercadoPago_description"] = Description;
                                            ViewData["MercadoPago_email"] = Email;

                                            _session["orderid"] = strOrderId;
                                            _session["email"] = Email;
                                            _session["amount"] = Amount;
                                            _session["currency"] = CurrencyISOCODE;
                                            _session["utcdate"] = dtUTC;
                                            Guid = oMercadoPagoConfiguration.MEPACON_GUID;
                                            _session["MercadoPagoGuid"] = Guid;

                                            Logger_AddLogMessage(string.Format("MercadoPagoRequest Begin: Guid={0}, ConfigurationId={1}", _session["MercadoPagoGuid"].ToString(), Configuration_Id), LogLevels.logINFO);


                                        }
                                        else
                                        {
                                            result = "error";
                                            errorCode = eErrorCode.ToString();
                                            if (string.IsNullOrEmpty(errorMessage))
                                            {
                                                errorMessage = MercadoPagoPayments.ErrorMessageDict[eErrorCode];
                                            }
                                        }

                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("MercadoPagoRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("MercadoPagoRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "MercadoPago configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "MercadoPago configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "MercadoPagoRequest Method Exception";
            }


            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

                SaveSession(_session["sessionid"].ToString());

                string strRedirectionURLLog = string.Format("MercadoPagoResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("MercadoPagoResult?r={0}&s={1}", MercadoPagoResultCalc(), _session["sessionid"]);
                Logger_AddLogMessage(string.Format("MercadoPagoRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {

                SaveSession(_session["sessionid"].ToString());

                Logger_AddLogMessage(string.Format("MercadoPagoRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
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
        public ActionResult MercadoPagoResponse()
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


            string strRedirectionURL = "MercadoPagoResult" + sSuffix;
            string strCardScheme = "";
            string strCardType = "";
            string strMercadoPagoDateTime = "";
            string strPAN = "";            
            string strDoctype = "";
            string strDocNumber = "";

            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strCustomerID = "";
            string strTransactionId = "";
            string strToken = "";
            string strOrderId = "";
            string strCardId = "";
            int iInstallaments = 1;
            int iSecurityCodeLength = 0;


            MercadoPagoPayments oPayments = new MercadoPagoPayments();

            string responseFromServer = "";
            try
            {

                // Get the stream containing content returned by the server.
                Stream dataStream = _request.InputStream;
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();

                Logger_AddLogMessage(string.Format("MercadoPagoResponse response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "MercadoPagoResponse::Exception", LogLevels.logERROR);
            }


            try
            {
                /*
                 * 	{
	                  "token": "253863ed4ba807afbf876c11c79cd16a",
	                  "issuer_id": "3",
	                  "payment_method_id": "master",
	                  "transaction_amount": 50,
	                  "installments": 1,
	                  "description": "fasdfasddfa",
	                  "payer": {
	                    "email": "febermejo@gmail.com",
	                    "identification": {
	                      "type": "DNI",
	                      "number": "46597693"
	                    }
	                  }
	                }
                 * 
                 */


                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                int iCancel = Convert.ToInt32(oResponse["cancel"].ToString());

                if (iCancel == 0)
                {

                    strToken = oResponse["token"].ToString();
                    string strDescription = oResponse["description"].ToString();
                    string strPaymentMethodId = oResponse["payment_method_id"].ToString();
                    string strInstallments = oResponse["installments"].ToString();
                    iInstallaments = Convert.ToInt32(strInstallments);


                    var oPayer = oResponse["payer"];
                    string strPaymentEmail = oPayer["email"].ToString();

                    var oPayerIdentification = oPayer.identification;
                    strDocNumber = oPayerIdentification.number.ToString();
                    strDoctype = oPayerIdentification.type.ToString();

                    NumberFormatInfo provider = new NumberFormatInfo();
                    provider.NumberDecimalSeparator = ".";
                    string strAmount = oResponse["transaction_amount"].ToString();
                    strAmount = strAmount.Replace(",", ".");
                    decimal dAmount = Convert.ToDecimal(strAmount, provider);                    

                    string strGuid = "";

                    if (_session["MercadoPagoGuid"] != null)
                    {
                        strGuid = _session["MercadoPagoGuid"].ToString();
                    }

                    if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                    {
                        result = "error";
                        errorMessage = "MercadoPago Guid not found";
                        errorCode = "invalid_configuration";

                    }
                    else
                    {

                        bool Configuration_OK = false;

                        MERCADOPAGO_CONFIGURATION oMercadoPagoConfiguration = null;


                        if (Configuration_Id != null)
                        {
                            Configuration_OK = infraestructureRepository.GetMercadoPagoConfigurationById(Configuration_Id, out oMercadoPagoConfiguration);
                        }
                        else
                        {
                            Configuration_OK = infraestructureRepository.GetMercadoPagoConfiguration(strGuid, out oMercadoPagoConfiguration);
                        }



                        if (Configuration_OK)
                        {
                            if (oMercadoPagoConfiguration == null)
                            {
                                result = "error";
                                errorCode = "configuration_not_found";
                                errorMessage = "MercadoPago configuration not found";
                            }
                            else
                            {
                                MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;


                                if (oPayments.CreateCardToken(oMercadoPagoConfiguration.MEPACON_API_URL,
                                    oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                    oMercadoPagoConfiguration.MEPACON_SERVICE_TIMEOUT,
                                     strPaymentEmail,
                                     strDoctype,
                                     strDocNumber,
                                     strToken,
                                     ref strCustomerID,
                                     out eErrorCode,
                                     out errorMessage,
                                     out strCardId,
                                     out strCardScheme,
                                     out strCardType,
                                     out strPAN,
                                     out iSecurityCodeLength,
                                     out strExpirationDateMonth,
                                     out strExpirationDateYear,
                                     out strMercadoPagoDateTime))
                                {

                                    if (dAmount > 0)
                                    {
                                        if (_session["orderid"] != null)
                                        {
                                            strOrderId = _session["orderid"].ToString();
                                        }
                                        else
                                        {
                                            strOrderId = MercadoPagoPayments.UserReference(); 
                                        }

                                        if (oPayments.AutomaticTransaction(
                                            oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                             strOrderId,
                                             dAmount,
                                             strDescription,
                                             strToken,
                                             strCustomerID,
                                             iInstallaments,
                                             true,
                                             false,
                                             out eErrorCode,
                                             out errorMessage,
                                             out strTransactionId,
                                             out strMercadoPagoDateTime))
                                        {
                                            int iAmount = Convert.ToInt32(_session["amount"]);
                                            DateTime dtNow = DateTime.Now;
                                            DateTime dtUTCNow = DateTime.UtcNow;
                                            customersRepository.StartRecharge(oMercadoPagoConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                                                    _session["email"].ToString(),
                                                                                                    dtUTCNow,
                                                                                                    dtNow,
                                                                                                    iAmount,
                                                                                                    infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                                                                    "",
                                                                                                    strOrderId,
                                                                                                    strTransactionId,
                                                                                                    "",
                                                                                                    strMercadoPagoDateTime,
                                                                                                    "",
                                                                                                    PaymentMeanRechargeStatus.Committed);


                                            result = "succeeded";
                                            errorCode = eErrorCode.ToString();
                                            
                                            /*strOrderId = MercadoPagoPayments.UserReference();

                                            oPayments.AutomaticTransaction(
                                            oMercadoPagoConfiguration.MEPACON_API_URL,
                                            oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                            oMercadoPagoConfiguration.MEPACON_WS_API_TIMEOUT,
                                             strOrderId,
                                             dAmount,
                                             strDescription,
                                             strCardId,
                                             strCustomerID,
                                             iInstallaments,
                                             true,
                                             out eErrorCode,
                                             out errorMessage,
                                             out strTransactionId);*/


                                            /*oPayments.CommitTransaction(
                                                oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                                 strTransactionId,
                                                 dAmount,
                                                 out eErrorCode,
                                                 out errorMessage);

                                            string strRefundTransactionId = "";
                                            oPayments.RefundTransaction(
                                                 oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                                  strTransactionId,
                                                  dAmount,
                                                  out eErrorCode,
                                                  out errorMessage/*,
                                                  out strRefundTransactionId
                                                  );*/
                                        }
                                        else
                                        {

                                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode2 = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                                            string errorMessage2 = "";

                                            oPayments.DeleteCardToken(
                                                oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                                 strCustomerID,
                                                 strCardId,
                                                 out eErrorCode2,
                                                 out errorMessage2);


                                            result = "error";
                                            errorCode = eErrorCode.ToString();
                                        }

                                    }
                                    else
                                    {
                                        result = "succeeded";
                                        errorCode = eErrorCode.ToString();                                       
                                    }

                                }
                                else
                                {
                                    result = "error";
                                    errorCode = eErrorCode.ToString();
                                }

                            }
                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "MercadoPago configuration not found";
                        }


                    }
                }
                else  //user has cancel 
                {
                    result = "error";
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
                _session["cardReference"] = strCardId;
                _session["cardHash"] = strCustomerID;
                _session["cardScheme"] = strCardScheme;
                _session["cardType"] = strCardType;
                _session["cardPAN"] = strPAN;
                _session["chargeDateTime"] = strMercadoPagoDateTime;
                _session["transactionID"] = strTransactionId;
                _session["installments"] = iInstallaments;
                _session["DocumentId"] = strDocNumber;
                _session["DocumentType"] = strDoctype;
                _session["cardExpMonth"] = strExpirationDateMonth;
                _session["cardExpYear"] = strExpirationDateYear;
                _session["reference"] = strOrderId;
                _session["cvvlength"] = iSecurityCodeLength;

                strRedirectionURLLog = string.Format("MercadoPagoResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardScheme={6}" +
                                                    "&cardPAN={7}" +
                                                    "&chargeDateTime={8}" +
                                                    "&transactionID={9}" +
                                                    "&installments={10}" +
                                                    "&DocumentId={11}" +
                                                    "&DocumentType={12}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(strCardId),
                    _server.UrlEncode(strCustomerID),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strCardType),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strMercadoPagoDateTime),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(iInstallaments.ToString()),
                    _server.UrlEncode(strDocNumber),
                    _server.UrlEncode(strDoctype)
                    );


                Logger_AddLogMessage(string.Format("MercadoPagoResponse End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strToken,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MercadoPagoResult" + sSuffix + "?r={0}", MercadoPagoResultCalc());

            }

            return Redirect(strRedirectionURL);
        }




        [HttpPost]
        public ActionResult MercadoPagoCVVRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return MercadoPagoCVVRequest(_request["Guid"],
                                _request["cardId"],
                                _request["cvvLength"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],
                                "",
                                _request["Hash"]);
        }

        [HttpGet]
        public ActionResult MercadoPagoCVVRequest(string Guid, string cardId, string cvvLength, string UTCDate, string Culture, string ReturnURL, string ExternalId, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";


            try
            {
                if (_session == null) _session = Session;
                MercadoPagoPayments oPayments = new MercadoPagoPayments();
                string strOrderId = MercadoPagoPayments.UserReference();
                _session["sessionid"] = strOrderId;


                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["utcdate"] = null;
                _session["MercadoPagoGuid"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["sessionid"] = null;
                _session["ExternalId"] = null;
                _session["cardid"] = null;
                _session["token"] = null;




                Logger_AddLogMessage(string.Format("MercadoPagoCVVRequest Begin: Guid={0}; CardId={1} ; UTCDate = {2} ; ReturnURL={3}; culture= {4}; ExternalId={5}; CVVLength={6}",
                                        Guid,
                                        cardId,
                                        UTCDate,
                                        ReturnURL,
                                        Culture,
                                        ExternalId,
                                        cvvLength), LogLevels.logINFO);


                _session["ReturnURL"] = ReturnURL;
                _session["ExternalId"] = ExternalId;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(cardId)) ||
                    (string.IsNullOrEmpty(cvvLength)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Hash) && bAvoidHashCheck == false))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }
                else
                {
                    int iSecurityCodeLength = 0;

                    try
                    {
                        iSecurityCodeLength = Convert.ToInt32(cvvLength);
                    }
                    catch
                    {

                        result = "error";
                        errorMessage = "Invalid or missing parameter";
                        errorCode = "invalid_parameter";

                    }

                    if (iSecurityCodeLength > 0)
                    {

                        MERCADOPAGO_CONFIGURATION oMercadoPagoConfiguration = null;
                        bool Configuration_OK = false;


                        if (Configuration_Id != null)
                        {
                            Configuration_OK = infraestructureRepository.GetMercadoPagoConfigurationById(Configuration_Id, out oMercadoPagoConfiguration);
                        }
                        else
                        {
                            Configuration_OK = infraestructureRepository.GetMercadoPagoConfiguration(Guid, out oMercadoPagoConfiguration);
                        }


                        if (Configuration_OK)
                        {
                            if (oMercadoPagoConfiguration != null)
                            {

                                _session["HashSeed"] = oMercadoPagoConfiguration.MEPACON_HASH_SEED;

                                string strCalcHash = CalculateHash(oMercadoPagoConfiguration.MEPACON_GUID, cardId, cvvLength, UTCDate, Culture, ReturnURL, ExternalId, oMercadoPagoConfiguration.MEPACON_HASH_SEED);

                                if ((oMercadoPagoConfiguration.MEPACON_CHECK_DATE_AND_HASH == 0) ||
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
                                        Logger_AddLogMessage(string.Format("MercadoPagoCVVRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }

                                    if (string.IsNullOrEmpty(result))
                                    {

                                        if ((oMercadoPagoConfiguration.MEPACON_CHECK_DATE_AND_HASH == 0) ||
                                            (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oMercadoPagoConfiguration.MEPACON_CONFIRMATION_TIME) ||
                                            (bAvoidHashCheck == true))
                                        {



                                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode;
                                            _session["sessionid"] = strOrderId;


                                            eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.Approved;
                                            if (!MercadoPagoPayments.IsError(eErrorCode))
                                            {

                                                string strCulture = "es-AR";

                                                try
                                                {
                                                    if (!string.IsNullOrEmpty(Culture))
                                                    {
                                                        strCulture = Culture.Replace("_", "-");
                                                    }

                                                    CultureInfo ci = new CultureInfo(strCulture);
                                                    Thread.CurrentThread.CurrentUICulture = ci;
                                                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                                    integraMobile.Properties.Resources.Culture = ci;
                                                }
                                                catch
                                                {
                                                    strCulture = "es-AR";
                                                    CultureInfo ci = new CultureInfo(strCulture);
                                                    Thread.CurrentThread.CurrentUICulture = ci;
                                                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                                    integraMobile.Properties.Resources.Culture = ci;
                                                }

                                                string strFullURL = _request.Url.AbsoluteUri;
                                                string strBaseURL = strFullURL.Substring(0, strFullURL.LastIndexOf("/"));
                                                string strReturnURL = strBaseURL + "/MercadoPagoCVVResponse";

                                                ViewData["MercadoPago_sdk_url"] = oMercadoPagoConfiguration.MEPACON_SDK_URL;
                                                ViewData["MercadoPago_public_key"] = oMercadoPagoConfiguration.MEPACON_PUBLIC_KEY;
                                                ViewData["MercadoPago_response_url"] = strReturnURL;
                                                ViewData["MercadoPago_cardid"] = cardId;
                                                ViewData["MercadoPago_cvvlength"] = iSecurityCodeLength;

                                                _session["cardid"] = cardId;
                                                _session["utcdate"] = dtUTC;
                                                Guid = oMercadoPagoConfiguration.MEPACON_GUID;
                                                _session["MercadoPagoGuid"] = Guid;

                                                Logger_AddLogMessage(string.Format("MercadoPagoCVVRequest Begin: Guid={0}, ConfigurationId={1}", _session["MercadoPagoGuid"].ToString(), Configuration_Id), LogLevels.logINFO);


                                            }
                                            else
                                            {
                                                result = "error";
                                                errorCode = eErrorCode.ToString();
                                                if (string.IsNullOrEmpty(errorMessage))
                                                {
                                                    errorMessage = MercadoPagoPayments.ErrorMessageDict[eErrorCode];
                                                }
                                            }

                                        }
                                        else
                                        {
                                            result = "error";
                                            errorMessage = "Invalid DateTime";
                                            errorCode = "invalid_datetime";
                                            Logger_AddLogMessage(string.Format("MercadoPagoCVVRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                               UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                        }
                                    }
                                }
                                else
                                {
                                    result = "error";
                                    errorCode = "invalid_hash";
                                    errorMessage = "Invalid Hash";

                                    Logger_AddLogMessage(string.Format("MercadoPagoCVVRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                           Hash, strCalcHash), LogLevels.logINFO);


                                }
                            }
                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "MercadoPago configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "MercadoPago configuration not found";
                    }
                }
            
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "MercadoPagoRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

                SaveSession(_session["sessionid"].ToString());

                string strRedirectionURLLog = string.Format("MercadoPagoResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("MercadoPagoResult?r={0}&s={1}", MercadoPagoResultCalc(), _session["sessionid"]);
                Logger_AddLogMessage(string.Format("MercadoPagoRequest End: Guid={0}; cardId={1} ; ResultURL={2}",
                                        Guid,
                                        cardId,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {

                SaveSession(_session["sessionid"].ToString());

                Logger_AddLogMessage(string.Format("MercadoPagoCVVRequest End: Guid={0}; cardId={1} ; UTCDate = {2}",
                                        Guid,
                                        cardId,
                                        UTCDate), LogLevels.logINFO);
                return View();
            }

        }


        [HttpPost]
        public ActionResult MercadoPagoCVVResponse()
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


            string strRedirectionURL = "MercadoPagoResult" + sSuffix;
            string strToken = "";


            MercadoPagoPayments oPayments = new MercadoPagoPayments();

            string responseFromServer = "";
            try
            {

                // Get the stream containing content returned by the server.
                Stream dataStream = _request.InputStream;
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();

                Logger_AddLogMessage(string.Format("MercadoPagoCVVResponse response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "MercadoPagoCVVResponse::Exception", LogLevels.logERROR);
            }


            try
            {                
                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                int iCancel = Convert.ToInt32(oResponse["cancel"].ToString());

                if (iCancel == 0)
                {

                    int iError = Convert.ToInt32(oResponse["error"].ToString());

                    if (iError == 1)
                    {
                        result = "error";
                        errorCode = "error_getting_token";
                        errorMessage = "error_getting_token";
                    }
                    else
                    {

                        strToken = oResponse["token"].ToString();

                        if (string.IsNullOrEmpty(strToken))
                        {
                            result = "error";
                            errorMessage = "empty_token";
                            errorCode = "empty_token";

                        }
                        else
                        {
                            string strGuid = "";

                            if (_session["MercadoPagoGuid"] != null)
                            {
                                strGuid = _session["MercadoPagoGuid"].ToString();
                            }

                            if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                            {
                                result = "error";
                                errorMessage = "MercadoPago Guid not found";
                                errorCode = "invalid_configuration";

                            }
                            else
                            {

                                bool Configuration_OK = false;

                                MERCADOPAGO_CONFIGURATION oMercadoPagoConfiguration = null;


                                if (Configuration_Id != null)
                                {
                                    Configuration_OK = infraestructureRepository.GetMercadoPagoConfigurationById(Configuration_Id, out oMercadoPagoConfiguration);
                                }
                                else
                                {
                                    Configuration_OK = infraestructureRepository.GetMercadoPagoConfiguration(strGuid, out oMercadoPagoConfiguration);
                                }


                                if (Configuration_OK)
                                {
                                    if (oMercadoPagoConfiguration == null)
                                    {
                                        result = "error";
                                        errorCode = "configuration_not_found";
                                        errorMessage = "MercadoPago configuration not found";
                                    }
                                    else
                                    {
                                        MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.Approved;
                                        result = "succeeded";
                                        errorCode = eErrorCode.ToString();
                                        /*
                                        string strOrderId = MercadoPagoPayments.UserReference();

                                        string strTransactionId = "";
                                        oPayments.AutomaticTransaction(
                                            oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                                "febermejo2@gmail.com",
                                                strOrderId,
                                                5000,
                                                "test",
                                                strToken,
                                                "1082226796-0VfBpzG5VIfOO6",
                                                1,
                                                true,
                                                out eErrorCode,
                                                out errorMessage,
                                                out strTransactionId);*/
                                    }
                        
                                }
                                else
                                {
                                    result = "error";
                                    errorCode = "configuration_not_found";
                                    errorMessage = "MercadoPago configuration not found";
                                }
                            }
                        }
                    }
                }
                else  //user has cancel 
                {
                    result = "error";
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
                _session["token"] = strToken;

                strRedirectionURLLog = string.Format("MercadoPagoResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(strToken)
                    );


                Logger_AddLogMessage(string.Format("MercadoPagoResponse End: Token={0} ; CardId={1} ResultURL={2}",
                                        strToken,
                                        _session["cardid"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MercadoPagoResult" + sSuffix + "?r={0}", MercadoPagoResultCalc());

            }

            return Redirect(strRedirectionURL);
        }



        [HttpGet]
        public ActionResult MercadoPagoProRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture, string ReturnURL, string ExternalId, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strInitPoint = "";

            try
            {
                if (_session == null) _session = Session;
                MercadoPagoPayments oPayments = new MercadoPagoPayments();
                string strOrderId = MercadoPagoPayments.UserReference();
                _session["sessionid"] = strOrderId;


                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                _session["cardid"] = null;
                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["MercadoPagoGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["cardExpMonth"] = null;
                _session["cardExpYear"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["sessionid"] = null;
                _session["orderid"] = null;
                _session["MD"] = null;
                _session["CAVV"] = null;
                _session["ECI"] = null;
                _session["ExternalId"] = null;
                _session["token"] = null;




                Logger_AddLogMessage(string.Format("MercadoPagoProRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}; culture= {7}; ExternalId={8}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        ReturnURL,
                                        Culture,
                                        ExternalId), LogLevels.logINFO);


                _session["ReturnURL"] = ReturnURL;
                _session["ExternalId"] = ExternalId;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value <= 0) ||
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

                    MERCADOPAGO_CONFIGURATION oMercadoPagoConfiguration = null;
                    bool Configuration_OK = false;



                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfigurationById(Configuration_Id, out oMercadoPagoConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfiguration(Guid, out oMercadoPagoConfiguration);
                    }


                    if (Configuration_OK)
                    {
                        if (oMercadoPagoConfiguration != null)
                        {

                            _session["HashSeed"] = oMercadoPagoConfiguration.MEPACON_HASH_SEED;

                            string strCalcHash = CalculateHash(oMercadoPagoConfiguration.MEPACON_GUID, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, ReturnURL, Culture, ExternalId, oMercadoPagoConfiguration.MEPACON_HASH_SEED);

                            if ((oMercadoPagoConfiguration.MEPACON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("MercadoPagoProRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oMercadoPagoConfiguration.MEPACON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oMercadoPagoConfiguration.MEPACON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        decimal dAmount = Convert.ToDecimal(Convert.ToDouble(Amount.Value) / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE)));


                                        MercadoPagoPayments.MercadoPagoErrorCode eErrorCode;
                                        _session["sessionid"] = strOrderId;


                                        eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.Approved;
                                        if (!MercadoPagoPayments.IsError(eErrorCode))
                                        {


                                            string strFullURL = _request.Url.AbsoluteUri;
                                            string strBaseURL = strFullURL.Substring(0, strFullURL.LastIndexOf("/"));
                                            string strSuccessURL = strBaseURL + "/MercadoPagoProSuccess";
                                            string strFailureURL = strBaseURL + "/MercadoPagoProFailure";
                                            string strPendingURL = strBaseURL + "/MercadoPagoProPending";
                                            string strPreferenceId = "";                                            

                                            if (oPayments.GeneratePreference(oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                                                        strOrderId,
                                                                        Description,
                                                                        CurrencyISOCODE,
                                                                        dAmount,
                                                                        strSuccessURL,
                                                                        strFailureURL,
                                                                        strPendingURL,
                                                                        out eErrorCode,
                                                                        out errorMessage,
                                                                        out strPreferenceId, 
                                                                        out strInitPoint))
                                            {



                                                ViewData["MercadoPago_sdk_url"] = oMercadoPagoConfiguration.MEPACON_SDK_URL;
                                                ViewData["MercadoPago_public_key"] = oMercadoPagoConfiguration.MEPACON_PUBLIC_KEY;
                                                ViewData["MercadoPago_preference_id"] = strPreferenceId;

                                                _session["orderid"] = strOrderId;
                                                _session["email"] = Email;
                                                _session["amount"] = Amount;
                                                _session["currency"] = CurrencyISOCODE;
                                                _session["utcdate"] = dtUTC;
                                                Guid = oMercadoPagoConfiguration.MEPACON_GUID;
                                                _session["MercadoPagoGuid"] = Guid;

                                                Logger_AddLogMessage(string.Format("MercadoPagoProRequest Begin: Guid={0}, ConfigurationId={1}", _session["MercadoPagoGuid"].ToString(), Configuration_Id), LogLevels.logINFO);
                                            }
                                            else
                                            {
                                                result = "error";
                                                errorCode = eErrorCode.ToString();
                                                if (string.IsNullOrEmpty(errorMessage))
                                                {
                                                    errorMessage = MercadoPagoPayments.ErrorMessageDict[eErrorCode];
                                                }
                                            }

                                        }
                                        else
                                        {
                                            result = "error";
                                            errorCode = eErrorCode.ToString();
                                            if (string.IsNullOrEmpty(errorMessage))
                                            {
                                                errorMessage = MercadoPagoPayments.ErrorMessageDict[eErrorCode];
                                            }
                                        }

                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("MercadoPagoProRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("MercadoPagoProRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "MercadoPago configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "MercadoPago configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "MercadoPagoProRequest Method Exception";
            }


            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

                SaveSession(_session["sessionid"].ToString());

                string strRedirectionURLLog = string.Format("MercadoPagoResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("MercadoPagoResult?r={0}&s={1}", MercadoPagoResultCalc(), _session["sessionid"]);
                Logger_AddLogMessage(string.Format("MercadoPagoProRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {

                SaveSession(_session["sessionid"].ToString());

                Logger_AddLogMessage(string.Format("MercadoPagoProRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                //return View();
                return Redirect(strInitPoint);
            }

        }

        [HttpGet]
        public ActionResult MercadoPagoProSuccess(string collection_id, string collection_status, string payment_id, 
                                                  string status, string external_reference, string payment_type,
                                                  string merchant_order_id, string preference_id, string site_id,
                                                  string processing_mode, string merchant_account_id)
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


            string strRedirectionURL = "MercadoPagoResult" + sSuffix;
            string strCardScheme = "";
            string strCardType = "";
            string strMercadoPagoDateTime = "";
            string strPAN = "";
            string strDoctype = "";
            string strDocNumber = "";

            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strTransactionId = "";
            string strOrderId = "";
            string strEmail = "";
            int iInstallaments = 1;           


            MercadoPagoPayments oPayments = new MercadoPagoPayments();

            try
            {                                
                string strGuid = "";

                if (_session["MercadoPagoGuid"] != null)
                {
                    strGuid = _session["MercadoPagoGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "MercadoPago Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    bool Configuration_OK = false;

                    MERCADOPAGO_CONFIGURATION oMercadoPagoConfiguration = null;


                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfigurationById(Configuration_Id, out oMercadoPagoConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfiguration(strGuid, out oMercadoPagoConfiguration);
                    }



                    if (Configuration_OK)
                    {
                        if (oMercadoPagoConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "MercadoPago configuration not found";
                        }
                        else
                        {
                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                            errorCode = eErrorCode.ToString();
                          

                            if (oPayments.GetTransaction(oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                payment_id,
                                out eErrorCode,
                                out errorMessage,
                                out strEmail,
                                out strDoctype,
                                out strDocNumber,
                                out strCardScheme,
                                out strCardType,
                                out strPAN,
                                out strExpirationDateMonth,
                                out strExpirationDateYear,
                                out strMercadoPagoDateTime,
                                out iInstallaments
                                ))

                            {
                                strTransactionId = payment_id;
                                strOrderId = external_reference;




                                int iAmount = Convert.ToInt32(_session["amount"]);
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                customersRepository.StartRecharge(oMercadoPagoConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                                        _session["email"].ToString(),
                                                                                        dtUTCNow,
                                                                                        dtNow,
                                                                                        iAmount,
                                                                                        infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                                                        "",
                                                                                        strOrderId,
                                                                                        strTransactionId,
                                                                                        "",
                                                                                        strMercadoPagoDateTime,
                                                                                        "",
                                                                                        PaymentMeanRechargeStatus.Committed);


                                result = "succeeded";
                                errorCode = eErrorCode.ToString();

                                /*strOrderId = MercadoPagoPayments.UserReference();

                                oPayments.AutomaticTransaction(
                                oMercadoPagoConfiguration.MEPACON_API_URL,
                                oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                oMercadoPagoConfiguration.MEPACON_WS_API_TIMEOUT,
                                 strOrderId,
                                 dAmount,
                                 strDescription,
                                 strCardId,
                                 strCustomerID,
                                 iInstallaments,
                                 true,
                                 out eErrorCode,
                                 out errorMessage,
                                 out strTransactionId);*/


                                /*oPayments.CommitTransaction(
                                    oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                     strTransactionId,
                                     dAmount,
                                     out eErrorCode,
                                     out errorMessage);

                                string strRefundTransactionId = "";
                                oPayments.RefundTransaction(
                                     oMercadoPagoConfiguration.MEPACON_ACCESS_TOKEN,
                                      strTransactionId,
                                      dAmount,
                                      out eErrorCode,
                                      out errorMessage/*,
                                      out strRefundTransactionId
                                      );*/
                            }
                            else
                            {
                                result = "error";
                                errorCode = "payment_id_not_found";
                                errorMessage = "Payment id not found";
                            }

                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "MercadoPago configuration not found";
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
                _session["cardReference"] = "";
                _session["cardHash"] = strEmail;
                _session["cardScheme"] = strCardScheme;
                _session["cardType"] = strCardType;
                _session["cardPAN"] = strPAN;
                _session["chargeDateTime"] = strMercadoPagoDateTime;
                _session["transactionID"] = strTransactionId;
                _session["installments"] = iInstallaments;
                _session["DocumentId"] = strDocNumber;
                _session["DocumentType"] = strDoctype;
                _session["cardExpMonth"] = strExpirationDateMonth;
                _session["cardExpYear"] = strExpirationDateYear;
                _session["reference"] = strOrderId;
                _session["cvvlength"] = 0;

                strRedirectionURLLog = string.Format("MercadoPagoResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardScheme={6}" +
                                                    "&cardPAN={7}" +
                                                    "&chargeDateTime={8}" +
                                                    "&transactionID={9}" +
                                                    "&installments={10}" +
                                                    "&DocumentId={11}" +
                                                    "&DocumentType={12}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(""),
                    _server.UrlEncode(""),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strCardType),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strMercadoPagoDateTime),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(iInstallaments.ToString()),
                    _server.UrlEncode(strDocNumber),
                    _server.UrlEncode(strDoctype)
                    );


                Logger_AddLogMessage(string.Format("MercadoPagoProSuccess End: Payment Id={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        payment_id,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MercadoPagoResult" + sSuffix + "?r={0}", MercadoPagoResultCalc());

            }

            return Redirect(strRedirectionURL);
        }


        [HttpGet]
        public ActionResult MercadoPagoProFailure(string collection_id, string collection_status, string payment_id,
                                                  string status, string external_reference, string payment_type,
                                                  string merchant_order_id, string preference_id, string site_id,
                                                  string processing_mode, string merchant_account_id)
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


            string strRedirectionURL = "MercadoPagoResult" + sSuffix;
            string strCardScheme = "";
            string strCardType = "";
            string strMercadoPagoDateTime = "";
            string strPAN = "";
            string strDoctype = "";
            string strDocNumber = "";

            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strTransactionId = "";
            string strOrderId = "";
            string strEmail = "";
            int iInstallaments = 1;

            

            try
            {
                string strGuid = "";

                if (_session["MercadoPagoGuid"] != null)
                {
                    strGuid = _session["MercadoPagoGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "MercadoPago Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    bool Configuration_OK = false;

                    MERCADOPAGO_CONFIGURATION oMercadoPagoConfiguration = null;


                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfigurationById(Configuration_Id, out oMercadoPagoConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMercadoPagoConfiguration(strGuid, out oMercadoPagoConfiguration);
                    }



                    if (Configuration_OK)
                    {
                        if (oMercadoPagoConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "MercadoPago configuration not found";
                        }
                        else
                        {
                            result = "error";
                            errorMessage = "Transaction cancelled By user";
                            errorCode = "transaction_cancelled";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "MercadoPago configuration not found";
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
                _session["cardReference"] = "";
                _session["cardHash"] = strEmail;
                _session["cardScheme"] = strCardScheme;
                _session["cardType"] = strCardType;
                _session["cardPAN"] = strPAN;
                _session["chargeDateTime"] = strMercadoPagoDateTime;
                _session["transactionID"] = strTransactionId;
                _session["installments"] = iInstallaments;
                _session["DocumentId"] = strDocNumber;
                _session["DocumentType"] = strDoctype;
                _session["cardExpMonth"] = strExpirationDateMonth;
                _session["cardExpYear"] = strExpirationDateYear;
                _session["reference"] = strOrderId;
                _session["cvvlength"] = 0;

                strRedirectionURLLog = string.Format("MercadoPagoResult" + sSuffix + "?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardScheme={6}" +
                                                    "&cardPAN={7}" +
                                                    "&chargeDateTime={8}" +
                                                    "&transactionID={9}" +
                                                    "&installments={10}" +
                                                    "&DocumentId={11}" +
                                                    "&DocumentType={12}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(""),
                    _server.UrlEncode(""),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strCardType),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strMercadoPagoDateTime),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(iInstallaments.ToString()),
                    _server.UrlEncode(strDocNumber),
                    _server.UrlEncode(strDoctype)
                    );


                Logger_AddLogMessage(string.Format("MercadoPagoProSuccess End: Payment Id={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        payment_id,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MercadoPagoResult" + sSuffix + "?r={0}", MercadoPagoResultCalc());

            }

            return Redirect(strRedirectionURL);
        }






        [HttpGet]
        public ActionResult MercadoPagoResult(string r, string s)
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

                if (_session["sessionid"] == null)
                {
                    LoadSession(s);
                }


                string strResultDec = DecryptCryptResult(r, _session["HashSeed"] != null ? _session["HashSeed"].ToString() : string.Empty);
                ViewData["Result"] = strResultDec;
                if (_session["PayerQuantity"] != null) ViewData["PayerQuantity"] = _session["PayerQuantity"];
                if (_session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = _session["PayerCurrencyISOCode"];
                if (_session["UserBalance"] != null) ViewData["UserBalance"] = _session["UserBalance"];


                Logger_AddLogMessage(string.Format("MercadoPagoResult Begin: ReturnURL={0}; ReturnByGet={1} ; ExternalId={2}",
                                        _session["ReturnURL"] != null ? _session["ReturnURL"].ToString() : "",
                                        _session["ReturnByGet"] != null ? _session["ReturnByGet"].ToString() : "",
                                        _session["ExternalId"] != null ? _session["ExternalId"].ToString() : ""), LogLevels.logINFO);

                if (_session["ReturnURL"] != null && !string.IsNullOrEmpty(_session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    if (_session["ReturnByGet"] != null && _session["ReturnByGet"].ToString() == "1")
                    {
                        string url = string.Empty;
                        if (_session["ExternalId"] != null && !string.IsNullOrEmpty(_session["ExternalId"].ToString()))
                        {
                            url = string.Format("{0}?r={1}&ExternalId={2}", _session["ReturnURL"].ToString(), r, _session["ExternalId"].ToString());
                        }
                        else
                        {
                            url = string.Format("{0}?r={1}", _session["ReturnURL"].ToString(), r);
                        }

                        Logger_AddLogMessage(string.Format("MercadoPagoResult: ReturnURL with QueryString={0};", url), LogLevels.logINFO);

                        return Redirect(url);
                    }
                    else
                    {
                        if (_session["ExternalId"] != null && !string.IsNullOrEmpty(_session["ExternalId"].ToString()))
                        {
                            postData.Add("ExternalId", _session["ExternalId"].ToString());
                        }

                        postData.Add("r", r);

                        RedirectWithData(_session["ReturnURL"].ToString(), postData);
                    }
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
                        _session["cardType"] = null;
                        _session["cardPAN"] = null;
                        _session["cardExpMonth"] = null;
                        _session["cardExpYear"] = null;
                        _session["chargeDateTime"] = null;
                        _session["transactionID"] = null;
                        _session["email"] = null;
                        _session["amount"] = null;
                        _session["currency"] = null;
                        _session["utcdate"] = null;
                        _session["MercadoPagoGuid"] = null;
                        _session["ResultURL"] = null;
                        _session["ExternalId"] = null;

                        string sSuffix = string.Empty;
                        if (_session["Suffix"] != null)
                        {
                            sSuffix = _session["Suffix"].ToString();
                        }
                        return RedirectToAction("MercadoPagoResult" + sSuffix, "Account", new { r = r });
                    }
                    else if (_session["PAYMENT_ORIGIN"] != null && _session["PAYMENT_ORIGIN"].ToString() == "FineController")
                    {
                        return RedirectToAction("MercadoPagoResult", "Fine", new { r = r });
                    }

                }

                return View();
            }

        }





        private string MercadoPagoResultCalc()
        {

            string strRes = "";

            Dictionary<string, object> oDataDict = new Dictionary<string, object>();

            oDataDict["sessionid"] = _session["sessionid"];
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

            if (_session["result"] != null && _session["result"].ToString() == "succeeded" && _session["cardid"] == null)
            {

                oDataDict["mercadopago_card_reference"] = _session["cardReference"];
                oDataDict["mercadopago_card_hash"] = _session["cardHash"];
                oDataDict["mercadopago_card_scheme"] = _session["cardScheme"];
                oDataDict["mercadopago_card_type"] = _session["cardType"];
                oDataDict["mercadopago_masked_card_number"] = _session["cardPAN"];
                oDataDict["mercadopago_expires_end_month"] = _session["cardExpMonth"];
                oDataDict["mercadopago_expires_end_year"] = _session["cardExpYear"];
                oDataDict["mercadopago_date_time_local_fmt"] = _session["chargeDateTime"];
                oDataDict["mercadopago_reference"] = _session["reference"];
                oDataDict["mercadopago_document_id"] = _session["DocumentId"];
                oDataDict["mercadopago_document_type"] = _session["DocumentType"];
                oDataDict["mercadopago_installaments"] = _session["installments"];
                oDataDict["mercadopago_transaction_id"] = _session["transactionID"];
                oDataDict["mercadopago_cvv_length"] = _session["cvvlength"];

            }
            else if (_session["result"] != null && _session["result"].ToString() == "succeeded" && _session["cardid"] != null)
            {               
                oDataDict["mercadopago_token"] = _session["token"];
                oDataDict["mercadopago_cardid"] = _session["cardid"];
            }


            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("MercadoPagoResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, _session["HashSeed"] != null ? _session["HashSeed"].ToString() : string.Empty);

            return strRes;


        }



        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string lang, string ExternalId, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL + lang + ExternalId;

            return CalculateHash(strHashString, strHashSeed);

        }


        private string CalculateHash(string Guid, string cardId, string cvvLength, string UTCDate, string lang, string ReturnURL, string ExternalId, string strHashSeed)
        {
            string strHashString = Guid + cardId  + cvvLength + UTCDate + lang + ReturnURL + ExternalId;

            return CalculateHash(strHashString, strHashSeed);

        }


      
    }
}
