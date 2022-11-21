using CSFacturacionBLL.Domain.Concrete;
using CSFacturacionBLL.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.API
{
    public class DataTicketResponse : BaseResponse
    {
        #region Attributes
        private string _data;
        private dynamic _dataObject;
        private Invoice _invoice;        
        #endregion

        #region Properties
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public dynamic DataObject
        {
            get { return _dataObject; }
            set { _dataObject = value; }
        }
        public Invoice Invoice
        {
            get { return _invoice; }
            set { _invoice = value; }
        }
        #endregion

        #region Constructors
        public DataTicketResponse()
        {
        }

        public DataTicketResponse(string m_data)
        {
            this.Data = m_data;

            if (!string.IsNullOrEmpty(m_data))
            {
                this.DataObject = Newtonsoft.Json.JsonConvert.DeserializeObject(m_data);
            }

            //Build the invoice
            if (this.DataObject != null)
            {
                this.Invoice = new Invoice();
                
                this.Invoice.TaxVersion = EnumUtils.GetValueFromDescription<Invoice.eTaxVersion>(Convert.ToString(this.DataObject["version"]));
                this.Invoice.DetainedTaxSubtotal = Convert.ToDecimal(this.DataObject["subtotalRetenidos"]);
                this.Invoice.DetainedTaxAmmount = Convert.ToDecimal(this.DataObject["retenidoImporte"]);
                this.Invoice.DetainedTaxType = EnumUtils.GetValueFromDescription<Invoice.eTaxType>(Convert.ToString(this.DataObject["retenidoImpuesto"]));
                this.Invoice.TransferredTaxSubtotal = Convert.ToDecimal(this.DataObject["subtotalTrasladados"]);
                this.Invoice.TransferredTaxRate = Convert.ToDecimal(this.DataObject["trasladadoTasa"]);
                this.Invoice.TransferredTaxAmmount = Convert.ToDecimal(this.DataObject["trasladadoImporte"]);
                this.Invoice.TransferredTaxType = EnumUtils.GetValueFromDescription<Invoice.eTaxType>(Convert.ToString(this.DataObject["trasladadoImpuesto"]));
                this.Invoice.VoucherType = EnumUtils.GetValueFromDescription<Invoice.eVoucherType>(Convert.ToString(this.DataObject["tipoComprobante"]));
                this.Invoice.DocumentType = Convert.ToString(this.DataObject["tipoDocumento"]);
                this.Invoice.RefID = Convert.ToString(this.DataObject["refID"]);
                this.Invoice.ExpeditionPlace = Convert.ToString(this.DataObject["lugarExpedicion"]);
                this.Invoice.PaymentAccountNumber = Convert.ToString(this.DataObject["numCtaPago"]);
                this.Invoice.PaymentMethod = EnumUtils.GetValueFromDescription<Invoice.ePaymentMethod>(Convert.ToString(this.DataObject["pagoMetodo"]));
                this.Invoice.PaymentConditions = Convert.ToString(this.DataObject["pagoCondiciones"]);
                //this.Invoice.PaymentManner = EnumUtils.GetValueFromDescription<Invoice.ePaymentManner>(Convert.ToString(this.DataObject["pagoForma"]));
                this.Invoice.Charges = Convert.ToDecimal(this.DataObject["cargos"]);
                this.Invoice.DiscountReason = Convert.ToDecimal(this.DataObject["descuentoMotivo"]);
                this.Invoice.DiscountAmmount = Convert.ToDecimal(this.DataObject["descuentoMonto"]);
                this.Invoice.DiscountPercentage = Convert.ToDecimal(this.DataObject["descuentoPorcentaje"]);

                if (!string.IsNullOrEmpty(Convert.ToString(this.DataObject["reNIM"])))
                {
                    this.Invoice.Receiver = new Receiver();

                    this.Invoice.Receiver.NIM = Convert.ToString(this.DataObject["reNIM"]);
                    this.Invoice.Receiver.Buyer = Convert.ToString(this.DataObject["reComprador"]);
                    this.Invoice.Receiver.Fax = Convert.ToString(this.DataObject["reFax"]);
                    this.Invoice.Receiver.Phone = Convert.ToString(this.DataObject["reTelefono"]);
                    this.Invoice.Receiver.Email = Convert.ToString(this.DataObject["reEmail"]);
                    this.Invoice.Receiver.CustomerNumber = Convert.ToString(this.DataObject["reNoCliente"]);
                    this.Invoice.Receiver.Name = Convert.ToString(this.DataObject["reNombre"]);
                    this.Invoice.Receiver.Rfc = Convert.ToString(this.DataObject["reRfc"]);

                    this.Invoice.Receiver.Address = new ReceiverAddress();
                    this.Invoice.Receiver.Address.PostalCode = Convert.ToString(this.DataObject["reCodigoPostal"]);
                    this.Invoice.Receiver.Address.Country = Convert.ToString(this.DataObject["rePais"]);
                    this.Invoice.Receiver.Address.State = Convert.ToString(this.DataObject["reEstado"]);
                    this.Invoice.Receiver.Address.Town = Convert.ToString(this.DataObject["reMunicipio"]);
                    this.Invoice.Receiver.Address.Reference = Convert.ToString(this.DataObject["reReferencia"]);
                    this.Invoice.Receiver.Address.Locality = Convert.ToString(this.DataObject["reLocalidad"]);
                    this.Invoice.Receiver.Address.Colony = Convert.ToString(this.DataObject["reColonia"]);
                    this.Invoice.Receiver.Address.NumberInterior = Convert.ToString(this.DataObject["reNoInterior"]);
                    this.Invoice.Receiver.Address.NumberExterior = Convert.ToString(this.DataObject["reNoExterior"]);
                    this.Invoice.Receiver.Address.Street = Convert.ToString(this.DataObject["reCalle"]);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(this.DataObject["emRfc"])))
                {
                    this.Invoice.Emissor = new Emissor();

                    this.Invoice.Emissor.Rfc = Convert.ToString(this.DataObject["emRfc"]);
                    this.Invoice.Emissor.Regime = Convert.ToString(this.DataObject["emRegimen"]);
                    this.Invoice.Emissor.Name = Convert.ToString(this.DataObject["emNombre"]);

                    this.Invoice.Emissor.Address = new EmissorAddress();
                    this.Invoice.Emissor.Address.PostalCode = Convert.ToString(this.DataObject["emCodigoPostal"]);
                    this.Invoice.Emissor.Address.Country = Convert.ToString(this.DataObject["emPais"]);
                    this.Invoice.Emissor.Address.State = Convert.ToString(this.DataObject["emEstado"]);
                    this.Invoice.Emissor.Address.Town = Convert.ToString(this.DataObject["emMunicipio"]);
                    this.Invoice.Emissor.Address.Reference = Convert.ToString(this.DataObject["emReferencia"]);
                    this.Invoice.Emissor.Address.Locality = Convert.ToString(this.DataObject["emLocalidad"]);
                    this.Invoice.Emissor.Address.Colony = Convert.ToString(this.DataObject["emColonia"]);
                    this.Invoice.Emissor.Address.NumberInterior = Convert.ToString(this.DataObject["emNoInterior"]);
                    this.Invoice.Emissor.Address.NumberExterior = Convert.ToString(this.DataObject["emNoExterior"]);
                    this.Invoice.Emissor.Address.Street = Convert.ToString(this.DataObject["emCalle"]);                    
                }

                if (this.DataObject["conceptos"] != null)
                {
                    this.Invoice.Concepts = new List<Concept>();

                    foreach (dynamic concept in this.DataObject["conceptos"])
                    {
                        Concept conceptObject = new Concept();

                        conceptObject.Unit = Convert.ToString(concept["unidad"]);
                        conceptObject.Ammount = Convert.ToDecimal(concept["importe"]);
                        conceptObject.Quantity = Convert.ToDecimal(concept["cantidad"]);
                        conceptObject.Description = Convert.ToString(concept["descripcion"]);
                        conceptObject.UnitValue = Convert.ToDecimal(concept["valorUnitario"]);
                        conceptObject.IdentificationNumber = Convert.ToString(concept["numIdentificacion"]);

                        this.Invoice.Concepts.Add(conceptObject);
                    }
                }

                //"impuestosagrupados" is already parsed in Invoce.*Tax* fields
                if (this.DataObject["impuestosagrupados"] != null)
                {                   
                }
            }
        } 
        #endregion
    }
}
