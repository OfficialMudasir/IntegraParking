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
    public class MagpieResponseModel
    {
        [DisplayName("Token")]
        public string magpieToken { get; set; }

        [DisplayName("Email")]
        public string magpieEmail { get; set; }

        [DisplayName("ErrorCode")]
        public string magpieErrorCode { get; set; }
    }



    #endregion
}