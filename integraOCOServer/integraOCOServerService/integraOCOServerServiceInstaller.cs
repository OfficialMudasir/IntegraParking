using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace integraOCOServerService
{
    [RunInstaller(true)]
    public partial class integraOCOServerServiceInstaller : System.Configuration.Install.Installer
    {
        public integraOCOServerServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
