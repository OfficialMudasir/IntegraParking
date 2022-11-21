using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Ninject;
//using integraMobileUserReplicationService.Properties;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.ExternalWS;
using System.Diagnostics;


namespace integraMobileUserReplicationService
{

    class CintegraMobileUserReplicationManager
    {
        #region -- Constant definitions --


        const System.String ct_POOLTIME_CONFIRMATIONS_MANAGER_TAG = "PoolingConfirmationsManager";

        const System.String ct_STOPTIME_TAG = "Stoptime";
        const System.String ct_RETRIES_TAG = "Retries";
        const System.String ct_RESENDTIME_TAG = "RetriesTime";
        const System.String ct_MAXRESENDTIME_TAG = "MaxRetriesTime";
        const System.String ct_MAXWORKINGTHREADS_TAG = "MaxWorkingThreads";
        const System.String ct_CONFIRM_WAIT_TIME = "ConfirmWaitTime";
        const System.String ct_MAXUSERSTOSEND_TAG = "ZendeskMaxUsersToSend";
        const System.String ct_QUEUEDTIMETOWAITINSECONDS_TAG = "ZendeskQueuedWaitTimeInSeconds";
        const System.String ct_APIQUERIESPERMINUTE_TAG = "ZendeskMaxAPIQueriesPerMinute";



        #endregion

        #region -- Member Variables --     
  
        private IKernel m_kernel = null;

        [Inject]
        public ICustomersRepository customersRepository { get; set; }

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CintegraMobileUserReplicationManager));

        //Thread Signal: Stop service event
        private static ManualResetEvent m_evStopServer = new ManualResetEvent(false);

        //Thread to managing operation and fine confirmations
        private Thread m_ConfirmationManagerThread;

        // Time to pooling for Managing Transactions
        private int m_iPoolTimeConfirmationManager;
        
        // Time to wait thread termination before stop the server
        private int m_iStopTime;

        // Num of retries
        private static int m_iRetries;

        //Time between retries
        private int m_iResendTime;
        private int m_iMaxResendTime;

        private int m_iMaxWorkingThreads;

        private int m_iConfirmWaitTime;

        private int m_iMaxUsersToSend;

        private int m_iQueuedTimeWaitingInSeconds;

        private int m_iMaxAPIQueriesPerMenute;

        private static ManualResetEvent m_evPushBrokerEvent = new ManualResetEvent(false);

        private ThirdPartyUser m_ThirdPartyUser = null;
        private Hashtable m_oHashUserReps = null;


        #endregion

		#region -- Constructor / Destructor --

      
        public CintegraMobileUserReplicationManager()
		{
            m_iPoolTimeConfirmationManager = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_CONFIRMATIONS_MANAGER_TAG].ToString());
            m_iStopTime             =  Convert.ToInt32(ConfigurationManager.AppSettings[ct_STOPTIME_TAG].ToString());
            m_iRetries              = Convert.ToInt32(ConfigurationManager.AppSettings[ct_RETRIES_TAG].ToString());
            m_iResendTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_RESENDTIME_TAG].ToString());
            m_iMaxResendTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXRESENDTIME_TAG].ToString());
            m_iMaxWorkingThreads = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXWORKINGTHREADS_TAG].ToString());
            m_iMaxUsersToSend = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXUSERSTOSEND_TAG].ToString());
            m_iQueuedTimeWaitingInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings[ct_QUEUEDTIMETOWAITINSECONDS_TAG].ToString());
            m_iMaxAPIQueriesPerMenute = Convert.ToInt32(ConfigurationManager.AppSettings[ct_APIQUERIESPERMINUTE_TAG].ToString());


            try
            {
                m_iConfirmWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_CONFIRM_WAIT_TIME].ToString());
            }
            catch
            {
                m_iConfirmWaitTime = 10;
            }

            

            m_kernel = new StandardKernel(new integraMobileConfirmationModule());
            m_kernel.Inject(this);

            m_ThirdPartyUser = new ThirdPartyUser();

            m_oHashUserReps = new Hashtable();

        }
        
        #endregion 

        #region -- Threads Bodies --

        public void Start()
		{
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileUserReplicationManager::Start");


            m_ConfirmationManagerThread = new Thread(new ThreadStart(this.ConfirmationManagerThread));
            m_ConfirmationManagerThread.Start();

            
            
            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileUserReplicationManager::Start");
        }

        public void Stop()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileUserReplicationManager::Stop");

            m_evStopServer.Set();

            m_ConfirmationManagerThread.Join(1000 * m_iStopTime);
            m_evStopServer.Reset();
          

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileUserReplicationManager::Stop");
        }




        protected void ConfirmationManagerThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileUserReplicationManager::ConfirmationManagerThread");

            List<stUserReplicationResult> oUsersRep = null;
            int iQueueLengthOps = 0;
            int iCurrentWaitTime = 0;
            int iAPiQueries = 0;
            bool bNeedToWait = false;
            Stopwatch oWatch = Stopwatch.StartNew();
            int iLastLoopQueries = 0;
            oWatch.Reset();


            bool bFinishServer = false;
            while (bFinishServer == false)
            {

                bFinishServer = (m_evStopServer.WaitOne(iCurrentWaitTime, false));
                bNeedToWait = false;
                iCurrentWaitTime = 0;
                iLastLoopQueries = 0;

                if (!bFinishServer)
                {
                    string strUsername = "";
                    string strPassword = "";
                    customersRepository.GetWaitingQueuedUserReplications(out oUsersRep, out iQueueLengthOps, UserReplicationWSSignatureType.urst_Zendesk, m_iQueuedTimeWaitingInSeconds, ref strUsername, ref strPassword);
                    while ((!bFinishServer) && (!bNeedToWait) &&(oUsersRep != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if ((!bFinishServer) && (oUsersRep != null))
                        {
                            if (!oWatch.IsRunning)
                            {
                                oWatch.Start();
                                iAPiQueries = 0;
                            }                            

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileUserReplicationManager::ConfirmationManagerThread: ZendeskCompleteQueuedUsers --> CurrentQueue={0}", iQueueLengthOps - oUsersRep.Count()));
                            ZendeskCompleteQueuedUsers(oUsersRep, strUsername, strPassword);
                            iAPiQueries++;
                            iLastLoopQueries++;
                            oUsersRep.Clear();
                            oUsersRep = null;

                            if ((iAPiQueries > m_iMaxAPIQueriesPerMenute) && (oWatch.ElapsedMilliseconds < 60000))
                            {
                                bNeedToWait = true;
                                iCurrentWaitTime = 60000 - (int)oWatch.ElapsedMilliseconds;
                                oWatch.Reset();
                                iAPiQueries = 0;
                            }
                            else if  (oWatch.ElapsedMilliseconds >= 60000)
                            {
                                oWatch.Reset();
                                oWatch.Start();
                                iAPiQueries = 0;
                            }



                        }

                        bFinishServer = (m_evStopServer.WaitOne(0, false));

                        if ((!bFinishServer)&&(!bNeedToWait))
                            customersRepository.GetWaitingQueuedUserReplications(out oUsersRep, out iQueueLengthOps, UserReplicationWSSignatureType.urst_Zendesk, m_iQueuedTimeWaitingInSeconds, ref strUsername, ref strPassword);
                    }

                }


                if (!bNeedToWait)
                {

                    bFinishServer = (m_evStopServer.WaitOne(0, false));

                    if (!bFinishServer)
                    {
                        customersRepository.GetWaitingUserReplications(out oUsersRep, out iQueueLengthOps, UserReplicationWSSignatureType.urst_Zendesk, m_iResendTime, m_iRetries, m_iMaxResendTime, m_iConfirmWaitTime, m_iMaxUsersToSend);
                        if (oUsersRep != null)
                        {
                            if (!oWatch.IsRunning)
                            {
                                oWatch.Start();
                                iAPiQueries = 0;
                            }

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileUserReplicationManager::ConfirmationManagerThread: ZendeskReplicateUsers --> CurrentQueue={0}", iQueueLengthOps - oUsersRep.Count()));
                            ZendeskReplicateUsers(oUsersRep, iQueueLengthOps);
                            iAPiQueries++;
                            iLastLoopQueries++;
                            oUsersRep.Clear();
                            oUsersRep = null;

                            if ((iAPiQueries > m_iMaxAPIQueriesPerMenute) && (oWatch.ElapsedMilliseconds < 60000))
                            {
                                bNeedToWait = true;
                                iCurrentWaitTime = 60000 - (int)oWatch.ElapsedMilliseconds;
                                oWatch.Reset();
                                iAPiQueries = 0;
                            }
                            else if  (oWatch.ElapsedMilliseconds >= 60000)
                            {
                                oWatch.Reset();
                                oWatch.Start();
                                iAPiQueries = 0;
                            }

                        }

                    }                   
                }
                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (iLastLoopQueries == 0)
                {
                    oWatch.Reset();
                    iCurrentWaitTime = m_iPoolTimeConfirmationManager;
                    iAPiQueries = 0;
                }
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileUserReplicationManager::ConfirmationManagerThread");
        }

        private void ZendeskReplicateUsers(List<stUserReplicationResult> oUsersReps, int iQueueLength)
        {

            try
            {
                DateTime datConfirmDate = new DateTime(1900, 1, 1);
                string strURL = "";
                string strUsername = "";
                string strPassword = "";
             
                Dictionary<string, object> oUsersDataDict = null;
                ResultType rtRes = ResultType.Result_OK;
                if (customersRepository.GetZendeskUserDataDict(ref oUsersReps, ref oUsersDataDict, ref strURL, ref strUsername, ref strPassword))
                {
                    if (oUsersDataDict != null)
                    {
                        rtRes = m_ThirdPartyUser.ZendeskUserReplication(ref oUsersReps, ref oUsersDataDict, strURL, strUsername, strPassword, iQueueLength);
                        customersRepository.UpdateUserReplications(ref oUsersReps, m_iRetries);

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileUserReplicationManager::ZendeskReplicateUsers: Exception {0}", e.Message));
            }
            finally
            {
                
            }

        }

        private void ZendeskCompleteQueuedUsers(List<stUserReplicationResult> oUsersReps,string strUsername, string strPassword)
        {

            try
            {
                DateTime datConfirmDate = new DateTime(1900, 1, 1);

                ResultType rtRes = ResultType.Result_OK;
                rtRes = m_ThirdPartyUser.ZendeskUpdateQueuedReplications(ref oUsersReps,strUsername, strPassword);
                customersRepository.UpdateUserReplications(ref oUsersReps, m_iRetries);

               
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileUserReplicationManager::ZendeskCompleteQueuedUsers: Exception {0}", e.Message));
            }
            finally
            {

            }

        }

		#endregion

    }
}
