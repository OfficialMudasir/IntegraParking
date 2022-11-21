using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;
using NHibernate.Cfg;
using integraMobile.Domain.NH.Entities;

namespace integraMobile.Domain.NH.Concrete
{

    public class NHSessionManager
    {
        //private static ISessionFactory _sessionFactory = null;
        private static Dictionary<string, ISessionFactory> _sessionFactories = new Dictionary<string, ISessionFactory>();

        /*public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    //_sessionFactory = ConfigurationHelper.CreateConfiguration().BuildSessionFactory();
                    Configuration oConfiguration = ConfigurationHelper.CreateConfiguration();
                    oConfiguration = oConfiguration.SetProperty("connection.connection_string", System.Configuration.ConfigurationManager.AppSettings["integraMobile.Domain.NH.Connection_string"].ToString());
                    _sessionFactory = oConfiguration.BuildSessionFactory();                    
                    
                }
                return _sessionFactory;
            }            
        }*/

        public static ISessionFactory SessionFactory()
        {
            return SessionFactory(System.Configuration.ConfigurationManager.AppSettings["integraMobile.Domain.NH.Connection_string"].ToString());
        }
        public static ISessionFactory SessionFactory(string sConnectionString)        
        {
            if (string.IsNullOrEmpty(sConnectionString)) sConnectionString = System.Configuration.ConfigurationManager.AppSettings["integraMobile.Domain.NH.Connection_string"].ToString();

            if (!_sessionFactories.ContainsKey(sConnectionString) || _sessionFactories[sConnectionString] == null)            
            {
                //_sessionFactory = ConfigurationHelper.CreateConfiguration().BuildSessionFactory();
                Configuration oConfiguration = ConfigurationHelper.CreateConfiguration();
                oConfiguration = oConfiguration.SetProperty("connection.connection_string", sConnectionString);
                if (!_sessionFactories.ContainsKey(sConnectionString))
                    _sessionFactories.Add(sConnectionString, oConfiguration.BuildSessionFactory());
                else
                    _sessionFactories[sConnectionString] = oConfiguration.BuildSessionFactory();                

            }
            return _sessionFactories[sConnectionString];
        }

        /*public static void InitializeSessionFactory()
        {
            if (SessionFactory == null)
            {
                SessionFactory = ConfigurationHelper.CreateConfiguration().BuildSessionFactory();
            }
        }*/

        public static void OpenSession()
        {
            var session = SessionFactory().OpenSession();
            CurrentSessionContext.Bind(session);
            //session.BeginTransaction();
        }

        public static void CloseSession()
        {
            var session = SessionFactory().GetCurrentSession();
            var transaction = session.Transaction;
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
            session = CurrentSessionContext.Unbind(SessionFactory());
            session.Close();
        }

        public static ISession CurrentSession()
        {
            return SessionFactory().GetCurrentSession();
        }
        public static ISession CurrentSession(string sConnectionString)
        {
            return SessionFactory(sConnectionString).GetCurrentSession();
        }


    }

}
