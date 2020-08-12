using Microsoft.EntityFrameworkCore;
using Shares.Core;
using Shares.Users.Data.Configurations;

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
            optionsBuilder.UseMySQL(_provider.Get());

            base.OnConfiguring(optionsBuilder);
        }
    }
}
