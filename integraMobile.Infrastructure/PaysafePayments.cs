using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using integraMobile.Infrastructure.Logging.Tools;
using Paysafe;
using Authorization = Paysafe.CardPayments.Authorization;
using Verification = Paysafe.CardPayments.Verification;
using Paysafe.CustomerVault;
using Newtonsoft.Json;

namespace integraMobile.Infrastructure
{
    public class PaysafePayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(PaysafePayments));

        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));

        public class PaysafeMerchantInfo
        {
            public int AccountNumber { get; private set; }
            public string ApiKey { get; private set; }
            public string ApiSecret { get; private set; }
            public Paysafe.Environment Environment { get; private set; }

            public PaysafeMerchantInfo(int iAccountNumber, string sApiKey, string sApiSecret, string sEnvironment)
            {
                this.AccountNumber = iAccountNumber;
                this.ApiKey = sApiKey;
                this.ApiSecret = sApiSecret;
                this.Environment = (Paysafe.Environment)Enum.Parse(typeof(Paysafe.Environment), sEnvironment, true);
            }
        }

        #region Public Methods

        public bool VerifyPaymentToken(PaysafeMerchantInfo oMerchantConfig, string sCustomerId, string sPaymentToken, string sZip, out string sVerificationId, out string sErrorMessage)
        {
            bool bRet = false;
            sVerificationId = "";
            sErrorMessage = "";

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);
                var oStoredCredential =  new Paysafe.CardPayments.StoredCredential();
                oStoredCredential.type("RECURRING");
                oStoredCredential.occurrence("INITIAL");

                
                Verification oVerification = Verification.Builder()
                        .merchantRefNum(sCustomerId)
                        .dupCheck(true)
                        .billingDetails().zip(sZip).Done()
                        .card()
                            .paymentToken(sPaymentToken)
                            .Done()
                        .storedCredential(oStoredCredential)
                        .Build();

                m_Log.LogMessage(LogLevels.logINFO, string.Format("VerifyPaymentToken request {0}", PrettyJSON(oVerification.ToString())));

                Verification response = oClient.cardPaymentService().verify(oVerification);

                sVerificationId = response.id();

                bRet = true;
            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }

            return bRet;
        }

        /*public bool GetProfileFromId(PaysafeMerchantInfo oMerchantConfig, string sProfileId, out Profile oProfileOut, out string sErrorMessage)
        {
            bool bRet = false;
            oProfileOut = null;
            sErrorMessage = "";

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);

                Profile profile = Profile.Builder().id(sProfileId)                        
                        .Build();

                Profile oResponse = oClient.customerVaultService().get(profile);

                oProfileOut = oResponse;

                bRet = true;
            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }

            return bRet;
        }*/

        public bool GetProfileFromMerchantCustomerId(PaysafeMerchantInfo oMerchantConfig, string sMerchantCustomerId, out Profile oProfileOut, out string sErrorMessage)
        {
            bool bRet = false;
            oProfileOut = null;
            sErrorMessage = "";

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);

                Profile profile = Profile.Builder().merchantCustomerId(sMerchantCustomerId)
                        .Build();

                Profile oResponse = oClient.customerVaultService().getProfileFromMerchantCustomerId(profile);

                oProfileOut = oResponse;

                bRet = true;
            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;                
            }

            return bRet;
        }

        public bool CreateProfileFromSingleUseToken(PaysafeMerchantInfo oMerchantConfig, string sCustomerId, string sSingleUseToken, string sLocale, out Profile oProfileOut, out string sErrorMessage)
        {
            bool bRet = false;
            oProfileOut = null;
            sErrorMessage = "";

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);

                Profile profile = Profile.Builder()
                        .merchantCustomerId(sCustomerId)
                        .locale(sLocale)
                        //.firstName("John")
                        //.lastName("Smith")
                        .email(sCustomerId)
                        //.phone("713-444-5555")
                        .card()
                            .singleUseToken(sSingleUseToken)
                            .Done()
                        .Build();

                m_Log.LogMessage(LogLevels.logINFO, string.Format("CreateProfileFromSingleUseToken request {0}", PrettyJSON(profile.ToString())));

                oProfileOut = oClient.customerVaultService().create(profile);
            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }

            return bRet;
        }

        public bool DeleteProfile(PaysafeMerchantInfo oMerchantConfig, Profile oProfile, out string sErrorMessage)
        {
            bool bRet = false;
            sErrorMessage = "";

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);

                bRet = oClient.customerVaultService().delete(oProfile);
            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
            }

            return bRet;
        }

        /*public async Task<bool> CreateProfileFromSingleUseTokenAPI(PaysafeMerchantInfo oMerchantConfig, string sCustomerId, string sSingleUseToken, string sLocale//, out string sPaymentToken)
        {
            bool bRet = false;
            //sPaymentToken = "";

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var baseAddress = new Uri("https://api.test.paysafe.com/customervault/v1/profiles?merchantCustomerId=" + sCustomerId);

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(oMerchantConfig.ApiKey + ":" + oMerchantConfig.ApiSecret)));
                    
                    using (var response = await httpClient.GetAsync("profiles"))
                    {

                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }

        
            }
            catch (Exception ex)
            {

            }

            return bRet;            
        }*/

        public bool Authorize(PaysafeMerchantInfo oMerchantConfig, string sPaymentToken, int iAmount, string sZip,
                              out string sTransactionId, out string sOpReference, out DateTime? dtDateTime, out string sExpirationDateYear, out string sExpirationDateMonth, out string sPAN, out string sErrorMessage)
        {
            bool bRet = false;
            sErrorMessage = "";
            sTransactionId = "";
            sOpReference = "";
            dtDateTime = null;
            sExpirationDateYear = "";
            sExpirationDateMonth = "";
            sPAN = "";

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);
                var oStoredCredential = new Paysafe.CardPayments.StoredCredential();
                oStoredCredential.type("RECURRING");
                oStoredCredential.occurrence("SUBSEQUENT");

                string sOrderId = OrderId();
                
                Authorization oAuth = Authorization.Builder()
                                .merchantRefNum(sOrderId)
                                .storedCredential(oStoredCredential)
                                .amount(iAmount)
                                .settleWithAuth(true)
                                .dupCheck(true)
                                .billingDetails().zip(sZip).Done()
                                .card()
                                    .paymentToken(sPaymentToken)
                                    .Done()                                
                                .Build();

                m_Log.LogMessage(LogLevels.logINFO, string.Format("Authorize request {0}", PrettyJSON(oAuth.ToString())));

                Authorization oResponse = oClient.cardPaymentService().authorize(oAuth);

                //m_Log.LogMessage(LogLevels.logINFO, string.Format("Authorize response.json={0}", PrettyJSON(oResponse.ToString())));
                
                bRet = (oResponse.status() == "COMPLETED");

                if (bRet)
                {
                    sTransactionId = oResponse.id();
                    sOpReference = oResponse.authCode();
                    dtDateTime = oResponse.txnTime();
                    var oCard = oResponse.card();
                    if (oCard != null)
                    {
                        sPAN = oCard.lastDigits();
                        if (oCard.cardExpiry() != null)
                        {
                            sExpirationDateYear = oCard.cardExpiry().year().ToString();
                            if (sExpirationDateYear.Length == 4) sExpirationDateYear = sExpirationDateYear.Substring(2, 2);
                            sExpirationDateMonth = oCard.cardExpiry().month().ToString();
                        }
                    }

                }

            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
                //ex.rawResponse().ToString();
                // Log
                // ...
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
                // Log
                // ...
            }

            return bRet;
        }

        public bool Refund(PaysafeMerchantInfo oMerchantConfig, string sAuthTransactionId, int iAmount,
                             out string sRefundTransactionId, out string sRefundOpReference, out DateTime? dtDateTime, out string sErrorMessage)
        {
            bool bRet = false;
            sErrorMessage = "";
            sRefundTransactionId = "";
            sRefundOpReference = "";
            dtDateTime = null;

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);

                string sOrderId = OrderId();

                var oRef = Paysafe.CardPayments.Refund.Builder()
                    .settlementId(sAuthTransactionId)                    
                    .merchantRefNum(sOrderId)
                    .amount(iAmount)
                    .dupCheck(true)
                    .Build();

                
                m_Log.LogMessage(LogLevels.logINFO, string.Format("Refund request {0}", PrettyJSON(oRef.ToString())));

                Paysafe.CardPayments.Refund oResponse = oClient.cardPaymentService().refund(oRef);

                bRet = (oResponse.status() == "PENDING" || oResponse.status() == "COMPLETED");

                if (bRet)
                {
                    sRefundTransactionId = oResponse.id();
                    sRefundOpReference = "";
                    dtDateTime = oResponse.txnTime();
                    
                }

            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
                if (ex.Code() == 3406)
                {
                    bRet=CancelSettlement(oMerchantConfig,sAuthTransactionId,out sRefundTransactionId,out sRefundOpReference,out dtDateTime, out sErrorMessage);
                }
                //ex.rawResponse().ToString();
                // Log
                // ...
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
                // Log
                // ...
            }

            return bRet;
        }


        public bool CancelSettlement(PaysafeMerchantInfo oMerchantConfig, string sAuthTransactionId,
                                        out string sRefundTransactionId, out string sRefundOpReference, out DateTime? dtDateTime, out string sErrorMessage)
        {
            bool bRet = false;
            sErrorMessage = "";
            sRefundTransactionId = "";
            sRefundOpReference = "";
            dtDateTime = null;

            try
            {
                PaysafeApiClient oClient = this.NewClient(oMerchantConfig);

                string sOrderId = OrderId();

                var oRef = Paysafe.CardPayments.Settlement.Builder()
                    .id(sAuthTransactionId)                    
                    .merchantRefNum(sOrderId)                   
                    .dupCheck(true)
                    .Build();


                m_Log.LogMessage(LogLevels.logINFO, string.Format("Cancel Settlement request {0}", PrettyJSON(oRef.ToString())));

                Paysafe.CardPayments.Settlement oResponse = oClient.cardPaymentService().cancelSettlement(oRef);

                bRet = (oResponse.status() == "CANCELLED");

                if (bRet)
                {
                    sRefundTransactionId = oResponse.id();
                    sRefundOpReference = "";
                    dtDateTime = oResponse.txnTime();

                }

            }
            catch (Paysafe.Common.PaysafeException ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
              
            }
            catch (Exception ex)
            {
                bRet = false;
                sErrorMessage = ex.Message;
                // Log
                // ...
            }

            return bRet;
        }


        #endregion

        #region Private Methods

        private PaysafeApiClient NewClient(PaysafeMerchantInfo oMerchantConfig)
        {
            return new PaysafeApiClient(oMerchantConfig.ApiKey, oMerchantConfig.ApiSecret, oMerchantConfig.Environment, oMerchantConfig.AccountNumber.ToString());
        }

        private string OrderId()
        {
            DateTime dtUtcNow = DateTime.UtcNow;
            return string.Format("{0:yyyyMMddHHmmss}{1:000}", dtUtcNow, m_oRandom.Next(0, 999));
        }

        private string PrettyJSON(string json)
        {

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string strRes = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }

        #endregion


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
