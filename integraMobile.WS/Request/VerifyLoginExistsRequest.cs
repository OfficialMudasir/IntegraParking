using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.WS.Request
{
    public class VerifyLoginExistsRequest
    {
        #region Properties
        public string u { get; set; }
        public string pasw { get; set; }
        #endregion

        #region
        public VerifyLoginExistsRequest()
        { }

        public VerifyLoginExistsRequest(string email, string password)
        {
            pasw = password;
            u = email;
        }
        #endregion

        //#region Methos Public
        //public VerifyLoginExistsRequest getRequest(string email, string password)
        //{
        //    VerifyLoginExistsRequest request = new VerifyLoginExistsRequest();
        //    request.pasw = password;
        //    request.u = email;
        //    return request;
        //}
        //#endregion
    }
}