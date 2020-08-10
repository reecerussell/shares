using Microsoft.EntityFrameworkCore;
using Shares.Auth.Infrastructure.Configuration;
using Shares.Core;
using Shares.Core.Entity;

namespace Shares.Auth.Infrastructure
{
    internal class AuthContext : BaseReadContext
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public AuthContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionStringProvider.Get());

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
