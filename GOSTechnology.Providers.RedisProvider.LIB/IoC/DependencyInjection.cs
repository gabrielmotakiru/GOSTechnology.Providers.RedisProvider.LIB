using Microsoft.Extensions.DependencyInjection;
using System;

namespace GOSTechnology.Providers.RedisProvider.LIB
{
    /// <summary>
    /// DependencyInjection.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// AddRedisProvider.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeInjection"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisProvider(this IServiceCollection builder, TypeInjection typeInjection = TypeInjection.SCOPED)
        {
            try
            {
                switch (typeInjection)
                {
                    case TypeInjection.SINGLETON:
                        {
                            builder.AddSingleton<IRedisProvider, RedisProvider>();
                            break;
                        }
                    case TypeInjection.SCOPED:
                        {
                            builder.AddScoped<IRedisProvider, RedisProvider>();
                            break;
                        }
                    case TypeInjection.TRANSIENT:
                        {
                            builder.AddTransient<IRedisProvider, RedisProvider>();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex?.ToString());
            }

            return builder;
        }
    }
}
