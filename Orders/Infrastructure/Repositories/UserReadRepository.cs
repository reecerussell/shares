using Microsoft.EntityFrameworkCore;
using Shares.Core.Entity;
using Shares.Orders.Domain.Models;
using System.Threading.Tasks;

namespace Shares.Orders.Infrastructure.Repositories
{
    internal class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(OrdersReadContext context) 
            : base(context)
        {
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await Set.AnyAsync(x => x.Id == id);
        }
    }
}
