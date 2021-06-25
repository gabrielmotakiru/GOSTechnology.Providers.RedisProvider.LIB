using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// IRedisProvider
    /// </summary>
    public interface IRedisProvider
    {
        /// <summary>
        /// Persist.
        /// </summary>
        /// <param name="key">Key string for persist object in Redis server.</param>
        /// <param name="obj">Object for persist in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        void Persist(String key, Object obj, CommandFlags commandFlags = CommandFlags.FireAndForget);

        /// <summary>
        /// PersistAsync.
        /// </summary>
        /// <param name="key">Key string for persist object in Redis server.</param>
        /// <param name="obj">Object for persist in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        /// <returns></returns>
        Task PersistAsync(String key, Object obj, CommandFlags commandFlags = CommandFlags.FireAndForget);

        /// <summary>
        /// Remove.
        /// </summary>
        /// <param name="key">Key string for remove object in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        void Remove(String key, CommandFlags commandFlags = CommandFlags.FireAndForget);

        /// <summary>
        /// RemoveAsync.
        /// </summary>
        /// <param name="key">Key string for remove object in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        /// <returns></returns>
        Task RemoveAsync(String key, CommandFlags commandFlags = CommandFlags.FireAndForget);

        /// <summary>
        /// Get.
        /// </summary>
        /// <typeparam name="T">Type object for requesting/parsing/conversion in Redis server.</typeparam>
        /// <param name="key">Key string for get object in Redis server.</param>
        /// <returns></returns>
        T Get<T>(String key);

        /// <summary>
        /// GetAsync.
        /// </summary>
        /// <typeparam name="T">Type object for requesting/parsing/conversion in Redis server.</typeparam>
        /// <param name="key">Key string for get object in Redis server.</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(String key);
    }
}
