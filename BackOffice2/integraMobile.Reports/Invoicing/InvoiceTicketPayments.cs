namespace integraMobile.Reports.Invoicing
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for InvoiceTicketPayments.
    /// </summary>
    public partial class InvoiceTicketPayments : Telerik.Reporting.Report
    {
        public InvoiceTicketPayments()
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

        private void Report_ItemDataBinding(object sender, EventArgs e)
        {
            Telerik.Reporting.Processing.Report oReport = (Telerik.Reporting.Processing.Report)sender;
            try
            {
                this.ApplyCurrency(oReport.Parameters["CurrencyIsoCode"].Value.ToString());
            }
            catch (Exception ex) { }
        }
    }
}