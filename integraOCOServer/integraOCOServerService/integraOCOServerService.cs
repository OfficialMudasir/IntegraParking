using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace integraOCOServerService
{
    public partial class integraOCOServerService : ServiceBase
    {
        

        #region -- Member variables --

        // Mail Sender class
        private integraOCOServerManager m_integraOCOServerManager;

        #endregion 


        public integraOCOServerService()
        {
            InitializeComponent();
        }

        // The main entry point for the process
        static void Main(string[] args)
        {      
  
            if (args.Length > 0 && args[0].ToUpper().Equals("-C"))
                ApplicationMain(args);
            else
                ServiceMain(args);
        }

        static void ServiceMain(string[] args)
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new integraOCOServerService() };

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);

            // Starts the main Thread of the Asynchronous Socket Server
        }

        static void ApplicationMain(string[] args)
        {

            // Start running
            integraOCOServerService engine = new integraOCOServerService();
        
            engine.OnStart(args);

            // Wait to end
            Console.WriteLine("*** Press Enter to exit ***");
            Console.ReadLine();
            Console.WriteLine("Quitting...");

            // Stop running
            engine.OnStop();

            GC.WaitForPendingFinalizers();
        }


        protected override void OnStart(string[] args)
        {
            // Initialize the service with the user configuration data

            m_integraOCOServerManager = new integraOCOServerManager();

            m_integraOCOServerManager.Start();
        }

        protected override void OnStop()
        {
            //Notify the Main Socket Server Thread the end of the service
            m_integraOCOServerManager.Stop();
        }
    }
}
