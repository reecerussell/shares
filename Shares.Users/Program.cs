using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Shares.Core;
using Shares.Core.Extensions;
using Shares.Users.Data;
using Shares.Users.Handlers;
using Shares.Users.Providers;
using Shares.Users.Repositories;
using Shares.Users.Services;
using System;

namespace Shares.Users
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
            var userHandler = services.GetRequiredService<UserHandler>();

            var server = new Server
            {
                Services = { Core.Services.UserService.BindService(userHandler) },
                Ports = { new ServerPort("0.0.0.0", port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Users server listening on port " + port);

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
            .AddScoped<IPasswordService, PasswordService>()
            .AddScoped<UserContext>()
            .AddScoped<UserRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserProvider, UserProvider>()
            .AddScoped<UserHandler>()
            .BuildServiceProvider();
    }
}
