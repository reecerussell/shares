using CSharpFunctionalExtensions;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Users.ValueObjects;
using System;
using System.Text.RegularExpressions;

namespace Shares.Users.Models
{
    public class User
    {
        public string Id { get; private set; }
        public UserFullname Name { get; private set; }
        public string Email { get; private set; }
        public string NormalizedEmail { get; private set; }
        public string PasswordHash { get; private set; }

        private User(UserFullname name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

        private User()
        {
        }
        
        private Result UpdateEmail(string email, INormalizer normalizer)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Result.Failure("Email cannot be empty.");
            }

            if (email.Length > 255)
            {
                return Result.Failure("Email cannot be greater than 255 characters long.");
            }

            var match = Regex.IsMatch(email, "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,6}");
            if (!match)
            {
                return Result.Failure("Email address is not valid.");
            }

            Email = email;
            NormalizedEmail = normalizer.Normalize(email);

            return Result.Success();
        }

        internal Result Update(UpdateUserDto dto, INormalizer normalizer)
        {
            return Name.Update(dto.Firstname, dto.Lastname)
                .Bind(() => UpdateEmail(dto.Email, normalizer));
        }

        private Result SetPassword(string password, IPasswordService service, IPasswordValidator validator)
        {
            return validator.Validate(password)
                .Tap(() => PasswordHash = service.Hash(password));
        }

        internal Result UpdatePassword(ChangePasswordDto dto, IPasswordService service, IPasswordValidator validator)
        {
            if (!service.Verify(dto.CurrentPassword, PasswordHash))
            {
                return Result.Failure("Current password is not valid.");
            }

            return validator.Validate(dto.NewPassword)
                .Tap(() => PasswordHash = service.Hash(dto.NewPassword));
        }

        internal static Result<User> Create(CreateUserDto dto, INormalizer normalizer, IPasswordService passwordService, IPasswordValidator validator)
        {
            User user = null;
            return UserFullname.Create(dto.Firstname, dto.Lastname)
                .Tap(n => user = new User(n))
                .Bind(_ => user.UpdateEmail(dto.Email, normalizer))
                .Bind(() => user.SetPassword(dto.Password, passwordService, validator))
                .Map(() => user);
        }
    }
}
