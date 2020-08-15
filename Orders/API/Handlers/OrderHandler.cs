using CSharpFunctionalExtensions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Shares.Core.Dtos;
using Shares.Core.Services;
using Shares.Orders.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace Shares.Orders.API.Handlers
{
    public class OrderHandler : OrderService.OrderServiceBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderHandler> _logger;

        public OrderHandler(
            IOrderService service,
            ILogger<OrderHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public override async Task<CreateOrderResponse> Create(CreateOrderRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received incoming RPC to OrderService.Create");

            try
            {
                var dto = new CreateOrderDto
                {
                    UserId = request.UserId,
                    InstrumentId = request.InstrumentId,
                    Price = request.Price,
                    Quantity = request.Quantity
                };
                var (success, _, id, error) = await _service.CreateAsync(dto);
                if (!success)
                {
                    return new CreateOrderResponse{Error = error};
                }

                return new CreateOrderResponse {Id = id};
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new CreateOrderResponse{Error = e.Message};
            }
        }
    }
}
