using System;
using System.Data.SqlClient;

namespace Database
{
    public class DBContext
    {
        public string DataSource { get; protected set; }
        public string UserId { get; protected set; }
        public string Password { get; protected set; }
        public string Database { get; protected set; }

        private SqlConnectionStringBuilder builder;

        public DBContext(string dataSource, string userId, string password, string database)
        {
            DataSource = dataSource;
            UserId = userId;
            Password = password;
            Database = database;
            builder = new SqlConnectionStringBuilder();
        }

        public SqlConnectionStringBuilder GetBuilder()
        {
            builder.DataSource = DataSource;
            builder.UserID = UserId;
            builder.Password = Password;
            builder.InitialCatalog = Database;
            return builder;
        }

        public string ConnectionString => GetBuilder().ConnectionString;
    }
}
