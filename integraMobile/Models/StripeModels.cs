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
    public class StripeResponseModel
    {
        [DisplayName("Token")]
        public string stripeToken { get; set; }

        [DisplayName("Email")]
        public string stripeEmail { get; set; }
    
        [DisplayName("ErrorCode")]
        public string stripeErrorCode { get; set; }
    }



    #endregion
}