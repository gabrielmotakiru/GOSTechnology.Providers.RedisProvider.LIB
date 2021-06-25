using System;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// ConstantsRedisProvider.
    /// </summary>
    public static class ConstantsRedisProvider
    {
        public const String Empty = "";
        public const String APP_SETTINGS = "appsettings.json";
        public const String REDIS_CONNECTION_STRING = "RedisConnectionString";
        public const String MSG_REQUIRED_CONNECTION_STRING = "Check the redis connection string in the environment variables. E.g: RedisConnectionString = host;port;password;timeout;timecache;";
    }
}
