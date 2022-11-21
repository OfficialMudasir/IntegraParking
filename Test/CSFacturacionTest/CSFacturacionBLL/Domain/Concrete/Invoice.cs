using CSFacturacionBLL.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Domain.Concrete
{
    public class Invoice : Base
    {
        #region Enums
        public enum ePaymentManner
        {
            [Description("Pago en una sola exhibición")]
            UniquePayment,
            [Description("Parcialidad X de Y")]
            Partial
        }
        public enum ePaymentMethod
        {
            [Description("cheque")]
            Check,
            [Description("tarjeta")]
            CreditCard,
            [Description("depósito en cuenta")]
            Deposit
        }
        public enum eVoucherType
        {
            [Description("ingreso")]
            Entrance,
            [Description("egreso")]
            Exit
        }
        public enum eTaxType
        {
            [Description("IVA")]
            IVA,
            [Description("IEPS")]
            IEPS
        }
        public enum eTaxVersion
        {
            [Description("1.0")]
            _1_0            
        }
        #endregion

        #region Attributes
        private Emissor _emissor;
        private Receiver _receiver;
        private List<Concept> _concepts;
        private Decimal _discountPercentage;
        private Decimal _discountAmmount;
        private Decimal _discountReason;
        private Decimal _charges;        
        private ePaymentManner _paymentManner;
        private string _paymentConditions;
        private ePaymentMethod _paymentMethod;
        private string _paymentAccountNumber;
        private string _expeditionPlace;
        private string _refID;
        private string _documentType;
        private eVoucherType _voucherType;
        private eTaxType _transferredTaxType;
        private Decimal _transferredTaxAmmount;
        private Decimal _transferredTaxRate;
        private Decimal _transferredTaxSubtotal;
        private eTaxType _detainedTaxType;
        private Decimal _detainedTaxAmmount;
        private Decimal _detainedTaxSubtotal;
        private eTaxVersion _taxVersion;
        private Decimal _iva;
        //private Decimal _tua;
        private Decimal _discount;
        private Decimal _other;
        private Decimal _subTotal;            
        #endregion

        #region Properties
        [FieldName("subtotalConcepto")]
        public Decimal SubTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; }
        }

        [FieldName("otroConcepto")]
        public Decimal Other
        {
            get { return _other; }
            set { _other = value; }
        }

        [FieldName("descConcepto")]
        public Decimal Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        /*
        [FieldName("tuaConcepto")]
        public Decimal Tua
        {
            get { return _tua; }
            set { _tua = value; }
        }
        */
        [FieldName("ivaConcepto")]
        public Decimal Iva
        {
            get { return _iva; }
            set { _iva = value; }
        }
        [FieldName("montoTotal")]
        public Decimal Total
        {
            get
            {
                return ConceptsTotal + _transferredTaxSubtotal - _detainedTaxSubtotal;
            }            
        }

        [FieldName("version")]
        public eTaxVersion TaxVersion
        {
            get { return _taxVersion; }
            set { _taxVersion = value; }
        }

        [FieldName("subtotalRetenidos")]
        public Decimal DetainedTaxSubtotal
        {
            get { return _detainedTaxSubtotal; }
            set { _detainedTaxSubtotal = value; }
        }

        [FieldName("retenidoImporte")]
        public Decimal DetainedTaxAmmount
        {
            get { return _detainedTaxAmmount; }
            set { _detainedTaxAmmount = value; }
        }

        [FieldName("retenidoImpuesto")]
        public eTaxType DetainedTaxType
        {
            get { return _detainedTaxType; }
            set { _detainedTaxType = value; }
        }

        [FieldName("subtotalTrasladados")]
        public Decimal TransferredTaxSubtotal
        {
            get { return _transferredTaxSubtotal; }
            set { _transferredTaxSubtotal = value; }
        }

        [FieldName("trasladadoTasa")]
        public Decimal TransferredTaxRate
        {
            get { return _transferredTaxRate; }
            set { _transferredTaxRate = value; }
        }

        [FieldName("trasladadoImporte")]
        public Decimal TransferredTaxAmmount
        {
            get { return _transferredTaxAmmount; }
            set { _transferredTaxAmmount = value; }
        }

        [FieldName("trasladadoImpuesto")]
        public eTaxType TransferredTaxType
        {
            get { return _transferredTaxType; }
            set { _transferredTaxType = value; }
        }
        [FieldName("tipoComprobante")]
        public eVoucherType VoucherType
        {
            get { return _voucherType; }
            set { _voucherType = value; }
        }

        [FieldName("tipoDocumento")]
        public string DocumentType
        {
            get { return _documentType; }
            set { _documentType = value; }
        }

        [FieldName("refID")]
        public string RefID
        {
            get { return _refID; }
            set { _refID = value; }
        }

        [FieldName("lugarExpedicion")]
        public string ExpeditionPlace
        {
            get { return _expeditionPlace; }
            set { _expeditionPlace = value; }
        }

        [FieldName("numCtaPago")]
        public string PaymentAccountNumber
        {
            get { return _paymentAccountNumber; }
            set { _paymentAccountNumber = value; }
        }

        [FieldName("pagoMetodo")]
        public ePaymentMethod PaymentMethod
        {
            get { return _paymentMethod; }
            set { _paymentMethod = value; }
        }

        [FieldName("pagoCondiciones")]
        public string PaymentConditions
        {
            get { return _paymentConditions; }
            set { _paymentConditions = value; }
        }        
        [FieldName("pagoForma")]
        public ePaymentManner PaymentManner
        {
            get { return _paymentManner; }
            set { _paymentManner = value; }
        }

        [FieldName("totalConceptos")]
        public Decimal ConceptsTotal
        {
            get {
                return ConceptsSubtotal - DiscountAmmount;
            }
            
        }

        [FieldName("cargos")]
        public Decimal Charges
        {
            get { return _charges; }
            set { _charges = value; }
        }

        [FieldName("descuentoMotivo")]
        public Decimal DiscountReason
        {
            get { return _discountReason; }
            set { _discountReason = value; }
        }

        [FieldName("descuentoMonto")]
        public Decimal DiscountAmmount
        {
            get { return _discountAmmount; }
            set { _discountAmmount = value; }
        }

        [FieldName("descuentoPorcentaje")]
        public Decimal DiscountPercentage
        {
            get { return _discountPercentage; }
            set { _discountPercentage = value; }
        }

        [FieldName("subtotalConceptos")]
        public Decimal ConceptsSubtotal
        {
            get {
                Decimal subtotal = 0;
                if (_concepts != null)                
                {
                    foreach (Concept concept in _concepts)
                    {
                        subtotal += concept.Ammount;
                    }
                }

                return subtotal;
            }
        }

        [FieldName("")]
        public List<Concept> Concepts
        {
            get { return _concepts; }
            set { _concepts = value; }
        }

        [FieldName("")]
        public Receiver Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        [FieldName("")]
        public Emissor Emissor
        {
            get { return _emissor; }
            set { _emissor = value; }
        } 
        #endregion

    }
}
