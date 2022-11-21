/*
 * Copyright (c) 2014 Paysafe Payments
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
 * NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using CardPaymentService = Paysafe.CardPayments.CardPaymentService;
using CustomerVaultService = Paysafe.CustomerVault.CustomerVaultService;
using DirectDebitService = Paysafe.DirectDebit.DirectDebitService;
using ThreeDSecureService = Paysafe.ThreeDSecure.ThreeDSecureService;
using Paysafe.Common;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;

[assembly: CLSCompliant(true)]

namespace Paysafe
{
    public class PaysafeApiClient
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(PaysafeApiClient));

        /// <summary>
        /// The api key id
        /// </summary>
        private String apiKeyId;

        /// <summary>
        /// The api key passsword
        /// </summary>
        private String apiKeyPassword;

        /// <summary>
        /// The api environment (test/live)
        /// </summary>
        private Environment apiEnvironment;

        /// <summary>
        /// The merchant account (used only by the CardPayments api)
        /// </summary>
        private String apiAccount;

        /// <summary>
        /// The api endpoint to which all requests should be sent
        /// </summary>
        private String apiEndPoint;

        /// <summary>
        /// Initialize the api client.
        /// </summary>
        /// <param name="keyId">string</param>
        /// <param name="keyPassword">string</param>
        /// <param name="environment">Environment</param>
        /// <param name="account">string</param>
        public PaysafeApiClient(string keyId, string keyPassword, Environment environment, string account = "")
        {
            this.apiKey(keyId);
            this.apiPassword(keyPassword);
            this.environment(environment);
            this.account(account);
        }

        /// <summary>
        /// Sets the api key id
        /// </summary>
        /// <param name="newKeyId">string</param>
        protected void apiKey(string newKeyId)
        {
            if (newKeyId == null)
            {
                throw new PaysafeException("You must specify an API Key");
            }
            this.apiKeyId = newKeyId;
        }

        /// <summary>
        /// Sets the api key password
        /// </summary>
        /// <param name="newKeyPassword">string</param>
        protected void apiPassword(string newKeyPassword)
        {
            if (newKeyPassword == null)
            {
                throw new PaysafeException("You must specify an API Password");
            }
            this.apiKeyPassword = newKeyPassword;
        }

        /// <summary>
        /// Sets the environment, and api endpoint
        /// </summary>
        /// <param name="newEnvironment">Environment</param>
        protected void environment(Environment newEnvironment)
        {
            this.apiEnvironment = newEnvironment;

            if (this.apiEnvironment == Environment.TEST)
            {
                this.apiEndPoint = "https://api.test.netbanx.com";
            }
            else
            {
                this.apiEndPoint = "https://api.netbanx.com";
            }

        }

        /// <summary>
        /// Sets the merchant account number for use with the card payments api
        /// </summary>
        /// <param name="account">string</param>
        public void account(string newAccount)
        {
            this.apiAccount = newAccount;
        }

        /// <summary>
        /// Get the merchant account number
        /// </summary>
        /// <returns>string</returns>
        public String account()
        {
            return this.apiAccount;
        }

        /// <summary>
        /// Get an instance of the card payment service
        /// </summary>
        /// <returns>CardPaymentService</returns>
        public CardPaymentService cardPaymentService()
        {
            return new CardPaymentService(this);
        }

        /// <summary>
        /// Get an instance of the customer vault service
        /// </summary>
        /// <returns>CustomerVaultService</returns>
        public CustomerVaultService customerVaultService() {
            return new Paysafe.CustomerVault.CustomerVaultService(this);
        }

	    

        /// <summary>
        /// Get an instance of the Direct debit service
        /// </summary>
        /// <returns>DirectDebitService</returns>
        public DirectDebitService directDebitService()
        
        {
            return new DirectDebitService(this);
        }

        /// <summary>
        /// Get an instance of the ThreeDSecure service
        /// </summary>
        /// <returns>ThreeDSecureService</returns>
        public ThreeDSecureService threeDSecureService()
        {
            return new ThreeDSecureService(this);
        }

        /// <summary>
        /// Returns the base64 encoded authentication string for the http request headers
        /// </summary>
        /// <returns>string</returns>
        private string getAuthString()
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.apiKeyId + ":" + this.apiKeyPassword));
        }

        /// <summary>
        /// This method will perform a the http request synchronously, and return the json decoded result
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Dictionary<string,object></returns>
        public Dictionary<string,object> processRequest(Request request)
        {
            AddTLS11Support();

            m_Log.LogMessage(LogLevels.logINFO, string.Format("Request method={0};url={1};body:{2}", request.method(), request.buildUrl(this.apiEndPoint), PrettyJSON(request.body())));

            HttpWebRequest conn = (HttpWebRequest)WebRequest.CreateHttp(request.buildUrl(this.apiEndPoint));
            conn.Headers["Authorization"] = "Basic " + this.getAuthString();
            conn.ContentType = "application/json; charset=utf-8";
	    conn.Headers["SDK-Type"] = "Paysafe_CSharp_SDK";

            conn.Method = request.method();
            if (request.method().Equals(RequestType.POST.ToString())
                || request.method().Equals(RequestType.PUT.ToString()))
            {
                string requestBody = request.body();
                byte[] requestData = Encoding.UTF8.GetBytes(requestBody);

                var resultRequest = conn.BeginGetRequestStream(null, null);
                Stream postStream = conn.EndGetRequestStream(resultRequest);
                postStream.Write(requestData, 0, requestData.Length);
                postStream.Dispose();
            }

            try
            {
                var responseRequest = conn.BeginGetResponse(null, null);
                WebResponse responseObject = conn.EndGetResponse(responseRequest);

                StreamReader sr = new StreamReader(responseObject.GetResponseStream());
                string sResponse = sr.ReadToEnd();

                m_Log.LogMessage(LogLevels.logINFO, string.Format("Response success {0}", PrettyJSON(sResponse)));

                return PaysafeApiClient.parseResponse(sResponse);
            }
            catch (System.Net.WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string body = sr.ReadToEnd();

                m_Log.LogMessage(LogLevels.logINFO, string.Format("Response error {0}", PrettyJSON(body)));

                string exceptionType = null;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest: // 400
                        exceptionType = "InvalidRequestException";
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        exceptionType = "InvalidCredentialsException";
                        break;
                    case HttpStatusCode.PaymentRequired: //402
                        exceptionType = "RequestDeclinedException";
                        break;
                    case HttpStatusCode.Forbidden: //403
                        exceptionType = "PermissionException";
                        break;
                    case HttpStatusCode.NotFound: //404
                        exceptionType = "EntityNotFoundException";
                        break;
                    case HttpStatusCode.Conflict: //409
                        exceptionType = "RequestConflictException";
                        break;
                    case HttpStatusCode.NotAcceptable: //406
                    case HttpStatusCode.UnsupportedMediaType: //415
                    case HttpStatusCode.InternalServerError: //500
                    case HttpStatusCode.NotImplemented: //501
                    case HttpStatusCode.BadGateway: //502
                    case HttpStatusCode.ServiceUnavailable: //503
                    case HttpStatusCode.GatewayTimeout: //504
                    case HttpStatusCode.HttpVersionNotSupported: //505
                        exceptionType = "APIException";
                        break;
                }
                if (exceptionType != null)
                {
                    String message = ex.Message;
                    Dictionary<string, dynamic> rawResponse = PaysafeApiClient.parseResponse(body);
                    if (rawResponse.ContainsKey("error"))
                    {
                        message = rawResponse["error"]["message"];
                    }

                    Object[] args = { message, ex.InnerException };
                    PaysafeException PaysafeException = Activator.CreateInstance
                        (Type.GetType("Paysafe.Common." + exceptionType), args) as PaysafeException;
                    PaysafeException.rawResponse(rawResponse);
                    if (rawResponse.ContainsKey("error"))
                    {
                        PaysafeException.code(int.Parse(rawResponse["error"]["code"]));
                    }
                    throw PaysafeException;
                }
                throw;
            }
            throw new PaysafeException("An unknown error has occurred.");
        }

        public static Dictionary<string, object> parseResponse(string response)
        {
            if (String.IsNullOrWhiteSpace(response))
            {
                return null;
            }
            return JsonHelper.Deserialize(response) as Dictionary<string, object>;
        }

        private static void AddTLS11Support()
        {
            /*if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls11) == 0) //Enable TLs 1.1
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls) == 0) //Enable TLs 1.0
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }*/

            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }

        private string PrettyJSON(string json)
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
    }
}
