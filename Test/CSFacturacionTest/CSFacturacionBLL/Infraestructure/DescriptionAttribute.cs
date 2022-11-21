using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Infraestructure
{
    public class DescriptionAttribute : Attribute
    {
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DescriptionAttribute(string m_description)
        {
            this.Description = m_description;
        }
    }
}
