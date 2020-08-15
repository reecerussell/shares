using System.Threading.Tasks;

namespace Shares.Orders.Infrastructure.Repositories
{
    public interface IUserReadRepository
    {
        Task<bool> ExistsAsync(string id);
    }
}
