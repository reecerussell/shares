using System.Threading.Tasks;

namespace Shares.Core
{
    public interface IConnectionStringProvider
    {
        Task<string> Get();
    }
}
