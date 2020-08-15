using System.Threading.Tasks;

namespace Shares.Orders.Infrastructure.Repositories
{
    public interface IInstrumentReadRepository
    {
        Task<bool> ExistsAsync(string id);
    }
}
