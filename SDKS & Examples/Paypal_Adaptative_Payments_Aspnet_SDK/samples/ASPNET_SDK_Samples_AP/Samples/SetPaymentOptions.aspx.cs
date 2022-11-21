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

    public partial  class SetPaymentOptions : System.Web.UI.Page
    {
        #region Private Members

        //protected System.Web.UI.HtmlControls.HtmlInputText paykey;
        //protected System.Web.UI.HtmlControls.HtmlInputText countrycode;
        //protected System.Web.UI.HtmlControls.HtmlInputText name;
        //protected System.Web.UI.HtmlControls.HtmlInputText email;
        //protected System.Web.UI.HtmlControls.HtmlInputText firstname;
        //protected System.Web.UI.HtmlControls.HtmlInputText lastname;
        //protected System.Web.UI.HtmlControls.HtmlInputText customerid;
        //protected System.Web.UI.HtmlControls.HtmlInputText institutionid;
        //protected System.Web.UI.HtmlControls.HtmlInputText emailheader;
        //protected System.Web.UI.HtmlControls.HtmlInputText emailmarketing;
        //protected System.Web.UI.HtmlControls.HtmlInputButton Submit;

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

           
                if (Session[Constants.SessionConstants.PAYKEY] != null)
                {
                    paykey.Value  = Session[Constants.SessionConstants.PAYKEY].ToString();
                }
           
        }

        /// <summary>
        /// Handles onclick event of Submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_ServerClick(object sender, System.EventArgs e)
        {
            
            SetPaymentOptionsRequest paymentOptions = null;
            BaseAPIProfile profile2 = null;
            try
            {
                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];

                paymentOptions = new SetPaymentOptionsRequest();
                paymentOptions.payKey = paykey.Value;
                paymentOptions.requestEnvelope = new RequestEnvelope();
                paymentOptions.requestEnvelope = ClientInfoUtil.getMyAppRequestEnvelope();
                paymentOptions.displayOptions = new DisplayOptions();
                paymentOptions.displayOptions.emailHeaderImageUrl = emailheader.Value;
                paymentOptions.displayOptions.emailMarketingImageUrl = emailmarketing.Value;
                
                if (countrycode.Value != "" )
                {
                    paymentOptions.initiatingEntity = new InitiatingEntity();
                    paymentOptions.initiatingEntity.institutionCustomer = new InstitutionCustomer();
                    paymentOptions.initiatingEntity.institutionCustomer.countryCode = countrycode.Value;
                }
                if(email.Value.ToString() != "")
                {
                    paymentOptions.initiatingEntity = new InitiatingEntity();
                    paymentOptions.initiatingEntity.institutionCustomer = new InstitutionCustomer();
                    paymentOptions.initiatingEntity.institutionCustomer.email = email.Value;
                }
                if(firstname.Value.ToString() != ""  )
                {
                    paymentOptions.initiatingEntity = new InitiatingEntity();
                    paymentOptions.initiatingEntity.institutionCustomer = new InstitutionCustomer();
                    paymentOptions.initiatingEntity.institutionCustomer.firstName = firstname.Value;
                }
                if(institutionid.Value != ""  )
                {
                    paymentOptions.initiatingEntity = new InitiatingEntity();
                    paymentOptions.initiatingEntity.institutionCustomer = new InstitutionCustomer();
                    paymentOptions.initiatingEntity.institutionCustomer.institutionId = institutionid.Value;
                }
                if( customerid.Value != ""  )
                {
                    paymentOptions.initiatingEntity = new InitiatingEntity();
                    paymentOptions.initiatingEntity.institutionCustomer = new InstitutionCustomer();
                    paymentOptions.initiatingEntity.institutionCustomer.institutionCustomerId = customerid.Value;
                }
                if( lastname.Value != "" )
                {
                    paymentOptions.initiatingEntity = new InitiatingEntity();
                    paymentOptions.initiatingEntity.institutionCustomer = new InstitutionCustomer();
                    paymentOptions.initiatingEntity.institutionCustomer.lastName = lastname.Value;
                }

                PayPal.Platform.SDK.AdapativePayments ap = new PayPal.Platform.SDK.AdapativePayments();
                ap.APIProfile = profile2;
                SetPaymentOptionsResponse SPResponse = ap.SetPaymentOptions(paymentOptions);

                if (ap.isSuccess == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                else
                {

                    Session[Constants.SessionConstants.SETPAYMENTOPTIONSRESPONSE] = SPResponse;
                    this.Response.Redirect("SetPaymentOptionsReceipt.aspx", false);
                }
            }
            catch (FATALException FATALEx)
            {
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL");
            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in Refund Page.", ex);
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                Response.Redirect("APIError.aspx?type=FATAL");
            }
        }

        #endregion
    }
}