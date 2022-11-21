using integraMobile.ExternalWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Response
{
    [Serializable]
    public class QueryLoginCityResponse
    {
        #region Properties
        public string bckwsurl { get; set; }
        public string u { get; set; }
        public ResultType r { get; set; }
        public int cityID { get; set; }
        #endregion

        #region Constructor
        public QueryLoginCityResponse()
        {
            
        }
        #endregion
    }
}