using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shares.Auth.Infrastructure.Tokens;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Auth.Infrastructure.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IUserProvider _userProvider;
        private readonly IPasswordService _passwordService;
        private readonly IAsymmetricHasher _hasher;
        private readonly ILogger<TokenService> _logger;

        public TokenService(
            IUserProvider userProvider,
            IPasswordService passwordService,
            IAsymmetricHasher hasher,
            ILogger<TokenService> logger)
        {
            _userProvider = userProvider;
            _passwordService = passwordService;
            _hasher = hasher;
            _logger = logger;
        }

        public async Task<Result<AccessTokenDto>> GenerateAsync(UserCredentialDto credential)
        {
            _logger.LogDebug("GenerateToken called for user '{0}'.", credential.Email);

            var userOrNothing = await _userProvider.FindByEmailAsync(credential.Email);
            if (userOrNothing.HasNoValue)
            {
                _logger.LogDebug("User with email '{0}' does not exist.", credential.Email);

                return Result.Failure<AccessTokenDto>(ErrorMessages.UserNotFound);
            }

            var user = userOrNothing.Value;

            if (!_passwordService.Verify(credential.Password, user.Hash))
            {
                _logger.LogDebug("Given password did not match user's.");

                return Result.Failure<AccessTokenDto>(ErrorMessages.UserPasswordInvalid);
            }

            var claims = new Dictionary<string, object>
            {
                {"user_id", user.Id },
            };
            var now = DateTime.UtcNow;
            var exp = now.AddHours(1);
            var builder = new TokenBuilder(_hasher.HashName)
                .AddClaims(claims)
                .SetExpiry(exp)
                .SetIssuedAt(now)
                .SetNotBefore(now);

            var (success, _, token, error) = await builder.BuildAsync(_hasher);
            if (!success)
            {
                return Result.Failure<AccessTokenDto>(error);
            }

            var accessToken = new AccessTokenDto
            {
                Token = Encoding.UTF8.GetString(token),
                Expires = (float)exp.Unix()
            };

            return accessToken;
        }

        public async Task<Result> VerifyAsync(string tokenString)
        {
            var token = new Token(Encoding.UTF8.GetBytes(tokenString));
            
            return await token.VerifyAsync(_hasher);
        }
    }
}
