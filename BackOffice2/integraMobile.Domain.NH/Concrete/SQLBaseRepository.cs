using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
//using System.Data.Linq;
using System.Text;
using System.Reflection;
using PIC.Domain.Abstract;
using integraMobile.Domain.NH;
using integraMobile.Domain.NH.Entities;
using PIC.Infrastructure.Logging;
using PIC.Infrastructure;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace integraMobile.Domain.NH.Concrete
{
    public class SQLBaseRepository : IBaseRepository
    {
        //Log4net Wrapper class
        protected static CLogWrapper m_Log = null;

        protected string m_sConnectionString = "";
        protected ISession m_session = null;

        public SQLBaseRepository(Type parentType, string sConnectionString, bool bOpenSession)
        {
            if (m_Log == null) m_Log = new CLogWrapper(parentType);
            m_sConnectionString = sConnectionString;
            if (bOpenSession)
            {
                m_session = NHSessionManager.SessionFactory(sConnectionString).OpenSession();
                //if (m_session.Connection != null) m_sConnectionString = m_session.Connection.ConnectionString;
                //m_session = ConfigurationHelper.CreateConfiguration().BuildSessionFactory().OpenSession();
            }
        }

        ~SQLBaseRepository()
        {
            if (m_session != null)
            {
                if (m_session.IsOpen) m_session.Close();
                m_session.Dispose();
                m_session = null;
            }
            /*if (m_sessionFact != null)
            {
                m_sessionFact.Close();
                m_sessionFact.Dispose();
                m_sessionFact = null;
            }
            m_config = null;*/
        }


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
        /*public ITransaction BeginTransaction(out ISession oSession)
        {
            oSession = this.GetSession();
            return BeginTransaction(oSession);
        }*/
        protected void CommitTransaction(ITransaction oTransaction)
        {
            if (oTransaction != null)
            {
                oTransaction.Commit();
                oTransaction.Dispose();
                oTransaction = null;
            }
        }
        protected void RollbackTransaction(ITransaction oTransaction)
        {
            if (oTransaction != null)
            {
                oTransaction.Rollback();
                oTransaction.Dispose();
                oTransaction = null;
            }
        }
        protected void FinishTransaction(bool bCommit, ITransaction oTransaction)
        {
            if (oTransaction != null)
            {
                if (bCommit)
                    oTransaction.Commit();
                else
                    oTransaction.Rollback();
                oTransaction.Dispose();
                oTransaction = null;
            }
        }

        public IPICTransaction CreateTransaction()
        {
            IPICTransaction oPICTransaction = null;
            if (m_session == null)
                oPICTransaction = new PICTransaction();
            else
                oPICTransaction = new PICTransaction(m_session);
            return oPICTransaction;
        }

        public Type GetEntityType(string sEntityName, string sNamespace = "integraMobile.Domain.NH.Entities")
        {
            Type oRet = null;
            try
            {
                oRet = System.Type.GetType(string.Format("{0}.{1}", sNamespace, sEntityName));
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetEntityType: ", ex);
            }
            return oRet;
        }

        public IQueryable GetQuery(Type typeEntity, IPICTransaction trans = null, int? iTimeoutSeconds = null)
        {
            IQueryable res = null;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);
            /*PICTransaction oPICTransaction = null;
            if (trans != null) oPICTransaction = (PICTransaction)trans;
            ISession oSession = (oPICTransaction != null ? oPICTransaction.Session : null);
            ITransaction oTransaction = (oPICTransaction != null ? oPICTransaction.Transaction : null);
            bool bInSession = (oSession != null);
            bool bInTransaction = (oTransaction != null);*/

            try
            {                
                oConn.BeginConnection("", true, System.Data.IsolationLevel.ReadUncommitted);
                //if (!bInSession) oSession = this.GetSession();
                //if (!bInTransaction) oTransaction = this.BeginTransaction(oSession);

                //MethodInfo queryMethod = session.GetType().GetMethod("Query").MakeGenericMethod(typeEntity);
                MethodInfo queryMethod = typeof(LinqExtensionMethods).GetMethod("Query", new Type[] { typeof(ISession) });
                if (queryMethod != null) queryMethod = queryMethod.MakeGenericMethod(typeEntity);
                res = (IQueryable)queryMethod.Invoke(null, new object[] { oConn.Session });

                if (iTimeoutSeconds.HasValue)
                {
                    MethodInfo timeoutMethod = typeof(LinqExtensionMethods).GetMethod("Timeout"); //, new Type[] { typeof(IQueryable), typeof(int) });
                    if (timeoutMethod != null) timeoutMethod = timeoutMethod.MakeGenericMethod(typeEntity);
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

        public bool Save(object oEntity, IPICTransaction trans = null, bool bFlush = true)
        {
            bool bRet = false;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection("", false);

                object oId = oConn.Session.Save(oEntity);
                if (bFlush) oConn.Session.Flush();
                //session.SaveOrUpdate(oEntity);                

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

        public bool SaveOrUpdate(object oEntity, IPICTransaction trans = null)
        {
            bool bRet = false;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                oConn.Session.SaveOrUpdate(oEntity);
                oConn.Session.Flush();                

                bRet = true;
            }
            catch (NHibernate.Exceptions.SqlParseException e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SaveOrUpdate: ", e);
                throw e;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SaveOrUpdate: ", e);
                throw e;
            }
            finally
            {
                oConn.EndConnection(bRet);
            }
            return bRet;
        }

        public bool Delete(object oEntity, IPICTransaction trans = null)
        {
            bool bRet = false;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                oConn.Session.Delete(oEntity);                
                oConn.Session.Flush();

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

        /*public bool Delete(string sQuery, object[] values, NHibernate.Type.IType[] types, IPICTransaction trans = null)
        {
            bool bRet = false;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                oConn.Session.Delete(sQuery, values, types);
                oConn.Session.Flush();

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
        }*/

        public IList GetList(Type oTypeEntity, ICriterion oCriteria = null, Order oOrder = null, PICLockMode lockMode = PICLockMode.None, IPICTransaction trans = null)
        {
            IList res = null;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try            
            {                
                oConn.BeginConnection();

                ICriteria criteria = oConn.Session.CreateCriteria(oTypeEntity);
                                
                if (oCriteria != null)
                    criteria.Add(oCriteria);
                if (oOrder != null)
                    criteria.AddOrder(oOrder);

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

        public IList GetSQLQuery(Type oTypeEntity, string sQuery, IPICTransaction trans = null)
        {
            IList res = null;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                res = oConn.Session.CreateSQLQuery(sQuery)
                                    .AddEntity(oTypeEntity)
                    //.SetInt32("orderYear", 2012)
                                    .List();


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
        public IList GetHQLQuery(Type oTypeEntity, string sQuery, IPICTransaction trans = null)
        {
            IList res = null;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                res = oConn.Session.CreateQuery(sQuery)
                    //.SetInt32("orderYear", 2012)
                                    .List();

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

        public IEnumerable<TOut> ExecuteStoredProcedure<TOut>(string procedureName, IList<PICStoredProcedureParameter> parameters, IPICTransaction trans = null)
        {
            IEnumerable<TOut> result = null;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                var query = oConn.Session.GetNamedQuery(procedureName);

                AddStoredProcedureParameters(query, parameters);
                result = query.List<TOut>();
                //result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(GetDbUnitsMeasuresHourResult)))
                //            .List<GetDbUnitsMeasuresHourResult>().AsQueryable();


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ExecuteStoredProcedure: ", e);
            }
            finally
            {
                oConn.EndConnection();
            }

            return result;
        }
        public IEnumerable<TOut> ExecuteStoredProcedure<TOut>(string procedureName, IList<PICStoredProcedureParameter> parameters, int iTimeout, IPICTransaction trans = null)
        {
            IEnumerable<TOut> result = null;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                var query = oConn.Session.GetNamedQuery(procedureName).SetTimeout(iTimeout);

                AddStoredProcedureParameters(query, parameters);
                result = query.List<TOut>();
                //result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(GetDbUnitsMeasuresHourResult)))
                //            .List<GetDbUnitsMeasuresHourResult>().AsQueryable();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ExecuteStoredProcedure: ", e);
            }
            finally
            {
                oConn.EndConnection();
            }

            return result;
        }

        public TOut ExecuteScalarStoredProcedure<TOut>(string procedureName, IList<PICStoredProcedureParameter> parameters, IPICTransaction trans = null)
        {
            TOut result = default(TOut);

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                var query = oConn.Session.GetNamedQuery(procedureName);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(TOut))).UniqueResult<TOut>();

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ExecuteScalarStoredProcedure: ", e);
            }
            finally
            {
                oConn.EndConnection();
            }

            return result;
        }
        public TOut ExecuteScalarStoredProcedure<TOut>(string procedureName, IList<PICStoredProcedureParameter> parameters, int iTimeout, IPICTransaction trans = null)
        {
            TOut result = default(TOut);

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                var query = oConn.Session.GetNamedQuery(procedureName).SetTimeout(iTimeout);
                AddStoredProcedureParameters(query, parameters);
                result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(TOut))).UniqueResult<TOut>();
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ExecuteScalarStoredProcedure: ", e);
            }
            finally
            {
                oConn.EndConnection();
            }

            return result;
        }

        public bool ExecuteHQLQuery(string sQuery, IList<PICStoredProcedureParameter> parameters = null, IPICTransaction trans = null)
        {
            bool bRet = false;

            SQLConnectionHelper oConn = new SQLConnectionHelper(m_session, trans);

            try
            {
                oConn.BeginConnection();

                var cmdUpdate = oConn.Session.CreateQuery(sQuery);

                if (parameters != null)
                    AddStoredProcedureParameters(cmdUpdate, parameters);

                cmdUpdate.ExecuteUpdate();

                bRet = true;
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SQLBaseRepository::ExecuteHQLQuery: ", e);
            }
            finally
            {
                oConn.EndConnection(bRet);
            }

            return bRet;
        }

        protected static IQuery AddStoredProcedureParameters(IQuery query, IEnumerable<PICStoredProcedureParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                query.SetParameter(parameter.ParamName, parameter.ParamValue);
            }

            return query;
        }

        public void Dispose()
        {
            if (m_session != null)
            {
                if (m_session.IsOpen) m_session.Close();
                m_session.Dispose();
                m_session = null;
            }
        }

        public bool IsSessionActive()
        {
            bool bRet = false;

            if (m_session != null)
            {
                bRet = m_session.IsOpen;
            }

            return bRet;
        }


        private LockMode GetNHLockMode(PICLockMode lockMode)
        {
            LockMode oRet = LockMode.None;
            switch (lockMode)
            {
                case PICLockMode.None: oRet = LockMode.None; break;
                case PICLockMode.Read: oRet = LockMode.Read; break;
                case PICLockMode.Upgrade: oRet = LockMode.Upgrade; break;
                case PICLockMode.UpgradeNoWait: oRet = LockMode.UpgradeNoWait; break;
                case PICLockMode.Write: oRet = LockMode.Write; break;
            }
            return oRet;
        }
    }
}
