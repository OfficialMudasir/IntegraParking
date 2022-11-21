
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using DBBench.Tools;


namespace DBBench.Mysql
{
    public class MysqlBaseRepository : IBaseRepository
    {
        #region Protected Attrib

        protected bool m_bStateless = false;
        protected ISession m_session = null;
        protected IStatelessSession m_statelessSession = null;
        protected NHSessionManager.ConnectionConfiguration m_oConnectionConfig = null;
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(MysqlBaseRepository));


        #endregion

        #region Constructor

        public MysqlBaseRepository(Type parentType, NHSessionManager.ConnectionConfiguration oConnectionConfig, bool bOpenSession, bool bStatelessSession = false)
        {
            m_bStateless = bStatelessSession;
            m_oConnectionConfig = oConnectionConfig;

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

        ~MysqlBaseRepository()
        {
            Dispose();
        }

        #endregion

        #region Protected Methods

        protected ITransaction BeginTransaction(ISession session)
        {
            return session.BeginTransaction();
        }

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
                oTransaction.Dispose();
            }
        }

        protected MysqlConnectionHelper NewMysqlConnectionHelper(IMysqlTransaction trans)
        {
            MysqlConnectionHelper oConn = !m_bStateless
                ? new MysqlConnectionHelper(m_oConnectionConfig, m_session, trans)
                : new MysqlConnectionHelper(m_oConnectionConfig, m_statelessSession, trans);

            return oConn;
        }

        #endregion

        #region Public Methods: Operations

        public IMysqlTransaction CreateTransaction()
        {
            IMysqlTransaction oiParkTicketTransaction;

            if (!m_bStateless)
            {
                oiParkTicketTransaction = m_session == null
                    ? new MysqlTransaction(m_oConnectionConfig, m_bStateless)
                    : new MysqlTransaction(m_session);
            }
            else
            {
                oiParkTicketTransaction = m_statelessSession == null
                    ? new MysqlTransaction(m_oConnectionConfig, m_bStateless)
                    : new MysqlTransaction(m_statelessSession);
            }

            return oiParkTicketTransaction;
        }

        public Type GetEntityType(string sEntityName, string sNamespace = "GtechnaExport.Domain.Entities")
        {
            return Type.GetType(string.Format("{0}.{1}", sNamespace, sEntityName));
        }

        public IQueryable GetQuery(Type typeEntity, IMysqlTransaction trans = null, int? iTimeoutSeconds = null)
        {
            IQueryable res = null;

            MysqlConnectionHelper oConn = NewMysqlConnectionHelper(trans);

            oConn.BeginConnection();

            MethodInfo queryMethod = typeof(LinqExtensionMethods).GetMethod("Query", new[] { typeof(ISession) });
            if (queryMethod != null) queryMethod = queryMethod.MakeGenericMethod(typeEntity);
            if (!m_bStateless)
                res = (IQueryable)queryMethod.Invoke(null, new object[] { oConn.Session });
            else
                res = (IQueryable)queryMethod.Invoke(null, new object[] { oConn.StatelessSession });

            if (iTimeoutSeconds.HasValue)
            {
                MethodInfo timeoutMethod = typeof(LinqExtensionMethods).GetMethod("Timeout");
                if (timeoutMethod != null) timeoutMethod = timeoutMethod.MakeGenericMethod(typeEntity);
                timeoutMethod.Invoke(null, new object[] { res, iTimeoutSeconds.Value });
            }

            oConn.EndConnection();

            return res;
        }

        public bool Save(object oEntity, IMysqlTransaction trans = null, bool bFlush = true)
        {
            var result = false;

            using (var connection = NewMysqlConnectionHelper(trans))
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
                    Console.Write(e.Message);
                }
                finally
                {
                    connection.EndConnection();
                }
            }

            return result;
        }

        public bool SaveOrUpdate(object oEntity, IMysqlTransaction trans = null)
        {
            bool bRet = false;

            var oConn = new MysqlConnectionHelper(m_oConnectionConfig, m_session, trans);

            oConn.BeginConnection();

            oConn.Session.SaveOrUpdate(oEntity);
            oConn.Session.Flush();

            oConn.EndConnection(bRet);

            bRet = true;

            return bRet;
        }

        public bool Delete(object oEntity, IMysqlTransaction trans = null)
        {
            bool bRet = false;

            var oConn = new MysqlConnectionHelper(m_oConnectionConfig, m_session, trans);

            oConn.BeginConnection();

            oConn.Session.Delete(oEntity);
            oConn.Session.Flush();

            oConn.EndConnection(bRet);

            bRet = true;

            return bRet;
        }

        public IList GetList(Type oTypeEntity, ICriterion oCriteria = null, Order oOrder = null,
            MysqlLockMode lockMode = MysqlLockMode.None, IMysqlTransaction trans = null)
        {
            IList res = null;

            var oConn = new MysqlConnectionHelper(m_oConnectionConfig, m_session, trans);

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

        public IList GetSQLQuery(Type oTypeEntity, string sQuery, IMysqlTransaction trans = null)
        {
            IList res = null;

            var oConn = new MysqlConnectionHelper(m_oConnectionConfig, m_session, trans);

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

        public IList GetHQLQuery(Type oTypeEntity, string sQuery, IMysqlTransaction trans = null)
        {
            IList res = null;

            var oConn = new MysqlConnectionHelper(m_oConnectionConfig, m_session, trans);

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
            IList<MysqlStoredProcedureParameter> parameters, IMysqlTransaction trans = null)
        {
            IEnumerable<TOut> result = null;

            MysqlConnectionHelper oConn = NewMysqlConnectionHelper(trans);

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
            IList<MysqlStoredProcedureParameter> parameters, IMysqlTransaction trans = null)
        {
            TOut result = default(TOut);

            MysqlConnectionHelper oConn = NewMysqlConnectionHelper(trans);

            oConn.BeginConnection();

            if (!m_bStateless)
            {
                IQuery query = oConn.Session.GetNamedQuery(procedureName);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(Transformers.AliasToBean(typeof(TOut))).UniqueResult<TOut>();
            }

            oConn.EndConnection();

            return result;
        }

        protected
            IQuery AddStoredProcedureParameters(IQuery query,
            IEnumerable<MysqlStoredProcedureParameter> parameters)
        {
            foreach (MysqlStoredProcedureParameter parameter in parameters)
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

        private LockMode GetNHLockMode(MysqlLockMode lockMode)
        {
            LockMode oRet = LockMode.None;

            switch (lockMode)
            {
                case MysqlLockMode.None:
                    oRet = LockMode.None;
                    break;
                case MysqlLockMode.Read:
                    oRet = LockMode.Read;
                    break;
                case MysqlLockMode.Upgrade:
                    oRet = LockMode.Upgrade;
                    break;
                case MysqlLockMode.UpgradeNoWait:
                    oRet = LockMode.UpgradeNoWait;
                    break;
                case MysqlLockMode.Write:
                    oRet = LockMode.Write;
                    break;
            }

            return oRet;
        }

        protected static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }


        protected static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        #endregion
    }
}
