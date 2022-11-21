using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;


namespace integraMobile.Infrastructure.ZeroBounce
{


    public class ZeroBounceResultsModel
    {
        public string address { get; set; } //email address being validated
        public string status { get; set; }//: "[Valid |Invalid |Catch-All |Unknown |Spamtrap |Abuse |DoNotMail]"
        public string subStatus { get; set; } //: "[antispam_system |greylisted |mail_server_temporary_error |forcible_disconnect |mail_server_did_not_respond |timeout_exceeded |failed_smtp_connection |mailbox_quota_exceeded |exception_occurred |possible_traps |role_based |global_suppression |mailbox_not_found |no_dns_entries |failed_syntax_check |possible_typo |unroutable_ip_address |leading_period_removed |does_not_accept_mail ]"
        public string account { get; set; }//:  "portion before the @ symbol" 
        public string domain { get; set; }//: "portion after the @ symbol"
        public string disposable { get; set; }//: true/false
        public string toxic { get; set; }//: true/false
        public string firstName { get; set; } //: email owner's first name if available
        public string lastName { get; set; } //: email owner's last name if available
        public string gender { get; set; }//: email owner's gender if available
        public string location { get; set; }//: email owner's location if available
        public string country { get; set; }//: the country of the IP passed in
        public string region { get; set; }//: the region of the IP passed in
        public string city { get; set; }//: the city of the IP passed in
        public string zipcode { get; set; }//: the zipcode of the IP passed in
        public string creationDate { get; set; }//: creation date of email if available
        public string processedAt { get; set; }//: "UTC time email was validated"
        public string errMsg { get; set; }
    }


    public class ZeroBounceCreditsModel
    {
        public string credits { get; set; }
        public string errMsg { get; set; }
    }

    public class ZeroBounceAPI
    {
        // "Your API Key";
        private string m_apiKey = "";
        public string ApiKey
        {
            get
            {
                return m_apiKey;
            }

            set
            {
                m_apiKey = value;
            }
        }
        // "Your IP address";
        private string m_IPAddress = "";
        public string IpAddress
        {
            get
            {
                return m_IPAddress;
            }

            set
            {
                m_IPAddress = value;
            }
        }
        private string m_emailToValidate = "";
        public string EmailToValidate
        {
            get
            {
                return m_emailToValidate;
            }

            set
            {
                m_emailToValidate = value;
            }
        }

        private int m_requestTimeOut = 0;
        public int RequestTimeOut
        {
            get
            {
                return m_requestTimeOut;
            }

            set
            {
                m_requestTimeOut = value;
            }
        }
        private int m_readTimeOut = 0;
        public int ReadTimeOut
        {
            get
            {
                return m_readTimeOut;
            }

            set
            {
                m_readTimeOut = value;
            }
        }

        public ZeroBounceResultsModel ValidateEmail()
        {
            // secure SSL / TLS channel for different .Net versions
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var apiUrl = "";

            if (IpAddress == "") apiUrl = "https://api.zerobounce.net/v1/validate?apikey=" + ApiKey + "&email=" + System.Net.WebUtility.UrlEncode(EmailToValidate);
            else apiUrl = "https://api.zerobounce.net/v1/validatewithip?apikey=" + ApiKey + "&email=" + System.Net.WebUtility.UrlEncode(EmailToValidate) + "&ipaddress=" + System.Net.WebUtility.UrlEncode(IpAddress);

            var responseString = "";
            var oResults = new ZeroBounceResultsModel();
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(apiUrl);
            request.Timeout = RequestTimeOut;
            request.Method = "GET";
            Console.WriteLine("Input APIKey: " + ApiKey);

            var serializer = new JavaScriptSerializer();
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    response.GetResponseStream().ReadTimeout = ReadTimeOut;
                    using (StreamReader ostream = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = ostream.ReadToEnd();
                        oResults = serializer.Deserialize<ZeroBounceResultsModel>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The operation has timed out")) oResults.subStatus = "timeout_exceeded";
                else oResults.subStatus = "exception_occurred";
                oResults.status = "Unknown";
                oResults.domain = EmailToValidate.Substring(EmailToValidate.IndexOf("@") + 1).ToLower();
                oResults.account = EmailToValidate.Substring(0, EmailToValidate.IndexOf("@")).ToLower();
                oResults.address = EmailToValidate.ToLower();

                Console.WriteLine(ex.ToString());
                oResults.errMsg = ex.ToString();
            }
            return oResults;
        }
        public ZeroBounceCreditsModel GetCredits()
        {
            // secure SSL / TLS channel for different .Net versions            
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var apiUrl = "https://api.zerobounce.net/v1/getcredits?apikey=" + ApiKey;
            var responseString = "";
            var oResults = new ZeroBounceCreditsModel();

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(apiUrl);
            request.Timeout = RequestTimeOut;
            request.Method = "GET";
            Console.WriteLine("APIKey: " + ApiKey);

            var serializer = new JavaScriptSerializer();
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    response.GetResponseStream().ReadTimeout = ReadTimeOut;
                    using (StreamReader ostream = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = ostream.ReadToEnd();
                        oResults = serializer.Deserialize<ZeroBounceCreditsModel>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                oResults.errMsg = ex.ToString();
            }
            return oResults;
        }
    }
    
}
