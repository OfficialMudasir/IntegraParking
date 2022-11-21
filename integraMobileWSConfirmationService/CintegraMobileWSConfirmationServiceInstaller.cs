using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace integraMobileWSConfirmationService
{
    [RunInstaller(true)]
    public partial class CintegraMobileWSConfirmationServiceInstaller : System.Configuration.Install.Installer
    {
        public CintegraMobileWSConfirmationServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
