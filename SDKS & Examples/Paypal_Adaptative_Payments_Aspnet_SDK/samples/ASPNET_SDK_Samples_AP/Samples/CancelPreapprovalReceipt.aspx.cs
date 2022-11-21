using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PayPal.Platform.SDK;
using PayPal.Services.Private.AP;

namespace ASPNET_SDK_Samples.Samples
{
    public partial class CancelPreapprovalReceipt : System.Web.UI.Page
    {
        #region Public Properties

        /// <summary>
        /// Returns CancelPreapprovalResponse value from Session
        /// </summary>
        public CancelPreapprovalResponse CancelPreapprovalResponse
         {
            get
            {
                if (Session[Constants.SessionConstants.CANCELPREAPPROVALRESPONSE] == null)
                    return null;

                return (CancelPreapprovalResponse)Session[Constants.SessionConstants.CANCELPREAPPROVALRESPONSE];
            }

        }

        #endregion

    }

}
