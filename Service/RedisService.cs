using InterfaceProject.Service;
using StackExchange.Redis;
using System.Text.Json;

namespace Service
{
    public class RedisService : IRedisService
    {
        private IDatabase _cacheDB;

        public RedisService()
        {
            var _redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDB = _redisConnection.GetDatabase();

        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return default;

            var value = await _cacheDB.StringGetAsync(key);

            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public bool RemoveData(string key)
        {
            if (_cacheDB.KeyExists(key)) return _cacheDB.KeyDelete(key);

            return false;
        }

        public bool SetData<T>(string key, T value, TimeSpan expirationTime)
        {
            return _cacheDB.StringSet(key, JsonSerializer.Serialize(value), expirationTime);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing) this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
