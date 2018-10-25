using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Database
{
    public class Connector : IDisposable
    {
        protected SqlConnection Connection { get; set; }
        protected DBContext Context { get; set; }

        public Connector(DBContext context)
        {
            Context = context;
            Connection = new SqlConnection(Context.ConnectionString);
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public SqlDataReader Execute(string command, Dictionary<string, SqlDbType> @params)
        {
            if(Connection.State == ConnectionState.Closed)
                Connection.Open();
            SqlDataReader reader = null;

            using (var cmd = new SqlCommand(command, Connection))
            {
                foreach (var param in @params)
                {
                    cmd.Parameters.Add(param.Key, param.Value);
                }
                reader = cmd.ExecuteReader();
            }

            return reader;
        }
    }
}