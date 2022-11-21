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
	/// Summary description for Refund.
	/// </summary>
	public class Refund : System.Web.UI.Page
	{
		#region Private Members

		protected System.Web.UI.HtmlControls.HtmlInputText payKey;
		protected System.Web.UI.HtmlControls.HtmlSelect currencyCode;
		protected System.Web.UI.HtmlControls.HtmlInputText receiveremail;
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit;
		protected System.Web.UI.HtmlControls.HtmlInputText amount;
	
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

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		/// <summary>
		/// Handles onclick event of Submit button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Submit_ServerClick(object sender, System.EventArgs e)
		{
			RefundRequest refundRequest = null;
            BaseAPIProfile profile2 = null;
           
          try
          {
                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];			
                refundRequest = new RefundRequest();
				refundRequest.payKey = payKey.Value ;
				refundRequest.currencyCode = currencyCode.Items[currencyCode.SelectedIndex].Value ;
				refundRequest.receiverList = new Receiver[1];
				refundRequest.receiverList[0] = new Receiver();
				refundRequest.receiverList[0].email = receiveremail.Value ;
				refundRequest.receiverList[0].amount = decimal.Parse(amount.Value) ;
				refundRequest.requestEnvelope = new RequestEnvelope();
				refundRequest.requestEnvelope.errorLanguage = "en-US";

                PayPal.Platform.SDK.AdapativePayments ap=new  PayPal.Platform.SDK.AdapativePayments();              
                ap.APIProfile = profile2;
                RefundResponse RResponse = ap.refund(refundRequest);
							
				if (ap.isSuccess == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
				else
				{
                     
					Session[Constants.SessionConstants.REFUNDRESPONSE] = RResponse;
					this.Response.Redirect("RefundReceipt.aspx", false);  					
				}


			}
			catch(FATALException FATALEx)
			{
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL"); 
			}
			catch(Exception ex)
			{
				FATALException FATALEx = new FATALException("Error occurred in Refund Page.", ex); 
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				Response.Redirect("APIError.aspx?type=FATAL"); 
			}
		}

		#endregion
	}
}
