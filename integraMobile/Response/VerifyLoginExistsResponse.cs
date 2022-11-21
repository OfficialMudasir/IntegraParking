using integraMobile.ExternalWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Response
{
    public class VerifyLoginExistsResponse
    {
        #region Properties
        public ResultType r { get; set; }
        public int user_id { get; set; }
        #endregion

        #region Constructor
        public VerifyLoginExistsResponse()
        { }
        #endregion

    }
}