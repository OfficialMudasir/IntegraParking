using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using integraMobile.Properties;
using integraMobile.Infrastructure;

namespace integraMobile.Models
{
    #region Models


    [Serializable]
    public class IECISAAcceptModel
    {
        [DisplayName("Token")]
        public string IECISAToken { get; set; }

        [DisplayName("Email")]
        public string IECISAEmail { get; set; }
    
        [DisplayName("ErrorCode")]
        public string IECISAErrorCode { get; set; }
    }

    [Serializable]
    public class IECISACancelModel
    {
        [DisplayName("Token")]
        public string IECISAToken { get; set; }

        [DisplayName("Email")]
        public string IECISAEmail { get; set; }

        [DisplayName("ErrorCode")]
        public string IECISAErrorCode { get; set; }
    }


    #endregion
}