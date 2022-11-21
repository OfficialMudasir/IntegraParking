using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace integraMobile.Domain.Abstract
{
    public interface IBackOfficeRepository
    {

        IQueryable<ALL_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, int iTransactionTimeout = 0);
        IQueryable<ALL_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<ALL_CURR_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_CURR_OPERATIONS_EXT, bool>> predicate, int iTransactionTimeout = 0);
        IQueryable<ALL_CURR_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_CURR_OPERATIONS_EXT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<USER> GetUsers(Expression<Func<USER, bool>> predicate);
        IQueryable<USER> GetUsers(Expression<Func<USER, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<GROUP> GetGroups(Expression<Func<GROUP, bool>> predicate = null);
        IQueryable<TARIFF> GetTariffs(Expression<Func<TARIFF, bool>> predicate = null);
        IQueryable<SERVICE_CHARGE_TYPE> GetServiceChargeTypes(Expression<Func<SERVICE_CHARGE_TYPE, bool>> predicate = null);
        IQueryable<CURRENCy> GetCurrencies(Expression<Func<CURRENCy, bool>> predicate = null);
        IQueryable<CURRENCy> GetCurrencies(Expression<Func<CURRENCy, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<COUNTRy> GetCountries(Expression<Func<COUNTRy, bool>> predicate = null);
        IQueryable<CUSTOMER_INVOICE> GetInvoices(Expression<Func<CUSTOMER_INVOICE, bool>> predicate);
        IQueryable<SERVICES_PHOTO> GetServiceUserPlatesPhotos(Expression<Func<SERVICES_PHOTO, bool>> predicate, out string Error);
        IQueryable<EXTERNAL_PARKING_OPERATION> GetExternalOperations(Expression<Func<EXTERNAL_PARKING_OPERATION, bool>> predicate);
        IQueryable<EXTERNAL_PROVIDER> GetExternalProviders(Expression<Func<EXTERNAL_PROVIDER, bool>> predicate = null);
        IQueryable<CUSTOMER_INSCRIPTION> GetCustomerInscriptions(Expression<Func<CUSTOMER_INSCRIPTION, bool>> predicate);
        IQueryable<INSTALLATION> GetInstallations(Expression<Func<INSTALLATION, bool>> predicate);
        IQueryable<SUPER_INSTALLATION> GetSuperInstallations(Expression<Func<SUPER_INSTALLATION, bool>> predicate);
        IQueryable<USERS_SECURITY_OPERATION> GetUsersSecurityOperations(Expression<Func<USERS_SECURITY_OPERATION, bool>> predicate);
        IQueryable<CURRENCY_RECHARGE_VALUE> GetCurrencyRechargeValues();

        bool SetUserEnabled(decimal dUserId, bool bEnabled, out USER oUser);

        bool UpdateCountry(ref COUNTRy country);
        bool DeleteCountry(ref COUNTRy country);

        IQueryable<OPERATION> GetOperations(Expression<Func<OPERATION, bool>> predicate);
        IQueryable<OPERATION> GetOperations(Expression<Func<OPERATION, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<HIS_OPERATION> GetHisOperations(Expression<Func<HIS_OPERATION, bool>> predicate);
        IQueryable<HIS_OPERATION> GetHisOperations(Expression<Func<HIS_OPERATION, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<TICKET_PAYMENT> GetTicketPayments(Expression<Func<TICKET_PAYMENT, bool>> predicate);
        IQueryable<TICKET_PAYMENT> GetTicketPayments(Expression<Func<TICKET_PAYMENT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGE> GetCustomerRecharges(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGE, bool>> predicate);
        IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGE> GetCustomerRecharges(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI> GetCustomerRechargesHis(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI, bool>> predicate);
        IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI> GetCustomerRechargesHis(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);        
        IQueryable<SERVICE_CHARGE> GetServiceCharges(Expression<Func<SERVICE_CHARGE, bool>> predicate);
        IQueryable<SERVICE_CHARGE> GetServiceCharges(Expression<Func<SERVICE_CHARGE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<OPERATIONS_DISCOUNT> GetDiscounts(Expression<Func<OPERATIONS_DISCOUNT, bool>> predicate);
        IQueryable<OPERATIONS_DISCOUNT> GetDiscounts(Expression<Func<OPERATIONS_DISCOUNT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<OPERATIONS_OFFSTREET> GetOperationsOffstreet(Expression<Func<OPERATIONS_OFFSTREET, bool>> predicate);
        IQueryable<OPERATIONS_OFFSTREET> GetOperationsOffstreet(Expression<Func<OPERATIONS_OFFSTREET, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<BALANCE_TRANSFER> GetBalanceTransfers(Expression<Func<BALANCE_TRANSFER, bool>> predicate);
        IQueryable<BALANCE_TRANSFER> GetBalanceTransfers(Expression<Func<BALANCE_TRANSFER, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);

        bool DeleteOperation(ChargeOperationsType type, decimal operationId, out object oDeleted, out int iBalanceBefore, out USER oUser, out OPERATIONS_DISCOUNT oDiscountDeleted, out bool bIsHisOperation);
        bool DeleteOperation(ChargeOperationsType type, decimal operationId, out object oDeleted, out int iBalanceBefore, out USER oUser, out OPERATIONS_DISCOUNT oDiscountDeleted, out bool bIsHisOperation, List<int> oInstallationsAllowed, out bool bErrorAccess);
        bool DeleteOperation(ChargeOperationsType type, decimal operationId, out object oDeleted, out int iBalanceBefore, out USER oUser, out OPERATIONS_DISCOUNT oDiscountDeleted, out bool bIsHisOperation, List<int> oInstallationsAllowed, out bool bErrorAccess, PaymentMeanRechargeStatus oRechargeStatus);

        IQueryable<VW_OPERATIONS_HOUR> GetVwOperationsHour(Expression<Func<VW_OPERATIONS_HOUR, bool>> predicate);
        IQueryable<VW_OPERATIONS_HOUR> GetVwOperationsHour(Expression<Func<VW_OPERATIONS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<VW_RECHARGES_HOUR> GetVwRechargesHour(Expression<Func<VW_RECHARGES_HOUR, bool>> predicate);
        IQueryable<VW_RECHARGES_HOUR> GetVwRechargesHour(Expression<Func<VW_RECHARGES_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<VW_INSCRIPTIONS_HOUR> GetVwInscriptionsHour(Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> predicate);
        IQueryable<VW_INSCRIPTIONS_HOUR> GetVwInscriptionsHour(Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<VW_INSCRIPTIONS_PLATFORM_HOUR> GetVwInscriptionsPlatformHour(Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> predicate);
        IQueryable<VW_INSCRIPTIONS_PLATFORM_HOUR> GetVwInscriptionsPlatformHour(Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<VW_OPERATIONS_USER_HOUR> GetVwOperationsUserHour(Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> predicate);
        IQueryable<VW_OPERATIONS_USER_HOUR> GetVwOperationsUserHour(Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<VW_OPERATIONS_MINUTE> GetVwOperationsMinute(Expression<Func<VW_OPERATIONS_MINUTE, bool>> predicate);
        IQueryable<VW_OPERATIONS_MINUTE> GetVwOperationsMinute(Expression<Func<VW_OPERATIONS_MINUTE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);

        IQueryable<DB_OPERATIONS_HOUR> GetDbOperationsHour(Expression<Func<DB_OPERATIONS_HOUR, bool>> predicate);
        IQueryable<DB_OPERATIONS_HOUR> GetDbOperationsHour(Expression<Func<DB_OPERATIONS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<DB_RECHARGES_HOUR> GetDbRechargesHour(Expression<Func<DB_RECHARGES_HOUR, bool>> predicate);
        IQueryable<DB_RECHARGES_HOUR> GetDbRechargesHour(Expression<Func<DB_RECHARGES_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<DB_OPERATIONS_USERS_HOUR> GetDbOperationsUsersHour(Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> predicate);
        IQueryable<DB_OPERATIONS_USERS_HOUR> GetDbOperationsUsersHour(Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<DB_OPERATIONS_MINUTE> GetDbOperationsMinute(Expression<Func<DB_OPERATIONS_MINUTE, bool>> predicate);
        IQueryable<DB_OPERATIONS_MINUTE> GetDbOperationsMinute(Expression<Func<DB_OPERATIONS_MINUTE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<Select_DB_INVITATIONS_HOURResult> GetDbInvitationsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_INVITATIONS_HOURResult, bool>> predicate);
        IQueryable<Select_DB_INVITATIONS_HOURResult> GetDbInvitationsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_INVITATIONS_HOURResult, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        IQueryable<Select_DB_RECHARGE_COUPONS_HOURResult> GetDbRechargeCouponsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_RECHARGE_COUPONS_HOURResult, bool>> predicate);
        IQueryable<Select_DB_RECHARGE_COUPONS_HOURResult> GetDbRechargeCouponsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_RECHARGE_COUPONS_HOURResult, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);

        bool RecalculateUserBalance(decimal dUserId, out USER oUser, decimal? operationId = null, ChargeOperationsType? operationType = null);

        IQueryable<EMAILTOOL_RECIPIENT> GetEmailToolRecipients(Expression<Func<EMAILTOOL_RECIPIENT, bool>> predicate);
        IQueryable<EMAILTOOL_RECIPIENT> GetEmailToolRecipients(Expression<Func<EMAILTOOL_RECIPIENT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        bool AddEmailToolRecipient(long dId, string sEmail);
        bool AddEmailToolRecipients(long dId, string[] sEmails);
        bool DeleteEmailToolRecipient(long dId, string sEmail);
        bool DeleteAllEmailToolRecipients(long dId);
        bool DeletePreviousEmailToolRecipients(long dId);
        bool DeleteEmailToolRecipients(Expression<Func<EMAILTOOL_RECIPIENT, bool>> predicate);

        IQueryable<USERS_WARNINGS_RECIPIENT> GetNotificationToolRecipients(Expression<Func<USERS_WARNINGS_RECIPIENT, bool>> predicate);
        IQueryable<USERS_WARNINGS_RECIPIENT> GetNotificationToolRecipients(Expression<Func<USERS_WARNINGS_RECIPIENT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext);
        bool AddNotificationToolRecipient(long dId, string sEmail);
        bool AddNotificationToolRecipients(long dId, string[] sEmails);
        bool DeleteNotificationToolRecipient(long dId, string sEmail);
        bool DeleteAllNotificationToolRecipients(long dId);
        bool DeletePreviousNotificationToolRecipients(long dId);
        bool DeleteNotificationToolRecipients(Expression<Func<USERS_WARNINGS_RECIPIENT, bool>> predicate);
        IQueryable<USERS_WARNINGS_FUNCTION> GetUsersWarningsFunctions(Expression<Func<USERS_WARNINGS_FUNCTION, bool>> predicate, out decimal? lang_id);

        bool SetUserShopkeeperStatus(decimal dUserId, ShopKeeperStatus eStatus, out USER oUser);
        bool SetUsername(decimal dUserId, string sUserName);
        bool SetBalance(decimal dUserId, decimal iBalance);
        bool InsertUsersWarnings(List<decimal> UserIds, string HeaderImage, string Title, string Body1, string Body2, string ButtonText, string ButtonAction, string FooterText, int Push, int Login, int Park, int Unpark, int Fine);

        bool RechargeCouponsGenerationGet(decimal dGenerationId, out RECHARGE_COUPONS_GENERATION oRechargeCouponGeneration);
        bool RechargeCouponsGenerationAdd(int iNumber, RechargeCouponsType eCouponType, int iAmountValue, decimal dCurrencyId, string sBackOfficeUser, decimal dFinanDistOperatorId, DateTime dtIniApply, DateTime dtEndApply, out RECHARGE_COUPONS_GENERATION oRechargeCouponGeneration);
        bool RechargeCouponsGenerationModifyApplyPeriod(decimal dGenerationId, DateTime dtIniApply, DateTime dtEndApply, out int iNotActivedCount);
        bool RechargeCouponsGenerationModifyStatus(decimal dGenerationId, RechargeCouponsStatus eStatus, out int iChangedCount);
        bool RechargeCouponGet(decimal dCouponId, out RECHARGE_COUPON oRechargeCoupon);
        bool RechargeCouponModifyApplyPeriod(decimal dCouponId, DateTime dtIniApply, DateTime dtEndApply);
        bool RechargeCouponModifyStatus(decimal dCouponId, RechargeCouponsStatus eStatus);
        bool RechargeValueTypesModifyAmount(int CuspmId, int? CuspmAmountToRecharge, int? CuspmRechargeWhenAmountIsLess, bool CuspmEnabled, bool CuspmAutomaticRecharge);

        bool UpdateOperationPlates(ref OPERATION operation, string sPlate, List<string> oAdditionalPlates);

        bool SubstractUserBalance(decimal dUserId, int iBalance);

        bool UpdateBankData(long id, decimal bg, decimal bf, out string ErrorMessage);
        bool UpdateGatewayConfig(long id, string Batch, string Timezone, decimal FixedFee, decimal PercFee, decimal MinFee, out string ErrorMessage);
        bool UpdateServiceUserPlateStatus(decimal SerupId, decimal StatusId, out string ErrorMessage);

        bool RestoreRechargeFromHis(CUSTOMER_PAYMENT_MEANS_RECHARGES_HI HisRecharge, out decimal? RechargeId);
    }
}
