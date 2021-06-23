using System;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// IRedisProvider
    /// </summary>
    public interface IRedisProvider
    {
        void Persist(String key, Object obj);
        T Get<T>(String key);
        void Remove(String key);
    }
}
