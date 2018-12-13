using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Services
{
    public static class SessionExtensions
    {
        public static ISession SetString(this ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
            return session;
        }

        public static string GetString(this ISession session, string key)
        {
            session.TryGetValue(key, out var value);
            if (value == null)
                return null;
            return Encoding.UTF8.GetString(value);
        }

        public static ISession SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
            return session;
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return (value == null || value == "null") ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}