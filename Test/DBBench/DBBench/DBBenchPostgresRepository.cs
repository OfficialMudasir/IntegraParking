using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using System.Globalization;
using System.Threading;
using System.Net;
using DBBench.Tools;
using EnterpriseDB.EDBClient;
using NHibernate.Hql.Ast;
using NHibernate;
using System.Configuration;


namespace DBBench.Postgres
{
    public class DBBenchPostgresRepository : PostgresBaseRepository, IDBBenchPostgresRepository
    {
        #region Fields

        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(DBBenchPostgresRepository));


        private PostgresConnectionHelper dbConnection = null;
        private static Random oRand = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

        #endregion

        #region Private Static

        //private static List<Parkingrights> _rights;

        #endregion

        #region Constructor

        public DBBenchPostgresRepository(bool bOpenSession)
            : base(typeof(DBBenchPostgresRepository), null, bOpenSession)
        {
            m_bStateless = false;            

        }

        public DBBenchPostgresRepository(NHSessionManager.ConnectionConfiguration oConnectionConfig, bool bOpenSession)
            : base(typeof(DBBenchPostgresRepository), oConnectionConfig, bOpenSession)
        {
            m_bStateless = false;

        }

        #endregion


        public bool InsertTestTable(PgTestTable o)
        {
            bool bRes = false;

            try
            {
                                     
                if (!m_bStateless)
                {
                    if (m_session == null)
                    {
                        m_session = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenSession();
                    }
                    if (dbConnection == null) dbConnection = new PostgresConnectionHelper(m_session);
                }
                else
                {
                    if (m_statelessSession == null)
                    {
                        m_statelessSession = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenStatelessSession();
                    }

                    if (dbConnection == null) dbConnection = new PostgresConnectionHelper(m_statelessSession);
                }




                using (var connection = dbConnection)
                {
                    try
                    {
                        connection.BeginConnection(true);
                        if (m_bStateless)
                        {
                            connection.StatelessSession.Insert(o);
                        }
                        else
                        {
                            var o2 = connection.Session.Save(o);
                            Logger_AddLogMessage(string.Format("Postgres InsertTestTable: {1},{0}", o.PgStringField, ((int)o2).ToString()), LogLevels.logINFO);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "Postgres InsertTestTable: ", LogLevels.logERROR);

                        m_session = null;
                        dbConnection = null;
                    }
                    finally
                    {
                        connection.EndConnection();
                    }

                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "Postgres InsertTestTable: ", LogLevels.logERROR);
                m_session = null;
                dbConnection = null;
            }
            finally
            {
            }

            return bRes;
        }



        public bool Select_Update_TestTable(int iIter, string strString)
        {
            bool bRes = false;

            try
            {

                if (!m_bStateless)
                {
                    if (m_session == null)
                    {
                        m_session = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenSession();
                    }
                    if (dbConnection == null) dbConnection = new PostgresConnectionHelper(m_session);
                }
                else
                {
                    if (m_statelessSession == null)
                    {
                        m_statelessSession = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenStatelessSession();
                    }

                    if (dbConnection == null) dbConnection = new PostgresConnectionHelper(m_statelessSession);
                }




                using (var connection = dbConnection)
                {
                    try
                    {
                        connection.BeginConnection(true);
                        if (m_bStateless)
                        {                          
                        }
                        else
                        {
                            int iRand = oRand.Next(1, 100);

                            var oRow =
                                connection.Session.Query<PgTestTable>()
                                    .Where(r => r.PgId % (iRand * (iIter+1)) == 0).FirstOrDefault();

                            if (oRow != null)
                            {

                                oRow.PgStringField = strString;
                                connection.Session.Save(oRow);
                                Logger_AddLogMessage(string.Format("Postgres Select_Update_TestTable: {1},{0}", oRow.PgStringField, oRow.PgId), LogLevels.logINFO);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("Postgres Select_Update_TestTable: Registry not found"), LogLevels.logINFO);

                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "Postgres Select_Update_TestTable: ", LogLevels.logERROR);

                        m_session = null;
                        dbConnection = null;
                    }
                    finally
                    {
                        connection.EndConnection();
                    }

                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "Postgres Select_Update_TestTable: ", LogLevels.logERROR);
                m_session = null;
                dbConnection = null;
            }
            finally
            {
            }

            return bRes;
        }




        public bool InsertTestTableADO(string strString)
        {
            bool bRes = false;

            try
            {
                

                EDBConnection conn = new EDBConnection(ConfigurationManager.ConnectionStrings["pgConnectionString"].ConnectionString);

                conn.Open();

                try
                {
                    var transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                    /*EDBCommand EDBSeletCommand = new EDBCommand("SELECT nextval('test.test_table_id_seq')", conn);
                    var oSeq = EDBSeletCommand.ExecuteScalar();
                    int iSeq = Convert.ToInt32(oSeq);

                    EDBCommand EDBInsertCommand = new EDBCommand(string.Format("INSERT INTO test.test_table(id, string_field) VALUES({1}, '{0}') returning ID", strString, iSeq), conn);*/
                    EDBCommand EDBInsertCommand = new EDBCommand(string.Format("INSERT INTO test.test_table(id, string_field) VALUES((SELECT nextval('test.test_table_id_seq')), '{0}') returning ID", strString), conn);
                    EDBInsertCommand.Transaction = transaction;
                    var oRes = EDBInsertCommand.ExecuteScalar();
                    Logger_AddLogMessage(string.Format("Postgres InsertTestTableADO: {1},{0}", strString, oRes.ToString()), LogLevels.logINFO);
                    transaction.Commit();

                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "Postgres InsertTestTableADO: ", LogLevels.logERROR);

                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "Postgres InsertTestTableADO: ", LogLevels.logERROR);
            }
            finally
            {
               
            }

            return bRes;





        }





        public void EndConnection()
        {

            try
            {

                if (dbConnection != null)
                {
                    dbConnection.EndConnection(false);
                }
            }
            catch { }
            finally
            {
                dbConnection = null;
                m_session = null;

            }
        }      
    }
}
