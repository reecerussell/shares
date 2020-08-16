using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Core.Services;
using Shares.Web.Auth;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shares.Web.Controllers
{
    [Route("api/users")]
    [ValidateToken]
    public class UsersController : BaseController
    {
        private readonly UserService.UserServiceClient _client;

        public UsersController(IConfiguration configuration)
        {
            var address = configuration["UsersHost"];
            var channel = new Channel(address, ChannelCredentials.Insecure);
            _client = new UserService.UserServiceClient(channel);
        }

        [HttpGet("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadGateway)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _client.GetAsync(new GetUserRequest{Id = id});
            if (!string.IsNullOrEmpty(response.Error))
            {
                if (response.Error == ErrorMessages.UserNotFound)
                {
                    return HandleNotFound(response.Error);
                }

                return HandleBadGateway(response.Error);
            }

            return HandleOk(new UserDto
            {
                Id = response.Id,
                Firstname = response.Firstname,
                Lastname = response.Lastname,
                Email = response.Email
            });
        }

        [HttpGet]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<UserDto[]>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadGateway)]
        public async Task<IActionResult> Get()
        {
            var response = await _client.ListAsync(new GetUsersRequest());
            if (!string.IsNullOrEmpty(response.Error))
            {
                Console.WriteLine("[ERROR]: " + response.Error);
                return HandleBadGateway(response.Error);
            }

            var users = response.Users.Select(x => new UserDto
            {
                Id = x.Id,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Email = x.Email
            }).ToList();

            return HandleOk(users);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            try
            {
                var response = await _client.CreateAsync(dto);
                if (!string.IsNullOrEmpty(response.Error))
                {
                    return HandleBadRequest(response.Error);
                }

                return HandleOk(response.Id);
            }
            catch (Exception e)
            {
                return HandleBadGateway(e.Message);
            }
        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdateUserDto dto)
        {
            try
            {
                var response = await _client.UpdateAsync(dto);
                if (!string.IsNullOrEmpty(response.Error))
                {
                    if (response.Error == ErrorMessages.UserNotFound)
                    {
                        return HandleNotFound(response.Error);
                    }

                    return HandleBadRequest(response.Error);
                }

                return HandleOk();
            }
            catch (Exception e)
            {
                return HandleBadGateway(e.Message);
            }
        }

        [HttpPost("changePassword")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                var response = await _client.ChangePasswordAsync(dto);
                if (!string.IsNullOrEmpty(response.Error))
                {
                    if (response.Error == ErrorMessages.UserNotFound)
                    {
                        return HandleNotFound(response.Error);
                    }

                    return HandleBadRequest(response.Error);
                }

                return HandleOk();
            }
            catch (Exception e)
            {
                return HandleBadGateway(e.Message);
            }
        }
    }
}
