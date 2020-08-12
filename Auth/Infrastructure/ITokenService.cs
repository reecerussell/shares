using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using System.Threading.Tasks;

namespace Shares.Auth.Infrastructure
{
    public interface ITokenService
    {
        Task<Result<AccessTokenDto>> GenerateAsync(UserCredentialDto credential);
        Task<Result> VerifyAsync(string token);
    }
}
