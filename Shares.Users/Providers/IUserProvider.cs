using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shares.Users.Providers
{
    public interface IUserProvider
    {
        Task<Maybe<UserDto>> GetAsync(string id);
        Task<IReadOnlyList<UserDto>> GetAsync();
    }
}
