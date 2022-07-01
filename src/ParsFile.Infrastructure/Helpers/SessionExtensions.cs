using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ParsFile.Infrastructure.Helpers
{
    public static class SessionExtensions
    {

        public static void Set<T>(this ISession session, String key, T obj)
        {
            session.SetString(key, JsonSerializer.Serialize(obj));
        }

        public static T Get<T>(this ISession session, String key)
        {
            var obj = session.GetString(key);
            return obj == null ? default : JsonSerializer.Deserialize<T>(obj);
        }
    }
}
