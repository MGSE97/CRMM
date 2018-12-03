using System;

namespace Data.Mapping.Attributes
{
    /// <summary>
    /// Sets table of model
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : Attribute
    {
        public string Table { get; }

        public TableAttribute(string table)
        {
            Table = table;
        }
    }
}