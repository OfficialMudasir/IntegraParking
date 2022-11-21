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
	/// Summary description for SetPay.
	/// </summary>

	public class PayCreate : System.Web.UI.Page
	{

       
		#region Private Members

		protected System.Web.UI.HtmlControls.HtmlInputText email;
        protected System.Web.UI.HtmlControls.HtmlInputText preapprovalkey;
        protected System.Web.UI.HtmlControls.HtmlInputText actiontype;
		protected System.Web.UI.HtmlControls.HtmlInputButton Submit;
		protected System.Web.UI.HtmlControls.HtmlInputText receiveremail_0;
		protected System.Web.UI.HtmlControls.HtmlInputText amount_0;
		protected System.Web.UI.HtmlControls.HtmlInputText receiveremail_1;
		protected System.Web.UI.HtmlControls.HtmlInputText amount_1;
		protected System.Web.UI.HtmlControls.HtmlSelect currencyCode;
			
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
            
			PayRequest payRequest = null;
            BaseAPIProfile profile2 = null;
            string preapprovalKey = preapprovalkey.Value;
			
			try
			{
                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
				string url=Request.Url.Scheme+ "://"+Request.Url.Host+":"+ Request.Url.Port; 
				string returnURL = url + ResolveUrl("PayCreateDetails.aspx");
				string cancelURL = url + ResolveUrl("PayCreate.aspx");
                
				payRequest = new PayRequest();
              
              	payRequest.cancelUrl = cancelURL;
				payRequest.returnUrl = returnURL;
				payRequest.senderEmail = email.Value  ;
				payRequest.clientDetails = new ClientDetailsType();
				payRequest.clientDetails =  ClientInfoUtil.getMyAppDetails();
                payRequest.actionType = actiontype.Value;
				payRequest.currencyCode = currencyCode.Items[currencyCode.SelectedIndex].Value;
				payRequest.requestEnvelope = new RequestEnvelope();
                payRequest.requestEnvelope = ClientInfoUtil.getMyAppRequestEnvelope();
				payRequest.receiverList = new Receiver[1];
				payRequest.receiverList[0] = new Receiver();
				payRequest.receiverList[0].amount = decimal.Parse(amount_0.Value) ;
				payRequest.receiverList[0].email = receiveremail_0.Value ; 
                //payRequest.receiverList[1] = new Receiver();
                //payRequest.receiverList[1].amount = decimal.Parse(amount_1.Value );
                //payRequest.receiverList[1].email = receiveremail_1.Value ;
                if (preapprovalKey.Trim() != "")
                {
                    payRequest.preapprovalKey = preapprovalkey.Value;
                }
                
                PayPal.Platform.SDK.AdapativePayments ap = new PayPal.Platform.SDK.AdapativePayments();
                ap.APIProfile=profile2;
                PayResponse PResponse = ap.pay(payRequest);
                
                if (ap.isSuccess.ToUpper() == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                else
                {
                   
                    if (PResponse.paymentExecStatus == "CREATED")
                    {

                        HttpContext.Current.Response.Redirect("PayCreateDetails.aspx?paykey=" + PResponse.payKey, false);
                        HttpContext.Current.Session[Constants.SessionConstants.PAYKEY] = PResponse.payKey;
                    }
                    else
                    {
                        HttpContext.Current.Session[Constants.SessionConstants.PAYKEY] = PResponse.payKey;
                        HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["PAYPAL_REDIRECT_URL"] + "_ap-payment&paykey=" + PResponse.payKey, false);

                    }

                }
             }
			catch(FATALException FATALEx)
			{
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				this.Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL", false); 
			}
			catch(Exception ex)
			{
				FATALException FATALEx = new FATALException("Error occurred in PayCreate Page.", ex); 
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				this.Response.Redirect("APIError.aspx?type=FATAL", false); 
			}
		}

		#endregion
	}
}
