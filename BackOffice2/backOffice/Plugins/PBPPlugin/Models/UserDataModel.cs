using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class UserDataModel
    {        
        [LocalizedDisplayNameBundle("UserDataModel_UserId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Id")]
        public decimal UserId { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        //[StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]//***        
        [LocalizedDisplayNameBundle("UserDataModel_Username", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Username")]
        public string Username { get; set; }
        
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]        
        [LocalizedDisplayNameBundle("UserDataModel_Email", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Email")]
        public string Email { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_Name", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Name")]
        public string Name { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_Surname1", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Surname1")]
        public string Surname1 { get; set; }

        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_Surname2", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Surname2")]
        public string Surname2 { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[a-zA-Z0-9 ]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_DocId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "DocId")]
        public string DocId { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]        
        [LocalizedDisplayNameBundle("UserDataModel_MainPhoneCountryId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "MainPhoneCountryId")]
        public decimal MainPhoneCountryId { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        //[RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        [DataType(DataType.PhoneNumber)]
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_MainPhoneNumber", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "MainPhoneNumber")]
        public string MainPhoneNumber { get; set; }

        [DataType(DataType.Text)]        
        [LocalizedDisplayNameBundle("UserDataModel_AlternativePhoneCountryID", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "AlternativePhoneCountryID")]
        public decimal? AlternativePhoneCountryID { get; set; }

        //[RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        [DataType(DataType.PhoneNumber)]
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_AlternativePhoneNumber", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "AlternativePhoneNumber")]
        public string AlternativePhoneNumber { get; set; }
        
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_StreetName", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "StreetName")]
        public string StreetName { get; set; }
        
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        //[RegularExpression(@"^[0-9]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        [DataType(DataType.Text)]
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_StreetNumber", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "StreetNumber")]
        public int StreetNumber { get; set; }

        [DataType(DataType.Text)]
        //[RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_LevelInStreetNumber", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "LevelInStreetNumber")]
        public int? LevelInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_DoorInStreetNumber", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "DoorInStreetNumber")]
        public string DoorInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_LetterInStreetNumber", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "LetterInStreetNumber")]
        public string LetterInStreetNumber { get; set; }

        [DataType(DataType.Text)]
        //[StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_StairInStreetNumber", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "StairInStreetNumber")]
        public string StairInStreetNumber { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [LocalizedDisplayNameBundle("UserDataModel_CountryId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "CountryId")]
        public decimal CountryId { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_State", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "State")]
        public string State { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_City", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "City")]
        public string City { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. '-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")] //***
        //[StringLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")] //***        
        [LocalizedDisplayNameBundle("UserDataModel_ZipCode", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "ZipCode")]
        public string ZipCode { get; set; }
        
        public int PlatesCount { get; set; }        
        [LocalizedDisplayNameBundle("UserDataModel_Plates", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Plates")]
        public string Plates { get; set; }
        
        [LocalizedDisplayNameBundle("UserDataModel_Balance", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Balance")]
        public double Balance { get; set; }        
        [LocalizedDisplayNameBundle("UserDataModel_BalanceCurrencyId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "BalanceCurrencyId")]
        public decimal BalanceCurrencyId { get; set; }        
        [LocalizedDisplayNameBundle("UserDataModel_BalanceCurrencyIsoCode", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "BalanceCurrencyIsoCode")]
        public string BalanceCurrencyIsoCode { get; set; }
        
        [LocalizedDisplayNameBundle("UserDataModel_PaymentMeanTypeId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "PaymentMeanTypeId")]
        public int PaymentMeanTypeId { get; set; }        
        [LocalizedDisplayNameBundle("UserDataModel_PaymentMeanSubTypeId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "PaymentMeanSubTypeId")]
        public int PaymentMeanSubTypeId { get; set; }
        
        [LocalizedDisplayNameBundle("UserDataModel_PaymentSuscriptionTypeId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "PaymentSuscriptionTypeId")]
        public int? PaymentSuscriptionTypeId { get; set; }
        
        [LocalizedDisplayNameBundle("UserDataModel_Enabled", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Enabled")]
        public int Enabled { get; set; }
        
        [LocalizedDisplayNameBundle("UserDataModel_InsertionUTCDate", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "InsertionUTCDate")]
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