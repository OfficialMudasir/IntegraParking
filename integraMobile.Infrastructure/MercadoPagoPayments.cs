using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using MercadoPago.Client;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Customer;
using MercadoPago.Resource;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Customer;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;



namespace integraMobile.Infrastructure
{
    public class MercadoPagoPayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(MercadoPagoPayments));
        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));

        public enum MercadoPagoErrorCode
        {
            Approved = 0,
            Authorized = 1,
            Rejected = 2,
            InternalError = 1000,

        }


        static readonly public Dictionary<MercadoPagoErrorCode, string> ErrorMessageDict = new Dictionary<MercadoPagoErrorCode, string>()
        {
            {MercadoPagoErrorCode.Approved,"Approved"},
            {MercadoPagoErrorCode.Authorized,"Authorized"},
            {MercadoPagoErrorCode.Rejected,"Rejected (Status=={0})"},
            {MercadoPagoErrorCode.InternalError,"Internal error."},
        };



        public bool SearchCustomerByEmail(string strAPIURL, 
                                          string strAccessToken,
                                          string strEmail,                                          
                                          int iWSTimeout,
                                          out string strCustomerId)
        {
            bool bResult = false;
            long lEllapsedTime = 0;
            Stopwatch watch = null;
            int iLocalWSTimeout = iWSTimeout;
            strCustomerId = "";


            try
            {
                AddTLS12Support();

                string sBaseUrl = strAPIURL;


                string sUrl = string.Format("{0}/v1/customers/search?email={1}", sBaseUrl, strEmail);

                WebRequest oRequest = WebRequest.Create(sUrl);


                oRequest.Method = "GET";
                oRequest.ContentType = "application/x-www-form-urlencoded";
                //oRequest.ContentType = "application/json";
                oRequest.Timeout = iLocalWSTimeout;
                oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", strAccessToken));

                watch = Stopwatch.StartNew();

                Logger_AddLogMessage(string.Format("SearchCustomerByEmail request.url={0}, Timeout={2}, request.authorization={1}", sUrl, strAccessToken, oRequest.Timeout), LogLevels.logINFO);


                try
                {

                    WebResponse response = oRequest.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.


                        Logger_AddLogMessage( (string.Format("SearchCustomerByEmail response.json={0}", PrettyJSON(responseFromServer))), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);
                        reader.Close();
                        dataStream.Close();

                        var oResults = oResponse["results"];

                        if (oResults != null)
                        {
                            foreach (var oResult in oResults)
                            {
                                strCustomerId = oResult["id"].ToString();
                                bResult = true;
                                break;

                            }
                        }                      
                    }

                    response.Close();
                }
                catch (WebException ex)
                {
                    Logger_AddLogException(ex, "MercadoPagoPayments.SearchCustomerByEmail: ", LogLevels.logERROR);

                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "MercadoPagoPayments.SearchCustomerByEmail: ", LogLevels.logERROR);

                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "MercadoPagoPayments.SearchCustomerByEmail: ", LogLevels.logERROR);

                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

            }

            return bResult;
        }


        public bool GetTokenFromCardId(string strAPIURL,
                                  string strAccessToken,
                                  string strCardId,
                                  int iWSTimeout,
                                  out string strTransactionToken)
        {
            bool bResult = false;
            long lEllapsedTime = 0;
            Stopwatch watch = null;
            int iLocalWSTimeout = iWSTimeout;
            strTransactionToken = "";


            try
            {
                AddTLS12Support();

                string sBaseUrl = strAPIURL;


                string sUrl = string.Format("{0}/v1/card_tokens", sBaseUrl);

                WebRequest oRequest = WebRequest.Create(sUrl);

                oRequest.Method = "POST";
                //oRequest.ContentType = "application/x-www-form-urlencoded";
                oRequest.ContentType = "application/json";
                oRequest.Timeout = iLocalWSTimeout;
                oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", strAccessToken));

                Dictionary<string, object> oParamsDict = new Dictionary<string, object>();
                oParamsDict["card_id"] = strCardId;

                string sJsonIn = JsonConvert.SerializeObject(oParamsDict);

                byte[] byteArray = Encoding.UTF8.GetBytes(sJsonIn);

                Logger_AddLogMessage(string.Format("GetTokenFromCardId request.url={0}, Timeout={2}, request.authorization={1}, json={3}", sUrl, strAccessToken, oRequest.Timeout, sJsonIn), LogLevels.logINFO);

                oRequest.ContentLength = byteArray.Length;
                Stream dataStream = oRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                watch = Stopwatch.StartNew();               

                try
                {

                    WebResponse response = oRequest.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if ((oWebResponse.StatusCode == HttpStatusCode.OK)|| (oWebResponse.StatusCode == HttpStatusCode.Created))
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.


                        Logger_AddLogMessage((string.Format("GetTokenFromCardId response.json={0}", PrettyJSON(responseFromServer))), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);
                        reader.Close();
                        dataStream.Close();

                        string strId = oResponse["id"].ToString();
                        string strStatus = oResponse["status"].ToString();


                        if (strStatus=="active")
                        {
                            strTransactionToken = strId;
                            bResult = true;
                        }

                    }

                    response.Close();
                }
                catch (WebException ex)
                {
                    Logger_AddLogException(ex, "MercadoPagoPayments.GetTokenFromCardId: ", LogLevels.logERROR);

                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "MercadoPagoPayments.GetTokenFromCardId: ", LogLevels.logERROR);

                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "MercadoPagoPayments.GetTokenFromCardId: ", LogLevels.logERROR);

                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

            }

            return bResult;
        }




        public bool CreateCardToken(string strAPIURL,
                                  string strAccessToken,
                                  int iWSTimeout, 
                                  string strEmail,
                                  string strIdentificationType,
                                  string strIdentificationNumber,
                                  string strTransactionToken,
                                  ref string strCustomerID,
                                  out MercadoPagoErrorCode eErrorCode,
                                  out string strErrorMessage,
                                  out string strCardId,
                                  out string strCardScheme,
                                  out string strCardType,
                                  out string strPAN,
                                  out int iSecurityCodeLength,
                                  out string strExpirationDateMonth,
                                  out string strExpirationDateYear,
                                  out string strMercadoPagoDateTime)
        {
            bool bRes = false;
            strErrorMessage = "";
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];
            strCardScheme = "";
            strCardType = "";
            strPAN = "";
            strExpirationDateMonth = "";
            strExpirationDateYear = "";
            strMercadoPagoDateTime = "";
            strCardId = "";
            iSecurityCodeLength = 3;


            AddTLS12Support();

            try
            {

                MercadoPagoConfig.AccessToken = strAccessToken;
                Customer customer = null;
                var customerClient = new CustomerClient();

                if (string.IsNullOrEmpty(strCustomerID))
                {

                    try
                    {
                        string strIntCustomerId = "";
                        if (SearchCustomerByEmail(strAPIURL, strAccessToken, strEmail, iWSTimeout, out strIntCustomerId))
                        {
                            customer = customerClient.Get(strIntCustomerId);
                        }
                        else
                        {
                            CustomerRequest customerRequest = new CustomerRequest
                            {
                                Email = strEmail,
                                Identification = new IdentificationRequest
                                {
                                    Type = strIdentificationType,
                                    Number = strIdentificationNumber,
                                },
                            };


                            if (customer == null)
                            {
                                customer = customerClient.Create(customerRequest);
                                Logger_AddLogMessage(string.Format("MercadoPagoPayments.CreateCardToken.CreateCustomer: Response: {0}", PrettyJSON(customer.ApiResponse.Content)), LogLevels.logINFO);
                            }
                        }
                    }
                    catch (MercadoPago.Error.MercadoPagoApiException e1)
                    {

                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e1.Message;
                        Logger_AddLogException(e1, "MercadoPagoPayments.CreateCardToken: ", LogLevels.logERROR);                      

                    }
                    catch (MercadoPago.Error.MercadoPagoException e2)
                    {
                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e2.Message;
                        Logger_AddLogException(e2, "MercadoPagoPayments.CreateCardToken: ", LogLevels.logERROR);


                    }
                    catch (Exception e3)
                    {
                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e3.Message;
                        Logger_AddLogException(e3, "MercadoPagoPayments.CreateCardToken: ", LogLevels.logERROR);

                    }
                }
                else
                {
                    customer = customerClient.Get(strCustomerID);
                }


                if (customer!=null)
                {
                    try
                    {
                        var cardRequest = new CustomerCardCreateRequest
                        {
                            Token = strTransactionToken,                                                       
                        };
                        CustomerCard card = customerClient.CreateCard(customer.Id, cardRequest);

                        Logger_AddLogMessage(string.Format("MercadoPagoPayments.CreateCardToken.CreateCard: Response: {0}", PrettyJSON(card.ApiResponse.Content)), LogLevels.logINFO);


                        if (card != null)
                        {
                            strCardId = card.Id;
                            strCustomerID = card.CustomerId;
                            strCardScheme = card.PaymentMethod.Id;
                            strCardType = card.PaymentMethod.PaymentTypeId;
                            strPAN = card.FirstSixDigits + "******" + card.LastFourDigits;
                            strExpirationDateMonth = card.ExpirationMonth.ToString().PadLeft(2, '0');
                            strExpirationDateYear = card.ExpirationYear.ToString();
                            strMercadoPagoDateTime = card.DateLastUpdated.Value.ToString("HHmmssddMMyy");

                            if (card.SecurityCode.Length.HasValue)
                                iSecurityCodeLength = card.SecurityCode.Length.Value;

                            Logger_AddLogMessage(
                                string.Format("MercadoPagoPayments.CreateCardToken Success: CustomerID={0}; CardScheme={1}; PAN={2}; ExpirationMonth={3}; ExpirationYear={4}; Token={5}; MercadoPagoDateTime={6}; CVVLength={7}",
                                                        strCustomerID,
                                                        strCardScheme,
                                                        strPAN,
                                                        strExpirationDateMonth,
                                                        strExpirationDateYear,
                                                        strTransactionToken,
                                                        strMercadoPagoDateTime,
                                                        iSecurityCodeLength), LogLevels.logINFO);

                            bRes = true;
                            eErrorCode = MercadoPagoErrorCode.Approved;
                            strErrorMessage = ErrorMessageDict[eErrorCode];

                        }
                    }
                    catch (MercadoPago.Error.MercadoPagoApiException e1)
                    {

                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e1.ApiError.Message;
                        Logger_AddLogException(e1, "MercadoPagoPayments.CreateCardToken: ", LogLevels.logERROR);

                    }
                    catch (MercadoPago.Error.MercadoPagoException e2)
                    {
                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e2.Message;
                        Logger_AddLogException(e2, "MercadoPagoPayments.CreateCardToken: ", LogLevels.logERROR);


                    }
                    catch (Exception e3)
                    {
                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e3.Message;
                        Logger_AddLogException(e3, "MercadoPagoPayments.CreateCardToken: ", LogLevels.logERROR);

                    }
                }

            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.CreateCardToken: ", LogLevels.logERROR);
            }

            return bRes;
        }



        public bool DeleteCardToken(string strAccessToken,
                          string strCustomerID,
                          string strCardId,
                          out MercadoPagoErrorCode eErrorCode,
                          out string strErrorMessage)
        {
            bool bRes = false;
            strErrorMessage = "";
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];


            AddTLS12Support();

            try
            {
                AddTLS12Support();

                MercadoPagoConfig.AccessToken = strAccessToken;
                Customer customer = null;
                var customerClient = new CustomerClient();


                customer = customerClient.Get(strCustomerID);


                if (customer != null)
                {
                    try
                    {                       
                        CustomerCard card = customerClient.DeleteCard(strCustomerID, strCardId);

                        Logger_AddLogMessage(string.Format("MercadoPagoPayments.DeleteCardToken.DeleteCard: Response: {0}", PrettyJSON(card.ApiResponse.Content)), LogLevels.logINFO);


                        if (card != null)
                        {
                                                 
                            bRes = true;
                            eErrorCode = MercadoPagoErrorCode.Approved;
                            strErrorMessage = ErrorMessageDict[eErrorCode];

                        }
                    }
                    catch (MercadoPago.Error.MercadoPagoApiException e1)
                    {

                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e1.ApiError.Message;
                        Logger_AddLogException(e1, "MercadoPagoPayments.DeleteCardToken: ", LogLevels.logERROR);

                    }
                    catch (MercadoPago.Error.MercadoPagoException e2)
                    {
                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e2.Message;
                        Logger_AddLogException(e2, "MercadoPagoPayments.DeleteCardToken: ", LogLevels.logERROR);


                    }
                    catch (Exception e3)
                    {
                        eErrorCode = MercadoPagoErrorCode.InternalError;
                        strErrorMessage = e3.Message;
                        Logger_AddLogException(e3, "MercadoPagoPayments.DeleteCardToken: ", LogLevels.logERROR);

                    }
                }

            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.DeleteCardToken: ", LogLevels.logERROR);
            }

            return bRes;
        }




        public bool AutomaticTransaction( string strAccessToken,
                                          string strOrderId,
                                          decimal dAmount,
                                          string strDescription,
                                          string strTransactionToken,
                                          string strCustomerID,
                                          int iInstallaments,
                                          bool bCapture,
                                          bool bBinaryMode,
                                          out MercadoPagoErrorCode eErrorCode,
                                          out string strErrorMessage,
                                          out string strTransactionId,
                                          out string strMercadoPagoDateTime)
        {
            bool bRes = false;
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];
            strTransactionId = "";
            strMercadoPagoDateTime = "";


            AddTLS12Support();

            try
            {

                MercadoPagoConfig.AccessToken = strAccessToken;

                try
                {
                    var client = new PaymentClient();

                   
                    var paymentRequest = new PaymentCreateRequest
                    {
                        ExternalReference = strOrderId,
                        TransactionAmount = dAmount,
                        Token = strTransactionToken,
                        Description = strDescription,
                        Installments = iInstallaments,                        
                        Capture = bCapture,
                        BinaryMode = bBinaryMode,
                        Payer = new PaymentPayerRequest
                        {
                            Id = strCustomerID,
                            Type = "customer",                           
                        },
                    };

                    
                    Payment payment = client.Create(paymentRequest);

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.AutomaticTransaction: Response: {0}", PrettyJSON(payment.ApiResponse.Content)), LogLevels.logINFO);
                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.AutomaticTransaction: Status={0} - {1}", payment.Status, payment.StatusDetail), LogLevels.logINFO);


                    if ((payment.Status == "approved") && (bCapture))
                    {
                        bRes = true;
                        eErrorCode = MercadoPagoErrorCode.Approved;
                        strErrorMessage = ErrorMessageDict[eErrorCode];
                        strTransactionId = payment.Id.ToString();
                        strMercadoPagoDateTime = payment.DateLastUpdated.Value.ToString("HHmmssddMMyy");
                    }
                    else if ((payment.Status == "authorized") && (!bCapture))
                    {
                        bRes = true;
                        eErrorCode = MercadoPagoErrorCode.Authorized;
                        strErrorMessage = ErrorMessageDict[eErrorCode];
                        strTransactionId = payment.Id.ToString();
                        strMercadoPagoDateTime = payment.DateLastUpdated.Value.ToString("HHmmssddMMyy");
                    }
                    /*else if (payment.Status == "in_process")
                    {
                        do
                        {
                            payment=client.Get(payment.Id.Value);
                            Logger_AddLogMessage(string.Format("MercadoPagoPayments.AutomaticTransaction: Response: {0}", PrettyJSON(payment.ApiResponse.Content)), LogLevels.logINFO);
                            Logger_AddLogMessage(string.Format("MercadoPagoPayments.AutomaticTransaction: Status={0} - {1}", payment.Status, payment.StatusDetail), LogLevels.logINFO);
                            System.Threading.Thread.Sleep(1000);

                        }
                        while (true);

                    }*/
                    else
                    {
                        eErrorCode = MercadoPagoErrorCode.Rejected;
                        strErrorMessage = payment.StatusDetail;
                    }

                }
                catch (MercadoPago.Error.MercadoPagoApiException e1)
                {

                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e1.ApiError.Message;
                    Logger_AddLogException(e1, "MercadoPagoPayments.AutomaticTransaction: ", LogLevels.logERROR);

                }
                catch (MercadoPago.Error.MercadoPagoException e2)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e2.Message;
                    Logger_AddLogException(e2, "MercadoPagoPayments.AutomaticTransaction: ", LogLevels.logERROR);


                }
                catch (Exception e3)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e3.Message;
                    Logger_AddLogException(e3, "MercadoPagoPayments.AutomaticTransaction: ", LogLevels.logERROR);

                }


            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.AutomaticTransaction: ", LogLevels.logERROR);
            }

            return bRes;
        }


        public bool AutomaticTransaction(string strAPIURL,
                                  string strAccessToken,
                                  int iWSTimeout,
                                  string strOrderId,
                                  decimal dAmount,
                                  string strDescription,
                                  string strCardId,
                                  string strCustomerID,
                                  int iInstallaments,
                                  bool bCapture,
                                  out MercadoPagoErrorCode eErrorCode,
                                  out string strErrorMessage,
                                  out string strTransactionId,
                                  out string strMercadoPagoDateTime)
        {
            bool bRes = false;
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];
            strTransactionId = "";
            string strTransactionToken = "";
            strMercadoPagoDateTime = "";



            try
            {
                AddTLS12Support();

                if (GetTokenFromCardId(strAPIURL, strAccessToken, strCardId, iWSTimeout, out strTransactionToken))
                {
                    bRes = AutomaticTransaction(strAccessToken,
                                           strOrderId,
                                           dAmount,
                                           strDescription,
                                           strTransactionToken,
                                           strCustomerID,
                                           iInstallaments,
                                           bCapture,
                                           true,
                                          out eErrorCode,
                                          out strErrorMessage,
                                          out strTransactionId,
                                          out strMercadoPagoDateTime);
                }
                else
                {
                    eErrorCode = MercadoPagoErrorCode.Rejected;
                    strErrorMessage = "token_not_available";
                }

            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.AutomaticTransaction: ", LogLevels.logERROR);
            }

            return bRes;
        }




        public bool CommitTransaction(string strAccessToken,
                                      string strTransactionId,
                                      decimal dAmount,
                                      out MercadoPagoErrorCode eErrorCode,
                                      out string strErrorMessage)
        {
            bool bRes = false;
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];


            AddTLS12Support();

            try
            {

                MercadoPagoConfig.AccessToken = strAccessToken;

                try
                {

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.CommitTransaction: Capturing {0} on transaction {1}", dAmount, strTransactionId), LogLevels.logINFO);

                    var client = new PaymentClient();
                    Payment payment = client.Capture(Convert.ToInt64(strTransactionId), dAmount);

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.CommitTransaction: Response: {0}", PrettyJSON(payment.ApiResponse.Content)), LogLevels.logINFO);
                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.CommitTransaction: Status={0} - {1}", payment.Status, payment.StatusDetail), LogLevels.logINFO);


                    if (payment.Status == "approved")
                    {
                        bRes = true;
                        eErrorCode = MercadoPagoErrorCode.Approved;
                        strErrorMessage = ErrorMessageDict[eErrorCode];
                    }
                    else
                    {
                        eErrorCode = MercadoPagoErrorCode.Rejected;
                        strErrorMessage = string.Format(ErrorMessageDict[eErrorCode], payment.Status);
                    }

                }
                catch (MercadoPago.Error.MercadoPagoApiException e1)
                {

                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e1.ApiError.Message;
                    Logger_AddLogException(e1, "MercadoPagoPayments.CommitTransaction: ", LogLevels.logERROR);

                }
                catch (MercadoPago.Error.MercadoPagoException e2)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e2.Message;
                    Logger_AddLogException(e2, "MercadoPagoPayments.CommitTransaction: ", LogLevels.logERROR);


                }
                catch (Exception e3)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e3.Message;
                    Logger_AddLogException(e3, "MercadoPagoPayments.CommitTransaction: ", LogLevels.logERROR);

                }


            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.CommitTransaction: ", LogLevels.logERROR);
            }

            return bRes;
        }


        public bool RefundTransaction(string strAccessToken,
                                      string strTransactionId,
                                      decimal? dAmount,
                                      out MercadoPagoErrorCode eErrorCode,
                                      out string strErrorMessage,
                                      out string strRefundTransactionId)
        {
            bool bRes = false;
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];
            strRefundTransactionId = "";


            AddTLS12Support();

            try
            {

                MercadoPagoConfig.AccessToken = strAccessToken;

                try
                {


                   
                    var client = new PaymentClient();
                    PaymentRefund refund = client.Refund(Convert.ToInt64(strTransactionId), dAmount);

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.RefundTransaction: Response: {0}", PrettyJSON(refund.ApiResponse.Content)), LogLevels.logINFO);
                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.RefundTransaction: Status={0}", refund.Status), LogLevels.logINFO);


                    if (refund.Status == "approved")
                    {
                        bRes = true;
                        eErrorCode = MercadoPagoErrorCode.Approved;
                        strErrorMessage = ErrorMessageDict[eErrorCode];
                        strRefundTransactionId = refund.Id.ToString();
                    }
                    else
                    {
                        eErrorCode = MercadoPagoErrorCode.Rejected;
                        strErrorMessage = string.Format(ErrorMessageDict[eErrorCode], refund.Status);
                    }

                }
                catch (MercadoPago.Error.MercadoPagoApiException e1)
                {

                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e1.ApiError.Message;
                    Logger_AddLogException(e1, "MercadoPagoPayments.RefundTransaction: ", LogLevels.logERROR);

                }
                catch (MercadoPago.Error.MercadoPagoException e2)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e2.Message;
                    Logger_AddLogException(e2, "MercadoPagoPayments.RefundTransaction: ", LogLevels.logERROR);


                }
                catch (Exception e3)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e3.Message;
                    Logger_AddLogException(e3, "MercadoPagoPayments.RefundTransaction: ", LogLevels.logERROR);

                }


            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.RefundTransaction: ", LogLevels.logERROR);
            }

            return bRes;
        }



        public bool CancelTransaction(string strAccessToken,
                                      string strTransactionId,
                                      out MercadoPagoErrorCode eErrorCode,
                                      out string strErrorMessage)
        {
            bool bRes = false;
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];


            AddTLS12Support();

            try
            {

                MercadoPagoConfig.AccessToken = strAccessToken;

                try
                {



                    var client = new PaymentClient();
                    Payment payment = client.Cancel(Convert.ToInt64(strTransactionId));

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.CancelTransaction: Response: {0}", PrettyJSON(payment.ApiResponse.Content)), LogLevels.logINFO);
                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.CancelTransaction: Status={0} - {1}", payment.Status, payment.StatusDetail), LogLevels.logINFO);


                    if (payment.Status == "cancelled")
                    {
                        bRes = true;
                        eErrorCode = MercadoPagoErrorCode.Approved;
                        strErrorMessage = ErrorMessageDict[eErrorCode];                        
                    }
                    else
                    {
                        eErrorCode = MercadoPagoErrorCode.Rejected;
                        strErrorMessage = string.Format(ErrorMessageDict[eErrorCode], payment.Status);
                    }

                }
                catch (MercadoPago.Error.MercadoPagoApiException e1)
                {

                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e1.ApiError.Message;
                    Logger_AddLogException(e1, "MercadoPagoPayments.CancelTransaction: ", LogLevels.logERROR);

                }
                catch (MercadoPago.Error.MercadoPagoException e2)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e2.Message;
                    Logger_AddLogException(e2, "MercadoPagoPayments.CancelTransaction: ", LogLevels.logERROR);


                }
                catch (Exception e3)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e3.Message;
                    Logger_AddLogException(e3, "MercadoPagoPayments.CancelTransaction: ", LogLevels.logERROR);

                }


            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.CancelTransaction: ", LogLevels.logERROR);
            }

            return bRes;
        }



        public bool GetTransaction(string strAccessToken,
                                   string strTransactionId,
                                   out MercadoPagoErrorCode eErrorCode,
                                   out string strErrorMessage,
                                   out string strEmail,
                                   out string strIdentificationType,
                                   out string strIdentificationNumber,
                                   out string strCardScheme,
                                   out string strCardType,
                                   out string strPAN,
                                   out string strExpirationDateMonth,
                                   out string strExpirationDateYear,
                                   out string strMercadoPagoDateTime,
                                   out int iInstallaments)
        {
            bool bRes = false;
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];

            strEmail = "";
            strIdentificationType = "";
            strIdentificationNumber = "";
            strCardScheme = "";
            strCardType = "";
            strPAN = "";
            strExpirationDateMonth = "";
            strExpirationDateYear = "";
            strMercadoPagoDateTime = "";
            iInstallaments = 0;


            AddTLS12Support();

            try
            {

                MercadoPagoConfig.AccessToken = strAccessToken;

                try
                {

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.GetTransaction: Getting Data from transaction {0}", strTransactionId), LogLevels.logINFO);

                    var client = new PaymentClient();
                    Payment payment = client.Get(Convert.ToInt64(strTransactionId));

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.GetTransaction: Response: {0}", PrettyJSON(payment.ApiResponse.Content)), LogLevels.logINFO);
                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.GetTransaction: Status={0} - {1}", payment.Status, payment.StatusDetail), LogLevels.logINFO);


                    if (payment.Status == "approved")
                    {
                        bRes = true;
                        eErrorCode = MercadoPagoErrorCode.Approved;
                        strErrorMessage = ErrorMessageDict[eErrorCode];
                        try
                        {
                            strMercadoPagoDateTime = payment.DateLastUpdated.Value.ToString("HHmmssddMMyy");
                            strEmail = payment.Payer.Email;
                            strIdentificationType = payment.Payer.Identification.Type;
                            strIdentificationNumber = payment.Payer.Identification.Number;
                            strCardScheme = payment.PaymentMethodId;
                            strCardType = payment.PaymentTypeId;
                            if (payment.Installments.HasValue)
                            {
                                iInstallaments = payment.Installments.Value;
                            }

                            if (payment.PaymentMethodId != "account_money")
                            {
                                strPAN = payment.Card.FirstSixDigits + "******" + payment.Card.LastFourDigits;
                                strExpirationDateMonth = payment.Card.ExpirationMonth.ToString().PadLeft(2, '0');
                                strExpirationDateYear = payment.Card.ExpirationYear.ToString();
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        eErrorCode = MercadoPagoErrorCode.Rejected;
                        strErrorMessage = string.Format(ErrorMessageDict[eErrorCode], payment.Status);
                    }

                }
                catch (MercadoPago.Error.MercadoPagoApiException e1)
                {

                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e1.ApiError.Message;
                    Logger_AddLogException(e1, "MercadoPagoPayments.GetTransaction: ", LogLevels.logERROR);

                }
                catch (MercadoPago.Error.MercadoPagoException e2)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e2.Message;
                    Logger_AddLogException(e2, "MercadoPagoPayments.GetTransaction: ", LogLevels.logERROR);


                }
                catch (Exception e3)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e3.Message;
                    Logger_AddLogException(e3, "MercadoPagoPayments.GetTransaction: ", LogLevels.logERROR);

                }


            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.GetTransaction: ", LogLevels.logERROR);
            }

            return bRes;
        }


        public bool GeneratePreference(string strAccessToken,
                                      string strOrderId,
                                      string strTitle,
                                      string strCurrencyISOCode,
                                      decimal dAmount,
                                      string strResponseSuccessURL,
                                      string strResponseFailureURL,
                                      string strResponsePedingURL,
                                      out MercadoPagoErrorCode eErrorCode,
                                      out string strErrorMessage,
                                      out string strPreferenceId,
                                      out string strIniPoint)
        {
            bool bRes = false;
            strErrorMessage = "";
            eErrorCode = MercadoPagoErrorCode.InternalError;
            strErrorMessage = ErrorMessageDict[eErrorCode];
            strPreferenceId = "";
            strIniPoint = "";


            try
            {
                AddTLS12Support();

                MercadoPagoConfig.AccessToken = strAccessToken;

                var request = new PreferenceRequest
                {
                    ExternalReference = strOrderId,
                    Items = new List<PreferenceItemRequest>
                    {                       
                        new PreferenceItemRequest
                        {
                            Title = strTitle,
                            Quantity = 1,
                            CurrencyId = strCurrencyISOCode,
                            UnitPrice = dAmount,
                        },
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = strResponseSuccessURL,
                        Failure = strResponseFailureURL,
                        Pending = strResponsePedingURL,
                    },
                    AutoReturn = "approved",
                    Purpose = "wallet_purchase",
                    BinaryMode = true,
                };

                try
                {

                    var client = new PreferenceClient();
                    Preference preference = client.Create(request);                   

                    Logger_AddLogMessage(string.Format("MercadoPagoPayments.GeneratePreference: Response: {0}", PrettyJSON(preference.ApiResponse.Content)), LogLevels.logINFO);


                    if (preference != null)
                    {

                        bRes = true;
                        eErrorCode = MercadoPagoErrorCode.Approved;
                        strErrorMessage = ErrorMessageDict[eErrorCode];
                        strPreferenceId = preference.Id;
                        strIniPoint = preference.InitPoint;
                    }
                }
                catch (MercadoPago.Error.MercadoPagoApiException e1)
                {

                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e1.ApiError.Message;
                    Logger_AddLogException(e1, "MercadoPagoPayments.GeneratePreference: ", LogLevels.logERROR);

                }
                catch (MercadoPago.Error.MercadoPagoException e2)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e2.Message;
                    Logger_AddLogException(e2, "MercadoPagoPayments.GeneratePreference: ", LogLevels.logERROR);


                }
                catch (Exception e3)
                {
                    eErrorCode = MercadoPagoErrorCode.InternalError;
                    strErrorMessage = e3.Message;
                    Logger_AddLogException(e3, "MercadoPagoPayments.GeneratePreference: ", LogLevels.logERROR);

                }
                

            }
            catch (Exception e)
            {
                eErrorCode = MercadoPagoErrorCode.InternalError;
                strErrorMessage = e.Message;
                Logger_AddLogException(e, "MercadoPagoPayments.GeneratePreference: ", LogLevels.logERROR);
            }

            return bRes;
        }

        public static bool AllowTransactionWithoutCVV(string cardSchema, string cardType)
        {
            bool bRes = false;

            switch (cardType.ToLower())
            {
                case "credit_card":
                    {
                        switch (cardSchema.ToLower())
                        {
                            case "master":
                                bRes = true;                                
                                break;
                            case "visa":
                                bRes = true;
                                break;
                            case "amex":
                                bRes = true;
                                break;
                            case "diners":
                                bRes = true;
                                break;
                            case "argencard":
                                bRes = true;
                                break;
                            case "cencosud":
                                bRes = true;
                                break;
                            case "cabal":
                                bRes = true;
                                break;
                            case "naranja":
                                bRes = false;
                                break;
                            case "tarshop":
                                bRes = false;
                                break;
                            default:
                                break;

                        }

                    }
                    break;
                case "debit_card":
                    {
                        switch (cardSchema.ToLower())
                        {
                            case "debmaster":
                                bRes = false;
                                break;
                            case "debvisa":
                                bRes = true;
                                break;
                            case "maestro":
                                bRes = false;
                                break;
                            case "debcabal":
                                bRes = true;
                                break;
                            default:
                                break;

                        }
                    }
                    break;

                default:
                    break;

            }

            return bRes;
        }

        public static bool AllowAuthorizationAndCapture(string cardSchema, string cardType)
        {
            bool bRes = false;

            switch (cardType.ToLower())
            {
                case "credit_card":
                    {
                        switch (cardSchema.ToLower())
                        {
                            case "master":
                                bRes = true;
                                break;
                            case "visa":
                                bRes = true;
                                break;
                            case "amex":
                                bRes = true;
                                break;
                            case "diners":
                                bRes = true;
                                break;
                            case "argencard":
                                bRes = true;
                                break;
                            case "cencosud":
                                bRes = true;
                                break;
                            case "cabal":
                                bRes = true;
                                break;
                            case "naranja":
                                bRes = true;
                                break;
                            case "tarshop":
                                bRes = true;
                                break;
                            default:
                                break;

                        }

                    }
                    break;
                case "debit_card":
                    {
                        switch (cardSchema.ToLower())
                        {
                            case "debmaster":
                                bRes = false;
                                break;
                            case "debvisa":
                                bRes = false;
                                break;
                            case "maestro":
                                bRes = false;
                                break;
                            case "debcabal":
                                bRes = false;
                                break;
                            default:
                                break;

                        }
                    }
                    break;

                default:
                    break;

            }

            return bRes;
        }


        public static bool IsError(MercadoPagoErrorCode eErrorCode)
        {
            return Convert.ToInt32(eErrorCode) > 1;
        }


        public static string UserReference()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}{1:000}", DateTime.Now.ToUniversalTime(), m_oRandom.Next(0, 999));
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

        protected static string PrettyJSON(string json)
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
            get { return true; }
        }
    }
}
