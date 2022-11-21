using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using Moneris;

namespace integraMobile.Infrastructure
{
    public class MonerisPayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(MonerisPayments));
        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));

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
            CreditCardInvalidCvv3= 490,
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
            CancelledByCardHolder=914,
            InvalidProfileId=940,
            ErrorGeneratingToken=941,
            InvalidProfileId2=942,
            CardDataIsInvalid=943,
            InvalidExpirationTime=944,
            InvalidCVDData=945,
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



        public bool GetTicketFromDataPreload(string strUrl,
                                       string strStoreId,
                                       string strHPPKey,
                                       string strOrderId,
                                       string strAmount,
                                       string strEmail,       
                                       string strLang,
                                       string strURLReferer,         
                                      out MonerisErrorCode eErrorCode,
                                      out string errorMessage,
                                      out string strHPPID,
                                      out string strTicket)
        {
            bool bRes = false;
            strTicket = null;
            strHPPID = null;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];


            try
            {

                AddTLS12Support();

                WebRequest req = WebRequest.Create(strUrl);
                string postData = string.Format("ps_store_id={0}&hpp_key={1}&hpp_preload=&charge_total={2}&order_id={3}&email={4}&lang={5}", strStoreId, strHPPKey, strAmount, strOrderId, strEmail,strLang);

                m_Log.LogMessage(LogLevels.logINFO, string.Format("MonerisPayments.GetTicketFromDataPreload: {0}?{1}", strUrl, postData));

                byte[] send = Encoding.Default.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = send.Length;

                (req as HttpWebRequest).Referer = strURLReferer;
                (req as HttpWebRequest).Headers.Add("Origin", strURLReferer);
               
                try
                {
                    Stream sout = req.GetRequestStream();
                    sout.Write(send, 0, send.Length);
                    sout.Flush();
                    sout.Close();

                    WebResponse res = req.GetResponse();
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    string returnvalue = sr.ReadToEnd();

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("MonerisPayments.GetTicketFromDataPreload: {0} ", returnvalue));


                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(returnvalue);

                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        foreach (XmlNode n in doc.SelectNodes("/response/*"))
                        {
                            dict[n.Name] = n.InnerText;
                        }

                        m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.GetTicketFromDataPreload.Data: ");
                        m_Log.LogMessage(LogLevels.logINFO, "hpp_id = " + dict["hpp_id"]);
                        m_Log.LogMessage(LogLevels.logINFO, "ticket = " + dict["ticket"]);
                        m_Log.LogMessage(LogLevels.logINFO, "order_id = " + dict["order_id"]);
                        m_Log.LogMessage(LogLevels.logINFO, "response_code = " + dict["response_code"]);


                        try
                        {
                            eErrorCode = (MonerisErrorCode)Convert.ToInt32(dict["response_code"]);
                        }
                        catch
                        {
                            eErrorCode = MonerisErrorCode.InternalError;
                        }
                       
                        errorMessage = ErrorMessageDict[eErrorCode];

                        if (!IsError(eErrorCode))
                        {
                            strHPPID=dict["hpp_id"];
                            strTicket=dict["ticket"];
                        }

                        bRes = true;

                    }

                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTicketFromDataPreload: ", e);
                    }

                  

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTicketFromDataPreload: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTicketFromDataPreload: ", e);

            }

            return bRes;
        }





        public bool GetTokenFromTransaction(string strStoreId,
                                       string strAPIToken,
                                       string strOrderId,
                                       string strTransaction,
                                       string strEmail,           
                                       string strProcesingCountryCode,
                                       bool bTestMode,    
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strCardReference    )
        {
            bool bRes = false;
            strCardReference = null;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];


            try
            {

                AddTLS12Support();

                string processing_country_code = strProcesingCountryCode;
                bool status_check = false;


                ResTokenizeCC resTokenizeCC = new ResTokenizeCC();
                resTokenizeCC.SetOrderId(strOrderId);
                resTokenizeCC.SetTxnNumber(strTransaction);
                resTokenizeCC.SetEmail(strEmail);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(processing_country_code);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resTokenizeCC);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.GetTokenFromTransaction.Receipt: "); 
                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());

                    //ResolveData
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResDataEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strCardReference = receipt.GetDataKey();
                    }

                    bRes = true;
                   
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e); 
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e); 

            }

            return bRes;
        }



        public bool CardVerification(string strStoreId,
                                       string strAPIToken,
                                       string strOrderId,
                                       string strTemporaryToken,
                                       string strProcesingCountryCode,
                                       string strEmail,
                                       bool bTestMode,
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strIssuerId,
                                       out string strTransaction,
                                       out string strAuthCode,
                                       out string strAuthResult,
                                       out string strDateTime,
                                       out string strPAN,
                                       out string strCardScheme,
                                       out string strExpDate)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strTransaction = "";
            strAuthCode = "";
            strAuthResult = "";
            strDateTime = "";
            strPAN="";
            strCardScheme="";
            strExpDate = "";
            strIssuerId ="";

            try
            {

                AddTLS12Support();

                string processing_country_code = strProcesingCountryCode;
                bool status_check = false;
                string crypt_type = "7";

                CofInfo cof = new CofInfo();
                cof.SetPaymentIndicator("C");
                cof.SetPaymentInformation("0");
                cof.SetIssuerId("");

                ResCardVerificationCC rescardverify = new ResCardVerificationCC();
                rescardverify.SetDataKey(strTemporaryToken);
                rescardverify.SetOrderId(strOrderId);
                rescardverify.SetCustId(strEmail);
                rescardverify.SetCryptType(crypt_type);
                rescardverify.SetCofInfo(cof);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(processing_country_code);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(rescardverify);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.CardVerification.Receipt: ");
                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "Issuer ID = " + receipt.GetIssuerId());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());

                    //ResolveData
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResDataEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Card Type = " + receipt.GetCardType());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strIssuerId = receipt.GetIssuerId();
                        strAuthCode = receipt.GetAuthCode();
                        strAuthResult = receipt.GetReferenceNum();
                        strDateTime = receipt.GetTransDate() + " " + receipt.GetTransTime();
                        strTransaction = receipt.GetTxnNumber();
                        strPAN = receipt.GetResDataMaskedPan();
                        strExpDate = receipt.GetResDataExpdate();
                        strCardScheme = receipt.GetCardType();



                    }

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);

            }

            return bRes;
        }


        public bool GetPermanentToken(string strStoreId,
                               string strAPIToken,
                               string strTemporaryToken,
                               string strIssuerId,
                               string strProcesingCountryCode,
                               string strEmail,
                               string strExpDate,
                               bool bTestMode,
                               out MonerisErrorCode eErrorCode,
                               out string errorMessage,
                               out string strCardReference)
        {
            bool bRes = false;
            strCardReference = null;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            try
            {

                AddTLS12Support();

                string processing_country_code = strProcesingCountryCode;
                bool status_check = false;
                string crypt_type = "7";

                CofInfo cof = new CofInfo();
                cof.SetIssuerId(strIssuerId);
                cof.SetPaymentIndicator("Z");
                cof.SetPaymentInformation("2");


                ResAddToken resaddtoken = new ResAddToken();
                resaddtoken.SetDataKey(strTemporaryToken);
                resaddtoken.SetCustId(strEmail);
                resaddtoken.SetEmail(strEmail);
                resaddtoken.SetCryptType(crypt_type);
                resaddtoken.SetExpDate(strExpDate);
                resaddtoken.SetCofInfo(cof);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(processing_country_code);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resaddtoken);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.GetTokenFromTransaction.Receipt: ");
                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "Issuer ID = " + receipt.GetIssuerId());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());

                    m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strCardReference = receipt.GetDataKey();
                    }

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.GetTokenFromTransaction: ", e);

            }

            return bRes;
        }







        public bool AutomaticTransaction(string strStoreId,
                                       string strAPIToken,
                                       string strOrderId,
                                       string strCardReference,
                                       string strIssuerId,
                                       string strAmount,
                                       string strProcesingCountryCode,
                                       string strDescriptor,
                                       bool bStatusCheck,
                                       bool bTestMode,
                                       string strECI,
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strTransaction,
                                       out string  strAuthCode,
                                       out string strAuthResult,
                                       out string strDateTime)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strTransaction="";
            strAuthCode="";
            strAuthResult="";
            strDateTime="";


            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.AutomaticTransaction: Starting ");

                AddTLS12Support();

                if (bTestMode)
                    bStatusCheck = false;

                string crypt_type = "7";      
                if (!string.IsNullOrEmpty(strECI))
                {
                    crypt_type=strECI;
                }

                CofInfo cof=null;
                if (!string.IsNullOrEmpty(strIssuerId))
                {
                    cof = new CofInfo();
                    cof.SetIssuerId(strIssuerId);
                    cof.SetPaymentIndicator("Z");
                    cof.SetPaymentInformation("2");

                }

                ResPurchaseCC resPurchaseCC = new ResPurchaseCC();
                resPurchaseCC.SetDataKey(strCardReference);
                resPurchaseCC.SetOrderId(strOrderId);
                if (bTestMode)
                    resPurchaseCC.SetAmount("1.00");
                else
                    resPurchaseCC.SetAmount(strAmount);

                resPurchaseCC.SetCryptType(crypt_type);
                if (!string.IsNullOrEmpty(strDescriptor))
                    resPurchaseCC.SetDynamicDescriptor(strDescriptor);

                if (cof != null)
                {
                    resPurchaseCC.SetCofInfo(cof);
                }

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resPurchaseCC);
                mpgReq.SetStatusCheck(bStatusCheck);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();


                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.AutomaticPayment.Receipt: ");

                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "IssuerID = " + receipt.GetIssuerId());
                    m_Log.LogMessage(LogLevels.logINFO, "ReceiptId = " + receipt.GetReceiptId());
                    m_Log.LogMessage(LogLevels.logINFO, "ReferenceNum = " + receipt.GetReferenceNum());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "AuthCode = " + receipt.GetAuthCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "TransType = " + receipt.GetTransType());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TransAmount = " + receipt.GetTransAmount());
                    m_Log.LogMessage(LogLevels.logINFO, "CardType = " + receipt.GetCardType());
                    m_Log.LogMessage(LogLevels.logINFO, "TxnNumber = " + receipt.GetTxnNumber());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());
                    m_Log.LogMessage(LogLevels.logINFO, "IsVisaDebit = " + receipt.GetIsVisaDebit());
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResDataEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "Masked Pan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());
                 

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strAuthCode = receipt.GetAuthCode();
                        strAuthResult = receipt.GetReferenceNum();
                        strDateTime = receipt.GetTransDate() + " " + receipt.GetTransTime();
                        strTransaction = receipt.GetTxnNumber();
                    }


                   
                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransaction: ", e);

            }

            return bRes;
        }

        public bool AutomaticTransactionMPIStep1(string strStoreId,
                                       string strAPIToken,
                                       string strOrderId,
                                       string strCardReference,
                                       string strIssuerId,
                                       string strAmount,
                                       string strProcesingCountryCode, 
                                       string strDescriptor, 
                                       string strCardScheme,
                                       DateTime strExpDate,
                                       string strMerchantURL,
                                       string strUserAgent,
                                       bool bStatusCheck,
                                       bool bTestMode,
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strTransaction,
                                       out string strAuthCode,
                                       out string strAuthResult,
                                       out string strDateTime,
                                       out string strInlineForm,
                                       out string strMD)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strTransaction = "";
            strAuthCode = "";
            strAuthResult = "";
            strDateTime = "";
            strInlineForm = "";
            strMD = "";


            try
            {

                m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.AutomaticTransactionMPIStep1: Starting ");

                AddTLS12Support();

                if (bTestMode)
                    bStatusCheck = false;               

                Moneris.ResMpiTxn resMPITxn = new ResMpiTxn();
                resMPITxn.SetDataKey(strCardReference);
                               

                resMPITxn.SetXid(strOrderId);
                if (bTestMode)
                    resMPITxn.SetAmount("1.00");
                else
                    resMPITxn.SetAmount(strAmount);


                strMD = strOrderId + "mycardinfo" + strAmount;
                resMPITxn.SetMD(strMD);
                resMPITxn.SetMerchantUrl(strMerchantURL);
                resMPITxn.SetAccept("true");
                resMPITxn.SetUserAgent(strUserAgent);
                resMPITxn.SetExpDate(strExpDate.ToString("yyMM"));
                resMPITxn.SetProcCountryCode(strProcesingCountryCode);

                
                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resMPITxn);
                mpgReq.SetStatusCheck(bStatusCheck);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.AutomaticTransactionMPIStep1.Receipt: ");


                    m_Log.LogMessage(LogLevels.logINFO, "MpiMessage = " + receipt.GetMpiMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "MpiSuccess = " + receipt.GetMpiSuccess());

                    if (receipt.GetMpiSuccess() == "true")
                    {

                        m_Log.LogMessage(LogLevels.logINFO, receipt.GetInLineForm());
                        strInlineForm = receipt.GetInLineForm();
                        bRes = true;
                      
                        eErrorCode = MonerisErrorCode.ApprovedAccountBalancesIncluded;

                        errorMessage = receipt.GetMessage();

                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            errorMessage = ErrorMessageDict[eErrorCode];
                        }                       

                    }
                    else
                    {
                        string strECI = "7"; //error or mpimessage="U" or (mpimessage"N" and "AMEX")

                        if ((receipt.GetMpiMessage() == "N") && (strCardScheme != "A"))
                        {
                                strECI = "6";
                        }


                        bRes = AutomaticTransaction(strStoreId,
                                       strAPIToken,
                                       strOrderId,
                                       strCardReference,
                                       strIssuerId,
                                       strAmount,
                                       strProcesingCountryCode,
                                       strDescriptor,
                                       bStatusCheck,
                                       bTestMode,
                                       strECI,
                                       out eErrorCode,
                                       out errorMessage,
                                       out strTransaction,
                                       out strAuthCode,
                                       out strAuthResult,
                                       out strDateTime);

                    }

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransactionMPIStep1: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransactionMPIStep1: ", e);

            }

            return bRes;
        }


        public bool AutomaticTransactionMPIStep2(string strStoreId,
                                       string strAPIToken,
                                       string strProcesingCountryCode,
                                       string strPaRes,
                                       string strMD,
                                       bool bStatusCheck,
                                       bool bTestMode,
                                       out MonerisErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strCAVV,
                                       out string strECI)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strCAVV = "";
            strECI = "";

            try
            {

                AddTLS12Support();

                if (bTestMode)
                    bStatusCheck = false;

             
                        
                MpiAcs resMpiAcs = new MpiAcs();
                resMpiAcs.SetPaRes(strPaRes);
                resMpiAcs.SetMD(strMD);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resMpiAcs);
                mpgReq.SetStatusCheck(bStatusCheck);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();

                    m_Log.LogMessage(LogLevels.logINFO, "MpiMessage = " + receipt.GetMpiMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "MpiSuccess = " + receipt.GetMpiSuccess());

                    if (receipt.GetMpiSuccess() == "true")
                    {
                        strCAVV = receipt.GetMpiCavv();
                        strECI = receipt.GetMpiEci();
                        m_Log.LogMessage(LogLevels.logINFO, "Cavv = " + strCAVV );
                        m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + strECI);

                        eErrorCode = MonerisErrorCode.ApprovedAccountBalancesIncluded;

                        errorMessage = receipt.GetMessage();

                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            errorMessage = ErrorMessageDict[eErrorCode];
                        }                       

                        bRes = true;

                    }
                    else
                    {
                        try
                        {
                            eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                        }
                        catch
                        {
                            eErrorCode = MonerisErrorCode.InternalError;
                        }

                        errorMessage = receipt.GetMessage();

                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            errorMessage = ErrorMessageDict[eErrorCode];
                        }
                        m_Log.LogMessage(LogLevels.logINFO, "Result = " + eErrorCode.ToString());
                        m_Log.LogMessage(LogLevels.logINFO, "Message = " + errorMessage);
                    }

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransactionMPIStep2: ", e);

                }
                       
              
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransactionMPIStep2: ", e);

            }

            return bRes;
        }

        public bool AutomaticTransactionMPIStep3(string strStoreId,
                                      string strAPIToken,
                                      string strOrderId,
                                      string strCardReference,
                                      string strIssuerId,
                                      string strAmount,
                                      string strProcesingCountryCode,
                                      string strDescriptor,
                                      bool bStatusCheck,
                                      bool bTestMode,
                                      string strCAVV,
                                      string strECI,
                                      out MonerisErrorCode eErrorCode,
                                      out string errorMessage,
                                      out string strTransaction,
                                      out string strAuthCode,
                                      out string strAuthResult,
                                      out string strDateTime)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strTransaction = "";
            strAuthCode = "";
            strAuthResult = "";
            strDateTime = "";


            try
            {

                AddTLS12Support();

                if (bTestMode)
                    bStatusCheck = false;

                string crypt_type = strECI;


                if (string.IsNullOrEmpty(crypt_type))
                {
                    crypt_type = "5";
                }

                ResCavvPurchaseCC resPurchaseCC = new ResCavvPurchaseCC();
                resPurchaseCC.SetDataKey(strCardReference);
                resPurchaseCC.SetOrderId(strOrderId);
                if (bTestMode)
                    resPurchaseCC.SetAmount("1.00");
                else
                    resPurchaseCC.SetAmount(strAmount);

/*                CvdInfo cvdCheck = new CvdInfo();
                cvdCheck.SetCvdIndicator("1");
                cvdCheck.SetCvdValue("111");
                resPurchaseCC.SetCvdInfo(cvdCheck);*/


                CofInfo cof = null;
                if (!string.IsNullOrEmpty(strIssuerId))
                {
                    cof = new CofInfo();
                    cof.SetIssuerId(strIssuerId);
                    cof.SetPaymentIndicator("Z");
                    cof.SetPaymentInformation("2");

                }

                resPurchaseCC.SetCryptType(crypt_type);
                resPurchaseCC.SetCavv(strCAVV);

                if (cof != null)
                {
                    resPurchaseCC.SetCofInfo(cof);
                }
                    
                if (!string.IsNullOrEmpty(strDescriptor))
                    resPurchaseCC.SetDynamicDescriptor(strDescriptor);
                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resPurchaseCC);
                mpgReq.SetStatusCheck(bStatusCheck);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();


                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.AutomaticTransactionMPIStep3.Receipt: ");

                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
                    m_Log.LogMessage(LogLevels.logINFO, "IssuerID = " + receipt.GetIssuerId());
                    m_Log.LogMessage(LogLevels.logINFO, "ReceiptId = " + receipt.GetReceiptId());
                    m_Log.LogMessage(LogLevels.logINFO, "ReferenceNum = " + receipt.GetReferenceNum());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "AuthCode = " + receipt.GetAuthCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "TransType = " + receipt.GetTransType());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TransAmount = " + receipt.GetTransAmount());
                    m_Log.LogMessage(LogLevels.logINFO, "CardType = " + receipt.GetCardType());
                    m_Log.LogMessage(LogLevels.logINFO, "TxnNumber = " + receipt.GetTxnNumber());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
                    m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
                    m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());
                    m_Log.LogMessage(LogLevels.logINFO, "IsVisaDebit = " + receipt.GetIsVisaDebit());
                    m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResDataEmail());
                    m_Log.LogMessage(LogLevels.logINFO, "Masked Pan = " + receipt.GetResDataMaskedPan());
                    m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResDataExpdate());
                    m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResDataCryptType());


                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }

                    if (!IsError(eErrorCode))
                    {
                        strAuthCode = receipt.GetAuthCode();
                        strAuthResult = receipt.GetReferenceNum();
                        strDateTime = receipt.GetTransDate() + " " + receipt.GetTransTime();
                        strTransaction = receipt.GetTxnNumber();
                    }



                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransactionMPIStep3: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.AutomaticTransactionMPIStep3: ", e);

            }

            return bRes;
        }

        public bool RefundTransaction(string strStoreId,
                                      string strAPIToken,
                                      string strOrderId,
                                      string strTransaction,
                                      string strAmount,
                                      string strProcesingCountryCode,
                                      bool bTestMode,
                                      out MonerisErrorCode eErrorCode,
                                      out string errorMessage,
                                      out string strRefundTransaction,
                                      out string  strAuthCode,
                                      out string strAuthResult,
                                      out string strDateTime)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strRefundTransaction = "";
            strAuthCode = "";
            strAuthResult = "";
            strDateTime = "";

            try
            {

                AddTLS12Support();

                bool status_check = false;
                string crypt_type = "7";

                Refund refund = new Refund();
                refund.SetTxnNumber(strTransaction);
                refund.SetOrderId(strOrderId);

                if (bTestMode)
                    refund.SetAmount("1.00");
                else
                    refund.SetAmount(strAmount);
               
                refund.SetCryptType(crypt_type);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(refund);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();


                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.RefundTransaction.Receipt: ");
                    
                    m_Log.LogMessage(LogLevels.logINFO, "CardType = " + receipt.GetCardType());
                    m_Log.LogMessage(LogLevels.logINFO, "TransAmount = " + receipt.GetTransAmount());
                    m_Log.LogMessage(LogLevels.logINFO, "TxnNumber = " + receipt.GetTxnNumber());
                    m_Log.LogMessage(LogLevels.logINFO, "ReceiptId = " + receipt.GetReceiptId());
                    m_Log.LogMessage(LogLevels.logINFO, "TransType = " + receipt.GetTransType());
                    m_Log.LogMessage(LogLevels.logINFO, "ReferenceNum = " + receipt.GetReferenceNum());
                    m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
                    m_Log.LogMessage(LogLevels.logINFO, "ISO = " + receipt.GetISO());
                    m_Log.LogMessage(LogLevels.logINFO, "BankTotals = " + receipt.GetBankTotals());
                    m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
                    m_Log.LogMessage(LogLevels.logINFO, "AuthCode = " + receipt.GetAuthCode());
                    m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
                    m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
                    m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
                    m_Log.LogMessage(LogLevels.logINFO, "Ticket = " + receipt.GetTicket());
                    m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());                    

                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }


                    if (!IsError(eErrorCode))
                    {
                        strAuthCode = receipt.GetAuthCode();
                        strAuthResult = receipt.GetReferenceNum();
                        strDateTime = receipt.GetTransDate() + " " + receipt.GetTransTime();
                        strRefundTransaction = receipt.GetTxnNumber();
                    }


                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.RefundTransaction: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.RefundTransaction: ", e);

            }

            return bRes;
        }


        public bool DeleteToken(string strStoreId,
                                string strAPIToken,
                                string strCardReference,
                                string strProcesingCountryCode,
                                bool bTestMode,
                                out MonerisErrorCode eErrorCode,
                                out string errorMessage)
        {
            bool bRes = false;
            eErrorCode = MonerisErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            try
            {

                AddTLS12Support();

                bool status_check = false;

                ResDelete resDelete = new ResDelete(strCardReference);

                HttpsPostRequest mpgReq = new HttpsPostRequest();
                mpgReq.SetProcCountryCode(strProcesingCountryCode);
                mpgReq.SetTestMode(bTestMode); //false or comment out this line for production transactions
                mpgReq.SetStoreId(strStoreId);
                mpgReq.SetApiToken(strAPIToken);
                mpgReq.SetTransaction(resDelete);
                mpgReq.SetStatusCheck(status_check);
                mpgReq.Send();

                try
                {
                    Receipt receipt = mpgReq.GetReceipt();


                    m_Log.LogMessage(LogLevels.logINFO, "MonerisPayments.DeleteToken.Receipt: ");

                    m_Log.LogMessage(LogLevels.logINFO, "DataKey = " + receipt.GetDataKey());
			        m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + receipt.GetResponseCode());
			        m_Log.LogMessage(LogLevels.logINFO, "Message = " + receipt.GetMessage());
			        m_Log.LogMessage(LogLevels.logINFO, "TransDate = " + receipt.GetTransDate());
			        m_Log.LogMessage(LogLevels.logINFO, "TransTime = " + receipt.GetTransTime());
			        m_Log.LogMessage(LogLevels.logINFO, "Complete = " + receipt.GetComplete());
			        m_Log.LogMessage(LogLevels.logINFO, "TimedOut = " + receipt.GetTimedOut());
			        m_Log.LogMessage(LogLevels.logINFO, "ResSuccess = " + receipt.GetResSuccess());
			        m_Log.LogMessage(LogLevels.logINFO, "PaymentType = " + receipt.GetPaymentType());

			        //ResolveData
			        m_Log.LogMessage(LogLevels.logINFO, "Email = " + receipt.GetResEmail());
			        m_Log.LogMessage(LogLevels.logINFO, "MaskedPan = " + receipt.GetResMaskedPan());
			        //m_Log.LogMessage(LogLevels.logINFO, "Exp Date = " + receipt.GetResExpdate());
			        m_Log.LogMessage(LogLevels.logINFO, "Crypt Type = " + receipt.GetResCryptType());


                    try
                    {
                        eErrorCode = (MonerisErrorCode)Convert.ToInt32(receipt.GetResponseCode());
                    }
                    catch
                    {
                        eErrorCode = MonerisErrorCode.InternalError;
                    }

                    errorMessage = receipt.GetMessage();

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ErrorMessageDict[eErrorCode];
                    }               

                    bRes = true;

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.DeleteToken: ", e);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MonerisPayments.DeleteToken: ", e);

            }

            return bRes;
        }


        public static bool IsError(MonerisErrorCode eErrorCode)
        {
            return Convert.ToInt32(eErrorCode) >= 50;
        }


        public static string UserReference()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}{1:000}", DateTime.Now.ToUniversalTime(), m_oRandom.Next(0, 999));
        }


        public static void AddTLS12Support()
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
        

        protected void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        protected void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        public bool RefundEnabled
        {
            get { return true; }
        }
        public bool PartialRefundEnabled
        {
            get { return true; }
        }
    }
}
