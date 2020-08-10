using System.Threading.Tasks;

namespace Shares.Core
{
    public interface IConnectionStringProvider
    {
        string Get();
        Task<string> GetAsync();
    }
}
