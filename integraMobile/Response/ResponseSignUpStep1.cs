using integraMobile.ExternalWS;
using integraMobile.Models;
using integraMobile.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace integraMobile.Response
{
    [XmlRoot("ipark_out")]
    [Serializable]
    public class ResponseSignUpStep1
    {
        #region Properties
        public String bin { get; set; }
        public String ccode { get; set; }
        public CountriesModel countries { get; set; }
        public ResultType r { get; set; }
        public string signup_guid { get; set; }
        public String utcdate  { get; set; }
        #endregion

        #region Constructor
        public ResponseSignUpStep1()
        {
            countries = new CountriesModel();
        }
        #endregion

        //#region Methos Public
        //public static SignUpStep2Request getRequest(String bin, String password, Int32 ccode, Int32 OSID, String appvers, Questions questions)
        //{
        //    SignUpStep2Request request = new SignUpStep2Request();
        //    request.bin = bin;
        //    request.pass = password;
        //    request.ccode = ccode;
        //    request.OSID = OSID;
        //    request.appvers = appvers;
        //    request.questions = questions;

        //    return request;
        //}
        //#endregion
    }

    
}