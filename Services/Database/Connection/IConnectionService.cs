using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Services.Database.Connection
{
    public interface IConnectionService
    {
        SqlDataReader Execute(string sql, Dictionary<string, SqlDbType> @params);
    }
}