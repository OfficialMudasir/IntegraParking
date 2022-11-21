using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Infrastructure.PermitsAPI;

namespace integraMobile.Domain.Concrete
{

    public class SQLGeograficAndTariffsRepository : IGeograficAndTariffsRepository
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SQLGeograficAndTariffsRepository));
        private const int ctnTransactionTimeout = 30;




        private const int ctMinutesDBSync = 30;
        private static List<INSTALLATION> oGlobal_INSTALLATIONs = new List<INSTALLATION>();
        private static List<GROUP> oGlobal_GROUPs = new List<GROUP>();
        private static List<TARIFF> oGlobal_TARIFFs = new List<TARIFF>();
        private static List<GROUPS_HIERARCHY> oGlobal_GROUP_HIERARCHIEs = new List<GROUPS_HIERARCHY>();
        private static List<GROUPS_GEOMETRY> oGlobal_GROUP_GEOMETRIEs = new List<GROUPS_GEOMETRY>();
        private static List<GROUPS_TYPES_ASSIGNATION> oGlobal_GROUPS_TYPES_ASSIGNATIONs = new List<GROUPS_TYPES_ASSIGNATION>();
        private static List<TARIFFS_IN_GROUP> oGlobal_TARIFFS_IN_GROUPs = new List<TARIFFS_IN_GROUP>();
        private static List<TARIFFS_IN_GROUPS_GEOMETRY> oGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs = new List<TARIFFS_IN_GROUPS_GEOMETRY>();
        private static List<LITERAL> oGlobal_LITERALs = new List<LITERAL>();
        private static List<LITERAL_LANGUAGE> oGlobal_LITERAL_LANGUAGEs = new List<LITERAL_LANGUAGE>();
        private static List<GROUP_CENTER> oGlobal_GROUP_CENTERs = new List<GROUP_CENTER>();
        private static List<LANGUAGE> oGlobal_LANGUAGEs = new List<LANGUAGE>();
        private static List<TARIFFS_CUSTOM_MESSAGE> oGlobal_TARIFFS_CUSTOM_MESSAGEs = new List<TARIFFS_CUSTOM_MESSAGE>();
        private static List<SERVICES_TYPE> oGlobal_SERVICE_TYPEs = new List<SERVICES_TYPE>();
        private static List<INSTALLATION_POLYGON> oGlobal_INSTALLATION_POLYGONs = new List<INSTALLATION_POLYGON>();
        private static List<INSTALLATION_POLYGON_GEOMETRy> oGlobal_INSTALLATION_POLYGON_GEOMETRIEs = new List<INSTALLATION_POLYGON_GEOMETRy>();
        private static List<SOURCE_APP> oGlobal_SOURCE_APPs = new List<SOURCE_APP>();


        private static DateTime dtGlobal_INSTALLATIONs = DateTime.UtcNow;
        private static DateTime dtGlobal_GROUPs = DateTime.UtcNow;
        private static DateTime dtGlobal_TARIFFs = DateTime.UtcNow;
        private static DateTime dtGlobal_GROUP_HIERARCHIEs = DateTime.UtcNow;
        private static DateTime dtGlobal_GROUP_GEOMETRIEs = DateTime.UtcNow;
        private static DateTime dtGlobal_GROUPS_TYPES_ASSIGNATIONs = DateTime.UtcNow;
        private static DateTime dtGlobal_TARIFFS_IN_GROUPs = DateTime.UtcNow;
        private static DateTime dtGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs = DateTime.UtcNow;
        private static DateTime dtGlobal_LITERALs = DateTime.UtcNow;
        private static DateTime dtGlobal_LITERAL_LANGUAGEs = DateTime.UtcNow;
        private static DateTime dtGlobal_GROUP_CENTERs = DateTime.UtcNow;
        private static DateTime dtGlobal_LANGUAGEs = DateTime.UtcNow;
        private static DateTime dtGlobal_TARIFFS_CUSTOM_MESSAGEs = DateTime.UtcNow;
        private static DateTime dtGlobal_SERVICE_TYPEs = DateTime.UtcNow;
        private static DateTime dtGlobal_INSTALLATION_POLYGONs = DateTime.UtcNow;
        private static DateTime dtGlobal_INSTALLATION_POLYGON_GEOMETRIEs = DateTime.UtcNow;
        private static DateTime dtGlobal_SOURCE_APPs = DateTime.UtcNow;




        private List<INSTALLATION> getINSTALLATIONs()
        {
            lock (oGlobal_INSTALLATIONs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_INSTALLATIONs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_INSTALLATIONs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_INSTALLATIONs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_INSTALLATIONs = (from p in dbContext.INSTALLATIONs select p).ToList();
                        dtGlobal_INSTALLATIONs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_INSTALLATIONs;
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

        private List<TARIFF> getTARIFFs()
        {
            lock (oGlobal_TARIFFs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_TARIFFs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_TARIFFs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_TARIFFs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_TARIFFs = (from p in dbContext.TARIFFs select p).ToList();
                        dtGlobal_TARIFFs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_TARIFFs;

            }
        }




        private List<GROUPS_HIERARCHY> getGROUP_HIERARCHIEs()
        {
            lock (oGlobal_GROUP_HIERARCHIEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_GROUP_HIERARCHIEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_GROUP_HIERARCHIEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_GROUP_HIERARCHIEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_GROUP_HIERARCHIEs = (from p in dbContext.GROUPS_HIERARCHies select p).ToList();
                        dtGlobal_GROUP_HIERARCHIEs = DateTime.UtcNow;

                        transaction.Complete();
                    }


                }
                return oGlobal_GROUP_HIERARCHIEs;

            }

        }


        private List<GROUPS_GEOMETRY> getGROUP_GEOMETRIEs()
        {
            lock (oGlobal_GROUP_GEOMETRIEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_GROUP_GEOMETRIEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_GROUP_GEOMETRIEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_GROUP_GEOMETRIEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_GROUP_GEOMETRIEs = (from p in dbContext.GROUPS_GEOMETRies select p).ToList();
                        dtGlobal_GROUP_GEOMETRIEs = DateTime.UtcNow;

                        transaction.Complete();
                    }


                }
                return oGlobal_GROUP_GEOMETRIEs;
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


        private List<TARIFFS_IN_GROUP> getTARIFFS_IN_GROUPs()
        {
            lock (oGlobal_TARIFFS_IN_GROUPs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_TARIFFS_IN_GROUPs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_TARIFFS_IN_GROUPs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_TARIFFS_IN_GROUPs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_TARIFFS_IN_GROUPs = (from p in dbContext.TARIFFS_IN_GROUPs select p).ToList();
                        dtGlobal_TARIFFS_IN_GROUPs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_TARIFFS_IN_GROUPs;
            }
        }


        private List<TARIFFS_IN_GROUPS_GEOMETRY> getTARIFFS_IN_GROUPS_GEOMETRIEs()
        {
            lock (oGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs = (from p in dbContext.TARIFFS_IN_GROUPS_GEOMETRies select p).ToList();
                        dtGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_TARIFFS_IN_GROUPS_GEOMETRIEs;
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

        private List<GROUP_CENTER> getGROUP_CENTERs()
        {
            lock (oGlobal_GROUP_CENTERs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_GROUP_CENTERs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_GROUP_CENTERs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_GROUP_CENTERs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_GROUP_CENTERs = (from p in dbContext.GROUP_CENTERs select p).ToList();
                        dtGlobal_GROUP_CENTERs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_GROUP_CENTERs;
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


        private List<TARIFFS_CUSTOM_MESSAGE> getTARIFFS_CUSTOM_MESSAGEs(decimal dTarId)
        {
            lock (oGlobal_TARIFFS_CUSTOM_MESSAGEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_TARIFFS_CUSTOM_MESSAGEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_TARIFFS_CUSTOM_MESSAGEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_TARIFFS_CUSTOM_MESSAGEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_TARIFFS_CUSTOM_MESSAGEs = (from p in dbContext.TARIFFS_CUSTOM_MESSAGEs select p).ToList();
                        dtGlobal_TARIFFS_CUSTOM_MESSAGEs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_TARIFFS_CUSTOM_MESSAGEs.Where(r => r.TARCME_TAR_ID == dTarId).ToList();

            }
        }


        private List<SERVICES_TYPE> getSERVICE_TYPEs()
        {
            lock (oGlobal_SERVICE_TYPEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_SERVICE_TYPEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_SERVICE_TYPEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_SERVICE_TYPEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_SERVICE_TYPEs = (from p in dbContext.SERVICES_TYPEs select p).ToList();
                        dtGlobal_SERVICE_TYPEs = DateTime.UtcNow;

                        transaction.Complete();
                    }

                }
                return oGlobal_SERVICE_TYPEs;

            }
        }


        private List<INSTALLATION_POLYGON> getINSTALLATION_POLYGONs()
        {
            lock (oGlobal_INSTALLATION_POLYGONs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_INSTALLATION_POLYGONs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_INSTALLATION_POLYGONs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_INSTALLATION_POLYGONs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_INSTALLATION_POLYGONs = (from p in dbContext.INSTALLATION_POLYGONs select p).ToList();
                        dtGlobal_INSTALLATION_POLYGONs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_INSTALLATION_POLYGONs;

            }
        }


        private List<INSTALLATION_POLYGON_GEOMETRy> getINSTALLATION_POLYGON_GEOMETRIEs(decimal dIP)
        {
            lock (oGlobal_INSTALLATION_POLYGON_GEOMETRIEs)
            {
                DateTime dtNow = DateTime.UtcNow;

                if ((oGlobal_INSTALLATION_POLYGON_GEOMETRIEs.Count() == 0) ||
                    ((dtNow.Minute % ctMinutesDBSync == 0) && ((dtNow - dtGlobal_INSTALLATION_POLYGON_GEOMETRIEs).TotalSeconds > 60)) ||
                    ((dtNow - dtGlobal_INSTALLATION_POLYGON_GEOMETRIEs).TotalSeconds > ctMinutesDBSync * 60))
                {


                    using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                                     new TransactionOptions()
                                                                                                     {
                                                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                                     }))
                    {

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();
                        oGlobal_INSTALLATION_POLYGON_GEOMETRIEs = (from p in dbContext.INSTALLATION_POLYGON_GEOMETRies select p).ToList();
                        dtGlobal_INSTALLATION_POLYGON_GEOMETRIEs = DateTime.UtcNow;

                        transaction.Complete();
                    }




                }
                return oGlobal_INSTALLATION_POLYGON_GEOMETRIEs.Where(r => r.IPGE_INSPOL_ID==dIP).ToList(); ;

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


        public SQLGeograficAndTariffsRepository(string connectionString)
        {
        }

        public bool getInstallation(decimal? dInstallationId,
                                    decimal? dLatitude, decimal? dLongitude,
                                    ref INSTALLATION oInstallation,
                                    ref DateTime? dtInsDateTime)
        {
            bool bValidCurrency = false;
            return getInstallation(dInstallationId, dLatitude, dLongitude, null, ref oInstallation, ref dtInsDateTime, out bValidCurrency);
        }

        public bool getInstallation(decimal? dInstallationId,
                                    decimal? dLatitude, decimal? dLongitude,
                                    decimal? dUserCurrencyId,
                                    ref INSTALLATION oInstallation,
                                    ref DateTime? dtInsDateTime,
                                    out bool bValidCurrency)
        {
            return getInstallation(dInstallationId, dLatitude, dLongitude, dUserCurrencyId, false, ref oInstallation, ref dtInsDateTime, out bValidCurrency);
        }

        public bool getInstallation(decimal? dInstallationId,
                                    decimal? dLatitude, decimal? dLongitude, 
                                    decimal? dUserCurrencyId,
                                    bool bGetSuperInstallation,
                                    ref INSTALLATION oInstallation, 
                                    ref DateTime ?dtInsDateTime,
                                    out bool bValidCurrency)
        {
            bool bRes = false;
            oInstallation = null;
            dtInsDateTime = null;
            bValidCurrency = false;

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


                    if (dInstallationId != null)
                    {


                        var oInstallations = (from r in dbContext.INSTALLATIONs
                                              join v in dbContext.VW_INSTALLATIONs on r.INS_ID equals v.INS_ID
                                              where r.INS_ID == dInstallationId &&
                                                    r.INS_ENABLED == 1
                                              select new { Installation = r, Currencies = v.Currencies }).ToArray();
                        if (oInstallations.Count() == 1)
                        {
                            if (dUserCurrencyId.HasValue)
                            {
                                var oCurrencies = oInstallations[0].Currencies.Split(',');
                                bValidCurrency = oCurrencies.Where(cur => Convert.ToDecimal(cur) == dUserCurrencyId.Value).Any();
                            }
                            oInstallation = oInstallations[0].Installation;
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].Installation.INS_TIMEZONE_ID);
                            DateTime dtServerTime = DateTime.Now;
                            dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                            bRes = true;

                        }
                        else
                        {
                            if (bGetSuperInstallation)
                            {
                                var oTempInstallation = (from r in dbContext.INSTALLATIONs
                                                         where r.INS_ID == dInstallationId &&
                                                               r.INS_ENABLED == 1
                                                         select r).FirstOrDefault();
                                if (oTempInstallation != null)
                                {
                                    DateTime dtUTCTime = DateTime.UtcNow;
                                    var oSuperInstallation = oTempInstallation.SUPER_INSTALLATIONs.Where(r => r.SINS_INI_APPLY_DATE <= dtUTCTime && r.SINS_END_APPLY_DATE > dtUTCTime).FirstOrDefault();
                                    if (oSuperInstallation != null)
                                    {
                                        dInstallationId = oSuperInstallation.SINS_SUPER_INS_ID;
                                        oInstallations = (from r in dbContext.INSTALLATIONs
                                                          join v in dbContext.VW_INSTALLATIONs on r.INS_ID equals v.INS_ID
                                                          where r.INS_ID == dInstallationId &&
                                                                r.INS_ENABLED == 1
                                                          select new { Installation = r, Currencies = v.Currencies }).ToArray();
                                        if (oInstallations.Count() == 1)
                                        {
                                            if (dUserCurrencyId.HasValue)
                                            {
                                                var oCurrencies = oInstallations[0].Currencies.Split(',');
                                                bValidCurrency = oCurrencies.Where(cur => Convert.ToDecimal(cur) == dUserCurrencyId.Value).Any();
                                            }
                                            oInstallation = oInstallations[0].Installation;
                                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].Installation.INS_TIMEZONE_ID);
                                            DateTime dtServerTime = DateTime.Now;
                                            dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                                            bRes = true;

                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if ((dLatitude != null) && (dLongitude != null))
                    {
                        var oInstallations = (from r in dbContext.INSTALLATIONs
                                              join v in dbContext.VW_INSTALLATIONs on r.INS_ID equals v.INS_ID
                                              where r.INS_ENABLED == 1
                                              orderby r.INS_ID
                                              select new { Installation = r, Currencies = v.Currencies }).ToArray();
                        bool bIsInside = false;

                        INSTALLATION oInst = null;

                        foreach (var oInsItem in oInstallations)
                        {
                            oInst = oInsItem.Installation;

                            bIsInside = false;
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInst.INS_TIMEZONE_ID);
                            DateTime dtServerTime = DateTime.Now;
                            DateTime dtLocalInstTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);

                            var PolygonNumbers = (from r in dbContext.INSTALLATIONS_GEOMETRies
                                                  where ((r.INSGE_INS_ID == oInst.INS_ID) &&
                                                  (r.INSGE_INI_APPLY_DATE <= dtLocalInstTime) &&
                                                  (r.INSGE_END_APPLY_DATE >= dtLocalInstTime))
                                                  group r by new
                                                  {
                                                    r.INSGE_POL_NUMBER
                                                  } into g
                                                  orderby g.Key.INSGE_POL_NUMBER
                                                  select new {iPolNumber = g.Key.INSGE_POL_NUMBER}).ToList();


                            Point p = new Point(Convert.ToDouble(dLongitude), Convert.ToDouble(dLatitude));

                            foreach (var oPolNumber in PolygonNumbers)
                            {
                                var Polygon = (from r in dbContext.INSTALLATIONS_GEOMETRies
                                               where ((r.INSGE_INS_ID == oInst.INS_ID) &&
                                               (r.INSGE_INI_APPLY_DATE <= dtLocalInstTime) &&
                                               (r.INSGE_END_APPLY_DATE >= dtLocalInstTime) &&
                                               (r.INSGE_POL_NUMBER == oPolNumber.iPolNumber))
                                               orderby r.INSGE_ORDER
                                               select new Point(Convert.ToDouble(r.INSGE_LONGITUDE),
                                                                Convert.ToDouble(r.INSGE_LATITUDE))).ToArray();


                                if (Polygon.Count() > 0)
                                {

                                    if (IsPointInsidePolygon(p, Polygon))
                                    {
                                        bIsInside = true;
                                        if (dUserCurrencyId.HasValue)
                                        {
                                            var oCurrencies = oInstallations[0].Currencies.Split(',');
                                            bValidCurrency = oCurrencies.Where(cur => Convert.ToDecimal(cur) == dUserCurrencyId.Value).Any();
                                        }
                                        break;
                                    }

                                }

                            }

                            if (bIsInside)
                            {
                                bRes = true;
                                oInstallation = oInst;
                                dtInsDateTime = dtLocalInstTime;
                                break;
                            }

                        }

                        

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallation: ", e);
            }

            return bRes;


        }


        public bool getInstallationById(decimal? dInstallationId,
                                   ref INSTALLATION oInstallation,
                                   ref DateTime? dtInsDateTime)
        {
            bool bRes = false;
            oInstallation = null;
            dtInsDateTime = null;

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


                    if (dInstallationId != null)
                    {


                        var oInstallations = (from r in dbContext.INSTALLATIONs
                                              where r.INS_ID == dInstallationId &&
                                                    (r.INS_ENABLED == 1 || r.INS_WEB_PORTAL_PAYMENT_ENABLED==1)
                                              select r).ToArray();
                        if (oInstallations.Count() == 1)
                        {
                            oInstallation = oInstallations[0];
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                            DateTime dtServerTime = DateTime.Now;
                            dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                            bRes = true;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationById: ", e);
            }

            return bRes;


        }

        public bool getInstallationByStandardId(string strStandardInstallationId,
                                   ref INSTALLATION oInstallation,
                                   ref DateTime? dtInsDateTime)
        {
            bool bRes = false;
            oInstallation = null;
            dtInsDateTime = null;

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


                    if (!string.IsNullOrEmpty(strStandardInstallationId))
                    {


                        var oInstallations = (from r in dbContext.INSTALLATIONs
                                              where r.INS_STANDARD_CITY_ID == strStandardInstallationId.Trim() &&
                                                    r.INS_ENABLED == 1
                                              select r).ToArray();
                        if (oInstallations.Count() == 1)
                        {
                            oInstallation = oInstallations[0];
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                            DateTime dtServerTime = DateTime.Now;
                            dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                            bRes = true;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationByStandardId: ", e);
            }

            return bRes;


        }



        public bool getInstallationByStandardIdWebPortal(string strStandardInstallationId,
                                   ref INSTALLATION oInstallation,
                                   ref DateTime? dtInsDateTime)
        {
            bool bRes = false;
            oInstallation = null;
            dtInsDateTime = null;

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


                    if (!string.IsNullOrEmpty(strStandardInstallationId))
                    {


                        var oInstallations = (from r in dbContext.INSTALLATIONs
                                              where r.INS_STANDARD_CITY_ID == strStandardInstallationId.Trim() &&
                                                    r.INS_WEB_PORTAL_PAYMENT_ENABLED == 1
                                              select r).ToArray();
                        if (oInstallations.Count() == 1)
                        {
                            oInstallation = oInstallations[0];
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                            DateTime dtServerTime = DateTime.Now;
                            dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                            bRes = true;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationByStandardId: ", e);
            }

            return bRes;


        }


        public IEnumerable<INSTALLATION> getInstallationsList(decimal? dCurrencyId = null)
        {

            List<INSTALLATION> res = new List<INSTALLATION>();

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

                    string sCurId = "";
                    if (dCurrencyId.HasValue)
                        sCurId = dCurrencyId.Value.ToString();

                    res = (from r in dbContext.INSTALLATIONs
                           join v in dbContext.VW_INSTALLATIONs on r.INS_ID equals v.INS_ID
                           where r.INS_ENABLED == 1 &&
                                 (!dCurrencyId.HasValue || v.Currencies == sCurId || v.Currencies.Contains(sCurId + ",") || v.Currencies.Contains("," + sCurId))
                           orderby r.INS_DESCRIPTION
                           select r).ToList();
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallations: ", e);
            }

            return (IEnumerable<INSTALLATION>)res;

        }


        public bool getGroup(decimal? dGroupId,
                ref GROUP oGroup,
                ref DateTime? dtgroupDateTime)
        {
            bool bRes = false;
            oGroup = null;
            dtgroupDateTime = null;

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


                    if (dGroupId != null)
                    {


                        var oGroups = (from r in dbContext.GROUPs
                                       where r.GRP_ID == dGroupId
                                       select r).ToArray();
                        if (oGroups.Count() == 1)
                        {
                            oGroup = oGroups[0];
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oGroups[0].INSTALLATION.INS_TIMEZONE_ID);
                            DateTime dtServerTime = DateTime.Now;
                            dtgroupDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                            bRes = true;

                        }
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getGroup: ", e);
            }

            return bRes;


        }




       
        public DateTime? getInstallationDateTime(decimal dInstallationId)
        {

            DateTime? dtRes = null;

            try
            {
                var oInstallations = (from r in getINSTALLATIONs()
                                        where r.INS_ID == dInstallationId
                                        select r).ToArray();
                if (oInstallations.Count() == 1)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                    DateTime dtServerTime = DateTime.Now;
                    dtRes = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationDateTime: ", e);
            }

            return dtRes;

        }

        public DateTime? ConvertInstallationDateTimeToUTC(decimal dInstallationId,DateTime dtInstallation)
        {

            DateTime? dtRes = null;

            try
            {



                var oInstallations = (from r in getINSTALLATIONs()
                                          where r.INS_ID == dInstallationId
                                          select r).ToArray();
                if (oInstallations.Count() == 1)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                    dtRes = TimeZoneInfo.ConvertTime(dtInstallation, tzi, TimeZoneInfo.Utc);
                }
                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ConvertInstallationDateTimeToUTC: ", e);
            }

            return dtRes;

        }


        public DateTime? ConvertUTCToInstallationDateTime(decimal dInstallationId, DateTime dtUTC)
        {

            DateTime? dtRes = null;

            try
            {


                var oInstallations = (from r in getINSTALLATIONs()
                                        where r.INS_ID == dInstallationId
                                        select r).ToArray();
                if (oInstallations.Count() == 1)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                    dtRes = TimeZoneInfo.ConvertTime(dtUTC, TimeZoneInfo.Utc, tzi );
                }
                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ConvertUTCToInstallationDateTime: ", e);
            }

            return dtRes;

        }

        public int? GetInstallationUTCOffSetInMinutes(decimal dInstallationId)
        {
            int? iRes = null;

            try
            {
              
                var oInstallations = (from r in getINSTALLATIONs()
                                        where r.INS_ID == dInstallationId
                                        select r).ToArray();
                if (oInstallations.Count() == 1)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                    DateTime dtServerTime = DateTime.Now;
                    DateTime dtInstallation = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                    DateTime dtUTC = TimeZoneInfo.ConvertTime(dtInstallation, tzi, TimeZoneInfo.Utc);

                    iRes = Convert.ToInt32((dtInstallation - dtUTC).TotalMinutes);

                }
                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInstallationUTCOffSetInMinutes: ", e);
            }

            return iRes;


        }


        public DateTime? getGroupDateTime(decimal dGroupID)
        {

            DateTime? dtRes = null;

            try
            {
               


                var oInstallations = (from r in getINSTALLATIONs()
                                        join g in getGROUPs() on r.INS_ID equals g.GRP_INS_ID
                                        where g.GRP_ID == dGroupID
                                        select r).ToArray();
                if (oInstallations.Count() == 1)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallations[0].INS_TIMEZONE_ID);
                    DateTime dtServerTime = DateTime.Now;
                    dtRes = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                }
                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getGroupDateTime: ", e);
            }

            return dtRes;

        }


        public IEnumerable<stZone> getInstallationGroupHierarchy(decimal dInstallationId, List<GroupType> groupTypes, int? lang, bool filterOnlyPermitRatesGroups)
        {

            List<stZone> res = new List<stZone>();

            try
            {
                DateTime? dtNow = getInstallationDateTime(dInstallationId);

                if (dtNow != null)
                {
                    List<decimal> oChildInstallations = GetChildInstallationsIds(dInstallationId, dtNow);

                    var firstLevelGrops = (from g in getGROUPs()
                                           join gh in getGROUP_HIERARCHIEs() on g.GRP_ID equals gh.GRHI_GPR_ID_CHILD
                                           where oChildInstallations.Contains(g.GRP_INS_ID) &&
                                                 gh.GRHI_GPR_ID_PARENT == null &&
                                                 gh.GRHI_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                 gh.GRHI_END_APPLY_DATE >= (DateTime)dtNow &&
                                                 groupTypes.Contains((GroupType)g.GRP_TYPE)
                                           orderby g.GRP_ID
                                           select g).ToArray();

                    if (firstLevelGrops.Count() > 0)
                    {

                        foreach (GROUP group in firstLevelGrops)
                        {

                            decimal dInsId = group.GRP_INS_ID;

                            stZone newZone = new stZone
                                            {
                                                level = 0,
                                                dID = group.GRP_ID,
                                                strDescription = group.GRP_DESCRIPTION,
                                                dLiteralID = group.GRP_LIT_ID,
                                                strColour = group.GRP_COLOUR,
                                                strShowId = group.GRP_SHOW_ID,
                                                subzones = new List<stZone>(),
                                                GPSpolygons = new List<stGPSPolygon>(),
                                                GroupType = (GroupType)group.GRP_TYPE,
                                                Occupancy = (float)(100 - (group.GRP_FREE_SPACES_PERC ?? 0)),
                                                ParkingType = group.GRP_OFFSTREET_TYPE ?? 0,
                                                center = new stGPSPoint(),
                                                allowByPassMap = (getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                    .Where(r => (r.TAGRGE_GRP_ID == group.GRP_ID) &&
                                                                       (r.TAGRGE_INI_APPLY_DATE < (DateTime)dtNow) &&
                                                                       (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow)).Count() == 0),
                                                permitMaxMonths = group.GRP_PERMIT_MAX_MONTHS,
                                                permitMaxBuyDay = group.GRP_PERMIT_MAX_CUR_MONTH_DAY,
                                                dMessageLiteralID = group.GRP_MESSAGE_LIT_ID,
                                            };


                            if (newZone.dMessageLiteralID.HasValue)
                            {
                                if (lang.HasValue)
                                {
                                    newZone.message = getGroupMessage(newZone.dMessageLiteralID.Value, lang.Value);
                                }
                            }

                            getInstallationGroupChilds((DateTime)dtNow,
                                                       1,
                                                       groupTypes,
                                                       lang,
                                                       ref newZone,
                                                       filterOnlyPermitRatesGroups
                                                       );

                            int iNumPoligons = 0;

                            foreach (int iPolNumber in getGROUP_GEOMETRIEs()
                                    .Where(r => r.GRGE_GRP_ID == group.GRP_ID &&
                                                r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                r.GRGE_END_APPLY_DATE >= (DateTime)dtNow)
                                    .GroupBy(r => r.GRGE_POL_NUMBER)
                                    .OrderBy(r => r.Key)
                                    .Select(r => r.Key))
                            {

                                stGPSPolygon oGPSPolygon = new stGPSPolygon();
                                oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                                oGPSPolygon.iPolNumber = iPolNumber;


                                foreach (GROUPS_GEOMETRY oGeometry in getGROUP_GEOMETRIEs()
                                        .Where(r => r.GRGE_GRP_ID == group.GRP_ID &&
                                                    r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                    r.GRGE_END_APPLY_DATE >= (DateTime)dtNow &&
                                                    r.GRGE_POL_NUMBER == iPolNumber)
                                        .OrderBy(r => r.GRGE_ORDER))
                                {
                                    stGPSPoint gpsPoint = new stGPSPoint
                                                        {
                                                            order = oGeometry.GRGE_ORDER,
                                                            dLatitude = oGeometry.GRGE_LATITUDE,
                                                            dLongitude = oGeometry.GRGE_LONGITUDE
                                                        };
                                    ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                                }

                                ((List<stGPSPolygon>)newZone.GPSpolygons).Add(oGPSPolygon);

                                if (iNumPoligons == 0)
                                {
                                    PolygonCentroid(group, dtNow.Value, (List<stGPSPoint>)(oGPSPolygon.GPSpolygon), ref newZone.center);
                                }

                                iNumPoligons++;

                            }

                            
                            bool bAddZone=true;

                            if ((filterOnlyPermitRatesGroups)&&(newZone.subzones.Count()==0))
                            {
                                IEnumerable<stTariff> zoneTariffs = getGroupTariffs(group.GRP_ID, lang);
                                bAddZone = zoneTariffs.Where(r => r.tariffType == (int)TariffType.RegularTariff).Any();
                                if (!bAddZone)
                                {
                                    var installation = (from i in getINSTALLATIONs()
                                                        where i.INS_ID == dInsId
                                                        select i).FirstOrDefault();

                                    if (installation != null)
                                    {
                                        if (installation.INS_MAP_SCREEN_TYPE ==  1)
                                        {
                                            bAddZone = true;
                                        }
                                    }

                                }

                            }


                            if (bAddZone)
                            {
                                res.Add(newZone);
                            }

                        }

                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationGroupHierarchy: ", e);
            }

            return (IEnumerable<stZone>)res;

        }

        public IEnumerable<stCityPolygon> getInstallationPolygons(decimal dInstallationId, int? lang)
        {
            List<stCityPolygon> res = new List<stCityPolygon>();
            try
            {


                DateTime? dtNow = getInstallationDateTime(dInstallationId);

                if (dtNow != null)
                {
                    List<decimal> oChildInstallations = GetChildInstallationsIds(dInstallationId, dtNow);

                    var installationpolygons = (from g in getINSTALLATION_POLYGONs()
                                                where oChildInstallations.Contains(g.INSPOL_INS_ID) &&
                                                      g.INSPOL_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                      g.INSPOL_END_APPLY_DATE >= (DateTime)dtNow
                                                orderby g.INSPOL_ID
                                                select g).ToArray();

                    if (installationpolygons.Count() > 0)
                    {

                        foreach (INSTALLATION_POLYGON ip in installationpolygons)
                        {

                            stCityPolygon newCityPolygons = new stCityPolygon
                            {
                                citiPolygonId = ip.INSPOL_ID,
                                colour = ip.INSPOL_COLOUR,
                                dMessageLiteralID = ip.INSPOL_MESSAGE_LIT_ID,
                                polygon = new List<stGPSPolygon>(),
                            };


                            if (newCityPolygons.dMessageLiteralID.HasValue)
                            {
                                if (lang.HasValue)
                                {
                                    newCityPolygons.message = getInstallationsPolyngosMessage(newCityPolygons.dMessageLiteralID.Value, lang.Value);
                                }
                            }


                            int iNumPoligons = 0;

                            foreach (int iPolNumber in getINSTALLATION_POLYGON_GEOMETRIEs(ip.INSPOL_ID)
                                    .Where(r => r.IPGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                r.IPGE_END_APPLY_DATE >= (DateTime)dtNow)
                                    .GroupBy(r => r.IPGE_POL_NUMBER)
                                    .OrderBy(r => r.Key)
                                    .Select(r => r.Key))
                            {

                                stGPSPolygon oGPSPolygon = new stGPSPolygon();
                                oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                                oGPSPolygon.iPolNumber = iPolNumber;


                                foreach (INSTALLATION_POLYGON_GEOMETRy oGeometry in getINSTALLATION_POLYGON_GEOMETRIEs(ip.INSPOL_ID)
                                        .Where(r => r.IPGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                    r.IPGE_END_APPLY_DATE >= (DateTime)dtNow &&
                                                    r.IPGE_POL_NUMBER == iPolNumber)
                                        .OrderBy(r => r.IPGE_ORDER))
                                {
                                    stGPSPoint gpsPoint = new stGPSPoint
                                    {
                                        order = oGeometry.IPGE_ORDER,
                                        dLatitude = oGeometry.IPGE_LATITUDE,
                                        dLongitude = oGeometry.IPGE_LONGITUDE,
                                        dtIniApply = oGeometry.IPGE_INI_APPLY_DATE,
                                        dtEndApply = oGeometry.IPGE_END_APPLY_DATE
                                    };
                                    ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                                }

                                ((List<stGPSPolygon>)newCityPolygons.polygon).Add(oGPSPolygon);

                                //if (iNumPoligons == 0)
                                //{
                                //    PolygonCentroid(dbContext, group, dtNow.Value, (List<stGPSPoint>)(oGPSPolygon.GPSpolygon), ref newZone.center);
                                //}

                                iNumPoligons++;

                            }

                            res.Add(newCityPolygons);

                        }

                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationGroupHierarchy: ", e);
            }

            return (IEnumerable<stCityPolygon>)res;
        }
        private string getGroupMessage(decimal dMessageLiteralID, int lang)
        {

            string message = String.Empty;

            try
            {
                var MessageLiteral = (from l in getLITERAL_LANGUAGEs()
                                                    where l.LITL_LIT_ID == dMessageLiteralID && l.LITL_LAN_ID == lang
                                                    select l).FirstOrDefault();

                if (MessageLiteral != null)
                {
                    message = MessageLiteral.LITL_LITERAL;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getGroupMessage: ", e);
            }

            return message;

        }

        private string getInstallationsPolyngosMessage(decimal dMessageLiteralID, int lang)
        {

            string message = String.Empty;

            try
            {
                var MessageLiteral = (from l in getLITERAL_LANGUAGEs()
                                      where l.LITL_LIT_ID == dMessageLiteralID && l.LITL_LAN_ID == lang
                                      select l).FirstOrDefault();

                if (MessageLiteral != null)
                {
                    message = MessageLiteral.LITL_LITERAL;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationsPolyngosMessage: ", e);
            }

            return message;

        }


         private stTariffZone NewTariffZone2(TARIFFS_IN_GROUP tariff, DateTime? dtNow, int iMinutesOffsetOffSet, decimal grp_id)
        {
            stTariffZone oTariffZone = new stTariffZone()
            {
                dID = (tariff.TARGR_GRP_ID!=null ? (decimal)tariff.TARGR_GRP_ID : grp_id),
                applicationPeriods = new List<stTariffZoneApplicationPeriods>(),     
                GPSpolygons = new List<stGPSPolygon>(),
            };



            foreach (int iPolNumber in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                            .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                        (r.TAGRGE_GRP_ID == oTariffZone.dID) &&                                                                                
                                                        (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow))
                                            .GroupBy(r => r.TAGRGE_POL_NUMBER)
                                            .OrderBy(r => r.Key)
                                            .Select(r => r.Key))
            {

                stGPSPolygon oGPSPolygon = new stGPSPolygon();
                oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                oGPSPolygon.iPolNumber = iPolNumber;

                foreach (TARIFFS_IN_GROUPS_GEOMETRY oGeometry in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                        .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                    (r.TAGRGE_GRP_ID == oTariffZone.dID) &&                                                                                
                                                    (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow)&&
                                                    (r.TAGRGE_POL_NUMBER == iPolNumber))
                                        .OrderBy(r => r.TAGRGE_ORDER))
                {
                    stGPSPoint gpsPoint = new stGPSPoint
                    {
                        order = oGeometry.TAGRGE_ORDER,
                        dLatitude = oGeometry.TAGRGE_LATITUDE,
                        dLongitude = oGeometry.TAGRGE_LONGITUDE,
                        dtIniApply = oGeometry.TAGRGE_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                        dtEndApply = oGeometry.TAGRGE_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                    };
                                        
                    ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                }

                ((List<stGPSPolygon>)oTariffZone.GPSpolygons).Add(oGPSPolygon);
                
                oTariffZone.applicationPeriods.Add(new stTariffZoneApplicationPeriods()
                {
                    bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                    dtIniApply = tariff.TARGR_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                    dtEndApply = tariff.TARGR_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                });
            }
            
            return oTariffZone;
        }

         private stTariffZone NewTariffZone( TARIFFS_IN_GROUP tariff, DateTime? dtNow, int iMinutesOffsetOffSet)
         {
             stTariffZone oTariffZone = NewTariffZone2( tariff, dtNow, iMinutesOffsetOffSet, (decimal)tariff.TARGR_GRP_ID);
             return oTariffZone;
         }

        public IEnumerable<stTariff> getInstallationTariffs(decimal dInstallationId, decimal? lang)
        {

            List<stTariff> res = new List<stTariff>();
            int iMinutesOffsetOffSet = GetInstallationUTCOffSetInMinutes(dInstallationId) ?? 0;

            try
            {

                    DateTime? dtNow = getInstallationDateTime(dInstallationId);

                    if (dtNow != null)
                    {
                        List<decimal> oChildInstallations = GetChildInstallationsIds(dInstallationId, dtNow);

                        Hashtable tariffHash = new Hashtable();

                        var Tariffs = (from g in getTARIFFS_IN_GROUPs()
                                       join t in getTARIFFs() on g.TARGR_TAR_ID equals t.TAR_ID
                                       where g.TARGR_INI_APPLY_DATE <= (DateTime)dtNow &&
                                             g.TARGR_END_APPLY_DATE >= (DateTime)dtNow &&
                                             oChildInstallations.Contains(t.TAR_INS_ID)
                                       orderby g.TARGR_TAR_ID
                                       select g).ToArray();

                        if (Tariffs.Count() > 0)
                        {
                            foreach (TARIFFS_IN_GROUP tariff in Tariffs)
                            {
                                var oTariff = getTARIFFs().Where(r => r.TAR_ID == tariff.TARGR_TAR_ID).FirstOrDefault();

                                if (tariff.TARGR_GRP_ID != null)
                                {
                                    if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                    {
                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.TARGR_TAR_ID,
                                            dLiteralID = tariff.TARGR_LIT_ID,
                                            strDescription = oTariff.TAR_DESCRIPTION,
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            zones = new List<decimal>(),
                                            tariffZones = new List<stTariffZone>(),
                                            tariffType = (TariffType)oTariff.TAR_TYPE,
                                            maxPlates = oTariff.TAR_MAX_PLATES,
                                            permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                            polygonShow = (tariff.TARGR_POLYGON_SHOW.HasValue ? tariff.TARGR_POLYGON_SHOW.Value : 0),
                                            polygonColour = tariff.TARGR_POLYGON_COLOUR,
                                            polygonZ = (tariff.TARGR_POLYGON_Z.HasValue ? tariff.TARGR_POLYGON_Z.Value : 0),
                                            polygonMapDescription = tariff.TARGR_POLYGON_MAP_DESCRIPTION,
                                            tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),
                                            tarCardPaymentMode = (CardPayment_Mode)tariff.TARGR_CARD_PAYMENT_MODE
                                        };

                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);



                                        if (newTariff.polygonShow == 1)
                                        {
                                            stTariffZone ostTariffZone = NewTariffZone(tariff, dtNow, iMinutesOffsetOffSet);
                                            ((List<stTariffZone>)newTariff.tariffZones).Add(ostTariffZone);
                                        }

                                        ((List<decimal>)newTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);

                                        tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                        res.Add(newTariff);

                                    }
                                    else
                                    {
                                        stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];
                                        bool bNewUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1);
                                        if ((oldTariff.bUserSelectable != bNewUserSelectable) ||
                                            (oldTariff.dLiteralID != tariff.TARGR_LIT_ID))
                                        {
                                            stTariff newTariff = new stTariff
                                            {
                                                dID = tariff.TARGR_TAR_ID,
                                                dLiteralID = tariff.TARGR_LIT_ID,
                                                strDescription = oTariff.TAR_DESCRIPTION,
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                zones = new List<decimal>(),
                                                tariffType = (TariffType)oTariff.TAR_TYPE,
                                                maxPlates = oTariff.TAR_MAX_PLATES,
                                                permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                                tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                                ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                                ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                                polygonShow = (tariff.TARGR_POLYGON_SHOW.HasValue ? tariff.TARGR_POLYGON_SHOW.Value : 0),
                                                polygonColour = tariff.TARGR_POLYGON_COLOUR,
                                                polygonZ = (tariff.TARGR_POLYGON_Z.HasValue ? tariff.TARGR_POLYGON_Z.Value : 0),
                                                polygonMapDescription = tariff.TARGR_POLYGON_MAP_DESCRIPTION,
                                                tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                                tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                                tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),                                                
                                                tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                                tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),
                                                tarCardPaymentMode = (CardPayment_Mode)tariff.TARGR_CARD_PAYMENT_MODE

                                            };
                                            ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                            newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);


                                            ((List<decimal>)newTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);

                                            tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                            res.Add(newTariff);

                                        }
                                        else
                                        {
                                            if (!((List<decimal>)oldTariff.zones).Exists(element => element == ((decimal)tariff.TARGR_GRP_ID)))
                                            {
                                                ((List<decimal>)oldTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);
                                            }
                                        }
                                    }

                                }
                                else if (tariff.TARGR_GRPT_ID != null)
                                {


                                    if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                    {
                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.TARGR_TAR_ID,
                                            dLiteralID = tariff.TARGR_LIT_ID,
                                            strDescription = oTariff.TAR_DESCRIPTION,
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            zones = new List<decimal>(),
                                            tariffZones = new List<stTariffZone>(),
                                            tariffType = (TariffType)oTariff.TAR_TYPE,
                                            maxPlates = oTariff.TAR_MAX_PLATES,
                                            permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                            polygonShow = (tariff.TARGR_POLYGON_SHOW.HasValue ? tariff.TARGR_POLYGON_SHOW.Value : 0),
                                            polygonColour = tariff.TARGR_POLYGON_COLOUR,
                                            polygonZ = (tariff.TARGR_POLYGON_Z.HasValue ? tariff.TARGR_POLYGON_Z.Value : 0),
                                            polygonMapDescription = tariff.TARGR_POLYGON_MAP_DESCRIPTION,
                                            tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),
                                            tarCardPaymentMode = (CardPayment_Mode)tariff.TARGR_CARD_PAYMENT_MODE


                                        };

                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);



                                        foreach (GROUPS_TYPES_ASSIGNATION group_assig in getGROUPS_TYPES_ASSIGNATIONs().Where(r=> r.GTA_GRPT_ID==tariff.TARGR_GRPT_ID))
                                        {
                                            ((List<decimal>)newTariff.zones).Add((decimal)group_assig.GTA_GRP_ID);
                                            //TO DO: MARIU
                                            if (newTariff.polygonShow == 1)
                                            {
                                                stTariffZone ostTariffZone = NewTariffZone2(tariff, dtNow, iMinutesOffsetOffSet, (decimal)group_assig.GTA_GRP_ID);
                                                ((List<stTariffZone>)newTariff.tariffZones).Add(ostTariffZone);
                                            }
                                        }



                                        tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                        res.Add(newTariff);

                                    }
                                    else
                                    {
                                        stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];
                                        bool bNewUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1);
                                        if ((oldTariff.bUserSelectable != bNewUserSelectable) ||
                                            (oldTariff.dLiteralID != tariff.TARGR_LIT_ID))
                                        {
                                            stTariff newTariff = new stTariff
                                            {
                                                dID = tariff.TARGR_TAR_ID,
                                                dLiteralID = tariff.TARGR_LIT_ID,
                                                strDescription = oTariff.TAR_DESCRIPTION,
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                zones = new List<decimal>(),
                                                tariffZones = new List<stTariffZone>(),
                                                tariffType = (TariffType)oTariff.TAR_TYPE,
                                                maxPlates = oTariff.TAR_MAX_PLATES,
                                                permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                                tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                                ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                                ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                                polygonShow = (tariff.TARGR_POLYGON_SHOW.HasValue ? tariff.TARGR_POLYGON_SHOW.Value : 0),
                                                polygonColour = tariff.TARGR_POLYGON_COLOUR,
                                                polygonZ = (tariff.TARGR_POLYGON_Z.HasValue ? tariff.TARGR_POLYGON_Z.Value : 0),
                                                polygonMapDescription = tariff.TARGR_POLYGON_MAP_DESCRIPTION,
                                                tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                                tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                                tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                                tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                                tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),
                                                tarCardPaymentMode = (CardPayment_Mode)tariff.TARGR_CARD_PAYMENT_MODE
                                            };

                                            ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                            newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);


                                            foreach (GROUPS_TYPES_ASSIGNATION group_assig in getGROUPS_TYPES_ASSIGNATIONs().Where(r => r.GTA_GRPT_ID == tariff.TARGR_GRPT_ID))
                                            {
                                                ((List<decimal>)newTariff.zones).Add((decimal)group_assig.GTA_GRP_ID);
                                                //TO DO: MARIU
                                                if (newTariff.polygonShow == 1)
                                                {
                                                    stTariffZone ostTariffZone = NewTariffZone2(tariff, dtNow, iMinutesOffsetOffSet, (decimal)group_assig.GTA_GRP_ID);
                                                    ((List<stTariffZone>)newTariff.tariffZones).Add(ostTariffZone);
                                                }
                                            }

                                            tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                            res.Add(newTariff);

                                        }
                                        else
                                        {


                                            foreach (GROUPS_TYPES_ASSIGNATION group_assig in tariff.GROUPS_TYPE.GROUPS_TYPES_ASSIGNATIONs)
                                            {
                                                if (!((List<decimal>)oldTariff.zones).Exists(element => element == ((decimal)group_assig.GTA_GRP_ID)))
                                                {
                                                    ((List<decimal>)oldTariff.zones).Add((decimal)group_assig.GTA_GRP_ID);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }                
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationTariffs: ", e);
            }

            return (IEnumerable<stTariff>)res;

        }





        /****/

        public IEnumerable<stZone> getInstallationGroupHierarchy2(decimal dInstallationId, List<GroupType> groupTypes, bool filterOnlyPermitRatesGroups)
        {

            List<stZone> res = new List<stZone>();

            try
            {

                int iMinutesOffsetOffSet = GetInstallationUTCOffSetInMinutes(dInstallationId) ?? 0;




                DateTime? dtNow = getInstallationDateTime(dInstallationId);

                if (dtNow != null)
                {
                    List<decimal> oChildInstallations = GetChildInstallationsIds(dInstallationId, dtNow);

                    var firstLevelGrops = (from g in getGROUPs()
                                           join gh in getGROUP_HIERARCHIEs() on g.GRP_ID equals gh.GRHI_GPR_ID_CHILD
                                           where oChildInstallations.Contains(g.GRP_INS_ID) &&
                                                 gh.GRHI_GPR_ID_PARENT == null &&
                                               //gh.GRHI_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                 gh.GRHI_END_APPLY_DATE >= (DateTime)dtNow &&
                                                 groupTypes.Contains((GroupType)g.GRP_TYPE)
                                           orderby g.GRP_ID
                                           select new { g, gh }).ToArray();

                    if (firstLevelGrops.Count() > 0)
                    {
                        foreach (var group in firstLevelGrops)
                        {
                            decimal dInsId = group.g.GRP_INS_ID;
                            stZone newZone = new stZone
                            {
                                level = 0,
                                dID = group.g.GRP_ID,
                                strDescription = group.g.GRP_DESCRIPTION,
                                dLiteralID = group.g.GRP_LIT_ID,
                                strColour = group.g.GRP_COLOUR,
                                strShowId = group.g.GRP_SHOW_ID,
                                subzones = new List<stZone>(),
                                GPSpolygons = new List<stGPSPolygon>(),
                                GroupType = (GroupType)group.g.GRP_TYPE,
                                Occupancy = (float)(100 - (group.g.GRP_FREE_SPACES_PERC ?? 0)),
                                ParkingType = group.g.GRP_OFFSTREET_TYPE ?? 0,
                                dtIniApply = group.gh.GRHI_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                dtEndApply = group.gh.GRHI_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                center = new stGPSPoint(),
                                allowByPassMap = (getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                    .Where(r => (r.TAGRGE_GRP_ID == group.g.GRP_ID) &&
                                                        (r.TAGRGE_INI_APPLY_DATE < (DateTime)dtNow) &&
                                                        (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow)).Count() == 0),
                                permitMaxMonths = group.g.GRP_PERMIT_MAX_MONTHS,
                                permitMaxBuyDay = group.g.GRP_PERMIT_MAX_CUR_MONTH_DAY

                            };


                            getInstallationGroupChilds2((DateTime)dtNow,
                                                       1,
                                                       groupTypes,
                                                       iMinutesOffsetOffSet,
                                                       ref newZone,
                                                       filterOnlyPermitRatesGroups);

                            int iNumPoligons = 0;

                            foreach (int iPolNumber in getGROUP_GEOMETRIEs()
                                    .Where(r => r.GRGE_GRP_ID == group.g.GRP_ID &&
                                                //r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                r.GRGE_END_APPLY_DATE >= (DateTime)dtNow)
                                    .GroupBy(r => r.GRGE_POL_NUMBER)
                                    .OrderBy(r => r.Key)
                                    .Select(r => r.Key))
                            {

                                stGPSPolygon oGPSPolygon = new stGPSPolygon();
                                oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                                oGPSPolygon.iPolNumber = iPolNumber;

                                foreach (GROUPS_GEOMETRY oGeometry in getGROUP_GEOMETRIEs()
                                        .Where(r => r.GRGE_GRP_ID == group.g.GRP_ID &&
                                                    //r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                    r.GRGE_END_APPLY_DATE >= (DateTime)dtNow &&
                                                    r.GRGE_POL_NUMBER == iPolNumber)
                                        .OrderBy(r => r.GRGE_ORDER))
                                {
                                    stGPSPoint gpsPoint = new stGPSPoint
                                    {
                                        order = oGeometry.GRGE_ORDER,
                                        dLatitude = oGeometry.GRGE_LATITUDE,
                                        dLongitude = oGeometry.GRGE_LONGITUDE,
                                        dtIniApply = oGeometry.GRGE_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                        dtEndApply = oGeometry.GRGE_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                    };

                                    ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                                }

                                ((List<stGPSPolygon>)newZone.GPSpolygons).Add(oGPSPolygon);

                                if (iNumPoligons == 0)
                                {
                                    PolygonCentroid(group.g, dtNow.Value, (List<stGPSPoint>)(oGPSPolygon.GPSpolygon), ref newZone.center);
                                }

                                iNumPoligons++;
                            }



                            bool bAddZone = true;

                            if ((filterOnlyPermitRatesGroups) && (newZone.subzones.Count() == 0))
                            {
                                IEnumerable<stTariff> zoneTariffs = getGroupTariffs(group.g.GRP_ID, null);
                                bAddZone = zoneTariffs.Where(r => r.tariffType == (int)TariffType.RegularTariff).Any();
                                if (!bAddZone)
                                {
                                    var installation = (from i in getINSTALLATIONs()
                                                        where i.INS_ID == dInsId
                                                        select i).FirstOrDefault();

                                    if (installation != null)
                                    {
                                        if (installation.INS_MAP_SCREEN_TYPE == 1)
                                        {
                                            bAddZone = true;
                                        }
                                    }

                                }

                            }


                            if (bAddZone)
                            {
                                res.Add(newZone);
                            }
                            

                        }

                    }

                }


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationGroupHierarchy2: ", e);
            }

            return (IEnumerable<stZone>)res;

        }



        public IEnumerable<stTariff> getInstallationTariffs2(decimal dInstallationId, decimal? lang)
        {

            List<stTariff> res = new List<stTariff>();

            try
            {
                int iMinutesOffsetOffSet = GetInstallationUTCOffSetInMinutes(dInstallationId) ?? 0;



                DateTime? dtNow = getInstallationDateTime(dInstallationId);

                if (dtNow != null)
                {

                    List<decimal> oChildInstallations = GetChildInstallationsIds(dInstallationId, dtNow);


                    Hashtable tariffHash = new Hashtable();

                    var Tariffs = (from g in getTARIFFS_IN_GROUPs()
                                   join t in getTARIFFs() on g.TARGR_TAR_ID equals t.TAR_ID
                                   where //g.TARGR_INI_APPLY_DATE <= (DateTime)dtNow &&
                                         g.TARGR_END_APPLY_DATE >= (DateTime)dtNow &&
                                         oChildInstallations.Contains(t.TAR_INS_ID)
                                   orderby g.TARGR_TAR_ID
                                   select g).ToArray();

                    if (Tariffs.Count() > 0)
                    {
                        foreach (TARIFFS_IN_GROUP tariff in Tariffs)
                        {
                            var oTariff = getTARIFFs().Where(r => r.TAR_ID == tariff.TARGR_TAR_ID).FirstOrDefault();

                            if (tariff.TARGR_GRP_ID != null)
                            {
                                if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                {
                                    stTariff newTariff = new stTariff
                                    {
                                        dID = tariff.TARGR_TAR_ID,
                                        dLiteralID = tariff.TARGR_LIT_ID,
                                        strDescription = oTariff.TAR_DESCRIPTION,
                                        tariffZones = new List<stTariffZone>(),
                                        tariffType = (TariffType)oTariff.TAR_TYPE,
                                        maxPlates = oTariff.TAR_MAX_PLATES,
                                        permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                        tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                        ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                        ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                        polygonShow = (tariff.TARGR_POLYGON_SHOW.HasValue ? tariff.TARGR_POLYGON_SHOW.Value : 0),
                                        polygonColour = tariff.TARGR_POLYGON_COLOUR,
                                        polygonZ = (tariff.TARGR_POLYGON_Z.HasValue ? tariff.TARGR_POLYGON_Z.Value : 0),
                                        polygonMapDescription = tariff.TARGR_POLYGON_MAP_DESCRIPTION,
                                        tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                        tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                        tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                        tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                        tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE??0),

                                    };

                                    ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                    newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                    stTariffZone oTariffZone = new stTariffZone()
                                    {
                                        dID = (decimal)tariff.TARGR_GRP_ID,
                                        applicationPeriods = new List<stTariffZoneApplicationPeriods>(),
                                        GPSpolygons = new List<stGPSPolygon>(),
                                    };



                                    foreach (int iPolNumber in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                    .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                                (r.TAGRGE_GRP_ID == (decimal)tariff.TARGR_GRP_ID) &&
                                                                                (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow))
                                                                    .GroupBy(r => r.TAGRGE_POL_NUMBER)
                                                                    .OrderBy(r => r.Key)
                                                                    .Select(r => r.Key))
                                    {

                                        stGPSPolygon oGPSPolygon = new stGPSPolygon();
                                        oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                                        oGPSPolygon.iPolNumber = iPolNumber;

                                        foreach (TARIFFS_IN_GROUPS_GEOMETRY oGeometry in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                            (r.TAGRGE_GRP_ID == (decimal)tariff.TARGR_GRP_ID) &&
                                                                            (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow) &&
                                                                            (r.TAGRGE_POL_NUMBER == iPolNumber))
                                                                .OrderBy(r => r.TAGRGE_ORDER))
                                        {
                                            stGPSPoint gpsPoint = new stGPSPoint
                                            {
                                                order = oGeometry.TAGRGE_ORDER,
                                                dLatitude = oGeometry.TAGRGE_LATITUDE,
                                                dLongitude = oGeometry.TAGRGE_LONGITUDE,
                                                dtIniApply = oGeometry.TAGRGE_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                                dtEndApply = oGeometry.TAGRGE_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                            };

                                            ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                                        }

                                        ((List<stGPSPolygon>)oTariffZone.GPSpolygons).Add(oGPSPolygon);
                                    }


                                    oTariffZone.applicationPeriods.Add(new stTariffZoneApplicationPeriods()
                                    {
                                        bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                        dtIniApply = tariff.TARGR_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                        dtEndApply = tariff.TARGR_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                    });


                                    ((List<stTariffZone>)newTariff.tariffZones).Add(oTariffZone);

                                    tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                    res.Add(newTariff);

                                }
                                else
                                {
                                    stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];

                                    if (!((List<stTariffZone>)oldTariff.tariffZones).Exists(element => element.dID == ((decimal)tariff.TARGR_GRP_ID)))
                                    {
                                        stTariffZone oTariffZone = new stTariffZone()
                                        {
                                            dID = (decimal)tariff.TARGR_GRP_ID,
                                            applicationPeriods = new List<stTariffZoneApplicationPeriods>(),
                                            GPSpolygons = new List<stGPSPolygon>(),

                                        };


                                        foreach (int iPolNumber in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                    .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                                (r.TAGRGE_GRP_ID == (decimal)tariff.TARGR_GRP_ID) &&
                                                                                (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow))
                                                                    .GroupBy(r => r.TAGRGE_POL_NUMBER)
                                                                    .OrderBy(r => r.Key)
                                                                    .Select(r => r.Key))
                                        {

                                            stGPSPolygon oGPSPolygon = new stGPSPolygon();
                                            oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                                            oGPSPolygon.iPolNumber = iPolNumber;

                                            foreach (TARIFFS_IN_GROUPS_GEOMETRY oGeometry in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                    .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                                (r.TAGRGE_GRP_ID == (decimal)tariff.TARGR_GRP_ID) &&
                                                                                (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow) &&
                                                                                (r.TAGRGE_POL_NUMBER == iPolNumber))
                                                                    .OrderBy(r => r.TAGRGE_ORDER))
                                            {
                                                stGPSPoint gpsPoint = new stGPSPoint
                                                {
                                                    order = oGeometry.TAGRGE_ORDER,
                                                    dLatitude = oGeometry.TAGRGE_LATITUDE,
                                                    dLongitude = oGeometry.TAGRGE_LONGITUDE,
                                                    dtIniApply = oGeometry.TAGRGE_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                                    dtEndApply = oGeometry.TAGRGE_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                                };

                                                ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                                            }

                                            ((List<stGPSPolygon>)oTariffZone.GPSpolygons).Add(oGPSPolygon);
                                        }

                                        oTariffZone.applicationPeriods.Add(new stTariffZoneApplicationPeriods()
                                        {

                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            dtIniApply = tariff.TARGR_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                            dtEndApply = tariff.TARGR_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                        });
                                        ((List<stTariffZone>)oldTariff.tariffZones).Add(oTariffZone);
                                    }
                                    else
                                    {

                                        var oTariffZone = ((List<stTariffZone>)oldTariff.tariffZones).Where(element => element.dID == ((decimal)tariff.TARGR_GRP_ID)).FirstOrDefault();


                                        oTariffZone.applicationPeriods.Add(new stTariffZoneApplicationPeriods()
                                        {
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            dtIniApply = tariff.TARGR_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                            dtEndApply = tariff.TARGR_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                        });
                                    }

                                }



                            }
                            else if (tariff.TARGR_GRPT_ID != null)
                            {

                                if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                {
                                    stTariff newTariff = new stTariff
                                    {
                                        dID = tariff.TARGR_TAR_ID,
                                        dLiteralID = tariff.TARGR_LIT_ID,
                                        strDescription = oTariff.TAR_DESCRIPTION,
                                        tariffZones = new List<stTariffZone>(),
                                        tariffType = (TariffType)oTariff.TAR_TYPE,
                                        maxPlates = oTariff.TAR_MAX_PLATES,
                                        permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                        tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                        ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                        ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                        polygonShow = (tariff.TARGR_POLYGON_SHOW.HasValue ? tariff.TARGR_POLYGON_SHOW.Value : 0),
                                        polygonColour = tariff.TARGR_POLYGON_COLOUR,
                                        polygonZ = (tariff.TARGR_POLYGON_Z.HasValue ? tariff.TARGR_POLYGON_Z.Value : 0),
                                        polygonMapDescription = tariff.TARGR_POLYGON_MAP_DESCRIPTION,
                                        tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                        tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                        tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                        tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                        tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                    };
                                    ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                    newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                    foreach (GROUPS_TYPES_ASSIGNATION group_assig in getGROUPS_TYPES_ASSIGNATIONs().Where(r => r.GTA_GRPT_ID == tariff.TARGR_GRPT_ID))
                                    {
                                        stTariffZone oTariffZone = new stTariffZone()
                                        {
                                            dID = (decimal)group_assig.GTA_GRP_ID,
                                            applicationPeriods = new List<stTariffZoneApplicationPeriods>(),
                                            GPSpolygons = new List<stGPSPolygon>(),

                                        };

                                        foreach (int iPolNumber in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                    .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                                (r.TAGRGE_GRP_ID == (decimal)group_assig.GTA_GRP_ID) &&
                                                                                (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow))
                                                                    .GroupBy(r => r.TAGRGE_POL_NUMBER)
                                                                    .OrderBy(r => r.Key)
                                                                    .Select(r => r.Key))
                                        {

                                            stGPSPolygon oGPSPolygon = new stGPSPolygon();
                                            oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                                            oGPSPolygon.iPolNumber = iPolNumber;

                                            foreach (TARIFFS_IN_GROUPS_GEOMETRY oGeometry in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                    .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                                (r.TAGRGE_GRP_ID == (decimal)group_assig.GTA_GRP_ID) &&
                                                                                (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow) &&
                                                                                (r.TAGRGE_POL_NUMBER == iPolNumber))
                                                                    .OrderBy(r => r.TAGRGE_ORDER))
                                            {
                                                stGPSPoint gpsPoint = new stGPSPoint
                                                {
                                                    order = oGeometry.TAGRGE_ORDER,
                                                    dLatitude = oGeometry.TAGRGE_LATITUDE,
                                                    dLongitude = oGeometry.TAGRGE_LONGITUDE,
                                                    dtIniApply = oGeometry.TAGRGE_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                                    dtEndApply = oGeometry.TAGRGE_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                                };

                                                ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                                            }

                                            ((List<stGPSPolygon>)oTariffZone.GPSpolygons).Add(oGPSPolygon);
                                        }

                                        oTariffZone.applicationPeriods.Add(new stTariffZoneApplicationPeriods()
                                        {
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            dtIniApply = tariff.TARGR_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                            dtEndApply = tariff.TARGR_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                        });


                                        ((List<stTariffZone>)newTariff.tariffZones).Add(oTariffZone);

                                    }



                                    tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                    res.Add(newTariff);

                                }
                                else
                                {
                                    stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];

                                    foreach (GROUPS_TYPES_ASSIGNATION group_assig in getGROUPS_TYPES_ASSIGNATIONs().Where(r => r.GTA_GRPT_ID == tariff.TARGR_GRPT_ID))
                                    {
                                        if (!((List<stTariffZone>)oldTariff.tariffZones).Exists(element => element.dID == ((decimal)group_assig.GTA_GRP_ID)))
                                        {
                                            stTariffZone oTariffZone = new stTariffZone()
                                            {
                                                dID = (decimal)group_assig.GTA_GRP_ID,
                                                applicationPeriods = new List<stTariffZoneApplicationPeriods>(),
                                                GPSpolygons = new List<stGPSPolygon>(),

                                            };

                                            foreach (int iPolNumber in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                    .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                                (r.TAGRGE_GRP_ID == (decimal)group_assig.GTA_GRP_ID) &&
                                                                                (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow))
                                                                    .GroupBy(r => r.TAGRGE_POL_NUMBER)
                                                                    .OrderBy(r => r.Key)
                                                                    .Select(r => r.Key))
                                            {

                                                stGPSPolygon oGPSPolygon = new stGPSPolygon();
                                                oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                                                oGPSPolygon.iPolNumber = iPolNumber;

                                                foreach (TARIFFS_IN_GROUPS_GEOMETRY oGeometry in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                                        .Where(r => (r.TAGRGE_TAR_ID == tariff.TARGR_TAR_ID) &&
                                                                                    (r.TAGRGE_GRP_ID == (decimal)group_assig.GTA_GRP_ID) &&
                                                                                    (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow) &&
                                                                                    (r.TAGRGE_POL_NUMBER == iPolNumber))
                                                                        .OrderBy(r => r.TAGRGE_ORDER))
                                                {
                                                    stGPSPoint gpsPoint = new stGPSPoint
                                                    {
                                                        order = oGeometry.TAGRGE_ORDER,
                                                        dLatitude = oGeometry.TAGRGE_LATITUDE,
                                                        dLongitude = oGeometry.TAGRGE_LONGITUDE,
                                                        dtIniApply = oGeometry.TAGRGE_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                                        dtEndApply = oGeometry.TAGRGE_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                                    };

                                                    ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                                                }

                                                ((List<stGPSPolygon>)oTariffZone.GPSpolygons).Add(oGPSPolygon);
                                            }

                                            oTariffZone.applicationPeriods.Add(new stTariffZoneApplicationPeriods()
                                            {
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                dtIniApply = tariff.TARGR_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                                dtEndApply = tariff.TARGR_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                            });
                                            ((List<stTariffZone>)oldTariff.tariffZones).Add(oTariffZone);
                                        }
                                        else
                                        {

                                            var oTariffZone = ((List<stTariffZone>)oldTariff.tariffZones).Where(element => element.dID == ((decimal)group_assig.GTA_GRP_ID)).FirstOrDefault();


                                            oTariffZone.applicationPeriods.Add(new stTariffZoneApplicationPeriods()
                                            {
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                dtIniApply = tariff.TARGR_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                                dtEndApply = tariff.TARGR_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                            });
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationTariffs2: ", e);
            }

            return (IEnumerable<stTariff>)res;

        }



        public IEnumerable<stTariff> getGroupTariffs(decimal dGroupId, decimal? lang)
        {

            List<stTariff> res = new List<stTariff>();

            try
            {

                DateTime? dtNow = getGroupDateTime(dGroupId);


                if (dtNow != null)
                {
                    GROUP oGroup = getGROUPs().Where(r => r.GRP_ID == dGroupId).FirstOrDefault();

                    Hashtable tariffHash = new Hashtable();

                    var Tariffs = (from g in getTARIFFS_IN_GROUPs()
                                   join t in getTARIFFs() on g.TARGR_TAR_ID equals t.TAR_ID
                                   where g.TARGR_INI_APPLY_DATE <= (DateTime)dtNow &&
                                         g.TARGR_END_APPLY_DATE >= (DateTime)dtNow &&
                                         t.TAR_INS_ID == oGroup.GRP_INS_ID
                                   orderby g.TARGR_TAR_ID
                                   select g).ToArray();

                    if (Tariffs.Count() > 0)
                    {
                        foreach (TARIFFS_IN_GROUP tariff in Tariffs)
                        {
                            var oTariff = getTARIFFs().Where(r => r.TAR_ID == tariff.TARGR_TAR_ID).FirstOrDefault();

                            if (tariff.TARGR_GRP_ID != null)
                            {
                                if (tariff.TARGR_GRP_ID == dGroupId)
                                {
                                    if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                    {
                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.TARGR_TAR_ID,
                                            dLiteralID = tariff.TARGR_LIT_ID,
                                            strDescription = oTariff.TAR_DESCRIPTION,
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            zones = new List<decimal>(),
                                            tariffType = (TariffType)oTariff.TAR_TYPE,
                                            maxPlates = oTariff.TAR_MAX_PLATES,
                                            permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                            tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                        };

                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                        ((List<decimal>)newTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);

                                        tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                        res.Add(newTariff);

                                    }
                                    else
                                    {
                                        stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];
                                        bool bNewUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1);
                                        if ((oldTariff.bUserSelectable != bNewUserSelectable) ||
                                            (oldTariff.dLiteralID != tariff.TARGR_LIT_ID))
                                        {
                                            stTariff newTariff = new stTariff
                                            {
                                                dID = tariff.TARGR_TAR_ID,
                                                dLiteralID = tariff.TARGR_LIT_ID,
                                                strDescription = oTariff.TAR_DESCRIPTION,
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                zones = new List<decimal>(),
                                                tariffType = (TariffType)oTariff.TAR_TYPE,
                                                maxPlates = oTariff.TAR_MAX_PLATES,
                                                permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                                tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                                ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                                ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                                tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                                tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                                tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                                tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                                tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                            };
                                            ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                            newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                            ((List<decimal>)newTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);

                                            tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                            res.Add(newTariff);

                                        }
                                        else
                                        {
                                            if (!((List<decimal>)oldTariff.zones).Exists(element => element == ((decimal)tariff.TARGR_GRP_ID)))
                                            {
                                                ((List<decimal>)oldTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);
                                            }
                                        }
                                    }

                                }

                            }
                            else if (tariff.TARGR_GRPT_ID != null)
                            {
                                bool bIsGroupInType = false;

                                foreach (GROUPS_TYPES_ASSIGNATION group_assig in getGROUPS_TYPES_ASSIGNATIONs().Where(r => r.GTA_GRPT_ID == tariff.TARGR_GRPT_ID))
                                {
                                    bIsGroupInType = (group_assig.GTA_GRP_ID == dGroupId);
                                    if (bIsGroupInType)
                                        break;

                                }

                                if (bIsGroupInType)
                                {

                                    if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                    {
                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.TARGR_TAR_ID,
                                            dLiteralID = tariff.TARGR_LIT_ID,
                                            strDescription = oTariff.TAR_DESCRIPTION,
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            zones = new List<decimal>(),
                                            tariffType = (TariffType)oTariff.TAR_TYPE,
                                            maxPlates = oTariff.TAR_MAX_PLATES,
                                            permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                            tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                        };
                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                        ((List<decimal>)newTariff.zones).Add(dGroupId);

                                        tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                        res.Add(newTariff);

                                    }
                                    else
                                    {
                                        stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];
                                        bool bNewUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1);
                                        if ((oldTariff.bUserSelectable != bNewUserSelectable) ||
                                            (oldTariff.dLiteralID != tariff.TARGR_LIT_ID))
                                        {
                                            stTariff newTariff = new stTariff
                                            {
                                                dID = tariff.TARGR_TAR_ID,
                                                dLiteralID = tariff.TARGR_LIT_ID,
                                                strDescription = oTariff.TAR_DESCRIPTION,
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                zones = new List<decimal>(),
                                                tariffType = (TariffType)oTariff.TAR_TYPE,
                                                maxPlates = oTariff.TAR_MAX_PLATES,
                                                permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                                tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                                ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                                ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                                tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                                tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                                tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                                tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                                tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),


                                            };
                                            ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                            newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                            ((List<decimal>)newTariff.zones).Add(dGroupId);

                                            tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                            res.Add(newTariff);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getGroupTariffs: ", e);
            }

            return (IEnumerable<stTariff>)res;

        }



        public IEnumerable<stTariff> getGroupTariffs(decimal dGroupId, decimal? dLatitude, decimal? dLongitude, decimal? lang)
        {

            List<stTariff> res = new List<stTariff>();

            try
            {

                DateTime? dtNow = getGroupDateTime(dGroupId);

                if (dtNow != null)
                {
                    GROUP oGroup = getGROUPs().Where(r => r.GRP_ID == dGroupId).FirstOrDefault();

                    Hashtable tariffHash = new Hashtable();

                    var Tariffs = (from g in getTARIFFS_IN_GROUPs()
                                   join t in getTARIFFs() on g.TARGR_TAR_ID equals t.TAR_ID
                                   where g.TARGR_INI_APPLY_DATE <= (DateTime)dtNow &&
                                         g.TARGR_END_APPLY_DATE >= (DateTime)dtNow &&
                                         t.TAR_INS_ID == oGroup.GRP_INS_ID
                                   orderby g.TARGR_TAR_ID
                                   select g).ToArray();

                    if (Tariffs.Count() > 0)
                    {
                        foreach (TARIFFS_IN_GROUP tariff in Tariffs)
                        {
                            var oTariff = getTARIFFs().Where(r => r.TAR_ID == tariff.TARGR_TAR_ID).FirstOrDefault();

                            if (tariff.TARGR_GRP_ID != null)
                            {
                                if (tariff.TARGR_GRP_ID == dGroupId)
                                {
                                    if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                    {
                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.TARGR_TAR_ID,
                                            dLiteralID = tariff.TARGR_LIT_ID,
                                            strDescription = oTariff.TAR_DESCRIPTION,
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            zones = new List<decimal>(),
                                            tariffType = (TariffType)oTariff.TAR_TYPE,
                                            maxPlates = oTariff.TAR_MAX_PLATES,
                                            permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                            tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                        };

                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                        ((List<decimal>)newTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);

                                        tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                        res.Add(newTariff);

                                    }
                                    else
                                    {
                                        stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];
                                        bool bNewUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1);
                                        if ((oldTariff.bUserSelectable != bNewUserSelectable) ||
                                            (oldTariff.dLiteralID != tariff.TARGR_LIT_ID))
                                        {
                                            stTariff newTariff = new stTariff
                                            {
                                                dID = tariff.TARGR_TAR_ID,
                                                dLiteralID = tariff.TARGR_LIT_ID,
                                                strDescription = oTariff.TAR_DESCRIPTION,
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                zones = new List<decimal>(),
                                                tariffType = (TariffType)oTariff.TAR_TYPE,
                                                maxPlates = oTariff.TAR_MAX_PLATES,
                                                permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                                tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                                ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                                ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                                tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                                tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                                tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                                tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                                tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                            };

                                            ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                            newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                            ((List<decimal>)newTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);

                                            tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                            res.Add(newTariff);

                                        }
                                        else
                                        {
                                            if (!((List<decimal>)oldTariff.zones).Exists(element => element == ((decimal)tariff.TARGR_GRP_ID)))
                                            {
                                                ((List<decimal>)oldTariff.zones).Add((decimal)tariff.TARGR_GRP_ID);
                                            }
                                        }
                                    }

                                }

                            }
                            else if (tariff.TARGR_GRPT_ID != null)
                            {
                                bool bIsGroupInType = false;

                                foreach (GROUPS_TYPES_ASSIGNATION group_assig in getGROUPS_TYPES_ASSIGNATIONs().Where(r => r.GTA_GRPT_ID == tariff.TARGR_GRPT_ID))
                                {
                                    bIsGroupInType = (group_assig.GTA_GRP_ID == dGroupId);
                                    if (bIsGroupInType)
                                        break;

                                }

                                if (bIsGroupInType)
                                {

                                    if (tariffHash[tariff.TARGR_TAR_ID] == null)
                                    {
                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.TARGR_TAR_ID,
                                            dLiteralID = tariff.TARGR_LIT_ID,
                                            strDescription = oTariff.TAR_DESCRIPTION,
                                            bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                            zones = new List<decimal>(),
                                            tariffType = (TariffType)oTariff.TAR_TYPE,
                                            maxPlates = oTariff.TAR_MAX_PLATES,
                                            permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                            tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                        };

                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);


                                        ((List<decimal>)newTariff.zones).Add(dGroupId);

                                        tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                        res.Add(newTariff);

                                    }
                                    else
                                    {
                                        stTariff oldTariff = (stTariff)tariffHash[tariff.TARGR_TAR_ID];
                                        bool bNewUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1);
                                        if ((oldTariff.bUserSelectable != bNewUserSelectable) ||
                                            (oldTariff.dLiteralID != tariff.TARGR_LIT_ID))
                                        {
                                            stTariff newTariff = new stTariff
                                            {
                                                dID = tariff.TARGR_TAR_ID,
                                                dLiteralID = tariff.TARGR_LIT_ID,
                                                strDescription = oTariff.TAR_DESCRIPTION,
                                                bUserSelectable = (tariff.TARGR_USER_SELECTABLE == 1),
                                                zones = new List<decimal>(),
                                                tariffType = (TariffType)oTariff.TAR_TYPE,
                                                maxPlates = oTariff.TAR_MAX_PLATES,
                                                permitMaxNum = oTariff.TAR_MAX_PERMITS_ONCE ?? 1,
                                                tariffBehavior = (TariffBehavior)oTariff.TAR_BEHAVIOR,
                                                ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MIN_VERSION),
                                                ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(oTariff.TAR_APP_MAX_VERSION),
                                                tariffAutoStart = (oTariff.TAR_AUTOSTART.HasValue ? (int?)oTariff.TAR_AUTOSTART.Value : null),
                                                tariffRestartTariff = (oTariff.TAR_RESTART_TARIFF.HasValue ? (int?)oTariff.TAR_RESTART_TARIFF.Value : null),
                                                tariffServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (decimal?)oTariff.TAR_SERTYP_ID.Value : null),
                                                tariffTypeOfServiceType = (oTariff.TAR_SERTYP_ID.HasValue ? (int?)getSERVICE_TYPEs().Where(r => r.SERTYP_ID == oTariff.TAR_SERTYP_ID.Value).FirstOrDefault().SERTYP_TYPE_TYPESERVICE_ID : null),
                                                tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(oTariff.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                            };
                                            ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                            newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARGR_TAR_ID).FirstOrDefault(), lang, tariff.TARGR_TAR_ID);

                                            ((List<decimal>)newTariff.zones).Add(dGroupId);

                                            tariffHash[tariff.TARGR_TAR_ID] = newTariff;
                                            res.Add(newTariff);

                                        }
                                    }
                                }
                            }
                        }
                    }


                    if ((res.Count() > 0) && (dLatitude.HasValue) && (dLongitude.HasValue))
                    {
                        List<stTariff> lstTemp = res;
                        res = new List<stTariff>();

                        foreach (stTariff tariff in lstTemp)
                        {

                            bool bIsInside = false;
                            var PolygonNumbers = (from r in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                  where ((r.TAGRGE_GRP_ID == dGroupId) &&
                                                  (r.TAGRGE_TAR_ID == tariff.dID) &&
                                                  (r.TAGRGE_INI_APPLY_DATE <= (DateTime)dtNow) &&
                                                  (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow))
                                                  group r by new
                                                  {
                                                      r.TAGRGE_POL_NUMBER
                                                  } into g
                                                  orderby g.Key.TAGRGE_POL_NUMBER
                                                  select new { iPolNumber = g.Key.TAGRGE_POL_NUMBER }).ToList();


                            if (PolygonNumbers.Count() > 0)
                            {
                                Point p = new Point(Convert.ToDouble(dLongitude), Convert.ToDouble(dLatitude));

                                foreach (var oPolNumber in PolygonNumbers)
                                {
                                    var Polygon = (from r in getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                   where ((r.TAGRGE_GRP_ID == dGroupId) &&
                                                   (r.TAGRGE_TAR_ID == tariff.dID) &&
                                                   (r.TAGRGE_INI_APPLY_DATE <= (DateTime)dtNow) &&
                                                   (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow) &&
                                                   (r.TAGRGE_POL_NUMBER == oPolNumber.iPolNumber))
                                                   orderby r.TAGRGE_ORDER
                                                   select new Point(Convert.ToDouble(r.TAGRGE_LONGITUDE),
                                                                    Convert.ToDouble(r.TAGRGE_LATITUDE))).ToArray();


                                    if (Polygon.Count() > 0)
                                    {
                                        if (IsPointInsidePolygon(p, Polygon))
                                        {
                                            bIsInside = true;
                                            break;
                                        }

                                    }

                                }
                            }
                            else
                            {
                                //no poligons defined for tariff in group so not filter by gps position
                                bIsInside = true;
                            }


                            if (bIsInside)
                            {
                                res.Add(tariff);
                            }
                        }


                        if (res.Count() == 0)
                        {
                            res = lstTemp;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getGroupTariffs: ", e);
            }

            return (IEnumerable<stTariff>)res;

        }


        public IEnumerable<stTariff> getPlateTariffsInGroup(string strPlate, decimal dGroupId, decimal? dLatitude, decimal? dLongitude, decimal? lang)
        {

            List<stTariff> res = new List<stTariff>();

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

                    DateTime? dtNow = getGroupDateTime(dGroupId);

                    if (dtNow != null)
                    {

                        var Tariffs = (from g in dbContext.PLATES_TARIFFs
                                       where g.PLTA_INI_APPLY_DATE <= (DateTime)dtNow &&
                                             g.PLTA_END_APPLY_DATE >= (DateTime)dtNow &&
                                             g.PLTA_PLATE.ToUpper().Trim() == strPlate.ToUpper().Trim()
                                       orderby g.PLTA_TAR_ID
                                       select g).ToArray();

                        if (Tariffs.Count() > 0)
                        {
                            foreach (PLATES_TARIFF tariff in Tariffs)
                            {
                                if (tariff.PLTA_GRP_ID != null)
                                {
                                    if (tariff.PLTA_GRP_ID == dGroupId)
                                    {

                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.PLTA_TAR_ID,
                                            dLiteralID = tariff.TARIFF.TAR_LIT_ID,
                                            strDescription = tariff.TARIFF.TAR_DESCRIPTION,
                                            bUserSelectable = false,
                                            zones = new List<decimal>(),
                                            tariffType = (TariffType)tariff.TARIFF.TAR_TYPE,
                                            maxPlates = tariff.TARIFF.TAR_MAX_PLATES,                                            
                                            permitMaxNum = tariff.TARIFF.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)tariff.TARIFF.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(tariff.TARIFF.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(tariff.TARIFF.TAR_APP_MAX_VERSION),
                                            tariffAutoStart = (tariff.TARIFF.TAR_AUTOSTART.HasValue ? (int?)tariff.TARIFF.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (tariff.TARIFF.TAR_RESTART_TARIFF.HasValue ? (int?)tariff.TARIFF.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (tariff.TARIFF.TAR_SERTYP_ID.HasValue ? (decimal?)tariff.TARIFF.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (tariff.TARIFF.SERVICES_TYPE != null ? (int?)tariff.TARIFF.SERVICES_TYPE.SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(tariff.TARIFF.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                        };
                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARIFF.TAR_ID).FirstOrDefault(), lang, tariff.TARIFF.TAR_ID);

                                        ((List<decimal>)newTariff.zones).Add((decimal)tariff.PLTA_GRP_ID);

                                        res.Add(newTariff);

                                        break;

                                    }

                                }
                                else if (tariff.PLTA_GRPT_ID != null)
                                {
                                    bool bIsGroupInType = false;

                                    foreach (GROUPS_TYPES_ASSIGNATION group_assig in tariff.GROUPS_TYPE.GROUPS_TYPES_ASSIGNATIONs)
                                    {
                                        bIsGroupInType = (group_assig.GTA_GRP_ID == dGroupId);
                                        if (bIsGroupInType)
                                            break;

                                    }

                                    if (bIsGroupInType)
                                    {

                                        stTariff newTariff = new stTariff
                                        {
                                            dID = tariff.PLTA_TAR_ID,
                                            dLiteralID = tariff.TARIFF.TAR_LIT_ID,
                                            strDescription = tariff.TARIFF.TAR_DESCRIPTION,
                                            bUserSelectable = false,
                                            zones = new List<decimal>(),
                                            tariffType = (TariffType)tariff.TARIFF.TAR_TYPE,
                                            maxPlates = tariff.TARIFF.TAR_MAX_PLATES,                                            
                                            permitMaxNum = tariff.TARIFF.TAR_MAX_PERMITS_ONCE ?? 1,
                                            tariffBehavior = (TariffBehavior)tariff.TARIFF.TAR_BEHAVIOR,
                                            ulMinVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(tariff.TARIFF.TAR_APP_MIN_VERSION),
                                            ulMaxVersion = integraMobile.Infrastructure.AppUtilities.AppVersion(tariff.TARIFF.TAR_APP_MAX_VERSION),
                                            tariffAutoStart = (tariff.TARIFF.TAR_AUTOSTART.HasValue ? (int?)tariff.TARIFF.TAR_AUTOSTART.Value : null),
                                            tariffRestartTariff = (tariff.TARIFF.TAR_RESTART_TARIFF.HasValue ? (int?)tariff.TARIFF.TAR_RESTART_TARIFF.Value : null),
                                            tariffServiceType = (tariff.TARIFF.TAR_SERTYP_ID.HasValue ? (decimal?)tariff.TARIFF.TAR_SERTYP_ID.Value : null),
                                            tariffTypeOfServiceType = (tariff.TARIFF.SERVICES_TYPE != null ? (int?)tariff.TARIFF.SERVICES_TYPE.SERTYP_TYPE_TYPESERVICE_ID : null),
                                            tariffShopkeeperBehavior = (TariffShopkeeperBehavior)(tariff.TARIFF.TAR_SHOPKEEPER_BEHAVIOR_TYPE ?? 0),

                                        };
                                        ///TO DO: Se agregan aqui los mensajes de los botones si existe un servicio asociado a una tarifa
                                        newTariff = AddTariffsCustomMessage(newTariff, getTARIFFS_CUSTOM_MESSAGEs(tariff.TARIFF.TAR_ID).FirstOrDefault(), lang, tariff.TARIFF.TAR_ID);

                                        ((List<decimal>)newTariff.zones).Add(dGroupId);

                                        res.Add(newTariff);

                                        break;

                                    }
                                }
                            }
                        }

                        if ((res.Count() > 0) && (dLatitude.HasValue) && (dLongitude.HasValue))
                        {
                            List<stTariff> lstTemp = res;
                            res = new List<stTariff>();

                            foreach (stTariff tariff in lstTemp)
                            {

                                bool bIsInside = false;
                                var PolygonNumbers = (from r in dbContext.TARIFFS_IN_GROUPS_GEOMETRies
                                                      where ((r.TAGRGE_GRP_ID == dGroupId) &&
                                                      (r.TAGRGE_TAR_ID == tariff.dID) &&
                                                      (r.TAGRGE_INI_APPLY_DATE <= (DateTime)dtNow) &&
                                                      (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow))
                                                      group r by new
                                                      {
                                                          r.TAGRGE_POL_NUMBER
                                                      } into g
                                                      orderby g.Key.TAGRGE_POL_NUMBER
                                                      select new { iPolNumber = g.Key.TAGRGE_POL_NUMBER }).ToList();


                                if (PolygonNumbers.Count() > 0)
                                {
                                    Point p = new Point(Convert.ToDouble(dLongitude), Convert.ToDouble(dLatitude));

                                    foreach (var oPolNumber in PolygonNumbers)
                                    {
                                        var Polygon = (from r in dbContext.TARIFFS_IN_GROUPS_GEOMETRies
                                                       where ((r.TAGRGE_GRP_ID == dGroupId) &&
                                                       (r.TAGRGE_TAR_ID == tariff.dID) &&
                                                       (r.TAGRGE_INI_APPLY_DATE <= (DateTime)dtNow) &&
                                                       (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow) &&
                                                       (r.TAGRGE_POL_NUMBER == oPolNumber.iPolNumber))
                                                       orderby r.TAGRGE_ORDER
                                                       select new Point(Convert.ToDouble(r.TAGRGE_LONGITUDE),
                                                                        Convert.ToDouble(r.TAGRGE_LATITUDE))).ToArray();


                                        if (Polygon.Count() > 0)
                                        {
                                            if (IsPointInsidePolygon(p, Polygon))
                                            {
                                                bIsInside = true;
                                                break;
                                            }

                                        }

                                    }
                                }
                                else
                                {
                                    //no poligons defined for tariff in group so not filter by gps position
                                    bIsInside = true;
                                }


                                if (bIsInside)
                                {
                                    res.Add(tariff);
                                }
                            }
                        }


                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getPlateTariffsInGroup: ", e);
            }

            return (IEnumerable<stTariff>)res;

        }





        public bool GetGroupAndTariffExternalTranslation(int iWSNumber,GROUP oGroup, TARIFF oTariff, ref string strExtGroupId, ref string strTarExtId)
        {
            bool bRes = false;
            strExtGroupId = "";
            strTarExtId = "";
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

                    if ((oGroup != null) && (oTariff != null))
                    {

                        if ((iWSNumber == 0) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_QUERY_EXT_ID)) &&
                            (!string.IsNullOrEmpty(oTariff.TAR_QUERY_EXT_ID)))
                        {
                            strExtGroupId = oGroup.GRP_QUERY_EXT_ID;
                            strTarExtId = oTariff.TAR_QUERY_EXT_ID;
                            bRes = true;
                        }
                        else if ((iWSNumber == 1) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_EXT1_ID)) &&
                            (!string.IsNullOrEmpty(oTariff.TAR_EXT1_ID)))
                        {
                            strExtGroupId = oGroup.GRP_EXT1_ID;
                            strTarExtId = oTariff.TAR_EXT1_ID;
                            bRes = true;
                        }
                        else if ((iWSNumber == 2) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_EXT2_ID)) &&
                            (!string.IsNullOrEmpty(oTariff.TAR_EXT2_ID)))
                        {
                            strExtGroupId = oGroup.GRP_EXT2_ID;
                            strTarExtId = oTariff.TAR_EXT2_ID;
                            bRes = true;
                        }
                        else if ((iWSNumber == 3) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_EXT3_ID)) &&
                            (!string.IsNullOrEmpty(oTariff.TAR_EXT3_ID)))
                        {
                            strExtGroupId = oGroup.GRP_EXT3_ID;
                            strTarExtId = oTariff.TAR_EXT3_ID;
                            bRes = true;
                        }
                        else
                        {

                            var oGroupTranslation = (from r in dbContext.GROUPS_TARIFFS_EXTERNAL_TRANSLATIONs
                                                     where r.GTET_IN_GRP_ID == oGroup.GRP_ID &&
                                                           r.GTET_IN_TAR_ID == oTariff.TAR_ID &&
                                                           r.GTET_WS_NUMBER == iWSNumber
                                                     select r).ToArray();

                            if (oGroupTranslation.Count() == 1)
                            {
                                strExtGroupId = oGroupTranslation.First().GTET_OUT_GRP_EXT_ID;
                                strTarExtId = oGroupTranslation.First().GTET_OUT_TAR_EXT_ID;
                                bRes = ((!string.IsNullOrEmpty(strExtGroupId)) &&
                                        (!string.IsNullOrEmpty(strTarExtId)));

                            }


                        }
                    }
                    else if ((oGroup != null) && (oTariff == null))
                    {
                        if ((iWSNumber == 0) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_QUERY_EXT_ID)))
                        {
                            strExtGroupId = oGroup.GRP_QUERY_EXT_ID;
                            bRes = true;
                        }
                        else if ((iWSNumber == 1) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_EXT1_ID)))
                        {
                            strExtGroupId = oGroup.GRP_EXT1_ID;
                            bRes = true;
                        }
                        else if ((iWSNumber == 2) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_EXT2_ID)))
                        {
                            strExtGroupId = oGroup.GRP_EXT2_ID;
                            bRes = true;
                        }
                        else if ((iWSNumber == 3) &&
                            (!string.IsNullOrEmpty(oGroup.GRP_EXT3_ID)))
                        {
                            strExtGroupId = oGroup.GRP_EXT3_ID;
                            bRes = true;
                        }
                        else
                        {

                            var oGroupTranslation = (from r in dbContext.GROUPS_TARIFFS_EXTERNAL_TRANSLATIONs
                                                     where r.GTET_IN_GRP_ID == oGroup.GRP_ID &&
                                                           r.GTET_IN_TAR_ID == null &&
                                                           r.GTET_WS_NUMBER == iWSNumber
                                                     select r).ToArray();

                            if (oGroupTranslation.Count() == 1)
                            {
                                strExtGroupId = oGroupTranslation.First().GTET_OUT_GRP_EXT_ID;
                                strTarExtId = oGroupTranslation.First().GTET_OUT_TAR_EXT_ID;
                                bRes = ((!string.IsNullOrEmpty(strExtGroupId)) &&
                                        (!string.IsNullOrEmpty(strTarExtId)));

                            }


                        }

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetGroupAndTariffExternalTranslation: ", e);
                bRes = false;
            }

            return bRes;

        }


        public bool GetGroupAndTariffExternalTranslation(int iWSNumber, decimal dGroupId,decimal dTariffId, ref string strExtGroupId, ref string strTarExtId)
        {
            bool bRes = false;
            strExtGroupId = "";
            strTarExtId = "";
            try
            {
                GROUP oGroup = null;
                TARIFF oTariff = null;

                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    oGroup = (from g in dbContext.GROUPs
                                    where g.GRP_ID == dGroupId
                                    select g).First();

                    oTariff = (from g in dbContext.TARIFFs
                                    where g.TAR_ID == dTariffId
                                    select g).First();


                }

                return GetGroupAndTariffExternalTranslation(iWSNumber, oGroup, oTariff, ref strExtGroupId, ref strTarExtId);

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetGroupAndTariffExternalTranslation: ", e);
                bRes = false;
            }

            return bRes;

        }





        public bool GetGroupAndTariffFromExternalId(int iWSNumber, DateTime dt, INSTALLATION oInstallation, string strExtGroupId, string strExtTarId, ref decimal? dGroupId, ref decimal? dTariffId)
        {
            bool bRes = false;
            dGroupId = null;
            dTariffId = null;

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

                    GROUP[] oCandidateGroup = null;

                    switch (iWSNumber)
                    {
                        case 0:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_QUERY_EXT_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 1:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_EXT1_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 2:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_EXT2_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 3:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_EXT3_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 4:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_ID_FOR_EXT_OPS == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        default:
                            break;

                    }



                    if (oCandidateGroup.Count() == 1)
                    {

                        var oCandidateTariffs = oCandidateGroup.First().TARIFFS_IN_GROUPs.Where(r=> r.TARGR_INI_APPLY_DATE<= dt && r.TARGR_END_APPLY_DATE>dt).ToArray();

                        foreach (TARIFFS_IN_GROUP oCandidateTariff in oCandidateTariffs)
                        {
                            bool bFound = false;
                            switch (iWSNumber)
                            {
                                case 0:
                                    bFound = (oCandidateTariff.TARIFF.TAR_QUERY_EXT_ID == strExtTarId);
                                    break;
                                case 1:
                                    bFound = (oCandidateTariff.TARIFF.TAR_EXT1_ID == strExtTarId);
                                    break;
                                case 2:
                                    bFound = (oCandidateTariff.TARIFF.TAR_EXT2_ID == strExtTarId);
                                    break;
                                case 3:
                                    bFound = (oCandidateTariff.TARIFF.TAR_EXT3_ID == strExtTarId);
                                    break;
                                case 4:
                                    bFound = (oCandidateTariff.TARIFF.TAR_ID_FOR_EXT_OPS == strExtTarId);
                                    break;
                                default:
                                    break;

                            }


                            if (bFound)
                            {
                                bRes = true;
                                dGroupId = oCandidateGroup.First().GRP_ID;
                                dTariffId = oCandidateTariff.TARIFF.TAR_ID;
                                break;
                            }

                        }


                        if (!bRes)
                        {
                            foreach (GROUPS_TYPES_ASSIGNATION oAssigns in oCandidateGroup.First().GROUPS_TYPES_ASSIGNATIONs)
                            {

                                oCandidateTariffs = oAssigns.GROUPS_TYPE.TARIFFS_IN_GROUPs.Where(r => r.TARGR_INI_APPLY_DATE <= dt && r.TARGR_END_APPLY_DATE > dt).ToArray();

                                foreach (TARIFFS_IN_GROUP oCandidateTariff in oCandidateTariffs)
                                {

                                    bool bFound = false;
                                    switch (iWSNumber)
                                    {
                                        case 0:
                                            bFound = (oCandidateTariff.TARIFF.TAR_QUERY_EXT_ID == strExtTarId);
                                            break;
                                        case 1:
                                            bFound = (oCandidateTariff.TARIFF.TAR_EXT1_ID == strExtTarId);
                                            break;
                                        case 2:
                                            bFound = (oCandidateTariff.TARIFF.TAR_EXT2_ID == strExtTarId);
                                            break;
                                        case 3:
                                            bFound = (oCandidateTariff.TARIFF.TAR_EXT3_ID == strExtTarId);
                                            break;
                                        case 4:
                                            bFound = (oCandidateTariff.TARIFF.TAR_ID_FOR_EXT_OPS == strExtTarId);
                                            break;
                                        default:
                                            break;

                                    }

                                    if (bFound)
                                    {
                                        bRes = true;
                                        dGroupId = oCandidateGroup.First().GRP_ID;
                                        dTariffId = oCandidateTariff.TARIFF.TAR_ID;
                                        break;
                                    }

                                }

                                if (bRes)
                                {
                                    break;
                                }
                            }


                        }


                    }



                    if (!bRes)
                    {
                        var oGroupTranslations = (from r in dbContext.GROUPS_TARIFFS_EXTERNAL_TRANSLATIONs
                                                  where r.GTET_OUT_GRP_EXT_ID == strExtGroupId &&
                                                        r.GTET_OUT_TAR_EXT_ID == strExtTarId &&
                                                        r.GTET_WS_NUMBER == iWSNumber
                                                  select r).ToArray();

                        if (oGroupTranslations.Count() == 1)
                        {
                            dGroupId = oGroupTranslations.First().GTET_IN_GRP_ID;
                            dTariffId = oGroupTranslations.First().GTET_IN_TAR_ID;
                            bRes = true;


                        }
                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetGroupAndTariffFromExternalId: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool GetStreetSectionFromExternalId(decimal dInstallationId, string sExtStreetSectionId, ref decimal? dStreetSectionId)
        {
            bool bRes = false;
            dStreetSectionId = null;

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

                    var oStreetSection = (from r in dbContext.STREET_SECTIONs
                                          where r.STRSE_ID_EXT == sExtStreetSectionId &&
                                                r.INSTALLATION.INS_ID == dInstallationId
                                          select r).FirstOrDefault();

                    if (oStreetSection != null)
                    {
                        dStreetSectionId = oStreetSection.STRSE_ID;
                        bRes = true;
                    }
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetStreetSectionFromExternalId: ", e);
                bRes = false;
            }

            return bRes;

        }


        public bool GetGroupAndTariffFromExternalId(int iWSNumber, DateTime dt, INSTALLATION oInstallation, string strExtGroupId, string strExtTarId, decimal dInTariffId, ref decimal? dGroupId, ref decimal? dTariffId)
        {
            bool bRes = false;
            dGroupId = null;
            dTariffId = null;

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

                    GROUP[] oCandidateGroup = null;

                    switch (iWSNumber)
                    {
                        case 0:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_QUERY_EXT_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 1:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_EXT1_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 2:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_EXT2_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 3:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_EXT3_ID == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        case 4:
                            oCandidateGroup = oInstallation.GROUPs.Where(r => r.GRP_ID_FOR_EXT_OPS == strExtGroupId && r.GRP_INS_ID == oInstallation.INS_ID).ToArray();
                            break;
                        default:
                            break;

                    }



                    if (oCandidateGroup.Count() == 1)
                    {

                        var oCandidateTariffs = oCandidateGroup.First().TARIFFS_IN_GROUPs.Where(r => r.TARGR_INI_APPLY_DATE <= dt && r.TARGR_END_APPLY_DATE > dt).ToArray();

                        foreach (TARIFFS_IN_GROUP oCandidateTariff in oCandidateTariffs)
                        {
                            bool bFound = false;
                            switch (iWSNumber)
                            {
                                case 0:
                                    bFound = (oCandidateTariff.TARIFF.TAR_QUERY_EXT_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                    break;
                                case 1:
                                    bFound = (oCandidateTariff.TARIFF.TAR_EXT1_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                    break;
                                case 2:
                                    bFound = (oCandidateTariff.TARIFF.TAR_EXT2_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                    break;
                                case 3:
                                    bFound = (oCandidateTariff.TARIFF.TAR_EXT3_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                    break;
                                case 4:
                                    bFound = (oCandidateTariff.TARIFF.TAR_ID_FOR_EXT_OPS == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                    break;
                                default:
                                    break;

                            }


                            if (bFound)
                            {
                                bRes = true;
                                dGroupId = oCandidateGroup.First().GRP_ID;
                                dTariffId = oCandidateTariff.TARIFF.TAR_ID;
                                break;
                            }

                        }


                        if (!bRes)
                        {
                            foreach (GROUPS_TYPES_ASSIGNATION oAssigns in oCandidateGroup.First().GROUPS_TYPES_ASSIGNATIONs)
                            {

                                oCandidateTariffs = oAssigns.GROUPS_TYPE.TARIFFS_IN_GROUPs.Where(r => r.TARGR_INI_APPLY_DATE <= dt && r.TARGR_END_APPLY_DATE > dt).ToArray();

                                foreach (TARIFFS_IN_GROUP oCandidateTariff in oCandidateTariffs)
                                {

                                    bool bFound = false;
                                    switch (iWSNumber)
                                    {
                                        case 0:
                                            bFound = (oCandidateTariff.TARIFF.TAR_QUERY_EXT_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                            break;
                                        case 1:
                                            bFound = (oCandidateTariff.TARIFF.TAR_EXT1_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                            break;
                                        case 2:
                                            bFound = (oCandidateTariff.TARIFF.TAR_EXT2_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                            break;
                                        case 3:
                                            bFound = (oCandidateTariff.TARIFF.TAR_EXT3_ID == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                            break;
                                        case 4:
                                            bFound = (oCandidateTariff.TARIFF.TAR_ID_FOR_EXT_OPS == strExtTarId && oCandidateTariff.TARIFF.TAR_ID == dInTariffId);
                                            break;
                                        default:
                                            break;

                                    }

                                    if (bFound)
                                    {
                                        bRes = true;
                                        dGroupId = oCandidateGroup.First().GRP_ID;
                                        dTariffId = oCandidateTariff.TARIFF.TAR_ID;
                                        break;
                                    }

                                }

                                if (bRes)
                                {
                                    break;
                                }
                            }


                        }


                    }



                    if (!bRes)
                    {
                        var oGroupTranslations = (from r in dbContext.GROUPS_TARIFFS_EXTERNAL_TRANSLATIONs
                                                  where r.GTET_OUT_GRP_EXT_ID == strExtGroupId &&
                                                        r.GTET_OUT_TAR_EXT_ID == strExtTarId &&
                                                        r.GTET_WS_NUMBER == iWSNumber
                                                  select r).ToArray();

                        if (oGroupTranslations.Count() == 1)
                        {
                            dGroupId = oGroupTranslations.First().GTET_IN_GRP_ID;
                            dTariffId = oGroupTranslations.First().GTET_IN_TAR_ID;
                            bRes = true;


                        }
                    }

                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetGroupAndTariffFromExternalId: ", e);
                bRes = false;
            }

            return bRes;

        }


        public bool GetGroupAndTariffStepOffsetMinutes(GROUP oGroup, TARIFF oTariff, out int? iOffset)
        {
            bool bReturn = true;
            iOffset = null;

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
                    decimal dInstallationId = oGroup.INSTALLATION.INS_ID;
                    DateTime? dtNow = getInstallationDateTime(dInstallationId);

                    if (dtNow != null)
                    {

                        Hashtable tariffHash = new Hashtable();

                        var Tariffs = (from g in dbContext.TARIFFS_IN_GROUPs
                                       where g.TARGR_INI_APPLY_DATE <= (DateTime)dtNow &&
                                             g.TARGR_END_APPLY_DATE >= (DateTime)dtNow &&
                                             g.TARIFF.TAR_ID == oTariff.TAR_ID
                                       orderby g.TARGR_TAR_ID,
                                               g.TARGR_GRP_ID.HasValue descending, g.TARGR_GRP_ID,
                                               g.TARGR_GRPT_ID
                                       select g).ToArray();

                        if (Tariffs.Count() > 0)
                        {
                            foreach (TARIFFS_IN_GROUP tariff in Tariffs)
                            {
                                if (tariff.TARGR_GRP_ID != null)
                                {
                                    if (tariff.GROUP.GRP_INS_ID == dInstallationId)
                                    {

                                        if (tariff.TARGR_GRP_ID == oGroup.GRP_ID)
                                        {
                                            iOffset = tariff.TARGR_TIME_STEPS_VALUE;
                                            break;
                                        }
                                    }

                                }
                                else if (tariff.TARGR_GRPT_ID != null)
                                {
                                    if (tariff.GROUPS_TYPE.GRPT_INS_ID == dInstallationId)
                                    {

                                        var oGroupTypes = oGroup.GROUPS_TYPES_ASSIGNATIONs.Where(r => r.GTA_GRPT_ID == tariff.GROUPS_TYPE.GRPT_ID);

                                        if (oGroupTypes.Count() > 0)
                                        {
                                            iOffset = tariff.TARGR_TIME_STEPS_VALUE;
                                            break;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetGroupAndTariffStepOffsetMinutes: ", e);
                bReturn=false;
            }

            return bReturn;

        }


        public bool GetSyncInstallation(long lVersionFrom, out INSTALLATIONS_SYNC[] oArrSync)
        {
            bool bRes = false;
            oArrSync = null;


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


                    oArrSync = (from r in dbContext.INSTALLATIONS_SYNCs
                                orderby r.INS_VERSION
                                where r.INS_VERSION > lVersionFrom
                                select r).ToArray();


                    bRes = true;


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSyncInstallation: ", e);
            }

            return bRes;

        }

        public bool GetSyncInstallationGeometry(long lVersionFrom, out INSTALLATIONS_GEOMETRY_SYNC[] oArrSync, int ?iMaxRegistriesToReturn)
        {
            bool bRes = false;
            oArrSync = null;


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

                    if (iMaxRegistriesToReturn.HasValue)
                    {

                        oArrSync = (from r in dbContext.INSTALLATIONS_GEOMETRY_SYNCs
                                    orderby r.INSGE_VERSION
                                    where r.INSGE_VERSION > lVersionFrom
                                    select r).Take(iMaxRegistriesToReturn.Value).ToArray();
                    }
                    else
                    {
                        oArrSync = (from r in dbContext.INSTALLATIONS_GEOMETRY_SYNCs
                                    orderby r.INSGE_VERSION
                                    where r.INSGE_VERSION > lVersionFrom
                                    select r).ToArray();

                    }



                    bRes = true;


                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSyncInstallationGeometry: ", e);
            }

            return bRes;

        }



        public decimal GetSyncInstallationCurrentVersion()
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
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();


                    dRes = dbContext.INSTALLATIONS_SYNCs.Max(r => r.INS_VERSION);
                   
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSyncInstallationCurrentVersion: ", e);
            }

            return dRes;

        }

        public decimal GetSyncInstallationGeometryCurrentVersion()
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
                    integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();

                    dRes = dbContext.INSTALLATIONS_GEOMETRY_SYNCs.Max(r => r.INSGE_VERSION);

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSyncInstallationGeometryCurrentVersion: ", e);
            }

            return dRes;

        }

        public List<INSTALLATIONS_GEOMETRY> GetInstallationsGeometries(decimal dInstallationId, int? iMaxRegistriesToReturn = null) 
        {
            var installationGeometriesResult = new List<INSTALLATIONS_GEOMETRY>();

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

                    if (iMaxRegistriesToReturn.HasValue)
                    {

                        installationGeometriesResult = (from r in dbContext.INSTALLATIONS_GEOMETRies
                                    where r.INSGE_INS_ID == dInstallationId
                                                        select r).Take(iMaxRegistriesToReturn.Value).ToList();
                    }
                    else
                    {
                        installationGeometriesResult = (from r in dbContext.INSTALLATIONS_GEOMETRies
                                                        where r.INSGE_INS_ID == dInstallationId
                                                        select r).ToList();

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInstallationsGeometries: ", e);
            }

            return installationGeometriesResult;
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



        private bool getInstallationGroupChilds(DateTime dtNow,
                                                int ilevel,
                                                List<GroupType> groupTypes,
                                                int? lang,
                                                ref stZone zone,
                                                bool filterOnlyPermitRatesGroups)
        {
            bool bRes = true;
            try
            {

                decimal zoneid = zone.dID;

                var Groups = (from g in getGROUPs()
                                       join gh in getGROUP_HIERARCHIEs() on g.GRP_ID equals gh.GRHI_GPR_ID_CHILD
                                        where gh.GRHI_GPR_ID_PARENT == zoneid &&
                                              gh.GRHI_INI_APPLY_DATE <= (DateTime)dtNow &&
                                              gh.GRHI_END_APPLY_DATE >= (DateTime)dtNow &&
                                              groupTypes.Contains((GroupType)g.GRP_TYPE)
                                        orderby g.GRP_ID
                                       select g).ToArray();

                if (Groups.Count() > 0)
                {
                    foreach (GROUP group in Groups)
                    {
                        decimal dInsId= group.GRP_INS_ID;

                        stZone newZone = new stZone
                        {
                            level = ilevel,
                            dID = group.GRP_ID,
                            strDescription = group.GRP_DESCRIPTION,
                            dLiteralID = group.GRP_LIT_ID,
                            strColour = group.GRP_COLOUR,
                            strShowId = group.GRP_SHOW_ID,
                            subzones = new List<stZone>(),
                            GPSpolygons = new List<stGPSPolygon>(),
                            GroupType = (GroupType)group.GRP_TYPE,
                            Occupancy = (float)(100 - (group.GRP_FREE_SPACES_PERC ?? 0)),
                            center = new stGPSPoint(),
                            allowByPassMap = (getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                                .Where(r => (r.TAGRGE_GRP_ID == group.GRP_ID) &&
                                                   (r.TAGRGE_INI_APPLY_DATE < (DateTime)dtNow) &&
                                                   (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow)).Count() == 0),
                            permitMaxMonths = group.GRP_PERMIT_MAX_MONTHS,
                            dMessageLiteralID = group.GRP_MESSAGE_LIT_ID,
                            permitMaxBuyDay = group.GRP_PERMIT_MAX_CUR_MONTH_DAY,
                        };

                        if (newZone.dMessageLiteralID.HasValue)
                        {
                            if (lang.HasValue)
                            {
                                newZone.message = getGroupMessage(newZone.dMessageLiteralID.Value, lang.Value);
                            }
                        }

                        int iNumPoligons = 0;
                        foreach (int iPolNumber in getGROUP_GEOMETRIEs()
                                                             .Where(r => r.GRGE_GRP_ID == group.GRP_ID &&
                                                                         r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                                         r.GRGE_END_APPLY_DATE >= (DateTime)dtNow)
                                                             .GroupBy(r => r.GRGE_POL_NUMBER)
                                                             .OrderBy(r => r.Key)
                                                             .Select(r => r.Key))
                        {

                            stGPSPolygon oGPSPolygon = new stGPSPolygon();
                            oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                            oGPSPolygon.iPolNumber = iPolNumber;


                            foreach (GROUPS_GEOMETRY oGeometry in getGROUP_GEOMETRIEs()
                                    .Where(r => r.GRGE_GRP_ID == group.GRP_ID &&
                                                r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                r.GRGE_END_APPLY_DATE >= (DateTime)dtNow &&
                                                r.GRGE_POL_NUMBER == iPolNumber)
                                    .OrderBy(r => r.GRGE_ORDER))
                            {
                                stGPSPoint gpsPoint = new stGPSPoint
                                {
                                    order = oGeometry.GRGE_ORDER,
                                    dLatitude = oGeometry.GRGE_LATITUDE,
                                    dLongitude = oGeometry.GRGE_LONGITUDE
                                };
                                ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                            }

                            ((List<stGPSPolygon>)newZone.GPSpolygons).Add(oGPSPolygon);

                            if (iNumPoligons == 0)
                            {
                                PolygonCentroid(group, dtNow, (List<stGPSPoint>)(oGPSPolygon.GPSpolygon), ref newZone.center);
                            }

                            iNumPoligons++;
                        }


                       

                        getInstallationGroupChilds(dtNow,
                                                    ilevel + 1,
                                                    groupTypes,
                                                    lang,
                                                    ref newZone,
                                                    filterOnlyPermitRatesGroups);



                        bool bAddZone = true;

                        if ((filterOnlyPermitRatesGroups) && (newZone.subzones.Count() == 0))
                        {
                            IEnumerable<stTariff> zoneTariffs = getGroupTariffs(group.GRP_ID, lang);
                            bAddZone = zoneTariffs.Where(r => r.tariffType == (int)TariffType.RegularTariff).Any();
                            if (!bAddZone)
                            {
                                var installation = (from i in getINSTALLATIONs()
                                                    where i.INS_ID == dInsId
                                                    select i).FirstOrDefault();

                                if (installation != null)
                                {
                                    if (installation.INS_MAP_SCREEN_TYPE == 1)
                                    {
                                        bAddZone = true;
                                    }
                                }

                            }

                        }


                        if (bAddZone)
                        {
                            ((List<stZone>)zone.subzones).Add(newZone);
                        }

                    }

                }
               

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationGroupChilds: ", e);
                bRes = false;
            }

            return bRes;

        }


        private bool getInstallationGroupChilds2(DateTime dtNow,
                                                int ilevel,
                                                List<GroupType> groupTypes,
                                                int iMinutesOffsetOffSet,
                                                ref stZone zone,
                                                bool filterOnlyPermitRatesGroups)
        {
            bool bRes = true;
            try
            {

                decimal zoneid = zone.dID;

                var Groups = (from g in getGROUPs()
                              join gh in getGROUP_HIERARCHIEs() on g.GRP_ID equals gh.GRHI_GPR_ID_CHILD
                              where gh.GRHI_GPR_ID_PARENT == zoneid &&
                                  //gh.GRHI_INI_APPLY_DATE <= (DateTime)dtNow &&
                                    gh.GRHI_END_APPLY_DATE >= (DateTime)dtNow &&
                                    groupTypes.Contains((GroupType)g.GRP_TYPE)
                              orderby g.GRP_ID
                              select new { g, gh }).ToArray();

                if (Groups.Count() > 0)
                {
                    foreach (var group in Groups)
                    {
                        decimal dInsId= group.g.GRP_INS_ID;

                        stZone newZone = new stZone
                        {
                            level = ilevel,
                            dID = group.g.GRP_ID,
                            strDescription = group.g.GRP_DESCRIPTION,
                            dLiteralID = group.g.GRP_LIT_ID,
                            strColour = group.g.GRP_COLOUR,
                            strShowId = group.g.GRP_SHOW_ID,
                            subzones = new List<stZone>(),
                            GPSpolygons = new List<stGPSPolygon>(),
                            GroupType = (GroupType)group.g.GRP_TYPE,
                            Occupancy = (float)(100 - (group.g.GRP_FREE_SPACES_PERC ?? 0)),
                            ParkingType = group.g.GRP_OFFSTREET_TYPE ?? 0,
                            dtIniApply = group.gh.GRHI_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                            dtEndApply = group.gh.GRHI_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                            center = new stGPSPoint(),
                            allowByPassMap = (getTARIFFS_IN_GROUPS_GEOMETRIEs()
                                .Where(r => (r.TAGRGE_GRP_ID == group.g.GRP_ID) &&
                                   (r.TAGRGE_INI_APPLY_DATE < (DateTime)dtNow) &&
                                   (r.TAGRGE_END_APPLY_DATE >= (DateTime)dtNow)).Count() == 0),
                            permitMaxMonths = group.g.GRP_PERMIT_MAX_MONTHS,
                            dMessageLiteralID = group.g.GRP_MESSAGE_LIT_ID,
                            permitMaxBuyDay = group.g.GRP_PERMIT_MAX_CUR_MONTH_DAY,

                        };


                        int iNumPoligons = 0;
                        foreach (int iPolNumber in getGROUP_GEOMETRIEs()
                                                             .Where(r => r.GRGE_GRP_ID == group.g.GRP_ID &&
                                                                        //r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                                         r.GRGE_END_APPLY_DATE >= (DateTime)dtNow)
                                                             .GroupBy(r => r.GRGE_POL_NUMBER)
                                                             .OrderBy(r => r.Key)
                                                             .Select(r => r.Key))
                        {

                            stGPSPolygon oGPSPolygon = new stGPSPolygon();
                            oGPSPolygon.GPSpolygon = new List<stGPSPoint>();
                            oGPSPolygon.iPolNumber = iPolNumber;


                            foreach (GROUPS_GEOMETRY oGeometry in getGROUP_GEOMETRIEs()
                                    .Where(r => r.GRGE_GRP_ID == group.g.GRP_ID &&
                                                r.GRGE_INI_APPLY_DATE <= (DateTime)dtNow &&
                                                r.GRGE_END_APPLY_DATE >= (DateTime)dtNow &&
                                                r.GRGE_POL_NUMBER == iPolNumber)
                                    .OrderBy(r => r.GRGE_ORDER))
                            {
                                stGPSPoint gpsPoint = new stGPSPoint
                                {
                                    order = oGeometry.GRGE_ORDER,
                                    dLatitude = oGeometry.GRGE_LATITUDE,
                                    dLongitude = oGeometry.GRGE_LONGITUDE,
                                    dtIniApply = oGeometry.GRGE_INI_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet),
                                    dtEndApply = oGeometry.GRGE_END_APPLY_DATE.AddMinutes(-iMinutesOffsetOffSet)
                                };
                                ((List<stGPSPoint>)oGPSPolygon.GPSpolygon).Add(gpsPoint);

                            }

                            ((List<stGPSPolygon>)newZone.GPSpolygons).Add(oGPSPolygon);

                            if (iNumPoligons == 0)
                            {
                                PolygonCentroid(group.g, dtNow, (List<stGPSPoint>)(oGPSPolygon.GPSpolygon), ref newZone.center);
                            }

                            iNumPoligons++;
                        }


                      
                        getInstallationGroupChilds2(dtNow,
                                                    ilevel + 1,
                                                    groupTypes,
                                                    iMinutesOffsetOffSet,
                                                    ref newZone,
                                                    filterOnlyPermitRatesGroups);


                        bool bAddZone = true;

                        if ((filterOnlyPermitRatesGroups) && (newZone.subzones.Count() == 0))
                        {
                            IEnumerable<stTariff> zoneTariffs = getGroupTariffs(group.g.GRP_ID, null);
                            bAddZone = zoneTariffs.Where(r => r.tariffType == (int)TariffType.RegularTariff).Any();
                            if (!bAddZone)
                            {
                                var installation = (from i in getINSTALLATIONs()
                                                    where i.INS_ID == dInsId
                                                    select i).FirstOrDefault();

                                if (installation != null)
                                {
                                    if (installation.INS_MAP_SCREEN_TYPE == 1)
                                    {
                                        bAddZone = true;
                                    }
                                }

                            }
                        }


                        if (bAddZone)
                        {
                            ((List<stZone>)zone.subzones).Add(newZone);
                        }
                    }

                }
               

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getInstallationGroupChilds2: ", e);
                bRes = false;
            }

            return bRes;

        }


        private bool IsPointInsidePolygon(Point p,Point[] Polygon)
        {
            double dAngle = 0;

            try
            {

                for (int i = 0; i < Polygon.Length; i++)
                {
                    Vector v1 = new Vector(Polygon[i].X - p.X, Polygon[i].Y - p.Y);
                    Vector v2 = new Vector(Polygon[(i + 1) % Polygon.Length].X - p.X,
                                           Polygon[(i + 1) % Polygon.Length].Y - p.Y);

                    dAngle = dAngle + (Vector.AngleBetween(v1, v2) * Math.PI / 180);

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "IsPointInsidePolygon: ", e);
                dAngle = 0;
            }


            return (Math.Abs(dAngle) > Math.PI);

        }


        private void PolygonCentroid(GROUP oGrp, DateTime dtNow, List<stGPSPoint> Polygon, ref stGPSPoint ostCenter)
        {
          

            var oCenter = (from g in getGROUP_CENTERs()
                           where g.GRCE_GRP_ID == oGrp.GRP_ID &&
                                 g.GRCE_INI_APPLY_DATE <= dtNow &&
                                 g.GRCE_END_APPLY_DATE > dtNow
                           select g).FirstOrDefault();


            if (oCenter!=null)
            {
                ostCenter.dLatitude = oCenter.GRCE_LATITUDE;
                ostCenter.dLongitude = oCenter.GRCE_LONGITUDE;
            }
            else
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

                        integraMobileDBEntitiesDataContext dbContext = DataContextFactory.GetScopedDataContext<integraMobileDBEntitiesDataContext>();


                        var oCenter2 = (from g in dbContext.GROUP_CENTERs
                                        where g.GRCE_GRP_ID == oGrp.GRP_ID &&
                                              g.GRCE_INI_APPLY_DATE <= dtNow &&
                                              g.GRCE_END_APPLY_DATE > dtNow
                                        select g).FirstOrDefault();
                        if (oCenter2 != null)
                        {
                            ostCenter.dLatitude = oCenter2.GRCE_LATITUDE;
                            ostCenter.dLongitude = oCenter2.GRCE_LONGITUDE;
                        }
                        else
                        {

                            decimal totalLatitude = 0;
                            decimal totalLongitude = 0;
                            foreach (stGPSPoint oPoint in Polygon)
                            {

                                totalLatitude += oPoint.dLatitude;
                                totalLongitude += oPoint.dLongitude;
                            }

                            if (Polygon.Count > 0)
                            {
                                ostCenter.dLatitude = totalLatitude / Polygon.Count;
                                ostCenter.dLongitude = totalLongitude / Polygon.Count;


                                dbContext.GROUP_CENTERs.InsertOnSubmit(new GROUP_CENTER()
                                {
                                    GRCE_GRP_ID = oGrp.GRP_ID,
                                    GRCE_DESCRIPTION = oGrp.GRP_DESCRIPTION,
                                    GRCE_LATITUDE = ostCenter.dLatitude,
                                    GRCE_LONGITUDE = ostCenter.dLongitude,
                                    GRCE_END_APPLY_DATE = new DateTime(2100, 12, 31, 23, 59, 59),
                                    GRCE_INI_APPLY_DATE = dtNow.AddDays(-1),

                                });

                                // Submit the change to the database.
                                try
                                {
                                    SecureSubmitChanges(ref dbContext);

                                }
                                catch (Exception e)
                                {
                                    m_Log.LogMessage(LogLevels.logERROR, "PolygonCentroid: ", e);

                                }

                            }

                            transaction.Complete();
                        }


                    }

                }
                catch (Exception e)
                {
                    m_Log.LogMessage(LogLevels.logERROR, "PolygonCentroid: ", e);
                }
            }

            return ;
        }

        public bool getExternalProvider(string strName, ref EXTERNAL_PROVIDER oExternalProvider)
        {
            bool bRes = false;
            oExternalProvider = null;            

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


                    if (strName != null)
                    {
                        var oExternalProviders = (from r in dbContext.EXTERNAL_PROVIDERs
                                                  where r.EXP_NAME.ToUpper() == strName.ToUpper()
                                                  select r).ToArray();
                        if (oExternalProviders.Count() == 1)
                        {
                            oExternalProvider = oExternalProviders[0];
                            bRes = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getExternalProvider: ", e);
            }

            return bRes;
        }

        
        public bool getOffStreetConfiguration(decimal? dGroupId, decimal? dLatitude, decimal? dLongitude, ref GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetConfiguration, ref DateTime? dtgroupDateTime)
        {
            bool bRes = false;
            oOffstreetConfiguration = null;            

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

                    if (dGroupId != null)
                    {
                        oOffstreetConfiguration = GetGroupOffstreetWsConfiguration(dGroupId.Value, dbContext);
                    }
                    else if ((dLatitude != null) && (dLongitude != null))
                    {
                        GROUP oGroup = null;
                        var oGroups = (from r in dbContext.GROUPs
                                       where r.GRP_TYPE == (int) GroupType.OffStreet                                       
                                       orderby r.GRP_ID
                                       select r).ToArray();

                        foreach (GROUP oGrp in oGroups)
                        {
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oGrp.INSTALLATION.INS_TIMEZONE_ID);
                            DateTime dtServerTime = DateTime.Now;
                            DateTime dtLocalInstTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);

                            var PolygonNumbers = (from r in dbContext.GROUPS_GEOMETRies
                                                  where ((r.GRGE_GRP_ID == oGrp.GRP_ID) &&
                                                  (r.GRGE_INI_APPLY_DATE <= dtLocalInstTime) &&
                                                  (r.GRGE_INI_APPLY_DATE >= dtLocalInstTime))
                                                  group r by new
                                                  {
                                                      r.GRGE_POL_NUMBER
                                                  } into g
                                                  orderby g.Key.GRGE_POL_NUMBER
                                                  select new { iPolNumber = g.Key.GRGE_POL_NUMBER }).ToList();


                            Point p = new Point(Convert.ToDouble(dLongitude), Convert.ToDouble(dLatitude));

                            foreach (var oPolNumber in PolygonNumbers)
                            {


                                var Polygon = (from r in dbContext.GROUPS_GEOMETRies
                                               where ((r.GRGE_GRP_ID == oGrp.GRP_ID) &&
                                               (r.GRGE_INI_APPLY_DATE <= dtLocalInstTime) &&
                                               (r.GRGE_END_APPLY_DATE >= dtLocalInstTime) &&
                                               (r.GRGE_POL_NUMBER == oPolNumber.iPolNumber))
                                               orderby r.GRGE_ORDER
                                               select new Point(Convert.ToDouble(r.GRGE_LONGITUDE),
                                                                Convert.ToDouble(r.GRGE_LATITUDE))).ToArray();

                                if (Polygon.Count() > 0)
                                {
                                    if (IsPointInsidePolygon(p, Polygon))
                                    {
                                        oGroup = oGrp;
                                        break;
                                    }
                                }
                            }

                            if (oGroup != null)
                            {
                                break;
                            }
                        }


                        if (oGroup != null)
                        {
                            oOffstreetConfiguration = GetGroupOffstreetWsConfiguration(oGroup.GRP_ID, dbContext);
                        }

                    }

                    if (oOffstreetConfiguration != null) {
                        bRes = true;
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oOffstreetConfiguration.GROUP.INSTALLATION.INS_TIMEZONE_ID);
                        DateTime dtServerTime = DateTime.Now;
                        dtgroupDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getOffStreetConfiguration: ", e);
            }

            return bRes;
        }

        public bool getOffStreetConfigurationByExtOpsId(string sExtGroupId, ref GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetConfiguration, ref DateTime? dtgroupDateTime)
        {
            bool bRes = false;
            oOffstreetConfiguration = null;

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

                    var groups = (from r in dbContext.GROUPs
                                  where r.GRP_TYPE == (int)GroupType.OffStreet &&
                                        r.GRP_ID_FOR_EXT_OPS == sExtGroupId
                                  select r);
                    if (groups.Count() == 1)
                    {
                        oOffstreetConfiguration = GetGroupOffstreetWsConfiguration(groups.First().GRP_ID, dbContext);
                    }
                    
                    if (oOffstreetConfiguration != null)
                    {
                        bRes = true;
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oOffstreetConfiguration.GROUP.INSTALLATION.INS_TIMEZONE_ID);
                        DateTime dtServerTime = DateTime.Now;
                        dtgroupDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getOffStreetConfigurationByExtId: ", e);
            }

            return bRes;
        }
        public bool getOffStreetConfigurationByExtOpsId(string sExtGroupId, int iExtTerminalId, ref GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetConfiguration, ref DateTime? dtgroupDateTime)
        {
            bool bRes = false;
            oOffstreetConfiguration = null;

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

                    GROUP oGroup = null;
                    var groups = (from r in dbContext.GROUPs
                                  where r.GRP_TYPE == (int)GroupType.OffStreet &&
                                        r.GRP_ID_FOR_EXT_OPS.StartsWith(sExtGroupId + "*")
                                  select r);
                    foreach (GROUP oItem in groups)
                    {
                        var oIds = oItem.GRP_ID_FOR_EXT_OPS.Split('*');
                        if (oIds.Length == 2)
                        {
                            oIds = oIds[1].Trim().Split(',');
                            if (oIds.Contains(iExtTerminalId.ToString()))
                            {
                                oGroup = oItem;
                                break;
                            }
                        }
                    }

                    if (oGroup != null)
                    {
                        oOffstreetConfiguration = GetGroupOffstreetWsConfiguration(oGroup.GRP_ID, dbContext);
                    }

                    if (oOffstreetConfiguration != null)
                    {
                        bRes = true;
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oOffstreetConfiguration.GROUP.INSTALLATION.INS_TIMEZONE_ID);
                        DateTime dtServerTime = DateTime.Now;
                        dtgroupDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                    }

                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getOffStreetConfigurationByExtId: ", e);
            }

            return bRes;
        }

        public bool getGroupByExtOpsId(string sExtGroupId,
                                        ref GROUP oGroup,
                                        ref DateTime? dtgroupDateTime)
        {
            bool bRes = false;
            oGroup = null;
            dtgroupDateTime = null;

            try
            {

                var oGroups = (from r in getGROUPs()
                               where r.GRP_ID_FOR_EXT_OPS == sExtGroupId
                               select r).ToArray();
                if (oGroups.Count() == 1)
                {
                    oGroup = oGroups[0];
                    decimal dInsId = oGroup.GRP_INS_ID;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(getINSTALLATIONs().Where(r => r.INS_ID == dInsId).FirstOrDefault().INS_TIMEZONE_ID);
                    DateTime dtServerTime = DateTime.Now;
                    dtgroupDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);
                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getGroupByExtOpsId: ", e);
            }

            return bRes;
        }

        private GROUPS_OFFSTREET_WS_CONFIGURATION GetGroupOffstreetWsConfiguration(decimal dGroupId, integraMobileDBEntitiesDataContext dbContext)
        {
            GROUPS_OFFSTREET_WS_CONFIGURATION oGroupWsConfiguration = null;

            var oGroups = (from r in dbContext.GROUPs
                           where r.GRP_ID == dGroupId && r.GRP_TYPE == (int)GroupType.OffStreet
                           select r).ToArray();
            if (oGroups.Count() == 1)
            {
                var oGroup = oGroups[0];
                var oConfigurations = (from r in dbContext.GROUPS_OFFSTREET_WS_CONFIGURATIONs
                                       where r.GOWC_GRP_ID == oGroup.GRP_ID
                                       select r).ToArray();
                if (oConfigurations.Count() >= 1)
                {
                    oGroupWsConfiguration = oConfigurations[0];
                }
                else
                {
                    if (oGroup.GROUPS_HIERARCHies != null && oGroup.GROUPS_HIERARCHies.Count > 0 && oGroup.GROUPS_HIERARCHies[0].GRHI_GPR_ID_PARENT.HasValue)
                        oGroupWsConfiguration = GetGroupOffstreetWsConfiguration(oGroup.GROUPS_HIERARCHies[0].GRHI_GPR_ID_PARENT.Value, dbContext);
                }
            }

            return oGroupWsConfiguration;
        }

        public bool GetFinanDistOperator(decimal dFinanDistOperatorId, ref FINAN_DIST_OPERATOR oFinanDistOperator)
        {
            bool bRes = false;
            oFinanDistOperator = null;            

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


                    var oOperators = (from r in dbContext.FINAN_DIST_OPERATORs
                                            where r.FDO_ID == dFinanDistOperatorId
                                            select r).ToArray();
                    if (oOperators.Count() == 1)
                    {
                        oFinanDistOperator = oOperators[0];
                        bRes = true;

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetFinanDistOperator: ", e);
            }

            return bRes;

        }


        public bool getTicketTypePaymentInfo(decimal dInsId, DateTime dtInsDate, DateTime dtTicketDate, string strTicketTypeExtID, 
                                            out bool bIsPayable, out DateTime? dtMaxPayDate, out int iAmount, out string strDesc1, out string strDesc2)
        {

            bIsPayable = false;
            dtMaxPayDate = null;
            iAmount = 0;
            strDesc1 = "";
            strDesc2 = "";
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


                    var oTicketType = (from r in dbContext.TICKET_TYPEs
                                           where r.TITY_INS_ID == dInsId && r.TITY_EXT_ID== strTicketTypeExtID
                                           select r).FirstOrDefault();

                    if (oTicketType != null)
                    {
                        int iNumMinutes=Convert.ToInt32(Math.Truncate((dtInsDate-dtTicketDate).TotalMinutes));
                        strDesc1 = oTicketType.TITY_DESCRIPTION1;
                        strDesc2 = oTicketType.TITY_DESCRIPTION2;

                        var oFeatures = oTicketType.TICKET_TYPES_FEATUREs.Where(r => r.TITF_INIDATE <= dtTicketDate &&
                                                                                                     r.TITF_ENDDATE >= dtTicketDate)                                                                                                            
                                                                                         .OrderBy(r => r.TITF_INI_NUM_MINUTES).ToList();

                        foreach (TICKET_TYPES_FEATURE oFeature in oFeatures)
                        {
                            bIsPayable = false;

                            if (oFeature.TITF_PAYABLE != 0)
                            {
                                bIsPayable = true;
                                if (oFeature.TITF_INI_NUM_MINUTES <= iNumMinutes && oFeature.TITF_END_NUM_MINUTES > iNumMinutes)
                                {                                   
                                    iAmount = oFeature.TITF_AMOUNT ?? 0;
                                }

                                switch ((TicketTypeFeaturePeriodType)oFeature.TITF_PERIOD_TYPE)
                                {
                                    default:
                                    case TicketTypeFeaturePeriodType.NoRestriction:
                                        break;
                                    case TicketTypeFeaturePeriodType.StartAtTicketType:                                       
                                    case TicketTypeFeaturePeriodType.StartAt00NextDay:
                                        {
                                            dtMaxPayDate = dtTicketDate;

                                            if ((TicketTypeFeaturePeriodType)oFeature.TITF_PERIOD_TYPE==TicketTypeFeaturePeriodType.StartAt00NextDay)
                                            {
                                                dtMaxPayDate=dtTicketDate.Date.AddDays(1);
                                            }

                                            dtMaxPayDate=dtMaxPayDate.Value.AddMinutes(oFeature.TITF_END_NUM_MINUTES.Value);

                                        }

                                        break;
                                }
                            }
                        }
                        bRes = true;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getTicketTypePaymentInfo: ", e);
            }

            return bRes;                      

        }


        public bool ExistTicketPayment(decimal dInsId, string strTicketNumber)
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


                    var oTicketPayments = (from r in dbContext.TICKET_PAYMENTs
                                      where r.TIPA_INS_ID == dInsId && r.TIPA_TICKET_NUMBER == strTicketNumber
                                      select r).FirstOrDefault();

                    bRes = (oTicketPayments != null);
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetFinanDistOperator: ", e);
            }

            return bRes;

        }

        public List<decimal> GetChildInstallationsIds(decimal dInstallationId, DateTime? dtNow = null, bool bIncludeId = true)
        {
            List<decimal> oChildInstallations = new List<decimal>();
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

                    if (!dtNow.HasValue)
                        dtNow = getInstallationDateTime(dInstallationId);

                    if (dtNow != null)
                    {
                        var oInstallation = (from i in dbContext.INSTALLATIONs
                                             where i.INS_ID == dInstallationId
                                             select i).FirstOrDefault();
                        if (oInstallation != null)
                        {                            
                            if (bIncludeId)
                                oChildInstallations.Add(dInstallationId);

                            // Check if dInstallationId is a SuperInstallation and get child installations
                            if (oInstallation.SUPER_INSTALLATIONs1 != null && oInstallation.SUPER_INSTALLATIONs1.Any())
                            {
                                oChildInstallations.AddRange(oInstallation.SUPER_INSTALLATIONs1.Where(si => si.SINS_INI_APPLY_DATE <= (DateTime)dtNow && si.SINS_END_APPLY_DATE >= (DateTime)dtNow)
                                                                                               .Select(i => i.SINS_CHILD_INS_ID).ToList());
                            }
                        }
                    }

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetChildInstallationsIds: ", e);
            }

            return oChildInstallations;
        }

        public List<INSTALLATION> GetChildInstallations(decimal dInstallationId, DateTime? dtNow = null, bool bIncludeSuperInst = true)
        {
            List<INSTALLATION> oChildInstallations = new List<INSTALLATION>();
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

                    if (!dtNow.HasValue)
                        dtNow = getInstallationDateTime(dInstallationId);

                    if (dtNow != null)
                    {
                        var oInstallation = (from i in dbContext.INSTALLATIONs
                                             where i.INS_ID == dInstallationId
                                             select i).FirstOrDefault();
                        if (oInstallation != null)
                        {
                            if (bIncludeSuperInst)
                                oChildInstallations.Add(oInstallation);

                            // Check if dInstallationId is a SuperInstallation and get child installations
                            if (oInstallation.SUPER_INSTALLATIONs1 != null && oInstallation.SUPER_INSTALLATIONs1.Any())
                            {
                                oChildInstallations.AddRange(oInstallation.SUPER_INSTALLATIONs1.Where(si => si.SINS_INI_APPLY_DATE <= (DateTime)dtNow && si.SINS_END_APPLY_DATE >= (DateTime)dtNow)
                                                                                               .Select(i => i.INSTALLATION).ToList());
                            }
                        }
                    }

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetChildInstallations: ", e);
            }

            return oChildInstallations;
        }

        public decimal? GetSuperInstallationId(decimal dInstallationId, DateTime? dtNow = null)
        {
            decimal? dRet = null;
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

                    dRet = GetSuperInstallationId(dInstallationId, dtNow, dbContext);

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSuperInstallationId: ", e);
            }

            return dRet;
        }

        public decimal? GetSuperInstallationId(decimal dInstallationId, DateTime? dtNow, integraMobileDBEntitiesDataContext dbContext)
        {
            decimal? dRet = null;
            try
            {
                if (!dtNow.HasValue)
                    dtNow = getInstallationDateTime(dInstallationId);

                if (dtNow != null)
                {
                    var oSuperInstallation = (from si in dbContext.SUPER_INSTALLATIONs
                                                where si.SINS_CHILD_INS_ID == dInstallationId &&
                                                      si.SINS_INI_APPLY_DATE <= (DateTime)dtNow && si.SINS_END_APPLY_DATE >= (DateTime)dtNow
                                                select si.INSTALLATION1).FirstOrDefault();
                    if (oSuperInstallation != null)
                    {
                        dRet = oSuperInstallation.INS_ID;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSuperInstallationId: ", e);
            }

            return dRet;
        }

        public INSTALLATION GetSuperInstallation(decimal dInstallationId, DateTime? dtNow = null)
        {
            INSTALLATION ORet = null;
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

                    if (!dtNow.HasValue)
                        dtNow = getInstallationDateTime(dInstallationId);

                    if (dtNow != null)
                    {
                        var oSuperInstallation = (from si in dbContext.SUPER_INSTALLATIONs
                                                  where si.SINS_CHILD_INS_ID == dInstallationId &&
                                                        si.SINS_INI_APPLY_DATE <= (DateTime)dtNow && si.SINS_END_APPLY_DATE >= (DateTime)dtNow
                                                  select si.INSTALLATION1).FirstOrDefault();
                        if (oSuperInstallation != null)
                        {
                            ORet = oSuperInstallation;
                        }
                    }

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSuperInstallation: ", e);
            }

            return ORet;
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



        public decimal GetSourceApp(string strCode)
        {
            decimal dRes = 1;

            try
            {
                var oSourceApps = (from r in getSOURCE_APPs()
                                   where r.SOAPP_CODE==strCode
                                   select r).ToArray();
                if (oSourceApps.Count() == 1)
                {
                    dRes = oSourceApps.FirstOrDefault().SOAPP_ID;
                }
                else
                {
                    dRes = GetDefaultSourceApp();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSourceApp: ", e);
            }

            return dRes;
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

        public string GetSourceAppDescription(decimal dSourceApp)
        {
            string strRes = "";

            try
            {
                var oSourceApps = (from r in getSOURCE_APPs()
                                   where r.SOAPP_ID == dSourceApp
                                   select r).ToArray();
                if (oSourceApps.Count() == 1)
                {
                    strRes = oSourceApps.FirstOrDefault().SOAPP_DESCRIPTION;
                }
                else
                {
                    strRes = (from r in getSOURCE_APPs()
                              where r.SOAPP_DEFAULT == 1
                              select r).FirstOrDefault().SOAPP_DESCRIPTION;
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetSourceAppDescription: ", e);
            }

            return strRes;
        }

        public bool GetStreetSectionsUpdateInstallations(out List<INSTALLATION> oInstallations)
        {
            bool bRes = false;
            oInstallations = new List<INSTALLATION>();

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

                    oInstallations = (from r in dbContext.INSTALLATIONs
                                      where r.INS_ENABLED == 1 && r.INS_STREET_SECTION_UPDATE_WS_SIGNATURE_TYPE != (int)StreetSectionsUpdateSignatureType.no_call
                                      select r).ToList();

                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "getGroupByExtOpsId: ", e);
            }

            return bRes;
        }

        public bool GetInstallationStreets(decimal dInstallationID, out List<STREET> oStreets)
        {
            bool bRes = false;
            oStreets = null;

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

                    var oInstallation = (from r in dbContext.INSTALLATIONs
                                         where r.INS_ID == dInstallationID
                                         select r).FirstOrDefault();


                    if (oInstallation != null)
                    {
                        oStreets = oInstallation.STREETs.OrderBy(r => r.STR_ID_EXT).ToList();                        
                    }

                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetInstallationStreets: ", e);
            }

            return bRes;
        }

        public bool UpdateStreets(decimal dInstallationID, 
                                  ref List<StreetData> oInsertStreetsData,
                                  ref List<StreetData> oUpdateStreetsData,
                                  ref List<StreetData> oDeleteStreetsData)
        {
            bool bRes = false;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                             new TransactionOptions()
                                                                             {
                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                 //Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                    var oInstallation = (from r in dbContext.INSTALLATIONs
                                         where r.INS_ID == dInstallationID
                                         select r).FirstOrDefault();


                    if (oInstallation != null)
                    {
                        STREET oLastStreet = dbContext.STREETs.OrderByDescending(r => r.STR_ID).FirstOrDefault();

                        decimal dNextStreetId = 0;

                        if (oLastStreet != null)
                        {
                            dNextStreetId = oLastStreet.STR_ID;
                        }

                        foreach (StreetData oData in oInsertStreetsData)
                        {
                            dNextStreetId++;

                            STREET oStreet = new STREET()
                            {
                                STR_ID = dNextStreetId,
                                STR_ID_EXT = oData.Id,                                
                                STR_DELETED = (oData.Deleted ? 1 : 0),
                                STR_DESCRIPTION = oData.Description,
                                STR_INS_ID = dInstallationID
                            };                            

                            dbContext.STREETs.InsertOnSubmit(oStreet);

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("UpdateStreets -> Insert Street {0}:{1}:{2}",
                                            oInstallation.INS_DESCRIPTION, oData.Id, oData.Description));


                        }


                        foreach (StreetData oData in oUpdateStreetsData)
                        {


                            STREET oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.Id).First();

                            oStreet.STR_ID_EXT = oData.Id;                            
                            oStreet.STR_DELETED = (oData.Deleted ? 1 : 0);
                            oStreet.STR_DESCRIPTION = oData.Description;                            
                            oStreet.STR_INS_ID = dInstallationID;

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("UpdateStreets -> Update Street {0}:{1}:{2}",
                                                                        oInstallation.INS_DESCRIPTION, oData.Id, oData.Description));


                        }


                        foreach (StreetData oData in oDeleteStreetsData)
                        {
                            STREET oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.Id).First();
                            oStreet.STR_DELETED = 1;

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("UpdateStreets -> Delete Street {0}:{1}:{2}",
                                        oInstallation.INS_DESCRIPTION, oData.Id, oData.Description));

                        }

                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "UpdateStreets: ", e);
                            bRes = false;
                        }

                    }


                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "UpdateStreets: ", e);
            }

            return bRes;


        }

        //public bool GetInstallationsStreetSections(decimal dInstallationID, out List<StreetSectionData> oStreetSectionsData, out Dictionary<int, GridElement> oGrid)
        //{
        //    bool bRes = false;
        //    oStreetSectionsData = new List<StreetSectionData>();
        //    oGrid = new Dictionary<int, GridElement>();
        //
        //    try
        //    {
        //        using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
        //                                                                     new TransactionOptions()
        //                                                                     {
        //                                                                         IsolationLevel = IsolationLevel.ReadUncommitted,
        //                                                                         Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
        //                                                                     }))
        //        {
        //            integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
        //
        //            var oInstallation = (from r in dbContext.INSTALLATIONs
        //                                 where r.INS_ID == dInstallationID
        //                                 select r).FirstOrDefault();
        //
        //
        //            if (oInstallation != null)
        //            {
        //
        //                foreach (STREET_SECTIONS_GRID oGridElement in oInstallation.STREET_SECTIONS_GRIDs.OrderBy(r => r.STRSEG_X).OrderBy(r => r.STRSEG_Y))
        //                {
        //                    GridElement oDataGridElement = new GridElement()
        //                    {
        //                        id = Convert.ToInt32(oGridElement.STRSEG_ID),
        //                        description = oGridElement.STRSEG_DESCRIPTION,
        //                        x = oGridElement.STRSEG_X,
        //                        y = oGridElement.STRSEG_Y,
        //                        maxX = oGridElement.STRSEG_MAX_X,
        //                        maxY = oGridElement.STRSEG_MAX_Y,
        //                        Polygon = new List<MapPoint>(),
        //                        LstStreetSections = new List<StreetSectionData>(),
        //                        ReferenceCount = 0,
        //                    };
        //
        //                    foreach (STREET_SECTIONS_GRID_GEOMETRY oGeometry in oGridElement.STREET_SECTIONS_GRID_GEOMETRies.OrderBy(r => r.STRSEGG_ORDER).ToList())
        //                    {
        //                        oDataGridElement.Polygon.Add(new MapPoint() { x = oGeometry.STRSEGG_LONGITUDE, y = oGeometry.STRSEGG_LATITUDE });
        //                    }
        //
        //                    oGrid[oDataGridElement.id] = oDataGridElement;
        //
        //                }
        //
        //
        //                foreach (STREET_SECTION oStreetSection in oInstallation.STREET_SECTIONs.OrderBy(r => r.STRSE_ID_EXT))
        //                {
        //                    StreetSectionData oData = new StreetSectionData()
        //                    {
        //                        Id = oStreetSection.STRSE_ID_EXT,                                
        //                        Description = oStreetSection.STRSE_DESCRIPTION,
        //                        Deleted = (oStreetSection.STRSE_DELETED == 1),
        //                        Zone = oStreetSection.GROUP.GRP_PERMITS_EXT_ID,
        //                        Street = oStreetSection.STREET.STR_ID_EXT,
        //                        StreetFrom = oStreetSection.STREET1?.STR_ID_EXT,
        //                        StreetTo = oStreetSection.STREET2?.STR_ID_EXT,
        //                        GeometryCoordinates = new List<MapPoint>(),
        //                        oGridElements = new Dictionary<int, GridElement>(),
        //                        Tariffs = new List<string>()
        //                    };
        //
        //
        //                    foreach (TARIFF_IN_STREETS_SECTIONS_COMPILED oTariff in oStreetSection.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.OrderBy(r => r.TARSTRSEC_TAR_ID))
        //                    {
        //                        oData.Tariffs.Add(Convert.ToInt32(oTariff.TARSTRSEC_TAR_ID));
        //                    }
        //
        //
        //                    foreach (STREET_SECTIONS_GEOMETRY oGeometry in oStreetSection.STREET_SECTIONS_GEOMETRies.OrderBy(r => r.STRSEGE_ORDER).ToList())
        //                    {
        //                        oData.Geometry.Add(new MapPoint() { x = oGeometry.STRSEGE_LONGITUDE, y = oGeometry.STRSEGE_LATITUDE });
        //                    }
        //
        //
        //                    foreach (STREET_SECTIONS_STREET_SECTIONS_GRID oGridAssignatton in oStreetSection.STREET_SECTIONS_STREET_SECTIONS_GRIDs.OrderBy(r => r.STRSESSG_STRSEG_ID).ToList())
        //                    {
        //                        oData.oGridElements[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)] = oGrid[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)];
        //                        oGrid[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)].LstStreetSections.Add(oData);
        //                        oGrid[Convert.ToInt32(oGridAssignatton.STRSESSG_STRSEG_ID)].ReferenceCount++;
        //                    }
        //
        //                    oStreetSectionsData.Add(oData);
        //
        //                }
        //
        //
        //            }
        //
        //            bRes = true;
        //        }
        //
        //    }
        //    catch (Exception e)
        //    {
        //        m_Log.LogMessage(LogLevels.logERROR, "GetInstallationsStreetSections: ", e);
        //    }
        //
        //    return bRes;
        //
        //
        //}        



        public bool RecreateStreetSectionsGrid(decimal dInstallationID, ref Dictionary<int, GridElement> oGrid)
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
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                    dbContext.STREET_SECTIONS_GRID_GEOMETRies.DeleteAllOnSubmit(dbContext.STREET_SECTIONS_GRID_GEOMETRies.Where(r => r.STREET_SECTIONS_GRID.INSTALLATION.INS_ID == dInstallationID).AsEnumerable());
                    dbContext.STREET_SECTIONS_STREET_SECTIONS_GRIDs.DeleteAllOnSubmit(dbContext.STREET_SECTIONS_STREET_SECTIONS_GRIDs.Where(r => r.STREET_SECTIONS_GRID.INSTALLATION.INS_ID == dInstallationID).AsEnumerable());
                    dbContext.STREET_SECTIONS_GRIDs.DeleteAllOnSubmit(dbContext.STREET_SECTIONS_GRIDs.Where(r => r.INSTALLATION.INS_ID == dInstallationID).AsEnumerable());

                    var oInstallation = (from r in dbContext.INSTALLATIONs
                                         where r.INS_ID == dInstallationID
                                         select r).FirstOrDefault();


                    if (oInstallation != null)
                    {

                        foreach (KeyValuePair<int, GridElement> entry in oGrid.OrderBy(r => r.Key))
                        {

                            STREET_SECTIONS_GRID oSectionGrid = new STREET_SECTIONS_GRID()
                            {
                                STRSEG_ID = entry.Value.id,
                                STRSEG_DESCRIPTION = entry.Value.description,
                                STRSEG_X = entry.Value.x,
                                STRSEG_Y = entry.Value.y,
                                STRSEG_MAX_X = entry.Value.maxX,
                                STRSEG_MAX_Y = entry.Value.maxY,
                                STRSEG_INS_ID = dInstallationID,
                            };

                            int i = 1;
                            foreach (MapPoint oPoint in entry.Value.Polygon)
                            {

                                oSectionGrid.STREET_SECTIONS_GRID_GEOMETRies.Add(new STREET_SECTIONS_GRID_GEOMETRY()
                                {
                                    STRSEGG_LATITUDE = Convert.ToDecimal(oPoint.y),
                                    STRSEGG_LONGITUDE = Convert.ToDecimal(oPoint.x),
                                    STRSEGG_ORDER = i++,
                                    STRSEGG_INI_APPLY_DATE = DateTime.UtcNow.AddDays(-1),
                                    STRSEGG_END_APPLY_DATE = DateTime.UtcNow.AddYears(50),

                                });
                            }

                            dbContext.STREET_SECTIONS_GRIDs.InsertOnSubmit(oSectionGrid);

                        }

                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "RecreateStreetSectionsGrid: ", e);
                            bRes = false;
                        }

                    }


                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RecreateStreetSectionsGrid: ", e);
            }

            return bRes;


        }


        public bool UpdateStreetSections(decimal dInstallationID, bool bGridRecreated,
                                               ref List<StreetSectionData> oInsertStreetSectionsData,
                                               ref List<StreetSectionData> oUpdateStreetSectionsData,
                                               ref List<StreetSectionData> oDeleteStreetSectionsData)
        {
            bool bRes = false;

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                             new TransactionOptions()
                                                                             {
                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                 //Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                             }))
                {
                    integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();


                    var oInstallation = (from r in dbContext.INSTALLATIONs
                                         where r.INS_ID == dInstallationID
                                         select r).FirstOrDefault();


                    if (oInstallation != null)
                    {
                        STREET oUnknownStreet = oInstallation.STREETs.Where(r => r.STR_DESCRIPTION == "UNKNOWN").FirstOrDefault();

                        STREET_SECTION oLastStreetSection = dbContext.STREET_SECTIONs.OrderByDescending(r => r.STRSE_ID).FirstOrDefault();

                        decimal dNextStreetSectionId = 0;

                        if (oLastStreetSection != null)
                        {
                            dNextStreetSectionId = oLastStreetSection.STRSE_ID;
                        }


                        foreach (StreetSectionData oData in oInsertStreetSectionsData)
                        {
                            dNextStreetSectionId++;

                            STREET_SECTION oStreetSection = new STREET_SECTION()
                            {
                                STRSE_ID = dNextStreetSectionId,
                                STRSE_ID_EXT = oData.Id,
                                STRSE_DELETED = oData.Deleted ? 1 : 0,
                                STRSE_DESCRIPTION = oData.Description,
                                STRSE_GRP_ID = oInstallation.GROUPs.Where(g => g.GRP_PERMITS_EXT_ID == oData.Zone).First().GRP_ID,
                                STRSE_INS_ID = dInstallationID,
                                STRSE_FROM = oData.StreetNumberFrom,
                                STRSE_TO = oData.StreetNumberTo,
                                STRSE_SIDE = (oData.Side ?? 0),
                                STRSE_COLOUR = oData.Colour
                            };


                            STREET oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.Street).FirstOrDefault();

                            if (oStreet != null)
                            {
                                oStreetSection.STREET = oStreet;
                            }
                            else
                            {
                                oStreetSection.STREET = oUnknownStreet;
                            }

                            oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.StreetFrom).FirstOrDefault();

                            if (oStreet != null)
                            {
                                oStreetSection.STREET1 = oStreet;
                            }
                            //else
                            //{
                            //    oStreetSection.STREET1 = oUnknownStreet;
                            //}

                            oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.StreetTo).FirstOrDefault();

                            if (oStreet != null)
                            {
                                oStreetSection.STREET2 = oStreet;
                            }
                            //else
                            //{
                            //    oStreetSection.STREET2 = oUnknownStreet;
                            //}


                            int i = 1;


                            foreach (MapPoint oPoint in oData.GeometryCoordinates)
                            {

                                oStreetSection.STREET_SECTIONS_GEOMETRies.Add(new STREET_SECTIONS_GEOMETRY()
                                {
                                    STRSEGE_LATITUDE = Convert.ToDecimal(oPoint.y),
                                    STRSEGE_LONGITUDE = Convert.ToDecimal(oPoint.x),
                                    STRSEGE_ORDER = i++,
                                    STRSEGE_INI_APPLY_DATE = DateTime.UtcNow.AddDays(-1),
                                    STRSEGE_END_APPLY_DATE = DateTime.UtcNow.AddYears(50),
                                    STRSEGE_POL_NUMBER = 1,
                                });

                            }
                            if (oData.oGridElements != null)
                            {

                                foreach (KeyValuePair<int, GridElement> entry in oData.oGridElements.OrderBy(r => r.Key))
                                {
                                    oStreetSection.STREET_SECTIONS_STREET_SECTIONS_GRIDs.Add(new STREET_SECTIONS_STREET_SECTIONS_GRID()
                                    {
                                        STRSESSG_STRSEG_ID = entry.Value.id,
                                    });

                                }
                            }

                            if (oData.Tariffs != null)
                            {
                                foreach (string sTariffExtId in oData.Tariffs)
                                {
                                    var oTariff = oInstallation.TARIFFs.Where(t => t.TAR_PERMITS_EXT_ID == sTariffExtId).FirstOrDefault();
                                    if (oTariff != null)
                                    {
                                        oStreetSection.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.Add(new TARIFF_IN_STREETS_SECTIONS_COMPILED()
                                        {
                                            TARSTRSEC_TAR_ID = oTariff.TAR_ID
                                        });
                                    }

                                }
                            }
                            else
                            {
                                oStreetSection.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.AddRange(oInstallation.TARIFFs.Select(tariff => new TARIFF_IN_STREETS_SECTIONS_COMPILED()
                                {
                                    TARSTRSEC_TAR_ID = tariff.TAR_ID
                                }));
                            }

                            dbContext.STREET_SECTIONs.InsertOnSubmit(oStreetSection);

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("UpdateStreetSections -> Insert Street Section {0}:{1}:{2}",
                                            oInstallation.INS_DESCRIPTION, oData.Id, oData.Description));


                        }


                        foreach (StreetSectionData oData in oUpdateStreetSectionsData)
                        {


                            STREET_SECTION oStreetSection = oInstallation.STREET_SECTIONs.Where(r => r.STRSE_ID_EXT == oData.Id).First();

                            oStreetSection.STRSE_ID_EXT = oData.Id;                            
                            oStreetSection.STRSE_DELETED = (oData.Deleted ? 1 : 0);
                            oStreetSection.STRSE_DESCRIPTION = oData.Description;
                            oStreetSection.STRSE_GRP_ID = oInstallation.GROUPs.Where(g => g.GRP_PERMITS_EXT_ID == oData.Zone).First().GRP_ID;
                            oStreetSection.STRSE_INS_ID = dInstallationID;
                            oStreetSection.STRSE_FROM = oData.StreetNumberFrom;
                            oStreetSection.STRSE_TO = oData.StreetNumberTo;
                            oStreetSection.STRSE_SIDE = (oData.Side ?? 0);
                            oStreetSection.STRSE_COLOUR = oData.Colour;

                            STREET oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.Street).FirstOrDefault();

                            if (oStreet != null)
                            {
                                oStreetSection.STREET = oStreet;
                            }
                            else
                            {
                                oStreetSection.STREET = oUnknownStreet;
                            }

                            oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.StreetFrom).FirstOrDefault();

                            if (oStreet != null)
                            {
                                oStreetSection.STREET1 = oStreet;
                            }
                            else
                            {
                                oStreetSection.STREET1 = null; // oUnknownStreet;
                            }

                            oStreet = oInstallation.STREETs.Where(r => r.STR_ID_EXT == oData.StreetTo).FirstOrDefault();

                            if (oStreet != null)
                            {
                                oStreetSection.STREET2 = oStreet;
                            }
                            else
                            {
                                oStreetSection.STREET2 = null; // oUnknownStreet;
                            }



                            dbContext.STREET_SECTIONS_GEOMETRies.DeleteAllOnSubmit(dbContext.STREET_SECTIONS_GEOMETRies.Where(r => r.STRSEGE_STRSE_ID == oStreetSection.STRSE_ID).AsEnumerable());

                            int i = 1;

                            foreach (MapPoint oPoint in oData.GeometryCoordinates)
                            {

                                oStreetSection.STREET_SECTIONS_GEOMETRies.Add(new STREET_SECTIONS_GEOMETRY()
                                {
                                    STRSEGE_LATITUDE = Convert.ToDecimal(oPoint.y),
                                    STRSEGE_LONGITUDE = Convert.ToDecimal(oPoint.x),
                                    STRSEGE_ORDER = i++,
                                    STRSEGE_INI_APPLY_DATE = DateTime.UtcNow.AddDays(-1),
                                    STRSEGE_END_APPLY_DATE = DateTime.UtcNow.AddYears(50),
                                    STRSEGE_POL_NUMBER = 1,
                                });

                            }


                            dbContext.STREET_SECTIONS_STREET_SECTIONS_GRIDs.DeleteAllOnSubmit(dbContext.STREET_SECTIONS_STREET_SECTIONS_GRIDs.Where(r => r.STRSESSG_STRSE_ID == oStreetSection.STRSE_ID).AsEnumerable());

                            if (oData.oGridElements != null)
                            {
                                foreach (KeyValuePair<int, GridElement> entry in oData.oGridElements.OrderBy(r => r.Key))
                                {
                                    oStreetSection.STREET_SECTIONS_STREET_SECTIONS_GRIDs.Add(new STREET_SECTIONS_STREET_SECTIONS_GRID()
                                    {
                                        STRSESSG_STRSEG_ID = entry.Value.id,
                                    });

                                }
                            }

                            dbContext.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.DeleteAllOnSubmit(dbContext.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.Where(r => r.TARSTRSEC_STRSE_ID == oStreetSection.STRSE_ID).AsEnumerable());

                            if (oData.Tariffs != null)
                            {
                                foreach (string sTariffExtId in oData.Tariffs)
                                {
                                    var oTariff = oInstallation.TARIFFs.Where(t => t.TAR_PERMITS_EXT_ID == sTariffExtId).FirstOrDefault();
                                    if (oTariff != null)
                                    {
                                        oStreetSection.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.Add(new TARIFF_IN_STREETS_SECTIONS_COMPILED()
                                        {
                                            TARSTRSEC_TAR_ID = oTariff.TAR_ID
                                        });
                                    }
                                }
                            }
                            else
                            {
                                oStreetSection.TARIFF_IN_STREETS_SECTIONS_COMPILEDs.AddRange(oInstallation.TARIFFs.Select(tariff => new TARIFF_IN_STREETS_SECTIONS_COMPILED()
                                {
                                    TARSTRSEC_TAR_ID = tariff.TAR_ID
                                }));
                            }

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("UpdateStreetSections -> Update Street Section {0}:{1}:{2}",
                                                                        oInstallation.INS_DESCRIPTION, oData.Id, oData.Description));


                        }


                        foreach (StreetSectionData oData in oDeleteStreetSectionsData)
                        {
                            STREET_SECTION oStreetSection = oInstallation.STREET_SECTIONs.Where(r => r.STRSE_ID_EXT == oData.Id).First();
                            oStreetSection.STRSE_DELETED = 1;

                            m_Log.LogMessage(LogLevels.logINFO, string.Format("UpdateStreetSections -> Delete Street Section {0}:{1}:{2}",
                                        oInstallation.INS_DESCRIPTION, oData.Id, oData.Description));

                        }

                        try
                        {
                            SecureSubmitChanges(ref dbContext);
                            transaction.Complete();
                        }
                        catch (Exception e)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "RecreateStreetSectionsGrid: ", e);
                            bRes = false;
                        }

                    }


                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RecreateStreetSectionsGrid: ", e);
            }

            return bRes;
        }


        public bool GetPackageFileData(decimal dInstallationID, out Dictionary<decimal, STREET> oStreets, out List<STREET_SECTION> oStreetSections,
                                       out List<STREET_SECTIONS_GRID> oGrid, out int iPackageNextVersion)

        {
            bool bRes = false;
            oStreets = new Dictionary<decimal, STREET>();
            oStreetSections = new List<STREET_SECTION>();
            oGrid = new List<STREET_SECTIONS_GRID>();
            iPackageNextVersion = 1;
            Dictionary<string, STREET> oStreetsNames = new Dictionary<string, STREET>();

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

                    var oInstallation = (from r in dbContext.INSTALLATIONs
                                         where r.INS_ID == dInstallationID
                                         select r).FirstOrDefault();


                    if (oInstallation != null)
                    {
                        foreach (STREET_SECTION oStreetSection in oInstallation.STREET_SECTIONs.Where(r => r.STRSE_DELETED == 0))
                        {
                            if ((!oStreets.ContainsKey(oStreetSection.STRSE_STR_ID)) &&
                                (!oStreetsNames.ContainsKey(oStreetSection.STREET.STR_DESCRIPTION)))
                            {
                                oStreets[oStreetSection.STRSE_STR_ID] = oStreetSection.STREET;
                                oStreetsNames[oStreetSection.STREET.STR_DESCRIPTION] = oStreetSection.STREET;
                            }
                        }

                        oStreetSections = oInstallation.STREET_SECTIONs.Where(r => r.STRSE_DELETED == 0).ToList();
                        oGrid = oInstallation.STREET_SECTIONS_GRIDs.ToList();
                        STREET_SECTIONS_PACKAGE_VERSION oVersion = oInstallation.STREET_SECTIONS_PACKAGE_VERSIONs
                                                                .OrderByDescending(r => r.STSEPV_ID).FirstOrDefault();

                        if (oVersion != null)
                        {
                            iPackageNextVersion = Convert.ToInt32(oVersion.STSEPV_ID) + 1;
                        }
                    }



                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RecreateStreetSectionsGrid: ", e);
            }
            finally
            {
                oStreetsNames = null;
            }

            return bRes;


        }


        public bool GetStreetSectionsExternalIds(decimal dInstallationID, out string[] oStreetSections)
        {
            bool bRes = false;
            oStreetSections = null;

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

                    var oInstallation = (from r in dbContext.INSTALLATIONs
                                         where r.INS_ID == dInstallationID
                                         select r).FirstOrDefault();


                    if (oInstallation != null)
                    {

                        oStreetSections = oInstallation.STREET_SECTIONs.Where(r => r.STRSE_DELETED == 0).Select(r => r.STRSE_ID_EXT).ToArray();
                    }



                    bRes = true;
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "RecreateStreetSectionsGrid: ", e);
            }

            return bRes;


        }

        public bool GetStreetSection(decimal dStreetSectionId, out STREET_SECTION oStreetSection)
        {
            bool bRes = false;
            oStreetSection = null;

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

                    oStreetSection = (from r in dbContext.STREET_SECTIONs
                                         where r.STRSE_ID == dStreetSectionId
                                         select r).FirstOrDefault();

                    bRes = (oStreetSection != null);
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetStreetSection: ", e);
            }

            return bRes;
        }

        private string GetLiteral(decimal literalId, string langCulture)
        {
            string sRes = "";

            try
            {
                var oLiterals = (from r in getLITERALs()
                                    where r.LIT_ID == literalId
                                    select r);

                if (oLiterals.Count() > 0)
                {
                    var oLiteral = oLiterals.First();
                    var oLiteralsLang = getLITERAL_LANGUAGEs().Where(l => l.LANGUAGE.LAN_CULTURE == langCulture && l.LITL_LIT_ID == literalId);
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
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLiteral: ", e);
                sRes = "";
            }

            return sRes;

        }

        private bool GetLanguage(decimal dLanId, out LANGUAGE oLanguage)
        {
            bool bRes = false;
            oLanguage = null;

            try
            {
                oLanguage = (from r in getLANGUAGEs()
                                where r.LAN_ID == dLanId
                                select r)
                            .FirstOrDefault();
                bRes = (oLanguage != null);
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetLanguage: ", e);
                bRes = false;
            }

            return bRes;
        }

        private stTariff AddTariffsCustomMessage(stTariff newTariff, TARIFFS_CUSTOM_MESSAGE tariffsCustomMessages, decimal? lang, decimal tariffId)
        {
            if (tariffsCustomMessages!=null && 
                tariffsCustomMessages.TARCME_ENABLED == 1 &&
                tariffId.Equals(tariffsCustomMessages.TARCME_TAR_ID) && 
                !tariffsCustomMessages.TARCME_DATE_INI.HasValue && 
                !tariffsCustomMessages.TARCME_DATE_END.HasValue)
            {
                LANGUAGE oLanguage;
                GetLanguage(lang.Value, out oLanguage);
                if (tariffsCustomMessages.TARCME_UNDER_WHEEL_LIT_ID.HasValue)
                {
                    newTariff.tarSerLitUnderWheel = GetLiteral(tariffsCustomMessages.TARCME_UNDER_WHEEL_LIT_ID.Value, oLanguage.LAN_CULTURE);
                }
                if (tariffsCustomMessages.TARCME_BUTTON_STOP_LIT_ID.HasValue)
                {
                    newTariff.tarSerLitButtonStop = GetLiteral(tariffsCustomMessages.TARCME_BUTTON_STOP_LIT_ID.Value, oLanguage.LAN_CULTURE);
                }
                if (tariffsCustomMessages.TARCME_END_PARKING_LIT_ID.HasValue)
                {
                    newTariff.tarSerLitEndParking = GetLiteral(tariffsCustomMessages.TARCME_END_PARKING_LIT_ID.Value, oLanguage.LAN_CULTURE);
                }
                if (tariffsCustomMessages.TARCME_BUTTON_END_PARKING_LIT_ID.HasValue)
                {
                    newTariff.tarSerLitButtonEndParking = GetLiteral(tariffsCustomMessages.TARCME_BUTTON_END_PARKING_LIT_ID.Value, oLanguage.LAN_CULTURE);
                }
            }
            return newTariff;
        }
    }
}
