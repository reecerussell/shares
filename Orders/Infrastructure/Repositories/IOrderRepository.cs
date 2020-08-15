using CSharpFunctionalExtensions;
using Shares.Orders.Domain.Models;
using System.Threading.Tasks;

namespace Shares.Orders.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<Maybe<Order>> FindByIdAsync(string id);
        void Add(Order order);
        void Remove(Order order);
        Task<Result> SaveChangesAsync();
    }
}
