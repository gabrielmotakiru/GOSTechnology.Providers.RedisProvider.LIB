using StackExchange.Redis;
using System;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// IArchitectureProvider.
    /// </summary>
    public interface IArchitectureProvider
    {
        /// <summary>
        /// GetTimeCache.
        /// </summary>
        /// <param name="defaultTimeCache">Default time cache for Redis server in secods.</param>
        /// <returns></returns>
        Int32 GetTimeCache(Int32 defaultTimeCache = ConstantsRedisProvider.CONFIG_DEFAULT_TIMECACHE);

        /// <summary>
        /// GetConfigurationOptions.
        /// </summary>
        /// <returns></returns>
        ConfigurationOptions GetConfigurationOptions();
    }
}
