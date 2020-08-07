using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shares.Users.Data;
using Shares.Users.Models;
using System.Threading.Tasks;

namespace Shares.Users.Repositories
{
    public class UserRepository
    {
        private DbContext _context;
        private DbSet<User> _set;

        public DbSet<User> Set => _set ??= _context.Set<User>();

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<Maybe<User>> FindByIdAsync(string id)
        {
            return await Set.FindAsync(id);
        }

        public void Add(User user)
        {
            Set.Add(user);
        }

        public void Remove(User user)
        {
            Set.Remove(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
