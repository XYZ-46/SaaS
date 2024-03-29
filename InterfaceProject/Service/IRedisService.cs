﻿namespace InterfaceProject.Service
{
    public interface IRedisService
    {
        Task<T?> GetDataAsync<T>(string key);
        Task<bool> SetDataAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task DeleteCacheAsync(string key);
        Task<bool> StringExistsAsync(string key);
    }
}
