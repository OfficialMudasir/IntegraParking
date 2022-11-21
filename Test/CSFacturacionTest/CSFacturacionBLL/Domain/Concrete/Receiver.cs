using CSFacturacionBLL.Infraestructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Domain.Concrete
{
    public class Receiver : Base
    {
        #region Atrributes
        private string _rfc;
        private string _name;
        private ReceiverAddress _address;
        private string _customerNumber;
        private string _email;
        private string _phone;
        private string _fax;
        private string _buyer;
        private string _nim;        
        #endregion

        #region Properties
        [FieldName("reNIM")]
        [JsonProperty("reNIM")]
        public string NIM
        {
            get { return _nim; }
            set { _nim = value; }
        }

        [FieldName("reComprador")]
        [JsonProperty("reComprador")]
        public string Buyer
        {
            get { return _buyer; }
            set { _buyer = value; }
        }

        [FieldName("reFax")]
        [JsonProperty("reFax")]
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        [FieldName("reTelefono")]
        [JsonProperty("reTelefono")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        [FieldName("reEmail")]
        [JsonProperty("reEmail")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [FieldName("reNoCliente")]
        [JsonProperty("reNoCliente")]
        public string CustomerNumber
        {
            get { return _customerNumber; }
            set { _customerNumber = value; }
        }

        [FieldName("")]
        [JsonIgnore]
        public ReceiverAddress Address
        {
            get { return _address; }
            set { _address = value; }
        }

        [FieldName("reNombre")]
        [JsonProperty("reNombre")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [FieldName("reRfc")]
        [JsonProperty("reRfc")]
        public string Rfc
        {
            get { return _rfc; }
            set { _rfc = value; }
        } 
        #endregion
    }
}
