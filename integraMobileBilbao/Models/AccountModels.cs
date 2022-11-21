using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using integraMobile.Properties;
using integraMobile.Infrastructure;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace integraMobile.Models
{

    #region Models


    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_PasswordsMustMatch")]
    public class ForgotPasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("ForgotPassword_Password", NameResourceType = typeof(Resources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("ForgotPassword_ConfirmPassword", NameResourceType = typeof(Resources))]
        public string ConfirmPassword { get; set; }
    }



    #endregion


}
