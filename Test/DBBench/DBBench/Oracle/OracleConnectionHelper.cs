    
using NHibernate;
using System;
using DBBench.Tools;
using System.Runtime.Serialization;

namespace DBBench.Oracle
{
    public class OracleConnectionHelper : IDisposable
    {
        #region Protected Attrib

        protected static readonly CLogWrapper m_Log = new CLogWrapper(typeof(OracleConnectionHelper));

        #endregion

        #region Private Attrib

        private readonly bool _inSession;
        private readonly bool _inTransaction;
        private readonly bool _inOracleSession;
        private readonly bool _stateless;
        private bool _newSession;

        private NHSessionManager.ConnectionConfiguration _connectionConfig = null;

        #endregion

        #region Properties

        public ISession Session { get; private set; }

        public IStatelessSession StatelessSession { get; private set; }

        public ITransaction Transaction { get; private set; }

        public static NHSessionManager.ConnectionConfiguration ConnectionConfiguration { get; set; }

        #endregion

        #region Constructor

        public OracleConnectionHelper(ISession oSession, IOracleTransaction trans = null)
        {
            _stateless = false;

            if (trans != null && trans.GetType() == typeof(OracleTransaction))
            {
                var oOracleTransaction = (OracleTransaction)trans;
                Session = oOracleTransaction.Session;
                Transaction = oOracleTransaction.Transaction;
                _inOracleSession = (Session != null);
            }
            else
            {
                Session = oSession;
                Transaction = (oSession != null ? oSession.GetCurrentTransaction() : null);
            }

            _inSession = (Session != null);
            _inTransaction = (Transaction != null);
        }

        public OracleConnectionHelper(IStatelessSession oStatelessSession, IOracleTransaction trans = null)
        {
            _stateless = true;

            if (trans != null && trans.GetType() == typeof(OracleTransaction))
            {
                var oOracleTransaction = (OracleTransaction)trans;
                StatelessSession = oOracleTransaction.StatelessSession;
                Transaction = oOracleTransaction.Transaction;
                _inOracleSession = (StatelessSession != null);
            }
            else
            {
                StatelessSession = oStatelessSession;
                Transaction = (oStatelessSession != null ? oStatelessSession.GetCurrentTransaction() : null);
            }

            _inSession = (StatelessSession != null);
            _inTransaction = (Transaction != null);
        }

        public OracleConnectionHelper(NHSessionManager.ConnectionConfiguration oConnectionConfig, ISession oSession, IOracleTransaction trans = null)
        {
            _stateless = false;
            _connectionConfig = oConnectionConfig;

            if (trans != null && trans.GetType() == typeof(OracleTransaction))
            {
                var oOracleTransaction = (OracleTransaction)trans;
                Session = oOracleTransaction.Session;
                Transaction = oOracleTransaction.Transaction;
                _inOracleSession = (Session != null);
            }
            else
            {
                Session = oSession;
                Transaction = (oSession != null ? oSession.GetCurrentTransaction() : null);
            }

            _inSession = (Session != null);
            _inTransaction = (Transaction != null);
        }

        public OracleConnectionHelper(NHSessionManager.ConnectionConfiguration oConnectionConfig, IStatelessSession oStatelessSession, IOracleTransaction trans = null)
        {
            _stateless = true;
            _connectionConfig = oConnectionConfig;

            if (trans != null && trans.GetType() == typeof(OracleTransaction))
            {
                var oOracleTransaction = (OracleTransaction)trans;
                StatelessSession = oOracleTransaction.StatelessSession;
                Transaction = oOracleTransaction.Transaction;
                _inOracleSession = (StatelessSession != null);
            }
            else
            {
                StatelessSession = oStatelessSession;
                Transaction = (oStatelessSession != null ? oStatelessSession.GetCurrentTransaction() : null);
            }

            _inSession = (StatelessSession != null);
            _inTransaction = (Transaction != null);
        }
        #endregion

        #region Destructor

        ~OracleConnectionHelper()
        {
            //if (!_stateless)
            //{
            //    if (_newSession && Session != null)
            //    {
            //        if (Session.IsOpen)
            //        {
            //            m_Log.LogMessage(LogLevels.logDEBUG, "~OracleConnectionHelper",
            //                string.Format("Close session '{0}'", SerializerHelper.GetObjectUniqueID(Session)));
            //            Session.Close();
            //        }

            //        Session.Dispose();
            //        Session = null;
            //    }
            //}
            //else
            //{
            //    if (_newSession && StatelessSession != null)
            //    {
            //        if (StatelessSession.IsOpen)
            //        {
            //            m_Log.LogMessage(LogLevels.logDEBUG, "~OracleConnectionHelper",
            //                string.Format("Close stateless session '{0}'",
            //                    SerializerHelper.GetObjectUniqueID(StatelessSession)));
            //            StatelessSession.Close();
            //        }

            //        StatelessSession.Dispose();
            //        StatelessSession = null;
            //    }
            //}
            Dispose();
        }

        #endregion

        #region Methods

        public void BeginConnection(bool bWithTransaction = true)
        {
            NHSessionManager.ConnectionConfiguration oConnectionConfig = _connectionConfig;
            if (oConnectionConfig == null) oConnectionConfig = ConnectionConfiguration;

            if (!_stateless)
            {
                if (!_inSession || (Session != null && !Session.IsOpen))
                {
                    try
                    {
                        Session = NHSessionManager.CurrentSession(oConnectionConfig);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        //NHSessionManager.OpenSession();
                        //this._session = NHSessionManager.CurrentSession();                    
                        Session = NHSessionManager.SessionFactory(oConnectionConfig).OpenSession();
                        m_Log.LogMessage(LogLevels.logDEBUG, 
                            string.Format("BeginConnection: Open session '{0}'", GetObjectUniqueID(Session)));
                        if (!_inSession) _newSession = true;
                    }
                    //if (_globalSession == null) _globalSession = this._sessionFact.OpenSession();
                    //this._session = _globalSession;
                }
                if (!_inTransaction && !_inOracleSession && bWithTransaction)
                    Transaction = Session.BeginTransaction( /*System.Data.IsolationLevel.ReadUncommitted*/);
            }
            else
            {
                if (!_inSession || (StatelessSession != null && !StatelessSession.IsOpen))
                {
                    StatelessSession = NHSessionManager.SessionFactory(oConnectionConfig).OpenStatelessSession();
                    m_Log.LogMessage(LogLevels.logDEBUG, 
                        string.Format("BeginConnection: Open stateless session '{0}'",
                            GetObjectUniqueID(StatelessSession)));
                    if (!_inSession)
                    {
                        _newSession = true;
                    }
                }

                if (!_inTransaction && !_inOracleSession && bWithTransaction)
                {
                    Transaction = StatelessSession.BeginTransaction( /*System.Data.IsolationLevel.ReadUncommitted*/);
                }
            }
        }

        public void EndConnection(bool bCommit = true)
        {
            if (!_stateless)
            {
                if (!_inTransaction && Transaction != null)
                {
                    if (Transaction.IsActive && Session.IsOpen)
                    {
                        try
                        {
                            if (bCommit)
                                Transaction.Commit();
                            else
                                Transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                    }

                    Transaction = null;
                }

                if (!_newSession || Session == null)
                {
                    return;
                }

                //this._session.Clear();
                if (Session.IsOpen)
                {
                    m_Log.LogMessage(LogLevels.logDEBUG,
                        string.Format("EndConnection: Close session '{0}'", GetObjectUniqueID(Session)));
                    Session.Close();
                }

                Session.Dispose();
                Session = null;
            }
            else
            {
                if (!_inTransaction && Transaction != null)
                {
                    if (Transaction.IsActive && StatelessSession.IsOpen)
                    {
                        try
                        {
                            if (bCommit)
                                Transaction.Commit();
                            else
                                Transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                    }

                    Transaction = null;
                }

                if (!_newSession || StatelessSession == null)
                {
                    return;
                }

                //this._session.Clear();
                if (StatelessSession.IsOpen)
                {
                    m_Log.LogMessage(LogLevels.logDEBUG,
                        string.Format("EndConnection: Close stateless session '{0}'",
                            GetObjectUniqueID(StatelessSession)));
                    StatelessSession.Close();
                }

                StatelessSession.Dispose();
                StatelessSession = null;
            }
        }

        public IOracleTransaction GetOracleTransaction()
        {
            return !_stateless
                ? new OracleTransaction(Session, Transaction)
                : new OracleTransaction(StatelessSession, Transaction);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (!_stateless)
            {
                if (_newSession && Session != null)
                {
                    if (Session.IsOpen)
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, 
                            string.Format("~OracleConnectionHelper: Close session '{0}'", GetObjectUniqueID(Session)));
                        Session.Close();
                    }

                    Session.Dispose();
                    Session = null;
                }
            }
            else
            {
                if (_newSession && StatelessSession != null)
                {
                    if (StatelessSession.IsOpen)
                    {
                        m_Log.LogMessage(LogLevels.logDEBUG, 
                            string.Format("~OracleConnectionHelper: Close stateless session '{0}'",
                                GetObjectUniqueID(StatelessSession)));
                        StatelessSession.Close();
                    }

                    StatelessSession.Dispose();
                    StatelessSession = null;
                }
            }
        }

        private long GetObjectUniqueID(object obj)
        {
            var oGenerator = new ObjectIDGenerator();
            bool bFirst = false;
            return oGenerator.GetId(obj, out bFirst);
        }

        #endregion
    }
}