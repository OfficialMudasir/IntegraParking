
using NHibernate;
using System;
using System.Data;

namespace DBBench.Oracle
{
    public class OracleTransaction : IOracleTransaction
    {
        #region Private Attrib

        private readonly bool _stateless;

        #endregion

        #region Properties

        public ISession Session { get; private set; }

        public IStatelessSession StatelessSession { get; private set; }

        public ITransaction Transaction { get; private set; }

        public OracleTransaction(bool bStateless)
        {
            _stateless = bStateless;

            if (!bStateless)
            {
                Session = NHSessionManager.SessionFactory().OpenSession();
            }
            else
            {
                StatelessSession = NHSessionManager.SessionFactory().OpenStatelessSession();
            }
        }

        public OracleTransaction(NHSessionManager.ConnectionConfiguration oConnectionConfig, bool bStateless)
        {
            _stateless = bStateless;

            if (!bStateless)
            {
                Session = NHSessionManager.SessionFactory(oConnectionConfig).OpenSession();
            }
            else
            {
                StatelessSession = NHSessionManager.SessionFactory(oConnectionConfig).OpenStatelessSession();
            }
        }
        /*public OracleTransaction(ISessionFactory oSessionFact)
        {
            _session = oSessionFact.OpenSession();
        }*/

        #endregion

        #region Constructor

        public OracleTransaction(ISession oSession)
        {
            _stateless = false;
            Session = oSession;
        }
        public OracleTransaction(ISession oSession, ITransaction oTransaction)
        {
            _stateless = false;
            Session = oSession;
            Transaction = oTransaction;
        }
        public OracleTransaction(IStatelessSession oStatelessSession)
        {
            _stateless = true;
            StatelessSession = oStatelessSession;
        }
        public OracleTransaction(IStatelessSession oStatelessSession, ITransaction oTransaction)
        {
            _stateless = true;
            StatelessSession = oStatelessSession;
            Transaction = oTransaction;
        }

        #endregion

        #region Destructor

        ~OracleTransaction()
        {
            FinishTransaction(false);

            if (Session != null)
            {
                if (Session.IsOpen)
                {
                    try
                    {
                        Session.Clear();
                        Session.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                Session.Dispose();
                Session = null;
            }

            if (StatelessSession != null)
            {
                if (StatelessSession.IsOpen)
                {
                    try
                    {
                        //_statelessSession.Clear();
                        StatelessSession.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                StatelessSession.Dispose();
                StatelessSession = null;
            }
        }

        #endregion

        #region Public Methods

        public bool BeginTransaction()
        {
            var bRet = false;

            if (!_stateless)
            {
                if (Session == null || Transaction != null)
                {
                    return bRet;
                }

                Transaction = Session.BeginTransaction();
                bRet = true;
            }
            else
            {
                if (StatelessSession == null || Transaction != null)
                {
                    return bRet;
                }

                Transaction = StatelessSession.BeginTransaction();
                bRet = true;
            }

            return bRet;
        }

        public bool BeginTransaction(IsolationLevel isolationLevel)
        {
            var bRet = false;

            if (!_stateless)
            {
                if (Session == null || Transaction != null)
                {
                    return bRet;
                }

                Transaction = Session.BeginTransaction(isolationLevel);
                bRet = true;
            }
            else
            {
                if (StatelessSession == null || Transaction != null)
                {
                    return bRet;
                }

                Transaction = StatelessSession.BeginTransaction(isolationLevel);
                bRet = true;
            }

            return bRet;
        }

        public bool FinishTransaction(bool bCommit)
        {
            var bRet = false;

            if (Transaction == null)
            {
                return bRet;
            }

            try
            {
                if (bCommit)
                {
                    Transaction.Commit();
                }
                else
                {
                    Transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            Transaction.Dispose();
            Transaction = null;
            bRet = true;

            return bRet;
        }

        #endregion
    }
}
