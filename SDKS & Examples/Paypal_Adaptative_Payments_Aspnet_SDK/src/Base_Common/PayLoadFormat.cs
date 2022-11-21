using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace PayPal.Platform.SDK
{
    /// <summary>
    /// Payload Formats
    /// </summary>
    public enum PayLoadFromat
    {
        SOAP11,
        NV,
        JSON,
        XML
    }
}
