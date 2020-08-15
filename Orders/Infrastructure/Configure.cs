using Microsoft.Extensions.DependencyInjection;
using Shares.Orders.Infrastructure.Repositories;
using Shares.Orders.Infrastructure.Services;

namespace Shares.Orders.Infrastructure
{
    public static class Configure
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<OrdersContext>()
                .AddScoped<OrdersReadContext>()
                .AddTransient<IInstrumentReadRepository, InstrumentReadRepository>()
                .AddTransient<IUserReadRepository, UserReadRepository>()
                .AddTransient<IOrderRepository, OrderRepository>()
                .AddTransient<IOrderService, OrderService>();
        }
    }
}
