using CSFacturacionBLL.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Domain.Concrete
{ 
    public class Emissor : Base
    {
        #region Attributes
        private string _regime;
        private string _rfc;
        private string _name;
        private EmissorAddress _address; 
        #endregion

        #region Properties
        [FieldName("")]
        public EmissorAddress Address
        {
            get { return _address; }
            set { _address = value; }
        }

        [FieldName("emNombre")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [FieldName("emRfc")]
        public string Rfc
        {
            get { return _rfc; }
            set { _rfc = value; }
        }

        [FieldName("emRegimen")]
        public string Regime
        {
            get { return _regime; }
            set { _regime = value; }
        } 
        #endregion

        #region Public methods

        /*
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (PropertyInfo prop in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                FieldNameAttribute customAtrribute = prop.GetCustomAttribute(typeof(FieldNameAttribute)) as FieldNameAttribute;
                
                if (!string.IsNullOrEmpty(customAtrribute.FieldName)) 
                {
                    stringBuilder.Append(customAtrribute.FieldName);
                    stringBuilder.Append("|");
                    stringBuilder.Append(prop.GetValue(this).ToString());
                    stringBuilder.Append(Environment.NewLine);
                }
            }        

            return stringBuilder.ToString();
        }
        */

        #endregion
    }
}
