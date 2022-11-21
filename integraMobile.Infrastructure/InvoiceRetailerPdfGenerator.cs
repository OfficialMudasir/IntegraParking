using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.Infrastructure.Invoicing
{
    public class InvoiceRetailerPdfGenerator : PdfGenerator
    {
        #region Declarations


        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(InvoiceRetailerPdfGenerator));

        private string _templateTotalPdf = "factura.pdf";
        private string _templateNoTotalPdf = "facturaNoTotal.pdf";
        private string _templateQRPdf = "retailer_qr.pdf";
        private string _generatedPdf = "Invoice_{0}.pdf";
        private string _resourcesPath = "";        

        private IInvoiceRetailerData _data = null;

        private bool _testMode = false;

        #endregion

        #region Constructor

        public InvoiceRetailerPdfGenerator(string workPath)
            : base(workPath)
        {            
            SetFilePaths();
        }

        public InvoiceRetailerPdfGenerator(string workPath, string templateFilename, string generatedPdfFilename, string resourcesPath)
            : base(workPath)
        {            
            _templateTotalPdf = templateFilename;
            _templateNoTotalPdf = templateFilename;
            _generatedPdf = generatedPdfFilename;
            _resourcesPath = resourcesPath;
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
                    //_data = new InvoiceDataTest();
                }
            }
        }

        public IInvoiceRetailerData Data
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
            _templateQRPdf = Path.Combine(_workPath, _templateQRPdf);
        }

        public override bool generatePdf()
        {
            if (_data != null)
            {
                refreshTmpStamp();

                string newPdf = Path.Combine(_workPath, _generatedPdf);
                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("generatePdf newPDF Path: {0}", newPdf));

                try
                {
                    // Calculate number of pages
                    List<int> lstPageRows = CalculateNumPages();
                    List<int> lstPageQRs = CalculateQRNumPages();
                    int iNumPages = lstPageRows.Count + lstPageQRs.Count;

                    PdfReader pdfReader;
                    PdfStamper pdfStamper;
                    AcroFields pdfFormFields;
                    int iRowIndex = 0;

                    for (int iPage = 0; iPage < lstPageRows.Count; iPage++)
                    {
                        using (FileStream stamperStream = new FileStream(iNumPages == 1 ? String.Format(newPdf, TmpStamp) : String.Format(TmpPdf, TmpStamp, iPage.ToString()), FileMode.Create))
                        {
                            pdfReader = new PdfReader((iPage == (iNumPages - 1)) ? _templateTotalPdf : _templateNoTotalPdf);

                            pdfStamper = new PdfStamper(pdfReader, stamperStream);

                            // set form pdfFormFields
                            pdfFormFields = pdfStamper.AcroFields;
                            pdfFormFields.SetField("fCompanyName", _data.CompanyName);
                            pdfFormFields.SetField("fCompanyInfo", _data.CompanyInfo);
                            pdfFormFields.SetField("fRetailerName", _data.RetailerName);
                            pdfFormFields.SetField("fRetailerAddress", _data.RetailerAddress);
                            pdfFormFields.SetField("fRetailerNIF", _data.RetailerNIF);
                            pdfFormFields.SetField("fDate", _data.Date);
                            pdfFormFields.SetField("fInvoiceNum", _data.InvoiceNum);
                            pdfFormFields.SetField("fTotalAmount", _data.TotalAmount);                                                                                    
                            pdfFormFields.SetField("fTotal", _data.Total);
                            pdfFormFields.SetField("fPageNum", String.Format("{0}/{1}", iPage + 1, iNumPages));
                            pdfFormFields.SetField("fLabelRetailerNIF", _data.LabelRetailerNIF);
                            pdfFormFields.SetField("fLabelDate", _data.LabelDate);
                            pdfFormFields.SetField("fLabelInvoiceNum", _data.LabelInvoiceNum);
                            pdfFormFields.SetField("fLabelTotalAmount", _data.LabelTotalAmount);
                            if (_data.TotalServiceFEE != "")
                            {
                                pdfFormFields.SetField("fLabelTotalServiceFEE", _data.LabelTotalServiceFEE);
                                pdfFormFields.SetField("fTotalServiceFEE", _data.TotalServiceFEE);
                            }
                            if (_data.TotalPayTypeFEE != "")
                            {
                                pdfFormFields.SetField("fLabelTotalPayTypeFEE", _data.LabelTotalPayTypeFEE);
                                pdfFormFields.SetField("fTotalPayTypeFEE", _data.TotalPayTypeFEE);
                            }
                            if (_data.TotalVat != "")
                            {
                                pdfFormFields.SetField("fLabelTotalVat", _data.LabelTotalVat);
                                pdfFormFields.SetField("fTotalVat", _data.TotalVat);
                            }
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

                    iRowIndex = 0;
                    for (int iPage = lstPageRows.Count; iPage < lstPageRows.Count + lstPageQRs.Count; iPage++)
                    {                        
                        using (FileStream stamperStream = new FileStream(iNumPages == 1 ? String.Format(newPdf, TmpStamp) : String.Format(TmpPdf, TmpStamp, iPage.ToString()), FileMode.Create))
                        {
                            pdfReader = new PdfReader(_templateQRPdf);

                            pdfStamper = new PdfStamper(pdfReader, stamperStream);

                            // Insert QR codes
                            var cb = pdfStamper.GetOverContent(1);
                            var ct = new ColumnText(cb);
                            ct.Alignment = Element.ALIGN_CENTER;
                            ct.SetSimpleColumn(45, 45, PageSize.A4.Width - 40, PageSize.A4.Height - 30);
                            PdfPTable table = createQRTable(iRowIndex, lstPageQRs[iPage - lstPageRows.Count]);
                            iRowIndex += lstPageQRs[iPage - lstPageRows.Count];
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
                catch (Exception ex)
                {
                    m_Log.LogMessage(LogLevels.logERROR, string.Format("generatePdf Exception: {0}", ex.Message));
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

        private PdfPTable createQRTable(int iRowIndex, int iNumRows)
        {
            iTextSharp.text.Font fontTable = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 9, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font fontTableSmall = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 8, iTextSharp.text.Font.NORMAL);

            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            float[] widths = new float[] { 30f, 65f, 100f, 65f, 30f };
            table.SetWidths(widths);

            PdfPCell cell;
            int iLineIndex;

            IInvoiceRetailerQRData _qr;

            using (Stream inputImageSepLeftStream = new FileStream(Path.Combine(_resourcesPath, "Recorta_izq.png"), FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageSepRightStream = new FileStream(Path.Combine(_resourcesPath, "Recorta_der.png"), FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageAvailableStream = new FileStream(Path.Combine(_resourcesPath, "RetailerAvailable.png"), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Image imageSepLeft = Image.GetInstance(inputImageSepLeftStream);
                Image imageSepRight = Image.GetInstance(inputImageSepRightStream);
                Image imageAvailable = Image.GetInstance(inputImageAvailableStream);

                bool bLeftAlign = true;

                PdfPTable tbAvailable = new PdfPTable(1);
                cell = new PdfPCell(new Phrase(_data.LabelQRAvailable, fontTableSmall));
                cell.BorderWidth = 0;
                tbAvailable.AddCell(cell);
                cell = new PdfPCell(imageAvailable);
                cell.BorderWidth = 0;
                tbAvailable.AddCell(cell);

                for (int iRow = 0; iRow < iNumRows; iRow++)
                {
                    iLineIndex = iRowIndex + iRow;

                    _qr = _data.QRs[iLineIndex];

                    using (Stream inputImageStream = new FileStream(_qr.Image, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Image imageQR = Image.GetInstance(inputImageStream);

                        if (bLeftAlign)
                        {
                            cell = new PdfPCell(tbAvailable);
                            cell.BorderWidth = 0;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);
                            cell = new PdfPCell(imageQR);
                            cell.FixedHeight = 100f;
                            cell.BorderWidth = 0;                            
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(String.Format(_data.LabelQRCode, _qr.KeyCode), fontTable));
                            cell.BorderWidth = 0;
                            cell.Colspan = 3;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //cell.PaddingRight = 10.0f;
                            table.AddCell(cell);
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(String.Format(_data.LabelQRCode, _qr.KeyCode), fontTable));
                            cell.BorderWidth = 0;
                            cell.Colspan = 3;
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;                            
                            table.AddCell(cell);
                            cell = new PdfPCell(imageQR);
                            cell.FixedHeight = 100f;
                            cell.BorderWidth = 0;
                            table.AddCell(cell);
                            cell = new PdfPCell(tbAvailable);
                            cell.BorderWidth = 0;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);
                        }

                        if (bLeftAlign)
                            cell = new PdfPCell(imageSepRight, true);
                        else
                            cell = new PdfPCell(imageSepLeft, true);
                        cell.Colspan = 5;                        
                        cell.BorderWidth = 0;
                        table.AddCell(cell);


                        inputImageStream.Close();
                    }
                    bLeftAlign = !bLeftAlign;
                }
                inputImageSepLeftStream.Close();
                inputImageSepRightStream.Close();
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

        private List<int> CalculateQRNumPages()
        {
            List<int> lstPageQRs = new List<int>();

            int iLinesNum = _data.QRs.Count;

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
                        while (fTableHeight > 790)
                        {
                            iPageRows -= 1;

                            var ct = new ColumnText(cb);
                            ct.Alignment = Element.ALIGN_CENTER;
                            ct.SetSimpleColumn(45, 45, PageSize.A4.Width - 40, PageSize.A4.Height - 30);
                            PdfPTable table = createQRTable(iRowIndex, iPageRows);
                            ct.AddElement(table);
                            ct.Go(true);
                            table.CalculateHeights();
                            fTableHeight = table.TotalHeight;

                        }
                        lstPageQRs.Add(iPageRows);
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
                lstPageQRs.Add(0);
            }

            return lstPageQRs;
        }

        #endregion

    }

}
