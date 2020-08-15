using System.Threading.Tasks;

namespace Shares.Orders.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string id);
    }
}
