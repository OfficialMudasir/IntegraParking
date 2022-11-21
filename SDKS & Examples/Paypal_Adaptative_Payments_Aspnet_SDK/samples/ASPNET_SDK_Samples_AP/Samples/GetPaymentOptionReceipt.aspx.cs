using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PayPal.Platform.SDK;
using PayPal.Services.Private.AP;
namespace ASPNET_SDK_Samples.Samples
{
    public partial class GetPaymentOptionReceipt : System.Web.UI.Page
    {

        #region Events

        public void Page_Load(object sender, System.EventArgs e)
        {
           
            
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
        /// Returns RefundResponse value from Session
        /// </summary>
        public GetPaymentOptionsResponse GetPaymentResponse
        {
       
            get
            {
                if (Session[Constants.SessionConstants.GETPAYMENTOPTIONSRESPONSE] == null)
                return null;
                return (GetPaymentOptionsResponse)Session[Constants.SessionConstants.GETPAYMENTOPTIONSRESPONSE];

             }
         
        }

        #endregion

     
    }
}
