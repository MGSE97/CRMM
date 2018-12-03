using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Data.Mapping.Attributes;

namespace Data.Mapping.Extensions
{
    public static class IgnoreAttributeExtensions
    {
        public static bool IsIgnored(this PropertyInfo property)
        {
            return property.GetCustomAttributes<IgnoreAttribute>().Any();
        }
    }
}
