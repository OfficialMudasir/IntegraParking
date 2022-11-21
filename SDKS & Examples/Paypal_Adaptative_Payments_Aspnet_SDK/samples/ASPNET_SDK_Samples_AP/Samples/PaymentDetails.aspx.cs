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
	/// Summary description for PaymentDetails.
	/// </summary>
	public class PaymentDetails : System.Web.UI.Page
	{

		#region Events

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if(!IsPostBack)
				LoadPaymentDetails();
			
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

		#region Public Properties
		
		/// <summary>
		/// PaymentDetailsResponse Session object
		/// </summary>
        public PayPal.Services.Private.AP.PaymentDetailsResponse PaymentDetailsResponse
		{
			get
			{
				if(Session[Constants.SessionConstants.PAYMENTDETAILSRESPONSE] == null)
					return null;

                return (PayPal.Services.Private.AP.PaymentDetailsResponse)Session[Constants.SessionConstants.PAYMENTDETAILSRESPONSE]; 
			}

		}

		#endregion

		#region Public Methods

        /// <summary>
        /// PreapprovalKey
        /// </summary>
        /// <returns></returns>
        public string GetPreapprovalKey()
        {
            string preapprovalKey = "";

            try
            {
                if (this.PaymentDetailsResponse.preapprovalKey == null)
                    
                    preapprovalKey = "NotApplicable";
                    
                else
                    preapprovalKey = this.PaymentDetailsResponse.preapprovalKey; 

                return preapprovalKey;

            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in PaymentDetails Page.", ex);
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
		private void LoadPaymentDetails()
		{
			string payKey = "";
            PaymentDetailsRequest pd = null;
            BaseAPIProfile profile2 = null;	
				if(Request.QueryString[Constants.QueryStringConstants.PAYKEY] != null)
				{
					payKey = Request.QueryString[Constants.QueryStringConstants.PAYKEY].ToString();
				}
				else
				{
					if(Session[Constants.SessionConstants.PAYKEY] != null)
					{
						payKey = Session[Constants.SessionConstants.PAYKEY].ToString();
					}
				}
                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
                pd = new PaymentDetailsRequest();
				pd.requestEnvelope = new RequestEnvelope();
                pd.requestEnvelope = ClientInfoUtil.getMyAppRequestEnvelope();
				pd.payKey = payKey;

                PayPal.Platform.SDK.AdapativePayments ap = new PayPal.Platform.SDK.AdapativePayments();
                ap.APIProfile = profile2;
                PaymentDetailsResponse PDResponse = ap.paymentDetails(pd);


                if (ap.isSuccess == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", true);
                   
                }
                else
				{
                    Session[Constants.SessionConstants.PAYMENTDETAILSRESPONSE] = PDResponse;
					
				}				


		}

		#endregion
		
	}
}
