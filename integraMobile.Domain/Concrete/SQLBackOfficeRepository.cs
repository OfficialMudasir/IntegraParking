using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Data.Linq;
using System.Transactions;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using integraMobile.Domain.Abstract;
using integraMobile.Domain;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Infrastructure;
using integraMobile.Domain.Helper;

namespace integraMobile.Domain.Concrete
{
    public class SQLBackOfficeRepository : IBackOfficeRepository
    {

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SQLBackOfficeRepository));
        private const int ctnTransactionTimeout = 30;


        public SQLBackOfficeRepository(string connectionString)
        {
        }


        public IQueryable<ALL_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, int iTransactionTimeout = 0)
        {
            IQueryable<ALL_OPERATIONS_EXT> res = null;
            try
            {
                if (iTransactionTimeout == 0) iTransactionTimeout = ctnTransactionTimeout;

                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(iTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    dbContext.CommandTimeout = iTransactionTimeout;
                    res = GetOperationsExt(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperationsExt: ", e);
            }

            return res;
        }
        public IQueryable<ALL_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<ALL_OPERATIONS_EXT> res = null;
            try
            {
                res = (from r in dbContext.ALL_OPERATIONS_EXTs
                        select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperationsExt: ", e);
            }

            return res;
        }

        public IQueryable<ALL_CURR_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_CURR_OPERATIONS_EXT, bool>> predicate, int iTransactionTimeout = 0)
        {
            IQueryable<ALL_CURR_OPERATIONS_EXT> res = null;
            try
            {
                if (iTransactionTimeout == 0) iTransactionTimeout = ctnTransactionTimeout;

                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    dbContext.CommandTimeout = iTransactionTimeout;
                    res = GetOperationsExt(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperationsExt: ", e);
            }

            return res;
        }
        public IQueryable<ALL_CURR_OPERATIONS_EXT> GetOperationsExt(Expression<Func<ALL_CURR_OPERATIONS_EXT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<ALL_CURR_OPERATIONS_EXT> res = null;
            try
            {
                res = (from r in dbContext.ALL_CURR_OPERATIONS_EXTs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperationsExt: ", e);
            }

            return res;
        }

        public IQueryable<USER> GetUsers(Expression<Func<USER, bool>> predicate)
        {
            IQueryable<USER> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetUsers(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUsers: ", e);
            }

            return res;
        }
        public IQueryable<USER> GetUsers(Expression<Func<USER, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<USER> res = null;
            try
            {
                res = (from r in dbContext.USERs
                        select r)
                        .Where(predicate)
                        .OrderBy(t => t.USR_USERNAME)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUsers: ", e);
            }

            return res;
        }

        public IQueryable<GROUP> GetGroups(Expression<Func<GROUP, bool>> predicate = null)
        {
            IQueryable<GROUP> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    if (predicate == null) predicate = PredicateBuilder.True<GROUP>();
                    predicate = predicate.And(group => group.INSTALLATION.INS_ENABLED != 0);

                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.GROUPs
                           select r)
                           .Where(predicate)
                           .AsQueryable();

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetGroups: ", e);
            }

            return res;
        }

        public IQueryable<TARIFF> GetTariffs(Expression<Func<TARIFF, bool>> predicate = null)
        {
            IQueryable<TARIFF> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    if (predicate == null) predicate = PredicateBuilder.True<TARIFF>();
                    predicate = predicate.And(tariff => tariff.INSTALLATION.INS_ENABLED != 0);

                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.TARIFFs
                           select r)
                           .Where(predicate)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTariffs: ", e);
            }

            return res;
        }

        public IQueryable<SERVICE_CHARGE_TYPE> GetServiceChargeTypes(Expression<Func<SERVICE_CHARGE_TYPE, bool>> predicate = null)
        {
            IQueryable<SERVICE_CHARGE_TYPE> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    if (predicate == null) predicate = PredicateBuilder.True<SERVICE_CHARGE_TYPE>();
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.SERVICE_CHARGE_TYPEs
                           select r)
                           .Where(predicate)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetServiceChargeTypes: ", e);
            }

            return res;
        }

        public IQueryable<CURRENCy> GetCurrencies(Expression<Func<CURRENCy, bool>> predicate = null)
        {
            IQueryable<CURRENCy> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    if (predicate == null) predicate = PredicateBuilder.True<CURRENCy>();

                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetCurrencies(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencies: ", e);
            }

            return res;
        }
        public IQueryable<CURRENCy> GetCurrencies(Expression<Func<CURRENCy, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<CURRENCy> res = null;
            try
            {
                res = (from r in dbContext.CURRENCies
                       select r)
                       .Where(predicate)
                       .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencies: ", e);
            }

            return res;
        }


        public IQueryable<COUNTRy> GetCountries(Expression<Func<COUNTRy, bool>> predicate = null)
        {
            IQueryable<COUNTRy> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    if (predicate == null) predicate = PredicateBuilder.True<COUNTRy>();

                    res = (from r in dbContext.COUNTRies
                           select r)
                           .Where(predicate)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountries: ", e);
            }

            return res;
        }


        public IQueryable<CUSTOMER_INVOICE> GetInvoices(Expression<Func<CUSTOMER_INVOICE, bool>> predicate)
        {
            IQueryable<CUSTOMER_INVOICE> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.CUSTOMER_INVOICEs
                           where r.CUSINV_INV_NUMBER != null
                           select r)
                           .Where(predicate)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInvoices: ", e);
            }

            return res;
        }

        public IQueryable<SERVICES_PHOTO> GetServiceUserPlatesPhotos(Expression<Func<SERVICES_PHOTO, bool>> predicate, out String Error)
        {
            IQueryable<SERVICES_PHOTO> res = null;
            Error = string.Empty;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.SERVICES_PHOTOs                           
                           select r)
                           .Where(predicate)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetServiceUserPlatesPhotos: ", e);
                Error = e.Message;
            }

            return res;
        }

        public IQueryable<EXTERNAL_PARKING_OPERATION> GetExternalOperations(Expression<Func<EXTERNAL_PARKING_OPERATION, bool>> predicate)
        {
            IQueryable<EXTERNAL_PARKING_OPERATION> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                             new TransactionOptions()
                                                                             {
                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.EXTERNAL_PARKING_OPERATIONs
                           select r)
                           .Where(predicate)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetExternalOperations: ", e);
            }

            return res;
        }

        public IQueryable<EXTERNAL_PROVIDER> GetExternalProviders(Expression<Func<EXTERNAL_PROVIDER, bool>> predicate = null)
        {
            IQueryable<EXTERNAL_PROVIDER> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                             new TransactionOptions()
                                                                             {
                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                             }))
                {
                    if (predicate == null) predicate = PredicateBuilder.True<EXTERNAL_PROVIDER>();

                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.EXTERNAL_PROVIDERs
                           select r)
                           .Where(predicate)
                           .AsQueryable();
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetExternalProviders: ", e);
            }

            return res;
        }

        public IQueryable<CUSTOMER_INSCRIPTION> GetCustomerInscriptions(Expression<Func<CUSTOMER_INSCRIPTION, bool>> predicate)
        {
            IQueryable<CUSTOMER_INSCRIPTION> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.CUSTOMER_INSCRIPTIONs
                           select r)
                           .Where(predicate)
                           .OrderByDescending(t => t.CUSINS_LAST_SENT_DATE)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomerInscriptions: ", e);
            }

            return res;
        }

        public IQueryable<INSTALLATION> GetInstallations(Expression<Func<INSTALLATION, bool>> predicate)
        {
            IQueryable<INSTALLATION> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.INSTALLATIONs
                           select r)
                           .Where(predicate)
                           .OrderBy(t => t.INS_DESCRIPTION)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInstallations: ", e);
            }

            return res;
        }

        public IQueryable<SUPER_INSTALLATION> GetSuperInstallations(Expression<Func<SUPER_INSTALLATION, bool>> predicate)
        {
            IQueryable<SUPER_INSTALLATION> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.SUPER_INSTALLATIONs
                           select r)
                           .Where(predicate)
                           .OrderBy(t => t.INSTALLATION.INS_DESCRIPTION)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSuperInstallations: ", e);
            }

            return res;
        }

        public IQueryable<USERS_SECURITY_OPERATION> GetUsersSecurityOperations(Expression<Func<USERS_SECURITY_OPERATION, bool>> predicate)
        {
            IQueryable<USERS_SECURITY_OPERATION> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.USERS_SECURITY_OPERATIONs
                           select r)
                           .Where(predicate)
                           .OrderByDescending(t => t.USOP_LAST_SENT_DATE)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUsersSecurityOperations: ", e);
            }

            return res;
        }

        public IQueryable<CURRENCY_RECHARGE_VALUE> GetCurrencyRechargeValues()
        {
            IQueryable<CURRENCY_RECHARGE_VALUE> res = null;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    res = (from r in dbContext.CURRENCY_RECHARGE_VALUEs
                           select r)
                           .AsQueryable();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyRechargeValues: ", e);
            }

            return res;
        }

        public bool SetUserEnabled(decimal dUserId, bool bEnabled, out USER oUser)
        {
            bool bRes = true;
            oUser = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    string sUsername = "";

                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        oUser = (from r in dbContext.USERs
                                 where r.USR_ID == dUserId
                                 select r).First();

                        oUser.USR_ENABLED = (bEnabled ? 1 : 0);                        

                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();                        

                        sUsername = oUser.USR_USERNAME;
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SetUserEnabled: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetUserEnabled: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool InsertUsersWarnings(List<decimal> UserIds, string HeaderImage, string Title, string Body1, string Body2, string ButtonText, string ButtonAction, string FooterText, int Push, int Login, int Park, int Unpark, int Fine)
        {
            bool bRes = true;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        foreach (decimal UserId in UserIds)
                        {
                            USERS_WARNING user_warning = new USERS_WARNING()
                            {
                                UWA_USER_ID = UserId,
                                UWA_URL_IMAGE = HeaderImage,
                                UWA_TITLE = Title,
                                UWA_TEXT1 = Body1,
                                UWA_TEXT2 = Body2,
                                UWA_BUTTON1_TEXT = ButtonText,
                                UWA_BUTTON1_FUNCTION = ButtonAction,
                                UWA_BUTTON2_TEXT = FooterText,
                                UWA_STATUS = Convert.ToInt32(UserWarningStatus.Inserted),
                                UWA_TYPE_PUSH = Push,
                                UWA_TYPE_LOGIN = Login,
                                UWA_TYPE_PARK = Park,
                                UWA_TYPE_UNPARK = Unpark,
                                UWA_TYPE_FINE = Fine
                            };
                            dbContext.USERS_WARNINGs.InsertOnSubmit(user_warning);
                        }

                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "InsertUsersWarnings: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetUserEnabled: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool UpdateServiceUserPlateStatus(decimal SerupId, decimal StatusId, out string ErrorMessage)
        {
            bool bRes = false;
            ErrorMessage = string.Empty;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        var oPlate = (from r in dbContext.SERVICES_USER_PLATEs
                                      where r.SERUP_ID == SerupId
                                      select r).First();

                        if (oPlate != null)
                        {
                            var oStatus = (from r in dbContext.SERVICES_STATUS
                                           where r.SERSTA_ID == StatusId
                                           select r).First();

                            oPlate.SERVICES_STATUS = oStatus;
                            bRes = true;
                        }

                        if (bRes)
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateServiceUserPlateStatus: ", e);
                        bRes = false;
                        ErrorMessage = e.Message;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateServiceUserPlateStatus: ", e);
                bRes = false;
                ErrorMessage = e.Message;
            }

            return bRes;
        }

        public bool UpdateCountry(ref COUNTRy country)
        {
            bool bRes = false;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        decimal countryId = country.COU_ID;
                        if (countryId != 0)
                        {
                            var oCountry = (from r in dbContext.COUNTRies
                                            where r.COU_ID == countryId
                                            select r).First();

                            if (oCountry != null)
                            {
                                oCountry.COU_DESCRIPTION = country.COU_DESCRIPTION;
                                oCountry.COU_CODE = country.COU_CODE;
                                oCountry.COU_TEL_PREFIX = country.COU_TEL_PREFIX;
                                oCountry.COU_CUR_ID = country.COU_CUR_ID;
                                country = oCountry;
                                bRes = true;
                            }
                        }
                        else
                        {
                            var oCountry = (from r in dbContext.COUNTRies
                                            orderby r.COU_ID descending
                                            select r).First();
                            if (oCountry != null)
                                country.COU_ID = oCountry.COU_ID + 1;
                            else
                                country.COU_ID = 1;
                            dbContext.COUNTRies.InsertOnSubmit(country);
                            bRes = true;
                        }

                        if (bRes)
                        {
                            SecureSubmitChanges(ref dbContext);

                            transaction.Complete();
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateCountry: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateCountry: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool RestoreRechargeFromHis(CUSTOMER_PAYMENT_MEANS_RECHARGES_HI HisRecharge, out decimal? RechargeId)
        {
            bool bRes = false;
            RechargeId = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        CUSTOMER_PAYMENT_MEANS_RECHARGE RechargeToInsert = new CUSTOMER_PAYMENT_MEANS_RECHARGE();
                        RechargeToInsert.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG = HisRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG;
                        RechargeToInsert.CURRENCy = HisRecharge.CURRENCy;
                        RechargeToInsert.CUSPMR_AMOUNT = HisRecharge.CUSPMR_AMOUNT;
                        RechargeToInsert.CUSPMR_APP_VERSION = HisRecharge.CUSPMR_APP_VERSION;
                        RechargeToInsert.CUSPMR_AUTH_CODE = HisRecharge.CUSPMR_AUTH_CODE;
                        RechargeToInsert.CUSPMR_AUTH_RESULT = HisRecharge.CUSPMR_AUTH_RESULT;
                        RechargeToInsert.CUSPMR_BACKOFFICE_USR = HisRecharge.CUSPMR_BACKOFFICE_USR;
                        RechargeToInsert.CUSPMR_BALANCE_BEFORE = HisRecharge.CUSPMR_BALANCE_BEFORE;
                        RechargeToInsert.CUSPMR_BALANCE_BONIFICATION_AMOUNT = HisRecharge.CUSPMR_BALANCE_BONIFICATION_AMOUNT;
                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_FRICTIONLESS = HisRecharge.CUSPMR_BSREDSYS_3DS_FRICTIONLESS;
                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_NUM_INLINE_FORMS = HisRecharge.CUSPMR_BSREDSYS_3DS_NUM_INLINE_FORMS;
                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_PROTOCOL_VERSION = HisRecharge.CUSPMR_BSREDSYS_3DS_PROTOCOL_VERSION;
                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_TRANS_ID = HisRecharge.CUSPMR_BSREDSYS_3DS_TRANS_ID;
                        RechargeToInsert.CUSPMR_CARD_EXPIRATION_DATE = HisRecharge.CUSPMR_CARD_EXPIRATION_DATE;
                        RechargeToInsert.CUSPMR_CARD_HASH = HisRecharge.CUSPMR_CARD_HASH;
                        RechargeToInsert.CUSPMR_CARD_REFERENCE = HisRecharge.CUSPMR_CARD_REFERENCE;
                        RechargeToInsert.CUSPMR_CARD_SCHEME = HisRecharge.CUSPMR_CARD_SCHEME;
                        RechargeToInsert.CUSPMR_CASHPAY_BARCODE = HisRecharge.CUSPMR_CASHPAY_BARCODE;
                        RechargeToInsert.CUSPMR_CASHPAY_EXPIRATION_DATE = HisRecharge.CUSPMR_CASHPAY_EXPIRATION_DATE;
                        RechargeToInsert.CUSPMR_CASHPAY_PAYU_URL = HisRecharge.CUSPMR_CASHPAY_PAYU_URL;
                        RechargeToInsert.CUSPMR_CASHPAY_REFERENCE = HisRecharge.CUSPMR_CASHPAY_REFERENCE;
                        RechargeToInsert.CUSPMR_CF_TRANSACTION_ID = HisRecharge.CUSPMR_CF_TRANSACTION_ID;
                        RechargeToInsert.CUSPMR_CPTGC_ID = HisRecharge.CUSPMR_CPTGC_ID;
                        RechargeToInsert.CUSPMR_CREATION_TYPE = HisRecharge.CUSPMR_CREATION_TYPE;
                        RechargeToInsert.CUSPMR_CUR_ID = HisRecharge.CUSPMR_CUR_ID;
                        RechargeToInsert.CUSPMR_CUS_ID = HisRecharge.CUSPMR_CUS_ID;
                        RechargeToInsert.CUSPMR_CUSINV_ID = HisRecharge.CUSPMR_CUSINV_ID;
                        RechargeToInsert.CUSPMR_CUSPM_ID = HisRecharge.CUSPMR_CUSPM_ID;
                        RechargeToInsert.CUSPMR_DATE = HisRecharge.CUSPMR_DATE;
                        RechargeToInsert.CUSPMR_DATE_UTC_OFFSET = HisRecharge.CUSPMR_DATE_UTC_OFFSET;
                        RechargeToInsert.CUSPMR_FDO_ID = HisRecharge.CUSPMR_FDO_ID;
                        RechargeToInsert.CUSPMR_FIXED_FEE = HisRecharge.CUSPMR_FIXED_FEE;
                        RechargeToInsert.CUSPMR_GATEWAY_DATE = HisRecharge.CUSPMR_GATEWAY_DATE;
                        RechargeToInsert.CUSPMR_INS_ID = HisRecharge.CUSPMR_INS_ID;
                        RechargeToInsert.CUSPMR_INSERTION_UTC_DATE = HisRecharge.CUSPMR_INSERTION_UTC_DATE;
                        RechargeToInsert.CUSPMR_LATITUDE = HisRecharge.CUSPMR_LATITUDE;
                        RechargeToInsert.CUSPMR_LONGITUDE = HisRecharge.CUSPMR_LONGITUDE;
                        RechargeToInsert.CUSPMR_MASKED_CARD_NUMBER = HisRecharge.CUSPMR_MASKED_CARD_NUMBER;
                        RechargeToInsert.CUSPMR_MONERIS_CAVV = HisRecharge.CUSPMR_MONERIS_CAVV;
                        RechargeToInsert.CUSPMR_MONERIS_ECI = HisRecharge.CUSPMR_MONERIS_ECI;
                        RechargeToInsert.CUSPMR_MONERIS_MD = HisRecharge.CUSPMR_MONERIS_MD;
                        RechargeToInsert.CUSPMR_MOSE_OS = HisRecharge.CUSPMR_MOSE_OS;
                        RechargeToInsert.CUSPMR_OP_REFERENCE = HisRecharge.CUSPMR_OP_REFERENCE;
                        RechargeToInsert.CUSPMR_PAGATELIA_NEW_BALANCE = HisRecharge.CUSPMR_PAGATELIA_NEW_BALANCE;
                        RechargeToInsert.CUSPMR_PARTIAL_FIXED_FEE = HisRecharge.CUSPMR_PARTIAL_FIXED_FEE;
                        RechargeToInsert.CUSPMR_PARTIAL_PERC_FEE = HisRecharge.CUSPMR_PARTIAL_PERC_FEE;
                        RechargeToInsert.CUSPMR_PARTIAL_VAT1 = HisRecharge.CUSPMR_PARTIAL_VAT1;
                        RechargeToInsert.CUSPMR_PAYPAL_3T_PAYER_ID = HisRecharge.CUSPMR_PAYPAL_3T_PAYER_ID;
                        RechargeToInsert.CUSPMR_PAYPAL_3T_TOKEN = HisRecharge.CUSPMR_PAYPAL_3T_TOKEN;
                        RechargeToInsert.CUSPMR_PAYPAL_INTENT = HisRecharge.CUSPMR_PAYPAL_INTENT;
                        RechargeToInsert.CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY = HisRecharge.CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY;
                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_FEE_CURRENCY_ISOCODE = HisRecharge.CUSPMR_PAYPAL_TRANSACTION_FEE_CURRENCY_ISOCODE;
                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_FEE_VALUE = HisRecharge.CUSPMR_PAYPAL_TRANSACTION_FEE_VALUE;
                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_REFUND_URL = HisRecharge.CUSPMR_PAYPAL_TRANSACTION_REFUND_URL;
                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_URL = HisRecharge.CUSPMR_PAYPAL_TRANSACTION_URL;
                        RechargeToInsert.CUSPMR_PERC_FEE = HisRecharge.CUSPMR_PERC_FEE;
                        RechargeToInsert.CUSPMR_PERC_FEE_TOPPED = HisRecharge.CUSPMR_PERC_FEE_TOPPED;
                        RechargeToInsert.CUSPMR_PERC_VAT1 = HisRecharge.CUSPMR_PERC_VAT1;
                        RechargeToInsert.CUSPMR_PERC_VAT2 = HisRecharge.CUSPMR_PERC_VAT2;
                        RechargeToInsert.CUSPMR_RCOUP_ID = HisRecharge.CUSPMR_RCOUP_ID;
                        RechargeToInsert.CUSPMR_REFUND_AMOUNT = HisRecharge.CUSPMR_REFUND_AMOUNT;
                        RechargeToInsert.CUSPMR_REFUND_OPE_ID = HisRecharge.CUSPMR_REFUND_OPE_ID;
                        RechargeToInsert.CUSPMR_RETRIES_NUM = HisRecharge.CUSPMR_RETRIES_NUM;
                        RechargeToInsert.CUSPMR_SECOND_AUTH_RESULT = HisRecharge.CUSPMR_SECOND_AUTH_RESULT;
                        RechargeToInsert.CUSPMR_SECOND_CF_TRANSACTION_ID = HisRecharge.CUSPMR_SECOND_CF_TRANSACTION_ID;
                        RechargeToInsert.CUSPMR_SECOND_GATEWAY_DATE = HisRecharge.CUSPMR_SECOND_GATEWAY_DATE;
                        RechargeToInsert.CUSPMR_SECOND_OP_REFERENCE = HisRecharge.CUSPMR_SECOND_OP_REFERENCE;
                        RechargeToInsert.CUSPMR_SECOND_TRANSACTION_ID = HisRecharge.CUSPMR_SECOND_TRANSACTION_ID;
                        RechargeToInsert.CUSPMR_SOAPP_ID = HisRecharge.CUSPMR_SOAPP_ID;
                        RechargeToInsert.CUSPMR_SRC_AMOUNT = HisRecharge.CUSPMR_SRC_AMOUNT;
                        RechargeToInsert.CUSPMR_SRC_CHANGE_APPLIED = HisRecharge.CUSPMR_SRC_CHANGE_APPLIED;
                        RechargeToInsert.CUSPMR_SRC_CHANGE_FEE_APPLIED = HisRecharge.CUSPMR_SRC_CHANGE_FEE_APPLIED;
                        RechargeToInsert.CUSPMR_SRC_CUR_ID = HisRecharge.CUSPMR_SRC_CUR_ID;
                        RechargeToInsert.CUSPMR_STATUS_DATE = HisRecharge.CUSPMR_STATUS_DATE;
                        RechargeToInsert.CUSPMR_SUSCRIPTION_TYPE = HisRecharge.CUSPMR_SUSCRIPTION_TYPE;
                        RechargeToInsert.CUSPMR_TOTAL_AMOUNT_CHARGED = HisRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED;
                        RechargeToInsert.CUSPMR_TRANS_STATUS = HisRecharge.CUSPMR_TRANS_STATUS;
                        RechargeToInsert.CUSPMR_TRANSACTION_ID = HisRecharge.CUSPMR_TRANSACTION_ID;
                        RechargeToInsert.CUSPMR_TYPE = HisRecharge.CUSPMR_TYPE;
                        RechargeToInsert.CUSPMR_USR_ID = HisRecharge.CUSPMR_USR_ID;
                        RechargeToInsert.CUSPMR_UTC_DATE = HisRecharge.CUSPMR_UTC_DATE;
                        RechargeToInsert.CUSTOMER = HisRecharge.CUSTOMER;
                        RechargeToInsert.CUSTOMER_INVOICE = HisRecharge.CUSTOMER_INVOICE;
                        RechargeToInsert.CUSTOMER_PAYMENT_MEAN = HisRecharge.CUSTOMER_PAYMENT_MEAN;
                        RechargeToInsert.FINAN_DIST_OPERATOR = HisRecharge.FINAN_DIST_OPERATOR;                        
                        RechargeToInsert.INSTALLATION = HisRecharge.INSTALLATION;
                        RechargeToInsert.RECHARGE_COUPON = HisRecharge.RECHARGE_COUPON;
                        RechargeToInsert.SOURCE_APP = HisRecharge.SOURCE_APP;
                        RechargeToInsert.USER = HisRecharge.USER;

                        dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGES_HIs.DeleteOnSubmit(HisRecharge);
                        dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs.InsertOnSubmit(RechargeToInsert);

                        SecureSubmitChanges(ref dbContext);

                        transaction.Complete();

                        RechargeId = RechargeToInsert.CUSPMR_ID;

                        bRes = true;

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "RestoreRechargeFromHis: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RestoreRechargeFromHis: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool DeleteCountry(ref COUNTRy country)
        {
            bool bRes = false;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        decimal Id = country.COU_ID;
                        var oCountry = (from r in dbContext.COUNTRies
                                        where r.COU_ID == Id
                                        select r).First();

                        if (oCountry != null)
                        {
                            dbContext.COUNTRies.DeleteOnSubmit(oCountry);
                        }

                        SecureSubmitChanges(ref dbContext);

                        transaction.Complete();

                        country = null;
                        bRes = true;

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "DeleteCountry: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteCountry: ", e);
                bRes = false;
            }

            return bRes;

        }

        public IQueryable<OPERATION> GetOperations(Expression<Func<OPERATION, bool>> predicate)
        {
            IQueryable<OPERATION> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetOperations(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperations: ", e);
            }

            return res;
        }
        public IQueryable<OPERATION> GetOperations(Expression<Func<OPERATION, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<OPERATION> res = null;
            try
            {
                res = (from r in dbContext.OPERATIONs
                        select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperations: ", e);
            }

            return res;
        }

        public IQueryable<HIS_OPERATION> GetHisOperations(Expression<Func<HIS_OPERATION, bool>> predicate)
        {
            IQueryable<HIS_OPERATION> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetHisOperations(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetHisOperations: ", e);
            }

            return res;
        }
        public IQueryable<HIS_OPERATION> GetHisOperations(Expression<Func<HIS_OPERATION, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<HIS_OPERATION> res = null;
            try
            {
                res = (from r in dbContext.HIS_OPERATIONs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetHisOperations: ", e);
            }

            return res;
        }

        public IQueryable<TICKET_PAYMENT> GetTicketPayments(Expression<Func<TICKET_PAYMENT, bool>> predicate)
        {
            IQueryable<TICKET_PAYMENT> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetTicketPayments(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTicketPayments: ", e);
            }

            return res;
        }

        public IQueryable<TICKET_PAYMENT> GetTicketPayments(Expression<Func<TICKET_PAYMENT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<TICKET_PAYMENT> res = null;
            try
            {
                res = (from r in dbContext.TICKET_PAYMENTs
                        select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTicketPayments: ", e);
            }

            return res;
        }

        public IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGE> GetCustomerRecharges(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGE, bool>> predicate)
        {
            IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGE> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetCustomerRecharges(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomerRecharges: ", e);
            }

            return res;
        }

        public IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI> GetCustomerRechargesHis(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI, bool>> predicate)
        {
            IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetCustomerRechargesHis(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomerRechargesHis: ", e);
            }

            return res;
        }

        public IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGE> GetCustomerRecharges(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGE> res = null;
            try
            {
                res = (from r in dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs
                        select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomerRecharges: ", e);
            }

            return res;
        }

        public IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI> GetCustomerRechargesHis(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI> res = null;
            try
            {
                res = (from r in dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGES_HIs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomerRechargesHis: ", e);
            }

            return res;
        }

        public IQueryable<SERVICE_CHARGE> GetServiceCharges(Expression<Func<SERVICE_CHARGE, bool>> predicate)
        {
            IQueryable<SERVICE_CHARGE> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetServiceCharges(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetServiceCharges: ", e);
            }

            return res;
        }
        public IQueryable<SERVICE_CHARGE> GetServiceCharges(Expression<Func<SERVICE_CHARGE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<SERVICE_CHARGE> res = null;
            try
            {
                res = (from r in dbContext.SERVICE_CHARGEs
                        select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetServiceCharges: ", e);
            }

            return res;
        }

        public IQueryable<OPERATIONS_DISCOUNT> GetDiscounts(Expression<Func<OPERATIONS_DISCOUNT, bool>> predicate)
        {
            IQueryable<OPERATIONS_DISCOUNT> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetDiscounts(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDiscounts: ", e);
            }

            return res;
        }
        public IQueryable<OPERATIONS_DISCOUNT> GetDiscounts(Expression<Func<OPERATIONS_DISCOUNT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<OPERATIONS_DISCOUNT> res = null;
            try
            {
                res = (from r in dbContext.OPERATIONS_DISCOUNTs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDiscounts: ", e);
            }

            return res;
        }

        public IQueryable<OPERATIONS_OFFSTREET> GetOperationsOffstreet(Expression<Func<OPERATIONS_OFFSTREET, bool>> predicate)
        {
            IQueryable<OPERATIONS_OFFSTREET> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetOperationsOffstreet(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperationsOffstreet: ", e);
            }

            return res;
        }
        public IQueryable<OPERATIONS_OFFSTREET> GetOperationsOffstreet(Expression<Func<OPERATIONS_OFFSTREET, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<OPERATIONS_OFFSTREET> res = null;
            try
            {
                res = (from r in dbContext.OPERATIONS_OFFSTREETs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOperationsOffstreet: ", e);
            }

            return res;
        }

        public IQueryable<BALANCE_TRANSFER> GetBalanceTransfers(Expression<Func<BALANCE_TRANSFER, bool>> predicate)
        {
            IQueryable<BALANCE_TRANSFER> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetBalanceTransfers(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetBalanceTransfers: ", e);
            }

            return res;
        }
        public IQueryable<BALANCE_TRANSFER> GetBalanceTransfers(Expression<Func<BALANCE_TRANSFER, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<BALANCE_TRANSFER> res = null;
            try
            {
                res = (from r in dbContext.BALANCE_TRANSFERs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetBalanceTransfers: ", e);
            }

            return res;
        }

        public bool DeleteOperation(ChargeOperationsType type, decimal operationId, out object oDeleted, out int iBalanceBefore, out USER oUser, out OPERATIONS_DISCOUNT oDiscountDeleted, out bool bIsHisOperation)
        {            
            oDeleted = null;
            iBalanceBefore = 0;
            oUser = null;
            oDiscountDeleted = null;
            bIsHisOperation = false;
            bool bErrorAccess = false;
            return DeleteOperation(type, operationId, out oDeleted, out iBalanceBefore, out oUser, out oDiscountDeleted, out bIsHisOperation, null, out bErrorAccess);
        }
        public bool DeleteOperation(ChargeOperationsType type, decimal operationId, out object oDeleted, out int iBalanceBefore, out USER oUser, out OPERATIONS_DISCOUNT oDiscountDeleted, out bool bIsHisOperation, List<int> oInstallationsAllowed, out bool bErrorAccess)
        {
            return DeleteOperation(type, operationId, out oDeleted, out iBalanceBefore, out oUser, out oDiscountDeleted, out bIsHisOperation, oInstallationsAllowed, out bErrorAccess, PaymentMeanRechargeStatus.Waiting_Refund);
        }
        public bool DeleteOperation(ChargeOperationsType type, decimal operationId, out object oDeleted, out int iBalanceBefore, out USER oUser, out OPERATIONS_DISCOUNT oDiscountDeleted, out bool bIsHisOperation, List<int> oInstallationsAllowed, out bool bErrorAccess, PaymentMeanRechargeStatus oRechargeStatus)
        {
            bool bRes = false;
            oDeleted = null;
            iBalanceBefore = 0;
            oUser = null;
            oDiscountDeleted = null;
            bIsHisOperation = false;
            bErrorAccess = false;
            INSTALLATION oInstallation = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        decimal dUserId = 0;                        
                        int iAmount = 0;
                        DateTime? oUtcDate = null;

                        if (type == ChargeOperationsType.ParkingOperation ||
                            type == ChargeOperationsType.ExtensionOperation ||
                            type == ChargeOperationsType.Permit) // ParkingRefund operations removed (IPM-2278 - Case 1 - Parking Refund)
                        {
                            //m_Log.LogMessage(LogLevels.logWARN, string.Format("DeleteOperation: Starting delete OPE_ID={0} ...", operationId));

                            // IPM-2278 - Case 2 - Parking or Extension
                            var predicateOp = PredicateBuilder.True<OPERATION>();
                            predicateOp = predicateOp.And(o => o.OPE_ID == operationId);
                            var operations = GetOperations(predicateOp, dbContext);
                            if (operations.Count() > 0)
                            {
                                bIsHisOperation = false;
                                var oOperation = operations.First();
                                if (oInstallationsAllowed != null) {
                                    bErrorAccess = !oInstallationsAllowed.Contains(Convert.ToInt32(oOperation.INSTALLATION.INS_STANDARD_CITY_ID));
                                }
                                if (!bErrorAccess)
                                {
                                    // We have to check if there is a ParkingRefund operation related to our operation (same operation external base id)
                                    // If any related operation is a ParkingRefund, delete operation has to be aborted
                                    var predicateOp2 = PredicateBuilder.True<OPERATION>();
                                    predicateOp2 = predicateOp2.And(o => o.OPE_EXTERNAL_BASE_ID1 == oOperation.OPE_EXTERNAL_BASE_ID1 &&
                                                                         o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund);
                                    var relatedOperations = GetOperations(predicateOp2, dbContext);
                                    // IPM-2278 - Case 2.1 - Existing parking refund in the same base operation
                                    bErrorAccess = (relatedOperations.Any());
                                    
                                    if (bErrorAccess == false)
                                    {
                                        // IPM-2278 - Case 2.2 - No parking refund
                                        dUserId = oOperation.OPE_USR_ID;
                                        if (oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                                        {
                                            // IPM-2278 - Case 2.2.2 - Prepayment                                            
                                            if (oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                            {
                                                // IPM-2278 - Case 2.2.2.2 - With recharge                                                                                                
                                                if (oOperation.OPE_TOTAL_AMOUNT.Value >= oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AMOUNT)
                                                {
                                                    // IPM-2278 - Case 2.2.2.2.1 - Q.Parking >= Q.Recharge
                                                    iAmount = -(oOperation.OPE_TOTAL_AMOUNT.Value - oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AMOUNT);
                                                    oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;
                                                }
                                                else {
                                                    // IPM-2278 - Case 2.2.2.2.2 - Q.Parking < Q.Recharge
                                                    iAmount = -(oOperation.OPE_TOTAL_AMOUNT.Value);
                                                }                                                    
                                            }
                                            else 
                                            {
                                                // IPM-2278 - Case 2.2.2.1 - No recharge 
                                                iAmount = -oOperation.OPE_TOTAL_AMOUNT.Value;                                            
                                            }

                                            if (oOperation.OPERATIONS_DISCOUNT != null)
                                            {
                                                iAmount += oOperation.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                                if (oOperation.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE != null)
                                                    oOperation.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS += oOperation.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                            }
                                        }
                                        else if (oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction && oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                        {
                                            // IPM-2278 - Case 2.2.1 - Per transaction 
                                            oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;                                            
                                        }

                                        if (oOperation.CUSTOMER_INVOICE != null)
                                        {
                                            if (type != ChargeOperationsType.ParkingRefund)
                                                oOperation.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS -= oOperation.OPE_TOTAL_AMOUNT.Value;
                                            else
                                                oOperation.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS += oOperation.OPE_TOTAL_AMOUNT.Value;
                                        }

                                        oUtcDate = oOperation.OPE_UTC_DATE;
                                        oDeleted = oOperation;
                                        oInstallation = oOperation.INSTALLATION;
                                        if (oOperation.OPERATIONS_DISCOUNT != null)
                                        {
                                            oDiscountDeleted = oOperation.OPERATIONS_DISCOUNT;
                                            dbContext.OPERATIONS_DISCOUNTs.DeleteOnSubmit(oOperation.OPERATIONS_DISCOUNT);
                                        }
                                        dbContext.OPERATIONs.DeleteOnSubmit(oOperation);
                                    }
                                    else
                                        m_Log.LogMessage(LogLevels.logWARN, string.Format("DeleteOperation: Operation with refund OPE_ID={0}, BASE_ID={1}", oOperation.OPE_ID, oOperation.OPE_EXTERNAL_BASE_ID1));
                                }
                                else
                                    m_Log.LogMessage(LogLevels.logWARN, string.Format("DeleteOperation: Operation installation not allowed OPE_ID={0}, INS_ID={1}", oOperation.OPE_ID, oOperation.INSTALLATION.INS_STANDARD_CITY_ID));
                            }
                            else
                            {
                                var predicateHisOp = PredicateBuilder.True<HIS_OPERATION>();
                                predicateHisOp = predicateHisOp.And(o => o.OPE_ID == operationId);
                                var hisOperations = GetHisOperations(predicateHisOp, dbContext);
                                if (hisOperations.Count() > 0)
                                {
                                    bIsHisOperation = true;
                                    var oHisOperation = hisOperations.First();
                                    if (oInstallationsAllowed != null)
                                    {
                                        bErrorAccess = !oInstallationsAllowed.Contains(Convert.ToInt32(oHisOperation.INSTALLATION.INS_STANDARD_CITY_ID));
                                    }
                                    if (!bErrorAccess)
                                    {
                                        // We have to check if there is a ParkingRefund operation related to our operation (same operation external base id)
                                        // If any related operation is a ParkingRefund, delete operation has to be aborted
                                        var predicateOp2 = PredicateBuilder.True<OPERATION>();
                                        predicateOp2 = predicateOp2.And(o => o.OPE_EXTERNAL_BASE_ID1 == oHisOperation.OPE_EXTERNAL_BASE_ID1 &&
                                                                             o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund);
                                        var relatedOperations = GetOperations(predicateOp2, dbContext);
                                        // IPM-2278 - Case 2.1 - Existing parking refund in the same base operation
                                        bErrorAccess = (relatedOperations.Any());

                                        if (bErrorAccess == false)
                                        {
                                            // IPM-2278 - Case 2.2 - No parking refund
                                            dUserId = oHisOperation.OPE_USR_ID;
                                            if (oHisOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                                            {
                                                // IPM-2278 - Case 2.2.2 - Prepayment                                            
                                                if (oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI != null)
                                                {
                                                    // IPM-2278 - Case 2.2.2.2 - With recharge                                                                                                
                                                    if (oHisOperation.OPE_TOTAL_AMOUNT.Value >= oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AMOUNT)
                                                    {
                                                        // IPM-2278 - Case 2.2.2.2.1 - Q.Parking >= Q.Recharge
                                                        iAmount = -(oHisOperation.OPE_TOTAL_AMOUNT.Value - oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AMOUNT);
                                                        oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;
                                                        oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_STATUS_DATE = DateTime.UtcNow;

                                                        // START BSU-871

                                                        CUSTOMER_PAYMENT_MEANS_RECHARGE RechargeToInsert = new CUSTOMER_PAYMENT_MEANS_RECHARGE();                                                        
                                                        RechargeToInsert.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG;
                                                        RechargeToInsert.CURRENCy = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CURRENCy;
                                                        RechargeToInsert.CUSPMR_AMOUNT = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AMOUNT;
                                                        RechargeToInsert.CUSPMR_APP_VERSION = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_APP_VERSION;
                                                        RechargeToInsert.CUSPMR_AUTH_CODE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AUTH_CODE;
                                                        RechargeToInsert.CUSPMR_AUTH_RESULT = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AUTH_RESULT;
                                                        RechargeToInsert.CUSPMR_BACKOFFICE_USR = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_BACKOFFICE_USR;
                                                        RechargeToInsert.CUSPMR_BALANCE_BEFORE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_BALANCE_BEFORE;
                                                        RechargeToInsert.CUSPMR_BALANCE_BONIFICATION_AMOUNT = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_BALANCE_BONIFICATION_AMOUNT;
                                                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_FRICTIONLESS = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_BSREDSYS_3DS_FRICTIONLESS;
                                                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_NUM_INLINE_FORMS = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_BSREDSYS_3DS_NUM_INLINE_FORMS;
                                                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_PROTOCOL_VERSION = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_BSREDSYS_3DS_PROTOCOL_VERSION;
                                                        RechargeToInsert.CUSPMR_BSREDSYS_3DS_TRANS_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_BSREDSYS_3DS_TRANS_ID;
                                                        RechargeToInsert.CUSPMR_CARD_EXPIRATION_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CARD_EXPIRATION_DATE;
                                                        RechargeToInsert.CUSPMR_CARD_HASH = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CARD_HASH;
                                                        RechargeToInsert.CUSPMR_CARD_REFERENCE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CARD_REFERENCE;
                                                        RechargeToInsert.CUSPMR_CARD_SCHEME = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CARD_SCHEME;
                                                        RechargeToInsert.CUSPMR_CASHPAY_BARCODE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CASHPAY_BARCODE;
                                                        RechargeToInsert.CUSPMR_CASHPAY_EXPIRATION_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CASHPAY_EXPIRATION_DATE;
                                                        RechargeToInsert.CUSPMR_CASHPAY_PAYU_URL = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CASHPAY_PAYU_URL;
                                                        RechargeToInsert.CUSPMR_CASHPAY_REFERENCE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CASHPAY_REFERENCE;
                                                        RechargeToInsert.CUSPMR_CF_TRANSACTION_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CF_TRANSACTION_ID;
                                                        RechargeToInsert.CUSPMR_CPTGC_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CPTGC_ID;
                                                        RechargeToInsert.CUSPMR_CREATION_TYPE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CREATION_TYPE;
                                                        RechargeToInsert.CUSPMR_CUR_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CUR_ID;
                                                        RechargeToInsert.CUSPMR_CUS_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CUS_ID;
                                                        RechargeToInsert.CUSPMR_CUSINV_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CUSINV_ID;
                                                        RechargeToInsert.CUSPMR_CUSPM_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_CUSPM_ID;
                                                        RechargeToInsert.CUSPMR_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_DATE;
                                                        RechargeToInsert.CUSPMR_DATE_UTC_OFFSET = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_DATE_UTC_OFFSET;
                                                        RechargeToInsert.CUSPMR_FDO_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_FDO_ID;
                                                        RechargeToInsert.CUSPMR_FIXED_FEE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_FIXED_FEE;
                                                        RechargeToInsert.CUSPMR_GATEWAY_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_GATEWAY_DATE;
                                                        RechargeToInsert.CUSPMR_INS_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_INS_ID;
                                                        RechargeToInsert.CUSPMR_INSERTION_UTC_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_INSERTION_UTC_DATE;
                                                        RechargeToInsert.CUSPMR_LATITUDE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_LATITUDE;
                                                        RechargeToInsert.CUSPMR_LONGITUDE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_LONGITUDE;
                                                        RechargeToInsert.CUSPMR_MASKED_CARD_NUMBER = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_MASKED_CARD_NUMBER;
                                                        RechargeToInsert.CUSPMR_MONERIS_CAVV = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_MONERIS_CAVV;
                                                        RechargeToInsert.CUSPMR_MONERIS_ECI = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_MONERIS_ECI;
                                                        RechargeToInsert.CUSPMR_MONERIS_MD = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_MONERIS_MD;
                                                        RechargeToInsert.CUSPMR_MOSE_OS = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_MOSE_OS;
                                                        RechargeToInsert.CUSPMR_OP_REFERENCE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_OP_REFERENCE;
                                                        RechargeToInsert.CUSPMR_PAGATELIA_NEW_BALANCE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAGATELIA_NEW_BALANCE;
                                                        RechargeToInsert.CUSPMR_PARTIAL_FIXED_FEE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PARTIAL_FIXED_FEE;
                                                        RechargeToInsert.CUSPMR_PARTIAL_PERC_FEE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PARTIAL_PERC_FEE;
                                                        RechargeToInsert.CUSPMR_PARTIAL_VAT1 = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PARTIAL_VAT1;
                                                        RechargeToInsert.CUSPMR_PAYPAL_3T_PAYER_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_3T_PAYER_ID;
                                                        RechargeToInsert.CUSPMR_PAYPAL_3T_TOKEN = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_3T_TOKEN;
                                                        RechargeToInsert.CUSPMR_PAYPAL_INTENT = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_INTENT;
                                                        RechargeToInsert.CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY;
                                                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_FEE_CURRENCY_ISOCODE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_TRANSACTION_FEE_CURRENCY_ISOCODE;
                                                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_FEE_VALUE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_TRANSACTION_FEE_VALUE;
                                                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_REFUND_URL = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_TRANSACTION_REFUND_URL;
                                                        RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_URL = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PAYPAL_TRANSACTION_URL;
                                                        RechargeToInsert.CUSPMR_PERC_FEE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PERC_FEE;
                                                        RechargeToInsert.CUSPMR_PERC_FEE_TOPPED = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PERC_FEE_TOPPED;
                                                        RechargeToInsert.CUSPMR_PERC_VAT1 = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PERC_VAT1;
                                                        RechargeToInsert.CUSPMR_PERC_VAT2 = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_PERC_VAT2;
                                                        RechargeToInsert.CUSPMR_RCOUP_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_RCOUP_ID;
                                                        RechargeToInsert.CUSPMR_REFUND_AMOUNT = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_REFUND_AMOUNT;
                                                        RechargeToInsert.CUSPMR_REFUND_OPE_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_REFUND_OPE_ID;
                                                        RechargeToInsert.CUSPMR_RETRIES_NUM = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_RETRIES_NUM;
                                                        RechargeToInsert.CUSPMR_SECOND_AUTH_RESULT = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SECOND_AUTH_RESULT;
                                                        RechargeToInsert.CUSPMR_SECOND_CF_TRANSACTION_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SECOND_CF_TRANSACTION_ID;
                                                        RechargeToInsert.CUSPMR_SECOND_GATEWAY_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SECOND_GATEWAY_DATE;
                                                        RechargeToInsert.CUSPMR_SECOND_OP_REFERENCE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SECOND_OP_REFERENCE;
                                                        RechargeToInsert.CUSPMR_SECOND_TRANSACTION_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SECOND_TRANSACTION_ID;
                                                        RechargeToInsert.CUSPMR_SOAPP_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SOAPP_ID;
                                                        RechargeToInsert.CUSPMR_SRC_AMOUNT = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SRC_AMOUNT;
                                                        RechargeToInsert.CUSPMR_SRC_CHANGE_APPLIED = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SRC_CHANGE_APPLIED;
                                                        RechargeToInsert.CUSPMR_SRC_CHANGE_FEE_APPLIED = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SRC_CHANGE_FEE_APPLIED;
                                                        RechargeToInsert.CUSPMR_SRC_CUR_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SRC_CUR_ID;
                                                        RechargeToInsert.CUSPMR_STATUS_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_STATUS_DATE;
                                                        RechargeToInsert.CUSPMR_SUSCRIPTION_TYPE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_SUSCRIPTION_TYPE;
                                                        RechargeToInsert.CUSPMR_TOTAL_AMOUNT_CHARGED = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TOTAL_AMOUNT_CHARGED;
                                                        RechargeToInsert.CUSPMR_TRANS_STATUS = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANS_STATUS;
                                                        RechargeToInsert.CUSPMR_TRANSACTION_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANSACTION_ID;
                                                        RechargeToInsert.CUSPMR_TYPE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TYPE;
                                                        RechargeToInsert.CUSPMR_USR_ID = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_USR_ID;
                                                        RechargeToInsert.CUSPMR_UTC_DATE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_UTC_DATE;
                                                        RechargeToInsert.CUSTOMER = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSTOMER;
                                                        RechargeToInsert.CUSTOMER_INVOICE = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSTOMER_INVOICE;
                                                        RechargeToInsert.CUSTOMER_PAYMENT_MEAN = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSTOMER_PAYMENT_MEAN;
                                                        RechargeToInsert.FINAN_DIST_OPERATOR = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.FINAN_DIST_OPERATOR;
                                                        RechargeToInsert.INSTALLATION = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.INSTALLATION;
                                                        RechargeToInsert.RECHARGE_COUPON = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.RECHARGE_COUPON;
                                                        RechargeToInsert.SOURCE_APP = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.SOURCE_APP;
                                                        RechargeToInsert.USER = oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.USER;

                                                        dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGES_HIs.DeleteOnSubmit(oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI);
                                                        dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs.InsertOnSubmit(RechargeToInsert);

                                                        // END BSU-871
                                                    }
                                                    else
                                                    {
                                                        // IPM-2278 - Case 2.2.2.2.2 - Q.Parking < Q.Recharge
                                                        iAmount = -(oHisOperation.OPE_TOTAL_AMOUNT.Value);
                                                    }
                                                }
                                                else
                                                {
                                                    // IPM-2278 - Case 2.2.2.1 - No recharge 
                                                    iAmount = -oHisOperation.OPE_TOTAL_AMOUNT.Value;
                                                }

                                                if (oHisOperation.OPERATIONS_DISCOUNT != null)
                                                {
                                                    iAmount += oHisOperation.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                                    if (oHisOperation.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE != null)
                                                        oHisOperation.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS += oHisOperation.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                                }
                                            }
                                            else if (oHisOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction && oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI != null)
                                            {
                                                // IPM-2278 - Case 2.2.1 - Per transaction 
                                                oHisOperation.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;
                                            }

                                            if (oHisOperation.CUSTOMER_INVOICE != null)
                                            {
                                                if (type != ChargeOperationsType.ParkingRefund)
                                                    oHisOperation.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS -= oHisOperation.OPE_TOTAL_AMOUNT.Value;
                                                else
                                                    oHisOperation.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS += oHisOperation.OPE_TOTAL_AMOUNT.Value;
                                            }

                                            oUtcDate = oHisOperation.OPE_UTC_DATE;
                                            oDeleted = oHisOperation;
                                            oInstallation = oHisOperation.INSTALLATION;
                                            if (oHisOperation.OPERATIONS_DISCOUNT != null)
                                            {
                                                oDiscountDeleted = oHisOperation.OPERATIONS_DISCOUNT;
                                                dbContext.OPERATIONS_DISCOUNTs.DeleteOnSubmit(oHisOperation.OPERATIONS_DISCOUNT);
                                            }
                                            dbContext.HIS_OPERATIONs.DeleteOnSubmit(oHisOperation);
                                        }
                                        else
                                            m_Log.LogMessage(LogLevels.logWARN, string.Format("DeleteOperation: HisOperation with refund OPE_ID={0}, BASE_ID={1}", oHisOperation.OPE_ID, oHisOperation.OPE_EXTERNAL_BASE_ID1));
                                    }
                                    else
                                        m_Log.LogMessage(LogLevels.logWARN, string.Format("DeleteOperation: HisOperation installation not allowed OPE_ID={0}, INS_ID={1}", oHisOperation.OPE_ID, oHisOperation.INSTALLATION.INS_STANDARD_CITY_ID));
                                }
                                else
                                {
                                    m_Log.LogMessage(LogLevels.logWARN, string.Format("DeleteOperation: Operation not found ID={0}", operationId));
                                }
                            }

                        }
                        else if (type == ChargeOperationsType.TicketPayment)
                        {

                            var predicateTicket = PredicateBuilder.True<TICKET_PAYMENT>();
                            predicateTicket = predicateTicket.And(t => t.TIPA_ID == operationId);
                            var tickets = GetTicketPayments(predicateTicket, dbContext);
                            if (tickets.Count() > 0)
                            {
                                var oTicket = tickets.First();
                                if (oInstallationsAllowed != null) {
                                    bErrorAccess = !oInstallationsAllowed.Contains(Convert.ToInt32(oTicket.INSTALLATION.INS_STANDARD_CITY_ID));
                                }

                                if (!bErrorAccess)
                                {                                    
                                    dUserId = oTicket.TIPA_USR_ID;
                                    if (oTicket.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                                    {
                                        if (oTicket.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI != null)
                                        {
                                            if (oTicket.TIPA_TOTAL_AMOUNT.Value >= oTicket.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AMOUNT)
                                            {
                                                iAmount = -(oTicket.TIPA_TOTAL_AMOUNT.Value - oTicket.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AMOUNT);
                                                oTicket.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;
                                            }
                                            else
                                            {
                                                iAmount = -(oTicket.TIPA_TOTAL_AMOUNT.Value);
                                            }
                                        }
                                        else
                                        {
                                            iAmount = -oTicket.TIPA_TOTAL_AMOUNT.Value;
                                        }

                                        if (oTicket.OPERATIONS_DISCOUNT != null)
                                        {
                                            iAmount += oTicket.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                            if (oTicket.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE != null)
                                                oTicket.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS += oTicket.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                        }
                                    }
                                    else if (oTicket.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction && oTicket.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI != null)
                                    {
                                        oTicket.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;
                                    }

                                    if (oTicket.CUSTOMER_INVOICE != null)
                                    {
                                        oTicket.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS -= oTicket.TIPA_TOTAL_AMOUNT.Value;
                                    }

                                    oUtcDate = oTicket.TIPA_UTC_DATE;
                                    oDeleted = oTicket;
                                    oInstallation = oTicket.INSTALLATION;
                                    if (oTicket.OPERATIONS_DISCOUNT != null)
                                    {
                                        oDiscountDeleted = oTicket.OPERATIONS_DISCOUNT;
                                        dbContext.OPERATIONS_DISCOUNTs.DeleteOnSubmit(oTicket.OPERATIONS_DISCOUNT);
                                    }
                                    dbContext.TICKET_PAYMENTs.DeleteOnSubmit(oTicket);
                                }
                            }

                        }
                        else if (type == ChargeOperationsType.BalanceRecharge)
                        {

                            var predicateRecharge = PredicateBuilder.True<CUSTOMER_PAYMENT_MEANS_RECHARGE>();
                            predicateRecharge = predicateRecharge.And(t => t.CUSPMR_ID == operationId);
                            var recharges = GetCustomerRecharges(predicateRecharge, dbContext);
                            if (recharges.Count() > 0)
                            {
                                var oRecharge = recharges.First();                                
                                dUserId = oRecharge.CUSPMR_USR_ID.Value;
                                iAmount = oRecharge.CUSPMR_AMOUNT;
                                oUtcDate = oRecharge.CUSPMR_UTC_DATE;
                                oDeleted = oRecharge;                                
                                if (oRecharge.CUSTOMER_INVOICE != null)
                                    oRecharge.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT -= Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                                oRecharge.CUSPMR_TRANS_STATUS = (int)oRechargeStatus;
                            }
                            else 
                            {
                                // START BSU-871
                                var predicateRechargeHis = PredicateBuilder.True<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI>();
                                predicateRechargeHis = predicateRechargeHis.And(t => t.CUSPMR_ID == operationId);
                                var rechargesHis = GetCustomerRechargesHis(predicateRechargeHis, dbContext);
                                if (rechargesHis.Count() > 0)
                                {
                                    var oRechargeHis = rechargesHis.First();
                                    dUserId = oRechargeHis.CUSPMR_USR_ID.Value;
                                    iAmount = oRechargeHis.CUSPMR_AMOUNT;
                                    oUtcDate = oRechargeHis.CUSPMR_UTC_DATE;
                                    oDeleted = oRechargeHis;
                                    if (oRechargeHis.CUSTOMER_INVOICE != null)
                                        oRechargeHis.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT -= Convert.ToInt32(oRechargeHis.CUSPMR_TOTAL_AMOUNT_CHARGED);
                                    oRechargeHis.CUSPMR_TRANS_STATUS = (int)oRechargeStatus;
                                    oRechargeHis.CUSPMR_STATUS_DATE = DateTime.UtcNow;

                                    CUSTOMER_PAYMENT_MEANS_RECHARGE RechargeToInsert = new CUSTOMER_PAYMENT_MEANS_RECHARGE();
                                    RechargeToInsert.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG = oRechargeHis.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG;
                                    RechargeToInsert.CURRENCy = oRechargeHis.CURRENCy;
                                    RechargeToInsert.CUSPMR_AMOUNT = oRechargeHis.CUSPMR_AMOUNT;
                                    RechargeToInsert.CUSPMR_APP_VERSION = oRechargeHis.CUSPMR_APP_VERSION;
                                    RechargeToInsert.CUSPMR_AUTH_CODE = oRechargeHis.CUSPMR_AUTH_CODE;
                                    RechargeToInsert.CUSPMR_AUTH_RESULT = oRechargeHis.CUSPMR_AUTH_RESULT;
                                    RechargeToInsert.CUSPMR_BACKOFFICE_USR = oRechargeHis.CUSPMR_BACKOFFICE_USR;
                                    RechargeToInsert.CUSPMR_BALANCE_BEFORE = oRechargeHis.CUSPMR_BALANCE_BEFORE;
                                    RechargeToInsert.CUSPMR_BALANCE_BONIFICATION_AMOUNT = oRechargeHis.CUSPMR_BALANCE_BONIFICATION_AMOUNT;
                                    RechargeToInsert.CUSPMR_BSREDSYS_3DS_FRICTIONLESS = oRechargeHis.CUSPMR_BSREDSYS_3DS_FRICTIONLESS;
                                    RechargeToInsert.CUSPMR_BSREDSYS_3DS_NUM_INLINE_FORMS = oRechargeHis.CUSPMR_BSREDSYS_3DS_NUM_INLINE_FORMS;
                                    RechargeToInsert.CUSPMR_BSREDSYS_3DS_PROTOCOL_VERSION = oRechargeHis.CUSPMR_BSREDSYS_3DS_PROTOCOL_VERSION;
                                    RechargeToInsert.CUSPMR_BSREDSYS_3DS_TRANS_ID = oRechargeHis.CUSPMR_BSREDSYS_3DS_TRANS_ID;
                                    RechargeToInsert.CUSPMR_CARD_EXPIRATION_DATE = oRechargeHis.CUSPMR_CARD_EXPIRATION_DATE;
                                    RechargeToInsert.CUSPMR_CARD_HASH = oRechargeHis.CUSPMR_CARD_HASH;
                                    RechargeToInsert.CUSPMR_CARD_REFERENCE = oRechargeHis.CUSPMR_CARD_REFERENCE;
                                    RechargeToInsert.CUSPMR_CARD_SCHEME = oRechargeHis.CUSPMR_CARD_SCHEME;
                                    RechargeToInsert.CUSPMR_CASHPAY_BARCODE = oRechargeHis.CUSPMR_CASHPAY_BARCODE;
                                    RechargeToInsert.CUSPMR_CASHPAY_EXPIRATION_DATE = oRechargeHis.CUSPMR_CASHPAY_EXPIRATION_DATE;
                                    RechargeToInsert.CUSPMR_CASHPAY_PAYU_URL = oRechargeHis.CUSPMR_CASHPAY_PAYU_URL;
                                    RechargeToInsert.CUSPMR_CASHPAY_REFERENCE = oRechargeHis.CUSPMR_CASHPAY_REFERENCE;
                                    RechargeToInsert.CUSPMR_CF_TRANSACTION_ID = oRechargeHis.CUSPMR_CF_TRANSACTION_ID;
                                    RechargeToInsert.CUSPMR_CPTGC_ID = oRechargeHis.CUSPMR_CPTGC_ID;
                                    RechargeToInsert.CUSPMR_CREATION_TYPE = oRechargeHis.CUSPMR_CREATION_TYPE;
                                    RechargeToInsert.CUSPMR_CUR_ID = oRechargeHis.CUSPMR_CUR_ID;
                                    RechargeToInsert.CUSPMR_CUS_ID = oRechargeHis.CUSPMR_CUS_ID;
                                    RechargeToInsert.CUSPMR_CUSINV_ID = oRechargeHis.CUSPMR_CUSINV_ID;
                                    RechargeToInsert.CUSPMR_CUSPM_ID = oRechargeHis.CUSPMR_CUSPM_ID;
                                    RechargeToInsert.CUSPMR_DATE = oRechargeHis.CUSPMR_DATE;
                                    RechargeToInsert.CUSPMR_DATE_UTC_OFFSET = oRechargeHis.CUSPMR_DATE_UTC_OFFSET;
                                    RechargeToInsert.CUSPMR_FDO_ID = oRechargeHis.CUSPMR_FDO_ID;
                                    RechargeToInsert.CUSPMR_FIXED_FEE = oRechargeHis.CUSPMR_FIXED_FEE;
                                    RechargeToInsert.CUSPMR_GATEWAY_DATE = oRechargeHis.CUSPMR_GATEWAY_DATE;
                                    RechargeToInsert.CUSPMR_INS_ID = oRechargeHis.CUSPMR_INS_ID;
                                    RechargeToInsert.CUSPMR_INSERTION_UTC_DATE = oRechargeHis.CUSPMR_INSERTION_UTC_DATE;
                                    RechargeToInsert.CUSPMR_LATITUDE = oRechargeHis.CUSPMR_LATITUDE;
                                    RechargeToInsert.CUSPMR_LONGITUDE = oRechargeHis.CUSPMR_LONGITUDE;
                                    RechargeToInsert.CUSPMR_MASKED_CARD_NUMBER = oRechargeHis.CUSPMR_MASKED_CARD_NUMBER;
                                    RechargeToInsert.CUSPMR_MONERIS_CAVV = oRechargeHis.CUSPMR_MONERIS_CAVV;
                                    RechargeToInsert.CUSPMR_MONERIS_ECI = oRechargeHis.CUSPMR_MONERIS_ECI;
                                    RechargeToInsert.CUSPMR_MONERIS_MD = oRechargeHis.CUSPMR_MONERIS_MD;
                                    RechargeToInsert.CUSPMR_MOSE_OS = oRechargeHis.CUSPMR_MOSE_OS;
                                    RechargeToInsert.CUSPMR_OP_REFERENCE = oRechargeHis.CUSPMR_OP_REFERENCE;
                                    RechargeToInsert.CUSPMR_PAGATELIA_NEW_BALANCE = oRechargeHis.CUSPMR_PAGATELIA_NEW_BALANCE;
                                    RechargeToInsert.CUSPMR_PARTIAL_FIXED_FEE = oRechargeHis.CUSPMR_PARTIAL_FIXED_FEE;
                                    RechargeToInsert.CUSPMR_PARTIAL_PERC_FEE = oRechargeHis.CUSPMR_PARTIAL_PERC_FEE;
                                    RechargeToInsert.CUSPMR_PARTIAL_VAT1 = oRechargeHis.CUSPMR_PARTIAL_VAT1;
                                    RechargeToInsert.CUSPMR_PAYPAL_3T_PAYER_ID = oRechargeHis.CUSPMR_PAYPAL_3T_PAYER_ID;
                                    RechargeToInsert.CUSPMR_PAYPAL_3T_TOKEN = oRechargeHis.CUSPMR_PAYPAL_3T_TOKEN;
                                    RechargeToInsert.CUSPMR_PAYPAL_INTENT = oRechargeHis.CUSPMR_PAYPAL_INTENT;
                                    RechargeToInsert.CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY = oRechargeHis.CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY;
                                    RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_FEE_CURRENCY_ISOCODE = oRechargeHis.CUSPMR_PAYPAL_TRANSACTION_FEE_CURRENCY_ISOCODE;
                                    RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_FEE_VALUE = oRechargeHis.CUSPMR_PAYPAL_TRANSACTION_FEE_VALUE;
                                    RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_REFUND_URL = oRechargeHis.CUSPMR_PAYPAL_TRANSACTION_REFUND_URL;
                                    RechargeToInsert.CUSPMR_PAYPAL_TRANSACTION_URL = oRechargeHis.CUSPMR_PAYPAL_TRANSACTION_URL;
                                    RechargeToInsert.CUSPMR_PERC_FEE = oRechargeHis.CUSPMR_PERC_FEE;
                                    RechargeToInsert.CUSPMR_PERC_FEE_TOPPED = oRechargeHis.CUSPMR_PERC_FEE_TOPPED;
                                    RechargeToInsert.CUSPMR_PERC_VAT1 = oRechargeHis.CUSPMR_PERC_VAT1;
                                    RechargeToInsert.CUSPMR_PERC_VAT2 = oRechargeHis.CUSPMR_PERC_VAT2;
                                    RechargeToInsert.CUSPMR_RCOUP_ID = oRechargeHis.CUSPMR_RCOUP_ID;
                                    RechargeToInsert.CUSPMR_REFUND_AMOUNT = oRechargeHis.CUSPMR_REFUND_AMOUNT;
                                    RechargeToInsert.CUSPMR_REFUND_OPE_ID = oRechargeHis.CUSPMR_REFUND_OPE_ID;
                                    RechargeToInsert.CUSPMR_RETRIES_NUM = oRechargeHis.CUSPMR_RETRIES_NUM;
                                    RechargeToInsert.CUSPMR_SECOND_AUTH_RESULT = oRechargeHis.CUSPMR_SECOND_AUTH_RESULT;
                                    RechargeToInsert.CUSPMR_SECOND_CF_TRANSACTION_ID = oRechargeHis.CUSPMR_SECOND_CF_TRANSACTION_ID;
                                    RechargeToInsert.CUSPMR_SECOND_GATEWAY_DATE = oRechargeHis.CUSPMR_SECOND_GATEWAY_DATE;
                                    RechargeToInsert.CUSPMR_SECOND_OP_REFERENCE = oRechargeHis.CUSPMR_SECOND_OP_REFERENCE;
                                    RechargeToInsert.CUSPMR_SECOND_TRANSACTION_ID = oRechargeHis.CUSPMR_SECOND_TRANSACTION_ID;
                                    RechargeToInsert.CUSPMR_SOAPP_ID = oRechargeHis.CUSPMR_SOAPP_ID;
                                    RechargeToInsert.CUSPMR_SRC_AMOUNT = oRechargeHis.CUSPMR_SRC_AMOUNT;
                                    RechargeToInsert.CUSPMR_SRC_CHANGE_APPLIED = oRechargeHis.CUSPMR_SRC_CHANGE_APPLIED;
                                    RechargeToInsert.CUSPMR_SRC_CHANGE_FEE_APPLIED = oRechargeHis.CUSPMR_SRC_CHANGE_FEE_APPLIED;
                                    RechargeToInsert.CUSPMR_SRC_CUR_ID = oRechargeHis.CUSPMR_SRC_CUR_ID;
                                    RechargeToInsert.CUSPMR_STATUS_DATE = oRechargeHis.CUSPMR_STATUS_DATE;
                                    RechargeToInsert.CUSPMR_SUSCRIPTION_TYPE = oRechargeHis.CUSPMR_SUSCRIPTION_TYPE;
                                    RechargeToInsert.CUSPMR_TOTAL_AMOUNT_CHARGED = oRechargeHis.CUSPMR_TOTAL_AMOUNT_CHARGED;
                                    RechargeToInsert.CUSPMR_TRANS_STATUS = oRechargeHis.CUSPMR_TRANS_STATUS;
                                    RechargeToInsert.CUSPMR_TRANSACTION_ID = oRechargeHis.CUSPMR_TRANSACTION_ID;
                                    RechargeToInsert.CUSPMR_TYPE = oRechargeHis.CUSPMR_TYPE;
                                    RechargeToInsert.CUSPMR_USR_ID = oRechargeHis.CUSPMR_USR_ID;
                                    RechargeToInsert.CUSPMR_UTC_DATE = oRechargeHis.CUSPMR_UTC_DATE;
                                    RechargeToInsert.CUSTOMER = oRechargeHis.CUSTOMER;
                                    RechargeToInsert.CUSTOMER_INVOICE = oRechargeHis.CUSTOMER_INVOICE;
                                    RechargeToInsert.CUSTOMER_PAYMENT_MEAN = oRechargeHis.CUSTOMER_PAYMENT_MEAN;
                                    RechargeToInsert.FINAN_DIST_OPERATOR = oRechargeHis.FINAN_DIST_OPERATOR;
                                    RechargeToInsert.INSTALLATION = oRechargeHis.INSTALLATION;
                                    RechargeToInsert.RECHARGE_COUPON = oRechargeHis.RECHARGE_COUPON;
                                    RechargeToInsert.SOURCE_APP = oRechargeHis.SOURCE_APP;
                                    RechargeToInsert.USER = oRechargeHis.USER;

                                    dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGES_HIs.DeleteOnSubmit(oRechargeHis);
                                    dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs.InsertOnSubmit(RechargeToInsert);
                                }
                                // END BSU-871
                            }

                        }
                        else if (type == ChargeOperationsType.ServiceCharge)
                        {

                            var predicateService = PredicateBuilder.True<SERVICE_CHARGE>();
                            predicateService = predicateService.And(t => t.SECH_ID == operationId);
                            var services = GetServiceCharges(predicateService, dbContext);
                            if (services.Count() > 0)
                            {
                                var oService = services.First();                                
                                dUserId = oService.SECH_USR_ID;
                                if (oService.SECH_SUSCRIPTION_TYPE == (int) PaymentSuscryptionType.pstPrepay)
                                {
                                    iAmount = -oService.SECH_FINAL_AMOUNT;
                                    if (oService.CUSTOMER_INVOICE != null)
                                        oService.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS -= oService.SECH_FINAL_AMOUNT;
                                }
                                else if (oService.SECH_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction && oService.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                {
                                    dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs.DeleteOnSubmit(oService.CUSTOMER_PAYMENT_MEANS_RECHARGE);
                                }
                                oUtcDate = oService.SECH_UTC_DATE;
                                oDeleted = oService;
                                dbContext.SERVICE_CHARGEs.DeleteOnSubmit(oService);
                            }

                        }
                        else if (type == ChargeOperationsType.Discount)
                        {
                            var predicateDiscount = PredicateBuilder.True<OPERATIONS_DISCOUNT>();
                            predicateDiscount = predicateDiscount.And(t => t.OPEDIS_ID == operationId);
                            var discounts = GetDiscounts(predicateDiscount, dbContext);
                            if (discounts.Count() > 0)
                            {
                                var oDiscount = discounts.First();                                
                                dUserId = oDiscount.OPEDIS_USR_ID;
                                iAmount = oDiscount.OPEDIS_FINAL_AMOUNT;
                                oUtcDate = oDiscount.OPEDIS_UTC_DATE;
                                if (oDiscount.CUSTOMER_INVOICE != null)
                                    oDiscount.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS += oDiscount.OPEDIS_FINAL_AMOUNT;

                                if (oDiscount.OPERATIONs != null)
                                {
                                    foreach (OPERATION oOperationRelated in oDiscount.OPERATIONs)
                                    {
                                        oOperationRelated.OPE_OPEDIS_ID = null;
                                    }
                                }
                                oDeleted = oDiscount;
                                dbContext.OPERATIONS_DISCOUNTs.DeleteOnSubmit(oDiscount);
                            }
                        }
                        else if (type == ChargeOperationsType.OffstreetEntry ||
                                 type == ChargeOperationsType.OffstreetExit ||
                                 type == ChargeOperationsType.OffstreetOverduePayment)
                        {
                            var predicateOffstreet = PredicateBuilder.True<OPERATIONS_OFFSTREET>();
                            predicateOffstreet = predicateOffstreet.And(t => t.OPEOFF_ID == operationId);
                            var operations = GetOperationsOffstreet(predicateOffstreet, dbContext);
                            if (operations.Count() > 0)
                            {
                                var oOperationOffstreet = operations.First();
                                if (oInstallationsAllowed != null) {
                                    bErrorAccess = !oInstallationsAllowed.Contains(Convert.ToInt32(oOperationOffstreet.INSTALLATION.INS_STANDARD_CITY_ID));
                                }
                                if (!bErrorAccess)
                                {
                                    dUserId = oOperationOffstreet.OPEOFF_USR_ID;
                                    if (oOperationOffstreet.OPEOFF_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                                    {
                                        if (oOperationOffstreet.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI != null)
                                        {
                                            if (oOperationOffstreet.OPEOFF_TOTAL_AMOUNT.Value >= oOperationOffstreet.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AMOUNT)
                                            {
                                                iAmount = -(oOperationOffstreet.OPEOFF_TOTAL_AMOUNT.Value - oOperationOffstreet.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_AMOUNT);
                                                oOperationOffstreet.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;
                                            }
                                            else
                                            {
                                                iAmount = -(oOperationOffstreet.OPEOFF_TOTAL_AMOUNT.Value);
                                            }
                                        }
                                        else
                                        {
                                            iAmount = -oOperationOffstreet.OPEOFF_TOTAL_AMOUNT.Value;
                                        }

                                        if (oOperationOffstreet.OPERATIONS_DISCOUNT != null)
                                        {
                                            iAmount += oOperationOffstreet.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                            if (oOperationOffstreet.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE != null)
                                                oOperationOffstreet.OPERATIONS_DISCOUNT.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS += oOperationOffstreet.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                        }
                                    }
                                    else if (oOperationOffstreet.OPEOFF_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPerTransaction && oOperationOffstreet.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI != null)
                                    {
                                        oOperationOffstreet.CUSTOMER_PAYMENT_MEANS_RECHARGES_HI.CUSPMR_TRANS_STATUS = (int)PaymentMeanRechargeStatus.Waiting_Refund;
                                    }

                                    if (oOperationOffstreet.CUSTOMER_INVOICE != null)
                                    {
                                        oOperationOffstreet.CUSTOMER_INVOICE.CUSINV_INV_AMOUNT_OPS -= oOperationOffstreet.OPEOFF_FINAL_AMOUNT;
                                    }

                                    if (type == ChargeOperationsType.OffstreetEntry)
                                        oUtcDate = oOperationOffstreet.OPEOFF_UTC_NOTIFY_ENTRY_DATE;
                                    else
                                        oUtcDate = oOperationOffstreet.OPEOFF_UTC_PAYMENT_DATE;                                    
                                    oDeleted = oOperationOffstreet;
                                    oInstallation = oOperationOffstreet.INSTALLATION;
                                    if (oOperationOffstreet.OPERATIONS_DISCOUNT != null)
                                    {
                                        oDiscountDeleted = oOperationOffstreet.OPERATIONS_DISCOUNT;
                                        dbContext.OPERATIONS_DISCOUNTs.DeleteOnSubmit(oOperationOffstreet.OPERATIONS_DISCOUNT);
                                    }
                                    dbContext.OPERATIONS_OFFSTREETs.DeleteOnSubmit(oOperationOffstreet);

                                }
                            }
                        }
                        else if (type == ChargeOperationsType.BalanceReception ||
                                 type == ChargeOperationsType.BalanceTransfer ||
                                 type == ChargeOperationsType.ParkingRefund) // We added ParkingRefund here because they can't be deleted now (IPM-2278 - Case 1)
                        {
                            bErrorAccess = true;
                        }

                        if (!bErrorAccess)
                        {
                            if (iAmount != 0)
                            {

                                var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
                                predicate = predicate.And(o => o.OPE_USR_ID == dUserId && o.OPE_UTC_DATE > oUtcDate);
                                var operationsExt = GetOperationsExt(predicate, dbContext).OrderBy(o => o.OPE_UTC_DATE);

                                foreach (ALL_OPERATIONS_EXT oOperationExt in operationsExt)
                                {
                                    if (oOperationExt.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                                    {
                                        if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.ParkingOperation ||
                                            (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.ExtensionOperation ||
                                            (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.Permit) // Parking Refund operations removed from here too (IPM-2278 - Case 1)
                                        {
                                            var predicateOp = PredicateBuilder.True<OPERATION>();
                                            predicateOp = predicateOp.And(o => o.OPE_ID == oOperationExt.OPE_ID);
                                            var operations = GetOperations(predicateOp, dbContext);
                                            if (operations.Count() > 0)
                                            {
                                                var oOperation = operations.First();
                                                oOperation.OPE_BALANCE_BEFORE -= iAmount;
                                            }
                                            else
                                            {
                                                var predicateHisOp = PredicateBuilder.True<HIS_OPERATION>();
                                                predicateHisOp = predicateHisOp.And(o => o.OPE_ID == oOperationExt.OPE_ID);
                                                var hisOperations = GetHisOperations(predicateHisOp, dbContext);
                                                if (hisOperations.Count() > 0)
                                                {
                                                    var oHisOperation = hisOperations.First();
                                                    oHisOperation.OPE_BALANCE_BEFORE -= iAmount;
                                                }
                                            }
                                        }
                                        else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.TicketPayment)
                                        {
                                            var predicateTicket = PredicateBuilder.True<TICKET_PAYMENT>();
                                            predicateTicket = predicateTicket.And(t => t.TIPA_ID == oOperationExt.OPE_ID);
                                            var tickets = GetTicketPayments(predicateTicket, dbContext);
                                            if (tickets.Count() > 0)
                                            {
                                                var oTicket = tickets.First();
                                                oTicket.TIPA_BALANCE_BEFORE -= iAmount;
                                            }
                                        }
                                        else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.BalanceRecharge)
                                        {
                                            var predicateRecharge = PredicateBuilder.True<CUSTOMER_PAYMENT_MEANS_RECHARGE>();
                                            predicateRecharge = predicateRecharge.And(t => t.CUSPMR_ID == oOperationExt.OPE_ID);
                                            var recharges = GetCustomerRecharges(predicateRecharge, dbContext);
                                            if (recharges.Count() > 0)
                                            {
                                                var oRecharge = recharges.First();
                                                oRecharge.CUSPMR_BALANCE_BEFORE -= iAmount;
                                            }
                                            else 
                                            {
                                                // START BSU-871
                                                var predicateRechargeHis = PredicateBuilder.True<CUSTOMER_PAYMENT_MEANS_RECHARGES_HI>();
                                                predicateRechargeHis = predicateRechargeHis.And(t => t.CUSPMR_ID == oOperationExt.OPE_ID);
                                                var rechargesHis = GetCustomerRechargesHis(predicateRechargeHis, dbContext);
                                                if (rechargesHis.Count() > 0)
                                                {
                                                    var oRechargeHis = rechargesHis.First();
                                                    oRechargeHis.CUSPMR_BALANCE_BEFORE -= iAmount;
                                                }
                                                // END BSU-871
                                            }
                                        }
                                        else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.ServiceCharge)
                                        {
                                            var predicateService = PredicateBuilder.True<SERVICE_CHARGE>();
                                            predicateService = predicateService.And(t => t.SECH_ID == oOperationExt.OPE_ID);
                                            var services = GetServiceCharges(predicateService, dbContext);
                                            if (services.Count() > 0)
                                            {
                                                var oService = services.First();
                                                oService.SECH_BALANCE_BEFORE -= iAmount;
                                            }
                                        }
                                        else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.Discount)
                                        {
                                            var predicateDiscount = PredicateBuilder.True<OPERATIONS_DISCOUNT>();
                                            predicateDiscount = predicateDiscount.And(t => t.OPEDIS_ID == oOperationExt.OPE_ID);
                                            var discounts = GetDiscounts(predicateDiscount, dbContext);
                                            if (discounts.Count() > 0)
                                            {
                                                var oDiscount = discounts.First();
                                                oDiscount.OPEDIS_BALANCE_BEFORE -= iAmount;
                                            }
                                        }
                                        else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.OffstreetEntry ||
                                                 (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.OffstreetExit ||
                                                 (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.OffstreetOverduePayment)
                                        {
                                            var predicateOffstreet = PredicateBuilder.True<OPERATIONS_OFFSTREET>();
                                            predicateOffstreet = predicateOffstreet.And(t => t.OPEOFF_ID == oOperationExt.OPE_ID);
                                            var operations = GetOperationsOffstreet(predicateOffstreet, dbContext);
                                            if (operations.Count() > 0)
                                            {
                                                var oOperationOffstreet = operations.First();
                                                oOperationOffstreet.OPEOFF_BALANCE_BEFORE -= iAmount;
                                            }
                                        }
                                        else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.BalanceTransfer)
                                        {
                                            var predicateTransfer = PredicateBuilder.True<BALANCE_TRANSFER>();
                                            predicateTransfer = predicateTransfer.And(t => t.BAT_ID == oOperationExt.OPE_ID);
                                            var transfers = GetBalanceTransfers(predicateTransfer, dbContext);
                                            if (transfers.Count() > 0)
                                            {
                                                var oRecharge = transfers.First();
                                                oRecharge.BAT_SRC_BALANCE_BEFORE -= iAmount;
                                            }
                                        }
                                        else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.BalanceReception)
                                        {
                                            var predicateReception = PredicateBuilder.True<BALANCE_TRANSFER>();
                                            predicateReception = predicateReception.And(t => t.BAT_ID == oOperationExt.OPE_ID);
                                            var transfers = GetBalanceTransfers(predicateReception, dbContext);
                                            if (transfers.Count() > 0)
                                            {
                                                var oRecharge = transfers.First();
                                                oRecharge.BAT_DST_BALANCE_BEFORE -= iAmount;
                                            }
                                        }

                                    }
                                }

                            }

                            // Update user balance
                            var predicateUser = PredicateBuilder.True<USER>();
                            predicateUser = predicateUser.And(t => t.USR_ID == dUserId);
                            oUser = GetUsers(predicateUser, dbContext).First();
                            iBalanceBefore = oUser.USR_BALANCE;
                            if (iAmount != 0)
                                ModifyUserBalance(ref oUser, -iAmount);

                            SecureSubmitChanges(ref dbContext);

                            transaction.Complete();

                            if (oInstallation != null)
                            {
                                if (oDeleted.GetType() == typeof(OPERATION))
                                    ((OPERATION)oDeleted).INSTALLATION = oInstallation;
                                else if (oDeleted.GetType() == typeof(HIS_OPERATION))
                                    ((HIS_OPERATION)oDeleted).INSTALLATION = oInstallation;
                                else if (oDeleted.GetType() == typeof(TICKET_PAYMENT))
                                    ((TICKET_PAYMENT)oDeleted).INSTALLATION = oInstallation;
                                else if (oDeleted.GetType() == typeof(OPERATIONS_OFFSTREET))
                                    ((OPERATIONS_OFFSTREET)oDeleted).INSTALLATION = oInstallation;
                            }
                                
                            bRes = true;
                        }
                        else
                            m_Log.LogMessage(LogLevels.logWARN, string.Format("DeleteOperation: bErrorAccess = true. Operation ID={0}", operationId));

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "DeleteOperation: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteOperation: ", e);
                bRes = false;
            }

            return bRes;
        }

        private bool ModifyUserBalance(ref USER oUser, int iAmount)
        {
            bool bRes = true;
            try
            {
                oUser.USR_BALANCE += iAmount;

                if (iAmount < 0)
                {
                    oUser.USR_LAST_BALANCE_USE = DateTime.UtcNow;

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ModifyUserBalance: ", e);
                bRes = false;
            }

            return bRes;

        }


        private bool SetUserBalance(ref USER oUser, int iUserBalance)
        {
            bool bRes = true;
            try
            {
                if (oUser.USR_BALANCE > iUserBalance)
                {
                    oUser.USR_LAST_BALANCE_USE = DateTime.UtcNow;
                }
                
                oUser.USR_BALANCE = iUserBalance;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ModifyUserBalance: ", e);
                bRes = false;
            }

            return bRes;

        }

        private void SecureSubmitChanges(ref integraMobileDBEntitiesDataContext dbContext)
        {

            try
            {
                dbContext.SubmitChanges(ConflictMode.ContinueOnConflict);
            }

            catch (ChangeConflictException e)
            {
                Console.WriteLine(e.Message);
                // Automerge database values for members that client
                // has not modified.
                foreach (ObjectChangeConflict occ in dbContext.ChangeConflicts)
                {
                    occ.Resolve(RefreshMode.KeepChanges);
                }
            }

            // Submit succeeds on second try.
            dbContext.SubmitChanges(ConflictMode.FailOnFirstConflict);
        }

        public IQueryable<VW_OPERATIONS_HOUR> GetVwOperationsHour(Expression<Func<VW_OPERATIONS_HOUR, bool>> predicate)
        {
            IQueryable<VW_OPERATIONS_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetVwOperationsHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwOperationsHour: ", e);
            }

            return res;
        }
        public IQueryable<VW_OPERATIONS_HOUR> GetVwOperationsHour(Expression<Func<VW_OPERATIONS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<VW_OPERATIONS_HOUR> res = null;
            try
            {
                res = (from r in dbContext.VW_OPERATIONS_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwOperationsHour: ", e);
            }

            return res;
        }

        public IQueryable<VW_RECHARGES_HOUR> GetVwRechargesHour(Expression<Func<VW_RECHARGES_HOUR, bool>> predicate)
        {
            IQueryable<VW_RECHARGES_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    dbContext.CommandTimeout = 3 * 60;
                    res = GetVwRechargesHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwRechargesHour: ", e);
            }

            return res;
        }
        public IQueryable<VW_RECHARGES_HOUR> GetVwRechargesHour(Expression<Func<VW_RECHARGES_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<VW_RECHARGES_HOUR> res = null;
            try
            {
                res = (from r in dbContext.VW_RECHARGES_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwRechargesHour: ", e);
            }

            return res;
        }

        public IQueryable<VW_INSCRIPTIONS_HOUR> GetVwInscriptionsHour(Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> predicate)
        {
            IQueryable<VW_INSCRIPTIONS_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetVwInscriptionsHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwInscriptionsHour: ", e);
            }

            return res;
        }
        public IQueryable<VW_INSCRIPTIONS_HOUR> GetVwInscriptionsHour(Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<VW_INSCRIPTIONS_HOUR> res = null;
            try
            {
                res = (from r in dbContext.VW_INSCRIPTIONS_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwInscriptionsHour: ", e);
            }

            return res;
        }

        public IQueryable<VW_INSCRIPTIONS_PLATFORM_HOUR> GetVwInscriptionsPlatformHour(Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> predicate)
        {
            IQueryable<VW_INSCRIPTIONS_PLATFORM_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetVwInscriptionsPlatformHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwInscriptionsPlatformHour: ", e);
            }

            return res;
        }
        public IQueryable<VW_INSCRIPTIONS_PLATFORM_HOUR> GetVwInscriptionsPlatformHour(Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<VW_INSCRIPTIONS_PLATFORM_HOUR> res = null;
            try
            {
                res = (from r in dbContext.VW_INSCRIPTIONS_PLATFORM_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwInscriptionsPlatformHour: ", e);
            }

            return res;
        }

        public IQueryable<VW_OPERATIONS_USER_HOUR> GetVwOperationsUserHour(Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> predicate)
        {
            IQueryable<VW_OPERATIONS_USER_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetVwOperationsUserHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwOperationsUserHour: ", e);
            }

            return res;
        }
        public IQueryable<VW_OPERATIONS_USER_HOUR> GetVwOperationsUserHour(Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<VW_OPERATIONS_USER_HOUR> res = null;
            try
            {
                res = (from r in dbContext.VW_OPERATIONS_USER_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwOperationsUserHour: ", e);
            }

            return res;
        }

        public IQueryable<VW_OPERATIONS_MINUTE> GetVwOperationsMinute(Expression<Func<VW_OPERATIONS_MINUTE, bool>> predicate)
        {
            IQueryable<VW_OPERATIONS_MINUTE> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    dbContext.CommandTimeout = 3 * 60;
                    res = GetVwOperationsMinute(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwOperationsMinute: ", e);
            }

            return res;
        }
        public IQueryable<VW_OPERATIONS_MINUTE> GetVwOperationsMinute(Expression<Func<VW_OPERATIONS_MINUTE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<VW_OPERATIONS_MINUTE> res = null;
            try
            {
                res = (from r in dbContext.VW_OPERATIONS_MINUTEs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVwOperationsMinute: ", e);
            }

            return res;
        }

        public IQueryable<DB_OPERATIONS_HOUR> GetDbOperationsHour(Expression<Func<DB_OPERATIONS_HOUR, bool>> predicate)
        {
            IQueryable<DB_OPERATIONS_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetDbOperationsHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbOperationsHour: ", e);
            }

            return res;
        }
        public IQueryable<DB_OPERATIONS_HOUR> GetDbOperationsHour(Expression<Func<DB_OPERATIONS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<DB_OPERATIONS_HOUR> res = null;
            try
            {
                res = (from r in dbContext.DB_OPERATIONS_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbOperationsHour: ", e);
            }

            return res;
        }

        public IQueryable<DB_RECHARGES_HOUR> GetDbRechargesHour(Expression<Func<DB_RECHARGES_HOUR, bool>> predicate)
        {
            IQueryable<DB_RECHARGES_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    dbContext.CommandTimeout = 3 * 60;
                    res = GetDbRechargesHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbRechargesHour: ", e);
            }

            return res;
        }
        public IQueryable<DB_RECHARGES_HOUR> GetDbRechargesHour(Expression<Func<DB_RECHARGES_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<DB_RECHARGES_HOUR> res = null;
            try
            {
                res = (from r in dbContext.DB_RECHARGES_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbRechargesHour: ", e);
            }

            return res;
        }

        public IQueryable<DB_OPERATIONS_USERS_HOUR> GetDbOperationsUsersHour(Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> predicate)
        {
            IQueryable<DB_OPERATIONS_USERS_HOUR> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    dbContext.CommandTimeout = 3 * 60;
                    res = GetDbOperationsUsersHour(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbOperationsUsersHour: ", e);
            }

            return res;
        }
        public IQueryable<DB_OPERATIONS_USERS_HOUR> GetDbOperationsUsersHour(Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<DB_OPERATIONS_USERS_HOUR> res = null;
            try
            {
                res = (from r in dbContext.DB_OPERATIONS_USERS_HOURs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbOperationsUsersHour: ", e);
            }

            return res;
        }

        public IQueryable<DB_OPERATIONS_MINUTE> GetDbOperationsMinute(Expression<Func<DB_OPERATIONS_MINUTE, bool>> predicate)
        {
            IQueryable<DB_OPERATIONS_MINUTE> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    dbContext.CommandTimeout = 3 * 60;
                    res = GetDbOperationsMinute(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbOperationsMinute: ", e);
            }

            return res;
        }
        public IQueryable<DB_OPERATIONS_MINUTE> GetDbOperationsMinute(Expression<Func<DB_OPERATIONS_MINUTE, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<DB_OPERATIONS_MINUTE> res = null;
            try
            {
                res = (from r in dbContext.DB_OPERATIONS_MINUTEs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbOperationsMinute: ", e);
            }

            return res;
        }

        public IQueryable<Select_DB_INVITATIONS_HOURResult> GetDbInvitationsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_INVITATIONS_HOURResult, bool>> predicate)
        {
            IQueryable<Select_DB_INVITATIONS_HOURResult> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetDbInvitationsHour(dtBegin, dtEnd, predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbInvitationsHour: ", e);
            }

            return res;
        }
        public IQueryable<Select_DB_INVITATIONS_HOURResult> GetDbInvitationsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_INVITATIONS_HOURResult, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<Select_DB_INVITATIONS_HOURResult> res = null;
            try
            {
                res = dbContext.Select_DB_INVITATIONS_HOUR(dtBegin, dtEnd).AsQueryable().Where(predicate);
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbInvitationsHour: ", e);
            }

            return res;
        }

        public IQueryable<Select_DB_RECHARGE_COUPONS_HOURResult> GetDbRechargeCouponsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_RECHARGE_COUPONS_HOURResult, bool>> predicate)
        {
            IQueryable<Select_DB_RECHARGE_COUPONS_HOURResult> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetDbRechargeCouponsHour(dtBegin, dtEnd, predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbRechargeCouponsHour: ", e);
            }

            return res;
        }
        public IQueryable<Select_DB_RECHARGE_COUPONS_HOURResult> GetDbRechargeCouponsHour(DateTime dtBegin, DateTime dtEnd, Expression<Func<Select_DB_RECHARGE_COUPONS_HOURResult, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<Select_DB_RECHARGE_COUPONS_HOURResult> res = null;
            try
            {
                res = dbContext.Select_DB_RECHARGE_COUPONS_HOUR(dtBegin, dtEnd).AsQueryable().Where(predicate);
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDbRechargeCouponsHour: ", e);
            }

            return res;
        }

        public bool RecalculateUserBalance(decimal dUserId, out USER oUser, decimal? operationId = null, ChargeOperationsType? operationType = null)
        {
            bool bRes = false;
            oUser = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    int iAmount = 0;
                    int iBalance = 0;
                    DateTime? oInsertionUtcDate = null;

                    ALL_OPERATIONS_EXT oFirstOperation = null;

                    if (!operationId.HasValue)
                    {
                        var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
                        predicate = predicate.And(o => o.OPE_USR_ID == dUserId);
                        var operationsExt = GetOperationsExt(predicate, dbContext).OrderBy(o => o.OPE_INSERTION_UTC_DATE);
                        if (operationsExt.Count() > 0)
                            oFirstOperation = operationsExt.First();
                    }
                    else
                    {
                        var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
                        predicate = predicate.And(o => o.OPE_USR_ID == dUserId && o.OPE_ID == operationId && o.OPE_TYPE == (int)operationType);
                        var operationsExt = GetOperationsExt(predicate, dbContext);
                        if (operationsExt.Count() == 1)
                            oFirstOperation = operationsExt.First();
                    }

                    if (oFirstOperation != null)
                    {
                        if (oFirstOperation.OPE_TYPE != (int) ChargeOperationsType.BalanceRecharge)
                            iAmount = oFirstOperation.OPE_FINAL_AMOUNT ?? 0;
                        else
                            iAmount = oFirstOperation.OPE_AMOUNT ?? 0;
                        iBalance = oFirstOperation.OPE_BALANCE_BEFORE;
                        oInsertionUtcDate = oFirstOperation.OPE_INSERTION_UTC_DATE;

                        var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
                        predicate = predicate.And(o => o.OPE_USR_ID == dUserId && o.OPE_INSERTION_UTC_DATE > oInsertionUtcDate);
                        var operationsExt = GetOperationsExt(predicate, dbContext).OrderBy(o => o.OPE_INSERTION_UTC_DATE);

                        foreach (ALL_OPERATIONS_EXT oOperationExt in operationsExt)
                        {
                            if (oOperationExt.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                            {
                                if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.ParkingOperation ||
                                    (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.ExtensionOperation ||
                                    (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.ParkingRefund)
                                {
                                    var predicateOp = PredicateBuilder.True<OPERATION>();
                                    predicateOp = predicateOp.And(o => o.OPE_ID == oOperationExt.OPE_ID);
                                    var operations = GetOperations(predicateOp, dbContext);
                                    if (operations.Count() > 0)
                                    {
                                        var oOperation = operations.First();
                                        if (oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay)
                                        {
                                            oOperation.OPE_BALANCE_BEFORE = iBalance + iAmount;
                                            if ((ChargeOperationsType)oOperationExt.OPE_TYPE != ChargeOperationsType.ParkingRefund)
                                                iAmount = -oOperation.OPE_FINAL_AMOUNT;
                                            else
                                                iAmount = oOperation.OPE_FINAL_AMOUNT;
                                            if (oOperation.OPERATIONS_DISCOUNT != null)
                                                iAmount += oOperation.OPERATIONS_DISCOUNT.OPEDIS_FINAL_AMOUNT;
                                            iBalance = oOperation.OPE_BALANCE_BEFORE;
                                        }
                                    }
                                }
                                else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.TicketPayment)
                                {
                                    var predicateTicket = PredicateBuilder.True<TICKET_PAYMENT>();
                                    predicateTicket = predicateTicket.And(t => t.TIPA_ID == oOperationExt.OPE_ID);
                                    var tickets = GetTicketPayments(predicateTicket, dbContext);
                                    if (tickets.Count() > 0)
                                    {
                                        var oTicket = tickets.First();
                                        oTicket.TIPA_BALANCE_BEFORE = iBalance + iAmount;
                                        iAmount = -oTicket.TIPA_FINAL_AMOUNT;
                                        iBalance = oTicket.TIPA_BALANCE_BEFORE;
                                    }
                                }
                                else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.BalanceRecharge)
                                {
                                    var predicateRecharge = PredicateBuilder.True<CUSTOMER_PAYMENT_MEANS_RECHARGE>();
                                    predicateRecharge = predicateRecharge.And(t => t.CUSPMR_ID == oOperationExt.OPE_ID);
                                    var recharges = GetCustomerRecharges(predicateRecharge, dbContext);
                                    if (recharges.Count() > 0)
                                    {
                                        var oRecharge = recharges.First();
                                        oRecharge.CUSPMR_BALANCE_BEFORE = iBalance + iAmount;
                                        iAmount = oRecharge.CUSPMR_AMOUNT;
                                        iBalance = oRecharge.CUSPMR_BALANCE_BEFORE;
                                    }
                                }
                                else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.ServiceCharge)
                                {
                                    var predicateService = PredicateBuilder.True<SERVICE_CHARGE>();
                                    predicateService = predicateService.And(t => t.SECH_ID == oOperationExt.OPE_ID);
                                    var services = GetServiceCharges(predicateService, dbContext);
                                    if (services.Count() > 0)
                                    {
                                        var oService = services.First();
                                        oService.SECH_BALANCE_BEFORE = iBalance + iAmount;
                                        iAmount = -oService.SECH_FINAL_AMOUNT;
                                        iBalance = oService.SECH_BALANCE_BEFORE;
                                    }
                                }
                                else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.Discount)
                                {
                                    var predicateDiscount = PredicateBuilder.True<OPERATIONS_DISCOUNT>();
                                    predicateDiscount = predicateDiscount.And(t => t.OPEDIS_ID == oOperationExt.OPE_ID);
                                    var discounts = GetDiscounts(predicateDiscount, dbContext);
                                    if (discounts.Count() > 0)
                                    {
                                        var oDiscount = discounts.First();
                                        oDiscount.OPEDIS_BALANCE_BEFORE = iBalance + iAmount;
                                        iAmount = oDiscount.OPEDIS_FINAL_AMOUNT;
                                        iBalance = oDiscount.OPEDIS_BALANCE_BEFORE;
                                    }
                                }
                                else if ((ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.OffstreetEntry ||
                                         (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.OffstreetExit ||
                                         (ChargeOperationsType)oOperationExt.OPE_TYPE == ChargeOperationsType.OffstreetOverduePayment)
                                {
                                    var predicateOffstreet = PredicateBuilder.True<OPERATIONS_OFFSTREET>();
                                    predicateOffstreet = predicateOffstreet.And(t => t.OPEOFF_ID == oOperationExt.OPE_ID);
                                    var operations = GetOperationsOffstreet(predicateOffstreet, dbContext);
                                    if (operations.Count() > 0)
                                    {
                                        var oOperationOffstreet = operations.First();
                                        oOperationOffstreet.OPEOFF_BALANCE_BEFORE = iBalance + iAmount;
                                        iAmount = -oOperationOffstreet.OPEOFF_FINAL_AMOUNT;
                                        iBalance = oOperationOffstreet.OPEOFF_BALANCE_BEFORE;
                                    }
                                }

                            }
                        }

                        // Update user balance
                        var predicateUser = PredicateBuilder.True<USER>();
                        predicateUser = predicateUser.And(t => t.USR_ID == dUserId);
                        oUser = GetUsers(predicateUser, dbContext).First();
                        SetUserBalance(ref oUser, iBalance + iAmount);

                        SecureSubmitChanges(ref dbContext);

                        transaction.Complete();

                        bRes = true;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RecalculateUserBalance: ", e);
                bRes = false;
            }

            return bRes;
        }

        public IQueryable<EMAILTOOL_RECIPIENT> GetEmailToolRecipients(Expression<Func<EMAILTOOL_RECIPIENT, bool>> predicate)
        {
            IQueryable<EMAILTOOL_RECIPIENT> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetEmailToolRecipients(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetEmailToolRecipients: ", e);
            }

            return res;
        }

        public IQueryable<USERS_WARNINGS_RECIPIENT> GetNotificationToolRecipients(Expression<Func<USERS_WARNINGS_RECIPIENT, bool>> predicate)
        {
            IQueryable<USERS_WARNINGS_RECIPIENT> res = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetNotificationToolRecipients(predicate, dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetNotificationToolRecipients: ", e);
            }

            return res;
        }

        public IQueryable<USERS_WARNINGS_FUNCTION> GetUsersWarningsFunctions(Expression<Func<USERS_WARNINGS_FUNCTION, bool>> predicate, out decimal? lang_id)
        {
            IQueryable<USERS_WARNINGS_FUNCTION> res = null;
            lang_id = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                    res = GetUsersWarningsFunctions(predicate, dbContext);
                    lang_id = GetCurrentLangId(dbContext);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUsersWarningsFunctions: ", e);
            }

            return res;
        }

        public IQueryable<EMAILTOOL_RECIPIENT> GetEmailToolRecipients(Expression<Func<EMAILTOOL_RECIPIENT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<EMAILTOOL_RECIPIENT> res = null;
            try
            {
                res = (from r in dbContext.EMAILTOOL_RECIPIENTs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetEmailToolRecipients: ", e);
            }

            return res;
        }

        public IQueryable<USERS_WARNINGS_RECIPIENT> GetNotificationToolRecipients(Expression<Func<USERS_WARNINGS_RECIPIENT, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<USERS_WARNINGS_RECIPIENT> res = null;
            try
            {
                res = (from r in dbContext.USERS_WARNINGS_RECIPIENTs
                       select r)
                        .Where(predicate)
                        .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetNotificationToolRecipients: ", e);
            }

            return res;
        }

        public decimal? GetCurrentLangId(integraMobileDBEntitiesDataContext dbContext)
        { 
            decimal? lang_id = null;
            try
            {
                string cc = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                lang_id = dbContext.LANGUAGEs.Where(l => l.LAN_CULTURE == cc).Select(ll => ll.LAN_ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrentLangId: ", ex);
            }
            return lang_id;
        }

        public IQueryable<USERS_WARNINGS_FUNCTION> GetUsersWarningsFunctions(Expression<Func<USERS_WARNINGS_FUNCTION, bool>> predicate, integraMobileDBEntitiesDataContext dbContext)
        {
            IQueryable<USERS_WARNINGS_FUNCTION> res = null;
            try
            {
                res = dbContext.USERS_WARNINGS_FUNCTIONs
                        .Where(predicate)
                        .AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUsersWarningsFunctions: ", e);
            }

            return res;
        }

        public bool AddEmailToolRecipient(long dId, string sEmail)
        {
            bool bRes = true;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        if (dbContext.EMAILTOOL_RECIPIENTs.Where(e => e.ETR_ID == dId && e.ETR_EMAIL == sEmail).FirstOrDefault() == null)
                        {
                            dbContext.EMAILTOOL_RECIPIENTs.InsertOnSubmit(new EMAILTOOL_RECIPIENT()
                            {
                                ETR_ID = dId,
                                ETR_EMAIL = sEmail
                            });

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "AddEmailToolRecipient: ", e);
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddEmailToolRecipient: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddEmailToolRecipient: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool AddNotificationToolRecipient(long dId, string sEmail)
        {
            bool bRes = true;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        if (dbContext.USERS_WARNINGS_RECIPIENTs.Where(e => e.ETR_ID == dId && e.ETR_EMAIL == sEmail).FirstOrDefault() == null)
                        {
                            dbContext.USERS_WARNINGS_RECIPIENTs.InsertOnSubmit(new USERS_WARNINGS_RECIPIENT()
                            {
                                ETR_ID = dId,
                                ETR_EMAIL = sEmail
                            });

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "AddNotificationToolRecipient: ", e);
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddNotificationToolRecipient: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddNotificationToolRecipient: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool AddEmailToolRecipients(long dId, string[] oEmails)
        {
            bool bRes = true;
            try
            {
                /*using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {*/
                try
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    //List<string> oExistingRecipients = dbContext.EMAILTOOL_RECIPIENTs.Where(e => e.ETR_ID == dId && oEmails.Contains(e.ETR_EMAIL)).Select(e => e.ETR_EMAIL).ToList();
                    //dbContext.EMAILTOOL_RECIPIENTs.InsertAllOnSubmit(oEmails.Where(email => !oExistingRecipients.Contains(email)).Select(email => new EMAILTOOL_RECIPIENT() { ETR_ID = dId, ETR_EMAIL = email }));

                    //using (dbContext.Connection)
                    //{

                    System.Data.Common.DbCommand oCommand = dbContext.Connection.CreateCommand();
                    oCommand.CommandType = System.Data.CommandType.Text;
                    int iCount;

                    if (dbContext.Connection.State != System.Data.ConnectionState.Open) dbContext.Connection.Open();


                    var oRecipients = oEmails.ToList();

                    string sSQL = "INSERT INTO EMAILTOOL_RECIPIENTS (ETR_ID, ETR_EMAIL)" +
                                  "SELECT DISTINCT {0}, USR_EMAIL " +
                                  "FROM USERS " +
                                  "WHERE USR_EMAIL IN ({1})";

                    int iBlockSize = 1500;
                    int iBlocks = oRecipients.Count / iBlockSize;
                    string sEmails = "";
                    string sEmailsIn = "";
                    for (int i = 0; i < iBlocks; i++)
                    {
                        sEmails = "";
                        sEmailsIn = "";
                        for (int j = 0; j < iBlockSize; j++)
                        {
                            sEmails += ";" + oRecipients[(i * iBlockSize) + j];
                            sEmailsIn += ",'" + oRecipients[(i * iBlockSize) + j] + "'";
                        }
                        if (sEmails.Length > 0) sEmails = sEmails.Substring(1);
                        if (sEmailsIn.Length > 0) sEmailsIn = sEmailsIn.Substring(1);
                        //dbContext.EmailTool_AddRecipient(dId, sEmails);
                        //dbContext.ExecuteCommand(sSQL, dId, sEmailsIn);
                        oCommand.CommandText = string.Format(sSQL, dId, sEmailsIn);
                        iCount = oCommand.ExecuteNonQuery();
                    }
                    if (oRecipients.Count > (iBlocks * iBlockSize))
                    {
                        sEmails = "";
                        sEmailsIn = "";
                        for (int j = 0; j < oRecipients.Count - (iBlocks * iBlockSize); j++)
                        {
                            sEmails += ";" + oRecipients[(iBlocks * iBlockSize) + j];
                            sEmailsIn += ",'" + oRecipients[(iBlocks * iBlockSize) + j] + "'";
                        }
                        if (sEmails.Length > 0) sEmails = sEmails.Substring(1);
                        if (sEmailsIn.Length > 0) sEmailsIn = sEmailsIn.Substring(1);
                        //dbContext.EmailTool_AddRecipient(dId, sEmails);
                        //int i = dbContext.ExecuteCommand(sSQL, dId, sEmailsIn);                            
                        oCommand.CommandText = string.Format(sSQL, dId, sEmailsIn);
                        iCount = oCommand.ExecuteNonQuery();

                    }

                    //}

                    //foreach (string sEmail in oEmails) {
                    //    dbContext.EmailTool_AddRecipient(dId, sEmail);
                    //}

                    // Submit the change to the database.
                    /*try
                    {
                        SecureSubmitChanges(ref dbContext);
                        //transaction.Complete();

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddEmailToolRecipients: ", e);
                        bRes = false;
                    }*/
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "AddEmailToolRecipients: ", e);
                    bRes = false;
                }
                //}
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddEmailToolRecipients: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool AddNotificationToolRecipients(long dId, string[] oEmails)
        {
            bool bRes = true;
            try
            {
                try
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    System.Data.Common.DbCommand oCommand = dbContext.Connection.CreateCommand();
                    oCommand.CommandType = System.Data.CommandType.Text;
                    int iCount;

                    if (dbContext.Connection.State != System.Data.ConnectionState.Open) dbContext.Connection.Open();


                    var oRecipients = oEmails.ToList();

                    string sSQL = "INSERT INTO USERS_WARNINGS_RECIPIENTS (ETR_ID, ETR_EMAIL)" +
                                  "SELECT DISTINCT {0}, USR_EMAIL " +
                                  "FROM USERS " +
                                  "WHERE USR_EMAIL IN ({1})";

                    int iBlockSize = 1500;
                    int iBlocks = oRecipients.Count / iBlockSize;
                    string sEmails = "";
                    string sEmailsIn = "";
                    for (int i = 0; i < iBlocks; i++)
                    {
                        sEmails = "";
                        sEmailsIn = "";
                        for (int j = 0; j < iBlockSize; j++)
                        {
                            sEmails += ";" + oRecipients[(i * iBlockSize) + j];
                            sEmailsIn += ",'" + oRecipients[(i * iBlockSize) + j] + "'";
                        }
                        if (sEmails.Length > 0) sEmails = sEmails.Substring(1);
                        if (sEmailsIn.Length > 0) sEmailsIn = sEmailsIn.Substring(1);
                        oCommand.CommandText = string.Format(sSQL, dId, sEmailsIn);
                        iCount = oCommand.ExecuteNonQuery();
                    }
                    if (oRecipients.Count > (iBlocks * iBlockSize))
                    {
                        sEmails = "";
                        sEmailsIn = "";
                        for (int j = 0; j < oRecipients.Count - (iBlocks * iBlockSize); j++)
                        {
                            sEmails += ";" + oRecipients[(iBlocks * iBlockSize) + j];
                            sEmailsIn += ",'" + oRecipients[(iBlocks * iBlockSize) + j] + "'";
                        }
                        if (sEmails.Length > 0) sEmails = sEmails.Substring(1);
                        if (sEmailsIn.Length > 0) sEmailsIn = sEmailsIn.Substring(1);
                        oCommand.CommandText = string.Format(sSQL, dId, sEmailsIn);
                        iCount = oCommand.ExecuteNonQuery();

                    }
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "AddNotificationToolRecipients: ", e);
                    bRes = false;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddNotificationToolRecipients: ", e);
                bRes = false;
            }
            return bRes;
        }

        public bool DeleteEmailToolRecipient(long dId, string sEmail)
        {
            bool bRes = true;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        var oRecipient = (from r in dbContext.EMAILTOOL_RECIPIENTs
                                          where r.ETR_ID == dId && r.ETR_EMAIL == sEmail
                                          select r).First();

                        if (oRecipient != null)
                        {

                            dbContext.EMAILTOOL_RECIPIENTs.DeleteOnSubmit(oRecipient);
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "DeleteEmailToolRecipient: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "DeleteEmailToolRecipient: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteEmailToolRecipient: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool DeleteNotificationToolRecipient(long dId, string sEmail)
        {
            bool bRes = true;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        var oRecipient = (from r in dbContext.USERS_WARNINGS_RECIPIENTs
                                          where r.ETR_ID == dId && r.ETR_EMAIL == sEmail
                                          select r).First();

                        if (oRecipient != null)
                        {

                            dbContext.USERS_WARNINGS_RECIPIENTs.DeleteOnSubmit(oRecipient);
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "DeleteNotificationToolRecipient: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "DeleteNotificationToolRecipient: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteNotificationToolRecipient: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool DeleteAllEmailToolRecipients(long dId)
        {
            bool bRes = true;

            try
            {
                /*using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {*/
                try
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    /* dbContext.EMAILTOOL_RECIPIENTs.DeleteAllOnSubmit<EMAILTOOL_RECIPIENT>(dbContext.EMAILTOOL_RECIPIENTs.Where(r => r.ETR_ID == dId));

                     // Submit the change to the database.
                     try
                     {
                         SecureSubmitChanges(ref dbContext);
                         transaction.Complete();
                     }
                     catch (Exception e)
                     {
                         m_Log.LogMessage(LogLevels.logERROR, "DeleteAllEmailToolRecipients: ", e);
                         bRes = false;
                     }*/

                    System.Data.Common.DbCommand oCommand = dbContext.Connection.CreateCommand();
                    oCommand.CommandType = System.Data.CommandType.Text;
                    int iCount;

                    if (dbContext.Connection.State != System.Data.ConnectionState.Open) dbContext.Connection.Open();

                    string sSQL = "DELETE " +
                                  "FROM EMAILTOOL_RECIPIENTS " +
                                  "WHERE ETR_ID = {0}";

                    oCommand.CommandText = string.Format(sSQL, dId);
                    iCount = oCommand.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "DeleteAllEmailToolRecipients: ", e);
                    bRes = false;
                }
                //}
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteAllEmailToolRecipients: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool DeleteAllNotificationToolRecipients(long dId)
        {
            bool bRes = true;

            try
            {
                try
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    System.Data.Common.DbCommand oCommand = dbContext.Connection.CreateCommand();
                    oCommand.CommandType = System.Data.CommandType.Text;
                    int iCount;

                    if (dbContext.Connection.State != System.Data.ConnectionState.Open) dbContext.Connection.Open();

                    string sSQL = "DELETE " +
                                  "FROM USERS_WARNINGS_RECIPIENTS " +
                                  "WHERE ETR_ID = {0}";

                    oCommand.CommandText = string.Format(sSQL, dId);
                    iCount = oCommand.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "DeleteAllNotificationToolRecipients: ", e);
                    bRes = false;
                }
                //}
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteAllNotificationToolRecipients: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool DeletePreviousEmailToolRecipients(long dId)
        {
            bool bRes = true;

            try
            {
                /*using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {*/
                try
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    System.Data.Common.DbCommand oCommand = dbContext.Connection.CreateCommand();
                    oCommand.CommandType = System.Data.CommandType.Text;
                    int iCount;

                    if (dbContext.Connection.State != System.Data.ConnectionState.Open) dbContext.Connection.Open();

                    string sSQL = "DELETE " +
                                  "FROM EMAILTOOL_RECIPIENTS " +
                                  "WHERE ETR_ID <= {0}";

                    oCommand.CommandText = string.Format(sSQL, dId);
                    iCount = oCommand.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "DeleteAllEmailToolRecipients: ", e);
                    bRes = false;
                }
                //}
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteAllEmailToolRecipients: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool DeletePreviousNotificationToolRecipients(long dId)
        {
            bool bRes = true;

            try
            {
                try
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    System.Data.Common.DbCommand oCommand = dbContext.Connection.CreateCommand();
                    oCommand.CommandType = System.Data.CommandType.Text;
                    int iCount;

                    if (dbContext.Connection.State != System.Data.ConnectionState.Open) dbContext.Connection.Open();

                    string sSQL = "DELETE " +
                                  "FROM USERS_WARNINGS_RECIPIENTS " +
                                  "WHERE ETR_ID <= {0}";

                    oCommand.CommandText = string.Format(sSQL, dId);
                    iCount = oCommand.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "DeletePreviousNotificationToolRecipients: ", e);
                    bRes = false;
                }
                //}
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeletePreviousNotificationToolRecipients: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool DeleteEmailToolRecipients(Expression<Func<EMAILTOOL_RECIPIENT, bool>> predicate)
        {
            bool bRes = true;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        dbContext.EMAILTOOL_RECIPIENTs.DeleteAllOnSubmit<EMAILTOOL_RECIPIENT>(dbContext.EMAILTOOL_RECIPIENTs.Where(predicate));

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "DeleteEmailToolRecipients: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "DeleteEmailToolRecipients: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteEmailToolRecipients: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool DeleteNotificationToolRecipients(Expression<Func<USERS_WARNINGS_RECIPIENT, bool>> predicate)
        {
            bool bRes = true;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        dbContext.USERS_WARNINGS_RECIPIENTs.DeleteAllOnSubmit<USERS_WARNINGS_RECIPIENT>(dbContext.USERS_WARNINGS_RECIPIENTs.Where(predicate));

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "DeleteNotificationToolRecipients: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "DeleteNotificationToolRecipients: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteNotificationToolRecipients: ", e);
                bRes = false;
            }
            return bRes;

        }

        public bool SetUserShopkeeperStatus(decimal dUserId, ShopKeeperStatus eStatus, out USER oUser)
        {
            bool bRes = true;
            oUser = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        oUser = (from r in dbContext.USERs
                                 where r.USR_ID == dUserId
                                 select r).First();

                        bRes = false;
                        switch (oUser.USR_SHOPKEEPER_STATUS ?? 0)
                        {
                            case (int)ShopKeeperStatus.RegularUser:
                                bRes = false;
                                break;

                            case (int)ShopKeeperStatus.PendingRequest:
                                bRes = (eStatus == ShopKeeperStatus.ShopKeeperUser || eStatus == ShopKeeperStatus.RegularUser);
                                break;

                            case (int)ShopKeeperStatus.ShopKeeperUser:
                                bRes = (eStatus == ShopKeeperStatus.RegularUser);
                                break;
                        }

                        if (bRes)
                        {
                            oUser.USR_SHOPKEEPER_STATUS = (int)eStatus;

                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();

                            bRes = true;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SetUserShopkeeperStatus: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetUserShopkeeperStatus: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool SetUsername(decimal dUserId, string sNewUsername)
        {
            bool bRes = true;
            USER oUser = null;

            sNewUsername = sNewUsername.ToLower();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        // Get User by UserId

                        oUser = (from r in dbContext.USERs
                                 where r.USR_ID == dUserId
                                 select r).First();

                        bRes = false;

                        if (oUser != null)
                        {
                            string CurrentUsername = oUser.USR_USERNAME.ToLower();

                            // Set new Username

                            oUser.USR_USERNAME = sNewUsername;
                            oUser.USR_EMAIL = sNewUsername;
                            SecureSubmitChanges(ref dbContext);

                            // Get aspnet_Memberships with destination username, and delete

                            IEnumerable<aspnet_Membership> ExistingMemberships = (from r in dbContext.aspnet_Memberships
                                                                                  where r.Email == sNewUsername
                                                                                  select r);

                            dbContext.aspnet_Memberships.DeleteAllOnSubmit(ExistingMemberships);
                            SecureSubmitChanges(ref dbContext);

                            // Get aspnet_Users with destination username, and delete

                            IEnumerable<aspnet_User> ExistingUsers = (from r in dbContext.aspnet_Users
                                                                      where r.UserName == sNewUsername
                                                                      select r);

                            dbContext.aspnet_Users.DeleteAllOnSubmit(ExistingUsers);
                            SecureSubmitChanges(ref dbContext);

                            // Get aspnet_User with source username

                            aspnet_User oAspnetUser = (from r in dbContext.aspnet_Users
                                                       where r.UserName == CurrentUsername
                                                       select r).First();
                            if (oAspnetUser != null)
                            {
                                // Set new username

                                oAspnetUser.UserName = sNewUsername;
                                oAspnetUser.LoweredUserName = sNewUsername;
                                SecureSubmitChanges(ref dbContext);
                               
                                // Get aspnet_Membership by UserId

                                aspnet_Membership oAspnetMembership = (from r in dbContext.aspnet_Memberships
                                                                       where r.UserId == oAspnetUser.UserId
                                                                       select r).First();
                                if (oAspnetMembership != null)
                                {
                                    // Set new username

                                    oAspnetMembership.Email = sNewUsername;
                                    oAspnetMembership.LoweredEmail = sNewUsername;
                                    SecureSubmitChanges(ref dbContext);
                                }
                            }
                            transaction.Complete();
                            bRes = true;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SetUsername: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetUsername: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool SetBalance(decimal dUserId, decimal iBalance)
        {
            bool bRes = true;
            USER oUser = null;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        oUser = (from r in dbContext.USERs
                                 where r.USR_ID == dUserId
                                 select r).First();

                        bRes = false;

                        if (oUser != null)
                        {
                            decimal CurFact = 0.01M;
                            if (oUser.CURRENCy.CUR_FACT != null)
                            {
                                CurFact = Convert.ToDecimal(oUser.CURRENCy.CUR_FACT);
                            }
                            oUser.USR_BALANCE = Convert.ToInt32(iBalance/CurFact);
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            bRes = true;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SetBalance: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetBalance: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool RechargeCouponsGenerationGet(decimal dGenerationId, out RECHARGE_COUPONS_GENERATION oRechargeCouponGeneration)
        {
            bool bRes = false;
            oRechargeCouponGeneration = null;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        oRechargeCouponGeneration = dbContext.RECHARGE_COUPONS_GENERATIONs.Where(t => t.RCOPG_ID == dGenerationId).FirstOrDefault();

                        bRes = (oRechargeCouponGeneration != null);
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationGet: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationGet: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool RechargeCouponsGenerationAdd(int iNumber, RechargeCouponsType eCouponType, int iAmountValue, decimal dCurrencyId, string sBackOfficeUser, decimal dFinanDistOperatorId, DateTime dtIniApply, DateTime dtEndApply, out RECHARGE_COUPONS_GENERATION oRechargeCouponGeneration)
        {
            bool bRes = false;
            oRechargeCouponGeneration = null;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        var oCurrency = dbContext.CURRENCies.Where(t => t.CUR_ID == dCurrencyId).FirstOrDefault();
                        if (oCurrency != null)
                        {
                            oRechargeCouponGeneration = new RECHARGE_COUPONS_GENERATION()
                            {
                                RCOPG_TYPE = (int)eCouponType,
                                RCOPG_VALUE = iAmountValue,
                                RCOPG_CUR_ID = oCurrency.CUR_ID,
                                RCOPG_START_DATE = dtIniApply,
                                RCOPG_EXP_DATE = dtEndApply,
                                RCOPG_BACKOFFICE_USR = sBackOfficeUser,
                                RCOPG_FDO_ID = dFinanDistOperatorId
                            };
                            dbContext.RECHARGE_COUPONS_GENERATIONs.InsertOnSubmit(oRechargeCouponGeneration);

                            RECHARGE_COUPON oRechargeCoupon;
                            for (int i = 0; i < iNumber; i++)
                            {
                                string sKeyCode = "";

                                oRechargeCoupon = new RECHARGE_COUPON()
                                {
                                    RCOUP_CODE = CouponCodeGenerator.GenerateCode(ref sKeyCode),
                                    RCOUP_COUPS_ID = (int)RechargeCouponsStatus.Actived,
                                    RCOUP_VALUE = iAmountValue,
                                    RCOUP_CUR_ID = oCurrency.CUR_ID,
                                    RCOUP_START_DATE = dtIniApply,
                                    RCOUP_EXP_DATE = dtEndApply,
                                    RETAILER_PAYMENT = null,
                                    RCOUP_KEYCODE = sKeyCode,
                                    RCOUP_TYPE = (int)eCouponType,
                                    RECHARGE_COUPONS_GENERATION = oRechargeCouponGeneration,
                                    RCOUP_EMAIL = null
                                };
                                dbContext.RECHARGE_COUPONs.InsertOnSubmit(oRechargeCoupon);
                            }
                            
                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();
                                bRes = true;
                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationAdd: ", e);
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationAdd: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationAdd: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool RechargeValueTypesModifyAmount(int CuspmId, int? CuspmAmountToRecharge, int? CuspmRechargeWhenAmountIsLess, bool CuspmEnabled, bool CuspmAutomaticRecharge)
        {
            bool bRes = false;
            
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        var oRechargeValue = dbContext.CUSTOMER_PAYMENT_MEANs.Where(t => t.CUSPM_ID == CuspmId).FirstOrDefault();
                        if (oRechargeValue != null)
                        {
                            oRechargeValue.CUSPM_AMOUNT_TO_RECHARGE = CuspmAmountToRecharge;
                            oRechargeValue.CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS = CuspmRechargeWhenAmountIsLess;
                            oRechargeValue.CUSPM_ENABLED = Convert.ToInt32(CuspmEnabled);
                            oRechargeValue.CUSPM_AUTOMATIC_RECHARGE = Convert.ToInt32(CuspmAutomaticRecharge);

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();
                                bRes = true;
                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "RechargeValueTypesModifyAmount: ", e);
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "RechargeValueTypesModifyAmount: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeValueTypesModifyAmount: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool RechargeCouponsGenerationModifyApplyPeriod(decimal dGenerationId, DateTime dtIniApply, DateTime dtEndApply, out int iNotActivedCount)
        {
            bool bRes = false;
            iNotActivedCount = 0;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        var oRechargeCouponGeneration = dbContext.RECHARGE_COUPONS_GENERATIONs.Where(t => t.RCOPG_ID == dGenerationId).FirstOrDefault();
                        if (oRechargeCouponGeneration != null)
                        {
                            oRechargeCouponGeneration.RCOPG_START_DATE = dtIniApply;
                            oRechargeCouponGeneration.RCOPG_EXP_DATE = dtEndApply;

                            iNotActivedCount = oRechargeCouponGeneration.RECHARGE_COUPONs.Where(t => t.RCOUP_COUPS_ID != (int)RechargeCouponsStatus.Actived).Count();

                            foreach (RECHARGE_COUPON oCoupon in oRechargeCouponGeneration.RECHARGE_COUPONs.Where(t => t.RCOUP_COUPS_ID == (int)RechargeCouponsStatus.Actived || t.RCOUP_COUPS_ID == (int)RechargeCouponsStatus.Cancelled))
                            {
                                oCoupon.RCOUP_START_DATE = dtIniApply;
                                oCoupon.RCOUP_EXP_DATE = dtEndApply;
                            }

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();
                                bRes = true;
                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationModifyApplyPeriod: ", e);
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationModifyApplyPeriod: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationModifyApplyPeriod: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool RechargeCouponsGenerationModifyStatus(decimal dGenerationId, RechargeCouponsStatus eStatus, out int iChangedCount)
        {
            bool bRes = false;
            iChangedCount = 0;

            try
            {
                RechargeCouponsStatus? eSearchStatus = null;
                if (eStatus == RechargeCouponsStatus.Actived)
                    eSearchStatus = RechargeCouponsStatus.Cancelled;
                else if (eStatus == RechargeCouponsStatus.Cancelled)
                    eSearchStatus = RechargeCouponsStatus.Actived;

                if (eSearchStatus.HasValue)
                {

                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                               new TransactionOptions()
                                                                               {
                                                                                   IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                   Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                               }))
                    {
                        try
                        {
                            integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                            var oRechargeCouponGeneration = dbContext.RECHARGE_COUPONS_GENERATIONs.Where(t => t.RCOPG_ID == dGenerationId).FirstOrDefault();
                            if (oRechargeCouponGeneration != null)
                            {

                                iChangedCount = oRechargeCouponGeneration.RECHARGE_COUPONs.Where(t => t.RCOUP_COUPS_ID == (int)eSearchStatus.Value).Count();

                                foreach (RECHARGE_COUPON oCoupon in oRechargeCouponGeneration.RECHARGE_COUPONs.Where(t => t.RCOUP_COUPS_ID == (int)eSearchStatus.Value))
                                {
                                    oCoupon.RCOUP_COUPS_ID = (int)eStatus;
                                }

                                // Submit the change to the database.
                                try
                                {
                                    SecureSubmitChanges(ref dbContext);
                                    transaction.Complete();
                                    bRes = true;
                                }
                                catch (Exception e)
                                {
                                    m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationModifyStatus: ", e);
                                    bRes = false;
                                    iChangedCount = 0;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationModifyStatus: ", e);
                            bRes = false;
                            iChangedCount = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponsGenerationModifyStatus: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool RechargeCouponGet(decimal dCouponId, out RECHARGE_COUPON oRechargeCoupon)
        {
            bool bRes = false;
            oRechargeCoupon = null;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        oRechargeCoupon = dbContext.RECHARGE_COUPONs.Where(t => t.RCOUP_ID == dCouponId).FirstOrDefault();

                        bRes = (oRechargeCoupon != null);
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponGet: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponGet: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool RechargeCouponModifyApplyPeriod(decimal dCouponId, DateTime dtIniApply, DateTime dtEndApply)
        {
            bool bRes = false;            

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        var oRechargeCoupon = dbContext.RECHARGE_COUPONs.Where(t => t.RCOUP_ID == dCouponId).FirstOrDefault();
                        if (oRechargeCoupon != null)
                        {
                            oRechargeCoupon.RCOUP_START_DATE = dtIniApply;
                            oRechargeCoupon.RCOUP_EXP_DATE = dtEndApply;
                            
                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();
                                bRes = true;
                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponModifyApplyPeriod: ", e);
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponModifyApplyPeriod: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponModifyApplyPeriod: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool RechargeCouponModifyStatus(decimal dCouponId, RechargeCouponsStatus eStatus)
        {
            bool bRes = false;

            try
            {
                RechargeCouponsStatus? eSearchStatus = null;
                if (eStatus == RechargeCouponsStatus.Actived)
                    eSearchStatus = RechargeCouponsStatus.Cancelled;
                else if (eStatus == RechargeCouponsStatus.Cancelled)
                    eSearchStatus = RechargeCouponsStatus.Actived;

                if (eSearchStatus.HasValue)
                {

                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                               new TransactionOptions()
                                                                               {
                                                                                   IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                   Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                               }))
                    {
                        try
                        {
                            integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                            var oRechargeCoupon = dbContext.RECHARGE_COUPONs.Where(t => t.RCOUP_ID == dCouponId && t.RCOUP_COUPS_ID == (int)eSearchStatus.Value).FirstOrDefault();
                            if (oRechargeCoupon != null)
                            {
                                oRechargeCoupon.RCOUP_COUPS_ID = (int)eStatus;

                                // Submit the change to the database.
                                try
                                {
                                    SecureSubmitChanges(ref dbContext);
                                    transaction.Complete();
                                    bRes = true;
                                }
                                catch (Exception e)
                                {
                                    m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponModifyStatus: ", e);
                                    bRes = false;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponModifyStatus: ", e);
                            bRes = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RechargeCouponModifyStatus: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool UpdateOperationPlates(ref OPERATION operation, string sPlate, List<string> oAdditionalPlates)
        {
            bool bRes = false;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        decimal opeId = operation.OPE_ID;
                        var oOperation = (from r in dbContext.OPERATIONs
                                          where r.OPE_ID == opeId 
                                          select r).First();

                        if (oOperation != null)
                        {
                            decimal? dPlateID = null;
                            USER_PLATE oPlateEntity = null;
                            try
                            {
                                var oPlate = oOperation.USER.USER_PLATEs.Where(r => r.USRP_PLATE == sPlate.ToUpper().Trim().Replace(" ", "") && r.USRP_ENABLED == 1).FirstOrDefault();
                                if (oPlate != null)
                                {
                                    dPlateID = oPlate.USRP_ID;
                                    oPlateEntity = oPlate;
                                }
                                else
                                {
                                    USER_PLATE oNewPlate = new USER_PLATE
                                    {
                                        USRP_ENABLED = 1,
                                        USRP_IS_DEFAULT = 0,
                                        USRP_USR_ID = oOperation.USER.USR_ID,
                                        USRP_PLATE = sPlate.ToUpper().Trim().Replace(" ", "")
                                    };

                                    dbContext.USER_PLATEs.InsertOnSubmit(oNewPlate);

                                    SecureSubmitChanges(ref dbContext);

                                    dPlateID = oNewPlate.USRP_ID;
                                    oPlateEntity = oNewPlate;
                                }

                            }
                            catch
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "UpdateOperationPlates: Plate is not from user or is not enabled: " + sPlate);
                                bRes = false;
                                return bRes;
                            }

                            List<decimal?> oPlatesIDs = new List<decimal?>();
                            List<USER_PLATE> oPlatesEntities = new List<USER_PLATE>();
                            for (int i = 0; i < 9; i += 1)
                            {
                                oPlatesIDs.Add(null);
                                oPlatesEntities.Add(null);
                            }

                            if (oAdditionalPlates != null)
                            {
                                string sAuxPlate = "";
                                for (int i = 0; i < 9; i += 1)
                                {
                                    if (oAdditionalPlates.Count > i)
                                        sAuxPlate = oAdditionalPlates[i];
                                    else
                                        sAuxPlate = "";
                                    if (sAuxPlate != "")
                                    {
                                        try
                                        {
                                            var oPlate = oOperation.USER.USER_PLATEs.Where(r => r.USRP_PLATE == sAuxPlate.ToUpper().Trim().Replace(" ", "") && r.USRP_ENABLED == 1).FirstOrDefault();
                                            if (oPlate != null)
                                            {
                                                oPlatesIDs[i] = oPlate.USRP_ID;
                                                oPlatesEntities[i] = oPlate;
                                            }
                                            else
                                            {
                                                USER_PLATE oNewPlate = new USER_PLATE
                                                {
                                                    USRP_ENABLED = 1,
                                                    USRP_IS_DEFAULT = 0,
                                                    USRP_USR_ID = oOperation.USER.USR_ID,
                                                    USRP_PLATE = sAuxPlate.ToUpper().Trim().Replace(" ", "")
                                                };
                                                
                                                dbContext.USER_PLATEs.InsertOnSubmit(oNewPlate);

                                                SecureSubmitChanges(ref dbContext);

                                                oPlatesIDs[i] = oNewPlate.USRP_ID;
                                                oPlatesEntities[i] = oNewPlate;
                                            }

                                        }
                                        catch
                                        {
                                            m_Log.LogMessage(LogLevels.logERROR, "UpdateOperationPlates: Plate is not from user or is not enabled: " + sAuxPlate);
                                            bRes = false;
                                            return bRes;

                                        }
                                    }
                                }

                            }
                            
                            oOperation.USER_PLATE = oPlateEntity;
                            oOperation.USER_PLATE1 = oPlatesEntities[0];
                            oOperation.USER_PLATE2 = oPlatesEntities[1];
                            oOperation.USER_PLATE3 = oPlatesEntities[2];
                            oOperation.USER_PLATE4 = oPlatesEntities[3];
                            oOperation.USER_PLATE5 = oPlatesEntities[4];
                            oOperation.USER_PLATE6 = oPlatesEntities[5];
                            oOperation.USER_PLATE7 = oPlatesEntities[6];
                            oOperation.USER_PLATE8 = oPlatesEntities[7];
                            oOperation.USER_PLATE9 = oPlatesEntities[8];

                            bRes = true;
                        }
                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            operation = oOperation;
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "ModifyUserEmailOrTelephone: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "ModifyUserEmailOrTelephone: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ModifyUserEmailOrTelephone: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool SubstractUserBalance(decimal dUserId, int iBalance)
        {
            bool bRes = true;            
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                           new TransactionOptions()
                                                                           {
                                                                               IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                               Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                           }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        var oUser = (from r in dbContext.USERs
                                 where r.USR_ID == dUserId
                                 select r).First();

                        oUser.USR_BALANCE -= iBalance;

                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();
                        
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SubstractUserBalance: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SubstractUserBalance: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool UpdateBankData(long id, decimal bg, decimal bf, out string ErrorMessage)
        {
            bool bRes = false;
            ErrorMessage = string.Empty;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                            new TransactionOptions()
                                                                            {
                                                                                IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                            }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        GATEWAY_BENEFIT oGatewayBenefit = dbContext.GATEWAY_BENEFITs.Where(t => t.GTWB_ID == id).FirstOrDefault();
                        if (oGatewayBenefit != null)
                        {
                            oGatewayBenefit.GTWB_BANK_GROSS = bg;
                            oGatewayBenefit.GTWB_BANK_FEE = bf;

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();
                                bRes = true;
                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "UpdateBankData: ", e);
                                ErrorMessage = e.Message;
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateBankData: ", e);
                        ErrorMessage = e.Message;
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateBankData: ", e);
                ErrorMessage = e.Message;
                bRes = false;
            }

            return bRes;
        }

        public bool UpdateGatewayConfig(long id, string Batch, string Timezone, decimal FixedFee, decimal PercFee, decimal MinFee, out string ErrorMessage)
        {
            bool bRes = false;
            ErrorMessage = string.Empty;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                            new TransactionOptions()
                                                                            {
                                                                                IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                            }))
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG oCPTGC = dbContext.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.Where(t => t.CPTGC_ID == id).FirstOrDefault();
                        if (oCPTGC != null)
                        {
                            oCPTGC.CPTGC_LOCAL_BATCH_TIME = Batch.Length > 5 ? Batch.Substring(0,5) : Batch;
                            oCPTGC.CPTGC_TIMEZONE = Timezone;
                            oCPTGC.CPTGC_FIXED_FEE = FixedFee;
                            oCPTGC.CPTGC_PERC_FEE = PercFee;
                            oCPTGC.CPTGC_MIN_FEE = MinFee;

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();
                                bRes = true;
                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "UpdateGatewayConfig: ", e);
                                ErrorMessage = e.Message;
                                bRes = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateGatewayConfig: ", e);
                        ErrorMessage = e.Message;
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateGatewayConfig: ", e);
                ErrorMessage = e.Message;
                bRes = false;
            }

            return bRes;
        }


    }
}
