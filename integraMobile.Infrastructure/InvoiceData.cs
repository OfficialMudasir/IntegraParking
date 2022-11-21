using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.Infrastructure.Invoicing
{

        public class InvoiceLineData : IInvoiceLineData
        {
            private string _units = "";
            private string _description = "";
            private string _price = "";
            private string _amount = "";

            public InvoiceLineData()
            {
            }

            public InvoiceLineData(string units, string description, string price, string amount)
            {
                _units = units;
                _description = description;
                _price = price;
                _amount = amount;
            }

            public string Units
            {
                get { return _units; }
                set { _units = value;  }
            }

            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            public string Price
            {
                get { return _price; }
                set { _price = value; }
            }

            public string Amount
            {
                get { return _amount; }
                set { _amount = value; }
            }

        }


        public class InvoiceData : IInvoiceData
        {

            private string _CompanyName = "";
            private string _CompanyInfo = "";
            private string _CustomerName = "";
            private string _CustomerInfo = "";
            private string _NIF = "";
            private string _Post = "";
            private string _Date = "";
            private string _Ref = "";
            private string _Contract = "";
            private string _InvoiceNum = "";
            private string _TotalBase = "";
            private string _TotalIVA = "";
            private string _Total = "";

            private string _LabelNIF = "";
            private string _LabelPost = "";
            private string _LabelDate = "";
            private string _LabelRef = "";
            private string _LabelContract = "";
            private string _LabelInvoiceNum = "";
            private string _LabelTotalBase = "";
            private string _LabelTotalIVA = "";
            private string _LabelTotal = "";
            private string _LabelLineUnits = "";
            private string _LabelLineDescription = "";
            private string _LabelLinePrice = "";
            private string _LabelLineAmount = "";
            private string _LabelFooter = "";


            public string CompanyName
            {
                get { return _CompanyName; }
                set { _CompanyName = value; }
            }
            public string CompanyInfo
            {
                get { return _CompanyInfo; }
                set { _CompanyInfo = value; }
            }
            public string CustomerName
            {
                get { return _CustomerName; }
                set { _CustomerName = value; }
            }
            public string CustomerInfo
            {
                get { return _CustomerInfo; }
                set { _CustomerInfo = value; }
            }
            public string NIF
            {
                get { return _NIF; }
                set { _NIF = value; }
            }
            public string Post{
                get { return _Post; }
                set { _Post = value; }
            }
            public string Date{
                get { return _Date; }
                set { _Date = value; }
            }
            public string Ref {
                get { return _Ref; }
                set { _Ref = value; }
            }
            public string Contract {
                get { return _Contract; }
                set { _Contract = value; }
            }
            public string InvoiceNum {
                get { return _InvoiceNum; }
                set { _InvoiceNum = value; }
            }
            public string TotalBase {
                get { return _TotalBase; }
                set { _TotalBase = value; }
            }
            public string TotalIVA {
                get { return _TotalIVA; }
                set { _TotalIVA = value; }
            }
            public string Total {
                get { return _Total; }
                set { _Total = value; }
            }

            public string LabelNIF {
                get { return _LabelNIF; }
                set { _LabelNIF = value; }
            }
            public string LabelPost {
                get { return _LabelPost; }
                set { _LabelPost = value; }
            }
            public string LabelDate {
                get { return _LabelDate; }
                set { _LabelDate = value; }
            }
            public string LabelRef {
                get { return _LabelRef; }
                set { _LabelRef = value; }
            }
            public string LabelContract {
                get { return _LabelContract; }
                set { _LabelContract = value; }
            }
            public string LabelInvoiceNum {
                get { return _LabelInvoiceNum; }
                set { _LabelInvoiceNum = value; }
            }
            public string LabelTotalBase {
                get { return _LabelTotalBase; }
                set { _LabelTotalBase = value; }
            }
            public string LabelTotalIVA {
                get { return _LabelTotalIVA; }
                set { _LabelTotalIVA = value; }
            }
            public string LabelTotal {
                get { return _LabelTotal; }
                set { _LabelTotal = value; }
            }
            public string LabelLineUnits {
                get { return _LabelLineUnits; }
                set { _LabelLineUnits = value; }
            }
            public string LabelLineDescription {
                get { return _LabelLineDescription; }
                set { _LabelLineDescription = value; }
            }
            public string LabelLinePrice {
                get { return _LabelLinePrice; }
                set { _LabelLinePrice = value; }
            }
            public string LabelLineAmount {
                get { return _LabelLineAmount; }
                set { _LabelLineAmount = value; }
            }
            public string LabelFooter {
                get { return _LabelFooter; }
                set { _LabelFooter = value; }
            }




            private List<InvoiceLineData> _lines = new List<InvoiceLineData>();

            public List<IInvoiceLineData> Lines
            {
                get
                {
                    return new List<IInvoiceLineData>(_lines.Cast<IInvoiceLineData>());
                }
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

        }
}


