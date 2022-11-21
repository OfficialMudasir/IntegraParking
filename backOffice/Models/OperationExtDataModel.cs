using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Configuration;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class OperationExtDataModel
    {

        public OperationExtDataModel()
        {
        }

        public decimal Id { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_TypeId", NameResourceType = typeof(Resources))]
        public int TypeId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_Type", NameResourceType = typeof(Resources))]
        public ChargeOperationsType Type { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_UserId", NameResourceType = typeof(Resources))]
        public decimal? UserId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_UserId", NameResourceType = typeof(Resources))]
        public string Username { get; set; }

        //public UserDataModel User { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_MobileOSId", NameResourceType = typeof(Resources))]
        public int MobileOSId { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_AppVersion", NameResourceType = typeof(Resources))]
        public string AppVersion { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_InstallationId", NameResourceType = typeof(Resources))]
        public decimal? InstallationId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_Installation", NameResourceType = typeof(Resources))]
        public string Installation { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_InstallationShortDesc", NameResourceType = typeof(Resources))]
        public string InstallationShortDesc { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_Date", NameResourceType = typeof(Resources))]        
        //[Newtonsoft.Json.JsonConverter(typeof(JSONCustomDateConverter))]
        public DateTime? Date { get; set; }        
        [LocalizedDisplayName("OperationExtDataModel_DateUTC", NameResourceType = typeof(Resources))]
        public DateTime? DateUTC { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DateIni", NameResourceType = typeof(Resources))]
        public DateTime? DateIni { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DateEnd", NameResourceType = typeof(Resources))]
        public DateTime? DateEnd { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_Amount", NameResourceType = typeof(Resources))]
        public double? Amount { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_AmountCurrencyId", NameResourceType = typeof(Resources))]
        public decimal AmountCurrencyId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_AmountCurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string AmountCurrencyIsoCode { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_AmountFinal", NameResourceType = typeof(Resources))]
        public double? AmountFinal { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_Time", NameResourceType = typeof(Resources))]
        public int? Time { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_BalanceBefore", NameResourceType = typeof(Resources))]
        public double? BalanceBefore { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_BalanceCurrencyId", NameResourceType = typeof(Resources))]
        public decimal? BalanceCurrencyId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_BalanceCurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string BalanceCurrencyIsoCode { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_ChangeApplied", NameResourceType = typeof(Resources))]
        public double? ChangeApplied { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_PlateId", NameResourceType = typeof(Resources))]
        public decimal? PlateId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_Plate", NameResourceType = typeof(Resources))]
        public string Plate { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_TicketNumber", NameResourceType = typeof(Resources))]
        public string TicketNumber { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_TicketData", NameResourceType = typeof(Resources))]
        public string TicketData { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_SectorId", NameResourceType = typeof(Resources))]
        public decimal? SectorId { get; set; }
        //public string Sector { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_TariffId", NameResourceType = typeof(Resources))]
        public decimal? TariffId { get; set; }
        //public string Tariff { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_SuscriptionType", NameResourceType = typeof(Resources))]
        public int SuscriptionType { get; set; }        

        [LocalizedDisplayName("OperationExtDataModel_InsertionUTCDate", NameResourceType = typeof(Resources))]
        public DateTime? InsertionUTCDate { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_RechargeId", NameResourceType = typeof(Resources))]
        public decimal? RechargeId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_RechargeDate", NameResourceType = typeof(Resources))]
        public DateTime? RechargeDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_RechargeAmount", NameResourceType = typeof(Resources))]
        public double? RechargeAmount { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_RechargeAmountCurrencyId", NameResourceType = typeof(Resources))]
        public decimal? RechargeAmountCurrencyId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_RechargeAmountCurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string RechargeAmountCurrencyIsoCode { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_RechargeBalanceBefore", NameResourceType = typeof(Resources))]
        public double? RechargeBalanceBefore { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_RechargeInsertionUTCDate", NameResourceType = typeof(Resources))]
        public DateTime? RechargeInsertionUTCDate { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_DiscountId", NameResourceType = typeof(Resources))]
        public decimal? DiscountId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountDate", NameResourceType = typeof(Resources))]
        public DateTime? DiscountDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountAmount", NameResourceType = typeof(Resources))]
        public double? DiscountAmount { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountAmountCurrencyId", NameResourceType = typeof(Resources))]
        public decimal? DiscountAmountCurrencyId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountAmountCurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string DiscountAmountCurrencyIsoCode { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountAmountFinal", NameResourceType = typeof(Resources))]
        public double? DiscountAmountFinal { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountBalanceCurrencyId", NameResourceType = typeof(Resources))]
        public decimal? DiscountBalanceCurrencyId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountBalanceCurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string DiscountBalanceCurrencyIsoCode { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountBalanceBefore", NameResourceType = typeof(Resources))]
        public double? DiscountBalanceBefore { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountChangeApplied", NameResourceType = typeof(Resources))]
        public double? DiscountChangeApplied { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_DiscountInsertionUTCDate", NameResourceType = typeof(Resources))]
        public DateTime? DiscountInsertionUTCDate { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_ServiceChargeTypeId", NameResourceType = typeof(Resources))]
        public int? ServiceChargeTypeId { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_OpReference", NameResourceType = typeof(Resources))]
        public string OpReference { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_TransactionId", NameResourceType = typeof(Resources))]
        public string TransactionId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_AuthCode", NameResourceType = typeof(Resources))]
        public string AuthCode { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_CardHash", NameResourceType = typeof(Resources))]
        public string CardHash { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_CardReference", NameResourceType = typeof(Resources))]
        public string CardReference { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_CardScheme", NameResourceType = typeof(Resources))]
        public string CardScheme { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_MaskedCardNumber", NameResourceType = typeof(Resources))]
        public string MaskedCardNumber { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_CardExpirationDate", NameResourceType = typeof(Resources))]
        public DateTime? CardExpirationDate { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_ExternalId1", NameResourceType = typeof(Resources))]
        public string ExternalId1 { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_ExternalId2", NameResourceType = typeof(Resources))]
        public string ExternalId2 { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_ExternalId3", NameResourceType = typeof(Resources))]
        public string ExternalId3 { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_Latitude", NameResourceType = typeof(Resources))]
        public decimal? Latitude { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_Longitude", NameResourceType = typeof(Resources))]
        public decimal? Longitude { get; set; }

        [LocalizedDisplayName("OperationExtDataModel_OffstreetEntryDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetEntryDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetNotifyEntryDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetNotifyEntryDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetPaymentDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetPaymentDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetEndDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetEndDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetExitLimitDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetExitLimitDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetExitDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetExitDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetUTCEntryDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetUTCEntryDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetUTCNotifyEntryDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetUTCNotifyEntryDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetUTCPaymentDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetUTCPaymentDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetUTCEndDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetUTCEndDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetUTCExitLimitDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetUTCExitLimitDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetUTCExitDate", NameResourceType = typeof(Resources))]
        public DateTime? OffstreetUTCExitDate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetLogicalId", NameResourceType = typeof(Resources))]
        public string OffstreetLogicalId { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetTariff", NameResourceType = typeof(Resources))]
        public string OffstreetTariff { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetGate", NameResourceType = typeof(Resources))]
        public string OffstreetGate { get; set; }
        [LocalizedDisplayName("OperationExtDataModel_OffstreetSpaceDescription", NameResourceType = typeof(Resources))]
        public string OffstreetSpaceDescription { get; set; }

        public string TypeId_FK { get; set; }
        public string UserId_FK { get; set; }
        public string MobileOSId_FK { get; set; }
        public string AmountCurrencyId_FK { get; set; }
        public string BalanceCurrencyId_FK { get; set; }
        public string SectorId_FK { get; set; }
        public string TariffId_FK { get; set; }
        public string SuscriptionType_FK { get; set; }
        public string RechargeAmountCurrencyId_FK { get; set; }
        public string DiscountAmountCurrencyId_FK { get; set; }
        public string DiscountBalanceCurrencyId_FK { get; set; }
        public string ServiceChargeTypeId_FK { get; set; }

        public static IQueryable<OperationExtDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            string sInstallation = ConfigurationManager.AppSettings["InstallationShortDesc"];           
            if (!string.IsNullOrEmpty(sInstallation))
            {
                predicate = predicate.And(a => a.INS_SHORTDESC == sInstallation);
            }

            string sInfrastructureFilter = ConfigurationManager.AppSettings["InfrastructureFilter"] ?? "";
            if (!string.IsNullOrWhiteSpace(sInfrastructureFilter))
            {
                decimal dId = 0;
                string[] infraFilter = sInfrastructureFilter.Split(',').Select(id => (!decimal.TryParse(id, out dId) ? id : "G" + id)).ToArray();
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, infraFilter);
            }

            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ".";

            var modelOps = (from domOp in backOfficeRepository.GetOperationsExt(predicate)
                            select new OperationExtDataModel()
                            {
                                Id = domOp.OPE_ID,
                                TypeId = domOp.OPE_TYPE,
                                Type = (ChargeOperationsType)domOp.OPE_TYPE,
                                UserId = domOp.OPE_USR_ID,
                                Username = domOp.USR_USERNAME,
                                MobileOSId = domOp.OPE_MOSE_OS,
                                AppVersion = domOp.OPE_APP_VERSION,
                                InstallationId = domOp.OPE_INS_ID,
                                Installation = domOp.INS_DESCRIPTION,
                                InstallationShortDesc = domOp.INS_SHORTDESC,
                                Date = domOp.OPE_DATE,
                                DateUTC = domOp.OPE_UTC_DATE,
                                DateIni = domOp.OPE_INIDATE,
                                DateEnd = domOp.OPE_ENDDATE,
                                Amount = Convert.ToDouble(domOp.OPE_AMOUNT / 100.0),
                                AmountCurrencyId = domOp.OPE_AMOUNT_CUR_ID,
                                AmountCurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                                AmountFinal = Convert.ToDouble(domOp.OPE_FINAL_AMOUNT / 100.0),
                                Time = domOp.OPE_TIME,
                                BalanceBefore = Convert.ToDouble(domOp.OPE_BALANCE_BEFORE / 100.0),
                                BalanceCurrencyId = domOp.OPE_BALANCE_CUR_ID,
                                BalanceCurrencyIsoCode = domOp.OPE_BALANCE_CUR_ISO_CODE,
                                ChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                                PlateId = Convert.ToInt32(domOp.USRP_ID),
                                Plate = domOp.USRP_PLATE,
                                TicketNumber = domOp.TIPA_TICKET_NUMBER,
                                TicketData = domOp.TIPA_TICKET_DATA,
                                SectorId = domOp.GRP_ID,
                                TariffId = domOp.TAR_ID,
                                SuscriptionType = domOp.OPE_SUSCRIPTION_TYPE,
                                InsertionUTCDate = domOp.OPE_INSERTION_UTC_DATE,

                                RechargeId = domOp.OPE_CUSPMR_ID,
                                RechargeDate = domOp.CUSPMR_DATE,
                                RechargeAmount = Convert.ToDouble(domOp.CUSPMR_AMOUNT / 100.0),
                                RechargeAmountCurrencyId = domOp.CUSPMR_CUR_ID,
                                RechargeAmountCurrencyIsoCode = domOp.CUSPMR_AMOUNT_ISO_CODE,
                                RechargeBalanceBefore = Convert.ToDouble(domOp.CUSPMR_BALANCE_BEFORE / 100.0),
                                RechargeInsertionUTCDate = domOp.CUSPMR_INSERTION_UTC_DATE,

                                DiscountId = domOp.OPE_OPEDIS_ID,
                                DiscountDate = domOp.OPEDIS_DATE,
                                DiscountAmount = Convert.ToDouble(domOp.OPEDIS_AMOUNT / 100.0),
                                DiscountAmountCurrencyId = domOp.OPEDIS_AMOUNT_CUR_ID,
                                DiscountAmountCurrencyIsoCode = domOp.OPEDIS_AMOUNT_CUR_ISO_CODE,
                                DiscountAmountFinal = Convert.ToDouble(domOp.OPEDIS_FINAL_AMOUNT / 100.0),
                                DiscountBalanceCurrencyId = domOp.OPEDIS_BALANCE_CUR_ID,
                                DiscountBalanceCurrencyIsoCode = domOp.OPEDIS_BALANCE_CUR_ISO_CODE,
                                DiscountBalanceBefore = Convert.ToDouble(domOp.OPEDIS_BALANCE_BEFORE / 100.0),
                                DiscountChangeApplied = Convert.ToDouble(domOp.OPEDIS_CHANGE_APPLIED),
                                DiscountInsertionUTCDate = domOp.OPEDIS_INSERTION_UTC_DATE,

                                ServiceChargeTypeId = domOp.SECH_SECHT_ID,

                                OpReference = domOp.CUSPMR_OP_REFERENCE,        
                                TransactionId = domOp.CUSPMR_TRANSACTION_ID,        
                                AuthCode = domOp.CUSPMR_AUTH_CODE,        
                                CardHash = domOp.CUSPMR_CARD_HASH,            
                                CardReference = domOp.CUSPMR_CARD_REFERENCE,        
                                CardScheme = domOp.CUSPMR_CARD_SCHEME,        
                                MaskedCardNumber = domOp.CUSPMR_MASKED_CARD_NUMBER,        
                                CardExpirationDate = domOp.CUSPMR_CARD_EXPIRATION_DATE,

                                ExternalId1 = domOp.OPE_EXTERNAL_ID1,
                                ExternalId2 = domOp.OPE_EXTERNAL_ID2,
                                ExternalId3 = domOp.OPE_EXTERNAL_ID3,

                                Latitude = domOp.OPE_LATITUDE,
                                Longitude = domOp.OPE_LONGITUDE,

                                OffstreetEntryDate = domOp.OPEOFF_ENTRY_DATE,
                                OffstreetNotifyEntryDate = domOp.OPEOFF_NOTIFY_ENTRY_DATE,
                                OffstreetPaymentDate = domOp.OPEOFF_PAYMENT_DATE,
                                OffstreetEndDate = domOp.OPEOFF_END_DATE,
                                OffstreetExitLimitDate = domOp.OPEOFF_EXIT_LIMIT_DATE,
                                OffstreetExitDate = domOp.OPEOFF_EXIT_DATE,
                                OffstreetUTCEntryDate = domOp.OPEOFF_UTC_ENTRY_DATE,
                                OffstreetUTCNotifyEntryDate = domOp.OPEOFF_UTC_NOTIFY_ENTRY_DATE,
                                OffstreetUTCPaymentDate = domOp.OPEOFF_UTC_PAYMENT_DATE,
                                OffstreetUTCEndDate = domOp.OPEOFF_UTC_END_DATE,
                                OffstreetUTCExitLimitDate = domOp.OPEOFF_UTC_EXIT_LIMIT_DATE,
                                OffstreetUTCExitDate = domOp.OPEOFF_UTC_EXIT_DATE,
                                OffstreetLogicalId = domOp.OPEOFF_LOGICAL_ID,
                                OffstreetTariff = domOp.OPEOFF_TARIFF,
                                OffstreetGate = domOp.OPEOFF_GATE,
                                OffstreetSpaceDescription = domOp.OPEOFF_SPACE_DESCRIPTION,

                                TypeId_FK = ChargeOperationTypeDataModel.GetTypeIdString(domOp.OPE_TYPE),
                                //UserId_FK = UserDataModel.Get(users, domOp.OPE_USR_ID).Username,
                                MobileOSId_FK = MobileOSDataModel.GetTypeIdString(domOp.OPE_MOSE_OS),
                                AmountCurrencyId_FK = domOp.OPE_AMOUNT_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPE_AMOUNT_CUR_ID).Name,
                                BalanceCurrencyId_FK = domOp.OPE_BALANCE_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPE_BALANCE_CUR_ID).Name,
                                SectorId_FK = domOp.GRP_DESCRIPTION, // GroupDataModel.Get(domOp.GRP_ID, backOfficeRepository).Description,
                                TariffId_FK = domOp.TAR_DESCRIPTION, // TariffDataModel.Get(domOp.TAR_ID, backOfficeRepository).Description,
                                SuscriptionType_FK = PaymentSuscryptionTypeDataModel.GetTypeIdString(domOp.OPE_SUSCRIPTION_TYPE),
                                RechargeAmountCurrencyId_FK = domOp.CUSPMR_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.CUSPMR_CUR_ID).Name,
                                DiscountAmountCurrencyId_FK = domOp.OPEDIS_AMOUNT_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPEDIS_AMOUNT_CUR_ID).Name,
                                DiscountBalanceCurrencyId_FK = domOp.OPEDIS_BALANCE_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPEDIS_BALANCE_CUR_ID).Name,
                                ServiceChargeTypeId_FK = ServiceChargeTypeDataModel.GetTypeIdString(domOp.SECH_SECHT_ID)

                            }).AsQueryable();
            
            return modelOps;
        }

        public static IQueryable<OperationExtDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs)
        {
            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
            return List(backOfficeRepository, loadFKs, predicate);
        }

        public static IQueryable<OperationExtDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs, Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate)
        {
                        
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ".";
            IQueryable<OperationExtDataModel> modelOps;
            if (loadFKs)
            {
                modelOps = List(backOfficeRepository);
            }
            else
            {                
                string sInstallation = ConfigurationManager.AppSettings["InstallationShortDesc"];
                if (!string.IsNullOrEmpty(sInstallation))
                {
                    predicate = predicate.And(a => a.INS_SHORTDESC == sInstallation);
                }

                string sInfrastructureFilter = ConfigurationManager.AppSettings["InfrastructureFilter"] ?? "";
                if (!string.IsNullOrWhiteSpace(sInfrastructureFilter))
                {
                    decimal dId = 0;
                    string[] infraFilter = sInfrastructureFilter.Split(',').Select(id => (!decimal.TryParse(id, out dId)?id: "G" + id)).ToArray();                    
                    predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, infraFilter);
                }

                modelOps = (from domOp in backOfficeRepository.GetOperationsExt(predicate)
                            select new OperationExtDataModel()
                            {
                                Id = domOp.OPE_ID,
                                TypeId = domOp.OPE_TYPE,
                                Type = (ChargeOperationsType)domOp.OPE_TYPE,
                                UserId = domOp.OPE_USR_ID,
                                Username = domOp.USR_USERNAME,
                                MobileOSId = domOp.OPE_MOSE_OS,
                                AppVersion = domOp.OPE_APP_VERSION,
                                InstallationId = domOp.OPE_INS_ID,
                                Installation = domOp.INS_DESCRIPTION,
                                InstallationShortDesc = domOp.INS_SHORTDESC,
                                Date = domOp.OPE_DATE,
                                DateUTC  = domOp.OPE_UTC_DATE,
                                DateIni = domOp.OPE_INIDATE,
                                DateEnd = domOp.OPE_ENDDATE,
                                Amount = Convert.ToDouble(domOp.OPE_AMOUNT / 100.0),
                                AmountCurrencyId = domOp.OPE_AMOUNT_CUR_ID,
                                AmountCurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                                AmountFinal = Convert.ToDouble(domOp.OPE_FINAL_AMOUNT / 100.0),
                                Time = domOp.OPE_TIME,
                                BalanceBefore = Convert.ToDouble(domOp.OPE_BALANCE_BEFORE / 100.0),
                                BalanceCurrencyId = domOp.OPE_BALANCE_CUR_ID,
                                BalanceCurrencyIsoCode = domOp.OPE_BALANCE_CUR_ISO_CODE,
                                ChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                                PlateId = Convert.ToInt32(domOp.USRP_ID),
                                Plate = domOp.USRP_PLATE,
                                TicketNumber = domOp.TIPA_TICKET_NUMBER,
                                TicketData = domOp.TIPA_TICKET_DATA,
                                SectorId = domOp.GRP_ID,
                                TariffId = domOp.TAR_ID,
                                SuscriptionType = domOp.OPE_SUSCRIPTION_TYPE,
                                InsertionUTCDate = domOp.OPE_INSERTION_UTC_DATE,

                                RechargeId = domOp.OPE_CUSPMR_ID,
                                RechargeDate = domOp.CUSPMR_DATE,
                                RechargeAmount = Convert.ToDouble(domOp.CUSPMR_AMOUNT / 100.0),
                                RechargeAmountCurrencyId = domOp.CUSPMR_CUR_ID,
                                RechargeAmountCurrencyIsoCode = domOp.CUSPMR_AMOUNT_ISO_CODE,
                                RechargeBalanceBefore = Convert.ToDouble(domOp.CUSPMR_BALANCE_BEFORE / 100.0),
                                RechargeInsertionUTCDate = domOp.CUSPMR_INSERTION_UTC_DATE,

                                DiscountId = domOp.OPE_OPEDIS_ID,
                                DiscountDate = domOp.OPEDIS_DATE,
                                DiscountAmount = Convert.ToDouble(domOp.OPEDIS_AMOUNT / 100.0),
                                DiscountAmountCurrencyId = domOp.OPEDIS_AMOUNT_CUR_ID,
                                DiscountAmountCurrencyIsoCode = domOp.OPEDIS_AMOUNT_CUR_ISO_CODE,
                                DiscountAmountFinal = Convert.ToDouble(domOp.OPEDIS_FINAL_AMOUNT / 100.0),
                                DiscountBalanceCurrencyId = domOp.OPEDIS_BALANCE_CUR_ID,
                                DiscountBalanceCurrencyIsoCode = domOp.OPEDIS_BALANCE_CUR_ISO_CODE,
                                DiscountBalanceBefore = Convert.ToDouble(domOp.OPEDIS_BALANCE_BEFORE / 100.0),
                                DiscountChangeApplied = Convert.ToDouble(domOp.OPEDIS_CHANGE_APPLIED),
                                DiscountInsertionUTCDate = domOp.OPEDIS_INSERTION_UTC_DATE,

                                ServiceChargeTypeId = domOp.SECH_SECHT_ID,

                                OpReference = domOp.CUSPMR_OP_REFERENCE,
                                TransactionId = domOp.CUSPMR_TRANSACTION_ID,
                                AuthCode = domOp.CUSPMR_AUTH_CODE,
                                CardHash = domOp.CUSPMR_CARD_HASH,
                                CardReference = domOp.CUSPMR_CARD_REFERENCE,
                                CardScheme = domOp.CUSPMR_CARD_SCHEME,
                                MaskedCardNumber = domOp.CUSPMR_MASKED_CARD_NUMBER,
                                CardExpirationDate = domOp.CUSPMR_CARD_EXPIRATION_DATE,

                                ExternalId1 = domOp.OPE_EXTERNAL_ID1,
                                ExternalId2 = domOp.OPE_EXTERNAL_ID2,
                                ExternalId3 = domOp.OPE_EXTERNAL_ID3,

                                Latitude = domOp.OPE_LATITUDE,
                                Longitude = domOp.OPE_LONGITUDE,

                                OffstreetEntryDate = domOp.OPEOFF_ENTRY_DATE,
                                OffstreetNotifyEntryDate = domOp.OPEOFF_NOTIFY_ENTRY_DATE,
                                OffstreetPaymentDate = domOp.OPEOFF_PAYMENT_DATE,
                                OffstreetEndDate = domOp.OPEOFF_END_DATE,
                                OffstreetExitLimitDate = domOp.OPEOFF_EXIT_LIMIT_DATE,
                                OffstreetExitDate = domOp.OPEOFF_EXIT_DATE,
                                OffstreetUTCEntryDate = domOp.OPEOFF_UTC_ENTRY_DATE,
                                OffstreetUTCNotifyEntryDate = domOp.OPEOFF_UTC_NOTIFY_ENTRY_DATE,
                                OffstreetUTCPaymentDate = domOp.OPEOFF_UTC_PAYMENT_DATE,
                                OffstreetUTCEndDate = domOp.OPEOFF_UTC_END_DATE,
                                OffstreetUTCExitLimitDate = domOp.OPEOFF_UTC_EXIT_LIMIT_DATE,
                                OffstreetUTCExitDate = domOp.OPEOFF_UTC_EXIT_DATE,
                                OffstreetLogicalId = domOp.OPEOFF_LOGICAL_ID,
                                OffstreetTariff = domOp.OPEOFF_TARIFF,
                                OffstreetGate = domOp.OPEOFF_GATE,
                                OffstreetSpaceDescription = domOp.OPEOFF_SPACE_DESCRIPTION

                            }).AsQueryable();
            }
            return modelOps;
        }


        public static Expression<Func<ALL_OPERATIONS_EXT, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID.HasValue);

            if (GroupDataModel.GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<ALL_OPERATIONS_EXT>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<ALL_OPERATIONS_EXT>();
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);

                /*// apply installations filter
                predicate = predicate.And(o => insIds.Contains(o.OPE_INS_ID.Value));

                // apply groups filter
                if (grpIds.Count() > 0)
                {
                    predicate = predicate.And(o => o.GRP_ID.HasValue && grpIds.Contains(o.GRP_ID.Value));
                }*/
            }

            return predicate;
        }

    }
}