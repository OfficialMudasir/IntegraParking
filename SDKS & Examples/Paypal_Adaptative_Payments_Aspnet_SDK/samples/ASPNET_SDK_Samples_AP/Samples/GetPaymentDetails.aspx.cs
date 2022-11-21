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
using System.Xml.Serialization ;
using System.Xml;
using PayPal.Platform.SDK;
namespace ASPNET_SDK_Samples.Samples
{
	/// <summary>
	/// Summary description for GetPaymentDetails.
	/// </summary>
	public class GetPaymentDetails : System.Web.UI.Page
	{
		#region Private Members

		protected System.Web.UI.HtmlControls.HtmlInputText PayKey;
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

            if (Session[Constants.SessionConstants.PAYKEY] != null)
            {
                PayKey.Value = Session[Constants.SessionConstants.PAYKEY].ToString();
                Session.Remove(Constants.SessionConstants.PAYKEY.ToString());
            }
            
		}

		/// <summary>
		/// Handles On click event of Submit button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Arguments</param>
		private void Submit_ServerClick(object sender, System.EventArgs e)
		{

			try
			{
				string redirectPage = Constants.ASPXPages.PAYMENTDETAILS + "?" + Constants.QueryStringConstants.PAYKEY +"=" + PayKey.Value ;
				this.Response.Redirect(redirectPage, false); 
				
			}
			catch(FATALException FATALEx)
			{
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				this.Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL", false); 
			}
			catch(Exception ex)
			{
				FATALException FATALEx = new FATALException("Error occurred in GetPaymentDetails Page.", ex); 
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
				this.Response.Redirect("APIError.aspx?type=FATAL", false); 

			}
		}

		#endregion
	}
}
