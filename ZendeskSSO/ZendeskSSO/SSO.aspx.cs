using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using integraMobile.Infrastructure.Logging.Tools;
using System.Web.Script.Serialization;
using Ninject;
using Ninject.Web;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;


namespace ZendeskSSO
{
    public partial class SSO : System.Web.UI.Page
    {      
        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SSO));

        [Inject]
        public ICustomersRepository customersRepository { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
                  
            string strToken = Request.Params["user_token"];
            string returnTo = Request.Params["return_to"];

            Logger_AddLogMessage(string.Format("Zendesk SSO Request Params => user_token = {0} ; return_to= {1}", strToken, returnTo), LogLevels.logINFO);
           
            Dictionary<string, object> payload=null;
            if ((!string.IsNullOrEmpty(strToken)) && (customersRepository.GetUserFromOpenSession(strToken, ref payload)))
            {
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1)); 
                int timestamp = (int)t.TotalSeconds;
                payload.Add("iat", timestamp);
                payload.Add("jti", System.Guid.NewGuid().ToString());
            }
            else
            {
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1)); 
                int timestamp = (int)t.TotalSeconds;
                payload = new Dictionary<string, object>() {
                    { "iat", timestamp },
                    { "jti", System.Guid.NewGuid().ToString()},
                };
            
            }
            string json = new JavaScriptSerializer().Serialize(payload);

            Logger_AddLogMessage(string.Format("JWT Payload => {0}", json), LogLevels.logINFO);

            string token = JWT.JsonWebToken.Encode(payload, ConfigurationManager.AppSettings["SharedAPIKey"], JWT.JwtHashAlgorithm.HS256);
            string redirectUrl = "https://" + ConfigurationManager.AppSettings["Subdomain"] + ".zendesk.com/access/jwt?jwt=" + token;
             
            if (returnTo != null)
            {
                redirectUrl += "&return_to=" + HttpUtility.UrlEncode(returnTo);
            }

            Logger_AddLogMessage(string.Format("Redirecting to => {0}", redirectUrl), LogLevels.logINFO);

            Response.Redirect(redirectUrl);

        }
        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }
    }
}