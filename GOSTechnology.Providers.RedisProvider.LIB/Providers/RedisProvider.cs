using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// RedisProvider.
    /// </summary>
    public class RedisProvider : IRedisProvider
    {
        /// <summary>
        /// _databaseRedis.
        /// </summary>
        private IDatabase _databaseRedis;

        /// <summary>
        /// _logger.
        /// </summary>
        private readonly ILogger<RedisProvider> _logger;

        /// <summary>
        /// _connectionMultiplexer.
        /// </summary>
        private ConnectionMultiplexer _connectionMultiplexer;

        /// <summary>
        /// _architectureProvider.
        /// </summary>
        private readonly IArchitectureProvider _architectureProvider;

        /// <summary>
        /// RedisProvider.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="architectureProvider"></param>
        public RedisProvider(ILogger<RedisProvider> logger, IArchitectureProvider architectureProvider)
        {
            this._logger = logger;
            this._architectureProvider = architectureProvider;
        }

        /// <summary>
        /// OpenConnection.
        /// </summary>
        /// <param name="defaultDatabase">Default database for connect in Redis server.</param>
        /// <returns></returns>
        private Boolean OpenConnection(Int32 defaultDatabase = ConstantsRedisProvider.CONFIG_DEFAULT_DATABASE)
        {
            Boolean result = false;

            try
            {
                ConfigurationOptions configurationOptions = this._architectureProvider.GetConfigurationOptions();
                this._connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
                this._databaseRedis = this._connectionMultiplexer.GetDatabase(defaultDatabase);

                if (this._databaseRedis != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }

            return result;
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
            this._databaseRedis = null;
        }

        /// <summary>
        /// Persist.
        /// </summary>
        /// <param name="key">Key string for persist object in Redis server.</param>
        /// <param name="obj">Object for persist in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        public void Persist(String key, Object obj, CommandFlags commandFlags = CommandFlags.FireAndForget)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(key) && obj != null)
                {
                    if (this.OpenConnection())
                    {
                        var timeCacheSeconds = this._architectureProvider.GetTimeCache();
                        this._databaseRedis.StringSet(key, JsonConvert.SerializeObject(obj), TimeSpan.FromSeconds(timeCacheSeconds), flags: commandFlags);
                        this._logger.LogInformation(ConstantsRedisProvider.MSG_SUCCESS_PERSIST_OBJECT);
                        this.CloseConnection();
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_KEY_OBJECT);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }
        }

        /// <summary>
        /// PersistAsync.
        /// </summary>
        /// <param name="key">Key string for persist object in Redis server.</param>
        /// <param name="obj">Object for persist in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        /// <returns></returns>
        public async Task PersistAsync(String key, Object obj, CommandFlags commandFlags = CommandFlags.FireAndForget)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(key) && obj != null)
                {
                    if (this.OpenConnection())
                    {
                        var timeCacheSeconds = this._architectureProvider.GetTimeCache();
                        await this._databaseRedis.StringSetAsync(key, JsonConvert.SerializeObject(obj), TimeSpan.FromSeconds(timeCacheSeconds), flags: commandFlags);
                        this._logger.LogInformation(ConstantsRedisProvider.MSG_SUCCESS_PERSIST_OBJECT);
                        this.CloseConnection();
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_KEY_OBJECT);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }
        }

        /// <summary>
        /// Remove.
        /// </summary>
        /// <param name="key">Key string for remove object in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        public void Remove(String key, CommandFlags commandFlags = CommandFlags.FireAndForget)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    if (this.OpenConnection())
                    {
                        this._databaseRedis.KeyDelete(key, flags: commandFlags);
                        this._logger.LogInformation(ConstantsRedisProvider.MSG_SUCCESS_PREMOVE_OBJECT);
                        this.CloseConnection();
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_KEY);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }
        }

        /// <summary>
        /// RemoveAsync.
        /// </summary>
        /// <param name="key">Key string for remove object in Redis server.</param>
        /// <param name="commandFlags">Command flag for processing request in Redis server.</param>
        /// <returns></returns>
        public async Task RemoveAsync(String key, CommandFlags commandFlags = CommandFlags.FireAndForget)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    if (this.OpenConnection())
                    {
                        await this._databaseRedis.KeyDeleteAsync(key, flags: commandFlags);
                        this._logger.LogInformation(ConstantsRedisProvider.MSG_SUCCESS_PREMOVE_OBJECT);
                        this.CloseConnection();
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_KEY);
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
        /// <typeparam name="T">Type object for requesting/parsing/conversion in Redis server.</typeparam>
        /// <param name="key">Key string for get object in Redis server.</param>
        /// <returns></returns>
        public T Get<T>(String key)
        {
            T result = default(T);

            try
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    if (this.OpenConnection())
                    {
                        var resultRedis = this._databaseRedis.StringGet(key);

                        if (resultRedis.HasValue)
                        {
                            result = JsonConvert.DeserializeObject<T>(resultRedis);
                            this._logger.LogInformation(ConstantsRedisProvider.MSG_SUCCESS_GET_OBJECT);
                        }
                        else
                        {
                            this._logger.LogInformation(ConstantsRedisProvider.MSG_NOT_FOUND_OBJECT);
                        }

                        this.CloseConnection();
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_KEY);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }

            return result;
        }

        /// <summary>
        /// GetAsync.
        /// </summary>
        /// <typeparam name="T">Type object for requesting/parsing/conversion in Redis server.</typeparam>
        /// <param name="key">Key string for get object in Redis server.</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(String key)
        {
            T result = default(T);

            try
            {
                if (!String.IsNullOrWhiteSpace(key))
                {
                    if (this.OpenConnection())
                    {
                        var resultRedis = await this._databaseRedis.StringGetAsync(key);

                        if (resultRedis.HasValue)
                        {
                            result = JsonConvert.DeserializeObject<T>(resultRedis);
                            this._logger.LogInformation(ConstantsRedisProvider.MSG_SUCCESS_GET_OBJECT);
                        }
                        else
                        {
                            this._logger.LogInformation(ConstantsRedisProvider.MSG_NOT_FOUND_OBJECT);
                        }

                        this.CloseConnection();
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_KEY);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }

            return result;
        }
    }
}
