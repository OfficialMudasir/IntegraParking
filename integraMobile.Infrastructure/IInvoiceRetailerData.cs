using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.Infrastructure.Invoicing
{
    public interface IInvoiceRetailerData
    {
        string CompanyName { get; set; }
        string CompanyInfo { get; set; }
        string RetailerName { get; set; }
        string RetailerAddress { get; set; }
        string RetailerNIF { get; set; }
        string Date { get; set; }
        string InvoiceNum { get; set; }
        string TotalAmount { get; set; }
        string TotalServiceFEE { get; set; }
        string TotalPayTypeFEE { get; set; }
        string TotalVat { get; set; }
        string Total { get; set; }

        string LabelRetailerNIF { get; set; }        
        string LabelDate { get; set; }
        string LabelInvoiceNum { get; set; }
        string LabelCoupons { get; set; }
        string LabelCouponAmount { get; set; }
        string LabelTotalAmount { get; set; }
        string LabelTotalServiceFEE { get; set; }
        string LabelTotalPayTypeFEE { get; set; }
        string LabelTotalVat { get; set; }
        string LabelTotal { get; set; }
        string LabelLineUnits { get; set; }
        string LabelLineDescription { get; set; }
        string LabelLinePrice { get; set; }
        string LabelLineAmount { get; set; }
        string LabelFooter { get; set; }

        string LabelQRAvailable { get; set; }
        string LabelQRCode { get; set; }

        List<IInvoiceLineData> Lines { get; }
        bool AddLine(IInvoiceLineData line);

        List<IInvoiceRetailerQRData> QRs { get; }
        bool AddQR(IInvoiceRetailerQRData qr);

    }

    public interface IInvoiceRetailerQRData
    {
        string Code { get; set; }
        string KeyCode { get; set; }
        string Image { get; set; }
    }

}
