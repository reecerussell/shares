using Microsoft.EntityFrameworkCore;
using Shares.Core;
using Shares.Users.Data.Configurations;
using System.Threading.Tasks;

namespace Shares.Users.Data
{
    public class UserContext : DbContext
    {
        private readonly IConnectionStringProvider _provider;

        public UserContext(IConnectionStringProvider provider)
        {
            _provider = provider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Task.Run(async () =>
            {
                var connectionString = await _provider.Get();
                optionsBuilder.UseMySQL(connectionString);
            }).Wait();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
