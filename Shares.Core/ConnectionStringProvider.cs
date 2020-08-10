using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Shares.Core
{
    internal class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get()
        {
            return _configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task<string> GetAsync()
        {
            return _configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
