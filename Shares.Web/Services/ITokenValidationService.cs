using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Shares.Web.Services
{
    public interface ITokenValidationService
    {
        Task<Result> ValidateAsync(string token);
    }
}
