using InterfaceProject.Service;
using System.Text.Json;

namespace Service
{
    public class RedisCacheHandler : ICacheHandler
    {
        private readonly IRedisDbProvider _redisDbProvider;

        public RedisCacheHandler(IRedisDbProvider redisDbProvider)
        {
            _redisDbProvider = redisDbProvider ?? throw new ArgumentNullException(nameof(redisDbProvider));

            if (_redisDbProvider.Database == null) throw new ArgumentNullException("The provided redisDbProvider or its database is null");
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            var value = await _redisDbProvider.Database.StringGetAsync(key);
            if (value.HasValue) return JsonSerializer.Deserialize<T>(value!);
            return default;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var isSet = await _redisDbProvider.Database.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
            return isSet;
        }

        public async Task DeleteCacheAsync(string key)
        {
            await _redisDbProvider.Database.StringGetDeleteAsync(key).ConfigureAwait(false);
        }

        public async Task<bool> StringExistsAsync(string key)
        {
            return await _redisDbProvider.Database.KeyExistsAsync(key).ConfigureAwait(false);
        }
    }
}
