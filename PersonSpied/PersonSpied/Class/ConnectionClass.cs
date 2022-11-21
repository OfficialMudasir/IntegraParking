using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;  

namespace PersonSpied.Class
{
    public class ConnectionClass
    {
        #region Properties
        private static SqlConnection connection { get; set; }
        #endregion

        #region Constructor
        public ConnectionClass()
        {
            Initialize();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            String connectionString=SettingsClass.ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        private static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        #endregion

        #region Public Methods
        public static Boolean Insert(String query)
        {
            Boolean bSave = false;
            //Example:
            //query = "INSERT INTO tableuser (name, age) VALUES('Mariu', '42')";
            if (OpenConnection() == true)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    bSave = true;
                    CloseConnection();
                }
                catch (Exception ex)
                {
                    bSave = false;
                }
            }

            return bSave;
        }

        public static Boolean Update(String query)
        {
            Boolean bSave = false;
            //Example:
            //query = "UPDATE tableuser SET name='mariu2', age='40' WHERE name='Mariu'";
            if (OpenConnection() == true)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    bSave = true;
                    CloseConnection();
                }
                catch(Exception ex)
                {
                    bSave = false;
                }
            }
            return bSave;
        }

        public static void Delete(String query)
        {
            //Example:
            //query = "DELETE FROM tableuser WHERE name='mariu2'";
            if (OpenConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public static List<T> Select<T>(String query)
        {
            T oObjeto = default(T);
            List<T> oLista = new List<T>();

            if (OpenConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    oObjeto = (T)Activator.CreateInstance(typeof(T));
                    oObjeto = RecorrerDataReader<T>(oObjeto, dataReader);
                    oLista.Add(oObjeto);
                }
                dataReader.Close();
                CloseConnection();
                return oLista;
            }
            else
            {
                return oLista;
            }
        }

        public static Int32 Count(String query)
        {
            //Example:
            //query = "SELECT Count(*) FROM tableuser";
            int Count = -1;
            if (OpenConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                Count = int.Parse(cmd.ExecuteScalar()+"");
                CloseConnection();
                return Count;
            }
            else
            {
                return Count;
            }
        }

        #endregion

        #region Métodos Publicos con Procedimientos Almacenados y parametros
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedimientoAlmacenado"></param>
        /// <param name="parametros">Formato de JSON</param>
        /// <returns></returns>
        public static T EjecutarObjeto<T>(string procedimientoAlmacenado, object[] parametros)
        {
            return Conectar<T>(procedimientoAlmacenado, parametros);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedimientoAlmacenado"></param>
        /// <param name="parametros">Formato de JSON</param>
        /// <returns></returns>
        public static List<T> ObtenerLista<T>(string procedimientoAlmacenado, object[] parametros)
        {
            List<T> oLista = ConectarLista<T>(procedimientoAlmacenado, parametros);
            return oLista;
        }
        #endregion

        #region Métodos Privados con Procedimientos Almacenados y parametros
        private static T Conectar<T>(string procedimientoAlmacenado, object[] parametros)
        {
            T oObjeto = default(T);
            if (OpenConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(procedimientoAlmacenado, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd = RecorrerParametros(cmd, parametros);
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    oObjeto = (T)Activator.CreateInstance(typeof(T));
                    oObjeto = RecorrerDataReader<T>(oObjeto, dataReader);
                }
                dataReader.Close();
                CloseConnection();
             }
            return oObjeto;
        }

        private static List<T> ConectarLista<T>(string procedimientoAlmacenado, object[] parametros)
        {
            List<T> oLista = new List<T>();
            if (OpenConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(procedimientoAlmacenado, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd = RecorrerParametros(cmd, parametros);
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    T oObjeto = (T)Activator.CreateInstance(typeof(T));
                    oObjeto = RecorrerDataReader<T>(oObjeto, dataReader);
                    oLista.Add(oObjeto);
                }
                dataReader.Close();
                CloseConnection();
             }
            return oLista;
        }

        private static T RecorrerDataReader<T>(T oObjeto, SqlDataReader rdr)
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (rdr[i] != DBNull.Value)
                {
                    String sPropertyName = rdr.GetName(i);
                    PropertyInfo oPropertyInfo = oObjeto.GetType().GetProperty(sPropertyName);
                    oPropertyInfo.SetValue(oObjeto, rdr[i], null);
                }
            }
            return oObjeto;
        }

        private static SqlCommand RecorrerParametros(SqlCommand cmd, object[] parametros)
        {
            for (int i = 0; i < parametros.Count(); )
            {
                string sNombreParametro = Convert.ToString(parametros[i]);
                i++;
                object oValorParametro = parametros[i];
                i++;
                cmd.Parameters.AddWithValue(sNombreParametro, oValorParametro);
            }
            return cmd;
        }
        #endregion
    }
}
