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
using NHibernate.Hql.Ast;
using NHibernate;
using System.Configuration;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;



namespace DBBench.Oracle
{
    public class DBBenchOracleRepository : OracleBaseRepository, IDBBenchOracleRepository
    {
        #region Fields

        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(DBBenchOracleRepository));


        private OracleConnectionHelper dbConnection = null;
        private static Random oRand = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

        #endregion

        #region Private Static

        //private static List<Parkingrights> _rights;

        #endregion

        #region Constructor

        public DBBenchOracleRepository(bool bOpenSession)
            : base(typeof(DBBenchOracleRepository), null, bOpenSession)
        {
            m_bStateless = false;

        }

        public DBBenchOracleRepository(NHSessionManager.ConnectionConfiguration oConnectionConfig, bool bOpenSession)
            : base(typeof(DBBenchOracleRepository), oConnectionConfig, bOpenSession)
        {
            m_bStateless = false;

        }

        #endregion


        public bool InsertTestTable(OraTestTable o)
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
                    if (dbConnection == null) dbConnection = new OracleConnectionHelper(m_session);
                }
                else
                {
                    if (m_statelessSession == null)
                    {
                        m_statelessSession = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenStatelessSession();
                    }

                    if (dbConnection == null) dbConnection = new OracleConnectionHelper(m_statelessSession);
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
                            Logger_AddLogMessage(string.Format("Oracle InsertTestTable: {1},{0}", o.OraStringField, ((decimal)o2).ToString()), LogLevels.logINFO);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "Oracle InsertTestTable: ", LogLevels.logERROR);

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
                Logger_AddLogException(e, "Oracle InsertTestTable: ", LogLevels.logERROR);
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
                    if (dbConnection == null) dbConnection = new OracleConnectionHelper(m_session);
                }
                else
                {
                    if (m_statelessSession == null)
                    {
                        m_statelessSession = NHSessionManager.SessionFactory(m_oConnectionConfig).OpenStatelessSession();
                    }

                    if (dbConnection == null) dbConnection = new OracleConnectionHelper(m_statelessSession);
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
                                connection.Session.Query<OraTestTable>()
                                    .Where(r => r.OraId % (iRand * (iIter+1)) == 0).FirstOrDefault();

                            if (oRow != null)
                            {

                                oRow.OraStringField = strString;
                                connection.Session.Save(oRow);
                                Logger_AddLogMessage(string.Format("Oracle Select_Update_TestTable: {1},{0}", oRow.OraStringField, oRow.OraId), LogLevels.logINFO);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("Oracle Select_Update_TestTable: Registry not found"), LogLevels.logINFO);

                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "Oracle Select_Update_TestTable: ", LogLevels.logERROR);

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
                Logger_AddLogException(e, "Oracle Select_Update_TestTable: ", LogLevels.logERROR);
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


                OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["oraConnectionString"].ConnectionString);

                conn.Open();

                try
                {
                    var transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    OracleCommand OracleInsertCommand = new OracleCommand();

                    // INSERT statement with RETURNING clause to get the generated ID 
                    OracleInsertCommand.CommandText = String.Format("INSERT INTO test_table(string_field) VALUES('{0}') returning ID INTO :id", strString);
                    OracleInsertCommand.Connection = conn;
                    OracleInsertCommand.Transaction = transaction;

                    OracleInsertCommand.Parameters.Add(new OracleParameter
                    {
                        ParameterName = ":id",
                        DbType = System.Data.DbType.Int32,
                        Direction = System.Data.ParameterDirection.Output
                    });

                    // Execute INSERT statement
                    OracleInsertCommand.ExecuteNonQuery();

                    transaction.Commit();
                   
                    Logger_AddLogMessage(string.Format("Oracle InsertTestTableADO: {1},{0}", strString, OracleInsertCommand.Parameters[":id"].Value.ToString()), LogLevels.logINFO);

                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "Oracle InsertTestTableADO: ", LogLevels.logERROR);

                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "Oracle InsertTestTableADO: ", LogLevels.logERROR);
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
