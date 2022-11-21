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
using System.Threading;
using PayPal.Platform.SDK;
using PayPal.Services.Private.AA;


namespace ASPNET_SDK_Samples.Samples
{
	/// <summary>
	/// Summary description for AddBankAccount.
	/// </summary>
    public class SetFundingSourceConfirmed : System.Web.UI.Page
	{
		#region Private Members


        protected System.Web.UI.HtmlControls.HtmlInputText emailid;
        protected System.Web.UI.HtmlControls.HtmlInputText fundingSourceKey;
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
            SetFundingSourceConfirmedRequest setFundingSourceConfirmedRequest = null;
            BaseAPIProfile profile2 = null;
            
			try
			{

                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];

                setFundingSourceConfirmedRequest = new SetFundingSourceConfirmedRequest();
                setFundingSourceConfirmedRequest.emailAddress = emailid.Value;
                setFundingSourceConfirmedRequest.fundingSourceKey = fundingSourceKey.Value;
                                

                PayPal.Platform.SDK.AdaptiveAccounts aa = new PayPal.Platform.SDK.AdaptiveAccounts();
                aa.APIProfile = profile2;
                SetFundingSourceConfirmedResponse SetFundingSourceResponse = aa.SetFundingSourceConfirmed(setFundingSourceConfirmedRequest);
				
				if (aa.isSuccess == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = aa.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                
				else
				{
                    Session[Constants.SessionConstants.SETFUNDINGSOURCECONFIRMED] = SetFundingSourceResponse;
                    Response.Redirect("SetFundingSourceConfirmedReceipt.aspx", false);
					
				}


			}
			catch(FATALException FATALEx)
			{
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL"); 
			}
			catch(Exception ex)
			{
				FATALException FATALEx = new FATALException("Error occurred in SetFundingSourceConfirmed Page.", ex); 
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				Response.Redirect("APIError.aspx?type=FATAL"); 
			}
		}

		#endregion
	}
}
