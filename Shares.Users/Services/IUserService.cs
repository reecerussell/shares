using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using System.Threading.Tasks;

namespace Shares.Users.Services
{
    public interface IUserService
    {
        Task<Result<string>> CreateAsync(CreateUserDto dto);
        Task<Result> UpdateAsync(UpdateUserDto dto);
        Task<Result> ChangePasswordAsync(ChangePasswordDto dto);
    }
}
