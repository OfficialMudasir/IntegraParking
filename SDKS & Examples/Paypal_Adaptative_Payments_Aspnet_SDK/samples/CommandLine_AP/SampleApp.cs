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
using PayPal.Platform.SDK;
using PayPal.Services.Private.AP;
using System.IO;
using System.Text;
using log4net;
using log4net.Config;
using System.Configuration;

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

                string paykey = Pay(profile);
                string preapprovalkey = Preapproval(profile);
                PaymentDetails(paykey, profile);
                Refund(paykey, profile);
                
                PreapprovalDetails(preapprovalkey, profile);
                GetAllowedFundingSources(preapprovalkey, profile);
                CancelPreapproval(preapprovalkey, profile);

                ConvertCurrency(profile);
                 
                string createpaykey = CreatePay(profile);
                GetFundingPlans(createpaykey, profile);
                GetAvailableShippingAddresses(createpaykey, profile);
                GetShippingAddresses(createpaykey, profile);                
                SetPaymentOption(createpaykey, profile);
                ExecutePayment(createpaykey, profile);
                GetPaymentOption(createpaykey, profile);

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
                    profile2.Environment = ConfigurationManager.AppSettings["ENDPOINT"];
                    profile2.RequestDataformat = ConfigurationManager.AppSettings["API_REQUESTFORMAT"];
                    profile2.ResponseDataformat = ConfigurationManager.AppSettings["API_RESPONSEFORMAT"];
                    profile2.ApplicationID = applicationID;

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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return profile2;
        }

        /// <summary>
        /// Calls the Pay Platform API
        /// </summary>
        /// <param name="profile">BaseAPIProfile</param>
        /// <returns>string payKey</returns>
        public static string Pay(BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.PayResponse PResponse = null;

            try
            {

                PayRequest payRequest = new PayRequest();
                payRequest.cancelUrl = "http://www.google.com";
                payRequest.returnUrl = "http://www.google.com";
                payRequest.senderEmail = "platfo_1255077030_biz@gmail.com";
                payRequest.clientDetails = new ClientDetailsType();
                payRequest.clientDetails.applicationId = "AppID";
                payRequest.clientDetails.deviceId = ConfigurationManager.AppSettings["deviceId"];
                payRequest.clientDetails.ipAddress = ConfigurationManager.AppSettings["ipAddress"];
                payRequest.actionType = "PAY";
                payRequest.currencyCode = "USD";
                payRequest.requestEnvelope = new RequestEnvelope();
                payRequest.requestEnvelope.errorLanguage = "en_US";
                payRequest.receiverList = new Receiver[2];
                payRequest.receiverList[0] = new Receiver();
                payRequest.receiverList[0].amount = decimal.Parse("1.00");
                payRequest.receiverList[0].email = "platfo_1255612361_per@gmail.com";
                payRequest.receiverList[1] = new Receiver();
                payRequest.receiverList[1].amount = decimal.Parse("1.00");
                payRequest.receiverList[1].email = "platfo_1255611349_biz@gmail.com";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                PResponse = ap.pay(payRequest);


                if (PResponse.responseEnvelope.ack == AckCode.Success )
                {
                    Console.WriteLine("Transaction Pay Successful! PayKey is=" + PResponse.payKey);
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction Pay  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return PResponse.payKey;
        }

        /// <summary>
        /// Calls the PaymentDetails Platform API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="profile"></param>
        public static void PaymentDetails(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.PaymentDetailsResponse paymentDetailsResponse = null;
            try
            {
                PaymentDetailsRequest paymentDetailsrequest = new PaymentDetailsRequest();
                paymentDetailsrequest.requestEnvelope = new RequestEnvelope();
                paymentDetailsrequest.requestEnvelope.errorLanguage = "en_US";
                paymentDetailsrequest.payKey = key;

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                paymentDetailsResponse = ap.paymentDetails(paymentDetailsrequest);



                if (paymentDetailsResponse.responseEnvelope.ack == AckCode.Success )
                {
                    Console.WriteLine("Transaction PaymentDetails Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction PaymentDetails  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the Refund Platform API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="profile"></param>
        public static void Refund(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.RefundResponse refundResponse = null;
            try
            {
                RefundRequest refundrequest = new RefundRequest();
                refundrequest.currencyCode = "USD";
                refundrequest.requestEnvelope = new RequestEnvelope();
                refundrequest.requestEnvelope.errorLanguage = "en_US";
                refundrequest.receiverList = new Receiver[2];
                refundrequest.receiverList[0] = new Receiver();
                refundrequest.receiverList[0].amount = decimal.Parse("1.00");
                refundrequest.receiverList[0].email = "platfo_1255612361_per@gmail.com";
                refundrequest.receiverList[1] = new Receiver();
                refundrequest.receiverList[1].amount = decimal.Parse("1.00");
                refundrequest.receiverList[1].email = "platfo_1255611349_biz@gmail.com";
                refundrequest.payKey = key;

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                refundResponse = ap.refund(refundrequest);


                if (refundResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction Refund Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction Refund  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the Preapproval Platform API
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static string Preapproval(BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.PreapprovalResponse preapprovalResponse = null;
            try
            {
                PreapprovalRequest preapprovalrequest = new PreapprovalRequest();
                preapprovalrequest.cancelUrl = "http://www.google.com";
                preapprovalrequest.returnUrl = "http://www.google.com";
                preapprovalrequest.currencyCode = "USD";
                preapprovalrequest.maxTotalAmountOfAllPayments = decimal.Parse("50.00");
                preapprovalrequest.maxTotalAmountOfAllPaymentsSpecified = true;
                preapprovalrequest.startingDate = DateTime.Today.AddDays(1);                
                preapprovalrequest.endingDate = DateTime.Today.AddDays(7);
                preapprovalrequest.endingDateSpecified = true;
                preapprovalrequest.senderEmail = "platfo_1255077030_biz@gmail.com";                
                preapprovalrequest.clientDetails = new ClientDetailsType();
                preapprovalrequest.clientDetails.applicationId = "AppID";
                preapprovalrequest.clientDetails.deviceId = "127001";
                preapprovalrequest.maxNumberOfPayments = int.Parse("10");                
                preapprovalrequest.clientDetails.ipAddress = "127.0.0.1";
                preapprovalrequest.requestEnvelope = new RequestEnvelope();
                preapprovalrequest.requestEnvelope.errorLanguage = "en_US";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                preapprovalResponse = ap.preapproval(preapprovalrequest);



                if (preapprovalResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction Preapproval Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction Preapproval  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return preapprovalResponse.preapprovalKey;
        }

        /// <summary>
        /// Calls the GetAllowedFundingSources Platform API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="profile"></param>
        public static void GetAllowedFundingSources(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse getAllowedFundingSourcesResponse = null;
            try
            {
                GetAllowedFundingSourcesRequest getAllowedFundingSourcesrequest = new GetAllowedFundingSourcesRequest();
                getAllowedFundingSourcesrequest.key = key;
                getAllowedFundingSourcesrequest.requestEnvelope = new RequestEnvelope();
                getAllowedFundingSourcesrequest.requestEnvelope.errorLanguage = "en_US";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                getAllowedFundingSourcesResponse = ap.GetAllowedFundingSources(getAllowedFundingSourcesrequest);



                if (getAllowedFundingSourcesResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction GetAllowedFundingSources Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction GetAllowedFundingSources  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the PreapprovalDetails Platform API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="profile"></param>
        public static void PreapprovalDetails(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.PreapprovalDetailsResponse preapprovaldetailsResponse = null;
            try
            {
                PreapprovalDetailsRequest preapprovaldetailsrequest = new PreapprovalDetailsRequest();
                preapprovaldetailsrequest.preapprovalKey = key;
                preapprovaldetailsrequest.requestEnvelope = new RequestEnvelope();
                preapprovaldetailsrequest.requestEnvelope.errorLanguage = "en_US";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                preapprovaldetailsResponse = ap.preapprovalDetails(preapprovaldetailsrequest);



                if (preapprovaldetailsResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction PreapprovalDetails Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction PreapprovalDetails  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the CancelPreapproval Platform API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="profile"></param>
        public static void CancelPreapproval(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.CancelPreapprovalResponse CancelPreapprovalResponse = null;
            try
            {

                profile.EndPointAppend = "AdaptivePayments/CancelPreapproval";
                CancelPreapprovalRequest CPRequest = new CancelPreapprovalRequest();
                CPRequest.preapprovalKey = key;
                CPRequest.requestEnvelope = new RequestEnvelope();
                CPRequest.requestEnvelope.errorLanguage = "en-US";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                CancelPreapprovalResponse = ap.CancelPreapproval(CPRequest);


                if (CancelPreapprovalResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction CancelPreapproval Successful!");

                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction CancelPreapproval  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the ConvertCurrency Platform API
        /// </summary>
        /// <param name="profile"></param>
        public static void ConvertCurrency(BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.ConvertCurrencyResponse ConvertCurrencyResponse = null;
            try
            {
                ConvertCurrencyRequest CCRequest = new ConvertCurrencyRequest();
                CurrencyType[] ct = new CurrencyType[2];
                ct[0] = new CurrencyType();
                ct[0].amount = decimal.Parse("1.00");
                ct[0].code = "GBP";
                ct[1] = new CurrencyType();
                ct[1].amount = decimal.Parse("100.00");
                ct[1].code = "EUR";
                CCRequest.baseAmountList = ct;
                string[] tocodes = new string[3];
                tocodes[0] = "USD";
                tocodes[1] = "CAD";
                tocodes[2] = "JPY";
                CCRequest.convertToCurrencyList = tocodes;
                CCRequest.requestEnvelope = new RequestEnvelope();
                CCRequest.requestEnvelope.errorLanguage = "en_US";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                ConvertCurrencyResponse = ap.ConvertCurrency(CCRequest);

                if (ConvertCurrencyResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction ConvertCurrency Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction ConvertCurrency  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the Pay Platform API
        /// </summary>
        /// <param name="profile">BaseAPIProfile</param>
        /// <returns>string payKey</returns>
        public static string CreatePay(BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.PayResponse PResponse = null;

            try
            {

                PayRequest payRequest = new PayRequest();
                payRequest.cancelUrl = "http://www.google.com";
                payRequest.returnUrl = "http://www.google.com";
                payRequest.senderEmail = "platfo_1255077030_biz@gmail.com";
                payRequest.clientDetails = new ClientDetailsType();
                payRequest.clientDetails.applicationId = "AppID";
                payRequest.clientDetails.deviceId = ConfigurationManager.AppSettings["deviceId"];
                payRequest.clientDetails.ipAddress = ConfigurationManager.AppSettings["ipAddress"];
                payRequest.actionType = "CREATE";
                payRequest.currencyCode = "USD";
                payRequest.requestEnvelope = new RequestEnvelope();
                payRequest.requestEnvelope.errorLanguage = "en_US";
                payRequest.receiverList = new Receiver[2];
                payRequest.receiverList[0] = new Receiver();
                payRequest.receiverList[0].amount = decimal.Parse("1.00");
                payRequest.receiverList[0].email = "platfo_1255612361_per@gmail.com";
                payRequest.receiverList[1] = new Receiver();
                payRequest.receiverList[1].amount = decimal.Parse("1.00");
                payRequest.receiverList[1].email = "platfo_1255611349_biz@gmail.com";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                PResponse = ap.pay(payRequest);

                if (PResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction CreatePay Successful! PayKey is=" + PResponse.payKey);
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction CreatePay  Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return PResponse.payKey;
        }

        ///// <summary>
        ///// Calls the PaymentDetails Platform API
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="profile"></param>
        public static void SetPaymentOption(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.SetPaymentOptionsResponse paymentOptionResponse = null;
            try
            {
                SetPaymentOptionsRequest paymentOptionRequest = new SetPaymentOptionsRequest();
                paymentOptionRequest.requestEnvelope = new RequestEnvelope();
                paymentOptionRequest.requestEnvelope.errorLanguage = "en_US";
                paymentOptionRequest.payKey = key;
                paymentOptionRequest.displayOptions = new DisplayOptions();
                //paymentOptionRequest.displayOptions.emailHeaderImageUrl = "";
                //paymentOptionRequest.displayOptions.emailMarketingImageUrl = "";
                //paymentOptionRequest.initiatingEntity = new InitiatingEntity();
                //paymentOptionRequest.initiatingEntity.institutionCustomer = new InstitutionCustomer();
                //paymentOptionRequest.initiatingEntity.institutionCustomer.countryCode ="";
                //paymentOptionRequest.initiatingEntity.institutionCustomer.email = "";
                //paymentOptionRequest.initiatingEntity.institutionCustomer.firstName = "";
                //paymentOptionRequest.initiatingEntity.institutionCustomer.institutionId = "";
                //paymentOptionRequest.initiatingEntity.institutionCustomer.institutionCustomerId = "";
                //paymentOptionRequest.initiatingEntity.institutionCustomer.lastName = "";
                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                paymentOptionResponse = ap.SetPaymentOptions(paymentOptionRequest);

                if (paymentOptionResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction SetPaymentOptions Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction SetPaymentOptions   Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the PaymentDetails Platform API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="profile"></param>
        public static void ExecutePayment(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.ExecutePaymentResponse EPResponse = null;
            try
            {
                ExecutePaymentRequest paymentRequest = new ExecutePaymentRequest();
                paymentRequest.requestEnvelope = new RequestEnvelope();
                paymentRequest.requestEnvelope.errorLanguage = "en_US";
                paymentRequest.payKey = key;

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                EPResponse = ap.ExecutePayment(paymentRequest);

                if (EPResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction ExecutePayment Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction ExecutePayment Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
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
        /// Calls the PaymentDetails Platform API
        /// </summary>
        /// <param name="key"></param>
        /// <param name="profile"></param>
        public static void GetPaymentOption(string key, BaseAPIProfile profile)
        {
            PayPal.Services.Private.AP.GetPaymentOptionsResponse GPResponse = null;
            try
            {
                GetPaymentOptionsRequest paymentRequest = new GetPaymentOptionsRequest();
                paymentRequest.requestEnvelope = new RequestEnvelope();
                paymentRequest.requestEnvelope.errorLanguage = "en_US";
                paymentRequest.payKey = key;

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile;
                GPResponse = ap.GetPaymentOptions(paymentRequest);

                if (GPResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction GetPaymentOptions Successful!");
                }
                else
                {
                    TransactionException tranactionEx = ap.LastError;
                    Console.WriteLine("Transaction GetPaymentOptions Failed:" + "CorrelationID=" + tranactionEx.CorrelationID +
                        "&" + "ErrorDetails=" + tranactionEx.ErrorDetails + "&" + "Ack=" + tranactionEx.Ack + "&" + "ErrorMessage=" + tranactionEx.Message);

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        public static void GetFundingPlans(string payKey, BaseAPIProfile profile)
        {
            GetFundingPlansRequest getFundingPlansRequest = new GetFundingPlansRequest();
            getFundingPlansRequest.requestEnvelope = new RequestEnvelope();
            getFundingPlansRequest.requestEnvelope.errorLanguage = "en_US";            
            getFundingPlansRequest.payKey = payKey;

            AdapativePayments ap = new AdapativePayments();
            ap.APIProfile = profile;

            try
            {
                GetFundingPlansResponse GFPResponse = ap.GetFundingPlans(getFundingPlansRequest);

                if (GFPResponse != null && GFPResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction GetFundingPlans Successful!");
                }
                else
                {
                    TransactionException transactionEx = ap.LastError;
                    Console.WriteLine("Transaction GetFundingPlans Failed:" + "CorrelationID=" + transactionEx.CorrelationID +
                        "&" + "Ack=" + transactionEx.Ack + "&" + "ErrorMessage=" + transactionEx.Message);
                    for (int i = 0; i < transactionEx.ErrorDetails.Length; i++)
                    {
                        Console.WriteLine("   ErrorDetails: " + transactionEx.ErrorDetails[i].message);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }



        public static void GetAvailableShippingAddresses(string key, BaseAPIProfile profile)
        {
            GetAvailableShippingAddressesRequest availableShipAddressesRequest = new GetAvailableShippingAddressesRequest();
            availableShipAddressesRequest.requestEnvelope = new RequestEnvelope();
            availableShipAddressesRequest.requestEnvelope.errorLanguage = "en_US";

            // Key can be an AdaptivePayments key such as payKey(actionType set to CREATE) or preapprovalKey
            availableShipAddressesRequest.key = key;

            AdapativePayments ap = new AdapativePayments();
            ap.APIProfile = profile;

            try
            {
                GetAvailableShippingAddressesResponse GASAResponse = ap.GetAvailableShippingAddresses(availableShipAddressesRequest);

                if (GASAResponse != null && GASAResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction GetAvailableShippingAddresses Successful!");
                }
                else
                {
                    TransactionException transactionEx = ap.LastError;
                    Console.WriteLine("Transaction GetAvailableShippingAddresses Failed:" + "CorrelationID=" + transactionEx.CorrelationID +
                        "&" + "Ack=" + transactionEx.Ack + "&" + "ErrorMessage=" + transactionEx.Message);
                    for(int i=0; i<transactionEx.ErrorDetails.Length; i++) {
                        Console.WriteLine("   ErrorDetails: " + transactionEx.ErrorDetails[i].message);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void GetShippingAddresses(string key, BaseAPIProfile profile)
        {
            GetShippingAddressesRequest shippingAddressesRequest = new GetShippingAddressesRequest();
            shippingAddressesRequest.requestEnvelope = new RequestEnvelope();
            shippingAddressesRequest.requestEnvelope.errorLanguage = "en_US";

            // Key can be an AdaptivePayments key such as payKey or preapprovalKey
            shippingAddressesRequest.key = key;

            AdapativePayments ap = new AdapativePayments();
            ap.APIProfile = profile;

            try
            {
                GetShippingAddressesResponse GSAResponse = ap.GetShippingAddresses(shippingAddressesRequest);

                if (GSAResponse != null && GSAResponse.responseEnvelope.ack == AckCode.Success)
                {
                    Console.WriteLine("Transaction GetShippingAddresses Successful!");
                }
                else
                {
                    TransactionException transactionEx = ap.LastError;
                    Console.WriteLine("Transaction GetShippingAddresses Failed:" + "CorrelationID=" + transactionEx.CorrelationID +
                        "&" + "Ack=" + transactionEx.Ack + "&" + "ErrorMessage=" + transactionEx.Message);
                    for (int i = 0; i < transactionEx.ErrorDetails.Length; i++)
                    {
                        Console.WriteLine("   ErrorDetails: " + transactionEx.ErrorDetails[i].message);
                    }
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
