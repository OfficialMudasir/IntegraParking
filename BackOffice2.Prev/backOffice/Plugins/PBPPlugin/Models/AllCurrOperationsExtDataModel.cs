using System;
using System.Collections.Generic;
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
    public class AllCurrOperationsExtDataModel
    {

        public decimal OpeId { get; set; }
        
        public int TypeId { get; set; }                
        public int /*ChargeOperationsType*/ OpeType { get; set; }

        public decimal? UserId { get; set; }
        public string UsrUsername { get; set; }

        public int OpeMoseOs { get; set; }

        public string OpeAppVersion { get; set; }

        public decimal? InstallationId { get; set; }
        public string InsDescription { get; set; }
        public string InsShortdesc { get; set; }

        public DateTime? OpeDate { get; set; }
        public DateTime? OpeUtcDate { get; set; }
        public DateTime? OpeInidate { get; set; }
        public DateTime? OpeEnddate { get; set; }

        public double? OpeAmount { get; set; }
        public decimal OpeAmountCurId { get; set; }
        public string AmountCurrencyIsoCode { get; set; }
        public double? OpeFinalAmount { get; set; }

        public int? OpeTime { get; set; }

        public double? OpeBalanceBefore { get; set; }
        public decimal? OpeBalanceCurId { get; set; }
        public string BalanceCurrencyIsoCode { get; set; }

        public double? OpeChangeApplied { get; set; }

        public decimal? PlateId { get; set; }
        public string UsrpPlate { get; set; }

        public string TipaTicketNumber { get; set; }
        public string TicketData { get; set; }

        public decimal? GrpId { get; set; }
        public decimal? TarId { get; set; }

        public int OpeSuscriptionType { get; set; }

        public DateTime? OpeInsertionUtcDate { get; set; }

        public decimal? RechargeId { get; set; }
        public DateTime? CuspmrDate { get; set; }
        public double? CuspmrAmount { get; set; }
        public decimal? CuspmrCurId { get; set; }
        public string RechargeAmountCurrencyIsoCode { get; set; }
        public double? CuspmrBalanceBefore { get; set; }
        public DateTime? CuspmrInsertionUtcDate { get; set; }

        public decimal? DiscountId { get; set; }
        public DateTime? OpedisDate { get; set; }
        public double? OpedisAmount { get; set; }
        public decimal? OpedisAmountCurId { get; set; }
        public string DiscountAmountCurrencyIsoCode { get; set; }
        public double? OpedisFinalAmount { get; set; }
        public decimal? OpedisBalanceCurId { get; set; }
        public string DiscountBalanceCurrencyIsoCode { get; set; }
        public double? OpedisBalanceBefore { get; set; }
        public double? OpedisChangeApplied { get; set; }
        public DateTime? OpedisInsertionUtcDate { get; set; }

        public int? SechSechtId { get; set; }

        public string CuspmrOpReference { get; set; }
        public string CuspmrTransactionId { get; set; }
        public string CuspmrAuthCode { get; set; }
        public string CuspmrCardHash { get; set; }
        public string CuspmrCardReference { get; set; }
        public string CuspmrCardScheme { get; set; }
        public string CuspmrMaskedCardNumber { get; set; }
        public DateTime? CuspmrCardExpirationDate { get; set; }

        public string OpeExternalId1 { get; set; }
        public string OpeExternalId2 { get; set; }
        public string OpeExternalId3 { get; set; }

        public decimal? OpeLatitude { get; set; }
        public decimal? OpeLongitude { get; set; }

        public DateTime? OpeoffEntryDate { get; set; }
        public DateTime? OpeoffNotifyEntryDate { get; set; }
        public DateTime? OpeoffPaymentDate { get; set; }
        public DateTime? OpeoffEndDate { get; set; }
        public DateTime? OpeoffExitLimitDate { get; set; }
        public DateTime? OpeoffExitDate { get; set; }
        public DateTime? OpeoffUtcEntryDate { get; set; }
        public DateTime? OpeoffUtcNotifyEntryDate { get; set; }
        public DateTime? OpeoffUtcPaymentDate { get; set; }
        public DateTime? OpeoffUtcEndDate { get; set; }
        public DateTime? OpeoffUtcExitLimitDate { get; set; }
        public DateTime? OpeoffUtcExitDate { get; set; }
        public string OpeoffLogicalId { get; set; }
        public string OffstreetTariff { get; set; }
        public string OpeoffGate { get; set; }
        public string OpeoffSpaceDescription { get; set; }

        public decimal? OpePercVat1 { get; set; }
        public decimal? OpePercVat2 { get; set; }
        public double OpePartialVat1 { get; set; }
        public decimal? OpePercFee { get; set; }
        public double OpePercFeeTopped { get; set; }
        public double OpePartialPercFee { get; set; }
        public double OpeFixedFee { get; set; }
        public double OpePartialFixedFee { get; set; }
        public double OpeTotalAmount { get; set; }

        public int? OpeCuspmrType { get; set; }
        public int? OpeCuspmrPagateliaNewBalance { get; set; }

        public string OpeType_FK { get; set; }
        public string UserId_FK { get; set; }
        public string OpeMoseOs_FK { get; set; }
        public string OpeAmountCurId_FK { get; set; }
        public string OpeBalanceCurId_FK { get; set; }
        public string GrpId_FK { get; set; }
        public string TarId_FK { get; set; }
        public string OpeSuscriptionType_FK { get; set; }
        public string CuspmrCurId_FK { get; set; }
        public string OpedisAmountCurId_FK { get; set; }
        public string OpedisBalanceCurId_FK { get; set; }
        public string SechSechtId_FK { get; set; }
        public string OpeCuspmrType_FK { get; set; }

        public static IQueryable<AllCurrOperationsExtDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<ALL_CURR_OPERATIONS_EXT, bool>> predicate)
        {            
            if (predicate == null) predicate = PredicateBuilder.True<ALL_CURR_OPERATIONS_EXT>();

            /*string sInstallation = ConfigurationManager.AppSettings["InstallationShortDesc"];
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
            }*/

            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ".";

            var modelOps = (from domOp in backOfficeRepository.GetOperationsExt(predicate, 600)
                            select new AllCurrOperationsExtDataModel()
                            {
                                OpeId = domOp.OPE_ID,
                                TypeId = domOp.OPE_TYPE,
                                OpeType = domOp.OPE_TYPE,
                                UserId = domOp.OPE_USR_ID,
                                UsrUsername = domOp.USR_USERNAME,
                                OpeMoseOs = domOp.OPE_MOSE_OS,
                                OpeAppVersion = domOp.OPE_APP_VERSION,
                                InstallationId = domOp.OPE_INS_ID,
                                InsDescription = domOp.INS_DESCRIPTION,
                                InsShortdesc = domOp.INS_SHORTDESC,
                                OpeDate = domOp.OPE_DATE,
                                OpeUtcDate = domOp.OPE_UTC_DATE,
                                OpeInidate = domOp.OPE_INIDATE,
                                OpeEnddate = domOp.OPE_ENDDATE,
                                OpeAmount = Convert.ToDouble(domOp.OPE_AMOUNT / 100.0),
                                OpeAmountCurId = domOp.OPE_AMOUNT_CUR_ID,
                                AmountCurrencyIsoCode = domOp.OPE_AMOUNT_CUR_ISO_CODE,
                                OpeFinalAmount = Convert.ToDouble(domOp.OPE_FINAL_AMOUNT / 100.0),
                                OpeTime = domOp.OPE_TIME,
                                OpeBalanceBefore = Convert.ToDouble(domOp.OPE_BALANCE_BEFORE / 100.0),
                                OpeBalanceCurId = domOp.OPE_BALANCE_CUR_ID,
                                BalanceCurrencyIsoCode = domOp.OPE_BALANCE_CUR_ISO_CODE,
                                OpeChangeApplied = Convert.ToDouble(domOp.OPE_CHANGE_APPLIED),
                                PlateId = Convert.ToInt32(domOp.USRP_ID),
                                UsrpPlate = domOp.USRP_PLATE,
                                TipaTicketNumber = domOp.TIPA_TICKET_NUMBER,
                                TicketData = domOp.TIPA_TICKET_DATA,
                                GrpId = domOp.GRP_ID,
                                TarId = domOp.TAR_ID,
                                OpeSuscriptionType = domOp.OPE_SUSCRIPTION_TYPE,
                                OpeInsertionUtcDate = domOp.OPE_INSERTION_UTC_DATE,

                                RechargeId = domOp.OPE_CUSPMR_ID,
                                CuspmrDate = domOp.CUSPMR_DATE,
                                CuspmrAmount = Convert.ToDouble(domOp.CUSPMR_AMOUNT / 100.0),
                                CuspmrCurId = domOp.CUSPMR_CUR_ID,
                                RechargeAmountCurrencyIsoCode = domOp.CUSPMR_AMOUNT_ISO_CODE,
                                CuspmrBalanceBefore = Convert.ToDouble(domOp.CUSPMR_BALANCE_BEFORE / 100.0),
                                CuspmrInsertionUtcDate = domOp.CUSPMR_INSERTION_UTC_DATE,

                                DiscountId = domOp.OPE_OPEDIS_ID,
                                OpedisDate = domOp.OPEDIS_DATE,
                                OpedisAmount = Convert.ToDouble(domOp.OPEDIS_AMOUNT / 100.0),
                                OpedisAmountCurId = domOp.OPEDIS_AMOUNT_CUR_ID,
                                DiscountAmountCurrencyIsoCode = domOp.OPEDIS_AMOUNT_CUR_ISO_CODE,
                                OpedisFinalAmount = Convert.ToDouble(domOp.OPEDIS_FINAL_AMOUNT / 100.0),
                                OpedisBalanceCurId = domOp.OPEDIS_BALANCE_CUR_ID,
                                DiscountBalanceCurrencyIsoCode = domOp.OPEDIS_BALANCE_CUR_ISO_CODE,
                                OpedisBalanceBefore = Convert.ToDouble(domOp.OPEDIS_BALANCE_BEFORE / 100.0),
                                OpedisChangeApplied = Convert.ToDouble(domOp.OPEDIS_CHANGE_APPLIED),
                                OpedisInsertionUtcDate = domOp.OPEDIS_INSERTION_UTC_DATE,

                                SechSechtId = domOp.SECH_SECHT_ID,

                                CuspmrOpReference = domOp.CUSPMR_OP_REFERENCE,
                                CuspmrTransactionId = domOp.CUSPMR_TRANSACTION_ID,
                                CuspmrAuthCode = domOp.CUSPMR_AUTH_CODE,
                                CuspmrCardHash = domOp.CUSPMR_CARD_HASH,
                                CuspmrCardReference = domOp.CUSPMR_CARD_REFERENCE,
                                CuspmrCardScheme = domOp.CUSPMR_CARD_SCHEME,
                                CuspmrMaskedCardNumber = domOp.CUSPMR_MASKED_CARD_NUMBER,
                                CuspmrCardExpirationDate = domOp.CUSPMR_CARD_EXPIRATION_DATE,

                                OpeExternalId1 = domOp.OPE_EXTERNAL_ID1,
                                OpeExternalId2 = domOp.OPE_EXTERNAL_ID2,
                                OpeExternalId3 = domOp.OPE_EXTERNAL_ID3,

                                OpeLatitude = domOp.OPE_LATITUDE,
                                OpeLongitude = domOp.OPE_LONGITUDE,

                                OpeoffEntryDate = domOp.OPEOFF_ENTRY_DATE,
                                OpeoffNotifyEntryDate = domOp.OPEOFF_NOTIFY_ENTRY_DATE,
                                OpeoffPaymentDate = domOp.OPEOFF_PAYMENT_DATE,
                                OpeoffEndDate = domOp.OPEOFF_END_DATE,
                                OpeoffExitLimitDate = domOp.OPEOFF_EXIT_LIMIT_DATE,
                                OpeoffExitDate = domOp.OPEOFF_EXIT_DATE,
                                OpeoffUtcEntryDate = domOp.OPEOFF_UTC_ENTRY_DATE,
                                OpeoffUtcNotifyEntryDate = domOp.OPEOFF_UTC_NOTIFY_ENTRY_DATE,
                                OpeoffUtcPaymentDate = domOp.OPEOFF_UTC_PAYMENT_DATE,
                                OpeoffUtcEndDate = domOp.OPEOFF_UTC_END_DATE,
                                OpeoffUtcExitLimitDate = domOp.OPEOFF_UTC_EXIT_LIMIT_DATE,
                                OpeoffUtcExitDate = domOp.OPEOFF_UTC_EXIT_DATE,
                                OpeoffLogicalId = domOp.OPEOFF_LOGICAL_ID,
                                OffstreetTariff = domOp.OPEOFF_TARIFF,
                                OpeoffGate = domOp.OPEOFF_GATE,
                                OpeoffSpaceDescription = domOp.OPEOFF_SPACE_DESCRIPTION,

                                OpePercVat1 = domOp.OPE_PERC_VAT1,
                                OpePercVat2 = domOp.OPE_PERC_VAT2,
                                OpePartialVat1 = Convert.ToDouble(Convert.ToInt32(domOp.OPE_PARTIAL_VAT1??0) / 100.0),
                                OpePercFee = domOp.OPE_PERC_FEE,
                                OpePercFeeTopped = Convert.ToDouble(Convert.ToInt32(domOp.OPE_PERC_FEE_TOPPED ?? 0) / 100.0),
                                OpePartialPercFee = Convert.ToDouble(Convert.ToInt32(domOp.OPE_PARTIAL_PERC_FEE ?? 0) / 100.0),
                                OpeFixedFee = Convert.ToDouble(Convert.ToInt32(domOp.OPE_FIXED_FEE ?? 0) / 100.0),
                                OpePartialFixedFee = Convert.ToDouble(Convert.ToInt32(domOp.OPE_PARTIAL_FIXED_FEE ?? 0) / 100.0),
                                OpeTotalAmount = Convert.ToDouble(Convert.ToInt32(domOp.OPE_TOTAL_AMOUNT ?? 0) / 100.0),

                                OpeCuspmrType = domOp.OPE_CUSPMR_TYPE,
                                OpeCuspmrPagateliaNewBalance = domOp.OPE_CUSPMR_PAGATELIA_NEW_BALANCE,

                                OpeType_FK = ChargeOperationTypeDataModel.GetTypeIdString(domOp.OPE_TYPE),
                                //UserId_FK = UserDataModel.Get(users, domOp.OPE_USR_ID).Username,
                                OpeMoseOs_FK = MobileOSDataModel.GetTypeIdString(domOp.OPE_MOSE_OS),
                                OpeAmountCurId_FK = domOp.OPE_AMOUNT_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPE_AMOUNT_CUR_ID).Name,
                                OpeBalanceCurId_FK = domOp.OPE_BALANCE_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPE_BALANCE_CUR_ID).Name,
                                GrpId_FK = domOp.GRP_DESCRIPTION, // GroupDataModel.Get(domOp.GRP_ID, backOfficeRepository).Description,
                                TarId_FK = domOp.TAR_DESCRIPTION, // TariffDataModel.Get(domOp.TAR_ID, backOfficeRepository).Description,
                                OpeSuscriptionType_FK = PaymentSuscryptionTypeDataModel.GetTypeIdString(domOp.OPE_SUSCRIPTION_TYPE),
                                CuspmrCurId_FK = domOp.CUSPMR_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.CUSPMR_CUR_ID).Name,
                                OpedisAmountCurId_FK = domOp.OPEDIS_AMOUNT_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPEDIS_AMOUNT_CUR_ID).Name,
                                OpedisBalanceCurId_FK = domOp.OPEDIS_BALANCE_CUR_NAME, // CurrencyDataModel.Get(currencies, domOp.OPEDIS_BALANCE_CUR_ID).Name,
                                SechSechtId_FK = ServiceChargeTypeDataModel.GetTypeIdString(domOp.SECH_SECHT_ID),
                                OpeCuspmrType_FK = PaymentMeanRechargeTypeDataModel.GetTypeIdString(domOp.OPE_CUSPMR_TYPE)

                            }).AsQueryable();

            return modelOps;
        }

    }
}