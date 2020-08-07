using Microsoft.Extensions.DependencyInjection;

namespace Shares.Core
{
    public static class Configure
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            return services
                .AddTransient<INormalizer, Normalizer>()
                .AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
        }
    }
}
