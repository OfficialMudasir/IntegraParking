using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonSpied.DataBaseModel
{
    public class TableUsers
    {
        #region Properties

        public Decimal USER_ID { get; set; }
       
        public String USER_EMAIL {get; set;}
       
        public String USER_JSON {get; set;}
     
        #endregion

        #region Constructor
        public TableUsers()
        {

        }
        #endregion
    }
}
