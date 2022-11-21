using integraMobile.ExternalWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.WS.Response
{
    public class VerifyLoginExistsResponse
    {
        #region Properties
        public ResultType r { get; set; }
        public int user_id { get; set; }
        public int usr_cur_id { get; set; }
        public int usr_cou_id { get; set; }
        public int InsVersion { get; set; }
        public int InsGeomVersion { get; set; }
        #endregion

        #region Constructor
        public VerifyLoginExistsResponse()
        { }
        #endregion
    }
}