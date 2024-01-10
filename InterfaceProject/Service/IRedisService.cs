namespace InterfaceProject.Service
{
    public interface IRedisService : IDisposable
    {
        Task<T> GetDataAsync<T>(string key);
        bool SetData<T>(string key, T value, TimeSpan expirationTime);
        bool RemoveData(string key);
    }
}
