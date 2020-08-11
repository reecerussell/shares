using CSharpFunctionalExtensions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Shares.Auth.Domain.Dtos;
using Shares.Auth.Infrastructure;
using Shares.Core.Services;
using System;
using System.Threading.Tasks;
using AccessToken = Shares.Core.Services.AccessToken;

namespace API.Handlers
{
    public class TokenHandler : TokenService.TokenServiceBase
    {
        private readonly ITokenService _service;
        private readonly ILogger<TokenHandler> _logger;

        public TokenHandler(
            ITokenService service,
            ILogger<TokenHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public override async Task<GetTokenResponse> Get(GetTokenRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to TokenService.Get");

            try
            {
                var credential = new UserCredential
                {
                    Email = request.Email,
                    Password = request.Password
                };
                var (success, _, token, error) = await _service.GenerateAsync(credential);
                if (!success)
                {
                    return new GetTokenResponse{Error = error};
                }

                var responseToken = new AccessToken
                {   
                    Token = token.Token,
                    Expires = token.Expires
                };
                return new GetTokenResponse{Token = responseToken};
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new GetTokenResponse{Error = e.Message};
            }
        }

        public override async Task<VerifyTokenResponse> Verify(VerifyTokenRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to TokenService.Verify");

            try
            {
                var (success, _, error) = await _service.VerifyAsync(request.Token);
                if (!success)
                {
                    return new VerifyTokenResponse{Error = error, Ok = false};
                }

                return new VerifyTokenResponse{Ok = true};
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new VerifyTokenResponse{Ok = false, Error = e.Message};
            }
        }
    }
}
