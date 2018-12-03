using System;
using System.Linq;
using System.Reflection;
using Data.Mapping.Attributes;

namespace Data.Mapping.Extensions
{
    public static class TableAttributeExtensions
    {
        public static string GetTableName<T>(this T obj)
        {
            return GetTableName(obj.GetType());
        }

        public static string GetTableName(this Type type)
        {
            return type.GetCustomAttributes<TableAttribute>()?.FirstOrDefault()?.Table ?? null;
        }
    }
}