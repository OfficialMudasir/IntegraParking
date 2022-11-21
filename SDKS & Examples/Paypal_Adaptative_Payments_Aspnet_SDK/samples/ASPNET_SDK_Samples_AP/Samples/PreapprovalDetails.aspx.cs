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
using PayPal.Platform.SDK;
using PayPal.Services.Private.AP;


namespace ASPNET_SDK_Samples.Samples
{
	/// <summary>
	/// Summary description for PreapprovalDetails.
	/// </summary>
	public class PreapprovalDetails : System.Web.UI.Page
	{	

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

		#region Events

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if(!IsPostBack)
				LoadPreapprovalDetails();
			
		}

		#endregion

		#region Public Properties
		/// <summary>
		/// PreapprovalDetailsResponse Session object 
		/// </summary>
        public PayPal.Services.Private.AP.PreapprovalDetailsResponse PreapprovalDetailsResponse
		{
			get
			{
				if(Session[Constants.SessionConstants.PREAPPROVALDETAILSRESPONSE] == null)
					return null;

                return (PayPal.Services.Private.AP.PreapprovalDetailsResponse)Session[Constants.SessionConstants.PREAPPROVALDETAILSRESPONSE]; 
			}

		}

        public string GetPreApprovalKey()
        {
            string preApprovalKey = "";

            try
            {
                if (this.PreapprovalDetailsResponse != null)
                    preApprovalKey = Session[Constants.SessionConstants.PREAPPROVALKEY].ToString();

                return preApprovalKey;
            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in Preapproval Details Page.", ex);
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                this.Response.Redirect("APIError.aspx?type=FATAL", false);

                return "";
            }
        }
		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the Payment Details info from PaymentDetails API Call
		/// </summary>
		private void LoadPreapprovalDetails()
		{
			string preapprovalKey = "";
			PreapprovalDetailsRequest preapprovalDetailsRequest = null;
            BaseAPIProfile profile2 = null;

				
				if(Session[Constants.SessionConstants.PREAPPROVALKEY] != null)
				{
					preapprovalKey = Session[Constants.SessionConstants.PREAPPROVALKEY].ToString();
				}

                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
                preapprovalDetailsRequest = new PreapprovalDetailsRequest();
				preapprovalDetailsRequest.preapprovalKey = preapprovalKey;
				preapprovalDetailsRequest.requestEnvelope = new RequestEnvelope();
				preapprovalDetailsRequest.requestEnvelope.errorLanguage = "en-US";

                PayPal.Platform.SDK.AdapativePayments ap = new PayPal.Platform.SDK.AdapativePayments();
                ap.APIProfile = profile2;
                PreapprovalDetailsResponse PDResponse = ap.preapprovalDetails(preapprovalDetailsRequest);

                if (ap.isSuccess.ToUpper() == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", true);
                }
                else
				
				{
                    
					Session[Constants.SessionConstants.PREAPPROVALDETAILSRESPONSE] = PDResponse;
				}
				
		}

		#endregion
	}


}
