using CSFacturacionBLL.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Domain.Concrete
{
    public class Concept : Base
    {
        #region Attributes
        private Decimal _quantity;
        private string _unit;
        private string _identificationNumber;
        private string _description;
        private Decimal _unitValue;
        private Decimal _ammount;
        #endregion

        #region Properties
        [FieldName("importe")]
        public Decimal Ammount
        {
            get { return _ammount; }
            set { _ammount = value; }
        }

        [FieldName("valorUnitario")]
        public Decimal UnitValue
        {
            get { return _unitValue; }
            set { _unitValue = value; }
        }

        [FieldName("descripcion")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [FieldName("numIdentificacion")]
        public string IdentificationNumber
        {
            get { return _identificationNumber; }
            set { _identificationNumber = value; }
        }

        [FieldName("unidad")]
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        [FieldName("cantidad")]
        public Decimal Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        } 
        #endregion
    }
}
