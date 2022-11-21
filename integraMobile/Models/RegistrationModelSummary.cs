using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using integraMobile.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Response;
using System.Text;
using Newtonsoft.Json;

namespace integraMobile.Models
{
    #region Models
    [Serializable]
    public class RegistrationModelSummary
    {
        #region Properties
        public String Email { get; set; }
        #endregion

        #region Constructor
        public RegistrationModelSummary()
        {
            Email = String.Empty;
        }
        #endregion

    }
    #endregion

    
}