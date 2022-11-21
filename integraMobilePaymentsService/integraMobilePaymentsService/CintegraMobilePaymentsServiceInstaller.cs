using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace integraMobilePaymentsService
{
    [RunInstaller(true)]
    public partial class CintegraMobilePaymentsServiceInstaller : System.Configuration.Install.Installer
    {
        public CintegraMobilePaymentsServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
