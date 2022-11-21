using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Request
{
    public class ForgotPasswordRequets
    {

        #region Properties
        public string u { get; set; }
        #endregion

        #region Methos Public
        public ForgotPasswordRequets getRequest(string user) 
        {
            ForgotPasswordRequets request = new ForgotPasswordRequets();
            if (!string.IsNullOrEmpty(user))
            {
                request.u = user;
            }
            return request;
        }
        #endregion
    }
}