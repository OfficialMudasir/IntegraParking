using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIC.Domain.Abstract;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace integraMobile.Domain.NH.Concrete
{
    public class SQLConnectionHelper
    {
        private ISession _session = null;
        private ITransaction _transaction = null;

        private bool _inSession = false;
        private bool _inTransaction = false;
        private bool _newSession = false;

        #region Properties

        public ISession Session
        {
            get { return _session; }
        }
        public ITransaction Transaction
        {
            get { return _transaction; }
        }

        #endregion

        #region Constructor

        public SQLConnectionHelper(ISession oSession, IPICTransaction trans = null)
        {
            if (trans != null)
            {
                PICTransaction oPICTransaction = null;
                if (trans != null) oPICTransaction = (PICTransaction)trans;
                this._session = (oPICTransaction != null ? oPICTransaction.Session : null);
                this._transaction = (oPICTransaction != null ? oPICTransaction.Transaction : null);
            }
            else
            {
                this._session = oSession;
                this._transaction = (oSession != null ? oSession.Transaction : null);
            }
            this._inSession = (this._session != null);
            this._inTransaction = (this._transaction != null && this._transaction.IsActive);

        }

        ~SQLConnectionHelper()
        {
            if (this._newSession && this._session != null)
            {
                if (this._session.IsOpen) this._session.Close();
                this._session.Dispose();
                this._session = null;
            }
        }

        #endregion

        #region Methods

        public void BeginConnection(string sConnectionString = "", bool bWithTransaction = true, System.Data.IsolationLevel? oIsolationLevel = null)
        {
            if (!this._inSession)
            {
                try
                {
                    this._session = NHSessionManager.CurrentSession(sConnectionString);
                }
                catch (Exception ex)
                {
                    //NHSessionManager.OpenSession();
                    //this._session = NHSessionManager.CurrentSession();
                    this._session = NHSessionManager.SessionFactory(sConnectionString).OpenSession();
                    this._newSession = true;
                }
                //if (_globalSession == null) _globalSession = this._sessionFact.OpenSession();
                //this._session = _globalSession;
            }
            if (!this._inTransaction && bWithTransaction)
            {
                if (oIsolationLevel.HasValue)
                    this._transaction = this._session.BeginTransaction(oIsolationLevel.Value);
                else
                    this._transaction = this._session.BeginTransaction(/*System.Data.IsolationLevel.ReadUncommitted*/);
            }
        }

        public void EndConnection(bool bCommit = true)
        {
            if (!this._inTransaction && this._transaction != null)
            {
                if (this._transaction.IsActive && this._session.IsOpen)
                {
                    try
                    {
                        if (bCommit)
                            this._transaction.Commit();
                        else
                            this._transaction.Rollback();
                    }
                    catch (Exception ex) { }
                }
                if (this._transaction != null) this._transaction.Dispose();
                this._transaction = null;
            }
            if (this._newSession && this._session != null)
            {
                if (this._session.IsOpen) this._session.Close();
                this._session.Dispose();
                this._session = null;
            }
        }

        public IPICTransaction GetPICTransaction()
        {
            return new PICTransaction(this._session, this._transaction);
        }

        #endregion

    }
}
