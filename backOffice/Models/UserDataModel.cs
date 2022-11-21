using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class UserDataModel
    {
        [LocalizedDisplayName("UserDataModel_UserId", NameResourceType = typeof(Resources))]
        public decimal UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("UserDataModel_Username", NameResourceType = typeof(Resources))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_Name", NameResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_Surname1", NameResourceType = typeof(Resources))]
        public string Surname1 { get; set; }

        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_Surname2", NameResourceType = typeof(Resources))]
        public string Surname2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_DocId", NameResourceType = typeof(Resources))]
        public string DocId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("UserDataModel_MainPhoneCountryId", NameResourceType = typeof(Resources))]
        public decimal MainPhoneCountryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_MainPhoneNumber", NameResourceType = typeof(Resources))]
        public string MainPhoneNumber { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("UserDataModel_AlternativePhoneCountryID", NameResourceType = typeof(Resources))]
        public decimal? AlternativePhoneCountryID { get; set; }

        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_AlternativePhoneNumber", NameResourceType = typeof(Resources))]
        public string AlternativePhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_StreetName", NameResourceType = typeof(Resources))]
        public string StreetName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^[0-9]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_StreetNumber", NameResourceType = typeof(Resources))]
        public int StreetNumber { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_LevelInStreetNumber", NameResourceType = typeof(Resources))]
        public int? LevelInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_DoorInStreetNumber", NameResourceType = typeof(Resources))]
        public string DoorInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_LetterInStreetNumber", NameResourceType = typeof(Resources))]
        public string LetterInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_StairInStreetNumber", NameResourceType = typeof(Resources))]
        public string StairInStreetNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]        
        [LocalizedDisplayName("UserDataModel_CountryId", NameResourceType = typeof(Resources))]
        public decimal CountryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_State", NameResourceType = typeof(Resources))]
        public string State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_City", NameResourceType = typeof(Resources))]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("UserDataModel_ZipCode", NameResourceType = typeof(Resources))]
        public string ZipCode { get; set; }
        
        public int PlatesCount { get; set; }
        [LocalizedDisplayName("UserDataModel_Plates", NameResourceType = typeof(Resources))]
        public string Plates { get; set; }

        [LocalizedDisplayName("UserDataModel_Balance", NameResourceType = typeof(Resources))]
        public double Balance { get; set; }
        [LocalizedDisplayName("UserDataModel_BalanceCurrencyId", NameResourceType = typeof(Resources))]
        public decimal BalanceCurrencyId { get; set; }
        [LocalizedDisplayName("UserDataModel_BalanceCurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string BalanceCurrencyIsoCode { get; set; }

        [LocalizedDisplayName("UserDataModel_PaymentMeanTypeId", NameResourceType = typeof(Resources))]
        public int PaymentMeanTypeId { get; set; }
        [LocalizedDisplayName("UserDataModel_PaymentMeanSubTypeId", NameResourceType = typeof(Resources))]
        public int PaymentMeanSubTypeId { get; set; }

        [LocalizedDisplayName("UserDataModel_PaymentSuscriptionTypeId", NameResourceType = typeof(Resources))]
        public int? PaymentSuscriptionTypeId { get; set; }

        [LocalizedDisplayName("UserDataModel_Enabled", NameResourceType = typeof(Resources))]
        public int Enabled { get; set; }

        [LocalizedDisplayName("UserDataModel_InsertionUTCDate", NameResourceType = typeof(Resources))]
        public DateTime InsertionUTCDate { get; set; }

        public string MainPhoneCountryId_FK { get; set; }
        public string AlternativePhoneCountryID_FK { get; set; }
        public string CountryId_FK { get; set; }
        public string BalanceCurrencyId_FK { get; set; }
        public string PaymentMeanTypeId_FK { get; set; }
        public string PaymentMeanSubTypeId_FK { get; set; }
        public string PaymentSuscriptionTypeId_FK { get; set; }

        public static IQueryable<UserDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<USER>();
            return List(backOfficeRepository, predicate, true);
        }

        public static IQueryable<UserDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs)
        {
            var predicate = PredicateBuilder.True<USER>();
            return List(backOfficeRepository, predicate, loadFKs);
        }

        public static IQueryable<UserDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<USER, bool>> predicate, bool loadFKs)
        {
            
            /*var dbUsers = backOfficeRepository.GetUsers(predicate);
            List<UserDataModel> users = new List<UserDataModel>();
            foreach (USER dom in dbUsers)
            {
                users.Add(new UserDataModel(dom, backOfficeRepository, loadFKs));
            }*/
            IQueryable<UserDataModel> users = null;
            if (loadFKs)
            {
                users = (from dom in backOfficeRepository.GetUsers(predicate)
                         select new UserDataModel()
                         {
                             UserId = dom.USR_ID,
                             Username = dom.USR_USERNAME,
                             Email = dom.USR_EMAIL,
                             Name = dom.CUSTOMER.CUS_NAME,
                             Surname1 = dom.CUSTOMER.CUS_SURNAME1,
                             Surname2 = dom.CUSTOMER.CUS_SURNAME2,
                             DocId = dom.CUSTOMER.CUS_DOC_ID,
                             MainPhoneCountryId = dom.USR_MAIN_TEL_COUNTRY,
                             MainPhoneNumber = dom.USR_MAIN_TEL,
                             AlternativePhoneCountryID = dom.USR_SECUND_TEL_COUNTRY,
                             AlternativePhoneNumber = dom.USR_SECUND_TEL,
                             StreetName = dom.CUSTOMER.CUS_STREET,
                             StreetNumber = dom.CUSTOMER.CUS_STREE_NUMBER,
                             LevelInStreetNumber = dom.CUSTOMER.CUS_LEVEL_NUM,
                             DoorInStreetNumber = dom.CUSTOMER.CUS_DOOR,
                             LetterInStreetNumber = dom.CUSTOMER.CUS_LETTER,
                             StairInStreetNumber = dom.CUSTOMER.CUS_STAIR,
                             CountryId = dom.CUSTOMER.CUS_COU_ID,
                             State = dom.CUSTOMER.CUS_STATE,
                             City = dom.CUSTOMER.CUS_CITY,
                             ZipCode = dom.CUSTOMER.CUS_ZIPCODE,
                             PlatesCount = dom.USER_PLATEs.Count,
                             Plates = string.Join(", ", dom.USER_PLATEs.Where(x => x.USRP_ENABLED == 1).Select(x => x.USRP_PLATE)),
                             Balance = Convert.ToDouble(dom.USR_BALANCE / 100.0),
                             BalanceCurrencyId = dom.USR_CUR_ID,
                             BalanceCurrencyIsoCode = dom.CURRENCy.CUR_ISO_CODE,
                             PaymentMeanTypeId = (dom.CUSTOMER_PAYMENT_MEAN != null) ? dom.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID : -1,
                             PaymentMeanSubTypeId = (dom.CUSTOMER_PAYMENT_MEAN != null) ? dom.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID : -1,
                             PaymentSuscriptionTypeId = (dom.USR_SUSCRIPTION_TYPE.HasValue ? dom.USR_SUSCRIPTION_TYPE : -1),
                             Enabled = dom.USR_ENABLED,
                             InsertionUTCDate = dom.USR_INSERT_UTC_DATE,
                             //Installations = string.Join(", ", dom.OPERATIONs.GroupBy(op => op.INSTALLATION).Select(op => op.First()).Select(op => op.INSTALLATION.INS_SHORTDESC)),
                             MainPhoneCountryId_FK = dom.COUNTRy1.COU_DESCRIPTION,
                             AlternativePhoneCountryID_FK = (dom.COUNTRy2 != null) ? dom.COUNTRy2.COU_DESCRIPTION : "",
                             CountryId_FK = dom.COUNTRy.COU_DESCRIPTION,
                             BalanceCurrencyId_FK = dom.CURRENCy.CUR_NAME, // CurrencyDataModel.Get(backOfficeRepository, dom.USR_CUR_ID).Name,
                             PaymentMeanTypeId_FK = PaymentMeanTypeDataModel.GetTypeIdString((dom.CUSTOMER_PAYMENT_MEAN != null) ? dom.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID : -1),
                             PaymentMeanSubTypeId_FK = PaymentMeanSubTypeDataModel.GetTypeIdString((dom.CUSTOMER_PAYMENT_MEAN != null) ? dom.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID : -1),
                             PaymentSuscriptionTypeId_FK = PaymentSuscryptionTypeDataModel.GetTypeIdString(dom.USR_SUSCRIPTION_TYPE)
                         });                         
            }
            else
            {
                users = (from dom in backOfficeRepository.GetUsers(predicate)
                         select new UserDataModel()
                         {
                            UserId = dom.USR_ID,
                            Username = dom.USR_USERNAME,
                            Email = dom.USR_EMAIL,
                            Name = dom.CUSTOMER.CUS_NAME,
                            Surname1 = dom.CUSTOMER.CUS_SURNAME1,
                            Surname2 = dom.CUSTOMER.CUS_SURNAME2,
                            DocId = dom.CUSTOMER.CUS_DOC_ID,
                            MainPhoneCountryId = dom.USR_MAIN_TEL_COUNTRY,
                            MainPhoneNumber = dom.USR_MAIN_TEL,
                            AlternativePhoneCountryID = dom.USR_SECUND_TEL_COUNTRY,
                            AlternativePhoneNumber = dom.USR_SECUND_TEL,
                            StreetName = dom.CUSTOMER.CUS_STREET,
                            StreetNumber = dom.CUSTOMER.CUS_STREE_NUMBER,
                            LevelInStreetNumber = dom.CUSTOMER.CUS_LEVEL_NUM,
                            DoorInStreetNumber = dom.CUSTOMER.CUS_DOOR,
                            LetterInStreetNumber = dom.CUSTOMER.CUS_LETTER,
                            StairInStreetNumber = dom.CUSTOMER.CUS_STAIR,
                            CountryId = dom.CUSTOMER.CUS_COU_ID,
                            State = dom.CUSTOMER.CUS_STATE,
                            City = dom.CUSTOMER.CUS_CITY,
                            ZipCode = dom.CUSTOMER.CUS_ZIPCODE,
                            PlatesCount = dom.USER_PLATEs.Count,
                            Plates = string.Join(", ", dom.USER_PLATEs.Where(x => x.USRP_ENABLED == 1).Select(x => x.USRP_PLATE)),
                            Balance = Convert.ToDouble(dom.USR_BALANCE / 100.0),
                            BalanceCurrencyId = dom.USR_CUR_ID,
                            BalanceCurrencyIsoCode = dom.CURRENCy.CUR_ISO_CODE,
                            PaymentMeanTypeId = (dom.CUSTOMER_PAYMENT_MEAN != null) ? dom.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID : -1,
                            PaymentMeanSubTypeId = (dom.CUSTOMER_PAYMENT_MEAN != null) ? dom.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID : -1,
                            PaymentSuscriptionTypeId = (dom.USR_SUSCRIPTION_TYPE.HasValue ? dom.USR_SUSCRIPTION_TYPE : -1),
                            Enabled = dom.USR_ENABLED,
                            InsertionUTCDate = dom.USR_INSERT_UTC_DATE
                            //Installations = string.Join(", ", dom.OPERATIONs.GroupBy(op => op.INSTALLATION).Select(op => op.First()).Select(op => op.INSTALLATION.INS_SHORTDESC)),
                         });                         
            }
            users = users.OrderBy(e => e.Username);

            var lstUsers = users.ToList();

            return lstUsers.AsQueryable();

            /*var lstUsers = users.ToList();
            foreach (UserDataModel user in lstUsers)
            {
                var predicateUser = PredicateBuilder.True<USER>();
                predicateUser = predicateUser.And(a => a.USR_ID == user.UserId);
                USER oUser = backOfficeRepository.GetUsers(predicateUser).FirstOrDefault();
                user.Plates = string.Join(", ", oUser.USER_PLATEs.Select(x => x.USRP_PLATE));
            }

            return lstUsers.AsQueryable();*/
        }

        public static UserDataModel Get(IBackOfficeRepository backOfficeRepository, decimal userId)
        {           
            var predicate = PredicateBuilder.True<USER>();
            predicate = predicate.And(a => a.USR_ID == userId);
            var users = (from dom in backOfficeRepository.GetUsers(predicate)
                            select new UserDataModel
                            {
                                UserId = dom.USR_ID,
                                Username = dom.USR_USERNAME,
                                Email = dom.USR_EMAIL,
                                Name = dom.CUSTOMER.CUS_NAME,
                                Surname1 = dom.CUSTOMER.CUS_SURNAME1,
                                Surname2 = dom.CUSTOMER.CUS_SURNAME2,
                                DocId = dom.CUSTOMER.CUS_DOC_ID,
                                MainPhoneCountryId = dom.USR_MAIN_TEL_COUNTRY,
                                MainPhoneNumber = dom.USR_MAIN_TEL,
                                AlternativePhoneCountryID = dom.USR_SECUND_TEL_COUNTRY,
                                AlternativePhoneNumber = dom.USR_SECUND_TEL,
                                StreetName = dom.CUSTOMER.CUS_STREET,
                                StreetNumber = dom.CUSTOMER.CUS_STREE_NUMBER,
                                LevelInStreetNumber = dom.CUSTOMER.CUS_LEVEL_NUM,
                                DoorInStreetNumber = dom.CUSTOMER.CUS_DOOR,
                                LetterInStreetNumber = dom.CUSTOMER.CUS_LETTER,
                                StairInStreetNumber = dom.CUSTOMER.CUS_STAIR,
                                CountryId = dom.CUSTOMER.CUS_COU_ID,
                                State = dom.CUSTOMER.CUS_STATE,
                                City = dom.CUSTOMER.CUS_CITY,
                                ZipCode = dom.CUSTOMER.CUS_ZIPCODE,
                                PlatesCount = dom.USER_PLATEs.Count,
                                Plates = string.Join(", ", dom.USER_PLATEs.Where(x => x.USRP_ENABLED == 1).Select(x => x.USRP_PLATE)),
                                MainPhoneCountryId_FK = dom.COUNTRy1.COU_DESCRIPTION,
                                AlternativePhoneCountryID_FK = dom.COUNTRy2.COU_DESCRIPTION,
                                CountryId_FK = dom.COUNTRy.COU_DESCRIPTION
                            })
                            .OrderBy(e => e.Username)
                            .AsQueryable();
            return users.First();
        }

        public static UserDataModel Get(IBackOfficeRepository backOfficeRepository, decimal? userId)
        {
            if (userId.HasValue)
            {
                return Get(backOfficeRepository, userId.Value);
            }
            else
                return new UserDataModel() { Username = "" };
        }

        public static UserDataModel Get(IQueryable<UserDataModel> users, decimal userId)
        {
            return users.Where(u => u.UserId == userId).First();
        }

        public static UserDataModel Get(IQueryable<UserDataModel> users, decimal? userId)
        {
            if (userId.HasValue)
            {
                return Get(users, userId.Value);
            }
            else
                return new UserDataModel() { Username = "" };
        }

    }
}