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

        /*public PaymentSuscryptionType SuscryptionType { get; set; }

        public bool PaymentMeanTypeEnabled(PaymentMeanType paymentMean)
        {

        }*/

        public bool PaymentMeanTypeEnabled(PaymentSuscryptionType suscryptionType, ICustomersRepository customersRepository,IInfraestructureRepository infraestructureRepository,
                                            ref USER oUser,PaymentMeanType paymentMean)
        {
            bool bRet = true;

            string sSuscriptionType = "";
            RefundBalanceType eRefundBalType = RefundBalanceType.rbtAmount;
            customersRepository.GetUserPossibleSuscriptionTypes(ref oUser, infraestructureRepository, out sSuscriptionType, out eRefundBalType);

            if (!string.IsNullOrWhiteSpace(sSuscriptionType))
            {
                bRet = (sSuscriptionType == ((int)suscryptionType).ToString());
            }

            if (bRet)
            {
                string sPaymentMeanType = ConfigurationManager.AppSettings[string.Format("PaymentMeanType_{0}", Enum.GetName(typeof(PaymentSuscryptionType), suscryptionType))] ?? "";
                if (!string.IsNullOrWhiteSpace(sPaymentMeanType))
                {
                    bRet = ((Int32.Parse(sPaymentMeanType) & (int)paymentMean) == (int)paymentMean);
                }
            }
            return bRet;
        }
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

        public decimal PercVAT1 { get; set; }
        public decimal PercVAT2 { get; set; }
        public decimal PercFEE { get; set; }
        public decimal PercFEETopped { get; set; }
        public decimal FixedFEE { get; set; }

    }



    public class OperationRowModel
    {

        public int TypeId { get; set; }
        public string Type { get; set; }        
        public string Installation { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateIni { get; set; }
        public DateTime? DateEnd { get; set; }
        public double Amount { get; set; }
        public double AmountFEE { get; set; }
        public double AmountBONUS { get; set; }
        public double AmountVAT { get; set; }
        public double AmountTotal { get; set; }
        public string CurrencyIsoCode { get; set; }
        public int? Time { get; set; }
        public double? ChangeApplied { get; set; }
        public int? PlateId { get; set; }
        public int? PlateId2 { get; set; }
        public int? PlateId3 { get; set; }
        public int? PlateId4 { get; set; }
        public int? PlateId5 { get; set; }
        public int? PlateId6 { get; set; }
        public int? PlateId7 { get; set; }
        public int? PlateId8 { get; set; }
        public int? PlateId9 { get; set; }
        public int? PlateId10 { get; set; }
        public string Plate { get; set; }
        public string Plate2 { get; set; }
        public string Plate3 { get; set; }
        public string Plate4 { get; set; }
        public string Plate5 { get; set; }
        public string Plate6 { get; set; }
        public string Plate7 { get; set; }
        public string Plate8 { get; set; }
        public string Plate9 { get; set; }
        public string Plate10 { get; set; }
        public string Plates { get; set; }
        public string TicketNumber { get; set; }
        public string TicketData { get; set; }
        public string Sector { get; set; }
        public string Tariff { get; set; }
        public string Source { get; set; }
        public string AmountStr { get; set; }
        public string AmountFEEStr { get; set; }
        public string AmountBONUSStr { get; set; }
        public string AmountVATStr { get; set; }
        public string AmountTotalStr { get; set; }
        public string AdditionalUser { get; set; }
        public int? CurrencyMinorUnit { get; set; }

        public void RecalculateAmountStrings()
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            AmountStr = string.Format(provider, "{0:" + GetDecimalFormatFromIsoCode(CurrencyMinorUnit) + "} {1}", Convert.ToDouble(Amount) / GetCurrencyDivisorFromIsoCode(CurrencyMinorUnit), CurrencyIsoCode);
            AmountFEEStr = string.Format(provider, "{0:" + GetDecimalFormatFromIsoCode(CurrencyMinorUnit) + "} {1}", Convert.ToDouble(AmountFEE) / GetCurrencyDivisorFromIsoCode(CurrencyMinorUnit), CurrencyIsoCode);
            AmountBONUSStr = string.Format(provider, "{0:" + GetDecimalFormatFromIsoCode(CurrencyMinorUnit) + "} {1}", Convert.ToDouble(AmountBONUS) / GetCurrencyDivisorFromIsoCode(CurrencyMinorUnit), CurrencyIsoCode);
            AmountVATStr = string.Format(provider, "{0:" + GetDecimalFormatFromIsoCode(CurrencyMinorUnit) + "} {1}", Convert.ToDouble(AmountVAT) / GetCurrencyDivisorFromIsoCode(CurrencyMinorUnit), CurrencyIsoCode);
            AmountTotalStr = string.Format(provider, "{0:" + GetDecimalFormatFromIsoCode(CurrencyMinorUnit) + "} {1}", Convert.ToDouble(AmountTotal) / GetCurrencyDivisorFromIsoCode(CurrencyMinorUnit), CurrencyIsoCode);

        }
        private string GetDecimalFormatFromIsoCode(int? iCurMinorUnit)
        {
            string strRes = "0.##";

            if (iCurMinorUnit.HasValue && iCurMinorUnit > 0)
            {
                strRes = "0.";
                for (int i = 0; i < iCurMinorUnit; i++)
                {
                    strRes += "0";
                }

            }

            return strRes;
        }


        private int GetCurrencyDivisorFromIsoCode(int? iCurMinorUnit)
        {
            return Convert.ToInt32(Math.Pow(10, Convert.ToDouble(iCurMinorUnit ?? 0)));
        }


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
        
        public GridSortOptions CurrentGridSortOptions { get; set; }

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
                                       Text = Resources.Permits_Permit,
                                       Value = Convert.ToInt32(ChargeOperationsType.Permit).ToString(),
                                       Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.Permit))
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
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_OffstreetEntry,
                                        Value = Convert.ToInt32(ChargeOperationsType.OffstreetEntry).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.OffstreetEntry))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_OffstreetExit,
                                        Value = Convert.ToInt32(ChargeOperationsType.OffstreetExit).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.OffstreetExit))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_OffstreetOverduePayment,
                                        Value = Convert.ToInt32(ChargeOperationsType.OffstreetOverduePayment).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.OffstreetOverduePayment))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_BalanceTransfer,
                                        Value = Convert.ToInt32(ChargeOperationsType.BalanceTransfer).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.BalanceTransfer))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_BalanceReception,
                                        Value = Convert.ToInt32(ChargeOperationsType.BalanceReception).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.BalanceReception))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_TollPayment,
                                        Value = Convert.ToInt32(ChargeOperationsType.TollPayment).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.TollPayment))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_TollLock,
                                        Value = Convert.ToInt32(ChargeOperationsType.TollLock).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.TollLock))
                                    },
                new SelectListItem
                                    {
                                        Text =  Resources.Account_Op_Type_TollUnlock,
                                        Value = Convert.ToInt32(ChargeOperationsType.TollUnlock).ToString(),
                                        Selected = (selectedType==Convert.ToInt32(ChargeOperationsType.TollUnlock))
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




    public class InvoiceRowModel
    {
        public decimal InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? Date { get; set; }
        public double Amount { get; set; }
        public double? AmountOps { get; set; }
        public string CurrencyIsoCode { get; set; }
        public string DownloadURL { get; set; }
    }



    public class InvoiceFilterModel
    {

        [LocalizedDisplayName("Account_Invoice_Start_Date", NameResourceType = typeof(Resources))]
        public DateTime? DateIni { get; set; }

        [LocalizedDisplayName("Account_Invoice_End_Date", NameResourceType = typeof(Resources))]
        public DateTime? DateEnd { get; set; }

        private DateTime? currentDateIni;
        private DateTime? currentDateEnd;

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

    }


    public class InvoicesListContainerViewModel
    {
        public IPagination<InvoiceRowModel> InvoicesPagedList { get; set; }
        public InvoiceFilterModel InvoicesFilterViewModel { get; set; }
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

        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("CustomerInscriptionModel_MainPhoneNumberPrefix", NameResourceType = typeof(Resources))]
        public string MainPhoneNumberPrefix { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
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

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_PasswordsMustMatch")]
    public class ResetPasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("ResetPassword_Password", NameResourceType = typeof(Resources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("ResetPassword_ConfirmPassword", NameResourceType = typeof(Resources))]
        public string ConfirmPassword { get; set; }
    }


    #endregion


}
