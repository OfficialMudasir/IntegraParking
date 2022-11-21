using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Request
{
    public class VerifySignUpExistsRequest
    {
        #region Properties
        public int lang { get; set; }
        public string email { get; set; }
        public int OSID { get; set; }
        //public string pasw { get; set; }
        //public int cityID { get; set; }
        public string vers { get; set; }
        #endregion

        #region Methos Public
        public VerifySignUpExistsRequest getRequest(int ilang, int iConsOsid, string sEmail, /*string password,int cityId,*/ string vr)
        {
            VerifySignUpExistsRequest request = new VerifySignUpExistsRequest();

            request.OSID = iConsOsid;
            request.lang = ilang;
            //request.pasw = password;
            request.email = sEmail;
            /*request.cityID = cityId;*/
            request.vers = vr;

            return request;
        }
        #endregion
    }
}