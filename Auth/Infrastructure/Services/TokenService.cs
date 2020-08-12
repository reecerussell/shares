using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shares.Auth.Infrastructure.Tokens;
using Shares.Core;
using Shares.Core.Dtos;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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

            var (success, _, token, error) = await CreateTokenAsync(claims);
            if (!success)
            {
                return Result.Failure<AccessTokenDto>(error);
            }

            var accessToken = new AccessTokenDto { Token = Encoding.UTF8.GetString(token)};

            return accessToken;
        }

        private async Task<Result<byte[]>> CreateTokenAsync(IDictionary<string, object> claims)
        {
            var headerPayload = new TokenHeader("RSA256", "JWT");
            var headerData = await SerializeAsync(headerPayload);
            var header = new byte[Base64.GetMaxEncodedToUtf8Length(headerData.Length)];
            Base64.EncodeToUtf8(headerData, header, out _, out _);
            var payload = await SerializeAsync(claims);
            var length = header.Length + 1 + Base64.GetMaxEncodedToUtf8Length(payload.Length);
            var keySize = await _hasher.GetPublicKeySizeAsync();
            var token = new byte[length + 1 + Base64.GetMaxEncodedToUtf8Length(keySize)];

            Buffer.BlockCopy(header, 0, token, 0, header.Length);
            var idx = header.Length;
            token[idx] = (byte)'.';
            idx++;
            Encode(payload, token, idx);

            var digest = SHA256.Create();
            var digestSum = digest.ComputeHash(token, 0, length);
            var (signSuccess, _, signature, signError) = await _hasher.SignAsync(digestSum);
            if (!signSuccess)
            {
                return Result.Failure<byte[]>(signError);
            }

            idx = length;
            token[idx] = (byte) '.';
            Encode(signature, token, idx + 1);

            return token;

            static async Task<byte[]> SerializeAsync(object value)
            {
                await using var ms = new MemoryStream();
                await JsonSerializer.SerializeAsync(ms, value);
                return ms.ToArray();
            }

            static void Encode(byte[] buf, byte[] dst, int dstOffset)
            {
                var data = new byte[Base64.GetMaxEncodedToUtf8Length(buf.Length)];
                Base64.EncodeToUtf8(buf, data, out _, out _);
                Buffer.BlockCopy(data, 0, dst, dstOffset, data.Length);
            }
        }

        public async Task<Result> VerifyAsync(string tokenString)
        {
            //var rawToken = Encoding.UTF8.GetBytes(tokenString);
            //var token = new Token(rawToken);

            //// TODO: error handling
            //var (data, signature) = token.Scan();
            //var algorithm = HashAlgorithm.Create(_hasher.HashName);
            //if (algorithm == null)
            //{
            //    throw new InvalidOperationException($"No hash algorithm was found with name '{_hasher.HashName}'.");
            //}

            //var digest = algorithm.ComputeHash(data);
            //var (success, _, error) = await _hasher.VerifyAsync(digest, signature);
            //if (!success)
            //{
            //    return Result.Failure(error);
            //}

            //if (!token.IsTimeValid())
            //{
            //    return Result.Failure("Token is not yet valid.");
            //}

            //if (token.HasExpired())
            //{
            //    return Result.Failure("Token has expired.");
            //}

            return Result.Success();
        }
    }
}
