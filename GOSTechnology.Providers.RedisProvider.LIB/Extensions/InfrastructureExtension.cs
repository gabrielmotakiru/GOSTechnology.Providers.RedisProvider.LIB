using StackExchange.Redis;
using System;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// InfrastructureExtension.
    /// </summary>
    public static class InfrastructureExtension
    {
        /// <summary>
        /// GetConfigurationOptions.
        /// </summary>
        /// <returns></returns>
        public static ConfigurationOptions GetConfigurationOptions()
        {
            ConfigurationOptions result = default(ConfigurationOptions);

            try
            {
                var redisConnectionString = GetRedisEnvironment()?.Split(';');

                if (redisConnectionString != null && redisConnectionString.Length == 5)
                {
                    String host = redisConnectionString[0];
                    Int32.TryParse(redisConnectionString[1], out Int32 port);
                    String password = redisConnectionString[2];
                    Int32.TryParse(redisConnectionString[3], out Int32 timeout);

                    result = new ConfigurationOptions
                    {
                        EndPoints = { { host, port } },
                        Password = password,
                        ConnectTimeout = timeout
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex?.ToString());
            }

            return result;
        }

        /// <summary>
        /// GetTimeCacheSecods.
        /// </summary>
        /// <returns></returns>
        public static Int32 GetTimeCacheSecods()
        {
            Int32 result = 0;

            try
            {
                var redisConnectionString = GetRedisEnvironment()?.Split(';');

                if (redisConnectionString != null && redisConnectionString.Length == 5)
                {
                    Int32.TryParse(redisConnectionString[4], out Int32 timeCacheSecods);
                    result = timeCacheSecods;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex?.ToString());
            }

            return result;
        }

        /// <summary>
        /// GetRedisEnvironment.
        /// </summary>
        /// <returns></returns>
        private static String GetRedisEnvironment(String environmentVariableKey = ConstantsRedisProvider.REDIS_CONNECTION_STRING)
        {
            String result = null;

            try
            {
                var defaultEnvironment = Environment.GetEnvironmentVariable(environmentVariableKey);
                var userEnvironment = Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.User);
                var processEnvironment = Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.Process);
                var machineEnvironment = Environment.GetEnvironmentVariable(environmentVariableKey, EnvironmentVariableTarget.Machine);

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
                Console.WriteLine(ex?.ToString());
            }

            return result;
        }
    }
}
