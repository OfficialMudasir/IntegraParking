using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using System.Xml;
using PayPal.Services.Private.AP;
using PayPal.Platform.SDK;
namespace ASPNET_SDK_Samples.Samples
{   
    /// <summary>
    /// Summary description for CancelPreapprovalDetails.
    /// </summary>
    public partial class CancelPreapproval : System.Web.UI.Page
    {
        #region Private Members

        protected System.Web.UI.HtmlControls.HtmlInputText preapprovalkey;
        protected System.Web.UI.HtmlControls.HtmlInputButton Submit;

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
            this.Submit.ServerClick += new System.EventHandler(this.Submit_ServerClick);
            this.Load += new System.EventHandler(this.Page_Load);

        }
     #endregion

        #region Events

        /// <summary>
        /// Handles Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
        }

        /// <summary>
        /// Handles On click event of Submit button
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        private void Submit_ServerClick(object sender, System.EventArgs e)
        {
            CancelPreapprovalRequest CPRequest = null;
            BaseAPIProfile profile2 = null;

            try
            {
                
                    profile2 =(BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
                    CPRequest = new CancelPreapprovalRequest();
                    CPRequest.preapprovalKey = preapprovalkey.Value;
                    CPRequest.requestEnvelope = new RequestEnvelope();
                    CPRequest.requestEnvelope.errorLanguage = "en-US";
                    PayPal.Platform.SDK.AdapativePayments ap = new PayPal.Platform.SDK.AdapativePayments();
                    ap.APIProfile = profile2;
                    CancelPreapprovalResponse CPResponse = ap.CancelPreapproval(CPRequest);

                if (ap.isSuccess.ToUpper() == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                else
                {
                    Session[Constants.SessionConstants.CANCELPREAPPROVALRESPONSE] = CPResponse;
                    this.Response.Redirect("CancelPreapprovalReceipt.aspx", false);
                }
            }

            catch (FATALException FATALEx)
            {
                    Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                    this.Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL");
            }
            catch (Exception ex)
            {
                    FATALException FATALEx = new FATALException("Error occurred in GetPaymentDetails Page.", ex);
                    Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                    this.Response.Redirect("APIError.aspx?type=FATAL", false);

            }
        }

        #endregion
    }
}
