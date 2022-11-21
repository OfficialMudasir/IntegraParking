using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Infraestructure
{
    public class FieldNameAttribute : Attribute
    {
        private string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public FieldNameAttribute(string m_fieldName)
        {
            this.FieldName = m_fieldName;
        }
    }
}
