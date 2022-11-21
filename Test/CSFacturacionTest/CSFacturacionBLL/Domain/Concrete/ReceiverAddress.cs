using CSFacturacionBLL.Infraestructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Domain.Concrete
{    
    public class ReceiverAddress : Base
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

        [FieldName("reCodigoPostal")]
        [JsonProperty("reCodigoPostal")]
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        [FieldName("rePais")]
        [JsonProperty("rePais")]
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        [FieldName("reEstado")]
        [JsonProperty("reEstado")]
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        [FieldName("reMunicipio")]
        [JsonProperty("reMunicipio")]
        public string Town
        {
            get { return _town; }
            set { _town = value; }
        }

        [FieldName("reReferencia")]
        [JsonProperty("reReferencia")]
        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        [FieldName("reLocalidad")]
        [JsonProperty("reLocalidad")]
        public string Locality
        {
            get { return _locality; }
            set { _locality = value; }
        }

        [FieldName("reColonia")]
        [JsonProperty("reColonia")]
        public string Colony
        {
            get { return _colony; }
            set { _colony = value; }
        }

        [FieldName("reNoInterior")]
        [JsonProperty("reNoInterior")]
        public string NumberInterior
        {
            get { return _numberInterior; }
            set { _numberInterior = value; }
        }

        [FieldName("reNoExterior")]
        [JsonProperty("reNoExterior")]
        public string NumberExterior
        {
            get { return _numberExterior; }
            set { _numberExterior = value; }
        }

        [FieldName("reCalle")]
        [JsonProperty("reCalle")]
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        } 
        #endregion
    }
}
