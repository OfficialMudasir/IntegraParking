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
using Moneris;

namespace integraMobile.Infrastructure
{
    public class MagpiePayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(MagpiePayments));
        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));

        public enum MagpieErrorCode
        {
            OK = 0,
            ConnectionFailed = 998,
            InternalError = 999,

        }


        static readonly public Dictionary<MagpieErrorCode, string> ErrorMessageDict = new Dictionary<MagpieErrorCode, string>()
        {
            {MagpieErrorCode.OK,"Transaction OK"},
            {MagpieErrorCode.ConnectionFailed,"Connection Failed"},
            {MagpieErrorCode.InternalError,"Internal Error"},
        };


       
        public bool CreateCheckoutSession(string strBaseUrl,
                                       string strSecretKey,
                                       string strDescription,
                                       string strCustomerId,
                                       int iAmount,
                                       string success_url,
                                       string cancel_url,
                                       int iServiceTimeout,
                                       string strUserReference,
                                       out MagpieErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strSessionId,
                                       out string strPaymentUrl)
        {
            bool bRes = false;
            strSessionId = null;
            strPaymentUrl = null;
            eErrorCode = MagpieErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];



            try
            {

                AddTLS12Support();

                string strUrl = strBaseUrl + "/sessions";
                WebRequest request = WebRequest.Create(strUrl);

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(strSecretKey + ":"));

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();


                oTransactionRequest["amount"] = iAmount;
                oTransactionRequest["currency"] = "PHP";
                oTransactionRequest["cancel_url"] = cancel_url;
                oTransactionRequest["submit_type"] = "pay";
                oTransactionRequest["success_url"] = success_url;
                oTransactionRequest["client_reference_id"] = strUserReference;
                oTransactionRequest["customer"] = strCustomerId;


                

                Dictionary<string, object> oLineItems = new Dictionary<string, object>();
                oLineItems["name"] = strDescription;
                oLineItems["description"] = strDescription;
                oLineItems["quantity"] = 1;
                oLineItems["amount"] = iAmount;
                oLineItems["currency"] = "php";

                oTransactionRequest["line_items"] = new ArrayList() { oLineItems };
                oTransactionRequest["payment_method_types"] = new ArrayList() { "card" };

                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("MagpiePayments.CreateCheckoutSession request.url={0}, request.json={1}", strUrl, PrettyJSON(json)), LogLevels.logINFO);


                request.Headers.Add("Authorization", "Basic " + encoded);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;

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


                    if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("MagpiePayments.CreateCheckoutSession response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        try
                        {

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);                           

                            try
                            {
                                strSessionId = oResponse["id"].Value;
                                strPaymentUrl = oResponse["payment_url"].Value;
                                eErrorCode = MagpieErrorCode.OK;
                                bRes = true;
                            }
                            catch
                            {
                                eErrorCode = MagpieErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];                            
                           
                        }

                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.CreateCheckoutSession: ", e);
                        }
                       
                    }
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("MagpiePayments.CreateCheckoutSession Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "MagpiePayments.CreateCheckoutSession::Exception", LogLevels.logERROR);
                    eErrorCode = MagpieErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }
            }            
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.CreateCheckoutSession: ", e);

            }

            return bRes;
        }


        public bool GetCheckoutSession(string strBaseUrl,
                                       string strSecretKey,
                                       int iServiceTimeout,
                                       string strSessionId,
                                       out MagpieErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strSourceId,
                                       out string strChargeId)
        {
            bool bRes = false;
            strChargeId = "";
            strSourceId = "";
            eErrorCode = MagpieErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];



            try
            {

                AddTLS12Support();

                string strUrl = strBaseUrl + "/sessions/" + strSessionId;
                WebRequest request = WebRequest.Create(strUrl);

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(strSecretKey + ":"));

                request.Method = "GET";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Logger_AddLogMessage(string.Format("MagpiePayments.GetCheckoutSession request.url={0}", strUrl), LogLevels.logINFO);

                request.Headers.Add("Authorization", "Basic " + encoded);

                request.ContentLength = 0;
              
                try
                {
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("MagpiePayments.GetCheckoutSession response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        try
                        {

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            try
                            {

                                var oPaymentDetails = oResponse["payment_details"];
                                var oSource = oPaymentDetails["source"];
                                strChargeId = oPaymentDetails["id"].Value;
                                strSourceId = oSource["id"].Value;
                                eErrorCode = MagpieErrorCode.OK;
                                bRes = true;
                            }
                            catch
                            {
                                eErrorCode = MagpieErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];

                        }

                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.GetCheckoutSession: ", e);
                        }

                    }
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("MagpiePayments.GetCheckoutSession Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "MagpiePayments.GetCheckoutSession::Exception", LogLevels.logERROR);
                    eErrorCode = MagpieErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.GetCheckoutSession: ", e);

            }

            return bRes;
        }



        public bool CreateCustomer(string strBaseUrl,
                              string strSecretKey,
                              int iServiceTimeout,
                              string strEmail,
                              out MagpieErrorCode eErrorCode,
                              out string errorMessage,
                              out string strCustomerId)
        {
            bool bRes = false;
            eErrorCode = MagpieErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strCustomerId = null;

            try
            {

                AddTLS12Support();

                string strUrl = strBaseUrl + "/customers/";
                WebRequest request = WebRequest.Create(strUrl);

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(strSecretKey + ":"));

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();


                oTransactionRequest["email"] = strEmail;
                oTransactionRequest["description"] = strEmail.Replace("@", "_") + " (created by Blinkay)";
                oTransactionRequest["mobile_number"] = "";

                Dictionary<string, object> oMetadataItems = new Dictionary<string, object>();
                oMetadataItems["example"] = "test";
                oTransactionRequest["metadata"] = oMetadataItems;

                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("MagpiePayments.CreateCustomer request.url={0}, request.json={1}", strUrl, PrettyJSON(json)), LogLevels.logINFO);


                request.Headers.Add("Authorization", "Basic " + encoded);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;

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


                    if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("MagpiePayments.CreateCustomer response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        try
                        {

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            try
                            {
                                strCustomerId = oResponse["id"].Value;
                                eErrorCode = MagpieErrorCode.OK;
                                bRes = true;
                            }
                            catch
                            {
                                eErrorCode = MagpieErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];

                        }

                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.CreateCustomer: ", e);
                        }

                    }
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("MagpiePayments.CreateCustomer Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "MagpiePayments.CreateCustomer::Exception", LogLevels.logERROR);
                    eErrorCode = MagpieErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.RefundCharge: ", e);

            }

            return bRes;
        }



        public bool RetrieveCustomerByEmail(string strBaseUrl,
                                       string strSecretKey,
                                       int iServiceTimeout,
                                       string strEmail,
                                       out MagpieErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strCustomerId)
        {
            bool bRes = false;
            strCustomerId = "";
            eErrorCode = MagpieErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];



            try
            {

                AddTLS12Support();

                string strUrl = strBaseUrl + "/customers/by_email/" + strEmail;
                WebRequest request = WebRequest.Create(strUrl);

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(strSecretKey + ":"));

                request.Method = "GET";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Logger_AddLogMessage(string.Format("MagpiePayments.RetrieveCustomerByEmail request.url={0}", strUrl), LogLevels.logINFO);

                request.Headers.Add("Authorization", "Basic " + encoded);

                request.ContentLength = 0;

                try
                {
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("MagpiePayments.RetrieveCustomerByEmail response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        try
                        {

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            try
                            {
                                strCustomerId = oResponse["id"].Value;
                                eErrorCode = MagpieErrorCode.OK;
                                bRes = true;
                            }
                            catch
                            {
                                eErrorCode = MagpieErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];

                        }

                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.RetrieveCustomerByEmail: ", e);
                        }

                    }
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("MagpiePayments.RetrieveCustomerByEmail Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "MagpiePayments.RetrieveCustomerByEmail::Exception", LogLevels.logERROR);
                    eErrorCode = MagpieErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.RetrieveCustomerByEmail: ", e);

            }

            return bRes;
        }


        public bool AttachSourceToCustomer(string strBaseUrl,
                      string strSecretKey,
                      int iServiceTimeout,
                      string strCustomerId,
                      string strSourceId,
                      out MagpieErrorCode eErrorCode,
                      out string errorMessage)
        {
            bool bRes = false;
            eErrorCode = MagpieErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            try
            {

                AddTLS12Support();

                string strUrl = strBaseUrl + "/customers/" + strCustomerId + "/sources";
                WebRequest request = WebRequest.Create(strUrl);

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(strSecretKey + ":"));

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();

                oTransactionRequest["source"] = strSourceId;

                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("MagpiePayments.AttachSourceToCustomer request.url={0}, request.json={1}", strUrl, PrettyJSON(json)), LogLevels.logINFO);


                request.Headers.Add("Authorization", "Basic " + encoded);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;

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


                    if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("MagpiePayments.AttachSourceToCustomer response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        try
                        {

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            try
                            {
                                eErrorCode = MagpieErrorCode.OK;
                                bRes = true;
                            }
                            catch
                            {
                                eErrorCode = MagpieErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];

                        }

                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.AttachSourceToCustomer: ", e);
                        }

                    }
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("MagpiePayments.AttachSourceToCustomer Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "MagpiePayments.AttachSourceToCustomer::Exception", LogLevels.logERROR);
                    eErrorCode = MagpieErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.AttachSourceToCustomer: ", e);

            }

            return bRes;
        }





        public bool AutomaticTransaction(string strBaseUrl,
                                         string strSecretKey,
                                         int iServiceTimeout,
                                         string strDescription,
                                         string strSourceId,
                                         int iAmount,
                                         out MagpieErrorCode eErrorCode,
                                         out string errorMessage,
                                         out string strChargeId)
        {
            bool bRes = false;
            eErrorCode = MagpieErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strChargeId = "";

            try
            {

                AddTLS12Support();
                    
                string strUrl = strBaseUrl + "/charges/";
                WebRequest request = WebRequest.Create(strUrl);

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(strSecretKey + ":"));

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();

                oTransactionRequest["amount"] = iAmount;
                oTransactionRequest["currency"] = "PHP";
                oTransactionRequest["source"] = strSourceId;
                oTransactionRequest["description "] = strDescription;
                oTransactionRequest["statement_descriptor"] = strDescription;
                oTransactionRequest["capture"] = true;
                oTransactionRequest["cvc"] = "724";

                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("MagpiePayments.AutomaticTransaction request.url={0}, request.json={1}", strUrl, PrettyJSON(json)), LogLevels.logINFO);


                request.Headers.Add("Authorization", "Basic " + encoded);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;

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


                    if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("MagpiePayments.AutomaticTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        try
                        {

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            try
                            {
                                if (oResponse["status"].Value = "succeeded")
                                {
                                    strChargeId = oResponse["id"].Value;
                                    eErrorCode = MagpieErrorCode.OK;
                                    bRes = true;
                                }
                            }
                            catch
                            {
                                eErrorCode = MagpieErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];

                        }

                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.RefundCharge: ", e);
                        }

                    }
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("MagpiePayments.AutomaticTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "MagpiePayments.AutomaticTransaction::Exception", LogLevels.logERROR);
                    eErrorCode = MagpieErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.AutomaticTransaction: ", e);

            }

            return bRes;
        }


        public bool RefundCharge(string strBaseUrl,
                               string strSecretKey,
                               int iServiceTimeout,
                               string strChargeId,
                               int iRefundAmount,
                               out MagpieErrorCode eErrorCode,
                               out string errorMessage)
        {
            bool bRes = false;
            eErrorCode = MagpieErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];



            try
            {

                AddTLS12Support();

                string strUrl = strBaseUrl + "/charges/"+strChargeId+"/refund";
                WebRequest request = WebRequest.Create(strUrl);

                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(strSecretKey + ":"));

                request.Method = "POST";
                request.ContentType = "application/json";
                ((HttpWebRequest)request).Accept = "application/json";
                request.Timeout = iServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();


                oTransactionRequest["amount"] = iRefundAmount;

                var json = JsonConvert.SerializeObject(oTransactionRequest, new DecimalJsonConverter());

                Logger_AddLogMessage(string.Format("MagpiePayments.RefundCharge request.url={0}, request.json={1}", strUrl, PrettyJSON(json)), LogLevels.logINFO);


                request.Headers.Add("Authorization", "Basic " + encoded);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;

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


                    if ((oWebResponse.StatusDescription == "OK") || (oWebResponse.StatusDescription == "Created"))
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.                       

                        Logger_AddLogMessage(string.Format("MagpiePayments.RefundCharge response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);


                        try
                        {

                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            try
                            {
                                if (oResponse["status"].Value = "refunded")
                                {
                                    eErrorCode = MagpieErrorCode.OK;
                                    bRes = true;
                                }
                            }
                            catch
                            {
                                eErrorCode = MagpieErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];

                        }

                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.RefundCharge: ", e);
                        }

                    }
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("MagpiePayments.RefundCharge Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "MagpiePayments.RefundCharge::Exception", LogLevels.logERROR);
                    eErrorCode = MagpieErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MagpiePayments.RefundCharge: ", e);

            }

            return bRes;
        }



        /*
        public bool GetTokenFromTransaction(string strStoreId,
                                       string strAPIToken,
                                       string strOrderId,
                                       string strTransaction,
                                       string strEmail,
                                       string strProcesingCountryCode,
                                       bool bTestMode,
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strCardReference)
        {
            bool bRes = false;
            strCardReference = null;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];


            try
            {

                AddTLS12Support();

                string processing_country_code = strProcesingCountryCode;
                bool status_check = false;


                ResTokenizeCC resTokenizeCC = new ResTokenizeCC();
                resTokenizeCC.SetOrderId(strOrderId);
                resTokenizeCC.SetTxnNumber(strTransaction);
                resTokenizeCC.SetEmail(strEmail);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(processing_country_code);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resTokenizeCC);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.GetTokenFromTransaction.Receipt: ");
                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());

                    //ResolveData
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResDataEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strCardReference = receipt.GetDataKey();
                    }

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);

            }

            return bRes;
        }



        public bool CardVerification(string strStoreId,
                                       string strAPIToken,
                                       string strOrderId,
                                       string strTemporaryToken,
                                       string strProcesingCountryCode,
                                       string strEmail,
                                       bool bTestMode,
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strIssuerId,
                                       out string strTransaction,
                                       out string strAuthCode,
                                       out string strAuthResult,
                                       out string strDateTime,
                                       out string strPAN,
                                       out string strCardScheme,
                                       out string strExpDate)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strTransaction = "";
            strAuthCode = "";
            strAuthResult = "";
            strDateTime = "";
            strPAN = "";
            strCardScheme = "";
            strExpDate = "";
            strIssuerId = "";

            try
            {

                AddTLS12Support();

                string processing_country_code = strProcesingCountryCode;
                bool status_check = false;
                string crypt_type = "7";

                CofInfo cof = new CofInfo();
                cof.SetPaymentIndicator("C");
                cof.SetPaymentInformation("0");
                cof.SetIssuerId("");

                ResCardVerificationCC rescardverify = new ResCardVerificationCC();
                rescardverify.SetDataKey(strTemporaryToken);
                rescardverify.SetOrderId(strOrderId);
                rescardverify.SetCustId(strEmail);
                rescardverify.SetCryptType(crypt_type);
                rescardverify.SetCofInfo(cof);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(processing_country_code);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(rescardverify);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.CardVerification.Receipt: ");
                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "Issuer ID = " + receipt.GetIssuerId());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());

                    //ResolveData
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResDataEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Card Type = " + receipt.GetCardType());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strIssuerId = receipt.GetIssuerId();
                        strAuthCode = receipt.GetAuthCode();
                        strAuthResult = receipt.GetReferenceNum();
                        strDateTime = receipt.GetTransDate() + " " + receipt.GetTransTime();
                        strTransaction = receipt.GetTxnNumber();
                        strPAN = receipt.GetResDataMaskedPan();
                        strExpDate = receipt.GetResDataExpdate();
                        strCardScheme = receipt.GetCardType();



                    }

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);

            }

            return bRes;
        }


        public bool GetPermanentToken(string strStoreId,
                               string strAPIToken,
                               string strTemporaryToken,
                               string strIssuerId,
                               string strProcesingCountryCode,
                               string strEmail,
                               string strExpDate,
                               bool bTestMode,
                               out MonerisErrorCode eErrorCode,
                               out string errorMessage,
                               out string strCardReference)
        {
            bool bRes = false;
            strCardReference = null;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            try
            {

                AddTLS12Support();

                string processing_country_code = strProcesingCountryCode;
                bool status_check = false;
                string crypt_type = "7";

                CofInfo cof = new CofInfo();
                cof.SetIssuerId(strIssuerId);
                cof.SetPaymentIndicator("Z");
                cof.SetPaymentInformation("2");


                ResAddToken resaddtoken = new ResAddToken();
                resaddtoken.SetDataKey(strTemporaryToken);
                resaddtoken.SetCustId(strEmail);
                resaddtoken.SetEmail(strEmail);
                resaddtoken.SetCryptType(crypt_type);
                resaddtoken.SetExpDate(strExpDate);
                resaddtoken.SetCofInfo(cof);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(processing_country_code);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resaddtoken);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.GetTokenFromTransaction.Receipt: ");
                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "Issuer ID = " + receipt.GetIssuerId());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());

                    m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strCardReference = receipt.GetDataKey();
                    }

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);

            }

            return bRes;
        }







        public bool AutomaticTransaction(string strStoreId,
                                       string strAPIToken,
                                       string strOrderId,
                                       string strCardReference,
                                       string strIssuerId,
                                       string strAmount,
                                       string strProcesingCountryCode,
                                       string strDescriptor,
                                       bool bStatusCheck,
                                       bool bTestMode,
                                       string strECI,
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strTransaction,
                                       out string strAuthCode,
                                       out string strAuthResult,
                                       out string strDateTime)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strTransaction = "";
            strAuthCode = "";
            strAuthResult = "";
            strDateTime = "";


            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.AutomaticTransaction: Starting ");

                AddTLS12Support();

                if (bTestMode)
                    bStatusCheck = false;

                string crypt_type = "7";
                if (!string.IsNullOrEmpty(strECI))
                {
                    crypt_type = strECI;
                }

                CofInfo cof = null;
                if (!string.IsNullOrEmpty(strIssuerId))
                {
                    cof = new CofInfo();
                    cof.SetIssuerId(strIssuerId);
                    cof.SetPaymentIndicator("Z");
                    cof.SetPaymentInformation("2");

                }

                ResPurchaseCC resPurchaseCC = new ResPurchaseCC();
                resPurchaseCC.SetDataKey(strCardReference);
                resPurchaseCC.SetOrderId(strOrderId);
                if (bTestMode)
                    resPurchaseCC.SetAmount("1.00");
                else
                    resPurchaseCC.SetAmount(strAmount);

                resPurchaseCC.SetCryptType(crypt_type);
                if (!string.IsNullOrEmpty(strDescriptor))
                    resPurchaseCC.SetDynamicDescriptor(strDescriptor);

                if (cof != null)
                {
                    resPurchaseCC.SetCofInfo(cof);
                }

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resPurchaseCC);
                mpgReq.SetStatusCheck(bStatusCheck);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();


                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.AutomaticPayment.Receipt: ");

                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "IssuerID = " + receipt.GetIssuerId());
                    m_Log.LogMessage(LogLevels.logINFO, "ReceiptId = " + receipt.GetReceiptId());
                    m_Log.LogMessage(LogLevels.logINFO, "ReferenceNum = " + receipt.GetReferenceNum());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "AuthCode = " + receipt.GetAuthCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "TransType = " + receipt.GetTransType());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TransAmount = " + receipt.GetTransAmount());
                    m_Log.LogMessage(LogLevels.logINFO, "CardType = " + receipt.GetCardType());
                    m_Log.LogMessage(LogLevels.logINFO, "TxnNumber = " + receipt.GetTxnNumber());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());
                    m_Log.LogMessage(LogLevels.logINFO, "IsVisaDebit = " + receipt.GetIsVisaDebit());
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResDataEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "Masked Pan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());


                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strAuthCode = receipt.GetAuthCode();
                        strAuthResult = receipt.GetReferenceNum();
                        strDateTime = receipt.GetTransDate() + " " + receipt.GetTransTime();
                        strTransaction = receipt.GetTxnNumber();
                    }



                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransaction: ", e);

            }

            return bRes;
        }


        public bool RefundTransaction(string strStoreId,
                                      string strAPIToken,
                                      string strOrderId,
                                      string strTransaction,
                                      string strAmount,
                                      string strProcesingCountryCode,
                                      bool bTestMode,
                                      out MonerisErrorCode eErrorCode,
                                      out string errorMessage,
                                      out string strRefundTransaction,
                                      out string strAuthCode,
                                      out string strAuthResult,
                                      out string strDateTime)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strRefundTransaction = "";
            strAuthCode = "";
            strAuthResult = "";
            strDateTime = "";

            try
            {

                AddTLS12Support();

                bool status_check = false;
                string crypt_type = "7";

                Refund refund = new Refund();
                refund.SetTxnNumber(strTransaction);
                refund.SetOrderId(strOrderId);

                if (bTestMode)
                    refund.SetAmount("1.00");
                else
                    refund.SetAmount(strAmount);

                refund.SetCryptType(crypt_type);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(refund);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();


                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.RefundTransaction.Receipt: ");

                    m_Log.LogMessage(LogLevels.logINFO, "CardType = " + receipt.GetCardType());
                    m_Log.LogMessage(LogLevels.logINFO, "TransAmount = " + receipt.GetTransAmount());
                    m_Log.LogMessage(LogLevels.logINFO, "TxnNumber = " + receipt.GetTxnNumber());
                    m_Log.LogMessage(LogLevels.logINFO, "ReceiptId = " + receipt.GetReceiptId());
                    m_Log.LogMessage(LogLevels.logINFO, "TransType = " + receipt.GetTransType());
                    m_Log.LogMessage(LogLevels.logINFO, "ReferenceNum = " + receipt.GetReferenceNum());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "ISO = " + receipt.GetISO());
                    m_Log.LogMessage(LogLevels.logINFO, "BankTotals = " + receipt.GetBankTotals());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "AuthCode = " + receipt.GetAuthCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "Ticket = " + receipt.GetTicket());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }


                    if (!IsError(eErrorCode))
                    {
                        strAuthCode = receipt.GetAuthCode();
                        strAuthResult = receipt.GetReferenceNum();
                        strDateTime = receipt.GetTransDate() + " " + receipt.GetTransTime();
                        strRefundTransaction = receipt.GetTxnNumber();
                    }


                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.RefundTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.RefundTransaction: ", e);

            }

            return bRes;
        }


        public bool DeleteToken(string strStoreId,
                                string strAPIToken,
                                string strCardReference,
                                string strProcesingCountryCode,
                                bool bTestMode,
                                out MonerisErrorCode eErrorCode,
                                out string errorMessage)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            try
            {

                AddTLS12Support();

                bool status_check = false;

                ResDelete resDelete = new ResDelete(strCardReference);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resDelete);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();


                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.DeleteToken.Receipt: ");

                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());

                    //ResolveData
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResMaskedPan());
                    //m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResCryptType());


                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.DeleteToken: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.DeleteToken: ", e);

            }

            return bRes;
        }

*/
        public static bool IsError(MagpieErrorCode eErrorCode)
        {
            return Convert.ToInt32(eErrorCode) >= 50;
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
            get { return true; }
        }
    }
}
