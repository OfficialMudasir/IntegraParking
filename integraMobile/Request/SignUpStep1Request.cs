using integraMobile.Helper;
using integraMobile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace integraMobile.Request
{
    public class SignUpStep1Request
    {
        #region Properties
        public String email { get; set; }
        public int lang { get; set; }
        public Int32 OSID { get; set; }
        public String ccode { get; set; }
        #endregion

        #region Methos Public
        public SignUpStep1Request getRequest(RegistrationModelSignUpStep1 model, int ilang, int iConsOsid)
        {
            SignUpStep1Request request = new SignUpStep1Request();
            if (!String.IsNullOrEmpty(model.ccode))
            {
                request.ccode = model.ccode;
            }

            request.email = model.Email;
            request.OSID = iConsOsid;
            request.lang = ilang;
            return request;
        }
        #endregion
    }
}