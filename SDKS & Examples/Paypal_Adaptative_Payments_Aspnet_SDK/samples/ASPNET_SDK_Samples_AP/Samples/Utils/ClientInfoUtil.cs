using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PayPal.Services.Private.AP;


namespace ASPNET_SDK_Samples.Samples
{
    /// <summary>
    /// ClientInfoUtil
    /// </summary>
    public class ClientInfoUtil
    {
        /// <summary>
        /// Returns the Client Details of Adaptive Payments
        /// </summary>
        /// <returns>ClientDetailsType</returns>
        public static ClientDetailsType getMyAppDetails()
        {
            ClientDetailsType cl = new ClientDetailsType();
            cl.applicationId = ConfigurationManager.AppSettings["APPLICATION-ID"];
            cl.deviceId = ConfigurationManager.AppSettings["deviceId"];
            cl.ipAddress = ConfigurationManager.AppSettings["ipAddress"];
            return cl;
        }



        /// <summary>
        /// Returns the default request envelope object 
        /// </summary>
        /// <returns>RequestEnvelope</returns>
        public static RequestEnvelope getMyAppRequestEnvelope()
        {
            RequestEnvelope en = new RequestEnvelope();
            en.errorLanguage="en_US";
            return en;
        }
    }
}
