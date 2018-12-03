using System.Collections.Generic;

namespace DataCore
{
    public interface ISqlBuilder
    {
        string Insert(string table, IList<string> columns);
        string Update(string table, IList<string> columns, IList<string> keys);
        string Delete(string table, IList<string> keys);
        string Search(string table, IList<string> columns = null, IList<string> keys = null);
    }
}