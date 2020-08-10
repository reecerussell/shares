using CSharpFunctionalExtensions;
using Shares.Auth.Domain.Models;
using System.Threading.Tasks;

namespace Shares.Auth.Infrastructure
{
    public interface IUserProvider
    {
        Task<Maybe<User>> FindByEmailAsync(string email);
    }
}
