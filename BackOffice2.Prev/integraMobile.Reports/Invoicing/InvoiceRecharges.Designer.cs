namespace integraMobile.Reports.Invoicing
{
    partial class InvoiceRecharges
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceRecharges));
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule2 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule3 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule4 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule5 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule6 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.lblUserName = new Telerik.Reporting.TextBox();
            this.lblDate = new Telerik.Reporting.TextBox();
            this.lblAmount = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.txtUser = new Telerik.Reporting.TextBox();
            this.txtDate = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.lblSumAmount = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.lblSumFEE = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.lblSumVAT = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.lblSumTotal = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.dsInvoiceRecharges = new Telerik.Reporting.SqlDataSource();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.lblTitle = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.3999993801116943D);
            this.groupFooterSection.Name = "groupFooterSection";
            this.groupFooterSection.Style.Visible = false;
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.7000001072883606D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.lblUserName,
            this.lblDate,
            this.lblAmount});
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.PrintOnEveryPage = true;
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5998997688293457D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblUserName.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblUserName, "lblUserName");
            // 
            // lblDate
            // 
            this.lblDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6002001762390137D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.6995992660522461D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblDate.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblDate, "lblDate");
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.300000190734863D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.699798583984375D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblAmount.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.lblAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.lblAmount, "lblAmount");
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.80000007152557373D);
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            this.pageHeaderSection1.PrintOnFirstPage = false;
            this.pageHeaderSection1.Style.Visible = false;
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.23622052371501923D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtUser,
            this.txtDate,
            this.textBox1});
            this.detail.Name = "detail";
            // 
            // txtUser
            // 
            this.txtUser.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5998997688293457D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            resources.ApplyResources(this.txtUser, "txtUser");
            // 
            // txtDate
            // 
            this.txtDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6002001762390137D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.6995992660522461D), Telerik.Reporting.Drawing.Unit.Cm(0.5998997688293457D));
            resources.ApplyResources(this.txtDate, "txtDate");
            // 
            // textBox1
            // 
            this.textBox1.Format = "{0:C2}";
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.300000190734863D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6997990608215332D), Telerik.Reporting.Drawing.Unit.Cm(0.5998997688293457D));
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox1, "textBox1");
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.3000001907348633D);
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // lblSumAmount
            // 
            this.lblSumAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.200201988220215D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.lblSumAmount.Name = "lblSumAmount";
            this.lblSumAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.099998950958252D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumAmount.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumAmount, "lblSumAmount");
            // 
            // textBox3
            // 
            this.textBox3.Format = "{0:C2}";
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.300400733947754D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.69959831237793D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // lblSumFEE
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.CUSPMR_FEE)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule1.Style.Visible = false;
            this.lblSumFEE.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.lblSumFEE.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.200201988220215D), Telerik.Reporting.Drawing.Unit.Cm(0.9001997709274292D));
            this.lblSumFEE.Name = "lblSumFEE";
            this.lblSumFEE.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.0997986793518066D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumFEE.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumFEE, "lblSumFEE");
            // 
            // textBox2
            // 
            formattingRule2.Filters.Add(new Telerik.Reporting.Filter("= Sum(Fields.CUSPMR_FEE)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule2.Style.Visible = false;
            this.textBox2.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule2});
            this.textBox2.Format = "{0:C2}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.300199508666992D), Telerik.Reporting.Drawing.Unit.Cm(0.900200605392456D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.69959831237793D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // lblSumVAT
            // 
            formattingRule3.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.CUSPMR_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule3.Style.Visible = false;
            this.lblSumVAT.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule3});
            this.lblSumVAT.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.200201988220215D), Telerik.Reporting.Drawing.Unit.Cm(1.5003997087478638D));
            this.lblSumVAT.Name = "lblSumVAT";
            this.lblSumVAT.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.0997986793518066D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumVAT.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumVAT, "lblSumVAT");
            // 
            // textBox4
            // 
            formattingRule4.Filters.Add(new Telerik.Reporting.Filter("= Sum(Fields.CUSPMR_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule4.Style.Visible = false;
            this.textBox4.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule4});
            this.textBox4.Format = "{0:C2}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.3006010055542D), Telerik.Reporting.Drawing.Unit.Cm(1.5003988742828369D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.69959831237793D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox4, "textBox4");
            // 
            // lblSumTotal
            // 
            formattingRule5.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.CUSPMR_FEE + Fields.CUSPMR_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule5.Style.Visible = false;
            this.lblSumTotal.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule5});
            this.lblSumTotal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.200201988220215D), Telerik.Reporting.Drawing.Unit.Cm(2.1005997657775879D));
            this.lblSumTotal.Name = "lblSumTotal";
            this.lblSumTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1002006530761719D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumTotal.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumTotal, "lblSumTotal");
            // 
            // textBox6
            // 
            formattingRule6.Filters.Add(new Telerik.Reporting.Filter("= Sum(Fields.CUSPMR_FEE + Fields.CUSPMR_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule6.Style.Visible = false;
            this.textBox6.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule6});
            this.textBox6.Format = "{0:C2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.3006010055542D), Telerik.Reporting.Drawing.Unit.Cm(2.1005988121032715D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.69959831237793D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox6, "textBox6");
            // 
            // dsInvoiceRecharges
            // 
            this.dsInvoiceRecharges.ConnectionString = "integraMobile.Reports.Properties.Settings.integraMobileReports";
            this.dsInvoiceRecharges.Name = "dsInvoiceRecharges";
            this.dsInvoiceRecharges.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@InvoiceId", System.Data.DbType.Decimal, "= Parameters.InvoiceId.Value")});
            this.dsInvoiceRecharges.SelectCommand = "dbo.Report_INVOICERECHARGES";
            this.dsInvoiceRecharges.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.7999999523162842D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox3,
            this.lblSumAmount,
            this.lblSumFEE,
            this.textBox2,
            this.lblSumVAT,
            this.textBox4,
            this.lblSumTotal,
            this.textBox6});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.99999994039535522D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.lblTitle});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.300302505493164D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.lblTitle.Style.Font.Bold = true;
            this.lblTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            resources.ApplyResources(this.lblTitle, "lblTitle");
            // 
            // InvoiceRecharges
            // 
            this.DataSource = this.dsInvoiceRecharges;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.CUSPMR_CUSINV_ID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.InvoiceId.Value"));
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.CUSPMR_CUSINV_ID"));
            group1.Name = "group";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1,
            this.reportFooterSection1,
            this.reportHeaderSection1});
            this.Name = "InvoiceRecharges";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowBlank = false;
            reportParameter1.Name = "InvoiceId";
            resources.ApplyResources(reportParameter1, "reportParameter1");
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "38";
            reportParameter2.Name = "CurrencyIsoCode";
            resources.ApplyResources(reportParameter2, "reportParameter2");
            reportParameter2.Value = "EUR";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.CUSPMR_DATE", Telerik.Reporting.SortDirection.Asc));
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(19.000200271606445D);
            this.ItemDataBinding += new System.EventHandler(this.Report_ItemDataBinding);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.SqlDataSource dsInvoiceRecharges;
        private Telerik.Reporting.TextBox lblUserName;
        private Telerik.Reporting.TextBox lblDate;
        private Telerik.Reporting.TextBox lblAmount;
        private Telerik.Reporting.TextBox txtUser;
        private Telerik.Reporting.TextBox txtDate;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox lblSumAmount;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox lblSumFEE;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox lblSumVAT;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox lblSumTotal;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.TextBox lblTitle;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
    }
}