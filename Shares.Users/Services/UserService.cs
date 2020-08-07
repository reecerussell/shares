using CSharpFunctionalExtensions;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Users.Models;
using Shares.Users.Repositories;
using System.Threading.Tasks;

namespace Shares.Users.Services
{
    internal class UserService : IUserService
    {
        private readonly UserRepository _repository;
        private readonly IPasswordService _passwordService;
        private readonly INormalizer _normalizer;

        public UserService(
            UserRepository repository,
            IPasswordService passwordService,
            INormalizer normalizer)
        {
            _repository = repository;
            _passwordService = passwordService;
            _normalizer = normalizer;
        }

        public async Task<Result<string>> CreateAsync(CreateUserDto dto)
        {
            return await User.Create(dto, _normalizer, _passwordService)
                .Tap(_repository.Add)
                .Tap(_repository.SaveChangesAsync)
                .Map(u => u.Id);
        }

        public async Task<Result> UpdateAsync(UpdateUserDto dto)
        {
            return await _repository.FindByIdAsync(dto.Id)
                .ToResult(ErrorMessages.UserNotFound)
                .Tap(u => u.Update(dto, _normalizer))
                .Tap(_repository.SaveChangesAsync);
        }

        public async Task<Result> ChangePasswordAsync(ChangePasswordDto dto)
        {
            return await _repository.FindByIdAsync(dto.Id)
                .ToResult(ErrorMessages.UserNotFound)
                .Tap(u => u.UpdatePassword(dto, _passwordService))
                .Tap(_repository.SaveChangesAsync);
        }
    }
}
