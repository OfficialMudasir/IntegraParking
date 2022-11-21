namespace integraMobile.Reports.Finantial
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using backOffice.Infrastructure;

    /// <summary>
    /// Summary description for Balance.
    /// </summary>
    public partial class Balance : Telerik.Reporting.Report
    {
        public Balance()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            this.ApplyResources();
            this.ApplyCurrency(System.Configuration.ConfigurationManager.AppSettings["ApplicationCurrencyISOCode"] ?? "EUR");            

        }
    }
}