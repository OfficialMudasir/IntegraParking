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
    public class SQLAppealRepository : IAppealRepository
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SQLBackOfficeRepository));
        private const int ctnTransactionTimeout = 30;
        private string _connectionString;

        public SQLAppealRepository(string connectionString)
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

        public bool UpdateFine(ref TICKET_PAYMENTS_NON_USER oNewTicketPaymentNonUser,
                                          int iInstallation, string sTicketNumber, string sEmail,
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
            bool bRet = true; // set to false
            /*
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
                        if (oCurrency.CUR_ISO_CODE != "")
                        {

                            // TicketPaymentNonUser
                            oNewTicketPaymentNonUser = new TicketPaymentNonUser()
                            {
                                RTL_NAME = sName,
                                RTL_EMAIL = sEmail,
                                RTL_ADDRESS = sAddress,
                                RTL_DOC_ID = sDocId
                            };
                            dbContext.TicketPaymentNonUsers.InsertOnSubmit(oNewTicketPaymentNonUser);

                            int iDefaultOperatorId = Int32.Parse(ConfigurationManager.AppSettings["DefaultOperatorID"].ToString());
                            OPERATOR oOperator = GetOperator(iDefaultOperatorId, dbContext);

                            CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oGatewayConfig = dbContext.CURRENCies
                                .Where(r => r.CUR_ISO_CODE == sCurrencyIsoCode).FirstOrDefault()
                                .CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs
                                .Where(r => r.CPTGC_ENABLED != 0 && r.CPTGC_PAT_ID == Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard) &&
                                       r.CPTGC_PROVIDER == Convert.ToInt32(eProviderType))
                                .FirstOrDefault();

                            decimal? dGatewayConfigId = ((oGatewayConfig != null) ? oGatewayConfig.CPTGC_ID : (decimal?)null);


                            if (oOperator != null && oOperator.OPR_CURRENT_INVOICE_NUMBER <= oOperator.OPR_END_INVOICE_NUMBER)
                            {

                                // TICKET_PAYMENTS_NON_USERS
                                TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment = new TICKET_PAYMENTS_NON_USER()
                                {
                                    TIPANU_RTL_ID = oNewTicketPaymentNonUser.RTL_ID,
                                    TicketPaymentNonUser = oNewTicketPaymentNonUser,
                                    TIPANU_AMOUNT = (int)(dAmount * 100),
                                    TIPANU_CUR_ID = oCurrency.CUR_ID,
                                    TIPANU_DATE = DateTime.Now,
                                    TIPANU_CREDIT_CARD_PAYMENT_PROVIDER = (int)eProviderType,
                                    TIPANU_OP_REFERENCE = sOpReference,
                                    TIPANU_TRANSACTION_ID = sTransactionId,
                                    TIPANU_GATEWAY_DATE = sGatewayDate,
                                    TIPANU_AUTH_CODE = sAuthCode,
                                    TIPANU_AUTH_RESULT = sAuthResult,
                                    TIPANU_CARD_HASH = sCardHash,
                                    TIPANU_CARD_REFERENCE = sCardReference,
                                    TIPANU_CARD_SCHEME = sCardScheme,
                                    TIPANU_MASKED_CARD_NUMBER = sMaskedCardNumber,
                                    TIPANU_CARD_EXPIRATION_DATE = dtCardExpirationDate,
                                    TIPANU_PAYPAL_3T_TOKEN = sPaypalToken,
                                    TIPANU_PAYPAL_3T_PAYER_ID = sPaypalPayerId,
                                    TIPANU_PAYPAL_PREAPPROVED_PAY_KEY = sPayPaypalPreapprovedPayKey,
                                    TIPANU_PERC_VAT1 = dPercVAT1,
                                    TIPANU_PERC_VAT2 = dPercVAT2,
                                    TIPANU_PARTIAL_VAT1 = iPartialVAT1,
                                    TIPANU_PERC_FEE = dPercFEE,
                                    TIPANU_PERC_FEE_TOPPED = iPercFEETopped,
                                    TIPANU_PARTIAL_PERC_FEE = iPartialPercFEE,
                                    TIPANU_FIXED_FEE = iFixedFEE,
                                    TIPANU_PARTIAL_FIXED_FEE = iPartialFixedFEE,
                                    TIPANU_TOTAL_AMOUNT_CHARGED = dTotalAmount * 100,
                                    TIPANU_SUSCRIPTION_TYPE = (int)PaymentSuscryptionType.pstPrepay,
                                    TIPANU_TRANS_STATUS = (int)((eProviderType == PaymentMeanCreditCardProviderType.pmccpIECISA) ? PaymentMeanRechargeStatus.Committed :
                                                                                                                               PaymentMeanRechargeStatus.Waiting_Commit), // ?????
                                    TIPANU_STATUS_DATE = DateTime.UtcNow,
                                    TIPANU_RETRIES_NUM = 0, // ?????
                                    TIPANU_INV_NUMBER = oOperator.OPR_CURRENT_INVOICE_NUMBER.ToString(),
                                    TIPANU_INSERTION_UTC_DATE = DateTime.UtcNow,
                                    TIPANU_INVSCH_ID = null,
                                    TIPANU_CPTGC_ID = dGatewayConfigId // ?????                                
                                };
                                dbContext.TICKET_PAYMENTS_NON_USERs.InsertOnSubmit(oTicketPaymentNonUserPayment);
                                oOperator.OPR_CURRENT_INVOICE_NUMBER++;

                               

                              

                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                                bRet = true;

                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "UpdateTicketPaymentNonUserCoupons: invalid operator");
                                bRet = false;
                            }
                        }
                        else
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "UpdateTicketPaymentNonUserCoupons: invalid current iso code");
                            bRet = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateTicketPaymentNonUserCoupons: ", e);
                        bRet = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateTicketPaymentNonUserCoupons: ", e);
                bRet = false;
            }
            */
            return bRet;
        }

        public TICKET_PAYMENTS_NON_USER GetTicketPaymentNonUser(decimal iId)
        {
            TICKET_PAYMENTS_NON_USER oTicketPaymentNonUser = null;
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

                    IQueryable<TICKET_PAYMENTS_NON_USER> res = null;
                    res = (from r in dbContext.TICKET_PAYMENTS_NON_USERs
                           select r)
                           .Where(c => c.TIPANU_ID == iId)
                           .AsQueryable();
                    if (res.Count() == 1)
                        oTicketPaymentNonUser = res.First();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getTicketPaymentNonUser: ", e);
            }

            return oTicketPaymentNonUser;
        }



        public bool GetWaitingCommitTicketPaymentNonUserPayment(out TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment, int iConfirmWaitTime,
                                            int iNumSecondsToWaitInCaseOfRetry, int iMaxRetries)
        {
            bool bRes = true;
            oTicketPaymentNonUserPayment = null;

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



                    var oTicketPaymentNonUserPayments = (from r in dbContext.TICKET_PAYMENTS_NON_USERs
                                                         where (r.TIPANU_TRANS_STATUS == (int)PaymentMeanRechargeStatus.Waiting_Commit) &&
                                                         (((r.TIPANU_RETRIES_NUM == 0) && ((r.TIPANU_STATUS_DATE.HasValue) && DateTime.UtcNow >= (r.TIPANU_STATUS_DATE.Value.AddSeconds(iConfirmWaitTime)))) ||
                                                         ((r.TIPANU_RETRIES_NUM > 0) && ((r.TIPANU_STATUS_DATE.HasValue) && DateTime.UtcNow >= (r.TIPANU_STATUS_DATE.Value.AddSeconds(iNumSecondsToWaitInCaseOfRetry)))))
                                                         orderby r.TIPANU_STATUS_DATE
                                                         select r).AsQueryable();


                    if (oTicketPaymentNonUserPayments.Count() > 0)
                    {
                        oTicketPaymentNonUserPayment = oTicketPaymentNonUserPayments.First();
                    }
                    else
                    {
                        dbContext.Close();
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetWaitingCommitTicketPaymentNonUserPayment: ", e);
                bRes = false;
            }

            return bRes;

        }




        public bool CommitTransaction(TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment,
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


                        var oTicketPaymentNonUserPayments = dbContext.TICKET_PAYMENTS_NON_USERs.
                                               Where(r => r.TIPANU_ID == oTicketPaymentNonUserPayment.TIPANU_ID);

                        if (oTicketPaymentNonUserPayments.Count() == 1)
                        {

                            oTicketPaymentNonUserPayments.First().TIPANU_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Committed;
                            oTicketPaymentNonUserPayments.First().TIPANU_STATUS_DATE = DateTime.UtcNow;
                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_TRANSACTION_ID = strCommitTransactionId;
                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_OP_REFERENCE = strUserReference;
                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_AUTH_RESULT = strAuthResult;
                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_GATEWAY_DATE = strGatewayDate;
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

        public bool CommitTransaction(TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment)
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


                        var oTicketPaymentNonUserPayments = dbContext.TICKET_PAYMENTS_NON_USERs.
                                               Where(r => r.TIPANU_ID == oTicketPaymentNonUserPayment.TIPANU_ID);

                        if (oTicketPaymentNonUserPayments.Count() == 1)
                        {

                            oTicketPaymentNonUserPayments.First().TIPANU_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Committed;
                            oTicketPaymentNonUserPayments.First().TIPANU_STATUS_DATE = DateTime.UtcNow;
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


        public bool RetriesForCommitTransaction(TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment, int iMaxRetries,
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


                        var oTicketPaymentNonUserPayments = dbContext.TICKET_PAYMENTS_NON_USERs.
                                               Where(r => r.TIPANU_ID == oTicketPaymentNonUserPayment.TIPANU_ID);

                        if (oTicketPaymentNonUserPayments.Count() == 1)
                        {

                            int iCurrRetries = oTicketPaymentNonUserPayments.First().TIPANU_RETRIES_NUM + 1;

                            if (iCurrRetries > iMaxRetries)
                            {
                                oTicketPaymentNonUserPayments.First().TIPANU_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Failed_To_Commit;
                                oTicketPaymentNonUserPayments.First().TIPANU_STATUS_DATE = DateTime.UtcNow;
                                oTicketPaymentNonUserPayments.First().TIPANU_RETRIES_NUM = iCurrRetries;
                            }
                            else
                            {
                                oTicketPaymentNonUserPayments.First().TIPANU_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Commit;
                                oTicketPaymentNonUserPayments.First().TIPANU_STATUS_DATE = DateTime.UtcNow;
                                oTicketPaymentNonUserPayments.First().TIPANU_RETRIES_NUM = iCurrRetries;
                            }

                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_TRANSACTION_ID = strCommitTransactionId;
                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_OP_REFERENCE = strUserReference;
                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_AUTH_RESULT = strAuthResult;
                            oTicketPaymentNonUserPayments.First().TIPANU_SECOND_GATEWAY_DATE = strGatewayDate;
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

        public bool ChargeFinePaymentNonUser(bool bSubstractFromBalance,
                            PaymentSuscryptionType suscriptionType,
                            decimal dInstallationID,
                            DateTime dtTicketPayment,
                            DateTime dtUTCPaymentDate,
                            string strPlate,
                            string strTicketNumber,
                            string strTicketData,
                            int iQuantity,
                            decimal dCurID,
                            decimal dBalanceCurID,
                            double dChangeApplied,
                            double dChangeFee,
                            int iCurrencyChargedQuantity,
                            decimal dPercVat1, decimal dPercVat2, int iPartialVat1, decimal dPercFEE, int iPercFEETopped, int iPartialPercFEE, int iFixedFEE, int iPartialFixedFEE, int iTotalAmount,
                            decimal? dRechargeId,
                            bool bConfirmedInWS,
                            decimal? dLatitude,
                            decimal? dLongitude, string strAppVersion,
                            decimal? dGrpId,
                            decimal? dBackofficeUsrId,
                            decimal? dPaymentType,
                            string sAdditionalinfo,
                            out decimal dTicketPaymentID,
                            out DateTime? dtUTCInsertionDate,
                            string strReference,
                            string strTransactionId,
                            string strCFTransactionId,
                            string strGatewayDate,
                            string strAuthCode,
                            string strAuthResult,
                            string strCardHash,
                            string strCardReference,
                            string strCardScheme,
                            string strMaskedCardNumber,
                            string strPaypal3tPayerId,
                            string strPaypal3tToken,
                            DateTime? dtExpDate,
                            INSTALLATION oInstallation,
                            PaymentMeanCreditCardProviderType providerType,
                            string currencyIsoCode,
                            decimal dGatewayConfigId,
                            string authId)
        {
            bool bRes = true;
            dTicketPaymentID = -1;
            dtUTCInsertionDate = null;

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
                        TICKET_PAYMENTS_NON_USER oTicketPaymentNonUser = null;
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        decimal? dPlateID = null;
                        try
                        {
                            var oPlate = dbContext.USER_PLATEs.Where(r => r.USRP_PLATE == strPlate.ToUpper().Trim().Replace(" ", "") && r.USRP_ENABLED == 1).First();
                            if (oPlate != null)
                            {
                                dPlateID = oPlate.USRP_ID;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                        //decimal? dCustomerInvoiceID = null;
                        //GetCustomerInvoice(dbContext, dtTicketPayment, oUser.CUSTOMER.CUS_ID, dCurID, 0, iTotalAmount, dInstallationID, out dCustomerInvoiceID);

                        // TIPANU_MOSE_OS?? TIPANU_BALANCE_BEFORE?? TIPANU_CUSINV_ID??

                        int iTransStatus = 0;
                        decimal? dGConfigIdNull = null;
                        if (dGatewayConfigId != 0)
                        {
                            dGConfigIdNull = dGatewayConfigId;

                            iTransStatus = (int)((((int)providerType == (int)PaymentMeanCreditCardProviderType.pmccpCreditCall) ||
                                                      ((int)providerType == (int)PaymentMeanCreditCardProviderType.pmccpPaypal) ||
                                                      ((int)providerType == (int)PaymentMeanCreditCardProviderType.pmccpStripe)) ? (int)PaymentMeanRechargeStatus.Waiting_Commit :
                                                                                                                                            (int)PaymentMeanRechargeStatus.Committed);
                        }

                        oTicketPaymentNonUser = new TICKET_PAYMENTS_NON_USER
                        {
                            TIPANU_MOSE_OS = 1, // iOSType,
                            TIPANU_INS_ID = dInstallationID,
                            TIPANU_DATE = dtTicketPayment,
                            TIPANU_UTC_DATE = dtUTCPaymentDate,
                            TIPANU_DATE_UTC_OFFSET = Convert.ToInt32((dtUTCPaymentDate - dtTicketPayment).TotalMinutes),
                            TIPANU_USRP_ID = dPlateID,
                            TIPANU_PLATE_STRING = strPlate,
                            TIPANU_TICKET_NUMBER = strTicketNumber,
                            TIPANU_TICKET_DATA = string.IsNullOrEmpty(strTicketData) ? "" : strTicketData.Substring(0, Math.Min(strTicketData.Length, 300)),
                            TIPANU_AMOUNT = iQuantity,
                            TIPANU_AMOUNT_CUR_ID = dCurID,
                            TIPANU_BALANCE_CUR_ID = dBalanceCurID,
                            TIPANU_CHANGE_APPLIED = Convert.ToDecimal(dChangeApplied),
                            TIPANU_CHANGE_FEE_APPLIED = Convert.ToDecimal(dChangeFee),
                            TIPANU_FINAL_AMOUNT = iCurrencyChargedQuantity,
                            TIPANU_INSERTION_UTC_DATE = dbContext.GetUTCDate(),
                            TIPANU_EXTERNAL_ID = authId,
                            //TIPANU_CUSPMR_ID = dRechargeId,
                            TIPANU_BALANCE_BEFORE = 0, //oUser.USR_BALANCE,
                            TIPANU_SUSCRIPTION_TYPE = (int)suscriptionType,
                            TIPANU_CONFIRMED_IN_WS = (bConfirmedInWS ? 1 : 0),
                            TIPANU_LATITUDE = dLatitude,
                            TIPANU_LONGITUDE = dLongitude,
                            TIPANU_APP_VERSION = strAppVersion,
                            TIPANU_GRP_ID = (dGrpId != 0 ? dGrpId : null),
                            TIPANU_PERC_VAT1 = Convert.ToDecimal(dPercVat1),
                            TIPANU_PERC_VAT2 = Convert.ToDecimal(dPercVat2),
                            TIPANU_PARTIAL_VAT1 = iPartialVat1,
                            TIPANU_PERC_FEE = Convert.ToDecimal(dPercFEE),
                            TIPANU_PERC_FEE_TOPPED = iPercFEETopped,
                            TIPANU_PARTIAL_PERC_FEE = iPartialPercFEE,
                            TIPANU_FIXED_FEE = iFixedFEE,
                            TIPANU_PARTIAL_FIXED_FEE = iPartialFixedFEE,
                            TIPANU_TOTAL_AMOUNT = iTotalAmount,
                            TIPANU_CUSINV_ID = null, //dCustomerInvoiceID
                            TIPANU_OP_REFERENCE = strReference,
                            TIPANU_TRANSACTION_ID = string.IsNullOrEmpty(strTransactionId) ? "" : strTransactionId,
                            TIPANU_GATEWAY_DATE = string.IsNullOrEmpty(strGatewayDate) ? "" : strGatewayDate,
                            TIPANU_AUTH_CODE = strAuthCode,
                            TIPANU_AUTH_RESULT = strAuthResult,
                            TIPANU_CARD_HASH = strCardHash,
                            TIPANU_CARD_REFERENCE = strCardReference,
                            TIPANU_CARD_SCHEME = strCardScheme,
                            TIPANU_MASKED_CARD_NUMBER = strMaskedCardNumber,
                            TIPANU_CARD_EXPIRATION_DATE = dtExpDate,
                            TIPANU_TOTAL_AMOUNT_CHARGED = iTotalAmount,
                            //TIPANU_TRANS_STATUS = 0,
                            TIPANU_TRANS_STATUS = iTransStatus,
                            TIPANU_STATUS_DATE = dtUTCPaymentDate,
                            TIPANU_RETRIES_NUM = 0,
                            TIPANU_CREDIT_CARD_PAYMENT_PROVIDER = (int)providerType,
                            TIPANU_CPTGC_ID = dGConfigIdNull,
                            TIPANU_BACKOFFICE_USR_ID = (dBackofficeUsrId != 0 ? dBackofficeUsrId : null),
                            TIPANU_PAYMENT_TYPE = (dPaymentType != 0 ? dPaymentType : null),
                            TIPANU_ADDITIONAL_INFO = string.IsNullOrEmpty(sAdditionalinfo) ? "" : sAdditionalinfo,
                            TIPANU_PAYPAL_3T_PAYER_ID = string.IsNullOrEmpty(strPaypal3tPayerId) ? "" : strPaypal3tPayerId,
                            TIPANU_PAYPAL_3T_TOKEN = string.IsNullOrEmpty(strPaypal3tToken) ? "" : strPaypal3tToken
                        };

                        dbContext.TICKET_PAYMENTS_NON_USERs.InsertOnSubmit(oTicketPaymentNonUser);

                        var oPendingTransaction = (from r in dbContext.PENDING_TRANSACTION_OPERATIONs
                                                   where r.PTROP_OP_TYPE == (int)PendingTransactionOperationOpType.Charge &&
                                                         r.PTROP_CPTGC_ID == dGatewayConfigId &&
                                                         r.PTROP_TRANSACTION_ID == strTransactionId
                                                   select r).FirstOrDefault();


                        if (oPendingTransaction != null)
                        {
                            dbContext.PENDING_TRANSACTION_OPERATIONs.DeleteOnSubmit(oPendingTransaction);
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            if (oTicketPaymentNonUser != null)
                            {
                                dTicketPaymentID = oTicketPaymentNonUser.TIPANU_ID;
                                dtUTCInsertionDate = oTicketPaymentNonUser.TIPANU_INSERTION_UTC_DATE;

                            }
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "ChargeFinePaymentNonUser: ", e);
                            bRes = false;
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "ChargeFinePaymentNonUser: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ChargeFinePaymentNonUser: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool UpdateThirdPartyIDInFinePaymentNonUser(decimal dTicketPaymentID,
                                             string str3rdPartyOpNum)
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
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        TICKET_PAYMENTS_NON_USER oTicketPayment = dbContext.TICKET_PAYMENTS_NON_USERs.Where(r => r.TIPANU_ID == dTicketPaymentID).First();
                        oTicketPayment.TIPANU_EXTERNAL_ID = str3rdPartyOpNum;

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "UpdateThirdPartyIDInFinePayment: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateThirdPartyIDInFinePayment: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateThirdPartyIDInFinePayment: ", e);
                bRes = false;
            }

            return bRes;

        }

        public IQueryable<TICKET_PAYMENTS_NON_USER> GetTicketsPaymentNonUser(DateTime dt)
        {
            IQueryable<TICKET_PAYMENTS_NON_USER> oTicketPaymentsNonUser = null;
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


                    oTicketPaymentsNonUser = (from r in dbContext.TICKET_PAYMENTS_NON_USERs
                                              select r)
                           .Where(c => c.TIPANU_STATUS_DATE < dt)
                           .AsQueryable();



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTicketsPaymentNonUser: ", e);
            }

            return oTicketPaymentsNonUser;
        }

        public IQueryable<TICKET_PAYMENTS_NON_USER> GetTicketsPaymentNonUser(decimal idini, decimal idend)
        {
            IQueryable<TICKET_PAYMENTS_NON_USER> oTicketPaymentsNonUser = null;
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


                    oTicketPaymentsNonUser = (from r in dbContext.TICKET_PAYMENTS_NON_USERs
                                              select r)
                           .Where(c => c.TIPANU_ID >= idini && c.TIPANU_ID <= idend)
                           .AsQueryable();



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTicketsPaymentNonUser: ", e);
            }

            return oTicketPaymentsNonUser;
        }

        public bool UpdateFine(decimal dFineID,
                                  string sOpReference,
                                  string sTransactionId,
                                  string sGatewayDate,
                                  string sAuthCode,
                                  string sAuthResult)
        {
            bool bRet = true; // set to false

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


                        TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment = dbContext.TICKET_PAYMENTS_NON_USERs.
                                                                                   Where(r => r.TIPANU_ID == dFineID).
                                                                                   FirstOrDefault();


                        oTicketPaymentNonUserPayment.TIPANU_OP_REFERENCE = sOpReference;
                        oTicketPaymentNonUserPayment.TIPANU_TRANSACTION_ID = sTransactionId;
                        oTicketPaymentNonUserPayment.TIPANU_GATEWAY_DATE = sGatewayDate;
                        oTicketPaymentNonUserPayment.TIPANU_AUTH_CODE = sAuthCode;
                        oTicketPaymentNonUserPayment.TIPANU_AUTH_RESULT = sAuthResult;
                        oTicketPaymentNonUserPayment.TIPANU_STATUS_DATE = DateTime.UtcNow;



                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();

                        bRet = true;

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateTicketPaymentNonUserCoupons: ", e);
                        bRet = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateTicketPaymentNonUserCoupons: ", e);
                bRet = false;
            }

            return bRet;
        }
    }
}
