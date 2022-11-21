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
using System.Management;
using System.Diagnostics;
using System.ServiceProcess;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;



namespace integraServiceRestarterService
{

    class integraServiceRestarterManager
    {
        #region -- Constant definitions --


        const System.String ct_POOLTIME_TIME_TAG = "PoolingTime";
        const System.String ct_STOPTIME_TAG = "Stoptime";
        const System.String ct_SERVICELIST_TAG = "ServicesList";
        const System.String ct_SERVICERESTARTTIMEOUT_TAG = "ServiceRestartTimeout";
        const System.String ct_MAXCPU_TAG = "MaxCPU";
        const System.String ct_TIMETOMAINTAINMAXCPU_TAG = "TimeToMaintainMaxCPU";
        const System.String ct_MAXMEM_TAG = "MaxMem";
        const System.String ct_TIMETOMAINTAINMAXMEM_TAG = "TimeToMaintainMaxMem";
        const System.String ct_HEALTHCHECKPOOLING_TAG = "HealthCheckPooling";
        const System.String ct_MAXRETRIESFORHEALTHCHECK_TAG = "MaxRetriesForHealthCheck";
        const System.String ct_MAXRETRIESFORRESTART_TAG = "MaxRetriesForRestart";
        const System.String ct_WSUSERNAMEHEALTHCHECK_TAG = "WSUsernameHealthCheck";
        const System.String ct_WSPASSWORDHEALTHCHECK_TAG = "WSPasswordHealthCheck";
        const System.String ct_WSTIMEOUTHEALTHCHECK_TAG = "WSTimeoutHealthCheck";
        const System.String ct_WSDISCSTRING_TAG = "WSDiscString";
        const System.String ct_TIMETSTARTAGAIN_TAG = "TimeToStartAgain";


        #endregion

        #region -- Member Variables --     
  

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(integraServiceRestarterManager));

        //Thread Signal: Stop service event
        private static ManualResetEvent m_evStopServer = new ManualResetEvent(false);
        private static object m_objLock = new object();
        private static string m_strListOfIps="";

        private Thread m_ResourcesControlThread;
        private Thread m_WSHealthCheckThread;

        // Time to pooling 
        private int m_iPoolTime;
        
        // Time to wait thread termination before stop the server
        private int m_iStopTime;

        // MaxCPU
        private int m_iMaxCPU;
        private int m_iTimeToMaintainMaxCPU;
        private int m_iMaxMem;
        private int m_iTimeToMaintainMaxMem;
        private int m_iRestartServiceTimeout;

        private string m_strServiceList;
        private string[] m_arrServices;


        private int m_iHealthCheckPoolTime;
        private int m_iHealthCheckMaxRetries;
        private int m_iRestartMaxRetries;
        private string m_strWSUsername;
        private string m_strWSPassword;
        private int m_iWSTimeout;
        private string m_strDiscString;
        private int m_iTimeToStartAgainServices;

       
        #endregion

		#region -- Constructor / Destructor --

        public integraServiceRestarterManager()
		{
            m_iPoolTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_POOLTIME_TIME_TAG].ToString());
            m_iStopTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_STOPTIME_TAG].ToString());
            m_iMaxCPU = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXCPU_TAG].ToString());
            m_iTimeToMaintainMaxCPU = Convert.ToInt32(ConfigurationManager.AppSettings[ct_TIMETOMAINTAINMAXCPU_TAG].ToString());
            m_iMaxMem = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXMEM_TAG].ToString());
            m_iTimeToMaintainMaxMem = Convert.ToInt32(ConfigurationManager.AppSettings[ct_TIMETOMAINTAINMAXMEM_TAG].ToString());
            m_strServiceList = ConfigurationManager.AppSettings[ct_SERVICELIST_TAG].ToString();
            m_iRestartServiceTimeout = Convert.ToInt32(ConfigurationManager.AppSettings[ct_SERVICERESTARTTIMEOUT_TAG].ToString());
            m_iHealthCheckPoolTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_HEALTHCHECKPOOLING_TAG].ToString());
            m_iHealthCheckMaxRetries = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXRETRIESFORHEALTHCHECK_TAG].ToString());
            m_iRestartMaxRetries = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXRETRIESFORRESTART_TAG].ToString());
            m_strWSUsername = ConfigurationManager.AppSettings[ct_WSUSERNAMEHEALTHCHECK_TAG].ToString();
            m_strWSPassword = ConfigurationManager.AppSettings[ct_WSPASSWORDHEALTHCHECK_TAG].ToString();
            m_iHealthCheckMaxRetries = Convert.ToInt32(ConfigurationManager.AppSettings[ct_MAXRETRIESFORHEALTHCHECK_TAG].ToString());
            m_iWSTimeout = Convert.ToInt32(ConfigurationManager.AppSettings[ct_WSTIMEOUTHEALTHCHECK_TAG].ToString());
            m_strDiscString = ConfigurationManager.AppSettings[ct_WSDISCSTRING_TAG].ToString();
            m_iTimeToStartAgainServices = Convert.ToInt32(ConfigurationManager.AppSettings[ct_TIMETSTARTAGAIN_TAG].ToString());
            
            char[] cSeparator = new char[1];
            cSeparator[0] = '|';
            m_arrServices = m_strServiceList.Split(cSeparator);


        }


        
        #endregion 

        #region -- Threads Bodies --

        public void Start()
		{
            m_Log.LogMessage(LogLevels.logDEBUG, ">> integraServiceRestarterManager::Start");


            m_ResourcesControlThread = new Thread(new ThreadStart(this.ResourcesControlThread));
            m_ResourcesControlThread.Start();
            m_WSHealthCheckThread = new Thread(new ThreadStart(this.WSHealthCheckThread));
            m_WSHealthCheckThread.Start();
            
            
            m_Log.LogMessage(LogLevels.logDEBUG, "<< integraServiceRestarterManager::Start");
        }

        public void Stop()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> integraServiceRestarterManager::Stop");

            m_evStopServer.Set();

            // We have to give time to close all the existing requests
            // Synchronize the finalization of the main thread
            m_ResourcesControlThread.Join(1000 * m_iStopTime);
            m_WSHealthCheckThread.Join(1000 * m_iStopTime);
            m_evStopServer.Reset();
          
            m_Log.LogMessage(LogLevels.logDEBUG, "<< integraServiceRestarterManager::Stop");
        }




        protected void ResourcesControlThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> integraServiceRestarterManager::ResourcesControlThread");


            int iLastCPU = -1;
            int iLastMem = -1;
            DateTime dtInitMaxCPU = DateTime.UtcNow;
            DateTime dtInitMaxMem = DateTime.UtcNow;

            bool bError1 = false;
            bool bError2 = false;
            DateTime dtLastErrorSent1 = DateTime.UtcNow.AddDays(-1);
            DateTime dtLastErrorSent2 = DateTime.UtcNow.AddDays(-1);

            bool bFinishServer = false;
            while (bFinishServer == false)
            {

                bFinishServer = (m_evStopServer.WaitOne(m_iPoolTime, false));

                if (!bFinishServer)
                {
                    lock (m_objLock)
                    {

                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            int iCurrentCPU = Convert.ToInt32(getCPUUsage());
                            if (iCurrentCPU > m_iMaxCPU)
                            {
                               
                                if (iLastCPU == -1)
                                {
                                    dtInitMaxCPU = DateTime.UtcNow;
                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("CPU = {0}% > {1}%", iCurrentCPU, m_iMaxCPU));
                                }
                                else
                                {
                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("CPU = {0}% > {1}%. Ellapsed Time = {2} s. Max Time {3} s", iCurrentCPU, m_iMaxCPU,
                                        Convert.ToInt32((DateTime.UtcNow - dtInitMaxCPU).TotalSeconds), m_iTimeToMaintainMaxCPU));

                                }
                                iLastCPU = iCurrentCPU;
                                if (Convert.ToInt32((DateTime.UtcNow - dtInitMaxCPU).TotalSeconds) > m_iTimeToMaintainMaxCPU)
                                {
                                    bError1 = true;
                                    if ((DateTime.UtcNow - dtLastErrorSent1).TotalMinutes >= Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Timeout_in_Minutes"]))
                                    {
                                        if (SendEmail(string.Format("Warning: CPU Performance in {0}", GetListOfIPS()), string.Format("CPU Has been higher than {0}% more than {1} seconds", m_iMaxCPU, m_iTimeToMaintainMaxCPU)))
                                            dtLastErrorSent1 = DateTime.UtcNow;
                                    }


                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("CPU Has been higher than {0}% more than {1} seconds. Restarting Configured Services...", m_iMaxCPU, m_iTimeToMaintainMaxCPU));

                                    foreach (string strService in m_arrServices)
                                    {
                                        RestartService(strService.Trim(), m_iRestartServiceTimeout);
                                    }
                                    iLastCPU = -1;
                                    iLastMem = -1;
                                }

                            }
                            else
                            {
                                if (bError1)
                                {
                                    bError1 = false;
                                    if (SendEmail(string.Format("CPU Performance OK in {0}", GetListOfIPS()), ""))
                                        dtLastErrorSent1 = DateTime.UtcNow.AddDays(-1);

                                }
                                iLastCPU = -1;
                            }
                        }
                    }
                }

                bFinishServer = (m_evStopServer.WaitOne(0, false));

                if (!bFinishServer)
                {

                    lock (m_objLock)
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));
                        if (!bFinishServer)
                        {
                            int iCurrentMem = Convert.ToInt32(getMemoryUsage());
                            if (iCurrentMem > m_iMaxMem)
                            {

                                if (iLastMem == -1)
                                {
                                    dtInitMaxMem = DateTime.UtcNow;
                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Mem = {0}% > {1}%", iCurrentMem, m_iMaxMem));
                                }
                                else
                                {
                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Mem = {0}% > {1}%. Ellapsed Time = {2} s. Max Time {3} s", iCurrentMem, m_iMaxMem,
                                        Convert.ToInt32((DateTime.UtcNow - dtInitMaxMem).TotalSeconds), m_iTimeToMaintainMaxMem));

                                }
                                iLastMem = iCurrentMem;
                                if (Convert.ToInt32((DateTime.UtcNow - dtInitMaxMem).TotalSeconds) > m_iTimeToMaintainMaxMem)
                                {
                                    bError2 = true;
                                    if ((DateTime.UtcNow - dtLastErrorSent2).TotalMinutes >= Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Timeout_in_Minutes"]))
                                    {
                                        if (SendEmail(string.Format("Warning: Memory Performance in {0}", GetListOfIPS()), string.Format("Memory Has been higher than {0}% more than {1} seconds", m_iMaxMem, m_iTimeToMaintainMaxMem)))
                                            dtLastErrorSent2 = DateTime.UtcNow;
                                    }
                                    m_Log.LogMessage(LogLevels.logINFO, string.Format("Memory Has been higher than {0}% more than {1} seconds. Restarting Configured Services...", m_iMaxMem, m_iTimeToMaintainMaxMem));

                                    foreach (string strService in m_arrServices)
                                    {
                                        RestartService(strService.Trim(), m_iRestartServiceTimeout);
                                    }
                                    iLastCPU = -1;
                                    iLastMem = -1;
                                }

                            }
                            else
                            {
                                if (bError2)
                                {
                                    bError2 = false;
                                    if (SendEmail(string.Format("Memory Performance OK in {0}", GetListOfIPS()), ""))
                                        dtLastErrorSent2 = DateTime.UtcNow.AddDays(-1);

                                }
                                iLastMem = -1;
                            }

                        }

                    }

                   
                }

            }
            m_Log.LogMessage(LogLevels.logDEBUG, "<< integraServiceRestarterManager::ResourcesControlThread");
        }


        protected void WSHealthCheckThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> integraServiceRestarterManager::WSHealthCheckThread");
            bool bServicesStopped = false;

            int iWSRetries=0;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            integraMobileWS.integraMobileWS oWS = new integraMobileWS.integraMobileWS();
            if (!string.IsNullOrEmpty(m_strWSUsername))
            {
                oWS.Credentials = new System.Net.NetworkCredential(m_strWSUsername, m_strWSPassword);
            }
            oWS.Timeout = m_iWSTimeout;
            bool bError = true;
            DateTime dtLastErrorSent = DateTime.UtcNow.AddDays(-1) ;
            int iRestartRetries = 0;

            int iCurrentWaitTimeout = m_iHealthCheckPoolTime;

            bool bFinishServer = false;
            while (bFinishServer == false)
            {
                bFinishServer = (m_evStopServer.WaitOne(iCurrentWaitTimeout, false));
                if (!bFinishServer)
                {
                    lock (m_objLock)
                    {
                        bFinishServer = (m_evStopServer.WaitOne(0, false));

                        if (!bFinishServer)
                        {

                            if (bServicesStopped)
                            {

                                m_Log.LogMessage(LogLevels.logINFO, string.Format("Starting Configured Services..."));

                                foreach (string strService in m_arrServices)
                                {
                                    StartService(strService.Trim(), m_iRestartServiceTimeout);
                                }

                                SendEmail(string.Format("HealthCheck of {0}. Services Started Again!!!!!", GetListOfIPS()), "");

                                m_Log.LogMessage(LogLevels.logINFO, string.Format("Configured Services Started", m_iHealthCheckMaxRetries));

                                bServicesStopped = false;
                                iCurrentWaitTimeout = m_iHealthCheckPoolTime;
                                iRestartRetries = 0;


                            }
                            else
                            {

                                string strErrorText = "";
                                int iWSRes = -1;

                                try
                                {
                                    iWSRes = oWS.HealthCheckDisc(m_strDiscString);
                                    if (iWSRes <= 0)
                                    {
                                        if (!bError)
                                        {
                                            bError = true;
                                        }
                                        strErrorText = string.Format("Error in WS call: WSRes={0}", iWSRes);
                                        m_Log.LogMessage(LogLevels.logERROR, strErrorText);
                                    }
                                    else
                                    {
                                        iRestartRetries = 0;
                                        //m_Log.LogMessage(LogLevels.logINFO, string.Format("WS call OK: WSRes={0}", iWSRes));
                                        if (bError)
                                        {
                                            dtLastErrorSent = DateTime.UtcNow.AddDays(-1);
                                            bError = false;
                                            SendEmail(string.Format("Health Check OK in {0}", GetListOfIPS()), string.Format("WSRes = {0}", iWSRes));
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    strErrorText = string.Format("Error in WS call: {0}", e.Message);
                                    m_Log.LogMessage(LogLevels.logERROR, strErrorText);
                                    iWSRes = -1;
                                }


                                if (iWSRes <= 0)
                                {
                                    iWSRetries++;

                                    if (iWSRetries > m_iHealthCheckMaxRetries)
                                    {
                                        iRestartRetries++;

                                        if ((DateTime.UtcNow - dtLastErrorSent).TotalMinutes >= Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Timeout_in_Minutes"]))
                                        {
                                            if (SendEmail(string.Format("Error in HealthCheck of {0}", GetListOfIPS()), strErrorText))
                                                dtLastErrorSent = DateTime.UtcNow;
                                        }


                                        if (iRestartRetries < m_iRestartMaxRetries)
                                        {
                                            m_Log.LogMessage(LogLevels.logINFO, string.Format("Return from WS has been Not OK more than {0} times . Restarting Configured Services...", m_iHealthCheckMaxRetries));

                                            foreach (string strService in m_arrServices)
                                            {
                                                RestartService(strService.Trim(), m_iRestartServiceTimeout);
                                            }
                                        }
                                        else
                                        {
                                            m_Log.LogMessage(LogLevels.logINFO, string.Format("Return from WS has been Not OK more than {0} times . Stopping Configured Services...", m_iHealthCheckMaxRetries));

                                            SendEmail(string.Format("Error in HealthCheck of {0}. Stopping Services!!!!!", GetListOfIPS()), strErrorText);

                                            foreach (string strService in m_arrServices)
                                            {
                                                StopService(strService.Trim(), m_iRestartServiceTimeout);
                                            }

                                            bServicesStopped = true;
                                            iCurrentWaitTimeout = m_iTimeToStartAgainServices*1000 ;                                           
                                        }


                                        iWSRetries = 0;
                                    }
                                }
                                else
                                {
                                    iWSRetries = 0;
                                }
                            }
                        }

                    }
                }
            }

            m_Log.LogMessage(LogLevels.logDEBUG, "<< integraServiceRestarterManager::WSHealthCheckThread");
        }



        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, 
                                            System.Security.Cryptography.X509Certificates.X509Chain chain, 
                                            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Gets CPU Usage in %
        /// </summary>
        /// <returns></returns>
        private double getCPUUsage()
        {
            ManagementObject processor = new ManagementObject("Win32_PerfFormattedData_PerfOS_Processor.Name='_Total'");
            processor.Get();

            return double.Parse(processor.Properties["PercentProcessorTime"].Value.ToString());
        }

        /// <summary>
        /// Gets memory usage in %
        /// </summary>
        /// <returns></returns>
        private double getMemoryUsage()
        {
            Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
            Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
            double percentFree = (double)(((decimal)phav / (decimal)tot) * 100);
            return (100 - percentFree);
        }


        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "Restarting Service: "+ serviceName);
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                m_Log.LogMessage(LogLevels.logINFO, "Service Restarted: " + serviceName);
            }
            catch
            {
                m_Log.LogMessage(LogLevels.logERROR, "Error Restarting Service: " + serviceName);
                // ...
            }
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "Starting Service: " + serviceName);
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
              
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                m_Log.LogMessage(LogLevels.logINFO, "Service Started: " + serviceName);
            }
            catch
            {
                m_Log.LogMessage(LogLevels.logERROR, "Error Starting Service: " + serviceName);
                // ...
            }
        }



        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                m_Log.LogMessage(LogLevels.logINFO, "Stopping Service: " + serviceName);
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                m_Log.LogMessage(LogLevels.logINFO, "Service Stopped: " + serviceName);
            }
            catch
            {
                m_Log.LogMessage(LogLevels.logERROR, "Error Stopping Service: " + serviceName);
                // ...
            }
        }


        public bool SendEmail(string strSubject, string strBody)
        {
            bool bRes=true;
            try
            {
                // Command line argument must the the SMTP host.
                SmtpClient client = new SmtpClient();
                bool bEnableSSL = (ConfigurationManager.AppSettings["Mail_SMTPEnableSSL"].ToString().ToUpper() == "TRUE");

                if (bEnableSSL)
                {
                    client.Port = 587;
                }

                client.Host = ConfigurationManager.AppSettings["Mail_SMTPServer"].ToString();
                client.EnableSsl = bEnableSSL;
                client.Timeout = 30000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                if (ConfigurationManager.AppSettings["Mail_SMTPUser"].ToString().Length > 0)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Mail_SMTPUser"].ToString(), 
                                                                          ConfigurationManager.AppSettings["Mail_SMTPPassword"].ToString());
                }
                else
                {
                    client.UseDefaultCredentials = true;
                }

                
                char[] cSeparator = new char[1];
                cSeparator[0] = ';';
                string [] arrRecipients = ConfigurationManager.AppSettings["Mail_Recipients"].ToString().Split(cSeparator);


                foreach (string strRecipient in arrRecipients)
                {
                    MailMessage mm = new MailMessage();
                    mm.From = new MailAddress(ConfigurationManager.AppSettings["Mail_FromAddress"].ToString(),
                                              ConfigurationManager.AppSettings["Mail_FromDisplayName"].ToString());
                    mm.To.Add(new MailAddress(strRecipient));
                    mm.Subject = strSubject;
                    mm.Body = strBody;
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);
                }
            }
            catch (Exception e)
            {
                bRes=false;
                m_Log.LogMessage(LogLevels.logERROR, "Exception : ",e);
                // ...
            }

            return bRes;
        }


        public string GetListOfIPS()
        {
            string strRes = "";
            try
            {

                if (m_strListOfIps.Length > 0)
                {
                    strRes = m_strListOfIps;
                }
                else
                {

                    int nIP = 0;

                    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {

                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    if (nIP == 0)
                                        strRes = ip.Address.ToString();
                                    else
                                        strRes += string.Format(" ({0})", ip.Address.ToString());

                                    nIP++;
                                }
                            }
                        }
                    }

                    m_strListOfIps = strRes;
                }
              
            }
            catch (Exception e)
            {
                strRes = "";
                m_Log.LogMessage(LogLevels.logERROR, "Exception : ",e);
                // ...
            }

            return strRes;
        }


        public static class PerformanceInfo
        {
            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

            [StructLayout(LayoutKind.Sequential)]
            public struct PerformanceInformation
            {
                public int Size;
                public IntPtr CommitTotal;
                public IntPtr CommitLimit;
                public IntPtr CommitPeak;
                public IntPtr PhysicalTotal;
                public IntPtr PhysicalAvailable;
                public IntPtr SystemCache;
                public IntPtr KernelTotal;
                public IntPtr KernelPaged;
                public IntPtr KernelNonPaged;
                public IntPtr PageSize;
                public int HandlesCount;
                public int ProcessCount;
                public int ThreadCount;
            }

            public static Int64 GetPhysicalAvailableMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }

            public static Int64 GetTotalMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }
        }
       
		#endregion



    }
}
