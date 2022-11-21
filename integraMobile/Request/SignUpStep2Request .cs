using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Request
{
    public class SignUpStep2Request
    {
        #region Properties
        public String bin { get; set; }
        public String pass { get; set; }
        public Int32 ccode { get; set; }
        public Int32 OSID { get; set; }
        public String appvers { get; set; }
        public Questions questions { get; set; }
        #endregion
        
        #region Methos Public
        public static SignUpStep2Request getRequest(String bin, String password, Int32 ccode, Int32 OSID, String appvers, Questions questions)
        {
            SignUpStep2Request request = new SignUpStep2Request();
            request.bin = bin;
            request.pass = password;
            request.ccode = ccode;
            request.OSID = OSID;
            request.appvers = appvers;
            request.questions = questions;

            return request;
        }
        #endregion
    }
    
    public  class Questions
    {
        #region Properties
        public List<Question> question { get; set; }
        #endregion

        #region Constructor
        public Questions() 
        {
            question = new List<Question>();
        }
        #endregion
    }

    public  class Question 
    {
        #region Properties
        public Int32 idversion { get; set; }
        public Int32 value { get; set; }
        #endregion

        #region Constructor
        public Question()
        {
        }
        #endregion

    }
    
}