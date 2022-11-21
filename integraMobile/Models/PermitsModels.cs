using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using System.Globalization;
using MvcContrib.Pagination;
using integraMobile.Properties;
using System.Configuration;
using integraMobile.Domain.Concrete;

namespace integraMobile.Models
{
    [Serializable()]
    public class PermitsModel : ICloneable
    {
        public List<Country> Countries;
        public List<Plate> Plates { get; set; }
        public List<Tariff> TariffList { get; set; }
        public Dictionary<decimal, string> Cities = new Dictionary<decimal, string>();
        public Dictionary<decimal, string> Zones = new Dictionary<decimal, string>();
        public Dictionary<decimal, string> Tariffs = new Dictionary<decimal, string>();
        public Dictionary<string, TimeStep> TimeSteps = new Dictionary<string, TimeStep>();
        public PaymentParams PaymentParams { get; set; }
        public PaymentReturnData PaymentReturn { get; set; }
        public int PaymentMethod { get; set; }
        public int PaypertransactionAvailable { get; set; }
        public int PrepaymentAvailable { get; set; }
        public decimal? BackofficeUserCountry { get; set; }
        public bool FromLogin { get; set; }
        public bool InFrame { get; set; }
        public string SessionId { get; set; }
        public string Error { get; set; }
        public string UserName { get; set; }
        public string UserNameForSummary { get; set; }
        public string UserCurrency { get; set; }
        public string HashSeed { get; set; }
        public string CCProvider { get; set; }
        public string SignupGuid { get; set; }
        public string UTC_Offset { get; set; }
        public decimal Country { get; set; }
        public decimal? OperationId { get; set; }
        public string Email { get; set; }
        public decimal CountryPrefix { get; set; }
        public string CellNumber { get; set; }
        public string Password { get; set; }
        public decimal SubscriptionType { get; set; }
        public decimal City { get; set; }
        public decimal Zone { get; set; }
        public List<string> PlateCollection { get; set; }
        public string LicensePlate1 { get; set; }
        public string LicensePlate2 { get; set; }
        public string LicensePlate3 { get; set; }
        public string LicensePlate4 { get; set; }
        public string LicensePlate5 { get; set; }
        public string LicensePlate6 { get; set; }
        public string LicensePlate7 { get; set; }
        public string LicensePlate8 { get; set; }
        public string LicensePlate9 { get; set; }
        public string LicensePlate10 { get; set; }
        public string CurrentMonth { get; set; }
        public decimal Tariff { get; set; }
        public string TimeStep { get; set; }
        public decimal User { get; set; }
        public int? MaxLicensePlates { get; set; }

        public decimal? SelectedCity { get; set; }
        public decimal? SelectedZone { get; set; }
        public string SelectedMonth { get; set; }        
        public string SelectedPlate1 { get; set; }
        public string SelectedPlate2 { get; set; }
        public string SelectedPlate3 { get; set; }
        public string SelectedPlate4 { get; set; }
        public string SelectedPlate5 { get; set; }
        public string SelectedPlate6 { get; set; }
        public string SelectedPlate7 { get; set; }
        public string SelectedPlate8 { get; set; }
        public string SelectedPlate9 { get; set; }
        public string SelectedPlate10 { get; set; }
        public decimal? SelectedTariff { get; set; }
        public int? SelectedNumPermits { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Serializable()]
    public class Country
    {
        public decimal id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Prefix { get; set; }
    }

    public class Zone
    {
        public decimal id { get; set; }
        public string Name { get; set; }
        public int? MaxMonths { get; set; }
        public int? MaxBuyDay { get; set; }
    }

    public class Month
    {
        public string id { get; set; }
        public string Name { get; set; }
    }

    public class NumPermits
    {
        public int id { get; set; }
        public string Name { get; set; }

        public NumPermits(int iId, string iName) 
        {
            id = iId;
            Name = iName;
        }
    }

    [Serializable()]
    public class Tariff
    {
        public decimal id { get; set; }
        public string Name { get; set; }
        public int? MaxLicensePlates { get; set; }
        public int MaxBuyOnce { get; set; }
    }

    [Serializable()]
    public class Plate
    {
        public string id { get; set; }
        public string LicensePlate { get; set; }
    }

    [Serializable()]
    public class TimeStep
    {
        public string EndDate { get; set; }
        public decimal per_bon { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountFee { get; set; }
        public decimal q_fee_plus_vat { get; set; }
        public decimal q_plus_vat { get; set; }
        public decimal q_subtotal { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal AmountVat { get; set; }
        public decimal AmountWithoutBon { get; set; }
        public decimal RealAmount { get; set; }
        public decimal MinimumTime { get; set; }
        public decimal TimeBalanceUsed { get; set; }
        public string InitialDate { get; set; }
        public decimal? qch_total { get; set; }
        public string text { get; set; }
    }

    [Serializable()]
    public class PaymentParams
    {
        public string RequestURL { get; set; }
        public string Guid { get; set; }
        public string Email { get; set; }
        public int Amount { get; set; }
        public string CurrencyISOCODE { get; set; }
        public string Description { get; set; }
        public string UTCDate { get; set; }
        public string Culture { get; set; }
        public string ReturnURL { get; set; }
        public string Hash { get; set; }
    }

    [Serializable()]
    public class PaymentReturnData
    {
        /* Common */
        public string Email { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Result { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        /* CreditCall */
        public string ekashu_reference { get; set; }
        public string ekashu_auth_code { get; set; }
        public string ekashu_auth_result { get; set; }
        public string ekashu_card_hash { get; set; }
        public string ekashu_card_reference { get; set; }
        public string ekashu_card_scheme { get; set; }
        public string ekashu_date_time_local_fmt { get; set; }
        public string ekashu_masked_card_number { get; set; }
        public string ekashu_transaction_id { get; set; }
        public string ekashu_expires_end_month { get; set; }
        public string ekashu_expires_end_year { get; set; }
        /* IECISA */
        public string CardToken { get; set; }
        public string CardHash { get; set; }
        public string CardScheme { get; set; }
        public string CardPAN { get; set; }
        public string CardExpirationDate { get; set; }
        public string ChargeDateTime { get; set; }
        public string CardCFAuthCode { get; set; }
        public string CardCFTicketNumber { get; set; }
        public string CardCFTransactionID { get; set; }
        public string CardTransactionID { get; set; }
        /* Stripe */
        public string stripe_customer_id { get; set; }
        public string stripe_card_reference { get; set; }
        public string stripe_card_scheme { get; set; }
        public string stripe_masked_card_number { get; set; }
        public string stripe_expires_end_month { get; set; }
        public string stripe_expires_end_year { get; set; }
        public string stripe_transaction_id { get; set; }
        public string stripe_date_time_utc { get; set; }
        /* Moneris */
        public string moneris_card_reference { get; set; }
        public string moneris_card_hash { get; set; }
        public string moneris_card_scheme { get; set; }
        public string moneris_masked_card_number { get; set; }
        public string moneris_expires_end_month { get; set; }
        public string moneris_expires_end_year { get; set; }
        public string moneris_date_time_local_fmt { get; set; }
        public string moneris_reference { get; set; }
        public string moneris_transaction_id { get; set; }
        public string moneris_auth_code { get; set; }
        public string moneris_auth_result { get; set; }
        /* Transbank */
        public string transbank_reference { get; set; }
        public string transbank_auth_code { get; set; }
        public string transbank_card_hash { get; set; }
        public string transbank_card_reference { get; set; }
        public string transbank_card_scheme { get; set; }
        public string transbank_date_time_local_fmt { get; set; }
        public string transbank_masked_card_number { get; set; }
        public string transbank_transaction_id { get; set; }
        /* Payu */
        public string payu_reference { get; set; }
        public string payu_auth_code { get; set; }
        public string payu_card_hash { get; set; }
        public string payu_card_reference { get; set; }
        public string payu_card_scheme { get; set; }
        public string payu_date_time_local_fmt { get; set; }
        public string payu_masked_card_number { get; set; }
        public string payu_transaction_id { get; set; }
        /* BSRedsys */
        public string bsredsys_reference { get; set; }
        public string bsredsys_auth_code { get; set; }
        public string bsredsys_auth_result { get; set; }
        public string bsredsys_card_hash { get; set; }
        public string bsredsys_card_reference { get; set; }
        public string bsredsys_card_scheme { get; set; }
        public string bsredsys_date_time_local_fmt { get; set; }
        public string bsredsys_masked_card_number { get; set; }
        public string bsredsys_transaction_id { get; set; }
        public string bsredsys_expires_end_month { get; set; }
        public string bsredsys_expires_end_year { get; set; }
        /* PayPal */
        public string paypal_PayerID { get; set; }
        public string paypal_paymentId { get; set; }
        public string paypal_token { get; set; }
    }

    public class PermitFilterModel
    {
        private string cs = ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ToString();
        private ICustomersRepository customersRepository;

        [LocalizedDisplayName("PermitsDataModel_Tariff", NameResourceType = typeof(Resources))]
		public int? Type { get; set; }
        [LocalizedDisplayName("Account_Op_Start_Date", NameResourceType = typeof(Resources))]
        public DateTime? DateIni { get; set; }
        [LocalizedDisplayName("Account_Op_End_Date", NameResourceType = typeof(Resources))]        
        public DateTime? DateEnd { get; set; }
        [LocalizedDisplayName("PermitsDataModel_Plate", NameResourceType = typeof(Resources))]
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
            customersRepository = new SQLCustomersRepository(cs);
            var predicate = PredicateBuilder.True<VW_ACTIVE_PERMIT_OPERATION>();

            IQueryable<VW_ACTIVE_PERMIT_OPERATION> ActivePermits = customersRepository.GetActivePermits(ref oUser, predicate, "TAR_DESCRIPTION", "ASC");

            Types = ActivePermits
                        .Select(domOp => (new SelectListItem
                        {
                            Text = domOp.TAR_DESCRIPTION,
                            Value = domOp.OPE_TAR_ID.ToString(),
                            Selected = (domOp.OPE_TAR_ID == selectedType)
                        }))
                        .GroupBy(g => g.Value).Select(x => x.FirstOrDefault());

            Plates = ActivePermits
                        .Select(domOp => (new SelectListItem
                        {
                            Text = domOp.USRP_PLATE,
                            Value = domOp.USRP_ID.ToString(),
                            Selected = (domOp.USRP_ID == selectedPlate)
                        }))
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE2,
                                    Value = domOp.OPE_PLATE2_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE2_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE3,
                                    Value = domOp.OPE_PLATE3_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE3_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE4,
                                    Value = domOp.OPE_PLATE4_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE4_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE5,
                                    Value = domOp.OPE_PLATE5_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE5_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE6,
                                    Value = domOp.OPE_PLATE6_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE6_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE7,
                                    Value = domOp.OPE_PLATE7_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE7_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE8,
                                    Value = domOp.OPE_PLATE8_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE8_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE9,
                                    Value = domOp.OPE_PLATE9_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE9_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        )
                        .Union(ActivePermits
                                .Select(domOp => (new SelectListItem
                                {
                                    Text = domOp.USRP_PLATE10,
                                    Value = domOp.OPE_PLATE10_USRP_ID.ToString(),
                                    Selected = (domOp.OPE_PLATE10_USRP_ID == selectedPlate)
                                }))
                                .Where(d => d.Text != null && d.Text.Trim() != null)
                        );
            Plates = Plates.GroupBy(g => g.Value).Select(x => x.FirstOrDefault());
        }
    }

    public class PermitRowHelperModel
    {
        public string Key { get; set; }
        public decimal Id { get; set; }
        public DateTime DateIni { get; set; }
        public bool RenewAutomatically { get; set; }
        public int UTC_Offset { get; set; }
        public int MaxMonths { get; set; }
    }

    public class PermitRowModel
    {
        public decimal Id { get; set; }
        public int TariffId { get; set; }
        public string Tariff { get; set; }
        public DateTime? DateIni { get; set; }
        public DateTime? DateEnd { get; set; }
        public double Amount { get; set; }
        public string AmountStr { get; set; }
        public string GrpDescription { get; set; }
        public string CurrencyIsoCode { get; set; }
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
        public int? CurrencyMinorUnit { get; set; }
        public bool RenewAutomatically { get; set; }
        public decimal CityId { get; set; }
        public decimal? GrpId { get; set; }
        public bool PaymentDisabled { get; set; }
        public bool AutoRenewalDisabled { get; set; }
        public int MaxMonths { get; set; }
        public int UTC_Offset { get; set; }

        public void RecalculateAmountStrings()
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            AmountStr = string.Format(provider, "{0:" + GetDecimalFormatFromIsoCode(CurrencyMinorUnit) + "} {1}", Convert.ToDouble(Amount) / GetCurrencyDivisorFromIsoCode(CurrencyMinorUnit), CurrencyIsoCode);

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

    public class PermitListContainerViewModel
    {
        public IPagination<PermitRowModel> PermitPagedList { get; set; }
        public PermitFilterModel PermitFilterViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }

}