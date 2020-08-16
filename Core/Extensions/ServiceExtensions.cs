using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Shares.Core.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services)
        {
            return services.AddLogging(logging => logging.AddConsole());
        }

        public static IServiceCollection ConfigureCache(this IServiceCollection services)
        {
            return services.AddMemoryCache();
        }

        public static IServiceCollection ConfigureConfig(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            return services.AddSingleton<IConfiguration>(builder.Build());
        }
    }
}
