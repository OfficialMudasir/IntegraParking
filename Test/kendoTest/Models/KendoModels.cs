using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using kendoTest.Properties;
using kendoTest.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace kendoTest.Models
{

    public class OperationModel
    {
        
        [LocalizedDisplayName("Account_Op_Operation", NameResourceType = typeof(Resources))]
        public int TypeId { get; set; }
        [LocalizedDisplayName("Account_Op_Operation", NameResourceType = typeof(Resources))]
        public ChargeOperationsType Type { get; set; }
        [LocalizedDisplayName("Account_Op_Installation", NameResourceType = typeof(Resources))]
        public string Installation { get; set; }
        [LocalizedDisplayName("Account_Op_Date", NameResourceType = typeof(Resources))]
        public DateTime? Date { get; set; }
        [LocalizedDisplayName("Account_Op_Start_Date", NameResourceType = typeof(Resources))]
        public DateTime? DateIni { get; set; }
        [LocalizedDisplayName("Account_Op_End_Date", NameResourceType = typeof(Resources))]
        public DateTime? DateEnd { get; set; }
        [LocalizedDisplayName("Account_Op_Amount", NameResourceType = typeof(Resources))]
        public double Amount { get; set; }
        [LocalizedDisplayName("Account_Op_Amount", NameResourceType = typeof(Resources))]
        public string AmountFormat { get; set; }
        public decimal AmountCurrencyId { get; set; }
        public string CurrencyIsoCode { get; set; }
        [LocalizedDisplayName("Account_Op_Duration", NameResourceType = typeof(Resources))]        
        public int? Time { get; set; }
        [LocalizedDisplayName("Account_Op_ChangeApplied", NameResourceType = typeof(Resources))]        
        public double? ChangeApplied { get; set; }
        public int? PlateId { get; set; }
        [LocalizedDisplayName("Account_Op_LicensePlate", NameResourceType = typeof(Resources))]        
        public string Plate { get; set; }
        [LocalizedDisplayName("Account_Op_TicketNumber", NameResourceType = typeof(Resources))]        
        public string TicketNumber { get; set; }
        [LocalizedDisplayName("Account_Op_TicketData", NameResourceType = typeof(Resources))]        
        public string TicketData { get; set; }
        public string Sector { get; set; }
        public string Tariff { get; set; }

    }

    public class OperationExtModel
    {
        
        public int TypeId { get; set; }        
        public ChargeOperationsType Type { get; set; }

        public decimal? UserId { get; set; }
        //public UserDataModel User { get; set; }
        
        public decimal? InstallationId { get; set; }        
        public string Installation { get; set; }
        public string InstallationShortDesc { get; set; }
        
        public DateTime? Date { get; set; }        
        public DateTime? DateIni { get; set; }        
        public DateTime? DateEnd { get; set; }

        public double? Amount { get; set; }
        public decimal AmountCurrencyId { get; set; }
        public string AmountCurrencyIsoCode { get; set; }
        public double? AmountFinal { get; set; }

        public int? Time { get; set; }

        public double? BalanceBefore { get; set; }
        public decimal? BalanceCurrencyId { get; set; }
        public string BalanceCurrencyIsoCode { get; set; }

        public double? ChangeApplied { get; set; }

        public decimal? PlateId { get; set; }        
        public string Plate { get; set; }

        public string TicketNumber { get; set; }
        public string TicketData { get; set; }


        public decimal? SectorId { get; set; }
        //public string Sector { get; set; }
        public decimal? TariffId { get; set; }
        //public string Tariff { get; set; }

        public int SuscriptionType { get; set; } // Hi ha type enum??        

        public DateTime? InsertionUTCDate { get; set; }

        public decimal? RechargeId { get; set; }
        public DateTime? RechargeDate { get; set; }
        public double? RechargeAmount { get; set; }
        public decimal? RechargeAmountCurrencyId { get; set; }
        public string RechargeAmountCurrencyIsoCode { get; set; }
        public double? RechargeBalanceBefore { get; set; }
        public DateTime? RechargeInsertionUTCDate { get; set; }

        public decimal? DiscountId { get; set; }
        public DateTime? DiscountDate { get; set; }
        public double? DiscountAmount { get; set; }
        public decimal? DiscountAmountCurrencyId { get; set; }
        public string DiscountAmountCurrencyIsoCode { get; set; }
        public double? DiscountAmountFinal { get; set; }
        public decimal? DiscountBalanceCurrencyId { get; set; }
        public string DiscountBalanceCurrencyIsoCode { get; set; }
        public double? DiscountBalanceBefore { get; set; }
        public double? DiscountChangeApplied { get; set; }
        public DateTime? DiscountInsertionUTCDate { get; set; }

        public int? ServiceChargeTypeId { get; set; }

        /*
        private System.Nullable<int> _SECH_SECHT_ID;



        private System.Nullable<decimal> _OPE_CUSPMR_ID;
        private System.Nullable<System.DateTime> _CUSPMR_DATE;
        private System.Nullable<int> _CUSPMR_AMOUNT;
        private System.Nullable<decimal> _CUSPMR_CUR_ID;
        private string _CUSPMR_AMOUNT_ISO_CODE;
        private System.Nullable<int> _CUSPMR_BALANCE_BEFORE;
        private System.Nullable<System.DateTime> _CUSPMR_INSERTION_UTC_DATE;

        private System.Nullable<decimal> _OPE_OPEDIS_ID;            
        private System.Nullable<System.DateTime> _OPEDIS_DATE;
        private System.Nullable<int> _OPEDIS_AMOUNT;
        private System.Nullable<decimal> _OPEDIS_AMOUNT_CUR_ID;
        private string _OPEDIS_AMOUNT_CUR_ISO_CODE;
        private System.Nullable<int> _OPEDIS_FINAL_AMOUNT;
        private System.Nullable<decimal> _OPEDIS_BALANCE_CUR_ID;
        private string _OPEDIS_BALANCE_CUR_ISO_CODE;
        private System.Nullable<decimal> _OPEDIS_CHANGE_APPLIED;
        private System.Nullable<int> _OPEDIS_BALANCE_BEFORE;
        private System.Nullable<System.DateTime> _OPEDIS_INSERTION_UTC_DATE;
        */
    }

    [PropertiesMustMatch("NewPassword", "ConfirmNewPassword", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_PasswordsMustMatch")]
    public class UserAdminDataModel
    {

        public UserAdminDataModel()
        {
        }
        public UserAdminDataModel(USER oUser)
        {
            UserID = oUser.USR_ID;

            CountryID = oUser.USR_COU_ID;
            Country = new CountryDataModel() {                                    
                CountryID = oUser.COUNTRy.COU_ID,
                Description = oUser.COUNTRy.COU_DESCRIPTION
            };

            Email = oUser.USR_EMAIL;
            Username = oUser.USR_USERNAME;
                                
            MainPhoneCountryID = oUser.USR_MAIN_TEL_COUNTRY;
            MainPhoneCountry = new CountryDataModel() {
                CountryID = oUser.COUNTRy1.COU_ID,
                Description = oUser.COUNTRy1.COU_DESCRIPTION
            };
            MainPhoneNumber = oUser.USR_MAIN_TEL;

            AlternativePhoneCountryID = oUser.USR_SECUND_TEL_COUNTRY;
            AlternativePhoneCountry = new CountryDataModel() {
                CountryID = oUser.COUNTRy2.COU_ID,
                Description = oUser.COUNTRy2.COU_DESCRIPTION
            };
            AlternativePhoneNumber = oUser.USR_SECUND_TEL;

            CurrencyID = oUser.USR_CUR_ID;
            Currency = new CurrencyDataModel() {
                CurrencyID = oUser.CURRENCy.CUR_ID,
                Name = oUser.CURRENCy.CUR_NAME
            };
                       
            CurrentPassword = "";
            NewPassword = "";
            ConfirmNewPassword = "";

        }

        public decimal UserID { get; set; }
        
        public decimal CountryID { get; set; }                

        [LocalizedDisplayName("CustomerInscriptionModel_Country", NameResourceType = typeof(Resources))]
        [UIHint("Country")]
        public CountryDataModel Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("CustomerInscriptionModel_Username", NameResourceType = typeof(Resources))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_MainPhoneNumberPrefix", NameResourceType = typeof(Resources))]
        public decimal MainPhoneCountryID { get; set; }

        [LocalizedDisplayName("CustomerInscriptionModel_MainPhoneNumberPrefix", NameResourceType = typeof(Resources))]
        [UIHint("CountryPrefixSelector")]
        public CountryDataModel MainPhoneCountry { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_MainPhoneNumber", NameResourceType = typeof(Resources))]
        public string MainPhoneNumber { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_AlternativePhoneNumberPrefix", NameResourceType = typeof(Resources))]
        public decimal? AlternativePhoneCountryID { get; set; }

        [UIHint("CountryPrefixSelectorOptional")]
        [LocalizedDisplayName("CustomerInscriptionModel_AlternativePhoneNumberPrefix", NameResourceType = typeof(Resources))]
        public CountryDataModel AlternativePhoneCountry { get; set; }

        [RegularExpression(@"^[0-9]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_AlternativePhoneNumber", NameResourceType = typeof(Resources))]
        public string AlternativePhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public decimal CurrencyID { get; set; }

        [UIHint("Currency")]
        public CurrencyDataModel Currency { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("UserData_CurrentPassword", NameResourceType = typeof(Resources))]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("UserData_NewPassword", NameResourceType = typeof(Resources))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("UserData_ConfirmNewPassword", NameResourceType = typeof(Resources))]
        public string ConfirmNewPassword { get; set; }


        /*
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_Name", NameResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_FirstSurname", NameResourceType = typeof(Resources))]
        public string Surname1 { get; set; }

        [DataType(DataType.Text)]
        //[RegularExpression(@"^[\p{L}\p{N}. &'-]*", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
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

         
        [LocalizedDisplayName("UserData_Plates", NameResourceType = typeof(Resources))]
        public IList<SelectListItem> Plates { get; set; }

        public string platesChanges { get; set; }

        public void Fill(USER oUser, bool bApplyChanges)
        {

            Plates = oUser.USER_PLATEs.Where(a => a.USRP_ENABLED == 1).
                               Select(a =>
                                  new SelectListItem
                                  {
                                      Text = a.USRP_PLATE,
                                      Value = a.USRP_ID.ToString(),
                                      Selected = false
                                  }).ToList();


            if (bApplyChanges)
            {
                if (!string.IsNullOrEmpty(platesChanges))
                {

                    int iMaxValue = -1;

                    foreach (SelectListItem item in Plates)
                    {
                        if (Convert.ToInt32(item.Value) > iMaxValue)
                        {
                            iMaxValue = Convert.ToInt32(item.Value);
                        }
                    }

                    iMaxValue++;


                    string strPlatesChanges = platesChanges.Remove(0, 1);

                    char[] delimiterChars = { '#' };

                    string[] strMovs = strPlatesChanges.Split(delimiterChars);

                    foreach (string strMov in strMovs)
                    {
                        char[] movDelimiterChars = { ':' };
                        string[] strPlateMovData = strMov.Split(movDelimiterChars);

                        if (strPlateMovData.Length == 2)
                        {
                            switch (strPlateMovData[0])
                            {
                                case "I":
                                    Plates.Add(new SelectListItem
                                    {
                                        Text = strPlateMovData[1],
                                        Value = iMaxValue.ToString(),
                                        Selected = false
                                    });
                                    iMaxValue++;
                                    break;
                                case "D":
                                    foreach (SelectListItem item in Plates)
                                    {
                                        if (item.Text == strPlateMovData[1])
                                        {
                                            Plates.Remove(item);
                                            break;
                                        }

                                    }
                                    break;
                                default:
                                    break;

                            }
                        }
                    }

                }
            }

        }*/

    }

    public class CountryDataModel
    {

        public CountryDataModel()
        {
        }
        public CountryDataModel(COUNTRy domCountry)
        {
            CountryID = domCountry.COU_ID;
            Description = domCountry.COU_DESCRIPTION;
            Code = domCountry.COU_CODE;
            TelPrefix = domCountry.COU_TEL_PREFIX;
            Currency = new CurrencyDataModel()
            {
                CurrencyID = domCountry.CURRENCy.CUR_ID,
                Name = domCountry.CURRENCy.CUR_NAME
            };
        }

        public decimal? CountryID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]        
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        //[LocalizedDisplayName("CustomerInscriptionModel_Username", NameResourceType = typeof(Resources))]
        [DisplayName("Descrption")]
        [UIHint("TextEditorTemplate")]
        public string Description { get; set; }
        
        [DataType(DataType.Text)]
        [StringLength(10, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        //[LocalizedDisplayName("CustomerInscriptionModel_Username", NameResourceType = typeof(Resources))]
        [DisplayName("Code")]
        [UIHint("TextEditorTemplate")]
        public string Code { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        //[LocalizedDisplayName("CustomerInscriptionModel_Username", NameResourceType = typeof(Resources))]
        [DisplayName("Telephon prefix")]
        [UIHint("TextEditorTemplate")]
        public string TelPrefix { get; set; }

        public decimal? CurrencyID { get; set; }
        
        [UIHint("Currency")]        
        public CurrencyDataModel Currency { get; set; }

        public bool SetDomain(SQLDataRepository dataRepository, bool remove = false)
        {
            bool bRes = true;

            try
            {
                COUNTRy oCountry = new COUNTRy();
                if (this.CountryID.HasValue) oCountry.COU_ID = this.CountryID.Value;
                oCountry.COU_DESCRIPTION = this.Description;
                oCountry.COU_CODE = this.Code;
                oCountry.COU_TEL_PREFIX = this.TelPrefix;
                if (this.Currency != null)
                    oCountry.COU_CUR_ID = this.Currency.CurrencyID;
                else
                    oCountry.COU_CUR_ID = null;

                if (!remove)
                {
                    bRes = dataRepository.UpdateCountry(ref oCountry);
                    if (bRes) this.CountryID = oCountry.COU_ID;
                }
                else
                    bRes = dataRepository.DeleteCountry(ref oCountry);

            }
            catch (Exception e)
            {
                bRes = false;

            }

            return bRes;
        }

    }

    public class CurrencyDataModel
    {
        public decimal? CurrencyID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]        
        [DisplayName("Currency")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [StringLength(10, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]        
        [DisplayName("ISO Code")]
        public string IsoCode { get; set; }        
    }

    /*public class CountryCurrencyDataModel
    {
        public decimal? CurrencyID { get; set; }
        
        [DisplayName("Currency")]
        public string CurrencyName { get; set; }
    }*/

    public class GroupDataModel
    {
        public decimal? GroupId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [DisplayName("Description")]
        public string Description{ get; set; }
    }

    public class TariffDataModel
    {
        public decimal? TariffId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [DisplayName("Description")]
        public string Description { get; set; }
    }

    public class ServiceChargeTypeModel
    {
        public int ServiceChargeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [DisplayName("Description")]
        public string Description { get; set; }
    }

    public class ChargeOperationTypeModel
    {
        public int ChargeOperationTypeId { get; set; }
        public string Description { get; set; }
    }

}