using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Data.Mapping.Attributes;

namespace Data.Mapping.Extensions
{
    public static class KeyAttributeExtensions
    {
        public static bool IsKey(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes<KeyAttribute>()?.Any() ?? false;
        }

        public static PropertyInfo[] GetTypeKeys(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(IsKey).ToArray();
        }

        public static bool CompareKeys<T>(this T obj, IDataReader reader)
        {
            var keys = obj.GetType().GetProperties().GetTypeKeys();
            int success = 0;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (keys.Any(key => key.Name.ToLower().Equals(reader.GetName(i)) &&
                                    key.GetValue(obj).Equals(reader.GetValue(i))))
                {
                    success++;
                    if (success.Equals(keys.Length))
                        return true;
                }
            }

            return false;
        }

        public static bool CompareKeys<TA, TB>(this TA objA, TB objB)
        {
            var keysB = objB.GetType().GetProperties().GetTypeKeys();
            return objA.GetType().GetProperties().GetTypeKeys().All(keyA =>
                keysB.Any(keyB => 
                    keyB.Name.Equals(keyA.Name) && 
                    keyB.GetValue(objB).Equals(keyA.GetValue(objA))
                ));
        }
    }
}