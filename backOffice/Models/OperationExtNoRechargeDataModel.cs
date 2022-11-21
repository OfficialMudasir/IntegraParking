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
    public class OperationExtNoRechargeDataModel
    {

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

        public string TypeId_FK { get; set; }
        public string UserId_FK { get; set; }
        public string MobileOSId_FK { get; set; }
        public string AmountCurrencyId_FK { get; set; }
        public string BalanceCurrencyId_FK { get; set; }
        public string SectorId_FK { get; set; }
        public string TariffId_FK { get; set; }
        public string SuscriptionType_FK { get; set; }

        public static IQueryable<OperationExtNoRechargeDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
            return List(backOfficeRepository, predicate, true);
        }
        public static IQueryable<OperationExtNoRechargeDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs)
        {
            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
            return List(backOfficeRepository, predicate, loadFKs);
        }

        public static IQueryable<OperationExtNoRechargeDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, bool loadFKs)
        {

            IQueryable<OperationExtNoRechargeDataModel> modelOps;

            predicate = predicate.And(a => a.OPE_TYPE != 5);
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
                predicate = OperationExtDataModel.FilterInstallationsAndGroups(backOfficeRepository, predicate, infraFilter);
            }

            if (loadFKs)
            {
                modelOps = (from domOp in backOfficeRepository.GetOperationsExt(predicate)
                            select new OperationExtNoRechargeDataModel()
                            {
                                TypeId = domOp.OPE_TYPE,
                                Type = (ChargeOperationsType)domOp.OPE_TYPE,
                                UserId = domOp.OPE_USR_ID,
                                Username = domOp.USR_USERNAME,
                                MobileOSId = domOp.OPE_MOSE_OS,
                                AppVersion = "",
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
                                ExternalId1 = domOp.OPE_EXTERNAL_ID1,
                                ExternalId2 = domOp.OPE_EXTERNAL_ID2,
                                ExternalId3 = domOp.OPE_EXTERNAL_ID3,
                                /*Latitude = domOp.OPE_LATITUDE,
                                Longitude = domOp.OPE_LONGITUDE,*/
                                Latitude = null,
                                Longitude = null,
                                TypeId_FK = ChargeOperationTypeDataModel.GetTypeIdString(domOp.OPE_TYPE),
                                UserId_FK = domOp.USR_USERNAME, // UserDataModel.Get(backOfficeRepository, domOp.OPE_USR_ID).Username,
                                MobileOSId_FK = MobileOSDataModel.GetTypeIdString(domOp.OPE_MOSE_OS),
                                AmountCurrencyId_FK = domOp.OPE_AMOUNT_CUR_NAME, // CurrencyDataModel.Get(backOfficeRepository, domOp.OPE_AMOUNT_CUR_ID).Name,
                                BalanceCurrencyId_FK = domOp.OPE_BALANCE_CUR_NAME, // CurrencyDataModel.Get(backOfficeRepository, domOp.OPE_BALANCE_CUR_ID).Name,
                                SectorId_FK = domOp.GRP_DESCRIPTION, // GroupDataModel.Get(domOp.GRP_ID, backOfficeRepository).Description,
                                TariffId_FK = domOp.TAR_DESCRIPTION, // TariffDataModel.Get(domOp.TAR_ID, backOfficeRepository).Description,
                                SuscriptionType_FK = PaymentSuscryptionTypeDataModel.GetTypeIdString(domOp.OPE_SUSCRIPTION_TYPE)
                            }
                            ).AsQueryable();
            }
            else
            {
                modelOps = (from domOp in backOfficeRepository.GetOperationsExt(predicate)
                            select new OperationExtNoRechargeDataModel()
                            {
                                TypeId = domOp.OPE_TYPE,
                                Type = (ChargeOperationsType)domOp.OPE_TYPE,
                                UserId = domOp.OPE_USR_ID,
                                Username = domOp.USR_USERNAME,
                                MobileOSId = domOp.OPE_MOSE_OS,
                                AppVersion = "",
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
                                ExternalId1 = domOp.OPE_EXTERNAL_ID1,
                                ExternalId2 = domOp.OPE_EXTERNAL_ID2,
                                ExternalId3 = domOp.OPE_EXTERNAL_ID3,
                                /*Latitude = domOp.OPE_LATITUDE,
                                Longitude = domOp.OPE_LONGITUDE,*/
                                Latitude = null,
                                Longitude = null
                            }
                            ).AsQueryable();
            }

            return modelOps;
        }
    }
}