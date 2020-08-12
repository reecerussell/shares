using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Shares.Web.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected virtual IActionResult HandleOk()
        {
            return Ok(new SuccessfulResponse<object> {Data = null});
        }

        protected virtual IActionResult HandleOk<T>(T data)
        {
            return Ok(new SuccessfulResponse<T> {Data = data});
        }

        protected virtual IActionResult HandleNotFound(string message)
        {
            return NotFound(new ErrorResponse
            {
                Message = message,
                StatusCode = (int) HttpStatusCode.NotFound
            });
        }

        protected virtual IActionResult HandleBadRequest(string message)
        {
            return BadRequest(new ErrorResponse
            {
                Message = message,
                StatusCode = (int)HttpStatusCode.BadRequest
            });
        }

        protected virtual IActionResult HandleBadGateway(string message)
        {
            var data = new ErrorResponse
            {
                Message = message,
                StatusCode = (int) HttpStatusCode.BadGateway
            };

            return new ObjectResult(data)
            {
                StatusCode = (int)HttpStatusCode.BadGateway,
            };
        }

        protected virtual IActionResult HandleInternalServerError(string message)
        {
            var data = new ErrorResponse
            {
                Message = message,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            return new ObjectResult(data)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        public class SuccessfulResponse<T>
        {
            public T Data { get; set; }
        }

        public class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }
    }
}
