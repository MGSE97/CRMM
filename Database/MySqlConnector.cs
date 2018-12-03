using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Database
{
    public class MySqlConnector : IDisposable
    {
        protected MySqlConnection Connection { get; set; }
        protected DBContext Context { get; set; }

        public MySqlConnector(DBContext context)
        {
            Context = context;
            Connection = new MySqlConnection(Context.ConnectionString);
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public MySqlDataReader Execute<TModel>(string command, TModel model = default(TModel)) where  TModel: new()
        {
            return Execute(command, model.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(model)));
        }

        public MySqlDataReader Execute(string command, Dictionary<string, SqlDbType> @params = null)
        {
            if(Connection.State == ConnectionState.Closed)
                Connection.Open();
            MySqlDataReader reader = null;

            var cmd = new MySqlCommand(command, Connection);
            
            if(@params != null)
                foreach (var param in @params)
                {
                    cmd.Parameters.AddWithValue($"@{param.Key}", param.Value);
                }
            reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            

            return reader;
        }
    }
}