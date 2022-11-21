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
    public class AddPaymentCardDirect : System.Web.UI.Page
    {
        #region Private Members

        protected System.Web.UI.HtmlControls.HtmlInputText emailid;
        protected System.Web.UI.HtmlControls.HtmlSelect creditCardType;
        protected System.Web.UI.HtmlControls.HtmlInputText creditCardNumber;
        protected System.Web.UI.HtmlControls.HtmlInputText firstName;
        protected System.Web.UI.HtmlControls.HtmlInputText lastName;
        protected System.Web.UI.HtmlControls.HtmlSelect expDateMonth;
        protected System.Web.UI.HtmlControls.HtmlSelect expDateYear;
        protected System.Web.UI.HtmlControls.HtmlSelect confirmationtype;
        protected System.Web.UI.HtmlControls.HtmlInputText address1;
        protected System.Web.UI.HtmlControls.HtmlInputText address2;
        protected System.Web.UI.HtmlControls.HtmlInputText city;
        protected System.Web.UI.HtmlControls.HtmlSelect addressState;
        protected System.Web.UI.HtmlControls.HtmlInputText zipCode;
        protected System.Web.UI.HtmlControls.HtmlInputText country;
        protected System.Web.UI.HtmlControls.HtmlInputText cardVerificationNumber;
        protected System.Web.UI.HtmlControls.HtmlInputText createAccountKey;
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
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
        }

        /// <summary>
        /// Handles onclick event of Submit button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void Submit_ServerClick(object sender, System.EventArgs e)
        {
            AddPaymentCardRequest addPaymentCardRequest = null;
            BaseAPIProfile profile2 = null;
            try
            {
                profile2 = (BaseAPIProfile)HttpContext.Current.Session[Constants.SessionConstants.PROFILE];
                addPaymentCardRequest = new AddPaymentCardRequest();
                addPaymentCardRequest.cardNumber = creditCardNumber.Value;
                addPaymentCardRequest.cardType = (CardTypeType)Enum.Parse(typeof(CardTypeType), creditCardType.Value, true);
                addPaymentCardRequest.confirmationType = (ConfirmationType)Enum.Parse(typeof(ConfirmationType), confirmationtype.Value, true);
                addPaymentCardRequest.emailAddress = emailid.Value;
                addPaymentCardRequest.expirationDate = new CardDateType();
                addPaymentCardRequest.expirationDate.month = expDateMonth.Value;
                addPaymentCardRequest.expirationDate.year = expDateYear.Value;
                addPaymentCardRequest.billingAddress = new AddressType();
                addPaymentCardRequest.billingAddress.line1 = address1.Value;
                addPaymentCardRequest.billingAddress.line2 = address2.Value;
                addPaymentCardRequest.billingAddress.city = city.Value;
                addPaymentCardRequest.billingAddress.state = addressState.Value;
                addPaymentCardRequest.billingAddress.postalCode = zipCode.Value;
                addPaymentCardRequest.billingAddress.countryCode = country.Value;
                addPaymentCardRequest.cardVerificationNumber = cardVerificationNumber.Value;
                addPaymentCardRequest.createAccountKey = createAccountKey.Value;
                addPaymentCardRequest.nameOnCard = new NameType();
                addPaymentCardRequest.nameOnCard.firstName = firstName.Value;
                addPaymentCardRequest.nameOnCard.lastName = lastName.Value;


                PayPal.Platform.SDK.AdaptiveAccounts aa = new PayPal.Platform.SDK.AdaptiveAccounts();
                aa.APIProfile = profile2;
                AddPaymentCardResponse AddPaymentCardResponse = aa.AddPaymentCard(addPaymentCardRequest);

                if (aa.isSuccess == "FAILURE")
                {
                    HttpContext.Current.Session[Constants.SessionConstants.FAULT] = aa.LastError;
                    HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }

                else
                {
                    Session[Constants.SessionConstants.ADDPAYMENTCARDRESPONSE] = AddPaymentCardResponse;
                    Response.Redirect("AddPaymentCardReceipt.aspx", false);

                }


            }
            catch (FATALException FATALEx)
            {
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL");
            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in AddPaymentCard Page.", ex);
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                Response.Redirect("APIError.aspx?type=FATAL");
            }
        }
        #endregion

    }
}
