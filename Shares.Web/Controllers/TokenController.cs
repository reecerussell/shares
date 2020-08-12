using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Core.Services;
using System;
using System.Threading.Tasks;

namespace Shares.Web.Controllers
{
    [Route("api/token")]
    public class TokenController : BaseController
    {
        private readonly TokenService.TokenServiceClient _client;

        public TokenController(IConfiguration configuration)
        {
            var address = configuration["AuthHost"];
            var channel = new Channel(address, ChannelCredentials.Insecure);
            _client = new TokenService.TokenServiceClient(channel);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> CreateToken(UserCredentialDto dto)
        {
            try
            {
                var request = new GetTokenRequest
                {
                    Email = dto.Email,
                    Password = dto.Password,
                };

                var response = await _client.GetAsync(request);
                if (!string.IsNullOrEmpty(response.Error))
                {
                    switch (response.Error)
                    {
                        case ErrorMessages.UserNotFound:
                        case ErrorMessages.UserPasswordInvalid:
                            return HandleBadRequest("Email and/or password is invalid.");
                        default:
                            return HandleInternalServerError(response.Error);
                    }
                }

                return HandleOk(response.Token);
            }
            catch (Exception e)
            {
                return HandleBadGateway(e.Message);
            }
        }
    }
}
