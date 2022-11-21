using CSFacturacionBLL.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Domain.Concrete
{    
    public class EmissorAddress : Base
    {
        #region Attributes
        private string _street;
        private string _numberExterior;
        private string _numberInterior;
        private string _colony;
        private string _locality;
        private string _reference;
        private string _town;
        private string _state;
        private string _country;
        private string _postalCode; 
        #endregion

        #region Properties

        [FieldName("emCodigoPostal")]
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        [FieldName("emPais")]
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        [FieldName("emEstado")]
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        [FieldName("emMunicipio")]
        public string Town
        {
            get { return _town; }
            set { _town = value; }
        }

        [FieldName("emReferencia")]
        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        [FieldName("emLocalidad")]
        public string Locality
        {
            get { return _locality; }
            set { _locality = value; }
        }

        [FieldName("emColonia")]
        public string Colony
        {
            get { return _colony; }
            set { _colony = value; }
        }

        [FieldName("emNoInterior")]
        public string NumberInterior
        {
            get { return _numberInterior; }
            set { _numberInterior = value; }
        }

        [FieldName("emNoExterior")]
        public string NumberExterior
        {
            get { return _numberExterior; }
            set { _numberExterior = value; }
        }

        [FieldName("emCalle")]
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        } 
        #endregion
    }
}
