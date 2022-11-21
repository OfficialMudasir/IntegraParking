namespace integraMobile.Reports.Invoicing
{
    partial class eysa_invoice
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eysa_invoice));
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.InstanceReportSource instanceReportSource1 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.Drawing.FormattingRule formattingRule2 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.InstanceReportSource instanceReportSource2 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.Drawing.FormattingRule formattingRule3 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.InstanceReportSource instanceReportSource3 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.Drawing.FormattingRule formattingRule4 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.InstanceReportSource instanceReportSource4 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.invoiceRecharges1 = new integraMobile.Reports.Invoicing.InvoiceRecharges();
            this.invoiceOperations1 = new integraMobile.Reports.Invoicing.InvoiceOperations();
            this.invoiceTicketPayments1 = new integraMobile.Reports.Invoicing.InvoiceTicketPayments();
            this.invoiceServiceCharges1 = new integraMobile.Reports.Invoicing.InvoiceServiceCharges();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.picLogoEysa = new Telerik.Reporting.PictureBox();
            this.txtCompanyName = new Telerik.Reporting.TextBox();
            this.txtCompanyInfo = new Telerik.Reporting.TextBox();
            this.panel1 = new Telerik.Reporting.Panel();
            this.txtCustomerName = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.txtCustomerNIF = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.txtDate = new Telerik.Reporting.TextBox();
            this.txtRef = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.srRecharges = new Telerik.Reporting.SubReport();
            this.srOperations = new Telerik.Reporting.SubReport();
            this.srTicketPayments = new Telerik.Reporting.SubReport();
            this.srServiceCharges = new Telerik.Reporting.SubReport();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.dsInvoices = new Telerik.Reporting.SqlDataSource();
            this.pictureBox2 = new Telerik.Reporting.PictureBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceRecharges1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceOperations1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceTicketPayments1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceServiceCharges1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // invoiceRecharges1
            // 
            this.invoiceRecharges1.Name = "InvoiceRecharges";
            // 
            // invoiceOperations1
            // 
            this.invoiceOperations1.Name = "InvoiceOperations";
            // 
            // invoiceTicketPayments1
            // 
            this.invoiceTicketPayments1.Name = "InvoiceTicketPayments";
            // 
            // invoiceServiceCharges1
            // 
            this.invoiceServiceCharges1.Name = "InvoiceServiceCharges";
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(7.6000003814697266D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.picLogoEysa,
            this.txtCompanyName,
            this.txtCompanyInfo,
            this.panel1,
            this.shape1,
            this.txtDate,
            this.txtRef,
            this.pictureBox2,
            this.pictureBox1});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // picLogoEysa
            // 
            this.picLogoEysa.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D), Telerik.Reporting.Drawing.Unit.Cm(0.7999994158744812D));
            this.picLogoEysa.MimeType = "image/png";
            this.picLogoEysa.Name = "picLogoEysa";
            this.picLogoEysa.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(2.5D));
            this.picLogoEysa.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.picLogoEysa.Value = ((object)(resources.GetObject("picLogoEysa.Value")));
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.9003000259399414D), Telerik.Reporting.Drawing.Unit.Cm(1.2998001575469971D));
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.79979944229126D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.txtCompanyName.Style.Font.Bold = true;
            this.txtCompanyName.Style.Font.Name = "Tahoma";
            this.txtCompanyName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txtCompanyName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.txtCompanyName, "txtCompanyName");
            // 
            // txtCompanyInfo
            // 
            this.txtCompanyInfo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.9003000259399414D), Telerik.Reporting.Drawing.Unit.Cm(2D));
            this.txtCompanyInfo.Name = "txtCompanyInfo";
            this.txtCompanyInfo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.79979944229126D), Telerik.Reporting.Drawing.Unit.Cm(1.2999993562698364D));
            this.txtCompanyInfo.Style.Font.Name = "Tahoma";
            this.txtCompanyInfo.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txtCompanyInfo.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            resources.ApplyResources(this.txtCompanyInfo, "txtCompanyInfo");
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtCustomerName,
            this.textBox1,
            this.txtCustomerNIF});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D), Telerik.Reporting.Drawing.Unit.Cm(4.1999998092651367D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.5001001358032227D), Telerik.Reporting.Drawing.Unit.Cm(2.5000002384185791D));
            this.panel1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4998998641967773D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.txtCustomerName.Style.Font.Bold = true;
            this.txtCustomerName.Style.Font.Name = "Tahoma";
            this.txtCustomerName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txtCustomerName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            resources.ApplyResources(this.txtCustomerName, "txtCustomerName");
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.700300395488739D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4998998641967773D), Telerik.Reporting.Drawing.Unit.Cm(1.099699854850769D));
            resources.ApplyResources(this.textBox1, "textBox1");
            // 
            // txtCustomerNIF
            // 
            this.txtCustomerNIF.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(1.8002005815505981D));
            this.txtCustomerNIF.Name = "txtCustomerNIF";
            this.txtCustomerNIF.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.4998989105224609D), Telerik.Reporting.Drawing.Unit.Cm(0.59979945421218872D));
            resources.ApplyResources(this.txtCustomerNIF, "txtCustomerNIF");
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.999798774719238D), Telerik.Reporting.Drawing.Unit.Cm(4.2000999450683594D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.NS);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.10000035166740418D), Telerik.Reporting.Drawing.Unit.Cm(1.40019953250885D));
            // 
            // txtDate
            // 
            this.txtDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.09999942779541D), Telerik.Reporting.Drawing.Unit.Cm(4.2000999450683594D));
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5199031829833984D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            resources.ApplyResources(this.txtDate, "txtDate");
            // 
            // txtRef
            // 
            this.txtRef.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.09999942779541D), Telerik.Reporting.Drawing.Unit.Cm(4.9003000259399414D));
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5199031829833984D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            resources.ApplyResources(this.txtRef, "txtRef");
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(2.5590546131134033D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.srRecharges,
            this.srOperations,
            this.srTicketPayments,
            this.srServiceCharges});
            this.detail.Name = "detail";
            // 
            // srRecharges
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=Fields.RECHARGES_COUNT", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule1.Style.Visible = false;
            this.srRecharges.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.srRecharges.KeepTogether = false;
            this.srRecharges.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.srRecharges.Name = "srRecharges";
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("InvoiceId", "=Parameters.InvoiceId.Value"));
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("CurrencyIsoCode", "=Fields.CUR_ISO_CODE"));
            instanceReportSource1.ReportDocument = this.invoiceRecharges1;
            this.srRecharges.ReportSource = instanceReportSource1;
            this.srRecharges.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.999799728393555D), Telerik.Reporting.Drawing.Unit.Cm(1.5000004768371582D));
            // 
            // srOperations
            // 
            formattingRule2.Filters.Add(new Telerik.Reporting.Filter("=Fields.OPERATIONS_COUNT", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule2.Style.Visible = false;
            this.srOperations.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule2});
            this.srOperations.KeepTogether = false;
            this.srOperations.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.5003007650375366D));
            this.srOperations.Name = "srOperations";
            instanceReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("InvoiceId", "=Fields.CUSINV_ID"));
            instanceReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("CurrencyIsoCode", "=Fields.CUR_ISO_CODE"));
            instanceReportSource2.ReportDocument = this.invoiceOperations1;
            this.srOperations.ReportSource = instanceReportSource2;
            this.srOperations.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.999898910522461D), Telerik.Reporting.Drawing.Unit.Cm(1.4996992349624634D));
            // 
            // srTicketPayments
            // 
            formattingRule3.Filters.Add(new Telerik.Reporting.Filter("=Fields.TICKET_PAYMENTS_COUNT", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule3.Style.Visible = false;
            this.srTicketPayments.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule3});
            this.srTicketPayments.KeepTogether = false;
            this.srTicketPayments.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(3.0002002716064453D));
            this.srTicketPayments.Name = "srTicketPayments";
            instanceReportSource3.Parameters.Add(new Telerik.Reporting.Parameter("InvoiceId", "=Fields.CUSINV_ID"));
            instanceReportSource3.Parameters.Add(new Telerik.Reporting.Parameter("CurrencyIsoCode", "=Fields.CUR_ISO_CODE"));
            instanceReportSource3.ReportDocument = this.invoiceTicketPayments1;
            this.srTicketPayments.ReportSource = instanceReportSource3;
            this.srTicketPayments.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.999898910522461D), Telerik.Reporting.Drawing.Unit.Cm(1.7998005151748657D));
            // 
            // srServiceCharges
            // 
            formattingRule4.Filters.Add(new Telerik.Reporting.Filter("=Fields.SERVICE_CHARGES_COUNT", Telerik.Reporting.FilterOperator.Equal, "0"));
            formattingRule4.Style.Visible = false;
            this.srServiceCharges.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule4});
            this.srServiceCharges.KeepTogether = false;
            this.srServiceCharges.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(4.8002018928527832D));
            this.srServiceCharges.Name = "srServiceCharges";
            instanceReportSource4.Parameters.Add(new Telerik.Reporting.Parameter("InvoiceId", "=Parameters.InvoiceId.Value"));
            instanceReportSource4.Parameters.Add(new Telerik.Reporting.Parameter("CurrencyIsoCode", "=Fields.CUR_ISO_CODE"));
            instanceReportSource4.ReportDocument = this.invoiceServiceCharges1;
            this.srServiceCharges.ReportSource = instanceReportSource4;
            this.srServiceCharges.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.999799728393555D), Telerik.Reporting.Drawing.Unit.Cm(1.5997973680496216D));
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4199988842010498D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox3});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9719363788608462E-05D), Telerik.Reporting.Drawing.Unit.Cm(0.29989930987358093D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.999799728393555D), Telerik.Reporting.Drawing.Unit.Cm(0.51989883184432983D));
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(5D);
            resources.ApplyResources(this.textBox2, "textBox2");
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.499898910522461D), Telerik.Reporting.Drawing.Unit.Cm(0.81999838352203369D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.5000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(0.60000050067901611D));
            resources.ApplyResources(this.textBox3, "textBox3");
            // 
            // dsInvoices
            // 
            this.dsInvoices.ConnectionString = "integraMobile.Reports.Properties.Settings.integraMobileReports";
            this.dsInvoices.Name = "dsInvoices";
            this.dsInvoices.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@InvoiceId", System.Data.DbType.Decimal, "= Parameters.InvoiceId.Value")});
            this.dsInvoices.SelectCommand = "dbo.Report_INVOICE";
            this.dsInvoices.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(1.6999999284744263D));
            this.pictureBox2.MimeType = "image/png";
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9997987747192383D), Telerik.Reporting.Drawing.Unit.Cm(2.0000004768371582D));
            this.pictureBox2.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox2.Value = ((object)(resources.GetObject("pictureBox2.Value")));
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.5D), Telerik.Reporting.Drawing.Unit.Cm(1.6999999284744263D));
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.219904899597168D), Telerik.Reporting.Drawing.Unit.Cm(2.0000002384185791D));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // eysa_invoice
            // 
            this.DataSource = this.dsInvoices;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.CUSINV_ID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.InvoiceId.Value"));
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1});
            this.Name = "eysa_invoice";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowBlank = false;
            reportParameter1.Name = "InvoiceId";
            resources.ApplyResources(reportParameter1, "reportParameter1");
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "38";
            reportParameter1.Visible = true;
            this.ReportParameters.Add(reportParameter1);
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
            ((System.ComponentModel.ISupportInitialize)(this.invoiceRecharges1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceOperations1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceTicketPayments1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceServiceCharges1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.SqlDataSource dsInvoices;
        private Telerik.Reporting.PictureBox picLogoEysa;
        private Telerik.Reporting.TextBox txtCompanyName;
        private Telerik.Reporting.TextBox txtCompanyInfo;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.TextBox txtCustomerName;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox txtCustomerNIF;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.TextBox txtDate;
        private Telerik.Reporting.TextBox txtRef;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.SubReport srRecharges;
        private InvoiceRecharges invoiceRecharges1;
        private Telerik.Reporting.SubReport srOperations;
        private InvoiceOperations invoiceOperations1;
        private Telerik.Reporting.SubReport srTicketPayments;
        private InvoiceTicketPayments invoiceTicketPayments1;
        private Telerik.Reporting.SubReport srServiceCharges;
        private InvoiceServiceCharges invoiceServiceCharges1;
        private Telerik.Reporting.PictureBox pictureBox2;
        private Telerik.Reporting.PictureBox pictureBox1;
    }
}