using Microsoft.EntityFrameworkCore;

namespace Shares.Core.Entity
{
    public abstract class BaseReadContext : DbContext
    {
        protected new virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
