using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Shares.Core.Entity
{
    public abstract class WriteRepository<T> where T : Aggregate
    {
        private readonly DbContext _context;
        private readonly ILogger<WriteRepository<T>> _logger;
        private DbSet<T> _set;

        protected DbSet<T> Set => _set ??= _context.Set<T>();

        protected WriteRepository(
            DbContext context,
            ILogger<WriteRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<Maybe<T>> FindByIdAsync(string id)
        {
            return await Set.FindAsync(id);
        }

        public void Add(T item)
        {
            Set.Add(item);
        }

        public void Remove(T item)
        {
            Set.Remove(item);
        }

        public async Task<Result> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e.Message, e);
                
                return Result.Failure(ErrorMessages.DbConcurrencyError);
            }
            catch (DbUpdateException e)
            {
                var inner = e.InnerException;
                if (inner != null)
                {
                    _logger.LogError(inner.Message, inner);
                }
                else
                {
                    _logger.LogError(e.Message, e);
                }

                return Result.Failure(ErrorMessages.DbUpdateError);
            }
        }
    }
}
