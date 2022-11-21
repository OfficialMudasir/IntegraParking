using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace integraMobile.Infrastructure.Invoicing
{
    public abstract class PdfGenerator
    {
        #region Declarations

        protected string _workPath;
        protected string _generatedPdfFilename;
        protected bool _pdfGenerated;

        private string _tmpPdf = "tmp{0}_{1}.pdf";
        private string _tmpStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        #endregion

        #region Constructors


        public PdfGenerator(string workPath)
        {
            _workPath = workPath;
            _generatedPdfFilename = "";
            _pdfGenerated = false;
            _tmpPdf = Path.Combine(_workPath, _tmpPdf);
        }

        #endregion

        #region Properties

        protected string TmpPdf
        {
            get
            {
                return _tmpPdf;
            }
        }

        protected string TmpStamp
        {
            get
            {
                return _tmpStamp;
            }
        }

        public string GeneratedPdfFilename
        {
            get
            {
                return _generatedPdfFilename;
            }
        }

        public bool PdfGenerated
        {
            get
            {
                return _pdfGenerated;
            }
        }

        #endregion

        #region Methods

        public abstract bool generatePdf();

        protected void refreshTmpStamp()
        {
            _tmpStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        #endregion

    }
}
