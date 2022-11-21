using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using integraMobile.Domain.Abstract;
using integraMobile.Domain;
using System.Data.Linq;
using System.Configuration;
using System.Transactions;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Infrastructure;
using integraMobile.Domain.Helper;

namespace kendoTest.Infrastructure
{
    public class SQLDataRepository
    {

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SQLDataRepository));


        public SQLDataRepository(string connectionString)
        {
        }

        public IQueryable<USER> GetUsers(ref USER user)
        {
            IQueryable<USER> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                decimal userId = user.USR_ID;
                res = (from r in dbContext.USERs                    
                       select r)
                       .AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUsers: ", e);
            }

            return res;
        }

        public IQueryable<ALL_OPERATION> GetUserOperations(ref USER user,
                                                           Expression<Func<ALL_OPERATION, bool>> predicate)
        {
            IQueryable<ALL_OPERATION> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                decimal userId = user.USR_ID;
                res = (from r in dbContext.ALL_OPERATIONs
                       where r.OPE_USR_ID == userId
                       select r)
                       .Where(predicate)
                       .AsQueryable();

                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUserOperations: ", e);
            }

            return res;
        }

        public IQueryable<ALL_OPERATIONS_EXT> GetUserOperationsExt(ref USER user,
                                                           Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate)
        {
            IQueryable<ALL_OPERATIONS_EXT> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                decimal userId = user.USR_ID;
                res = (from r in dbContext.ALL_OPERATIONS_EXTs
                       //where r.OPE_USR_ID == userId
                       select r)
                       .Where(predicate)
                       .AsQueryable();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetUserOperationsExt: ", e);
            }

            return res;
        }

        public IQueryable<COUNTRy> GetCountries(Expression<Func<COUNTRy, bool>> predicate = null)
        {
            IQueryable<COUNTRy> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                
                if (predicate == null) predicate = PredicateBuilder.True<COUNTRy>();

                res = (from r in dbContext.COUNTRies
                       select r)
                       .Where(predicate)
                       .AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountries: ", e);
            }

            return res;
        }

        public IQueryable<CURRENCy> GetCurrencies()
        {
            IQueryable<CURRENCy> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                
                res = (from r in dbContext.CURRENCies
                       select r)
                       .AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencies: ", e);
            }

            return res;
        }

        public bool UpdateCountry(ref COUNTRy country)
        {
            bool bRes = false;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                using (var transaction = new TransactionScope())
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

        public bool DeleteCountry(ref COUNTRy country)
        {
            bool bRes = false;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

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

        public IQueryable<GROUP> GetGroups()
        {
            IQueryable<GROUP> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                res = (from r in dbContext.GROUPs
                       select r)
                       .AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetGroups: ", e);
            }

            return res;
        }

        public IQueryable<TARIFF> GetTariffs()
        {
            IQueryable<TARIFF> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                res = (from r in dbContext.TARIFFs
                       select r)
                       .AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTariffs: ", e);
            }

            return res;
        }

        public IQueryable<SERVICE_CHARGE_TYPE> GetServiceChargeTypes()
        {
            IQueryable<SERVICE_CHARGE_TYPE> res = null;
            try
            {
                integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                res = (from r in dbContext.SERVICE_CHARGE_TYPEs
                       select r)
                       .AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetServiceChargeTypes: ", e);
            }

            return res;
        }

    }
}