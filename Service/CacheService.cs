using InterfaceProject.Service;
using StackExchange.Redis;
using System.Text.Json;

namespace Service
{
    public class CacheService(string redisConnection) : ICacheService
    {
        private readonly IDatabase _db = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisConnection)).Value.GetDatabase();

        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value)) return JsonSerializer.Deserialize<T>(value);
            return default;
        }
        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _db.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }
        public async Task<object> RemoveDataAsync(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist) return await _db.KeyDeleteAsync(key);
            return false;
        }
    }
}
