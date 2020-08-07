using CSharpFunctionalExtensions;
using Grpc.Core;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Core.Services;
using Shares.Users.Providers;
using Shares.Users.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserService = Shares.Core.Services.UserService;

namespace Shares.Users.Handlers
{
    public class UserHandler : UserService.UserServiceBase
    {
        private readonly IUserService _service;
        private readonly IUserProvider _provider;
        private readonly ILogger<UserHandler> _logger;

        public UserHandler(
            IUserService service,
            IUserProvider provider,
            ILogger<UserHandler> logger)
        {
            _service = service;
            _provider = provider;
            _logger = logger;
        }

        public override async Task<GetUserResponse> Get(GetUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to UserService.Get");

            try
            {
                var userOrNothing = await _provider.GetAsync(request.Id);
                if (userOrNothing.HasNoValue)
                {
                    return new GetUserResponse
                    {
                        Error = ErrorMessages.UserNotFound
                    };
                }

                var user = userOrNothing.Value;

                return new GetUserResponse
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new GetUserResponse{Error = e.Message};
            }
        }

        public override async Task<GetUsersResponse> List(GetUsersRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to UserService.List");

            try
            {
                var users = await _provider.GetAsync();
                var res = new GetUsersResponse();
                res.Users.AddRange(users?.Select(x => new GetUserResponse
                {
                    Id = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    Email = x.Email
                }));

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new GetUsersResponse {Error = e.Message};
            }
        }

        public override async Task<CreateUserResponse> Create(CreateUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to UserService.Create");

            try
            {
                var (success, _, id, error) = await _service.CreateAsync(new CreateUserDto(request));
                if (!success)
                {
                    return new CreateUserResponse
                    {
                        Error = error
                    };
                }

                return new CreateUserResponse
                {
                    Id = id,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new CreateUserResponse { Error = e.Message };
            }
        }

        public override async Task<UpdateUserResponse> Update(UpdateUserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to UserService.Update");

            try
            {
                var (success, _, error) = await _service.UpdateAsync(new UpdateUserDto(request));
                if (!success)
                {
                    return new UpdateUserResponse
                    {
                        Error = error
                    };
                }

                return new UpdateUserResponse();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new UpdateUserResponse{Error = e.Message};
            }
        }

        public override async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to UserService.ChangePassword");

            try
            {
                var (success, _, error) = await _service.ChangePasswordAsync(new ChangePasswordDto(request));
                if (!success)
                {
                    return new ChangePasswordResponse
                    {
                        Error = error
                    };
                }

                return new ChangePasswordResponse();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new ChangePasswordResponse { Error = e.Message };
            }
        }
    }
}
