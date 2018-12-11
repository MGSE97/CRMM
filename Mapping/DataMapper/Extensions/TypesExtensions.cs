using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace Data.Mapping.Extensions
{
    public static class TypesExtensions
    {
        public static bool IsLazy(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsGenericType && typeof(Lazy<>).IsAssignableFrom(propertyInfo.PropertyType.GetGenericTypeDefinition());
        }

        public static bool IsNull<T>(this T property, PropertyInfo info = null)
        {
            if (property == null)
                return true;
            if (info == null)
                return EqualityComparer<T>.Default.Equals(property, default(T));
            return property.Equals(info.PropertyType.GetDefault());
    }

        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}