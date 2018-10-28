using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Database;

namespace Services.Database.Connection
{
    public class ConnectionService : IConnectionService
    {
        private SqlConnector _sqlConnector;

        public ConnectionService(SqlConnector sqlConnector)
        {
            _sqlConnector = sqlConnector;
        }

        public SqlDataReader Execute(string sql, Dictionary<string, SqlDbType> @params = null)
        {
            return _sqlConnector.Execute(sql, @params);
        }
    }
}