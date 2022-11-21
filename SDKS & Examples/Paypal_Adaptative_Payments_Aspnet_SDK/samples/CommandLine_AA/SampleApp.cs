using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using log4net;
using log4net.Config;
using System.Configuration;
using PayPal.Platform.SDK;


namespace CommandLineSamples
{
    public class CommandLineSamples
    {
        #region Main Method

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(CommandLineSamples));
        [STAThread]
        public static void Main(string[] EvenArgs)
        {
            log.Info("Entering application.");
            CommmandLineSamples();
            log.Info("Exiting application.");
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// CommmandLineSamples
        /// </summary>
        public static void CommmandLineSamples()
        {
            BaseAPIProfile profile = null;

            try
            {


                profile = CreateProfile(ConfigurationManager.AppSettings["APPLICATION-ID"]);
                
                string email_per = "platform.test" + DateTime.Now.TimeOfDay.Ticks + "@gmail.com";
                string createAccountKey_per = CreateAccount(email_per,profile);                
                string email_biz = "platform.test" + DateTime.Now.TimeOfDay.Ticks + "@gmail.com";                
                string createAccountKey_biz = CreateAccount_Business(email_biz, profile);
                
                GetVerifiedStatus(profile);
                AddBankAccount(profile);
                AddBankAccount_Direct(createAccountKey_per, email_per, profile);
                AddPaymentCard(profile);
                AddPaymentCard_direct(createAccountKey_biz, email_biz, profile);
                SetFundingSourceConfirmed(profile);                
                GetUserAgreement(createAccountKey_biz, profile);
                Console.ReadLine();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }

        }

        /// <summary>
        /// Creates the Profile object from the given Application ID
        /// </summary>
        /// <returns>BaseAPIProfile</returns>
        public static BaseAPIProfile CreateProfile(string applicationID)
        {
            BaseAPIProfile profile2 = new BaseAPIProfile();
            byte[] bCert = null;
            string filePath = string.Empty;
            FileStream fs = null;
            try
            {
                if (ConfigurationManager.AppSettings["API_AUTHENTICATION_MODE"] == "3TOKEN")
                {
                    profile2.APIProfileType = ProfileType.ThreeToken;
                    profile2.APIUsername = ConfigurationManager.AppSettings["API_USERNAME"];
                    profile2.APIPassword = ConfigurationManager.AppSettings["API_PASSWORD"];
                    profile2.APISignature = ConfigurationManager.AppSettings["API_SIGNATURE"];
                    profile2.DeviceIpAddress = ConfigurationManager.AppSettings["ipAddress"];
                    profile2.RequestDataformat = ConfigurationManager.AppSettings["API_REQUESTFORMAT"];
                    profile2.ResponseDataformat = ConfigurationManager.AppSettings["API_RESPONSEFORMAT"];
                    profile2.Environment = ConfigurationManager.AppSettings["ENDPOINT"];
                    profile2.ApplicationID = applicationID;
                    profile2.SandboxMailAddress = "platform.sdk.seller@gmail.com";
                    profile2.IsTrustAllCertificates =Convert.ToBoolean(ConfigurationManager.AppSettings["TrustAll"]);
                }
                else
                {
                    ////Certificate
                    profile2.APIProfileType = ProfileType.Certificate;
                    profile2.APIUsername = ConfigurationManager.AppSettings["API_USERNAME"];
                    profile2.APIPassword = ConfigurationManager.AppSettings["API_PASSWORD"];
                    profile2.ApplicationID = ConfigurationManager.AppSettings["APPLICATION-ID"];
                    profile2.RequestDataformat = ConfigurationManager.AppSettings["API_REQUESTFORMAT"];
                    profile2.ResponseDataformat = ConfigurationManager.AppSettings["API_RESPONSEFORMAT"];

                    ///loading the certificate file into profile.
                    filePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CERTIFICATE"].ToString());
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    bCert = new byte[fs.Length];
                    fs.Read(bCert, 0, int.Parse(fs.Length.ToString()));
                    fs.Close();

                    profile2.Certificate = bCert;
                    profile2.PrivateKeyPassword = ConfigurationManager.AppSettings["PRIVATE_KEY_PASSWORD"];
                    profile2.APISignature = "";
                    profile2.Environment = ConfigurationManager.AppSettings["ENDPOINT"];
                    profile2.IsTrustAllCertificates = Convert.ToBoolean(ConfigurationManager.AppSettings["TrustAll"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return profile2;
        }

       public static string GenerateCreditCardNumber()
        {
            int[] cc_number = new int[16];
            int cc_len = 16;
            int start = 0;
            Random r = new Random();
            cc_number[start++] = 4;


            for (int i = start; i < (cc_len - 1); i++)
            {
                cc_number[i] = (int)Math.Floor(r.NextDouble() * 10);
            }

            int sum = 0;
            for (int j = 0; j < (cc_len - 1); j++)
            {
                int digit = cc_number[j];
                if ((j & 1) == (cc_len & 1)) digit *= 2;
                if (digit > 9) digit -= 9;
                sum += digit;
            }

            int[] check_digit = new int[] { 0, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            cc_number[cc_len - 1] = check_digit[sum % 10];

            string result = string.Empty;
            foreach (int digit in cc_number)
            {
                result += digit;
            }
            return result;
        }
        /// <summary>
        /// Calls the CreateAccount Platform API
        /// </summary>
        /// <param name="profile"></param>
        public static string CreateAccount(string email , BaseAPIProfile profile)
        {
            
            PayPal.Services.Private.AA.CreateAccountResponse cretaeAccountResponse = null;
            try
            {
                PayPal.Services.Private.AA.CreateAccountRequest AccountRequest = new PayPal.Services.Private.AA.CreateAccountRequest();
                AccountRequest.accountType = "PERSONAL";
                AccountRequest.name = new PayPal.Services.Private.AA.NameType();
                AccountRequest.name.salutation = "Dr.";
                AccountRequest.name.firstName = "Bonzop";
                AccountRequest.name.middleName = "Simore";
                AccountRequest.name.lastName = "Zaius";
                AccountRequest.dateOfBirth = DateTime.Parse("1968/01/01Z");
                AccountRequest.address = new PayPal.Services.Private.AA.AddressType();
                AccountRequest.address.line1 = "1968 Ape Way";
                AccountRequest.address.line2 = "Apt 123";
                AccountRequest.address.city = "Austin";
                AccountRequest.address.state = "TX";
                AccountRequest.address.postalCode = "78750";
                AccountRequest.address.countryCode = "US";
                AccountRequest.citizenshipCountryCode = "US";
                AccountRequest.partnerField1 = "p1";
                AccountRequest.partnerField2 = "p2";
                AccountRequest.partnerField3 = "p3";
                AccountRequest.partnerField4 = "p4";
                AccountRequest.partnerField5 = "p5";
                AccountRequest.currencyCode = "USD";
                AccountRequest.contactPhoneNumber = "512-691-4160";
                AccountRequest.preferredLanguageCode = "en_US";
                AccountRequest.clientDetails = new PayPal.Services.Private.AA.ClientDetailsType();
                AccountRequest.clientDetails.applicationId = ConfigurationManager.AppSettings["APPLICATION-ID"];
                AccountRequest.clientDetails.deviceId = "127001";
                AccountRequest.clientDetails.ipAddress = "127.0.0.1";
                AccountRequest.emailAddress = email;
                //AccountRequest.sandboxEmailAddress = "platform.sdk.seller@gmail.com";
                AccountRequest.createAccountWebOptions = new PayPal.Services.Private.AA.CreateAccountWebOptionsType();
                AccountRequest.createAccountWebOptions.returnUrl = "http://www.google.com";
                AccountRequest.registrationType = "WEB";
                profile.SandboxMailAddress = "platform.sdk.seller@gmail.com";
                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;
                cretaeAccountResponse = aa.CreateAccount(AccountRequest);



                if (cretaeAccountResponse != null && cretaeAccountResponse.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("Transaction CreateAccount Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("Transaction CreateAccount  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return cretaeAccountResponse.createAccountKey;
        }

        /// <summary>
        /// CreateAccount Business
        /// </summary>
        /// <param name="profile"></param>
        public static string CreateAccount_Business(string email , BaseAPIProfile profile)
        {
            PayPal.Services.Private.AA.CreateAccountResponse cretaeAccountResponse = null;
            try
            {
                PayPal.Services.Private.AA.CreateAccountRequest AccountRequest = new PayPal.Services.Private.AA.CreateAccountRequest();
                AccountRequest.accountType = "BUSINESS";
                AccountRequest.name = new PayPal.Services.Private.AA.NameType();
                AccountRequest.name.salutation = "Dr.";
                AccountRequest.name.firstName = "Bonzop";
                AccountRequest.name.middleName = "Simore";
                AccountRequest.name.lastName = "Zaius";
                AccountRequest.dateOfBirth = DateTime.Parse("1968/01/01Z");
                AccountRequest.address = new PayPal.Services.Private.AA.AddressType();
                AccountRequest.address.line1 = "1968 Ape Way";
                AccountRequest.address.line2 = "Apt 123";
                AccountRequest.address.city = "Austin";
                AccountRequest.address.state = "TX";
                AccountRequest.address.postalCode = "78750";
                AccountRequest.address.countryCode = "US";
                AccountRequest.citizenshipCountryCode = "US";
                AccountRequest.partnerField1 = "p1";
                AccountRequest.partnerField2 = "p2";
                AccountRequest.partnerField3 = "p3";
                AccountRequest.partnerField4 = "p4";
                AccountRequest.partnerField5 = "p5";
                AccountRequest.currencyCode = "USD";
                AccountRequest.contactPhoneNumber = "512-691-4160";
                AccountRequest.preferredLanguageCode = "en_US";
                AccountRequest.clientDetails = new PayPal.Services.Private.AA.ClientDetailsType();
                AccountRequest.clientDetails.applicationId = ConfigurationManager.AppSettings["APPLICATION-ID"];
                AccountRequest.clientDetails.deviceId = "127001";
                AccountRequest.clientDetails.ipAddress = "127.0.0.1";
                AccountRequest.emailAddress = email;
                //AccountRequest.sandboxEmailAddress = "platform.sdk.seller@gmail.com";
                AccountRequest.createAccountWebOptions = new PayPal.Services.Private.AA.CreateAccountWebOptionsType();
                AccountRequest.createAccountWebOptions.returnUrl = "http://www.google.com";
                AccountRequest.registrationType = "WEB";


                ////Business Info
                AccountRequest.businessInfo = new PayPal.Services.Private.AA.BusinessInfoType();
                AccountRequest.businessInfo.businessName = "Bonzop";
                AccountRequest.businessInfo.businessAddress = new PayPal.Services.Private.AA.AddressType();
                AccountRequest.businessInfo.businessAddress.line1 = "1968 Ape Way";
                AccountRequest.businessInfo.businessAddress.line2 = "Apt 123";
                AccountRequest.businessInfo.businessAddress.city = "Austin";
                AccountRequest.businessInfo.businessAddress.state = "TX";
                AccountRequest.businessInfo.businessAddress.postalCode = "78750";
                AccountRequest.businessInfo.businessAddress.countryCode = "US";
                AccountRequest.businessInfo.workPhone ="5126914160";
                AccountRequest.businessInfo.category = "1001";
                AccountRequest.businessInfo.subCategory = "2001";
                AccountRequest.businessInfo.customerServiceEmail = "platfo_1255076101_per@gmail.com";
                AccountRequest.businessInfo.customerServicePhone = "5126914160";
                AccountRequest.businessInfo.webSite = "https://www.x.com";
                AccountRequest.businessInfo.dateOfEstablishment = Convert.ToDateTime("2000-01-01");
                AccountRequest.businessInfo.dateOfEstablishmentSpecified = true;

                AccountRequest.businessInfo.businessType = PayPal.Services.Private.AA.BusinessType.INDIVIDUAL;
                AccountRequest.businessInfo.businessTypeSpecified = true;
                AccountRequest.businessInfo.averagePrice = Convert.ToDecimal("1.00");
                AccountRequest.businessInfo.averagePriceSpecified = true;
                AccountRequest.businessInfo.averageMonthlyVolume = Convert.ToDecimal("100");
                AccountRequest.businessInfo.averageMonthlyVolumeSpecified = true;
                AccountRequest.businessInfo.percentageRevenueFromOnline = "60";
                AccountRequest.businessInfo.salesVenue = new PayPal.Services.Private.AA.SalesVenueType[1];
                AccountRequest.businessInfo.salesVenue[0] = new PayPal.Services.Private.AA.SalesVenueType();
                AccountRequest.businessInfo.salesVenue[0] = PayPal.Services.Private.AA.SalesVenueType.WEB;
                ////


                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;
                cretaeAccountResponse = aa.CreateAccount(AccountRequest);


                if (cretaeAccountResponse!= null && cretaeAccountResponse.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("Transaction CreateAccount - Business Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("Transaction CreateAccount Business  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return cretaeAccountResponse.createAccountKey;
        }

        /// <summary>
        /// GetVerifiedStatus
        /// </summary>
        /// <param name="profile"></param>
        public static void GetVerifiedStatus(BaseAPIProfile profile)
        {
            
            PayPal.Services.Private.AA.GetVerifiedStatusResponse getVerifiedStatusRes = null;
            try
            {
                
                PayPal.Services.Private.AA.GetVerifiedStatusRequest getVerifiedStatusRequest = new PayPal.Services.Private.AA.GetVerifiedStatusRequest();

                getVerifiedStatusRequest.emailAddress = "platfo11_per@gmail.com";
                getVerifiedStatusRequest.firstName = "Bonzop";
                getVerifiedStatusRequest.lastName = "Zaius";
                getVerifiedStatusRequest.matchCriteria = "NAME";
                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;
                



                
                aa.APIProfile = profile;
                getVerifiedStatusRes = aa.GetVerifiedStatus(getVerifiedStatusRequest);


                if (getVerifiedStatusRes!= null && getVerifiedStatusRes.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("GetVerifiedStatus Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("GetVerifiedStatus  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        /// <summary>
        /// AddBankAccount
        /// </summary>
        /// <param name="profile"></param>
        public static void AddBankAccount(BaseAPIProfile profile)
        {

            PayPal.Services.Private.AA.AddBankAccountResponse addBankAccountRes = null;
            try
            {

                PayPal.Services.Private.AA.AddBankAccountRequest addBankAccountRequest = new PayPal.Services.Private.AA.AddBankAccountRequest();

                addBankAccountRequest.emailAddress = ConfigurationManager.AppSettings["EmailId"];
                addBankAccountRequest.bankCountryCode = "US";
                addBankAccountRequest.bankName = "Huntington Bank";
                addBankAccountRequest.routingNumber = "021473030";
                addBankAccountRequest.bankAccountNumber = "162951";
                addBankAccountRequest.bankAccountType = PayPal.Services.Private.AA.BankAccountType.CHECKING;
                addBankAccountRequest.bankAccountTypeSpecified = true;
                addBankAccountRequest.confirmationType = PayPal.Services.Private.AA.ConfirmationType.WEB;
                addBankAccountRequest.webOptions = new PayPal.Services.Private.AA.WebOptionsType();
                addBankAccountRequest.webOptions.cancelUrl = "http://www.PayPal.com";
                addBankAccountRequest.webOptions.cancelUrlDescription = "test";
                addBankAccountRequest.webOptions.returnUrl = "http://www.x.com"; ;
                addBankAccountRequest.webOptions.returnUrlDescription = "returnURL";


                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;





                aa.APIProfile = profile;
                addBankAccountRes = aa.AddBankAccount(addBankAccountRequest);


                if (addBankAccountRes!= null && addBankAccountRes.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("AddBankAccount Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("AddBankAccount  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }
        public static void AddBankAccount_Direct(string createAccountKey,String emailId, BaseAPIProfile profile)
        {

            PayPal.Services.Private.AA.AddBankAccountResponse addBankAccountRes = null;
            try
            {

                PayPal.Services.Private.AA.AddBankAccountRequest addBankAccountRequest = new PayPal.Services.Private.AA.AddBankAccountRequest();

                addBankAccountRequest.emailAddress = emailId;
                addBankAccountRequest.createAccountKey = createAccountKey;
                addBankAccountRequest.bankCountryCode = "US";
                addBankAccountRequest.bankName = "Huntington Bank";
                addBankAccountRequest.routingNumber = "021473030";
                addBankAccountRequest.bankAccountNumber = "162951";
                addBankAccountRequest.bankAccountType = PayPal.Services.Private.AA.BankAccountType.CHECKING;
                addBankAccountRequest.bankAccountTypeSpecified = true;
                addBankAccountRequest.confirmationType = PayPal.Services.Private.AA.ConfirmationType.WEB;
                addBankAccountRequest.webOptions = new PayPal.Services.Private.AA.WebOptionsType();
                addBankAccountRequest.webOptions.cancelUrl = "http://www.PayPal.com";
                addBankAccountRequest.webOptions.cancelUrlDescription = "test";
                addBankAccountRequest.webOptions.returnUrl = "http://www.x.com"; ;
                addBankAccountRequest.webOptions.returnUrlDescription = "returnURL";


                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;





                aa.APIProfile = profile;
                addBankAccountRes = aa.AddBankAccount(addBankAccountRequest);


                if (addBankAccountRes!= null && addBankAccountRes.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("AddBankAccount Direct Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("AddBankAccount Direct Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }
        public static void AddPaymentCard(BaseAPIProfile profile)
        {
            PayPal.Services.Private.AA.AddPaymentCardResponse addPaymentCardRes = null;
            try
            {
                PayPal.Services.Private.AA.AddPaymentCardRequest addPaymentCardRequest = new PayPal.Services.Private.AA.AddPaymentCardRequest();
                
                addPaymentCardRequest.cardNumber = "4739919358323864";
                addPaymentCardRequest.cardType = PayPal.Services.Private.AA.CardTypeType.Visa;
                addPaymentCardRequest.confirmationType = PayPal.Services.Private.AA.ConfirmationType.WEB;
                addPaymentCardRequest.emailAddress = ConfigurationManager.AppSettings["EmailId"];
                addPaymentCardRequest.expirationDate = new PayPal.Services.Private.AA.CardDateType();
                addPaymentCardRequest.expirationDate.month = "01";
                addPaymentCardRequest.expirationDate.year = "2014";
                addPaymentCardRequest.billingAddress = new PayPal.Services.Private.AA.AddressType();
                addPaymentCardRequest.billingAddress.line1 = "1 Main St";
                addPaymentCardRequest.billingAddress.line2 = "2nd cross";
                addPaymentCardRequest.billingAddress.city = "Austin";
                addPaymentCardRequest.billingAddress.state = "TX";
                addPaymentCardRequest.billingAddress.postalCode = "78750";
                addPaymentCardRequest.billingAddress.countryCode = "US";
                addPaymentCardRequest.nameOnCard = new PayPal.Services.Private.AA.NameType();
                addPaymentCardRequest.nameOnCard.firstName = "John";
                addPaymentCardRequest.nameOnCard.lastName = "Deo";

                addPaymentCardRequest.webOptions = new PayPal.Services.Private.AA.WebOptionsType();

                addPaymentCardRequest.webOptions.cancelUrl = "http://www.PayPal.com";
                addPaymentCardRequest.webOptions.cancelUrlDescription = "cancelURL";
                addPaymentCardRequest.webOptions.returnUrl = "http://www.x.com";
                addPaymentCardRequest.webOptions.returnUrlDescription = "returnURL";

                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;





                aa.APIProfile = profile;
                addPaymentCardRes = aa.AddPaymentCard(addPaymentCardRequest);


                if (addPaymentCardRes!= null && addPaymentCardRes.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("AddPaymentCard Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("AddPaymentCard Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        public static void AddPaymentCard_direct(string createAccountKey, String emailId, BaseAPIProfile profile)
        {
            System.Random RandNum = new System.Random();
            string creditCardNumber = GenerateCreditCardNumber();

            PayPal.Services.Private.AA.AddPaymentCardResponse addPaymentCardRes = null;
            try
            {

                PayPal.Services.Private.AA.AddPaymentCardRequest addPaymentCardRequest = new PayPal.Services.Private.AA.AddPaymentCardRequest();

                addPaymentCardRequest.cardNumber = creditCardNumber;
                addPaymentCardRequest.cardType = PayPal.Services.Private.AA.CardTypeType.Visa;
                addPaymentCardRequest.confirmationType = PayPal.Services.Private.AA.ConfirmationType.NONE;
                addPaymentCardRequest.emailAddress = emailId;
                addPaymentCardRequest.createAccountKey = createAccountKey;
                addPaymentCardRequest.cardVerificationNumber = "956";
                addPaymentCardRequest.expirationDate = new PayPal.Services.Private.AA.CardDateType();
                addPaymentCardRequest.expirationDate.month = "01";
                addPaymentCardRequest.expirationDate.year = "2014";
                addPaymentCardRequest.billingAddress = new PayPal.Services.Private.AA.AddressType();
                addPaymentCardRequest.billingAddress.line1 = "1 Main St";
                addPaymentCardRequest.billingAddress.line2 = "2nd cross";
                addPaymentCardRequest.billingAddress.city = "Austin";
                addPaymentCardRequest.billingAddress.state = "TX";
                addPaymentCardRequest.billingAddress.postalCode = "78750";
                addPaymentCardRequest.billingAddress.countryCode = "US";
                addPaymentCardRequest.nameOnCard = new PayPal.Services.Private.AA.NameType();
                addPaymentCardRequest.nameOnCard.firstName = "John";
                addPaymentCardRequest.nameOnCard.lastName = "Deo";


                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;

                addPaymentCardRes = aa.AddPaymentCard(addPaymentCardRequest);


                if (addPaymentCardRes!= null && addPaymentCardRes.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("AddPaymentCard Direct Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("AddPaymentCard Direct Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        public static void SetFundingSourceConfirmed(BaseAPIProfile profile)
        {
            Console.WriteLine("SetFundingSourceConfirmed - one of the input parameters require web flow");
        }

        public static void GetUserAgreement(string createAccountKey, BaseAPIProfile profile)
        {

            PayPal.Services.Private.AA.GetUserAgreementResponse getUserAgreementRes = null;
            try
            {

                PayPal.Services.Private.AA.GetUserAgreementRequest getUserAgreementRequest = new PayPal.Services.Private.AA.GetUserAgreementRequest();
                getUserAgreementRequest.createAccountKey = createAccountKey;                

                AdaptiveAccounts aa = new AdaptiveAccounts();
                aa.APIProfile = profile;
                getUserAgreementRes = aa.GetUserAgreement(getUserAgreementRequest);

                if (getUserAgreementRes != null && getUserAgreementRes.responseEnvelope.ack == PayPal.Services.Private.AA.AckCode.Success)
                {
                    Console.WriteLine("GetUserAgreement Successful!");
                }
                else
                {
                    TransactionException tranactionEx = aa.LastError;
                    Console.WriteLine("GetUserAgreement Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        #endregion


    }


}
