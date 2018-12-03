using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Data.Mapping.Attributes;

namespace Data.Mapping.Extensions
{
    public static class ReadOnlyAttributeExtensions
    {
        public static bool IsReadOnly(this PropertyInfo property)
        {
            return property.GetCustomAttributes<ReadOnlyAttribute>()?.Any() ?? false;
        }

        public static bool IsReadOnly<T>(this T obj)
        {
            return IsReadOnly(obj.GetType());
        }

        public static bool IsReadOnly(this Type type)
        {
            return type.GetCustomAttributes<ReadOnlyAttribute>()?.Any() ?? false;
        }
    }
}