using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.SessionState;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net.Security;
using log4net;

namespace PayPal.Platform.SDK
{
    /// <summary>
    /// Calls the actual Platform API web service for the given Payload and APIProfile settings
    /// </summary>
    public class CallerServices
    {
        /// <summary>
        /// To read the certificate .
        /// </summary>
        private X509Certificate x509;
        /// <summary>
        /// Profile settings need to be passed.
        /// </summary>
        private BaseAPIProfile apiProfile;
        /// <summary>
        /// payload needs to be passed.
        /// </summary>
        private string payLoad = string.Empty;
        /// <summary>
        /// HTTP Method needs to be set.
        /// </summary>
        private const string RequestMethod = BaseConstants.REQUESTMETHOD;
        /// <summary>
        /// used to log the request and response.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(BaseConstants.PAYPALLOGFILE);

        /// <summary>
        /// Profile settings need to be passed while calling Platform API web service.
        /// </summary>
        public BaseAPIProfile APIProfile
        {
            get { return this.apiProfile; }
            set { this.apiProfile = value; }

        }
        /// <summary>
        /// HttpWebRequest
        /// </summary>
        HttpWebRequest objRequest = null;
        /// <summary>
        /// payload needs to be passed while calling Platform API web service.
        /// </summary>
        public string PayLoad
        {
            get { return this.payLoad; }
            set { this.payLoad = value; }
        }

        /// <summary>
        /// Calls the platform API web service for given payload and returns the response payload.
        /// </summary>
        /// <returns>returns the response payload</returns>
        public object CallAPI()
        {

            string url, responseString = string.Empty;
            if (this.APIProfile.IsTrustAllCertificates == true)
                //ServicePointManager.CertificatePolicy = new MyPolicy();           
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslError)
                {
                    bool validationResult = true;
                    return validationResult;
                };

            try
            {

                /// Throw exception if APIProfile or payLoad is null/empty.
                if (this.apiProfile == null)
                    throw new NullReferenceException(BaseConstants.ErrorMessages.PROFILE_NULL);
                if (this.payLoad == string.Empty)
                    throw new NullReferenceException(BaseConstants.ErrorMessages.PAYLOAD_NULL);


                /// Constructing the URL to be called from Profile settings
                url = this.apiProfile.Environment + this.apiProfile.EndPointAppend;
                /// Constructing HttpWebRequest object
                objRequest = (HttpWebRequest)WebRequest.Create(url);
                objRequest.Method = RequestMethod;
                if (this.apiProfile.Timeout < 1)
                    objRequest.Timeout = BaseConstants.DEFAULT_TIMEOUT;
                else
                    objRequest.Timeout = this.apiProfile.Timeout;

                //Set up Headers
                setupHeaders();
                /// Add the certificate to HttpWebRequest obejct if Profile is certificate enabled
                if (this.apiProfile.APIProfileType == ProfileType.Certificate)
                {
                    // Load the certificate into an X509Certificate2 object.
                    if (this.apiProfile.PrivateKeyPassword.Trim() == string.Empty)
                    {
                        x509 = new X509Certificate2(this.apiProfile.Certificate);
                    }
                    else
                    {
                        x509 = new X509Certificate2(this.apiProfile.Certificate, this.apiProfile.PrivateKeyPassword);
                    }
                    objRequest.ClientCertificates.Add(x509);
                }
                else
                {
                    objRequest.Headers.Add(BaseConstants.XPAYPALSECURITYSIGNATURE, this.apiProfile.APISignature);
                }


                if (this.apiProfile.SandboxMailAddress != null && this.apiProfile.SandboxMailAddress.Length>0)
                    objRequest.Headers.Add(BaseConstants.XPAYPALSANDBOXEMAILADDRESS, this.apiProfile.SandboxMailAddress);

                /// Adding payLoad to HttpWebRequest object
                using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
                {

                    myWriter.Write(this.payLoad);
                    if (log.IsInfoEnabled)
                    {
                        log.Info("Request Starts");
                        log.Info("####" + this.apiProfile.RequestDataformat + "####");
                        log.Info(this.payLoad);
                        log.Info("Request Ends");
                    }
                }

                /// calling the plaftform API web service and getting the resoponse
                using (WebResponse response = objRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = sr.ReadToEnd();
                        if (log.IsInfoEnabled)
                        {
                            log.Info("Response Starts");
                            log.Info(responseString);
                            log.Info("Response Ends");
                        }

                        return responseString;
                    }
                }

            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in CallerServices->CallAPI method", ex);
            }
            finally
            {
                objRequest = null;
            }


        }

        /// <summary>
        /// Setting up HttpWebRequest's Header info 
        /// </summary>
        public void setupHeaders()
        {

            // This header is used to track the calls from PayPal SDKs
            // Change the version only if you re-built the official PayPal SDK
            if (this.apiProfile.Source!=null && this.apiProfile.Source.Length>0)
            {
                string appsource = BaseConstants.XPAYPALSOURCE + this.apiProfile.Source;
                objRequest.Headers.Add(BaseConstants.XPAYPALREQUESTSOURCE, appsource);
            }
                else
            {
            objRequest.Headers.Add(BaseConstants.XPAYPALREQUESTSOURCE, BaseConstants.XPAYPALSOURCE);
            }
            //
            //
            
            // Adding Credential and payload request/resposne information to the HttpWebRequest obejct's header
            objRequest.Headers.Add(BaseConstants.XPAYPALSECURITYUSERID, this.apiProfile.APIUsername);
            objRequest.Headers.Add(BaseConstants.XPAYPALSECURITYPASSWORD, this.apiProfile.APIPassword);
            objRequest.Headers.Add(BaseConstants.XPAYPALAPPLICATIONID, this.apiProfile.ApplicationID);                       
            if (APIProfile.RequestDataformat == "SOAP11")
            {

                //objRequest.Headers.Add(BaseConstants.XPAYPALREQUESTDATAFORMAT, this.apiProfile.RequestDataformat);
                //objRequest.Headers.Add(BaseConstants.XPAYPALRESPONSEDATAFORMAT, this.apiProfile.ResponseDataformat);
                objRequest.Headers.Add(BaseConstants.XPAYPALMESSAGEPROTOCOL, this.apiProfile.RequestDataformat);
            }
            else
            {
                objRequest.Headers.Add(BaseConstants.XPAYPALREQUESTDATAFORMAT, this.apiProfile.RequestDataformat);
                objRequest.Headers.Add(BaseConstants.XPAYPALRESPONSEDATAFORMAT, this.apiProfile.ResponseDataformat);

            }
            objRequest.Headers.Add(BaseConstants.XPAYPALDEVICEIPADDRESS, this.apiProfile.DeviceIpAddress);
            

        }

    }

}
