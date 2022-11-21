using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.API
{
    public class VerifyTicketResponse : BaseResponse
    {
        #region Attributes
        private string _fileName;
        private string _pdfData64; 
        #endregion

        #region Properties
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string PdfData64
        {
            get { return _pdfData64; }
            set { _pdfData64 = value; }
        } 
        #endregion

        #region Constructors
        public VerifyTicketResponse()
        {
        }

        public VerifyTicketResponse(string m_filename, string m_fileData64)
        {
            this.FileName = m_filename;
            this.PdfData64 = m_fileData64;
        } 
        #endregion
    }
}
