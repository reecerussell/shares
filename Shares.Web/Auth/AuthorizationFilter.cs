using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shares.Web.Controllers;
using Shares.Web.Services;

namespace Shares.Web.Auth
{
    public class AuthorizationFilter : IAsyncActionFilter
    {
        private readonly ITokenValidationService _service;
        private readonly ILogger<AuthorizationFilter> _logger;

        public AuthorizationFilter(
            ITokenValidationService service,
            ILogger<AuthorizationFilter> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Evaluating authorization token.");

            var tokenHeader = context.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key.Equals("authorization", StringComparison.OrdinalIgnoreCase)).Value
                .FirstOrDefault();

            var (success, _, error) = await _service.ValidateAsync(tokenHeader);
            if (!success)
            {
                _logger.LogInformation("Token is invalid: {0}", error);

                var response = new BaseController.ErrorResponse
                {
                    Message = error,
                    StatusCode = (int) HttpStatusCode.Unauthorized,
                };
                context.Result = new UnauthorizedObjectResult(response);
                
                return;
            }

            _logger.LogInformation("Token is valid, moving on.");
            await next.Invoke();
        }
    }
}
