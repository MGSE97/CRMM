using System.Reflection;
using System.Runtime.CompilerServices;

namespace Data.Mapping
{
    public static class AccessProtectionChecker
    {
        public static bool CanWrite(this PropertyInfo property, AccessProtection accessProtection = AccessProtection.Private)
        {
            bool can = property.CanWrite;
            if (!can || accessProtection.HasFlag(AccessProtection.Private))
                return can;
            return property.SetMethod.IsPublic;
        }

        public static bool CanRead(this PropertyInfo property, AccessProtection accessProtection = AccessProtection.Private)
        {
            bool can = property.CanRead;
            if (!can || accessProtection.HasFlag(AccessProtection.Private))
                return can;
            return property.GetMethod.IsPublic;
        }
    }
}