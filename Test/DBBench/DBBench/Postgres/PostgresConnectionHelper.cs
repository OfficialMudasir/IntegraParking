
using NHibernate;
using System;
using DBBench.Tools;


namespace DBBench.Postgres
{ 
    public class PostgresConnectionHelper : IDisposable
    {
        #region Protected Attrib

        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(PostgresConnectionHelper));

        #endregion

        #region Private Attrib

        private readonly bool _inSession;
        private readonly bool _inTransaction;
        private readonly bool _iniParkTicketSession;
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

        public PostgresConnectionHelper(ISession oSession, IPostgresTransaction trans = null)
        {
            _stateless = false;

            if (trans != null && trans.GetType() == typeof(PostgresTransaction))
            {
                var oiParkTicketTransaction = (PostgresTransaction)trans;
                Session = oiParkTicketTransaction.Session;
                Transaction = oiParkTicketTransaction.Transaction;
                _iniParkTicketSession = (Session != null);
            }
            else
            {
                Session = oSession;
                Transaction = (oSession != null ? oSession.GetCurrentTransaction() : null);
            }

            _inSession = (Session != null);
            _inTransaction = (Transaction != null);
        }

        public PostgresConnectionHelper(IStatelessSession oStatelessSession, IPostgresTransaction trans = null)
        {
            _stateless = true;

            if (trans != null && trans.GetType() == typeof(PostgresTransaction))
            {
                var oiParkTicketTransaction = (PostgresTransaction)trans;
                StatelessSession = oiParkTicketTransaction.StatelessSession;
                Transaction = oiParkTicketTransaction.Transaction;
                _iniParkTicketSession = (StatelessSession != null);
            }
            else
            {
                StatelessSession = oStatelessSession;
                Transaction = (oStatelessSession != null ? oStatelessSession.GetCurrentTransaction() : null);
            }

            _inSession = (StatelessSession != null);
            _inTransaction = (Transaction != null);
        }

        public PostgresConnectionHelper(NHSessionManager.ConnectionConfiguration oConnectionConfig, ISession oSession, IPostgresTransaction trans = null)
        {
            _stateless = false;
            _connectionConfig = oConnectionConfig;

            if (trans != null && trans.GetType() == typeof(PostgresTransaction))
            {
                var oiParkTicketTransaction = (PostgresTransaction)trans;
                Session = oiParkTicketTransaction.Session;
                Transaction = oiParkTicketTransaction.Transaction;
                _iniParkTicketSession = (Session != null);
            }
            else
            {
                Session = oSession;
                Transaction = (oSession != null ? oSession.GetCurrentTransaction() : null);
            }

            _inSession = (Session != null);
            _inTransaction = (Transaction != null);
        }

        public PostgresConnectionHelper(NHSessionManager.ConnectionConfiguration oConnectionConfig, IStatelessSession oStatelessSession, IPostgresTransaction trans = null)
        {
            _stateless = true;
            _connectionConfig = oConnectionConfig;

            if (trans != null && trans.GetType() == typeof(PostgresTransaction))
            {
                var oiParkTicketTransaction = (PostgresTransaction)trans;
                StatelessSession = oiParkTicketTransaction.StatelessSession;
                Transaction = oiParkTicketTransaction.Transaction;
                _iniParkTicketSession = (StatelessSession != null);
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

        ~PostgresConnectionHelper()
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
                        if (!_inSession)
                        {
                            _newSession = true;

                        }
                    }
                    //if (_globalSession == null) _globalSession = this._sessionFact.OpenSession();
                    //this._session = _globalSession;
                }
                if (!_inTransaction && !_iniParkTicketSession && bWithTransaction)
                    Transaction = Session.BeginTransaction( /*System.Data.IsolationLevel.ReadUncommitted*/);
            }
            else
            {
                if (!_inSession || (StatelessSession != null && !StatelessSession.IsOpen))
                {
                    StatelessSession = NHSessionManager.SessionFactory(oConnectionConfig).OpenStatelessSession();


                    if (!_inSession)
                    {
                        _newSession = true;
                    }
                }

                if (!_inTransaction && !_iniParkTicketSession && bWithTransaction)
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
                    StatelessSession.Close();
                }

                StatelessSession.Dispose();
                StatelessSession = null;
            }
        }

        public IPostgresTransaction GetiParkTicketTransaction()
        {
            return !_stateless
                ? new PostgresTransaction(Session, Transaction)
                : new PostgresTransaction(StatelessSession, Transaction);
        }

        #endregion

        protected static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }


        protected static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        #region IDisposable

        public void Dispose()
        {
            if (!_stateless)
            {
                if (_newSession && Session != null)
                {
                    if (Session.IsOpen)
                    {
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
                        StatelessSession.Close();
                    }

                    StatelessSession.Dispose();
                    StatelessSession = null;
                }
            }
        }

        #endregion
    }
}