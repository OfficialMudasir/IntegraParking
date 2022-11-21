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
using integraMobile.Infrastructure;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace kendoTest.Models
{

    #region Models

    public class SelectPayMethodModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_PaymentMean", NameResourceType = typeof(Resources))]
        public string PaymentMean { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_AutomaticRecharge", NameResourceType = typeof(Resources))]
        public bool AutomaticRecharge { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_AutomaticRechargeQuantity", NameResourceType = typeof(Resources))]
        public string AutomaticRechargeQuantity { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("CustomerInscriptionModel_AutomaticRechargeWhenBelowQuantity", NameResourceType = typeof(Resources))]
        public string AutomaticRechargeWhenBelowQuantity { get; set; }

    }


    public class SelectSuscriptionTypeModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_SuscriptionType", NameResourceType = typeof(Resources))]
        public int? SuscriptionType { get; set; }        

    }

    public class RechargeModel
    {
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Cantidad a Recargar")]
        public string RechargeQuantity { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_AutomaticRecharge", NameResourceType = typeof(Resources))]
        public bool AutomaticRecharge { get; set; }

        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_AutomaticRechargeQuantity", NameResourceType = typeof(Resources))]
        public string AutomaticRechargeQuantity { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("CustomerInscriptionModel_AutomaticRechargeWhenBelowQuantity", NameResourceType = typeof(Resources))]
        public string AutomaticRechargeWhenBelowQuantity { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [LocalizedDisplayName("CustomerInscriptionModel_AutomaticRechargePaypalId", NameResourceType = typeof(Resources))]
        public string PaypalID { get; set; }

    }



    public class OperationRowModel
    {

        //[DisplayName(Resources.Account_Op_Operation)]
        public int TypeId { get; set; }
        public string Type { get; set; }        
        public string Installation { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateIni { get; set; }
        public DateTime? DateEnd { get; set; }
        public double Amount { get; set; }
        public string CurrencyIsoCode { get; set; }
        public int? Time { get; set; }
        public double? ChangeApplied { get; set; }
        public int? PlateId { get; set; }
        public string Plate { get; set; }
        public string TicketNumber { get; set; }
        public string TicketData { get; set; }

    }


    public class OperationFilterModel
    {

        [LocalizedDisplayName("Account_Op_Operation", NameResourceType = typeof(Resources))]
        public int? Type { get; set; }

        [LocalizedDisplayName("Account_Op_Start_Date", NameResourceType = typeof(Resources))]
        public DateTime? DateIni { get; set; }

        [LocalizedDisplayName("Account_Op_End_Date", NameResourceType = typeof(Resources))]        
        public DateTime? DateEnd { get; set; }

        [LocalizedDisplayName("Account_Op_LicensePlate", NameResourceType = typeof(Resources))]
        public int? Plate { get; set; }

        private int selectedType;
        private int selectedPlate;
        private DateTime? currentDateIni;
        private DateTime? currentDateEnd;

        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> Plates { get; set; }

        public int SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
            }
        }


        public int SelectedPlate
        {
            get
            {
                return selectedPlate;
            }
            set
            {
                selectedPlate = value;
            }
        }


        public DateTime? CurrentDateIni
        {
            get
            {
                return currentDateIni;
            }
            set
            {
                currentDateIni = value;
            }
        }

        public DateTime? CurrentDateEnd
        {
            get
            {
                return currentDateEnd;
            }
            set
            {
                currentDateEnd = value;
            }
        }

        public void Fill(USER oUser)
        {

                /*
                public enum ChargeOperationsType
                {
                    ParkingOperation=1,
                    ExtensionOperation=2,
                    ParkingRefund=3,
                    TicketPayment=4,
                    BalanceRecharge=5,
                    ServiceCharge=6,
                    Discount=7
                }*/

            Types = new SelectListItem[] 
            {
                new SelectListItem
                                   {
                                       Text = Resources.Account_Op_Type_Parking,
                                       Value = Convert.ToInt32(ChargeOperationsType.ParkingOperation).ToString(),
                                       Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ParkingOperation))
                                   },
                new SelectListItem
                                   {
                                       Text = Resources.Account_Op_Type_Extension,
                                       Value = Convert.ToInt32(ChargeOperationsType.ExtensionOperation).ToString(),
                                       Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ExtensionOperation))
                                   },
                new SelectListItem
                                   {
                                       Text = Resources.Account_Op_Type_Refund,
                                       Value = Convert.ToInt32(ChargeOperationsType.ParkingRefund).ToString(),
                                       Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ParkingRefund))
                                   },
                new SelectListItem
                                    {
                                        Text = Resources.Account_Op_Type_TicketPayment,
                                        Value = Convert.ToInt32(ChargeOperationsType.TicketPayment).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.TicketPayment))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_Recharge,
                                        Value = Convert.ToInt32(ChargeOperationsType.BalanceRecharge).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.BalanceRecharge))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_ServiceCharge,
                                        Value = Convert.ToInt32(ChargeOperationsType.ServiceCharge).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.ServiceCharge))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_Discount,
                                        Value = Convert.ToInt32(ChargeOperationsType.Discount).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.Discount))
                                    }

            
            
            };
           
            Plates =  oUser.USER_PLATEs.Where(a => a.USRP_ENABLED == 1).
                            Select(a =>
                               new SelectListItem
                               {
                                   Text = a.USRP_PLATE,
                                   Value = a.USRP_ID.ToString(),
                                   Selected = (a.USRP_ID == selectedPlate)
                               }).ToList();

        }
        


    }

    public class OperationsListContainerViewModel
    {
        public IPagination<OperationRowModel> OperationsPagedList { get; set; }
        public OperationFilterModel OperationsFilterViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }


    [PropertiesMustMatch("NewPassword", "ConfirmNewPassword", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_PasswordsMustMatch")]
    public class UserDataModel
    {

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

        [LocalizedDisplayName("UserData_Plates", NameResourceType = typeof(Resources))]
        public IList<SelectListItem> Plates { get; set; }

        public string platesChanges { get; set; }

        public void Fill(USER oUser,bool bApplyChanges)
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

        }
    }




    public class ChangeEmailOrMobileModel
    {


        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("CustomerInscriptionModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }

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

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("UserData_CurrentPassword", NameResourceType = typeof(Resources))]
        public string CurrentPassword { get; set; }


    }

    public class DeleteUserModel
    {




        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("UserData_DeleteUser_IntroducePassword", NameResourceType = typeof(Resources))]
        public string CurrentPassword { get; set; }

        public bool ConfirmDeletion { get; set; }

    }



    public class ForgotPasswordModel
    {

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("Home_Username", NameResourceType = typeof(Resources))]
        public string Username { get; set; }
    }



    #endregion


}
