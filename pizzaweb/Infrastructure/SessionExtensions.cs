using System.Text.Json;
namespace pizzaweb.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value) 
        {
            var a = JsonSerializer.Serialize(value);
            session.SetString(key, a);
        }

        public static T? GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonSerializer.Deserialize<T>(sessionData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });   
        }

    }
}
