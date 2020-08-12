using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.KeyManagementService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shares.Core;

namespace Shares.AWS
{
    public static class Configure
    {
        public static IServiceCollection ConfigureAws(this IServiceCollection services)
        {
            return services
                .AddDefaultAWSOptions(new AWSOptions
                {
                    Profile = "Shares",
                    ProfilesLocation = "/app/aws-credentials",
                    Region = RegionEndpoint.EUWest2
                })
                .AddLogging(options =>
                {
                    options.AddConsole();
                    options.AddAWSProvider();
                    options.SetMinimumLevel(LogLevel.Debug);
                })
                .AddAWSService<IAmazonKeyManagementService>()
                .AddTransient<IAsymmetricHasher, AsymmetricHasher>();
        }
    }
}
