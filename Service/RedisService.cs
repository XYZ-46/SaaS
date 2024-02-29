using InterfaceProject.Service;
using StackExchange.Redis;
using System.Text.Json;

namespace Service
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase RedisDb;

        public RedisService(string connectionString)
        {
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
            RedisDb = lazyConnection.Value.GetDatabase() ?? throw new ArgumentNullException("The provided redisDbProvider or its database is null");

        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            var value = await RedisDb.StringGetAsync(key);
            if (value.HasValue) return JsonSerializer.Deserialize<T>(value!);
            return default;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, TimeSpan? expiry = null) => await RedisDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
        public async Task DeleteCacheAsync(string key) => await RedisDb.StringGetDeleteAsync(key).ConfigureAwait(false);
        public async Task<bool> StringExistsAsync(string key) => await RedisDb.KeyExistsAsync(key).ConfigureAwait(false);
    }
}
