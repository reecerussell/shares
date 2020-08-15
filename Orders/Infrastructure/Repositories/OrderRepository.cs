using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shares.Core.Entity;
using Shares.Orders.Domain.Models;
using System.Threading.Tasks;

namespace Shares.Orders.Infrastructure.Repositories
{
    internal class OrderRepository : WriteRepository<Order>, IOrderRepository
    {
        public OrderRepository(
            OrdersContext context, 
            ILogger<WriteRepository<Order>> logger) 
            : base(context, logger)
        {
        }

        public override async Task<Maybe<Order>> FindByIdAsync(string id)
        {
            return await Set
                .Include(x => x.SellOrders)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
