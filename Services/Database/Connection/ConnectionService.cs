using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Database;

namespace Services.Database.Connection
{
    public class ConnectionService : IConnectionService
    {
        private Connector _connector;

        public ConnectionService(Connector connector)
        {
            _connector = connector;
        }

        public SqlDataReader Execute(string sql, Dictionary<string, SqlDbType> @params)
        {
            return _connector.Execute(sql, @params);
        }
    }
}