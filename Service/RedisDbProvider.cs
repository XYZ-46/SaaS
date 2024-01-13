using InterfaceProject.Service;
using StackExchange.Redis;

namespace Service
{
    public class RedisDbProvider : IRedisDbProvider
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private readonly string _connectionString;

        public RedisDbProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(connectionString);
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_connectionString));
        }

        public IDatabase Database => _lazyConnection.Value.GetDatabase();
    }
}
