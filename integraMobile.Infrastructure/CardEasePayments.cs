using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using CardEaseXML;
using System.Configuration;
using System.Globalization;
using System.Diagnostics;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;


namespace integraMobile.Infrastructure
{
    public class CardEasePayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CardEasePayments));
        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));
        protected const int DEFAULT_WS_TIMEOUT = 5000; //ms   
        
        public bool AutomaticPayment(string strCreditCallTerminalId,
                                    string strCreditCallTransactionKey,
                                    string strCardEaseURL,
                                    int iCardEaseTimeout,
                                    string strEmail,
                                    decimal dQuantity,
                                    string strCurISOCode,
                                    string strCardHash,
                                    string strCardReference,
                                    bool bAutoConfirm,
                                    out string strUserReference,
                                    out string strAuthCode,
                                    out string strAuthResult,
                                    out string strGatewayDate,
                                    out string strCardScheme,
                                    out string strTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthCode = null;
            strAuthResult=null;
            strGatewayDate=null;
            strCardScheme = null;
            strTransactionId=null;
            
            AddTLS12Support();


            // Setup the request
            Request request = new Request();

            request.SoftwareName = "SoftwareName";
            request.SoftwareVersion = "SoftwareVersion";
            request.TerminalID =  strCreditCallTerminalId;
            request.TransactionKey = strCreditCallTransactionKey;
            request.UserReference = UserReference();

            // Setup the request detail
            request.RequestType = RequestType.Auth;
            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
            numberFormatProvider.NumberDecimalSeparator = ".";

            request.Amount = dQuantity.ToString(numberFormatProvider);
            request.CurrencyCode = strCurISOCode;
            request.CardHash = strCardHash;
            request.CardReference = strCardReference;
            request.AutoConfirm = bAutoConfirm;
            request.AmountUnit = AmountUnit.Major;
            request.CardHolderEmailAddresses.Add(new EmailAddress(strEmail,EmailAddressType.Unknown));


            // Setup the client
            Client client = new Client();
            client.AddServerURL(strCardEaseURL,
                                Convert.ToInt32(iCardEaseTimeout));
            client.Request = request;
            
            try
            {
                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.AutomaticPayment.Request: " + System.Environment.NewLine + request.ToString());
                
                // Process the request
                client.ProcessRequest();
                Response response = client.Response;

                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.AutomaticPayment.Response: " + System.Environment.NewLine + response.ToString());

                
                bRes = (response.ResultCode == ResultCode.Approved);

                if (bRes)
                {
                    strUserReference = response.UserReference;
                    strAuthResult = response.ResultCode.ToString();
                    strAuthCode = response.AuthCode;
                    strGatewayDate=response.LocalDateTime;
                    strCardScheme = response.CardScheme;
                    strTransactionId=response.CardEaseReference;
                }

            }
            catch (CardEaseXMLCommunicationException e)
            {
                // There is something wrong with communication
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.AutomaticPayment: ", e);
                //Console.WriteLine(e.Message + System.Environment.NewLine + e.StackTrace);
            }
            catch (CardEaseXMLRequestException e)
            {
                // There is something wrong with the request
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.AutomaticPayment: ", e);
            }
            catch (CardEaseXMLResponseException e)
            {
                // There is something wrong with the response            
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.AutomaticPayment: ", e);
            }

            // Get the response


            return bRes;
        }


        public bool ConfirmUnCommitedPayment(string strCreditCallTerminalId,
                                    string strCreditCallTransactionKey,
                                    string strCardEaseURL,
                                    int iCardEaseTimeout, 
                                    string strTransactionId,
                                    out string strUserReference,
                                    out string strAuthResult,
                                    out string strGatewayDate,
                                    out string strCommitTransactionId)
        {
            bool bRes = false;

            strUserReference=null;
            strAuthResult=null;
            strGatewayDate=null;
            strCommitTransactionId = null;

            AddTLS12Support();

            // Setup the request
            Request request = new Request();

            request.SoftwareName = "SoftwareName";
            request.SoftwareVersion = "SoftwareVersion";
            request.TerminalID = strCreditCallTerminalId;
            request.TransactionKey = strCreditCallTransactionKey;
            request.UserReference = UserReference();

            // Setup the request detail
            request.RequestType = RequestType.Conf;
            request.CardEaseReference = strTransactionId;


            // Setup the client
            Client client = new Client();
            client.AddServerURL(strCardEaseURL,
                                Convert.ToInt32(iCardEaseTimeout));
            client.Request = request;

            try
            {
                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.ConfirmUnCommitedPayment.Request: " + System.Environment.NewLine + request.ToString());

                // Process the request
                client.ProcessRequest();
                Response response = client.Response;

                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.ConfirmUnCommitedPayment.Response: " + System.Environment.NewLine + response.ToString());


                bRes = (response.ResultCode == ResultCode.Approved);

                if (bRes)
                {
                    strUserReference = response.UserReference;
                    strAuthResult = response.ResultCode.ToString();
                    strGatewayDate = response.LocalDateTime;
                    strCommitTransactionId = response.CardEaseReference;
                }


            }
            catch (CardEaseXMLCommunicationException e)
            {
                // There is something wrong with communication
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.ConfirmUnCommitedPayment: ", e);
                //Console.WriteLine(e.Message + System.Environment.NewLine + e.StackTrace);
            }
            catch (CardEaseXMLRequestException e)
            {
                // There is something wrong with the request
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.ConfirmUnCommitedPayment: ", e);
            }
            catch (CardEaseXMLResponseException e)
            {
                // There is something wrong with the response            
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.ConfirmUnCommitedPayment: ", e);
            }

            // Get the response


            return bRes;
        }


        public bool VoidUnCommitedPayment(string strCreditCallTerminalId,
                                    string strCreditCallTransactionKey,
                                    string strCardEaseURL,
                                    int iCardEaseTimeout, 
                                    string strTransactionId,
                                    out string strUserReference,
                                    out string strAuthResult,
                                    out string strGatewayDate,
                                    out string strVoidTransactionId)
        {
            bool bRes = false;

            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strVoidTransactionId = null;

            AddTLS12Support();

            // Setup the request
            Request request = new Request();

            request.SoftwareName = "SoftwareName";
            request.SoftwareVersion = "SoftwareVersion";
            request.TerminalID = strCreditCallTerminalId;
            request.TransactionKey = strCreditCallTransactionKey;
            request.UserReference = UserReference();

            // Setup the request detail
            request.RequestType = RequestType.Void;
            request.CardEaseReference = strTransactionId;
            request.VoidReason = VoidReason.VendFailure;


            // Setup the client
            Client client = new Client();
            client.AddServerURL(strCardEaseURL,
                                Convert.ToInt32(iCardEaseTimeout));
            client.Request = request;

            try
            {
                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.RefundUnCommitedPayment.Request: " + System.Environment.NewLine + request.ToString());

                // Process the request
                client.ProcessRequest();
                Response response = client.Response;

                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.RefundUnCommitedPayment.Response: " + System.Environment.NewLine + response.ToString());


                bRes = (response.ResultCode == ResultCode.Approved);

                if (bRes)
                {
                    strUserReference = response.UserReference;
                    strAuthResult = response.ResultCode.ToString();
                    strGatewayDate = response.LocalDateTime;
                    strVoidTransactionId = response.CardEaseReference;
                }

            }
            catch (CardEaseXMLCommunicationException e)
            {
                // There is something wrong with communication
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundUnCommitedPayment: ", e);
                //Console.WriteLine(e.Message + System.Environment.NewLine + e.StackTrace);
            }
            catch (CardEaseXMLRequestException e)
            {
                // There is something wrong with the request
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundUnCommitedPayment: ", e);
            }
            catch (CardEaseXMLResponseException e)
            {
                // There is something wrong with the response            
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundUnCommitedPayment: ", e);
            }

            // Get the response


            return bRes;
        }



        public bool RefundCommitedPayment(string strCreditCallTerminalId,
                                    string strCreditCallTransactionKey,
                                    string strCardEaseURL,
                                    int iCardEaseTimeout, 
                                    string strTransactionId,
                                    out string strUserReference,
                                    out string strAuthResult,
                                    out string strGatewayDate,
                                    out string strRefundTransactionId)
        {
            bool bRes = false;

            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strRefundTransactionId = null;

            AddTLS12Support();

            // Setup the request
            Request request = new Request();

            request.SoftwareName = "SoftwareName";
            request.SoftwareVersion = "SoftwareVersion";
            request.TerminalID = strCreditCallTerminalId;
            request.TransactionKey = strCreditCallTransactionKey;
            request.UserReference = UserReference();

            // Setup the request detail
            request.RequestType = RequestType.Refund;
            request.CardEaseReference = strTransactionId;


            // Setup the client
            Client client = new Client();
            client.AddServerURL(strCardEaseURL,
                                Convert.ToInt32(iCardEaseTimeout));
            client.Request = request;

            try
            {
                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.RefundCommitedPayment.Request: " + System.Environment.NewLine + request.ToString());

                // Process the request
                client.ProcessRequest();
                Response response = client.Response;

                m_Log.LogMessage(LogLevels.logDEBUG, "CardEasePayments.RefundCommitedPayment.Response: " + System.Environment.NewLine + response.ToString());


                bRes = (response.ResultCode == ResultCode.Approved);

                if (bRes)
                {
                    strUserReference = response.UserReference;
                    strAuthResult = response.ResultCode.ToString();
                    strGatewayDate = response.LocalDateTime;
                    strRefundTransactionId = response.CardEaseReference;
                }           


            }
            catch (CardEaseXMLCommunicationException e)
            {
                // There is something wrong with communication
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundCommitedPayment: ", e);
                //Console.WriteLine(e.Message + System.Environment.NewLine + e.StackTrace);
            }
            catch (CardEaseXMLRequestException e)
            {
                // There is something wrong with the request
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundCommitedPayment: ", e);
            }
            catch (CardEaseXMLResponseException e)
            {
                // There is something wrong with the response            
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.RefundCommitedPayment: ", e);
            }

            // Get the response


            return bRes;
        }



        static public bool GetMobileTransactionInfo( string strTransactionId,
                                                        string strURLBase,
                                                        string strSellerID,
                                                        string strTransactionKey,  
                                                        int iTimeout,  
                                                        out string strReference ,
                                                        out string strAuthCode,
                                                        out string strAuthResult,
                                                        out string strCardHash,
                                                        out string strCardReference,
                                                        out string strCardScheme,
                                                        out string strGatewayDate,
                                                        out string strMaskedCardNumber,
                                                        out string strExpMonth,
                                                        out string strExpYear,
                                                        out int iRetries)

        {

            bool bRes = false;
            strReference = "";
            strAuthCode = "";
            strAuthResult = "";
            strCardHash = "";
            strCardReference = "";
            strCardScheme = "";
            strGatewayDate = "";
            strMaskedCardNumber = "";
            strExpMonth = "";
            strExpYear = "";
            iRetries = 0;
            Stopwatch watch = null;


            try
            {
                AddTLS12Support();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = string.Format("{0}/transaction.php?ekashu_transaction_id={1}&ekashu_seller_id={2}&ekashu_seller_key={3}", 
                                            strURLBase,
                                            HttpUtility.UrlEncode(strTransactionId),
                                            HttpUtility.UrlEncode(strSellerID),
                                            HttpUtility.UrlEncode(strTransactionKey.Substring(0, 8)));
                //m_Log.LogMessage(LogLevels.logINFO, "CardEasePayments.GetMobileTransactionInfo.URL: " + strURL);

                //string strURL = @"https://www.iparkme.com/test_trans.txt";

                             

                try
                {
                    string responseFromServer = "";
                    watch = Stopwatch.StartNew();
                    do
                    {

                        try
                        {

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
                            request.Method = WebRequestMethods.Http.Get;
                            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
                            //request.ContentType = "application/json";
                            //request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                            //request.Credentials = CredentialCache.DefaultCredentials;
                            //request.Referer = strURL;
                            request.Timeout = 5000;

                            responseFromServer = "";
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

                                responseFromServer = reader.ReadToEnd();

                                reader.Close();
                                dataStream.Close();

                                if (!string.IsNullOrEmpty(responseFromServer))
                                {
                                    m_Log.LogMessage(LogLevels.logINFO, "CardEasePayments.GetMobileTransactionInfo.Response:\n" + responseFromServer);

                                    string[] lines = responseFromServer.Split('\n');
                                    var dict = lines.Select(l => l.Split(':')).ToDictionary(a => (a.Length > 0 ? a[0] : null), a => (a.Length > 1 ? a[1] : null));

                                    strReference = dict["ekashu_reference"];
                                    strAuthCode = dict["ekashu_auth_code"];
                                    strAuthResult = dict["ekashu_auth_result"];
                                    strCardHash = dict["ekashu_card_hash"];
                                    strCardReference = dict["ekashu_card_reference"];
                                    strCardScheme = dict["ekashu_card_scheme"];
                                    strGatewayDate = dict["ekashu_date_time_local_fmt"];
                                    strMaskedCardNumber = dict["ekashu_masked_card_number"];
                                    strExpMonth = dict["ekashu_expires_end_month"];
                                    strExpYear = dict["ekashu_expires_end_year"];

                                    bRes = true;
                                }
                                else
                                {
                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("CardEasePayments.GetMobileTransactionInfo.Response Empty. URL={0} | Retry={1}", strURL, iRetries));
                                }

                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logINFO, "CardEasePayments.GetMobileTransactionInfo.Status: " + oWebResponse.StatusDescription);
                            }
                            response.Close();
                           
                        }
                        catch (WebException e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.GetMobileTransactionInfo: ", e);
                            bRes = false;
                        }

                       
                        iRetries++;

                    }
                    while (string.IsNullOrEmpty(responseFromServer) && watch.ElapsedMilliseconds < iTimeout);

                    if (iRetries > 1)
                    {
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CardEasePayments.GetMobileTransactionInfo. bRes={0} | Retries={1} | URL={2}", bRes, iRetries, strURL));
                    }

                    watch.Stop();
                }
                catch (WebException e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.GetMobileTransactionInfo: ", e);
                    bRes = false;
                }

            }
            catch (Exception e)
            {
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "CardEasePayments.GetMobileTransactionInfo: ", e);

            }

            return bRes;


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


        public static void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12)==0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }


        public static string UserReference()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}{1:000}", DateTime.Now.ToUniversalTime(), m_oRandom.Next(0, 999));
        }

        public static string HashCode(string strCreditCallTerminalId, string strCreditCallHashKey,  string userReference, string strAmount)
        {

            if (!string.IsNullOrEmpty(strCreditCallHashKey))
            {

                string terminalId = strCreditCallTerminalId;
                string hashKey = strCreditCallHashKey;
                string reference = userReference;
                string amount = strAmount;
            


                return Convert.ToBase64String(
                        new SHA1CryptoServiceProvider().ComputeHash(
                                            Encoding.UTF8.GetBytes(
                                            string.Concat(hashKey, terminalId, reference, amount))));
            }
            
            return "";
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
}
