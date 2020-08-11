using Microsoft.Extensions.DependencyInjection;
using Shares.Auth.Infrastructure.Providers;
using Shares.Auth.Infrastructure.Services;

namespace Shares.Auth.Infrastructure
{
    public static class Configure
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<AuthContext>()
                .AddTransient<IUserProvider, UserProvider>()
                .AddTransient<ITokenService, TokenService>();
        }
    }
}
