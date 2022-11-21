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
    public class LogOnModel
    {
        //[Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }

        public string culture { get; set; }

        public string urlreferer { get; set; }

        public string finan_operator { get; set; }

    }
    
    [Serializable]
    public class CustomerInscriptionModelStep1
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }                

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_Name", NameResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_FirstSurname", NameResourceType = typeof(Resources))]
        public string Surname1 { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. &'-]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_SecondSurname", NameResourceType = typeof(Resources))]
        public string Surname2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_DocID", NameResourceType = typeof(Resources))]
        public string DocId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_MainPhoneNumberPrefix", NameResourceType = typeof(Resources))]
        public string MainPhoneNumberPrefix { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_MainPhoneNumber", NameResourceType = typeof(Resources))]
        public string MainPhoneNumber { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_AlternativePhoneNumberPrefix", NameResourceType = typeof(Resources))]
        public string AlternativePhoneNumberPrefix { get; set; }

       
        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_AlternativePhoneNumber", NameResourceType = typeof(Resources))]
        public string AlternativePhoneNumber { get; set; }


    }

    [Serializable]
    public class CustomerInscriptionModelStep3
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_StreetName", NameResourceType = typeof(Resources))]
        public string StreetName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^[0-9]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_StreetNumber", NameResourceType = typeof(Resources))]
        public string StreetNumber { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_LevelInStreetNumber", NameResourceType = typeof(Resources))]
        public string LevelInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_DoorInStreetNumber", NameResourceType = typeof(Resources))]
        public string DoorInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_LetterInStreetNumber", NameResourceType = typeof(Resources))]
        public string LetterInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_StairInStreetNumber", NameResourceType = typeof(Resources))]
        public string StairInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_Country", NameResourceType = typeof(Resources))]
        public string Country { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]       
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_State", NameResourceType = typeof(Resources))]
        public string State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_City", NameResourceType = typeof(Resources))]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]       
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_ZipCode", NameResourceType = typeof(Resources))]
        public string ZipCode { get; set; }

        [DefaultValue(false)]
        public bool Valid { get; set; }


    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_PasswordsMustMatch")]
    [Serializable]
    public class CustomerInscriptionModelStep4
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_Plate", NameResourceType = typeof(Resources))]
        public string Plate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("CustomerInscriptionModel_Username", NameResourceType = typeof(Resources))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }                

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("CustomerInscriptionModel_Password", NameResourceType = typeof(Resources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("CustomerInscriptionModel_ConfirmPassword", NameResourceType = typeof(Resources))]
        public string ConfirmPassword { get; set; }

        public bool ConfirmServiceCondictions { get; set; }

        [DefaultValue(false)]
        public bool Valid { get; set; }


       

    }


    #endregion
}