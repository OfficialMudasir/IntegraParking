namespace integraMobile.Reports.Finantial
{
    partial class Deposits3
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Deposits3));
            Telerik.Reporting.TypeReportSource typeReportSource7 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.ReportParameter reportParameter37 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter38 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter39 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter40 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter41 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter42 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule13 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule14 = new Telerik.Reporting.Drawing.StyleRule();
            this.dsInstallations = new Telerik.Reporting.SqlDataSource();
            this.dsDetail = new Telerik.Reporting.SqlDataSource();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.subReportLiduidation = new Telerik.Reporting.SubReport();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // dsInstallations
            // 
            this.dsInstallations.ConnectionString = "integraMobile.Reports.Properties.Settings.integraMobileReports";
            this.dsInstallations.Name = "dsInstallations";
            this.dsInstallations.SelectCommand = "SELECT INS_ID, INS_DESCRIPTION\r\nFROM INSTALLATIONS\r\nWHERE INS_ENABLED = 1\r\nUNION\r" +
    "\nSELECT 0, \'*** Instalación recargas ***\'";
            // 
            // dsDetail
            // 
            this.dsDetail.CommandTimeout = 0;
            this.dsDetail.ConnectionString = "integraMobile.Reports.Properties.Settings.integraMobileReports";
            this.dsDetail.Name = "dsDetail";
            this.dsDetail.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@DateIniUTC", System.Data.DbType.DateTime, "= Parameters.DateIniUTC.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@DateEndUTC", System.Data.DbType.DateTime, "= Parameters.DateEndUTC.Value")});
            this.dsDetail.SelectCommand = "dbo.Report_Deposits";
            this.dsDetail.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.43307080864906311D);
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            this.pageHeaderSection1.Style.Visible = false;
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.15748023986816406D);
            this.detailSection1.Name = "detailSection1";
            this.detailSection1.Style.Visible = false;
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.55118054151535034D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.600098609924316D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.09970235824585D), Telerik.Reporting.Drawing.Unit.Cm(0.39989924430847168D));
            this.pageInfoTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            resources.ApplyResources(this.pageInfoTextBox, "pageInfoTextBox");
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3.3000001907348633D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.subReportLiduidation});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // subReportLiduidation
            // 
            this.subReportLiduidation.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.subReportLiduidation.Name = "subReportLiduidation";
            typeReportSource7.Parameters.Add(new Telerik.Reporting.Parameter("DateIni", "=Parameters.DateIniUTC.Value"));
            typeReportSource7.Parameters.Add(new Telerik.Reporting.Parameter("DateEnd", "=Parameters.DateEndUTC.Value"));
            typeReportSource7.Parameters.Add(new Telerik.Reporting.Parameter("Installations", "=Parameters.Installations.Value"));
            typeReportSource7.Parameters.Add(new Telerik.Reporting.Parameter("IsCouncil", "1"));
            typeReportSource7.TypeName = "integraMobile.Reports.Finantial.LiquidationDetail3Sub, integraMobile.Reports, Ver" +
    "sion=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            this.subReportLiduidation.ReportSource = typeReportSource7;
            this.subReportLiduidation.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.499799728393555D), Telerik.Reporting.Drawing.Unit.Cm(2.9999997615814209D));
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.8000001907348633D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox12,
            this.textBox14});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.499799728393555D), Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896D));
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(20D);
            resources.ApplyResources(this.textBox12, "textBox12");
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(1.3000001907348633D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.499799728393555D), Telerik.Reporting.Drawing.Unit.Cm(1.0995997190475464D));
            this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            resources.ApplyResources(this.textBox14, "textBox14");
            // 
            // Deposits3
            // 
            this.Culture = new System.Globalization.CultureInfo("");
            this.DataSource = this.dsDetail;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detailSection1,
            this.pageFooterSection1,
            this.reportFooterSection1,
            this.reportHeaderSection1});
            this.Name = "Deposits3";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter37.AvailableValues.DisplayMember = "Description";
            reportParameter37.AvailableValues.ValueMember = "Value";
            reportParameter37.Name = "TimeZone";
            resources.ApplyResources(reportParameter37, "reportParameter37");
            reportParameter37.Visible = true;
            reportParameter38.Name = "DateIni";
            resources.ApplyResources(reportParameter38, "reportParameter38");
            reportParameter38.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter38.Value = "";
            reportParameter38.Visible = true;
            reportParameter39.Name = "DateEnd";
            resources.ApplyResources(reportParameter39, "reportParameter39");
            reportParameter39.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter39.Value = "";
            reportParameter39.Visible = true;
            reportParameter40.AvailableValues.DataSource = this.dsInstallations;
            reportParameter40.AvailableValues.DisplayMember = "= Fields.INS_DESCRIPTION";
            reportParameter40.AvailableValues.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.INS_DESCRIPTION", Telerik.Reporting.SortDirection.Asc));
            reportParameter40.AvailableValues.ValueMember = "= Fields.INS_ID";
            reportParameter40.MultiValue = true;
            reportParameter40.Name = "Installations";
            resources.ApplyResources(reportParameter40, "reportParameter40");
            reportParameter40.Type = Telerik.Reporting.ReportParameterType.Float;
            reportParameter40.Value = "= Fields.INS_ID";
            reportParameter40.Visible = true;
            reportParameter41.Name = "DateIniUTC";
            reportParameter41.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter41.Value = "= Date(Today().Year, Today().Month, 1)";
            reportParameter42.Name = "DateEndUTC";
            reportParameter42.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter42.Value = "= Today()";
            this.ReportParameters.Add(reportParameter37);
            this.ReportParameters.Add(reportParameter38);
            this.ReportParameters.Add(reportParameter39);
            this.ReportParameters.Add(reportParameter40);
            this.ReportParameters.Add(reportParameter41);
            this.ReportParameters.Add(reportParameter42);
            this.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.DATE_UTC", Telerik.Reporting.SortDirection.Asc));
            styleRule13.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule13.Style.Font.Name = "Tahoma";
            styleRule13.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule13.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule14.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule14.Style.Font.Name = "Tahoma";
            styleRule14.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule14.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule13,
            styleRule14});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(6.2677168846130371D);
            this.ItemDataBinding += new System.EventHandler(this.Deposits3_ItemDataBinding);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource dsDetail;
        private Telerik.Reporting.SqlDataSource dsInstallations;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.SubReport subReportLiduidation;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox14;

    }
}