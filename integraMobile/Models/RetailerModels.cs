using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using System.Reflection;
using System.Drawing;
using integraMobile.Properties;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Infrastructure.QrCodeNet.Rendering;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.Models
{
    [PropertiesMustMatch("Email", "ConfirmEmail", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_EmailsMustMatch")]
    [Serializable]
    public class RetailerCouponsModel
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(RetailerCouponsModel));

        #region Public Properties
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("RetailerCouponsModel_Name", NameResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("RetailerCouponsModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("RetailerCouponsModel_ConfirmEmail", NameResourceType = typeof(Resources))]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("RetailerCouponsModel_Address", NameResourceType = typeof(Resources))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("RetailerCouponsModel_DocID", NameResourceType = typeof(Resources))]
        public string DocId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_GreaterThanZeroRequired")]
        [LocalizedDisplayName("RetailerCouponsModel_Coupons", NameResourceType = typeof(Resources))]
        public int Coupons { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [Range(0.00000001, double.MaxValue, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_GreaterThanZeroRequired")]
        [LocalizedDisplayName("RetailerCouponsModel_CouponAmount", NameResourceType = typeof(Resources))]
        public decimal CouponAmount { get; set; }
        public string CouponAmountString
        {
            get { return string.Format("{0} {1}", CouponAmount.ToString("n" + NumDecimals), _amountCurrencyIsoCode); }
        }
    
        public string CompanyName { get { return _companyName; } }
        public string CompanyInfo { get { return _companyInfo; } }

        // Càlcul imports        
        private string _amountCurrencyIsoCode;
        private int _numDecimals = 2;
        
        public decimal PercVAT1 { get; set; }
        public decimal PercVAT2 { get; set; }
        public decimal PercFEE { get; set; }
        public decimal PercFEETopped { get; set; }
        public decimal FixedFEE { get; set; }
        public int PartialVAT1 { get; set; }
        public int PartialPercFEE { get; set; }
        public int PartialFixedFEE { get; set; }

        /*[LocalizedDisplayName("RetailerCouponsModel_Vat", NameResourceType = typeof(Resources))]        
        public decimal Vat
        {
            get { return _vat; }
        }*/
        public int NumDecimals
        {
            get { return _numDecimals; }
            set { _numDecimals = value; }
        }

        public string AmountCurrencyIsoCode
        {
            get { return _amountCurrencyIsoCode; }
        }

        [LocalizedDisplayName("RetailerCouponsModel_TotalAmount", NameResourceType = typeof(Resources))]
        public decimal TotalAmount
        {
            get { return _totalAmount; }
        }
        [LocalizedDisplayName("RetailerCouponsModel_TotalServiceFEE", NameResourceType = typeof(Resources))]
        public decimal TotalServiceFEE
        {
            get { return _totalServiceFEE; }
        }
        [LocalizedDisplayName("RetailerCouponsModel_TotalVat", NameResourceType = typeof(Resources))]
        public decimal TotalVat
        {
            get { return _totalVat; }
        }
        [LocalizedDisplayName("RetailerCouponsModel_Total", NameResourceType = typeof(Resources))]
        public decimal Total
        {
            get { return _total; }
        }

        public string VatString
        {
            get
            {
                if (PercVAT1 == PercVAT2)
                    return ((int)(PercVAT1*100)).ToString();
                else
                {
                    string sRet = "";
                    if (PercVAT1 != 0) sRet = ((int)(PercVAT1*100)).ToString();
                    if (PercVAT2 != 0)
                    {
                        if (sRet.Length > 0) sRet += "-";
                        sRet += ((int)(PercVAT2*100)).ToString();
                    }
                    return sRet;
                }

            }
        }
        public string TotalAmountString
        {
            get { return string.Format("{0} {1}", _totalAmount.ToString("n" + NumDecimals), _amountCurrencyIsoCode); }
        }        
        public string TotalServiceFEEString
        {
            get { return string.Format("{0} {1}", _totalServiceFEE.ToString("n" + +NumDecimals), _amountCurrencyIsoCode); }
        }        
        public string TotalVatString
        {
            get { return string.Format("{0} {1}", _totalVat.ToString("n" + NumDecimals), _amountCurrencyIsoCode); }
        }        
        public string TotalString
        {
            get { return string.Format("{0} {1}", _total.ToString("n" + NumDecimals), _amountCurrencyIsoCode); }
        }
        public decimal? RetailerId
        {
            get { return _retailerId; }
            set { _retailerId = value; }
        }
        public List<string> RechargeCoupons
        {
            get { return _rechargeCoupons; }
        }
        public DateTime? PaymentDate
        {
            get { return _paymentDate; }
        }
        public string InvoiceNum
        {
            get { return _invoiceNum; }
        }
        public decimal CurrencyDivisorFromIsoCode;
        #endregion

        #region Privates Properties
        private decimal? _retailerId;        
        private List<string> _rechargeCoupons;
        private DateTime? _paymentDate;
        private string _invoiceNum;
        private decimal _totalAmount;
        private decimal _totalServiceFEE;
        private decimal _totalVat;
        private decimal _total;
        private string _companyName;
        private string _companyInfo;
        #endregion

        #region Public Methods
        public void Init(IRetailerRepository retailerRepository, ICustomersRepository customersRepository)
        {

            // Obtenir moneda
            _amountCurrencyIsoCode = ConfigurationManager.AppSettings["CouponsCurrencyISOCode"].ToString();

            // Obtenir dades operador i percentatge IVA de l'operador
            int iDefaultOperatorId = Int32.Parse(ConfigurationManager.AppSettings["DefaultOperatorID"].ToString());
            OPERATOR oOperator = retailerRepository.GetOperator(iDefaultOperatorId);
            if (oOperator != null)
            {
                _companyName = oOperator.OPR_NAME_FOR_INVOICE;
                _companyInfo = Resources.RetailerInvoice_CompanyNIF + oOperator.OPR_VAT_NUMBER + "\n" + oOperator.OPR_ADDRESS_FOR_INVOICE.Replace("\\n", "\n");                
            }

            PAYMENT_TYPE oPaymentType = retailerRepository.GetPaymentType(1);
            
            decimal dPercVAT1;
            decimal dPercVAT2;
            decimal dPercFEE;
            decimal dPercFEETopped;
            decimal dFixedFEE;
            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;
            if (oPaymentType != null) iPaymentTypeId = oPaymentType.PAT_ID;

            if (customersRepository.GetFinantialParams(_amountCurrencyIsoCode, "", iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.CouponCharge,
                                                        out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
            {

                PercVAT1 = dPercVAT1;
                PercVAT2 = dPercVAT2;
                PercFEE = dPercFEE;
                PercFEETopped = dPercFEETopped;
                FixedFEE = dFixedFEE;                

            }

        }

        public void CalculateTotals(ICustomersRepository customersRepository)
        {            
            int iQuantity = Convert.ToInt32(Coupons * CouponAmount * 100);

            int iQFEE = 0;
            decimal dQFEE = 0;
            int iQVAT = 0;
            int iQTotal = 0;
            int iPartialVAT1;
            int iPartialPercFEE;
            int iPartialFixedFEE;
            int iPartialPercFEEVAT;
            int iPartialFixedFEEVAT;

            iQTotal = customersRepository.CalculateFEE(iQuantity, PercVAT1, PercVAT2, PercFEE, PercFEETopped, FixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);


            dQFEE = Math.Round(iQuantity * PercFEE, MidpointRounding.AwayFromZero);
            if (PercFEETopped > 0 && dQFEE > PercFEETopped) dQFEE = PercFEETopped;
            dQFEE += FixedFEE;
            iQFEE = Convert.ToInt32(dQFEE);

          
            iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;

            _totalAmount = (Coupons * CouponAmount);
            _totalServiceFEE = iQFEE / Convert.ToDecimal(100);
            _totalVat = iQVAT / Convert.ToDecimal(100);
            _total = iQTotal / Convert.ToDecimal(100);

            PartialVAT1 = iPartialVAT1;
            PartialPercFEE = iPartialPercFEE;
            PartialFixedFEE = iPartialFixedFEE;
        }

        public void LoadRechargeData(IRetailerRepository retailerRepository)
        {
            if (_retailerId.HasValue)
            {
                _rechargeCoupons = new List<string>();
                RETAILER oRetailer = retailerRepository.GetRetailer(_retailerId.Value);
                if (oRetailer.RETAILER_PAYMENTs.Count > 0)
                {
                    RETAILER_PAYMENT oPayment = oRetailer.RETAILER_PAYMENTs.First();
                    _paymentDate = oPayment.RTLPY_DATE;
                    int iDefaultOperatorId = Int32.Parse(ConfigurationManager.AppSettings["DefaultOperatorID"].ToString());
                    OPERATOR oOperator = retailerRepository.GetOperator(iDefaultOperatorId);
                    if (oOperator != null)
                    {
                        _invoiceNum = string.Format(oOperator.OPR_INVOICE_NUMBER_FORMAT, Convert.ToInt32(oPayment.RTLPY_INV_NUMBER), oPayment.RTLPY_DATE);
                    }
                    string sFilePath = HttpContext.Current.Server.MapPath("~/Tmp/QR{0}.png");
                    foreach (RECHARGE_COUPON oCoupon in oPayment.RECHARGE_COUPONs)
                    {
                        _rechargeCoupons.Add(oCoupon.RCOUP_KEYCODE);
                        QrRenderer.Render(oCoupon.RCOUP_CODE, 4, string.Format(sFilePath, oCoupon.RCOUP_KEYCODE), System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        public Dictionary<string, object> ToDictionary(System.Collections.Specialized.NameValueCollection reqParams)
        {
            List<string> lstModelKeys = new List<string>(new string[] { "Name", "Address", "DocId", "Email", "ConfirmEmail",
                                                                        "Coupons", "CouponAmount", "NumDecimals" });

            Dictionary<string, object> paramsDic = reqParams.AllKeys
                .Where(p => lstModelKeys.Contains(p))
                .ToDictionary(k => k, k => (object)reqParams[k]);

            return paramsDic;
        }

        public Dictionary<string, object> ToDictionary()
        {
            List<string> lstModelKeys = new List<string>(new string[] { "Name", "Address", "DocId", "Email", "ConfirmEmail",
                                                                        "Coupons", "CouponAmount", "NumDecimals" });

            Dictionary<string, object> paramsDic = new Dictionary<string, object>();
            
            foreach (string sPropertyName in lstModelKeys) {
                PropertyInfo propInfo = this.GetType().GetProperty(sPropertyName);
                object objValue = propInfo.GetValue(this, null);
                paramsDic.Add(sPropertyName, objValue);
            }

            return paramsDic;
        }

        public string GenerateInvoicePdf(IRetailerRepository retailerRepository, string sServerPath, string sResPath)
        {
            string sGeneratedFilename = "";
            try
            {
                if (this.PaymentDate.HasValue)
                {
                    string sInvoiceFormat = "";
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["COUPON_INVOICE_TEMPLATE"]))
                        sInvoiceFormat = ConfigurationManager.AppSettings["COUPON_INVOICE_TEMPLATE"].ToString();

                    /*int iOperatorId = 0;
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultOperatorID"]))
                        iOperatorId = int.Parse(ConfigurationManager.AppSettings["DefaultOperatorID"].ToString());
                    OPERATOR oOperator = retailerRepository.GetOperator(iOperatorId);*/


                    InvoiceRetailerPdfGenerator pdfGenerator = new InvoiceRetailerPdfGenerator(sServerPath, sInvoiceFormat, "coupons_" + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".pdf", sResPath);
                    pdfGenerator.TestMode = false;

                    InvoiceRetailerData invData = new InvoiceRetailerData();
                    invData.CompanyName = this._companyName;
                    invData.CompanyInfo = this._companyInfo;                    
                    invData.RetailerName = this.Name;
                    invData.RetailerAddress = this.Address;
                    invData.RetailerNIF = this.DocId;
                    invData.Date = this.PaymentDate.Value.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern);
                    invData.InvoiceNum = this.InvoiceNum;
                    invData.TotalAmount = this.TotalAmountString;// (Convert.ToDouble(oInvoice.CUSINV_INV_AMOUNT) / 100).ToString("###########.00") + " " + oInvoice.CURRENCy.CUR_ISO_CODE;
                    if (this.TotalServiceFEE != 0) invData.TotalServiceFEE = this.TotalServiceFEEString;
                    //if (this.TotalPayTypeFEE != 0) invData.TotalPayTypeFEE = this.TotalPayTypeFEEString;
                    if (this.TotalVat != 0) invData.TotalVat = this.TotalVatString;
                    invData.Total = this.TotalString;


                    invData.LabelRetailerNIF = Resources.InvoiceRetailer_PDF_NIF;                    
                    invData.LabelDate = Resources.InvoiceRetailer_PDF_Date;
                    invData.LabelInvoiceNum = Resources.InvoiceRetailer_PDF_InvoiceNumber;
                    invData.LabelTotalAmount = Resources.InvoiceRetailer_PDF_TotalAmount;
                    invData.LabelTotalServiceFEE = Resources.InvoiceRetailer_PDF_TotalServiceFEE;
                    invData.LabelTotalPayTypeFEE = Resources.InvoiceRetailer_PDF_TotalPayTypeFEE;
                    invData.LabelTotalVat = String.Format(Resources.InvoiceRetailer_PDF_VAT, this.VatString);
                    invData.LabelTotal = Resources.InvoiceRetailer_PDF_Total;
                    invData.LabelLineUnits = Resources.InvoiceRetailer_PDF_Units;
                    invData.LabelLineDescription = Resources.InvoiceRetailer_PDF_Detail;
                    invData.LabelLinePrice = Resources.InvoiceRetailer_PDF_Unit_Amount;
                    invData.LabelLineAmount = Resources.InvoiceRetailer_PDF_Amount;
                    invData.LabelFooter = "Domicilio social: Cardenal Marcelo Spinola, 50-52 28016 Madrid. Inscripta en el Registro Mercantil de Madrid ...";

                    invData.LabelQRAvailable = Resources.InvoiceRetailer_PDF_QRAvailable;
                    invData.LabelQRCode = Resources.InvoiceRetailer_PDF_QRCode;

                    invData.AddLine(new InvoiceLineData(this.Coupons.ToString(),
                                                         Resources.InvoiceRetailer_PDF_CouponsLine,
                                                         this.CouponAmountString, this.TotalAmountString));

                    string sFilePath = HttpContext.Current.Server.MapPath("~/Tmp/QR{0}.png");
                    foreach (string sKeyCode in _rechargeCoupons)
                    {
                        invData.AddQR(new InvoiceRetailerQRData(sKeyCode, sKeyCode, string.Format(sFilePath, sKeyCode)));
                    }

                    pdfGenerator.Data = invData;

                    if (pdfGenerator.generatePdf())
                    {
                        sGeneratedFilename = pdfGenerator.GeneratedPdfFilename;

                        /*Response.Clear();
                        Response.Buffer = false;
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-disposition", "attachment; filename=invoice_" + oInvoice.CUSINV_INV_DATE.ToString("yyyyMMdd") + ".pdf");

                        FileInfo oFileInfo = new FileInfo(pdfGenerator.generatedPDFPath());
                        long lfull_size = oFileInfo.Length;
                        oFileInfo = null;

                        Response.AddHeader("Content-length", lfull_size.ToString());
                        Response.WriteFile(pdfGenerator.generatedPDFPath());
                        Response.End();
                        System.IO.File.Delete(pdfGenerator.generatedPDFPath());*/


                    }
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("GenerateInvoicePdf Exception: {0}", ex.Message));
            }

            return sGeneratedFilename;
        }

        public static void DeleteQRTmpFiles(IRetailerRepository retailerRepository, decimal iRetailerId)
        {
            RETAILER oRetailer = retailerRepository.GetRetailer(iRetailerId);
            if (oRetailer.RETAILER_PAYMENTs.Count > 0)
            {
                RETAILER_PAYMENT oPayment = oRetailer.RETAILER_PAYMENTs.First();                
                string sFilePath = HttpContext.Current.Server.MapPath("~/Tmp/QR{0}.png");
                foreach (RECHARGE_COUPON oCoupon in oPayment.RECHARGE_COUPONs)
                {
                    if(System.IO.File.Exists(string.Format(sFilePath, oCoupon.RCOUP_KEYCODE)))
                    {
                        System.IO.File.Delete(string.Format(sFilePath, oCoupon.RCOUP_KEYCODE));
                    }
                }
            }
        }
        #endregion
    }

}