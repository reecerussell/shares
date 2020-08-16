using Microsoft.Extensions.DependencyInjection;
using Shares.Core.Password;

namespace Shares.Core
{
    public static class Configure
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            return services
                .AddTransient<INormalizer, Normalizer>()
                .AddTransient<IConnectionStringProvider, ConnectionStringProvider>()
                .AddTransient<IPasswordService, PasswordService>()
                .AddTransient<IPasswordValidator, PasswordValidator>();
        }
    }
}
