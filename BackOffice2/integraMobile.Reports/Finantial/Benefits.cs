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
    /// Summary description for Benefits.
    /// </summary>
    public partial class Benefits : Telerik.Reporting.Report
    {
        public Benefits()
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