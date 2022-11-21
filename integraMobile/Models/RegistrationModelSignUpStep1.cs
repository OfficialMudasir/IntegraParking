using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using integraMobile.Properties;
using integraMobile.Infrastructure;
using integraMobile.WS;
using integraMobile.ExternalWS;

namespace integraMobile.Models
{
    #region Models
    [Serializable]
    public class RegistrationModelSignUpStep1
    {
        #region Properties
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("RegistrationModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }
        public string ccode { get; set; }
        public ResultType r { get; set; }
        public String Message { get; set; }

        #endregion

        #region Constructor
        public RegistrationModelSignUpStep1()
        {
            r = ResultType.Result_OK;
            Message = String.Empty;
        }
        #endregion
    }
    #endregion
}