using System;
using System.Collections.Generic;
using System.Text;

namespace DataCore
{
    public interface IConnector
    {
        ISqlBuilder SqlBuilder { get; }

        IEnumerable<TModel> ExecuteSqlReader<TModel>(string sql, IDictionary<string, object> @params) where TModel : new();
        TValue ExecuteSqlScalar<TValue>(string sql, IDictionary<string, object> @params) where TValue : new();
        int ExecuteSql(string sql, IDictionary<string, object> @params);

        IEnumerable<TModel> ExecuteFunctionReader<TModel>(string name, IDictionary<string, object> @params) where TModel : new();
        TValue ExecuteFunctionScalar<TValue>(string name, IDictionary<string, object> @params) where TValue : new();
        int ExecuteFunction(string name, IDictionary<string, object> @params);
    }
}
