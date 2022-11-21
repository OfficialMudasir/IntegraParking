namespace integraMobile.Reports.Finantial
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.Globalization;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using backOffice.Infrastructure;
    using backOffice.Infrastructure.Security;

    /// <summary>
    /// Summary description for Deposits2.
    /// </summary>
    public partial class Deposits2 : Telerik.Reporting.Report
    {
        public Deposits2()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.Visible = this.ReportAccess();

            this.ApplyResources();
            this.ApplyCurrency(System.Configuration.ConfigurationManager.AppSettings["ApplicationCurrencyISOCode"] ?? "EUR");
            this.ConfigureInstallationsDatasource(this.dsInstallations,false);

            this.ReportParameters["TimeZone"].AvailableValues.DataSource = TimeZoneInfo.GetSystemTimeZones();
            this.ReportParameters["TimeZone"].AvailableValues.DisplayMember = "DisplayName";
            this.ReportParameters["TimeZone"].AvailableValues.ValueMember = "Id";
            this.ReportParameters["TimeZone"].AllowNull = false;
            this.ReportParameters["TimeZone"].AllowBlank = false;
            this.ReportParameters["TimeZone"].Value = TimeZoneInfo.Local.Id;
            
        }

        private void Deposits2_ItemDataBinding(object sender, EventArgs e)
        {
            Telerik.Reporting.Processing.Report oReport = (Telerik.Reporting.Processing.Report)sender;
            try
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oReport.Parameters["TimeZone"].Value.ToString());
                object objDateIni = oReport.Parameters["DateIni"].Value;
                if (objDateIni != null)
                {
                    DateTime dtParam = Convert.ToDateTime(objDateIni);
                    DateTime dtDateIni = new DateTime(dtParam.Year, dtParam.Month, dtParam.Day, dtParam.Hour, dtParam.Minute, dtParam.Second);
                    oReport.Parameters["DateIniUTC"].Value = TimeZoneInfo.ConvertTime(dtDateIni, tzi, TimeZoneInfo.Utc);
                }
                object objDateEnd = oReport.Parameters["DateEnd"].Value;
                if (objDateEnd != null)
                {
                    DateTime dtParam = Convert.ToDateTime(objDateEnd);
                    DateTime dtDateEnd = new DateTime(dtParam.Year, dtParam.Month, dtParam.Day, dtParam.Hour, dtParam.Minute, dtParam.Second);
                    oReport.Parameters["DateEndUTC"].Value = TimeZoneInfo.ConvertTime(dtDateEnd, tzi, TimeZoneInfo.Utc);
                }
            }
            catch (Exception ex) { }
        }


    }
}