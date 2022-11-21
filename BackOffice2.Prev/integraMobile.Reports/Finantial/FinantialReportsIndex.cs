namespace integraMobile.Reports.Finantial
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Reflection;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using backOffice.Infrastructure;
    using integraMobile.Reports;

    /// <summary>
    /// Summary description for FinantialReportsIndex.
    /// </summary>
    public partial class FinantialReportsIndex : Telerik.Reporting.Report
    {        
        public FinantialReportsIndex()
        {
            //this.Culture = new System.Globalization.CultureInfo("ca-ES");
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinantialReportsIndex));
            //resources.ApplyResources(this.textBox1, "textBox1");

            //ResourceBundle resBundle = ResourceBundle.GetInstance("PBPPlugin");
            //this.Visible = this.ReportAccess();

            this.ApplyResources();
            this.ApplyCurrency(System.Configuration.ConfigurationManager.AppSettings["ApplicationCurrencyISOCode"] ?? "EUR");

            panDeposits.Visible = this.ReportAccess("FinantialReports.Deposits");
            panLiquidationDetail.Visible = this.ReportAccess("FinantialReports.LiquidationDetail");
            panBank.Visible = this.ReportAccess("FinantialReports.Bank");
            panGeneralData.Visible = this.ReportAccess("FinantialReports.GeneralData");
            panGeneralDataInst.Visible = this.ReportAccess("FinantialReports.GeneralDataInstallation");
            panRegisteredUsers.Visible = this.ReportAccess("FinantialReports.RegisteredUsers");

        }
    }
}