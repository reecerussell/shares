using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Core.Services;
using Shares.Web.Auth;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Shares.Web.Controllers
{
    [Route("api/orders")]
    [ValidateToken]
    public class OrdersController : BaseController
    {
        private readonly OrderService.OrderServiceClient _client;
        private readonly IUser _user;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IConfiguration configuration,
            IUser user,
            ILogger<OrdersController> logger)
        {
            var address = configuration["OrdersHost"];
            var channel = new Channel(address, ChannelCredentials.Insecure);
            _client = new OrderService.OrderServiceClient(channel);
            _user = user;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            _logger.LogInformation("API request received to create an order.");

            try
            {
                var request = new CreateOrderRequest
                {
                    InstrumentId = dto.InstrumentId,
                    UserId = _user.Id,
                    Price = (float) dto.Price,
                    Quantity = (float) dto.Quantity,
                };
                var response = await _client.CreateAsync(request);
                if (!string.IsNullOrEmpty(response.Error))
                {
                    return HandleBadRequest(response.Error);
                }

                return HandleOk(response.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return HandleBadGateway(e.Message);
            }
        }


        [HttpDelete("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Create(string id)
        {
            _logger.LogInformation("API request received to delete order '{0}'.", id);

            try
            {
                var request = new DeleteOrderRequest{Id = id};
                var response = await _client.DeleteAsync(request);
                if (!string.IsNullOrEmpty(response.Error))
                {
                    if (response.Error == ErrorMessages.OrderNotFound)
                    {
                        return HandleNotFound(response.Error);
                    }

                    return HandleBadRequest(response.Error);
                }

                return HandleOk();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return HandleBadGateway(e.Message);
            }
        }

        [HttpPost("{orderId}/sell")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SuccessfulResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromRoute]string orderId, [FromBody]CreateSellOrderDto dto)
        {
            _logger.LogInformation("API request received to sell an order.");

            try
            {
                var request = new SellOrderRequest()
                {
                    OrderId = orderId,
                    UserId = _user.Id,
                    Price = (float)dto.Price,
                    Quantity = (float)dto.Quantity,
                };
                var response = await _client.SellAsync(request);
                if (!string.IsNullOrEmpty(response.Error))
                {
                    if (response.Error == ErrorMessages.OrderNotFound)
                    {
                        return HandleNotFound(response.Error);
                    }

                    return HandleBadRequest(response.Error);
                }

                return HandleOk(response.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return HandleBadGateway(e.Message);
            }
        }
    }
}
