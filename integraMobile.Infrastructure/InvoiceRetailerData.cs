using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.Infrastructure.Invoicing
{
    public class InvoiceRetailerData : IInvoiceRetailerData
    {
        private string _companyName;
        private string _companyInfo;
        private string _retailerName;
        private string _retailerAddress;
        private string _retailerNIF;
        private string _date;
        private string _invoiceNum;
        private string _totalAmount;
        private string _totalServiceFEE = "";
        private string _totalPayTypeFEE = "";
        private string _totalVat = "";
        private string _total;

        private string _labelRetailerNIF;
        private string _labelDate;
        private string _labelInvoiceNum;
        private string _labelCoupons;
        private string _labelCouponAmount;
        private string _labelTotalAmount;
        private string _labelTotalServiceFEE = "";
        private string _labelTotalPayTypeFEE = "";
        private string _labelTotalVat = "";
        private string _labelTotal;
        private string _LabelLineUnits = "";
        private string _LabelLineDescription = "";
        private string _LabelLinePrice = "";
        private string _LabelLineAmount = "";
        private string _LabelFooter = "";

        private string _labelQRAvailable = "";
        private string _labelQRCode = "";

        private List<InvoiceLineData> _lines = new List<InvoiceLineData>();
        private List<InvoiceRetailerQRData> _qrs = new List<InvoiceRetailerQRData>();

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public string CompanyInfo
        {
            get { return _companyInfo; }
            set { _companyInfo = value; }
        }

        public string RetailerName
        {
            get { return _retailerName; }
            set { _retailerName = value; }
        }

        public string RetailerAddress
        {
            get { return _retailerAddress; }
            set { _retailerAddress = value; }
        }

        public string RetailerNIF
        {
            get { return _retailerNIF; }
            set { _retailerNIF = value; }
        }

        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public string InvoiceNum
        {
            get { return _invoiceNum; }
            set { _invoiceNum = value; }            
        }

        public string TotalAmount
        {
            get { return _totalAmount; }
            set { _totalAmount = value; }
        }

        public string TotalServiceFEE
        {
            get { return _totalServiceFEE; }
            set { _totalServiceFEE = value; }
        }

        public string TotalPayTypeFEE
        {
            get { return _totalPayTypeFEE; }
            set { _totalPayTypeFEE = value; }
        }

        public string TotalVat
        {
            get { return _totalVat; }
            set { _totalVat = value; }
        }

        public string Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public string LabelRetailerNIF
        {
            get { return _labelRetailerNIF; }
            set { _labelRetailerNIF = value; }
        }

        public string LabelDate
        {
            get { return _labelDate; }
            set { _labelDate = value; }
        }

        public string LabelInvoiceNum
        {
            get { return _labelInvoiceNum; }
            set { _labelInvoiceNum = value; }
        }

        public string LabelCoupons
        {
            get { return _labelCoupons; }
            set { _labelCoupons = value; }
        }

        public string LabelCouponAmount
        {
            get { return _labelCouponAmount; }
            set { _labelCouponAmount = value; }
        }

        public string LabelTotalAmount
        {
            get { return _labelTotalAmount; }
            set { _labelTotalAmount = value; }
        }

        public string LabelTotalServiceFEE
        {
            get { return _labelTotalServiceFEE; }
            set { _labelTotalServiceFEE = value; }
        }

        public string LabelTotalPayTypeFEE
        {
            get { return _labelTotalPayTypeFEE; }
            set { _labelTotalPayTypeFEE = value; }
        }

        public string LabelTotalVat
        {
            get { return _labelTotalVat; }
            set { _labelTotalVat = value; }
        }

        public string LabelTotal
        {
            get { return _labelTotal; }
            set { _labelTotal = value; }
        }

        public string LabelLineUnits
        {
            get { return _LabelLineUnits; }
            set { _LabelLineUnits = value; }
        }
        public string LabelLineDescription
        {
            get { return _LabelLineDescription; }
            set { _LabelLineDescription = value; }
        }
        public string LabelLinePrice
        {
            get { return _LabelLinePrice; }
            set { _LabelLinePrice = value; }
        }
        public string LabelLineAmount
        {
            get { return _LabelLineAmount; }
            set { _LabelLineAmount = value; }
        }
        public string LabelFooter
        {
            get { return _LabelFooter; }
            set { _LabelFooter = value; }
        }

        public string LabelQRAvailable
        {
            get { return _labelQRAvailable; }
            set { _labelQRAvailable = value; }
        }

        public string LabelQRCode
        {
            get { return _labelQRCode; }
            set { _labelQRCode = value; }
        }

        public List<IInvoiceLineData> Lines
        {
            get { return new List<IInvoiceLineData>(_lines.Cast<IInvoiceLineData>()); }
        }

        public bool AddLine(IInvoiceLineData line)
        {
            try
            {
                _lines.Add((InvoiceLineData)line);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<IInvoiceRetailerQRData> QRs
        {
            get { return new List<IInvoiceRetailerQRData>(_qrs.Cast<IInvoiceRetailerQRData>()); }
        }

        public bool AddQR(IInvoiceRetailerQRData qr)
        {
            try
            {
                _qrs.Add((InvoiceRetailerQRData)qr);
                return true;
            }
            catch
            {
                return false;
            }

        }

    }

    public class InvoiceRetailerQRData : IInvoiceRetailerQRData
    {
        private string _code = "";
        private string _keyCode = "";
        private string _image = "";        

        public InvoiceRetailerQRData()
        {
        }

        public InvoiceRetailerQRData(string code, string keyCode, string image)
        {
            _code = code;
            _keyCode = keyCode;
            _image = image;            
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string KeyCode
        {
            get { return _keyCode; }
            set { _keyCode = value; }
        }

        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

    }

}
