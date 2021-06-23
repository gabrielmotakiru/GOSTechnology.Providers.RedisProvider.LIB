using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// InfrastructureExtension.
    /// </summary>
    public static class InfrastructureExtension
    {
        /// <summary>
        /// GetConfiguration.
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetConfiguration()
        {
            IConfiguration result = default(IConfiguration);

            try
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(ConstantsRedisProvider.APP_SETTINGS, optional: false, reloadOnChange: false);

                result = builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex?.ToString());
                throw ex;
            }

            return result;
        }
    }
}
