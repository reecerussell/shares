using Microsoft.EntityFrameworkCore;

namespace Shares.Core.Entity
{
    public abstract class ReadRepository<T> where T : Aggregate
    {
        private readonly BaseReadContext _context;
        private DbSet<T> _set;

        protected DbSet<T> Set => _set ??= _context.Set<T>();
        
        protected ReadRepository(BaseReadContext context)
        {
            _context = context;
        }
    }
}
