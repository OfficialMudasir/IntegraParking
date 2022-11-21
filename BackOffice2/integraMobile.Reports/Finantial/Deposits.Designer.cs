namespace integraMobile.Reports.Finantial
{
    partial class Deposits
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Deposits));
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.InstanceReportSource instanceReportSource1 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.InstanceReportSource instanceReportSource2 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter5 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter6 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            this.benefits1 = new integraMobile.Reports.Finantial.Benefits();
            this.balance1 = new integraMobile.Reports.Finantial.Balance();
            this.dsInstallations = new Telerik.Reporting.SqlDataSource();
            this.dsDetail = new Telerik.Reporting.SqlDataSource();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.subReportLiduidation = new Telerik.Reporting.SubReport();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.subReportBenefits = new Telerik.Reporting.SubReport();
            this.subReportBalance = new Telerik.Reporting.SubReport();
            this.lblPagatelia = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.benefits1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.balance1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // benefits1
            // 
            this.benefits1.Name = "Benefits";
            // 
            // balance1
            // 
            this.balance1.Name = "Balance";
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
            this.dsDetail.SelectCommand = "dbo.Report_DEPOSITS";
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
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.59055119752883911D);
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
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.599900245666504D), Telerik.Reporting.Drawing.Unit.Cm(2.9999008178710938D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0999011993408203D), Telerik.Reporting.Drawing.Unit.Cm(0.39989924430847168D));
            this.currentTimeTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.currentTimeTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.currentTimeTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.currentTimeTextBox.Style.Visible = false;
            this.currentTimeTextBox.StyleName = "PageInfo";
            resources.ApplyResources(this.currentTimeTextBox, "currentTimeTextBox");
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(8.1999998092651367D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox2,
            this.textBox3,
            this.textBox4,
            this.subReportLiduidation,
            this.textBox6,
            this.textBox7,
            this.textBox8,
            this.textBox9,
            this.textBox10,
            this.subReportBenefits,
            this.subReportBalance,
            this.lblPagatelia,
            this.textBox13});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // textBox1
            // 
            this.textBox1.Format = "{0:C2}";
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.5D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1998014450073242D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox1, "textBox1");
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.9000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(1.1000006198883057D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.900001049041748D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // textBox4
            // 
            this.textBox4.Format = "{0:C2}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.5D), Telerik.Reporting.Drawing.Unit.Cm(1.100200891494751D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1998014450073242D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox4, "textBox4");
            // 
            // subReportLiduidation
            // 
            this.subReportLiduidation.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(5.2000007629394531D));
            this.subReportLiduidation.Name = "subReportLiduidation";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("DateIni", "=Parameters.DateIniUTC.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("DateEnd", "=Parameters.DateEndUTC.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("Installations", "=Parameters.Installations.Value"));
            typeReportSource1.TypeName = "integraMobile.Reports.Finantial.LiquidationDetailSub, integraMobile.Reports, Vers" +
    "ion=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            this.subReportLiduidation.ReportSource = typeReportSource1;
            this.subReportLiduidation.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.499799728393555D), Telerik.Reporting.Drawing.Unit.Cm(2.9999997615814209D));
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(1.6401004791259766D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.9001011848449707D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox6, "textBox6");
            // 
            // textBox7
            // 
            this.textBox7.Format = "{0:C2}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.5D), Telerik.Reporting.Drawing.Unit.Cm(1.6403011083602905D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1998014450073242D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox7, "textBox7");
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(2.180401086807251D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.9000020027160645D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox8, "textBox8");
            // 
            // textBox9
            // 
            this.textBox9.Format = "{0:C2}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.5D), Telerik.Reporting.Drawing.Unit.Cm(2.1804006099700928D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1999025344848633D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox9, "textBox9");
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(4.4607019424438477D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.40000057220459D), Telerik.Reporting.Drawing.Unit.Cm(0.63929963111877441D));
            this.textBox10.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox10, "textBox10");
            // 
            // subReportBenefits
            // 
            this.subReportBenefits.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(3.1000008583068848D));
            this.subReportBenefits.Name = "subReportBenefits";
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("DateIni", "=Parameters.DateIniUTC.Value"));
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("DateEnd", "=Parameters.DateEndUTC.Value"));
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("Installations", "=Parameters.Installations.Value"));
            instanceReportSource1.ReportDocument = this.benefits1;
            this.subReportBenefits.ReportSource = instanceReportSource1;
            this.subReportBenefits.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.499799728393555D), Telerik.Reporting.Drawing.Unit.Cm(1.0399994850158691D));
            // 
            // subReportBalance
            // 
            this.subReportBalance.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(4.5000009536743164D));
            this.subReportBalance.Name = "subReportBalance";
            instanceReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("Date", "=Parameters.DateIniUTC.Value"));
            instanceReportSource2.ReportDocument = this.balance1;
            this.subReportBalance.ReportSource = instanceReportSource2;
            this.subReportBalance.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8998012542724609D), Telerik.Reporting.Drawing.Unit.Cm(0.69979977607727051D));
            this.subReportBalance.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // lblPagatelia
            // 
            this.lblPagatelia.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.70010095834732056D), Telerik.Reporting.Drawing.Unit.Cm(0.54010027647018433D));
            this.lblPagatelia.Name = "lblPagatelia";
            this.lblPagatelia.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.9000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.lblPagatelia.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.lblPagatelia, "lblPagatelia");
            // 
            // textBox13
            // 
            this.textBox13.Format = "{0:C2}";
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.5001001358032227D), Telerik.Reporting.Drawing.Unit.Cm(0.54010027647018433D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.1998014450073242D), Telerik.Reporting.Drawing.Unit.Cm(0.539900004863739D));
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox13, "textBox13");
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(4.0999999046325684D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5,
            this.textBox12,
            this.textBox14,
            this.currentTimeTextBox});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(3.4000003337860107D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.499799728393555D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.textBox5.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(167)))), ((int)(((byte)(227)))));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.textBox5, "textBox5");
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
            // Deposits
            // 
            this.Culture = new System.Globalization.CultureInfo("");
            this.DataSource = this.dsDetail;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detailSection1,
            this.pageFooterSection1,
            this.reportFooterSection1,
            this.reportHeaderSection1});
            this.Name = "Deposits";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AvailableValues.DisplayMember = "Description";
            reportParameter1.AvailableValues.ValueMember = "Value";
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
            reportParameter5.Name = "DateIniUTC";
            reportParameter5.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter5.Value = "= Date(Today().Year, Today().Month, 1)";
            reportParameter6.Name = "DateEndUTC";
            reportParameter6.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter6.Value = "= Today()";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.ReportParameters.Add(reportParameter5);
            this.ReportParameters.Add(reportParameter6);
            this.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.DATE_UTC", Telerik.Reporting.SortDirection.Asc));
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
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(6.2677168846130371D);
            this.ItemDataBinding += new System.EventHandler(this.Deposits_ItemDataBinding);
            ((System.ComponentModel.ISupportInitialize)(this.benefits1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.balance1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource dsDetail;
        private Telerik.Reporting.SqlDataSource dsInstallations;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.SubReport subReportLiduidation;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.SubReport subReportBenefits;
        private Benefits benefits1;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.SubReport subReportBalance;
        private Balance balance1;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox lblPagatelia;
        private Telerik.Reporting.TextBox textBox13;

    }
}