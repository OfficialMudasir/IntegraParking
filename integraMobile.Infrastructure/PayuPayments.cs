using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Net.Sockets;
using Newtonsoft.Json;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.Infrastructure
{
    public class PayuPayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(PayuPayments));
        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));

        public enum PayuErrorCode
        {
            ERROR = 0,
            APPROVED = 1,
            ANTIFRAUD_REJECTED = 2,
            PAYMENT_NETWORK_REJECTED = 3,
            ENTITY_DECLINED = 4,
            INTERNAL_PAYMENT_PROVIDER_ERROR = 5,
            INACTIVE_PAYMENT_PROVIDER = 6,
            DIGITAL_CERTIFICATE_NOT_FOUND = 7,
            INVALID_EXPIRATION_DATE_OR_ = 8,
            SECURITY_CODE = 9,
            INVALID_RESPONSE_PARTIAL_APPROVAL = 10,
            INSUFFICIENT_FUNDS = 11,
            CREDIT_CARD_NOT_AUTHORIZED_FOR_INTERNET_TRANSACTIONS = 12,
            INVALID_TRANSACTION = 13,
            INVALID_CARD = 14,
            EXPIRED_CARD = 15,
            RESTRICTED_CARD = 16,
            CONTACT_THE_ENTITY = 17,
            REPEAT_TRANSACTION = 18,
            ENTITY_MESSAGING_ERROR = 19,
            BANK_UNREACHABLE = 20,
            EXCEEDED_AMOUNT = 21,
            NOT_ACCEPTED_TRANSACTION = 22,
            ERROR_CONVERTING_TRANSACTION_AMOUNTS = 23,
            EXPIRED_TRANSACTION = 24,
            PENDING_TRANSACTION_REVIEW = 25,
            PENDING_TRANSACTION_CONFIRMATION = 26,
            PENDING_TRANSACTION_TRANSMISSION = 27,
            PAYMENT_NETWORK_BAD_RESPONSE = 28,
            PAYMENT_NETWORK_NO_CONNECTION = 29,
            PAYMENT_NETWORK_NO_RESPONSE = 30,
            FIX_NOT_REQUIRED = 31,

            ConnectionFailed = 1000,
            InternalError = 1001,
            DECLINED = 1002,

        }


        /*public Dictionary<string, int> PayuConfirmationErrorCode = new Dictionary<string, int>()         
        {         
            {"APPROVED" , 1},
            {"PAYMENT_NETWORK_REJECTED" , 4},
            {"ENTITY_DECLINED" , 5},
            {"INSUFFICIENT_FUNDS" , 6},
            {"INVALID_CARD" , 7},
            {"CONTACT_THE_ENTITY" , 8},
            {"BANK_ACCOUNT_ACTIVATION_ERROR" , 8},
            {"BANK_ACCOUNT_NOT_AUTHORIZED_FOR_AUTOMATIC_DEBIT" , 8},
            {"INVALID_AGENCY_BANK_ACCOUNT" , 8},
            {"INVALID_BANK_ACCOUNT" , 8},
            {"INVALID_BANK" , 8},
            {"EXPIRED_CARD" , 9},
            {"RESTRICTED_CARD" , 10},
            {"INVALID_EXPIRATION_DATE_OR_SECURITY_CODE" , 12},
            {"REPEAT_TRANSACTION" , 13},
            {"INVALID_TRANSACTION" , 14},
            {"EXCEEDED_AMOUNT" , 17},
            {"ABANDONED_TRANSACTION" , 19},
            {"CREDIT_CARD_NOT_AUTHORIZED_FOR_INTERNET_TRANSACTIONS" , 22},
            {"ANTIFRAUD_REJECTED" , 23},
            {"DIGITAL_CERTIFICATE_NOT_FOUND" , 9995},
            {"BANK_UNREACHABLE" , 9996},
            {"PAYMENT_NETWORK_NO_CONNECTION" , 9996},
            {"PAYMENT_NETWORK_NO_RESPONSE" , 9996},
            {"ENTITY_MESSAGING_ERROR" , 9997},
            {"NOT_ACCEPTED_TRANSACTION" , 9998},
            {"INTERNAL_PAYMENT_PROVIDER_ERROR" , 9999},
            {"INACTIVE_PAYMENT_PROVIDER" , 9999},
            {"ERROR" , 9999},
            {"ERROR_CONVERTING_TRANSACTION_AMOUNTS" , 9999},
            {"BANK_ACCOUNT_ACTIVATION_ERROR" , 9999},
            {"FIX_NOT_REQUIRED" , 9999},
            {"AUTOMATICALLY_FIXED_AND_SUCCESS_REVERSAL" , 9999},
            {"AUTOMATICALLY_FIXED_AND_UNSUCCESS_REVERSAL" , 9999},
            {"AUTOMATIC_FIXED_NOT_SUPPORTED" , 9999},
            {"NOT_FIXED_FOR_ERROR_STATE" , 9999},
            {"ERROR_FIXING_AND_REVERSING" , 9999},
            {"ERROR_FIXING_INCOMPLETE_DATA" , 9999},
            {"PAYMENT_NETWORK_BAD_RESPONSE" , 9999},
            {"EXPIRED_TRANSACTION" , 20},
        };*/

        public enum PayuConfStatePol
        {
            Approved = 4,
            Expired = 5,
            Declined = 6,
        }

        static readonly public Dictionary<PayuErrorCode, string> ErrorMessageDict = new Dictionary<PayuErrorCode, string>()
        {
            {PayuErrorCode.ERROR,"Ocurrió un error general."},
            {PayuErrorCode.APPROVED,"La transacción fue aprobada."},
            {PayuErrorCode.ANTIFRAUD_REJECTED,"La transacción fue rechazada por el sistema anti-fraude."},
            {PayuErrorCode.PAYMENT_NETWORK_REJECTED,"La red financiera rechazó la transacción."},
            {PayuErrorCode.ENTITY_DECLINED,"La transacción fue declinada por el banco o por la red financiera debido a un error."},
            {PayuErrorCode.INTERNAL_PAYMENT_PROVIDER_ERROR,"Ocurrió un error en el sistema intentando procesar el pago."},
            {PayuErrorCode.INACTIVE_PAYMENT_PROVIDER,"El proveedor de pagos no se encontraba activo."},
            {PayuErrorCode.DIGITAL_CERTIFICATE_NOT_FOUND,"La red financiera reportó un error en la autenticación."},
            {PayuErrorCode.INVALID_EXPIRATION_DATE_OR_,"El código de seguridad o la fecha de expiración estaba inválido."},
            {PayuErrorCode.SECURITY_CODE,""},
            {PayuErrorCode.INVALID_RESPONSE_PARTIAL_APPROVAL,"Tipo de respuesta no válida. La entidad aprobó parcialmente la transacción y debe ser cancelada automáticamente por el sistema."},
            {PayuErrorCode.INSUFFICIENT_FUNDS,"La cuenta no tenía fondos suficientes."},
            {PayuErrorCode.CREDIT_CARD_NOT_AUTHORIZED_FOR_INTERNET_TRANSACTIONS,"La tarjeta de crédito no estaba autorizada para transacciones por Internet."},
            {PayuErrorCode.INVALID_TRANSACTION,"La red financiera reportó que la transacción fue inválida."},
            {PayuErrorCode.INVALID_CARD,"La tarjeta es inválida."},
            {PayuErrorCode.EXPIRED_CARD,"La tarjeta ya expiró."},
            {PayuErrorCode.RESTRICTED_CARD,"La tarjeta presenta una restricción."},
            {PayuErrorCode.CONTACT_THE_ENTITY,"Debe contactar al banco."},
            {PayuErrorCode.REPEAT_TRANSACTION,"Se debe repetir la transacción."},
            {PayuErrorCode.ENTITY_MESSAGING_ERROR,"La red financiera reportó un error de comunicaciones con el banco."},
            {PayuErrorCode.BANK_UNREACHABLE,"El banco no se encontraba disponible."},
            {PayuErrorCode.EXCEEDED_AMOUNT,"La transacción excede un monto establecido por el banco."},
            {PayuErrorCode.NOT_ACCEPTED_TRANSACTION,"La transacción no fue aceptada por el banco por algún motivo."},
            {PayuErrorCode.ERROR_CONVERTING_TRANSACTION_AMOUNTS,"Ocurrió un error convirtiendo los montos a la moneda de pago."},
            {PayuErrorCode.EXPIRED_TRANSACTION,"La transacción expiró."},
            {PayuErrorCode.PENDING_TRANSACTION_REVIEW,"La transacción fue detenida y debe ser revisada, esto puede ocurrir por filtros de seguridad."},
            {PayuErrorCode.PENDING_TRANSACTION_CONFIRMATION,"La transacción está pendiente de ser confirmada."},
            {PayuErrorCode.PENDING_TRANSACTION_TRANSMISSION,"La transacción está pendiente para ser trasmitida a la red financiera. Normalmente esto aplica para transacciones con medios de pago en efectivo."},
            {PayuErrorCode.PAYMENT_NETWORK_BAD_RESPONSE,"El mensaje retornado por la red financiera es inconsistente."},
            {PayuErrorCode.PAYMENT_NETWORK_NO_CONNECTION,"No se pudo realizar la conexión con la red financiera."},
            {PayuErrorCode.PAYMENT_NETWORK_NO_RESPONSE,"La red financiera no respondió."},
            {PayuErrorCode.FIX_NOT_REQUIRED,"Clínica de transacciones: Código de manejo interno."},

            {PayuErrorCode.ConnectionFailed,"Connection Failed."},
            {PayuErrorCode.InternalError,"Internal error."},
            {PayuErrorCode.DECLINED,"Transacción declinada."},
        };


        /*
             {
               "language": "es",
               "command": "SUBMIT_TRANSACTION",
               "merchant": {
                  "apiKey": "4Vj8eK4rloUd272L48hsrarnUA",
                  "apiLogin": "pRRXKOl8ikMmt9u"
               },
               "transaction": {
                  "order": {
                     "accountId": "512324",
                     "referenceCode": "payment_test_00000001",
                     "description": "payment test",
                     "language": "es",
                     "signature": "a88cba16c6fc54a4d31f696cfcbd41fc",
                     "notifyUrl": "http://www.tes.com/confirmation",
                     "additionalValues": {
                        "TX_VALUE": {
                           "value": 100,
                           "currency": "MXN"
                        }
                     },
                     "buyer": {
                        "merchantBuyerId": "1",
                        "fullName": "First name and second buyer  name",
                        "emailAddress": "buyer_test@test.com",
                        "contactPhone": "7563126",
                        "dniNumber": "5415668464654",
                        "shippingAddress": {
                           "street1": "Calle Salvador Alvarado",
                           "street2": "8 int 103",
                           "city": "Guadalajara",
                           "state": "Jalisco",
                           "country": "MX",
                           "postalCode": "000000",
                           "phone": "7563126"
                        }
                     },
                     "shippingAddress": {
                        "street1": "Calle Salvador Alvarado",
                        "street2": "8 int 103",
                        "city": "Guadalajara",
                        "state": "Jalisco",
                        "country": "MX",
                        "postalCode": "0000000",
                        "phone": "7563126"
                     }
                  },
                  "payer": {
                     "merchantPayerId": "1",
                     "fullName": "First name and second payer name",
                     "emailAddress": "payer_test@test.com",
                     "birthdate": "1985-05-25",
                     "contactPhone": "7563126",
                     "dniNumber": "5415668464654",
                     "billingAddress": {
                        "street1": "Calle Zaragoza esquina",
                        "street2": "calle 5 de Mayo",
                        "city": "Monterrey",
                        "state": "Nuevo Leon",
                        "country": "MX",
                        "postalCode": "64000",
                        "phone": "7563126"
                     }
                  },
                  "creditCardTokenId": "8604789e-80ef-439d-9c3f-5d4a546bf637",
                  "extraParameters": {
                     "INSTALLMENTS_NUMBER": 1
                  },
                  "type": "AUTHORIZATION_AND_CAPTURE",
                  "paymentMethod": "VISA",
                  "paymentCountry": "MX",
                  "deviceSessionId": "vghs6tvkcle931686k1900o6e1",
                  "ipAddress": "127.0.0.1",
                  "cookie": "pt1t38347bs6jc9ruv2ecpv7o2",
                  "userAgent": "Mozilla/5.0 (Windows NT 5.1; rv:18.0) Gecko/20100101 Firefox/18.0"
               },
               "test": false
            }
         * 
         * 
         * 
         * 
         * 
         * {
               "code": "SUCCESS",
               "error": null,
               "transactionResponse": {
                  "orderId": 39841000,
                  "transactionId": "11d5e0af-7cc3-4d50-9452-7cd8970b43dd",
                  "state": "DECLINED",
                  "paymentNetworkResponseCode": null,
                  "paymentNetworkResponseErrorMessage": null,
                  "trazabilityCode": null,
                  "authorizationCode": null,
                  "pendingReason": null,
                  "responseCode": "PAYMENT_NETWORK_REJECTED",
                  "errorCode": null,
                  "responseMessage": null,
                  "transactionDate": null,
                  "transactionTime": null,
                  "operationDate": null,
                  "extraParameters": null
               }
            }
         */

        public bool AutomaticTransaction(string strServiceURL,
                                          string strAPIKey,
                                          string strAPILogin,
                                          string strAccountId,
                                          string strMerchantId,
                                          int iServiceTimeout,
                                          string strCountry,
                                          bool isTest,
                                          string strCardReference,
                                          string strPayerId,
                                          string strLang,
                                          string strEmail,
                                          decimal dAmount,
                                          string strCurISOCode,
                                          string strDescription,
                                          string strUserAgent,
                                          string strMethod,
                                          string strName,
                                          string strIdNumber,
                                          string strSecurityCode,
                                          out PayuErrorCode eErrorCode,
                                          out string errorMessage,
                                          out string strTransactionId,
                                          out string strReference,
                                          out string strOrderId,
                                          out DateTime? dtTransaction)
        {
            bool bRes = false;
            strTransactionId = null;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = PayuErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strOrderId = "";
            strReference = "";
            dtTransaction = null;
            string strUserReference = UserReference();

            AddTLS12Support();


            try
            {
                
                string strURL = strServiceURL;
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();
                

                oTransactionRequest["language"] = strLang;
                oTransactionRequest["command"] = "SUBMIT_TRANSACTION";
                oTransactionRequest["test"] = isTest.ToString().ToLower();


                Dictionary<string, object> oMerchant = new Dictionary<string, object>();
                oMerchant["apiKey"]= strAPIKey;
                oMerchant["apiLogin"] =strAPILogin;

                oTransactionRequest["merchant"] = oMerchant;


                Dictionary<string, object> oTransaction = new Dictionary<string, object>();
                Dictionary<string, object> oOrder = new Dictionary<string, object>();
                oOrder["accountId"] = strAccountId;
                oOrder["referenceCode"] = strUserReference;
                oOrder["description"] = strDescription;
                oOrder["language"] = strLang;
                oOrder["signature"] = CalculateMD5Hash(strAPIKey + "~" + strMerchantId + "~" + strUserReference + "~" + dAmount.ToString("#.##", CultureInfo.InvariantCulture) + "~" + strCurISOCode).ToLower();


                Dictionary<string, object> oBuyer = new Dictionary<string, object>();
                oBuyer["merchantBuyerId"] = strEmail.Trim();
                oBuyer["emailAddress"] = strEmail.Trim();
                oBuyer["contactPhone"] = "";

                if (!string.IsNullOrEmpty(strName))
                    oBuyer["fullName"] = strName;
                /*else
                    oBuyer["fullName"] = "";*/

                if (!string.IsNullOrEmpty(strIdNumber))
                    oBuyer["dniNumber"] = strIdNumber;
                /*else
                    oBuyer["dniNumber"] = "";*/
                
                oOrder["buyer"] = oBuyer;

               


                Dictionary<string, object> oAdditionalValues = new Dictionary<string, object>();
                Dictionary<string, object> oTxValue = new Dictionary<string, object>();
                oTxValue["value"] = dAmount;
                oTxValue["currency"]=strCurISOCode;
                oAdditionalValues["TX_VALUE"] = oTxValue;
                oOrder["additionalValues"] = oAdditionalValues;                
                                             
                oTransaction["order"] = oOrder;

                Dictionary<string, object> oPayer = new Dictionary<string, object>();
                oPayer["merchantPayerId"] = strEmail.Trim();
                oPayer["emailAddress"] = strEmail.Trim();
                oPayer["contactPhone"] = "";


                if (!string.IsNullOrEmpty(strName))
                    oPayer["fullName"] = strName;
                /*else
                    oPayer["fullName"] = "";*/

                if (!string.IsNullOrEmpty(strIdNumber))
                    oPayer["dniNumber"] = strIdNumber;
                /*else
                    oPayer["dniNumber"] = "";*/

                oTransaction["payer"] = oPayer;


                oTransaction["creditCardTokenId"] = strCardReference;

                if (!String.IsNullOrEmpty(strSecurityCode))
                {
                    Dictionary<string, object> oCreditCard = new Dictionary<string, object>();
                    oCreditCard["securityCode"] = strSecurityCode;
                    oTransaction["creditCard"] = oCreditCard;
                }
                Dictionary<string, object> oExtraParameters = new Dictionary<string, object>();
                oExtraParameters["INSTALLMENTS_NUMBER"] = 1;
                oTransaction["extraParameters"] = oExtraParameters;

                
                oTransaction["type"] = "AUTHORIZATION_AND_CAPTURE";

                oTransaction["paymentMethod"] = strMethod;
                oTransaction["paymentCountry"] = strCountry;
                if (!string.IsNullOrEmpty(strUserAgent))
                {
                    oTransaction["userAgent"] = strUserAgent;
                }
                oTransaction["ipAddress"] = "127.0.0.1";
                                                            
                oTransactionRequest["transaction"] = oTransaction;


                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("AutomaticTransaction request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("AutomaticTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        string strError = "ERROR";

                        /*                          
                          	{
	                          "code": "SUCCESS",
	                          "error": null,
	                          "transactionResponse": {
	                            "orderId": 843931046,
	                            "transactionId": "43545f86-ff0e-49b2-bca8-eaeccb46e531",
	                            "state": "APPROVED",
	                            "paymentNetworkResponseCode": null,
	                            "paymentNetworkResponseErrorMessage": null,
	                            "trazabilityCode": "00000000",
	                            "authorizationCode": "00000000",
	                            "pendingReason": null,
	                            "responseCode": "APPROVED",
	                            "errorCode": null,
	                            "responseMessage": null,
	                            "transactionDate": null,
	                            "transactionTime": null,
	                            "operationDate": 1519991141317,
	                            "referenceQuestionnaire": null,
	                            "extraParameters": null,
	                            "additionalInfo": null
	                          }
	                        }
                         */

                        try
                        {
                            strError = oResponse["code"];
                            if (strError == "ERROR")
                            {

                                eErrorCode = PayuErrorCode.ERROR;
                                errorMessage = oResponse["error"];
                            }
                            else
                            {

                                var oTransactionReponse = oResponse["transactionResponse"];


                                if (oTransactionReponse["state"] == "APPROVED")
                                {
                                    eErrorCode = PayuErrorCode.APPROVED;
                                    errorMessage = ErrorMessageDict[eErrorCode];
                                    strReference = strUserReference;
                                    strTransactionId = oTransactionReponse["transactionId"];
                                    strOrderId = oTransactionReponse["orderId"];
                                    string strDate = oTransactionReponse["operationDate"].ToString();
                                    dtTransaction=DateHelpers.DateTimeFromUnixTimeStamp(Convert.ToDouble(strDate.Substring(0,strDate.Length-3)));
                               
                                }
                                else
                                {
                                    eErrorCode = PayuErrorCode.DECLINED;
                                    errorMessage = oTransactionReponse["responseCode"];

                                }

                            }
                        }
                        catch
                        {
                            eErrorCode = PayuErrorCode.InternalError;
                        }
                        

                      
                        if (eErrorCode == PayuErrorCode.APPROVED)
                        {                            
                            bRes = true;
                        }                       
                      
                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("AutomaticTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "AutomaticTransaction::Exception", LogLevels.logERROR);
                    eErrorCode = PayuErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "AutomaticTransaction::Exception", LogLevels.logERROR);

            }

            return bRes;
        }


        public bool OXXOTransaction(string strServiceURL,
                                          string strAPIKey,
                                          string strAPILogin,
                                          string strAccountId,
                                          string strMerchantId,
                                          int iServiceTimeout,
                                          string strCountry,
                                          bool isTest,
                                          string strCallBackURL,
                                          string strLang,
                                          string strEmail,
                                          decimal dAmount,
                                          string strCurISOCode,
                                          string strDescription,
                                          out PayuErrorCode eErrorCode,
                                          out string errorMessage,
                                          out string strTransactionId,
                                          out string strReference,
                                          out string strOrderId,
                                          out DateTime? dtExpirationDate,
                                          out string strBarcode,
                                          out string strOxxoReference,
                                          out string strPayuURL)

        {
            bool bRes = false;
            strTransactionId = null;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = PayuErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strOrderId = "";
            strReference = "";
            dtExpirationDate=null;
            strBarcode="";
            strOxxoReference="";
            strPayuURL="";
            string strUserReference = UserReference();


            try
            {
                AddTLS12Support();

                string strURL = strServiceURL;
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();


                oTransactionRequest["language"] = strLang;
                oTransactionRequest["command"] = "SUBMIT_TRANSACTION";
                oTransactionRequest["test"] = isTest.ToString().ToLower();


                Dictionary<string, object> oMerchant = new Dictionary<string, object>();
                oMerchant["apiKey"] = strAPIKey;
                oMerchant["apiLogin"] = strAPILogin;

                oTransactionRequest["merchant"] = oMerchant;


                Dictionary<string, object> oTransaction = new Dictionary<string, object>();
                Dictionary<string, object> oOrder = new Dictionary<string, object>();
                oOrder["accountId"] = strAccountId;
                oOrder["referenceCode"] = strUserReference;
                oOrder["description"] = strDescription;
                oOrder["language"] = strLang;
                oOrder["signature"] = CalculateMD5Hash(strAPIKey + "~" + strMerchantId + "~" + strUserReference + "~" + dAmount.ToString("#.##", CultureInfo.InvariantCulture) + "~" + strCurISOCode).ToLower();
                oOrder["notifyUrl"] = strCallBackURL;


                Dictionary<string, object> oBuyer = new Dictionary<string, object>();
                oBuyer["merchantBuyerId"] = strEmail;
                oBuyer["emailAddress"] = strEmail;

                oOrder["buyer"] = oBuyer;

                Dictionary<string, object> oAdditionalValues = new Dictionary<string, object>();
                Dictionary<string, object> oTxValue = new Dictionary<string, object>();
                oTxValue["value"] = dAmount;
                oTxValue["currency"] = strCurISOCode;
                oAdditionalValues["TX_VALUE"] = oTxValue;
                oOrder["additionalValues"] = oAdditionalValues;

                oTransaction["order"] = oOrder;

                Dictionary<string, object> oPayer = new Dictionary<string, object>();
                oPayer["merchantPayerId"] = strEmail;
                oPayer["emailAddress"] = strEmail;

                oTransaction["payer"] = oPayer;

                

                oTransaction["type"] = "AUTHORIZATION_AND_CAPTURE";

                oTransaction["paymentMethod"] = "OXXO";
                oTransaction["paymentCountry"] = strCountry;
                
                oTransaction["ipAddress"] = "127.0.0.1";

                oTransactionRequest["transaction"] = oTransaction;


                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("OXXOTransaction request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("OXXOTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        string strError = "ERROR";

                        /*                          
                          	{
                                   "code": "SUCCESS",
                                   "error": null,
                                   "transactionResponse": {
                                      "orderId": 39869020,
                                      "transactionId": "6ba817ea-964b-4738-9897-3e4b1f2f31a7",
                                      "state": "PENDING",
                                      "paymentNetworkResponseCode": null,
                                      "paymentNetworkResponseErrorMessage": null,
                                      "trazabilityCode": null,
                                      "authorizationCode": null,
                                      "pendingReason": "AWAITING_NOTIFICATION",
                                      "responseCode": "PENDING_TRANSACTION_CONFIRMATION",
                                      "errorCode": null,
                                      "responseMessage": null,
                                      "transactionDate": null,
                                      "transactionTime": null,
                                      "operationDate": null,
                                      "extraParameters": {
                                         "EXPIRATION_DATE": 1415577600000,
                                         "BAR_CODE": "27000039869020201411090000100004",
                                         "REFERENCE": 39869020,
                                         "URL_PAYMENT_RECEIPT_HTML": "https://gateway.payulatam.com/ppp-web-gateway/voucher.zul?vid=39869020Y6ba817ea964b473Y1328ee36f9f93f5"
                                      }
                                   }
                            }
                         */

                        try
                        {
                            strError = oResponse["code"];
                            if (strError == "ERROR")
                            {

                                eErrorCode = PayuErrorCode.ERROR;
                                errorMessage = oResponse["error"];
                            }
                            else
                            {

                                var oTransactionReponse = oResponse["transactionResponse"];


                                if (oTransactionReponse["state"] == "PENDING")
                                {
                                    eErrorCode = PayuErrorCode.PENDING_TRANSACTION_CONFIRMATION;
                                    errorMessage = ErrorMessageDict[eErrorCode];
                                    strReference = strUserReference;
                                    strTransactionId = oTransactionReponse["transactionId"];
                                    strOrderId = oTransactionReponse["orderId"];
                                   

                                    var oOutExtraParameters = oTransactionReponse["extraParameters"];

                                    if (oOutExtraParameters != null)
                                    {
                                          string strDate = oOutExtraParameters["EXPIRATION_DATE"];
                                          dtExpirationDate= DateHelpers.DateTimeFromUnixTimeStamp(Convert.ToDouble(strDate.Substring(0, strDate.Length - 3)));
                                          strBarcode=oOutExtraParameters["BAR_CODE"];
                                          strOxxoReference= oOutExtraParameters["REFERENCE"];
                                          strPayuURL = oOutExtraParameters["URL_PAYMENT_RECEIPT_HTML"];
                                    }


                                }
                                else
                                {
                                    eErrorCode = PayuErrorCode.DECLINED;
                                    errorMessage = oTransactionReponse["responseCode"];

                                }

                            }
                        }
                        catch
                        {
                            eErrorCode = PayuErrorCode.InternalError;
                        }



                        if (eErrorCode == PayuErrorCode.PENDING_TRANSACTION_CONFIRMATION)
                        {
                            bRes = true;
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("OXXOTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "OXXOTransaction::Exception", LogLevels.logERROR);
                    eErrorCode = PayuErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "OXXOTransaction::Exception", LogLevels.logERROR);

            }

            return bRes;
        }




        public bool RefundTransaction(string strServiceURL,
                                  string strAPIKey,
                                  string strAPILogin,
                                  int iServiceTimeout,
                                  string strOrderId,
                                  string strTransactionId,
                                  string strReason,
                                  string strLang,
                                  out PayuErrorCode eErrorCode,
                                  out string errorMessage,
                                  out string strRefundOrderID
                                  )
        {
            bool bRes = false;
            strRefundOrderID = null;

            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = PayuErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            AddTLS12Support();


            try
            {

                string strURL = strServiceURL;

                if (strURL.Contains("sandbox"))
                {
                    bRes = true;
                    eErrorCode = PayuErrorCode.APPROVED;
                    errorMessage = ErrorMessageDict[eErrorCode];
                    strRefundOrderID = "TEST_REFUND_ORDER_ID";

                }
                else
                {

                    WebRequest request = WebRequest.Create(strURL);

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    ((HttpWebRequest)request).Accept = "application/json";
                    request.Timeout = iServiceTimeout;

                    Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();


                    oTransactionRequest["language"] = strLang;
                    oTransactionRequest["command"] = "SUBMIT_TRANSACTION";


                    Dictionary<string, object> oMerchant = new Dictionary<string, object>();
                    oMerchant["apiKey"] = strAPIKey;
                    oMerchant["apiLogin"] = strAPILogin;

                    oTransactionRequest["merchant"] = oMerchant;



                    Dictionary<string, object> oTransaction = new Dictionary<string, object>();
                    Dictionary<string, object> oOrder = new Dictionary<string, object>();
                    oOrder["id"] = strOrderId;

                    oTransaction["order"] = oOrder;
                    oTransaction["type"] = "REFUND";
                    oTransaction["reason"] = strReason;
                    oTransaction["parentTransactionId"] = strTransactionId;
                    oTransactionRequest["transaction"] = oTransaction;


                    var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                    Logger_AddLogMessage(string.Format("RefundTransaction request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                    byte[] byteArray = Encoding.UTF8.GetBytes(json);

                    request.ContentLength = byteArray.Length;
                    // Get the request stream.
                    watch = Stopwatch.StartNew();

                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();


                    try
                    {

                        WebResponse response = request.GetResponse();
                        // Display the status.
                        HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                        if (oWebResponse.StatusDescription == "OK")
                        {
                            // Get the stream containing content returned by the server.
                            dataStream = response.GetResponseStream();
                            // Open the stream using a StreamReader for easy access.
                            StreamReader reader = new StreamReader(dataStream);
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();

                            Logger_AddLogMessage(string.Format("RefundTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);
                            string strError = "ERROR";

                            try
                            {
                                strError = oResponse["code"];
                                if (strError == "ERROR")
                                {

                                    eErrorCode = PayuErrorCode.ERROR;
                                    errorMessage = oResponse["error"];
                                }
                                else
                                {

                                    var oTransactionReponse = oResponse["transactionResponse"];


                                    if ((oTransactionReponse["state"] == "APPROVED") ||
                                        (oTransactionReponse["state"] == "PENDING"))
                                    {
                                        eErrorCode = PayuErrorCode.APPROVED;
                                        errorMessage = ErrorMessageDict[eErrorCode];
                                        strRefundOrderID = oTransactionReponse["orderId"];
                                    }
                                    else
                                    {
                                        eErrorCode = PayuErrorCode.DECLINED;
                                        errorMessage = oTransactionReponse["responseCode"];
                                    }

                                }
                            }
                            catch
                            {
                                eErrorCode = PayuErrorCode.InternalError;
                            }



                            if (eErrorCode == PayuErrorCode.APPROVED)
                            {
                                bRes = true;
                            }


                            reader.Close();
                            dataStream.Close();
                        }

                        response.Close();
                    }
                    catch (WebException e)
                    {
                        Logger_AddLogMessage(string.Format("RefundTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                        Logger_AddLogException(e, "RefundTransaction::Exception", LogLevels.logERROR);
                        eErrorCode = PayuErrorCode.ConnectionFailed;
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }


                    lEllapsedTime = watch.ElapsedMilliseconds;
                }


            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "RefundTransaction::Exception", LogLevels.logERROR);

            }

            return bRes;
        }




        public bool GetTokenInfo(string strServiceURL,
                                  string strAPIKey,
                                  string strAPILogin,
                                  int iServiceTimeout,
                                  string strCardReference,
                                  string strPayerId,
                                  string strLang,
                                  out PayuErrorCode eErrorCode,
                                  out string errorMessage,
                                  out string strMethod,
                                  out string strPAN,
                                  out string strName,
                                  out string strSecurityCode,
                                  out string strIdNumber)
        {
            bool bRes = false;
            strMethod = null;
            strPAN = null;
            strName = null;
            strIdNumber = null;
            strSecurityCode= null;

            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = PayuErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            AddTLS12Support();


            try
            {

                string strURL = strServiceURL;
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();


                oTransactionRequest["language"] = strLang;
                oTransactionRequest["command"] = "GET_TOKENS";


                Dictionary<string, object> oMerchant = new Dictionary<string, object>();
                oMerchant["apiKey"] = strAPIKey;
                oMerchant["apiLogin"] = strAPILogin;

                oTransactionRequest["merchant"] = oMerchant;


                Dictionary<string, object> oTokenInformation = new Dictionary<string, object>();
                oTokenInformation["payerId"] = strPayerId;
                oTokenInformation["creditCardTokenId"] = strCardReference;
                oTokenInformation["startDate"] = "2018-01-01T12:00:00";
                oTokenInformation["endDate"] = "2050-01-01T12:00:00";

                oTransactionRequest["creditCardTokenInformation"] = oTokenInformation;

                if (!String.IsNullOrEmpty(strSecurityCode))
                {
                    Dictionary<string, object> oCreditCard = new Dictionary<string, object>();
                    oCreditCard["securityCode"] = strSecurityCode;
                    oTransactionRequest["creditCard"] = oCreditCard;
                }


                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("GetTokenInfo request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        
                        Logger_AddLogMessage(string.Format("GetTokenInfo response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);
                        string strError = "ERROR";
                      
                        try
                        {
                            strError = oResponse["code"];
                            if (strError == "ERROR")
                            {

                                eErrorCode = PayuErrorCode.ERROR;
                                errorMessage = oResponse["error"];
                            }
                            else
                            {
                                eErrorCode = PayuErrorCode.APPROVED;
                                errorMessage = ErrorMessageDict[eErrorCode];
                                var oCreditCardTokenList = oResponse["creditCardTokenList"];

                                foreach (var oCreditCardToken in oCreditCardTokenList)
                                {
                                    strPAN = oCreditCardToken["maskedNumber"];
                                    strMethod = oCreditCardToken["paymentMethod"];
                                    strName = oCreditCardToken["name"];
                                    strIdNumber = oCreditCardToken["identificationNumber"];
                                    strSecurityCode = oCreditCardToken["securityCode"];
                                    break;

                                }
                            }
                        }
                        catch
                        {
                            eErrorCode = PayuErrorCode.InternalError;
                        }


                        if (eErrorCode == PayuErrorCode.APPROVED)
                        {
                            bRes = true;
                        }


                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("GetTokenInfo Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "GetTokenInfo::Exception", LogLevels.logERROR);
                    eErrorCode = PayuErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "GetTokenInfo::Exception", LogLevels.logERROR);

            }

            return bRes;
        }



        public bool DeleteToken(string strServiceURL,
                                 string strAPIKey,
                                 string strAPILogin,
                                 int iServiceTimeout,
                                 string strCardReference,
                                 string strPayerId,
                                 string strLang,
                                 out PayuErrorCode eErrorCode,
                                 out string errorMessage)
        {
            bool bRes = false;

            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = PayuErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            AddTLS12Support();


            try
            {

                string strURL = strServiceURL;
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();


                oTransactionRequest["language"] = strLang;
                oTransactionRequest["command"] = "REMOVE_TOKEN";


                Dictionary<string, object> oMerchant = new Dictionary<string, object>();
                oMerchant["apiKey"] = strAPIKey;
                oMerchant["apiLogin"] = strAPILogin;

                oTransactionRequest["merchant"] = oMerchant;


                Dictionary<string, object> oTokenInformation = new Dictionary<string, object>();
                oTokenInformation["payerId"] = strPayerId;
                oTokenInformation["creditCardTokenId"] = strCardReference;


                oTransactionRequest["removeCreditCardToken"] = oTokenInformation;

               

                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("DeleteToken request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        
                        Logger_AddLogMessage(string.Format("DeleteToken response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);
                        string strError = "ERROR";
                      
                        try
                        {
                            strError = oResponse["code"];
                            if (strError == "ERROR")
                            {

                                eErrorCode = PayuErrorCode.ERROR;
                                errorMessage = oResponse["error"];
                            }
                            else
                            {
                                eErrorCode = PayuErrorCode.APPROVED;
                                errorMessage = ErrorMessageDict[eErrorCode];                                    
                            }
                        }
                        catch
                        {
                            eErrorCode = PayuErrorCode.InternalError;
                        }


                        if (eErrorCode == PayuErrorCode.APPROVED)
                        {
                            bRes = true;
                        }


                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("DeleteToken Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "DeleteToken::Exception", LogLevels.logERROR);
                    eErrorCode = PayuErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "DeleteToken::Exception", LogLevels.logERROR);

            }

            return bRes;
        }


        public static string UserReference()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}{1:000}", DateTime.Now.ToUniversalTime(), m_oRandom.Next(0, 999));
        }

        public static bool IsError(PayuErrorCode eErrorCode)
        {
            return (eErrorCode != PayuErrorCode.APPROVED);
        }


        public static string Language(string strLang)
        {
            return (strLang == "es") ? strLang : "en";
        }

        public string CalculateMD5Hash(string input)
        {
         
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

         
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {                
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    
        


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }

        public static void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }

        static string PrettyJSON(string json)
        {

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string strRes = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented, new DecimalJsonConverter());
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }

        protected void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        protected void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        public bool RefundEnabled
        {
            get { return true; }
        }
        public bool PartialRefundEnabled
        {
            get { return false; }
        }
    }


    class DecimalJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((decimal)value).ToString("#.##", CultureInfo.InvariantCulture));
        }
    }
}
