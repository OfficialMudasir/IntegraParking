using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Request
{
    public class VerifyLoginExistsRequest
    {
        #region Properties
        //public int lang { get; set; }
        public string u { get; set; }
        //public int OSID { get; set; }
        public string pasw { get; set; }
        //public int cityID { get; set; }
        //public string vers { get; set; }
        #endregion

        #region Methos Public
        public VerifyLoginExistsRequest getRequest(/*int ilang, int iConsOsid,*/ string email, string password/* ,int cityId, string vr*/)
        {
            VerifyLoginExistsRequest request = new VerifyLoginExistsRequest();

            //request.OSID = iConsOsid;
            //request.lang = ilang;
            request.pasw = password;
            request.u = email;
            /*request.cityID = cityId;*/
            //request.vers = vr;

            return request;
        }
        #endregion
    }
}