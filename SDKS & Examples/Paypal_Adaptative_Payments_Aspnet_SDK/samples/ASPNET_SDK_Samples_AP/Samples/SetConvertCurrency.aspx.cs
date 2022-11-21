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
    public partial class SetConvertCurrency : System.Web.UI.Page
    {

        #region Private Members
        protected System.Web.UI.HtmlControls.HtmlInputText amount_0;
        protected System.Web.UI.HtmlControls.HtmlInputText fromcode_0;
        protected System.Web.UI.HtmlControls.HtmlInputText amount_1;
        protected System.Web.UI.HtmlControls.HtmlInputText fromcode_1;
        protected System.Web.UI.HtmlControls.HtmlInputText tocode_0;
        protected System.Web.UI.HtmlControls.HtmlInputText tocode_1;
        protected System.Web.UI.HtmlControls.HtmlInputText tocode_2;
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

            ConvertCurrencyRequest CCRequest = null;
            BaseAPIProfile profile2 = null;
      
            try
            {
                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
                CCRequest = new ConvertCurrencyRequest();
                CurrencyType[] ct = new CurrencyType[2];
                ct[0] = new CurrencyType();
                ct[0].amount =decimal.Parse(amount_0.Value); //decimal.Parse("1.00");
                ct[0].code =fromcode_0.Value;
                ct[1] = new CurrencyType();
                ct[1].amount = decimal.Parse(amount_1.Value);//decimal.Parse("100.00");
                ct[1].code =fromcode_1.Value;
                CCRequest.baseAmountList = ct;
                string[] tocodes = new string[3];
                tocodes[0] = tocode_0.Value ;
                tocodes[1] = tocode_1.Value;
                tocodes[2] = tocode_2.Value;
                CCRequest.convertToCurrencyList = tocodes;
                CCRequest.requestEnvelope = new RequestEnvelope();
                CCRequest.requestEnvelope = ClientInfoUtil.getMyAppRequestEnvelope();
                
                PayPal.Platform.SDK.AdapativePayments ap = new PayPal.Platform.SDK.AdapativePayments();
                ap.APIProfile = profile2;
                ConvertCurrencyResponse CCResponse = ap.ConvertCurrency(CCRequest);

                if (ap.isSuccess.ToUpper() == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                else
                {
                    Session[Constants.SessionConstants.CONVERTCURRENCYRESPONSE] = CCResponse;
                    this.Response.Redirect("ConvertCurrencyReceipt.aspx", false);

                }
            }
            catch (FATALException FATALEx)
            {
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                this.Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL", false);
            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in SetPay Page.", ex);
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                this.Response.Redirect("APIError.aspx?type=FATAL", false);
            }
        }

        #endregion
    }
}
