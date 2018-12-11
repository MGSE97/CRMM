using System.Collections.Generic;
using System.Linq;
using DataCore;

namespace MySql
{
    public class MySqlBuilder : ISqlBuilder
    {
        #region Singleton

        private static MySqlBuilder _instance;

        private MySqlBuilder()
        {

        }

        public static MySqlBuilder GetInstance()
        {
            if (_instance == null)
                _instance = new MySqlBuilder();
            return _instance;
        }

        #endregion

        public string Insert(string table, IList<string> columns)
        {
            return $"INSERT INTO `{table}` ({string.Join(",", columns)}) VALUES ({string.Join(",", columns.Select(c => $"@p_{c}"))}); SELECT LAST_INSERT_ID();";
        }

        public string Update(string table, IList<string> columns, IList<string> keys)
        {
            return $"UPDATE `{table}` SET {string.Join(",", columns.Select(c => $"{c} = @p_{c}"))} WHERE {string.Join(" AND ", keys.Select(k => $"{k} = @p_{k}"))}";
        }

        public string Delete(string table, IList<string> keys)
        {
            return $"DELETE FROM `{table}` WHERE {string.Join(" AND ", keys.Select(k => $"{k} = @p_{k}"))}";
        }

        public string Search(string table, IList<string> columns, IList<string> keys = null)
        {
            if (keys != null && keys.Count > 0)
                return $"SELECT {string.Join(",", columns)} FROM `{table}` WHERE {string.Join(" AND ", keys.Select(k => $"{k} = @p_{k}"))}";
            
            return $"SELECT {string.Join(",", columns)} FROM `{table}`";
        }
    }
}