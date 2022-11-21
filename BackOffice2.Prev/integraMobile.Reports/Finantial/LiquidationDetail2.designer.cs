namespace integraMobile.Reports.Finantial
{
    partial class LiquidationDetail
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiquidationDetail));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group3 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter5 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter6 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.OperatorFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.panel1 = new Telerik.Reporting.Panel();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.OperatorHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.InstallationFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.InstallationHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.dsInstallations = new Telerik.Reporting.SqlDataSource();
            this.dsDetail = new Telerik.Reporting.SqlDataSource();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // OperatorFooterSection
            // 
            this.OperatorFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681D);
            this.OperatorFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel1});
            this.OperatorFooterSection.Name = "OperatorFooterSection";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5,
            this.textBox6});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3001976013183594D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.199902534484863D), Telerik.Reporting.Drawing.Unit.Cm(1.100100040435791D));
            this.panel1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox5, "textBox5");
            // 
            // textBox6
            // 
            this.textBox6.Format = "{0:C2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.9999985694885254D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1999039649963379D), Telerik.Reporting.Drawing.Unit.Cm(0.69989991188049316D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox6, "textBox6");
            // 
            // OperatorHeaderSection
            // 
            this.OperatorHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D);
            this.OperatorHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox7});
            this.OperatorHeaderSection.Name = "OperatorHeaderSection";
            this.OperatorHeaderSection.Style.Visible = true;
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6D), Telerik.Reporting.Drawing.Unit.Cm(0.5999000072479248D));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox7, "textBox7");
            // 
            // InstallationFooterSection
            // 
            this.InstallationFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.50000017881393433D);
            this.InstallationFooterSection.Name = "InstallationFooterSection";
            this.InstallationFooterSection.Style.Visible = true;
            // 
            // InstallationHeaderSection
            // 
            this.InstallationHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.5400005578994751D);
            this.InstallationHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1});
            this.InstallationHeaderSection.Name = "InstallationHeaderSection";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.7000001072883606D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.600001335144043D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox1, "textBox1");
            // 
            // GroupFooterSection
            // 
            this.GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1199002265930176D);
            this.GroupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox3,
            this.textBox2,
            this.textBox9,
            this.textBox10});
            this.GroupFooterSection.Name = "GroupFooterSection";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.00019943872757721692D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.499799728393555D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0:C2}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.700000762939453D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.799999475479126D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // textBox9
            // 
            this.textBox9.Format = "{0:C2}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.700000762939453D), Telerik.Reporting.Drawing.Unit.Cm(0.54029971361160278D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.8000996112823486D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox9, "textBox9");
            // 
            // GroupHeaderSection
            // 
            this.GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999920129776D);
            this.GroupHeaderSection.Name = "GroupHeaderSection";
            this.GroupHeaderSection.Style.Visible = false;
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
            this.dsDetail.SelectCommand = "dbo.Report_LIQUIDATIONDETAIL";
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
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.42523655295372009D);
            this.detailSection1.Name = "detailSection1";
            this.detailSection1.Style.Visible = false;
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.70866173505783081D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox4});
            this.pageFooterSection1.Name = "pageFooterSection1";
            this.pageFooterSection1.Style.Visible = false;
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.499998092651367D), Telerik.Reporting.Drawing.Unit.Cm(0.80000042915344238D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(1.0000003576278687D));
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox4, "textBox4");
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.5000004768371582D);
            this.reportFooterSection1.Name = "reportFooterSection1";
            this.reportFooterSection1.Style.Visible = false;
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3.3999998569488525D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox14,
            this.textBox12,
            this.currentTimeTextBox});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(1.3000001907348633D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(1.3997994661331177D));
            this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            resources.ApplyResources(this.textBox14, "textBox14");
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896D));
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(20D);
            resources.ApplyResources(this.textBox12, "textBox12");
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.400097846984863D), Telerik.Reporting.Drawing.Unit.Cm(2.9000000953674316D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0999011993408203D), Telerik.Reporting.Drawing.Unit.Cm(0.39989924430847168D));
            this.currentTimeTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.currentTimeTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.currentTimeTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.currentTimeTextBox.Style.Visible = false;
            this.currentTimeTextBox.StyleName = "PageInfo";
            resources.ApplyResources(this.currentTimeTextBox, "currentTimeTextBox");
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.1999955177307129D), Telerik.Reporting.Drawing.Unit.Cm(0.54029971361160278D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.4998040199279785D), Telerik.Reporting.Drawing.Unit.Cm(0.53990083932876587D));
            this.textBox10.Style.Font.Bold = false;
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox10, "textBox10");
            // 
            // LiquidationDetail
            // 
            this.DataSource = this.dsDetail;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.OPE_INS_ID", Telerik.Reporting.FilterOperator.In, "=Parameters.Installations.Value"));
            group1.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.DIST_AMOUNT)", Telerik.Reporting.FilterOperator.NotEqual, "0"));
            group1.GroupFooter = this.OperatorFooterSection;
            group1.GroupHeader = this.OperatorHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OHD_FDO_ID"));
            group1.Name = "Operator";
            group1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.FDO_DESCCRIPTION", Telerik.Reporting.SortDirection.Asc));
            group2.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.DIST_AMOUNT)", Telerik.Reporting.FilterOperator.NotEqual, "0"));
            group2.GroupFooter = this.InstallationFooterSection;
            group2.GroupHeader = this.InstallationHeaderSection;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OPE_INS_ID"));
            group2.Name = "Installation";
            group2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.INS_DESCRIPTION", Telerik.Reporting.SortDirection.Asc));
            group3.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.DIST_AMOUNT)", Telerik.Reporting.FilterOperator.NotEqual, "0"));
            group3.GroupFooter = this.GroupFooterSection;
            group3.GroupHeader = this.GroupHeaderSection;
            group3.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.GRP_ID"));
            group3.Name = "Group";
            group3.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.GRP_DESCRIPTION", Telerik.Reporting.SortDirection.Asc));
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2,
            group3});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.OperatorHeaderSection,
            this.OperatorFooterSection,
            this.InstallationHeaderSection,
            this.InstallationFooterSection,
            this.GroupHeaderSection,
            this.GroupFooterSection,
            this.pageHeaderSection1,
            this.detailSection1,
            this.pageFooterSection1,
            this.reportFooterSection1,
            this.reportHeaderSection1});
            this.Name = "LiquidationDetail";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "TimeZone";
            resources.ApplyResources(reportParameter1, "reportParameter1");
            reportParameter1.Visible = true;
            reportParameter2.Name = "DateIni";
            resources.ApplyResources(reportParameter2, "reportParameter2");
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.Value = "";
            reportParameter2.Visible = true;
            reportParameter3.Name = "DateEnd";
            resources.ApplyResources(reportParameter3, "reportParameter3");
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter3.Value = "";
            reportParameter3.Visible = true;
            reportParameter4.AvailableValues.DataSource = this.dsInstallations;
            reportParameter4.AvailableValues.DisplayMember = "= Fields.INS_DESCRIPTION";
            reportParameter4.AvailableValues.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.INS_DESCRIPTION", Telerik.Reporting.SortDirection.Asc));
            reportParameter4.AvailableValues.ValueMember = "= Fields.INS_ID";
            reportParameter4.MultiValue = true;
            reportParameter4.Name = "Installations";
            resources.ApplyResources(reportParameter4, "reportParameter4");
            reportParameter4.Type = Telerik.Reporting.ReportParameterType.Float;
            reportParameter4.Value = "= Fields.INS_ID";
            reportParameter4.Visible = true;
            reportParameter5.AllowNull = true;
            reportParameter5.Name = "DateIniUTC";
            reportParameter5.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter6.AllowNull = true;
            reportParameter6.Name = "DateEndUTC";
            reportParameter6.Type = Telerik.Reporting.ReportParameterType.DateTime;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.ReportParameters.Add(reportParameter5);
            this.ReportParameters.Add(reportParameter6);
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
            this.ItemDataBinding += new System.EventHandler(this.LiquidationDetail2_ItemDataBinding);
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
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.GroupHeaderSection OperatorHeaderSection;
        private Telerik.Reporting.GroupFooterSection OperatorFooterSection;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.GroupHeaderSection InstallationHeaderSection;
        private Telerik.Reporting.GroupFooterSection InstallationFooterSection;
        private Telerik.Reporting.GroupHeaderSection GroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection GroupFooterSection;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox10;

    }
}