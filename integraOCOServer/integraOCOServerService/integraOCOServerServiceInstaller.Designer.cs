using System.Configuration.Install;
using System.Configuration;

namespace integraOCOServerService
{
    partial class integraOCOServerServiceInstaller
    {

        private System.ServiceProcess.ServiceProcessInstaller integraOCOServerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller integraOCOServerServiceInstallerMember;

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

            this.integraOCOServerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.integraOCOServerServiceInstallerMember = new System.ServiceProcess.ServiceInstaller();
            // 
            // integraOCOServerServiceProcessInstaller
            // 
            this.integraOCOServerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.integraOCOServerServiceProcessInstaller.Password = null;
            this.integraOCOServerServiceProcessInstaller.Username = null;
            // 
            // integraOCOServerServiceInstaller
            // 

            this.integraOCOServerServiceInstallerMember.DisplayName = (string)appSettings.GetValue("ServiceDisplayName", typeof(string));
            this.integraOCOServerServiceInstallerMember.ServiceName = (string)appSettings.GetValue("ServiceName", typeof(string));

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

                this.integraOCOServerServiceInstallerMember.ServicesDependedOn = serviceDependencies;


            }

            integraOCOServerServiceInstallerMember.StartType = System.ServiceProcess.ServiceStartMode.Automatic;


            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                                                                                      this.integraOCOServerServiceProcessInstaller,
                                                                                      this.integraOCOServerServiceInstallerMember});

        }


        #endregion
    }
}