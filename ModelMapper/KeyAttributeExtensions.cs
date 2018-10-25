using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Data.Mapping
{
    public static class KeyAttributeExtensions
    {
        public static PropertyInfo[] GetKeys(this PropertyInfo[] propertyInfos)
        {
            return propertyInfos.Where(p => CustomAttributeExtensions.GetCustomAttribute<KeyAttribute>((MemberInfo) p) != null).ToArray();
        }

        public static bool CompareKeys<T>(this T obj, SqlDataReader reader)
        {
            var keys = obj.GetType().GetProperties().GetKeys();
            int success = 0;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (keys.Any(key => key.Name.ToLower().Equals(reader.GetName(i)) &&
                                    key.GetValue(obj).Equals(reader.GetValue(i))))
                    success++;
            }

            return keys.Length.Equals(success);
        }

        public static bool CompareKeys<TA, TB>(this TA objA, TB objB)
        {
            var keysB = objB.GetType().GetProperties().GetKeys();
            return objA.GetType().GetProperties().GetKeys().All(keyA =>
                keysB.Any(keyB => 
                    keyB.Name.Equals(keyA.Name) && 
                    keyB.GetValue(objB).Equals(keyA.GetValue(objA))
                ));
        }
    }
}