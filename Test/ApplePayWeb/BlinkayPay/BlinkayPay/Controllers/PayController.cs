using Moneris;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace BlinkayPay.Controllers
{
    [RoutePrefix("pay")]
    public class PayController : ApiController
    {
        #region API methods

        [HttpGet]
        [Route("alive")]
        public string GetAlive()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new AliveResponse());
        }

        /*
        [HttpGet]
        [Route("session")]
        public async Task<String> GetSession()
        {
            String applePaySessionUrl = "https://apple-pay-gateway.apple.com/paymentservices/paymentSession";

            String certificatePath = @"C:\inetpub\https_aptest.blinkay.app\BlinkayPay\bin\certificate.pfx";
            //String certificatePath = @"C:\Integra\BlinkayPay\BlinkayPay\bin\certificate.pfx";

            //var model = new ApplePayResponse();
            //return Newtonsoft.Json.JsonConvert.SerializeObject(model);

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            
            //WebRequestHandler handler = new WebRequestHandler();

            

            ////X509Certificate2 certificate = new X509Certificate2(@"C:\inetpub\https_aptest.blinkay.app\BlinkayPay\bin\merchant_id.cer");            
            //X509Certificate2 certificate = new X509Certificate2(@"C:\Integra\BlinkayPay\BlinkayPay\bin\merchant_id.cer");

            //handler.ClientCertificates.Add(certificate);

            //HttpClient client = new HttpClient(handler);

            //AppleSessionRequestBody appleSessionRequest = new AppleSessionRequestBody();

            //String content = Newtonsoft.Json.JsonConvert.SerializeObject(appleSessionRequest);
            //StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            
            //var response = await client.PostAsync(applePaySessionUrl, stringContent);            

            //return await response.Content.ReadAsStringAsync();
            
            HttpWebRequest request = WebRequest.Create(applePaySessionUrl) as HttpWebRequest;
            request.ContentType = "application/json;charset=UTF8";
            X509Certificate2 cert = new X509Certificate2(certificatePath, "hola");
            request.ClientCertificates.Clear();
            request.ClientCertificates.Add(cert);
            request.Accept = "application/json";
            
            request.Method = WebRequestMethods.Http.Post;

            string result = null;
            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }

            return result;
        }
        */

        /// <summary>
        /// Method that is called from Moneris after GetTicketFromDataPreload operation
        /// </summary>
        /// <param name="receipt"></param>
        [HttpGet]
        [Route("monreceipt")]
        public void SetMonerisTicket(string receipt)
        {
        }

        /// <summary>
        /// Returns the Moneris Preload Ticket Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("moneris")]
        public string GetTicketFromDataPreload()
        {
            //Set call fields
            string strMonerisUrl = "https://esqa.moneris.com/gateway2/servlet/MpgRequest";
            string strStoreId = "monca05653";            
            string strOrderId = System.Guid.NewGuid().ToString();
            string strAmount = "1.00";            
            string strAPIToken = "l5yh7lcfsICXp5SZMKV8";
            string strReceiptUrl = "https://aptest.blinkay.app/BlinkayPay/pay/monreceipt";

            //Returned value
            string preloadTicket = "";

            MonerisErrorCode eErrorCode = MonerisErrorCode.InternalError;
            string errorMessage = ErrorMessageDict[eErrorCode];            

            try
            {
                //Moneris request

                string postData = string.Format(@"<request>
                       <store_id>{0}</store_id>
                       <api_token>{1}</api_token>
                       <applepay_preload>
                          <order_id>{2}</order_id>
                          <amount>{3}</amount>
                          <receipt_url>{4}</receipt_url>
                       </applepay_preload>
                    </request>               
                ", strStoreId, strAPIToken, strOrderId, strAmount, strReceiptUrl);                

                //Make the call to the server

                AddTLS12Support();

                WebRequest req = WebRequest.Create(strMonerisUrl);

                byte[] send = Encoding.Default.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = send.Length;                

                try
                {
                    Stream sout = req.GetRequestStream();
                    sout.Write(send, 0, send.Length);
                    sout.Flush();
                    sout.Close();

                    WebResponse res = req.GetResponse();
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    string returnvalue = sr.ReadToEnd();

                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(MonerisTicketPreloadResponse));
                        using (TextReader reader = new StringReader(returnvalue))
                        {
                            MonerisTicketPreloadResponse monerisResponse = (MonerisTicketPreloadResponse)serializer.Deserialize(reader);

                            try
                            {
                                if (monerisResponse != null && monerisResponse.Receipt != null) 
                                { 
                                    eErrorCode = (MonerisErrorCode)Convert.ToInt32(monerisResponse.Receipt.ResponseCode);

                                    if (eErrorCode == MonerisErrorCode.ApprovedAccountBalancesNotIncluded)
                                    {
                                        preloadTicket = monerisResponse.Receipt.PreloadTicket;
                                    }                                
                                }
                            }
                            catch
                            {
                                eErrorCode = MonerisErrorCode.InternalError;
                            }

                            errorMessage = ErrorMessageDict[eErrorCode];        
                        }
                    }

                    catch (Exception e)
                    {

                    }

                }
                catch (Exception e)
                {

                }
            }
            catch (Exception e)
            {


            }

            return preloadTicket;
        }

        #endregion

        #region Responses classes

        [Serializable]
        public class AliveResponse
        {
            public String status { get; set; }

            public AliveResponse()
            {
                status = "OK";
            }
        }

        [Serializable]
        public class AppleSessionRequestBody
        {
            public String merchantIdentifier { get; set; }
            public String displayName { get; set; }
            public String initiative { get; set; }
            public String initiativeContext { get; set; }

            public AppleSessionRequestBody()
            {
                merchantIdentifier = "merchant.com.blinkay.storedev";
                displayName = "BlinkayStore";
                initiative = "web";
                initiativeContext = "https://aptest.blinkay.app/BlinkayPay/pay/";
            }
        }

        [XmlRoot("response")]
        public class MonerisTicketPreloadResponse
        {
            [XmlElement("receipt")]
            public MonerisTicketPreloadReceipt Receipt { get; set; }
        }

        public class MonerisTicketPreloadReceipt
        {
            [XmlElement("PreloadTicket")]
            public string PreloadTicket { get; set; }

            [XmlElement("Message")]
            public string Message { get; set; }

            [XmlElement("ResponseCode")]
            public string ResponseCode { get; set; }
        }

        #endregion

        #region Moneris Enums

        public enum MonerisErrorCode
        {
            ApprovedAccountBalancesIncluded = 0,
            ApprovedAccountBalancesNotIncluded = 1,
            ApprovedCountryClub = 2,
            ApprovedMaybeMoreId = 3,
            ApprovedPendingIdSignPaperDraft = 4,
            ApprovedBlind = 5,
            ApprovedVip = 6,
            ApprovedAdministrativeTransaction = 7,
            ApprovedNationalNegFileHitOk = 8,
            ApprovedCommercial = 9,
            AmexCreditApproval1 = 23,
            Amex77CreditApproval = 24,
            AmexCreditApproval2 = 25,
            AmexCreditApproval3 = 26,
            CreditCardApproval = 27,
            VipCreditApproved = 28,
            CreditResponseAcknowledgement = 29,
            Decline = 50,
            ExpiredCard = 51,
            PinRetriesExceeded = 52,
            NoSharing = 53,
            NoSecurityModule = 54,
            InvalidTransaction = 55,
            NoSupport = 56,
            LostOrStolenCard = 57,
            InvalidStatus = 58,
            RestrictedCard = 59,
            NoSavingsAccount = 60,
            NoPbf = 61,
            PbfUpdateError = 62,
            InvalidAuthorizationType = 63,
            BadTrack2 = 64,
            AdjustmentNotAllowed = 65,
            InvalidCreditCardAdvanceIncrement = 66,
            InvalidTransactionDate = 67,
            PtlfError1 = 68,
            BadMessageError = 69,
            NoIdf = 70,
            InvalidRouteAuthorization = 71,
            CardOnNationalNegFile = 72,
            InvalidRouteServiceDestination = 73,
            UnableToAuthorize = 74,
            InvalidPanLength = 75,
            LowFunds = 76,
            PreAuthFull = 77,
            DuplicateTransaction = 78,
            MaximumOnlineRefundReached = 79,
            MaximumOfflineRefundReached = 80,
            MaximumCreditPerRefundReached = 81,
            NumberOfTimesUsedExceeded1 = 82,
            MaximumRefundCreditReached = 83,
            DuplicateTransactionAuthorizationNumberHasAlreadyBeenCorrectedByHost = 84,
            InquiryNotAllowed = 85,
            OverFloorLimit = 86,
            MaximumNumberOfRefundCreditByRetailer = 87,
            PlaceCall1 = 88,
            CafStatusInactiveOrClosed = 89,
            ReferralFileFull = 90,
            NegFileProblem1 = 91,
            AdvanceLessThanMinimum1 = 92,
            Delinquent1 = 93,
            OverTableLimit1 = 94,
            AmountOverMaximum1 = 95,
            PinRequired = 96,
            Mod10CheckFailure = 97,
            ForcePost = 98,
            BadPbf = 99,
            UnableToProcessTransaction = 100,
            PlaceCall2 = 101,
            PlaceCall3 = 102,
            NegFileProblem2 = 103,
            CafProblem1 = 104,
            CardNotSupported = 105,
            AmountOverMaximum2 = 106,
            OverDailyLimit = 107,
            CafProblem2 = 108,
            AdvanceLessThanMinimum2 = 109,
            NumberOfTimesUsedExceeded2 = 110,
            Delinquent2 = 111,
            OverTableLimit2 = 112,
            Timeout = 113,
            PtlfError2 = 115,
            AdministrationFileProblem = 121,
            UnableToValidatePinSecurityModuleDown = 122,
            MerchantNotOnFile = 150,
            InvalidAccount = 200,
            IncorrectPin = 201,
            AdvanceLessThanMinimum3 = 202,
            AdministrativeCardNeeded = 203,
            AmountOverMaximum3 = 204,
            InvalidAdvanceAmount = 205,
            CafNotFound = 206,
            InvalidTransactionDate2 = 207,
            InvalidExpirationDate = 208,
            InvalidTransactionCode = 209,
            PinKeySyncError = 210,
            DestinationNotAvailable = 212,
            ErrorOnCashAmount = 251,
            DebitNotSupported = 252,
            AmexDenial12 = 426,
            AmexInvalidMerchant = 427,
            AmexAccountError = 429,
            AmexExpiredCard = 430,
            AmexCallAmex1 = 431,
            AmexCall03 = 434,
            AmexSystemDown = 435,
            AmexCall05 = 436,
            AmexDeclined1 = 437,
            AmexDeclined2 = 438,
            AmexServiceError = 439,
            AmexCallAmex2 = 440,
            AmexAmountError = 441,
            CreditCardInvalidExpirationDate = 475,
            CreditCardInvalidTransactionRejected = 476,
            CreditCardReferCall = 477,
            CreditCardDeclinePickUpCardCall = 478,
            CreditCardDeclinePickUpCard1 = 479,
            CreditCardDeclinePickUpCard2 = 480,
            CreditCardDecline = 481,
            CreditCardExpiredCard = 482,
            CreditCardRefer = 483,
            CreditCardExpiredCardRefer = 484,
            CreditCardNotAuthorized = 485,
            CreditCardCvvCryptographicError = 486,
            CreditCardInvalidCvv1 = 487,
            CreditCardInvalidCvv2 = 489,
            CreditCardInvalidCvv3 = 490,
            BadFormat = 800,
            BadData = 801,
            InvalidClerkId = 802,
            BadClose = 809,
            SystemTimeout = 810,
            SystemError = 811,
            BadResponseLength = 821,
            InvalidPinBlock = 877,
            PinLengthError = 878,
            FinalPacketOfAMultiPacketTransaction = 880,
            IntermediatePacketOfAMultiPacketTransaction = 881,
            MacKeySyncError = 889,
            BadMacValue = 898,
            BadSequenceNumberResendTransaction = 899,
            CapturePinTriesExceeded = 900,
            CaptureExpiredCard = 901,
            CaptureNegCapture = 902,
            CaptureCafStatus3 = 903,
            CaptureAdvanceMinimum = 904,
            CaptureNumTimesUsed = 905,
            CaptureDelinquent = 906,
            CaptureOverLimitTable = 907,
            CaptureAmountOverMaximum = 908,
            CaptureCapture = 909,
            CancelledByCardHolder = 914,
            InvalidProfileId = 940,
            ErrorGeneratingToken = 941,
            InvalidProfileId2 = 942,
            CardDataIsInvalid = 943,
            InvalidExpirationTime = 944,
            InvalidCVDData = 945,
            InitializationFailureMerchantNumberMismatch = 960,
            InitializationFailurePinpadMismatch = 961,
            NoMatchOnPollCode = 963,
            NoMatchOnConcentratorId = 964,
            InvalidSoftwareVersionNumber = 965,
            DuplicateTerminalName = 966,
            TerminalClerkTableFull = 970,
            UnableToLocateMerchantCfDetails = 973,
            InvalidAmount = 977,
            FailedCfTransaction = 978,
            DataErrorOptionalFieldName = 984,
            InvalidTransaction2 = 987,
            CannotFindPrevious = 983,
            IncompleteTimedOut = 986,
            CannotFindExpiringCards = 988,
            InternalError = 1000,

        }

        static readonly public Dictionary<MonerisErrorCode, string> ErrorMessageDict = new Dictionary<MonerisErrorCode, string>()
        {
            {MonerisErrorCode.ApprovedAccountBalancesIncluded,"Approved, account balances included"},
            {MonerisErrorCode.ApprovedAccountBalancesNotIncluded,"Approved, account balances not included"},
            {MonerisErrorCode.ApprovedCountryClub,"Approved, country club"},
            {MonerisErrorCode.ApprovedMaybeMoreId,"Approved, maybe more ID"},
            {MonerisErrorCode.ApprovedPendingIdSignPaperDraft,"Approved, pending ID (sign paper draft)"},
            {MonerisErrorCode.ApprovedBlind,"Approved, blind"},
            {MonerisErrorCode.ApprovedVip,"Approved, VIP"},
            {MonerisErrorCode.ApprovedAdministrativeTransaction,"Approved, administrative transaction"},
            {MonerisErrorCode.ApprovedNationalNegFileHitOk,"Approved, national NEG file hit OK"},
            {MonerisErrorCode.ApprovedCommercial,"Approved, commercial"},
            {MonerisErrorCode.AmexCreditApproval1,"Amex - credit approval"},
            {MonerisErrorCode.Amex77CreditApproval,"Amex 77 - credit approval"},
            {MonerisErrorCode.AmexCreditApproval2,"Amex - credit approval"},
            {MonerisErrorCode.AmexCreditApproval3,"Amex - credit approval"},
            {MonerisErrorCode.CreditCardApproval,"Credit card approval"},
            {MonerisErrorCode.VipCreditApproved,"VIP Credit Approved"},
            {MonerisErrorCode.CreditResponseAcknowledgement,"Credit Response Acknowledgement"},
            {MonerisErrorCode.Decline,"Decline"},
            {MonerisErrorCode.ExpiredCard,"Expired Card"},
            {MonerisErrorCode.PinRetriesExceeded,"PIN retries exceeded"},
            {MonerisErrorCode.NoSharing,"No sharing"},
            {MonerisErrorCode.NoSecurityModule,"No security module"},
            {MonerisErrorCode.InvalidTransaction,"Invalid transaction"},
            {MonerisErrorCode.NoSupport,"No Support"},
            {MonerisErrorCode.LostOrStolenCard,"Lost or stolen card"},
            {MonerisErrorCode.InvalidStatus,"Invalid status"},
            {MonerisErrorCode.RestrictedCard,"Restricted Card"},
            {MonerisErrorCode.NoSavingsAccount,"No Savings account"},
            {MonerisErrorCode.NoPbf,"No PBF"},
            {MonerisErrorCode.PbfUpdateError,"PBF update error"},
            {MonerisErrorCode.InvalidAuthorizationType,"Invalid authorization type"},
            {MonerisErrorCode.BadTrack2,"Bad Track 2"},
            {MonerisErrorCode.AdjustmentNotAllowed,"Adjustment not allowed"},
            {MonerisErrorCode.InvalidCreditCardAdvanceIncrement,"Invalid credit card advance increment"},
            {MonerisErrorCode.InvalidTransactionDate,"Invalid transaction date"},
            {MonerisErrorCode.PtlfError1,"PTLF error"},
            {MonerisErrorCode.BadMessageError,"Bad message error"},
            {MonerisErrorCode.NoIdf,"No IDF"},
            {MonerisErrorCode.InvalidRouteAuthorization,"Invalid route authorization"},
            {MonerisErrorCode.CardOnNationalNegFile,"Card on National NEG file"},
            {MonerisErrorCode.InvalidRouteServiceDestination,"Invalid route service (destination)"},
            {MonerisErrorCode.UnableToAuthorize,"Unable to authorize"},
            {MonerisErrorCode.InvalidPanLength,"Invalid PAN length"},
            {MonerisErrorCode.LowFunds,"Low funds"},
            {MonerisErrorCode.PreAuthFull,"Pre-auth full"},
            {MonerisErrorCode.DuplicateTransaction,"Duplicate transaction"},
            {MonerisErrorCode.MaximumOnlineRefundReached,"Maximum online refund reached"},
            {MonerisErrorCode.MaximumOfflineRefundReached,"Maximum offline refund reached"},
            {MonerisErrorCode.MaximumCreditPerRefundReached,"Maximum credit per refund reached"},
            {MonerisErrorCode.NumberOfTimesUsedExceeded1,"Number of times used exceeded"},
            {MonerisErrorCode.MaximumRefundCreditReached,"Maximum refund credit reached"},
            {MonerisErrorCode.DuplicateTransactionAuthorizationNumberHasAlreadyBeenCorrectedByHost,"Duplicate transaction - authorization number has already been corrected by host."},
            {MonerisErrorCode.InquiryNotAllowed,"Inquiry not allowed"},
            {MonerisErrorCode.OverFloorLimit,"Over floor limit"},
            {MonerisErrorCode.MaximumNumberOfRefundCreditByRetailer,"Maximum number of refund credit by retailer"},
            {MonerisErrorCode.PlaceCall1,"Place call"},
            {MonerisErrorCode.CafStatusInactiveOrClosed,"CAF status inactive or closed"},
            {MonerisErrorCode.ReferralFileFull,"Referral file full"},
            {MonerisErrorCode.NegFileProblem1,"NEG file problem"},
            {MonerisErrorCode.AdvanceLessThanMinimum1,"Advance less than minimum"},
            {MonerisErrorCode.Delinquent1,"Delinquent"},
            {MonerisErrorCode.OverTableLimit1,"Over table limit"},
            {MonerisErrorCode.AmountOverMaximum1,"Amount over maximum"},
            {MonerisErrorCode.PinRequired,"PIN required"},
            {MonerisErrorCode.Mod10CheckFailure,"Mod 10 check failure"},
            {MonerisErrorCode.ForcePost,"Force Post"},
            {MonerisErrorCode.BadPbf,"Bad PBF"},
            {MonerisErrorCode.UnableToProcessTransaction,"Unable to process transaction"},
            {MonerisErrorCode.PlaceCall2,"Place call"},
            {MonerisErrorCode.PlaceCall3,"Place call"},
            {MonerisErrorCode.NegFileProblem2,"NEG file problem"},
            {MonerisErrorCode.CafProblem1,"CAF problem"},
            {MonerisErrorCode.CardNotSupported,"Card not supported"},
            {MonerisErrorCode.AmountOverMaximum2,"Amount over maximum"},
            {MonerisErrorCode.OverDailyLimit,"Over daily limit"},
            {MonerisErrorCode.CafProblem2,"CAF Problem"},
            {MonerisErrorCode.AdvanceLessThanMinimum2,"Advance less than minimum"},
            {MonerisErrorCode.NumberOfTimesUsedExceeded2,"Number of times used exceeded"},
            {MonerisErrorCode.Delinquent2,"Delinquent"},
            {MonerisErrorCode.OverTableLimit2,"Over table limit"},
            {MonerisErrorCode.Timeout,"Timeout"},
            {MonerisErrorCode.PtlfError2,"PTLF error"},
            {MonerisErrorCode.AdministrationFileProblem,"Administration file problem"},
            {MonerisErrorCode.UnableToValidatePinSecurityModuleDown,"Unable to validate PIN: security module down"},
            {MonerisErrorCode.MerchantNotOnFile,"Merchant not on file"},
            {MonerisErrorCode.InvalidAccount,"Invalid account"},
            {MonerisErrorCode.IncorrectPin,"Incorrect PIN"},
            {MonerisErrorCode.AdvanceLessThanMinimum3,"Advance less than minimum"},
            {MonerisErrorCode.AdministrativeCardNeeded,"Administrative card needed"},
            {MonerisErrorCode.AmountOverMaximum3,"Amount over maximum"},
            {MonerisErrorCode.InvalidAdvanceAmount,"Invalid Advance amount"},
            {MonerisErrorCode.CafNotFound,"CAF not found"},
            {MonerisErrorCode.InvalidTransactionDate2,"Invalid transaction date"},
            {MonerisErrorCode.InvalidExpirationDate,"Invalid expiration date"},
            {MonerisErrorCode.InvalidTransactionCode,"Invalid transaction code"},
            {MonerisErrorCode.PinKeySyncError,"PIN key sync error"},
            {MonerisErrorCode.DestinationNotAvailable,"Destination not available"},
            {MonerisErrorCode.ErrorOnCashAmount,"Error on cash amount"},
            {MonerisErrorCode.DebitNotSupported,"Debit not supported"},
            {MonerisErrorCode.AmexDenial12,"AMEX - Denial 12"},
            {MonerisErrorCode.AmexInvalidMerchant,"AMEX - Invalid merchant"},
            {MonerisErrorCode.AmexAccountError,"AMEX - Account error"},
            {MonerisErrorCode.AmexExpiredCard,"AMEX - Expired card"},
            {MonerisErrorCode.AmexCallAmex1,"AMEX - Call Amex"},
            {MonerisErrorCode.AmexCall03,"AMEX - Call 03"},
            {MonerisErrorCode.AmexSystemDown,"AMEX - System down"},
            {MonerisErrorCode.AmexCall05,"AMEX - Call 05"},
            {MonerisErrorCode.AmexDeclined1,"AMEX - Declined"},
            {MonerisErrorCode.AmexDeclined2,"AMEX - Declined"},
            {MonerisErrorCode.AmexServiceError,"AMEX - Service error"},
            {MonerisErrorCode.AmexCallAmex2,"AMEX - Call Amex"},
            {MonerisErrorCode.AmexAmountError,"AMEX - Amount error"},
            {MonerisErrorCode.CreditCardInvalidExpirationDate,"CREDIT CARD - Invalid expiration date"},
            {MonerisErrorCode.CreditCardInvalidTransactionRejected,"CREDIT CARD - Invalid transaction, rejected"},
            {MonerisErrorCode.CreditCardReferCall,"CREDIT CARD - Refer Call"},
            {MonerisErrorCode.CreditCardDeclinePickUpCardCall,"CREDIT CARD - Decline, Pick up card, Call"},
            {MonerisErrorCode.CreditCardDeclinePickUpCard1,"CREDIT CARD - Decline, Pick up card"},
            {MonerisErrorCode.CreditCardDeclinePickUpCard2,"CREDIT CARD - Decline, Pick up card"},
            {MonerisErrorCode.CreditCardDecline,"CREDIT CARD - Decline"},
            {MonerisErrorCode.CreditCardExpiredCard,"CREDIT CARD - Expired Card"},
            {MonerisErrorCode.CreditCardRefer,"CREDIT CARD - Refer"},
            {MonerisErrorCode.CreditCardExpiredCardRefer,"CREDIT CARD - Expired card - refer"},
            {MonerisErrorCode.CreditCardNotAuthorized,"CREDIT CARD - Not authorized"},
            {MonerisErrorCode.CreditCardCvvCryptographicError,"CREDIT CARD - CVV Cryptographic error"},
            {MonerisErrorCode.CreditCardInvalidCvv1,"CREDIT CARD - Invalid CVV"},
            {MonerisErrorCode.CreditCardInvalidCvv2,"CREDIT CARD - Invalid CVV"},
            {MonerisErrorCode.CreditCardInvalidCvv3,"CREDIT CARD - Invalid CVV"},
            {MonerisErrorCode.BadFormat,"Bad format"},
            {MonerisErrorCode.BadData,"Bad data"},
            {MonerisErrorCode.InvalidClerkId,"Invalid Clerk ID"},
            {MonerisErrorCode.BadClose,"Bad close"},
            {MonerisErrorCode.SystemTimeout,"System timeout"},
            {MonerisErrorCode.SystemError,"System error"},
            {MonerisErrorCode.BadResponseLength,"Bad response length"},
            {MonerisErrorCode.InvalidPinBlock,"Invalid PIN block"},
            {MonerisErrorCode.PinLengthError,"PIN length error"},
            {MonerisErrorCode.FinalPacketOfAMultiPacketTransaction,"Final packet of a multi-packet transaction"},
            {MonerisErrorCode.IntermediatePacketOfAMultiPacketTransaction,"Intermediate packet of a multi-packet transaction"},
            {MonerisErrorCode.MacKeySyncError,"MAC key sync error"},
            {MonerisErrorCode.BadMacValue,"Bad MAC value"},
            {MonerisErrorCode.BadSequenceNumberResendTransaction,"Bad sequence number - resend transaction"},
            {MonerisErrorCode.CapturePinTriesExceeded,"Capture - PIN Tries Exceeded"},
            {MonerisErrorCode.CaptureExpiredCard,"Capture - Expired Card"},
            {MonerisErrorCode.CaptureNegCapture,"Capture - NEG Capture"},
            {MonerisErrorCode.CaptureCafStatus3,"Capture - CAF Status 3"},
            {MonerisErrorCode.CaptureAdvanceMinimum,"Capture - Advance < Minimum"},
            {MonerisErrorCode.CaptureNumTimesUsed,"Capture - Num Times Used"},
            {MonerisErrorCode.CaptureDelinquent,"Capture - Delinquent"},
            {MonerisErrorCode.CaptureOverLimitTable,"Capture - Over Limit Table"},
            {MonerisErrorCode.CaptureAmountOverMaximum,"Capture - Amount Over Maximum"},
            {MonerisErrorCode.CaptureCapture,"Capture - Capture"},
            {MonerisErrorCode.CancelledByCardHolder,"Cancelled By Card Holder"},
            { MonerisErrorCode.InvalidProfileId, "Invalid profile id (on tokenization request)" },            
            {MonerisErrorCode.ErrorGeneratingToken,"Error generating token"},            
            {MonerisErrorCode.InvalidProfileId2,"Invalid Profile ID, or source URL"},            
            {MonerisErrorCode.CardDataIsInvalid,"Card data is invalid (not numeric, fails mod10, we will remove spaces)"},            
            {MonerisErrorCode.InvalidExpirationTime,"Invalid expiration date (mmyy, must be current month or in the future)"},            
            {MonerisErrorCode.InvalidCVDData,"Invalid CVD data (not 3-4 digits)"},            
            {MonerisErrorCode.InitializationFailureMerchantNumberMismatch,"Initialization failure - merchant number mismatch"},
            {MonerisErrorCode.InitializationFailurePinpadMismatch,"Initialization failure -pinpad mismatch"},
            {MonerisErrorCode.NoMatchOnPollCode,"No match on Poll code"},
            {MonerisErrorCode.NoMatchOnConcentratorId,"No match on Concentrator ID"},
            {MonerisErrorCode.InvalidSoftwareVersionNumber,"Invalid software version number"},
            {MonerisErrorCode.DuplicateTerminalName,"Duplicate terminal name"},
            {MonerisErrorCode.TerminalClerkTableFull,"Terminal/Clerk table full"},
            {MonerisErrorCode.UnableToLocateMerchantCfDetails,"Unable to locate merchant CF details"},
            {MonerisErrorCode.InvalidAmount,"Invalid amount"},
            {MonerisErrorCode.FailedCfTransaction,"Failed CF transaction"},
            {MonerisErrorCode.DataErrorOptionalFieldName,"Data error: (optional: field name)"},
            {MonerisErrorCode.CannotFindPrevious,"Cannot find previous"},
            {MonerisErrorCode.IncompleteTimedOut,"Incomplete: timed out"},
            {MonerisErrorCode.InvalidTransaction2,"Invalid transaction"},
            {MonerisErrorCode.CannotFindExpiringCards,"Cannot find expiring cards"},  
            {MonerisErrorCode.InternalError,"Internal error."},
        };

        public static bool IsError(MonerisErrorCode eErrorCode)
        {
            return Convert.ToInt32(eErrorCode) >= 50;
        }

        #endregion

        #region Helper methods

        private static void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }

        #endregion
    }
}
