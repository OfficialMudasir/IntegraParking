namespace integraMobile.Reports.Finantial
{
    partial class LiquidationDetailSub
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiquidationDetailSub));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.InstallationFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.panel1 = new Telerik.Reporting.Panel();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.InstallationHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.OperatorFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.OperatorHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.dsInstallations = new Telerik.Reporting.SqlDataSource();
            this.dsDetail = new Telerik.Reporting.SqlDataSource();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // InstallationFooterSection
            // 
            this.InstallationFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.99990016222000122D);
            this.InstallationFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel1});
            this.InstallationFooterSection.Name = "InstallationFooterSection";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5,
            this.textBox6});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.399996280670166D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.200003623962402D), Telerik.Reporting.Drawing.Unit.Cm(0.89979976415634155D));
            this.panel1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010052680590888485D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox5, "textBox5");
            // 
            // textBox6
            // 
            this.textBox6.Format = "{0:C2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.5001037120819092D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6998004913330078D), Telerik.Reporting.Drawing.Unit.Cm(0.69989991188049316D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox6, "textBox6");
            // 
            // InstallationHeaderSection
            // 
            this.InstallationHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.60000014305114746D);
            this.InstallationHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox7});
            this.InstallationHeaderSection.Name = "InstallationHeaderSection";
            this.InstallationHeaderSection.Style.Visible = true;
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6D), Telerik.Reporting.Drawing.Unit.Cm(0.5999000072479248D));
            this.textBox7.Style.Font.Bold = true;
            resources.ApplyResources(this.textBox7, "textBox7");
            // 
            // OperatorFooterSection
            // 
            this.OperatorFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.3401006460189819D);
            this.OperatorFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox2,
            this.textBox9,
            this.textBox10});
            this.OperatorFooterSection.Name = "OperatorFooterSection";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.999798774719238D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            resources.ApplyResources(this.textBox1, "textBox1");
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0:C2}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6000006198883057D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // textBox9
            // 
            this.textBox9.Format = "{0:C2}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.54020035266876221D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6001007556915283D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox9, "textBox9");
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.299799919128418D), Telerik.Reporting.Drawing.Unit.Cm(0.54020035266876221D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6999993324279785D), Telerik.Reporting.Drawing.Unit.Cm(0.53990083932876587D));
            this.textBox10.Style.Font.Bold = false;
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox10, "textBox10");
            // 
            // OperatorHeaderSection
            // 
            this.OperatorHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.9999995231628418D);
            this.OperatorHeaderSection.Name = "OperatorHeaderSection";
            this.OperatorHeaderSection.Style.Visible = false;
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
            new Telerik.Reporting.SqlDataSourceParameter("@DateIniUTC", System.Data.DbType.DateTime, "= Parameters.DateIni.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@DateEndUTC", System.Data.DbType.DateTime, "= Parameters.DateEnd.Value")});
            this.dsDetail.SelectCommand = "dbo.Report_LIQUIDATIONDETAILSUB";
            this.dsDetail.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.31496062874794006D);
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            this.pageHeaderSection1.Style.Visible = false;
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.29921245574951172D);
            this.detailSection1.Name = "detailSection1";
            this.detailSection1.Style.Visible = false;
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.57480305433273315D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox4});
            this.pageFooterSection1.Name = "pageFooterSection1";
            this.pageFooterSection1.Style.Visible = false;
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.59999942779541D), Telerik.Reporting.Drawing.Unit.Cm(0.35999980568885803D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(1.0000003576278687D));
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox4, "textBox4");
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.7400003671646118D);
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox3,
            this.textBox11,
            this.textBox8});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(1.5D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.textBox3.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(167)))), ((int)(((byte)(227)))));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.6997995376586914D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox11, "textBox11");
            // 
            // textBox8
            // 
            this.textBox8.Format = "{0:C2}";
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.8999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.7000002861022949D), Telerik.Reporting.Drawing.Unit.Cm(0.53980004787445068D));
            this.textBox8.Style.Font.Bold = false;
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox8, "textBox8");
            // 
            // LiquidationDetailSub
            // 
            this.DataSource = this.dsDetail;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.OPE_INS_ID", Telerik.Reporting.FilterOperator.In, "=Parameters.Installations.Value"));
            group1.GroupFooter = this.InstallationFooterSection;
            group1.GroupHeader = this.InstallationHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OPE_INS_ID"));
            group1.Name = "Installation";
            group1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.INS_DESCRIPTION", Telerik.Reporting.SortDirection.Asc));
            group2.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.DIST_AMOUNT)", Telerik.Reporting.FilterOperator.NotEqual, "0"));
            group2.GroupFooter = this.OperatorFooterSection;
            group2.GroupHeader = this.OperatorHeaderSection;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OHD_FDO_ID"));
            group2.Name = "Operator";
            group2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.FDO_DESCCRIPTION", Telerik.Reporting.SortDirection.Asc));
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.InstallationHeaderSection,
            this.InstallationFooterSection,
            this.OperatorHeaderSection,
            this.OperatorFooterSection,
            this.pageHeaderSection1,
            this.detailSection1,
            this.pageFooterSection1,
            this.reportFooterSection1,
            this.reportHeaderSection1});
            this.Name = "LiquidationDetailSub";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AvailableValues.DataSource = this.dsInstallations;
            reportParameter1.AvailableValues.DisplayMember = "= Fields.INS_DESCRIPTION";
            reportParameter1.AvailableValues.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.INS_DESCRIPTION", Telerik.Reporting.SortDirection.Asc));
            reportParameter1.AvailableValues.ValueMember = "= Fields.INS_ID";
            reportParameter1.MultiValue = true;
            reportParameter1.Name = "Installations";
            resources.ApplyResources(reportParameter1, "reportParameter1");
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Float;
            reportParameter1.Value = "= Fields.INS_ID";
            reportParameter1.Visible = true;
            reportParameter2.Name = "DateIni";
            resources.ApplyResources(reportParameter2, "reportParameter2");
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.Value = "= Date(Today().Year, Today().Month, 1)";
            reportParameter2.Visible = true;
            reportParameter3.Name = "DateEnd";
            resources.ApplyResources(reportParameter3, "reportParameter3");
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter3.Value = "= Today()";
            reportParameter3.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(6.2676773071289062D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource dsDetail;
        private Telerik.Reporting.SqlDataSource dsInstallations;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.GroupHeaderSection InstallationHeaderSection;
        private Telerik.Reporting.GroupFooterSection InstallationFooterSection;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.GroupHeaderSection OperatorHeaderSection;
        private Telerik.Reporting.GroupFooterSection OperatorFooterSection;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox10;

    }
}