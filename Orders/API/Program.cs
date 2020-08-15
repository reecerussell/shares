using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Shares.AWS;
using Shares.Core;
using Shares.Core.Extensions;
using Shares.Core.Services;
using Shares.Orders.API.Handlers;
using Shares.Orders.Infrastructure;
using System;

namespace Shares.Orders.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!int.TryParse(Environment.GetEnvironmentVariable("PORT"), out var port))
            {
                port = 8443;
            }

            var services = RegisterServices();
            var orderHandler = services.GetRequiredService<OrderHandler>();

            var server = new Server
            {
                Services = { OrderService.BindService(orderHandler) },
                Ports = { new ServerPort("0.0.0.0", port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Token server listening on port " + port);

            // Keep the application running, to allow incoming RPC calls.
            // Cannot use Console.ReadKey as Docker Compose doesn't support reading from console.
            while (true) { }
            // ReSharper disable once FunctionNeverReturns
        }

        private static IServiceProvider RegisterServices() => new ServiceCollection()
            .ConfigureConfig()
            .ConfigureCache()
            .ConfigureLogging()
            .RegisterCoreServices()
            .ConfigureAws()
            .ConfigureInfrastructure()
            .AddScoped<OrderHandler>()
            .BuildServiceProvider();
    }
}
