using System.Collections.Generic;
using System.Data;
using Data.Mapping;
using DataCore;
using MySql.Data.MySqlClient;

namespace MySql
{
    public class MySqlConnector : BaseConnector
    {
        public MySqlConnector(string connectionString) : base(connectionString, MySqlBuilder.GetInstance())
        {
        }

        protected override IList<TValue> ExecuteCommand<TValue>(CommandMode mode, string commandText, IDictionary<string, object> @params, CommandType type = CommandType.Text)
        {
            List<TValue> result = new List<TValue>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var command = new MySqlCommand(commandText, connection) {CommandType = type})
                {
                    if (@params != null)
                        foreach (var param in @params)
                            command.Parameters.AddWithValue($"@p_{param.Key}", param.Value);

                    connection.Open();
                    switch (mode)
                    {
                        case CommandMode.Reader:
                            result.AddRange(command.ExecuteReader().MapAll<TValue>(accessProtection: AccessProtection.Private));
                            break;
                        case CommandMode.Scalar:
                            result.Add((TValue) command.ExecuteScalar());
                            break;
                        case CommandMode.NonQuery:
                            result.Add((TValue) (object) command.ExecuteNonQuery());
                            break;
                    }
                }
            }

            return result;
        }
    }
}
