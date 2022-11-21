using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobileNotificationService.Properties;
using Ninject;
using PushSharp;
using PushSharp.Google;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using dotAPNS;



namespace integraMobileNotificationService
{
    public class Http2CustomHandler : WinHttpHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Version = new Version("2.0");
            return base.SendAsync(request, cancellationToken);
        }
    }

    class CintegraMobileNotifierTag
    {
        public IInfraestructureRepository Repository { get; set; }
        public decimal dPushNotifID;
        public string strRawData;
    }


    class APNSInfo
    {
        public string BundleId;
        public string CertFilePath;
        public string KeyId;
        public string TeamId;
        public ApnsJwtOptions apnsConfig;
        public ApnsClient apnsClient;
        public DateTime dtClientUTC;        
    }

    class CintegraMobileNotifier
    {
        #region -- Constant definitions --
        const string ct_POOLTIMENOTIFICATIONGENERATOR_TAG = "PoolingTimeNotificationGeneration";
        const string ct_POOLTIMENOTIFICATIONS_TAG = "PoolingTimeNotifications";
        const string ct_POOLTIMEPLATESSENDING_TAG = "PoolingTimePlatesSending";
        const string ct_PASSWORD_RECOVERY_NUM_MINUTES_TAG = "PasswordRecoveryNumMinutes";
        const string ct_POOL_NOTIFIER_MANAGER_TAG = "PoolingNotifierManager";
        const string ct_MAX_NOTIFIER_WORKING_THREADS_TAG = "MaxNotifierWorkingThreads";
        const string ct_STOPTIME_TAG = "Stoptime";
        const string ct_RETRIES_TAG = "Retries";
        const string ct_RESENDTIME_TAG = "RetriesTime";
        const string ct_MAXRESENDTIME_TAG = "MaxRetriesTime";
        const string ct_MINUTESBEFOREENDTOWARNPARKING_TAG = "ParkingMinutesBeforeEndToWarn";
        const string ct_NUMMAXPLATESTOSEND_TAG = "NumOfPlatesToSend";
        const string ct_CONFIRM_WAIT_TIME = "ConfirmWaitTime";

        const string ct_MAX_RECORDS_RESOLVE_NOTIFIER_SENDER_SMS_TAG = "MaxOfRecordsToResolveByNotifierSenderSMS";
        const string ct_MAX_RECORDS_RESOLVE_USERS_WARNINGS = "MaxOfRecordsToResolveByUsersWarnings";
        const string ct_LITERAL_ID_SMS_TAG = "LiteralIdSMS";
        
        private const int DEFAULT_WS_TIMEOUT = 5000;
        static string _xmlTagName = "ipark";
        private const long BIG_PRIME_NUMBER = 2147483647;
        private const string IN_SUFIX = "_in";
        private const string OUT_SUFIX = "_out";
        private const int ct_APNS_CLIENT_GENERATION_MINUTES = 30;
        #endregion

        #region -- Member Variables --     
  
        private IKernel m_kernel = null;

        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infraestructureRepository { get; set; }
        [Inject]
        public IGeograficAndTariffsRepository geograficAndTariffsRepository { get; set; }

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CintegraMobileNotifier));

        //Thread Signal: Stop service event
        private static ManualResetEvent m_evStopServer = new ManualResetEvent(false);

        //Thread to NotificationsManager
        private Thread m_NofitierManagerThread;

        ////Thread to Create Notifications
        //private Thread m_NotifierGeneratorThread;

        ////Thread to Generate Notifications
        //private Thread m_NotifierMultiplierThread;

        ////Thread to Send Notifications
        //private Thread m_NotifierSenderThread;

        ////Thread to Send SMS Notifications
        //private Thread m_NotifierSenderSMSThread;

        ////Thread to Send Push User Warnings
        //private Thread m_UsersWarningsThread;
        
        ////Thread to Send Plates
        //private Thread m_PlatesSenderThread;

        // Time to pooling for sending Notification Generator
        private int m_iPoolTimeNotificationGenerator;
        
        // Time to pooling for sending Notifications
        private int m_iPoolTimeNotifications;

        // Time to pooling for sending Notifications
        private int m_iPoolTimePlatesSending;

        // Time to wait thread termination before stop the server
        private int m_iStopTime;

        // Num of retries
        private static int m_iRetries;

        //Time between retries
        private int m_iResendTime;
        private int m_iMaxResendTime;

        private int m_iConfirmWaitTime;

        //Max Number of plates to send simultaneously
        private int m_iMaxPlatesToSend;
        
        //Minutes before parking end to send warning
        private int m_iMinutesBeforeEndToSendWarning;

 		//Minutes before parking end to send warning
        private int m_iNumPasswordRecoveryNumMinutes;

        private Dictionary<decimal, GcmServiceBroker> m_GcmPushBrokers = null;
        private Dictionary<decimal, GcmServiceBroker> m_FireBasePushBrokers = null;
        private Dictionary<decimal, APNSInfo> m_ApnsPushInfos = null;


        private static int m_NumPushPendingOfFinishing=0;
        private static object m_NumPushPendingOfFinishingLock = new object();

        private static ManualResetEvent m_evPushBrokerEvent = new ManualResetEvent(false);

        //Number Of Records To Resolve 
        private int m_iMaxOfRecordsToResolveByNotifierSenderSMS;
        private int m_iMaxOfRecordsToResolveByUsersWarnings;

        private int m_iLiteralIdSMS;
        
        private static ulong _VERSION_2_13 = AppUtilities.AppVersion("2.13");

        private int m_iPoolingNotifierManager;
        //private Hashtable m_oHashNotifier = null;
        private int m_iMaxWorkingThreads;
        //private Dictionary<decimal, object> m_oNotifierLock = null;


        private Hashtable m_oHashNotifierManager = null;
        private Hashtable m_oHashNotifierExternalTicket = null;
        private Hashtable m_oHashInsertionParkingNotification = null;
        private Hashtable m_oHashBeforeEndParkingNotification = null;
        private Hashtable m_oHashOperationsOffStreetNotification = null;
        private Hashtable m_oHashUsersSecurityOperationNotification = null;
        private Hashtable m_oHashUserNotificationMultiplier = null;
        private static  Hashtable m_oHashPushIdNotificationSender = null;
        private Hashtable m_oHashUserOperationsSenderSMS = null;
        private Hashtable m_oHashUsersWarnings = null;

        private List<LITERAL_LANGUAGE> m_ListLiteralLanguage = null;
        #endregion

		#region -- Constructor / Destructor --

        public class NotifierExternalTicketParameter
        {
            public EXTERNAL_TICKET m_oExternarTicket = null;
            public int m_iQueueLength = 0;
        }

        public class InsertionParkingNotificationParameter
        {
            public EXTERNAL_PARKING_OPERATION m_oInsertionParking = null;
            public int m_iQueueLength = 0;
        }

        public class BeforeEndParkingNotificationParameter
        {
            public EXTERNAL_PARKING_OPERATION m_oBeforeEndParking = null;
            public int m_iQueueLength = 0;
        }

        public class OperationsOffStreetNotificationParameter
        {
            public OPERATIONS_OFFSTREET m_oOperationsOffStreet = null;
            public int m_iQueueLength = 0;
        }

        public class UsersSecurityOperationNotificationParameter
        {
            public USERS_SECURITY_OPERATION m_oUsersSecurityOperation = null;
            public int m_iQueueLength = 0;
        }

        public class UserNotificationMultiplierParameter
        {
            public USERS_NOTIFICATION m_oUserNotificationMultiplier = null;
            public int m_iQueueLength = 0;
        }

        public class PushIdNotificationSenderParameter
        {
            public PUSHID_NOTIFICATION m_oPushIdNotificationSender = null;
            public int m_iQueueLength = 0;
        }

        public class UserOperationsSenderSMSParameter
        {
            public UserOperations m_oUserOperationsSenderSMS = null;
            public string sliteral = string.Empty;
            public int m_iQueueLength = 0;
        }

        public class UsersWarningsParameter
        {
            public USERS_WARNING m_oUsersWarnings = null;
            public int m_iQueueLength = 0;
        }

      

        public CintegraMobileNotifier()
		{
            
            m_iPoolTimeNotificationGenerator = ValidAppSettingsConvertToInt(ct_POOLTIMENOTIFICATIONGENERATOR_TAG);
            m_iPoolTimeNotifications = ValidAppSettingsConvertToInt(ct_POOLTIMENOTIFICATIONS_TAG);
            m_iPoolTimePlatesSending = ValidAppSettingsConvertToInt(ct_POOLTIMEPLATESSENDING_TAG);
            m_iStopTime             =  ValidAppSettingsConvertToInt(ct_STOPTIME_TAG);
            m_iRetries              = ValidAppSettingsConvertToInt(ct_RETRIES_TAG);
            m_iResendTime = ValidAppSettingsConvertToInt(ct_RESENDTIME_TAG);
            m_iMaxPlatesToSend = ValidAppSettingsConvertToInt(ct_NUMMAXPLATESTOSEND_TAG);
            m_iMaxResendTime = ValidAppSettingsConvertToInt(ct_MAXRESENDTIME_TAG);
            m_iMinutesBeforeEndToSendWarning = ValidAppSettingsConvertToInt(ct_MINUTESBEFOREENDTOWARNPARKING_TAG);
            m_iNumPasswordRecoveryNumMinutes = ValidAppSettingsConvertToInt(ct_PASSWORD_RECOVERY_NUM_MINUTES_TAG);
            m_iMaxOfRecordsToResolveByNotifierSenderSMS = ValidAppSettingsConvertToInt(ct_MAX_RECORDS_RESOLVE_NOTIFIER_SENDER_SMS_TAG);
            m_iMaxOfRecordsToResolveByUsersWarnings = ValidAppSettingsConvertToInt(ct_MAX_RECORDS_RESOLVE_USERS_WARNINGS);
            m_iLiteralIdSMS = ValidAppSettingsConvertToInt(ct_LITERAL_ID_SMS_TAG);
            m_iPoolingNotifierManager = ValidAppSettingsConvertToInt(ct_POOL_NOTIFIER_MANAGER_TAG);
            m_iMaxWorkingThreads = ValidAppSettingsConvertToInt(ct_MAX_NOTIFIER_WORKING_THREADS_TAG);
            m_iConfirmWaitTime = ValidAppSettingsConvertToInt(ct_CONFIRM_WAIT_TIME);
            
            AddTLS12Support();
            

            m_kernel = new StandardKernel(new integraNotifierModule());
            m_kernel.Inject(this);


            m_oHashNotifierManager = new Hashtable();
            m_oHashNotifierExternalTicket = new Hashtable();
            m_oHashInsertionParkingNotification = new Hashtable();
            m_oHashBeforeEndParkingNotification = new Hashtable();
            m_oHashOperationsOffStreetNotification = new Hashtable();
            m_oHashUsersSecurityOperationNotification = new Hashtable();
            m_oHashUserNotificationMultiplier = new Hashtable();
            m_oHashPushIdNotificationSender = new Hashtable();
            m_oHashUserOperationsSenderSMS = new Hashtable();
            m_oHashUsersWarnings = new Hashtable();
            
           

            m_GcmPushBrokers = new Dictionary<decimal, GcmServiceBroker>();
            m_FireBasePushBrokers = new Dictionary<decimal, GcmServiceBroker>();
            m_ApnsPushInfos = new Dictionary<decimal, APNSInfo>();


            List<SOURCE_APPS_CONFIGURATION> oConfigurations= infraestructureRepository.GetSourceAppsConfigurations();


            foreach (var oConf in oConfigurations)
            {

                try
                {
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Configuring GCM Push Broker for App ID: {0}...", infraestructureRepository.GetSourceAppCode(oConf.SOAPC_SOAPP_ID)));

                    var configGCM = new GcmConfiguration(oConf.SOAPC_PUSH_GCM_SENDER_ID, oConf.SOAPC_PUSH_GCM_AUTH_TOKEN, null);

                    if (!string.IsNullOrEmpty(oConf.SOAPC_PUSH_GCM_OVERRIDE_URL))
                    {
                        configGCM.OverrideUrl(oConf.SOAPC_PUSH_GCM_OVERRIDE_URL);
                    }


                    var o_GcmPushBroker = new GcmServiceBroker(configGCM);

                    o_GcmPushBroker.OnNotificationSucceeded += NotificationSent;
                    o_GcmPushBroker.OnNotificationFailed += NotificationFailed;

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Starting GCM Push Broker for App ID: {0}...", infraestructureRepository.GetSourceAppCode(oConf.SOAPC_SOAPP_ID)));

                    o_GcmPushBroker.Start();

                    m_GcmPushBrokers.Add(oConf.SOAPC_SOAPP_ID, o_GcmPushBroker);



                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "RegisterGcmService Exception: " + e.Message);
                }

                try
                {
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Configuring FireBase Push Broker for App ID: {0}...", infraestructureRepository.GetSourceAppCode(oConf.SOAPC_SOAPP_ID)));


                    var configFireBase = new GcmConfiguration(oConf.SOAPC_PUSH_FIREBASE_SENDER_ID, oConf.SOAPC_PUSH_FIREBASE_AUTH_TOKEN, null);

                    configFireBase.OverrideUrl("https://fcm.googleapis.com/fcm/send");

                    var o_FireBasePushBroker = new GcmServiceBroker(configFireBase);

                    o_FireBasePushBroker.OnNotificationSucceeded += NotificationSent;
                    o_FireBasePushBroker.OnNotificationFailed += NotificationFailed;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Starting FireBase Push Broker for App ID: {0}...", infraestructureRepository.GetSourceAppCode(oConf.SOAPC_SOAPP_ID)));

                    o_FireBasePushBroker.Start();

                    m_FireBasePushBrokers.Add(oConf.SOAPC_SOAPP_ID, o_FireBasePushBroker);


                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "RegisterFireBaseService Exception: " + e.Message);
                }

                try
                {
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Configuring APNS Push Broker for App ID: {0}...", infraestructureRepository.GetSourceAppCode(oConf.SOAPC_SOAPP_ID)));

                    string strCertificateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oConf.SOAPC_PUSH_APNS_COMPANY_CERT);
                   

                    /*Bundleids
                      com.integraparking.iParkME
                      com.integraparking.pre.iParkME
                      com.integraparking.test.iParkME
                      com.integraparking.Rimouski
                      com.integraparking.pre.Rimouski
                      com.integraparking.test.Rimouski

                    */


                    m_ApnsPushInfos.Add(oConf.SOAPC_SOAPP_ID, new APNSInfo()
                    {
                        BundleId = oConf.SOAPC_PUSH_APNS_BUNDLE_ID,
                        CertFilePath = strCertificateFile,
                        KeyId = oConf.SOAPC_PUSH_APNS_KEY_ID,
                        TeamId = oConf.SOAPC_PUSH_APNS_TEAM_ID,
                        apnsConfig = null,
                        apnsClient = null,
                        dtClientUTC = DateTime.UtcNow
                    });

                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Starting APNS Push Broker for App ID: {0} (BundleID: {1} KeyID: {2} TeamId: {3} CertFile: {4})...",
                        infraestructureRepository.GetSourceAppCode(oConf.SOAPC_SOAPP_ID), 
                        oConf.SOAPC_PUSH_APNS_BUNDLE_ID, 
                        oConf.SOAPC_PUSH_APNS_KEY_ID, 
                        oConf.SOAPC_PUSH_APNS_TEAM_ID,
                        strCertificateFile));


                    
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "RegisterAppleService Exception: " + e.Message);
                }
            }
            //m_PushBroker.RegisterWindowsPhoneService();
        }


        
        #endregion 

        #region -- Threads Bodies --

        public void Start()
		{
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::Start");

            m_NofitierManagerThread = new Thread(new ThreadStart(this.NofitierManagerThread));
            m_NofitierManagerThread.Start();

            //m_NotifierGeneratorThread = new Thread(new ThreadStart(this.NotifierGeneratorThread));
            //m_NotifierGeneratorThread.Start();

            //m_NotifierMultiplierThread = new Thread(new ThreadStart(this.NotifierMultiplierThread));
            //m_NotifierMultiplierThread.Start();

            //m_NotifierSenderThread = new Thread(new ThreadStart(this.NotifierSenderThread));
            //m_NotifierSenderThread.Start();

            //m_NotifierSenderSMSThread = new Thread(new ThreadStart(this.NotifierSenderSMSThread));
            //m_NotifierSenderSMSThread.Start();

            //m_UsersWarningsThread = new Thread(new ThreadStart(this.UsersWarningsThread));
            //m_UsersWarningsThread.Start();


            /*m_PlatesSenderThread = new Thread(new ThreadStart(this.PlatesSenderThread));
            m_PlatesSenderThread.Start();*/
            
            
            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::Start");
        }

        public void Stop()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::Stop");

            m_evStopServer.Set();

            // We have to give time to close all the existing requests
            // Synchronize the finalization of the main thread
            m_NofitierManagerThread.Join(1000 * m_iStopTime);
            
            /* TO DO: Se comenta porque todo esta incorporado en NotifierManagerThead
            m_NotifierGeneratorThread.Join(1000 * m_iStopTime);
            m_NotifierMultiplierThread.Join(1000 * m_iStopTime);
            m_NotifierSenderThread.Join(1000 * m_iStopTime);
            m_NotifierSenderSMSThread.Join(1000 * m_iStopTime);
            m_UsersWarningsThread.Join(1000 * m_iStopTime);
            */

            //m_PlatesSenderThread.Join(1000 * m_iStopTime);
            m_evStopServer.Reset();

            /*foreach (KeyValuePair<decimal, ApnsServiceBroker> oEntry in m_ApnsPushBrokers)
            {
                oEntry.Value.Stop();
            }*/


            foreach (KeyValuePair<decimal, GcmServiceBroker> oEntry in m_GcmPushBrokers)
            {
                oEntry.Value.Stop();
            }


            foreach (KeyValuePair<decimal, GcmServiceBroker> oEntry in m_FireBasePushBrokers)
            {
                oEntry.Value.Stop();
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::Stop");
        }

        protected void NofitierManagerThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::NofitierManagerThread");

            int iTempFreeThreads = 0;
            
            /*********  NOTIFIER GENERATOR EXTERNAL TICKET    ************/
            EXTERNAL_TICKET oExternalTicket = null;
            int iQueueLengthNotifierExternalTicket = 0;

            EXTERNAL_PARKING_OPERATION oInsertionParking = null;
            int iQueueLengthInsertionParkingNotification = 0;

            EXTERNAL_PARKING_OPERATION oBeforeEndParking = null;
            int iQueueLengthBeforeEndParkingNotification = 0;

            OPERATIONS_OFFSTREET oOperationsOffStreet = null;
            int iQueueLengthOperationsOffStreetNotification = 0;

            USERS_SECURITY_OPERATION oUsersSecurityOperation = null;
            int iQueueLengthUsersSecurityOperationNotification = 0;

            USERS_NOTIFICATION oUserNotificationMultiplier = null;
            int iQueueLengthUserNotificationMultiplier = 0;

            PUSHID_NOTIFICATION oPushIdNotificationSender = null;
            int iQueueLengthPushIdNotificationSender = 0;

            List<UserOperations> oListUserOperationsSenderSMS = null;
            int iQueueLengthUserOperationsSenderSMS = 0;

            List<USERS_WARNING> oListUsersWarnings = null;
            int iQueueLengthUsersWarnings = 0;
            m_ListLiteralLanguage = null;
                        
            infraestructureRepository.GetLiteralsOfTheMessage(out m_ListLiteralLanguage, m_iLiteralIdSMS);
            

            bool bFinishServer = false;
            while (bFinishServer == false)
            {


                 bFinishServer = (m_evStopServer.WaitOne(m_iPoolingNotifierManager, false));
                 if (!bFinishServer)
                 {
                     int iCurrentRunningThreads = GetCurrentRunningThreads();
                     if (iCurrentRunningThreads < m_iMaxWorkingThreads)
                     {

                         iTempFreeThreads = m_iMaxWorkingThreads - iCurrentRunningThreads;
                         /*********  NOTIFIER GENERATOR EXTERNAL TICKET    ************/
                         GetWaitingNotifierExternalTicket(out oExternalTicket, out iQueueLengthNotifierExternalTicket, GetListOfRunningNotifierExternalTicket());
                         if (oExternalTicket != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads--;
                         }
                         /*********  FIN NOTIFIER GENERATOR EXTERNAL TICKET    ************/



                         /*********  NOTIFIER GENERATOR INSERTION PARKING   ************/
                         GetWaitingInsertionParkingNotification(out oInsertionParking, out iQueueLengthInsertionParkingNotification, GetListOfRunningInsertionParkingNotification());
                         if (oInsertionParking != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads--;
                         }


                         /*********  FIN NOTIFIER GENERATOR INSERTION PARKING   ************/

                         /*********  NOTIFIER GENERATOR BEFORE END PARKING   ************/
                         GetWaitingBeforeEndParkingNotification(m_iMinutesBeforeEndToSendWarning, out oBeforeEndParking, out iQueueLengthBeforeEndParkingNotification, GetListOfRunningBeforeEndParkingNotification());
                         if (oBeforeEndParking != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads--;
                         }
                         /********* FIN NOTIFIER GENERATOR BEFORE END PARKING   ************/


                         /*********  NOTIFIER GENERATOR OPERATIONS OFFSTREET   ************/
                         GetWaitingOperationsOffStreetNotification(out oOperationsOffStreet, out iQueueLengthOperationsOffStreetNotification, GetListOfRunningOperationsOffStreetNotification());
                         if (oOperationsOffStreet != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads--;
                         }
                         /*********  FIN NOTIFIER GENERATOR OPERATIONS OFFSTREET   ************/


                         /*********  NOTIFIER GENERATOR USERS SECURITY OPERATION  ************/
                         GetWaitingUsersSecurityOperationNotification(out oUsersSecurityOperation, out iQueueLengthUsersSecurityOperationNotification, GetListOfRunningUsersSecurityOperationNotification());
                         if (oUsersSecurityOperation != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads--;
                         }
                         /*********  FIN NOTIFIER GENERATOR USERS SECURITY OPERATION  ************/


                         /*********  NOTIFIER MULTIPLIER USERS NOTIFICATION  ************/
                         GetWaitingUserNotificationMultiplier(out oUserNotificationMultiplier, out iQueueLengthUserNotificationMultiplier, GetListOfRunningUserNotificationMultiplier());
                         if (oUserNotificationMultiplier != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads--;
                         }
                         /*********  FIN NOTIFIER MULTIPLIER USERS NOTIFICATION  ************/


                         /*********  NOTIFIER SENDER PUSHID NOTIFICATION ************/
                         GetWaitingPushIdNotificationSender(out oPushIdNotificationSender, m_iResendTime, out iQueueLengthPushIdNotificationSender, GetListOfRunningPushIdNotificationSender());
                         if (oPushIdNotificationSender != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads--;
                         }
                         /*********  FIN NOTIFIER SENDER PUSHID NOTIFICATION ************/



                         /*********  USER OPERATIONS SENDER SMS ************/
                         oListUserOperationsSenderSMS = null;
                         GetWaitingUserOperationsSenderSMS(out oListUserOperationsSenderSMS, (iTempFreeThreads > m_iMaxWorkingThreads ? m_iMaxWorkingThreads : iTempFreeThreads), out iQueueLengthUserOperationsSenderSMS, GetListOfRunningUserOperationsSenderSMS());
                         if (oListUserOperationsSenderSMS != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads -= oListUserOperationsSenderSMS.Count;
                         }
                         /*********  FIN USER OPERATIONS SENDER SMS ************/



                         /******************  USER WARNING *********************/



                         GetWaitingUsersWarnings(out oListUsersWarnings, (iTempFreeThreads > m_iMaxWorkingThreads ? m_iMaxWorkingThreads : iTempFreeThreads), out iQueueLengthUsersWarnings, GetListOfRunningUsersWarnings());
                         if (oListUsersWarnings != null && iTempFreeThreads > 0)
                         {
                             iTempFreeThreads -= oListUsersWarnings.Count;
                         }
                         /******************  USER WARNING *********************/



                         

                         while ((!bFinishServer) &&
                              ((oExternalTicket != null) || (oInsertionParking != null) || (oBeforeEndParking != null) || (oOperationsOffStreet != null) || (oUsersSecurityOperation != null) ||
                               (oUserNotificationMultiplier != null) || (oPushIdNotificationSender != null) || (oListUserOperationsSenderSMS != null && oListUserOperationsSenderSMS.Count()>0) || 
                               (oListUsersWarnings != null && oListUsersWarnings.Count() > 0)) &&
                              (iCurrentRunningThreads < m_iMaxWorkingThreads))
                         {




                             /*********  NOTIFIER GENERATOR EXTERNAL TICKET    ************/
                             if ((!bFinishServer) && (oExternalTicket != null))
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddNotifierExternalTicketToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads, m_iMaxWorkingThreads, iQueueLengthNotifierExternalTicket));
                                 AddNotifierExternalTicketToRunningThreads(oExternalTicket);
                                 ThreadPool.QueueUserWorkItem(new WaitCallback(NotifierExternalTicket), (object)new NotifierExternalTicketParameter { m_oExternarTicket = oExternalTicket, m_iQueueLength = iQueueLengthNotifierExternalTicket });
                                 oExternalTicket = null;
                             }

                             m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddNotifierExternalTicketToRunningThreads --> 13"));

                             /*********  NOTIFIER GENERATOR INSERTION PARKING    ************/
                             if ((!bFinishServer) && (oInsertionParking != null))
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddInsertionParkingNotificationToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads, m_iMaxWorkingThreads, iQueueLengthInsertionParkingNotification));
                                 AddInsertionParkingNotificationToRunningThreads(oInsertionParking);
                                 ThreadPool.QueueUserWorkItem(new WaitCallback(InsertionParkingNotification), (object)new InsertionParkingNotificationParameter { m_oInsertionParking = oInsertionParking, m_iQueueLength = iQueueLengthInsertionParkingNotification });
                                 oInsertionParking = null;
                             }


                             /*********  NOTIFIER GENERATOR BEFORE END PARKING    ************/
                             if ((!bFinishServer) && (oBeforeEndParking != null))
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddBeforeEndParkingNotificationToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads, m_iMaxWorkingThreads, iQueueLengthBeforeEndParkingNotification));
                                 AddBeforeEndParkingNotificationToRunningThreads(oBeforeEndParking);
                                 ThreadPool.QueueUserWorkItem(new WaitCallback(BeforeEndParkingNotification), (object)new BeforeEndParkingNotificationParameter { m_oBeforeEndParking = oBeforeEndParking, m_iQueueLength = iQueueLengthBeforeEndParkingNotification });
                                 oBeforeEndParking = null;
                             }



                             /*********  NOTIFIER GENERATOR OPERATIONS OFFSTREET   ************/
                             if ((!bFinishServer) && (oOperationsOffStreet != null))
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddOperationsOffStreetNotificationToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads, m_iMaxWorkingThreads, iQueueLengthOperationsOffStreetNotification));
                                 AddOperationsOffStreetNotificationToRunningThreads(oOperationsOffStreet);
                                 ThreadPool.QueueUserWorkItem(new WaitCallback(OperationsOffStreetNotification), (object)new OperationsOffStreetNotificationParameter { m_oOperationsOffStreet = oOperationsOffStreet, m_iQueueLength = iQueueLengthOperationsOffStreetNotification });
                                 oOperationsOffStreet = null;
                             }


                             /*********  NOTIFIER GENERATOR USERS SECURITY OPERATION   ************/
                             if ((!bFinishServer) && (oUsersSecurityOperation != null))
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddUsersSecurityOperationNotificationToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads, m_iMaxWorkingThreads, iQueueLengthUsersSecurityOperationNotification));
                                 AddUsersSecurityOperationNotificationToRunningThreads(oUsersSecurityOperation);
                                 ThreadPool.QueueUserWorkItem(new WaitCallback(UsersSecurityOperationNotification), (object)new UsersSecurityOperationNotificationParameter { m_oUsersSecurityOperation = oUsersSecurityOperation, m_iQueueLength = iQueueLengthUsersSecurityOperationNotification });
                                 oUsersSecurityOperation = null;
                             }

                             /*********  NOTIFIER MULTIPLIER USERS NOTIFICATION  ************/
                             if ((!bFinishServer) && (oUserNotificationMultiplier != null))
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddUserNotificationMultiplierToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads, m_iMaxWorkingThreads, iQueueLengthUserNotificationMultiplier));
                                 AddUserNotificationMultiplierToRunningThreads(oUserNotificationMultiplier);
                                 ThreadPool.QueueUserWorkItem(new WaitCallback(UserNotificationMultiplier), (object)new UserNotificationMultiplierParameter { m_oUserNotificationMultiplier = oUserNotificationMultiplier, m_iQueueLength = iQueueLengthUserNotificationMultiplier });
                                 oUserNotificationMultiplier = null;
                             }

                             /*********  NOTIFIER SENDER PUSHID NOTIFICATION ************/
                             if ((!bFinishServer) && (oPushIdNotificationSender != null))
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier:: AddPushIdNotificationSenderToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads, m_iMaxWorkingThreads, iQueueLengthPushIdNotificationSender));
                                 AddPushIdNotificationSenderToRunningThreads(oPushIdNotificationSender);
                                 ThreadPool.QueueUserWorkItem(new WaitCallback(PushIdNotificationSender), (object)new PushIdNotificationSenderParameter { m_oPushIdNotificationSender = oPushIdNotificationSender, m_iQueueLength = iQueueLengthPushIdNotificationSender });
                                 oPushIdNotificationSender = null;
                             }

                             /*********  USER OPERATIONS SENDER SMS ************/
                             bFinishServer = (m_evStopServer.WaitOne(0, false));
                             if ((!bFinishServer) && (oListUserOperationsSenderSMS != null))
                             {
                                 int i = 0;

                                 List<LITERAL_LANGUAGE> listLiterlLanguage = new List<LITERAL_LANGUAGE>();
                                 string sLitLang = string.Empty;
                                 if (oListUserOperationsSenderSMS.Count > 0)
                                 {
                                     infraestructureRepository.GetLiteralsOfTheMessage(out listLiterlLanguage, m_iLiteralIdSMS);
                                 }

                                 foreach (UserOperations oUserOpe in oListUserOperationsSenderSMS)
                                 {
                                     iCurrentRunningThreads = GetCurrentRunningThreads();
                                     AddUserOperationsSenderSMSToRunningThreads(oUserOpe);

                                     if (m_ListLiteralLanguage == null)
                                     {
                                         infraestructureRepository.GetLiteralsOfTheMessage(out m_ListLiteralLanguage, m_iLiteralIdSMS);
                                     }
                                     try
                                     {
                                         LITERAL_LANGUAGE oLitLang = m_ListLiteralLanguage.FirstOrDefault(x => x.LANGUAGE.LAN_CULTURE.Equals(oUserOpe.UsrCultureLang));
                                         sLitLang = oLitLang.LITL_LITERAL;
                                     }
                                     catch (Exception ex)
                                     {
                                         m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier:: Get --> ListLiteralLanguage", ex);
                                     }
                                     ThreadPool.QueueUserWorkItem(new WaitCallback(UserOperationsSenderSMS), (object)new UserOperationsSenderSMSParameter { m_oUserOperationsSenderSMS = oUserOpe, m_iQueueLength = iQueueLengthUserOperationsSenderSMS - i, sliteral = sLitLang });
                                     i++;
                                 }
                                 oListUserOperationsSenderSMS.Clear();
                                 oListUserOperationsSenderSMS = null;
                             }
                             /*********  FIN USER OPERATIONS SENDER SMS ************/


                             /*********  USER WARNING ************/
                             //bFinishServer = (m_evStopServer.WaitOne(0, false));
                             if ((!bFinishServer) && (oListUsersWarnings != null))
                             {


                                 int i = 0;

                                 foreach (USERS_WARNING oUserWar in oListUsersWarnings)
                                 {
                                     m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileNotifier::AddUsersWarningsToRunningThreads --> RunningThreads={0}; MaxThreads={1}; CurrentQueue={2}", iCurrentRunningThreads + i, m_iMaxWorkingThreads, iQueueLengthUsersWarnings - i));
                                     AddUsersWarningsToRunningThreads(oUserWar);
                                     ThreadPool.QueueUserWorkItem(new WaitCallback(UsersWarnings), (object)new UsersWarningsParameter { m_oUsersWarnings = oUserWar, m_iQueueLength = iQueueLengthUsersWarnings - i });
                                     i++;
                                 }

                                 oListUsersWarnings.Clear();
                                 oListUsersWarnings = null;
                             }
                             /*********  FIN USER WARNING ************/


                             if (!bFinishServer)
                             {
                                 iCurrentRunningThreads = GetCurrentRunningThreads();
                                 iTempFreeThreads = m_iMaxWorkingThreads - iCurrentRunningThreads + 1;

                                 /*********  NOTIFIER GENERATOR EXTERNAL TICKET    ************/
                                 //bFinishServer = (m_evStopServer.WaitOne(0, false));
                                 GetWaitingNotifierExternalTicket(out oExternalTicket, out iQueueLengthNotifierExternalTicket, GetListOfRunningNotifierExternalTicket());
                                 if (oExternalTicket != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads--;
                                 }
                                 /*********  FIN NOTIFIER GENERATOR EXTERNAL TICKET    ************/

                                 /*********  NOTIFIER GENERATOR INSERTION PARKING   ************/
                                 //bFinishServer = (m_evStopServer.WaitOne(0, false));
                                 GetWaitingInsertionParkingNotification(out oInsertionParking, out iQueueLengthInsertionParkingNotification, GetListOfRunningInsertionParkingNotification());
                                 if (oInsertionParking != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads--;
                                 }
                                 /*********  FIN NOTIFIER GENERATOR INSERTION PARKING   ************/

                                 /*********  NOTIFIER GENERATOR BEFORE END PARKING   ************/
                                 //bFinishServer = (m_evStopServer.WaitOne(0, false));
                                 GetWaitingBeforeEndParkingNotification(m_iMinutesBeforeEndToSendWarning, out oBeforeEndParking, out iQueueLengthBeforeEndParkingNotification, GetListOfRunningBeforeEndParkingNotification());
                                 if (oBeforeEndParking != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads--;
                                 }
                                 /********* FIN NOTIFIER GENERATOR BEFORE END PARKING   ************/

                                 /*********  NOTIFIER GENERATOR OPERATIONS OFFSTREET   ************/
                                 GetWaitingOperationsOffStreetNotification(out oOperationsOffStreet, out iQueueLengthOperationsOffStreetNotification, GetListOfRunningOperationsOffStreetNotification());
                                 if (oOperationsOffStreet != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads--;
                                 }
                                 /*********  FIN NOTIFIER GENERATOR OPERATIONS OFFSTREET   ************/

                                 /*********  NOTIFIER GENERATOR USERS SECURITY OPERATION  ************/
                                 GetWaitingUsersSecurityOperationNotification(out oUsersSecurityOperation, out iQueueLengthUsersSecurityOperationNotification, GetListOfRunningUsersSecurityOperationNotification());
                                 if (oUsersSecurityOperation != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads--;
                                 }
                                 /*********  FIN NOTIFIER GENERATOR USERS SECURITY OPERATION  ************/

                                 /*********  NOTIFIER MULTIPLIER USERS NOTIFICATION  ************/
                                 GetWaitingUserNotificationMultiplier(out oUserNotificationMultiplier, out iQueueLengthUserNotificationMultiplier, GetListOfRunningUserNotificationMultiplier());
                                 if (oUserNotificationMultiplier != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads--;
                                 }
                                 /*********  FIN NOTIFIER MULTIPLIER USERS NOTIFICATION  ************/

                                 /*********  NOTIFIER SENDER PUSHID NOTIFICATION ************/
                                 GetWaitingPushIdNotificationSender(out oPushIdNotificationSender, m_iResendTime, out iQueueLengthPushIdNotificationSender, GetListOfRunningPushIdNotificationSender());
                                 if (oPushIdNotificationSender != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads--;
                                 }
                                 /*********  FIN NOTIFIER SENDER PUSHID NOTIFICATION ************/


                                 /*********  USER OPERATIONS SENDER SMS ************/
                                 GetWaitingUserOperationsSenderSMS(out oListUserOperationsSenderSMS, (iTempFreeThreads > m_iMaxWorkingThreads ? m_iMaxWorkingThreads : iTempFreeThreads), out iQueueLengthUserOperationsSenderSMS, GetListOfRunningUserOperationsSenderSMS());
                                 if (oListUserOperationsSenderSMS != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads -= oListUserOperationsSenderSMS.Count;
                                 }
                                 /*********  FIN USER OPERATIONS SENDER SMS ************/

                                 /******************  USER WARNING *********************/
                                 GetWaitingUsersWarnings(out oListUsersWarnings, (iTempFreeThreads > m_iMaxWorkingThreads ? m_iMaxWorkingThreads : iTempFreeThreads), out iQueueLengthUsersWarnings, GetListOfRunningUsersWarnings());

                                 if (oListUsersWarnings != null && iTempFreeThreads > 0)
                                 {
                                     iTempFreeThreads -= oListUsersWarnings.Count;
                                 }
                                 /******************  USER WARNING *********************/
                             }
                         }
                     }
                     else
                     {
                         if (iCurrentRunningThreads >= m_iMaxWorkingThreads)
                         {
                             m_Log.LogMessage(LogLevels.logINFO, "iCurrentRunningThreads > m_iMaxWorkingThreads-->< iCurrentRunningThreads: " + iCurrentRunningThreads + " <  m_iMaxWorkingThreads: " + m_iMaxWorkingThreads);
                         }
                     }
                 }
                 bFinishServer = (m_evStopServer.WaitOne(0, false));                              
            }
            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::NofitierManagerThread");
        }


        #region  NOTIFIER GENERATOR EXTERNAL TICKET
        
        private bool GetWaitingNotifierExternalTicket(out EXTERNAL_TICKET oExternalTicket, out int iQueueLengthNotifierExternalTicket, List<decimal> oListRunningNotifierExternalTicket)
        {
            bool bReps = true;
            oExternalTicket = null;
            iQueueLengthNotifierExternalTicket = 0;
            try
            {
                infraestructureRepository.GetInsertionTicketNotificationData(out oExternalTicket, out iQueueLengthNotifierExternalTicket, oListRunningNotifierExternalTicket);
            }
            catch (Exception e)
            {
                bReps = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingNotifierExternalTicket: Exception {0}", e.Message));
            }
            return bReps;
        }

        List<decimal> GetListOfRunningNotifierExternalTicket()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashNotifierExternalTicket)
            {
                IDictionaryEnumerator denum = m_oHashNotifierExternalTicket.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((EXTERNAL_TICKET)dentry.Value).EXTI_ID);
                }
            }

            return oList;
        }

        private void NotifierExternalTicket(object input)
        {
            EXTERNAL_TICKET oExternalTicket = null;

            try
            {
                NotifierExternalTicketParameter oInputParameter = (NotifierExternalTicketParameter)input;
                oExternalTicket = oInputParameter.m_oExternarTicket;
                if (GenerateInsertionTicketNotification(oExternalTicket))
                {
                    if (infraestructureRepository.MarkAsGeneratedInsertionTicketNotification(oExternalTicket))
                    {
                        DeleteNotifierGeneratorFromRunningThreads(oExternalTicket);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::NotifierExternalTicket: Exception {0}", e.Message));
            }
        }

        void AddNotifierExternalTicketToRunningThreads(EXTERNAL_TICKET oExternalTicket)
        {
            try
            {
                lock (m_oHashNotifierExternalTicket)
                {
                    if (!m_oHashNotifierExternalTicket.ContainsKey(oExternalTicket.EXTI_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddNotifierExternalTicketToRunningThreads >>" + string.Format("EXTERNAL_TICKET.EXTI_ID={0}", oExternalTicket.EXTI_ID));
                        m_oHashNotifierExternalTicket.Add(oExternalTicket.EXTI_ID, oExternalTicket);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddNotifierExternalTicketToRunningThreads", ex);
            }
        }

        void DeleteNotifierGeneratorFromRunningThreads(EXTERNAL_TICKET oExternalTicket)
        {
            try
            {
                lock (m_oHashNotifierExternalTicket)
                {
                    if (m_oHashNotifierExternalTicket.ContainsKey(oExternalTicket.EXTI_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteNotifierGeneratorFromRunningThreads >>" + string.Format("EXTERNAL_TICKET.EXTI_ID={0}", oExternalTicket.EXTI_ID));
                        m_oHashNotifierExternalTicket.Remove(oExternalTicket.EXTI_ID);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteNotifierGeneratorFromRunningThreads", e);
            }
        }

        #endregion

        #region NOTIFIER GENERATOR INSERTION PARKING

        private bool GetWaitingInsertionParkingNotification(out EXTERNAL_PARKING_OPERATION oInsertionParking, out int iQueueLengthInsertionParkingNotification, List<decimal> oListRunningInsertionParkingNotification)
        {
            bool bReps = true;
            oInsertionParking = null;
            iQueueLengthInsertionParkingNotification = 0;
            try
            {
                bReps = infraestructureRepository.GetInsertionParkingNotificationData(out oInsertionParking, out iQueueLengthInsertionParkingNotification, oListRunningInsertionParkingNotification);
            }
            catch (Exception e)
            {
                bReps = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingInsertionParkingNotification: Exception {0}", e.Message));

            }
            return bReps;
        }

        List<decimal> GetListOfRunningInsertionParkingNotification()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashInsertionParkingNotification)
            {
                IDictionaryEnumerator denum = m_oHashInsertionParkingNotification.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((EXTERNAL_PARKING_OPERATION)dentry.Value).EPO_ID);
                }
            }

            return oList;
        }

        private void InsertionParkingNotification(object input)
        {
            EXTERNAL_PARKING_OPERATION oInsertionParking = null;
            try
            {
                InsertionParkingNotificationParameter oInputParameter = (InsertionParkingNotificationParameter)input;
                oInsertionParking = oInputParameter.m_oInsertionParking;
                if (GenerateInsertionParkingNotification(oInsertionParking))
                {
                    if (infraestructureRepository.MarkAsGeneratedInsertionParkingNotificationData(oInsertionParking))
                    {
                        DeleteInsertionParkingNotificationFromRunningThreads(oInsertionParking);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::InsertionParkingNotification: Exception {0}", e.Message));
            }
        }

        void AddInsertionParkingNotificationToRunningThreads(EXTERNAL_PARKING_OPERATION oInsertionParkingNotification)
        {
            try
            {
                lock (m_oHashInsertionParkingNotification)
                {
                    if (!m_oHashInsertionParkingNotification.ContainsKey(oInsertionParkingNotification.EPO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddInsertionParkingNotificationToRunningThreads >>" + string.Format("EXTERNAL_PARKING_OPERATION.EPO_ID={0}", oInsertionParkingNotification.EPO_ID));
                        m_oHashInsertionParkingNotification.Add(oInsertionParkingNotification.EPO_ID, oInsertionParkingNotification);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddInsertionParkingNotificationToRunningThreads", ex);
            }
        }

        void DeleteInsertionParkingNotificationFromRunningThreads(EXTERNAL_PARKING_OPERATION oInsertionParkingNotification)
        {
            try
            {
                lock (m_oHashInsertionParkingNotification)
                {
                    if (m_oHashInsertionParkingNotification.ContainsKey(oInsertionParkingNotification.EPO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteInsertionParkingNotificationFromRunningThreads >>" + string.Format("EXTERNAL_PARKING_OPERATION.EPO_ID={0}", oInsertionParkingNotification.EPO_ID));
                        m_oHashInsertionParkingNotification.Remove(oInsertionParkingNotification.EPO_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteInsertionParkingNotificationFromRunningThreads", ex);
            }
        }
        #endregion

        #region NOTIFIER GENERATOR BEFORE END PARKING

        private bool GetWaitingBeforeEndParkingNotification(int iMinutesBeforeEndToSendWarning, out EXTERNAL_PARKING_OPERATION oBeforeEndParking, out int iQueueLengthBeforeEndParkingNotification, List<decimal> oListRunningBeforeEndParkingNotification)
        {
            bool bResp = true;
            oBeforeEndParking = null;
            iQueueLengthBeforeEndParkingNotification = 0;
            try
            {
                bResp = infraestructureRepository.GetBeforeEndParkingNotificationData(iMinutesBeforeEndToSendWarning, out oBeforeEndParking, out iQueueLengthBeforeEndParkingNotification, oListRunningBeforeEndParkingNotification);
            }
            catch (Exception e)
            {
                bResp = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingBeforeEndParkingNotification: Exception {0}", e.Message));

            }
            return bResp;
        }

        List<decimal> GetListOfRunningBeforeEndParkingNotification()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashBeforeEndParkingNotification)
            {
                IDictionaryEnumerator denum = m_oHashBeforeEndParkingNotification.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((EXTERNAL_PARKING_OPERATION)dentry.Value).EPO_ID);
                }
            }

            return oList;
        }

        private void BeforeEndParkingNotification(object input)
        {
            EXTERNAL_PARKING_OPERATION oBeforeEndParking = null;
            try
            {
                BeforeEndParkingNotificationParameter oInputParameter = (BeforeEndParkingNotificationParameter)input;
                oBeforeEndParking = oInputParameter.m_oBeforeEndParking;
                if (GenerateBeforeEndParkingNotification(oBeforeEndParking))
                {
                    if (infraestructureRepository.MarkAsGeneratedBeforeEndParkingNotificationData(oBeforeEndParking))
                    {
                        DeleteBeforeEndParkingNotificationFromRunningThreads(oBeforeEndParking);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::BeforeEndParkingNotification: Exception {0}", e.Message));
            }
        }

        void AddBeforeEndParkingNotificationToRunningThreads(EXTERNAL_PARKING_OPERATION oBeforeEndParkingNotification)
        {
            try
            {
                lock (m_oHashBeforeEndParkingNotification)
                {
                    if (!m_oHashBeforeEndParkingNotification.ContainsKey(oBeforeEndParkingNotification.EPO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddBeforeEndParkingNotificationToRunningThreads >>" + string.Format("EXTERNAL_PARKING_OPERATION.EPO_ID={0}", oBeforeEndParkingNotification.EPO_ID));
                        m_oHashBeforeEndParkingNotification.Add(oBeforeEndParkingNotification.EPO_ID, oBeforeEndParkingNotification);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddBeforeEndParkingNotificationToRunningThreads ", ex);
            }
        }

        void DeleteBeforeEndParkingNotificationFromRunningThreads(EXTERNAL_PARKING_OPERATION oBeforeEndParkingNotification)
        {
            try
            {
                lock (m_oHashBeforeEndParkingNotification)
                {
                    if (m_oHashBeforeEndParkingNotification.ContainsKey(oBeforeEndParkingNotification.EPO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteBeforeEndParkingNotificationFromRunningThreads >>" + string.Format("EXTERNAL_PARKING_OPERATION.EPO_ID={0}", oBeforeEndParkingNotification.EPO_ID));
                        m_oHashBeforeEndParkingNotification.Remove(oBeforeEndParkingNotification.EPO_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteBeforeEndParkingNotificationFromRunningThreads", ex);
            }
        }
        #endregion

        #region NOTIFIER GENERATOR OPERATIONS OFFSTREET

        private bool GetWaitingOperationsOffStreetNotification(out OPERATIONS_OFFSTREET oOperationOffStreet, out int iQueueLengthOperationOffStreetNotification, List<decimal> oListRunningOperationOffStreetNotification)
        {
            bool bResp = true;
            oOperationOffStreet = null;
            iQueueLengthOperationOffStreetNotification = 0;
            try
            {
                infraestructureRepository.GetOffstreetOperationNotificationData(out oOperationOffStreet, out iQueueLengthOperationOffStreetNotification, oListRunningOperationOffStreetNotification);
            }
            catch (Exception e)
            {
                bResp = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingOperationsOffStreetNotification: Exception {0}", e.Message));

            }
            return bResp;
        }

        List<decimal> GetListOfRunningOperationsOffStreetNotification()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashOperationsOffStreetNotification)
            {
                IDictionaryEnumerator denum = m_oHashOperationsOffStreetNotification.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((OPERATIONS_OFFSTREET)dentry.Value).OPEOFF_ID);
                }
            }

            return oList;
        }

        private void OperationsOffStreetNotification(object input)
        {
            OPERATIONS_OFFSTREET oOperationOffStreet = null;
            try
            {
                OperationsOffStreetNotificationParameter oInputParameter = (OperationsOffStreetNotificationParameter)input;
                oOperationOffStreet = oInputParameter.m_oOperationsOffStreet;
                if (GenerateOffstreetOperationNotification(oOperationOffStreet))
                {
                    if (infraestructureRepository.MarkAsGeneratedOffstreetOperationNotificationData(oOperationOffStreet))
                    {
                        DeleteOperationsOffStreetNotificationFromRunningThreads(oOperationOffStreet);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::OperationsOffStreetNotification: Exception {0}", e.Message));
            }
        }

        void AddOperationsOffStreetNotificationToRunningThreads(OPERATIONS_OFFSTREET oOperationOffStreet)
        {
            try
            {
                lock (m_oHashOperationsOffStreetNotification)
                {
                    if (!m_oHashOperationsOffStreetNotification.ContainsKey(oOperationOffStreet.OPEOFF_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddOperationsOffStreetNotificationToRunningThreads >>" + string.Format("OPERATIONS_OFFSTREET.OPEOFF_ID={0}", oOperationOffStreet.OPEOFF_ID));
                        m_oHashOperationsOffStreetNotification.Add(oOperationOffStreet.OPEOFF_ID, oOperationOffStreet);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddOperationsOffStreetNotificationToRunningThreads", ex);
            }
 
        }

        void DeleteOperationsOffStreetNotificationFromRunningThreads(OPERATIONS_OFFSTREET oOperationOffStreet)
        {
            try
            {
                lock (m_oHashOperationsOffStreetNotification)
                {
                    if (m_oHashOperationsOffStreetNotification.ContainsKey(oOperationOffStreet.OPEOFF_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteOperationsOffStreetNotificationFromRunningThreads >>" + string.Format("OPERATIONS_OFFSTREET.OPEOFF_ID={0}", oOperationOffStreet.OPEOFF_ID));
                        m_oHashOperationsOffStreetNotification.Remove(oOperationOffStreet.OPEOFF_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteOperationsOffStreetNotificationFromRunningThreads", ex);
            }
        }
        #endregion

        #region NOTIFIER GENERATOR USERS SECURITY OPERATION

        private bool GetWaitingUsersSecurityOperationNotification(out USERS_SECURITY_OPERATION oUsersSecurityOperation, out int iQueueLengthoUsersSecurityOperationNotification, List<decimal> oListRunningUsersSecurityOperationNotification)
        {
            bool bResp = true;
            oUsersSecurityOperation = null;
            iQueueLengthoUsersSecurityOperationNotification = 0; 
            try
            {
                bResp = infraestructureRepository.GetInsertionUserSecurityDataNotificationData(out oUsersSecurityOperation, out iQueueLengthoUsersSecurityOperationNotification, oListRunningUsersSecurityOperationNotification);
            }
            catch (Exception e)
            {
                bResp = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingUsersSecurityOperationNotification: Exception {0}", e.Message));
            }
            return bResp;
        }

        List<decimal> GetListOfRunningUsersSecurityOperationNotification()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashUsersSecurityOperationNotification)
            {
                IDictionaryEnumerator denum = m_oHashUsersSecurityOperationNotification.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((USERS_SECURITY_OPERATION)dentry.Value).USOP_ID);
                }
            }

            return oList;
        }

        private void UsersSecurityOperationNotification(object input)
        {
            USERS_SECURITY_OPERATION oUsersSecurityOperation = null;
            try
            {
                UsersSecurityOperationNotificationParameter oInputParameter = (UsersSecurityOperationNotificationParameter)input;
                oUsersSecurityOperation = oInputParameter.m_oUsersSecurityOperation;
                decimal dNotifId = 0;
                if (GenerateInsertionUserSecurityDataNotification(oUsersSecurityOperation, ref dNotifId))
                {
                    if(infraestructureRepository.MarkAsGeneratedUserSecurityDataNotificationData(oUsersSecurityOperation, dNotifId))
                    {
                        DeleteOperationsOffStreetNotificationFromRunningThreads(oUsersSecurityOperation);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::UsersSecurityOperationNotification: Exception {0}", e.Message));
            }
        }

        void AddUsersSecurityOperationNotificationToRunningThreads(USERS_SECURITY_OPERATION oUsersSecurityOperation)
        {
            try
            {
                lock (m_oHashUsersSecurityOperationNotification)
                {
                    if (!m_oHashUsersSecurityOperationNotification.ContainsKey(oUsersSecurityOperation.USOP_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddUsersSecurityOperationNotificationToRunningThreads >>" + string.Format("USERS_SECURITY_OPERATION.USOP_ID={0}", oUsersSecurityOperation.USOP_ID));
                        m_oHashUsersSecurityOperationNotification.Add(oUsersSecurityOperation.USOP_ID, oUsersSecurityOperation);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddUsersSecurityOperationNotificationToRunningThreads", ex);
            }
        }

        void DeleteOperationsOffStreetNotificationFromRunningThreads(USERS_SECURITY_OPERATION oUsersSecurityOperation)
        {
            try
            {
                lock (m_oHashUsersSecurityOperationNotification)
                {
                    if (m_oHashUsersSecurityOperationNotification.ContainsKey(oUsersSecurityOperation.USOP_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteOperationsOffStreetNotificationFromRunningThreads >>" + string.Format("USERS_SECURITY_OPERATION.USOP_ID={0}", oUsersSecurityOperation.USOP_ID));
                        m_oHashUsersSecurityOperationNotification.Remove(oUsersSecurityOperation.USOP_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteOperationsOffStreetNotificationFromRunningThreads", ex);
            }
        }
        #endregion

        #region  NOTIFIER MULTIPLIER USERS NOTIFICATION

        private bool GetWaitingUserNotificationMultiplier(out USERS_NOTIFICATION oUserNotificationMultiplier, out int iQueueLengthUserNotificationMultiplier, List<decimal> oListRunningUserNotificationMultiplier)
        {
            bool bResp = true;
            oUserNotificationMultiplier = null;
            iQueueLengthUserNotificationMultiplier=0;
            try
            {
                bResp = infraestructureRepository.GetFirstNotGeneratedUserNotification(out oUserNotificationMultiplier, out iQueueLengthUserNotificationMultiplier, oListRunningUserNotificationMultiplier);
            }
            catch(Exception e)
            {
                bResp = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingUserNotificationMultiplier: Exception {0}", e.Message));
            }
            return bResp;
        }

        List<decimal> GetListOfRunningUserNotificationMultiplier()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashUserNotificationMultiplier)
            {
                IDictionaryEnumerator denum = m_oHashUserNotificationMultiplier.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((USERS_NOTIFICATION)dentry.Value).UNO_ID);
                }
            }

            return oList;
        }

        private void UserNotificationMultiplier(object input)
        {
            USERS_NOTIFICATION oUserNotificationMultiplier = null;
            try
            {
                UserNotificationMultiplierParameter oInputParameter = (UserNotificationMultiplierParameter)input;
                oUserNotificationMultiplier = oInputParameter.m_oUserNotificationMultiplier;
                
                if (infraestructureRepository.GenerateUserNotification(ref oUserNotificationMultiplier))
                {
                    DeleteUserNotificationMultiplierFromRunningThreads(oUserNotificationMultiplier);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::UserNotificationMultiplier: Exception {0}", e.Message));
            }
        }

        void AddUserNotificationMultiplierToRunningThreads(USERS_NOTIFICATION oUserNotificationMultiplier)
        {
            try
            {
                lock (m_oHashUserNotificationMultiplier)
                {
                    if (!m_oHashUserNotificationMultiplier.ContainsKey(oUserNotificationMultiplier.UNO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddUserNotificationMultiplierToRunningThreads >>" + string.Format("USERS_NOTIFICATION.UNO_ID={0}", oUserNotificationMultiplier.UNO_ID));
                        m_oHashUserNotificationMultiplier.Add(oUserNotificationMultiplier.UNO_ID, oUserNotificationMultiplier);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddUserNotificationMultiplierToRunningThreads", ex);
            }
        }

        void DeleteUserNotificationMultiplierFromRunningThreads(USERS_NOTIFICATION oUserNotificationMultiplier)
        {
            try
            {
                lock (m_oHashUserNotificationMultiplier)
                {
                    if (m_oHashUserNotificationMultiplier.ContainsKey(oUserNotificationMultiplier.UNO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteUserNotificationMultiplierFromRunningThreads >>" + string.Format("USERS_NOTIFICATION.UNO_ID={0}", oUserNotificationMultiplier.UNO_ID));
                        m_oHashUserNotificationMultiplier.Remove(oUserNotificationMultiplier.UNO_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteUserNotificationMultiplierFromRunningThreads", ex);
            }
        }
        #endregion

        #region NOTIFIER SENDER PUSHID NOTIFICATION
        private bool GetWaitingPushIdNotificationSender(out PUSHID_NOTIFICATION oPushIdNotificationSender, int iResendTime, out int iQueueLengthPushIdNotificationSender, List<decimal> oListRunningPushIdNotificationSender)
        {
            bool bResp = true;
            oPushIdNotificationSender = null;
            iQueueLengthPushIdNotificationSender = 0;
            try
            {
                bResp = infraestructureRepository.GetFirstNotSentNotification(out oPushIdNotificationSender, iResendTime, out iQueueLengthPushIdNotificationSender, oListRunningPushIdNotificationSender);
            }
            catch (Exception e)
            {
                bResp = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingPushIdNotificationSender: Exception {0}", e.Message));
            }
            return bResp;
        }

        List<decimal> GetListOfRunningPushIdNotificationSender()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashPushIdNotificationSender)
            {
                IDictionaryEnumerator denum = m_oHashPushIdNotificationSender.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((PUSHID_NOTIFICATION)dentry.Value).PNO_ID);
                }
            }

            return oList;
        }

        private void PushIdNotificationSender(object input)
        {
            PUSHID_NOTIFICATION oPushIdNotificationSender = null;
            try
            {
                PushIdNotificationSenderParameter oInputParameter = (PushIdNotificationSenderParameter)input;
                oPushIdNotificationSender = oInputParameter.m_oPushIdNotificationSender;

                if (oPushIdNotificationSender != null)
                {
                    SendNotification(ref oPushIdNotificationSender);
                    //DeletePushIdNotificationSenderFromRunningThreads(oPushIdNotificationSender);
                }
                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::PushIdNotificationSender: Exception {0}", e.Message));
            }
        }

        void AddPushIdNotificationSenderToRunningThreads(PUSHID_NOTIFICATION oPushIdNotificationSender)
        {
            try
            {
                lock (m_oHashPushIdNotificationSender)
                {
                    if (!m_oHashPushIdNotificationSender.ContainsKey(oPushIdNotificationSender.PNO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddPushIdNotificationSenderToRunningThreads >>" + string.Format("PUSHID_NOTIFICATION.PNO_ID={0}", oPushIdNotificationSender.PNO_ID));
                        m_oHashPushIdNotificationSender.Add(oPushIdNotificationSender.PNO_ID, oPushIdNotificationSender);
                        lock (m_NumPushPendingOfFinishingLock)
                        {
                            m_NumPushPendingOfFinishing++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddPushIdNotificationSenderToRunningThreads", ex);
            }
        }

        void DeletePushIdNotificationSenderFromRunningThreads(decimal dPNO_ID)
        {
            try
            {
                lock (m_oHashPushIdNotificationSender)
                {
                    if (m_oHashPushIdNotificationSender.ContainsKey(dPNO_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeletePushIdNotificationSenderFromRunningThreads >>" + string.Format("PUSHID_NOTIFICATION.PNO_ID={0}", dPNO_ID));
                        m_oHashUserNotificationMultiplier.Remove(dPNO_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeletePushIdNotificationSenderFromRunningThreads", ex);
            }
        }
        #endregion

        #region USER OPERATIONS SENDER SMS
        private bool GetWaitingUserOperationsSenderSMS(out List<UserOperations> oUserOperationsSenderSMS, int iMaxOperationsToReturn, out int iQueueLengthUserOperationsSenderSMS, List<decimal> oListRunningUserOperationsSenderSMS)
        {
            bool bResp = true;
            oUserOperationsSenderSMS = null;
            iQueueLengthUserOperationsSenderSMS = 0;
            try
            {
                // infraestructureRepository.FilterSMS();
                infraestructureRepository.GetListOfUsersToSendSMS(out oUserOperationsSenderSMS, iMaxOperationsToReturn, out iQueueLengthUserOperationsSenderSMS, oListRunningUserOperationsSenderSMS);
            }
            catch (Exception e)
            {
                bResp = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingUserOperationsSenderSMS: Exception {0}", e.Message));
            }

            return bResp;
        }

        List<decimal> GetListOfRunningUserOperationsSenderSMS()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashUserOperationsSenderSMS)
            {
                IDictionaryEnumerator denum = m_oHashUserOperationsSenderSMS.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((UserOperations)dentry.Value).OpeId);
                }
            }

            return oList;
        }

        private void UserOperationsSenderSMS(object input)
        {
            UserOperations oUser = null;
            string strLiteral = string.Empty;
            string strMessage = string.Empty;
            try
            {
                    UserOperationsSenderSMSParameter oInputParameter = (UserOperationsSenderSMSParameter)input;

                    oUser = oInputParameter.m_oUserOperationsSenderSMS;
                    strLiteral = oInputParameter.sliteral;
               
                    strMessage = string.Format(strLiteral, oUser.Minute, oUser.Plate, oUser.GrpDescription);
               
                if (!String.IsNullOrEmpty(strMessage))
                {
                    string strCompleteTelephone = string.Empty;
                    long? lSendSMSTo=null;
                    
                    lSendSMSTo = infraestructureRepository.SendSMSTo(Convert.ToInt32(oUser.UsrMainTelCountry), oUser.UsrMainTel, strMessage,oUser.UsrLastSourceApp, ref  strCompleteTelephone);
   
                    if (lSendSMSTo.HasValue && lSendSMSTo.Value != -1)
                    {
                            DeleteUserOperationsSenderSMSFromRunningThreads(oUser);
                            infraestructureRepository.MarkSMSOperation(oUser);
                    }
                }
                else
                {
                    m_Log.LogMessage(LogLevels.logERROR, "<< CintegraMobileNotifier::NotifierSenderSMSThread:: Message=Empty message verify the literals");
                }
            }
            catch(Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::UserOperationsSenderSMS: Exception {0}", e.Message));
            }
        }

        void AddUserOperationsSenderSMSToRunningThreads(UserOperations oUserOperations)
        {
            
            try
            {
                lock (m_oHashUserOperationsSenderSMS)
                {
                    if (!m_oHashUserOperationsSenderSMS.ContainsKey(oUserOperations.OpeId))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AppUserOperationsSenderSMSToRunningThreads >>" + string.Format("OPERATIONS.OPE_ID={0}", oUserOperations.OpeId));
                        m_oHashUserOperationsSenderSMS.Add(oUserOperations.OpeId, oUserOperations);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AppUserOperationsSenderSMSToRunningThreads", ex);
            }
        }

        void DeleteUserOperationsSenderSMSFromRunningThreads(UserOperations oUserOperations)
        {
            try
            {
                lock (m_oHashUserOperationsSenderSMS)
                {
                    if (m_oHashUserOperationsSenderSMS.ContainsKey(oUserOperations.OpeId))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteUserOperationsSenderSMSFromRunningThreads >>" + string.Format("OPERATIONS.OPE_ID={0}", oUserOperations.OpeId));
                        m_oHashUserOperationsSenderSMS.Remove(oUserOperations.OpeId);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteUserOperationsSenderSMSFromRunningThreads", ex);
            }
        }
        #endregion

        #region USER WARNING
        private bool GetWaitingUsersWarnings(out List<USERS_WARNING> oListUsersWarnings, int iMaxUserWarningsToReturn, out int iQueueLengthUsersWarnings, List<decimal> oListRunningUsersWarnings)
        {
            bool bResp = true;
            oListUsersWarnings = null;
            iQueueLengthUsersWarnings = 0;
            try
            {
                infraestructureRepository.GetListOfUsersWarningsToSend(out oListUsersWarnings, iMaxUserWarningsToReturn, out iQueueLengthUsersWarnings, oListRunningUsersWarnings);

            }
            catch (Exception e)
            {
                bResp = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GetWaitingUsersWarnings: Exception {0}", e.Message));
            }

            return bResp;
        }

        List<decimal> GetListOfRunningUsersWarnings()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashUsersWarnings)
            {
                IDictionaryEnumerator denum = m_oHashUsersWarnings.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((USERS_WARNING)dentry.Value).UWA_ID);
                }
            }

            return oList;
        }

        private void UsersWarnings(object input)
        {
            USERS_WARNING oUsersWarnings = null;
            
            try
            {
                UsersWarningsParameter oInputParameter = (UsersWarningsParameter)input;
                oUsersWarnings = oInputParameter.m_oUsersWarnings;

                decimal oNotif = 0;
                if (GenerateInsertionUserNotification(oUsersWarnings, ref oNotif))
                {
                    if (oNotif != 0)
                    {
                        if (infraestructureRepository.SetUsersWarningsStatus(oUsersWarnings, oNotif))
                        {
                            DeleteUsersWarningsFromRunningThreads(oUsersWarnings);
                        }
                    }
                }
                else
                {
                    DeleteUsersWarningsFromRunningThreads(oUsersWarnings);
                }
            }
            catch(Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::UsersWarnings: Exception {0}", e.Message));
            }
        }

        void AddUsersWarningsToRunningThreads(USERS_WARNING oUsersWarnings)
        {
            try
            {
                lock (m_oHashUsersWarnings)
                {
                    if (!m_oHashUsersWarnings.ContainsKey(oUsersWarnings.UWA_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::AddUsersWarningsToRunningThreads >>" + string.Format("USERS_WARNING.UWA_ID={0}", oUsersWarnings.UWA_ID));
                        m_oHashUsersWarnings.Add(oUsersWarnings.UWA_ID, oUsersWarnings);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::AddUsersWarningsToRunningThreads", ex);
            }
        }

        void DeleteUsersWarningsFromRunningThreads(USERS_WARNING oUsersWarnings)
        {
            try
            {
                lock (m_oHashUsersWarnings)
                {
                    if (m_oHashUsersWarnings.ContainsKey(oUsersWarnings.UWA_ID))
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, "CintegraMobileNotifier::DeleteUsersWarningsFromRunningThreads >>" + string.Format("USERS_WARNING.UWA_ID={0}", oUsersWarnings.UWA_ID));
                        m_oHashUsersWarnings.Remove(oUsersWarnings.UWA_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CintegraMobileNotifier::DeleteUsersWarningsFromRunningThreads", ex);
            }
        }
        #endregion

        #region Methods Notifier Manager
        int GetCurrentRunningThreads()
        {
            int iRunningThreads = 0;

            lock (infraestructureRepository)
            {
               iRunningThreads = m_oHashNotifierExternalTicket.Count + 
                                  m_oHashInsertionParkingNotification.Count + 
                                  m_oHashBeforeEndParkingNotification.Count +
                                  m_oHashOperationsOffStreetNotification.Count +
                                  m_oHashUsersSecurityOperationNotification.Count +
                                  m_oHashUserNotificationMultiplier.Count +
                                  m_oHashPushIdNotificationSender.Count +
                                  m_oHashUserOperationsSenderSMS.Count +
                                  m_oHashUsersWarnings.Count;
            }
            return iRunningThreads;
        }

        #endregion

        #region Methods Obsolete
        [Obsolete]
        protected void NotifierGeneratorThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::NotifierGeneratorThread");

            bool bFinishServer = false;
            while (bFinishServer == false)
            {

                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimeNotificationGenerator, false));
                if (!bFinishServer)
                {
                    EXTERNAL_TICKET oTicket = null;
                    int iQueueLengthExternalTicket = 0;
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GetInsertionTicketNotificationData(out oTicket, out iQueueLengthExternalTicket, null);
                    }
                    while ((!bFinishServer) && (oTicket != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            if (GenerateInsertionTicketNotification(oTicket))
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.MarkAsGeneratedInsertionTicketNotification(oTicket);
                                    infraestructureRepository.GetInsertionTicketNotificationData(out oTicket, out iQueueLengthExternalTicket, null);
                                }
                            }
                            else
                            {
                                oTicket = null;
                            }
                        }
                    }
                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    EXTERNAL_PARKING_OPERATION oParking = null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GetInsertionParkingNotificationData(out oParking, out iQueueLength, null);
                    }
                    while ((!bFinishServer) && (oParking != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            if (GenerateInsertionParkingNotification(oParking))
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.MarkAsGeneratedInsertionParkingNotificationData(oParking);
                                    infraestructureRepository.GetInsertionParkingNotificationData(out oParking, out iQueueLength, null);
                                }
                            }
                            else
                            {
                                oParking = null;
                            }
                        }
                    }
                }


                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    EXTERNAL_PARKING_OPERATION oParking = null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GetBeforeEndParkingNotificationData(m_iMinutesBeforeEndToSendWarning, out oParking, out iQueueLength, null);
                    }
                    while ((!bFinishServer) && (oParking != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            if (GenerateBeforeEndParkingNotification(oParking))
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.MarkAsGeneratedBeforeEndParkingNotificationData(oParking);
                                    infraestructureRepository.GetBeforeEndParkingNotificationData(m_iMinutesBeforeEndToSendWarning, out oParking, out iQueueLength, null);
                                }
                            }
                            else
                            {
                                oParking = null;
                            }
                        }
                    }
                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    OPERATIONS_OFFSTREET oOperation = null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GetOffstreetOperationNotificationData(out oOperation, out iQueueLength, null);
                    }
                    while ((!bFinishServer) && (oOperation != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            if (GenerateOffstreetOperationNotification(oOperation))
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.MarkAsGeneratedOffstreetOperationNotificationData(oOperation);
                                    infraestructureRepository.GetOffstreetOperationNotificationData(out oOperation, out iQueueLength, null);
                                }
                            }
                            else
                            {
                                oOperation = null;
                            }
                        }
                    }
                }

				 bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {
                    USERS_SECURITY_OPERATION oSecurityOperation = null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GetInsertionUserSecurityDataNotificationData(out oSecurityOperation, out iQueueLength, null);
                    }

                    decimal dNotifId;

                    while ((!bFinishServer) && (oSecurityOperation != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            dNotifId = 0;
                            if (GenerateInsertionUserSecurityDataNotification(oSecurityOperation, ref dNotifId))
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.MarkAsGeneratedUserSecurityDataNotificationData(oSecurityOperation, dNotifId);
                                    infraestructureRepository.GetInsertionUserSecurityDataNotificationData(out oSecurityOperation, out iQueueLength, null);
                                }
                            }
                            else
                            {
                                oSecurityOperation = null;
                            }
                        }
                    }
                }
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::NotifierGeneratorThread");
        }
        [Obsolete]
        protected void NotifierMultiplierThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::NotifierMultiplierThread");

            bool bFinishServer = false;

            while (bFinishServer == false)
            {
                
                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimeNotifications, false));
                if (!bFinishServer)
                {
                    USERS_NOTIFICATION oNotif=null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GetFirstNotGeneratedUserNotification(out oNotif, out iQueueLength, null);
                    }
                    while ((!bFinishServer)&&(oNotif!=null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0 , false));
                        if (!bFinishServer)
                        {
                            lock (infraestructureRepository)
                            {
                                if (infraestructureRepository.GenerateUserNotification(ref oNotif))
                                {
                                    oNotif = null;
                                    infraestructureRepository.GetFirstNotGeneratedUserNotification(out oNotif, out iQueueLength, null);
                                }
                                else
                                {
                                    oNotif = null;
                                }
                            }
                        }
                    }
                }
                      
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::NotifierMultiplierThread");
        }
        [Obsolete]
        protected void NotifierSenderThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::NotifierSenderThread");

            bool bFinishServer = false;

            while (!bFinishServer)
            {
                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimeNotifications, false));
                if (!bFinishServer)
                {
                    PUSHID_NOTIFICATION oNotif = null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GetFirstNotSentNotification(out oNotif, m_iResendTime, out iQueueLength, null);
                    }

                    if (oNotif != null)
                    {
                        lock (m_NumPushPendingOfFinishingLock) 
                        {
                            m_NumPushPendingOfFinishing++;
                        }
                    }

                    while ((!bFinishServer) && (oNotif != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            SendNotification(ref oNotif);
                            oNotif = null;
                            lock (infraestructureRepository)
                            {
                                infraestructureRepository.GetFirstNotSentNotification(out oNotif, m_iResendTime, out iQueueLength, null);
                            }
                        }
                    }
                }
            }

            while (m_NumPushPendingOfFinishing > 0)
            {
                Thread.Sleep(500);
            }

            
            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::NotifierSenderThread");
        }
        [Obsolete]
        protected void NotifierSenderSMSThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::NotifierSenderSMSThread");

            bool bFinishServer = false;

            while (!bFinishServer)
            {
                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimeNotifications, false));
                if (!bFinishServer)
                {

                    List<UserOperations> oUSERList = null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                       // infraestructureRepository.FilterSMS();
                        infraestructureRepository.GetListOfUsersToSendSMS(out oUSERList, m_iMaxOfRecordsToResolveByNotifierSenderSMS, out iQueueLength, null);
                    }
                    while ((!bFinishServer) && (oUSERList != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            List<LITERAL_LANGUAGE> listLiterlLanguage = new List<LITERAL_LANGUAGE>();
                            if (oUSERList.Count > 0)
                            {
                                infraestructureRepository.GetLiteralsOfTheMessage(out listLiterlLanguage, m_iLiteralIdSMS);
                            }

                            foreach (UserOperations oUser in oUSERList)
                            {
                                bFinishServer = (m_evStopServer.WaitOne(0, false));

                                if (bFinishServer)
                                    break;

                                LITERAL_LANGUAGE oLitLang = listLiterlLanguage.FirstOrDefault(x => x.LANGUAGE.LAN_CULTURE.Equals(oUser.UsrCultureLang));
                                string strMessage = string.Format(oLitLang.LITL_LITERAL, oUser.Minute, oUser.Plate, oUser.GrpDescription);

                                if (!String.IsNullOrEmpty(strMessage))
                                {
                                    m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Sending SMS to User:{0} / {1} / {2} / {3} ", oUser.UsrEmail, Convert.ToInt32(oUser.UsrMainTelCountry), oUser.UsrMainTel, strMessage));

                                    string strCompleteTelephone = string.Empty;
                                    long lSendSMSTo;
                                    lSendSMSTo = infraestructureRepository.SendSMSTo(Convert.ToInt32(oUser.UsrMainTelCountry), oUser.UsrMainTel, strMessage, oUser.UsrLastSourceApp, ref  strCompleteTelephone);
                                    if (lSendSMSTo != null && lSendSMSTo != -1)
                                    {
                                        lock (infraestructureRepository)
                                        {
                                            infraestructureRepository.MarkSMSOperation(oUser);
                                        }
                                    }
                                }
                                else
                                {
                                    m_Log.LogMessage(LogLevels.logERROR, "<< CintegraMobileNotifier::NotifierSenderSMSThread:: Message=Empty message verify the literals");
                                }
                            }

                            oUSERList = null;
                        }
                    }
                }
            }
            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::NotifierSenderSMSThread");
        }
        [Obsolete]
        protected void UsersWarningsThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::UsersWarningsThread");

            bool bFinishServer = false;

            while (!bFinishServer)
            {
                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimeNotifications, false));
                if (!bFinishServer)
                {

                    List<USERS_WARNING> oUserWarningList = null;
                    int iQueueLength = 0;
                    lock (infraestructureRepository)
                    {
                        //m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Acceso al Get de Obtener la lista de los userswarning"));
                        infraestructureRepository.GetListOfUsersWarningsToSend(out oUserWarningList, m_iMaxOfRecordsToResolveByUsersWarnings, out iQueueLength, null);
                    }

                    while ((!bFinishServer) && (oUserWarningList != null))
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            foreach (USERS_WARNING oUser in oUserWarningList)
                            {
                                //m_Log.LogMessage(LogLevels.logDEBUG, string.Format("entra en el forech de users_warning del Star {0}", oUser.UWA_USER_ID));
                                bFinishServer = (m_evStopServer.WaitOne(0, false));

                                if (bFinishServer)
                                {
                                    break;
                                }
                                decimal oNotif = 0;
                                if (GenerateInsertionUserNotification(oUser, ref oNotif))
                                {
                                    if (oNotif!=0)
                                    {
                                        //m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Notifications users created according to Users warning: UNO_ID:{0} / UWA_ID:{1}", oNotif, oUser.UWA_ID));
                                        infraestructureRepository.SetUsersWarningsStatus(oUser,oNotif);
                                    }
                                }
                            }
                        }
                        oUserWarningList = null;
                    }
                }
            }
            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::UsersWarningsThread");
        }
        [Obsolete]
        protected void PlatesSenderThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileNotifier::PlatesSenderThread");

            bool bFinishServer = false;

            int iCurrRetries = 0;

            while (!bFinishServer)
            {
                iCurrRetries = 0;
                    
                bFinishServer = (m_evStopServer.WaitOne(1000 * m_iPoolTimePlatesSending, false));

                if (!bFinishServer)
                {
                    lock (infraestructureRepository)
                    {
                        infraestructureRepository.GeneratePlatesSending();
                    }

                    while (!bFinishServer)
                    {
                        IEnumerable<USER_PLATE_MOVS_SENDING> oPlateList = null;
                        lock (infraestructureRepository)
                        {
                             oPlateList = infraestructureRepository.GetPlatesForSending(m_iMaxPlatesToSend);
                        }

                        if (oPlateList.Count() > 0)
                        {
                            if (SendPlateMovements(oPlateList))
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.ConfirmSentPlates(oPlateList);
                                }
                                iCurrRetries = 0;
                            }
                            else
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.ErrorSedingPlates(oPlateList);
                                }
                                iCurrRetries++;

                                if (iCurrRetries == m_iRetries)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }

                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                    }
                }
                bFinishServer = (m_evStopServer.WaitOne(0, false));
            }
            

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileNotifier::PlatesSenderThread");
        }
        #endregion

        #region Methods Privates
        private bool GenerateInsertionTicketNotification(EXTERNAL_TICKET oTicket)
        {
            bool bRes = true;
            try
            {
                IEnumerable<USER> oUsersList = null;
                if (customersRepository.GetUsersWithPlate(oTicket.EXTI_PLATE, out oUsersList))
                {
                    FineNotificationType oFineNotificationType = oTicket.INSTALLATION.INS_FINE_NOTIFICATION_TYPE.HasValue ? (FineNotificationType)oTicket.INSTALLATION.INS_FINE_NOTIFICATION_TYPE.Value : FineNotificationType.Nocontrol;
                    foreach (USER user in oUsersList)
                    {
                        
                        USER oUser = user;
                        if (oFineNotificationType == FineNotificationType.Nocontrol ||
                                       (oUser.USR_FIRST_OPERATION_INS_ID.HasValue && oUser.USR_FIRST_OPERATION_INS_ID.Value == oTicket.INSTALLATION.INS_ID))
                        {
                            string culture = user.USR_CULTURE_LANG;
                            CultureInfo ci = new CultureInfo(culture);
                            Thread.CurrentThread.CurrentUICulture = ci;
                            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

                            // WP Notification
                            string strToastText1 = string.Format(Resources.WPTicketNotificationText1, oTicket.EXTI_TICKET_NUMBER);
                            string strToastText2 = string.Format(Resources.WPTicketNotificationText2, oTicket.EXTI_LIMIT_DATE);
                            string strToastParam = string.Format("/Screens/UserTicketSummaryPage.xaml?finenumber={0}&CityID={1}", oTicket.EXTI_TICKET_NUMBER, oTicket.INSTALLATION.INS_ID);
                            string strTileTitle = "";
                            int iTileCount = 0;
                            string strBackgroundImage = "";

                            //Android notification
                            Dictionary<string, string> oDictAndroid = new Dictionary<string, string>();
                            Dictionary<string, object> oDictIOS = new Dictionary<string, object>();
                            Dictionary<string, string> oDictAPS = new Dictionary<string, string>();

                            oDictAndroid["message"] = string.Format(Resources.AndroidTicketNotificationTextMessage, oTicket.EXTI_TICKET_NUMBER, oTicket.EXTI_PLATE);
                            oDictAPS["alert"] = oDictAndroid["message"];
                            oDictIOS["aps"] = oDictAPS;
                            oDictIOS["message"] = oDictAndroid["message"]; ;
                            oDictIOS["type"] = oDictAndroid["type"] = Convert.ToInt32(NotificationEventType.TicketInsertion).ToString();
                            oDictIOS["city_id"] = oDictAndroid["city_id"] = oTicket.INSTALLATION.INS_ID.ToString();
                            oDictIOS["f"] = oDictAndroid["f"] = oTicket.EXTI_TICKET_NUMBER;
                            oDictIOS["q"] = oDictAndroid["q"] = oTicket.EXTI_AMOUNT.ToString();
                            oDictIOS["cur"] = oDictAndroid["cur"] = oTicket.INSTALLATION.CURRENCy.CUR_ISO_CODE;
                            oDictIOS["d"] = oDictAndroid["d"] = oTicket.EXTI_DATE.ToString("HHmmssddMMyy");
                            oDictIOS["lp"] = oDictAndroid["lp"] = oTicket.EXTI_PLATE;
                            oDictIOS["df"] = oDictAndroid["df"] = oTicket.EXTI_LIMIT_DATE.ToString("HHmmssddMMyy");
                            oDictIOS["ta"] = oDictAndroid["ta"] = oTicket.EXTI_ARTICLE_TYPE;
                            oDictAndroid["dta"] = oTicket.EXTI_ARTICLE_DESCRIPTION;


                            string strAndroidRawData = JsonConvert.SerializeObject(oDictAndroid, Newtonsoft.Json.Formatting.None);
                            string strIOSRawData = JsonConvert.SerializeObject(oDictIOS, Newtonsoft.Json.Formatting.None);

                            decimal dNotifId = 0;

                            bRes = customersRepository.AddPushIDNotification(ref oUser,
                                                                            strToastText1,
                                                                            strToastText2,
                                                                            strToastParam,
                                                                            strTileTitle,
                                                                            iTileCount,
                                                                            strBackgroundImage,
                                                                            strAndroidRawData,
                                                                            strIOSRawData,
                                                                            ref dNotifId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GenerateInsertionTicketNotification: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool GenerateInsertionParkingNotification(EXTERNAL_PARKING_OPERATION oParking)
        {
            bool bRes = true;
            try
            {
                IEnumerable<USER> oUsersList=null;
                if (customersRepository.GetUsersWithPlate(oParking.EPO_PLATE, out oUsersList))
                {

                    foreach (USER user in oUsersList)
                    {

                        string culture = user.USR_CULTURE_LANG;
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);  
                        
                        string strToastText1="";
                        string strToastText2="";
                        string strToastParam="";
                        string strTileTitle="";
                        int iTileCount=0;
                        string strBackgroundImage="";
                        string strAndroidRawData="";
                        USER oUser = user;

                        //Android notification
                        Dictionary<string, string> oDict = new Dictionary<string, string>();

                        oDict["message"] = string.Format(Resources.AndroidParkingNotificationTextMessage, oParking.EPO_PLATE);
                        oDict["type"] = Convert.ToInt32(NotificationEventType.ParkingInsertion).ToString();
                        oDict["city_id"] = oParking.INSTALLATION.INS_ID.ToString();
                        oDict["p"] = oParking.EPO_PLATE;
                        oDict["g"] = oParking.EPO_ZONE.ToString();
                        oDict["ad"] = oParking.EPO_TARIFF.ToString();
                        oDict["bd"] = oParking.EPO_INIDATE.HasValue ? oParking.EPO_INIDATE.Value.ToString("HHmmssddMMyy") : "";
                        oDict["q"] = oParking.EPO_AMOUNT.ToString();
                        oDict["cur"] = oParking.INSTALLATION.CURRENCy.CUR_ISO_CODE;
                        oDict["t"] = oParking.EPO_TIME.ToString();
                        oDict["ed"] = oParking.EPO_ENDDATE.ToString("HHmmssddMMyy");
                        

                        strAndroidRawData = JsonConvert.SerializeObject(oDict, Newtonsoft.Json.Formatting.None);

                        decimal dNotifId = 0;

                        bRes = customersRepository.AddPushIDNotification(ref oUser,
                                                                        strToastText1,
                                                                        strToastText2,
                                                                        strToastParam,
                                                                        strTileTitle,
                                                                        iTileCount,
                                                                        strBackgroundImage,
                                                                        strAndroidRawData,
                                                                        strAndroidRawData,
                                                                        ref dNotifId);
                            
                    }

                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GenerateInsertionParkingNotification: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool GenerateBeforeEndParkingNotification(EXTERNAL_PARKING_OPERATION oParking)
        {
            bool bRes = true;
            try
            {
                IEnumerable<USER> oUsersList = null;
                if (customersRepository.GetUsersWithPlate(oParking.EPO_PLATE, out oUsersList))
                {

                    foreach (USER user in oUsersList)
                    {

                        string culture = user.USR_CULTURE_LANG;
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);  
                        
                        string strToastText1 = "";
                        string strToastText2 = "";
                        string strToastParam = "";
                        string strTileTitle = "";
                        int iTileCount = 0;
                        string strBackgroundImage = "";
                        string strAndroidRawData = "";
                        USER oUser = user;


                        //Android notification
                        Dictionary<string, string> oDict = new Dictionary<string, string>();

                        oDict["message"] = string.Format(Resources.AndroidExpiringParkingNotificationTextMessage, oParking.EPO_PLATE, oParking.EPO_ENDDATE);
                        oDict["type"] = Convert.ToInt32(NotificationEventType.BeforeEndParking).ToString();
                        oDict["city_id"] = oParking.INSTALLATION.INS_ID.ToString();
                        oDict["p"] = oParking.EPO_PLATE;
                        oDict["g"] = oParking.EPO_ZONE.ToString();
                        oDict["ad"] = oParking.EPO_TARIFF.ToString();
                        oDict["bd"] = oParking.EPO_INIDATE.HasValue ? oParking.EPO_INIDATE.Value.ToString("HHmmssddMMyy") : "";
                        oDict["q"] = oParking.EPO_AMOUNT.ToString();
                        oDict["cur"] = oParking.INSTALLATION.CURRENCy.CUR_ISO_CODE;
                        oDict["t"] = oParking.EPO_TIME.ToString();
                        oDict["ed"] = oParking.EPO_ENDDATE.ToString("HHmmssddMMyy");


                        strAndroidRawData = JsonConvert.SerializeObject(oDict, Newtonsoft.Json.Formatting.None);


                        decimal dNotifId = 0;


                        bRes = customersRepository.AddPushIDNotification(ref oUser,
                                                                        strToastText1,
                                                                        strToastText2,
                                                                        strToastParam,
                                                                        strTileTitle,
                                                                        iTileCount,
                                                                        strBackgroundImage,
                                                                        strAndroidRawData,
                                                                        strAndroidRawData,
                                                                        ref dNotifId);

                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GenerateBeforeEndParkingNotification: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool GenerateOffstreetOperationNotification(OPERATIONS_OFFSTREET oOperation)
        {
            bool bRes = true;
            try
            {
                IEnumerable<USER> oUsersList = null;
                if (customersRepository.GetUsersWithPlate(oOperation.USER_PLATE.USRP_PLATE, out oUsersList))
                {
                    foreach (USER user in oUsersList)
                    {
                        USER oUser = user;

                        string culture = user.USR_CULTURE_LANG;
                        CultureInfo ci = new CultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = ci;
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

                        // WP Notification
                        string strToastText1 = "";
                        string strToastText2 = "";
                        string strToastParam = "";
                        string strTileTitle = "";
                        int iTileCount = 0;
                        string strBackgroundImage = "";

                        //Android notification
                        Dictionary<string, string> oDict = new Dictionary<string, string>();

                        decimal? dGroupId = oOperation.OPEOFF_GRP_ID;
                        GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetWsConfiguration = null;
                        DateTime? dtgroupDateTime = null;
                        geograficAndTariffsRepository.getOffStreetConfiguration(dGroupId, null, null, ref oOffstreetWsConfiguration, ref dtgroupDateTime);

                        oDict["message"] = string.Format(Resources.AndroidOffstreetEntryNotificationTextMessage, oOperation.USER_PLATE.USRP_PLATE);
                        oDict["type"] = Convert.ToInt32(NotificationEventType.OffstreetParkingEntry).ToString();
                        oDict["g"] = oOffstreetWsConfiguration.GOWC_GRP_ID.ToString();
                        oDict["ope_id"] = oOperation.OPEOFF_ID.ToString();
                        oDict["p"] = oOperation.USER_PLATE.USRP_PLATE;

                        if (oOperation.OPEOFF_TYPE == (int)OffstreetOperationType.Entry)
                        {
                            oDict["gate_id"] = oOperation.OPEOFF_GATE;
                            oDict["ed"] = oOperation.OPEOFF_ENTRY_DATE.ToString("HHmmssddMMyy");
                        }
                        else
                        {
                            oDict["q"] = oOperation.OPEOFF_AMOUNT.ToString();
                            oDict["cur"] = oOperation.CURRENCy.CUR_ISO_CODE;
                            oDict["bd"] = oOperation.OPEOFF_ENTRY_DATE.ToString("HHmmssddMMyy");
                            oDict["ed"] = oOperation.OPEOFF_EXIT_DATE.Value.ToString("HHmmssddMMyy");
                        }

                        string strAndroidRawData = JsonConvert.SerializeObject(oDict, Newtonsoft.Json.Formatting.None);


                        decimal dNotifId = 0;
                        bRes = customersRepository.AddPushIDNotification(ref oUser,
                                                                        strToastText1,
                                                                        strToastText2,
                                                                        strToastParam,
                                                                        strTileTitle,
                                                                        iTileCount,
                                                                        strBackgroundImage,
                                                                        strAndroidRawData,
                                                                        strAndroidRawData,
                                                                        ref dNotifId);
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GenerateOffstreetOperationNotification: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool GenerateInsertionUserSecurityDataNotification(USERS_SECURITY_OPERATION oSecurityOperation,ref decimal dNotifId)
        {
            bool bRes = true;
            try
            {
                USER oUser = oSecurityOperation.USER;

                string culture = oUser.USR_CULTURE_LANG;
                CultureInfo ci = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

                // WP Notification
                string strToastText1 = string.Format(Resources.PasswordRecovery, oSecurityOperation.USOP_ACTIVATION_CODE, m_iNumPasswordRecoveryNumMinutes);

                //Android notification
                Dictionary<string, string> oDict = new Dictionary<string, string>();

                oDict["message"] = string.Format(Resources.PasswordRecovery, oSecurityOperation.USOP_ACTIVATION_CODE, m_iNumPasswordRecoveryNumMinutes);
                oDict["type"] = Convert.ToInt32(NotificationEventType.PasswordRecovery).ToString();
                oDict["code"] = oSecurityOperation.USOP_ACTIVATION_CODE;

                string strAndroidRawData = JsonConvert.SerializeObject(oDict, Newtonsoft.Json.Formatting.None);
                oDict["dta"] = "";
                string strIOSRawData = JsonConvert.SerializeObject(oDict, Newtonsoft.Json.Formatting.None);

                bRes = customersRepository.AddPushIDNotification(ref oUser,
                                                                strToastText1,
                                                                "",
                                                                "",
                                                                "",
                                                                0,
                                                                "",
                                                                strAndroidRawData,
                                                                strIOSRawData,
                                                                ref dNotifId,
                                                                oSecurityOperation.USERS_PUSH_ID.UPID_ID);
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GenerateInsertionUserSecurityDataNotification: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool SendNotification (ref PUSHID_NOTIFICATION oNotif)
        {
            bool bRes = true;
            try
            {
                switch ((MobileOS)oNotif.PNO_OS)
                {

                    case MobileOS.Android:
                        {
                            if (!string.IsNullOrEmpty(oNotif.PNO_ANDROID_RAW_DATA))
                            {
                                GcmNotification gcmNotification = new GcmNotification()
                                {
                                    RegistrationIds = { oNotif.PNO_PUSHID },
                                    Data = JObject.Parse(oNotif.PNO_ANDROID_RAW_DATA)
                                };
                                   
                                    
                                    
                                gcmNotification.Tag = new CintegraMobileNotifierTag
                                    {
                                        dPushNotifID = oNotif.PNO_ID,
                                        Repository = infraestructureRepository
                                    };

                                USERS_PUSH_ID oUserPushId=null;
                                if (customersRepository.GetUserPushID(oNotif.PNO_PUSHID, ref oUserPushId))
                                {
                                    if (!string.IsNullOrEmpty(oUserPushId.UPID_APP_VERSION))
                                    {
                                        ulong version = AppUtilities.AppVersion(oUserPushId.UPID_APP_VERSION);

                                        if (version >= _VERSION_2_13)
                                        {
                                            m_Log.LogMessage(LogLevels.logINFO, string.Format("SendNotification: FireBase: Queuing notification push id = {0} for app {1}", oNotif.PNO_PUSHID, infraestructureRepository.GetSourceAppCode(oNotif.PNO_SOAPP_ID.Value)));
                                            m_FireBasePushBrokers[oNotif.PNO_SOAPP_ID.Value].QueueNotification(gcmNotification);
                                        }
                                        else
                                        {
                                            m_Log.LogMessage(LogLevels.logINFO, string.Format("SendNotification: GCM: Queuing notification push id = {0} for app {1}", oNotif.PNO_PUSHID, infraestructureRepository.GetSourceAppCode(oNotif.PNO_SOAPP_ID.Value)));
                                            m_GcmPushBrokers[oNotif.PNO_SOAPP_ID.Value].QueueNotification(gcmNotification);
                                        }
                                    }
                                    else
                                    {
                                        m_Log.LogMessage(LogLevels.logINFO, string.Format("SendNotification: APP Version =NULL: GCM: Queuing notification push id = {0} for app {1}", oNotif.PNO_PUSHID, infraestructureRepository.GetSourceAppCode(oNotif.PNO_SOAPP_ID.Value)));
                                        m_GcmPushBrokers[oNotif.PNO_SOAPP_ID.Value].QueueNotification(gcmNotification);
                                    }
                                }
                                else
                                {
                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("SendNotification: USER PUSH ID=NULL: GCM: Queuing notification push id = {0} for app {1}", oNotif.PNO_PUSHID, infraestructureRepository.GetSourceAppCode(oNotif.PNO_SOAPP_ID.Value)));
                                    m_GcmPushBrokers[oNotif.PNO_SOAPP_ID.Value].QueueNotification(gcmNotification);
                                }

                            }
                            else
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.PushIdNotificationSent(Convert.ToDecimal(oNotif.PNO_ID));
                                }
                            }
                        }
                        break;

                    case MobileOS.iOS:
                        {
                            if (!string.IsNullOrEmpty(oNotif.PNO_iOS_RAW_DATA))
                            {
                                string strJson = oNotif.PNO_iOS_RAW_DATA;


                                var appleNotification = new ApplePush(ApplePushType.Alert)
                                            .AddToken(oNotif.PNO_PUSHID);
                                            
                                
                                /*
                                ApnsNotification appleNotification = new ApnsNotification()
                                {
                                    DeviceToken = oNotif.PNO_PUSHID,
                                    Payload = JObject.Parse(strJson)
                                };*/

                                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(oNotif.PNO_iOS_RAW_DATA);

                                foreach (KeyValuePair<string, object> value in values)
                                {

                                    if (value.Key == "message")
                                    {
                                        appleNotification.AddAlert((string)value.Value);
                                    }
                                    else if (value.Key != "aps")
                                    {
                                        appleNotification.AddCustomProperty(value.Key, (string)value.Value,false);
                                    }
                                }

                                appleNotification.AddSound("default")
                                                 .AddBadge(7);


                                CintegraMobileNotifierTag  oNotifierTag = new CintegraMobileNotifierTag
                                {
                                    dPushNotifID = oNotif.PNO_ID,
                                    strRawData = oNotif.PNO_iOS_RAW_DATA,
                                    Repository = infraestructureRepository
                                };
                                m_Log.LogMessage(LogLevels.logINFO, string.Format("SendNotification: APNS: Queuing notification push id = {0} for app {1}", oNotif.PNO_ID, infraestructureRepository.GetSourceAppCode(oNotif.PNO_SOAPP_ID.Value)));

                                //m_ApnsPushBrokers[oNotif.PNO_SOAPP_ID.Value].QueueNotification(appleNotification);

                                APNSInfo apnsInfo = m_ApnsPushInfos[oNotif.PNO_SOAPP_ID.Value];


                                lock (apnsInfo)
                                {
                                    if (apnsInfo.apnsClient == null ||
                                        (DateTime.UtcNow - apnsInfo.dtClientUTC).TotalMilliseconds > ct_APNS_CLIENT_GENERATION_MINUTES * 60 * 1000)
                                    {
                                        if (apnsInfo.apnsClient != null)
                                            apnsInfo.apnsClient = null;

                                        var options = new ApnsJwtOptions()
                                        {
                                            BundleId = apnsInfo.BundleId,
                                            //CertContent = "",// use either CertContent or CertFilePath, not both
                                            CertFilePath = apnsInfo.CertFilePath,
                                            KeyId = apnsInfo.KeyId,
                                            TeamId = apnsInfo.TeamId
                                        };

                                        apnsInfo.apnsConfig = options;

                                        apnsInfo.apnsClient = ApnsClient.CreateUsingJwt(new HttpClient(new Http2CustomHandler()), apnsInfo.apnsConfig);
                                        apnsInfo.dtClientUTC = DateTime.UtcNow;
                                    }

                                    SendAPNSNotification(apnsInfo, appleNotification, oNotifierTag);
                                }

                            }
                            else
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.PushIdNotificationSent(Convert.ToDecimal(oNotif.PNO_ID));
                                }
                            }
                        }
                        break;

                   case MobileOS.WindowsPhone:
                        /*{


                            if ((!string.IsNullOrEmpty(oNotif.PNO_WP_TEXT1)) ||
                                (!string.IsNullOrEmpty(oNotif.PNO_WP_TEXT2)))
                            {
                                WindowsPhoneToastNotificationOverride WPNotification = new WindowsPhoneToastNotificationOverride();
                                WPNotification.ForEndpointUri(new Uri(oNotif.PNO_PUSHID));                                            
                                WPNotification.ForOSVersion(WindowsPhoneDeviceOSVersion.MangoSevenPointFive);
                                WPNotification.WithBatchingInterval(BatchingInterval.Immediate);
                                
                                if (!string.IsNullOrEmpty(oNotif.PNO_WP_TEXT1))
                                {
                                    WPNotification.WithText1(oNotif.PNO_WP_TEXT1);
                                }

                                if (!string.IsNullOrEmpty(oNotif.PNO_WP_TEXT2))
                                {
                                    WPNotification.WithText2(oNotif.PNO_WP_TEXT2);
                                }

                                if (!string.IsNullOrEmpty(oNotif.PNO_WP_PARAM))
                                {
                                    WPNotification.WithNavigatePath(oNotif.PNO_WP_PARAM);
                                }
                                


                                WPNotification.Tag = new CintegraMobileNotifierTag
                                    {
                                        dPushNotifID = oNotif.PNO_ID,
                                        Repository = infraestructureRepository
                                    };
                                m_PushBroker.QueueNotification((WindowsPhoneToastNotification)WPNotification);
                            }
                            else if ((!string.IsNullOrEmpty(oNotif.PNO_WP_TILE_TITLE)) ||
                                     (oNotif.PNO_WP_COUNT.HasValue) ||
                                     (!string.IsNullOrEmpty(oNotif.PNO_WP_BACKGROUND_IMAGE)))
                            {

                                WindowsPhoneTileNotification WPNotification = new WindowsPhoneTileNotification()
                                                                            .ForEndpointUri(new Uri(oNotif.PNO_PUSHID))
                                                                            .ForOSVersion(WindowsPhoneDeviceOSVersion.MangoSevenPointFive)
                                                                            .WithBatchingInterval(BatchingInterval.Immediate);

                                if (!string.IsNullOrEmpty(oNotif.PNO_WP_TILE_TITLE))
                                {
                                    WPNotification.WithTitle(oNotif.PNO_WP_TILE_TITLE);
                                }

                                if (oNotif.PNO_WP_COUNT.HasValue)
                                {
                                    WPNotification.WithCount(oNotif.PNO_WP_COUNT.Value);
                                }

                                if (!string.IsNullOrEmpty(oNotif.PNO_WP_BACKGROUND_IMAGE))
                                {
                                    WPNotification.WithBackgroundImage(oNotif.PNO_WP_BACKGROUND_IMAGE);
                                }

                                WPNotification.Tag = new CintegraMobileNotifierTag
                                {
                                    dPushNotifID = oNotif.PNO_ID,
                                    Repository = infraestructureRepository
                                };
                                m_PushBroker.QueueNotification(WPNotification);
                            }
                            else
                            {
                                lock (infraestructureRepository)
                                {
                                    infraestructureRepository.PushIdNotificationSent(Convert.ToDecimal(oNotif.PNO_ID));
                                }
                            }
                        }
                        break;*/
                    case MobileOS.Blackberry:
                        lock (infraestructureRepository)
                        {
                            infraestructureRepository.PushIdNotificationSent(Convert.ToDecimal(oNotif.PNO_ID));
                        }
                        break;
                    default:
                        lock (infraestructureRepository)
                        {
                            infraestructureRepository.PushIdNotificationSent(Convert.ToDecimal(oNotif.PNO_ID));
                        }
                        break;
                    

                }              
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::SendNotification: Exception {0}", e.Message));
                decimal dNoti = 0;
                lock (infraestructureRepository)
                {
                    infraestructureRepository.PushIdNotificationFailed(oNotif.PNO_ID, m_iRetries);
                    dNoti = oNotif.PNO_ID;
                }
                lock (m_NumPushPendingOfFinishingLock)
                {
                    m_NumPushPendingOfFinishing--;
                    if (dNoti != 0)
                    {
                        m_oHashPushIdNotificationSender.Remove(dNoti);
                    }
                }
                bRes = false;
            }

            return bRes;

        }




        private async void SendAPNSNotification(APNSInfo apnsInfo, ApplePush appleNotification, CintegraMobileNotifierTag  oTag )
        {
            decimal dNoti = 0;
            bool bError = false;
            bool bResetClient = false;
            try
            {
                if (apnsInfo.apnsClient != null)
                {
                    var response = await apnsInfo.apnsClient.Send(appleNotification);
                    if (response.IsSuccessful)
                    {
                        if (oTag != null)
                        {
                            dNoti = oTag.dPushNotifID;
                            m_Log.LogMessage(LogLevels.logINFO, "Sent: -> " + oTag.strRawData);
                            lock (oTag.Repository)
                            {
                                oTag.Repository.PushIdNotificationSent(Convert.ToDecimal(oTag.dPushNotifID));
                            }
                        }
                    }
                    else
                    {
                        bResetClient = true;
                        bError = true;
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("Failure: -> {0} -> {1}", response.ReasonString, oTag.strRawData));
                    }
                }
                else
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("Client is null"));
                    bError = true;
                }
            }
            catch (System.Threading.Tasks.TaskCanceledException ex1)
            {
                bResetClient = true;
                bError = true;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::SendAPNSNotification:Failed to send a push: HTTP request timed out. Exception {0}", ex1.Message));               
            }
            catch (HttpRequestException ex2)
            {
                bResetClient = true;
                bError = true;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::SendAPNSNotification:Failed to send a push. HTTP request failed Exception {0}", ex2.Message));
            }
            catch (Exception ex3)
            {
                bResetClient = true;
                bError = true;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::SendAPNSNotification:Failed to send a push. Generic Exception {0}", ex3.Message));
            }


            try
            {

                if (bError)
                {
                    if (bResetClient)
                    {
                        lock (apnsInfo)
                        {
                            apnsInfo.apnsClient = null;
                        }
                    }
                     
                    if (oTag != null)
                    {
                        dNoti = oTag.dPushNotifID;
                        lock (oTag.Repository)
                        {
                            /*if (notificationFailureException.InnerException.Message == "Device Subscription has Expired")
                            {
                                oTag.Repository.PushIdExpired(Convert.ToDecimal(oTag.dPushNotifID), "");
                            }
                            else
                            {*/
                            oTag.Repository.PushIdNotificationFailed(Convert.ToDecimal(oTag.dPushNotifID), m_iRetries);
                            //}
                        }
                    }

                }
            }
            catch { }

            lock (m_NumPushPendingOfFinishingLock)
            {
                m_NumPushPendingOfFinishing--;
                if (dNoti != 0)
                {
                    m_oHashPushIdNotificationSender.Remove(dNoti);
                }
            }
        }

        private bool SendPlateMovements(IEnumerable<USER_PLATE_MOVS_SENDING> oPlateList)
        {
            bool bRes = true;
            try
            {
                USER_PLATE_MOVS_SENDING oFirstMov = oPlateList.First();

                string strMovsXML = "<movs>";
                string strMovsForHash = "";

                foreach (USER_PLATE_MOVS_SENDING oMov in oPlateList)
                {
                    strMovsXML+=string.Format("<mov type=\"{0}\"><state>{1}</state><plate>{2}</plate></mov>",
                                                oMov.USER_PLATE_MOV.USRPM_MOV_TYPE,
                                                "",
                                                oMov.USER_PLATE_MOV.USRPM_PLATE);

                    strMovsForHash += "" + oMov.USER_PLATE_MOV.USRPM_PLATE;

                }

                strMovsXML += "</movs>";



                switch ((PlateSendingWSSignatureType)oFirstMov.INSTALLATION.INS_PLATE_UPDATE_WS_SIGNATURE_TYPE)
                {

                    case PlateSendingWSSignatureType.psst_test:
                        bRes=true;
                        break;
                    case PlateSendingWSSignatureType.psst_internal:
                        bRes=false;
                        break;
                    case PlateSendingWSSignatureType.psst_standard:
                        bRes = SendPlateMovementsStandard(strMovsXML, strMovsForHash, oFirstMov.INSTALLATION);
                        break;
                    case PlateSendingWSSignatureType.psst_eysa:
                        bRes = SendPlateMovementsEysa(strMovsXML, oFirstMov.INSTALLATION);
                        break;
                    default:
                        bRes=false;
                        break;
                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::SendPlateMovements: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool SendPlateMovementsStandard(string strXMLMovs, string strXMLHash, INSTALLATION oInstallation)
        {
            bool bRes = true;
            try
            {
                //StandardThirdPartyFineWS.PayByPhoneSoapImplService oPlateWS = new StandardThirdPartyFineWS.PayByPhoneSoapImplService();
                //oPlateWS.Url = oInstallation.INS_PLATE_UPDATE_WS_URL;
                //oPlateWS.Timeout = Get3rdPartyWSTimeout();

                DateTime? dtInstallation = geograficAndTariffsRepository.getInstallationDateTime(oInstallation.INS_ID);
                string strvers = "1.0";

                string strAuthHash = CalculateStandardWSHash(oInstallation.INS_PLATE_UPDATE_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1:HHmmssddMMyy}{2}", strXMLHash,dtInstallation, strvers));
                string strMessage = string.Format("<ipark_in>{0}<d>{1:HHmmssddMMyy}</d><vers>{2}</vers><ah>{3}</ah></ipark_in>",
                    strXMLMovs, dtInstallation, strvers, strAuthHash);

                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("StandardThirdPartyPlateChangesSending xmlIn = {0}", strMessage));


                string strOut = "";//oPlateWS.queryFinePaymentQuantity(strMessage);
                strOut = strOut.Replace("\n  ", "");
                strOut = strOut.Replace("\n ", "");
                strOut = strOut.Replace("\n", "");

                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("StandardThirdPartyPlateChangesSending xmlOut = {0}", strOut));


                SortedList wsParameters = null;

                bRes = FindOutParameters(strOut, out wsParameters);

                if (bRes)
                {

                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        bRes = true;

                    }
                    else
                    {
                        bRes = false;
                    }

                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::SendPlateMovementsStandard: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool SendPlateMovementsEysa(string strXMLMovs,INSTALLATION oInstallation)
        {
            bool bRes = true;
            try
            {

                //StandardThirdPartyFineWS.PayByPhoneSoapImplService oPlateWS = new StandardThirdPartyFineWS.PayByPhoneSoapImplService();
                //oPlateWS.Url = oInstallation.INS_PLATE_UPDATE_WS_URL;
                //oPlateWS.Timeout = Get3rdPartyWSTimeout();

                //oPlateWS.ConsolaSoapHeader authentication = new oPlateWS.ConsolaSoapHeader();
                //authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                //oPlateWS.ConsolaSoapHeaderValue = authentication;


                DateTime? dtInstallation = geograficAndTariffsRepository.getInstallationDateTime(oInstallation.INS_ID);
                string strvers = "1.0";

                string strMessage = string.Format("<ipark_in>{0}<d>{1:yyyy-MM-ddTHH:mm:ss.fff}</d><vers>{2}</vers><ah>authenticacion hash</ah></ipark_in>",
                    strXMLMovs, dtInstallation, strvers);

                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("EysaThirdPartyPlateChangesSending xmlIn = {0}", strMessage));


                string strOut = "";//oPlateWS.queryFinePaymentQuantity(strMessage);
                strOut = strOut.Replace("\n  ", "");
                strOut = strOut.Replace("\n ", "");
                strOut = strOut.Replace("\n", "");

                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("EysaThirdPartyPlateChangesSending xmlOut = {0}", strOut));


                SortedList wsParameters = null;

                bRes = FindOutParameters(strOut, out wsParameters);

                if (bRes)
                {

                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        bRes = true;

                    }
                    else
                    {
                        bRes = false;
                    }

                }



            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::SendPlateMovementsEysa: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private bool FindOutParameters(string xmlIn, out SortedList parameters)
        {
            bool bRes=true;
            parameters = new SortedList();


            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    xmlInDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xmlIn);

                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        switch (Node.Name)
                        {
                            default:

                                if (Node.HasChildNodes)
                                {
                                    if (Node.ChildNodes[0].HasChildNodes)
                                    {
                                        foreach (XmlNode ChildNode in Node.ChildNodes)
                                        {
                                            parameters[Node.Name + "_" + ChildNode.Name] = ChildNode.InnerText.Trim();

                                        }
                                    }
                                    else
                                    {
                                        parameters[Node.Name] = Node.InnerText.Trim();
                                    }
                                }
                                else
                                {
                                    parameters[Node.Name] = null;
                                }

                                break;

                        }

                    }

                    if (Nodes.Count == 0)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("FindParameters: Bad Input XML: xmlIn= {0}", xmlIn));
                        bRes = false;

                    }


                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("FindInputParameters: Bad Input XML: xmlIn= {0}:Exception", xmlIn), e);
                    bRes = false;
                }

            }
            catch (Exception e)
            {
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, "FindInputParameters::Exception",e);

            }


            return bRes;
        }

        private string CalculateStandardWSHash(string strMACKey, string strInput)
        {
            string strRes = "";
            int iKeyLength = 64;
            byte[] normMACKey = null;
            HMACSHA256 oMACsha256 = null;

            try
            {

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strMACKey);
                normMACKey = new byte[iKeyLength];
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
                    normMACKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }

                oMACsha256 = new HMACSHA256(normMACKey);


                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = oMACsha256.ComputeHash(inputBytes); ;

                if (hash.Length >= 8)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = hash.Length - 8; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    strRes = sb.ToString();
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "CalculateWSHash::Exception", e);
            }

            return strRes;
        }

        private int Get3rdPartyWSTimeout()
        {
            int iRes = DEFAULT_WS_TIMEOUT;
            try
            {
                iRes = Convert.ToInt32(ConfigurationManager.AppSettings["3rdPartyWSTimeout"].ToString());
            }
            catch
            {
                iRes = DEFAULT_WS_TIMEOUT;
            }

            return iRes;

        }

        public void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls11) == 0) //Disable TLs 1.1
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }

        static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Currently this event will only ever happen for Android GCM
            decimal dNoti = 0;
             m_Log.LogMessage(LogLevels.logDEBUG,"Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
             CintegraMobileNotifierTag oTag = (CintegraMobileNotifierTag)notification.Tag;
             if (oTag != null)
             {
                 lock (oTag.Repository)
                 {
                     dNoti = Convert.ToDecimal(oTag.dPushNotifID);
                     oTag.Repository.PushIdExpired(Convert.ToDecimal(oTag.dPushNotifID), newSubscriptionId);
                 }
             }
             lock (m_NumPushPendingOfFinishingLock)
             {
                 m_NumPushPendingOfFinishing--;
                 if (dNoti != 0)
                 {
                     m_oHashPushIdNotificationSender.Remove(dNoti);
                 }
                 
             }

        }

        static void NotificationSent(INotification notification)
        {
            decimal dNoti=0;
            m_Log.LogMessage(LogLevels.logDEBUG, "Sent: -> " + notification);
            CintegraMobileNotifierTag oTag = (CintegraMobileNotifierTag)notification.Tag;
            if (oTag != null)
            {
                lock (oTag.Repository)
                {
                    dNoti=Convert.ToDecimal(oTag.dPushNotifID);
                    oTag.Repository.PushIdNotificationSent(Convert.ToDecimal(oTag.dPushNotifID));
                }
            }
            lock (m_NumPushPendingOfFinishingLock)
            {
                m_NumPushPendingOfFinishing--;
                if(dNoti!=0)
                {
                    m_oHashPushIdNotificationSender.Remove(dNoti);
                }
            }
        }

        static void NotificationFailed(INotification notification, Exception notificationFailureException)
        {
            m_Log.LogMessage(LogLevels.logERROR, string.Format("Failure: -> {0} {1} -> {2} ", notificationFailureException.Message, notificationFailureException.InnerException.Message, notification));
            CintegraMobileNotifierTag oTag = (CintegraMobileNotifierTag)notification.Tag;
            decimal dNoti = 0;
            if (oTag != null)
            {
                lock (oTag.Repository)
                {
                    dNoti = oTag.dPushNotifID;
                    if (notificationFailureException.InnerException.Message == "Device Subscription has Expired")
                    {
                        oTag.Repository.PushIdExpired(Convert.ToDecimal(oTag.dPushNotifID), "");
                    }
                    else
                    {
                        oTag.Repository.PushIdNotificationFailed(Convert.ToDecimal(oTag.dPushNotifID), m_iRetries);
                    }
                }
            }
            lock (m_NumPushPendingOfFinishingLock)
            {
                m_NumPushPendingOfFinishing--;
                if (dNoti != 0)
                {
                    m_oHashPushIdNotificationSender.Remove(dNoti);
                }
            }
        }

        private bool GenerateInsertionUserNotification(USERS_WARNING usersWarning, ref decimal dNotifId)
        {
            bool bRes = true;
            try
            {
                USER oUser = usersWarning.USER;
                string culture = oUser.USR_CULTURE_LANG;
                CultureInfo ci = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

                string strToastText1 = "";
                string strToastText2 = "";
                string strToastParam = "";
                string strTileTitle = "";
                int iTileCount = 0;
                string strBackgroundImage = "";


                //Android notification
                Dictionary<string, string> oDictAndroid = new Dictionary<string, string>();
                Dictionary<string, object> oDictIOS = new Dictionary<string, object>();
                Dictionary<string, string> oDictAPS = new Dictionary<string, string>();

                oDictIOS["type"] = oDictAndroid["type"] = Convert.ToInt32(NotificationEventType.UserWarning).ToString();
                oDictIOS["text1"] = oDictAndroid["text1"] = usersWarning.UWA_TEXT1;
                oDictIOS["text2"] = oDictAndroid["text2"] = usersWarning.UWA_TEXT2;
                oDictIOS["url_image"] = oDictAndroid["url_image"] = usersWarning.UWA_URL_IMAGE;
                oDictIOS["title"] = oDictAndroid["title"] = usersWarning.UWA_TITLE;
                oDictIOS["button1_text"] = oDictAndroid["button1_text"] = usersWarning.UWA_BUTTON1_TEXT;
                oDictIOS["button1_function"] = oDictAndroid["button1_function"] = usersWarning.UWA_BUTTON1_FUNCTION;
                oDictIOS["button2_text"] = oDictAndroid["button2_text"] = usersWarning.UWA_BUTTON2_TEXT;
                oDictIOS["message"] = oDictAndroid["message"] = usersWarning.UWA_TITLE;
                oDictAPS["alert"] = oDictIOS["message"].ToString();
                oDictIOS["aps"] = oDictAPS;



                string strAndroidRawData = string.Empty;
                string strIOSRawData = string.Empty;
                foreach (var oPushId in oUser.USERS_PUSH_IDs)
                {
                    //m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Entra en el forech de crear la notificacion version:{0} y OSMobile:{1}", oPushId.UPID_APP_VERSION, oPushId.UPID_OS));
                    if (AppUtilities.AppVersion(oPushId.UPID_APP_VERSION) >= _VERSION_2_13 && oPushId.UPID_OS == (int)MobileOS.Android)
                    {
                        //m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Entra al Android. version:{0} y OSMobile:{1}", oPushId.UPID_APP_VERSION, oPushId.UPID_OS));
                        strAndroidRawData = JsonConvert.SerializeObject(oDictAndroid, Newtonsoft.Json.Formatting.None);
                    }

                    if (AppUtilities.AppVersion(oPushId.UPID_APP_VERSION) >= _VERSION_2_13 && oPushId.UPID_OS == (int)MobileOS.iOS)
                    {

                        //m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Entra al iOS. version:{0} y OSMobile:{1}", oPushId.UPID_APP_VERSION, oPushId.UPID_OS));
                        strIOSRawData = JsonConvert.SerializeObject(oDictIOS, Newtonsoft.Json.Formatting.None);
                    }
                }

                if (!string.IsNullOrEmpty(strAndroidRawData) || !string.IsNullOrEmpty(strIOSRawData))
                {
                    //m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Entra para resolver Agregar", oPushId.UPID_APP_VERSION, oPushId.UPID_OS));
                    bRes = customersRepository.AddPushIDNotification(ref oUser, strToastText1, strToastText2, strToastParam, strTileTitle, iTileCount, strBackgroundImage,
                                                                           strAndroidRawData, strIOSRawData, ref dNotifId);
                }

                if (string.IsNullOrEmpty(strAndroidRawData) && string.IsNullOrEmpty(strIOSRawData))
                {
                    bRes = false;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileNotifier::GenerateInsertionUserNotification: Exception {0}", e.Message));
                bRes = false;
            }

            return bRes;
        }

        private int ValidAppSettingsConvertToInt(string sKey)
        {
            int iAppSetting = 0;
            if (ConfigurationManager.AppSettings[sKey] != null)
            {
                iAppSetting = Convert.ToInt32(ConfigurationManager.AppSettings[sKey].ToString());
            }
            else
            {
                m_Log.LogMessage(LogLevels.logERROR, "It was not found in:: APP.CONFIG >> AppSettings >> " + sKey);
            }
            return iAppSetting;
        }

        /*static void ChannelException(object sender, IPushChannel channel, Exception exception)
        {
            m_Log.LogMessage(LogLevels.logDEBUG, "Channel Exception: " + sender + " -> " + exception);
        }

        static void ServiceException(object sender, Exception exception)
        {
            m_Log.LogMessage(LogLevels.logDEBUG, "Channel Exception: " + sender + " -> " + exception);
        }

        static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
        {
            m_Log.LogMessage(LogLevels.logDEBUG, "Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
            CintegraMobileNotifierTag oTag = (CintegraMobileNotifierTag)notification.Tag;
            if (oTag != null)
            {
                lock (oTag.Repository)
                {
                    oTag.Repository.PushIdExpired(Convert.ToDecimal(oTag.dPushNotifID), "");
                }
            }
            lock (m_NumPushPendingOfFinishingLock)
            {
                m_NumPushPendingOfFinishing--;
            }
        }

        static void ChannelDestroyed(object sender)
        {
            m_Log.LogMessage(LogLevels.logDEBUG, "Channel Destroyed for: " + sender);
        }

        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            m_Log.LogMessage(LogLevels.logDEBUG, "Channel Created for: " + sender);
        }*/
        #endregion

        #endregion
    }
}
