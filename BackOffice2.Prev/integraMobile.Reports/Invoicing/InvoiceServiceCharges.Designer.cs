namespace integraMobile.Reports.Invoicing
{
    partial class InvoiceServiceCharges
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceServiceCharges));
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule2 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule3 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule4 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule5 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule6 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.FormattingRule formattingRule7 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.lblUserName = new Telerik.Reporting.TextBox();
            this.lblDate = new Telerik.Reporting.TextBox();
            this.lblServiceType = new Telerik.Reporting.TextBox();
            this.lblAmount = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.txtUserName = new Telerik.Reporting.TextBox();
            this.txtDate = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.dsInvoiceServiceCharges = new Telerik.Reporting.SqlDataSource();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.lblTitle = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.lblSumAmount = new Telerik.Reporting.TextBox();
            this.lblSumFEE = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.lblSumVAT = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.lblSumTotal = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D);
            this.groupFooterSection.Name = "groupFooterSection";
            this.groupFooterSection.Style.Visible = false;
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.79979979991912842D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.lblUserName,
            this.lblDate,
            this.lblServiceType,
            this.lblAmount});
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.PrintOnEveryPage = true;
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5998997688293457D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblUserName.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblUserName, "lblUserName");
            // 
            // lblDate
            // 
            this.lblDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.600100040435791D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.399899959564209D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblDate.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblDate, "lblDate");
            // 
            // lblServiceType
            // 
            this.lblServiceType.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.0002012252807617D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblServiceType.Name = "lblServiceType";
            this.lblServiceType.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.2870974540710449D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblServiceType.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblServiceType, "lblServiceType");
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.300200462341309D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.699798583984375D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblAmount.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.lblAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.lblAmount, "lblAmount");
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D);
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            this.pageHeaderSection1.PrintOnFirstPage = false;
            this.pageHeaderSection1.Style.Visible = false;
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.23622052371501923D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtUserName,
            this.txtDate,
            this.textBox1,
            this.textBox2});
            this.detail.Name = "detail";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.599799633026123D), Telerik.Reporting.Drawing.Unit.Cm(0.5999000072479248D));
            resources.ApplyResources(this.txtUserName, "txtUserName");
            // 
            // txtDate
            // 
            this.txtDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6001019477844238D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3998994827270508D), Telerik.Reporting.Drawing.Unit.Cm(0.5999000072479248D));
            resources.ApplyResources(this.txtDate, "txtDate");
            // 
            // textBox1
            // 
            this.textBox1.Format = "{0:C2}";
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.28749942779541D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6997990608215332D), Telerik.Reporting.Drawing.Unit.Cm(0.5999000072479248D));
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox1, "textBox1");
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.0002012252807617D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.2870988845825195D), Telerik.Reporting.Drawing.Unit.Cm(0.5999000072479248D));
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // dsInvoiceServiceCharges
            // 
            this.dsInvoiceServiceCharges.ConnectionString = "integraMobile.Reports.Properties.Settings.integraMobileReports";
            this.dsInvoiceServiceCharges.Name = "dsInvoiceServiceCharges";
            this.dsInvoiceServiceCharges.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@InvoiceId", System.Data.DbType.Decimal, "= Parameters.InvoiceId.Value")});
            this.dsInvoiceServiceCharges.SelectCommand = "dbo.Report_INVOICESERVICECHARGES";
            this.dsInvoiceServiceCharges.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.lblTitle});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.9998993873596191D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.lblTitle.Style.Font.Bold = true;
            this.lblTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            resources.ApplyResources(this.lblTitle, "lblTitle");
            // 
            // reportFooterSection1
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("= Count(Fields.SECH_ID)", Telerik.Reporting.FilterOperator.LessOrEqual, "1"));
            formattingRule1.Style.Visible = false;
            this.reportFooterSection1.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3.3000004291534424D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox3,
            this.lblSumAmount,
            this.lblSumFEE,
            this.textBox4,
            this.lblSumVAT,
            this.textBox5,
            this.lblSumTotal,
            this.textBox7});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // textBox3
            // 
            this.textBox3.Format = "{0:C2}";
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.788957595825195D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.12520170211792D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // lblSumAmount
            // 
            this.lblSumAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.lblSumAmount.Name = "lblSumAmount";
            this.lblSumAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumAmount.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumAmount, "lblSumAmount");
            // 
            // lblSumFEE
            // 
            formattingRule2.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.SECH_FEE)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule2.Style.Visible = false;
            this.lblSumFEE.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule2});
            this.lblSumFEE.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10D), Telerik.Reporting.Drawing.Unit.Cm(1.2085421085357666D));
            this.lblSumFEE.Name = "lblSumFEE";
            this.lblSumFEE.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumFEE.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumFEE, "lblSumFEE");
            // 
            // textBox4
            // 
            formattingRule3.Filters.Add(new Telerik.Reporting.Filter("= Sum(Fields.SECH_FEE)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule3.Style.Visible = false;
            this.textBox4.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule3});
            this.textBox4.Format = "{0:C2}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.801660537719727D), Telerik.Reporting.Drawing.Unit.Cm(1.2168827056884766D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1124992370605469D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox4, "textBox4");
            // 
            // lblSumVAT
            // 
            formattingRule4.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.SECH_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule4.Style.Visible = false;
            this.lblSumVAT.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule4});
            this.lblSumVAT.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10D), Telerik.Reporting.Drawing.Unit.Cm(1.8170838356018066D));
            this.lblSumVAT.Name = "lblSumVAT";
            this.lblSumVAT.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumVAT.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumVAT, "lblSumVAT");
            // 
            // textBox5
            // 
            formattingRule5.Filters.Add(new Telerik.Reporting.Filter("= Sum(Fields.SECH_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule5.Style.Visible = false;
            this.textBox5.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule5});
            this.textBox5.Format = "{0:C2}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.788957595825195D), Telerik.Reporting.Drawing.Unit.Cm(1.8170838356018066D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.12520170211792D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox5, "textBox5");
            // 
            // lblSumTotal
            // 
            formattingRule6.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.SECH_FEE + Fields.SECH_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule6.Style.Visible = false;
            this.lblSumTotal.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule6});
            this.lblSumTotal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10D), Telerik.Reporting.Drawing.Unit.Cm(2.4172849655151367D));
            this.lblSumTotal.Name = "lblSumTotal";
            this.lblSumTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumTotal.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumTotal, "lblSumTotal");
            // 
            // textBox7
            // 
            formattingRule7.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.SECH_FEE + Fields.SECH_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule7.Style.Visible = false;
            this.textBox7.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule7});
            this.textBox7.Format = "{0:C2}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.801559448242188D), Telerik.Reporting.Drawing.Unit.Cm(2.4172849655151367D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1126008033752441D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox7, "textBox7");
            // 
            // InvoiceServiceCharges
            // 
            this.DataSource = this.dsInvoiceServiceCharges;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.SECH_CUSINV_ID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.InvoiceId.Value"));
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.SECH_CUSINV_ID"));
            group1.Name = "group";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.pageHeaderSection1,
            this.detail,
            this.reportHeaderSection1,
            this.reportFooterSection1});
            this.Name = "InvoiceServiceCharges";
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
            reportParameter3.AllowBlank = false;
            reportParameter3.Name = "LanguageCulture";
            resources.ApplyResources(reportParameter3, "reportParameter3");
            reportParameter3.Value = "es-ES";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(18.999898910522461D);
            this.ItemDataBinding += new System.EventHandler(this.Report_ItemDataBinding);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SqlDataSource dsInvoiceServiceCharges;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox lblTitle;
        private Telerik.Reporting.TextBox lblAmount;
        private Telerik.Reporting.TextBox lblDate;
        private Telerik.Reporting.TextBox lblUserName;
        private Telerik.Reporting.TextBox lblServiceType;
        private Telerik.Reporting.TextBox txtUserName;
        private Telerik.Reporting.TextBox txtDate;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox lblSumAmount;
        private Telerik.Reporting.TextBox lblSumFEE;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox lblSumVAT;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox lblSumTotal;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
    }
}