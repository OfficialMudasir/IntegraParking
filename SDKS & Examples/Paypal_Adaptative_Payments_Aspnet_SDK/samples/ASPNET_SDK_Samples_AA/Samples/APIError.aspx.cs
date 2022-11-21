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

namespace ASPNET_SDK_Samples.Samples
{
	/// <summary>
	/// Summary description for APIError.
	/// </summary>
	public class APIError : System.Web.UI.Page
	{
		#region private members

		protected System.Web.UI.HtmlControls.HtmlGenericControl divAPIError;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divFATALError;
	
		#endregion

		#region Events

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			showHideAPIError();
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

		#region Private Methods
		
		/// <summary>
		/// Shows and Hides APIError table
		/// </summary>
		private void showHideAPIError()
		{
			try
			{
				/// checks if FATAL exception.
				if(Request.QueryString[Constants.QueryStringConstants.TYPE] != null)
				{
					string type = Request.QueryString[Constants.QueryStringConstants.TYPE].ToString();
					if(type.ToUpper() == "FATAL")
					{
					
						divAPIError.Visible = false;
						divFATALError.Visible = true;
					}
				
				}
				else
				{
					divAPIError.Visible = true;
					divFATALError.Visible = false;
				}

			}
			catch(Exception ex)
			{
				FATALException FATALEx = new FATALException("Error occurred in APIError Page.", ex); 
				Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx ;
			}
		}

		#endregion

		#region Public Properties
		
		/// <summary>
		/// returns Transaction Exception stored in Session
		/// </summary>
		public TransactionException TransactionEx
		{
			get
			{
				if(Session[Constants.SessionConstants.FAULT] == null)
					return null;

				return (TransactionException)Session[Constants.SessionConstants.FAULT];

			}
		}
		
		/// <summary>
		/// returns FATAL Exception stored in Session
		/// </summary>
		public FATALException FATALEx
		{
			get
			{
				if(Session[Constants.SessionConstants.FATALEXCEPTION] == null)
					return null;

				return (FATALException)Session[Constants.SessionConstants.FATALEXCEPTION];

			}
		}

		#endregion

        #region Public Methods

        public string GetErrorDetails(object errorDetails)
        {
            string error = string.Empty;
            string param = string.Empty;
            try
            {
                FaultDetailFaultMessageError[] errors = (FaultDetailFaultMessageError[])errorDetails;
                if (errors == null)
                    return "There is no Error details available.";

                for (int index = 0; index < errors.Length; index++)
                {
                    error += "Error ID : " + errors[index].errorId + "</br>";
                    error += "Domain : " + errors[index].domain + "</br>";
                    error += "Severity : " + errors[index].severity + "</br>";
                    error += "Category : " + errors[index].category + "</br>";
                    error += "Message : " + errors[index].message + "</br>";
                    if (errors[index].parameter != null)
                    {

                        foreach (FaultDetailFaultMessageErrorParameter errorParam in errors[index].parameter)
                        {
                            param += errorParam.Value + ",";
                        }
                        if (param.Length > 0)
                            param = param.Substring(0, param.Length - 1);

                        error += "Parameter : " + param + "</br></br>";
                    }
                    else
                    {
                        error += "</br>";
                    }

                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return error;
        }

        #endregion


	}
}
