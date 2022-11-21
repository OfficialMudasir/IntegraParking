using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonSpied.Class
{
    public class PersonClass
    {
        #region Properties
        public String Email { get; set; }
        public String JsonPerson { get; set; }
        #endregion

        #region Constructor
        public PersonClass()
        { 
        }

        public PersonClass(String email, String json)
        {
            Email = email;
            JsonPerson = json;
        }
        #endregion
    }
}
