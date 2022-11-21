using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Transactions;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace integraMobile.Domain.Concrete
{
    public class SQLInfraestructureRepository : IInfraestructureRepository
    {

        protected const int DEFAULT_WS_TIMEOUT = 20000; //ms   
        protected const int DEFAULT_WS_INCREASE_TIMEOUT = 5000; //ms   


        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SQLInfraestructureRepository));
        private const int ctnTransactionTimeout = 30;

        private List<COUNTRy> countriesTable = null;
        private List<FINAN_DIST_OPERATORS_INSTALLATION> finanDistOperatorsInstallationTable = null;
        private List<CURRENCy> currenciesTable=null;

        private const string PARAMETER_VAT_PERC = "VAT_PERC";
        private const string PARAMETER_CHANGE_FEE_PERC = "CHANGE_FEE_PERC";
        private static ulong _VERSION_2_13 = AppUtilities.AppVersion("2.13");


        private const int ctMinutesDBSync = 30;
        private static List<INSTALLATION> oGlobal_INSTALLATIONs = new List<INSTALLATION>();
        private static List<PARAMETER> oGlobal_PARAMETERs = new List<PARAMETER>();
        private static List<CURRENCY_RECHARGE_VALUE> oGlobal_CURRENCY_RECHARGE_VALUEs = new List<CURRENCY_RECHARGE_VALUE>();
        private static List<SOURCE_APP> oGlobal_SOURCE_APPs = new List<SOURCE_APP>();
        private static List<SOURCE_APPS_CONFIGURATION> oGlobal_SOURCE_APPS_CONFIGURATIONs = new List<SOURCE_APPS_CONFIGURATION>();
        private static List<ERROR_CUSTOM_MESSAGE> oGlobal_ERROR_CUSTOM_MESSAGEs = new List<ERROR_CUSTOM_MESSAGE>();
        private static List<GROUP> oGlobal_GROUPs = new List<GROUP>();
        private static List<GROUPS_TYPES_ASSIGNATION> oGlobal_GROUPS_TYPES_ASSIGNATIONs = new List<GROUPS_TYPES_ASSIGNATION>();
        private static List<LITERAL> oGlobal_LITERALs = new List<LITERAL>();
        private static List<LITERAL_LANGUAGE> oGlobal_LITERAL_LANGUAGEs = new List<LITERAL_LANGUAGE>();
        private static List<LANGUAGE> oGlobal_LANGUAGEs = new List<LANGUAGE>();


        private static DateTime dtGlobal_INSTALLATIONs = DateTime.UtcNow;
        private static DateTime dtGlobal_PARAMETERs = DateTime.UtcNow;
        private static DateTime dtGlobal_CURRENCY_RECHARGE_VALUEs = DateTime.UtcNow;
        private static DateTime dtGlobal_SOURCE_APPs = DateTime.UtcNow;
        private static DateTime dtGlobal_SOURCE_APPS_CONFIGURATIONs = DateTime.UtcNow;
        private static DateTime dtGlobal_ERROR_CUSTOM_MESSAGEs = DateTime.UtcNow;
        private static DateTime dtGlobal_GROUPs = DateTime.UtcNow;
        private static DateTime dtGlobal_GROUPS_TYPES_ASSIGNATIONs = DateTime.UtcNow;
        private static DateTime dtGlobal_LITERALs = DateTime.UtcNow;
        private static DateTime dtGlobal_LITERAL_LANGUAGEs = DateTime.UtcNow;
        private static DateTime dtGlobal_LANGUAGEs = DateTime.UtcNow;






        public SQLInfraestructureRepository(string connectionString)
        {
            //
            //
            //
            //
        }

        private List<INSTALLATION> getINSTALLATIONs(integraMobileDBEntitiesDataContext dbContext = null)
        {
            lock (oGlobal_INSTALLATIONs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_INSTALLATIONs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_INSTALLATIONs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_INSTALLATIONs).TotalSeconds > ctMinutesDBSync * 60))
                {

                    if (dbContext == null)
                    {
                        using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                         new TransactionOptions()
                                                                                                         {
                                                                                                             IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                             Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                         }))
                        {

                            integraMobileDBEntitiesDataContext dbContext2 = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                            oGlobal_INSTALLATIONs = (from p in dbContext2.INSTALLATIONs select p).ToList();

                            transaction.Complete();
                        }
                    }
                    else
                    {
                        oGlobal_INSTALLATIONs = (from p in dbContext.INSTALLATIONs select p).ToList();

                    }

                    dtGlobal_INSTALLATIONs = DateTime.UtcNow;
                }
                return oGlobal_INSTALLATIONs;

            }
        }



        private List<PARAMETER> getPARAMETERs(integraMobileDBEntitiesDataContext dbContext = null)
        {
            lock (oGlobal_PARAMETERs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_PARAMETERs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_PARAMETERs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_PARAMETERs).TotalSeconds > ctMinutesDBSync * 60))
                {

                    if (dbContext == null)
                    {
                        using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                         new TransactionOptions()
                                                                                                         {
                                                                                                             IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                             Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                         }))
                        {

                            integraMobileDBEntitiesDataContext dbContext2 = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                            oGlobal_PARAMETERs = (from p in dbContext2.PARAMETERs select p).ToList();

                            transaction.Complete();
                        }
                    }
                    else
                    {
                        oGlobal_PARAMETERs = (from p in dbContext.PARAMETERs select p).ToList();

                    }

                    dtGlobal_PARAMETERs = DateTime.UtcNow;
                }
                return oGlobal_PARAMETERs;

            }
        }



        public List<CURRENCY_RECHARGE_VALUE> getCURRENCY_RECHARGE_VALUEs(decimal dCurId,integraMobileDBEntitiesDataContext dbContext = null)
        {
            lock (oGlobal_CURRENCY_RECHARGE_VALUEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_CURRENCY_RECHARGE_VALUEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_CURRENCY_RECHARGE_VALUEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_CURRENCY_RECHARGE_VALUEs).TotalSeconds > ctMinutesDBSync * 60))
                {

                    if (dbContext == null)
                    {
                        using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                         new TransactionOptions()
                                                                                                         {
                                                                                                             IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                             Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                         }))
                        {

                            integraMobileDBEntitiesDataContext dbContext2 = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                            oGlobal_CURRENCY_RECHARGE_VALUEs = (from p in dbContext2.CURRENCY_RECHARGE_VALUEs select p).ToList();

                            transaction.Complete();
                        }
                    }
                    else
                    {
                        oGlobal_CURRENCY_RECHARGE_VALUEs = (from p in dbContext.CURRENCY_RECHARGE_VALUEs select p).ToList();

                    }

                    dtGlobal_CURRENCY_RECHARGE_VALUEs = DateTime.UtcNow;
                }
                return oGlobal_CURRENCY_RECHARGE_VALUEs.Where(r=> r.CURV_CUR_ID == dCurId).ToList();

            }
        }

        private List<SOURCE_APP> getSOURCE_APPs()
        {
            lock (oGlobal_SOURCE_APPs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_SOURCE_APPs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_SOURCE_APPs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_SOURCE_APPs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_SOURCE_APPs = (from p in dbContext.SOURCE_APPs select p).ToList();
                        dtGlobal_SOURCE_APPs = DateTime.UtcNow;

                        transaction.Complete();
                    }

                }
                return oGlobal_SOURCE_APPs;
            }

        }


        private List<SOURCE_APPS_CONFIGURATION> getSOURCE_APPS_CONFIGURATIONs()
        {
            lock (oGlobal_SOURCE_APPS_CONFIGURATIONs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_SOURCE_APPS_CONFIGURATIONs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_SOURCE_APPS_CONFIGURATIONs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_SOURCE_APPS_CONFIGURATIONs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {
                        
                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_SOURCE_APPS_CONFIGURATIONs = (from p in dbContext.SOURCE_APPS_CONFIGURATIONs select p).ToList();
                        dtGlobal_SOURCE_APPS_CONFIGURATIONs = DateTime.UtcNow;

                        transaction.Complete();
                    }

                }
                return oGlobal_SOURCE_APPS_CONFIGURATIONs;
            }

        }


        private List<ERROR_CUSTOM_MESSAGE> getERROR_CUSTOM_MESSAGEs()
        {
            lock (oGlobal_ERROR_CUSTOM_MESSAGEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_ERROR_CUSTOM_MESSAGEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_ERROR_CUSTOM_MESSAGEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_ERROR_CUSTOM_MESSAGEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_ERROR_CUSTOM_MESSAGEs = (from p in dbContext.ERROR_CUSTOM_MESSAGEs select p).ToList();
                        dtGlobal_ERROR_CUSTOM_MESSAGEs = DateTime.UtcNow;

                        transaction.Complete();
                    }

                }
                return oGlobal_ERROR_CUSTOM_MESSAGEs;
            }

        }


        private List<GROUP> getGROUPs()
        {
            lock (oGlobal_GROUPs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_GROUPs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_GROUPs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_GROUPs).TotalSeconds > ctMinutesDBSync * 60))

                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_GROUPs = (from p in dbContext.GROUPs select p).ToList();
                        dtGlobal_GROUPs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_GROUPs;

            }
        }


        private List<GROUPS_TYPES_ASSIGNATION> getGROUPS_TYPES_ASSIGNATIONs()
        {
            lock (oGlobal_GROUPS_TYPES_ASSIGNATIONs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_GROUPS_TYPES_ASSIGNATIONs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_GROUPS_TYPES_ASSIGNATIONs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_GROUPS_TYPES_ASSIGNATIONs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_GROUPS_TYPES_ASSIGNATIONs = (from p in dbContext.GROUPS_TYPES_ASSIGNATIONs select p).ToList();
                        dtGlobal_GROUPS_TYPES_ASSIGNATIONs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_GROUPS_TYPES_ASSIGNATIONs;

            }
        }


        private List<LITERAL> getLITERALs()
        {
            lock (oGlobal_LITERALs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_LITERALs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_LITERALs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_LITERALs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_LITERALs = (from p in dbContext.LITERALs select p).ToList();
                        dtGlobal_LITERALs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_LITERALs;
            }
        }


        private List<LITERAL_LANGUAGE> getLITERAL_LANGUAGEs()
        {
            lock (oGlobal_LITERAL_LANGUAGEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_LITERAL_LANGUAGEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_LITERAL_LANGUAGEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_LITERAL_LANGUAGEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_LITERAL_LANGUAGEs = (from p in dbContext.LITERAL_LANGUAGEs select p).ToList();
                        dtGlobal_LITERAL_LANGUAGEs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_LITERAL_LANGUAGEs;
            }
        }

        private List<LANGUAGE> getLANGUAGEs()
        {
            lock (oGlobal_LANGUAGEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_LANGUAGEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_LANGUAGEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_LANGUAGEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_LANGUAGEs = (from p in dbContext.LANGUAGEs select p).ToList();
                        dtGlobal_LANGUAGEs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_LANGUAGEs;

            }
        }

        public decimal GetDefaultSourceApp()
        {
            decimal dRes = 1;

            try
            {
                var oSourceApps = (from r in getSOURCE_APPs()
                                   where r.SOAPP_DEFAULT == 1
                                   select r).ToArray();
                if (oSourceApps.Count() == 1)
                {
                    dRes = oSourceApps.FirstOrDefault().SOAPP_ID;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDefaultSourceApp: ", e);
            }

            return dRes;
        }

        public List<SOURCE_APPS_CONFIGURATION> GetSourceAppsConfigurations()
        {
            List<SOURCE_APPS_CONFIGURATION> oRes = new List<SOURCE_APPS_CONFIGURATION>();

            try
            {
                oRes = (from r in getSOURCE_APPS_CONFIGURATIONs()
                        select r).ToList();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDefaultSourceApp: ", e);
            }

            return oRes;
        }


        public SOURCE_APPS_CONFIGURATION GetSourceAppsConfiguration(decimal dSourceApp)
        {
            SOURCE_APPS_CONFIGURATION oRes=null;

            try
            {
                oRes = (from r in getSOURCE_APPS_CONFIGURATIONs()
                        where r.SOAPC_SOAPP_ID==dSourceApp
                        select r).FirstOrDefault();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSourceAppsConfiguration: ", e);
            }

            return oRes;
        }

        public string GetSourceAppCode(decimal dSourceApp)
        {
            string strRes = "";

            try
            {
                var oSourceApps = (from r in getSOURCE_APPs()
                                   where r.SOAPP_ID == dSourceApp
                                   select r).ToArray();
                if (oSourceApps.Count() == 1)
                {
                    strRes = oSourceApps.FirstOrDefault().SOAPP_CODE;
                }
                else
                {
                    strRes = (from r in getSOURCE_APPs()
                              where r.SOAPP_DEFAULT == 1
                              select r).FirstOrDefault().SOAPP_CODE;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSourceAppCode: ", e);
            }

            return strRes;
        }


        public List<COUNTRy> Countries
        {
            get
            {

                if (countriesTable == null)
                    countriesTable = (DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null)).GetTable<COUNTRy>().OrderBy(coun => coun.COU_DESCRIPTION).ToList();
                return countriesTable;
            }
        }

        public List<INSTALLATION> Installations
        {
            get
            {               
                return getINSTALLATIONs().OrderBy(coun => coun.INS_DESCRIPTION).ToList();
            }
        }

        public List<CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG> PaymentGateways
        {
            get
            {                
                return (DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null)).GetTable<CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG>().ToList();
            }
        }

        public List<FINAN_DIST_OPERATORS_INSTALLATION> FinanDistOperatorsInstallation
        {
            get {
                if (finanDistOperatorsInstallationTable == null)
                    finanDistOperatorsInstallationTable = (DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null)).GetTable<FINAN_DIST_OPERATORS_INSTALLATION>().OrderBy(coun => coun.INSTALLATION.INS_DESCRIPTION).ToList();

                return finanDistOperatorsInstallationTable;
            }
        }


        public List<CURRENCy> Currencies
        {
            get {

                if (currenciesTable == null)
                    currenciesTable = (DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>(null)).GetTable<CURRENCy>().OrderBy(coun => coun.CUR_ISO_CODE).ToList();

                return currenciesTable; 
            }
        }

        public string GetParameterValue(string strParName, integraMobileDBEntitiesDataContext dbContext = null)
        {
            string strRes = "";
            try
            {
                var oResultParameters = getPARAMETERs(dbContext).Where(par => par.PAR_NAME == strParName);
                if (oResultParameters.Count() > 0)
                {
                    PARAMETER oParameter = oResultParameters.First();
                    strRes = oParameter.PAR_VALUE;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetParameterValue: ", e);
                strRes = "";
            }

            return strRes;
        }

        public string GetParameterValueNoCache(string strParName)
        {
            string strRes = "";
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

                    var oResultParameters = (from r in dbContext.PARAMETERs
                                             where r.PAR_NAME == strParName
                                             select r);
                    if (oResultParameters.Count() > 0)
                    {
                        PARAMETER oParameter = oResultParameters.First();
                        strRes = oParameter.PAR_VALUE;
                    }

                    transaction.Complete();
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetParameterValueNoCache: ", e);
                strRes = "";
            }

            return strRes;
        }

        public decimal GetVATPerc()
        {
            string strRes = "";
            decimal dRes = 0;
            try
            {
                strRes = GetParameterValue(PARAMETER_VAT_PERC);
                dRes = decimal.Parse(strRes,CultureInfo.InvariantCulture);
               
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetVATPerc: ", e);
                strRes = "";
            }

            return dRes;
        }

        public decimal GetChangeFeePerc()
        {
            string strRes = "";
            decimal dRes = 0;
            try
            {
                strRes = GetParameterValue(PARAMETER_CHANGE_FEE_PERC);
                dRes = decimal.Parse(strRes, CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetChangeFeePerc: ", e);
                strRes = "";
            }

            return dRes;
        }

        
        public string GetCountryTelephonePrefix(int iCountryId)
        {
            string strRes = "";
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    var oResultCountries = Countries.Where(coun => coun.COU_ID == iCountryId);
                    if (oResultCountries.Count() > 0)
                    {
                        COUNTRy oCountry = oResultCountries.First();
                        strRes = oCountry.COU_TEL_PREFIX;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountryTelephonePrefix: ", e);
                strRes = "";
            }

            return strRes;
        }


        public COUNTRy GetCountry(int iCountryId)
        {
            COUNTRy oCountry = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    var oResultCountries = Countries.Where(coun => coun.COU_ID == iCountryId);
                    if (oResultCountries.Count() > 0)
                    {
                        oCountry = oResultCountries.First();                       
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountry: ", e);                
            }

            return oCountry;
        }


        public int? GetCountryIdFromCountryCode(string strCode)
        {
            int? iRes = null;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    var oResultCountries = Countries.Where(coun => coun.COU_CODE == strCode);
                    if (oResultCountries.Count() > 0)
                    {
                        iRes = Convert.ToInt32(oResultCountries.First().COU_ID);
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountryIdFromCountryCode: ", e);
            }

            return iRes;
        }


        public int GetTelephonePrefixCountry(string strPrefix)
        {
            int iRes = -1;
            string strPrefixNorm = strPrefix.Replace("+", "").Trim() + " ";
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    var oResultCountries = Countries.Where(coun => coun.COU_TEL_PREFIX == strPrefixNorm);
                    if (oResultCountries.Count() > 0)
                    {
                        COUNTRy oCountry = oResultCountries.First();
                        iRes = Convert.ToInt32(oCountry.COU_ID);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTelephonePrefixCountry: ", e);
                iRes = -1;                
            }

            return iRes;
        }

        public string GetCountryName(int iCountryId)
        {
            string strRes = "";
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                             new TransactionOptions()
                                                                             {
                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                             }))
                {
                    var oResultCountries = Countries.Where(coun => coun.COU_ID == iCountryId);
                    if (oResultCountries.Count() > 0)
                    {
                        COUNTRy oCountry = oResultCountries.First();
                        strRes = oCountry.COU_DESCRIPTION;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountryName: ", e);
                strRes = "";
            }

            return strRes;
        }

        public int GetCountryCurrency(int iCountryId)
        {
            int iRes = -1;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    string strApplicationCurrencyISOCode = ConfigurationManager.AppSettings["ApplicationCurrencyISOCode"];

                    if (string.IsNullOrEmpty(strApplicationCurrencyISOCode))
                    {
                        var oResultCountries = Countries.Where(coun => coun.COU_ID == iCountryId);
                        if (oResultCountries.Count() > 0)
                        {
                            COUNTRy oCountry = oResultCountries.First();
                            iRes = Convert.ToInt32(oCountry.COU_CUR_ID);
                        }
                    }
                    else
                    {
                        var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strApplicationCurrencyISOCode);
                        if (oResultCurrencies.Count() > 0)
                        {
                            CURRENCy oCurrency = oResultCurrencies.First();
                            iRes = Convert.ToInt32(oCurrency.CUR_ID);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountryCurrency: ", e);
                iRes = -1;
            }

            return iRes;
        }


        public bool GetCountryPossibleSuscriptionTypes(int iCountryId, out string sSuscriptionType, out RefundBalanceType eRefundBalType)
        {
            bool bRes = false;
            sSuscriptionType = "";
            eRefundBalType = RefundBalanceType.rbtAmount;
                           
            try
            {
                   
                string sGeneralSuscriptionType = ConfigurationManager.AppSettings["SuscriptionType"] ?? "";
                eRefundBalType = (RefundBalanceType)Convert.ToInt32(ConfigurationManager.AppSettings["RefundBalanceType"] ?? "1");

                var oSuscType = Countries.Where(coun => coun.COU_ID == iCountryId)
                                    .First()
                                    .COUNTRIES_SUSCRIPTION_TYPEs
                                    .FirstOrDefault();

                if (oSuscType != null)
                {
                    if (oSuscType.COUSUST_SUSCR_TYPE.HasValue)
                    {
                        sSuscriptionType = oSuscType.COUSUST_SUSCR_TYPE.ToString();
                    }
                    eRefundBalType = (RefundBalanceType)oSuscType.COUSUST_REFUND_BALANCE_TYPE;
                }
                else
                {
                    sSuscriptionType = sGeneralSuscriptionType;
                }

                bRes = true;

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountryPossibleSuscriptionTypes: ", e);
                bRes = false;
            }
         

            return bRes;

        }

        public List<COUNTRIES_REDIRECTION> GetCountriesRedirections()
        {
            List<COUNTRIES_REDIRECTION> oResult = null;
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

                    var countriesRedirec = (from r in dbContext.COUNTRIES_REDIRECTIONs
                                     select r);

                    if (countriesRedirec.Count() > 0)
                    {
                        IEnumerable<COUNTRIES_REDIRECTION> filteredList = countriesRedirec.GroupBy(cr => cr.COURE_COUNTRY_REDIRECTION_WS_URL).Select(group => group.First());
                        oResult = filteredList.ToList();
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountriesRedirections: ", e);
            }
            return oResult;
        }

        public COUNTRIES_REDIRECTION GetCountriesRedirectionsByCityId(int iCity_ID)
        {
            COUNTRIES_REDIRECTION oResult = null;
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

                    var oInstallation = (from i in dbContext.INSTALLATIONs
                                     where i.INS_ID == iCity_ID
                                     select i);


                    if (oInstallation.Count() > 0)
                    {
                        INSTALLATION oIns = oInstallation.First();

                        var Countries = (from r in dbContext.COUNTRIES_REDIRECTIONs
                                         where r.COURE_COU_ID == oIns.INS_COU_ID
                                      select r);

                        if (Countries.Count() > 0)
                        {
                            oResult = Countries.First();
                        }
                    }
                    
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountriesRedirectionsByCityId: ", e);
            }
            return oResult;
        }

        public COUNTRIES_REDIRECTION GetCountriesRedirectionsByCountryId(decimal country_ID)
        {
            COUNTRIES_REDIRECTION oResult = null;
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

                    var Countries = (from r in dbContext.COUNTRIES_REDIRECTIONs
                                        where r.COURE_COU_ID == country_ID
                                    select r);

                    if (Countries.Count() > 0)
                    {
                        oResult = Countries.First();
                    }
                    
                    
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountriesRedirectionsByCountryId: ", e);
            }
            return oResult;
        }


        public List<COUNTRIES_REDIRECTION> GetCountriesRedirectionsGroupByPICURL()
        {
            List<COUNTRIES_REDIRECTION> oResult = null;
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

                    var countriesRedirec = (from r in dbContext.COUNTRIES_REDIRECTIONs
                                            select r);

                    if (countriesRedirec.Count() > 0)
                    {
                        IEnumerable<COUNTRIES_REDIRECTION> filteredList = countriesRedirec.GroupBy(cr => cr.COURE_COUNTRY_REDIRECTION_PICWS_URL).Select(group => group.First());
                        oResult = filteredList.ToList();
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCountriesRedirections: ", e);
            }
            return oResult;
        }


        public string GetCurrencyIsoCode(int iCurrencyId)
        {
            string strRes = "";
            try
            {
               
                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ID == iCurrencyId);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency = oResultCurrencies.First();
                    strRes = oCurrency.CUR_ISO_CODE;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyIsoCode: ", e);
                strRes = "";
            }

            return strRes;
        }


        


        public string GetCurrencySymbolOrIsoCode(int iCurrencyId)
        {
            string strRes = "";
            try
            {

                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ID == iCurrencyId);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency = oResultCurrencies.First();
                    strRes = string.IsNullOrEmpty(oCurrency.CUR_SYMBOL) ? oCurrency.CUR_ISO_CODE : oCurrency.CUR_SYMBOL;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyIsoCode: ", e);
                strRes = "";
            }

            return strRes;
        }

        public decimal GetCurrencyFromIsoCode(string strISOCode)
        {
            decimal dRes = -1;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                             new TransactionOptions()
                                                                             {
                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                             }))
                {
                    var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode);
                    if (oResultCurrencies.Count() > 0)
                    {
                        CURRENCy oCurrency = oResultCurrencies.First();
                        dRes = oCurrency.CUR_ID;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyFromIsoCode: ", e);
            }

            return dRes;
        }


        public string GetCurrencyIsoCodeNumericFromIsoCode(string strISOCode)
        {
            string strRes = "";
            try
            {
                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency = oResultCurrencies.First();
                    strRes = oCurrency.CUR_ISO_CODE_NUM;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyIsoCodeNumericFromIsoCode: ", e);
            }

            return strRes;
        }

        public int GetCurrencyDivisorFromIsoCode(string strISOCode)
        {
            int iRes = 1;
            try
            {                
                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency = oResultCurrencies.First();
                    iRes = Convert.ToInt32(Math.Pow(10,Convert.ToDouble(oCurrency.CUR_MINOR_UNIT)));
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyDivisorFromIsoCode: ", e);
            }

            return iRes;
        }

        public string GetCurSymbolFromIsoCode(string strISOCode)
        {
            string strRes = strISOCode;
            try
            {
                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency = oResultCurrencies.First();

                    if (!string.IsNullOrEmpty(oCurrency.CUR_SYMBOL))
                    {
                        strRes = oCurrency.CUR_SYMBOL;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurSymbolFromIsoCode: ", e);
            }

            return strRes;
        }


        public string GetDecimalFormatFromIsoCode(string strISOCode)
        {
            string strRes = "0.##";
            try
            {
                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency = oResultCurrencies.First();

                    if (oCurrency.CUR_MINOR_UNIT > 0)
                    {
                        strRes = "0.";
                        for (int i = 0; i < oCurrency.CUR_MINOR_UNIT;i++ )
                        {
                            strRes += "0";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetDecimalFormatFromIsoCode: ", e);
            }

            return strRes;
        }

        public int GetCurrenciesFactorDifference(string strISOCode1, string strISOCode2)
        {
            int iRes = 0;
            try
            {
                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode1);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency1 = oResultCurrencies.First();
                    int iMinorUnit1 = oCurrency1.CUR_MINOR_UNIT.Value;
                    var oResultCurrencies2 = Currencies.Where(curr => curr.CUR_ISO_CODE == strISOCode2);
                    if (oResultCurrencies2.Count() > 0)
                    {
                        CURRENCy oCurrency2 = oResultCurrencies2.First();
                        int iMinorUnit2 = oCurrency2.CUR_MINOR_UNIT.Value;
                        iRes = iMinorUnit2 - iMinorUnit1;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrenciesFactorDifference: ", e);
            }

            return iRes;
        }



        public int GetCurrenciesFactorDifference(int iCurId1, int iCurId2)
        {
            int iRes = 0;
            try
            {
                var oResultCurrencies = Currencies.Where(curr => curr.CUR_ID == (decimal)iCurId1);
                if (oResultCurrencies.Count() > 0)
                {
                    CURRENCy oCurrency1 = oResultCurrencies.First();
                    int iMinorUnit1 = oCurrency1.CUR_MINOR_UNIT.Value;
                    var oResultCurrencies2 = Currencies.Where(curr => curr.CUR_ID == (decimal)iCurId2);
                    if (oResultCurrencies2.Count() > 0)
                    {
                        CURRENCy oCurrency2 = oResultCurrencies2.First();
                        int iMinorUnit2 = oCurrency2.CUR_MINOR_UNIT.Value;
                        iRes = iMinorUnit2 - iMinorUnit1;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrenciesFactorDifference: ", e);
            }

            return iRes;
        }

        public long SendEmailTo(string strEmailAddress, string strSubject, string strMessageBody, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal)
        {
            long lRes = -1;
            try
            {
                if (!IsInternalEmail(strEmailAddress))
                {
                    if (!dSourceApp.HasValue) dSourceApp = GetDefaultSourceApp();

                    var oSourceAppConf = (from r in getSOURCE_APPS_CONFIGURATIONs()
                                          where r.SOAPC_SOAPP_ID == dSourceApp
                                          select r).FirstOrDefault();
                    if (oSourceAppConf != null)
                    {
                        integraSenderWS.integraSender oSender = new integraSenderWS.integraSender();
                        oSender.Url = oSourceAppConf.SOAPC_SENDER_WS_URL;
                        oSender.Credentials = new NetworkCredential(oSourceAppConf.SOAPC_SENDER_WS_USERNAME, oSourceAppConf.SOAPC_SENDER_WS_PASSWORD);

                        lRes = oSender.AddEmailToSendWithPriority(strSubject, strMessageBody, strEmailAddress, "", "", "", emailPriority);
                    }
                }
                else
                {
                    lRes = 1;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("SendEmailTo: '{0}' is an internal mail. No send needed.", strEmailAddress));
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SendEmailTo: ", e);
                lRes = -1;
            }

            return lRes;
        }

        public List<long> SendEmailToMultiRecipients(List<string> lstRecipients, string strSubject, string strMessageBody, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal)
        {
            List<long> lstRes = new List<long>();
            try
            {
                var oSendMails = lstRecipients.Where(mail => !IsInternalEmail(mail)).ToList();
                if (oSendMails.Any())
                {
                    if (!dSourceApp.HasValue) dSourceApp = GetDefaultSourceApp();

                    var oSourceAppConf = (from r in getSOURCE_APPS_CONFIGURATIONs()
                                          where r.SOAPC_SOAPP_ID == dSourceApp
                                          select r).FirstOrDefault();
                    if (oSourceAppConf != null)
                    {
                        integraSenderWS.integraSender oSender = new integraSenderWS.integraSender();
                        oSender.Url = oSourceAppConf.SOAPC_SENDER_WS_URL;
                        oSender.Credentials = new NetworkCredential(oSourceAppConf.SOAPC_SENDER_WS_USERNAME, oSourceAppConf.SOAPC_SENDER_WS_PASSWORD);

                        long[] arrRes = oSender.AddEmailToSendMultiRecipients(strSubject, strMessageBody, oSendMails.ToArray(), "", "", "", emailPriority);
                        if (arrRes != null) lstRes = arrRes.ToList();
                    }
                }

                var oDicRes = lstRecipients.Select((mail, index) => new { mail = mail, index = index }).ToDictionary(item => item.mail, (item) => new SendEmailResult { mail = item.mail, index = item.index, res = (long)1 });
                for (int i = 0; i < lstRes.Count; i += 1)
                {
                    if (oDicRes.ContainsKey(oSendMails[i]))
                        oDicRes[oSendMails[i]].res = lstRes[i];
                }
                lstRes = oDicRes.Values.OrderBy(i => i.index).Select(i => i.res).ToList();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SendEmailToMultiRecipients: ", e);                
            }

            return lstRes;
        }

        public long SendEmailWithAttachmentsTo(string strEmailAddress, string strSubject, string strMessageBody, List<FileAttachmentInfo> lstAttachments, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal)
        {
            long lRes = -1;
            try
            {
                if (!IsInternalEmail(strEmailAddress))
                {

                    if (!dSourceApp.HasValue) dSourceApp = GetDefaultSourceApp();

                    var oSourceAppConf = (from r in getSOURCE_APPS_CONFIGURATIONs()
                                          where r.SOAPC_SOAPP_ID == dSourceApp
                                          select r).FirstOrDefault();

                    if (oSourceAppConf != null)
                    {
                        integraSenderWS.integraSender oSender = new integraSenderWS.integraSender();
                        oSender.Url = oSourceAppConf.SOAPC_SENDER_WS_URL;
                        oSender.Credentials = new NetworkCredential(oSourceAppConf.SOAPC_SENDER_WS_USERNAME, oSourceAppConf.SOAPC_SENDER_WS_PASSWORD);

                        if (lstAttachments != null)
                        {

                            if (lstAttachments.Count() > 0)
                            {
                                int i = 0;
                                integraSenderWS.FileAttachmentInfo[] arrFiles = new integraSenderWS.FileAttachmentInfo[lstAttachments.Count()];

                                foreach (FileAttachmentInfo oAttach in lstAttachments)
                                {
                                    integraSenderWS.FileAttachmentInfo file = new integraSenderWS.FileAttachmentInfo();
                                    file.strName = oAttach.strName;
                                    file.strMediaType = oAttach.strMediaType;
                                    if ((oAttach.fileContent == null) && (!string.IsNullOrEmpty(oAttach.filePath)))
                                    {
                                        file.fileContent = System.IO.File.ReadAllBytes(oAttach.filePath);
                                    }
                                    else if ((oAttach.fileContent != null) && (oAttach.fileContent.Length > 0))
                                    {
                                        file.fileContent = new byte[oAttach.fileContent.Length];
                                        Array.Copy(oAttach.fileContent, file.fileContent, oAttach.fileContent.Length);
                                    }

                                    arrFiles[i++] = file;
                                }


                                lRes = oSender.AddEmailWithAttachementsToSendWithPriority(strSubject, strMessageBody, strEmailAddress, arrFiles, "", "", "", emailPriority);

                            }
                            else
                            {
                                lRes = oSender.AddEmailToSendWithPriority(strSubject, strMessageBody, strEmailAddress, "", "", "", emailPriority);
                            }

                        }
                        else
                        {
                            lRes = oSender.AddEmailToSendWithPriority(strSubject, strMessageBody, strEmailAddress, "", "", "", emailPriority);
                        }
                    }
                }
                else
                {
                    lRes = 1;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("SendEmailWithAttachmentsTo: '{0}' is an internal mail. No send needed.", strEmailAddress));
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SendEmailWithAttachmentsTo: ", e);
                lRes = -1;
            }

            return lRes;
        }

        public List<long> SendEmailWithAttachmentsToMultiRecipients(List<string> lstRecipients, string strSubject, string strMessageBody, List<FileAttachmentInfo> lstAttachments, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal)
        {
            List<long> lstRes = new List<long>();
            try
            {
                var oSendMails = lstRecipients.Where(mail => !IsInternalEmail(mail)).ToList();
                if (oSendMails.Any())
                {
                    if (!dSourceApp.HasValue) dSourceApp = GetDefaultSourceApp();

                    var oSourceAppConf = (from r in getSOURCE_APPS_CONFIGURATIONs()
                                          where r.SOAPC_SOAPP_ID == dSourceApp
                                          select r).FirstOrDefault();
                    if (oSourceAppConf != null)
                    {
                        integraSenderWS.integraSender oSender = new integraSenderWS.integraSender();
                        oSender.Url = oSourceAppConf.SOAPC_SENDER_WS_URL;
                        oSender.Credentials = new NetworkCredential(oSourceAppConf.SOAPC_SENDER_WS_USERNAME, oSourceAppConf.SOAPC_SENDER_WS_PASSWORD);

                        int iRecipientsBlockSize = Convert.ToInt32(ConfigurationManager.AppSettings["integraSenderWS_RecipientsBlockSize"] ?? "500");

                        if (lstAttachments != null)
                        {

                            if (lstAttachments.Count() > 0)
                            {
                                int i = 0;
                                integraSenderWS.FileAttachmentInfo[] arrFiles = new integraSenderWS.FileAttachmentInfo[lstAttachments.Count()];

                                foreach (FileAttachmentInfo oAttach in lstAttachments)
                                {
                                    integraSenderWS.FileAttachmentInfo file = new integraSenderWS.FileAttachmentInfo();
                                    file.strName = oAttach.strName;
                                    file.strMediaType = oAttach.strMediaType;
                                    if ((oAttach.fileContent == null) && (!string.IsNullOrEmpty(oAttach.filePath)))
                                    {
                                        file.fileContent = System.IO.File.ReadAllBytes(oAttach.filePath);
                                    }
                                    else if ((oAttach.fileContent != null) && (oAttach.fileContent.Length > 0))
                                    {
                                        file.fileContent = new byte[oAttach.fileContent.Length];
                                        Array.Copy(oAttach.fileContent, file.fileContent, oAttach.fileContent.Length);
                                    }

                                    arrFiles[i++] = file;
                                }

                                if (lstRecipients.Count > iRecipientsBlockSize)
                                {
                                    List<string> oRecipientsBlock = null;
                                    int iBlocksCount = lstRecipients.Count / iRecipientsBlockSize;
                                    if ((lstRecipients.Count % iRecipientsBlockSize) > 0) iBlocksCount += 1;
                                    for (int iBlock = 0; iBlock < iBlocksCount; iBlock++)
                                    {
                                        if (iBlock < (iBlocksCount - 1))
                                            oRecipientsBlock = lstRecipients.GetRange(iBlock * iRecipientsBlockSize, iRecipientsBlockSize);
                                        else
                                            oRecipientsBlock = lstRecipients.GetRange(iBlock * iRecipientsBlockSize, lstRecipients.Count - (iBlock * iRecipientsBlockSize));
                                        long[] arrRes = oSender.AddEmailWithAttachementsToSendMultiRecipients(strSubject, strMessageBody, oRecipientsBlock.ToArray(), arrFiles, "", "", "", emailPriority);
                                        if (arrRes != null) lstRes = arrRes.ToList();
                                    }
                                }
                                else
                                {
                                    long[] arrRes = oSender.AddEmailWithAttachementsToSendMultiRecipients(strSubject, strMessageBody, lstRecipients.ToArray(), arrFiles, "", "", "", emailPriority);
                                    if (arrRes != null) lstRes = arrRes.ToList();
                                }

                            }
                            else
                            {
                                if (lstRecipients.Count > iRecipientsBlockSize)
                                {
                                    List<string> oRecipientsBlock = null;
                                    int iBlocksCount = lstRecipients.Count / iRecipientsBlockSize;
                                    if ((lstRecipients.Count % iRecipientsBlockSize) > 0) iBlocksCount += 1;
                                    for (int iBlock = 0; iBlock < iBlocksCount; iBlock++)
                                    {
                                        if (iBlock < (iBlocksCount - 1))
                                            oRecipientsBlock = lstRecipients.GetRange(iBlock * iRecipientsBlockSize, iRecipientsBlockSize);
                                        else
                                            oRecipientsBlock = lstRecipients.GetRange(iBlock * iRecipientsBlockSize, lstRecipients.Count - (iBlock * iRecipientsBlockSize));
                                        long[] arrRes = oSender.AddEmailToSendMultiRecipients(strSubject, strMessageBody, oRecipientsBlock.ToArray(), "", "", "", emailPriority);
                                        if (arrRes != null) lstRes = arrRes.ToList();
                                    }
                                }
                                else
                                {
                                    long[] arrRes = oSender.AddEmailToSendMultiRecipients(strSubject, strMessageBody, lstRecipients.ToArray(), "", "", "", emailPriority);
                                    if (arrRes != null) lstRes = arrRes.ToList();
                                }
                            }

                        }
                        else
                        {
                            if (lstRecipients.Count > iRecipientsBlockSize)
                            {
                                List<string> oRecipientsBlock = null;
                                int iBlocksCount = lstRecipients.Count / iRecipientsBlockSize;
                                if ((lstRecipients.Count % iRecipientsBlockSize) > 0) iBlocksCount += 1;
                                for (int iBlock = 0; iBlock < iBlocksCount; iBlock++)
                                {
                                    if (iBlock < (iBlocksCount - 1))
                                        oRecipientsBlock = lstRecipients.GetRange(iBlock * iRecipientsBlockSize, iRecipientsBlockSize);
                                    else
                                        oRecipientsBlock = lstRecipients.GetRange(iBlock * iRecipientsBlockSize, lstRecipients.Count - (iBlock * iRecipientsBlockSize));
                                    long[] arrRes = oSender.AddEmailToSendMultiRecipients(strSubject, strMessageBody, oRecipientsBlock.ToArray(), "", "", "", emailPriority);
                                    if (arrRes != null) lstRes = arrRes.ToList();
                                }
                            }
                            else
                            {
                                long[] arrRes = oSender.AddEmailToSendMultiRecipients(strSubject, strMessageBody, lstRecipients.ToArray(), "", "", "", emailPriority);
                                if (arrRes != null) lstRes = arrRes.ToList();
                            }
                        }
                    }
                }
                var oDicRes = lstRecipients.Select((mail, index) => new { mail = mail, index = index }).ToDictionary(item => item.mail, (item) => new SendEmailResult { mail = item.mail, index = item.index, res = (long)1 });
                for (int i = 0; i < lstRes.Count; i += 1)
                {
                    if (oDicRes.ContainsKey(oSendMails[i]))
                        oDicRes[oSendMails[i]].res = lstRes[i];
                }
                lstRes = oDicRes.Values.OrderBy(i => i.index).Select(i => i.res).ToList();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SendEmailWithAttachmentsToMultiRecipients: ", e);                
            }

            return lstRes;
        }

        public List<long> SendEmailToMultiRecipientsTool(decimal dUniqueId, string strSubject, string strMessageBody, decimal? dSourceApp, integraSenderWS.EmailPriority emailPriority = integraSenderWS.EmailPriority.Normal)
        {
            List<long> lstRes = new List<long>();

            System.Data.Common.DbCommand oCommand = null;

            try
            {

                 if (!dSourceApp.HasValue) dSourceApp = GetDefaultSourceApp();

                    var oSourceAppConf = (from r in getSOURCE_APPS_CONFIGURATIONs()
                                          where r.SOAPC_SOAPP_ID == dSourceApp
                                          select r).FirstOrDefault();
                    if (oSourceAppConf != null)
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                        oCommand = dbContext.Connection.CreateCommand();
                        oCommand.CommandTimeout = 60 * 5;
                        oCommand.CommandType = System.Data.CommandType.Text;
                        int iCount;

                        if (dbContext.Connection.State != System.Data.ConnectionState.Open) dbContext.Connection.Open();

                        string sIntegraSenderDB = oSourceAppConf.SOAPC_SENDER_DBNAME;

                        string sSQL = "INSERT INTO {0}.dbo.EMAIL_MESSAGES (EMSG_SUBJECT, EMSG_BODY, EMSG_RECIPIENT, EMSG_INSERTION_DATE, EMSG_PRIORITY) " +
                                      "SELECT '{1}', '{2}', ETR_EMAIL, GETUTCDATE(), {3} " +
                                      "FROM EMAILTOOL_RECIPIENTS " +
                                      "WHERE ETR_ID = {4} AND ETR_EMAIL NOT LIKE '##Status_%'";

                        oCommand.CommandText = string.Format(sSQL, sIntegraSenderDB, strSubject.Replace("'", "''"), strMessageBody.Replace("'", "''"), (int)emailPriority, dUniqueId);
                        iCount = oCommand.ExecuteNonQuery();
                    }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SendEmailToMultiRecipientsTool: ", e);
            }
            finally
            {
                if (oCommand != null)
                {
                    oCommand.Dispose();
                    oCommand = null;
                }
            }

            return lstRes;
        }

        public long SendSMSTo(int iCountryId, string strTelephone, string strMessage, decimal? dSourceApp, ref string strCompleteTelephone)
        {
            long lRes = -1;
            try
            {
                 if (!dSourceApp.HasValue) dSourceApp = GetDefaultSourceApp();

                    var oSourceAppConf = (from r in getSOURCE_APPS_CONFIGURATIONs()
                                          where r.SOAPC_SOAPP_ID == dSourceApp
                                          select r).FirstOrDefault();
                    if (oSourceAppConf != null)
                    {
                        integraSenderWS.integraSender oSender = new integraSenderWS.integraSender();
                        oSender.Url = oSourceAppConf.SOAPC_SENDER_WS_URL;
                        oSender.Credentials = new NetworkCredential(oSourceAppConf.SOAPC_SENDER_WS_USERNAME, oSourceAppConf.SOAPC_SENDER_WS_PASSWORD);

                        strCompleteTelephone = GetCountryTelephonePrefix(iCountryId) + strTelephone;
                        strCompleteTelephone = RemoveNotNumericCharacters(strCompleteTelephone);

                        if (oSourceAppConf.SOAPC_SENDER_SEND_SMS==1)
                        {
                            lRes = oSender.AddSMSToSend(strMessage, strCompleteTelephone, "", "", "");
                        }
                        else
                        {
                            lRes = 1;
                        }
                    }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SendSMSTo: ", e);
                lRes = -1;
            }

            return lRes;
        }


        public bool GetFirstNotGeneratedUserNotification(out USERS_NOTIFICATION notif, out int iQueueLengthUserNotificationMultiplier, List<decimal> oListRunningUserNotificationMultiplier)
        {

            bool bRes = true;
            notif = null;
            iQueueLengthUserNotificationMultiplier = 0;
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
                        integraMobileDBEntitiesDataContext dbContext =new integraMobileDBEntitiesDataContext();

                        var predicate = PredicateBuilder.True<USERS_NOTIFICATION>();

                        if ((oListRunningUserNotificationMultiplier != null) && (oListRunningUserNotificationMultiplier.Count() > 0))
                        {
                            foreach (decimal UserNotf in oListRunningUserNotificationMultiplier)
                            {
                                predicate = predicate.And(a => a.UNO_ID != UserNotf);
                            }
                        }

                        var notifs = (from r in dbContext.USERS_NOTIFICATIONs
                                      where r.UNO_STATUS == Convert.ToInt32(UserNotificationStatus.Inserted) && r.USER.USR_ENABLED == 1 &&
                                       (!r.UNO_STARTDATETIME.HasValue || r.UNO_STARTDATETIME <= DateTime.UtcNow)
                                      orderby r.UNO_ID
                                      select r).Where(predicate);

                        if (notifs.Count() > 0)
                        {
                            iQueueLengthUserNotificationMultiplier = notifs.Count();
                            notif = notifs.First();
                        }
                        else
                        {
                            dbContext.Close();
                        }
                       

                    }
                    catch(Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetFirstNotGeneratedUserNotification: ", e);
                        bRes = false;

                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetFirstNotGeneratedUserNotification: ", e);
                bRes = false;
            }

            return bRes;


        }



        public bool GenerateUserNotification(ref USERS_NOTIFICATION notif)
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
                    int iNumPushGenerated = 0;
                    try
                    {

                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                        decimal notifId = notif.UNO_ID;


                        var oNotifs = (from r in dbContext.USERS_NOTIFICATIONs
                                       where r.UNO_ID == notifId &&
                                         r.UNO_STATUS == Convert.ToInt32(UserNotificationStatus.Inserted)
                                       select r);

                        USERS_NOTIFICATION oNotif = null;
                        if (oNotifs.Count() > 0)
                        {
                            oNotif = oNotifs.First();
                        }

                        if (oNotif != null)
                        {
                            decimal? dSourceApp = null;

                            if (oNotif.USERS_WARNINGs.Any())
                            {
                                dSourceApp = oNotif.USERS_WARNINGs.FirstOrDefault().UWA_SOAPP_ID;
                            }


                            var predicate = PredicateBuilder.True<USERS_PUSH_ID>();

                            if (oNotif.UNO_UPID_ID != null)
                            {
                                predicate = predicate.And(r => r.UPID_ID == oNotif.UNO_UPID_ID.Value);
                            }


                            var oPushIds = (from r in dbContext.USERS_PUSH_IDs
                                            where r.UPID_USR_ID == oNotif.USER.USR_ID
                                                     select r)
                                            .Where(predicate);

                            foreach (USERS_PUSH_ID oPushId in oPushIds)
                            {
                                if (!dSourceApp.HasValue || oPushId.UPID_LAST_SOAPP_ID.Value == dSourceApp.Value)
                                {

                                    switch ((MobileOS)oPushId.UPID_OS)
                                    {
                                        case MobileOS.WindowsPhone:
                                            {

                                                if ((!string.IsNullOrEmpty(oNotif.UNO_WP_TEXT1)) ||
                                                    (!string.IsNullOrEmpty(oNotif.UNO_WP_TEXT2)))
                                                {

                                                    oNotif.PUSHID_NOTIFICATIONs.Add(new PUSHID_NOTIFICATION
                                                    {
                                                        PNO_OS = oPushId.UPID_OS,
                                                        PNO_PUSHID = oPushId.UPID_PUSHID,
                                                        PNO_LAST_RETRY_DATETIME = null,
                                                        PNO_LIMITDATETIME = oNotif.UNO_LIMITDATETIME,
                                                        PNO_RETRIES = 0,
                                                        PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Inserted),
                                                        PNO_WP_TEXT1 = oNotif.UNO_WP_TEXT1,
                                                        PNO_WP_TEXT2 = oNotif.UNO_WP_TEXT2,
                                                        PNO_WP_PARAM = oNotif.UNO_WP_PARAM,
                                                        PNO_WP_BACKGROUND_IMAGE = "",
                                                        PNO_WP_COUNT = null,
                                                        PNO_WP_TILE_TITLE = "",
                                                        PNO_WP_RAW_DATA = "",
                                                        PNO_ANDROID_RAW_DATA = "",
                                                        PNO_iOS_RAW_DATA = "",
                                                        PNO_SOAPP_ID = oPushId.UPID_LAST_SOAPP_ID,
                                                    });
                                                    iNumPushGenerated++;
                                                }

                                                if ((!string.IsNullOrEmpty(oNotif.UNO_WP_TILE_TITLE)) ||
                                                    (oNotif.UNO_WP_COUNT.HasValue) ||
                                                    (!string.IsNullOrEmpty(oNotif.UNO_WP_BACKGROUND_IMAGE)))
                                                {

                                                    oNotif.PUSHID_NOTIFICATIONs.Add(new PUSHID_NOTIFICATION
                                                    {
                                                        PNO_OS = oPushId.UPID_OS,
                                                        PNO_PUSHID = oPushId.UPID_PUSHID,
                                                        PNO_LAST_RETRY_DATETIME = null,
                                                        PNO_LIMITDATETIME = oNotif.UNO_LIMITDATETIME,
                                                        PNO_RETRIES = 0,
                                                        PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Inserted),
                                                        PNO_WP_TEXT1 = "",
                                                        PNO_WP_TEXT2 = "",
                                                        PNO_WP_PARAM = "",
                                                        PNO_WP_BACKGROUND_IMAGE = oNotif.UNO_WP_BACKGROUND_IMAGE,
                                                        PNO_WP_COUNT = oNotif.UNO_WP_COUNT,
                                                        PNO_WP_TILE_TITLE = oNotif.UNO_WP_TILE_TITLE,
                                                        PNO_WP_RAW_DATA = "",
                                                        PNO_ANDROID_RAW_DATA = "",
                                                        PNO_iOS_RAW_DATA = "",
                                                        PNO_SOAPP_ID = oPushId.UPID_LAST_SOAPP_ID,

                                                    });
                                                    iNumPushGenerated++;
                                                }

                                                if (!string.IsNullOrEmpty(oNotif.UNO_WP_RAW_DATA))
                                                {

                                                    oNotif.PUSHID_NOTIFICATIONs.Add(new PUSHID_NOTIFICATION
                                                    {
                                                        PNO_OS = oPushId.UPID_OS,
                                                        PNO_PUSHID = oPushId.UPID_PUSHID,
                                                        PNO_LAST_RETRY_DATETIME = null,
                                                        PNO_LIMITDATETIME = oNotif.UNO_LIMITDATETIME,
                                                        PNO_RETRIES = 0,
                                                        PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Inserted),
                                                        PNO_WP_TEXT1 = "",
                                                        PNO_WP_TEXT2 = "",
                                                        PNO_WP_PARAM = "",
                                                        PNO_WP_BACKGROUND_IMAGE = "",
                                                        PNO_WP_COUNT = null,
                                                        PNO_WP_TILE_TITLE = "",
                                                        PNO_WP_RAW_DATA = oNotif.UNO_WP_RAW_DATA,
                                                        PNO_ANDROID_RAW_DATA = "",
                                                        PNO_iOS_RAW_DATA = "",
                                                        PNO_SOAPP_ID = oPushId.UPID_LAST_SOAPP_ID,

                                                    });
                                                    iNumPushGenerated++;
                                                }

                                            }
                                            break;


                                        case MobileOS.Android:
                                            {


                                                if (!string.IsNullOrEmpty(oNotif.UNO_ANDROID_RAW_DATA))
                                                {

                                                    oNotif.PUSHID_NOTIFICATIONs.Add(new PUSHID_NOTIFICATION
                                                    {
                                                        PNO_OS = oPushId.UPID_OS,
                                                        PNO_PUSHID = oPushId.UPID_PUSHID,
                                                        PNO_LAST_RETRY_DATETIME = null,
                                                        PNO_LIMITDATETIME = oNotif.UNO_LIMITDATETIME,
                                                        PNO_RETRIES = 0,
                                                        PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Inserted),
                                                        PNO_WP_TEXT1 = "",
                                                        PNO_WP_TEXT2 = "",
                                                        PNO_WP_PARAM = "",
                                                        PNO_WP_BACKGROUND_IMAGE = "",
                                                        PNO_WP_COUNT = null,
                                                        PNO_WP_TILE_TITLE = "",
                                                        PNO_WP_RAW_DATA = "",
                                                        PNO_ANDROID_RAW_DATA = oNotif.UNO_ANDROID_RAW_DATA,
                                                        PNO_iOS_RAW_DATA = "",
                                                        PNO_SOAPP_ID = oPushId.UPID_LAST_SOAPP_ID,
                                                    });
                                                    iNumPushGenerated++;
                                                }

                                            }
                                            break;
                                        case MobileOS.iOS:
                                            {


                                                if (!string.IsNullOrEmpty(oNotif.UNO_iOS_RAW_DATA))
                                                {

                                                    oNotif.PUSHID_NOTIFICATIONs.Add(new PUSHID_NOTIFICATION
                                                    {
                                                        PNO_OS = oPushId.UPID_OS,
                                                        PNO_PUSHID = oPushId.UPID_PUSHID,
                                                        PNO_LAST_RETRY_DATETIME = null,
                                                        PNO_LIMITDATETIME = oNotif.UNO_LIMITDATETIME,
                                                        PNO_RETRIES = 0,
                                                        PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Inserted),
                                                        PNO_WP_TEXT1 = "",
                                                        PNO_WP_TEXT2 = "",
                                                        PNO_WP_PARAM = "",
                                                        PNO_WP_BACKGROUND_IMAGE = "",
                                                        PNO_WP_COUNT = null,
                                                        PNO_WP_TILE_TITLE = "",
                                                        PNO_WP_RAW_DATA = "",
                                                        PNO_ANDROID_RAW_DATA = "",
                                                        PNO_iOS_RAW_DATA = oNotif.UNO_iOS_RAW_DATA,
                                                        PNO_SOAPP_ID = oPushId.UPID_LAST_SOAPP_ID,
                                                    });
                                                    iNumPushGenerated++;
                                                }

                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                }
                            }

                            if (iNumPushGenerated > 0)
                            {
                                oNotif.UNO_STATUS = Convert.ToInt32(UserNotificationStatus.Generated);
                            }
                            else
                            {
                                oNotif.UNO_STATUS = Convert.ToInt32(UserNotificationStatus.Finished_Partially);
                            }
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();
                            notif = oNotif;
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "GenerateUserNotification: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GenerateUserNotification: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GenerateUserNotification: ", e);
                bRes = false;
            }


            return bRes;


        }


        public bool GetFirstNotSentNotification(out PUSHID_NOTIFICATION notif, int iResendTime, out int iQueueLengthPushIdNotificationSender, List<decimal> oListRunningPushIdNotificationSender)
        {

            bool bRes = true;
            notif = null;
            iQueueLengthPushIdNotificationSender=0;
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                        var predicate = PredicateBuilder.True<PUSHID_NOTIFICATION>();

                        if ((oListRunningPushIdNotificationSender != null) && (oListRunningPushIdNotificationSender.Count() > 0))
                        {
                            foreach (decimal PushIdNot in oListRunningPushIdNotificationSender)
                            {
                                predicate = predicate.And(a => a.PNO_ID != PushIdNot);
                            }
                        }

                        var notifs = (from r in dbContext.PUSHID_NOTIFICATIONs
                                      where ((r.PNO_STATUS == Convert.ToInt32(PushIdNotificationStatus.Inserted)) ||
                                             ((r.PNO_LAST_RETRY_DATETIME.HasValue) &&
                                              ((DateTime.UtcNow - r.PNO_LAST_RETRY_DATETIME.Value).TotalSeconds >= iResendTime) &&
                                              (r.PNO_STATUS == Convert.ToInt32(PushIdNotificationStatus.Waiting_Next_Retry))))
                                      orderby r.PNO_ID
                                      select r).Where(predicate);

                        if (notifs.Count() > 0)
                        {
                            iQueueLengthPushIdNotificationSender = notifs.Count();
                            notif = notifs.First();
                        }

                        if (notif != null)
                        {
                            notif.PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Sending);
                            SetUserNotificationStatus(notif.PNO_UTNO_ID,ref dbContext);

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                               
                                transaction.Complete();
                                dbContext.Close();
                               

                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "GetFirstNotSentNotification: ", e);
                                bRes = false;
                            }
                        }


                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetFirstNotSentNotification: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetFirstNotSentNotification: ", e);
                bRes = false;
            }


            return bRes;


        }


        public bool PushIdNotificationSent(decimal dPushNotifID)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                        var oNotifs = (from r in dbContext.PUSHID_NOTIFICATIONs
                                       where r.PNO_ID == dPushNotifID &&
                                         r.PNO_STATUS == Convert.ToInt32(PushIdNotificationStatus.Sending)
                                       select r);


                        PUSHID_NOTIFICATION oNotif = null;
                        if (oNotifs.Count() > 0)
                        {
                            oNotif = oNotifs.First();
                        }


                        if (oNotif != null)
                        {
                            oNotif.PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Sent);
                            oNotif.PNO_LAST_RETRY_DATETIME = DateTime.UtcNow;

                            var oUserPushIds = (from r in dbContext.USERS_PUSH_IDs
                                                where r.UPID_PUSHID == oNotif.PNO_PUSHID &&
                                                  r.UPID_USR_ID == oNotif.USERS_NOTIFICATION.UNO_USR_ID
                                                select r);

                            if (oUserPushIds.Count() > 0)
                            {
                                oUserPushIds.First().UPID_PUSH_RETRIES = 0;
                                oUserPushIds.First().UPID_LAST_RETRY_DATETIME = oNotif.PNO_LAST_RETRY_DATETIME;
                                oUserPushIds.First().UPID_LAST_SUCESSFUL_PUSH = oNotif.PNO_LAST_RETRY_DATETIME;

                            }

                            SetUserNotificationStatus(oNotif.PNO_UTNO_ID, ref dbContext);


                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "PushIdNotificationSent: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "PushIdNotificationSent: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "PushIdNotificationSent: ", e);
                bRes = false;
            }


            return bRes;


        }



        public bool PushIdNotificationFailed(decimal dPushNotifID, int iMaxRetries)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                        var oNotifs = (from r in dbContext.PUSHID_NOTIFICATIONs
                                       where r.PNO_ID == dPushNotifID &&
                                         r.PNO_STATUS == Convert.ToInt32(PushIdNotificationStatus.Sending)
                                       select r);


                        PUSHID_NOTIFICATION oNotif = null;
                        if (oNotifs.Count() > 0)
                        {
                            oNotif = oNotifs.First();
                        }


                        if (oNotif != null)
                        {
                            oNotif.PNO_RETRIES++;
                            oNotif.PNO_LAST_RETRY_DATETIME = DateTime.UtcNow;

                            if (oNotif.PNO_RETRIES >= iMaxRetries)
                            {
                                oNotif.PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Failed);
                            }
                            else
                            {
                                oNotif.PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Waiting_Next_Retry);

                            }

                            var oUserPushIds = (from r in dbContext.USERS_PUSH_IDs
                                                where r.UPID_PUSHID == oNotif.PNO_PUSHID &&
                                                  r.UPID_USR_ID == oNotif.USERS_NOTIFICATION.UNO_USR_ID
                                                select r);

                            if (oUserPushIds.Count() > 0)
                            {
                                oUserPushIds.First().UPID_PUSH_RETRIES++;
                                oUserPushIds.First().UPID_LAST_RETRY_DATETIME = oNotif.PNO_LAST_RETRY_DATETIME;
                            }

                            SetUserNotificationStatus(oNotif.PNO_UTNO_ID,ref dbContext);

                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);                              
                            transaction.Complete();
                            dbContext.Close();


                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "PushIdNotificationFailed: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "PushIdNotificationFailed: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "PushIdNotificationFailed: ", e);
                bRes = false;
            }


            return bRes;


        }

        public bool PushIdExpired(decimal dPushNotifID, string strNewPushId)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                        var oNotifs = (from r in dbContext.PUSHID_NOTIFICATIONs
                                       where r.PNO_ID == dPushNotifID &&
                                         r.PNO_STATUS == Convert.ToInt32(PushIdNotificationStatus.Sending)
                                       select r);

                        PUSHID_NOTIFICATION oNotif = null;
                        if (oNotifs.Count() > 0)
                        {
                            oNotif = oNotifs.First();
                        }


                        if (oNotif != null)
                        {
                            oNotif.PNO_RETRIES++;
                            oNotif.PNO_LAST_RETRY_DATETIME = DateTime.UtcNow;

                            var oUserPushIds = (from r in dbContext.USERS_PUSH_IDs
                                                where r.UPID_PUSHID == oNotif.PNO_PUSHID &&
                                                  r.UPID_USR_ID == oNotif.USERS_NOTIFICATION.UNO_USR_ID
                                                select r);

                            if (string.IsNullOrEmpty(strNewPushId))
                            {
                                oNotif.PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.SubcriptionExpired);
                                if (oUserPushIds.Count() > 0)
                                {
                                    dbContext.USERS_PUSH_IDs.DeleteOnSubmit(oUserPushIds.First());
                                }

                            }
                            else
                            {
                                if (oUserPushIds.Count() > 0)
                                {
                                    oUserPushIds.First().UPID_PUSHID = strNewPushId;
                                    oUserPushIds.First().UPID_LAST_RETRY_DATETIME = null;
                                    oUserPushIds.First().UPID_PUSH_RETRIES = 0;
                                    oUserPushIds.First().UPID_LAST_SUCESSFUL_PUSH = null;

                                }



                                oNotif.PNO_LAST_RETRY_DATETIME = DateTime.UtcNow;
                                oNotif.PNO_STATUS = Convert.ToInt32(PushIdNotificationStatus.Waiting_Next_Retry);
                                oNotif.PNO_RETRIES++;
                                oNotif.PNO_PUSHID = strNewPushId;

                            }

                            SetUserNotificationStatus(oNotif.PNO_UTNO_ID, ref dbContext);


                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();


                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "PushIdExpired: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "PushIdExpired: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "PushIdExpired: ", e);
                bRes = false;
            }


            return bRes;


        }


        private bool SetUserNotificationStatus(decimal dUserNotificationID,ref integraMobileDBEntitiesDataContext dbContext)
        {
            bool bRes = true;


            try
            {
                var oPushNotifs = (from r in dbContext.PUSHID_NOTIFICATIONs
                                    where r.PNO_UTNO_ID == dUserNotificationID
                                    select r);

                int iNumSending = 0;
                int iNumSent = 0;
                int iNumFailed = 0;


                foreach (PUSHID_NOTIFICATION oPushNotif in oPushNotifs)
                {
                    switch ((PushIdNotificationStatus)oPushNotif.PNO_STATUS)
                    {
                        case PushIdNotificationStatus.Inserted:
                        case PushIdNotificationStatus.Sending:
                        case PushIdNotificationStatus.Waiting_Next_Retry:
                            iNumSending++;
                            break;
                        case PushIdNotificationStatus.Sent:
                            iNumSent++;
                            break;
                        case PushIdNotificationStatus.Failed:
                        case PushIdNotificationStatus.SubcriptionExpired:
                            iNumFailed++;
                            break;
                        default:
                            break;
                    }
                }

                UserNotificationStatus oNewStatus;

                if (iNumSending > 0)
                {
                    oNewStatus = UserNotificationStatus.Sending;
                }
                else
                {
                    if ((iNumFailed == 0) && (iNumSent > 0))
                    {
                        oNewStatus = UserNotificationStatus.Finished_Completely;
                    }
                    else
                    {
                        oNewStatus = UserNotificationStatus.Finished_Partially;
                    }
                }


                var oUserNotif = (from r in dbContext.USERS_NOTIFICATIONs
                                    where r.UNO_ID == dUserNotificationID
                                    select r).First();

                if (oNewStatus != (UserNotificationStatus)oUserNotif.UNO_STATUS)
                {
                    oUserNotif.UNO_STATUS = Convert.ToInt32(oNewStatus);

                    if ((oUserNotif.UNO_STATUS == (int)UserNotificationStatus.Finished_Partially) || (oUserNotif.UNO_STATUS == (int)UserNotificationStatus.Finished_Completely))
                    {
                        var oUserWarning = (from uw in dbContext.USERS_WARNINGs
                                            where (uw.UWA_UNO_ID.HasValue && (uw.UWA_UNO_ID == oUserNotif.UNO_ID) && uw.UWA_STATUS == (int)UserWarningStatus.CreatedUserNotification)
                                            select uw).FirstOrDefault();
                        if (oUserWarning != null)
                        {
                            oUserWarning.UWA_STATUS = (int)UserWarningStatus.Send;
                            SecureSubmitChanges(ref dbContext);
                        }
                    }
                   
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetUserNotificationStatus: ", e);
                bRes = false;
            }
                

            return bRes;


        }


        public bool GeneratePlatesSending()
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

                        var oDistinctURLsToSend = (from r in dbContext.INSTALLATIONs
                                                   where r.INS_PLATE_UPDATE_WS_URL != null
                                                   group r by new
                                                   {
                                                       r.INS_PLATE_UPDATE_WS_SIGNATURE_TYPE,
                                                       r.INS_PLATE_UPDATE_WS_URL
                                                   } into grp
                                                   select new
                                                   {
                                                       grp.Key.INS_PLATE_UPDATE_WS_SIGNATURE_TYPE,
                                                       grp.Key.INS_PLATE_UPDATE_WS_URL
                                                   });


                        var oPlatesPendingGeneration = (from r in dbContext.USER_PLATE_MOVs
                                                        where r.USRPM_SEND_INSERTION == 0
                                                        orderby r.USRPM_ID
                                                        select r);


                        foreach (var oURL in oDistinctURLsToSend)
                        {
                            decimal dINSId = (from r in dbContext.INSTALLATIONs
                                              where r.INS_PLATE_UPDATE_WS_URL == oURL.INS_PLATE_UPDATE_WS_URL &&
                                                    r.INS_PLATE_UPDATE_WS_SIGNATURE_TYPE == oURL.INS_PLATE_UPDATE_WS_SIGNATURE_TYPE
                                              orderby r.INS_ID
                                              select r.INS_ID).First();

                            foreach (USER_PLATE_MOV oPlate in oPlatesPendingGeneration)
                            {

                                dbContext.USER_PLATE_MOVS_SENDINGs.InsertOnSubmit(new USER_PLATE_MOVS_SENDING
                                    {
                                        USRPMS_INS_ID = dINSId,
                                        USRPMS_LAST_DATE = DateTime.UtcNow,
                                        USRPMS_USRPMD_ID = oPlate.USRPM_ID,
                                        USRPMS_STATUS = Convert.ToInt32(PlateMovSendingStatus.Inserted)
                                    });

                            }
                        }


                        foreach (USER_PLATE_MOV oPlate in oPlatesPendingGeneration)
                        {
                            oPlate.USRPM_SEND_INSERTION = 1;
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "GeneratePlatesSending: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GeneratePlatesSending: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GeneratePlatesSending: ", e);
                bRes = false;
            }

            return bRes;


        }

        public IEnumerable<USER_PLATE_MOVS_SENDING> GetPlatesForSending(int iMaxNumPlates)
        {
            IEnumerable<USER_PLATE_MOVS_SENDING> oPlateList=new List<USER_PLATE_MOVS_SENDING>();
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


                        var oInstallationsPendingSending = (from r in dbContext.USER_PLATE_MOVS_SENDINGs
                                                            where r.USRPMS_STATUS == Convert.ToInt32(PlateMovSendingStatus.Inserted) ||
                                                                  r.USRPMS_STATUS == Convert.ToInt32(PlateMovSendingStatus.Waiting_Next_Retry)
                                                            group r by r.USRPMS_INS_ID into grp
                                                            select new { insID = grp.Key, minMovId = grp.Min(m => m.USRPMS_ID) })
                                                            .OrderBy(r => r.minMovId);

                        decimal dInstallation = -1;
                        decimal dMovId = -1;
                        int iStatus = -1;
                        DateTime dtMinDate = DateTime.UtcNow;

                        foreach (var oIns in oInstallationsPendingSending)
                        {
                            var oMov = (from r in dbContext.USER_PLATE_MOVS_SENDINGs
                                        where r.USRPMS_INS_ID == oIns.insID &&
                                                r.USRPMS_ID == oIns.minMovId
                                        select r).First();

                            if (dMovId == -1)
                            {
                                dInstallation = oIns.insID;
                                dMovId = oIns.minMovId;
                                iStatus = oMov.USRPMS_STATUS;
                                dtMinDate = oMov.USRPMS_LAST_DATE;
                            }
                            else if ((oMov.USRPMS_STATUS == Convert.ToInt32(PlateMovSendingStatus.Inserted)) &&
                                    (iStatus == Convert.ToInt32(PlateMovSendingStatus.Waiting_Next_Retry)))
                            {
                                dInstallation = oIns.insID;
                                dMovId = oIns.minMovId;
                                iStatus = oMov.USRPMS_STATUS;
                                dtMinDate = oMov.USRPMS_LAST_DATE;

                            }
                            else if ((oMov.USRPMS_STATUS == Convert.ToInt32(PlateMovSendingStatus.Waiting_Next_Retry)) &&
                                    (iStatus == Convert.ToInt32(PlateMovSendingStatus.Waiting_Next_Retry)))
                            {
                                if (oMov.USRPMS_LAST_DATE < dtMinDate)
                                {
                                    dInstallation = oIns.insID;
                                    dMovId = oIns.minMovId;
                                    iStatus = oMov.USRPMS_STATUS;
                                    dtMinDate = oMov.USRPMS_LAST_DATE;
                                }
                            }


                            if (iStatus == Convert.ToInt32(PlateMovSendingStatus.Inserted))
                            {
                                break;
                            }

                        }


                        var oMovsPendingSending = (from r in dbContext.USER_PLATE_MOVS_SENDINGs
                                                   where (r.USRPMS_STATUS == Convert.ToInt32(PlateMovSendingStatus.Inserted) ||
                                                          r.USRPMS_STATUS == Convert.ToInt32(PlateMovSendingStatus.Waiting_Next_Retry)) &&
                                                          r.USRPMS_INS_ID == dInstallation &&
                                                          r.USRPMS_ID >= dMovId
                                                   orderby r.USRPMS_ID
                                                   select r);


                        foreach (USER_PLATE_MOVS_SENDING oMov in oMovsPendingSending)
                        {
                            ((List<USER_PLATE_MOVS_SENDING>)oPlateList).Add(oMov);
                            oMov.USRPMS_STATUS = Convert.ToInt32(PlateMovSendingStatus.Sending);
                            if (oPlateList.Count() == iMaxNumPlates)
                            {
                                break;
                            }
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "GetPlatesForSending: ", e);
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetPlatesForSending: ", e);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPlatesForSending: ", e);
            }


            return oPlateList;

        }


        public bool ErrorSedingPlates(IEnumerable<USER_PLATE_MOVS_SENDING> oPlateList)
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

                        var oPlatesInTheList = (from r in dbContext.USER_PLATE_MOVS_SENDINGs
                                                where oPlateList.Contains(r)
                                                select r);



                        foreach (USER_PLATE_MOVS_SENDING oPlate in oPlatesInTheList)
                        {
                            oPlate.USRPMS_STATUS = Convert.ToInt32(PlateMovSendingStatus.Waiting_Next_Retry);
                            oPlate.USRPMS_LAST_DATE = DateTime.UtcNow;
                        }


                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "ErrorSedingPlates: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "ErrorSedingPlates: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ErrorSedingPlates: ", e);
                bRes = false;
            }


            return bRes;


        }



        public bool ConfirmSentPlates(IEnumerable<USER_PLATE_MOVS_SENDING> oPlateList)
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


                        var oPlatesInTheList = (from r in dbContext.USER_PLATE_MOVS_SENDINGs
                                                where oPlateList.Contains(r)
                                                select r);



                        foreach (USER_PLATE_MOVS_SENDING oPlate in oPlatesInTheList)
                        {
                            oPlate.USRPMS_STATUS = Convert.ToInt32(PlateMovSendingStatus.Sent);
                            oPlate.USRPMS_LAST_DATE = DateTime.UtcNow;
                        }

                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "ConfirmSentPlates: ", e);
                            bRes = false;
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "ConfirmSentPlates: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ConfirmSentPlates: ", e);
                bRes = false;
            }

            return bRes;


        }        



        public bool ExistPlateInSystem(string strPlate)
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
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    var plates = (from r in dbContext.USER_PLATEs
                                  where r.USRP_ENABLED == 1 && r.USRP_PLATE == strPlate.ToUpper().Trim().Replace(" ", "") && r.USER.USR_ENABLED == 1
                                  select r);

                    if (plates.Count() > 0)
                    {
                        bRes = true;
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ExistPlateInSystem: ", e);
                bRes = false;
            }

            return bRes;
        }



        public bool AddExternalPlateFine(decimal dInstallation,
                                  string strPlate,
                                  DateTime dtTicket,
                                  DateTime dtTicketUTC,
                                  string strFineNumber,
                                  int iQuantity,
                                  DateTime dtLimit,
                                  DateTime dtLimitUTC,
                                  string strArticleType,
                                  string strArticleDescription)
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

                        var oExternalFines = (from r in dbContext.EXTERNAL_TICKETs
                                              where r.EXTI_INS_ID == dInstallation &&
                                                    r.EXTI_PLATE == strPlate &&
                                                    r.EXTI_DATE == dtTicket
                                              select r);

                        if (oExternalFines.Count() > 0)
                        {
                            bRes = true;
                        }
                        else
                        {
                            dbContext.EXTERNAL_TICKETs.InsertOnSubmit(new EXTERNAL_TICKET
                                {
                                    EXTI_INS_ID = dInstallation,
                                    EXTI_PLATE = strPlate,
                                    EXTI_DATE = dtTicket,
                                    EXTI_DATE_UTC = dtTicketUTC,
                                    EXTI_LIMIT_DATE = dtLimit,
                                    EXTI_LIMIT_DATE_UTC = dtLimitUTC,
                                    EXTI_TICKET_NUMBER = strFineNumber,
                                    EXTI_AMOUNT = iQuantity,
                                    EXTI_ARTICLE_TYPE = strArticleType,
                                    EXTI_ARTICLE_DESCRIPTION = strArticleDescription
                                });


                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                                bRes = true;

                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "AddExternalPlateFine: ", e);
                                bRes = false;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddExternalPlateFine: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddExternalPlateFine: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool AddExternalPlateParking(decimal dInstallation,
                                  string strPlate,
                                  DateTime dtDate,
                                  DateTime dtDateUTC,
                                  DateTime dtEndDate,
                                  DateTime dtEndDateUTC,
                                  decimal? dGroup,
                                  decimal? dTariff,
                                  DateTime? dtIniDate,
                                  DateTime? dtIniDateUTC,
                                  int? iQuantity,
                                  int? iTime,
                                  decimal dExternalProvider,
                                  OperationSourceType operationSourceType,
                                  string strSourceIdent,
                                  ChargeOperationsType chargeType,
                                  string strOperationId1, string strOperationId2,
                                  out decimal dOperationId)
        {
            bool bRes = false;
            dOperationId = 0;
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

                        var oExternalParkings = (from r in dbContext.EXTERNAL_PARKING_OPERATIONs
                                                 where r.EPO_INS_ID == dInstallation &&
                                                       r.EPO_PLATE == strPlate &&
                                                       ((r.EPO_DATE != null && r.EPO_DATE == dtDate) || (r.EPO_DATE == null && r.EPO_ENDDATE == dtEndDate))
                                                 select r);

                        if (oExternalParkings.Count() > 0)
                        {
                            bRes = true;
                            dOperationId = oExternalParkings.First().EPO_ID;
                        }
                        else
                        {
                            EXTERNAL_PARKING_OPERATION oExternalOperation = new EXTERNAL_PARKING_OPERATION()
                                {
                                    EPO_INS_ID = dInstallation,
                                    EPO_PLATE = strPlate,
                                    EPO_DATE = dtDate,
                                    EPO_DATE_UTC = dtDateUTC,
                                    EPO_ENDDATE = dtEndDate,
                                    EPO_ENDDATE_UTC = dtEndDateUTC,
                                    EPO_ZONE = dGroup,
                                    EPO_TARIFF = dTariff,
                                    EPO_INIDATE = dtIniDate,
                                    EPO_INIDATE_UTC = dtIniDateUTC,
                                    EPO_AMOUNT = iQuantity,
                                    EPO_TIME = iTime,
                                    EPO_EXP_ID = dExternalProvider,
                                    EPO_SRCTYPE = (int)operationSourceType,
                                    EPO_SRCIDENT = strSourceIdent,
                                    EPO_TYPE = (int)chargeType,
                                    EPO_INSERTION_UTC_DATE = DateTime.UtcNow,                                    
                                    EPO_DATE_UTC_OFFSET = Convert.ToInt32((dtDateUTC - dtDate).TotalMinutes),
                                    EPO_INIDATE_UTC_OFFSET = (dtIniDateUTC.HasValue && dtIniDate.HasValue?Convert.ToInt32((dtIniDateUTC.Value - dtIniDate.Value).TotalMinutes):0),
                                    EPO_ENDDATE_UTC_OFFSET = Convert.ToInt32((dtEndDateUTC - dtEndDate).TotalMinutes),
                                    EPO_OPERATION_ID1 = strOperationId1,
                                    EPO_OPERATION_ID2 = strOperationId2
                                };

                            dbContext.EXTERNAL_PARKING_OPERATIONs.InsertOnSubmit(oExternalOperation);


                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                                dOperationId = oExternalOperation.EPO_ID;

                                bRes = true;

                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "AddExternalPlateParking: ", e);
                                bRes = false;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddExternalPlateParking: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddExternalPlateParking: ", e);
                bRes = false;
            }

            return bRes;
        }


        public bool GetInsertionTicketNotificationData(out EXTERNAL_TICKET oTicket, out int iQueueLengthExternalTicket, List<decimal> oListRunningExternalTicket)
        {
            bool bRes = true;
            oTicket = null;
            iQueueLengthExternalTicket = 0;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                    var predicate = PredicateBuilder.True<EXTERNAL_TICKET>();

                    if ((oListRunningExternalTicket != null) && (oListRunningExternalTicket.Count() > 0))
                    {
                        foreach (decimal ExTiId in oListRunningExternalTicket)
                        {
                            predicate = predicate.And(a => a.EXTI_ID != ExTiId);
                        }
                    }


                    var oExternalTickets = (from r in dbContext.EXTERNAL_TICKETs
                                            where r.EXTI_INSERTION_NOTIFIED == 0
                                            orderby r.EXTI_ID
                                            select r).Where(predicate);

                    if (oExternalTickets.Count() > 0)
                    {
                        iQueueLengthExternalTicket = oExternalTickets.Count();
                        oTicket = oExternalTickets.First();
                    }
                    else
                    {
                        dbContext.Close();
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInsertionTicketNotificationData: ", e);
                bRes = false;
            }

            return bRes;
        }



        public bool GetInsertionUserSecurityDataNotificationData(out USERS_SECURITY_OPERATION oSecurityOperation, out int iQueueLengthoUsersSecurityOperationNotification, List<decimal> oListRunningUsersSecurityOperationNotification)
        {
            bool bRes = true;
            oSecurityOperation = null;
            iQueueLengthoUsersSecurityOperationNotification = 0;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                    var predicate = PredicateBuilder.True<USERS_SECURITY_OPERATION>();

                    if ((oListRunningUsersSecurityOperationNotification != null) && (oListRunningUsersSecurityOperationNotification.Count() > 0))
                    {
                        foreach (decimal UserSercOpe in oListRunningUsersSecurityOperationNotification)
                        {
                            predicate = predicate.And(a => a.USOP_ID != UserSercOpe);
                        }
                    }

                    var oSecurityOperations = (from r in dbContext.USERS_SECURITY_OPERATIONs
                                            where r.USOP_SEND_BY_PUSH == 1 && r.USOP_UNO_ID == null
                                            orderby r.USOP_ID
                                            select r).Where(predicate);

                    if (oSecurityOperations.Count() > 0)
                    {
                        iQueueLengthoUsersSecurityOperationNotification = oSecurityOperations.Count(); 
                        oSecurityOperation = oSecurityOperations.First();
                    }
                    else
                    {
                        dbContext.Close();
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInsertionUserSecurityDataNotificationData: ", e);
                bRes = false;
            }

            return bRes;
        }



        public bool GetInsertionParkingNotificationData(out EXTERNAL_PARKING_OPERATION oParking, out int iQueueLengthInsertionParkingNotification, List<decimal> oListRunningInsertionParkingNotification)
        {
            bool bRes = true;
            oParking = null;
            iQueueLengthInsertionParkingNotification = 0;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext =  new integraMobileDBEntitiesDataContext();

                    var predicate = PredicateBuilder.True<EXTERNAL_PARKING_OPERATION>();

                    if ((oListRunningInsertionParkingNotification != null) && (oListRunningInsertionParkingNotification.Count() > 0))
                    {
                        foreach (decimal ExParkOpe in oListRunningInsertionParkingNotification)
                        {
                            predicate = predicate.And(a => a.EPO_ID != ExParkOpe);
                        }
                    }

                    var oExternalParkings = (from r in dbContext.EXTERNAL_PARKING_OPERATIONs
                                             where r.EPO_INSERTION_NOTIFIED == 0
                                             orderby r.EPO_ID
                                             select r).Where(predicate);

                    if (oExternalParkings.Count() > 0)
                    {
                        iQueueLengthInsertionParkingNotification = oExternalParkings.Count();
                        oParking = oExternalParkings.First();
                    }
                    else
                    {
                        dbContext.Close();
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInsertionParkingNotificationData: ", e);
                bRes = false;
            }

            return bRes;
        }        
        
        
        public bool GetBeforeEndParkingNotificationData(int iNumMinutesBeforeEndToWarn, out EXTERNAL_PARKING_OPERATION oParking, out int iQueueLengthBeforeEndParkingNotification, List<decimal> oListRunningBeforeEndParkingNotification)
        {
            bool bRes = true;
            oParking = null;
            iQueueLengthBeforeEndParkingNotification=0;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext(); ;

                    var predicate = PredicateBuilder.True<EXTERNAL_PARKING_OPERATION>();

                    if ((oListRunningBeforeEndParkingNotification != null) && (oListRunningBeforeEndParkingNotification.Count() > 0))
                    {
                        foreach (decimal ExParkOpeBeforeEnd in oListRunningBeforeEndParkingNotification)
                        {
                            predicate = predicate.And(a => a.EPO_ID != ExParkOpeBeforeEnd);
                        }
                    }


                    TimeSpan ts = new TimeSpan(0, iNumMinutesBeforeEndToWarn, 0);

                    var oExternalParkings = (from r in dbContext.EXTERNAL_PARKING_OPERATIONs
                                             where r.EPO_ENDING_NOTIFIED == 0 &&
                                                   (r.EPO_ENDDATE_UTC - DateTime.UtcNow) < ts
                                             orderby r.EPO_ENDDATE_UTC
                                             select r).Where(predicate);

                    if (oExternalParkings.Count() > 0)
                    {
                        iQueueLengthBeforeEndParkingNotification = oExternalParkings.Count();
                        oParking = oExternalParkings.First();
                    }
                    else
                    {
                        dbContext.Close();
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetBeforeEndParkingNotificationData: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool GetOffstreetOperationNotificationData(out OPERATIONS_OFFSTREET oOperation, out int iQueueLengthOperationOffStreetNotification, List<decimal> oListRunningOperationOffStreetNotification)
        {
            bool bRes = true;
            oOperation = null;
            iQueueLengthOperationOffStreetNotification = 0;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                    var predicate = PredicateBuilder.True<OPERATIONS_OFFSTREET>();

                    if ((oListRunningOperationOffStreetNotification != null) && (oListRunningOperationOffStreetNotification.Count() > 0))
                    {
                        foreach (decimal OpeOffStreet in oListRunningOperationOffStreetNotification)
                        {
                            predicate = predicate.And(a => a.OPEOFF_ID != OpeOffStreet);
                        }
                    }

                    var oOperations = (from r in dbContext.OPERATIONS_OFFSTREETs
                                       where r.OPEOFF_MUST_NOTIFY == 1 && r.OPEOFF_NOTIFIED == 0
                                             orderby r.OPEOFF_EXIT_DATE
                                             select r).Where(predicate);

                    if (oOperations.Count() > 0)
                    {
                        iQueueLengthOperationOffStreetNotification = oOperations.Count();
                        oOperation = oOperations.First();
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetOffstreetOperationNotificationData: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool MarkAsGeneratedInsertionTicketNotification(EXTERNAL_TICKET oTicket)
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
                        integraMobileDBEntitiesDataContext dbContext =new integraMobileDBEntitiesDataContext();

                        var oFoundTicket = (from r in dbContext.EXTERNAL_TICKETs
                                            where r.EXTI_ID == oTicket.EXTI_ID
                                            select r).First();


                        oFoundTicket.EXTI_INSERTION_NOTIFIED = 1;
                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();
                            bRes = true;

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedInsertionTicketNotification: ", e);
                            bRes = false;
                        }


                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedInsertionTicketNotification: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedInsertionTicketNotification: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool MarkAsGeneratedInsertionParkingNotificationData(EXTERNAL_PARKING_OPERATION oParking)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                        var oFoundParking = (from r in dbContext.EXTERNAL_PARKING_OPERATIONs
                                             where r.EPO_ID == oParking.EPO_ID
                                             select r).First();


                        oFoundParking.EPO_INSERTION_NOTIFIED = 1;
                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();

                            bRes = true;

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedInsertionParkingNotificationData: ", e);
                            bRes = false;
                        }


                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedInsertionParkingNotificationData: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedInsertionParkingNotificationData: ", e);
                bRes = false;
            }

            return bRes;
        }


        public bool MarkAsGeneratedBeforeEndParkingNotificationData(EXTERNAL_PARKING_OPERATION oParking)
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

                        var oFoundParking = (from r in dbContext.EXTERNAL_PARKING_OPERATIONs
                                             where r.EPO_ID == oParking.EPO_ID
                                             select r).First();


                        oFoundParking.EPO_ENDING_NOTIFIED = 1;
                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                            dbContext.Close();
                            bRes = true;

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedBeforeEndParkingNotificationData: ", e);
                            bRes = false;
                        }


                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedBeforeEndParkingNotificationData: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedBeforeEndParkingNotificationData: ", e);
                bRes = false;
            }
            
            return bRes;
        }

        public bool MarkAsGeneratedOffstreetOperationNotificationData(OPERATIONS_OFFSTREET oOperation)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                        var oFoundOperation = (from r in dbContext.OPERATIONS_OFFSTREETs
                                               where r.OPEOFF_ID == oOperation.OPEOFF_ID
                                               select r).First();

                        oFoundOperation.OPEOFF_NOTIFIED = 1;
                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();

                            bRes = true;

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedOffstreetOperationNotificationData: ", e);
                            bRes = false;
                        }


                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedOffstreetOperationNotificationData: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedOffstreetOperationNotificationData: ", e);
                bRes = false;
            }

            return bRes;
        }


        public bool MarkAsGeneratedUserSecurityDataNotificationData(USERS_SECURITY_OPERATION oSecurityOperation, decimal oNotifID)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                        var oFoundOperation = (from r in dbContext.USERS_SECURITY_OPERATIONs
                                               where r.USOP_ID == oSecurityOperation.USOP_ID
                                               select r).First();

                        oFoundOperation.USERS_NOTIFICATION = (from r in dbContext.USERS_NOTIFICATIONs
                                                              where r.UNO_ID == oNotifID
                                                              select r).First();
                        // Submit the change to the database.
                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();

                            bRes = true;

                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedUserSecurityDataNotificationData: ", e);
                            bRes = false;
                        }


                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedUserSecurityDataNotificationData: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MarkAsGeneratedUserSecurityDataNotificationData: ", e);
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


        private static string RemoveNotNumericCharacters(string str)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                    sb.Append(str[i]);
            }

            return sb.ToString();
        }

        public string GetLiteral(decimal literalId, string langCulture)
        {
            string sRes = "";
            
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

                    var oLiterals = (from r in getLITERALs()
                                     where r.LIT_ID == literalId 
                                     select r);

                    if (oLiterals.Count() > 0)
                    {
                        var oLanguage = (from r in getLANGUAGEs()
                                         where r.LAN_CULTURE == langCulture
                                         select r).FirstOrDefault();

                        var oLiteral = oLiterals.First();
                        var oLiteralsLang = getLITERAL_LANGUAGEs().Where(l => l.LITL_LIT_ID== literalId && l.LITL_LAN_ID == oLanguage.LAN_ID);
                        if (oLiteralsLang.Count() > 0)
                        {
                            sRes = oLiteralsLang.First().LITL_LITERAL;
                        }
                        else
                        {
                            sRes = oLiteral.LIT_DESCRIPTION;
                        }
                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLiteral: ", e);
                sRes = "";
            }

            return sRes;

        }

        public string GetLiteralFromKey(string literalKey, string langCulture)
        {
            string sRes = "";

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

                    var oLiterals = (from r in dbContext.LITERALs
                                     where r.LIT_KEY == literalKey
                                     select r);

                    if (oLiterals.Count() > 0)
                    {
                        var oLiteral = oLiterals.First();
                        var oLiteralsLang = oLiteral.LITERAL_LANGUAGEs.Where(l => l.LANGUAGE.LAN_CULTURE == langCulture);
                        if (oLiteralsLang.Count() > 0)
                        {
                            sRes = oLiteralsLang.First().LITL_LITERAL;
                        }
                        else
                        {
                            sRes = oLiteral.LIT_DESCRIPTION;
                        }
                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLiteralFromKey: ", e);
                sRes = "";
            }

            return sRes;

        }

        public bool getCarrouselVersion(int iVersion, int iLang, decimal dSourceApp, out CARROUSEL_SCREEN_VERSION oCarrouselVersion)
        {
            bool bRes = false;
            oCarrouselVersion = null;

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

                
                       oCarrouselVersion = (from r in dbContext.CARROUSEL_SCREEN_VERSIONs
                                                    where r.CASCV_VERSION_NUMBER > iVersion &&
                                                        r.CASCV_LANG == iLang && r.CASCV_SOAPP_ID==dSourceApp
                                                    orderby r.CASCV_VERSION_NUMBER descending
                                                    select r).FirstOrDefault();
                       bRes=true;
                    

                    
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getCarrouselVersion: ", e);
            }

            return bRes;


        }


        public bool GetCreditCallConfiguration(string Guid, out CREDIT_CALL_CONFIGURATION oCreditCallConfiguration)
        {
            bool bRes = false;
            oCreditCallConfiguration = null;

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


                    oCreditCallConfiguration = (from r in dbContext.CREDIT_CALL_CONFIGURATIONs
                                            where r.CCCON_GUID == Guid
                                            select r).FirstOrDefault();
                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCreditCallConfiguration: ", e);
            }

            return bRes;

        }



        public bool GetMonerisConfiguration(string Guid, out MONERIS_CONFIGURATION oMonerisConfiguration)
        {
            bool bRes = false;
            oMonerisConfiguration = null;

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


                    oMonerisConfiguration = (from r in dbContext.MONERIS_CONFIGURATIONs
                                             where r.MONCON_GUID == Guid
                                             select r).FirstOrDefault();

                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetMonerisConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetMonerisConfigurationById(decimal? id, out MONERIS_CONFIGURATION oMonerisConfiguration)
        {
            bool bRes = false;
            oMonerisConfiguration = null;

            if (id == null)
            {
                return bRes;
            }

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

                    oMonerisConfiguration = (from r in dbContext.MONERIS_CONFIGURATIONs
                                             where r.MONCON_ID == id
                                             select r).FirstOrDefault();
                    bRes = true;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetMonerisConfiguration: ", e);
            }
            return bRes;
        }


        public bool AddMoneris3DSTransaction(decimal dMonerisConfiguration, string strMD, string strEmail, int iAmount, DateTime utcDate, string strInlineForm, out decimal? dTransId)
        {
            bool bRes = false;
            dTransId = null;
            
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

                    MONERIS_3DS_TRANSACTION_INFO oTransactionInfo = new MONERIS_3DS_TRANSACTION_INFO
                                {
                                  MON3IN_MD = strMD,
                                  MON3IN_EMAIL = strEmail,
                                  MON3IN_AMOUNT = iAmount,
                                  MON3IN_CREATION_UTC_DATE = utcDate,
                                  MON3IN_INLINE_FORM = strInlineForm,
                                  MON3IN_MONCON_ID = dMonerisConfiguration
                                };



                    dbContext.MONERIS_3DS_TRANSACTION_INFOs.InsertOnSubmit(oTransactionInfo);


                    // Submit the change to the database.
                    try
                    {
                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();
                        dTransId = oTransactionInfo.MON3IN_ID;
                        bRes = true;

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddMoneris3DSTransaction: ", e);
                        bRes = false;
                    }                       
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddMoneris3DSTransaction: ", e);
            }
            return bRes;
        }




        public bool GetMoneris3DSTransactionInlineForm(decimal dId, string strMD, string strEmail, out string strInlineForm)
        {
            bool bRes = false;
            strInlineForm = "";

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


                    var oTransaction = (from r in dbContext.MONERIS_3DS_TRANSACTION_INFOs
                                        where r.MON3IN_ID == dId &&
                                               r.MON3IN_MD == strMD &&
                                               r.MON3IN_EMAIL == strEmail
                                        select r).FirstOrDefault();

                    if (oTransaction != null)
                    {
                        strInlineForm = oTransaction.MON3IN_INLINE_FORM;
                        bRes = true;
                    }


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetMoneris3DSTransactionInlineForm: ", e);
            }
            return bRes;
        }


        public bool UpdateMoneris3DSTransaction(string strMD, string strEmail, string strCAVV, string strECI, DateTime utcdate)
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
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();


                    var oTransaction = (from r in dbContext.MONERIS_3DS_TRANSACTION_INFOs
                                        where  r.MON3IN_MD == strMD &&
                                               r.MON3IN_EMAIL == strEmail
                                        select r).FirstOrDefault();

                    if (oTransaction != null)
                    {
                        oTransaction.MON3IN_CAVV = strCAVV;
                        oTransaction.MON3IN_ECI = strECI;
                        oTransaction.MON3IN_RESPONSE_UTC_DATE = utcdate;                       
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
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateMoneris3DSTransaction: ", e);
                        bRes = false;
                    }      


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateMoneris3DSTransaction: ", e);
            }
            return bRes;
        }

        public bool GetTransBankConfiguration(string Guid, out TRANSBANK_CONFIGURATION oTransbankConfiguration)
        {
            bool bRes = false;
            oTransbankConfiguration = null;

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


                    oTransbankConfiguration = (from r in dbContext.TRANSBANK_CONFIGURATIONs
                                               where r.TRBACON_GUID == Guid
                                               select r).FirstOrDefault();



                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetMonerisConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetTransBankConfigurationById(decimal? id, out TRANSBANK_CONFIGURATION oTransbankConfiguration)
        {
            bool bRes = false;
            oTransbankConfiguration = null;

            if (id == null)
            {
                return bRes;
            }

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

                    oTransbankConfiguration = (from r in dbContext.TRANSBANK_CONFIGURATIONs
                                               where r.TRBACON_ID == id
                                               select r).FirstOrDefault();
                    bRes = true;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetMonerisConfiguration: ", e);
            }
            return bRes;
        }

        public bool GetPayuConfiguration(string Guid, out PAYU_CONFIGURATION oPayuConfiguration)
        {
            bool bRes = false;
            oPayuConfiguration = null;

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


                    oPayuConfiguration = (from r in dbContext.PAYU_CONFIGURATIONs
                                          where r.PAYUCON_GUID == Guid
                                          select r).FirstOrDefault();

                    /* oPayuConfiguration = new PAYU_CONFIGURATION()
                        {
                            PAYUCON_ID=1,
                            PAYUCON_GUID = "268bc35d-dc43-43f1-a39a-20e3139c8042",
                            PAYUCON_TOKEN_URL = "https://sandbox.api.payulatam.com/payments-api/4.0/service",
                            PAYUCON_API_URL = "https://sandbox.api.payulatam.com/payments-api/4.0/service.cgi",
                            PAYUCON_API_KEY = "4Vj8eK4rloUd272L48hsrarnUA",
                            PAYUCON_API_LOGIN = "pRRXKOl8ikMmt9u",
                            PAYUCON_PUBLIC_KEY = "PKaC6H4cEDJD919n705L544kSU",
                            PAYUCON_ACCOUNT_ID = "512324",
                            PAYUCON_MERCHANT_ID = "508029",
                            PAYUCON_IS_TEST = 1,
                            PAYUCON_COUNTRY = "MX",
                            PAYUCON_CONFIRMATION_TIME = 300,
                            PAYUCON_CHECK_DATE_AND_HASH = 0,
                            PAYUCON_SERVICE_TIMEOUT = 10000,  
                            PAYUCON_HASH_SEED = "h@qu9(m&/HWdJ43@mE)y/Mn"
                        };*/




                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPayuConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetPayuConfigurationById(decimal? id, out PAYU_CONFIGURATION oPayuConfiguration)
        {
            bool bRes = false;
            oPayuConfiguration = null;

            if (id == null)
            {
                return bRes;
            }

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

                    oPayuConfiguration = (from r in dbContext.PAYU_CONFIGURATIONs
                                          where r.PAYUCON_ID == id
                                          select r).FirstOrDefault();
                    bRes = true;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPayuConfiguration: ", e);
            }
            return bRes;
        }

        public bool GetStripeConfiguration(string Guid, out STRIPE_CONFIGURATION oStripeConfiguration )
        {
            bool bRes = false;
            oStripeConfiguration = null;

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


                    oStripeConfiguration = (from r in dbContext.STRIPE_CONFIGURATIONs
                                                    where r.STRCON_GUID == Guid
                                                    select r).FirstOrDefault();
                    bRes=true;
                    

                    
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetStripeConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetIECISAConfiguration(string Guid, out IECISA_CONFIGURATION oiecisaConfiguration)
        {
            bool bRes = false;
            oiecisaConfiguration = null;

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


                    oiecisaConfiguration = (from r in dbContext.IECISA_CONFIGURATIONs
                                            where r.IECCON_GUID == Guid
                                            select r).FirstOrDefault();
                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetIECISAConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetBSRedsysConfiguration(string Guid, out BSREDSYS_CONFIGURATION oConfiguration)
        {
            bool bRes = false;
            oConfiguration = null;

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


                    oConfiguration = (from r in dbContext.BSREDSYS_CONFIGURATIONs
                                             where r.BSRCON_GUID == Guid
                                             select r).FirstOrDefault();

                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetBSRedsysConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetBSRedsysConfigurationById(decimal? id, out BSREDSYS_CONFIGURATION oConfiguration)
        {
            bool bRes = false;
            oConfiguration = null;

            if (id == null)
            {
                return bRes;
            }

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

                    oConfiguration = (from r in dbContext.BSREDSYS_CONFIGURATIONs
                                             where r.BSRCON_ID == id
                                             select r).FirstOrDefault();
                    bRes = true;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetBSRedsysConfigurationById: ", e);
            }
            return bRes;
        }


        public bool GetBSRedsys3DSTransactionInlineForm(decimal dId, string strTransID, string strEmail, out string strInlineForm)
        {
            bool bRes = false;
            strInlineForm = "";

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


                    var oTransaction = (from r in dbContext.BSREDSYS_3DS_TRANSACTION_INFOs
                                        where r.BSR3IN_ID == dId &&
                                               r.BSR3IN_3DS_TRANS_ID == strTransID &&
                                               r.BSR3IN_EMAIL == strEmail
                                        select r).FirstOrDefault();

                    if (oTransaction != null)
                    {
                        if (oTransaction.BSR3IN_NUM_INLINE_FORMS == 1)
                        {
                            strInlineForm = oTransaction.BSR3IN_INLINE_FORM1;
                        }
                        else
                        {
                            strInlineForm = oTransaction.BSR3IN_INLINE_FORM2;
                        }
                        bRes = true;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetBSRedsys3DSTransactionInlineForm: ", e);
            }
            return bRes;
        }

        public bool GetBSRedsys3DSTransactionEmail(string strTransID, out string strEmail)
        {
            bool bRes = false;
            strEmail = "";

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


                    var oTransaction = (from r in dbContext.BSREDSYS_3DS_TRANSACTION_INFOs
                                        where  r.BSR3IN_3DS_TRANS_ID == strTransID 
                                        select r).FirstOrDefault();

                    if (oTransaction != null)
                    {
                        strEmail = oTransaction.BSR3IN_EMAIL;
                        bRes = true;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetBSRedsys3DSTransactionEmail: ", e);
            }
            return bRes;
        }



        public bool AddBSRedsys3DSTransaction(decimal dBSRedsysConfiguration, string strTransId, string strOrderId, string strEmail, int iAmount, DateTime utcDate, string strInlineForm, string strProtocolVersion, out decimal? dTransId)
        {
            bool bRes = false;
            dTransId = null;

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

                    BSREDSYS_3DS_TRANSACTION_INFO oTransactionInfo = new BSREDSYS_3DS_TRANSACTION_INFO
                    {

                        BSR3IN_3DS_TRANS_ID = strTransId,
                        BSR3IN_ORDER_ID = strOrderId,
                        BSR3IN_EMAIL = strEmail,
                        BSR3IN_AMOUNT = iAmount,
                        BSR3IN_CREATION_UTC_DATE = utcDate,
                        BSR3IN_INLINE_FORM1 = strInlineForm,
                        BSR3IN_NUM_INLINE_FORMS = 1,
                        BSR3IN_BSRCON_ID = dBSRedsysConfiguration,
                        BSR3IN_PROTOCOL_VERSION = strProtocolVersion
                    };



                    dbContext.BSREDSYS_3DS_TRANSACTION_INFOs.InsertOnSubmit(oTransactionInfo);


                    // Submit the change to the database.
                    try
                    {
                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();
                        dTransId = oTransactionInfo.BSR3IN_ID;
                        bRes = true;

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddBSRedsys3DSTransaction: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddBSRedsys3DSTransaction: ", e);
            }
            return bRes;
        }




        public bool UpdateBSRedsys3DSTransaction(string TransId, string strEmail, string strBSRedsys3DSPares, string strBSRedsys3DSCres, DateTime utcdate, out string strOrderId, out string strProtocolVersion,out int? iBSRedsysNumInlineForms)
        {
            bool bRes = false;
            strOrderId = null;
            strProtocolVersion = null;
            iBSRedsysNumInlineForms = 1;

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


                    var oTransaction = (from r in dbContext.BSREDSYS_3DS_TRANSACTION_INFOs
                                        where r.BSR3IN_3DS_TRANS_ID == TransId &&
                                               r.BSR3IN_EMAIL == strEmail
                                        select r).FirstOrDefault();

                    if (oTransaction != null)
                    {
                        oTransaction.BSR3IN_PARES = strBSRedsys3DSPares;
                        oTransaction.BSR3IN_CRES = strBSRedsys3DSCres;
                        oTransaction.BSR3IN_RESPONSE_UTC_DATE = utcdate;
                        strOrderId = oTransaction.BSR3IN_ORDER_ID;
                        strProtocolVersion = oTransaction.BSR3IN_PROTOCOL_VERSION;
                        iBSRedsysNumInlineForms = oTransaction.BSR3IN_NUM_INLINE_FORMS;
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
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateBSRedsys3DSTransaction: ", e);
                        bRes = false;
                    }


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateBSRedsys3DSTransaction: ", e);
            }
            return bRes;
        }


        public bool UpdateBSRedsys3DSTransaction(string TransId, string strEmail, string strInlineForm, DateTime utcdate, out decimal? dTransId)
        {
            bool bRes = false;
            dTransId = null;

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


                    var oTransaction = (from r in dbContext.BSREDSYS_3DS_TRANSACTION_INFOs
                                        where r.BSR3IN_3DS_TRANS_ID == TransId &&
                                               r.BSR3IN_EMAIL == strEmail
                                        select r).FirstOrDefault();

                    if (oTransaction != null)
                    {
                        if (oTransaction.BSR3IN_NUM_INLINE_FORMS == 0)
                        {
                            oTransaction.BSR3IN_INLINE_FORM1 = strInlineForm;
                        }
                        else
                        {
                            oTransaction.BSR3IN_INLINE_FORM2 = strInlineForm;
                        }
                        oTransaction.BSR3IN_NUM_INLINE_FORMS++;
                        oTransaction.BSR3IN_RESPONSE_UTC_DATE = utcdate;
                        dTransId = oTransaction.BSR3IN_ID;
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
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateBSRedsys3DSTransaction: ", e);
                        bRes = false;
                    }


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateBSRedsys3DSTransaction: ", e);
            }
            return bRes;
        }


        public bool UpdateBSRedsys3DSTransaction(string TransId, string strEmail, string strProtocolVersion, DateTime utcdate)
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
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();


                    var oTransaction = (from r in dbContext.BSREDSYS_3DS_TRANSACTION_INFOs
                                        where r.BSR3IN_3DS_TRANS_ID == TransId &&
                                               r.BSR3IN_EMAIL == strEmail
                                        select r).FirstOrDefault();

                    if (oTransaction != null)
                    {
                        oTransaction.BSR3IN_PROTOCOL_VERSION = strProtocolVersion;
                        oTransaction.BSR3IN_RESPONSE_UTC_DATE = utcdate;
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
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateBSRedsys3DSTransaction: ", e);
                        bRes = false;
                    }


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateBSRedsys3DSTransaction: ", e);
            }
            return bRes;
        }


        public bool GetPaysafeConfiguration(string Guid, out PAYSAFE_CONFIGURATION oConfiguration)
        {
            bool bRes = false;
            oConfiguration = null;

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


                    oConfiguration = (from r in dbContext.PAYSAFE_CONFIGURATIONs
                                      where r.PYSCON_GUID == Guid
                                      select r).FirstOrDefault();

                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPaysafeConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetPaysafeConfigurationById(decimal? id, out PAYSAFE_CONFIGURATION oConfiguration)
        {
            bool bRes = false;
            oConfiguration = null;

            if (id == null)
            {
                return bRes;
            }

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

                    oConfiguration = (from r in dbContext.PAYSAFE_CONFIGURATIONs
                                      where r.PYSCON_ID == id
                                      select r).FirstOrDefault();
                    bRes = true;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPaysafeConfigurationById: ", e);
            }
            return bRes;
        }


        public bool GetPaypalConfiguration(string Guid, out PAYPAL_CONFIGURATION oPaypalConfiguration)
        {
            bool bRes = false;
            oPaypalConfiguration = null;

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


                    oPaypalConfiguration = (from r in dbContext.PAYPAL_CONFIGURATIONs
                                            where r.PPCON_GUID == Guid
                                             select r).FirstOrDefault();

                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPaypalConfiguration: ", e);
            }

            return bRes;

        }

        public bool GetPaypalConfigurationById(decimal? id, out PAYPAL_CONFIGURATION oPaypalConfiguration)
        {
            bool bRes = false;
            oPaypalConfiguration = null;

            if (id == null)
            {
                return bRes;
            }

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

                    oPaypalConfiguration = (from r in dbContext.PAYPAL_CONFIGURATIONs
                                            where r.PPCON_ID == id
                                             select r).FirstOrDefault();
                    bRes = true;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetPaypalConfiguration: ", e);
            }
            return bRes;
        }





        public bool GetMercadoPagoConfiguration(string Guid, out MERCADOPAGO_CONFIGURATION oConfiguration)
        {
            bool bRes = false;
            oConfiguration = null;

            /*oConfiguration = new MERCADOPAGO_CONFIGURATION()
            {
                MEPACON_ID = 1,
                MEPACON_GUID = "xxxxxxxxxxxxxxxxxxxxxxxxxx",
                MEPACON_SDK_URL = "https://sdk.mercadopago.com/js/v2",
                MEPACON_API_URL = "https://api.mercadopago.com",
                MEPACON_HASH_SEED = "",
                MEPACON_CHECK_DATE_AND_HASH = 0,
                MEPACON_CONFIRMATION_TIME = 5000,
                MEPACON_PUBLIC_KEY = "TEST-5a3c6e71-db63-4669-abed-050810d4b302",
                MEPACON_ACCESS_TOKEN = "TEST-1612770678428042-022410-70a9e83dc180c720e82253a1f34707eb-1075501969",
                //MEPACON_PUBLIC_KEY = "APP_USR-0bde9d84-61f9-4bf3-b85a-b88aac71b856";
                //MEPACON_ACCESS_TOKEN = "APP_USR-7008263556073796-040614-d70f216aa2b2f0519a359091b8d8c809-1102379214";
                MEPACON_SERVICE_TIMEOUT = 5000,
            };


            bRes = true;*/


           


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


                    oConfiguration = (from r in dbContext.MERCADOPAGO_CONFIGURATIONs
                                      where r.MEPACON_GUID == Guid
                                      select r).FirstOrDefault();                   


                    bRes = true;



                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetMercadoPagoConfiguration: ", e);
            }
           
            return bRes;

        }

        public bool GetMercadoPagoConfigurationById(decimal? id, out MERCADOPAGO_CONFIGURATION oConfiguration)
        {
            bool bRes = false;
            oConfiguration = null;

            if (id == null)
            {
                return bRes;
            }

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

                    oConfiguration = (from r in dbContext.MERCADOPAGO_CONFIGURATIONs
                                      where r.MEPACON_ID == id
                                      select r).FirstOrDefault();
                    bRes = true;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetMercadoPagoConfigurationById: ", e);
            }
            return bRes;
        }







        public bool GetLanguage(decimal dLanId, out LANGUAGE oLanguage)
        {
            bool bRes = false;
            oLanguage = null;

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

                    oLanguage = (from r in dbContext.LANGUAGEs
                                 where r.LAN_ID == dLanId
                                 select r)
                                .FirstOrDefault();
                    bRes = (oLanguage != null);
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLanguage: ", e);
                bRes = false;
            }

            return bRes;
        }

        public decimal  GetLanguage(string sLanguage)
        {
            decimal dResult = -1;
            LANGUAGE oLanguage = null;

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

                    oLanguage = (from r in dbContext.LANGUAGEs
                                 where r.LAN_CULTURE == sLanguage
                                 select r)
                                .FirstOrDefault();
                    if (oLanguage != null)
                    {
                        dResult = oLanguage.LAN_ID;
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLanguage: ", e);
            }

            return dResult;
        }
        
        public bool GetTariffFromQueryExtId(decimal dInsId, string sExtId, out TARIFF oTariff)
        {
            bool bRes = false;
            oTariff = null;

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

                    oTariff = (from r in dbContext.TARIFFs
                               where r.TAR_INS_ID == dInsId &&
                                     r.TAR_QUERY_EXT_ID == sExtId
                               select r)
                              .FirstOrDefault();
                    bRes = (oTariff != null);
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetTariffFromQueryExtId: ", e);
                bRes = false;
            }

            return bRes;
        }

        public bool InsertTariffIfRequired(decimal dInsId, string sExtId, string sDescription, TariffType eType, TariffBehavior eBehavior, out decimal dTarId)
        {
            bool bRes = false;
            dTarId = 0;
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

                        var oTariff = (from r in dbContext.TARIFFs
                                              where r.TAR_INS_ID == dInsId &&
                                                    r.TAR_QUERY_EXT_ID == sExtId
                                              select r).FirstOrDefault();

                        if (oTariff == null)
                        {
                            decimal dNextTarId = (from r in dbContext.TARIFFs
                                                  where r.TAR_INS_ID == dInsId
                                                  select r.TAR_ID).Max() + 1;
                            decimal dNextLitId = (from r in dbContext.LITERALs
                                                  select r.LIT_ID).Max() + 1;

                            oTariff = new TARIFF
                            {
                                TAR_ID = dNextTarId,
                                TAR_DESCRIPTION = sDescription,
                                LITERAL = new LITERAL()
                                {
                                    LIT_ID = dNextLitId,
                                    LIT_DESCRIPTION = sDescription
                                },
                                TAR_QUERY_EXT_ID = sExtId,
                                TAR_EXT1_ID = sExtId,
                                TAR_INS_ID = dInsId,
                                TAR_TYPE = (int)eType,
                                TAR_BEHAVIOR = (int)eBehavior
                            };
                            dbContext.TARIFFs.InsertOnSubmit(oTariff);

                            // Submit the change to the database.
                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                                bRes = true;

                                dTarId = oTariff.TAR_ID;

                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "InsertTariffIfRequired: ", e);
                                bRes = false;
                            }
                        }
                        else
                        {
                            dTarId = oTariff.TAR_ID;
                            bRes = true;
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "InsertTariffIfRequired: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "InsertTariffIfRequired: ", e);
                bRes = false;
            }

            return bRes;
        }        

        public bool IsInternalEmail(string sEmail)
        {
            return Utils.StringLike(sEmail, string.Format((ConfigurationManager.AppSettings["EmailFromPhoneNumberFormat"] ?? ""), "*")) ||
                Utils.StringLike(sEmail, string.Format((ConfigurationManager.AppSettings["EmailGuestFormat"] ?? ""), "*"));
        }



        public bool FilterSMS()
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                        bool bUpdate = false;
                        DateTime dtUTCTime = DateTime.UtcNow;
                        //1) There is an operation with a later utc date with the same ope_external_base_id1 than the one being analyzed
                        var duplicates = (from o in dbContext.OPERATIONs
                                          join i in dbContext.INSTALLATIONs on o.OPE_INS_ID equals i.INS_ID 
                                          where ((!o.OPE_EXPIRE_SMS_SENT.HasValue || o.OPE_EXPIRE_SMS_SENT == 0) &&
                                             (o.OPE_UTC_ENDDATE > dtUTCTime) && (i.INS_SEND_EXPIRE_OP_SMS.HasValue && i.INS_SEND_EXPIRE_OP_SMS.Value==1))
                                          group o by o.OPE_EXTERNAL_BASE_ID1 into grouped
                                          where grouped.Key != null && grouped.Key.Count() > 1
                                          select grouped.Key);

                        if (duplicates.Count() > 0)
                        {
                            foreach (string opeExternalBaseId in duplicates)
                            {
                                var operationsList = (from op in dbContext.OPERATIONs
                                                      join i in dbContext.INSTALLATIONs on op.OPE_INS_ID equals i.INS_ID
                                                      join u in dbContext.USERs on op.OPE_USR_ID equals u.USR_ID
                                                      where (op.OPE_EXTERNAL_BASE_ID1.Equals(opeExternalBaseId))
                                                      select op);
                                if (operationsList.Count() > 0)
                                {
                                    OPERATION ope = operationsList.OrderByDescending(x => x.OPE_UTC_DATE).FirstOrDefault();
                                    foreach (OPERATION operation in operationsList)
                                    {
                                        if (operation != null && ope != null && !operation.OPE_ID.Equals(ope.OPE_ID))
                                        {
                                            operation.OPE_EXPIRE_SMS_SENT = 1;
                                            SecureSubmitChanges(ref dbContext);
                                            bUpdate = true;
                                        }
                                    }
                                }
                            }

                            //2) Ope_parking_mode = 1 (start/stop) and ope_parking_mode_status in (1,2) (closed or closed automatically) 
                            var operationsStartStopList = (from o in dbContext.OPERATIONs
                                                           join i in dbContext.INSTALLATIONs on o.OPE_INS_ID equals i.INS_ID
                                                           join u in dbContext.USERs on o.OPE_USR_ID equals u.USR_ID
                                                           where ((i.INS_SEND_EXPIRE_OP_SMS.HasValue && i.INS_SEND_EXPIRE_OP_SMS.Value == 1) &&
                                                                   (!o.OPE_EXPIRE_SMS_SENT.HasValue || o.OPE_EXPIRE_SMS_SENT == 0) &&
                                                                    (o.OPE_UTC_ENDDATE > dtUTCTime) &&
                                                                   (o.OPE_PARKING_MODE == (int)ParkingMode.StartStop || o.OPE_PARKING_MODE == (int)ParkingMode.StartStopHybrid) &&
                                                                   ((o.OPE_PARKING_MODE_STATUS == (int)ParkingModeStatus.Closed) ||
                                                                   (o.OPE_PARKING_MODE_STATUS == (int)ParkingModeStatus.ClosedAutomatically))
                                                                   )
                                                           select o);

                            foreach (OPERATION operation in operationsStartStopList)
                            {
                                if (operation != null)
                                {
                                    operation.OPE_EXPIRE_SMS_SENT = 1;
                                    SecureSubmitChanges(ref dbContext);
                                    bUpdate = true;
                                }
                            }

                            //3) Ope_parking_mode = 0 (standard) y ope_type=3 (ParkingRefund) 
                            var operationsStandardList = (from o in dbContext.OPERATIONs
                                                          join i in dbContext.INSTALLATIONs on o.OPE_INS_ID equals i.INS_ID
                                                          join u in dbContext.USERs on o.OPE_USR_ID equals u.USR_ID
                                                          where ((i.INS_SEND_EXPIRE_OP_SMS.HasValue && i.INS_SEND_EXPIRE_OP_SMS.Value == 1) &&
                                                                  (!o.OPE_EXPIRE_SMS_SENT.HasValue || o.OPE_EXPIRE_SMS_SENT == 0) &&
                                                                  (o.OPE_UTC_ENDDATE > dtUTCTime) &&
                                                                  (o.OPE_PARKING_MODE == (int)ParkingMode.Normal) &&
                                                                  (o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund))
                                                          select o);

                            foreach (OPERATION operation in operationsStandardList)
                            {
                                if (operation != null)
                                {
                                    operation.OPE_EXPIRE_SMS_SENT = 1;
                                    SecureSubmitChanges(ref dbContext);
                                    bUpdate = true;
                                }
                            }

                            if (bUpdate)
                            {
                                transaction.Complete();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "FilterSMS: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "FilterSMS: ", e);
                bRes = false;
            }
            return bRes;
        }


        public bool GetListOfUsersToSendSMS(out List<UserOperations> userOperations, int iMaxOperationsToReturn, out int iQueueLengthUserOperationsSenderSMS, List<decimal> oListRunningUserOperationsSenderSMS)
        {
            bool bRes = true;
            userOperations = null;
            iQueueLengthUserOperationsSenderSMS = 0;
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                        DateTime dtUTCTime = DateTime.UtcNow;

                        var operationsList = (from o in dbContext.OPERATIONs
                                                join i in dbContext.INSTALLATIONs on o.OPE_INS_ID equals i.INS_ID
                                                join u in dbContext.USERs on o.OPE_USR_ID equals u.USR_ID
                                                where   ((o.OPE_UTC_ENDDATE > dtUTCTime) &&
                                                        (i.INS_SEND_EXPIRE_OP_SMS.HasValue) && 
                                                        (i.INS_SEND_EXPIRE_OP_SMS.Value == 1) &&
                                                        (!o.OPE_EXPIRE_SMS_SENT.HasValue || o.OPE_EXPIRE_SMS_SENT == 0) &&                                                         
                                                        ((o.OPE_UTC_ENDDATE - dtUTCTime).TotalSeconds < i.INS_SEND_EXPIRE_OP_SMS_MINUTES_BEFORE*60) &&
                                                        (u.USR_MAIN_TEL_COUNTRY.HasValue) && 
                                                        (u.USR_MAIN_TEL != null) &&
                                                        (u.USR_MAIN_TEL != "") &&
                                                         !oListRunningUserOperationsSenderSMS.Contains(o.OPE_ID))
                                                        orderby o.OPE_UTC_ENDDATE ascending
                                                select new
                                                {
                                                    u.USR_EMAIL,
                                                    u.USR_MAIN_TEL,
                                                    u.USR_MAIN_TEL_COUNTRY,
                                                    o.OPE_ID,
                                                    o.GROUP.GRP_DESCRIPTION,
                                                    o.USER_PLATE.USRP_PLATE,
                                                    u.USR_CULTURE_LANG,
                                                    u.USR_LAST_SOAPP_ID,
                                                    (o.OPE_UTC_ENDDATE - dtUTCTime).TotalMinutes
                                                }).Take(iMaxOperationsToReturn);

                        if (operationsList.Count() > 0)
                        {
                            iQueueLengthUserOperationsSenderSMS = operationsList.Count();
                            foreach (var op in operationsList)
                            {
                                if (!String.IsNullOrEmpty(op.USR_MAIN_TEL) && op.USR_MAIN_TEL_COUNTRY.HasValue)
                                {
                                    UserOperations userOp = new UserOperations();
                                    userOp.OpeId = op.OPE_ID;
                                    userOp.UsrMainTel = op.USR_MAIN_TEL;
                                    userOp.UsrMainTelCountry = op.USR_MAIN_TEL_COUNTRY.Value;
                                    userOp.Plate = op.USRP_PLATE;
                                    userOp.GrpDescription = op.GRP_DESCRIPTION;
                                    userOp.UsrCultureLang = op.USR_CULTURE_LANG;
                                    userOp.Minute = Convert.ToInt32(op.TotalMinutes);
                                    userOp.UsrEmail = op.USR_EMAIL;
                                    userOp.UsrLastSourceApp = op.USR_LAST_SOAPP_ID.Value;
                                    if (userOperations == null)
                                    {
                                        userOperations = new List<UserOperations>();
                                    }
                                    userOperations.Add(userOp);                                        
                                }
                            }
                        }
                       
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetListOfUsersToSendSMS: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetListOfUsersToSendSMS: ", e);
                bRes = false;
            }
            return bRes;
        }

        public bool MarkSMSOperation(UserOperations userOperation)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();



                        OPERATION operation = (from o in dbContext.OPERATIONs
                                              where (o.OPE_ID == userOperation.OpeId)
                                              select  o).FirstOrDefault();

                        if (operation != null)
                        {
                            operation.OPE_EXPIRE_SMS_SENT = 1;
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }


                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "MarkSMSOperation: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "MarkSMSOperation: ", e);
                bRes = false;
            }
            return bRes;
        }

        public bool GetLiteralsOfTheMessage(out List<LITERAL_LANGUAGE> listLiterlLanguage, int iLiteralIdSMS)
        {
            bool bRes = true;
            listLiterlLanguage = null;
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                         listLiterlLanguage = (from l in dbContext.LITERALs
                                              join ll in dbContext.LITERAL_LANGUAGEs on l.LIT_ID equals ll.LITL_LIT_ID
                                               where (l.LIT_ID == Convert.ToDecimal(iLiteralIdSMS))
                                              select ll).ToList();

                         if (listLiterlLanguage.Count() == 0)
                         {
                             m_Log.LogMessage(LogLevels.logERROR, "GetLiteralsOfTheMessage::  Message=Empty message verify the literals");
                             bRes = false;
                         }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetLiteralsOfTheMessage: ", e);
                        bRes = false;
                    }
                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLiteralsOfTheMessage: ", e);
                bRes = false;
            }
            return bRes;
        }


        public bool GetListOfUsersWarningsToSend(out List<USERS_WARNING> userWarningList, int iMaxUserWarningsToReturn, out int iQueueLengthUsersWarnings, List<decimal> oListRunningUsersWarnings)
        {
            bool bRes = true;
            userWarningList = null;
            iQueueLengthUsersWarnings = 0;
            if (iMaxUserWarningsToReturn > 0)
            {
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
                            integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                            var usersWarning = (from uw in dbContext.USERS_WARNINGs
                                                where (uw.UWA_TYPE_PUSH == 1 && uw.UWA_STATUS == Convert.ToInt32(UserWarningStatus.Inserted) && !oListRunningUsersWarnings.Contains(uw.UWA_ID))
                                                select uw).ToList();


                            if (usersWarning.Count() > 0)
                            {
                                //iQueueLengthUsersWarnings = usersWarning.Count();
                                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("Número de registros encontrados en la tabla Users_Warnings:{0}", usersWarning.Count()));
                                int iCurrNumber = 0;
                                if (userWarningList == null)
                                {
                                    userWarningList = new List<USERS_WARNING>();
                                }

                                foreach (USERS_WARNING userW in usersWarning)
                                {
                                    if (userW.USER.USERS_PUSH_IDs.Where(r => r.UPID_OS == (int)MobileOS.iOS || r.UPID_OS == (int)MobileOS.Android).Count() > 0)
                                    {
                                        bool bAdd = false;
                                        foreach (var oPushId in userW.USER.USERS_PUSH_IDs)
                                        {
                                            m_Log.LogMessage(LogLevels.logDEBUG, string.Format("GetListOfUsersWarningsToSend --> Tipo de Movil:{0},  usuarioId:{1} y versionApp:{2}", oPushId.UPID_OS, oPushId.UPID_USR_ID, oPushId.UPID_APP_VERSION));
                                            if (AppUtilities.AppVersion(oPushId.UPID_APP_VERSION) >= _VERSION_2_13)
                                            {
                                                bAdd = true;
                                                userW.UWA_STATUS = (int)UserWarningStatus.Inserted;
                                                break;
                                            }
                                            else
                                            {
                                                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("GetListOfUsersWarningsToSend --> Es version Obsolte:{0} ,  usuarioId:{1} ", oPushId.UPID_APP_VERSION, userW.UWA_USER_ID));
                                                userW.UWA_STATUS = (int)UserWarningStatus.ObsoleteVersion;
                                            }

                                        }
                                        if (bAdd)
                                        {
                                            m_Log.LogMessage(LogLevels.logDEBUG, string.Format("GetListOfUsersWarningsToSend --> Agrega el usuario:{0}", userW.UWA_USER_ID));
                                            userWarningList.Add(userW);
                                            iCurrNumber++;
                                            iQueueLengthUsersWarnings++;
                                        }
                                    }
                                    else
                                    {
                                        m_Log.LogMessage(LogLevels.logDEBUG, string.Format("GetListOfUsersWarningsToSend -> No Push ID ,  usuarioId:{0} ", userW.UWA_USER_ID));
                                        userW.UWA_STATUS = (int)UserWarningStatus.NoPushId;
                                    }

                                    if (iCurrNumber == iMaxUserWarningsToReturn)
                                        break;
                                }
                            }

                            try
                            {
                                SecureSubmitChanges(ref dbContext);
                                transaction.Complete();

                            }
                            catch (Exception e)
                            {
                                m_Log.LogMessage(LogLevels.logERROR, "GetListOfUsersWarningsToSend: ", e);
                                bRes = false;
                            }
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "GetListOfUsersWarningsToSend: ", e);
                            bRes = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "GetListOfUsersWarningsToSend: ", e);
                    bRes = false;
                }
            }
            return bRes;
        }

        public bool SetUsersWarningsStatus(USERS_WARNING userWarning, decimal notif)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                        USERS_WARNING usersWarning = (from uw in dbContext.USERS_WARNINGs
                                            where (uw.UWA_ID == userWarning.UWA_ID)
                                            select uw).FirstOrDefault();

                        if (usersWarning!= null)
                        {
                            usersWarning.UWA_STATUS = (int)UserWarningStatus.CreatedUserNotification;
                            usersWarning.UWA_UNO_ID = notif;
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SetUsersWarningsStatus: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetUsersWarningsStatus: ", e);
                bRes = false;
            }
            return bRes;
        }


        public bool SetSignUpGuidCountriesRedirections(string signUpGuid, decimal cou_id)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                        SIGNUP_GUID_COUNTRIES_REDIRECTION oSignUpGuidCountriesRedirections = (from s in dbContext.SIGNUP_GUID_COUNTRIES_REDIRECTIONs
                                                                                              where ((s.SGCR_SIGNUP_GUID == signUpGuid) && (s.SGCR_COU_ID == cou_id))
                                                                                              select s).FirstOrDefault();

                        if (oSignUpGuidCountriesRedirections == null)
                        {
                            oSignUpGuidCountriesRedirections = new SIGNUP_GUID_COUNTRIES_REDIRECTION();
                            oSignUpGuidCountriesRedirections.SGCR_SIGNUP_GUID = signUpGuid;
                            oSignUpGuidCountriesRedirections.SGCR_COU_ID = cou_id;
                            dbContext.SIGNUP_GUID_COUNTRIES_REDIRECTIONs.InsertOnSubmit(oSignUpGuidCountriesRedirections);
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SetSignUpGuidCountriesRedirections: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetSignUpGuidCountriesRedirections: ", e);
                bRes = false;
            }
            return bRes;
        }

        public SIGNUP_GUID_COUNTRIES_REDIRECTION GetSignUpGuidCountriesRedirections(string signUpGuid)
        {
            SIGNUP_GUID_COUNTRIES_REDIRECTION oSignUpGuidCountriesRedirections = null;
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                        oSignUpGuidCountriesRedirections = (from s in dbContext.SIGNUP_GUID_COUNTRIES_REDIRECTIONs
                                                                                              where (s.SGCR_SIGNUP_GUID == signUpGuid)
                                                                                              select s).FirstOrDefault();

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetSignUpGuidCountriesRedirections: ", e);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSignUpGuidCountriesRedirections: ", e);
            }
            return oSignUpGuidCountriesRedirections;
        }

        private class SendEmailResult
        {
            public string mail { get; set; }
            public int index { get; set; }
            public long res { get; set; }
        }
        public string GetCustomErrorMessage(DateTime date, int iErrorCode, decimal grpId, decimal? tarId, string lang)
        {
            string sliteral = string.Empty;

            try
            {


                var oGroup = (from s in getGROUPs()
                              where s.GRP_ID == grpId
                              select s).FirstOrDefault();


                decimal dInstId = oGroup.GRP_INS_ID;

                var oGroupTypes = (from s in getGROUPS_TYPES_ASSIGNATIONs()
                                   where s.GTA_GRP_ID == grpId
                                   select s.GTA_GRPT_ID).Distinct().ToList();

                decimal dTarId = tarId ?? 0;


                var oCustomMessage = getERROR_CUSTOM_MESSAGEs()
                                      .Where( s => date >= s.ERCM_INI_APPLY_DATE &&
                                             date <= s.ERCM_END_APPLY_DATE &&
                                             s.ERCM_ERROR_CODE == iErrorCode &&
                                             (!s.ERCM_INS_ID.HasValue || s.ERCM_INS_ID.Value == dInstId) &&
                                             (!s.ERCM_GRP_ID.HasValue || s.ERCM_GRP_ID.Value == grpId) &&
                                             (!s.ERCM_GRPT_ID.HasValue || oGroupTypes.Contains(s.ERCM_GRPT_ID.Value)) &&
                                             (!s.ERCM_TAR_ID.HasValue || s.ERCM_TAR_ID.Value == dTarId))
                                       .Select(s => new{
                                                        ERCM_LIT_ID= s.ERCM_LIT_ID, 
                                                        ERCM_TAR_ID = (s.ERCM_TAR_ID ?? 0),
                                                        ERCM_GRP_ID = (s.ERCM_GRP_ID ?? 0),
                                                        ERCM_GRPT_ID = (s.ERCM_GRPT_ID ?? 0),
                                                        ERCM_INS_ID = (s.ERCM_INS_ID ?? 0),
                                                        ERCM_INI_APPLY_DATE= s.ERCM_INI_APPLY_DATE})
                                       .OrderByDescending(r => r.ERCM_TAR_ID)
                                       .ThenByDescending(r => r.ERCM_GRP_ID)
                                       .ThenByDescending(r => r.ERCM_GRPT_ID)
                                       .ThenByDescending(r => r.ERCM_INS_ID )
                                       .ThenByDescending(r => r.ERCM_INI_APPLY_DATE)
                                       .FirstOrDefault();


            

                if (oCustomMessage!=null)
                {
                    sliteral = GetLiteral(oCustomMessage.ERCM_LIT_ID, lang);
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomErrorMessage: ", e);
            }


            return sliteral;
        }

        public string GetCustomErrorMessageForOperation(DateTime date, int iErrorCode, decimal grpId, decimal? tarId, string lang)
        {
            string sliteral = string.Empty;

            try
            {


                var oGroup = (from s in getGROUPs()
                              where s.GRP_ID == grpId
                              select s).FirstOrDefault();


                decimal dInstId = oGroup.GRP_INS_ID;

                var oGroupTypes = (from s in getGROUPS_TYPES_ASSIGNATIONs()
                                   where s.GTA_GRP_ID == grpId
                                   select s.GTA_GRPT_ID).Distinct().ToList();

                decimal dTarId = tarId ?? 0;


                var oCustomMessage = getERROR_CUSTOM_MESSAGEs()
                                      .Where(s => date >= s.ERCM_INI_APPLY_DATE &&
                                            date <= s.ERCM_END_APPLY_DATE &&
                                            s.ERCM_ERROR_CODE == iErrorCode &&
                                            (!s.ERCM_INS_ID.HasValue || s.ERCM_INS_ID.Value == dInstId) &&
                                            (!s.ERCM_GRP_ID.HasValue || s.ERCM_GRP_ID.Value == grpId) &&
                                            (!s.ERCM_GRPT_ID.HasValue || oGroupTypes.Contains(s.ERCM_GRPT_ID.Value)) &&
                                            (!s.ERCM_TAR_ID.HasValue || s.ERCM_TAR_ID.Value == dTarId))
                                       .Select(s => new {
                                           ERCM_LIT2_ID = s.ERCM_LIT2_ID,
                                           ERCM_TAR_ID = (s.ERCM_TAR_ID ?? 0),
                                           ERCM_GRP_ID = (s.ERCM_GRP_ID ?? 0),
                                           ERCM_GRPT_ID = (s.ERCM_GRPT_ID ?? 0),
                                           ERCM_INS_ID = (s.ERCM_INS_ID ?? 0),
                                           ERCM_INI_APPLY_DATE = s.ERCM_INI_APPLY_DATE
                                       })
                                       .OrderByDescending(r => r.ERCM_TAR_ID)
                                       .ThenByDescending(r => r.ERCM_GRP_ID)
                                       .ThenByDescending(r => r.ERCM_GRPT_ID)
                                       .ThenByDescending(r => r.ERCM_INS_ID)
                                       .ThenByDescending(r => r.ERCM_INI_APPLY_DATE)
                                       .FirstOrDefault();




                if ((oCustomMessage != null) && (oCustomMessage.ERCM_LIT2_ID.HasValue))
                {
                    sliteral = GetLiteral(oCustomMessage.ERCM_LIT2_ID.Value, lang);
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomErrorMessageForOperation: ", e);
            }


            return sliteral;
        }


        public string GetCustomErrorMessage(DateTime date, int iErrorCode, decimal dInstId, string lang)
        {
            string sliteral = string.Empty;

            try
            {



                var oCustomMessage = getERROR_CUSTOM_MESSAGEs()
                                      .Where(s => date >= s.ERCM_INI_APPLY_DATE &&
                                            date <= s.ERCM_END_APPLY_DATE &&
                                            s.ERCM_ERROR_CODE == iErrorCode &&
                                            (!s.ERCM_INS_ID.HasValue || s.ERCM_INS_ID.Value == dInstId) &&
                                            (!s.ERCM_GRP_ID.HasValue) &&
                                            (!s.ERCM_GRPT_ID.HasValue) &&
                                            (!s.ERCM_TAR_ID.HasValue))
                                       .Select(s => new {
                                           ERCM_LIT_ID = s.ERCM_LIT_ID,
                                           ERCM_TAR_ID = (s.ERCM_TAR_ID ?? 0),
                                           ERCM_GRP_ID = (s.ERCM_GRP_ID ?? 0),
                                           ERCM_GRPT_ID = (s.ERCM_GRPT_ID ?? 0),
                                           ERCM_INS_ID = (s.ERCM_INS_ID ?? 0),
                                           ERCM_INI_APPLY_DATE = s.ERCM_INI_APPLY_DATE
                                       })
                                       .OrderByDescending(r => r.ERCM_INS_ID)
                                       .ThenByDescending(r => r.ERCM_INI_APPLY_DATE)
                                       .FirstOrDefault();




                if (oCustomMessage != null)
                {
                    sliteral = GetLiteral(oCustomMessage.ERCM_LIT_ID, lang);
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCustomErrorMessage: ", e);
            }


            return sliteral;
        }


        public string GetRestrictedTariffMessage(DateTime date, decimal grpId, string lang)
        {
            string sliteral = string.Empty;
                        
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                        RESTRICTED_TARIFF_MESSAGE oRestrictedTariffMessage = (from s in dbContext.RESTRICTED_TARIFF_MESSAGEs
                                                            where ( (date>=s.RTM_INIDATE)  && 
                                                                     (date<=s.RTM_ENDDATE) && 
                                                                     (s.RTM_GRP_ID.Equals(grpId)
                                                                     ))
                                                            select s).FirstOrDefault();

                        if (oRestrictedTariffMessage != null)
                        {
                            sliteral = GetLiteral(oRestrictedTariffMessage.RTM_LIT_ID, lang);
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetRestrictedTariffMessage: ", e);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetRestrictedTariffMessage: ", e);
            }
            

            return sliteral;
        }


        public bool InsertOrUpdateSessionVariables(string key, string values)
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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

                        var sessionValue = (from s in dbContext.SESSION_VARIABLES_STOREs
                                            where (s.SVS_KEY == key)
                                            select s).FirstOrDefault();

                        if (sessionValue != null)
                        {
                            sessionValue.SVS_VALUE = values;
                        }
                        else
                        {
                            dbContext.SESSION_VARIABLES_STOREs.InsertOnSubmit(new SESSION_VARIABLES_STORE
                            {
                                SVS_KEY = key,
                                SVS_VALUE = values,
                                SVS_INSERTION_DATE = DateTime.UtcNow
                            });

                        }
                       
                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "SetUsersWarningsStatus: ", e);
                        bRes = false;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetUsersWarningsStatus: ", e);
                bRes = false;
            }
            return bRes;
        }

        public string GetSessionVariables(string key)
        {
            string strValues = "";

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
                        integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
                        var sessionValue = (from s in dbContext.SESSION_VARIABLES_STOREs
                                            where (s.SVS_KEY == key)
                                            select s).FirstOrDefault();

                        if (sessionValue != null)
                        {
                            strValues = sessionValue.SVS_VALUE;
                        }

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetSessionVariables: ", e);
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSessionVariables: ", e);
            }
            return strValues;
        }




        public int GetRateWSTimeout(decimal dInstallation)
        {
           
            int? iRes = null;

            try
            {

                var oInst = getINSTALLATIONs().Where(r => r.INS_ID == dInstallation).FirstOrDefault();

                if (oInst != null)
                {
                    if (oInst.INS_RATE_WS_TIMEOUT.HasValue)
                    {
                        iRes = oInst.INS_RATE_WS_TIMEOUT;
                    }
                }

                if (!iRes.HasValue)
                {

                    var oPar = getPARAMETERs().Where(r => r.PAR_VALUE == "RateWSTimeoutDefaultValue").FirstOrDefault();

                    if (oPar != null)

                    {
                        string strValue = oPar.PAR_VALUE;

                        try
                        {
                            iRes = Convert.ToInt32(strValue);

                        }
                        catch { }

                    }
                }

                if (!iRes.HasValue)
                {

                    iRes = DEFAULT_WS_TIMEOUT;

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetRateWSTimeout: ", e);

            }
            return iRes.Value;
        }



        public int GetRateWSTimeout()
        {

            int? iRes = null;

            try
            {
                var oPar = getPARAMETERs().Where(r => r.PAR_VALUE == "RateWSTimeoutDefaultValue").FirstOrDefault();

                if (oPar != null)

                {
                    string strValue = oPar.PAR_VALUE;

                    try
                    {
                        iRes = Convert.ToInt32(strValue);

                    }
                    catch { }

                }

                if (!iRes.HasValue)
                {

                    iRes = DEFAULT_WS_TIMEOUT;

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetRateWSTimeout: ", e);

            }
            return iRes.Value;
        }


        public int GetRateWSTimeoutIncreaseValue(decimal dInstallation)
        {

            int? iRes = null;

            try
            {

                var oInst = getINSTALLATIONs().Where(r => r.INS_ID == dInstallation).FirstOrDefault();

                if (oInst != null)
                {
                    if (oInst.INS_RATE_WS_TIMEOUT_INCREASE_VALUE.HasValue)
                    {
                        iRes = oInst.INS_RATE_WS_TIMEOUT_INCREASE_VALUE;
                    }
                }

                if (!iRes.HasValue)
                {

                    var oPar = getPARAMETERs().Where(r => r.PAR_VALUE == "RateWSTimeoutIncreaseValueDefaultValue").FirstOrDefault();

                    if (oPar != null)

                    {
                        string strValue = oPar.PAR_VALUE;

                        try
                        {
                            iRes = Convert.ToInt32(strValue);

                        }
                        catch { }

                    }
                }

                if (!iRes.HasValue)
                {

                    iRes = DEFAULT_WS_INCREASE_TIMEOUT;

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetRateWSTimeoutIncreaseValue: ", e);

            }
            return iRes.Value;
        }

        public bool AddStreetSectionPackage(decimal dInstallationID, decimal id, byte[] file)
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
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();




                    STREET_SECTIONS_PACKAGE_VERSION oVersion = new STREET_SECTIONS_PACKAGE_VERSION()
                    {
                        STSEPV_INS_ID = dInstallationID,
                        STSEPV_ID = id,
                        STSEPV_FILE = file,
                        STSEPV_UTC_DATE = DateTime.UtcNow,
                    };

                    dbContext.STREET_SECTIONS_PACKAGE_VERSIONs.InsertOnSubmit(oVersion);

                    try
                    {
                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();
                        bRes = true;
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "AddStreetSectionPackage: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "AddStreetSectionPackage: ", e);
            }

            return bRes;

        }




        public bool DeleteOlderStreetSectionPackage(decimal dInstallationID, decimal id)
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
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    dbContext.STREET_SECTIONS_PACKAGE_VERSIONs.DeleteAllOnSubmit(dbContext.STREET_SECTIONS_PACKAGE_VERSIONs.Where(c => c.STSEPV_ID < (id - 5)));

                    try
                    {
                        SecureSubmitChanges(ref dbContext);
                        transaction.Complete();
                        bRes = true;
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "DeleteOlderStreetSectionPackage: ", e);
                        bRes = false;
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "DeleteOlderStreetSectionPackage: ", e);
            }

            return bRes;

        }

        public bool GetLastStreetSectionPackageId(decimal dInstallationID, out decimal id)
        {
            bool bRes = false;
            id = -1;


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


                    decimal? dVersion = (from r in dbContext.STREET_SECTIONS_PACKAGE_VERSIONs
                                         where r.STSEPV_INS_ID == dInstallationID
                                         orderby r.STSEPV_ID descending
                                         select r.STSEPV_ID).FirstOrDefault();


                    if (dVersion.HasValue)
                    {
                        id = dVersion.Value;

                    }

                    bRes = true;


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLastStreetSectionPackageId: ", e);
            }

            return bRes;

        }


        public bool GetLastStreetSectionPackage(decimal dInstallationID, out byte[] file)
        {
            bool bRes = false;
            file = null;


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


                    var oFile = (from r in dbContext.STREET_SECTIONS_PACKAGE_VERSIONs
                                 where r.STSEPV_INS_ID == dInstallationID
                                 orderby r.STSEPV_ID descending
                                 select r.STSEPV_FILE).FirstOrDefault();


                    if (oFile != null)
                    {
                        file = oFile.ToArray();
                    }


                    bRes = true;


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLastStreetSectionPackageId: ", e);
            }

            return bRes;

        }


    }

}



