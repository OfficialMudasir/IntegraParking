using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using integraMobilePaymentsService.Properties;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Ninject;
using System.ComponentModel;



namespace integraMobilePaymentsService
{

    class CintegraMobilePaymentsManager
    {
        #region -- Constant definitions --


        const System.String ct_POOLTIME_TRANSACTIONS_MANAGER_TAG = "PoolingTransactionsManager";
        const System.String ct_POOLTIME_AUTOMATICRECHARGES_TAG = "PoolingAutomaticRecharges";
        const System.String ct_POOLTIME_PAYMENT_MEANS_INVALIDATION_TAG = "PoolingPaymentMeansInvalidation";
        const System.String ct_POOLTIME_DAYS_INVALIDATION_EXPIRED_MEANS_TAG = "InvalidateExpiredMeansAfterNumberOfDays";
        const System.String ct_STOPTIME_TAG = "Stoptime";
        const System.String ct_RETRIES_TAG = "Retries";
        const System.String ct_RESENDTIME_TAG = "RetriesTime";
        const System.String ct_CANCELTIME_TAG = "CancelTime";
        const System.String ct_CONFIRM_WAIT_TIME = "ConfirmWaitTime";
        const System.String ct_TOKEN_DELETION_WAIT_TIME = "TokenDeletionWaitTime";
        protected const long BIG_PRIME_NUMBER = 472189635;
        protected const long BIG_PRIME_NUMBER2 = 624159837;
        #endregion

        #region -- Member Variables --     
  
        private IKernel m_kernel = null;

        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infraestructureRepository { get; set; }
        [Inject]
        public IGeograficAndTariffsRepository geograficAndTariffsRepository { get; set; }
        [Inject]
        public IRetailerRepository retailerRepository { get; set; }
        [Inject]
        public IFineRepository fineRepository { get; set; }


        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CintegraMobilePaymentsManager));

        //Thread Signal: Stop service event
        private static ManualResetEvent m_evStopServer = new ManualResetEvent(false);

        //Thread to managing Transactions
        private Thread m_TransactionManagerThread;

        //Thread to Generate Payments
        private Thread m_AutomaticRechargesThread;

        //Thread to Send Paymentss
        private Thread m_PaymentInvalidationThread;

        // Time to pooling for Managing Transactions
        private int m_iPoolTimeTransactionManager;
        
        // Time to pooling for Automatic Recharges
        private int m_iPoolTimeAutomaticRecharges;

        // Time to pooling for invalidate Payment Means
        private int m_iPoolTimePaymentInvalidation;
        
        // Number of days to wait to invalidate payment mean after expiration
        private int m_iDaysAfterExpiredPaymentToInvalidate;

        // Time to wait thread termination before stop the server
        private int m_iStopTime;

        // Num of retries
        private static int m_iRetries;

        //Time between retries
        private int m_iResendTime;

        //Cancel time for started transactions
        private int m_iCancelTime;

        //Confirm Wait Time
        private int m_iConfirmWaitTime;

        // Token Deletion Wait Time (in seconds)
        private int m_iTokenDeletionWaitTimeSeconds;
        
        private static ManualResetEvent m_evPushBrokerEvent = new ManualResetEvent(false);
       
        #endregion

		#region -- Constructor / Destructor --

        public CintegraMobilePaymentsManager()
		{
            m_iPoolTimeTransactionManager = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_TRANSACTIONS_MANAGER_TAG].ToString());
            m_iPoolTimeAutomaticRecharges = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_AUTOMATICRECHARGES_TAG].ToString());
            m_iPoolTimePaymentInvalidation = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_PAYMENT_MEANS_INVALIDATION_TAG].ToString());
            m_iDaysAfterExpiredPaymentToInvalidate = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_DAYS_INVALIDATION_EXPIRED_MEANS_TAG].ToString());
            m_iStopTime             =  Convert.ToInt32(ConfigurationManager.AppSettings[ct_STOPTIME_TAG].ToString());
            m_iRetries              = Convert.ToInt32(ConfigurationManager.AppSettings[ct_RETRIES_TAG].ToString());
            m_iResendTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_RESENDTIME_TAG].ToString());
            m_iCancelTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_CANCELTIME_TAG].ToString());
            if (ConfigurationManager.AppSettings[ct_TOKEN_DELETION_WAIT_TIME] != null)
                m_iTokenDeletionWaitTimeSeconds = Convert.ToInt32(ConfigurationManager.AppSettings[ct_TOKEN_DELETION_WAIT_TIME].ToString());
            else
                m_iTokenDeletionWaitTimeSeconds = 1;

            try
            {
                m_iConfirmWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_CONFIRM_WAIT_TIME].ToString());
            }
            catch
            {
                m_iConfirmWaitTime = 90;
            }



            m_kernel = new StandardKernel(new integraMobilePaymementsModule());
            m_kernel.Inject(this);

        }


        
        #endregion 

        private enum enumPayPalMode
        {
            [Description("sandbox")]
            sandbox = 0,
            [Description("live")]
            Live = 1,
        }

        #region -- Threads Bodies --

        public void Start()
		{
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobilePaymentsManager::Start");


            m_TransactionManagerThread = new Thread(new ThreadStart(this.TransactionManagerThread));
            m_TransactionManagerThread.Start();

            m_AutomaticRechargesThread = new Thread(new ThreadStart(this.AutomaticRechargesThread));
            m_AutomaticRechargesThread.Start();

            m_PaymentInvalidationThread = new Thread(new ThreadStart(this.PaymentInvalidationThread));
            m_PaymentInvalidationThread.Start();
                       
            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::Start");
        }

        public void Stop()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobilePaymentsManager::Stop");

            m_evStopServer.Set();

            // We have to give time to close all the existing requests
            // Synchronize the finalization of the main thread
            m_TransactionManagerThread.Join(1000 * m_iStopTime);
            m_AutomaticRechargesThread.Join(1000 * m_iStopTime);
            m_PaymentInvalidationThread.Join(1000 * m_iStopTime);
            m_evStopServer.Reset();
          

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::Stop");
        }




        protected void TransactionManagerThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobilePaymentsManager::TransactionManagerThread");

            bool bFinishServer = false;
            while (bFinishServer == false)
            {

                CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = null;
                RETAILER_PAYMENT oRetailerPayment = null;
                TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment = null;
                PENDING_TRANSACTION_OPERATION oPendingTransactionOperation = null;


                string strUserReference = null;
                string strAuthCode=null;
                string strAuthResult = null;
                string strAuthResultDesc = null;
                string strGatewayDate = null;
                string strSecundaryTransactionId = null;
                int iTransactionFee=0;
                string strTransactionFeeCurrencyIsocode=null;
                string strTransactionURL = null;
                string strRefundTransactionURL = null;

                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimeTransactionManager, false));
                if (!bFinishServer)
                {         
                    //Commit pending commit transactions
                    lock (customersRepository)
                    {
                        customersRepository.GetWaitingCommitRecharge(out oRecharge,m_iConfirmWaitTime,  m_iResendTime, m_iRetries);
                    }
                    while ((!bFinishServer) && (oRecharge != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            strUserReference = null;
                            strAuthResult=null;
                            strGatewayDate=null;
                            strSecundaryTransactionId = null;

                            bool bBDCommitSuccess = false;

                            lock(customersRepository)
                            {
                                bBDCommitSuccess = customersRepository.CommitTransaction(oRecharge);
                            }


                            if (bBDCommitSuccess)
                            {
                                if (CommitTransaction(oRecharge,
                                                        out strUserReference,
                                                        out strAuthResult,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId,
                                                        out iTransactionFee,
                                                        out strTransactionFeeCurrencyIsocode,
                                                        out strTransactionURL,
                                                        out strRefundTransactionURL))
                                {
                                    lock (customersRepository)
                                    {
                                        customersRepository.CommitTransaction(oRecharge, strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId,
                                                                                         iTransactionFee,
                                                                                         strTransactionFeeCurrencyIsocode,
                                                                                         strTransactionURL,
                                                                                         strRefundTransactionURL);
                                    }
                                }
                                else
                                {
                                    lock (customersRepository)
                                    {
                                        customersRepository.RetriesForCommitTransaction(oRecharge, m_iRetries,
                                                                                         strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::TransactionManagerThread: Error Commiting Transaction in BDs");
                            }

                            lock (customersRepository)
                            {
                                customersRepository.GetWaitingCommitRecharge(out oRecharge, m_iConfirmWaitTime,  m_iResendTime, m_iRetries);
                            }                           
                        }
                    }
                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    //Commit pending commit transactions
                    lock (retailerRepository)
                    {
                        retailerRepository.GetWaitingCommitRetailerPayment(out oRetailerPayment,m_iConfirmWaitTime, m_iResendTime, m_iRetries);
                    }


                    while ((!bFinishServer) && (oRetailerPayment != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            strUserReference = null;
                            strAuthResult = null;
                            strGatewayDate = null;
                            strSecundaryTransactionId = null;
                            bool bBDCommitSuccess = false;

                            lock(customersRepository)
                            {
                                bBDCommitSuccess = retailerRepository.CommitTransaction(oRetailerPayment);
                            }


                            if (bBDCommitSuccess)
                            {

                                if (CommitTransaction(oRetailerPayment,
                                                        out strUserReference,
                                                        out strAuthResult,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId))
                                {
                                    lock (retailerRepository)
                                    {
                                        retailerRepository.CommitTransaction(oRetailerPayment, strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                                else
                                {
                                    lock (retailerRepository)
                                    {
                                        retailerRepository.RetriesForCommitTransaction(oRetailerPayment, m_iRetries,
                                                                                         strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::TransactionManagerThread: Error Commiting Transaction in BDs");
                            }

                            lock (retailerRepository)
                            {
                                retailerRepository.GetWaitingCommitRetailerPayment(out oRetailerPayment, m_iConfirmWaitTime, m_iResendTime, m_iRetries);
                            }
                        }
                    }



                }


                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    //Commit pending commit transactions
                    lock (fineRepository)
                    {
                        fineRepository.GetWaitingCommitTicketPaymentNonUserPayment(out oTicketPaymentNonUserPayment, m_iConfirmWaitTime, m_iResendTime, m_iRetries);
                    }


                    while ((!bFinishServer) && (oTicketPaymentNonUserPayment != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            strUserReference = null;
                            strAuthResult = null;
                            strGatewayDate = null;
                            strSecundaryTransactionId = null;
                            bool bBDCommitSuccess = false;

                            lock (customersRepository)
                            {
                                bBDCommitSuccess = fineRepository.CommitTransaction(oTicketPaymentNonUserPayment);
                            }


                            if (bBDCommitSuccess)
                            {

                                if (CommitTransaction(oTicketPaymentNonUserPayment,
                                                        out strUserReference,
                                                        out strAuthResult,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId))
                                {
                                    lock (fineRepository)
                                    {
                                        fineRepository.CommitTransaction(oTicketPaymentNonUserPayment, strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                                else
                                {
                                    lock (fineRepository)
                                    {
                                        fineRepository.RetriesForCommitTransaction(oTicketPaymentNonUserPayment, m_iRetries,
                                                                                         strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::TransactionManagerThread: Error Commiting Transaction in BDs");
                            }

                            lock (fineRepository)
                            {
                                fineRepository.GetWaitingCommitTicketPaymentNonUserPayment(out oTicketPaymentNonUserPayment, m_iConfirmWaitTime, m_iResendTime, m_iRetries);
                            }
                        }
                    }



                }


                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    //refund pending refund transactions
                    lock (customersRepository)
                    {
                        customersRepository.GetWaitingRefundRecharge(out oRecharge, m_iConfirmWaitTime,m_iResendTime, m_iRetries);
                    }
                    while ((!bFinishServer) && (oRecharge != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            strUserReference = null;
                            strAuthCode = null;
                            strAuthResult = null;
                            strAuthResultDesc = "";
                            strGatewayDate = null;
                            strSecundaryTransactionId = null;

                            bool bBDRefundSuccess = false;

                            lock (customersRepository)
                            {
                                bBDRefundSuccess = customersRepository.RefundTransaction(oRecharge);
                            }


                            if (bBDRefundSuccess)
                            {
                                int? iNewRefundAmount = null;
                                if (RefundTransaction(oRecharge,
                                                        out strUserReference,
                                                        out strAuthCode,
                                                        out strAuthResult,
                                                        out strAuthResultDesc,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId,
                                                        out iNewRefundAmount))
                                {
                                    lock (customersRepository)
                                    {
                                        customersRepository.RefundTransaction(oRecharge, strUserReference,
                                                                                         strAuthCode,
                                                                                         strAuthResult,
                                                                                         strAuthResultDesc,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                                else
                                {
                                    lock (customersRepository)
                                    {
                                        customersRepository.RetriesForRefundTransaction(oRecharge, m_iRetries,
                                                                                         strUserReference,
                                                                                         strAuthResult,
                                                                                         strAuthResultDesc,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId,
                                                                                         iNewRefundAmount);
                                    }
                                }
                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::TransactionManagerThread: Error Refunding Transaction in BDs");
                            }


                            lock (customersRepository)
                            {
                                customersRepository.GetWaitingRefundRecharge(out oRecharge, m_iConfirmWaitTime, m_iResendTime, m_iRetries);
                            }
                        }
                    }
                }
               
                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    lock (customersRepository)
                    {
                        customersRepository.GetCancelableRecharges(out oPendingTransactionOperation, m_iCancelTime, m_iResendTime, m_iRetries);
                    }


                    while ((!bFinishServer) && (oPendingTransactionOperation != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                         
                            bool bResult = false;


                            if (oPendingTransactionOperation.PTROP_TRANS_STATUS == (int)PaymentMeanRechargeStatus.Waiting_Refund)
                            {
                                bResult = RefundTransaction(oPendingTransactionOperation,
                                                        out strUserReference,
                                                        out strAuthCode,
                                                        out strAuthResult,
                                                        out strAuthResultDesc,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId);
                            }
                            else
                            {
                                bResult = CancelTransaction(oPendingTransactionOperation,
                                                        out strUserReference,
                                                        out strAuthResult,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId);

                            }


                            if (bResult)
                            {
                                lock (customersRepository)
                                {
                                       bResult = customersRepository.CancelTransaction(oPendingTransactionOperation,
                                                            strUserReference,
                                                            strAuthCode,
                                                            strAuthResult,
                                                            strAuthResultDesc,
                                                            strGatewayDate,
                                                            strSecundaryTransactionId);
                                }
                            }
                            else
                            {
                                lock (customersRepository)
                                {
                                    customersRepository.RetriesForCancelTransaction(oPendingTransactionOperation, m_iRetries,
                                                                                    strAuthResult,
                                                                                    strAuthResultDesc);
                                }
                            }
                            lock (customersRepository)
                            {
                                customersRepository.GetCancelableRecharges(out oPendingTransactionOperation, m_iCancelTime, m_iResendTime, m_iRetries);
                            }
                        }
                    }



                }


                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    lock (customersRepository)
                    {
                        customersRepository.GetTokenDeletions(out oPendingTransactionOperation, m_iResendTime, m_iRetries, m_iTokenDeletionWaitTimeSeconds);
                    }


                    while ((!bFinishServer) && (oPendingTransactionOperation != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {

                            bool bResult = false;

                            bResult = TokenDeletion(oPendingTransactionOperation,
                                                        m_iRetries,
                                                        out strUserReference,
                                                        out strAuthCode,
                                                        out strAuthResult,
                                                        out strAuthResultDesc,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId);
                           


                            if (bResult)
                            {
                                lock (customersRepository)
                                {
                                    bResult = customersRepository.TokenDeletion(oPendingTransactionOperation,
                                                         strUserReference,
                                                         strAuthCode,
                                                         strAuthResult,
                                                         strAuthResultDesc,
                                                         strGatewayDate,
                                                         strSecundaryTransactionId);
                                }
                            }
                            else
                            {
                                lock (customersRepository)
                                {
                                    customersRepository.RetriesForTokenDeletion(oPendingTransactionOperation, m_iRetries,
                                                                                    strAuthResult,
                                                                                    strAuthResultDesc);
                                }
                            }
                            lock (customersRepository)
                            {
                                customersRepository.GetTokenDeletions(out oPendingTransactionOperation, m_iResendTime, m_iRetries, m_iTokenDeletionWaitTimeSeconds);
                            }
                        }
                    }



                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    //void pending void transactions
                    lock (customersRepository)
                    {
                        customersRepository.GetWaitingCancellationRecharge(out oRecharge,m_iConfirmWaitTime, m_iResendTime, m_iRetries);
                    }
                    while ((!bFinishServer) && (oRecharge != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            strUserReference = null;
                            strAuthResult = null;
                            strGatewayDate = null;
                            strSecundaryTransactionId = null;


                            bool bBDCancelSuccess = false;

                            lock(customersRepository)
                            {
                                bBDCancelSuccess = customersRepository.CancelTransaction(oRecharge);
                            }


                            if (bBDCancelSuccess)
                            {


                                if (CancelTransaction(oRecharge,
                                                        out strUserReference,
                                                        out strAuthResult,
                                                        out strGatewayDate,
                                                        out strSecundaryTransactionId))
                                {
                                    lock (customersRepository)
                                    {
                                        customersRepository.CancelTransaction(oRecharge, strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                                else
                                {
                                    lock (customersRepository)
                                    {
                                        customersRepository.RetriesForCancellationTransaction(oRecharge, m_iRetries,
                                                                                         strUserReference,
                                                                                         strAuthResult,
                                                                                         strGatewayDate,
                                                                                         strSecundaryTransactionId);
                                    }
                                }
                            }
                            else
                            {
                                m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::TransactionManagerThread: Error Canceling Transaction in BDs");
                            }


                            lock (customersRepository)
                            {
                                customersRepository.GetWaitingCancellationRecharge(out oRecharge, m_iConfirmWaitTime, m_iResendTime, m_iRetries);
                            }
                        }
                    }

                }


                

            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::TransactionManagerThread");
        }



        protected void AutomaticRechargesThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobilePaymentsManager::AutomaticRechargesThread");

            bool bFinishServer = false;
            while (bFinishServer == false)
            {

                CUSTOMER_PAYMENT_MEAN oPaymentMean = null;

                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimeAutomaticRecharges, false));
                if (!bFinishServer)
                {
                    //Commit pending commit transactions
                    lock (customersRepository)
                    {
                        customersRepository.GetAutomaticPaymentMeanPendingAutomaticRecharge(out  oPaymentMean, m_iResendTime);
                    }
                    while ((!bFinishServer) && (oPaymentMean != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            PerformPrepayRecharge(oPaymentMean, PaymentMeanRechargeCreationType.pmrctAutomaticRecharge);

                            lock (customersRepository)
                            {
                                customersRepository.GetAutomaticPaymentMeanPendingAutomaticRecharge(out  oPaymentMean, m_iResendTime);
                            }
                        }
                    }
                }


            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::AutomaticRechargesThread");
        }


        protected void PaymentInvalidationThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobilePaymentsManager::PaymentInvalidationThread");

            bool bFinishServer = false;
            while (bFinishServer == false)
            {
                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimePaymentInvalidation, false));
                if (!bFinishServer)
                {                                      
                    lock (customersRepository)
                    {
                        customersRepository.InvalidatePaymentMeans(m_iDaysAfterExpiredPaymentToInvalidate,m_iRetries);
                    }
                }


            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobilePaymentsManager::PaymentInvalidationThread");
        }

        

        private bool CommitTransaction(CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge,
                                        out string strUserReference,
                                        out string strAuthResult,
                                        out string strGatewayDate,
                                        out string strCommitTransactionId,
                                        out int iTransactionFee,
                                        out string strTransactionFeeCurrencyIsocode,
                                        out string strTransactionURL,
                                        out string strRefundTransactionURL)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strCommitTransactionId = null;
            iTransactionFee = 0;
            strTransactionFeeCurrencyIsocode = "";
            strTransactionURL = "";
            strRefundTransactionURL = "";

            try
            {

                if (oRecharge.CUSPMR_TYPE == (int)PaymentMeanRechargeType.Payment)
                {
                    if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpCreditCall)
                    {
                        CardEasePayments cardPayment = new CardEasePayments();
                        bRes = cardPayment.ConfirmUnCommitedPayment(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                   oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                   oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                   oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                   oRecharge.CUSPMR_TRANSACTION_ID,
                                                                   out strUserReference,
                                                                   out strAuthResult,
                                                                   out strGatewayDate,
                                                                   out strCommitTransactionId);
                    }
                    else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpStripe)
                    {
                        string result = "";
                        string errorMessage = "";
                        string errorCode = "";

                        bRes = StripePayments.CaptureCharge(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                            oRecharge.CUSPMR_TRANSACTION_ID,
                                                             out result,
                                                             out errorCode,
                                                             out errorMessage,
                                                             out strCommitTransactionId);

                        if (bRes)
                        {
                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                        }

                    }
                    else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                               PaymentMeanCreditCardProviderType.pmccpMercadoPago)
                    {
                        MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                        string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID);
                        string strErrorMessage = "";

                        MercadoPagoPayments cardPayment = new MercadoPagoPayments();


                        int iAmount = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED) - (oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);

                        NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        numberFormatProvider.NumberDecimalSeparator = ".";
                        decimal dAmount = Convert.ToDecimal(iAmount, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(strCurISOCode);


                        if (dAmount > 0)
                        {

                            bRes = cardPayment.CommitTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                                 oRecharge.CUSPMR_TRANSACTION_ID,
                                                                 dAmount,
                                                                 out eErrorCode,
                                                                 out strErrorMessage);
                        }
                        else
                        {
                            bRes = cardPayment.CancelTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                                 oRecharge.CUSPMR_TRANSACTION_ID,
                                                                 out eErrorCode,
                                                                 out strErrorMessage);

                        }

                        if (bRes)
                        {
                            strCommitTransactionId = oRecharge.CUSPMR_TRANSACTION_ID;
                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                        }

                    }



                }
                else if (oRecharge.CUSPMR_TYPE == (int)PaymentMeanRechargeType.Paypal)
                {
                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";
                    decimal dQuantity = Convert.ToDecimal(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID));


                    bRes = PaypalPayments.ConfirmAppSDKPaypalPayment(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID,
                                                                     oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET,
                                                                     oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX,
                                                                     oRecharge.CUSPMR_AUTH_CODE,
                                                                     dQuantity.ToString(infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID)), numberFormatProvider),
                                                                     infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID),
                                                                     out strCommitTransactionId,
                                                                     out iTransactionFee,
                                                                     out strTransactionFeeCurrencyIsocode,
                                                                     out strTransactionURL,
                                                                     out strRefundTransactionURL);
                }
           
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::CommitTransaction: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }






       

        private bool CommitTransaction(RETAILER_PAYMENT oRetailerPayment,
                                out string strUserReference,
                                out string strAuthResult,
                                out string strGatewayDate,
                                out string strCommitTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strCommitTransactionId = null;

            try
            {

                if ((PaymentMeanCreditCardProviderType)oRetailerPayment.RTLPY_CREDIT_CARD_PAYMENT_PROVIDER ==
                       PaymentMeanCreditCardProviderType.pmccpCreditCall)
                {

                    CardEasePayments cardPayment = new CardEasePayments();
                    bRes = cardPayment.ConfirmUnCommitedPayment(oRetailerPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                            oRetailerPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                            oRetailerPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                            oRetailerPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                            oRetailerPayment.RTLPY_TRANSACTION_ID,
                                                            out strUserReference,
                                                            out strAuthResult,
                                                            out strGatewayDate,
                                                            out strCommitTransactionId);
                }
                else if ((PaymentMeanCreditCardProviderType)oRetailerPayment.RTLPY_CREDIT_CARD_PAYMENT_PROVIDER ==
                                           PaymentMeanCreditCardProviderType.pmccpStripe)
                {
                    string result = "";
                    string errorMessage = "";
                    string errorCode = "";

                    bRes = StripePayments.CaptureCharge(oRetailerPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                        oRetailerPayment.RTLPY_TRANSACTION_ID,
                                                         out result,
                                                         out errorCode,
                                                         out errorMessage,
                                                         out strCommitTransactionId);

                    if (bRes)
                    {
                        strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    }                
                
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::CommitTransaction: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }



        private bool CommitTransaction(TICKET_PAYMENTS_NON_USER oTicketPaymentNonUserPayment,
                                out string strUserReference,
                                out string strAuthResult,
                                out string strGatewayDate,
                                out string strCommitTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strCommitTransactionId = null;
            

            try
            {

                if ((PaymentMeanCreditCardProviderType)oTicketPaymentNonUserPayment.TIPANU_CREDIT_CARD_PAYMENT_PROVIDER ==
                       PaymentMeanCreditCardProviderType.pmccpCreditCall)
                {

                    CardEasePayments cardPayment = new CardEasePayments();
                    bRes = cardPayment.ConfirmUnCommitedPayment(oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                            oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                            oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                            oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                            oTicketPaymentNonUserPayment.TIPANU_TRANSACTION_ID,
                                                            out strUserReference,
                                                            out strAuthResult,
                                                            out strGatewayDate,
                                                            out strCommitTransactionId);
                }
                else if ((PaymentMeanCreditCardProviderType)oTicketPaymentNonUserPayment.TIPANU_CREDIT_CARD_PAYMENT_PROVIDER ==
                                           PaymentMeanCreditCardProviderType.pmccpStripe)
                {
                    string result = "";
                    string errorMessage = "";
                    string errorCode = "";

                    bRes = StripePayments.CaptureCharge(oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                        oTicketPaymentNonUserPayment.TIPANU_TRANSACTION_ID,
                                                         out result,
                                                         out errorCode,
                                                         out errorMessage,
                                                         out strCommitTransactionId);

                    if (bRes)
                    {
                        strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    }               
                
                }
                else if ((PaymentMeanCreditCardProviderType)oTicketPaymentNonUserPayment.TIPANU_CREDIT_CARD_PAYMENT_PROVIDER ==
                                           PaymentMeanCreditCardProviderType.pmccpPaypal)
                {
                    int iTransactionFee = 0;
                    string strTransactionFeeCurrencyIsocode = "";
                    string strTransactionURL = "";
                    string strRefundTransactionURL = "";
                    
                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";
                    decimal dQuantity = Convert.ToDecimal(oTicketPaymentNonUserPayment.TIPANU_TOTAL_AMOUNT, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode((int)oTicketPaymentNonUserPayment.TIPANU_BALANCE_CUR_ID));

                    bRes = PaypalPayments.ConfirmAppSDKPaypalPayment(oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID,
                                                                    oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET,
                                                                    oTicketPaymentNonUserPayment.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX,
                                                                    oTicketPaymentNonUserPayment.TIPANU_AUTH_CODE,
                                                                    dQuantity.ToString(infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode((int)oTicketPaymentNonUserPayment.TIPANU_BALANCE_CUR_ID)), numberFormatProvider),
                                                                    infraestructureRepository.GetCurrencyIsoCode((int)oTicketPaymentNonUserPayment.TIPANU_BALANCE_CUR_ID),
                                                                    out strCommitTransactionId,
                                                                    out iTransactionFee,
                                                                    out strTransactionFeeCurrencyIsocode,
                                                                    out strTransactionURL,
                                                                    out strRefundTransactionURL);
                    if (bRes)
                    {
                        strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    }

                }
                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::CommitTransaction: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool CancelTransaction(CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge,
                                        out string strUserReference,
                                        out string strAuthResult,
                                        out string strGatewayDate,
                                        out string strCommitTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strCommitTransactionId = null;

            try
            {


                if (oRecharge.CUSPMR_TYPE == (int)PaymentMeanRechargeType.Payment)
                {
                    if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpCreditCall)
                    {
                        CardEasePayments cardPayment = new CardEasePayments();
                        bRes = cardPayment.VoidUnCommitedPayment(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                oRecharge.CUSPMR_TRANSACTION_ID,
                                                                out strUserReference,
                                                                out strAuthResult,
                                                                out strGatewayDate,
                                                                out strCommitTransactionId);
                    }
                    else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpStripe)
                    {
                        string result = "";
                        string errorMessage = "";
                        string errorCode = "";

                        bRes = StripePayments.RefundCharge(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                            oRecharge.CUSPMR_TRANSACTION_ID,
                                                            Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED),
                                                            out result,
                                                            out errorCode,
                                                            out errorMessage,
                                                            out strCommitTransactionId);

                        if (bRes)
                        {
                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                        }

                    }
                }



             

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::CancelTransaction: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }


        private bool RefundTransaction(CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge,
                                        out string strUserReference,
                                        out string strAuthCode,
                                        out string strAuthResult,
                                        out string strAuthResultDesc,
                                        out string strGatewayDate,
                                        out string strRefundTransactionId,
                                        out int? iNewRefundAmount)
        {
            bool bRes = false;
            
            strUserReference = null;
            strAuthCode = null;
            strAuthResult = null;
            strAuthResultDesc = "";
            strGatewayDate = null;
            strRefundTransactionId = null;
            iNewRefundAmount = null;

            try
            {

                if (oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                {
                    USER oUser = oRecharge.USER;

                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";

                    bRes = true;
                    
                    int iRefundAmount = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                    if (iRefundAmount == 0) iRefundAmount = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);

                    if (iRefundAmount < oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED)
                    {
                        int iChargeQuantity = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED) - iRefundAmount;
                        decimal dQuantityToCharge = Convert.ToDecimal(iChargeQuantity, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE);


                        if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                PaymentMeanCreditCardProviderType.pmccpCreditCall)
                        {
                            CardEasePayments cardPayment = new CardEasePayments();

                            string strCardScheme = null;
                            string strTransactionId = null;

                            bRes = cardPayment.AutomaticPayment(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                oRecharge.USER.USR_EMAIL,
                                                                dQuantityToCharge,
                                                                oRecharge.CURRENCy.CUR_ISO_CODE,
                                                                oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                false,
                                                                out strUserReference,
                                                                out strAuthCode,
                                                                out strAuthResult,
                                                                out strGatewayDate,
                                                                out strCardScheme,
                                                                out strTransactionId);
                            if (bRes)
                            {
                                decimal? dRechargeId;
                                lock (customersRepository)
                                {
                                    if (!customersRepository.RechargeUserBalance(ref oUser, oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                                                Convert.ToInt32(MobileOS.Web),
                                                                                true,
                                                                                iChargeQuantity,iChargeQuantity,
                                                                                0, 0, 0, 0, 0, 0, 0, 0, iChargeQuantity,
                                                                                oUser.CURRENCy.CUR_ID,
                                                                                PaymentSuscryptionType.pstPrepay,
                                                                                PaymentMeanRechargeStatus.Waiting_Commit,
                                                                                PaymentMeanRechargeCreationType.pmrctRegularRecharge,
                                                                                strUserReference,
                                                                                strTransactionId,
                                                                                null,
                                                                                strGatewayDate,
                                                                                strAuthCode,
                                                                                strAuthResult,
                                                                                strAuthResultDesc,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                                strCardScheme,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_MASKED_CARD_NUMBER,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_NAME,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_EXPIRATION_DATE,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                false,
                                                                                null,
                                                                                null,
                                                                                null, "", "", "","","",null,null, oUser.USR_LAST_SOAPP_ID.Value,
                                                                                infraestructureRepository,
                                                                                out dRechargeId,
                                                                                true))
                                    {
                                        m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::RefundTransaction::Error CreditCall provider");
                                        bRes = false;
                                    }
                                }
                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                    PaymentMeanCreditCardProviderType.pmccpPayu)
                        {
                            PayuPayments cardPayment = new PayuPayments();
                            string errorMessage = "";
                            PayuPayments.PayuErrorCode eErrorCode = PayuPayments.PayuErrorCode.InternalError;
                            DateTime? dtTransaction = null;
                            string strCardScheme = null;
                            string strTransactionId = null;


                            string strLang = ((oUser.USR_CULTURE_LANG.ToLower() ?? "").Length >= 2) ? oUser.USR_CULTURE_LANG.Substring(0, 2) : "es";


                            bRes = cardPayment.AutomaticTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_URL,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_KEY,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_LOGIN,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_ACCOUNT_ID,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_MERCHANT_ID,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_SERVICE_TIMEOUT,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_COUNTRY,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_IS_TEST != 1 ? false : true,
                                                                    oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                    oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                    PayuPayments.Language(strLang),
                                                                    oUser.USR_EMAIL,
                                                                    dQuantityToCharge,
                                                                    oRecharge.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE,
                                                                    "RECARGA IPARKME",
                                                                    "",
                                                                    oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA,
                                                                    oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_NAME,
                                                                    //oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                    "",
                                                                    (!String.IsNullOrEmpty(oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CARD_SECURITY_CODE) ? DecryptCryptResult(oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CARD_SECURITY_CODE, ConfigurationManager.AppSettings["CryptKey"]) : String.Empty),
                                                                    out eErrorCode,
                                                                    out errorMessage,
                                                                    out strTransactionId,
                                                                    out strUserReference,
                                                                    out strAuthCode,
                                                                    out dtTransaction);

                            if (bRes)
                            {
                                bRes = !PayuPayments.IsError(eErrorCode);
                                strGatewayDate = dtTransaction.Value.ToString("HHmmssddMMyy");
                                strCardScheme = oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA;                                
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                customersRepository.StartRecharge(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                          oUser.USR_EMAIL,
                                                                          dtUTCNow,
                                                                          dtNow,
                                                                          iChargeQuantity,
                                                                          oRecharge.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ID,
                                                                          strAuthResult,
                                                                          strUserReference,
                                                                          strTransactionId,
                                                                          "",
                                                                          strGatewayDate,
                                                                          strAuthCode,
                                                                          PaymentMeanRechargeStatus.Committed);
                            }

                            if (bRes)
                            {
                                decimal? dRechargeId;
                                lock (customersRepository)
                                {
                                    if (!customersRepository.RechargeUserBalance(ref oUser,
                                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                                                Convert.ToInt32(MobileOS.Web),
                                                                                true,
                                                                                iChargeQuantity,iChargeQuantity,
                                                                                0, 0, 0, 0, 0, 0, 0, 0, iChargeQuantity,
                                                                                oUser.CURRENCy.CUR_ID,
                                                                                PaymentSuscryptionType.pstPrepay,
                                                                                PaymentMeanRechargeStatus.Committed,
                                                                                PaymentMeanRechargeCreationType.pmrctRegularRecharge,
                                                                                strUserReference,
                                                                                strTransactionId,
                                                                                null,
                                                                                strGatewayDate,
                                                                                strAuthCode,
                                                                                strAuthResult,
                                                                                strAuthResultDesc,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                                strCardScheme,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_MASKED_CARD_NUMBER,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_NAME,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_EXPIRATION_DATE,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                false,
                                                                                null,
                                                                                null,
                                                                                null, "", "", "", "", "", null, null, oUser.USR_LAST_SOAPP_ID.Value,
                                                                                infraestructureRepository,
                                                                                out dRechargeId,
                                                                                true))
                                    {
                                        m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::RefundTransaction::Error Payu provider");
                                        bRes = false;
                                    }
                                }
                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                    PaymentMeanCreditCardProviderType.pmccpTransbank)
                        {
                            TransBankPayments cardPayment = new TransBankPayments();
                            string errorMessage = "";
                            TransBankPayments.TransBankErrorCode eErrorCode = TransBankPayments.TransBankErrorCode.InternalError;
                            string strTransactionId = null;
                            string strCardScheme = null;

                            NumberFormatInfo provider = new NumberFormatInfo();
                            string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE), provider);

                            strUserReference = TransBankPayments.UserReference();
                            bRes = cardPayment.AutomaticTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_ENVIRONMENT,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_COMMERCECODE,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PUBLICCERT_FILE,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_WEBPAYCERT_FILE,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PASSWORD,
                                                                    oUser.USR_EMAIL,
                                                                    oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                    strUserReference,
                                                                    strAmount,
                                                                    out eErrorCode,
                                                                    out errorMessage,
                                                                    out strTransactionId,
                                                                    out strAuthCode,
                                                                    out strGatewayDate);


                            if (bRes)
                            {
                                bRes = !TransBankPayments.IsError(eErrorCode);                                
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                strCardScheme = oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA;
                                customersRepository.StartRecharge(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                    oUser.USR_EMAIL,
                                                                    dtUTCNow,
                                                                    dtNow,
                                                                    iChargeQuantity,
                                                                    oRecharge.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ID,
                                                                    "",
                                                                    strUserReference,
                                                                    strTransactionId,
                                                                    "",
                                                                    strGatewayDate,
                                                                    strAuthCode,
                                                                    PaymentMeanRechargeStatus.Committed);
                            }

                            if (bRes)
                            {
                                decimal? dRechargeId;
                                lock (customersRepository)
                                {
                                    if (!customersRepository.RechargeUserBalance(ref oUser,
                                                                                oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                                                Convert.ToInt32(MobileOS.Web),
                                                                                true,
                                                                                iChargeQuantity,iChargeQuantity,
                                                                                0, 0, 0, 0, 0, 0, 0, 0, iChargeQuantity,
                                                                                oUser.CURRENCy.CUR_ID,
                                                                                PaymentSuscryptionType.pstPrepay,
                                                                                PaymentMeanRechargeStatus.Committed,
                                                                                PaymentMeanRechargeCreationType.pmrctRegularRecharge,
                                                                                strUserReference,
                                                                                strTransactionId,
                                                                                null,
                                                                                strGatewayDate,
                                                                                strAuthCode,
                                                                                strAuthResult,
                                                                                strAuthResultDesc,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                                strCardScheme,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_MASKED_CARD_NUMBER,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_NAME,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                                oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_EXPIRATION_DATE,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                false,
                                                                                null,
                                                                                null,
                                                                                null, "", "", "", "", "", null, null, oUser.USR_LAST_SOAPP_ID.Value,
                                                                                infraestructureRepository,
                                                                                out dRechargeId,
                                                                                true))
                                    {
                                        m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::RefundTransaction::Error Transbank provider");
                                        bRes = false;
                                    }
                                }
                            }
                        }
                    }




                    if (bRes)
                    {

                        if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                               PaymentMeanCreditCardProviderType.pmccpCreditCall)
                        {
                            CardEasePayments cardPayment = new CardEasePayments();

                            bRes = cardPayment.RefundCommitedPayment(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                        oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                        oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                        oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                    oRecharge.CUSPMR_TRANSACTION_ID,
                                                                    out strUserReference,
                                                                    out strAuthResult,
                                                                    out strGatewayDate,
                                                                    out strRefundTransactionId);
                            if (!bRes && (oRecharge.CUSPMR_REFUND_AMOUNT ?? 0) < oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED)
                                iNewRefundAmount = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                        }

                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpIECISA)
                        {

                            DateTime? dtExpDate = null;
                            DateTime? dtTransactionDate = null;
                            string strExpMonth = "";
                            string strExpYear = "";
                            string strOpReference = "";
                            string strCFTransactionID = "";
                            string strMaskedCardNumber = "";
                            string strCardReference = "";
                            IECISAPayments cardPayment = new IECISAPayments();                            
                            IECISAPayments.IECISAErrorCode eErrorCode;
                            string errorMessage = "";

                            cardPayment.GetTransactionStatus(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                             oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                             oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                             oRecharge.CUSPMR_TRANSACTION_ID,
                                                            out eErrorCode,
                                                            out errorMessage,
                                                            out strMaskedCardNumber,
                                                            out strCardReference,
                                                            out dtExpDate,
                                                            out strExpMonth,
                                                            out strExpYear,
                                                            out dtTransactionDate,
                                                            out strOpReference,
                                                            out strCFTransactionID,
                                                            out strAuthCode);

                            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                            {

                                if (eErrorCode == IECISAPayments.IECISAErrorCode.TransactionNotCompleted)
                                {
                                    strAuthResult = "cancelled";
                                    bRes = true;

                                }

                            }
                            else
                            {

                                int iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                                if (iQuantityToRefund == 0) iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;

                                string strErrorMessage = "";
                                string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID);

                                cardPayment.RefundTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_USER,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_REFUNDSERVICE_URL,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                                    oRecharge.CUSPMR_TRANSACTION_ID,
                                                    oRecharge.CUSPMR_CF_TRANSACTION_ID,
                                                    dtTransactionDate.Value,
                                                    oRecharge.CUSPMR_AUTH_CODE,
                                                    iQuantityToRefund,
                                                    strCurISOCode,
                                                    infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(strCurISOCode),
                                                    dtNow,
                                                    out eErrorCode,
                                                    out errorMessage,
                                                    out strRefundTransactionId);

                                if ((eErrorCode != IECISAPayments.IECISAErrorCode.OK) && (eErrorCode != IECISAPayments.IECISAErrorCode.OriginalAmountLessAmountReturned))
                                {
                                    strAuthResult = eErrorCode.ToString();
                                    strAuthResultDesc = errorMessage;

                                    m_Log.LogMessage(LogLevels.logERROR, string.Format("RefundTransaction : errorCode={0} ; errorMessage={1}",
                                                strAuthResult, strErrorMessage));


                                }
                                else
                                {
                                    //strGatewayDate = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                                    strAuthResult = "succeeded";
                                    bRes = true;
                                }

                            }
                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                         PaymentMeanCreditCardProviderType.pmccpStripe)
                        {
                            string result = "";
                            string errorMessage = "";
                            string errorCode = "";

                            int iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                            if (iQuantityToRefund == 0) iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);

                            bRes = StripePayments.RefundCharge(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                                oRecharge.CUSPMR_TRANSACTION_ID,
                                                                iQuantityToRefund,
                                                                out result,
                                                                out errorCode,
                                                                out errorMessage,
                                                                out strRefundTransactionId);

                            if (bRes)
                            {
                                strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                                             PaymentMeanCreditCardProviderType.pmccpMoneris)
                        {
                            string errorMessage = "";
                            MonerisPayments.MonerisErrorCode eErrorCode;
                            MonerisPayments oPayments = new MonerisPayments();
                            string strDateTime = "";
                            NumberFormatInfo provider = new NumberFormatInfo();
                            string strISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID);

                            int iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                            if (iQuantityToRefund == 0) iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                            string strAmount = (Convert.ToDouble(iQuantityToRefund) / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(strISOCode), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(strISOCode), provider);

                            bRes = oPayments.RefundTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                        oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                        oRecharge.CUSPMR_OP_REFERENCE,
                                                        oRecharge.CUSPMR_TRANSACTION_ID,
                                                        strAmount,
                                                        oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                        oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                                        out eErrorCode, out errorMessage, out strRefundTransactionId, out strAuthCode, out strAuthResult,
                                                        out strDateTime);



                            if (bRes)
                            {
                                DateTime dt = DateTime.ParseExact(strDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                strGatewayDate = dt.ToString("yyyyMMddHHmmss");
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                                                                PaymentMeanCreditCardProviderType.pmccpPayu)
                        {
                            string errorMessage = "";
                            PayuPayments.PayuErrorCode eErrorCode;
                            PayuPayments oPayments = new PayuPayments();
                            string strLang = ((oRecharge.USER.USR_CULTURE_LANG.ToLower() ?? "").Length >= 2) ? oRecharge.USER.USR_CULTURE_LANG.Substring(0, 2) : "es";

                            bRes = oPayments.RefundTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_URL,
                                              oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_KEY,
                                              oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_LOGIN,
                                              oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_SERVICE_TIMEOUT,
                                              oRecharge.CUSPMR_AUTH_CODE,
                                              oRecharge.CUSPMR_TRANSACTION_ID,
                                              "Iparkme Refund Recharge",
                                              PayuPayments.Language(strLang),
                                              out eErrorCode,
                                              out errorMessage,
                                              out strRefundTransactionId);

                            if (bRes)
                            {
                                strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                            }

                            if (!bRes && (oRecharge.CUSPMR_REFUND_AMOUNT ?? 0) < oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED)
                                iNewRefundAmount = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);

                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                                                                 PaymentMeanCreditCardProviderType.pmccpTransbank)
                        {
                            string errorMessage = "";
                            TransBankPayments.TransBankErrorCode eErrorCode;
                            TransBankPayments oPayments = new TransBankPayments();


                            bRes = oPayments.RefundTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_ENVIRONMENT,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_COMMERCECODE,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PUBLICCERT_FILE,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_WEBPAYCERT_FILE,
                                                                    oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PASSWORD,
                                                                    oRecharge.CUSPMR_OP_REFERENCE,
                                                                    out eErrorCode,
                                                                    out errorMessage,
                                                                    out strRefundTransactionId);


                            if (bRes)
                            {
                                strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                            }

                            if (!bRes && (oRecharge.CUSPMR_REFUND_AMOUNT ?? 0) < oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED)
                                iNewRefundAmount = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                    PaymentMeanCreditCardProviderType.pmccpBSRedsys)
                        {
                            string errorMessage = "";
                            BSRedsysPayments.BSRedsysErrorCode eErrorCode;
                            BSRedsysPayments oPayments = new BSRedsysPayments();

                            int iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                            if (iQuantityToRefund == 0) iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);

                            string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID);
                            string strCurISOCodeNum = infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(strCurISOCode);

                            string strMerchantGroup = null;

                            if (oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP != null)
                            {
                                if (!string.IsNullOrEmpty(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP))
                                {
                                    strMerchantGroup = oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP;
                                }
                            }


                            bRes = oPayments.RefundTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                               oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                               oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                               oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                               oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,                                                                    
                                                               oRecharge.CUSPMR_OP_REFERENCE, oRecharge.CUSPMR_TRANSACTION_ID,
                                                               iQuantityToRefund, strCurISOCodeNum, strMerchantGroup,
                                                               out eErrorCode, out errorMessage,
                                                               out strUserReference, out strRefundTransactionId, out strGatewayDate);


                            if (!bRes && (oRecharge.CUSPMR_REFUND_AMOUNT ?? 0) < oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED)
                                iNewRefundAmount = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                    PaymentMeanCreditCardProviderType.pmccpPaysafe)
                        {
                            string errorMessage = "";                            
                            PaysafePayments oPayments = new PaysafePayments();

                            int iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                            if (iQuantityToRefund == 0) iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);

                            //string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID);
                            //string strCurISOCodeNum = infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(strCurISOCode);

                            DateTime? dtDateTime;

                            var oPaysafeConfig = new PaysafePayments.PaysafeMerchantInfo(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ACCOUNT_NUMBER,
                                                                                         oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_KEY,
                                                                                         oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_SECRET,
                                                                                         oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ENVIRONMENT);

                            bRes = oPayments.Refund(oPaysafeConfig, oRecharge.CUSPMR_TRANSACTION_ID,
                                                    iQuantityToRefund, 
                                                    out strRefundTransactionId, out strUserReference, out dtDateTime, out errorMessage);


                            if (!bRes && (oRecharge.CUSPMR_REFUND_AMOUNT ?? 0) < oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED)
                                iNewRefundAmount = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);

                            if (dtDateTime.HasValue)
                                strGatewayDate = dtDateTime.Value.ToString("yyyyMMddHHmmss");
                        }

                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                      PaymentMeanCreditCardProviderType.pmccpMercadoPago)
                        {
                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                            string strErrorMessage = "";

                            MercadoPagoPayments cardPayment = new MercadoPagoPayments();
                            string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID);

                            int iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                            if (iQuantityToRefund == 0) iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                            
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            decimal dAmount = Convert.ToDecimal(iQuantityToRefund, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(strCurISOCode);

                            if (dAmount > 0)
                            {
                                bRes = cardPayment.RefundTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                                     oRecharge.CUSPMR_TRANSACTION_ID,
                                                                     dAmount,
                                                                     out eErrorCode,
                                                                     out strErrorMessage,
                                                                     out strRefundTransactionId);
                            }
                            else
                            {
                                bRes = true;
                                strRefundTransactionId = oRecharge.CUSPMR_TRANSACTION_ID;


                            }

                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                        }
                        else if ((PaymentMeanCreditCardProviderType)oRecharge.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                      PaymentMeanCreditCardProviderType.pmccpMercadoPagoPro)
                        {
                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                            string strErrorMessage = "";

                            MercadoPagoPayments cardPayment = new MercadoPagoPayments();
                            string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oRecharge.CUSPMR_CUR_ID);

                            int iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_REFUND_AMOUNT ?? 0);
                            if (iQuantityToRefund == 0) iQuantityToRefund = Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);

                            numberFormatProvider.NumberDecimalSeparator = ".";
                            decimal dAmount = Convert.ToDecimal(iQuantityToRefund, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(strCurISOCode);

                            if (dAmount > 0)
                            {
                                bRes = cardPayment.RefundTransaction(oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                                     oRecharge.CUSPMR_TRANSACTION_ID,
                                                                     dAmount,
                                                                     out eErrorCode,
                                                                     out strErrorMessage,
                                                                     out strRefundTransactionId);
                            }
                            else
                            {
                                bRes = true;
                                strRefundTransactionId = oRecharge.CUSPMR_TRANSACTION_ID;


                            }

                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                        }

                    }


                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::Refund: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }


        private bool CancelTransaction(PENDING_TRANSACTION_OPERATION oPendingTransactionOperation,
                                        out string strUserReference,
                                        out string strAuthResult,
                                        out string strGatewayDate,
                                        out string strCommitTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthResult = null;
            strGatewayDate = null;
            strCommitTransactionId = null;

            try
            {


                if (oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                {
                    if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpCreditCall)
                    {
                        CardEasePayments cardPayment = new CardEasePayments();
                        bRes = cardPayment.VoidUnCommitedPayment(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                                out strUserReference,
                                                                out strAuthResult,
                                                                out strGatewayDate,
                                                                out strCommitTransactionId);
                    }
                    else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpStripe)
                    {
                        string result = "";
                        string errorMessage = "";
                        string errorCode = "";

                        bRes = StripePayments.RefundCharge(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                            oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                            Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED),
                                                            out result,
                                                            out errorCode,
                                                            out errorMessage,
                                                            out strCommitTransactionId);

                        if (bRes)
                        {
                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                        }

                    }
                    else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                         PaymentMeanCreditCardProviderType.pmccpMercadoPago)
                    {
                        MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                        string strErrorMessage = "";

                        MercadoPagoPayments cardPayment = new MercadoPagoPayments();



                        bRes = cardPayment.CancelTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                             oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                             out eErrorCode,
                                                             out strErrorMessage);

                        if (bRes)
                        {
                            strCommitTransactionId = oPendingTransactionOperation.PTROP_TRANSACTION_ID;
                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                        }


                    }

                    else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                           PaymentMeanCreditCardProviderType.pmccpPaypal)
                    {
                        string modePaypal = Enum.GetName(typeof(enumPayPalMode), (object)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_ENVIRONMENT);
                        string sAuthCode = string.Empty;
                        string stransactionID = string.Empty;
                        bRes = PaypalPayments.VoidPaypal(oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX,                                                            
                                                            modePaypal,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_SERVICE_TIMEOUT,
                                                            out sAuthCode, out stransactionID);

                        if (bRes)
                        {
                            strAuthResult = sAuthCode;
                            strCommitTransactionId = stransactionID;
                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                        }

                    }
                }
                else if (oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                {
                    string modePaypal = Enum.GetName(typeof(enumPayPalMode), (object)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_ENVIRONMENT);
                    string sAuthCode = string.Empty;
                    string stransactionID = string.Empty;
                    bRes = PaypalPayments.VoidPaypal(oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX,
                                                        modePaypal,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_SERVICE_TIMEOUT,
                                                        out sAuthCode, out stransactionID);

                    if (bRes)
                    {
                        strAuthResult = sAuthCode;
                        strCommitTransactionId = stransactionID;
                        strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    }

                }





            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::CancelTransaction: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }


        private bool RefundTransaction(PENDING_TRANSACTION_OPERATION oPendingTransactionOperation,
                                        out string strUserReference,
                                        out string strAuthCode,
                                        out string strAuthResult,
                                        out string strAuthResultDesc,
                                        out string strGatewayDate,
                                        out string strRefundTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthCode = null;
            strAuthResult = null;
            strAuthResultDesc = "";
            strGatewayDate = null;
            strRefundTransactionId = null;

            try
            {

                if (oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                {

                    if (oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED <= 0)
                    {
                        bRes = true;
                    }
                    else
                    {

                        if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                               PaymentMeanCreditCardProviderType.pmccpCreditCall)
                        {

                            CardEasePayments cardPayment = new CardEasePayments();
                            bRes = cardPayment.RefundCommitedPayment(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                     oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                     oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                     oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT,
                                                                   oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                                   out strUserReference,
                                                                   out strAuthResult,
                                                                   out strGatewayDate,
                                                                   out strRefundTransactionId);
                        }

                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpIECISA)
                        {


                            int iQuantityToRefund = Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED);
                            DateTime dtNow = DateTime.Now;
                            IECISAPayments.IECISAErrorCode eErrorCode;
                            string errorMessage = "";
                            DateTime dtUTCNow = DateTime.UtcNow;
                            IECISAPayments cardPayment = new IECISAPayments();
                            string strErrorMessage = "";


                            DateTime? dtExpDate = null;
                            DateTime? dtTransactionDate = null;
                            string strExpMonth = "";
                            string strExpYear = "";
                            string strOpReference = "";
                            string strCFTransactionID = "";
                            string strMaskedCardNumber = "";
                            string strCardReference = "";


                            cardPayment.GetTransactionStatus(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                             oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                             oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                             oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                            out eErrorCode,
                                                            out errorMessage,
                                                            out strMaskedCardNumber,
                                                            out strCardReference,
                                                            out dtExpDate,
                                                            out strExpMonth,
                                                            out strExpYear,
                                                            out dtTransactionDate,
                                                            out strOpReference,
                                                            out strCFTransactionID,
                                                            out strAuthCode);

                            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                            {

                                if (eErrorCode == IECISAPayments.IECISAErrorCode.TransactionNotCompleted)
                                {
                                    strAuthResult = "cancelled";
                                    bRes = true;

                                }

                            }
                            else
                            {

                                string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oPendingTransactionOperation.PTROP_CUR_ID);

                                cardPayment.RefundTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_USER,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_REFUNDSERVICE_URL,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                                    oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                    strCFTransactionID,
                                                    dtTransactionDate.Value,
                                                    strAuthCode,
                                                    iQuantityToRefund,
                                                    strCurISOCode,
                                                    infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(strCurISOCode),
                                                    dtNow,
                                                    out eErrorCode,
                                                    out errorMessage,
                                                    out strRefundTransactionId);

                                if ((eErrorCode != IECISAPayments.IECISAErrorCode.OK) && (eErrorCode != IECISAPayments.IECISAErrorCode.OriginalAmountLessAmountReturned))
                                {
                                    strAuthResult = eErrorCode.ToString();
                                    strAuthResultDesc = errorMessage;

                                    m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPrepayRecharge.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                                                strAuthResult, strErrorMessage));
                                }
                                else
                                {
                                    //strGatewayDate = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                                    strAuthResult = "succeeded";
                                    bRes = true;
                                }
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                         PaymentMeanCreditCardProviderType.pmccpStripe)
                        {
                            string result = "";
                            string errorMessage = "";
                            string errorCode = "";

                            bRes = StripePayments.RefundCharge(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                                oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                                Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED),
                                                                out result,
                                                                out errorCode,
                                                                out errorMessage,
                                                                out strRefundTransactionId);

                            if (bRes)
                            {
                                strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                                                PaymentMeanCreditCardProviderType.pmccpMoneris)
                        {
                            string errorMessage = "";
                            MonerisPayments.MonerisErrorCode eErrorCode;
                            MonerisPayments oPayments = new MonerisPayments();
                            string strDateTime = "";
                            NumberFormatInfo provider = new NumberFormatInfo();
                            string strISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oPendingTransactionOperation.PTROP_CUR_ID);
                            string strAmount = (Convert.ToDouble(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED) /
                                Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(strISOCode), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(strISOCode), provider);

                            bRes = oPayments.RefundTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                        oPendingTransactionOperation.PTROP_OP_REFERENCE,
                                                        oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                        strAmount,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                                        out eErrorCode, out errorMessage, out strRefundTransactionId, out strAuthCode, out strAuthResult,
                                                        out strDateTime);



                            if (bRes)
                            {
                                DateTime dt = DateTime.ParseExact(strDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                strGatewayDate = dt.ToString("yyyyMMddHHmmss");

                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                                                                   PaymentMeanCreditCardProviderType.pmccpPayu)
                        {
                            string errorMessage = "";
                            PayuPayments.PayuErrorCode eErrorCode;
                            PayuPayments oPayments = new PayuPayments();


                            bRes = oPayments.RefundTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_URL,
                                          oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_KEY,
                                          oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_LOGIN,
                                          oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_SERVICE_TIMEOUT,
                                          oPendingTransactionOperation.PTROP_AUTH_CODE,
                                          oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                          "Iparkme Refund Payment",
                                          PayuPayments.Language("es"),
                                          out eErrorCode,
                                          out errorMessage,
                                          out strRefundTransactionId);


                            if (bRes)
                            {
                                strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                                             PaymentMeanCreditCardProviderType.pmccpTransbank)
                        {
                            string errorMessage = "";
                            TransBankPayments.TransBankErrorCode eErrorCode;
                            TransBankPayments oPayments = new TransBankPayments();


                            bRes = oPayments.RefundTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_ENVIRONMENT,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_COMMERCECODE,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PUBLICCERT_FILE,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_WEBPAYCERT_FILE,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PASSWORD,
                                                                    oPendingTransactionOperation.PTROP_OP_REFERENCE,
                                                                    out eErrorCode,
                                                                    out errorMessage,
                                                                    out strRefundTransactionId);


                            if (bRes)
                            {
                                strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                    PaymentMeanCreditCardProviderType.pmccpBSRedsys)
                        {
                            string errorMessage = "";
                            BSRedsysPayments.BSRedsysErrorCode eErrorCode;
                            BSRedsysPayments oPayments = new BSRedsysPayments();

                            int iQuantityToRefund = Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED);

                            string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oPendingTransactionOperation.PTROP_CUR_ID);
                            string strCurISOCodeNum = infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(strCurISOCode);

                            string strMerchantGroup = null;

                            if (oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP != null)
                            {
                                if (!string.IsNullOrEmpty(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP))
                                {
                                    strMerchantGroup = oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP;
                                }
                            }

                            bRes = oPayments.RefundTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_WS_URL,
                                                               oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_CODE,
                                                               oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_SIGNATURE,
                                                               oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_MERCHANT_TERMINAL,
                                                               oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.BSREDSYS_CONFIGURATION.BSRCON_SERVICE_TIMEOUT,
                                                               oPendingTransactionOperation.PTROP_OP_REFERENCE, oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                               iQuantityToRefund, strCurISOCodeNum, strMerchantGroup,
                                                               out eErrorCode, out errorMessage,
                                                               out strUserReference, out strRefundTransactionId, out strGatewayDate);
                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                    PaymentMeanCreditCardProviderType.pmccpPaysafe)
                        {
                            string errorMessage = "";
                            PaysafePayments oPayments = new PaysafePayments();

                            int iQuantityToRefund = Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED);

                            //string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oPendingTransactionOperation.PTROP_CUR_ID);
                            //string strCurISOCodeNum = infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(strCurISOCode);

                            DateTime? dtDateTime = null;

                            var oPaysafeConfig = new PaysafePayments.PaysafeMerchantInfo(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ACCOUNT_NUMBER,
                                                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_KEY,
                                                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_SECRET,
                                                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ENVIRONMENT);

                            bRes = oPayments.Refund(oPaysafeConfig, oPendingTransactionOperation.PTROP_TRANSACTION_ID, iQuantityToRefund,
                                                               out strRefundTransactionId, out strUserReference, out dtDateTime, out errorMessage);

                            if (dtDateTime.HasValue)
                                strGatewayDate = dtDateTime.Value.ToString("yyyyMMddHHmmss");
                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                    PaymentMeanCreditCardProviderType.pmccpMercadoPago)
                        {

                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                            string strErrorMessage = "";

                            MercadoPagoPayments cardPayment = new MercadoPagoPayments();
                            string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oPendingTransactionOperation.PTROP_CUR_ID);

                            int iQuantityToRefund = Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED);

                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            decimal dAmount = Convert.ToDecimal(iQuantityToRefund, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(strCurISOCode);


                            bRes = cardPayment.RefundTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                                 oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                                 dAmount,
                                                                 out eErrorCode,
                                                                 out strErrorMessage,
                                                                 out strRefundTransactionId);

                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                    PaymentMeanCreditCardProviderType.pmccpMercadoPagoPro)
                        {

                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                            string strErrorMessage = "";

                            MercadoPagoPayments cardPayment = new MercadoPagoPayments();
                            string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oPendingTransactionOperation.PTROP_CUR_ID);

                            int iQuantityToRefund = Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED);

                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            decimal dAmount = Convert.ToDecimal(iQuantityToRefund, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(strCurISOCode);


                            bRes = cardPayment.RefundTransaction(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                                 oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                                 dAmount,
                                                                 out eErrorCode,
                                                                 out strErrorMessage,
                                                                 out strRefundTransactionId);

                            strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

                        }

                    }
                }

                else if (oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PAT_ID == (int)PaymentMeanType.pmtPaypal)
                {
                    string modePaypal = Enum.GetName(typeof(enumPayPalMode), (object)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_ENVIRONMENT);
                    string sAuthCode = string.Empty;
                    string stransactionID = string.Empty;

                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";
                    int iQuantityToRefund = Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED);
                    string strCurISOCode = infraestructureRepository.GetCurrencyIsoCode((int)oPendingTransactionOperation.PTROP_CUR_ID);

                    decimal dQuantityToRefund = Convert.ToDecimal(iQuantityToRefund, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(strCurISOCode);



                    bRes = PaypalPayments.RefundCapturedPaymentPaypal(dQuantityToRefund.ToString(numberFormatProvider), strCurISOCode, oPendingTransactionOperation.PTROP_TRANSACTION_ID,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_ID,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_CLIENT_SECRET,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_RESTAPI_URL_PREFIX,
                                                        modePaypal,
                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYPAL_CONFIGURATION.PPCON_SERVICE_TIMEOUT,
                                                        out stransactionID);

                    if (bRes)
                    {
                        strRefundTransactionId = stransactionID;
                        strGatewayDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::Refund: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }


        private bool TokenDeletion(PENDING_TRANSACTION_OPERATION oPendingTransactionOperation,
                                        int iMaxRetries,
                                        out string strUserReference,
                                        out string strAuthCode,
                                        out string strAuthResult,
                                        out string strAuthResultDesc,
                                        out string strGatewayDate,
                                        out string strRefundTransactionId)
        {
            bool bRes = false;
            strUserReference = null;
            strAuthCode = null;
            strAuthResult = null;
            strAuthResultDesc = "";
            strGatewayDate = null;
            strRefundTransactionId = null;

            try
            {

                if (oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PAT_ID == (int)PaymentMeanType.pmtDebitCreditCard)
                {

                    if (oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ENABLED != 1)
                    {
                        bRes = true;
                    }
                    else
                    {

                        if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                            PaymentMeanCreditCardProviderType.pmccpIECISA)
                        {


                            int iQuantityToRefund = Convert.ToInt32(oPendingTransactionOperation.PTROP_TOTAL_AMOUNT_CHARGED);
                            DateTime dtNow = DateTime.Now;
                            IECISAPayments.IECISAErrorCode eErrorCode;
                            //string errorMessage = "";
                            DateTime dtUTCNow = DateTime.UtcNow;
                            IECISAPayments cardPayment = new IECISAPayments();
                            string strErrorMessage = "";
                            string strTransactionId = "";
                            string strCFTransactionID = "";

                            cardPayment.StartTokenDeletion(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_USER,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                                            oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                                            oPendingTransactionOperation.PTROP_TOKEN_CARD_REFERENCE,
                                                            oPendingTransactionOperation.PTROP_TOKEN_CARD_HASH,
                                                            dtNow,
                                                            out eErrorCode,
                                                            out strErrorMessage,
                                                            out strTransactionId);

                            if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                            {
                                string errorCode = eErrorCode.ToString();

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("TokenDeletion.StartTokenDeletion : errorCode={0} ; errorMessage={1}",
                                          errorCode, strErrorMessage));


                            }
                            else
                            {
                                string strRedirectURL = "";
                                cardPayment.GetWebTransactionPaymentTypes(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                                        oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                                        strTransactionId,
                                                                        out eErrorCode,
                                                                        out strErrorMessage,
                                                                        out strRedirectURL);
                                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                {
                                    string errorCode = eErrorCode.ToString();

                                    m_Log.LogMessage(LogLevels.logERROR, string.Format("TokenDeletion.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                              errorCode, strErrorMessage));


                                }
                                else
                                {

                                    DateTime? dtTransactionDate = null;
                                    cardPayment.CompleteTokenDeletion(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                                      oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                                      oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                                    strTransactionId,
                                                                    oPendingTransactionOperation.PTROP_RETRIES_NUM,
                                                                    iMaxRetries,
                                                                    out eErrorCode,
                                                                    out strErrorMessage,
                                                                    out dtTransactionDate,
                                                                    out strCFTransactionID,
                                                                    out strAuthCode);


                                    if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                    {
                                        string errorCode = eErrorCode.ToString();

                                        m_Log.LogMessage(LogLevels.logERROR, string.Format("TokenDeletion.CompleteTokenDeletion : errorCode={0} ; errorMessage={1}",
                                                  errorCode, strErrorMessage));



                                    }
                                    else
                                    {
                                        bRes = true;

                                    }
                                }

                            }



                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                               PaymentMeanCreditCardProviderType.pmccpMoneris)
                        {

                            MonerisPayments oPayments = new MonerisPayments();
                            MonerisPayments.MonerisErrorCode eErrorCode;
                            string errorMessage = "";

                            oPayments.DeleteToken(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                  oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                  oPendingTransactionOperation.PTROP_TOKEN_CARD_REFERENCE,
                                                  oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                  oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                                  out eErrorCode, out errorMessage);


                            if (MonerisPayments.IsError(eErrorCode))
                            {
                                string errorCode = eErrorCode.ToString();

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("TokenDeletion.MonerisDeleteToken : errorCode={0} ; errorMessage={1}",
                                          errorCode, errorMessage));

                            }
                            else
                            {
                                bRes = true;

                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                                                     PaymentMeanCreditCardProviderType.pmccpPayu)
                        {
                            string errorMessage = "";
                            PayuPayments.PayuErrorCode eErrorCode;
                            PayuPayments oPayments = new PayuPayments();

                            bRes = oPayments.DeleteToken(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_URL,
                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_KEY,
                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_LOGIN,
                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_SERVICE_TIMEOUT,
                                                         oPendingTransactionOperation.PTROP_TOKEN_CARD_REFERENCE,
                                                         oPendingTransactionOperation.PTROP_TOKEN_CARD_HASH,
                                                         "es",
                                                         out eErrorCode,
                                                         out errorMessage);

                            if (PayuPayments.IsError(eErrorCode))
                            {
                                string errorCode = eErrorCode.ToString();

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("TokenDeletion.PayuDeleteToken : errorCode={0} ; errorMessage={1}",
                                          errorCode, errorMessage));

                            }
                            else
                            {
                                bRes = true;

                            }


                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                                                                PaymentMeanCreditCardProviderType.pmccpTransbank)
                        {
                            string errorMessage = "";
                            TransBankPayments.TransBankErrorCode eErrorCode;
                            TransBankPayments oPayments = new TransBankPayments();


                            bRes = oPayments.DeleteToken(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_ENVIRONMENT,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_COMMERCECODE,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PUBLICCERT_FILE,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_WEBPAYCERT_FILE,
                                                                    oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PASSWORD,
                                                                    oPendingTransactionOperation.PTROP_EMAIL,
                                                                    oPendingTransactionOperation.PTROP_TOKEN_CARD_REFERENCE,
                                                                    out eErrorCode,
                                                                    out errorMessage);


                            if (TransBankPayments.IsError(eErrorCode))
                            {
                                string errorCode = eErrorCode.ToString();

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("TokenDeletion.Transbank.DeleteToken : errorCode={0} ; errorMessage={1}",
                                          errorCode, errorMessage));
                            }
                            else
                            {
                                bRes = true;
                            }

                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                    PaymentMeanCreditCardProviderType.pmccpBSRedsys)
                        {
                            // TODO: ...
                            bRes = true;
                        }
                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                                                               PaymentMeanCreditCardProviderType.pmccpPaysafe)
                        {


                            PaysafePayments oPayments = new PaysafePayments();


                            var oPaysafeConfig = new PaysafePayments.PaysafeMerchantInfo(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ACCOUNT_NUMBER,
                                                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_KEY,
                                                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_API_SECRET,
                                                                                         oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYSAFE_CONFIGURATION.PYSCON_ENVIRONMENT);


                            Paysafe.CustomerVault.Profile oProfile = null;
                            string sErrorMessage = "";

                            if (oPayments.GetProfileFromMerchantCustomerId(oPaysafeConfig, oPendingTransactionOperation.PTROP_TOKEN_CARD_HASH, out oProfile, out sErrorMessage))
                            {

                                bRes = oPayments.DeleteProfile(oPaysafeConfig, oProfile, out sErrorMessage);
                            }

                        }

                        else if ((PaymentMeanCreditCardProviderType)oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER ==
                                                                                                                              PaymentMeanCreditCardProviderType.pmccpMercadoPago)
                        {


                            MercadoPagoPayments.MercadoPagoErrorCode eErrorCode = MercadoPagoPayments.MercadoPagoErrorCode.InternalError;
                            string strErrorMessage = "";

                            MercadoPagoPayments cardPayment = new MercadoPagoPayments();


                            bRes = cardPayment.DeleteCardToken(oPendingTransactionOperation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_ACCESS_TOKEN,
                                                                 oPendingTransactionOperation.PTROP_TOKEN_CARD_HASH,
                                                                 oPendingTransactionOperation.PTROP_TOKEN_CARD_REFERENCE,
                                                                 out eErrorCode,
                                                                 out strErrorMessage);


                            if (MercadoPagoPayments.IsError(eErrorCode))
                            {
                                string errorCode = eErrorCode.ToString();

                                m_Log.LogMessage(LogLevels.logERROR, string.Format("TokenDeletion.Transbank.DeleteToken : errorCode={0} ; errorMessage={1}",
                                          errorCode, strErrorMessage));

                            }
                            else
                            {
                                bRes = true;
                            }

                        }

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::Refund: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }



        private bool PerformPrepayRecharge(CUSTOMER_PAYMENT_MEAN oPaymentMean, PaymentMeanRechargeCreationType rechargeCreationType )
        {
            bool bRes = false;

            try
            {
                USER oUser = oPaymentMean.CUSTOMER.USER;
                decimal? dRechargeId;


                if ((oPaymentMean != null) &&
                    (oPaymentMean.CUSPM_ENABLED == 1) &&
                    (oPaymentMean.CUSPM_VALID == 1) &&
                    (oPaymentMean.CUSPM_AUTOMATIC_RECHARGE==1))
                {

                    int iAmountToRecharge = oPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value;

                    if (oPaymentMean.CUSPM_AMOUNT_TO_RECHARGE + oPaymentMean.CUSTOMER.USER.USR_BALANCE < oPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS)
                    {
                        iAmountToRecharge = (oPaymentMean.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS.Value - oPaymentMean.CUSTOMER.USER.USR_BALANCE)
                                            + oPaymentMean.CUSPM_AMOUNT_TO_RECHARGE.Value;
                    }

                    decimal dPercVAT1;
                    decimal dPercVAT2;
                    decimal dPercFEE;
                    decimal dPercFEETopped;
                    decimal dFixedFEE;
                    int? iPaymentTypeId = null;
                    int? iPaymentSubtypeId = null;
                    if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                    {
                        iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                        iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                    }

                    bRes = true;

                    if (!customersRepository.GetFinantialParams(oUser, "", iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.BalanceRecharge,
                                                                out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
                    {
                        bRes = false;                        
                        m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::PerformPrepayRecharge::Error : Error getting finantial parameters.");
                    }

                    int iPartialVAT1;
                    int iPartialPercFEE;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;


                    int iTotalQuantity = customersRepository.CalculateFEE(iAmountToRecharge, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE);

                    NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                    numberFormatProvider.NumberDecimalSeparator = ".";
                    decimal dQuantity = Convert.ToDecimal(iAmountToRecharge, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CURRENCy.CUR_ISO_CODE);
                    decimal dQuantityToCharge = Convert.ToDecimal(iTotalQuantity, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CURRENCy.CUR_ISO_CODE);

                    if (bRes)
                    {
                        bRes = false;

                        if ((PaymentMeanType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID == PaymentMeanType.pmtDebitCreditCard)
                        {



                            string strUserReference = null;
                            string strAuthCode = null;
                            string strAuthResult = null;
                            string strAuthResultDesc = "";
                            string strGatewayDate = null;
                            string strTransactionId = null;
                            string strCardScheme = null;
                            string strCFTransactionID = null;

                            bool bPayIsCorrect = false;
                            PaymentMeanRechargeStatus rechargeStatus = PaymentMeanRechargeStatus.Waiting_Commit;

                            if ((PaymentMeanCreditCardProviderType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                PaymentMeanCreditCardProviderType.pmccpCreditCall)
                            {
                                CardEasePayments cardPayment = new CardEasePayments();

                                bPayIsCorrect = cardPayment.AutomaticPayment(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TERMINAL_ID,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_TRANSACTION_KEY,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_URL,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CREDIT_CALL_CONFIGURATION.CCCON_CARDEASE_TIMEOUT, 
                                                                            oUser.USR_EMAIL,
                                                                            dQuantityToCharge,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                            false,
                                                                            out strUserReference,
                                                                            out strAuthCode,
                                                                            out strAuthResult,
                                                                            out strGatewayDate,
                                                                            out strCardScheme,
                                                                            out strTransactionId);

                            }
                            else if ((PaymentMeanCreditCardProviderType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                PaymentMeanCreditCardProviderType.pmccpIECISA)
                            {
                                int iQuantityToRecharge = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE));
                                DateTime dtNow = DateTime.Now;
                                IECISAPayments.IECISAErrorCode eErrorCode;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                IECISAPayments cardPayment = new IECISAPayments();
                                string strErrorMessage = "";

                                cardPayment.StartAutomaticTransaction(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_FORMAT_ID,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_USER,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_MERCHANT_ID,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_INSTANCE,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_CENTRE_ID,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_POS_ID,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CUSTOMER_ID,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_CF_TEMPLATE,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                   oUser.USR_EMAIL,
                                                   iQuantityToRecharge,
                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE,
                                                   infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE),
                                                   dtNow,
                                                   out eErrorCode,
                                                   out strErrorMessage,
                                                   out strTransactionId,
                                                   out strUserReference);

                                if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                {
                                    string errorCode = eErrorCode.ToString();

                                    m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPrepayRecharge.StartWebTransaction : errorCode={0} ; errorMessage={1}",
                                              errorCode, strErrorMessage));


                                }
                                else
                                {
                                    string strRedirectURL = "";
                                    cardPayment.GetWebTransactionPaymentTypes(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                                            strTransactionId,
                                                                            out eErrorCode,
                                                                            out strErrorMessage,
                                                                            out strRedirectURL);
                                    if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                    {
                                        string errorCode = eErrorCode.ToString();

                                        m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPrepayRecharge.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                                  errorCode, strErrorMessage));


                                    }
                                    else
                                    {

                                        customersRepository.StartRecharge(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                                   oUser.USR_EMAIL,
                                                                                   dtUTCNow,
                                                                                   dtNow,
                                                                                   iQuantityToRecharge,
                                                                                   oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ID,
                                                                                   "",
                                                                                   strUserReference,
                                                                                   strTransactionId,
                                                                                   "",
                                                                                   "",
                                                                                   "",
                                                                                   PaymentMeanRechargeStatus.Committed);

                                        DateTime? dtTransactionDate = null;
                                        cardPayment.CompleteAutomaticTransaction(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_URL,
                                                                               oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_SERVICE_TIMEOUT,
                                                                               oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.IECISA_CONFIGURATION.IECCON_MAC_KEY,
                                                                               strTransactionId,
                                                                              out eErrorCode,
                                                                              out strErrorMessage,
                                                                              out dtTransactionDate,
                                                                              out strCFTransactionID,
                                                                              out strAuthCode);


                                        if (eErrorCode != IECISAPayments.IECISAErrorCode.OK)
                                        {
                                            string errorCode = eErrorCode.ToString();

                                            m_Log.LogMessage(LogLevels.logERROR, string.Format("PerformPrepayRecharge.GetWebTransactionPaymentTypes : errorCode={0} ; errorMessage={1}",
                                                      errorCode, strErrorMessage));



                                        }
                                        else
                                        {

                                            strAuthResult = "succeeded";
                                            customersRepository.CompleteStartRecharge(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                                      oUser.USR_EMAIL,
                                                                                      strTransactionId,
                                                                                      strAuthResult,
                                                                                      strCFTransactionID,
                                                                                      dtTransactionDate.Value.ToString("HHmmssddMMyyyy"),
                                                                                      strAuthCode,
                                                                                      PaymentMeanRechargeStatus.Committed);
                                            strGatewayDate = dtTransactionDate.Value.ToString("HHmmssddMMyyyy");
                                            rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                            bPayIsCorrect = true;
                                            strCardScheme = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA;

                                        }
                                    }

                                }
                                

                               

                            }
                            else if ((PaymentMeanCreditCardProviderType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                    PaymentMeanCreditCardProviderType.pmccpStripe)
                            {

                                string result = "";
                                string errorMessage = "";
                                string errorCode = "";
                                string strPAN = "";
                                string strExpirationDateMonth = "";
                                string strExpirationDateYear = "";
                                string strCustomerId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH;

                                int iQuantityToRecharge = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE));
                                bPayIsCorrect = StripePayments.PerformCharge(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.STRIPE_CONFIGURATION.STRCON_SECRET_KEY,
                                                                            oUser.USR_EMAIL,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                            ref strCustomerId,
                                                                            iQuantityToRecharge,
                                                                            oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE,
                                                                            false,
                                                                            out result,
                                                                            out errorCode,
                                                                            out errorMessage,
                                                                            out strCardScheme,
                                                                            out strPAN,
                                                                            out strExpirationDateMonth,
                                                                            out strExpirationDateYear,
                                                                            out strTransactionId,
                                                                            out strGatewayDate);

                                if (bPayIsCorrect)
                                {
                                    strUserReference = strTransactionId;
                                    strAuthCode = "";
                                    strAuthResult = "succeeded";

                                }

                            }
                            else if ((PaymentMeanCreditCardProviderType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                        PaymentMeanCreditCardProviderType.pmccpMoneris)
                            {
                                MonerisPayments cardPayment = new MonerisPayments();
                                string errorMessage = "";
                                MonerisPayments.MonerisErrorCode eErrorCode = MonerisPayments.MonerisErrorCode.InternalError;

                                NumberFormatInfo provider = new NumberFormatInfo();
                                string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE), provider);

                                strUserReference = MonerisPayments.UserReference();



                                if (oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_3DS_TRANSACTIONS != 0)
                                {

                                    string strFormURL = oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_FORM_URL;
                                    string strBaseURL = strFormURL.Substring(0, strFormURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/MonerisMPIResponse";
                                    string strInlineForm = "";
                                    string strMDStep1 = "";


                                    bPayIsCorrect = cardPayment.AutomaticTransactionMPIStep1(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                         oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                         strUserReference,
                                         oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                         oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE == oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH ? "" : oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                         strAmount,
                                         oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY, "",
                                         oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA,
                                         oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_EXPIRATION_DATE.Value,
                                        strReturnURL, "Mozilla",
                                         oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                         oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                        out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate, out strInlineForm, out strMDStep1);


                                    if ((bPayIsCorrect) && (!string.IsNullOrEmpty(strInlineForm)))
                                    {


                                        bPayIsCorrect = cardPayment.AutomaticTransaction(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                                        strUserReference,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE == oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH ? "" : oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                        strAmount,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                                        "",
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0, "",
                                                                        out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate);
                                    }
                                }
                                else
                                {
                                    bPayIsCorrect = cardPayment.AutomaticTransaction(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                                                    oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                                                    strUserReference,
                                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE == oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH ? "" : oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                    strAmount,
                                                                    oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                                                    "",
                                                                    oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                                                    oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0, "",
                                                                    out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate);


                                }


                                if (bPayIsCorrect)
                                {
                                    bPayIsCorrect = !MonerisPayments.IsError(eErrorCode);
                                    DateTime dt = DateTime.ParseExact(strGatewayDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    strGatewayDate = dt.ToString("HHmmssddMMyyyy");
                                    strCardScheme = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA;

                                    rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                    DateTime dtNow = DateTime.Now;
                                    DateTime dtUTCNow = DateTime.UtcNow;
                                    int iQuantityToCharge = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE));
                                    customersRepository.StartRecharge(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                    oUser.USR_EMAIL,
                                                                    dtUTCNow,
                                                                    dtNow,
                                                                    iQuantityToCharge,
                                                                    oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ID,
                                                                    strAuthResult,
                                                                    strUserReference,
                                                                    strTransactionId,
                                                                    "",
                                                                    strGatewayDate,
                                                                    strAuthCode,
                                                                    PaymentMeanRechargeStatus.Committed);
                                }

                                

                            }
                            else if ((PaymentMeanCreditCardProviderType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                   PaymentMeanCreditCardProviderType.pmccpPayu)
                            {
                                

                                PayuPayments cardPayment = new PayuPayments();
                                string errorMessage = "";
                                PayuPayments.PayuErrorCode eErrorCode = PayuPayments.PayuErrorCode.InternalError;
                                DateTime? dtTransaction = null;


                                string strLang = ((oUser.USR_CULTURE_LANG.ToLower() ?? "").Length >= 2) ? oUser.USR_CULTURE_LANG.Substring(0, 2) : "es"; 
                               

                                bPayIsCorrect = cardPayment.AutomaticTransaction(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_URL,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_KEY,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_API_LOGIN,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_ACCOUNT_ID,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_MERCHANT_ID,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_SERVICE_TIMEOUT,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_COUNTRY,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.PAYU_CONFIGURATION.PAYUCON_IS_TEST != 1 ? false : true,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                                        PayuPayments.Language(strLang),
                                                                        oUser.USR_EMAIL,
                                                                        dQuantityToCharge,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE,
                                                                        "RECARGA IPARKME",
                                                                        "",
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA,
                                                                        oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_NAME,
                                                                        //oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                                        "",
                                                                        (!String.IsNullOrEmpty(oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CARD_SECURITY_CODE)?DecryptCryptResult(oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CARD_SECURITY_CODE,ConfigurationManager.AppSettings["CryptKey"]):String.Empty),
                                                                        out eErrorCode,
                                                                        out errorMessage,
                                                                        out strTransactionId,
                                                                        out strUserReference,
                                                                        out strAuthCode,
                                                                        out dtTransaction);

                                if (bPayIsCorrect)
                                {
                                    strGatewayDate = dtTransaction.Value.ToString("HHmmssddMMyy");
                                    strCardScheme = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA;
                                    rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                    DateTime dtNow = DateTime.Now;
                                    DateTime dtUTCNow = DateTime.UtcNow;
                                    int iQuantityToCharge = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE));
                                    customersRepository.StartRecharge(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                              oUser.USR_EMAIL,
                                                                              dtUTCNow,
                                                                              dtNow,
                                                                              iQuantityToCharge,
                                                                              oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ID,
                                                                              strAuthResult,
                                                                              strUserReference,
                                                                              strTransactionId,
                                                                              "",
                                                                              strGatewayDate,
                                                                              strAuthCode,
                                                                              PaymentMeanRechargeStatus.Committed);
                                }

                                

                                if (bPayIsCorrect)
                                {
                                    bPayIsCorrect = !PayuPayments.IsError(eErrorCode);
                                }
                                

                            }
                            else if ((PaymentMeanCreditCardProviderType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER ==
                                                                  PaymentMeanCreditCardProviderType.pmccpTransbank)
                            {
                                TransBankPayments cardPayment = new TransBankPayments();
                                string errorMessage = "";
                                TransBankPayments.TransBankErrorCode eErrorCode = TransBankPayments.TransBankErrorCode.InternalError;

                                NumberFormatInfo provider = new NumberFormatInfo();
                                string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE), provider);


                                strUserReference = TransBankPayments.UserReference();

                                bPayIsCorrect = cardPayment.AutomaticTransaction(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_ENVIRONMENT,
                                                                              oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_COMMERCECODE,
                                                                              oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PUBLICCERT_FILE,
                                                                              oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_WEBPAYCERT_FILE,
                                                                              oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.TRANSBANK_CONFIGURATION.TRBACON_PASSWORD,
                                                                              oUser.USR_EMAIL,
                                                                              oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                                              strUserReference,
                                                                              strAmount,
                                                                              out eErrorCode,
                                                                              out errorMessage,
                                                                              out strTransactionId,
                                                                              out strAuthCode,
                                                                              out strGatewayDate);


                              

                                if (bPayIsCorrect)
                                {
                                    bPayIsCorrect = !TransBankPayments.IsError(eErrorCode);
                                    DateTime dt = DateTime.ParseExact(strGatewayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                    strGatewayDate = dt.ToString("HHmmssddMMyyyy");
                                    strCardScheme = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_SCHEMA;

                                    rechargeStatus = PaymentMeanRechargeStatus.Committed;
                                    DateTime dtNow = DateTime.Now;
                                    DateTime dtUTCNow = DateTime.UtcNow;
                                    int iQuantityToCharge = Convert.ToInt32(dQuantityToCharge * infraestructureRepository.GetCurrencyDivisorFromIsoCode(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ISO_CODE));
                                    customersRepository.StartRecharge(oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_ID,
                                                                              oUser.USR_EMAIL,
                                                                              dtUTCNow,
                                                                              dtNow,
                                                                              iQuantityToCharge,
                                                                              oUser.CUSTOMER_PAYMENT_MEAN.CURRENCy.CUR_ID,
                                                                              "",
                                                                              strUserReference,
                                                                              strTransactionId,
                                                                              "",
                                                                              strGatewayDate,
                                                                              strAuthCode,
                                                                              PaymentMeanRechargeStatus.Committed);
                                }

                            }



                            if (bPayIsCorrect)
                            {
                                int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                                int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                                lock (customersRepository)
                                {
                                    if (!customersRepository.RechargeUserBalance(ref oUser,
                                                    oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG,
                                                    Convert.ToInt32(MobileOS.Web),
                                                    true,
                                                    iAmountToRecharge,iAmountToRecharge,
                                                    dPercVAT1, dPercVAT2, iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity,                                                    
                                                    oUser.CURRENCy.CUR_ID,
                                                    PaymentSuscryptionType.pstPrepay,
                                                    rechargeStatus,
                                                    rechargeCreationType,
                                                    strUserReference,
                                                    strTransactionId,
                                                    strCFTransactionID,
                                                    strGatewayDate,
                                                    strAuthCode,
                                                    strAuthResult,
                                                    strAuthResultDesc,
                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_HASH,
                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_REFERENCE,
                                                    strCardScheme,
                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_MASKED_CARD_NUMBER,
                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_NAME,
                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_DOCUMENT_ID,
                                                    oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_CARD_EXPIRATION_DATE,
                                                    null,
                                                    null,
                                                    null,
                                                    false,
                                                    null,
                                                    null,
                                                    null, "", "", "", "", "", null, null, oUser.USR_LAST_SOAPP_ID.Value,
                                                    infraestructureRepository,
                                                    out dRechargeId,
                                                    true))
                                    {
                                        m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::PerformPrepayRecharge::Error");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (dRechargeId != null)
                                            {
                                                CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = null;
                                                if (customersRepository.GetRechargeData(ref oUser, dRechargeId.Value, out oRecharge))
                                                {
                                                    if ((PaymentSuscryptionType)oRecharge.CUSPMR_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPrepay)
                                                    {
                                                        string culture = oUser.USR_CULTURE_LANG;
                                                        CultureInfo ci = new CultureInfo(culture);
                                                        Thread.CurrentThread.CurrentUICulture = ci;
                                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

                                                        int iQuantity = oRecharge.CUSPMR_AMOUNT;
                                                        dPercVAT1 = oRecharge.CUSPMR_PERC_VAT1 ?? 0;
                                                        dPercVAT2 = oRecharge.CUSPMR_PERC_VAT2 ?? 0;
                                                        dPercFEE = oRecharge.CUSPMR_PERC_FEE ?? 0;
                                                        iPercFEETopped = (int)(oRecharge.CUSPMR_PERC_FEE_TOPPED ?? 0);
                                                        iFixedFEE = (int)(oRecharge.CUSPMR_FIXED_FEE ?? 0);

                                                        iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                                                        int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                                                        if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
                                                        iQFEE += iFixedFEE;
                                                        int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                                                        int iQSubTotal = iQuantity + iQFEE;

                                                        int iLayout = 0;
                                                        if (iQFEE != 0 || iQVAT != 0)
                                                        {
                                                            OPERATOR oOperator = customersRepository.GetDefaultOperator();
                                                            if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                                                        }


                                                        string sLayoutSubtotal = "";
                                                        string sLayoutTotal = "";

                                                        string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID));

                                                        if (iLayout == 2)
                                                        {
                                                            sLayoutSubtotal = string.Format(Resources.Email_LayoutSubtotal,
                                                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                                                            (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                                            (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode));
                                                        }
                                                        else if (iLayout == 1)
                                                        {
                                                            sLayoutTotal = string.Format(Resources.Email_LayoutTotal,
                                                                                         string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                                                         string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode),
                                                                                         (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                                         (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                                         string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), sCurIsoCode));
                                                        }

                                                        string strRechargeEmailSubject = Resources.ConfirmAutomaticRecharge_EmailHeader;
                                                        /*
                                                            ID: {0}<br>
                                                         *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                                                         *  Cantidad Recargada: {2} 
                                                         */
                                                        string strRechargeEmailBody = string.Format(Resources.ConfirmRecharge_EmailBody,
                                                            oRecharge.CUSPMR_ID,
                                                            oRecharge.CUSPMR_DATE,
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                                          infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                                            string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                                infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                            sLayoutSubtotal, sLayoutTotal);

                                                        SendEmail(ref oUser, strRechargeEmailSubject, strRechargeEmailBody, oUser.USR_LAST_SOAPP_ID.Value);

                                                    }
                                                }
                                            }
                                        }
                                        catch { }

                                        bRes = true;
                                    }
                                }

                            }
                            else
                            {
                                lock (customersRepository)
                                {
                                    customersRepository.AutomaticRechargeFailure(ref oUser);
                                    m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::PerformPrepayRecharge::Gateway Error");
                                }

                            }

                        }
                        else if (((PaymentMeanType)oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID == PaymentMeanType.pmtPaypal) &&
                            (oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_AUTOMATIC_RECHARGE == 1))
                        {
                            //PAypal



                        }
                        else
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::PerformPrepayRecharge::Error");
                        }
                    }
                }
                else
                {
                    m_Log.LogMessage(LogLevels.logERROR, "CintegraMobilePaymentsManager::PerformPrepayRecharge::Error");
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobilePaymentsManager::PerformPrepayRecharge::Exception: {0}", e.Message));

            }


            return bRes;

        }


        private bool SendEmail(ref USER oUser, string strEmailSubject, string strEmailBody, decimal dSourceApp)
        {
            bool bRes = true;
            try
            {

                long lSenderId = infraestructureRepository.SendEmailTo(oUser.USR_EMAIL, strEmailSubject, strEmailBody, dSourceApp);

                if (lSenderId > 0)
                {
                    customersRepository.InsertUserEmail(ref oUser, oUser.USR_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                }

            }
            catch
            {
                bRes = false;
            }

            return bRes;
        }

        private string GetEmailSourceAppEmailPrefix(decimal dSourceApp)
        {
            string strRes = "";

            decimal dDefaultSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();
            if (dSourceApp != dDefaultSourceApp)
            {
                strRes = geograficAndTariffsRepository.GetSourceAppCode(dSourceApp) + "_";
            }

            return strRes;
        }

        protected string CalculateCryptResult(string strInput, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;

                int iKeyLength = 32;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }


                byte[] _iv = null;

                int iIVLength = 16;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _iv = new byte[iIVLength];
                iSum = 0;

                for (int i = 0; i < iIVLength; i++)
                {
                    if (i < ivBytes.Length)
                    {
                        iSum += ivBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _iv[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER2) % (Byte.MaxValue + 1));

                }

                strRes = ByteArrayToString(EncryptStringToBytes_Aes(strInput, _normKey, _iv));



            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR,"CalculateHash::Exception",e);

            }


            return strRes;
        }


        public string DecryptCryptResult(string strHexByteArray, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;

                int iKeyLength = 32;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }


                byte[] _iv = null;

                int iIVLength = 16;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _iv = new byte[iIVLength];
                iSum = 0;

                for (int i = 0; i < iIVLength; i++)
                {
                    if (i < ivBytes.Length)
                    {
                        iSum += ivBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _iv[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER2) % (Byte.MaxValue + 1));

                }

                strRes = DecryptStringFromBytes_Aes(StringToByteArray(strHexByteArray), _normKey, _iv);



            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CalculateHash::Exception", e);

            }


            return strRes;
        }
        protected static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }

        protected static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        protected static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        protected static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
		#endregion



    }
}
