using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using System.Threading.Tasks;

namespace Shares.Orders.Infrastructure.Services
{
    public interface IOrderService
    {
        Task<Result<string>> CreateAsync(CreateOrderDto dto);
        Task<Result> DeleteAsync(string id);
        Task<Result<string>> SellAsync(CreateSellOrderDto dto);
    }
}
