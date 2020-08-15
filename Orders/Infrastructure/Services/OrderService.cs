using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Shares.Core;
using Shares.Core.Dtos;
using Shares.Orders.Domain.Models;
using Shares.Orders.Infrastructure.Repositories;

namespace Shares.Orders.Infrastructure.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInstrumentReadRepository _instrumentRepository;
        private readonly IUserReadRepository _userRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IInstrumentReadRepository instrumentRepository,
            IUserReadRepository userRepository)
        {
            _orderRepository = orderRepository;
            _instrumentRepository = instrumentRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<string>> CreateAsync(CreateOrderDto dto)
        {
            if (!await _userRepository.ExistsAsync(dto.UserId))
            {
                return Result.Failure<string>("User was not found.");
            }

            if (!await _instrumentRepository.ExistsAsync(dto.InstrumentId))
            {
                return Result.Failure<string>("Instrument was not found.");
            }

            return await Order.Create(dto)
                .Tap(_orderRepository.Add)
                .Tap(_ => _orderRepository.SaveChangesAsync())
                .Map(o => o.Id);
        }

        public async Task<Result> DeleteAsync(string id)
        {
            return await _orderRepository.FindByIdAsync(id)
                .ToResult(ErrorMessages.OrderNotFound)
                .Tap(_orderRepository.Remove)
                .Bind(_ => _orderRepository.SaveChangesAsync());
        }

        public async Task<Result<string>> SellAsync(CreateSellOrderDto dto)
        {
            return await _orderRepository.FindByIdAsync(dto.OrderId)
                .ToResult(ErrorMessages.OrderNotFound)
                .Ensure(o => _userRepository.ExistsAsync(dto.UserId), "User was not found.")
                .Bind(o => o.CreateSellOrder(dto))
                .Tap(_ => _orderRepository.SaveChangesAsync())
                .Map(so => so.Id);
        }
    }
}
