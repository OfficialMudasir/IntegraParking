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
using System.Net;
using System.Net.Sockets;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;



namespace integraOCOServerService
{

    class integraOCOServerManager
    {
        public class StateObject
        {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
            public string strLocalAddress;
        }
        #region -- Constant definitions --


        const System.String ct_STOPTIME_TAG = "Stoptime";
        const System.String ct_LISTENPORT_TAG = "ListenPort";
        const System.String ct_RESULTSFOLDER_TAG = "ResultsFolder";
        const System.String ct_EXPIRATIONTIME_TAG = "ResultFileExpirationTime";

        const int ctSuccessCode = 200;
        const int ctFirstErrorCode = 300;
        const int ctFileTooOldCode = 398;
        const int ctFilesNotExistCode = 399;
        const string ctOCO_VERSION = "oco-1.15";
        const string ctBaseFileName = "Base.txt";


        #endregion

        #region -- Member Variables --     
  

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(integraOCOServerManager));

        //Thread Signal: Stop service event
        private static ManualResetEvent m_evStopServer = new ManualResetEvent(false);
        public static AutoResetEvent m_evConnectionDone = new AutoResetEvent(false);

        private Thread m_ListenThread;
        
        // Time to wait thread termination before stop the server
        static private int m_iStopTime;
        static private int m_iListenPort;
        static private string m_strResultsFolder;
        static private int m_iExpirationTime;
        static private string m_HostName;

       
        #endregion

		#region -- Constructor / Destructor --

        public integraOCOServerManager()
		{
            m_iListenPort = Convert.ToInt32(ConfigurationManager.AppSettings[ct_LISTENPORT_TAG].ToString());
            m_iStopTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_STOPTIME_TAG].ToString());
            m_strResultsFolder = ConfigurationManager.AppSettings[ct_RESULTSFOLDER_TAG].ToString();
            m_iExpirationTime = Convert.ToInt32(ConfigurationManager.AppSettings[ct_EXPIRATIONTIME_TAG].ToString());
            
        }


        
        #endregion 

        #region -- Threads Bodies --

        public void Start()
		{
            m_Log.LogMessage(LogLevels.logDEBUG, ">> integraOCOServerManager::Start");


            m_ListenThread = new Thread(new ThreadStart(this.ListenThread));
            m_ListenThread.Start();
            
            
            m_Log.LogMessage(LogLevels.logDEBUG, "<< integraOCOServerManager::Start");
        }

        public void Stop()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> integraOCOServerManager::Stop");

            m_evStopServer.Set();

            // We have to give time to close all the existing requests
            // Synchronize the finalization of the main thread
            m_ListenThread.Join(1000 * m_iStopTime);
            m_evStopServer.Reset();
          
            m_Log.LogMessage(LogLevels.logDEBUG, "<< integraOCOServerManager::Stop");
        }




        protected void ListenThread()
        {
            m_Log.LogMessage(LogLevels.logDEBUG, ">> integraOCOServerManager::ListenThread");

            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            m_HostName = Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(m_HostName);
            /*IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);*/
            IPEndPoint localEndPoint = new IPEndPoint (IPAddress.Any, m_iListenPort);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp );

            WaitHandle[] handleArray = new WaitHandle[] { m_evStopServer, m_evConnectionDone };

            bool bFinishServer = false;
            while (!bFinishServer)
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (!bFinishServer)
                {
                    try
                    {
                        // Start an asynchronous socket to listen for connections.
                        listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                        bFinishServer = (0 == WaitHandle.WaitAny(handleArray));
                        // Wait until a connection is made before continuing or the stop event is received.
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "integraOCOServerManager::ListenThread - Exception accepting connection!!!", e);
                    }
                }                
            }
            m_Log.LogMessage(LogLevels.logDEBUG, "<< integraOCOServerManager::ListenThread");
        }


        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            m_evConnectionDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            state.strLocalAddress = IPAddress.Parse(((IPEndPoint)state.workSocket.LocalEndPoint).Address.ToString()).ToString(); 
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                    // All the data has been read from the 
                    // client. Display it on the console.
                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("integraOCOServerManager::ReadCallback: Received: {0}", content));

                string response = GenerateResponse(state.strLocalAddress);

                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("integraOCOServerManager::ReadCallback: Response: {0}", response));

                Send(handler, response);
                /*else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }*/
            }
        }


        private static string GenerateResponse(string strLocalAddress)
        {
            string strResponse = "";
            int iErrorCode = ctSuccessCode;
            string strErrorMsg = "";
            int iNumFiles = 0;
            bool bAllOk = true;

            try
            {
                string[] files = Directory.GetFiles(m_strResultsFolder, "*.*",SearchOption.TopDirectoryOnly);

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        iNumFiles++;
                        System.IO.FileInfo fileFeatures = new System.IO.FileInfo(file);

                        if (((DateTime.UtcNow - fileFeatures.LastWriteTimeUtc).TotalMinutes > m_iExpirationTime)&&( Path.GetFileName(file)!=ctBaseFileName))
                        {
                            iErrorCode = ctFileTooOldCode;
                            strErrorMsg += string.Format(",{0}-{1}",iErrorCode,file);
                            bAllOk = false;
                            m_Log.LogMessage(LogLevels.logERROR, string.Format("integraOCOServerManager::GenerateResponse: {0}-{1}-{2}", ctFileTooOldCode, file, fileFeatures.LastWriteTimeUtc));

                        }
                        else
                        {
                            string fileContent = System.IO.File.ReadAllText(file);
                            int iFileContent = Convert.ToInt32(fileContent);

                            if (iFileContent>=ctFirstErrorCode)
                            {
                                bAllOk = false;

                            }

                            if (iFileContent > ctSuccessCode)
                            {
                                strErrorMsg += string.Format(",{0}-{1}", iFileContent, file);
                                m_Log.LogMessage(LogLevels.logERROR, string.Format("integraOCOServerManager::GenerateResponse: {0}-{1}",iFileContent,file ));
                            }

                            if (iFileContent > iErrorCode)
                            {
                                iErrorCode = iFileContent;
                            }
                            
                        }     

                    
                    }
                }

                if (iNumFiles == 0)
                {
                    iErrorCode = ctFilesNotExistCode;
                    strErrorMsg = string.Format("{0}-no_results", iErrorCode);
                    m_Log.LogMessage(LogLevels.logERROR, "integraOCOServerManager::GenerateResponse: NO FILES!!!!");

                }
            }
            catch(Exception e)
            {
                iErrorCode = ctFirstErrorCode;
                strErrorMsg += string.Format(",{0}-exception", iErrorCode);
                m_Log.LogMessage(LogLevels.logERROR, "integraOCOServerManager::GenerateResponse", e);
            }


            if (iErrorCode == ctSuccessCode)
            {
                strErrorMsg = "";
            }
            else
            {
                if (strErrorMsg[0] == ',')
                {
                    strErrorMsg = strErrorMsg.Remove(0, 1);
                }

            }

            try
            {
                if (iErrorCode >= ctFirstErrorCode)
                {
                    strResponse = string.Format("{0} KO {1} {2} {3} {4}", iErrorCode, strLocalAddress, m_HostName, ctOCO_VERSION, strErrorMsg);
                }
                else
                {
                    strResponse = string.Format("{0} OK {1} {2} {3} {4}", iErrorCode, strLocalAddress, m_HostName, ctOCO_VERSION,strErrorMsg);
                }
            }
            catch (Exception e)
            {

            }


            return strResponse;
        }


        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "integraOCOServerManager::SendCallback", e);
                Console.WriteLine(e.ToString());
            }
        }
       
		#endregion

    }
}
