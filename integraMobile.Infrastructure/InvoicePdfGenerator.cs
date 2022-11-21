using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace integraMobile.Infrastructure.Invoicing
{
    public class InvoicePdfGenerator : PdfGenerator
    {
        #region Declarations

        private string _templateTotalPdf = "factura.pdf";
        private string _templateNoTotalPdf = "facturaNoTotal.pdf";
        private string _generatedPdf = "Invoice_{0}.pdf";

        private IInvoiceData _data = null;

        private bool _testMode = false;

        #endregion

        #region Constructor

        public InvoicePdfGenerator(string workPath)
            : base(workPath)
        {
            SetFilePaths();
        }

        public InvoicePdfGenerator(string workPath, string templateFilename, string templateNoTotalFilename, string generatedPdfFilename)
            : base(workPath)
        {
            _templateTotalPdf = templateFilename;
            _templateNoTotalPdf = templateNoTotalFilename;
            _generatedPdf = generatedPdfFilename;
            SetFilePaths();
        }

        #endregion

        #region Properties

        public bool TestMode
        {
            get
            {
                return _testMode;
            }
            set
            {
                _testMode = value;
                if (_testMode)
                {
                    _data = new InvoiceDataTest();
                }
            }
        }

        public IInvoiceData Data
        {
            set
            {
                _data = value;
            }
        }

        public string generatedPDFPath()
        {
            return Path.Combine(_workPath, _generatedPdf);
        }

        #endregion

        #region Methods

        private void SetFilePaths()
        {
            _templateTotalPdf = Path.Combine(_workPath, _templateTotalPdf);
            _templateNoTotalPdf = Path.Combine(_workPath, _templateNoTotalPdf);
        }

        public override bool generatePdf()
        {
            if (_data != null)
            {
                refreshTmpStamp();

                string newPdf = Path.Combine(_workPath, _generatedPdf);

                try
                {
                    // Calculate number of pages
                    List<int> lstPageRows = CalculateNumPages();
                    int iNumPages = lstPageRows.Count;

                    PdfReader pdfReader;
                    PdfStamper pdfStamper;
                    AcroFields pdfFormFields;
                    int iRowIndex = 0;

                    for (int iPage = 0; iPage < iNumPages; iPage++)
                    {
                        using (FileStream stamperStream = new FileStream(iNumPages == 1 ? String.Format(newPdf, TmpStamp) : String.Format(TmpPdf, TmpStamp, iPage.ToString()), FileMode.Create))
                        {
                            pdfReader = new PdfReader((iPage == (iNumPages - 1)) ? _templateTotalPdf : _templateNoTotalPdf);

                            pdfStamper = new PdfStamper(pdfReader, stamperStream);

                            // set form pdfFormFields
                            pdfFormFields = pdfStamper.AcroFields;
                            pdfFormFields.SetField("fCompanyName", _data.CompanyName);
                            pdfFormFields.SetField("fCompanyInfo", _data.CompanyInfo);
                            pdfFormFields.SetField("fCustomerName", _data.CustomerName);
                            pdfFormFields.SetField("fCustomerInfo", _data.CustomerInfo);
                            pdfFormFields.SetField("fNIF", _data.NIF);
                            pdfFormFields.SetField("fPost", _data.Post);
                            pdfFormFields.SetField("fDate", _data.Date);
                            pdfFormFields.SetField("fRef", _data.Ref);
                            pdfFormFields.SetField("fContract", _data.Contract);
                            pdfFormFields.SetField("fInvoiceNum", _data.InvoiceNum);
                            pdfFormFields.SetField("fTotalBase", _data.TotalBase);
                            pdfFormFields.SetField("fTotalIVA", _data.TotalIVA);
                            pdfFormFields.SetField("fTotal", _data.Total);
                            pdfFormFields.SetField("fPageNum", String.Format("{0}/{1}", iPage + 1, iNumPages));
                            pdfFormFields.SetField("fLabelNIF", _data.LabelNIF);
                            pdfFormFields.SetField("fLabelPost", _data.LabelPost);
                            pdfFormFields.SetField("fLabelDate", _data.LabelDate);
                            pdfFormFields.SetField("fLabelRef", _data.LabelRef);
                            pdfFormFields.SetField("fLabelContract", _data.LabelContract);
                            pdfFormFields.SetField("fLabelInvoiceNum", _data.LabelInvoiceNum);
                            pdfFormFields.SetField("fLabelTotalBase", _data.LabelTotalBase);
                            pdfFormFields.SetField("fLabelTotalIVA", _data.LabelTotalIVA);
                            pdfFormFields.SetField("fLabelTotal", _data.LabelTotal);
                            pdfFormFields.SetField("fLabelLineUnits", _data.LabelLineUnits);
                            pdfFormFields.SetField("fLabelLineDescription", _data.LabelLineDescription);
                            pdfFormFields.SetField("fLabelLinePrice", _data.LabelLinePrice);
                            pdfFormFields.SetField("fLabelLineAmount", _data.LabelLineAmount);
                            pdfFormFields.SetField("fLabelFooter", _data.LabelFooter);

                            // Insert lines
                            var cb = pdfStamper.GetOverContent(1);
                            var ct = new ColumnText(cb);
                            ct.Alignment = Element.ALIGN_CENTER;
                            ct.SetSimpleColumn(45, 45, PageSize.A4.Width - 40, PageSize.A4.Height - 320);
                            PdfPTable table = createDetailsTable(iRowIndex, lstPageRows[iPage]);
                            iRowIndex += lstPageRows[iPage];
                            ct.AddElement(table);
                            ct.Go();

                            // flatten the form to remove editting options, set it to false
                            // to leave the form open to subsequent manual edits
                            pdfStamper.FormFlattening = true;

                            // close the pdf
                            pdfStamper.Close();
                            pdfReader.Close();
                        }
                    }

                    if (iNumPages > 1)
                    {

                        using (FileStream totalPdfStream = new FileStream(_templateTotalPdf, FileMode.Open))
                        using (FileStream newPdfStream = new FileStream(String.Format(newPdf, TmpStamp), FileMode.Create))
                        {
                            PdfReader pdfReaderNewPdf = new PdfReader(totalPdfStream);
                            using (Document document = new Document(pdfReaderNewPdf.GetPageSizeWithRotation(1)))
                            {

                                PdfCopy pdfCopy = new PdfCopy(document, newPdfStream);

                                document.Open();

                                for (int iPage = 0; iPage < iNumPages; iPage++)
                                {
                                    using (FileStream tmpStream = new FileStream(String.Format(TmpPdf, TmpStamp, iPage.ToString()), FileMode.Open))
                                    {
                                        pdfReader = new PdfReader(tmpStream);
                                        for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                                            pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, i));
                                        pdfReader.Close();
                                    }
                                }
                            }
                            pdfReaderNewPdf.Close();
                        }


                    }

                    // Delete tmp files
                    for (int iPage = 0; iPage < iNumPages; iPage++)
                    {
                        File.Delete(String.Format(TmpPdf, TmpStamp, iPage.ToString()));
                    }

                    _generatedPdfFilename = String.Format(newPdf, TmpStamp);
                    _pdfGenerated = true;

                }
                catch (Exception)
                {
                    _pdfGenerated = false;
                }

            }
            else
                _pdfGenerated = false;

            return _pdfGenerated;

        }

        private PdfPTable createDetailsTable(int iRowIndex, int iNumRows)
        {

            iTextSharp.text.Font fontTable = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 9, iTextSharp.text.Font.NORMAL);

            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            float[] widths = new float[] { 23f, 65f, 18f, 21f, 3f };
            table.SetWidths(widths);

            PdfPCell cell;
            int iLineIndex;

            IInvoiceLineData _line;

            for (int iRow = 0; iRow < iNumRows; iRow++)
            {
                iLineIndex = iRowIndex + iRow;

                _line = _data.Lines[iLineIndex];

                cell = new PdfPCell(new Phrase(_line.Units, fontTable));
                cell.BorderWidth = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(_line.Description, fontTable));
                cell.BorderWidth = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(_line.Price, fontTable));
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 10.0f;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(_line.Amount, fontTable));
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.PaddingRight = 0.0f;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(" ", fontTable));
                cell.BorderWidth = 0;
                table.AddCell(cell);
            }

            return table;
        }

        private List<int> CalculateNumPages()
        {
            List<int> lstPageRows = new List<int>();

            int iLinesNum = _data.Lines.Count;

            if (iLinesNum > 0)
            {
                // Calculate number of pages checking table height
                using (FileStream stamperStream = new FileStream(String.Format(TmpPdf, TmpStamp, 0), FileMode.Create))
                {
                    PdfReader pdfReader = new PdfReader(_templateTotalPdf);

                    PdfStamper pdfStamper = new PdfStamper(pdfReader, stamperStream);

                    var cb = pdfStamper.GetOverContent(1);

                    int iRowIndex = 0;
                    float fTableHeight;
                    int iPageRows;

                    while (iRowIndex < iLinesNum)
                    {
                        fTableHeight = 999;
                        iPageRows = ((iRowIndex + 30) < iLinesNum ? 30 : iLinesNum - iRowIndex) + 1;
                        while (fTableHeight > 390)
                        {
                            iPageRows -= 1;

                            var ct = new ColumnText(cb);
                            ct.Alignment = Element.ALIGN_CENTER;
                            ct.SetSimpleColumn(45, 45, PageSize.A4.Width - 40, PageSize.A4.Height - 320);
                            PdfPTable table = createDetailsTable(iRowIndex, iPageRows);
                            ct.AddElement(table);
                            ct.Go(true);
                            table.CalculateHeights();
                            fTableHeight = table.TotalHeight;

                        }
                        lstPageRows.Add(iPageRows);
                        iRowIndex += iPageRows;
                    }

                    pdfStamper.FormFlattening = true;

                    // close the pdf
                    pdfStamper.Close();
                    pdfReader.Close();
                }

            }
            else
            {
                lstPageRows.Add(0);
            }

            return lstPageRows;
        }

        #endregion

        private class InvoiceDataTest : IInvoiceData
        {

            private List<InvoiceLineDataTest> _lines = new List<InvoiceLineDataTest>()
            {
                new InvoiceLineDataTest("1", "Concepto descipción\n\ndetail1\ndetail2", "94.256,98 €", "94.256,98 €"),
                new InvoiceLineDataTest("2", "Concepto descipción\n\ndetail1","94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("2", "Concepto descipción\ndetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("4", "Concepto descipción", "256,98 €", "513,96 €"),
                new InvoiceLineDataTest("10", "Concepto descipción\n\ndetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("10", "Concepto descipción\n\nDetail1\nDetail2", "256 €", "256 €"),
                new InvoiceLineDataTest("120", "Concepto descipción", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("120", "Concepto descipción\nDetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("120", "Concepto descipción\nDetail1\nDetail2", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("1234", "Concepto descipción", "94.256,98 €", "188.513,96 €")                

                /*,new InvoiceLineDataTest("1", "Concepto descipción\n\ndetail1\ndetail2", "94.256,98 €", "94.256,98 €"),
                new InvoiceLineDataTest("2", "Concepto descipción\n\ndetail1","94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("2", "Concepto descipción\ndetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("4", "Concepto descipción", "256,98 €", "513,96 €"),
                new InvoiceLineDataTest("10", "Concepto descipción\n\ndetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("10", "Concepto descipción\n\nDetail1\nDetail2", "256 €", "256 €"),
                new InvoiceLineDataTest("120", "Concepto descipción", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("120", "Concepto descipción\nDetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("120", "Concepto descipción\nDetail1\nDetail2", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("1234", "Concepto descipción", "94.256,98 €", "188.513,96 €")                

                ,new InvoiceLineDataTest("1", "Concepto descipción\n\ndetail1\ndetail2", "94.256,98 €", "94.256,98 €"),
                new InvoiceLineDataTest("2", "Concepto descipción\n\ndetail1","94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("2", "Concepto descipción\ndetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("4", "Concepto descipción", "256,98 €", "513,96 €"),
                new InvoiceLineDataTest("10", "Concepto descipción\n\ndetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("10", "Concepto descipción\n\nDetail1\nDetail2", "256 €", "256 €"),
                new InvoiceLineDataTest("120", "Concepto descipción", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("120", "Concepto descipción\nDetail1", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("120", "Concepto descipción\nDetail1\nDetail2", "94.256,98 €", "188.513,96 €"),
                new InvoiceLineDataTest("1234", "Concepto descipción", "94.256,98 €", "188.513,96 €")*/

            };

            public string CompanyName { set {} get { return "ESTACIONAMIENTOS Y SERVICIOS, S.A.U."; } }
            public string CompanyInfo { set {} get { return "N.I.F. A28385458\nc/ Barahundillo, 4-3001 Murcia\nt.(+34) 968 32 97 33 - f.(+34) 968 21 51 89"; } }
            public string CustomerName { set {} get { return "AYUNTAMIENTO DE ..."; } }
            public string CustomerInfo { set {} get { return "Seguridad Ciudadana, Centro Histórico y vía Pública\nc/ San Miguel, 8 Planta Baja - 30201 Catagena"; } }
            public string NIF { set {} get { return "12345679F"; } }
            public string Post { set {} get { return "Cargo"; } }
            public string Date { set {} get { return "28/08/2013"; } }
            public string Ref { set {} get { return "WE3454"; } }
            public string Contract { set {} get { return "232343"; } }
            public string InvoiceNum { set {} get { return "34675"; } }
            public string TotalBase { set {} get { return "94.427,97 €"; } }
            public string TotalIVA { set {} get { return "18.829,88 €"; } }
            public string Total { set { } get { return "114.257,85 €"; } }
            public List<IInvoiceLineData> Lines
            {
                get
                {
                    return new List<IInvoiceLineData>(_lines.Cast<IInvoiceLineData>());
                }
            }


            public bool AddLine(IInvoiceLineData line)
            {
                return false;
            }

            public string LabelNIF { set {} get { return "DNI/NIF :"; } }
            public string LabelPost { set {} get { return "Con Cargo:"; } }
            public string LabelDate { set {} get { return "Fecha:"; } }
            public string LabelRef { set {} get { return "s/Ref:"; } }
            public string LabelContract { set {} get { return "s/Contrato:"; } }
            public string LabelInvoiceNum { set {} get { return "Factura nº:"; } }
            public string LabelTotalBase { set {} get { return "Base imponible"; } }
            public string LabelTotalIVA { set {} get { return "I.V.A."; } }
            public string LabelTotal { set {} get { return "TOTAL"; } }
            public string LabelLineUnits { set {} get { return "Unidades"; } }
            public string LabelLineDescription { set {} get { return "Concepto"; } }
            public string LabelLinePrice { set {} get { return "Precio"; } }
            public string LabelLineAmount { set {} get { return "Importe"; } }
            public string LabelFooter { set {} get { return "Domicilio social: Cardenal Marcelo Spinola, 50-52 28016 Madrid. Inscripta en el REgistro Mercantil de Madrid ..."; } }
        }

        private class InvoiceLineDataTest : IInvoiceLineData
        {
            private string _units;
            private string _description;
            private string _price;
            private string _amount;

            public InvoiceLineDataTest()
            {
            }

            public InvoiceLineDataTest(string units, string description, string price, string amount)
            {
                _units = units;
                _description = description;
                _price = price;
                _amount = amount;
            }

            public string Units
            {
                set {} get { return _units; }
            }

            public string Description
            {
                set {} get { return _description; }
            }

            public string Price
            {
                set {} get { return _price; }
            }

            public string Amount
            {
                set {} get { return _amount; }
            }

        }

    }

}
