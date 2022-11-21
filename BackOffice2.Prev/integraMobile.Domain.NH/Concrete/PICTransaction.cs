using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Transactions;
using System.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace integraMobile.Domain.NH.Concrete
{
    class PICTransaction : PIC.Domain.Abstract.IPICTransaction 
    {
        private ISession _session = null;
        private ITransaction _transaction = null;

        public ISession Session
        {
            get { return _session; }
        }

        public ITransaction Transaction
        {
            get { return _transaction; }
        }

        public PICTransaction()
        {
            _session = NHSessionManager.SessionFactory().OpenSession();
        }
        /*public PICTransaction(ISessionFactory oSessionFact)
        {
            _session = oSessionFact.OpenSession();
        }*/
        public PICTransaction(ISession oSession)
        {
            _session = oSession;
        }
        public PICTransaction(ISession oSession, ITransaction oTransaction)
        {
            _session = oSession;
            _transaction = oTransaction;
        }
        ~PICTransaction()
        {
            FinishTransaction(false);
            if (_session != null)
            {
                if (_session.IsOpen)
                {
                    try
                    {
                        _session.Clear();
                        _session.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                _session.Dispose();
                _session = null;
            }
        }

        public bool BeginTransaction()
        {
            bool bRet = false;
            if (_session != null && _transaction == null)
            {
                _transaction = _session.BeginTransaction();
                bRet = true;
            }
            return bRet;
        }

        public bool BeginTransaction(IsolationLevel isolationLevel)
        {
            bool bRet = false;
            if (_session != null && _transaction == null)
            {
                _transaction = _session.BeginTransaction(isolationLevel);
                bRet = true;
            }
            return bRet;
        }

        public bool FinishTransaction(bool bCommit)
        {
            bool bRet = false;
            if (_transaction != null)
            {
                try
                {
                    if (bCommit)
                        _transaction.Commit();
                    else
                        _transaction.Rollback();
                }
                catch (Exception ex) { }
                _transaction.Dispose();
                _transaction = null;
                bRet = true;
            }
            return bRet;
        }
    }
}
