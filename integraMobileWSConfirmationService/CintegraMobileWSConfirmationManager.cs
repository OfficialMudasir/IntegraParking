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
//using integraMobileWSConfirmationService.Properties;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Infrastructure.PermitsAPI;
using integraMobile.ExternalWS;

namespace integraMobileWSConfirmationService
{

    class CintegraMobileWSConfirmationManager
    {
        #region -- Constant definitions --


        const System.String ct_POOLTIME_CONFIRMATIONS_MANAGER_TAG = "PoolingConfirmationsManager";
        const System.String ct_POOLTIME_STREETSECTIONS_MANAGER_TAG = "PoolingStreetSectionsManager";

        const System.String ct_STOPTIME_TAG = "Stoptime";
        const System.String ct_RETRIES_TAG = "Retries";
        const System.String ct_RESENDTIME_TAG = "RetriesTime";
        const System.String ct_MAXRESENDTIME_TAG = "MaxRetriesTime";
        const System.String ct_MAXWORKINGTHREADS_TAG = "MaxWorkingThreads";
        const System.String ct_CONFIRM_WAIT_TIME = "ConfirmWaitTime";
        const System.String ct_QUEUEANALYSISTIME = "QueueAnalysisTime";
        const System.String ct_QUEUEANALYSISTIMEFREQUENCY = "QueueAnalysisTimeFrequency";
        const System.String ct_QUEUETHREADPOOLINGTIME = "QueueThreadPoolingTime";
        const System.String NO_URL_QUEUE_NAME = "NO_URL_QUEUE";


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

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CintegraMobileWSConfirmationManager));

        //Thread Signal: Stop service event
        private static ManualResetEvent m_evStopServer = new ManualResetEvent(false);

        //Thread to managing operation and fine confirmations
        private Thread m_ConfirmationManagerThread;

        //Thread to managing Recalculation of queues capacities
        private Thread m_CapacitiesRecalculationThread;

        //Thread to StreetSections update
        private Thread m_StreetSectionsUpdateThread;

        private static object m_StreetSectionsLock = new object();

        // Time to pooling for Managing Transactions
        private int m_iPoolTimeConfirmationManager;

        //  Pooling time of UpdateStreetSections thread in seconds
        private int m_iPoolTimeUpdateStreetSections;

        // Time to wait thread termination before stop the server
        private int m_iStopTime;

        // Num of retries
        private static int m_iRetries;

        //Time between retries
        private int m_iResendTime;
        private int m_iMaxResendTime;

        private int m_iMaxWorkingThreads;

        private int m_iConfirmWaitTime;
        private int m_iQueueAnalysisTime;
        private int m_iQueueAnalysisTimeFreq;
        private int m_iQueueThreadPoolingTime;


        private static ManualResetEvent m_evPushBrokerEvent = new ManualResetEvent(false);

        private ThirdPartyOperation m_ThirdPartyOperation = null;
        private ThirdPartyFine m_ThirdPartyFine = null;
        private ThirdPartyOffstreet m_ThirdPartyOffstreet = null;
        private ThirdPartyUser m_ThirdPartyUserImport = null;
        private ThirdPartyStreetSection m_ThirdPartyStreetSection = null;
        private Hashtable m_oHashOperations = null;
        private Hashtable m_oHashFines = null;
        private Hashtable m_oHashOffstreetOperations = null;
        private Hashtable m_oHashUserImportOperations = null;
        private Hashtable m_oHashURLConfirmationQueue = null;


        #endregion

		#region -- Constructor / Destructor --


        public class CapacityCalculationInfo
        {
            public string URL;
            public int CalculatedCapacity;
            public int AssignedCapacity;
            public bool Assigned;
        }

        public class CapacityNeededSample
        {
            public DateTime UTCDate;
            public int NeededThreads;
        }

        public class QueueElement
        {
            public ConfirmParameter ConfirmParameter;
            public int SignatureType;
            public string URL;
            public int WS;
            public int Retries;
            public DateTime ConfirmDate;
            public DateTime InsertionDate;
            public ConfirmationURLInfoContainer URLInfoContainer;
            public int QueueLengthOnInsertion;
            
        }


        public class ConfirmationURLInfoContainer
        {
            public string URL;
            public int SignatureType;
            public List<CapacityNeededSample> CapacityNeededSamples = new List<CapacityNeededSample>();
            public List<QueueElement> WaitingElementsQueue = new List<QueueElement>();
            public AutoResetEvent DequeueEvent = new AutoResetEvent(false);
            public int AssignedCapacity = 1;
            public int RunningThreads = 0;
            public Thread Thread = null;
            
        }


        public abstract class ConfirmParameter
        {
             public int RemainingConfirmations=0;
        }

        public class ConfirmOperationParameter: ConfirmParameter
        {
            public OperationConfirmData Operation=null;
           
        }

        public class ConfirmFineParameter:ConfirmParameter
        {
            public TicketPaymentConfirmData Operation = null;
        }

        public class ConfirmOffstreetOperationParameter:ConfirmParameter
        {
            public OperationOffStreetConfirmData Operation = null;
        }

        public class ConfirmUserImportParameter : ConfirmParameter
        {
            public UserImportConfirmData Operation = null;
        }



        public CintegraMobileWSConfirmationManager()
		{
            m_iPoolTimeConfirmationManager = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_CONFIRMATIONS_MANAGER_TAG].ToString());
            try
            {
                m_iPoolTimeUpdateStreetSections = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_STREETSECTIONS_MANAGER_TAG].ToString());
            }
            catch
            {
                m_iPoolTimeUpdateStreetSections = 60;
            }
            m_iStopTime             =  Convert.ToInt32(ConfigurationManager.AppSettings[ct_STOPTIME_TAG].ToString());
            m_iRetries              = Convert.ToInt32(ConfigurationManager.AppSettings[ct_RETRIES_TAG].ToString());
            m_iResendTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_RESENDTIME_TAG].ToString());
            m_iMaxResendTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXRESENDTIME_TAG].ToString());
            m_iMaxWorkingThreads = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXWORKINGTHREADS_TAG].ToString());

            try
            {
                m_iConfirmWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_CONFIRM_WAIT_TIME].ToString());
            }
            catch
            {
                m_iConfirmWaitTime = 10;
            }

            try
            {
                m_iQueueAnalysisTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_QUEUEANALYSISTIME].ToString());

            }
            catch
            {
                m_iQueueAnalysisTime = 180;
            }


            try
            {
                m_iQueueAnalysisTimeFreq = Convert.ToInt32(ConfigurationManager.AppSettings[ct_QUEUEANALYSISTIMEFREQUENCY].ToString());

            }
            catch
            {
                m_iQueueAnalysisTimeFreq = 30;
            }

            try
            {
                m_iQueueThreadPoolingTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_QUEUETHREADPOOLINGTIME].ToString());

            }
            catch
            {
                m_iQueueThreadPoolingTime = 5000;
            }

            
            

            m_kernel = new StandardKernel(new integraMobileConfirmationModule());
            m_kernel.Inject(this);

            m_ThirdPartyOperation = new ThirdPartyOperation();
            m_ThirdPartyFine = new ThirdPartyFine();
            m_ThirdPartyOffstreet = new ThirdPartyOffstreet ();
            m_ThirdPartyUserImport = new ThirdPartyUser();
            m_ThirdPartyStreetSection = new ThirdPartyStreetSection();

            m_oHashOperations = new Hashtable();
            m_oHashFines = new Hashtable();
            m_oHashOffstreetOperations = new Hashtable();
            m_oHashUserImportOperations = new Hashtable();
            m_oHashURLConfirmationQueue = new Hashtable();

        }
        
        #endregion 

        #region -- Threads Bodies --

        public void Start()
		{
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileWSConfirmationManager::Start");


            m_ConfirmationManagerThread = new Thread(new ThreadStart(this.ConfirmationManagerThread));
            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Start --> Starting ConfirmationManagerThread"));
            m_ConfirmationManagerThread.Start();

            m_CapacitiesRecalculationThread = new Thread(new ThreadStart(this.CapacitiesRecalculationThread));
            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Start --> Starting CapacitiesRecalculationThread"));
            m_CapacitiesRecalculationThread.Start();

            m_StreetSectionsUpdateThread = new Thread(new ThreadStart(this.StreetSectionsUpdateThread));
            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Start --> Starting StreetSectionsUpdateThread"));
            m_StreetSectionsUpdateThread.Start();


            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileWSConfirmationManager::Start");
        }

        public void Stop()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileWSConfirmationManager::Stop");

            m_evStopServer.Set();

            // We have to give time to close all the existing requests
            // Synchronize the finalization of the main thread

            int iTimeAccum = 0;
            while (GetCurrentRunningThreads() > 0)
            {
                Thread.Sleep(100);
                iTimeAccum += 100;
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Stop --> GetCurrentRunningThread={0}", GetCurrentRunningThreads()));
                if (iTimeAccum >= 1000 * m_iStopTime) break;
            }
            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Stop --> Stopping ConfirmationManagerThread"));
            m_ConfirmationManagerThread.Join(1000 * m_iStopTime);
            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Stop --> Stopping CapacitiesRecalculationThread"));
            m_CapacitiesRecalculationThread.Join(1000 * m_iStopTime);

            lock (m_oHashURLConfirmationQueue)
            {
                IDictionaryEnumerator denum = m_oHashURLConfirmationQueue.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Stop --> Stopping QueueManagerThread for URL={0}",  ((ConfirmationURLInfoContainer)dentry.Value).URL));
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Stop --> Joining={0}", ((ConfirmationURLInfoContainer)dentry.Value).URL));

                }
            }


            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::Stop All"));

            m_evStopServer.Reset();
          

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileWSConfirmationManager::Stop");
        }




        protected void ConfirmationManagerThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileWSConfirmationManager::ConfirmationManagerThread");
            List<OperationConfirmData> oOperations = null;
            List<TicketPaymentConfirmData> oTicketPayments = null;
            List<OperationOffStreetConfirmData> oOperationsOffstreet = null;
            List<UserImportConfirmData> oUserImports = null;
            int iQueueLengthOps = 0;
            int iQueueLengthTickets = 0;
            int iQueueLengthOffstreetOps = 0;
            int iQueueLengthUserImportOps = 0;
            List<URLConfirmData> oConfirmDataList = null;

            bool bFinishServer = false;
            bool bEnqueued = true;
            bool bNeedToRest = false;
            bool bTempEnq = false;


            while (bFinishServer == false)
            {
                try
                {
                   
                    bFinishServer = (m_evStopServer.WaitOne(m_iPoolTimeConfirmationManager, false));
                    if (!bFinishServer)
                    {
                        CalculateMaxNumberDatabaseRegistries(out oConfirmDataList);
                        customersRepository.GetWaitingConfirmationUserImport(out oUserImports, out iQueueLengthTickets, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfUserImportstOperations(), m_iConfirmWaitTime, m_iMaxWorkingThreads, ref oConfirmDataList);
                        customersRepository.GetWaitingConfirmationFine(out oTicketPayments, out iQueueLengthTickets, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfRunningFines(), m_iConfirmWaitTime, m_iMaxWorkingThreads, ref oConfirmDataList);
                        customersRepository.GetWaitingConfirmationOffstreetOperation(out oOperationsOffstreet, out iQueueLengthOffstreetOps, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfRunningOffstreetOperations(), m_iConfirmWaitTime, m_iMaxWorkingThreads, ref oConfirmDataList);
                        customersRepository.GetWaitingConfirmationOperation(out oOperations, out iQueueLengthOps, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfRunningOperations(), m_iConfirmWaitTime, m_iMaxWorkingThreads, ref oConfirmDataList);

                        bEnqueued = true;
                        bNeedToRest = (oOperations.Count() == 0 && oTicketPayments.Count() == 0 && oOperationsOffstreet.Count() == 0 && oUserImports.Count() == 0);
                        
                        while (!bFinishServer && !bNeedToRest)
                        {
                            bTempEnq=false;
                            bEnqueued=false;
                            bNeedToRest = true;
                            bFinishServer = (m_evStopServer.WaitOne(0, false));
                            if ((!bFinishServer) && (oOperations != null))
                            {

                                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmationManagerThread::GetWaitingConfirmationOperation --> DatabaseQueue={0}; Returned Registries:{1}", iQueueLengthOps, oOperations.Count));

                                foreach (OperationConfirmData Operation in oOperations)
                                {
                                    ConfirmOperationParameter oOperationParameter = new ConfirmOperationParameter { Operation = Operation};
                                    ConfirmOperation(oOperationParameter, ref bTempEnq);
                                    bEnqueued|=bTempEnq;
                                }

                                oOperations.Clear();
                                oOperations = null;
                            }

                            bFinishServer = (m_evStopServer.WaitOne(0, false));

                            if ((!bFinishServer) && (oTicketPayments != null))
                            {
                                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmationManagerThread::GetWaitingConfirmationFine --> DatabaseQueue={0}; Returned Registries:{1}", iQueueLengthTickets, oTicketPayments.Count));

                                foreach (TicketPaymentConfirmData oTicketPayment in oTicketPayments)
                                {
                                    ConfirmFineParameter oTicketParameter = new ConfirmFineParameter { Operation = oTicketPayment };
                                    ConfirmFine(oTicketParameter, ref bTempEnq);
                                    bEnqueued |= bTempEnq;
                                }

                                oTicketPayments.Clear();
                                oTicketPayments = null;
                            }

                            bFinishServer = (m_evStopServer.WaitOne(0, false));
                            if ((!bFinishServer) && (oOperationsOffstreet != null))
                            {
                                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmationManagerThread::GetWaitingConfirmationOffstreetOperation --> DatabaseQueue={0}; Returned Registries:{1}", iQueueLengthOffstreetOps, oOperationsOffstreet.Count));

                                foreach (OperationOffStreetConfirmData Operation in oOperationsOffstreet)
                                {
                                    ConfirmOffstreetOperationParameter oOperationParameter = new ConfirmOffstreetOperationParameter { Operation = Operation };
                                    ConfirmOffstreetOperation(oOperationParameter, ref bTempEnq);
                                    bEnqueued |= bTempEnq;

                                }


                                oOperationsOffstreet.Clear();
                                oOperationsOffstreet = null;
                            }

                            bFinishServer = (m_evStopServer.WaitOne(0, false));
                            if ((!bFinishServer) && (oUserImports != null))
                            {
                                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmationManagerThread::GetWaitingUserImportOperation --> DatabaseQueue={0}; Returned Registries:{1}", iQueueLengthOffstreetOps, oUserImports.Count));

                                foreach (UserImportConfirmData Operation in oUserImports)
                                {
                                    ConfirmUserImportParameter oOperationParameter = new ConfirmUserImportParameter { Operation = Operation };
                                    ConfirmUserImport(oOperationParameter, ref bTempEnq);
                                    bEnqueued |= bTempEnq;

                                }


                                oUserImports.Clear();
                                oUserImports = null;
                            }



                            if ((!bFinishServer)&&(bEnqueued))
                            {
                                CalculateMaxNumberDatabaseRegistries(out oConfirmDataList);
                                customersRepository.GetWaitingConfirmationUserImport(out oUserImports, out iQueueLengthTickets, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfUserImportstOperations(), m_iConfirmWaitTime, m_iMaxWorkingThreads, ref oConfirmDataList);
                                customersRepository.GetWaitingConfirmationFine(out oTicketPayments, out iQueueLengthTickets, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfRunningFines(), m_iConfirmWaitTime, m_iMaxWorkingThreads, ref oConfirmDataList);
                                customersRepository.GetWaitingConfirmationOffstreetOperation(out oOperationsOffstreet, out iQueueLengthOffstreetOps, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfRunningOffstreetOperations(), m_iConfirmWaitTime, m_iMaxWorkingThreads,ref oConfirmDataList);
                                customersRepository.GetWaitingConfirmationOperation(out oOperations, out iQueueLengthOps, m_iResendTime, m_iRetries, m_iMaxResendTime, GetListOfRunningOperations(), m_iConfirmWaitTime, m_iMaxWorkingThreads,ref oConfirmDataList);
                                bNeedToRest = (oOperations.Count() == 0 && oTicketPayments.Count() == 0 && oOperationsOffstreet.Count() == 0);
                            }

                        }

                        if (oOperations != null)
                        {
                            oOperations.Clear();
                            oOperations = null;
                        }

                        if (oTicketPayments != null)
                        {
                            oTicketPayments.Clear();
                            oTicketPayments = null;
                        }

                        if (oOperationsOffstreet != null)
                        {
                            oOperationsOffstreet.Clear();
                            oOperationsOffstreet = null;
                        }

                    }
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmationManagerThread: Exception {0}", e.Message));
                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));                              
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileWSConfirmationManager::ConfirmationManagerThread");
        }


        protected void CapacitiesRecalculationThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileWSConfirmationManager::CapacitiesRecalculationThread");

            bool bFinishServer = false;
            while (bFinishServer == false)
            {
                try
                {
                    bFinishServer = (m_evStopServer.WaitOne(m_iQueueAnalysisTimeFreq * 1000, false));
                    if (!bFinishServer)
                    {
                        RecalculateAssignedCapacities();
                    }
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::CapacitiesRecalculationThread: Exception {0}", e.Message));
                }
                                             
                bFinishServer = (m_evStopServer.WaitOne(0, false));
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileWSConfirmationManager::CapacitiesRecalculationThread");
        }

        protected void StreetSectionsUpdateThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileWSConfirmationManager::StreetSectionsUpdateThread");

            int iPoolingTime = 5;
            bool bFinishServer = false;
            while (bFinishServer == false)
            {
                try
                {
                    bFinishServer = (m_evStopServer.WaitOne(iPoolingTime, false));
                    iPoolingTime = m_iPoolTimeUpdateStreetSections * 1000;
                    if (!bFinishServer)
                    {
                        List<INSTALLATION> oInstallations = null;

                        lock (m_StreetSectionsLock)
                        {
                            bFinishServer = (m_evStopServer.WaitOne(0, false));

                            if (bFinishServer)
                                break;

                            if (geograficAndTariffsRepository.GetStreetSectionsUpdateInstallations(out oInstallations))
                            {
                                bFinishServer = (m_evStopServer.WaitOne(0, false));
                                if (!bFinishServer)
                                {
                                    foreach (INSTALLATION oInstallation in oInstallations)
                                    {

                                        StreetSectionsUpdateSignatureType signatureType = (StreetSectionsUpdateSignatureType)oInstallation.INS_STREET_SECTION_UPDATE_WS_SIGNATURE_TYPE;                                        

                                        bool bRes = false;
                                        switch (signatureType)
                                        {
                                            case StreetSectionsUpdateSignatureType.no_call:
                                                bRes = true;
                                                break;
                                            case StreetSectionsUpdateSignatureType.Permits:
                                                {
                                                    List<StreetData> oWSStreetsData = null;
                                                    List<StreetData> oDBStreetsData = null;

                                                    List<StreetSectionData> oWSStreetSectionsData = null;
                                                    Dictionary<int, GridElement> oWSGrid = new Dictionary<int, GridElement>();
                                                    List<StreetSectionData> oDBStreetSectionsData = null;
                                                    Dictionary<int, GridElement> oDBGrid = new Dictionary<int, GridElement>();

                                                    bRes = true;
                                                    bool bChangeAppDatabasePackageVersion = false;
                                                    bool bGridRecreated = false;
                                                    DateTime? dtInstDateTime = geograficAndTariffsRepository.getInstallationDateTime(oInstallation.INS_ID);
                                                    PermitsErrorResponse oErrorResponse = null;
                                                    long lEllapsedTime;

                                                    // Get streets from WS
                                                    if (m_ThirdPartyStreetSection.PermitsGetStreets(oInstallation, out oErrorResponse, out oWSStreetsData, out lEllapsedTime))
                                                    {
                                                        if (oWSStreetsData.Any())
                                                        {
                                                            bFinishServer = (m_evStopServer.WaitOne(0, false));

                                                            if (bFinishServer)
                                                                break;

                                                            List<STREET> oStreets = null;
                                                            if (geograficAndTariffsRepository.GetInstallationStreets(oInstallation.INS_ID, out oStreets))
                                                            {
                                                                oDBStreetsData = oStreets.Select(oStreet => new StreetData()
                                                                {
                                                                    Id = oStreet.STR_ID_EXT,
                                                                    Description = oStreet.STR_DESCRIPTION,
                                                                    Deleted = (oStreet.STR_DELETED == 1)
                                                                }).ToList();

                                                                List<StreetData> oInsertStreetsData = null;
                                                                List<StreetData> oUpdateStreetsData = null;
                                                                List<StreetData> oDeleteStreetsData = null;


                                                                if (CompareStreets(ref oWSStreetsData, ref oDBStreetsData,
                                                                                   out oInsertStreetsData, out oUpdateStreetsData, out oDeleteStreetsData))
                                                                {
                                                                    bFinishServer = (m_evStopServer.WaitOne(0, false));

                                                                    if (bFinishServer)
                                                                        break;

                                                                    if (oInsertStreetsData.Count() > 0 || oUpdateStreetsData.Count() > 0 || oDeleteStreetsData.Count() > 0)
                                                                    {

                                                                        bChangeAppDatabasePackageVersion = true;
                                                                        bRes = geograficAndTariffsRepository.UpdateStreets(oInstallation.INS_ID,
                                                                                                                           ref oInsertStreetsData, ref oUpdateStreetsData, ref oDeleteStreetsData);

                                                                        m_Log.LogMessage(LogLevels.logINFO, string.Format("StreetSectionsUpdateThread -> UpdateStreets {0} bRes={1} bChangeAppDatabasePackageVersion={2}",
                                                                            oInstallation.INS_DESCRIPTION, bRes, bChangeAppDatabasePackageVersion));


                                                                    }
                                                                }


                                                            }
                                                        }
                                                    }

                                                    if (m_ThirdPartyStreetSection.PermitsGetStreetSections(oInstallation, out oErrorResponse, out oWSStreetSectionsData, out oWSGrid, out lEllapsedTime))
                                                    {
                                                        if (oWSStreetSectionsData.Count() > 0)
                                                        {
                                                            bFinishServer = (m_evStopServer.WaitOne(0, false));

                                                            if (bFinishServer)
                                                                break;

                                                            var oInsStreets = oInstallation.STREETs.ToList();
                                                            foreach (var oStreetSection in oWSStreetSectionsData.Where(sts => String.IsNullOrEmpty(sts.Description)))
                                                            {
                                                                oStreetSection.Description = string.Format("{0}, {1}-{2}", oInsStreets.Where(s => s.STR_ID_EXT == oStreetSection.Street).Select(s => s.STR_DESCRIPTION).FirstOrDefault()??oStreetSection.Street,
                                                                                                                           oStreetSection.StreetNumberFrom ?? 0,
                                                                                                                           oStreetSection.StreetNumberTo ?? 0);
                                                            }

                                                            if (this.GetInstallationStreetSectionsFromDB(oInstallation, out oDBStreetSectionsData, out oDBGrid))
                                                            {

                                                                if (!GridsAreEqual(ref oWSGrid, ref oDBGrid))
                                                                {
                                                                    bRes = geograficAndTariffsRepository.RecreateStreetSectionsGrid(oInstallation.INS_ID, ref oWSGrid);
                                                                    bChangeAppDatabasePackageVersion = true;
                                                                    bGridRecreated = true;
                                                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("StreetSectionsUpdateThread -> Grid for {0} Recreated",
                                                                        oInstallation.INS_DESCRIPTION));

                                                                }


                                                                if (bRes)
                                                                {
                                                                    bFinishServer = (m_evStopServer.WaitOne(0, false));

                                                                    if (bFinishServer)
                                                                        break;

                                                                    List<StreetSectionData> oInsertStreetSectionsData = null;
                                                                    List<StreetSectionData> oUpdateStreetSectionsData = null;
                                                                    List<StreetSectionData> oDeleteStreetSectionsData = null;


                                                                    if (CompareStreetSections(bGridRecreated, ref oWSStreetSectionsData, ref oDBStreetSectionsData,
                                                                                               out oInsertStreetSectionsData, out oUpdateStreetSectionsData, out oDeleteStreetSectionsData))
                                                                    {
                                                                        bFinishServer = (m_evStopServer.WaitOne(0, false));

                                                                        if (bFinishServer)
                                                                            break;

                                                                        if (oInsertStreetSectionsData.Count() > 0 || oUpdateStreetSectionsData.Count() > 0 || oDeleteStreetSectionsData.Count() > 0)
                                                                        {

                                                                            bChangeAppDatabasePackageVersion = true;
                                                                            bRes = geograficAndTariffsRepository.UpdateStreetSections(oInstallation.INS_ID, bGridRecreated,
                                                                                                                                       ref oInsertStreetSectionsData, ref oUpdateStreetSectionsData,
                                                                                                                                       ref oDeleteStreetSectionsData);

                                                                            m_Log.LogMessage(LogLevels.logINFO, string.Format("StreetSectionsUpdateThread -> UpdateStreetSections {0} bRes={1} bChangeAppDatabasePackageVersion={2}",
                                                                                oInstallation.INS_DESCRIPTION, bRes, bChangeAppDatabasePackageVersion));


                                                                        }
                                                                    }

                                                                }



                                                                if (bRes && bChangeAppDatabasePackageVersion)
                                                                {
                                                                    Dictionary<decimal, STREET> oStreets = null;
                                                                    List<STREET_SECTION> oStreetSections = null;
                                                                    List<STREET_SECTIONS_GRID> oGrid = null;
                                                                    int iPackageNextVersion = 1;

                                                                    if (geograficAndTariffsRepository.GetPackageFileData(oInstallation.INS_ID, out oStreets, out oStreetSections,
                                                                                                                        out oGrid, out iPackageNextVersion))
                                                                    {
                                                                        byte[] packagecontent = null;
                                                                        bRes = GeneratePackageFile(oInstallation.INS_ID, ref oStreets, ref oStreetSections,
                                                                                                    ref oGrid, iPackageNextVersion, out packagecontent);

                                                                        if (bRes)
                                                                        {
                                                                            bRes = infraestructureRepository.AddStreetSectionPackage(oInstallation.INS_ID, Convert.ToDecimal(iPackageNextVersion), packagecontent);
                                                                            m_Log.LogMessage(LogLevels.logINFO, string.Format("StreetSectionsUpdateThread -> Package new version {0} for {1} created",
                                                                                                                                iPackageNextVersion, oInstallation.INS_DESCRIPTION));
                                                                            if (bRes)
                                                                            {
                                                                                infraestructureRepository.DeleteOlderStreetSectionPackage(oInstallation.INS_ID, Convert.ToDecimal(iPackageNextVersion));
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                                break;
                                            default:
                                                bRes = false;
                                                break;
                                        }


                                        bFinishServer = (m_evStopServer.WaitOne(0, false));

                                        if (bFinishServer)
                                            break;

                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::StreetSectionsUpdateThread: Exception {0}", e.Message));
                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileWSConfirmationManager::StreetSectionsUpdateThread");
        }

        //protected bool StreetsAreEqual(ref List<StreetData> oStreets1, ref List<StreetData> oStreets2)
        //{
        //    bool bRes = false;
        //
        //    bRes = (oStreets1.Count() == oStreets2.Count);
        //
        //    if (bRes)
        //    {
        //        foreach (var oS1 in oStreets1.OrderBy(r => r.Id))
        //        {
        //            var oS2 = oStreets2.Where(s => s.Id == oS1.Id).FirstOrDefault();
        //            bRes = (oS2 != null);
        //            if (!bRes)
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                bRes = oS2.isEqual(oS1);
        //                if (!bRes)
        //                    break;
        //
        //
        //            }
        //        }
        //    }
        //
        //    return bRes;
        //}

        protected bool CompareStreets(ref List<StreetData> oWSStreetsData, ref List<StreetData> oDBStreetsData,
                                      out List<StreetData> oInsertStreetsData, out List<StreetData> oUpdateStreetsData, out List<StreetData> oDeleteStreetsData)
        {
            oInsertStreetsData = new List<StreetData>();
            oUpdateStreetsData = new List<StreetData>();
            oDeleteStreetsData = new List<StreetData>();

            Dictionary<string, StreetData> oDictDBStreetsData = new Dictionary<string, StreetData>();
            Dictionary<string, StreetData> oDictWSStreetsData = new Dictionary<string, StreetData>();


            foreach (StreetData oData in oDBStreetsData)
            {
                oDictDBStreetsData[oData.Id] = oData;
            }

            foreach (StreetData oData in oWSStreetsData)
            {
                oDictWSStreetsData[oData.Id] = oData;
            }


            StreetData oDBData = null;
            foreach (StreetData oWSData in oWSStreetsData)
            {

                if (!oDictDBStreetsData.ContainsKey(oWSData.Id))
                {
                    oInsertStreetsData.Add(oWSData);
                }
                else
                {

                    oDBData = oDictDBStreetsData[oWSData.Id];

                    if (!oWSData.isEqual(oDBData))
                    {
                        oUpdateStreetsData.Add(oWSData);
                    }
                }
            }


            foreach (StreetData oData in oDBStreetsData.Where(r => !r.Deleted))
            {
                if (!oDictWSStreetsData.ContainsKey(oData.Id))
                {
                    oDeleteStreetsData.Add(oData);
                }
            }


            return true;
        }

        protected bool GetInstallationStreetSectionsFromDB(INSTALLATION oInstallation, out List<StreetSectionData> oStreetSectionsData, out Dictionary<int, GridElement> oGrid)
        {
            bool bRes = false;
            oStreetSectionsData = new List<StreetSectionData>();
            oGrid = new Dictionary<int, GridElement>();

            try
            {

                foreach (STREET_SECTIONS_GRID oGridElement in oInstallation.STREET_SECTIONS_GRIDs.OrderBy(r => r.STRSEG_X).OrderBy(r => r.STRSEG_Y))
                {
                    GridElement oDataGridElement = new GridElement()
                    {
                        id = Convert.ToInt32(oGridElement.STRSEG_ID),
                        description = oGridElement.STRSEG_DESCRIPTION,
                        x = oGridElement.STRSEG_X,
                        y = oGridElement.STRSEG_Y,
                        maxX = oGridElement.STRSEG_MAX_X,
                        maxY = oGridElement.STRSEG_MAX_Y,
                        Polygon = new List<MapPoint>(),
                        LstStreetSections = new List<StreetSectionData>(),
                        ReferenceCount = 0,
                    };

                    foreach (STREET_SECTIONS_GRID_GEOMETRY oGeometry in oGridElement.STREET_SECTIONS_GRID_GEOMETRies.OrderBy(r => r.STRSEGG_ORDER).ToList())
                    {
                        oDataGridElement.Polygon.Add(new MapPoint() { x = oGeometry.STRSEGG_LONGITUDE, y = oGeometry.STRSEGG_LATITUDE });
                    }

                    oGrid[oDataGridElement.id] = oDataGridElement;

                }


                foreach (STREET_SECTION oStreetSection in oInstallation.STREET_SECTIONs.OrderBy(r => r.STRSE_ID_EXT))
                {
                    StreetSectionData oData = new StreetSectionData()
                    {
                        Id = oStreetSection.STRSE_ID_EXT,                        
                        Description = oStreetSection.STRSE_DESCRIPTION,
                        Deleted = (oStreetSection.STRSE_DELETED == 1),
                        Zone = oStreetSection.GROUP.GRP_PERMITS_EXT_ID,
                        Street = oStreetSection.STREET.STR_ID_EXT,
                        StreetFrom = oStreetSection.STREET1?.STR_ID_EXT,
                        StreetTo = oStreetSection.STREET2?.STR_ID_EXT,
                        StreetNumberFrom = oStreetSection.STRSE_FROM,
                        StreetNumberTo = oStreetSection.STRSE_TO,
                        Side = oStreetSection.STRSE_SIDE,
                        GeometryCoordinates = new List<MapPoint>(),
                        Colour = oStreetSection.STRSE_COLOUR,
                        oGridElements = new Dictionary<int, GridElement>(),
                        Tariffs = new List<string>()
                    };


                    //foreach (TARIFF_IN_STREETS_SECTIONS_COMPILED oTariff in oStreetSection.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.OrderBy(r => r.TARSTRSEC_TAR_ID))
                    //{
                    //    oData.Tariffs.Add(oTariff.TARIFF.TAR_PERMITS_EXT_ID);
                    //}


                    foreach (STREET_SECTIONS_GEOMETRY oGeometry in oStreetSection.STREET_SECTIONS_GEOMETRies.OrderBy(r => r.STRSEGE_ORDER).ToList())
                    {
                        oData.GeometryCoordinates.Add(new MapPoint() { x = oGeometry.STRSEGE_LONGITUDE, y = oGeometry.STRSEGE_LATITUDE });
                    }


                    foreach (STREET_SECTIONS_STREET_SECTIONS_GRID oGridAssignatton in oStreetSection.STREET_SECTIONS_STREET_SECTIONS_GRIDs.OrderBy(r => r.STRSESSG_STRSEG_ID).ToList())
                    {
                        oData.oGridElements[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)] = oGrid[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)];
                        oGrid[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)].LstStreetSections.Add(oData);
                        oGrid[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)].ReferenceCount++;
                    }

                    oStreetSectionsData.Add(oData);

                }

                bRes = true;

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInstallationStreetSectionsFromDB: ", e);
            }

            return bRes;
        }

        protected bool GridsAreEqual(ref Dictionary<int, GridElement> oGrid1, ref Dictionary<int, GridElement> oGrid2)
        {
            bool bRes = false;

            bRes = (oGrid1.Count() == oGrid2.Count);

            if (bRes)
            {
                foreach (KeyValuePair<int, GridElement> entry in oGrid1.OrderBy(r => r.Key))
                {

                    bRes = oGrid2[entry.Key] != null;
                    if (!bRes)
                    {
                        break;
                    }
                    else
                    {
                        bRes = oGrid2[entry.Key].IsEqual(entry.Value);
                        if (!bRes)
                            break;


                    }

                }

            }

            return bRes;
        }


        protected bool CompareStreetSections(bool bGridRecreated, ref List<StreetSectionData> oWSStreetSectionsData, ref List<StreetSectionData> oDBStreetSectionsData,
                                         out List<StreetSectionData> oInsertStreetSectionsData, out List<StreetSectionData> oUpdateStreetSectionsData, out List<StreetSectionData> oDeleteStreetSectionsData)
        {
            oInsertStreetSectionsData = new List<StreetSectionData>();
            oUpdateStreetSectionsData = new List<StreetSectionData>();
            oDeleteStreetSectionsData = new List<StreetSectionData>();

            Dictionary<string, StreetSectionData> oDictDBStreetSectionsData = new Dictionary<string, StreetSectionData>();
            Dictionary<string, StreetSectionData> oDictWSStreetSectionsData = new Dictionary<string, StreetSectionData>();


            foreach (StreetSectionData oData in oDBStreetSectionsData)
            {
                oDictDBStreetSectionsData[oData.Id] = oData;
            }

            foreach (StreetSectionData oData in oWSStreetSectionsData)
            {
                oDictWSStreetSectionsData[oData.Id] = oData;
            }


            StreetSectionData oDBData = null;
            foreach (StreetSectionData oWSData in oWSStreetSectionsData)
            {

                if (!oDictDBStreetSectionsData.ContainsKey(oWSData.Id))
                {
                    oInsertStreetSectionsData.Add(oWSData);
                }
                else
                {

                    oDBData = oDictDBStreetSectionsData[oWSData.Id];

                    if (bGridRecreated || !oWSData.isEqual(oDBData))
                    {
                        oUpdateStreetSectionsData.Add(oWSData);
                    }
                }
            }


            foreach (StreetSectionData oData in oDBStreetSectionsData.Where(r => !r.Deleted))
            {
                if (!oDictWSStreetSectionsData.ContainsKey(oData.Id))
                {
                    oDeleteStreetSectionsData.Add(oData);
                }
            }


            return true;
        }


        protected bool GeneratePackageFile(decimal dInstallationID, ref Dictionary<decimal, STREET> oStreets, ref List<STREET_SECTION> oStreetSections,
                                    ref List<STREET_SECTIONS_GRID> oGrid, int iPackageNextVersion,
                                    out byte[] packagecontent)
        {

            bool bRes = false;
            packagecontent = null;

            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"streets.txt"))
                {
                    foreach (KeyValuePair<decimal, STREET> entry in oStreets.OrderBy(r => r.Value.STR_DESCRIPTION))
                    {
                        string line = string.Format("{0};{1};{2}", entry.Value.STR_ID, entry.Value.STR_ID_EXT, entry.Value.STR_DESCRIPTION);
                        file.WriteLine(line);
                    }
                }


                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"grid.txt"))
                {
                    foreach (STREET_SECTIONS_GRID oSSGrid in oGrid.OrderBy(r => r.STRSEG_ID))
                    {
                        string line = string.Format("{0};{1};{2};{3};{4};{5}", oSSGrid.STRSEG_ID, oSSGrid.STRSEG_DESCRIPTION, oSSGrid.STRSEG_X, oSSGrid.STRSEG_Y, oSSGrid.STRSEG_MAX_X, oSSGrid.STRSEG_MAX_Y);
                        file.WriteLine(line);
                    }
                }

                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"gridgeometry.txt"))
                {

                    foreach (STREET_SECTIONS_GRID oSSGrid in oGrid.OrderBy(r => r.STRSEG_ID))
                    {
                        foreach (STREET_SECTIONS_GRID_GEOMETRY oPoint in oSSGrid.STREET_SECTIONS_GRID_GEOMETRies.OrderBy(r => r.STRSEGG_ORDER))
                        {
                            string line = string.Format("{0};{1};{2};{3}", oSSGrid.STRSEG_ID, oPoint.STRSEGG_ORDER, oPoint.STRSEGG_LATITUDE.ToString().Replace(",", "."), oPoint.STRSEGG_LONGITUDE.ToString().Replace(",", "."));
                            file.WriteLine(line);
                        }
                    }


                }


                using (System.IO.StreamWriter file =
                                                new System.IO.StreamWriter(@"streetssectionstariff.txt"))
                {
                    foreach (STREET_SECTION oStreetSection in oStreetSections.OrderBy(r => r.STRSE_ID))
                    {

                        foreach (TARIFF_IN_STREETS_SECTIONS_COMPILED oTariff in oStreetSection.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.OrderBy(r => r.TARSTRSEC_TAR_ID))
                        {
                            string line = string.Format("{0};{1}", oStreetSection.STRSE_ID, oTariff.TARSTRSEC_TAR_ID);
                            file.WriteLine(line);
                        }
                    }
                }



                /*string line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", oTramo.id, oTramo.idext, oTramo.descripcio, oTramo.idcalle, oTramo.calledesde, oTramo.callehasta, iGroup, oTramo.colour);
                file.WriteLine(line);*/


                /*
                * 
                    GridId    |    StreetSectionId    |    Color    |    PolygonNumber    |    Lat    |    Lon    | GrpID
                    205;1;#00CD66 ;1;43.264263342892;-2.928883022117
                * 
                */


                using (System.IO.StreamWriter file =
                                    new System.IO.StreamWriter(@"streetssections.txt"))
                {

                    foreach (STREET_SECTION oStreetSection in oStreetSections.OrderBy(r => r.STRSE_ID))
                    {

                        foreach (STREET_SECTIONS_STREET_SECTIONS_GRID oGridAssig in oStreetSection.STREET_SECTIONS_STREET_SECTIONS_GRIDs.OrderBy(r => r.STRSESSG_STRSEG_ID))
                        {

                            foreach (STREET_SECTIONS_GEOMETRY oPoint in oStreetSection.STREET_SECTIONS_GEOMETRies.OrderBy(r => r.STRSEGE_ORDER))
                            {

                                string line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", oGridAssig.STRSESSG_STRSEG_ID, oStreetSection.STRSE_ID, oStreetSection.STRSE_COLOUR, 1, oPoint.STRSEGE_LATITUDE.ToString().Replace(",", "."), oPoint.STRSEGE_LONGITUDE.ToString().Replace(",", "."), oStreetSection.STRSE_GRP_ID, oStreetSection.STRSE_DESCRIPTION);
                                file.WriteLine(line);
                            }

                        }

                    }

                }

                using (System.IO.StreamWriter file =
                                                new System.IO.StreamWriter(@"version.txt"))
                {

                    string line = string.Format("{0}", iPackageNextVersion);
                    file.WriteLine(line);

                }



                bRes = integraMobile.Infrastructure.Compression.Compress(new List<string>() {@"streets.txt",
                                                                                             @"grid.txt",
                                                                                             @"gridgeometry.txt",
                                                                                             @"streetssectionstariff.txt",
                                                                                             @"streetssections.txt",
                                                                                             @"version.txt"},
                                                                                            @"package.zip");


                if (bRes)
                {
                    packagecontent = File.ReadAllBytes(@"package.zip");

                }


            }
            catch
            {
                bRes = false;
            }
            finally
            {
                try
                {
                    File.Delete(@"streets.txt");
                    File.Delete(@"grid.txt");
                    File.Delete(@"gridgeometry.txt");
                    File.Delete(@"streetssectionstariff.txt");
                    File.Delete(@"streetssections.txt");
                    File.Delete(@"version.txt");
                    File.Delete(@"package.zip");
                }
                catch { }

            }



            return bRes;


        }

        protected void QueueManagerThread(string strURL)
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> CintegraMobileWSConfirmationManager::QueueManagerThread");

            bool bFinishServer = false;
            ConfirmationURLInfoContainer oURLInfoContainer = (ConfirmationURLInfoContainer)m_oHashURLConfirmationQueue[strURL];
            EventWaitHandle[] events = new EventWaitHandle[2];

            events[0] = m_evStopServer;
            events[1] = oURLInfoContainer.DequeueEvent;

            while (bFinishServer == false)
            {
               
                try
                {
                    int iHandleIndex = WaitHandle.WaitAny(events, m_iQueueThreadPoolingTime);

                    switch (iHandleIndex)
                    {
                        case 0: //stop event
                            bFinishServer = true;
                            break;
                        case 1: //dequeue event
                        case WaitHandle.WaitTimeout:
                            {
                                QueueElement oQueueElement;
                                bool bFound;

                                if (ExistPendingConfirmation(ref oURLInfoContainer))
                                {
                                    do
                                    {
                                        oQueueElement = null;
                                        bFound = false;

                                        bFinishServer = (m_evStopServer.WaitOne(0, false));

                                        if (!bFinishServer)
                                        {
                                            if (CanDequeue(ref oURLInfoContainer))
                                            {
                                                bFound = DequeueConfirmation(ref oURLInfoContainer, out oQueueElement);

                                                if ((bFound) && (oQueueElement != null))
                                                {
                                                    if (oQueueElement.ConfirmParameter.GetType() == typeof(ConfirmOperationParameter))
                                                    {
                                                        ThreadPool.QueueUserWorkItem(new WaitCallback(ConfirmOperation), (object)oQueueElement);
                                                    }
                                                    else if (oQueueElement.ConfirmParameter.GetType() == typeof(ConfirmFineParameter))
                                                    {
                                                        ThreadPool.QueueUserWorkItem(new WaitCallback(ConfirmFine), (object)oQueueElement);
                                                    }
                                                    else if (oQueueElement.ConfirmParameter.GetType() == typeof(ConfirmOffstreetOperationParameter))
                                                    {
                                                        ThreadPool.QueueUserWorkItem(new WaitCallback(ConfirmOffstreetOperation), (object)oQueueElement);
                                                    }
                                                    else if (oQueueElement.ConfirmParameter.GetType() == typeof(ConfirmUserImportParameter))
                                                    {
                                                        ThreadPool.QueueUserWorkItem(new WaitCallback(ConfirmUserImport), (object)oQueueElement);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::QueueManagerThread --> Free threads not found for URL={0}", oURLInfoContainer.URL));
                                            }
                                        }                                       
                                    }
                                    while ((!bFinishServer) && (bFound));
                                }
                            }
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::QueueManagerThread: Exception {0}", e.Message));
                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< CintegraMobileWSConfirmationManager::QueueManagerThread");
        }


        private bool CalculateMaxNumberDatabaseRegistries(out List<URLConfirmData> oConfirmDataList)
        {
            bool bRes = true;
            int iTotalRunningThreads = 0;
            oConfirmDataList = new List<URLConfirmData>();
            try
            {

                lock (m_oHashURLConfirmationQueue)
                {
                   
                    IDictionaryEnumerator denum = m_oHashURLConfirmationQueue.GetEnumerator();
                    DictionaryEntry dentry;

                    while (denum.MoveNext())
                    {
                        dentry = (DictionaryEntry)denum.Current;

                        if (((ConfirmationURLInfoContainer)dentry.Value).URL != NO_URL_QUEUE_NAME)
                        {
                            oConfirmDataList.Add(new URLConfirmData
                            {
                                URL = ((ConfirmationURLInfoContainer)dentry.Value).URL,
                                AssignedElements = ((ConfirmationURLInfoContainer)dentry.Value).RunningThreads + ((ConfirmationURLInfoContainer)dentry.Value).WaitingElementsQueue.Count(),
                                MaxElementsToReturn = 0,
                            });
                        }
                        iTotalRunningThreads += ((ConfirmationURLInfoContainer)dentry.Value).RunningThreads;
                    }

                    int iTotalRemainingCapacity = m_iMaxWorkingThreads - iTotalRunningThreads;

                    if (iTotalRemainingCapacity > 0)
                    {

                        for(int i=0; i<oConfirmDataList.Count(); i++)                       
                        {
                            ConfirmationURLInfoContainer oURLInfoContainer = (ConfirmationURLInfoContainer)m_oHashURLConfirmationQueue[oConfirmDataList[i].URL];

                            if (oConfirmDataList[i].AssignedElements < oURLInfoContainer.AssignedCapacity)
                            {
                                oConfirmDataList[i].MaxElementsToReturn = oURLInfoContainer.AssignedCapacity - oConfirmDataList[i].AssignedElements + iTotalRemainingCapacity;
                               
                            }
                            else
                            {
                                oConfirmDataList[i].MaxElementsToReturn = iTotalRemainingCapacity;
                            }
                                                    
                            /*m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::CalculateMaxNumberDatabaseRegistries --> URL={0};  MaxElementsToReturn={1}",
                                oInfo.URL, oInfo.MaxElementsToReturn));*/
                        }

                    }
                    else
                        bRes = false;

                   

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::CalculateMaxNumberDatabaseRegistries: Exception {0}", e.Message));
                bRes = false;
            }


            return bRes;
        }


        private bool EnqueueConfirmation(QueueElement oQueueElement)
        {
            bool bRes = false;
            try
            {

                lock (m_oHashURLConfirmationQueue)
                {
                    ConfirmationURLInfoContainer oURLInfoContainer = null;

                    if (!m_oHashURLConfirmationQueue.ContainsKey(oQueueElement.URL))
                    {
                        oURLInfoContainer = new ConfirmationURLInfoContainer
                        {
                            URL = oQueueElement.URL,
                            SignatureType = oQueueElement.SignatureType,
                            AssignedCapacity = 1,
                            RunningThreads = 0,
                            Thread =   new Thread(() => QueueManagerThread(oQueueElement.URL)),
                        };

                        m_oHashURLConfirmationQueue.Add(oQueueElement.URL, oURLInfoContainer);

                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::EnqueueConfirmation --> Starting QueueManagerThread for URL={0}", oQueueElement.URL));
                        oURLInfoContainer.Thread.Start();                      
                    }
                    else
                    {
                        oURLInfoContainer = (ConfirmationURLInfoContainer)m_oHashURLConfirmationQueue[oQueueElement.URL];
                    }


                    if (oURLInfoContainer != null)
                    {
                        lock (oURLInfoContainer)
                        {
                            if ( oURLInfoContainer.WaitingElementsQueue.Count() < m_iMaxWorkingThreads/2)
                            {
                                oQueueElement.URLInfoContainer = oURLInfoContainer;
                                oQueueElement.QueueLengthOnInsertion = oURLInfoContainer.WaitingElementsQueue.Count + 1;
                                oURLInfoContainer.WaitingElementsQueue.Add(oQueueElement);


                                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::EnqueueConfirmation --> Enqueued Element in Queue for URL={0}; RunningThreads={1}; QueueLength={2}; AssignedCapacity={3}",
                                    oQueueElement.URL, oURLInfoContainer.RunningThreads, oURLInfoContainer.WaitingElementsQueue.Count, oURLInfoContainer.AssignedCapacity));

                                oURLInfoContainer.CapacityNeededSamples.Add(new CapacityNeededSample
                                {
                                    UTCDate = DateTime.UtcNow,
                                    NeededThreads = oURLInfoContainer.RunningThreads + oURLInfoContainer.WaitingElementsQueue.Count,
                                });

                                lock (oQueueElement.ConfirmParameter)
                                {
                                    oQueueElement.ConfirmParameter.RemainingConfirmations++;
                                }

                                oURLInfoContainer.DequeueEvent.Set();
                                bRes = true;
                            }
                        }
                        
                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::EnqueueConfirmation: Exception {0}", e.Message));
                bRes = false;
            }


            return bRes;
        }



        private bool DequeueConfirmation(ref ConfirmationURLInfoContainer oURLInfoContainer, out QueueElement oQueueElement)
        {
            bool bRes = false;
            oQueueElement = null;
            try
            {
                if (oURLInfoContainer != null)
                {
                    lock (oURLInfoContainer)
                    {
                        if (oURLInfoContainer.WaitingElementsQueue.Count > 0)
                        {
                            DateTime dtMinDate = oURLInfoContainer.WaitingElementsQueue.Min(r => r.InsertionDate);
                            oQueueElement = oURLInfoContainer.WaitingElementsQueue.First(r => r.InsertionDate == dtMinDate);
                            oURLInfoContainer.WaitingElementsQueue.Remove(oQueueElement);
                            oURLInfoContainer.RunningThreads++;
                            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DequeueConfirmation --> Dequeuing Element in Queue for URL={0}; RunningThreads={1}; QueueLength={2}; AssignedCapacity={3}",
                                oQueueElement.URL, oURLInfoContainer.RunningThreads, oURLInfoContainer.WaitingElementsQueue.Count, oURLInfoContainer.AssignedCapacity));

                            bRes = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::DequeueConfirmation: Exception {0}", e.Message));
                bRes = false;
            }


            return bRes;
        }


        private bool ExistPendingConfirmation(ref ConfirmationURLInfoContainer oURLInfoContainer)
        {
            bool bRes = false;
            try
            {
                if (oURLInfoContainer != null)
                {
                    lock (oURLInfoContainer)
                    {
                        bRes = oURLInfoContainer.WaitingElementsQueue.Count > 0;
                    }
                }
               
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ExistPendingConfirmation: Exception {0}", e.Message));
                bRes = false;
            }


            return bRes;
        }


        private bool CanDequeue(ref ConfirmationURLInfoContainer oURLInfoContainer)
        {
            bool bRes = false;
            try
            {
                if (oURLInfoContainer != null)
                {
                    lock (oURLInfoContainer)
                    {
                        if (oURLInfoContainer.RunningThreads < oURLInfoContainer.AssignedCapacity)
                        {
                            bRes = true;
                        }
                    }

                    if (!bRes)
                    {
                        if (NotAssignedFreeThreads() > 0)
                        {
                            bRes = true;
                        }

                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::CanDequeue: Exception {0}", e.Message));
                bRes = false;
            }


            return bRes;
        }


        private int NotAssignedFreeThreads()
        {
            int iRes = 0;
            int iRunningThreads=0;
            int iAssignedThreads=0;
            try
            {

                lock (m_oHashURLConfirmationQueue)
                {

                    IDictionaryEnumerator denum = m_oHashURLConfirmationQueue.GetEnumerator();
                    DictionaryEntry dentry;

                    while (denum.MoveNext())
                    {
                        dentry = (DictionaryEntry)denum.Current;
                        iRunningThreads += ((ConfirmationURLInfoContainer)dentry.Value).RunningThreads;
                        iAssignedThreads += ((ConfirmationURLInfoContainer)dentry.Value).AssignedCapacity;
                    }

                }

                if (iAssignedThreads < m_iMaxWorkingThreads)
                {
                    if (iRunningThreads < m_iMaxWorkingThreads)
                    {
                        iRes = m_iMaxWorkingThreads - iRunningThreads;
                    }

                }
                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::NotAssignedFreeThreads: Exception {0}", e.Message));
                iRes = 0;
            }


            return iRes;
        }


        private bool RecalculateAssignedCapacities()
        {
            bool bRes = true;
            try
            {

                lock (m_oHashURLConfirmationQueue)
                {

                    List<CapacityCalculationInfo> oThreadList = new List<CapacityCalculationInfo>();

                    IDictionaryEnumerator denum = m_oHashURLConfirmationQueue.GetEnumerator();
                    DictionaryEntry dentry;

                    while (denum.MoveNext())
                    {
                        dentry = (DictionaryEntry)denum.Current;
                        oThreadList.Add(new CapacityCalculationInfo{URL=((ConfirmationURLInfoContainer)dentry.Value).URL,
                                                                    AssignedCapacity=0,
                                                                    CalculatedCapacity=0,
                                                                    Assigned=false});
                    }


                    foreach (CapacityCalculationInfo oInfo in oThreadList)
                    {
                        ConfirmationURLInfoContainer oURLInfoContainer = (ConfirmationURLInfoContainer)m_oHashURLConfirmationQueue[oInfo.URL];
                        oURLInfoContainer.CapacityNeededSamples.RemoveAll(r => r.UTCDate < DateTime.UtcNow.AddSeconds(-m_iQueueAnalysisTime));

                        double dMedian=0;
                        if (oURLInfoContainer.CapacityNeededSamples.Count > 0)
                        {
                            dMedian = GetMedian(oURLInfoContainer.CapacityNeededSamples.Select(r => Convert.ToDouble(r.NeededThreads)).ToArray());
                        }
                        else
                            dMedian = 1;

                        oInfo.CalculatedCapacity = Convert.ToInt32(Math.Truncate(dMedian + 0.5));

                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::RecalculateAssignedCapacities --> URL={0};  CalculatedCapacity={1}",
                            oInfo.URL, oInfo.CalculatedCapacity));

                    }

                    CapacityCalculationInfo oMinimumCapacityInfo=null;
                    int iRemainingThreads=m_iMaxWorkingThreads;
                    int iThreadsCalculatedQuota;
                    int iMinimumCalculatedCapacity;

                    

                    do
                    {
                        oMinimumCapacityInfo = null;
                        
                        if (oThreadList.Count(r => !r.Assigned) > 0)
                        {

                            iThreadsCalculatedQuota = iRemainingThreads / oThreadList.Count(r => !r.Assigned);

                            iMinimumCalculatedCapacity=oThreadList.Where(r => !r.Assigned).Min(r => r.CalculatedCapacity);

                            oMinimumCapacityInfo = oThreadList.Where(r => !r.Assigned && r.CalculatedCapacity == iMinimumCalculatedCapacity).FirstOrDefault();

                            if (oMinimumCapacityInfo!=null)
                            {

                                if (iMinimumCalculatedCapacity <= iThreadsCalculatedQuota)
                                {
                                    if (iMinimumCalculatedCapacity <= iRemainingThreads)
                                    {
                                        oMinimumCapacityInfo.AssignedCapacity = iMinimumCalculatedCapacity;
                                    }
                                    else
                                    {
                                        oMinimumCapacityInfo.AssignedCapacity = iRemainingThreads;
                                    }
                                }
                                else
                                {

                                    if (iThreadsCalculatedQuota <= iRemainingThreads)
                                    {
                                        oMinimumCapacityInfo.AssignedCapacity = iThreadsCalculatedQuota;
                                    }
                                    else
                                    {
                                        oMinimumCapacityInfo.AssignedCapacity = iRemainingThreads;
                                    }

                                }

                                oMinimumCapacityInfo.Assigned = true;
                                ConfirmationURLInfoContainer oURLInfoContainer = (ConfirmationURLInfoContainer)m_oHashURLConfirmationQueue[oMinimumCapacityInfo.URL];
                                lock (oURLInfoContainer)
                                {
                                    oURLInfoContainer.AssignedCapacity = oMinimumCapacityInfo.AssignedCapacity;
                                }
                                iRemainingThreads -= oMinimumCapacityInfo.AssignedCapacity;

                                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::RecalculateAssignedCapacities --> URL={0};  AssignedCapacity={1}",
                                                    oURLInfoContainer.URL, oURLInfoContainer.AssignedCapacity));




                            }

                        }


                    }
                    while ((oMinimumCapacityInfo != null)&&(iRemainingThreads>0));



                }
              
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::RecalculateAssignedCapacities: Exception {0}", e.Message));
                bRes = false;
            }


            return bRes;
        }


        private void ConfirmOperation(ConfirmOperationParameter oOperationParameter, ref bool bEnqueued)
        {
            OperationConfirmData oOperation = null;
            try
            {
                oOperation = oOperationParameter.Operation;
                bEnqueued = false;

                if (oOperation != null)
                {
                    bool bNeedConfirm = false;
                    bool bConfirmed = false;
                    int iRetries = 0;
                    string strURL = "";
                    DateTime datConfirmDate = new DateTime(1900, 1, 1);
                    DateTime datInsertDate = oOperation.OPE_INSERTION_UTC_DATE ?? new DateTime(1900, 1, 1);
                    int iSignatureType=-1;

                    for (int iWS = 1; iWS <= 3; iWS += 1)
                    {
                        switch (iWS)
                        {
                            case 1:
                                bConfirmed = (oOperation.OPE_CONFIRMED_IN_WS1 == 1);
                                iRetries = oOperation.OPE_CONFIRM_IN_WS1_RETRIES_NUM ?? 0;
                                datConfirmDate = oOperation.OPE_CONFIRM_IN_WS1_DATE ?? new DateTime(1900, 1, 1);
                                strURL = oOperation.INSTALLATION.INS_PARK_CONFIRM_WS_URL;
                                iSignatureType = oOperation.INSTALLATION.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE;

                                break;
                            case 2:
                                bConfirmed = (oOperation.OPE_CONFIRMED_IN_WS2 == 1);
                                iRetries = oOperation.OPE_CONFIRM_IN_WS2_RETRIES_NUM ?? 0;
                                datConfirmDate = oOperation.OPE_CONFIRM_IN_WS2_DATE ?? new DateTime(1900, 1, 1);
                                strURL = oOperation.INSTALLATION.INS_PARK_CONFIRM_WS2_URL;
                                iSignatureType = oOperation.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE??-1;
                                break;
                            case 3:
                                bConfirmed = (oOperation.OPE_CONFIRMED_IN_WS3 == 1);
                                iRetries = oOperation.OPE_CONFIRM_IN_WS3_RETRIES_NUM ?? 0;
                                datConfirmDate = oOperation.OPE_CONFIRM_IN_WS3_DATE ?? new DateTime(1900, 1, 1);
                                strURL = oOperation.INSTALLATION.INS_PARK_CONFIRM_WS3_URL;
                                iSignatureType = oOperation.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE??-1;

                                break;
                        }

                        bNeedConfirm = ((!bConfirmed)&&(iSignatureType!=-1));

                        if (bNeedConfirm)
                        {
                            if (EnqueueConfirmation(new QueueElement
                            {
                                ConfirmParameter = oOperationParameter,
                                URL = strURL,
                                SignatureType = iSignatureType,
                                Retries = iRetries,
                                ConfirmDate = datConfirmDate,
                                InsertionDate = datInsertDate,
                                WS = iWS,
                            }
                            ))
                            {

                                if (!bEnqueued)
                                {
                                    AddOperationToRunningThreads(oOperationParameter);
                                    bEnqueued = true;
                                }
                            }
                        }
                        else
                        {
                            if (!bConfirmed)
                            {
                                if (EnqueueConfirmation(new QueueElement
                                {
                                    ConfirmParameter = oOperationParameter,
                                    URL = NO_URL_QUEUE_NAME,
                                    SignatureType = iSignatureType,
                                    Retries = iRetries,
                                    ConfirmDate = datConfirmDate,
                                    InsertionDate = datInsertDate,
                                    WS = iWS,
                                }
                                ))
                                {

                                    if (!bEnqueued)
                                    {
                                        AddOperationToRunningThreads(oOperationParameter);
                                        bEnqueued = true;
                                    }
                                }
                            }

                        }

                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmParkingOperation: Exception {0}", e.Message));
            }

        }


        private void ConfirmFine(ConfirmFineParameter oTicketParameter, ref bool bEnqueued)
        {
            TicketPaymentConfirmData oTicketPayment = null;

            try
            {
                oTicketPayment = oTicketParameter.Operation;
                bEnqueued = false;

                

                bool bNeedConfirm = false;
                bool bConfirmed = false;
                int iRetries = 0;
                string strURL = "";
                DateTime datConfirmDate = new DateTime(1900, 1, 1);
                DateTime datInsertDate = oTicketPayment.TIPA_INSERTION_UTC_DATE ?? new DateTime(1900, 1, 1);

                int iSignatureType=-1;

                for (int iWS = 1; iWS <= 1; iWS += 1)
                {
                    switch (iWS)
                    {
                        case 1:
                            bConfirmed = (oTicketPayment.TIPA_CONFIRMED_IN_WS == 1);
                            iRetries = oTicketPayment.TIPA_CONFIRM_IN_WS_RETRIES_NUM ?? 0;
                            datConfirmDate = oTicketPayment.TIPA_CONFIRM_IN_WS_DATE ?? new DateTime(1900, 1, 1);
                            strURL = oTicketPayment.INSTALLATION.INS_FINE_WS_URL;
                            iSignatureType = oTicketPayment.INSTALLATION.INS_FINE_WS_SIGNATURE_TYPE;

                            break;
                        case 2:
                            bConfirmed = true;
                            iSignatureType = -1;
                            // ...
                            break;
                        case 3:
                            bConfirmed = true;
                            iSignatureType = -1;
                            // ...
                            break;
                    }

                    bNeedConfirm = ((!bConfirmed) && (iSignatureType != -1));

                    if (bNeedConfirm)
                    {
                        if (EnqueueConfirmation(new QueueElement
                                                {
                                                    ConfirmParameter = oTicketParameter,
                                                    URL = strURL,
                                                    SignatureType = iSignatureType,
                                                    Retries = iRetries,
                                                    ConfirmDate = datConfirmDate,
                                                    InsertionDate = datInsertDate,
                                                    WS = iWS,
                                                }
                                           ))
                        {

                            if (!bEnqueued)
                            {
                                AddFineToRunningThreads(oTicketParameter);
                                bEnqueued = true;
                            }
                        }

                    }
                    else
                    {
                        if (!bConfirmed)
                        {
                            if (EnqueueConfirmation(new QueueElement
                            {
                                ConfirmParameter = oTicketParameter,
                                URL = NO_URL_QUEUE_NAME,
                                SignatureType = iSignatureType,
                                Retries = iRetries,
                                ConfirmDate = datConfirmDate,
                                InsertionDate = datInsertDate,
                                WS = iWS,
                            }
                            ))
                            {

                                if (!bEnqueued)
                                {
                                    AddFineToRunningThreads(oTicketParameter);
                                    bEnqueued = true;
                                }
                            }
                        }                           
                    }
                    
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmFine: Exception {0}", e.Message));
            }
          

        }


        private void ConfirmOffstreetOperation(ConfirmOffstreetOperationParameter oOperationParameter, ref bool bEnqueued)
        {
            OperationOffStreetConfirmData oOperation = null;
            try
            {
                oOperation = oOperationParameter.Operation;
               
                bool bNeedConfirm = false;
                bool bConfirmed = false;
                int iRetries = 0;
                DateTime datConfirmDate = new DateTime(1900, 1, 1);
                DateTime datInsertDate = oOperation.OPEOFF_INSERTION_UTC_DATE ?? new DateTime(1900, 1, 1);
                bEnqueued = false;


                for (int iWS = 1; iWS <= 3; iWS += 1)
                {
                    switch (iWS)
                    {
                        case 1:
                            bConfirmed = (oOperation.OPEOFF_CONFIRMED_IN_WS1 == 1);
                            iRetries = oOperation.OPEOFF_CONFIRM_IN_WS1_RETRIES_NUM ?? 0;
                            datConfirmDate = oOperation.OPEOFF_CONFIRM_IN_WS1_DATE ?? new DateTime(1900, 1, 1);
                            break;
                        case 2:
                            bConfirmed = (oOperation.OPEOFF_CONFIRMED_IN_WS2 == 1);
                            iRetries = oOperation.OPEOFF_CONFIRM_IN_WS2_RETRIES_NUM ?? 0;
                            datConfirmDate = oOperation.OPEOFF_CONFIRM_IN_WS2_DATE ?? new DateTime(1900, 1, 1);
                            break;
                        case 3:
                            bConfirmed = (oOperation.OPEOFF_CONFIRMED_IN_WS3 == 1);
                            iRetries = oOperation.OPEOFF_CONFIRM_IN_WS3_RETRIES_NUM ?? 0;
                            datConfirmDate = oOperation.OPEOFF_CONFIRM_IN_WS3_DATE ?? new DateTime(1900, 1, 1);
                            break;
                    }

                    bNeedConfirm = (!bConfirmed);

                    string str3dPartyOpNum = "";
                    long lEllapsedTime = 0;

                    if (bNeedConfirm)
                    {
                        ResultType rt = ResultType.Result_Error_Generic;

                        if ((OffstreetOperationType)oOperation.OPEOFF_TYPE == OffstreetOperationType.Exit ||
                            (OffstreetOperationType)oOperation.OPEOFF_TYPE == OffstreetOperationType.OverduePayment)
                        {
                            rt = ConfirmExitOffstreetOperation(oOperationParameter, iRetries, datConfirmDate, iWS, out str3dPartyOpNum, out lEllapsedTime, ref bEnqueued);
                        }
                        else if ((OffstreetOperationType)oOperation.OPEOFF_TYPE == OffstreetOperationType.Entry)
                        {
                            if (EnqueueConfirmation(new QueueElement
                            {
                                ConfirmParameter = oOperationParameter,
                                URL = NO_URL_QUEUE_NAME,
                                SignatureType = -1,
                                Retries = iRetries,
                                ConfirmDate = datConfirmDate,
                                InsertionDate = datInsertDate,
                                WS = iWS,
                            }
                            ))
                            {

                                if (!bEnqueued)
                                {
                                    AddOffstreetOperationToRunningThreads(oOperationParameter);
                                    bEnqueued = true;
                                }
                            }      
                        }                        
                    }
                    else
                    {
                        if (!bConfirmed)
                        {
                            if (EnqueueConfirmation(new QueueElement
                            {
                                ConfirmParameter = oOperationParameter,
                                URL = NO_URL_QUEUE_NAME,
                                SignatureType = -1,
                                Retries = iRetries,
                                ConfirmDate = datConfirmDate,
                                InsertionDate = datInsertDate,
                                WS = iWS,
                            }
                            ))
                            {

                                if (!bEnqueued)
                                {
                                    AddOffstreetOperationToRunningThreads(oOperationParameter);
                                    bEnqueued = true;
                                }
                            }
                        }
                    }

                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmOffstreetOperation: Exception {0}", e.Message));
            }
           
        }



        private void ConfirmUserImport(ConfirmUserImportParameter oUserImportParameter, ref bool bEnqueued)
        {
            UserImportConfirmData oUserImport = null;

            try
            {
                oUserImport = oUserImportParameter.Operation;
                bEnqueued = false;



                bool bNeedConfirm = false;
                bool bConfirmed = false;
                int iRetries = 0;
                string strURL = "";
                DateTime datConfirmDate = new DateTime(1900, 1, 1);
                DateTime datInsertDate = oUserImport.USIM_STATUS_DATE ?? new DateTime(1900, 1, 1);

                int iSignatureType = -1;

                for (int iWS = 1; iWS <= 1; iWS += 1)
                {
                    switch (iWS)
                    {
                        case 1:
                            bConfirmed = (oUserImport.USIM_STATUS == UserImportStatus.Deleted);
                            iRetries = oUserImport.USIM_RETRIES_NUM;
                            datConfirmDate = oUserImport.USIM_STATUS_DATE ?? new DateTime(1900, 1, 1);
                            strURL = oUserImport.USIM_UICON.UICON_DROP_USER_WS_URL;
                            iSignatureType = oUserImport.USIM_UICON.UICON_DROP_USER_WS_SIGNATURE_TYPE.Value;

                            break;
                        case 2:
                            bConfirmed = true;
                            iSignatureType = -1;
                            // ...
                            break;
                        case 3:
                            bConfirmed = true;
                            iSignatureType = -1;
                            // ...
                            break;
                    }

                    bNeedConfirm = ((!bConfirmed) && (iSignatureType != -1));

                    if (bNeedConfirm)
                    {
                        if (EnqueueConfirmation(new QueueElement
                        {
                            ConfirmParameter = oUserImportParameter,
                            URL = strURL,
                            SignatureType = iSignatureType,
                            Retries = iRetries,
                            ConfirmDate = datConfirmDate,
                            InsertionDate = datInsertDate,
                            WS = iWS,
                        }
                                           ))
                        {

                            if (!bEnqueued)
                            {
                                AddUserImportToRunningThreads(oUserImportParameter);
                                bEnqueued = true;
                            }
                        }

                    }
                    else
                    {
                        if (!bConfirmed)
                        {
                            if (EnqueueConfirmation(new QueueElement
                            {
                                ConfirmParameter = oUserImportParameter,
                                URL = NO_URL_QUEUE_NAME,
                                SignatureType = iSignatureType,
                                Retries = iRetries,
                                ConfirmDate = datConfirmDate,
                                InsertionDate = datInsertDate,
                                WS = iWS,
                            }
                            ))
                            {

                                if (!bEnqueued)
                                {
                                    AddUserImportToRunningThreads(oUserImportParameter);
                                    bEnqueued = true;
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmUserImport: Exception {0}", e.Message));
            }


        }


        private ResultType ConfirmExitOffstreetOperation(ConfirmOffstreetOperationParameter oOperationParameter, int iRetries, DateTime datConfirmDate,
                                                        int iWS, out string str3dPartyOpNum, out long lEllapsedTime, ref bool bEnqueued)
        {
            SortedList parametersOut = new SortedList();
            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            string strURL = "";

            bEnqueued = false;

            ResultType rt = ResultType.Result_Error_Generic;
            OperationOffStreetConfirmData oOperation = oOperationParameter.Operation;
            DateTime datInsertDate = oOperation.OPEOFF_INSERTION_UTC_DATE ?? new DateTime(1900, 1, 1);

            ConfirmExitOffstreetWSSignatureType signatureType = ConfirmExitOffstreetWSSignatureType.no_call;

            GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetWsConfiguration = null;
            decimal? dGroupId = oOperation.OPEOFF_GRP_ID;
            DateTime? dtGroupDateTime = null;
            if (geograficAndTariffsRepository.getOffStreetConfiguration(dGroupId, null, null, ref oOffstreetWsConfiguration, ref dtGroupDateTime))
            {

                switch (iWS)
                {
                    case 1:
                        signatureType = (ConfirmExitOffstreetWSSignatureType)oOffstreetWsConfiguration.GOWC_EXIT_WS1_SIGNATURE_TYPE;
                        strURL = oOffstreetWsConfiguration.GOWC_EXIT_WS1_URL;
                        break;
                    case 2:
                        signatureType = (ConfirmExitOffstreetWSSignatureType)(oOffstreetWsConfiguration.GOWC_EXIT_WS2_SIGNATURE_TYPE ?? (int)ConfirmExitOffstreetWSSignatureType.no_call);
                        strURL = oOffstreetWsConfiguration.GOWC_EXIT_WS2_URL;
                        break;
                    case 3:
                        signatureType = (ConfirmExitOffstreetWSSignatureType)(oOffstreetWsConfiguration.GOWC_EXIT_WS3_SIGNATURE_TYPE ?? (int)ConfirmExitOffstreetWSSignatureType.no_call);
                        strURL = oOffstreetWsConfiguration.GOWC_EXIT_WS3_URL;
                        break;
                    default:
                        break;
                }

                string strPlate = (oOperation.USER_PLATE != null ? oOperation.USER_PLATE.USRP_PLATE : "");


                switch (signatureType)
                {
                    case ConfirmExitOffstreetWSSignatureType.no_call:
                        rt = ResultType.Result_OK;
                        break;
                    case ConfirmExitOffstreetWSSignatureType.test:
                        break;

                    case ConfirmExitOffstreetWSSignatureType.meypar:
                        {

                            if (EnqueueConfirmation(new QueueElement
                                {
                                    ConfirmParameter = oOperationParameter,
                                    URL = strURL,
                                    SignatureType = (int)signatureType,
                                    Retries = iRetries,
                                    ConfirmDate = datConfirmDate,
                                    InsertionDate = datInsertDate,
                                    WS = iWS,
                                }
                            ))
                            {

                                if (!bEnqueued)
                                {
                                    AddOffstreetOperationToRunningThreads(oOperationParameter);
                                    bEnqueued = true;
                                }
                            }


                        }
                        break;

                    default:
                        rt = ResultType.Result_Error_Generic;
                        break;
                }
            }

            return rt;
        }

        


        private void ConfirmOperation(object input)
        {
            OperationConfirmData oOperation = null;
            QueueElement oQueueElement = null;
            try
            {
                oQueueElement = (QueueElement)input;
                oOperation = ((ConfirmOperationParameter)oQueueElement.ConfirmParameter).Operation;
                if (oOperation != null)
                {
                    
                    
                    ResultType rt = ResultType.Result_Error_Generic;
                    string str3dPartyOpNum = "";
                    string str3dPartyBaseOpNum = "";
                    long lEllapsedTime = 0;

                    if (oQueueElement.SignatureType != -1)
                    {

                        if ((ChargeOperationsType)oOperation.OPE_TYPE == ChargeOperationsType.ParkingOperation ||
                            (ChargeOperationsType)oOperation.OPE_TYPE == ChargeOperationsType.ExtensionOperation)
                        {
                            rt = ConfirmParkingOperation(oOperation, oQueueElement.WS, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                        }
                        else if ((ChargeOperationsType)oOperation.OPE_TYPE == ChargeOperationsType.ParkingRefund)
                        {
                            rt = ConfirmUnParkingOperation(oOperation, oQueueElement.WS, out str3dPartyOpNum, out lEllapsedTime);
                        }
                    }
                    else
                    {
                        rt = ResultType.Result_OK;
                    }

                    lock (((ConfirmOperationParameter)oQueueElement.ConfirmParameter).Operation)
                    {
                        OPERATION oOperationOut = null;
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmOperation: UpdateThirdPartyConfirmedInParkingOperation({0},{1},{2},{3})", oOperation.OPE_ID, oQueueElement.WS, rt, oQueueElement.QueueLengthOnInsertion));
                        if (!customersRepository.UpdateThirdPartyConfirmedInParkingOperation(oOperation.OPE_ID, oQueueElement.WS, (rt == ResultType.Result_OK), str3dPartyOpNum, str3dPartyBaseOpNum, lEllapsedTime, oQueueElement.QueueLengthOnInsertion, out oOperationOut))
                        {
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmOperation: UpdateThirdPartyConfirmedInParkingOperation Error"));
                        }
                    }
                                           
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmParkingOperation: Exception {0}", e.Message));
            }
            finally
            {
                if (oOperation != null)
                {
                    DeleteOperationFromRunningThreads(oQueueElement);
                }
            }

        }

        private ResultType ConfirmParkingOperation(OperationConfirmData oOperation, int iWS, out string str3dPartyOpNum, out string str3dPartyBaseOpNum, out long lEllapsedTime)
        {
            SortedList parametersOut = new SortedList();
            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
            str3dPartyOpNum = "";
            str3dPartyBaseOpNum = "";
            lEllapsedTime = 0;

            ConfirmParkWSSignatureType signatureType = ConfirmParkWSSignatureType.cpst_nocall;

            switch(iWS)
            {
                case 1:
                    signatureType = (ConfirmParkWSSignatureType)oOperation.INSTALLATION.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE;
                    break;
                case 2:
                    signatureType = (ConfirmParkWSSignatureType)(oOperation.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE ?? (int)ConfirmParkWSSignatureType.cpst_nocall);
                    break;
                case 3:
                    signatureType = (ConfirmParkWSSignatureType)(oOperation.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE ?? (int)ConfirmParkWSSignatureType.cpst_nocall);
                    break;
                default:
                    break;

            }

             
            string strPlate = (oOperation.USER_PLATE != null ? oOperation.USER_PLATE.USRP_PLATE : "");

            List<string> oAdditionalPlates = new List<string>();
            if (oOperation.USER_PLATE2 != null) oAdditionalPlates.Add(oOperation.USER_PLATE2.USRP_PLATE);
            if (oOperation.USER_PLATE3 != null) oAdditionalPlates.Add(oOperation.USER_PLATE3.USRP_PLATE);
            if (oOperation.USER_PLATE4 != null) oAdditionalPlates.Add(oOperation.USER_PLATE4.USRP_PLATE);
            if (oOperation.USER_PLATE5 != null) oAdditionalPlates.Add(oOperation.USER_PLATE5.USRP_PLATE);
            if (oOperation.USER_PLATE6 != null) oAdditionalPlates.Add(oOperation.USER_PLATE6.USRP_PLATE);
            if (oOperation.USER_PLATE7 != null) oAdditionalPlates.Add(oOperation.USER_PLATE7.USRP_PLATE);
            if (oOperation.USER_PLATE8 != null) oAdditionalPlates.Add(oOperation.USER_PLATE8.USRP_PLATE);
            if (oOperation.USER_PLATE9 != null) oAdditionalPlates.Add(oOperation.USER_PLATE9.USRP_PLATE);
            if (oOperation.USER_PLATE10 != null) oAdditionalPlates.Add(oOperation.USER_PLATE10.USRP_PLATE);

            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;

            USER oUser = oOperation.USER;
            customersRepository.GetUserDataById(ref oUser, oUser.USR_ID);

            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }

            int? iTariffType = null;
            if (oOperation.OPE_TAR_ID.HasValue && oOperation.OPE_GRP_ID.HasValue)
            {
                TARIFF oTariff = GetTariff(oOperation.OPE_GRP_ID.Value, oOperation.OPE_TAR_ID.Value);
                if (oTariff != null)
                    iTariffType = oTariff.TAR_TYPE;
            }

            customersRepository.GetFinantialParams(oOperation.USER, oOperation.INSTALLATION.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, 
                                                    iPaymentTypeId, iPaymentSubtypeId, (ChargeOperationsType)oOperation.OPE_TYPE, iTariffType, out eTaxMode);
            
            int iWSTimeout = infraestructureRepository.GetRateWSTimeout(oOperation.INSTALLATION.INS_ID);

            // ***
            if (signatureType == ConfirmParkWSSignatureType.cpst_madridplatform && oOperation.OPE_GRP_ID.HasValue)
            {
                GROUP oGroup = null;
                DateTime? dtInstDateTime = null;
                if (geograficAndTariffsRepository.getGroup(oOperation.OPE_GRP_ID.Value, ref oGroup, ref dtInstDateTime))
                {
                    if (m_ThirdPartyOperation.Madrid2AllowedZone(oGroup, iWS))
                        signatureType = ConfirmParkWSSignatureType.cpst_madrid2platform;
                }
            }
            // *** 

            ResultType rt = ResultType.Result_Error_Generic;
            switch (signatureType)
            {
                case ConfirmParkWSSignatureType.cpst_nocall:
                    rt = ResultType.Result_OK;
                    break;
                case ConfirmParkWSSignatureType.cpst_test:
                    break;

                case ConfirmParkWSSignatureType.cpst_eysa:
                    {
                        int iQuantity = oOperation.OPE_AMOUNT;
                        decimal dPercVAT1 = oOperation.OPE_PERC_VAT1 ?? 0;
                        decimal dPercVAT2 = oOperation.OPE_PERC_VAT2 ?? 0;
                        decimal dPercFEE = oOperation.OPE_PERC_FEE ?? 0;
                        int iPercFEETopped =Convert.ToInt32(oOperation.OPE_PERC_FEE_TOPPED ?? 0);
                        int iFixedFEE = Convert.ToInt32(oOperation.OPE_FIXED_FEE ?? 0);
                        int iPartialVAT1;
                        int iPartialPercFEE;
                        int iPartialFixedFEE;
                        int iPartialPercFEEVAT;
                        int iPartialFixedFEEVAT;
                        int iTotalQuantity;
                        decimal dPercBonus = oOperation.OPE_PERC_BONUS ?? 0;
                        int iPartialBonusFEE;
                        int iPartialBonusFEEVAT;

                        iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, dPercBonus,
                                                  out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                  out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

                        int iQT = (iPartialPercFEE - iPartialPercFEEVAT) + (iPartialFixedFEE - iPartialFixedFEEVAT);
                        int iQC = iPartialBonusFEE - iPartialBonusFEEVAT;
                        int iIVA = iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;

                        rt = m_ThirdPartyOperation.EysaConfirmParking(iWS, strPlate, oOperation.OPE_INSERTION_UTC_DATE.Value, oOperation.USER, oOperation.INSTALLATION, oOperation.OPE_GRP_ID, oOperation.OPE_TAR_ID, oOperation.OPE_AMOUNT, oOperation.OPE_TIME, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE,
                                                                      iQT, iQC, iIVA, oOperation.OPE_BONUS_MARCA, oOperation.OPE_BONUS_TYPE, oOperation.OPE_LATITUDE, oOperation.OPE_LONGITUDE,iWSTimeout,
                                                                      ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_internal:
                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                    break;

                case ConfirmParkWSSignatureType.cpst_standard:
                    {
                        int iQuantity = oOperation.OPE_AMOUNT;
                        if ((oOperation.OPE_AMOUNT_WITHOUT_BON ?? oOperation.OPE_AMOUNT) == oOperation.OPE_AMOUNT)
                        {
                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                iQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                            }
                        }
                        else
                        {
                            iQuantity = (oOperation.OPE_AMOUNT_WITHOUT_BON ?? oOperation.OPE_AMOUNT);

                            decimal dPercVAT1 = oOperation.OPE_PERC_VAT1 ?? 0;
                            decimal dPercVAT2 = oOperation.OPE_PERC_VAT2 ?? 0;
                            decimal dPercFEE = oOperation.OPE_PERC_FEE ?? 0;
                            int iPercFEETopped = Convert.ToInt32(oOperation.OPE_PERC_FEE_TOPPED ?? 0);
                            int iFixedFEE = Convert.ToInt32(oOperation.OPE_FIXED_FEE ?? 0);
                            decimal dPercBonus = oOperation.OPE_PERC_BONUS ?? 0;
                            int iPartialVAT1;
                            int iPartialPercFEE;
                            int iPartialFixedFEE;
                            int iPartialPercFEEVAT;
                            int iPartialFixedFEEVAT;                            
                            int iPartialBonusFEE;
                            int iPartialBonusFEEVAT;

                            int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, dPercBonus,
                                                                                  out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                                  out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);
                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                iQuantity += iPartialVAT1;
                            }
                        }

                        integraMobile.Domain.CAMPAING oCampaing = null;
                        int? iQTIVAWihtBon = null;

                        if (oOperation.OPE_CAMP_ID.HasValue)
                        {
                            customersRepository.GetCampaign(oOperation.OPE_CAMP_ID.Value, out oCampaing);
                            switch ((CampaingShema)oCampaing.CAMP_SCHEMA)
                            {
                                case CampaingShema.DiscountCampaignByZoneForDailyFirstParkingWithTimeMoreThanX:
                                    {
                                        iQTIVAWihtBon = iQuantity;
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }

                        rt = m_ThirdPartyOperation.StandardConfirmParking(1, strPlate, oAdditionalPlates, oOperation.OPE_DATE, oUser, oOperation.INSTALLATION, oOperation.OPE_GRP_ID, oOperation.OPE_STRSE_ID, oOperation.OPE_TAR_ID, iQuantity, 
                                                                          oOperation.OPE_TIME, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE,oOperation.OPE_ID, oOperation.OPE_SPACE_STRING ?? string.Empty, 
                                                                          oOperation.OPE_POSTPAY, oOperation.OPE_AUTH_ID,  BonificationLogic(oOperation.OPE_BON_MLT, oOperation.OPE_BON_EXT_MLT),
                                                                          oOperation.OPE_BON_EXT_MLT, iQTIVAWihtBon, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_gtechna:
                    {
                        int iTotalQuantity = oOperation.OPE_AMOUNT;
                        if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                        {
                            iTotalQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                        }
                        rt = m_ThirdPartyOperation.GtechnaConfirmParking(iWS, oOperation.OPE_MOSE_ID, strPlate, oOperation.OPE_DATE, oOperation.INSTALLATION, oOperation.OPE_GRP_ID, oOperation.OPE_TAR_ID, iTotalQuantity, 
                                                                         oOperation.OPE_TIME, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE,oOperation.OPE_ID, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_standardmadrid:
                    {                        
                        // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                        // ...
                        int iTotalQuantity = oOperation.OPE_AMOUNT;

                        if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                        {
                            iTotalQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                        }

                        iTotalQuantity = ApplyPercentageBonExtMlt(oOperation.OPE_BON_MLT, oOperation.OPE_BON_EXT_MLT, iTotalQuantity);

                        integraMobile.Domain.CAMPAING oCampaing = null;
                        int? iQTIVAWihtBon = null;

                        if (oOperation.OPE_CAMP_ID.HasValue)
                        {
                            customersRepository.GetCampaign(oOperation.OPE_CAMP_ID.Value, out oCampaing);
                            switch ((CampaingShema)oCampaing.CAMP_SCHEMA)
                            {
                                case CampaingShema.DiscountCampaignByZoneForDailyFirstParkingWithTimeMoreThanX:
                                    {
                                        iQTIVAWihtBon = iTotalQuantity;
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }

                        rt = m_ThirdPartyOperation.StandardConfirmParking(iWS, strPlate, oAdditionalPlates, oOperation.OPE_DATE, oUser, oOperation.INSTALLATION, oOperation.OPE_GRP_ID, oOperation.OPE_STRSE_ID, oOperation.OPE_TAR_ID, 
                                                                          iTotalQuantity, oOperation.OPE_TIME, oOperation.OPE_INIDATE, 
                                                                          oOperation.OPE_ENDDATE, oOperation.OPE_ID, oOperation.OPE_SPACE_STRING ?? string.Empty, oOperation.OPE_POSTPAY, oOperation.OPE_AUTH_ID, 
                                                                          BonificationLogic(oOperation.OPE_BON_MLT, oOperation.OPE_BON_EXT_MLT), oOperation.OPE_BON_EXT_MLT, iQTIVAWihtBon, iWSTimeout,
                                                                          ref parametersOut, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                        
                        /*if (oOperation.OPE_AUTH_ID.HasValue)
                        {
                            decimal dAuthId = oOperation.OPE_AUTH_ID.Value;

                            int iTotalQuantity = oOperation.OPE_AMOUNT;
                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                iTotalQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                            }

                            rt = m_ThirdPartyOperation.MadridPlatformConfirmParking(iWS, strPlate, oOperation.OPE_DATE, oOperation.OPE_INSERTION_UTC_DATE.Value, oOperation.USER, oOperation.INSTALLATION, oOperation.OPE_GRP_ID, oOperation.OPE_TAR_ID, iTotalQuantity, oOperation.OPE_TIME, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE, oOperation.OPE_ID, dAuthId,
                                                                                 ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        else
                        {
                            rt = ResultType.Result_Error_Invalid_Input_Parameter;
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmParkingOperation: AuthId from operation ({0}) required", oOperation.OPE_ID));
                        }*/
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_madridplatform:
                    {
                        int iTotalQuantity = oOperation.OPE_AMOUNT;

                        if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                        {
                            iTotalQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                        }
                        rt = m_ThirdPartyOperation.MadridPlatformConfirmParking(iWS, strPlate, oOperation.OPE_DATE, oOperation.OPE_INSERTION_UTC_DATE.Value, oUser, oOperation.INSTALLATION, oOperation.OPE_GRP_ID,
                                                                                oOperation.OPE_TAR_ID, ApplyPercentageBonExtMlt(oOperation.OPE_BON_MLT, oOperation.OPE_BON_EXT_MLT, iTotalQuantity), oOperation.OPE_TIME,
                                                                                oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE,
                                                                                oOperation.OPE_ID, oOperation.OPE_AUTH_ID ?? 0, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_madrid2platform:
                    {
                        int iTotalQuantity = oOperation.OPE_AMOUNT;

                        if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                        {
                            iTotalQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                        }
                        rt = m_ThirdPartyOperation.Madrid2PlatformConfirmParking(iWS, strPlate, oOperation.OPE_DATE, oOperation.OPE_INSERTION_UTC_DATE.Value, oUser, oOperation.INSTALLATION, oOperation.OPE_GRP_ID,
                                                                                 oOperation.OPE_TAR_ID, ApplyPercentageBonExtMlt(oOperation.OPE_BON_MLT, oOperation.OPE_BON_EXT_MLT, iTotalQuantity), oOperation.OPE_TIME,
                                                                                 oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE,
                                                                                 oOperation.OPE_ID, oOperation.OPE_AUTH_ID ?? 0, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_SIR:
                    {
                        int iTotalAmount = oOperation.OPE_TOTAL_AMOUNT;
                        if (oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop || oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStopHybrid)
                        {
                            iTotalAmount = Convert.ToInt32((oOperation.CUSPMR_CUSPMR_TOTAL_AMOUNT_CHARGED ?? Convert.ToDecimal(oOperation.OPE_TOTAL_AMOUNT)) - (oOperation.CUSPMR_REFUND_AMOUNT ?? 0));
                        }

                        double dTotalAmount = Convert.ToDouble(Convert.ToDecimal(iTotalAmount) / Convert.ToInt32(Math.Pow(10, Convert.ToDouble(oOperation.CUR_MINOR_UNIT))));

                        SIRResponse<SIRCobroResponse> oSIRResponse = null;
                        rt = m_ThirdPartyOperation.SIRConfirmPayment(iWS, oOperation.INSTALLATION, oOperation.MEPACON_CUENTAMP, oOperation.OPE_DATE, oOperation.CUSPMR_OP_REFERENCE, dTotalAmount, iWSTimeout,
                                                                    out str3dPartyOpNum, out oSIRResponse, out lEllapsedTime);
                    }
                    break;

                default:
                    rt = ResultType.Result_Error_Generic;
                    break;
            }
            return rt;
        }

        private decimal? BonificationLogic(decimal? bonMlt, decimal? bonExtMlt)
        {
            decimal? dBonMlt = null;
            if (bonMlt.HasValue && bonExtMlt.HasValue)
            {
                dBonMlt = bonMlt / bonExtMlt;
                if (dBonMlt == 1)
                {
                    dBonMlt = null; ;
                }
            }
            else if (bonMlt.HasValue && !bonExtMlt.HasValue)
            {
                dBonMlt = bonMlt;
            }
            return dBonMlt;
        }

        private int ApplyPercentageBonExtMlt(decimal? bonMlt, decimal? bonExtMlt, int totalAmount)
        {
            decimal? dBonMltTemp = BonificationLogic(bonMlt, bonExtMlt);
            if (dBonMltTemp.HasValue)
            {
                if (bonExtMlt.HasValue)
                {
                    return Convert.ToInt32(Math.Round((totalAmount * bonExtMlt.Value), MidpointRounding.AwayFromZero));
                }
            }
            return totalAmount;
        }

        private ResultType ConfirmUnParkingOperation(OperationConfirmData oOperation, int iWS, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            SortedList parametersOut = new SortedList();
            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
            str3dPartyOpNum = "";
            lEllapsedTime = 0;

            ConfirmParkWSSignatureType signatureType = ConfirmParkWSSignatureType.cpst_nocall;

            switch (iWS)
            {
                case 1:
                    signatureType = (ConfirmParkWSSignatureType)oOperation.INSTALLATION.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE;
                    break;
                case 2:
                    signatureType = (ConfirmParkWSSignatureType)(oOperation.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE ?? (int)ConfirmParkWSSignatureType.cpst_nocall);
                    break;
                case 3:
                    signatureType = (ConfirmParkWSSignatureType)(oOperation.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE ?? (int)ConfirmParkWSSignatureType.cpst_nocall);
                    break;
                default:
                    break;

            }

            string strPlate = (oOperation.USER_PLATE != null ? oOperation.USER_PLATE.USRP_PLATE : "");

            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;

            USER oUser = oOperation.USER;
            customersRepository.GetUserDataById(ref oUser, oUser.USR_ID);

            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }

            int? iTariffType = null;
            if (oOperation.OPE_TAR_ID.HasValue && oOperation.OPE_GRP_ID.HasValue)
            {
                TARIFF oTariff = GetTariff(oOperation.OPE_GRP_ID.Value, oOperation.OPE_TAR_ID.Value);
                if (oTariff != null)
                    iTariffType = oTariff.TAR_TYPE;
            }

            int iWSTimeout = infraestructureRepository.GetRateWSTimeout(oOperation.INSTALLATION.INS_ID);


            customersRepository.GetFinantialParams(oOperation.USER, oOperation.INSTALLATION.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE,
                                                   iPaymentTypeId, iPaymentSubtypeId, (ChargeOperationsType)oOperation.OPE_TYPE, iTariffType, out eTaxMode);

            ResultType rt = ResultType.Result_Error_Generic;
            switch (signatureType)
            {
                case ConfirmParkWSSignatureType.cpst_nocall:
                    rt = ResultType.Result_OK;
                    break;
                case ConfirmParkWSSignatureType.cpst_test:
                    break;

                case ConfirmParkWSSignatureType.cpst_eysa:
                    {
                        int iTotalQuantity = oOperation.OPE_AMOUNT;
                        if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                        {
                            iTotalQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                        }

                        rt = m_ThirdPartyOperation.EysaConfirmUnParking(iWS, strPlate, oOperation.OPE_DATE, oOperation.USER, oOperation.INSTALLATION, iTotalQuantity, oOperation.OPE_TIME, 
                                                                        oOperation.OPE_GRP_ID, oOperation.OPE_TAR_ID, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE, iWSTimeout,
                                                                        ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_internal:
                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                    break;

                case ConfirmParkWSSignatureType.cpst_standard:
                    {
                        int iTotalQuantity = oOperation.OPE_AMOUNT;
                        if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                        {
                            iTotalQuantity += Convert.ToInt32(oOperation.OPE_PARTIAL_VAT1 ?? 0);
                        }

                        rt = m_ThirdPartyOperation.StandardConfirmUnParking(iWS, strPlate, oOperation.OPE_DATE, oOperation.USER, oOperation.INSTALLATION, iTotalQuantity, oOperation.OPE_TIME, 
                                                                            oOperation.OPE_GRP_ID.Value, oOperation.OPE_STRSE_ID, oOperation.OPE_TAR_ID.Value, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE,
                                                                            oOperation.OPE_ID, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;

                case ConfirmParkWSSignatureType.cpst_gtechna:
                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                    break;

                default:
                    rt = ResultType.Result_Error_Generic;
                    break;
            }
            return rt;
        }

        private void ConfirmFine(object input)
        {
            TicketPaymentConfirmData oTicketPayment = null;
            QueueElement oQueueElement = null;

            try
            {
                oQueueElement = (QueueElement)input;
                oTicketPayment = ((ConfirmFineParameter)oQueueElement.ConfirmParameter).Operation;

                if (oTicketPayment != null)
                {
                    ResultType rt = ResultType.Result_Error_Generic;
                    string str3dPartyOpNum = "";
                    long lEllapsedTime = 0;
                    if (oQueueElement.SignatureType != -1)
                    {
                        rt = ConfirmFinePayment(oTicketPayment, oQueueElement.WS, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    else
                    {
                        rt = ResultType.Result_OK;
                    }


                    lock (((ConfirmFineParameter)oQueueElement.ConfirmParameter).Operation)
                    {
                        TICKET_PAYMENT oTicketPaymentOut = null;
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmFine: UpdateThirdPartyConfirmedInFine({0},{1},{2},{3})", oTicketPayment.TIPA_ID, oQueueElement.WS, rt, oQueueElement.QueueLengthOnInsertion));
                        if (!customersRepository.UpdateThirdPartyConfirmedInFine(oTicketPayment.TIPA_ID, oQueueElement.WS, (rt == ResultType.Result_OK), str3dPartyOpNum, lEllapsedTime, oQueueElement.QueueLengthOnInsertion, out oTicketPaymentOut))
                        {
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmFine: UpdateThirdPartyConfirmedInFine Error"));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmFine: Exception {0}", e.Message));
            }
            finally
            {
                if (oTicketPayment!=null)
                {
                    DeleteFineFromRunningThreads(oQueueElement);
                }
            }

        }

        private ResultType ConfirmFinePayment(TicketPaymentConfirmData oTicketPayment, int iWS, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            SortedList parametersOut = new SortedList();
            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
            str3dPartyOpNum = "";
            lEllapsedTime = 0;

            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;

            USER oUser = oTicketPayment.USER;
            customersRepository.GetUserDataById(ref oUser, oUser.USR_ID);

            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }

            customersRepository.GetFinantialParams(oTicketPayment.USER, oTicketPayment.INSTALLATION.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE,
                                                   iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.TicketPayment, null, out eTaxMode);

            int iTotalQuantity = oTicketPayment.TIPA_AMOUNT;
            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
            {
                iTotalQuantity += Convert.ToInt32(oTicketPayment.TIPA_PARTIAL_VAT1 ?? 0);
            }


            FineWSSignatureType signatureType = FineWSSignatureType.fst_internal;

            switch (iWS)
            {
                case 1:
                    signatureType = (FineWSSignatureType)oTicketPayment.INSTALLATION.INS_FINE_WS_SIGNATURE_TYPE;
                    break;
                case 2:
                    // ...
                    break;
                case 3:
                    // ...
                    break;
                default:
                    break;

            }
            int iWSTimeout = infraestructureRepository.GetRateWSTimeout(oTicketPayment.INSTALLATION.INS_ID);

            ResultType rt = ResultType.Result_Error_Generic;
            switch (signatureType)
            {
                case FineWSSignatureType.fst_gtechna:
                    {
                        rt = m_ThirdPartyFine.GtechnaConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, iTotalQuantity,
                                                                        oTicketPayment.TIPA_ID, oTicketPayment.INSTALLATION, iWSTimeout, ref parametersOut, out str3dPartyOpNum, 
                                                                        out lEllapsedTime);
                    }
                    break;
                case FineWSSignatureType.fst_standard:
                    {
                        rt = m_ThirdPartyFine.StandardConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, iTotalQuantity, oTicketPayment.USER,
                                                                     oTicketPayment.TIPA_ID, oTicketPayment.INSTALLATION, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;
                case FineWSSignatureType.fst_eysa:
                    {
                        rt = m_ThirdPartyFine.EysaConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, iTotalQuantity, oTicketPayment.USER,
                                                                     oTicketPayment.INSTALLATION, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;
                case FineWSSignatureType.fst_madidplatform:
                    {
                        decimal dAuthId = 0;
                        try
                        {
                            if (iWS == 1)
                                dAuthId = Convert.ToDecimal(oTicketPayment.TIPA_EXTERNAL_ID);
                            else if (iWS == 2)
                                dAuthId = 0;
                            else if (iWS == 3)
                                dAuthId = 0;
                        }
                        catch (Exception ex) { }

                        GROUP oGroup = null;
                        DateTime? drGroupDateTime = null;
                        if (oTicketPayment.TIPA_GRP_ID.HasValue)
                        {
                            if (geograficAndTariffsRepository.getGroup(oTicketPayment.TIPA_GRP_ID, ref oGroup, ref drGroupDateTime))
                            {
                            }
                        }

                        rt = m_ThirdPartyFine.MadridPlatformConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, oTicketPayment.TIPA_INSERTION_UTC_DATE.Value, iTotalQuantity, oTicketPayment.USER,
                                                                               oTicketPayment.INSTALLATION, oTicketPayment.TIPA_ID, dAuthId, oGroup, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;
                case FineWSSignatureType.fst_madrid2platform:
                    {
                        decimal dAuthId = 0;
                        try
                        {
                            if (iWS == 1)
                                dAuthId = Convert.ToDecimal(oTicketPayment.TIPA_EXTERNAL_ID);
                            else if (iWS == 2)
                                dAuthId = 0;
                            else if (iWS == 3)
                                dAuthId = 0;
                        }
                        catch (Exception ex) { }

                        GROUP oGroup = null;
                        DateTime? drGroupDateTime = null;
                        if (oTicketPayment.TIPA_GRP_ID.HasValue)
                        {
                            if (geograficAndTariffsRepository.getGroup(oTicketPayment.TIPA_GRP_ID, ref oGroup, ref drGroupDateTime))
                            {
                            }
                        }

                        rt = m_ThirdPartyFine.Madrid2PlatformConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, oTicketPayment.TIPA_INSERTION_UTC_DATE.Value, iTotalQuantity, oTicketPayment.USER,
                                                                               oTicketPayment.INSTALLATION, oTicketPayment.TIPA_ID, dAuthId, oGroup, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;

                case FineWSSignatureType.fst_santboi:
                    rt = m_ThirdPartyFine.SantBoiConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, oTicketPayment.USER,
                                                                    oTicketPayment.INSTALLATION, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    break;
                case FineWSSignatureType.fst_valoriza:
                    {
                        string strIdVSM = "";
                        try
                        {
                            if (iWS == 1)
                                strIdVSM = oTicketPayment.TIPA_EXTERNAL_ID;
                        }
                        catch (Exception ex) { }


                        rt = m_ThirdPartyFine.ValorizaConfirmFinePayment(strIdVSM, oTicketPayment.USER,oTicketPayment.INSTALLATION, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);

                    }
                    break;

                case FineWSSignatureType.fst_mifas:
                    rt = m_ThirdPartyFine.MifasConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.USER, oTicketPayment.INSTALLATION, oTicketPayment.TIPA_DATE,
                                                                  iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    break;
                case FineWSSignatureType.fst_bsm:
                    rt = m_ThirdPartyFine.BSMConfirmFinePayment(oTicketPayment.INSTALLATION, oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, iWSTimeout,
                                                                ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    break;
                case FineWSSignatureType.fst_emisalba:
                    {
                        rt = m_ThirdPartyFine.EmisalbaConfirmFinePayment(oTicketPayment.TIPA_TICKET_NUMBER, oTicketPayment.TIPA_DATE, iTotalQuantity, oTicketPayment.USER,
                                                                        oTicketPayment.INSTALLATION, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    break;
                case FineWSSignatureType.fst_internal:
                    rt = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();
                    break;
                case FineWSSignatureType.fst_test:
                    rt = ResultType.Result_OK;
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();
                    break;


                default:
                    rt = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();
                    break;
            }
            return rt;
        }

        private void ConfirmOffstreetOperation(object input)
        {
            OperationOffStreetConfirmData oOperation = null;
            QueueElement oQueueElement = null;
            SortedList parametersOut = new SortedList();
            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
            string str3dPartyOpNum = "";
            long lEllapsedTime = 0;

            try
            {
                oQueueElement = (QueueElement)input;
                oOperation = ((ConfirmOffstreetOperationParameter)oQueueElement.ConfirmParameter).Operation;
                if (oOperation != null)
                {

                    ResultType rt = ResultType.Result_Error_Generic;
                    string strPlate = (oOperation.USER_PLATE != null ? oOperation.USER_PLATE.USRP_PLATE : "");

                    if (oQueueElement.SignatureType != -1)
                    {

                        GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetWsConfiguration = null;
                        decimal? dGroupId = oOperation.OPEOFF_GRP_ID;
                        DateTime? dtGroupDateTime = null;
                        if (geograficAndTariffsRepository.getOffStreetConfiguration(dGroupId, null, null, ref oOffstreetWsConfiguration, ref dtGroupDateTime))
                        {

                            int iWSTimeout = infraestructureRepository.GetRateWSTimeout(oOffstreetWsConfiguration.GROUP.GRP_INS_ID);

                            switch ((ConfirmExitOffstreetWSSignatureType)oQueueElement.SignatureType)
                            {
                                case ConfirmExitOffstreetWSSignatureType.no_call:
                                    rt = ResultType.Result_OK;
                                    break;
                                case ConfirmExitOffstreetWSSignatureType.test:
                                    rt = ResultType.Result_OK;
                                    break;

                                case ConfirmExitOffstreetWSSignatureType.meypar:
                                    {                                        
                                        rt = m_ThirdPartyOffstreet.MeyparNotifyCarPayment(oQueueElement.WS, oOffstreetWsConfiguration, oOperation.OPEOFF_LOGICAL_ID, OffstreetOperationIdType.MeyparId, strPlate, 
                                                                                          oOperation.OPEOFF_AMOUNT, oOperation.CURRENCy.CUR_ISO_CODE, oOperation.OPEOFF_TIME, oOperation.OPEOFF_ENTRY_DATE, 
                                                                                          oOperation.OPEOFF_END_DATE.Value, oOperation.OPEOFF_GATE, oOperation.OPEOFF_TARIFF, oOperation.OPEOFF_ID,iWSTimeout,
                                                                                          ref oOperation.USER, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                    }
                                    break;

                                default:
                                    rt = ResultType.Result_Error_Generic;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        rt = ResultType.Result_OK;
                    }

                    lock (((ConfirmOffstreetOperationParameter)oQueueElement.ConfirmParameter).Operation)
                    {
                        OPERATIONS_OFFSTREET oOperationOut = null;
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmOffstreetOperation: UpdateThirdPartyConfirmedInOffstreetOperation({0},{1},{2},{3})", oOperation.OPEOFF_ID, oQueueElement.WS, rt, oQueueElement.QueueLengthOnInsertion));
                        if (!customersRepository.UpdateThirdPartyConfirmedInOffstreetOperation(oOperation.OPEOFF_ID, oQueueElement.WS, (rt == ResultType.Result_OK), str3dPartyOpNum, lEllapsedTime, oQueueElement.QueueLengthOnInsertion, out oOperationOut))
                        {
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmOffstreetOperation: UpdateThirdPartyConfirmedInOffstreetOperation Error"));
                        }
                    }

                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmOffstreetOperation: Exception {0}", e.Message));
            }
            finally
            {
                if (oOperation != null)
                {
                    DeleteOffstreetOperationFromRunningThreads(oQueueElement);
                }
            }
        }



        private void ConfirmUserImport(object input)
        {
            UserImportConfirmData oUserImport = null;
            QueueElement oQueueElement = null;

            try
            {
                oQueueElement = (QueueElement)input;
                oUserImport = ((ConfirmUserImportParameter)oQueueElement.ConfirmParameter).Operation;

                if (oUserImport != null)
                {
                    ResultType rt = ResultType.Result_Error_Generic;
                    string str3dPartyOpNum = "";
                    long lEllapsedTime = 0;
                    if (oQueueElement.SignatureType != -1)
                    {
                        rt = ConfirmUserImport(oUserImport, oQueueElement.WS, out str3dPartyOpNum, out lEllapsedTime);
                    }
                    else
                    {
                        rt = ResultType.Result_OK;
                    }


                    lock (((ConfirmUserImportParameter)oQueueElement.ConfirmParameter).Operation)
                    {
                        if (rt == ResultType.Result_OK)
                        {
                            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmUserImport: SetUserImportStatus({0},{1},{2},{3},{4})", oUserImport.USIM_ID, oUserImport.USIM_EMAIL, UserImportStatus.Deleted, rt, oQueueElement.QueueLengthOnInsertion));
                            if (!customersRepository.SetUserImportStatus(oUserImport.USIM_ID,lEllapsedTime,  UserImportStatus.Deleted))
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmImport: SetUserImportStatus Error"));
                            }
                        }
                        else
                        {
                            m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::ConfirmUserImport: SetUserImportIncRetries({0},{1},{2},{3})", oUserImport.USIM_ID, oUserImport.USIM_EMAIL, rt, oQueueElement.QueueLengthOnInsertion));
                            if (!customersRepository.SetUserImportIncRetries(oUserImport.USIM_ID, lEllapsedTime))
                            {
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmImport: SetUserImportIncRetries Error"));
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("CintegraMobileWSConfirmationManager::ConfirmUserImport: Exception {0}", e.Message));
            }
            finally
            {
                if (oUserImport != null)
                {
                    DeleteUserImportFromRunningThreads(oQueueElement);
                }
            }

        }

        private ResultType ConfirmUserImport(UserImportConfirmData oUserImport, int iWS, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            SortedList parametersOut = new SortedList();
            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
            str3dPartyOpNum = "";
            lEllapsedTime = 0;

            UserImportDroUserSignature signatureType = UserImportDroUserSignature.ui_internal;

            switch (iWS)
            {
                case 1:
                    signatureType = (UserImportDroUserSignature)oUserImport.USIM_UICON.UICON_DROP_USER_WS_SIGNATURE_TYPE.Value;
                    break;
                case 2:
                    // ...
                    break;
                case 3:
                    // ...
                    break;
                default:
                    break;

            }

            ResultType rt = ResultType.Result_Error_Generic;
            switch (signatureType)
            {
                case UserImportDroUserSignature.AparcApp:
                    {
                        rt = m_ThirdPartyUserImport.AparcAppDropUser(oUserImport.USIM_UICON.UICON_DROP_USER_WS_URL, oUserImport.USIM_EMAIL, out lEllapsedTime);
                    }
                    break;
                default:
                    rt = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rt).ToString();
                    break;
            }
            return rt;
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

        private TARIFF GetTariff(decimal dGroupId, decimal dTariffId)
        {
            TARIFF oTariff = null;
            GROUP oGroup = null;
            DateTime? dtInstDateTime = null;
            if (geograficAndTariffsRepository.getGroup(dGroupId, ref oGroup, ref dtInstDateTime))
                oTariff = GetTariff(oGroup, dTariffId);

            return oTariff;
        }
        private TARIFF GetTariff(GROUP oGroup, decimal dTariffId)
        {
            TARIFF oTariff = null;

            try
            {
                oTariff = oGroup.TARIFFS_IN_GROUPs.Where(r => r.TARGR_TAR_ID == dTariffId).First().TARIFF;
            }
            catch
            {
                foreach (GROUPS_TYPES_ASSIGNATION oAssigns in oGroup.GROUPS_TYPES_ASSIGNATIONs)
                {
                    try
                    {
                        oTariff = oAssigns.GROUPS_TYPE.TARIFFS_IN_GROUPs.Where(r => r.TARGR_TAR_ID == dTariffId).First().TARIFF;
                        break;
                    }
                    catch { }
                }
            }

            return oTariff;
        }

        int GetCurrentRunningThreads()
        {
            int iRunningThreads=0;

            lock (m_oHashFines)
            {
                lock (m_oHashOperations)
                {
                    lock (m_oHashOffstreetOperations)
                    {
                        lock (m_oHashUserImportOperations)
                        {
                            iRunningThreads = m_oHashFines.Count + m_oHashOperations.Count + m_oHashOffstreetOperations.Count + m_oHashUserImportOperations.Count;
                        }
                    }
                }

            }

            return iRunningThreads;
        }

        void AddOperationToRunningThreads(ConfirmOperationParameter oOperation)
        {
            lock (m_oHashOperations)
            {
                m_oHashOperations.Add(oOperation.Operation.OPE_ID, oOperation);
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::AddOperationToRunningThreads --> RunningThreads={0}; ID={1}", m_oHashOperations.Count, oOperation.Operation.OPE_ID));

            }
        }

        void AddFineToRunningThreads(ConfirmFineParameter oTicketPayment)
        {
            lock (m_oHashFines)
            {
                m_oHashFines.Add(oTicketPayment.Operation.TIPA_ID, oTicketPayment);
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::AddOperationToRunningThreads --> RunningThreads={0}; ID={1}", m_oHashFines.Count, oTicketPayment.Operation.TIPA_ID));
            }
        }

        void AddOffstreetOperationToRunningThreads(ConfirmOffstreetOperationParameter oOffstreetOperation)
        {
            lock (m_oHashOffstreetOperations)
            {
                m_oHashOffstreetOperations.Add(oOffstreetOperation.Operation.OPEOFF_ID, oOffstreetOperation);
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::AddOffstreetOperationToRunningThreads --> RunningThreads={0}; ID={1}", m_oHashOffstreetOperations.Count, oOffstreetOperation.Operation.OPEOFF_ID));

            }
        }


        void AddUserImportToRunningThreads(ConfirmUserImportParameter oUserImport)
        {
            lock (m_oHashUserImportOperations)
            {
                m_oHashUserImportOperations.Add(oUserImport.Operation.USIM_ID, oUserImport);
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::AddUSerImportToRunningThreads --> RunningThreads={0}; ID={1}", m_oHashUserImportOperations.Count, oUserImport.Operation.USIM_ID));

            }
        }

        void DeleteOperationFromRunningThreads(QueueElement oQueueElement)
        {
            lock (oQueueElement.URLInfoContainer)
            {
                oQueueElement.URLInfoContainer.RunningThreads--;
                oQueueElement.URLInfoContainer.DequeueEvent.Set();
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteOperationFromRunningThreads --> URL={0};  RunningThreads={1}",
                    oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads));

            }

            lock ((ConfirmOperationParameter)oQueueElement.ConfirmParameter)            
            {
                ((ConfirmOperationParameter)oQueueElement.ConfirmParameter).RemainingConfirmations--;

                if (((ConfirmOperationParameter)oQueueElement.ConfirmParameter).RemainingConfirmations == 0)
                {
                    lock (m_oHashOperations)
                    {
                        m_oHashOperations.Remove(((ConfirmOperationParameter)oQueueElement.ConfirmParameter).Operation.OPE_ID);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteOperationFromRunningThreads --> URL={0};  RunningThreads={1}; ID={2}",
                            oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads, ((ConfirmOperationParameter)oQueueElement.ConfirmParameter).Operation.OPE_ID));

                    }
                }

            }

        }


        void DeleteFineFromRunningThreads(QueueElement oQueueElement)
        {
            lock (oQueueElement.URLInfoContainer)
            {
                oQueueElement.URLInfoContainer.RunningThreads--;
                oQueueElement.URLInfoContainer.DequeueEvent.Set();
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteFineFromRunningThreads --> URL={0};  RunningThreads={1}",
                    oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads));
            }


            lock ((ConfirmFineParameter)oQueueElement.ConfirmParameter)
            {
                ((ConfirmFineParameter)oQueueElement.ConfirmParameter).RemainingConfirmations--;

                if (((ConfirmFineParameter)oQueueElement.ConfirmParameter).RemainingConfirmations == 0)
                {
                    lock (m_oHashFines)
                    {

                        m_oHashFines.Remove(((ConfirmFineParameter)oQueueElement.ConfirmParameter).Operation.TIPA_ID);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteFineFromRunningThreads --> URL={0};  RunningThreads={1}; ID={2}",
                            oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads, ((ConfirmFineParameter)oQueueElement.ConfirmParameter).Operation.TIPA_ID));

                    }
                }

            }

        }

        void DeleteOffstreetOperationFromRunningThreads(QueueElement oQueueElement)
        {
            lock (oQueueElement.URLInfoContainer)
            {
                oQueueElement.URLInfoContainer.RunningThreads--;
                oQueueElement.URLInfoContainer.DequeueEvent.Set();
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteOffstreetOperationFromRunningThreads --> URL={0};  RunningThreads={1}",
                    oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads));

            }


            lock ((ConfirmOffstreetOperationParameter)oQueueElement.ConfirmParameter)
            {
                ((ConfirmOffstreetOperationParameter)oQueueElement.ConfirmParameter).RemainingConfirmations--;

                if (((ConfirmOffstreetOperationParameter)oQueueElement.ConfirmParameter).RemainingConfirmations == 0)
                {
                    lock (m_oHashOffstreetOperations)
                    {
                        m_oHashOffstreetOperations.Remove(((ConfirmOffstreetOperationParameter)oQueueElement.ConfirmParameter).Operation.OPEOFF_ID);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteOffstreetOperationFromRunningThreads --> URL={0};  RunningThreads={1}; ID={2}",
                            oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads, ((ConfirmOffstreetOperationParameter)oQueueElement.ConfirmParameter).Operation.OPEOFF_ID));

                    }
                }

            }           
        }


        void DeleteUserImportFromRunningThreads(QueueElement oQueueElement)
        {
            lock (oQueueElement.URLInfoContainer)
            {
                oQueueElement.URLInfoContainer.RunningThreads--;
                oQueueElement.URLInfoContainer.DequeueEvent.Set();
                m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteUserImportFromRunningThreads --> URL={0};  RunningThreads={1}",
                    oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads));
            }


            lock ((ConfirmUserImportParameter)oQueueElement.ConfirmParameter)
            {
                ((ConfirmUserImportParameter)oQueueElement.ConfirmParameter).RemainingConfirmations--;

                if (((ConfirmUserImportParameter)oQueueElement.ConfirmParameter).RemainingConfirmations == 0)
                {
                    lock (m_oHashUserImportOperations)
                    {

                        m_oHashUserImportOperations.Remove(((ConfirmUserImportParameter)oQueueElement.ConfirmParameter).Operation.USIM_ID);
                        m_Log.LogMessage(LogLevels.logINFO, string.Format("CintegraMobileWSConfirmationManager::DeleteUserImportFromRunningThreads --> URL={0};  RunningThreads={1}; ID={2}",
                            oQueueElement.URLInfoContainer.URL, oQueueElement.URLInfoContainer.RunningThreads, ((ConfirmUserImportParameter)oQueueElement.ConfirmParameter).Operation.USIM_ID));

                    }
                }

            }

        }

        List<decimal> GetListOfRunningOperations()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashOperations)
            {
                IDictionaryEnumerator denum = m_oHashOperations.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((ConfirmOperationParameter)dentry.Value).Operation.OPE_ID);
                }

            }

            return oList;


        }



        List<decimal> GetListOfRunningFines()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashFines)
            {
                IDictionaryEnumerator denum = m_oHashFines.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((ConfirmFineParameter)dentry.Value).Operation.TIPA_ID);
                }

            }

            return oList;


        }

        List<decimal> GetListOfRunningOffstreetOperations()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashOffstreetOperations)
            {
                IDictionaryEnumerator denum = m_oHashOffstreetOperations.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((ConfirmOffstreetOperationParameter)dentry.Value).Operation.OPEOFF_ID);
                }

            }

            return oList;
        }

        List<decimal> GetListOfUserImportstOperations()
        {
            List<decimal> oList = new List<decimal>();

            lock (m_oHashUserImportOperations)
            {
                IDictionaryEnumerator denum = m_oHashUserImportOperations.GetEnumerator();
                DictionaryEntry dentry;

                while (denum.MoveNext())
                {
                    dentry = (DictionaryEntry)denum.Current;
                    oList.Add(((ConfirmUserImportParameter)dentry.Value).Operation.USIM_ID);
                }

            }

            return oList;
        }


        public static double GetMedian(double[] sourceNumbers)
        {
            //Framework 2.0 version of this method. there is an easier way in F4        
            if (sourceNumbers == null || sourceNumbers.Length == 0)
                throw new System.Exception("Median of empty array not defined.");

            //make sure the list is sorted, but use a new array
            double[] sortedPNumbers = (double[])sourceNumbers.Clone();
            Array.Sort(sortedPNumbers);

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)sortedPNumbers[mid] : ((double)sortedPNumbers[mid] + (double)sortedPNumbers[mid - 1]) / 2;
            return median;
        }

        /*decimal Median(decimal[] xs)
        {
            var ys = xs.OrderBy(x => x).ToList();
            double mid = (ys.Count - 1) / 2.0;
            return (ys[(int)(mid)] + ys[(int)(mid + 0.5)]) / 2;
        }*/

		#endregion

    }
}
