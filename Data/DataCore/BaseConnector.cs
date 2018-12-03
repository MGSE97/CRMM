using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataCore
{
    public abstract class BaseConnector : IConnector
    {
        public ISqlBuilder SqlBuilder { get; protected set; }

        protected string ConnectionString { get; set; }

        public BaseConnector(string connectionString, ISqlBuilder sqlBuilder)
        {
            ConnectionString = connectionString;
            SqlBuilder = sqlBuilder;
        }


        public IEnumerable<TModel> ExecuteSqlReader<TModel>(string sql, IDictionary<string, object> @params) where TModel : new()
        {
            return ExecuteCommand<TModel>(CommandMode.Reader, sql, @params);
        }

        public TValue ExecuteSqlScalar<TValue>(string sql, IDictionary<string, object> @params) where TValue : new()
        {
            return ExecuteCommand<TValue>(CommandMode.Scalar, sql, @params).FirstOrDefault();
        }

        public int ExecuteSql(string sql, IDictionary<string, object> @params)
        {
            return ExecuteCommand<int>(CommandMode.NonQuery, sql, @params).FirstOrDefault();
        }

        public IEnumerable<TModel> ExecuteFunctionReader<TModel>(string name, IDictionary<string, object> @params) where TModel : new()
        {
            return ExecuteCommand<TModel>(CommandMode.Reader, name, @params, CommandType.StoredProcedure);
        }

        public TValue ExecuteFunctionScalar<TValue>(string name, IDictionary<string, object> @params) where TValue : new()
        {
            return ExecuteCommand<TValue>(CommandMode.Scalar, name, @params, CommandType.StoredProcedure).FirstOrDefault();
        }

        public int ExecuteFunction(string name, IDictionary<string, object> @params)
        {
            return ExecuteCommand<int>(CommandMode.NonQuery, name, @params, CommandType.StoredProcedure).FirstOrDefault();
        }

        protected abstract IList<TValue> ExecuteCommand<TValue>(CommandMode mode, string commandText, IDictionary<string, object> @params, CommandType type = CommandType.Text) where TValue : new();
    }
}