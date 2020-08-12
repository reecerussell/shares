using Microsoft.EntityFrameworkCore;
using Shares.Auth.Infrastructure.Configuration;
using Shares.Core;

namespace Shares.Auth.Infrastructure
{
    public class AuthContext : DbContext
    {
        private readonly IConnectionStringProvider _provider;

        public AuthContext(IConnectionStringProvider provider)
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
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.UseMySQL(_provider.Get());

            base.OnConfiguring(optionsBuilder);
        }
    }
}
