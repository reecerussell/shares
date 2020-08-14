using CSharpFunctionalExtensions;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shares.Core.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shares.Web.Services
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly TokenService.TokenServiceClient _client;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TokenValidationService> _logger;

        public TokenValidationService(
            IConfiguration configuration,
            IMemoryCache cache,
            ILogger<TokenValidationService> logger)
        {
            var address = configuration["AuthHost"];
            var channel = new Channel(address, ChannelCredentials.Insecure);
            _client = new TokenService.TokenServiceClient(channel);
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result> ValidateAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Result.Failure("Missing authorization header in request.");
            }

            if (token.Length < 8 && token[..7] != "Bearer ")
            {
                return Result.Failure("Invalid authorization token type.");
            }

            token = token[7..];
            var tokenKey = $"ValidateToken_{token}";
            if (_cache.TryGetValue(tokenKey, out string error))
            {
                _logger.LogInformation("Using cached token result");
                _logger.LogDebug("Cache token result: {0}", error ?? "Valid.");

                return string.IsNullOrEmpty(error) ?
                    Result.Success() :
                    Result.Failure(error);
            }

            var request = new VerifyTokenRequest { Token = token };
            var response = await _client.VerifyAsync(request);
            if (!response.Ok)
            {
                CacheTokenResult(token, response.Error);
                return Result.Failure<IReadOnlyList<Claim>>(response.Error);
            }

            CacheTokenResult(token, null);
            return Result.Success();
        }

        private void CacheTokenResult(string token, string error)
        {
            _logger.LogInformation("Caching token response.");
            _logger.LogDebug("Token response: {0}", error ?? "Valid.");

            var tokenKey = $"ValidateToken_{token}";
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            _cache.Set(tokenKey, error, cacheOptions);
        }
    }
}
