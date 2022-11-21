namespace integraMobile.Reports.Invoicing
{
    partial class InvoiceTicketPayments
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceTicketPayments));
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
            this.lblTicketNum = new Telerik.Reporting.TextBox();
            this.lblPlate = new Telerik.Reporting.TextBox();
            this.lblDate = new Telerik.Reporting.TextBox();
            this.lblAmount = new Telerik.Reporting.TextBox();
            this.lblInstallation = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.txtPlate = new Telerik.Reporting.TextBox();
            this.txtDate = new Telerik.Reporting.TextBox();
            this.txtAmount = new Telerik.Reporting.TextBox();
            this.txtTicketNum = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.lblTitle = new Telerik.Reporting.TextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.lblSumAmount = new Telerik.Reporting.TextBox();
            this.lblSumFEE = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.lblSumVAT = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.lblSumTotal = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.dsInvoiceTicketPayments = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.59990084171295166D);
            this.groupFooterSection.Name = "groupFooterSection";
            this.groupFooterSection.Style.Visible = false;
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.69999969005584717D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.lblTicketNum,
            this.lblPlate,
            this.lblDate,
            this.lblAmount,
            this.lblInstallation});
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.PrintOnEveryPage = true;
            // 
            // lblTicketNum
            // 
            this.lblTicketNum.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblTicketNum.Name = "lblTicketNum";
            this.lblTicketNum.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.0766901969909668D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblTicketNum.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblTicketNum, "lblTicketNum");
            // 
            // lblPlate
            // 
            this.lblPlate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.076991081237793D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblPlate.Name = "lblPlate";
            this.lblPlate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.7228085994720459D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblPlate.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblPlate, "lblPlate");
            // 
            // lblDate
            // 
            this.lblDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.8228087425231934D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3771908283233643D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblDate.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblDate, "lblDate");
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.322809219360352D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6756319999694824D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblAmount.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.lblAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.lblAmount, "lblAmount");
            // 
            // lblInstallation
            // 
            this.lblInstallation.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.200199127197266D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.lblInstallation.Name = "lblInstallation";
            this.lblInstallation.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.1224098205566406D), Telerik.Reporting.Drawing.Unit.Cm(0.59990018606185913D));
            this.lblInstallation.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            resources.ApplyResources(this.lblInstallation, "lblInstallation");
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
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.23625977337360382D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtPlate,
            this.txtDate,
            this.txtAmount,
            this.txtTicketNum,
            this.textBox1});
            this.detail.Name = "detail";
            // 
            // txtPlate
            // 
            this.txtPlate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.076991081237793D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtPlate.Name = "txtPlate";
            this.txtPlate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.722808837890625D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            resources.ApplyResources(this.txtPlate, "txtPlate");
            // 
            // txtDate
            // 
            this.txtDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3999991416931152D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            resources.ApplyResources(this.txtDate, "txtDate");
            // 
            // txtAmount
            // 
            this.txtAmount.Format = "{0:C2}";
            this.txtAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.322807312011719D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6771917343139648D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.txtAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.txtAmount, "txtAmount");
            // 
            // txtTicketNum
            // 
            this.txtTicketNum.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.txtTicketNum.Name = "txtTicketNum";
            this.txtTicketNum.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.0766901969909668D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            resources.ApplyResources(this.txtTicketNum, "txtTicketNum");
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.200199127197266D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.1224095821380615D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            resources.ApplyResources(this.textBox1, "textBox1");
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
            this.lblTitle.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.9998993873596191D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.lblTitle.Style.Font.Bold = true;
            this.lblTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            resources.ApplyResources(this.lblTitle, "lblTitle");
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3.0998995304107666D);
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
            // textBox3
            // 
            this.textBox3.Format = "{0:C2}";
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.888957977294922D), Telerik.Reporting.Drawing.Unit.Cm(0.39989966154098511D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1094822883605957D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // lblSumAmount
            // 
            this.lblSumAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.100000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0.39989966154098511D));
            this.lblSumAmount.Name = "lblSumAmount";
            this.lblSumAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumAmount.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumAmount, "lblSumAmount");
            // 
            // lblSumFEE
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.TIPA_FEE)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule1.Style.Visible = false;
            this.lblSumFEE.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.lblSumFEE.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.100000381469727D), Telerik.Reporting.Drawing.Unit.Cm(1.0084413290023804D));
            this.lblSumFEE.Name = "lblSumFEE";
            this.lblSumFEE.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumFEE.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumFEE, "lblSumFEE");
            // 
            // textBox2
            // 
            formattingRule2.Filters.Add(new Telerik.Reporting.Filter("= Sum(Fields.TIPA_FEE)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule2.Style.Visible = false;
            this.textBox2.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule2});
            this.textBox2.Format = "{0:C2}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.888957977294922D), Telerik.Reporting.Drawing.Unit.Cm(1.0084413290023804D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1094827651977539D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // lblSumVAT
            // 
            formattingRule3.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.TIPA_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule3.Style.Visible = false;
            this.lblSumVAT.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule3});
            this.lblSumVAT.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.100000381469727D), Telerik.Reporting.Drawing.Unit.Cm(1.6086421012878418D));
            this.lblSumVAT.Name = "lblSumVAT";
            this.lblSumVAT.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumVAT.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumVAT, "lblSumVAT");
            // 
            // textBox4
            // 
            formattingRule4.Filters.Add(new Telerik.Reporting.Filter("= Sum(Fields.TIPA_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule4.Style.Visible = false;
            this.textBox4.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule4});
            this.textBox4.Format = "{0:C2}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.874798774719238D), Telerik.Reporting.Drawing.Unit.Cm(1.6086424589157105D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1236419677734375D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox4, "textBox4");
            // 
            // lblSumTotal
            // 
            formattingRule5.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.TIPA_FEE + Fields.TIPA_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule5.Style.Visible = false;
            this.lblSumTotal.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule5});
            this.lblSumTotal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.100000381469727D), Telerik.Reporting.Drawing.Unit.Cm(2.2088427543640137D));
            this.lblSumTotal.Name = "lblSumTotal";
            this.lblSumTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7745976448059082D), Telerik.Reporting.Drawing.Unit.Cm(0.60000056028366089D));
            this.lblSumTotal.Style.Font.Bold = true;
            resources.ApplyResources(this.lblSumTotal, "lblSumTotal");
            // 
            // textBox6
            // 
            formattingRule6.Filters.Add(new Telerik.Reporting.Filter("=Sum(Fields.TIPA_FEE + Fields.TIPA_VAT)", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule6.Style.Visible = false;
            this.textBox6.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule6});
            this.textBox6.Format = "{0:C2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.901558876037598D), Telerik.Reporting.Drawing.Unit.Cm(2.20884370803833D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.09688138961792D), Telerik.Reporting.Drawing.Unit.Cm(0.60000091791152954D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.textBox6, "textBox6");
            // 
            // dsInvoiceTicketPayments
            // 
            this.dsInvoiceTicketPayments.ConnectionString = "integraMobile.Reports.Properties.Settings.integraMobileReports";
            this.dsInvoiceTicketPayments.Name = "dsInvoiceTicketPayments";
            this.dsInvoiceTicketPayments.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@InvoiceId", System.Data.DbType.Decimal, "= Parameters.InvoiceId.Value")});
            this.dsInvoiceTicketPayments.SelectCommand = "dbo.Report_INVOICETICKETPAYMENTS";
            this.dsInvoiceTicketPayments.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // InvoiceTicketPayments
            // 
            this.DataSource = this.dsInvoiceTicketPayments;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.TIPA_CUSINV_ID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.InvoiceId.Value"));
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.TIPA_CUSINV_ID"));
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
            this.Name = "InvoiceTicketPayments";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowBlank = false;
            reportParameter1.Name = "InvoiceId";
            resources.ApplyResources(reportParameter1, "reportParameter1");
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "38";
            reportParameter2.AllowBlank = false;
            reportParameter2.Name = "CurrencyIsoCode";
            resources.ApplyResources(reportParameter2, "reportParameter2");
            reportParameter2.Value = "EUR";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(19D);
            this.ItemDataBinding += new System.EventHandler(this.Report_ItemDataBinding);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox lblAmount;
        private Telerik.Reporting.TextBox lblDate;
        private Telerik.Reporting.TextBox lblPlate;
        private Telerik.Reporting.TextBox lblTitle;
        private Telerik.Reporting.SqlDataSource dsInvoiceTicketPayments;
        private Telerik.Reporting.TextBox txtPlate;
        private Telerik.Reporting.TextBox txtDate;
        private Telerik.Reporting.TextBox txtAmount;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox lblSumAmount;
        private Telerik.Reporting.TextBox lblSumFEE;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox lblSumVAT;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox lblSumTotal;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox lblTicketNum;
        private Telerik.Reporting.TextBox txtTicketNum;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.TextBox lblInstallation;
        private Telerik.Reporting.TextBox textBox1;
    }
}