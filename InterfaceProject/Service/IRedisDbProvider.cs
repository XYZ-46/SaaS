using StackExchange.Redis;

namespace InterfaceProject.Service
{
    public interface IRedisDbProvider
    {
        public IDatabase Database { get; }
    }
}
