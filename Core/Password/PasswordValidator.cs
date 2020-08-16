using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Shares.Core.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Core.Tests")]
namespace Shares.Core.Password
{
    internal class PasswordValidator : IPasswordValidator
    {
        private readonly int? _requiredLength;
        private readonly bool _requireNonAlphanumeric;
        private readonly bool _requireDigit;
        private readonly bool _requireLowercase;
        private readonly bool _requireUppercase;
        private readonly int? _requiredUniqueChars;

        public PasswordValidator(IConfiguration configuration)
        {
            _requiredLength = configuration.GetNullInt(Constants.PasswordRequiredLengthKey);
            _requireNonAlphanumeric = configuration.GetBool(Constants.PasswordRequireNonAlphanumericKey);
            _requireDigit = configuration.GetBool(Constants.PasswordRequireDigitKey);
            _requireLowercase = configuration.GetBool(Constants.PasswordRequireLowercaseKey);
            _requireUppercase = configuration.GetBool(Constants.PasswordRequireUppercaseKey);
            _requiredUniqueChars = configuration.GetNullInt(Constants.PasswordRequiredUniqueCharsKey);
        }

        public Result Validate(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Result.Failure("Password cannot be empty.");
            }

            if (_requiredLength != null && password.Length < _requiredLength)
            {
                return Result.Failure($"Password must be at least {_requiredLength} characters long.");
            }

            var hasNonAlphanumeric = false;
            var hasDigit = false;
            var hasLower = false;
            var hasUpper = false;
            var uniqueChars = new List<char>();

            foreach (var c in password)
            {
                if (!hasNonAlphanumeric && !IsLetterOrDigit(c))
                {
                    hasNonAlphanumeric = true;
                }

                if (!hasDigit && IsDigit(c))
                {
                    hasDigit = true;
                }

                if (!hasLower && IsLower(c))
                {
                    hasLower = true;
                }

                if (!hasUpper && IsUpper(c))
                {
                    hasUpper = true;
                }

                if (!uniqueChars.Contains(c))
                {
                    uniqueChars.Add(c);
                }
            }

            if (_requireNonAlphanumeric && !hasNonAlphanumeric)
            {
                return Result.Failure("Password requires an non-alphanumeric character.");
            }

            if (_requireDigit && !hasDigit)
            {
                return Result.Failure("Password requires a number.");
            }

            if (_requireLowercase && !hasLower)
            {
                return Result.Failure("Password requires a lowercase character.");
            }

            if (_requireUppercase && !hasUpper)
            {
                return Result.Failure("Password requires an uppercase character.");
            }

            if (_requiredUniqueChars != null && _requiredUniqueChars > 0 && uniqueChars.Count < _requiredUniqueChars)
            {
                return Result.Failure($"Password requires at least");
            }

            return Result.Success();
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character
        /// is a digit - true if the character is a digit, otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is a digit.</returns>
        internal static bool IsDigit(char c) => c >= '0' && c <= '9';

        /// <summary>
        /// Returns a flag indicating whether the supplied character
        /// is lowercase - true if the character is a lowercase, otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is lowercase.</returns>
        internal static bool IsLower(char c) => c >= 'a' && c <= 'z';

        /// <summary>
        /// Returns a flag indicating whether the supplied character
        /// is uppercase - true if the character is a uppercase, otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is uppercase.</returns>
        internal static bool IsUpper(char c) => c >= 'A' && c <= 'Z';

        /// <summary>
        /// Returns a flag indicating whether the supplied character is
        /// an ASCII letter or digit - true if the character is an ASCII letter or digit,
        /// otherwise false.
        /// </summary>
        /// <param name="c">An ASCII character</param>
        /// <returns>A flag determining if <paramref name="c"/> is an ASCII letter or digit.</returns>
        internal static bool IsLetterOrDigit(char c) => IsDigit(c) || IsLower(c) || IsUpper(c);
    }
}
