using System.Configuration.Install;
using System.Configuration;

namespace integraMobilePaymentsService
{
    partial class CintegraMobilePaymentsServiceInstaller
    {

        private System.ServiceProcess.ServiceProcessInstaller integraMobilePaymentsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller integraMobilePaymentsServiceInstallerMember;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Configuration.AppSettingsReader appSettings = new System.Configuration.AppSettingsReader();

            this.integraMobilePaymentsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.integraMobilePaymentsServiceInstallerMember = new System.ServiceProcess.ServiceInstaller();
            // 
            // integraMobilePaymentsServiceProcessInstaller
            // 
            this.integraMobilePaymentsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.integraMobilePaymentsServiceProcessInstaller.Password = null;
            this.integraMobilePaymentsServiceProcessInstaller.Username = null;
            // 
            // integraMobilePaymentsServiceInstaller
            // 

            this.integraMobilePaymentsServiceInstallerMember.DisplayName = (string)appSettings.GetValue("ServiceDisplayName", typeof(string));
            this.integraMobilePaymentsServiceInstallerMember.ServiceName = (string)appSettings.GetValue("ServiceName", typeof(string));

            string delimStr = ";";
            char[] delimiter = delimStr.ToCharArray();
            string[] splitDependencies = null;
            //string strDependencies = (string)appSettings.GetValue("ServiceDependencies", typeof(string));
            string strDependencies = "";
            splitDependencies = strDependencies.Split(delimiter);
            int nValues = 0;

            for (int i = 0; i < splitDependencies.Length; i++)
            {
                splitDependencies[i] = splitDependencies[i].Trim();
                if (splitDependencies[i].Length > 0)
                {
                    nValues++;
                }
            }

            if (nValues > 0)
            {
                string[] serviceDependencies = new string[nValues];
                int j = 0;
                for (int i = 0; i < splitDependencies.Length; i++)
                {
                    if (splitDependencies[i].Length > 0)
                    {
                        serviceDependencies[j++] = splitDependencies[i];
                    }
                }

                this.integraMobilePaymentsServiceInstallerMember.ServicesDependedOn = serviceDependencies;


            }

            integraMobilePaymentsServiceInstallerMember.StartType = System.ServiceProcess.ServiceStartMode.Automatic;


            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                                                                                      this.integraMobilePaymentsServiceProcessInstaller,
                                                                                      this.integraMobilePaymentsServiceInstallerMember});

        }


        #endregion
    }
}