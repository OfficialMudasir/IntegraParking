using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Net;
using System.IO;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using PayPal;
using PayPal.Api;
using System.Xml.Serialization;
using System.Web.Script.Serialization;

namespace integraMobile.Infrastructure
{
    public class PaypalPayments : IPayments
    {
        #region Constructor
        //private static readonly string TEXT_END_POINT = "endpoint";
        private static readonly string TEXT_MODE = "mode";
        private static readonly string TEXT_CLIENT_ID = "clientId";
        private static readonly string TEXT_CLIENT_SECRET = "clientSecret";
        private static readonly string TEXT_CONNECTION_TIME_OUT = "connectionTimeout";
        private static readonly string TEXT_REQUEST_RETRIES = "requestRetries";
        private static readonly int DEFAULT_WS_TIMEOUT = 5000; //ms        
        #endregion

        #region Properties
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(PaypalPayments));
        public bool RefundEnabled
        {
            get { return true; }
        }
        public bool PartialRefundEnabled
        {
            get { return true; }
        }
        #endregion

        #region Public Methods


        static public bool ConfirmAppSDKPaypalPayment(string strPaypalClientID,
                                                     string strPaypalClientSecret,
                                                     string strPaypalURLPrefix,
                                                     string strAuthorizationId,
                                                     string strQuantity,
                                                     string strCurrencyISOCODE,
                                                     out string strSecondPaypalAuthId,
                                                     out int iTransactionFee,
                                                     out string strTransactionFeeCurrencyIsocode,
                                                     out string strTransactionURL,
                                                     out string strRefundTransactionURL)
        {

            bool bRes = false;
            strSecondPaypalAuthId = "";
            iTransactionFee = 0;
            strTransactionFeeCurrencyIsocode = "";
            strTransactionURL = "";
            strRefundTransactionURL = "";

            try
            {
                string strAccessToken = "";

                AddTLS12Support();

                if (GetPayPalToken(strPaypalClientID,
                                   strPaypalClientSecret,
                                   strPaypalURLPrefix,
                                   out strAccessToken))
                {
                    string strURL = string.Format("{0}payments/authorization/{1}/capture", strPaypalURLPrefix, strAuthorizationId);
                    WebRequest request = WebRequest.Create(strURL);

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Timeout = Get3rdPartyWSTimeout();

                    request.Headers["Authorization"] = "Bearer " + strAccessToken;

                    Dictionary<string, object> oDataDict = new Dictionary<string, object>();
                    Dictionary<string, object> oAmountDict = new Dictionary<string, object>();

                    oAmountDict["currency"] = strCurrencyISOCODE;
                    oAmountDict["total"] = strQuantity;
                    oDataDict["amount"] = oAmountDict;
                    oDataDict["is_final_capture"] = true;

                    var json = JsonConvert.SerializeObject(oDataDict);

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.ConfirmAppSDKPaypalPayment: request.url={0}, request.json={1}", strURL, PrettyJSON(json)));

                    byte[] byteArray = Encoding.UTF8.GetBytes(json);

                    request.ContentLength = byteArray.Length;
                    // Get the request stream.

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


                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.ConfirmAppSDKPaypalPayment: StatusDescription={0}", oWebResponse.StatusDescription)); ;


                        if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                        {
                            // Get the stream containing content returned by the server.
                            dataStream = response.GetResponseStream();
                            // Open the stream using a StreamReader for easy access.
                            StreamReader reader = new StreamReader(dataStream);
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();
                            // Display the content.

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.ConfirmAppSDKPaypalPayment: responseFromServer={0}", PrettyJSON(responseFromServer)));
                            // Clean up the streams.

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            strSecondPaypalAuthId = oResponse["id"];
                            if (oResponse["transaction_fee"] != null)
                            {
                                strTransactionFeeCurrencyIsocode = oResponse["transaction_fee"]["currency"];
                                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                                numberFormatProvider.NumberDecimalSeparator = ".";
                                decimal dQuantity = Convert.ToDecimal(oResponse["transaction_fee"]["value"], numberFormatProvider) * 100;
                                iTransactionFee = Convert.ToInt32(dQuantity, numberFormatProvider);
                            }

                            if (oResponse["links"][0] != null)
                            {
                                strTransactionURL = oResponse["links"][0]["href"];
                            }
                            if (oResponse["links"][1] != null)
                            {
                                strRefundTransactionURL = oResponse["links"][1]["href"];
                            }


                            //strJobId = oJobStatus["id"];
                            reader.Close();
                            dataStream.Close();
                            bRes = true;
                        }
                        else
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.ConfirmAppSDKPaypalPayment: Got " + oWebResponse.StatusDescription + " response from server");

                        }

                        response.Close();
                    }
                    catch (WebException ex)
                    {
                        bRes = false;
                        if (ex.Response is HttpWebResponse)
                        {
                            HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                            m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.ConfirmAppSDKPaypalPayment: Error " + statusCode.ToString() + " response from server");
                            using (WebResponse wResponse = (HttpWebResponse)ex.Response)
                            {
                                try
                                {
                                    using (Stream data = wResponse.GetResponseStream())
                                    {
                                        string responseFromServer = new StreamReader(data).ReadToEnd();

                                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.ConfirmAppSDKPaypalPayment: responseFromServer={0}", PrettyJSON(responseFromServer)));
                                        // Clean up the streams.

                                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                        if (oResponse["name"] == "AUTHORIZATION_ALREADY_COMPLETED")
                                        {
                                            bRes = true;
                                        }

                                    }

                                }
                                catch
                                { }

                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.ConfirmAppSDKPaypalPayment: ", e);

            }

            return bRes;


        }



        static public bool ExpressCheckoutPassOne(string strBaseAmount,
                                                 string strCurISOCode,
                                                 string strCancelURL,
                                                 string strReturnURL,
                                                 string strPaypalOrderDescription,
                                                 string QFEE,
                                                 string QVAT,
                                                 string Total,
                                                 string paypalApiClientId, 
                                                 string paypalApiClientSecret,
                                                 string paypalApiClientUrl,
                                                 string paypalApiMode,
                                                 int paypalConnectionTimeout,
                                                 out string slinkPayPal,
                                                 out string sToken)
                                                            
        {
            bool bRes = false;
            string strLink = string.Empty;
            sToken = string.Empty;

            try
            {

                APIContext apiContext = CallApiContext(paypalApiClientId, paypalApiClientSecret, paypalApiClientUrl, paypalApiMode, paypalConnectionTimeout);

                if (apiContext != null)
                {
                    // ###Items
                    // Items within a transaction.
                    var itemList = new ItemList()
                    {
                        items = new List<Item>() 
                    {
                        new Item()
                        {
                            
                            name = strPaypalOrderDescription,
                            currency = strCurISOCode,
                            price = strBaseAmount,
                            quantity="1",
                            sku = "sku"
                        }
                    }
                    };

                    // ###Payer
                    var payer = new Payer() { payment_method = "paypal" };

                    // ###Redirect URLS
                    var baseURI = strReturnURL;
                    var redirectUrl = baseURI;
                    var redirUrls = new RedirectUrls()
                    {
                        cancel_url = strCancelURL,
                        return_url = strReturnURL
                    };

                    // ###Details
                    // Let's you specify details of a payment amount.
                    var details = new Details()
                    {  
                        tax = QVAT,
                        shipping ="0",
                        handling_fee = QFEE,
                        //shipping = QFEE,
                        subtotal = strBaseAmount
                    };

                    // ###Amount
                    // Let's you specify a payment amount.
                    var amount = new Amount()
                    {
                        currency = strCurISOCode,
                        total = Total, // Total must be equal to sum of shipping, tax and subtotal.
                        details = details
                    };

                    // ###Transaction
                    var transactionList = new List<Transaction>();
                    transactionList.Add(new Transaction()
                    {
                        description = strPaypalOrderDescription,
                        invoice_number = GetRandomInvoiceNumber(),
                        amount = amount,
                        item_list = itemList
                    });

                    // ###Payment
                    // A Payment Resource; create one using
                    // the above types and intent as `sale` or `authorize`
                    var payment = new Payment()
                    {
                        intent = "authorize",
                        payer = payer,
                        transactions = transactionList,
                        redirect_urls = redirUrls
                    };

                    // Create a payment using a valid APIContext
                    var createdPayment = payment.Create(apiContext);
                    if (createdPayment != null)
                    {
                        var jsoncreatedPayment = new JavaScriptSerializer().Serialize(createdPayment);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.ExpressCheckoutPassOne. createdPayment={0}", PrettyJSON(jsoncreatedPayment)));
                    }

                    // Using the `links` provided by the `createdPayment` object, we can give the user the option to redirect to PayPal to approve the payment.
                    if (createdPayment != null && createdPayment.links.Count > 0)
                    {
                        m_Log.LogMessage(LogLevels.logINFO, "Create PayPal payment:: createdPayment.links:" + Convert.ToString(createdPayment.links.Count));
                    }
                    var links = createdPayment.links.GetEnumerator();

                    while (links.MoveNext())
                    {
                        if (links.Current != null)
                        {
                            m_Log.LogMessage(LogLevels.logINFO, "Create PayPal payment:: createdPayment.links:" + Convert.ToString(createdPayment.links.Count));
                        }

                        var link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            m_Log.LogMessage(LogLevels.logINFO, "Redirect to PayPal to approve the payment...");
                            strLink = link.href;
                            m_Log.LogMessage(LogLevels.logINFO, "Redirect= " + strLink);
                            sToken = createdPayment.token;
                            bRes = true;
                            break;
                        }
                    }
                }
            }
            catch (PayPalException ex)
            //catch (Exception ex)
            {

                string mensaje = string.Empty;
                if (ex.InnerException is PayPal.ConnectionException)
                {
                    mensaje = ex.InnerException.ToString();
                }
                else
                {
                    mensaje = ex.Message.ToString();
                }
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.ExpressCheckoutPassOne: ", ex);

            }
            slinkPayPal = strLink;
            return bRes;
        }

        static public bool VoidPaypal(string paymentid,
                                        string paypalApiClientId,
                                        string paypalApiClientSecret,
                                        string paypalApiClientUrl,
                                        string paypalApiMode,
                                        int paypalConnectionTimeout,
                                        out string sAuthCode,
                                        out string stransactionID)
        {
            bool bRes = false;
            string strLink = string.Empty;
            string strAuthCode = string.Empty;
            string strtransactionID = string.Empty;
            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "PaypalPayments.VoidPaypal:: CallApiContext");
                APIContext apiContext = CallApiContext(paypalApiClientId, paypalApiClientSecret, paypalApiClientUrl, paypalApiMode, paypalConnectionTimeout);

                if (apiContext != null)
                {
                    m_Log.LogMessage(LogLevels.logINFO, "PaypalPayments.VoidPaypal:: Payment.Get");
                    Payment payment = Payment.Get(apiContext, paymentid);
                    
                    if (payment != null)
                    {
                        var jsonPayment = new JavaScriptSerializer().Serialize(payment);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.VoidPaypal.Payment.Get: payment={0}", PrettyJSON(jsonPayment)));
                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.VoidPaypal:: Payment.Get payment is null");
                    }


                    m_Log.LogMessage(LogLevels.logINFO, "PaypalPayments.VoidPaypal:: payment.transactions.GetEnumerator");
                    List<Transaction> transactions = payment.transactions;

                    if (transactions.Count>0)
                    {
                        var jsonTransactions = new JavaScriptSerializer().Serialize(transactions);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.VoidPaypal.payment.transactions: transactions={0}", PrettyJSON(jsonTransactions)));
                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.VoidPaypal:: transactions.Count=0");
                    }



                    m_Log.LogMessage(LogLevels.logINFO, "PaypalPayments.VoidPaypal:: transactions.Current.related_resources[0].authorization");
                    PayPal.Api.Authorization authorization = new PayPal.Api.Authorization();
                    
                    foreach (PayPal.Api.Transaction oTransaction in transactions)
                    {
                        authorization = oTransaction.related_resources[0].authorization;
                        if (authorization != null)
                        {
                            var jsonAuthorization = new JavaScriptSerializer().Serialize(authorization);
                            m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.VoidPaypal. Transaction.related_resources[0].authorization: authorization={0}", PrettyJSON(jsonAuthorization)));
                        }
                        else
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.VoidPaypal::  Transaction.related_resources[0].authorization authorization is null");
                        }
                        break;
                    }

                    m_Log.LogMessage(LogLevels.logINFO, "PaypalPayments.VoidPaypal:: authorization.Void");
                    var response = authorization.Void(apiContext);

                    if (response.state.Equals("voided"))
                    {
                        var jsonResponse = new JavaScriptSerializer().Serialize(response);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.VoidPaypal. authorization.Void: response={0}", PrettyJSON(jsonResponse)));
                        strAuthCode = response.id;
                        strtransactionID = response.parent_payment;
                        bRes = true;
                    }
                    else
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("PaypalPayments.VoidPaypal. authorization.Void: response.state={0}", response.state));
                    }

                }

            }
            catch (PayPalException ex)
            //catch (Exception ex)
            {

                string mensaje = string.Empty;
                if (ex.InnerException is PayPal.ConnectionException)
                {
                    mensaje = ex.InnerException.ToString();
                }
                else
                {
                    mensaje = ex.Message.ToString();
                }
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.VoidPaypal: ", ex);

            }
            sAuthCode = strAuthCode;
            stransactionID = strtransactionID;
            return bRes;
        }

        static public bool RefundCapturedPaymentPaypal(string strAmount,
                                             string strCurISOCode,
                                             string paymentid,
                                             string paypalApiClientId,
                                             string paypalApiClientSecret,
                                             string paypalApiClientUrl,
                                             string paypalApiMode,
                                             int paypalConnectionTimeout,
                                             out string stransactionID)
        {
            bool bRes = false;
            string strLink = string.Empty;
            stransactionID=null;


            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "PaypalPayments.RefundCapturedPaymentPaypal:: CallApiContext");
                APIContext apiContext = CallApiContext(paypalApiClientId, paypalApiClientSecret, paypalApiClientUrl, paypalApiMode, paypalConnectionTimeout);


                if (apiContext != null)
                {

                    var payment = Capture.Get(apiContext, paymentid);

                    var refund = new Refund()
                    {
                        amount = new Amount()
                        {
                            currency = strCurISOCode,
                            total = strAmount
                        },
                    };

                    var response = payment.Refund(apiContext, refund);


                    if (response != null)
                    {
                        if (response.state.Equals("completed"))
                        {
                            stransactionID = response.id;
                            bRes = true;
                        }
                        try
                        {
                            var jsonResponse = new JavaScriptSerializer().Serialize(response);
                            m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.RefundCapturedPaymentPaypal. Capture.Refund: response={0}", PrettyJSON(jsonResponse)));
                        }
                        catch { }

                    }
                }                   
                                    
            }
            catch (PayPalException ex)
            {

                string mensaje = string.Empty;
                if (ex.InnerException is PayPal.ConnectionException)
                {
                    mensaje = ex.InnerException.ToString();
                }
                else
                {
                    mensaje = ex.Message.ToString();
                }
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.RefundCapturedPaymentPaypal: ", ex);

            }
            catch (Exception ex)
            {
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.RefundCapturedPaymentPaypal: ", ex);

            }
            
            return bRes;
        }


        static public bool ExpressCheckoutPassTwo(string sTokenPassTwo, string sPaymentId, string sPayerID, string paypalApiClientId,
                                                 string paypalApiClientSecret, string paypalApiClientUrl, string paypalApiMode, int paypalConnectionTimeout,
                                                 out string strAuthCode, out string strCreationTime, out string strIntent, out string  strState)
        {
            bool bRes = false;
            string time = string.Empty;
            string authCode = string.Empty;
            string state = string.Empty;
            string intent = string.Empty;
            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "PaypalPayments.ExpressCheckoutPassTwo::");
                APIContext apiContext = CallApiContext(paypalApiClientId, paypalApiClientSecret, paypalApiClientUrl, paypalApiMode, paypalConnectionTimeout);

                // Using the information from the redirect, setup the payment to execute.
                var paymentId = sPaymentId;
                var paymentExecution = new PaymentExecution() { payer_id = sPayerID };
                var payment = new Payment() { id = paymentId };

                // Execute the payment.
                m_Log.LogMessage(LogLevels.logINFO, "Execute PayPal payment");
                var executedPayment = payment.Execute(apiContext, paymentExecution);
                if (executedPayment.state == "approved")
                {
                    time = executedPayment.create_time;
                    authCode = executedPayment.transactions[0].related_resources[0].authorization.id;
                    intent = executedPayment.intent;
                    state = executedPayment.state;                    
                    bRes = true;
                    m_Log.LogMessage(LogLevels.logINFO, "PayPal payment approved successfully.");
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.ExpressCheckoutPassTwo:: ", e);
                bRes = false;
            }
            strCreationTime = time;
            strAuthCode = authCode;
            strIntent = intent;
            strState = state;
            return bRes;
        }

        
        #endregion

        #region Private Methods
        static private APIContext CallApiContext(string paypalApiClientId, string paypalApiClientSecret, string paypalApiClientUrl, string paypalApiMode, int connectionTimeout)
        {
            string accessToken = string.Empty;
            APIContext apiContext = null;
            m_Log.LogMessage(LogLevels.logINFO, "PayPal CallApiContext");
            if (!string.IsNullOrEmpty(paypalApiClientId) && !string.IsNullOrEmpty(paypalApiClientSecret) && !string.IsNullOrEmpty(paypalApiClientUrl))
            {
                try
                {
                    AddTLS12Support();

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("PayPal CallApiContext:: ClientId={0} ClientSecret={1} ClientURL={2} Enviroment={3} ",
                        paypalApiClientId, paypalApiClientSecret, paypalApiClientUrl, paypalApiMode.ToLower()));


                    Dictionary<string, string> config = new Dictionary<string, string>();
                    //config.Add(TEXT_END_POINT, paypalApiClientUrl);
                    config.Add(TEXT_MODE, paypalApiMode.ToLower());
                    config.Add(TEXT_CLIENT_ID, paypalApiClientId);
                    config.Add(TEXT_CLIENT_SECRET, paypalApiClientSecret);
                    config.Add(TEXT_CONNECTION_TIME_OUT, connectionTimeout.ToString());
                    config.Add(TEXT_REQUEST_RETRIES, ConfigurationModel.PaypalRequestRetries);

                    if (String.IsNullOrEmpty(ConfigurationModel.PaypalConnectionTimeout.ToString()))
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "ERROR::PayPal CallApiContext:: WebConfig PAYPAL_CONNECTION_TIMEOUT is Null or Empty ");
                    }

                    if (String.IsNullOrEmpty(ConfigurationModel.PaypalRequestRetries.ToString()))
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "ERROR::PayPal CallApiContext:: WebConfig PAYPAL_REQUEST_RETRIES is Null or Empty ");
                    }

                    OAuthTokenCredential target = new OAuthTokenCredential(config);
                    accessToken = target.GetAccessToken();
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "ERROR::PayPal CallApiContext:: accessToken IS NULL ");
                    }
                    apiContext = new APIContext(accessToken);
                    apiContext.Config = config;

                }
                catch (PayPalException ex)
                //catch (Exception ex)
                {

                    string mensaje = string.Empty;
                    if (ex.InnerException is PayPal.ConnectionException)
                    {
                        mensaje = ex.InnerException.ToString();
                    }
                    else
                    {
                        mensaje = ex.Message.ToString();
                    }
                    m_Log.LogMessage(LogLevels.logERROR, "Error::PayPal CallApiContext: ", ex);

                }
                
            }
            else
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("Error::CallApiContext:: No data for PayPal settings: paypalApiClientId={0} paypalApiClientSecret={1} paypalApiClientUrl={2}: ", paypalApiClientId, paypalApiClientSecret, paypalApiClientUrl));
            }
            return apiContext;
        }

        private static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }

        static protected bool GetPayPalToken(string strClientId,
                                             string strSecret,
                                             string strPaypalURLPrefix,
                                             out string strAccessToken)
        {

            bool bRes = false;
            strAccessToken = "";

            try
            {
                AddTLS12Support();

                string strURL = string.Format("{0}oauth2/token", strPaypalURLPrefix);

                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Language:en_US");
                request.Timeout = Get3rdPartyWSTimeout();

                string authInfo = strClientId + ":" + strSecret;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                request.Headers["Authorization"] = "Basic " + authInfo;

                using (StreamWriter swt = new StreamWriter(request.GetRequestStream()))
                {
                    swt.Write("grant_type=client_credentials");
                }


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("PaypalPayments.GetPayPalToken response.json={0}", PrettyJSON(responseFromServer)));
                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        strAccessToken = (string)oResponse["access_token"];
                        bRes = true;
                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.GetPayPalToken: ", e);
                    bRes = false;
                }




            }
            catch (Exception e)
            {
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "PaypalPayments.GetPayPalToken: ", e);

            }

            return bRes;


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

        static protected int Get3rdPartyWSTimeout()
        {
            int iRes = DEFAULT_WS_TIMEOUT;
            try
            {
                iRes = Convert.ToInt32(ConfigurationManager.AppSettings["3rdPartyWSTimeout"].ToString());
            }
            catch
            {
                iRes = DEFAULT_WS_TIMEOUT;
            }

            return iRes;

        }

        protected static void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls11) != 0) //Disable TLS 1.1
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls) != 0) //Disable Tls 1.0
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }
        #endregion

        
    }
}