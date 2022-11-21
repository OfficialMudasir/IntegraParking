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
    public class ExternalOperationDataModel
    {

        [LocalizedDisplayName("ExternalOperationDataModel_Id", NameResourceType = typeof(Resources))]
        public decimal Id { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_Installation", NameResourceType = typeof(Resources))]
        public string Installation { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_InstallationShortDesc", NameResourceType = typeof(Resources))]
        public string InstallationShortDesc { get; set; }
        
        [LocalizedDisplayName("ExternalOperationDataModel_Plate", NameResourceType = typeof(Resources))]
        public string Plate { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_ZoneId", NameResourceType = typeof(Resources))]
        public decimal? ZoneId { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_TariffId", NameResourceType = typeof(Resources))]
        public decimal? TariffId { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_Date", NameResourceType = typeof(Resources))]
        public DateTime Date { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_IniDate", NameResourceType = typeof(Resources))]
        public DateTime? IniDate { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_EndDate", NameResourceType = typeof(Resources))]
        public DateTime EndDate { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_DateUTC", NameResourceType = typeof(Resources))]
        public DateTime DateUTC { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_IniDateUTC", NameResourceType = typeof(Resources))]
        public DateTime? IniDateUTC { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_EndDateUTC", NameResourceType = typeof(Resources))]
        public DateTime EndDateUTC { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_Amount", NameResourceType = typeof(Resources))]
        public double? Amount { get; set; }
        [LocalizedDisplayName("ExternalOperationDataModel_AmountCurrencyId", NameResourceType = typeof(Resources))]
        public decimal AmountCurrencyId { get; set; }
        [LocalizedDisplayName("ExternalOperationDataModel_AmountCurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string AmountCurrencyIsoCode { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_Time", NameResourceType = typeof(Resources))]
        public int? Time { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_InsertionNotified", NameResourceType = typeof(Resources))]
        public int InsertionNotified { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_EndingNotified", NameResourceType = typeof(Resources))]
        public int EndingNotified { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_ExternalProviderId", NameResourceType = typeof(Resources))]
        public decimal ExternalProviderId { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_SourceId", NameResourceType = typeof(Resources))]
        public int SourceId { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_SourceIdentifier", NameResourceType = typeof(Resources))]
        public string SourceIdentifier { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_TypeId", NameResourceType = typeof(Resources))]
        public int TypeId { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_InsertionUTCDate", NameResourceType = typeof(Resources))]
        public DateTime InsertionUTCDate { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_DateUTCOffset", NameResourceType = typeof(Resources))]
        public int DateUTCOffset { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_IniDateUTCOffset", NameResourceType = typeof(Resources))]
        public int? IniDateUTCOffset { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_EndDateUTCOffset", NameResourceType = typeof(Resources))]
        public int EndDateUTCOffset { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_OperationId1", NameResourceType = typeof(Resources))]
        public string OperationId1 { get; set; }

        [LocalizedDisplayName("ExternalOperationDataModel_OperationId2", NameResourceType = typeof(Resources))]
        public string OperationId2 { get; set; }
        
        public string AmountCurrencyId_FK { get; set; }        
        public string ZoneId_FK { get; set; }
        public string TariffId_FK { get; set; }
        public string TypeId_FK { get; set; }
        public string SourceId_FK { get; set; }


        public static IQueryable<ExternalOperationDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<EXTERNAL_PARKING_OPERATION>();
            return List(backOfficeRepository, predicate, true);
        }
        public static IQueryable<ExternalOperationDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs)
        {
            var predicate = PredicateBuilder.True<EXTERNAL_PARKING_OPERATION>();
            return List(backOfficeRepository, predicate, loadFKs);
        }

        public static IQueryable<ExternalOperationDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<EXTERNAL_PARKING_OPERATION, bool>> predicate, bool loadFKs)
        {

            IQueryable<ExternalOperationDataModel> modelOps;

            if (loadFKs)
            {
                modelOps = (from domOp in backOfficeRepository.GetExternalOperations(predicate)
                            select new ExternalOperationDataModel()
                            {
                                Id = domOp.EPO_ID,
                                Installation = domOp.INSTALLATION.INS_DESCRIPTION,
                                InstallationShortDesc = domOp.INSTALLATION.INS_SHORTDESC,
                                Plate = domOp.EPO_PLATE,
                                ZoneId = domOp.EPO_ZONE,
                                TariffId = domOp.EPO_TARIFF,
                                Date = domOp.EPO_DATE,
                                IniDate = domOp.EPO_INIDATE,
                                EndDate = domOp.EPO_ENDDATE,
                                DateUTC = domOp.EPO_DATE_UTC,
                                IniDateUTC = domOp.EPO_INIDATE_UTC,
                                EndDateUTC = domOp.EPO_ENDDATE_UTC,                                
                                Amount = Convert.ToDouble((domOp.EPO_AMOUNT.HasValue?domOp.EPO_AMOUNT:0) / 100.0),
                                AmountCurrencyId = domOp.INSTALLATION.INS_CUR_ID,
                                AmountCurrencyIsoCode = domOp.INSTALLATION.CURRENCy.CUR_ISO_CODE,
                                Time = domOp.EPO_TIME,
                                InsertionNotified = domOp.EPO_INSERTION_NOTIFIED,
                                EndingNotified = domOp.EPO_ENDING_NOTIFIED,
                                ExternalProviderId = domOp.EPO_EXP_ID,
                                SourceId = domOp.EPO_SRCTYPE,
                                SourceIdentifier = domOp.EPO_SRCIDENT,
                                TypeId = domOp.EPO_TYPE,
                                InsertionUTCDate = domOp.EPO_INSERTION_UTC_DATE,
                                DateUTCOffset = domOp.EPO_DATE_UTC_OFFSET,
                                IniDateUTCOffset = domOp.EPO_INIDATE_UTC_OFFSET,
                                EndDateUTCOffset = domOp.EPO_ENDDATE_UTC_OFFSET,
                                OperationId1 = domOp.EPO_OPERATION_ID1,
                                OperationId2 = domOp.EPO_OPERATION_ID2,

                                AmountCurrencyId_FK = domOp.INSTALLATION.CURRENCy.CUR_NAME,
                                ZoneId_FK = domOp.GROUP.GRP_DESCRIPTION,
                                TariffId_FK = domOp.TARIFF.TAR_DESCRIPTION,
                                TypeId_FK = ChargeOperationTypeDataModel.GetTypeIdString(domOp.EPO_TYPE),
                                SourceId_FK = OperationSourceTypeDataModel.GetTypeIdString(domOp.EPO_SRCTYPE)
                            }
                            ).AsQueryable();
            }
            else
            {
                modelOps = (from domOp in backOfficeRepository.GetExternalOperations(predicate)
                            select new ExternalOperationDataModel()
                            {
                                Id = domOp.EPO_ID,
                                Installation = domOp.INSTALLATION.INS_DESCRIPTION,
                                InstallationShortDesc = domOp.INSTALLATION.INS_SHORTDESC,
                                Plate = domOp.EPO_PLATE,
                                ZoneId = domOp.EPO_ZONE,
                                TariffId = domOp.EPO_TARIFF,
                                Date = domOp.EPO_DATE,
                                IniDate = domOp.EPO_INIDATE,
                                EndDate = domOp.EPO_ENDDATE,
                                DateUTC = domOp.EPO_DATE_UTC,
                                IniDateUTC = domOp.EPO_INIDATE_UTC,
                                EndDateUTC = domOp.EPO_ENDDATE_UTC,
                                Amount = Convert.ToDouble((domOp.EPO_AMOUNT.HasValue ? domOp.EPO_AMOUNT : 0) / 100.0),
                                AmountCurrencyId = domOp.INSTALLATION.INS_CUR_ID,
                                AmountCurrencyIsoCode = domOp.INSTALLATION.CURRENCy.CUR_ISO_CODE,
                                Time = domOp.EPO_TIME,
                                InsertionNotified = domOp.EPO_INSERTION_NOTIFIED,
                                EndingNotified = domOp.EPO_ENDING_NOTIFIED,
                                ExternalProviderId = domOp.EPO_EXP_ID,
                                SourceId = domOp.EPO_SRCTYPE,
                                SourceIdentifier = domOp.EPO_SRCIDENT,
                                TypeId = domOp.EPO_TYPE,
                                InsertionUTCDate = domOp.EPO_INSERTION_UTC_DATE,
                                DateUTCOffset = domOp.EPO_DATE_UTC_OFFSET,
                                IniDateUTCOffset = domOp.EPO_INIDATE_UTC_OFFSET,
                                EndDateUTCOffset = domOp.EPO_ENDDATE_UTC_OFFSET,
                                OperationId1 = domOp.EPO_OPERATION_ID1,
                                OperationId2 = domOp.EPO_OPERATION_ID2
                            }
                            ).AsQueryable();
            }

            return modelOps;
        }

    }
}