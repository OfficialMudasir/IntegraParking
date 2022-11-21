using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.Infrastructure.Invoicing
{
    public interface IInvoiceData
    {
        string CompanyName { get; set; }
        string CompanyInfo { get; set; }
        string CustomerName { get; set; }
        string CustomerInfo { get; set; }
        string NIF { get; set; }
        string Post { get; set; }
        string Date { get; set; }
        string Ref { get; set; }
        string Contract { get; set; }
        string InvoiceNum { get; set; }
        string TotalBase { get; set; }
        string TotalIVA { get; set; }
        string Total { get; set; }
        List<IInvoiceLineData> Lines { get;  }
        bool AddLine(IInvoiceLineData line);

        string LabelNIF { get; set; }
        string LabelPost { get; set; }
        string LabelDate { get; set; }
        string LabelRef { get; set; }
        string LabelContract { get; set; }
        string LabelInvoiceNum { get; set; }
        string LabelTotalBase { get; set; }
        string LabelTotalIVA { get; set; }
        string LabelTotal { get; set; }
        string LabelLineUnits { get; set; }
        string LabelLineDescription { get; set; }
        string LabelLinePrice { get; set; }
        string LabelLineAmount { get; set; }
        string LabelFooter { get; set; }
    }

    public interface IInvoiceLineData
    {
        string Units { get; set; }
        string Description { get; set; }
        string Price { get; set; }
        string Amount { get; set; }
    }
}
