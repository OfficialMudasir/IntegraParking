using VehicleType.WS.Infrastructure.Logging.Tools;
using VehicleType.WS.Domain.Abstract;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VehicleType.WS.Domain.Concrete
{
    public class SQLStatelessBaseRepository : IStatelessBaseRepository
    {
        #region Protected Attrib
        
        //Log4net Wrapper class
        protected CLogWrapper m_Log = null;

        protected IStatelessSession m_statelessSession = null; 

        #endregion

        #region Constructor

        public SQLStatelessBaseRepository(Type parentType, NHSessionManager.ConnectionConfiguration oConnectionConfig, bool bOpenSession)
        {
            if (m_Log == null)
            {
                m_Log = new CLogWrapper(parentType);
            }

            if (bOpenSession)
            {
                m_statelessSession = NHSessionManager.SessionFactory(oConnectionConfig).OpenStatelessSession();
                //m_Log.LogMessage(LogLevels.logDEBUG, "SQLStatelessBaseRepository",
                //    string.Format("Open stateless session '{0}'", SerializerHelper.GetObjectUniqueID(m_statelessSession)));
            }
        } 
        #endregion

        #region Destructor

        ~SQLStatelessBaseRepository()
        {
            Dispose();
        }
 
        #endregion

        #region Protected Methods

        protected ITransaction BeginTransaction(IStatelessSession statelessSession)
        {
            return statelessSession.BeginTransaction();
        }

        protected void CommitTransaction(ITransaction oTransaction)
        {
            if (oTransaction == null)
            {
                return;
            }

            oTransaction.Commit();
            oTransaction.Dispose();
        }
        protected void RollbackTransaction(ITransaction oTransaction)
        {
            if (oTransaction == null)
            {
                return;
            }

            oTransaction.Rollback();
            oTransaction.Dispose();
        }
        protected void FinishTransaction(bool bCommit, ITransaction oTransaction)
        {
            if (oTransaction == null)
            {
                return;
            }

            if (bCommit)
            {
                oTransaction.Commit();
            }
            else
            {
                oTransaction.Rollback();
            }

            oTransaction.Dispose();
        } 
        #endregion

        #region Public Methods

        public IVehicleTypeTransaction CreateTransaction()
        {
            IVehicleTypeTransaction oiParkTicketTransaction = m_statelessSession == null
                ? new VehicleTypeTransaction(true)
                : new VehicleTypeTransaction(m_statelessSession);

            return oiParkTicketTransaction;
        }

        public Type GetEntityType(string sEntityName, string sNamespace = "VehicleType.WS.Domain.Entities")
        {
            Type oRet = null;

            try
            {
                oRet = Type.GetType(string.Format("{0}.{1}", sNamespace, sEntityName));
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetEntityType: ", ex);
            }

            return oRet;
        }

        public IQueryable GetQuery(Type typeEntity, IVehicleTypeTransaction trans = null, int? iTimeoutSeconds = null)
        {
            IQueryable res = null;

            OracleConnectionHelper oConn = new OracleConnectionHelper(m_statelessSession, trans);

            try
            {
                oConn.BeginConnection();
                //if (!bInSession) oSession = this.GetSession();
                //if (!bInTransaction) oTransaction = this.BeginTransaction(oSession);

                //MethodInfo queryMethod = session.GetType().GetMethod("Query").MakeGenericMethod(typeEntity);
                MethodInfo queryMethod = typeof(LinqExtensionMethods).GetMethod("Query", new Type[] { typeof(IStatelessSession) });

                if (queryMethod != null)
                {
                    queryMethod = queryMethod.MakeGenericMethod(typeEntity);
                }

                res = (IQueryable)queryMethod.Invoke(null, new object[] { oConn.StatelessSession });

                if (iTimeoutSeconds.HasValue)
                {
                    MethodInfo timeoutMethod = typeof(LinqExtensionMethods).GetMethod("Timeout"); //, new Type[] { typeof(IQueryable), typeof(int) });
                    
                    if (timeoutMethod != null)
                    {
                        timeoutMethod = timeoutMethod.MakeGenericMethod(typeEntity);
                    }

                    timeoutMethod.Invoke(null, new object[] { res, iTimeoutSeconds.Value });
                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetQuery: ", e);
            }
            finally
            {
                oConn.EndConnection();
                //if (!bInTransaction) this.FinishTransaction(true, oTransaction);
                //if (!bInSession) this.CloseSession(oSession);
            }

            return res;
        }

        public bool Insert(object oEntity, IVehicleTypeTransaction trans = null)
        {
            bool bRet = false;

            OracleConnectionHelper oConn = new OracleConnectionHelper(m_statelessSession, trans);

            try
            {
                oConn.BeginConnection(true);

                // ***
                if (!oConn.StatelessSession.IsOpen)
                {
                    m_Log.LogMessage(LogLevels.logWARN, "Save: STATELESS SESSION CLOSED!!!!!!");
                }
                // ***

                var oId = oConn.StatelessSession.Insert(oEntity);

                bRet = true;
            }
            catch (NHibernate.Exceptions.SqlParseException e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Save: ", e);
                throw e;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Save: ", e);
                throw e;
            }
            finally
            {
                oConn.EndConnection(bRet);
            }

            return bRet;
        }

        public bool Update(object oEntity, IVehicleTypeTransaction trans = null)
        {
            bool bRet = false;

            OracleConnectionHelper oConn = new OracleConnectionHelper(m_statelessSession, trans);

            try
            {
                oConn.BeginConnection();
                oConn.StatelessSession.Update(oEntity);

                bRet = true;
            }
            catch (NHibernate.Exceptions.SqlParseException e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Update: ", e);
                throw e;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Update: ", e);
                throw e;
            }
            finally
            {
                oConn.EndConnection(bRet);
            }
            return bRet;
        }

        public bool Delete(object oEntity, IVehicleTypeTransaction trans = null)
        {
            bool bRet = false;

            OracleConnectionHelper oConn = new OracleConnectionHelper(m_statelessSession, trans);

            try
            {
                oConn.BeginConnection();
                oConn.StatelessSession.Delete(oEntity);

                bRet = true;
            }
            catch (NHibernate.Exceptions.SqlParseException e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Delete: ", e);
                throw e;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Delete: ", e);
                throw e;
            }
            finally
            {
                oConn.EndConnection(bRet);
            }

            return bRet;
        }

        public bool ExecuteUpdate(string sQuery, List<QueryParameter> oParameters, IVehicleTypeTransaction trans = null)
        {
            bool bRet = false;

            OracleConnectionHelper oConn = new OracleConnectionHelper(m_statelessSession, trans);

            try
            {
                oConn.BeginConnection();

                //var sQuery = "update Unit set UniDate = :date, UniIp = :ip where UniId = :id";
                var cmdUpdate = oConn.StatelessSession.CreateQuery(sQuery);

                cmdUpdate = oParameters.Aggregate(cmdUpdate,
                    (current, oParam) =>
                        current.SetParameter(oParam.Name, oParam.Value, NHibernateUtil.GuessType(oParam.Type)));
                cmdUpdate.ExecuteUpdate();

                bRet = true;
            }
            catch (NHibernate.Exceptions.SqlParseException e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ExecuteUpdate: ", e);
                throw e;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ExecuteUpdate: ", e);
                throw e;
            }
            finally
            {
                oConn.EndConnection(bRet);
            }
            return bRet;

        }

        public IList GetList(Type oTypeEntity, ICriterion oCriteria = null, Order oOrder = null, VehicleTypeLockMode lockMode = VehicleTypeLockMode.None, IVehicleTypeTransaction trans = null)
        {
            IList res = null;

            OracleConnectionHelper oConn = new OracleConnectionHelper(m_statelessSession, trans);

            try
            {
                oConn.BeginConnection();

                ICriteria criteria = oConn.StatelessSession.CreateCriteria(oTypeEntity);

                if (oCriteria != null)
                {
                    criteria.Add(oCriteria);
                }

                if (oOrder != null)
                {
                    criteria.AddOrder(oOrder);
                }

                criteria.SetLockMode(this.GetNHLockMode(lockMode));

                //ICriterion oWhere = NHibernate.Criterion.Expression.Where<MsgsCommand>(t => t.MsgcStatus == 0);
                //ICriteria criteria = oConn.Session.CreateCriteria(typeof(MsgsCommand)).Add(oCriteria).SetLockMode(this.GetNHLockMode(lockMode));

                res = criteria.List();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetList: ", e);
            }
            finally
            {
                oConn.EndConnection();
            }

            return res;
        }

        public void Dispose()
        {
            if (m_statelessSession == null)
            {
                return;
            }

            if (m_statelessSession.IsOpen)
            {
                //m_Log.LogMessage(LogLevels.logDEBUG, "Dispose",
                //    string.Format("Close stateless session '{0}'",
                //        SerializerHelper.GetObjectUniqueID(m_statelessSession)));
                m_statelessSession.Close();
            }

            m_statelessSession.Dispose();
            m_statelessSession = null;
        }

        public bool IsSessionActive()
        {
            bool bRet = false;

            if (m_statelessSession != null)
            {
                bRet = m_statelessSession.IsOpen;
            }

            return bRet;
        } 
        #endregion

        #region Private Methods
        private LockMode GetNHLockMode(VehicleTypeLockMode lockMode)
        {
            LockMode oRet = LockMode.None;

            switch (lockMode)
            {
                case VehicleTypeLockMode.None: oRet = LockMode.None; break;
                case VehicleTypeLockMode.Read: oRet = LockMode.Read; break;
                case VehicleTypeLockMode.Upgrade: oRet = LockMode.Upgrade; break;
                case VehicleTypeLockMode.UpgradeNoWait: oRet = LockMode.UpgradeNoWait; break;
                case VehicleTypeLockMode.Write: oRet = LockMode.Write; break;
            }

            return oRet;
        } 

        #endregion
    }
}