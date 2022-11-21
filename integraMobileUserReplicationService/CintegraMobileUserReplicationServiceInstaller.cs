using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace integraMobileUserReplicationService
{
    [RunInstaller(true)]
    public partial class CintegraMobileUserReplicationServiceInstaller : System.Configuration.Install.Installer
    {
        public CintegraMobileUserReplicationServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
