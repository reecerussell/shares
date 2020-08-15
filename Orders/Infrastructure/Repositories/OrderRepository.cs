using Microsoft.Extensions.Logging;
using Shares.Core.Entity;
using Shares.Orders.Domain.Models;

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
    }
}
