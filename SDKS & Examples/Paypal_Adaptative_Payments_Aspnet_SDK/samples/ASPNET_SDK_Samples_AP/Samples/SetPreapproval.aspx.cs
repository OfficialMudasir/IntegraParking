using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PayPal.Platform.SDK;
using PayPal.Services.Private.AP;

namespace ASPNET_SDK_Samples.Samples
{
	/// <summary>
	/// Summary description for SetPreapproval.
	/// </summary>
	public class SetPreapproval : System.Web.UI.Page
	{
	
		#region Private Members

		protected System.Web.UI.HtmlControls.HtmlInputText senderEmail;
		protected System.Web.UI.HtmlControls.HtmlInputText startingDate;
		protected System.Web.UI.HtmlControls.HtmlInputText endingDate;
		protected System.Web.UI.HtmlControls.HtmlInputText maxNumberOfPayments;
		protected System.Web.UI.HtmlControls.HtmlInputText maxTotalAmountOfAllPayments;
		protected System.Web.UI.HtmlControls.HtmlSelect currencyCode;
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit;
		
		#endregion

        #region Events

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                startingDate.Value = DateTime.Now.ToString("MM-dd-yyyy");
                endingDate.Value = DateTime.Now.AddMonths(12).ToString("MM-dd-yyyy");
            }
        }

        /// <summary>
        /// Handles onlick event of Submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_ServerClick(object sender, System.EventArgs e)
        {
         
            PreapprovalRequest preapprovalRequest = null;
            BaseAPIProfile profile2 = null;
            
            try
            {
                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
                string url = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
                string returnURL = url + ResolveUrl("PreapprovalDetails.aspx");
                string cancelURL = url + ResolveUrl("Preapproval.aspx");
                preapprovalRequest = new PreapprovalRequest();
                preapprovalRequest.cancelUrl = cancelURL;
                preapprovalRequest.returnUrl = returnURL;
                preapprovalRequest.senderEmail = senderEmail.Value;
                preapprovalRequest.requestEnvelope = new RequestEnvelope();
                preapprovalRequest.requestEnvelope.errorLanguage = "en-US";
                preapprovalRequest.maxNumberOfPayments = int.Parse(maxNumberOfPayments.Value);
                preapprovalRequest.maxTotalAmountOfAllPayments = decimal.Parse(maxTotalAmountOfAllPayments.Value);
                preapprovalRequest.maxTotalAmountOfAllPaymentsSpecified = true;
                preapprovalRequest.currencyCode = currencyCode.Value;
                preapprovalRequest.startingDate = DateTime.Parse(startingDate.Value);
                preapprovalRequest.endingDate = DateTime.Parse(endingDate.Value);
                preapprovalRequest.endingDateSpecified = true;
                preapprovalRequest.clientDetails = new ClientDetailsType();
                preapprovalRequest.clientDetails = ClientInfoUtil.getMyAppDetails();

                PayPal.Platform.SDK.AdapativePayments ap = new PayPal.Platform.SDK.AdapativePayments();
                ap.APIProfile = profile2;
                
                PreapprovalResponse PResponse = ap.preapproval(preapprovalRequest);

                if (ap.isSuccess.ToUpper() == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                else
                {

                    Session[Constants.SessionConstants.PREAPPROVALKEY] = PResponse.preapprovalKey;
                    this.Response.Redirect(ConfigurationManager.AppSettings["PAYPAL_REDIRECT_URL"] + "_ap-preapproval&preapprovalkey=" + PResponse.preapprovalKey, false);

                }


            }
            catch (FATALException FATALEx)
            {
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                this.Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL", false);
            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in PreApproval Page.", ex);
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                this.Response.Redirect("APIError.aspx?type=FATAL", false);
            }
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
			this.Submit.ServerClick += new System.EventHandler(this.Submit_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
