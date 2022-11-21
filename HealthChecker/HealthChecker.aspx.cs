using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;

namespace HealthChecker
{
    public partial class HealthChecker : System.Web.UI.Page
    {
        const System.String ct_WSUSERNAMEHEALTHCHECK_TAG = "WSUsernameHealthCheck";
        const System.String ct_WSPASSWORDHEALTHCHECK_TAG = "WSPasswordHealthCheck";
        const System.String ct_WSTIMEOUTHEALTHCHECK_TAG = "WSTimeoutHealthCheck";
        const System.String ct_WSDISCSTRING_TAG = "WSDiscString";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);



            Response.ContentType = "text/html";

            try
            {
                string strWSUsername = ConfigurationManager.AppSettings[ct_WSUSERNAMEHEALTHCHECK_TAG].ToString();
                string strWSPassword = ConfigurationManager.AppSettings[ct_WSPASSWORDHEALTHCHECK_TAG].ToString();
                int iWSTimeout = Convert.ToInt32(ConfigurationManager.AppSettings[ct_WSTIMEOUTHEALTHCHECK_TAG].ToString());
                string strDiscString = ConfigurationManager.AppSettings[ct_WSDISCSTRING_TAG].ToString();


                integraMobileWS.integraMobileWS oWS = new integraMobileWS.integraMobileWS();
                if (!string.IsNullOrEmpty(strWSUsername))
                {
                    oWS.Credentials = new System.Net.NetworkCredential(strWSUsername, strWSPassword);
                }
                oWS.Timeout = iWSTimeout;

                int iWSRes = -1;

                try
                {
                    iWSRes = oWS.HealthCheckDisc(strDiscString);
                }
                catch (Exception exc)
                {
                    iWSRes = -1;
                }

                Response.Write(iWSRes.ToString());
                             
            }
            catch
            {
                Response.Write("-1");
            }

        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification,
                                                   System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                   System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }


}