using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Models
{
    [Serializable]
    public class CountryModel
    {
        #region Properties
        public String Id { get; set; }
        public String Description { get; set; }
        #endregion

        #region Constructor
        public CountryModel()
        {
 
        }
        #endregion
    }
}