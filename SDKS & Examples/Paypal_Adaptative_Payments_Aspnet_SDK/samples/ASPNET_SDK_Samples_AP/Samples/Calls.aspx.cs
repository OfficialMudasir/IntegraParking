using System;
using System.Collections;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.HtmlControls;
using PayPal.Platform.SDK;


namespace ASPNET_SDK_Samples.Samples
{
    /// <summary>
    /// Summary description for Calls.
    /// </summary>
    public class Calls : System.Web.UI.Page
    {
        #region Events

        /// <summary>
        /// Handles PageLoad Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here

            Session["profile"] = CreateProfile();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Reads the credential settings from Web.config and creates BaseAPIProfile object respectively.
        /// </summary>
        /// <returns>BaseAPIProfile</returns>
        private BaseAPIProfile CreateProfile()
        {
            Session["PROFILE"] = null;
            BaseAPIProfile profile = null;
            byte[] bCert = null;
            string filePath = string.Empty;
            FileStream fs = null;

            try
            {

                if (ConfigurationManager.AppSettings["API_AUTHENTICATION_MODE"] == "3TOKEN")
                {
                    ////Three token 
                    profile = new BaseAPIProfile();
                    profile.APIProfileType = ProfileType.ThreeToken;
                    profile.ApplicationID = ConfigurationManager.AppSettings["APPLICATION-ID"];
                    profile.APIUsername = ConfigurationManager.AppSettings["API_USERNAME"];
                    profile.APIPassword = ConfigurationManager.AppSettings["API_PASSWORD"];
                    profile.APISignature = ConfigurationManager.AppSettings["API_SIGNATURE"];
                    profile.Environment = ConfigurationManager.AppSettings["ENDPOINT"];
                    profile.RequestDataformat = ConfigurationManager.AppSettings["API_REQUESTFORMAT"];
                    profile.ResponseDataformat = ConfigurationManager.AppSettings["API_RESPONSEFORMAT"];

                    profile.IsTrustAllCertificates = Convert.ToBoolean(ConfigurationManager.AppSettings["TrustAll"]);
                }
                else
                {
                    ////Certificate
                    profile = new BaseAPIProfile();
                    profile.APIProfileType = ProfileType.Certificate;
                    profile.APIUsername = ConfigurationManager.AppSettings["API_USERNAME"];
                    profile.APIPassword = ConfigurationManager.AppSettings["API_PASSWORD"];
                    profile.ApplicationID = ConfigurationManager.AppSettings["APPLICATION-ID"];
                    profile.RequestDataformat = ConfigurationManager.AppSettings["API_REQUESTFORMAT"];
                    profile.ResponseDataformat = ConfigurationManager.AppSettings["API_RESPONSEFORMAT"];

                    profile.IsTrustAllCertificates = Convert.ToBoolean(ConfigurationManager.AppSettings["TrustAll"]);
                    ///loading the certificate file into profile.
                    filePath = Server.MapPath(ConfigurationManager.AppSettings["CERTIFICATE"].ToString());
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    bCert = new byte[fs.Length];
                    fs.Read(bCert, 0, int.Parse(fs.Length.ToString()));
                    fs.Close();

                    profile.Certificate = bCert;
                    profile.PrivateKeyPassword = ConfigurationManager.AppSettings["PRIVATE_KEY_PASSWORD"];
                    profile.APISignature = "";
                    profile.Environment = ConfigurationManager.AppSettings["ENDPOINT"];
                }

            }
            catch (FATALException FATALEx)
            {
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL");
            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in Default Page.", ex);
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                Response.Redirect("APIError.aspx?type=FATAL");
            }

            return profile;
        }

        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}
