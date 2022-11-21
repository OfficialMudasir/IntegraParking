using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Data.Linq;
using System.Transactions;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using integraMobile.Domain.Abstract;
using integraMobile.Domain;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Infrastructure;
using integraMobile.Domain.Helper;

namespace integraMobile.Domain.Concrete
{
    public class SQLRetailerRepository : IRetailerRepository
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SQLBackOfficeRepository));
        private const int ctnTransactionTimeout = 30;
        private IQueryable<CURRENCy> currenciesTable = null;


        private string _connectionString;

        public SQLRetailerRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public CURRENCy GetCurrencyByIsoCode(string sIsoCode)
        {
            CURRENCy oCurrency = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null, _connectionString);

                    IQueryable<CURRENCy> res = null;
                    res = (from r in dbContext.CURRENCies
                           select r)
                           .Where(c => c.CUR_ISO_CODE == sIsoCode)
                           .AsQueryable();
                    if (res.Count() == 1)
                        oCurrency = res.First();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyByIsoCode: ", e);
            }

            return oCurrency;
        }

        public OPERATOR GetOperator(int iId, integraMobileDBEntitiesDataContext dbContext = null)
        {
            OPERATOR oOperator = null;
            try
            {

                if (dbContext == null) dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null, _connectionString);

                IQueryable<OPERATOR> res = null;
                res = (from r in dbContext.OPERATORs
                       select r)
                       .Where(c => c.OPR_ID == iId)
                       .AsQueryable();
                if (res.Count() == 1)
                    oOperator = res.First();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getOperator: ", e);
            }

            return oOperator;
        }

        public PAYMENT_TYPE GetPaymentType(int iId)
        {
            PAYMENT_TYPE oPaymentType = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null, _connectionString);

                    IQueryable<PAYMENT_TYPE> res = null;
                    res = (from r in dbContext.PAYMENT_TYPEs
                           select r)
                           .Where(c => c.PAT_ID == iId)
                           .AsQueryable();
                    if (res.Count() == 1)
                        oPaymentType = res.First();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPaymentType: ", e);
            }

            return oPaymentType;

        }

        public bool UpdateRetailerCoupons(ref RETAILER oNewRetailer,
                                          string sName, string sEmail, string sAddress, string sDocId, int iCoupons, decimal dCouponAmount,
                                          string sCurrencyIsoCode, decimal dAmount,
                                          decimal dPercVAT1, decimal dPercVAT2, int iPartialVAT1, decimal dPercFEE, int iPercFEETopped, int iPartialPercFEE, int iFixedFEE, int iPartialFixedFEE,
                                          decimal dTotalAmount,
                                          decimal dCurrencyDivisorFromIsoCode,  
                                          PaymentMeanCreditCardProviderType eProviderType,                          
                                          string sOpReference,
                                          string sTransactionId,
                                          string sGatewayDate,
                                          string sAuthCode,
                                          string sAuthResult,
                                          string sAuthResultDesc,
                                          string sCardHash,
                                          string sCardReference,
                                          string sCardScheme,
                                          string sMaskedCardNumber,
                                          DateTime? dtCardExpirationDate,
                                          string sPaypalToken,
                                          string sPaypalPayerId,
                                          string sPayPaypalPreapprovedPayKey)
        {
            bool bRet = false;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null, _connectionString);

                        CURRENCy oCurrency = dbContext.CURRENCies.Where(cur => cur.CUR_ISO_CODE == sCurrencyIsoCode).FirstOrDefault();
                        if (oCurrency.CUR_ISO_CODE != "") {

                            // RETAILER
                            oNewRetailer = new RETAILER() {
                                RTL_NAME = sName,
                                RTL_EMAIL = sEmail,
                                RTL_ADDRESS = sAddress,
                                RTL_DOC_ID = sDocId
                            };
                            dbContext.RETAILERs.InsertOnSubmit(oNewRetailer);

                            int iDefaultOperatorId = Int32.Parse(ConfigurationManager.AppSettings["DefaultOperatorID"].ToString());
                            OPERATOR oOperator = GetOperator(iDefaultOperatorId, dbContext);


                            var oSourceApp = dbContext.SOURCE_APPs
                                               .Where(r => r.SOAPP_DEFAULT == 1)
                                               .FirstOrDefault();


                            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = dbContext.CURRENCies
                              .Where(r => r.CUR_ISO_CODE == sCurrencyIsoCode).FirstOrDefault()
                              .CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                              .Where(r => r.CPTGC_ENABLED != 0 &&
                                          r.CPTGC_IS_INTERNAL != 0 &&
                                          r.CPTGC_INTERNAL_SOAPP_ID.HasValue &&
                                          r.CPTGC_INTERNAL_SOAPP_ID == oSourceApp.SOAPP_ID &&
                                          r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                     r.CPTGC_PROVIDER == Convert.ToInt32(eProviderType))
                              .FirstOrDefault();


                            if (oGatewayConfig == null)
                            {
                                oGatewayConfig = dbContext.CURRENCies
                                    .Where(r => r.CUR_ISO_CODE == sCurrencyIsoCode).FirstOrDefault()
                                    .CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                    .Where(r => r.CPTGC_ENABLED != 0 &&
                                                r.CPTGC_IS_INTERNAL != 0 &&
                                                !r.CPTGC_INTERNAL_SOAPP_ID.HasValue &&
                                                r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                           r.CPTGC_PROVIDER == Convert.ToInt32(eProviderType))
                                    .FirstOrDefault();
                            }
                            decimal? dGatewayConfigId = ((oGatewayConfig != null) ? oGatewayConfig.CPTGC_ID : (decimal?)null);


                            if (oOperator != null && oOperator.OPR_CURRENT_INVOICE_NUMBER <= oOperator.OPR_END_INVOICE_NUMBER)
                            {

                                // RETAILER_PAYMENTS
                                RETAILER_PAYMENT oRetailerPayment = new RETAILER_PAYMENT()
                                {
                                    RTLPY_RTL_ID = oNewRetailer.RTL_ID,
                                    RETAILER = oNewRetailer,
                                    RTLPY_AMOUNT = (int)(dAmount * dCurrencyDivisorFromIsoCode),
                                    RTLPY_CUR_ID = oCurrency.CUR_ID,
                                    RTLPY_DATE = DateTime.Now,
                                    RTLPY_CREDIT_CARD_PAYMENT_PROVIDER = (int)eProviderType,
                                    RTLPY_OP_REFERENCE = sOpReference,
                                    RTLPY_TRANSACTION_ID = sTransactionId,
                                    RTLPY_GATEWAY_DATE = sGatewayDate,
                                    RTLPY_AUTH_CODE = sAuthCode,
                                    RTLPY_AUTH_RESULT = sAuthResult,
                                    RTLPY_CARD_HASH = sCardHash,
                                    RTLPY_CARD_REFERENCE = sCardReference,
                                    RTLPY_CARD_SCHEME = sCardScheme,
                                    RTLPY_MASKED_CARD_NUMBER = sMaskedCardNumber,
                                    RTLPY_CARD_EXPIRATION_DATE = dtCardExpirationDate,
                                    RTLPY_PAYPAL_3T_TOKEN = sPaypalToken,
                                    RTLPY_PAYPAL_3T_PAYER_ID = sPaypalPayerId,
                                    RTLPY_PAYPAL_PREAPPROVED_PAY_KEY = sPayPaypalPreapprovedPayKey,
                                    RTLPY_PERC_VAT1 = dPercVAT1,
                                    RTLPY_PERC_VAT2 = dPercVAT2,
                                    RTLPY_PARTIAL_VAT1 = iPartialVAT1,
                                    RTLPY_PERC_FEE = dPercFEE,
                                    RTLPY_PERC_FEE_TOPPED = iPercFEETopped,
                                    RTLPY_PARTIAL_PERC_FEE = iPartialPercFEE,
                                    RTLPY_FIXED_FEE = iFixedFEE,
                                    RTLPY_PARTIAL_FIXED_FEE = iPartialFixedFEE,
                                    RTLPY_TOTAL_AMOUNT_CHARGED = dTotalAmount * dCurrencyDivisorFromIsoCode,
                                    RTLPY_SUSCRIPTION_TYPE = (int)PaymentSuscryptionType.pstPrepay,
                                    RTLPY_TRANS_STATUS = (int)((eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA)?PaymentMeanRechargeStatus.Committed:
                                                                                                                               PaymentMeanRechargeStatus.Waiting_Commit), // ?????
                                    RTLPY_STATUS_DATE = DateTime.UtcNow,
                                    RTLPY_RETRIES_NUM = 0, // ?????
                                    RTLPY_INV_NUMBER = oOperator.OPR_CURRENT_INVOICE_NUMBER.ToString(),
                                    RTLPY_INSERTION_UTC_DATE = DateTime.UtcNow,
                                    RTLPY_INVSCH_ID = null,
                                    RTLPY_CPTGC_ID = dGatewayConfigId // ?????                                
                                };                                
                                dbContext.RETAILER_PAYMENTs.InsertOnSubmit(oRetailerPayment);
                                oOperator.OPR_CURRENT_INVOICE_NUMBER++;

                                // RECHARGE_COUPONS
                                RECHARGE_COUPON oRechargeCoupon;
                                for (int i = 0; i < iCoupons; i++)
                                {
                                    string sKeyCode = "";

                                    oRechargeCoupon = new RECHARGE_COUPON()
                                    {
                                        RCOUP_CODE = CouponCodeGenerator.GenerateCode(ref sKeyCode),
                                        RCOUP_COUPS_ID = 2,
                                        RCOUP_VALUE = (int)(dCouponAmount * dCurrencyDivisorFromIsoCode),
                                        RCOUP_CUR_ID = oCurrency.CUR_ID,
                                        RCOUP_START_DATE = DateTime.UtcNow,
                                        RCOUP_EXP_DATE = new DateTime(2999, 1, 1),
                                        RETAILER_PAYMENT = oRetailerPayment,
                                        RCOUP_KEYCODE = sKeyCode,
                                        RCOUP_RTLPY_ID = oRetailerPayment.RTLPY_ID                                    
                                    };
                                    dbContext.RECHARGE_COUPONs.InsertOnSubmit(oRechargeCoupon);
                                }

                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                                bRet = true;

                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "UpdateRetailerCoupons: invalid operator");
                                bRet = false;
                            }
                        }
                        else {
                            m_Log.LogMessage(LogLevels.logERROR, "UpdateRetailerCoupons: invalid current iso code");
                            bRet = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateRetailerCoupons: ", e);
                        bRet = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateRetailerCoupons: ", e);
                bRet = false;
            }
            return bRet;
        }

        public RETAILER GetRetailer(decimal iId)
        {
            RETAILER oRetailer = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null, _connectionString);

                    IQueryable<RETAILER> res = null;
                    res = (from r in dbContext.RETAILERs
                           select r)
                           .Where(c => c.RTL_ID == iId)
                           .AsQueryable();
                    if (res.Count() == 1)
                        oRetailer = res.First();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getRetailer: ", e);
            }

            return oRetailer;
        }



        public bool GetWaitingCommitRetailerPayment(out RETAILER_PAYMENT oRetailerPayment, int iConfirmWaitTime,
                                            int iNumSecondsToWaitInCaseOfRetry, int iMaxRetries)
        {
            bool bRes = true;
            oRetailerPayment = null;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                    var oRetailerPayments = (from r in dbContext.RETAILER_PAYMENTs
                                             where (r.RTLPY_TRANS_STATUS == (int)PaymentMeanRechargeStatus.Waiting_Commit) &&
                                             (((r.RTLPY_RETRIES_NUM == 0) && (DateTime.UtcNow >= (r.RTLPY_STATUS_DATE.AddSeconds(iConfirmWaitTime)))) ||
                                             ((r.RTLPY_RETRIES_NUM > 0) && (DateTime.UtcNow >= (r.RTLPY_STATUS_DATE.AddSeconds(iNumSecondsToWaitInCaseOfRetry)))))
                                             orderby r.RTLPY_STATUS_DATE
                                             select r).AsQueryable();

                    if (oRetailerPayments.Count() > 0)
                    {
                        oRetailerPayment = oRetailerPayments.First();
                    }
                    else
                    {
                        dbContext.Close();
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetWaitingCommitRetailerPayment: ", e);
                bRes = false;
            }

            return bRes;

        }


        public bool CommitTransaction(RETAILER_PAYMENT oRetailerPayment,
                                        string strUserReference,
                                        string strAuthResult,
                                        string strGatewayDate,
                                        string strCommitTransactionId)
        {
            bool bRes = true;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                        var oRetailerPayments = dbContext.RETAILER_PAYMENTs.
                                               Where(r => r.RTLPY_ID == oRetailerPayment.RTLPY_ID);

                        if (oRetailerPayments.Count() == 1)
                        {

                            oRetailerPayments.First().RTLPY_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Committed;
                            oRetailerPayments.First().RTLPY_STATUS_DATE = DateTime.UtcNow;
                            oRetailerPayments.First().RTLPY_SECOND_TRANSACTION_ID = strCommitTransactionId;
                            oRetailerPayments.First().RTLPY_SECOND_OP_REFERENCE = strUserReference;
                            oRetailerPayments.First().RTLPY_SECOND_AUTH_RESULT = strAuthResult;
                            oRetailerPayments.First().RTLPY_SECOND_GATEWAY_DATE = strGatewayDate;
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                        bRes = false;
                    }
                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool CommitTransaction(RETAILER_PAYMENT oRetailerPayment)
                                        
        {
            bool bRes = true;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                        var oRetailerPayments = dbContext.RETAILER_PAYMENTs.
                                               Where(r => r.RTLPY_ID == oRetailerPayment.RTLPY_ID);

                        if (oRetailerPayments.Count() == 1)
                        {

                            oRetailerPayments.First().RTLPY_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Committed;
                            oRetailerPayments.First().RTLPY_STATUS_DATE = DateTime.UtcNow;
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                        bRes = false;
                    }
                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                bRes = false;
            }

            return bRes;

        }


        public bool RetriesForCommitTransaction(RETAILER_PAYMENT oRetailerPayment, int iMaxRetries,
                                                string strUserReference,
                                                string strAuthResult,
                                                string strGatewayDate,
                                                string strCommitTransactionId)
        {
            bool bRes = true;          

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                        var oRetailerPayments = dbContext.RETAILER_PAYMENTs.
                                               Where(r => r.RTLPY_ID == oRetailerPayment.RTLPY_ID);

                        if (oRetailerPayments.Count() == 1)
                        {

                            int iCurrRetries = oRetailerPayments.First().RTLPY_RETRIES_NUM + 1;

                            if (iCurrRetries > iMaxRetries)
                            {
                                oRetailerPayments.First().RTLPY_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Failed_To_Commit;
                                oRetailerPayments.First().RTLPY_STATUS_DATE = DateTime.UtcNow;
                                oRetailerPayments.First().RTLPY_RETRIES_NUM = iCurrRetries;
                            }
                            else
                            {
                                oRetailerPayments.First().RTLPY_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Commit;
                                oRetailerPayments.First().RTLPY_STATUS_DATE = DateTime.UtcNow;
                                oRetailerPayments.First().RTLPY_RETRIES_NUM = iCurrRetries;
                            }

                            oRetailerPayments.First().RTLPY_SECOND_TRANSACTION_ID = strCommitTransactionId;
                            oRetailerPayments.First().RTLPY_SECOND_OP_REFERENCE = strUserReference;
                            oRetailerPayments.First().RTLPY_SECOND_AUTH_RESULT = strAuthResult;
                            oRetailerPayments.First().RTLPY_SECOND_GATEWAY_DATE = strGatewayDate;
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                        bRes = false;
                    }
                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CommitTransaction: ", e);
                bRes = false;
            }



            return bRes;

        }

        private IQueryable<CURRENCy> Currencies
        {
            get
            {
                if (currenciesTable == null)
                    currenciesTable = (DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null)).GetTable<CURRENCy>().OrderBy(coun => coun.CUR_ISO_CODE);

                return currenciesTable;
            }
        }


        private int GetCurrencyDivisorFromIsoCode(string strISOCode)
        {
            int iRes = 1;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                             new TransactionOptions()
                                                                             {
                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                             }))
                {
                    var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode);
                    if (oResultCurrencies.Count() > 0)
                    {
                        CURRENCy oCurrency = oResultCurrencies.First();
                        iRes = Convert.ToInt32(Math.Pow(10, Convert.ToDouble(oCurrency.CUR_MINOR_UNIT)));
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyIsoCodeNumericFromIsoCode: ", e);
            }

            return iRes;
        }


        private void SecureSubmitChanges(ref integraMobileDBEntitiesDataContext dbContext)
        {

            try
            {
                dbContext.SubmitChanges(ConflictMode.ContinueOnConflict);
            }

            catch (ChangeConflictException e)
            {
                Console.WriteLine(e.Message);
                // Automerge database values for members that client
                // has not modified.
                foreach (ObjectChangeConflict occ in dbContext.ChangeConflicts)
                {
                    occ.Resolve(RefreshMode.KeepChanges);
                }
            }

            // Submit succeeds on second try.
            dbContext.SubmitChanges(ConflictMode.FailOnFirstConflict);
        }

    }
}
