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
    /// Summary description for RefundReceipt.
    /// </summary>
    public class ExecutePaymentReceipt : System.Web.UI.Page
    {

        #region Events

        private void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here

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
        public ExecutePaymentResponse PaymentResponse
        {
            get
            {
                if (Session[Constants.SessionConstants.EXECUTEPAYMENTRESPONSE] == null)
                    return null;

                return (ExecutePaymentResponse)Session[Constants.SessionConstants.EXECUTEPAYMENTRESPONSE];

            }

        }

        #endregion

    }
}
