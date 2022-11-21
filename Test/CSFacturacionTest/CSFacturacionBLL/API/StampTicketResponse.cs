using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.API
{
    public class StampTicketResponse : BaseResponse
    {
        #region Attributes
        private string _fileName;
        private string _pdfData; 
        #endregion

        #region Properties
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string PdfData
        {
            get { return _pdfData; }
            set { _pdfData = value; }
        } 
        #endregion

        #region Constructors
        public StampTicketResponse()
        {
        }

        public StampTicketResponse(string m_filename, string m_fileData)
        {
            this.FileName = m_filename;
            this.PdfData = m_fileData;
        } 
        #endregion
    }
}
