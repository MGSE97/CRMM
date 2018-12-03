﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Database
{
    public class SqlConnector : IDisposable
    {
        protected SqlConnection Connection { get; set; }
        protected DBContext Context { get; set; }

        public SqlConnector(DBContext context)
        {
            Context = context;
            Connection = new SqlConnection(Context.ConnectionString);
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public SqlDataReader Execute<TModel>(string command, TModel model = default(TModel)) where  TModel: new()
        {
            return Execute(command, model.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(model)));
        }

        public SqlDataReader Execute(string command, Dictionary<string, SqlDbType> @params = null)
        {
            if(Connection.State == ConnectionState.Closed)
                Connection.Open();
            SqlDataReader reader = null;

            var cmd = new SqlCommand(command, Connection);

            if(@params != null)
                foreach (var param in @params)
                {
                    cmd.Parameters.Add($"@{param.Key}", param.Value);
                }
            reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            
            return reader;
        }
    }
}