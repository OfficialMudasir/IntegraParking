using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using Devart.Data.PostgreSql;

namespace DBBench.Postgres
{
    public class NHSessionManager
    {
        //private static ISessionFactory _sessionFactory = null;
        private static readonly Dictionary<string, ISessionFactory> SessionFactories = new Dictionary<string, ISessionFactory>();

        /*public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory != null)
                {
                    return _sessionFactory;
                }

                //_sessionFactory = ConfigurationHelper.CreateConfiguration().BuildSessionFactory();
                Configuration oConfiguration = ConfigurationHelper.CreateConfiguration();
                _sessionFactory = oConfiguration.BuildSessionFactory();

                return _sessionFactory;
            }            
        }*/

        public static ISessionFactory SessionFactory()
        {
            return SessionFactory(null);
        }
        public static ISessionFactory SessionFactory(ConnectionConfiguration oConnectionConfig)
        {
            string sConnectionString = "";

            if (oConnectionConfig != null)
                sConnectionString = oConnectionConfig.ConnectionString;

            lock (SessionFactories)
            {
                if (!SessionFactories.ContainsKey(sConnectionString) || SessionFactories[sConnectionString] == null)
                {
                    //_sessionFactory = ConfigurationHelper.CreateConfiguration().BuildSessionFactory();
                    Configuration oConfiguration = ConfigurationHelper.CreateConfiguration();

                    if (oConnectionConfig != null)
                    {
                        oConfiguration = oConfiguration.SetProperty("connection.connection_string", oConnectionConfig.ConnectionString);
                        oConfiguration = oConfiguration.SetProperty("dialect", oConnectionConfig.Dialect);
                        oConfiguration = oConfiguration.SetProperty("connection.driver_class", oConnectionConfig.DriverClass);
                        //oConfiguration = oConfiguration.SetProperty("query.substitutions", oConnectionConfig.QuerySubstitutions);
                    }

                    if (!SessionFactories.ContainsKey(sConnectionString))
                        SessionFactories.Add(sConnectionString, oConfiguration.BuildSessionFactory());
                    else
                        SessionFactories[sConnectionString] = oConfiguration.BuildSessionFactory();
                }

                return SessionFactories[sConnectionString];
            }
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
            OpenSession(null);
        }
        public static void OpenSession(ConnectionConfiguration oConnectionConfig)
        {
            var session = SessionFactory(oConnectionConfig).OpenSession();
            CurrentSessionContext.Bind(session);
            //session.BeginTransaction();            
        }

        public static void CloseSession()
        {
            CloseSession(null);
        }
        public static void CloseSession(ConnectionConfiguration oConnectionConfig)
        {
            ISessionFactory oSessionFactory = SessionFactory(oConnectionConfig);
            var session = oSessionFactory.GetCurrentSession();
            var transaction = session.GetCurrentTransaction();

            if (transaction != null && transaction.IsActive)
                transaction.Commit();

            session = CurrentSessionContext.Unbind(oSessionFactory);
            session.Close();
        }

        public static void ResetSessionFactory()
        {
            try
            {
                CloseSession();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //_sessionFactory = null;
        }

        public static ISession CurrentSession()
        {
            return SessionFactory().GetCurrentSession();
        }
        public static ISession CurrentSession(ConnectionConfiguration oConnectionConfig)
        {
            return SessionFactory(oConnectionConfig).GetCurrentSession();
        }

        #region ConnectionConfiguration

        public class ConnectionConfiguration
        {
            public string ConnectionString { get; set; }
            public string Dialect { get; set; }
            public string DriverClass { get; set; }
            public string QuerySubstitutions { get; set; }
        }

        #endregion
    }
}
