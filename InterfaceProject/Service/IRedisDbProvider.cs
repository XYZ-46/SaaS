using StackExchange.Redis;

namespace InterfaceProject.Service
{
    public interface IRedisDbProvider : IDisposable
    {
        public IDatabase Database { get; }
    }
}
