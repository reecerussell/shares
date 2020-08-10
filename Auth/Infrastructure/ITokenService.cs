using CSharpFunctionalExtensions;
using Shares.Auth.Domain.Dtos;
using System.Threading.Tasks;

namespace Shares.Auth.Infrastructure
{
    public interface ITokenService
    {
        Task<Result<AccessToken>> GenerateAsync(UserCredential credential);
        Task<Result> VerifyAsync(string token);
    }
}
