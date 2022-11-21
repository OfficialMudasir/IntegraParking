namespace integraMobile.Reports.Finantial
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for GeneralData.
    /// </summary>
    public partial class GeneralData : Telerik.Reporting.Report
    {
        public GeneralData()
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
            this.ConfigureInstallationsDatasource(this.dsInstallations);

            this.ReportParameters["TimeZone"].AvailableValues.DataSource = TimeZoneInfo.GetSystemTimeZones();
            this.ReportParameters["TimeZone"].AvailableValues.DisplayMember = "DisplayName";
            this.ReportParameters["TimeZone"].AvailableValues.ValueMember = "Id";
            this.ReportParameters["TimeZone"].AllowNull = false;
            this.ReportParameters["TimeZone"].AllowBlank = false;
            this.ReportParameters["TimeZone"].Value = TimeZoneInfo.Local.Id;

            //Telerik.Reporting.Services.Engine.CacheFactory.CreateDatabaseCache("mssql", "Data Source=HBUSQUE-LAPTOP\SQLEXPRESS2012;Initial Catalog=ASPState;Integrated Security=True");
            

        }

        private void Report_ItemDataBinding(object sender, EventArgs e)
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