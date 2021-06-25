using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// ArchitectureProvider.
    /// </summary>
    public class ArchitectureProvider : IArchitectureProvider
    {
        /// <summary>
        /// _logger.
        /// </summary>
        private readonly ILogger<ArchitectureProvider> _logger;

        /// <summary>
        /// ArchitectureProvider.
        /// </summary>
        public ArchitectureProvider(ILogger<ArchitectureProvider> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// GetEnvironmentVariable.
        /// </summary>
        /// <param name="environmentVariable"></param>
        /// <returns></returns>
        private String GetEnvironmentVariable()
        {
            String result = null;

            try
            {
                var defaultEnvironment = Environment.GetEnvironmentVariable(ConstantsRedisProvider.CONFIG_REDIS_CONNECTION_STRING);
                var userEnvironment = Environment.GetEnvironmentVariable(ConstantsRedisProvider.CONFIG_REDIS_CONNECTION_STRING, EnvironmentVariableTarget.User);
                var processEnvironment = Environment.GetEnvironmentVariable(ConstantsRedisProvider.CONFIG_REDIS_CONNECTION_STRING, EnvironmentVariableTarget.Process);
                var machineEnvironment = Environment.GetEnvironmentVariable(ConstantsRedisProvider.CONFIG_REDIS_CONNECTION_STRING, EnvironmentVariableTarget.Machine);

                if (!String.IsNullOrWhiteSpace(defaultEnvironment))
                {
                    result = defaultEnvironment;
                }
                else if (!String.IsNullOrWhiteSpace(userEnvironment))
                {
                    result = userEnvironment;
                }
                else if (!String.IsNullOrWhiteSpace(processEnvironment))
                {
                    result = processEnvironment;
                }
                else if (!String.IsNullOrWhiteSpace(machineEnvironment))
                {
                    result = machineEnvironment;
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }

            return result;
        }

        /// <summary>
        /// GetTimeCache.
        /// </summary>
        /// <param name="defaultTimeCache">Default time cache for Redis server in secods.</param>
        /// <returns></returns>
        public Int32 GetTimeCache(Int32 defaultTimeCache = ConstantsRedisProvider.CONFIG_DEFAULT_TIMECACHE)
        {
            Int32 result = defaultTimeCache;

            try
            {
                IEnumerable<String> redisConnectionString = GetEnvironmentVariable()?.Split(ConstantsRedisProvider.CONFIG_SPLIT);

                if (redisConnectionString != null && redisConnectionString.Count() == 5)
                {
                    Int32.TryParse(redisConnectionString.FirstOrDefault(_ => _.Contains(ConstantsRedisProvider.CONFIG_TIMECACHE)), out Int32 timeCache);

                    if (timeCache > 0)
                    {
                        result = timeCache;
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex?.ToString());
            }

            return result;
        }

        /// <summary>
        /// GetConfigurationOptions.
        /// </summary>
        /// <returns></returns>
        public ConfigurationOptions GetConfigurationOptions()
        {
            ConfigurationOptions result = default(ConfigurationOptions);

            try
            {
                IEnumerable<String> redisConnectionString = GetEnvironmentVariable()?.Split(ConstantsRedisProvider.CONFIG_SPLIT);

                if (redisConnectionString != null && redisConnectionString.Count() == 5)
                {
                    String host = redisConnectionString.FirstOrDefault(_ => _.Contains(ConstantsRedisProvider.CONFIG_HOST)).Replace(ConstantsRedisProvider.CONFIG_HOST, String.Empty);
                    Int32.TryParse(redisConnectionString.FirstOrDefault(_ => _.Contains(ConstantsRedisProvider.CONFIG_PORT)).Replace(ConstantsRedisProvider.CONFIG_PORT, String.Empty), out Int32 port);
                    String password = redisConnectionString.FirstOrDefault(_ => _.Contains(ConstantsRedisProvider.CONFIG_PASSWORD)).Replace(ConstantsRedisProvider.CONFIG_PASSWORD, String.Empty);
                    Int32.TryParse(redisConnectionString.FirstOrDefault(_ => _.Contains(ConstantsRedisProvider.CONFIG_TIMEOUT)).Replace(ConstantsRedisProvider.CONFIG_TIMEOUT, String.Empty), out Int32 timeout);

                    if (!String.IsNullOrEmpty(host) && port > 0 && timeout > 0)
                    {
                        result = new ConfigurationOptions
                        {
                            EndPoints = { { host, port } },
                            Password = password,
                            ConnectTimeout = timeout
                        };
                    }
                    else
                    {
                        this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
                    }
                }
                else
                {
                    this._logger.LogWarning(ConstantsRedisProvider.MSG_REQUIRED_CONNECTION_STRING);
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
