using VehicleType.WS.Infrastructure.Logging.Tools;
using VehicleType.WS.Domain.Abstract;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VehicleType.WS.Domain.Concrete
{
    public class OracleBaseRepository : IBaseRepository
    {
        #region Protected Attrib

        protected CLogWrapper Log = null;
        protected bool m_bStateless = false;
        protected NHibernate.ISession m_session = null;
        protected IStatelessSession m_statelessSession = null;
        protected NHSessionManager.ConnectionConfiguration m_oConnectionConfig = null;

        #endregion

        #region Constructor

        public OracleBaseRepository(
            Type parentType, 
            NHSessionManager.ConnectionConfiguration oConnectionConfig,
            bool bOpenSession, 
            bool bStatelessSession = false)
        {
            m_bStateless = bStatelessSession;
            m_oConnectionConfig = oConnectionConfig;

            if (Log == null)
                Log = new CLogWrapper(parentType);

            if (!bOpenSession)
                return;

            if (!m_bStateless)
            {
                m_session = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenSession();
            }
            else
            {
                m_statelessSession = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenStatelessSession();
            }

            //m_session = ConfigurationHelper.CreateConfiguration().BuildSessionFactory().OpenSession();
        }

        #endregion

        #region Destructor

        ~OracleBaseRepository()
        {
            Dispose();
            /*if (m_sessionFact != null)
            {
                m_sessionFact.Close();
                m_sessionFact.Dispose();
                m_sessionFact = null;
            }
            m_config = null;*/
        }

        #endregion

        #region Protected Methods

        /*protected ISession GetSession()
        {
            return m_sessionFact.OpenSession();
        }
        protected void CloseSession(ISession oSession)
        {
            if (oSession != null)
            {
                if (oSession.IsOpen) oSession.Close();
                oSession.Dispose();
                oSession = null;
            }
        }*/

        protected ITransaction BeginTransaction(ISession session)
        {
            return session.BeginTransaction();
        }

        protected ITransaction BeginTransaction(IStatelessSession statelessSession)
        {
            return statelessSession.BeginTransaction();
        }

        /*public ITransaction BeginTransaction(out ISession oSession)
        {
            oSession = this.GetSession();
            return BeginTransaction(oSession);
        }*/

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
                oTransaction.Dispose();
            }
        }

        protected OracleConnectionHelper NewOracleConnectionHelper(IVehicleTypeTransaction trans)
        {
            OracleConnectionHelper oConn = !m_bStateless
                ? new OracleConnectionHelper(m_oConnectionConfig, m_session, trans)
                : new OracleConnectionHelper(m_oConnectionConfig, m_statelessSession, trans);

            return oConn;
        }

        #endregion

        #region Public Methods: Operations

        public IVehicleTypeTransaction CreateTransaction()
        {
            IVehicleTypeTransaction oiParkTicketTransaction;

            if (!m_bStateless)
            {
                oiParkTicketTransaction = m_session == null
                    ? new VehicleTypeTransaction(m_oConnectionConfig, m_bStateless)
                    : new VehicleTypeTransaction(m_session);
            }
            else
            {
                oiParkTicketTransaction = m_statelessSession == null
                    ? new VehicleTypeTransaction(m_oConnectionConfig, m_bStateless)
                    : new VehicleTypeTransaction(m_statelessSession);
            }

            return oiParkTicketTransaction;
        }

        public Type GetEntityType(string sEntityName, string sNamespace = "iParkTicket.Domain.Entities")
        {   
            return Type.GetType(string.Format("{0}.{1}", sNamespace, sEntityName));
        }

        public IQueryable GetQuery(Type typeEntity, IVehicleTypeTransaction trans = null, int? iTimeoutSeconds = null)
        {
            IQueryable res = null;

            OracleConnectionHelper oConn = NewOracleConnectionHelper(trans);
            
            oConn.BeginConnection();
            
            MethodInfo queryMethod = typeof (LinqExtensionMethods).GetMethod("Query", new[] {typeof (ISession)});
            if (queryMethod != null) queryMethod = queryMethod.MakeGenericMethod(typeEntity);
            if (!m_bStateless)
                res = (IQueryable) queryMethod.Invoke(null, new object[] {oConn.Session});
            else
                res = (IQueryable) queryMethod.Invoke(null, new object[] {oConn.StatelessSession});

            if (iTimeoutSeconds.HasValue)
            {
                MethodInfo timeoutMethod = typeof (LinqExtensionMethods).GetMethod("Timeout");
                    //, new Type[] { typeof(IQueryable), typeof(int) });
                if (timeoutMethod != null) timeoutMethod = timeoutMethod.MakeGenericMethod(typeEntity);
                timeoutMethod.Invoke(null, new object[] {res, iTimeoutSeconds.Value});
            }            
            
            oConn.EndConnection();
            
            return res;
        }

        public bool Save(object oEntity, IVehicleTypeTransaction trans = null, bool bFlush = true)
        {
            var result = false;

            using (var connection = NewOracleConnectionHelper(trans))
            {
                try
                {
                    connection.BeginConnection();

                    if (!m_bStateless)
                    {
                        connection.Session.Save(oEntity);

                        if (bFlush)
                            connection.Session.Flush();

                        result = true;
                    }
                    else
                    {
                        connection.StatelessSession.Insert(oEntity);

                        if (bFlush)
                            connection.Session.Flush();

                        result = true;
                    }
                }
                catch (Exception e)
                {
                    Log.LogMessage(LogLevels.logERROR, string.Format("OracleBaseRepository error (Save): {0}", e));
                }
                finally
                {
                    connection.EndConnection();
                }
            }

            return result;
        }

        public bool SaveOrUpdate(object oEntity, IVehicleTypeTransaction trans = null)
        {
            bool bRet = false;

            var oConn = new OracleConnectionHelper(m_oConnectionConfig, m_session, trans);

            oConn.BeginConnection();

            oConn.Session.SaveOrUpdate(oEntity);
            oConn.Session.Flush();

            oConn.EndConnection(bRet);

            bRet = true;
            
            return bRet;
        }

        public bool Delete(object oEntity, IVehicleTypeTransaction trans = null)
        {
            bool bRet = false;

            var oConn = new OracleConnectionHelper(m_oConnectionConfig, m_session, trans);

            oConn.BeginConnection();

            oConn.Session.Delete(oEntity);
            oConn.Session.Flush();

            oConn.EndConnection(bRet);

            bRet = true;

            return bRet;
        }

        public IList GetList(Type oTypeEntity, ICriterion oCriteria = null, Order oOrder = null,
            VehicleTypeLockMode lockMode = VehicleTypeLockMode.None, IVehicleTypeTransaction trans = null)
        {
            IList res = null;

            var oConn = new OracleConnectionHelper(m_oConnectionConfig, m_session, trans);

            oConn.BeginConnection();

            ICriteria criteria = oConn.Session.CreateCriteria(oTypeEntity);

            if (oCriteria != null)
            {
                criteria.Add(oCriteria);
            }

            if (oOrder != null)
            {
                criteria.AddOrder(oOrder);
            }

            criteria.SetLockMode(GetNHLockMode(lockMode));

            //ICriterion oWhere = NHibernate.Criterion.Expression.Where<MsgsCommand>(t => t.MsgcStatus == 0);
            //ICriteria criteria = oConn.Session.CreateCriteria(typeof(MsgsCommand)).Add(oCriteria).SetLockMode(this.GetNHLockMode(lockMode));

            res = criteria.List();
            
            oConn.EndConnection();
            
            return res;
        }

        public IList GetSQLQuery(Type oTypeEntity, string sQuery, IVehicleTypeTransaction trans = null)
        {
            IList res = null;

            var oConn = new OracleConnectionHelper(m_oConnectionConfig, m_session, trans);

            oConn.BeginConnection();

            if (!m_bStateless)
            {
                res = oConn.Session.CreateSQLQuery(sQuery)
                    .AddEntity(oTypeEntity)
                    //.SetInt32("orderYear", 2012)
                    .List();
            }
            else
            {
                res = oConn.StatelessSession.CreateSQLQuery(sQuery)
                    .AddEntity(oTypeEntity)
                    .List();
            }
        
            oConn.EndConnection();

            return res;
        }

        public IList GetHQLQuery(Type oTypeEntity, string sQuery, IVehicleTypeTransaction trans = null)
        {
            IList res = null;

            var oConn = new OracleConnectionHelper(m_oConnectionConfig, m_session, trans);

            oConn.BeginConnection();

            if (!m_bStateless)
            {
                res = oConn.Session.CreateQuery(sQuery)
                    //.SetInt32("orderYear", 2012)
                    .List();
            }
            else
            {
                res = oConn.StatelessSession.CreateQuery(sQuery)
                    .List();
            }
            
            oConn.EndConnection();
            
            return res;
        }

        public IEnumerable<TOut> ExecuteStoredProcedure<TOut>(string procedureName,
            IList<StoredProcedureParameter> parameters, IVehicleTypeTransaction trans = null)
        {
            IEnumerable<TOut> result = null;

            OracleConnectionHelper oConn = NewOracleConnectionHelper(trans);

            oConn.BeginConnection();

            if (!m_bStateless)
            {
                IQuery query = oConn.Session.GetNamedQuery(procedureName);

                AddStoredProcedureParameters(query, parameters);
                result = query.List<TOut>();
                //result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(GetDbUnitsMeasuresHourResult)))
                //            .List<GetDbUnitsMeasuresHourResult>().AsQueryable();
            }
            
            oConn.EndConnection();
            
            return result;
        }

        public TOut ExecuteScalarStoredProcedure<TOut>(string procedureName,
            IList<StoredProcedureParameter> parameters, IVehicleTypeTransaction trans = null)
        {
            TOut result = default(TOut);

            OracleConnectionHelper oConn = NewOracleConnectionHelper(trans);

            oConn.BeginConnection();

            if (!m_bStateless)
            {
                IQuery query = oConn.Session.GetNamedQuery(procedureName);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(Transformers.AliasToBean(typeof (TOut))).UniqueResult<TOut>();
            }
            
            oConn.EndConnection();
            
            return result;
        }

        protected 
            IQuery AddStoredProcedureParameters(IQuery query,
            IEnumerable<StoredProcedureParameter> parameters)
        {
            foreach (StoredProcedureParameter parameter in parameters)
            {
                query.SetParameter(parameter.ParamName, parameter.ParamValue);
            }

            return query;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (!m_bStateless)
            {
                if (m_session == null)
                {
                    return;
                }

                if (m_session.IsOpen)
                {
                    //Log.LogMessage(LogLevels.logDEBUG, "Dispose",
                    //    string.Format("Close session '{0}'", SerializerHelper.GetObjectUniqueID(m_session)));
                    m_session.Close();
                }

                m_session.Dispose();
                m_session = null;
            }
            else
            {
                if (m_statelessSession == null)
                {
                    return;
                }

                if (m_statelessSession.IsOpen)
                {
                    //Log.LogMessage(LogLevels.logDEBUG, "Dispose",
                    //    string.Format("Close stateless session '{0}'",
                    //        SerializerHelper.GetObjectUniqueID(m_statelessSession)));
                    m_statelessSession.Close();
                }

                m_statelessSession.Dispose();
                m_statelessSession = null;
            }
        }

        #endregion

        #region Public Methods

        public bool IsSessionActive()
        {
            bool bRet = false;

            if (!m_bStateless)
            {
                if (m_session != null)
                {
                    bRet = m_session.IsOpen;
                }
            }
            else
            {
                if (m_statelessSession != null)
                {
                    bRet = m_statelessSession.IsOpen;
                }
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
                case VehicleTypeLockMode.None:
                    oRet = LockMode.None;
                    break;
                case VehicleTypeLockMode.Read:
                    oRet = LockMode.Read;
                    break;
                case VehicleTypeLockMode.Upgrade:
                    oRet = LockMode.Upgrade;
                    break;
                case VehicleTypeLockMode.UpgradeNoWait:
                    oRet = LockMode.UpgradeNoWait;
                    break;
                case VehicleTypeLockMode.Write:
                    oRet = LockMode.Write;
                    break;
            }

            return oRet;
        }

        #endregion
    }
}