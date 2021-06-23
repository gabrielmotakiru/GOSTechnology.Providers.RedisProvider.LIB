using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// RedisProvider.
    /// </summary>
    public class RedisProvider : IRedisProvider
    {
        /// <summary>
        /// _redis.
        /// </summary>
        private IDatabase _redis;

        /// <summary>
        /// _configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// _logger.
        /// </summary>
        private readonly ILogger<RedisProvider> _logger;

        /// <summary>
        /// _connectionMultiplexer.
        /// </summary>
        private ConnectionMultiplexer _connectionMultiplexer;

        /// <summary>
        /// RedisProvider.
        /// </summary>
        /// <param name="configuration"></param>
        public RedisProvider(IConfiguration configuration, ILogger<RedisProvider> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// OpenConnection.
        /// </summary>
        private void OpenConnection()
        {
            try
            { 
                String host = this._configuration.GetSection(ConstantsRedisProvider.REDIS_HOST).Value;
                String password = this._configuration.GetSection(ConstantsRedisProvider.REDIS_PASSWORD).Value;
                Int32.TryParse(this._configuration.GetSection(ConstantsRedisProvider.REDIS_CONNECTION_TIMEOUT).Value, out Int32 timeout);
                Int32.TryParse(this._configuration.GetSection(ConstantsRedisProvider.REDIS_PORT).Value, out Int32 port);

                ConfigurationOptions options = new ConfigurationOptions
                {
                    EndPoints = { { host, port } },
                    Password = password,
                    ConnectTimeout = timeout
                };

                this._connectionMultiplexer = ConnectionMultiplexer.Connect(options);
                this._redis = this._connectionMultiplexer.GetDatabase(0);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }
        }

        /// <summary>
        /// CloseConnection.
        /// </summary>
        private void CloseConnection()
        {
            if (this._connectionMultiplexer != null)
            {
                this._connectionMultiplexer.Close();
            }

            this._connectionMultiplexer = null;
            this._redis = null;
        }

        /// <summary>
        /// Persist.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Persist(String key, Object obj)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(key) && obj != null)
                {
                    this.OpenConnection();

                    if (this._redis != null)
                    {
                        Int32.TryParse(this._configuration.GetSection(ConstantsRedisProvider.REDIS_TIME_CACHE_SECONDS).Value, out Int32 timeCacheSeconds);
                        this._redis.StringSetAsync($"{key}", JsonConvert.SerializeObject(obj), TimeSpan.FromSeconds(timeCacheSeconds)).GetAwaiter().GetResult();
                    }

                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(String key)
        {
            T result = default(T);

            try
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    this.OpenConnection();

                    if (this._redis != null)
                    {
                        var resultRedis = this._redis.StringGet($"{key}");

                        if (resultRedis.HasValue)
                        {
                            result = JsonConvert.DeserializeObject<T>(resultRedis);
                        }
                    }

                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }

            return result;
        }

        /// <summary>
        /// Remove.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(String key)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    this.OpenConnection();

                    if (this._redis != null)
                    {
                        this._redis.KeyDelete(key);
                    }

                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }
        }
    }
}
