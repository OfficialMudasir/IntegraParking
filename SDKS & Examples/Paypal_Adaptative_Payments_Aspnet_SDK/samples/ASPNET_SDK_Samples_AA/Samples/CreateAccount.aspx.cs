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
	/// Summary description for CreateAccount.
	/// </summary>
	public class CreateAccount : System.Web.UI.Page
	{
		#region Private Members

		protected System.Web.UI.HtmlControls.HtmlSelect accountType;
		protected System.Web.UI.HtmlControls.HtmlSelect nameSalutation;
		protected System.Web.UI.HtmlControls.HtmlInputText nameFirstName;
		protected System.Web.UI.HtmlControls.HtmlInputText nameMiddleName;
		protected System.Web.UI.HtmlControls.HtmlInputText nameLastName;
		protected System.Web.UI.HtmlControls.HtmlInputText dateOfBirth;
		protected System.Web.UI.HtmlControls.HtmlInputText addressLine1;
		protected System.Web.UI.HtmlControls.HtmlInputText addressLine2;
		protected System.Web.UI.HtmlControls.HtmlInputText addressCity;
		protected System.Web.UI.HtmlControls.HtmlSelect addressState;
		protected System.Web.UI.HtmlControls.HtmlInputText addressPostalCode;
		protected System.Web.UI.HtmlControls.HtmlInputText addressCountryCode;
		protected System.Web.UI.HtmlControls.HtmlInputText citizenshipCountryCode;
		protected System.Web.UI.HtmlControls.HtmlInputText contactPhoneNumber;
		protected System.Web.UI.HtmlControls.HtmlInputText partnerField1;
		protected System.Web.UI.HtmlControls.HtmlInputText partnerField2;
		protected System.Web.UI.HtmlControls.HtmlInputText partnerField3;
		protected System.Web.UI.HtmlControls.HtmlInputText partnerField4;
		protected System.Web.UI.HtmlControls.HtmlInputText partnerField5;
		protected System.Web.UI.HtmlControls.HtmlSelect currencyCode;
        protected System.Web.UI.HtmlControls.HtmlInputText sandboxEmail;
        protected System.Web.UI.HtmlControls.HtmlInputText sandboxDeveloperEmail;
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
            
			CreateAccountRequest cretaeAccountRequest = null;
            BaseAPIProfile profile2 = null;
            
			try
			{

                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
                
                cretaeAccountRequest =new CreateAccountRequest();
                cretaeAccountRequest.accountType = accountType.Items[accountType.SelectedIndex].Value ;
				cretaeAccountRequest.name = new NameType();
				cretaeAccountRequest.name.salutation = nameSalutation.Items[nameSalutation.SelectedIndex].Value ;
				cretaeAccountRequest.name.firstName = nameFirstName.Value ;
				cretaeAccountRequest.name.middleName = nameMiddleName.Value;
				cretaeAccountRequest.name.lastName = nameLastName.Value ;
				cretaeAccountRequest.dateOfBirth = DateTime.Parse(dateOfBirth.Value);
				cretaeAccountRequest.address = new AddressType();
				cretaeAccountRequest.address.line1 = addressLine1.Value ;
				cretaeAccountRequest.address.line2 = addressLine2.Value ;
				cretaeAccountRequest.address.city = addressCity.Value ;
				cretaeAccountRequest.address.state = addressState.Value ;
				cretaeAccountRequest.address.postalCode = addressPostalCode.Value;
				cretaeAccountRequest.address.countryCode = addressCountryCode.Value;
				cretaeAccountRequest.citizenshipCountryCode = citizenshipCountryCode.Value;
				cretaeAccountRequest.partnerField1 = partnerField1.Value ;
				cretaeAccountRequest.partnerField2 = partnerField2.Value ;
				cretaeAccountRequest.partnerField3 = partnerField3.Value ;
				cretaeAccountRequest.partnerField4 = partnerField4.Value ;
				cretaeAccountRequest.partnerField5 = partnerField5.Value ;
				cretaeAccountRequest.currencyCode = currencyCode.Value ;
				cretaeAccountRequest.contactPhoneNumber = contactPhoneNumber.Value ;
				cretaeAccountRequest.preferredLanguageCode ="en_US";
                
                //cretaeAccountRequest.sandboxEmailAddress = sandboxDeveloperEmail.Value;
                profile2.SandboxMailAddress = sandboxDeveloperEmail.Value;

                cretaeAccountRequest.emailAddress = sandboxEmail.Value;
                string url = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
                string returnURL = url + ResolveUrl("CreateAccountReceipt.aspx");
                cretaeAccountRequest.createAccountWebOptions = new CreateAccountWebOptionsType();
                cretaeAccountRequest.createAccountWebOptions.returnUrl = returnURL;
                cretaeAccountRequest.registrationType = "WEB";
                
				cretaeAccountRequest.clientDetails = ClientInfoUtil.getMyAppDetails();
                PayPal.Platform.SDK.AdaptiveAccounts aa = new PayPal.Platform.SDK.AdaptiveAccounts();
                aa.APIProfile = profile2;
                CreateAccountResponse CAResponse = aa.CreateAccount(cretaeAccountRequest);
				
				if (aa.isSuccess == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = aa.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                
				else
				{
                    Session[Constants.SessionConstants.CREATEACCOUNTRESPONSE] = CAResponse;
                    Response.Redirect(CAResponse.redirectURL, false);
					
				}


			}
			catch(FATALException FATALEx)
			{
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL"); 
			}
			catch(Exception ex)
			{
				FATALException FATALEx = new FATALException("Error occurred in CreateAccount Page.", ex); 
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				Response.Redirect("APIError.aspx?type=FATAL"); 
			}
		}

		#endregion
	}
}
