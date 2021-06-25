using System;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// ConstantsRedisProvider.
    /// </summary>
    public static class ConstantsRedisProvider
    {
        #region "CONFIGURATIONS"

        public const String CONFIG_APP_SETTINGS = "appsettings.json";
        public const String CONFIG_HOST = "host=";
        public const String CONFIG_PORT = "port=";
        public const String CONFIG_PASSWORD = "password=";
        public const String CONFIG_TIMEOUT = "timeout=";
        public const String CONFIG_TIMECACHE = "timecache=";
        public const String CONFIG_REDIS_CONNECTION_STRING = "RedisConnectionString";
        public const String CONFIG_EMPTY = "";
        public const Int32 CONFIG_DEFAULT_TIMECACHE = 300;
        public const Int32 CONFIG_DEFAULT_DATABASE = 0;
        public const Char CONFIG_SPLIT = ';';

        #endregion

        #region "MESSAGES"

        public const String MSG_REQUIRED_CONNECTION_STRING = "Check the Redis Connection String in the Environment Variables. E.g: RedisConnectionString = \"host=x.x.x.x;port=6379;password=mypassword;timeout=500;timecache=300;\"";
        public const String MSG_REQUIRED_KEY_OBJECT = "Check the sent Key and Object. Both need to have values to be processed.";
        public const String MSG_REQUIRED_KEY = "Check the Key sent. She must have value to be processed.";
        public const String MSG_NOT_FOUND_OBJECT = "The searched object was not found in the Redis bases.";
        public const String MSG_SUCCESS_GET_OBJECT = "Success getting object from Redis bases.";
        public const String MSG_SUCCESS_PERSIST_OBJECT = "Success persist object from Redis bases.";
        public const String MSG_SUCCESS_PREMOVE_OBJECT = "Success remove object from Redis bases.";

        #endregion

        #region "TESTS" 

        public const String MESSAGE_SUCCESS_PERSIST = "ShouldSuccessPersistObjectByKeyObject";
        public const String MESSAGE_SUCCESS_REMOVE = "ShouldFSuccessRemoveObjectByKey";
        public const String MESSAGE_SUCCESS_GET = "ShouldSuccessGetObjectbyKey";
        public const String KEY_NULL = null;
        public const String KEY_EMPTY = "00000000-0000-0000-0000-000000000000";
        public const String KEY_1 = "861708da-924e-4d07-ac83-f6c6d3ff28b2";
        public const String KEY_2 = "feb0ffee-75e4-4ecd-9861-bdd92371939a";
        public const String KEY_3 = "b02ca313-3afe-4321-b12d-076d586b1404";

        #endregion
    }
}
