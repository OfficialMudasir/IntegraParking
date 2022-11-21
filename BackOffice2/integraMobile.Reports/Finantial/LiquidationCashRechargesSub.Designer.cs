namespace integraMobile.Reports.Finantial
{
    partial class LiquidationCashRechargesSub
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.txtCashAmount = new Telerik.Reporting.TextBox();
            this.txtBackOfficeUser = new Telerik.Reporting.TextBox();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.lblCashTotal = new Telerik.Reporting.TextBox();
            this.txtCashTotal = new Telerik.Reporting.TextBox();
            this.lblCashVAT = new Telerik.Reporting.TextBox();
            this.txtCashVAT = new Telerik.Reporting.TextBox();
            this.dsDetail = new Telerik.Reporting.SqlDataSource();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.54000091552734375D);
            this.groupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtCashAmount,
            this.txtBackOfficeUser});
            this.groupFooterSection.Name = "groupFooterSection";
            // 
            // txtCashAmount
            // 
            this.txtCashAmount.Format = "{0:C2}";
            this.txtCashAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.7002997398376465D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtCashAmount.Name = "txtCashAmount";
            this.txtCashAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.9998998641967773D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.txtCashAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txtCashAmount.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtCashAmount.Value = "=-Sum(Fields.DIST_AMOUNT)/100";
            // 
            // txtBackOfficeUser
            // 
            this.txtBackOfficeUser.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtBackOfficeUser.Name = "txtBackOfficeUser";
            this.txtBackOfficeUser.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6999993324279785D), Telerik.Reporting.Drawing.Unit.Cm(0.53990083932876587D));
            this.txtBackOfficeUser.Style.Font.Bold = false;
            this.txtBackOfficeUser.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txtBackOfficeUser.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtBackOfficeUser.Value = "= Fields.BACKOFFICE_USR";
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.60000014305114746D);
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.Style.Visible = false;
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D);
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            this.pageHeaderSection1.Style.Visible = false;
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.90000003576278687D);
            this.detail.Name = "detail";
            this.detail.Style.Visible = false;
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.5999991893768311D);
            this.pageFooterSection1.Name = "pageFooterSection1";
            this.pageFooterSection1.Style.Visible = false;
            // 
            // lblCashTotal
            // 
            this.lblCashTotal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.85999876260757446D));
            this.lblCashTotal.Name = "lblCashTotal";
            this.lblCashTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6999993324279785D), Telerik.Reporting.Drawing.Unit.Cm(0.53990083932876587D));
            this.lblCashTotal.Style.Font.Bold = true;
            this.lblCashTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.lblCashTotal.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.lblCashTotal.Value = "Total:";
            // 
            // txtCashTotal
            // 
            this.txtCashTotal.Format = "{0:C2}";
            this.txtCashTotal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.7002997398376465D), Telerik.Reporting.Drawing.Unit.Cm(0.85999876260757446D));
            this.txtCashTotal.Name = "txtCashTotal";
            this.txtCashTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.9998998641967773D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.txtCashTotal.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txtCashTotal.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtCashTotal.Value = "=IsNull(-Sum(Fields.DIST_AMOUNT+ Fields.DIST_VAT)/100,0)";
            // 
            // lblCashVAT
            // 
            this.lblCashVAT.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.3198980987071991D));
            this.lblCashVAT.Name = "lblCashVAT";
            this.lblCashVAT.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6999993324279785D), Telerik.Reporting.Drawing.Unit.Cm(0.53990083932876587D));
            this.lblCashVAT.Style.Font.Bold = true;
            this.lblCashVAT.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.lblCashVAT.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.lblCashVAT.Value = "IVA:";
            // 
            // txtCashVAT
            // 
            this.txtCashVAT.Format = "{0:C2}";
            this.txtCashVAT.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.7002997398376465D), Telerik.Reporting.Drawing.Unit.Cm(0.3198980987071991D));
            this.txtCashVAT.Name = "txtCashVAT";
            this.txtCashVAT.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.9998998641967773D), Telerik.Reporting.Drawing.Unit.Cm(0.53990042209625244D));
            this.txtCashVAT.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txtCashVAT.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtCashVAT.Value = "=IsNull(-Sum(Fields.DIST_VAT)/100,0)";
            // 
            // dsDetail
            // 
            this.dsDetail.CommandTimeout = 0;
            this.dsDetail.ConnectionString = "integraMobile.Reports.Properties.Settings.integraMobileReports";
            this.dsDetail.Name = "dsDetail";
            this.dsDetail.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@DateIniUTC", System.Data.DbType.DateTime, "= Parameters.DateIni.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@DateEndUTC", System.Data.DbType.DateTime, "= Parameters.DateEnd.Value")});
            this.dsDetail.SelectCommand = "dbo.Report_CASHRECHARGES_LIQUIDATIONDETAIL";
            this.dsDetail.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4599988460540772D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.lblCashVAT,
            this.txtCashVAT,
            this.txtCashTotal,
            this.lblCashTotal});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // LiquidationCashRechargesSub
            // 
            this.DataSource = this.dsDetail;
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.FDO_ID", Telerik.Reporting.FilterOperator.Equal, "= Parameters.FdoId.Value"));
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.INS_ID", Telerik.Reporting.FilterOperator.Equal, "= Parameters.InsId.Value"));
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.BACKOFFICE_USR"));
            group1.Name = "group";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1,
            this.reportFooterSection1});
            this.Name = "LiquidationCashRechargesSub";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "DateIni";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.Name = "DateEnd";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter3.Name = "FdoId";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter4.Name = "InsId";
            reportParameter4.Type = Telerik.Reporting.ReportParameterType.Integer;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(9.0000009536743164D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.SqlDataSource dsDetail;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.TextBox txtCashAmount;
        private Telerik.Reporting.TextBox txtBackOfficeUser;
        private Telerik.Reporting.TextBox lblCashTotal;
        private Telerik.Reporting.TextBox txtCashTotal;
        private Telerik.Reporting.TextBox lblCashVAT;
        private Telerik.Reporting.TextBox txtCashVAT;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
    }
}