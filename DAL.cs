using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace PhoneBook
{
    public class DAL
    {
        private static SqlConnection _Connection;

        private static SqlConnection CreateConn()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return new SqlConnection(configuration.GetConnectionString("SQLConnection"));
        }

        private static DataSet ExecuteSQL(string SQL, Hashtable Parameters)
        {
            DataSet ds = new DataSet();

            using (_Connection = CreateConn())
            {
                _Connection.Open();

                using (SqlCommand _Command = new SqlCommand(SQL, _Connection))
                {
                    _Command.CommandType = CommandType.Text;

                    foreach (DictionaryEntry de in Parameters)
                    {
                        _Command.Parameters.Add(new SqlParameter(de.Key.ToString(), de.Value));
                    }

                    using (SqlDataAdapter _DataAdapter = new SqlDataAdapter(_Command))
                    {
                        _DataAdapter.Fill(ds);
                    }
                }

                _Connection.Close();
            }

            return ds;
        }

        public static DataSet ExecuteSP(string StoredProcedureName)
        {
            return ExecuteSP(StoredProcedureName, new Hashtable());
        }

        public static DataSet ExecuteSP(string StoredProcedureName, Hashtable Parameters)
        {
            DataSet ds = new DataSet();

            using (_Connection = CreateConn())
            {
                _Connection.Open();

                using (SqlCommand _Command = new SqlCommand(StoredProcedureName, _Connection))
                {
                    _Command.CommandType = CommandType.StoredProcedure;

                    foreach (DictionaryEntry de in Parameters)
                    {
                        _Command.Parameters.Add(new SqlParameter(de.Key.ToString(), de.Value));
                    }

                    using (SqlDataAdapter _DataAdapter = new SqlDataAdapter(_Command))
                    {
                        _DataAdapter.Fill(ds);
                    }
                }

                _Connection.Close();
            }
            return ds;
        }

        public static object ExecuteScalarSP(string StoredProcedureName, Hashtable Parameters)
        {
            object objResult;

            using (_Connection = CreateConn())
            {
                _Connection.Open();

                using (SqlCommand _Command = new SqlCommand(StoredProcedureName, _Connection))
                {
                    _Command.CommandType = CommandType.StoredProcedure;

                    foreach (DictionaryEntry de in Parameters)
                    {
                        _Command.Parameters.Add(new SqlParameter(de.Key.ToString(), de.Value));
                    }

                    objResult = _Command.ExecuteScalar();
                }

                _Connection.Close();
            }
            return objResult;
        }

        public static string GetEnviromentKey()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();
            var enviromentEndPoint = configuration.GetSection("EnvironmentEndPoint");

            return enviromentEndPoint.Value.ToString();
        }
    }
}
