using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using Shares.Core.Entity;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Domain.Test")]
namespace Shares.Orders.Domain.Models
{
    public class Order : Aggregate
    {
        public string InstrumentId { get; protected set; }
        public string UserId { get; protected set; }
        public double Quantity { get; private set; }
        public double Price { get; private set; }

        private Order(string instrumentId, string userId)
        {
            if (string.IsNullOrEmpty(instrumentId))
            {
                throw new ArgumentNullException(nameof(instrumentId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            InstrumentId = instrumentId;
            UserId = userId;
        }

        private Order()
        {
        }

        internal Result UpdateQuantity(double quantity)
        {
            if (quantity <= 0)
            {
                return Result.Failure("Quantity must be greater than zero.");
            }

            Quantity = quantity;

            return Result.Success();
        }

        internal Result UpdatePrice(double price)
        {
            if (price <= 0)
            {
                return Result.Failure("Price must be greater than zero.");
            }

            Price = price;

            return Result.Success();
        }

        public static Result<Order> Create(CreateOrderDto dto)
        {
            var order = new Order(dto.InstrumentId, dto.UserId);
            return order.UpdateQuantity(dto.Quantity)
                .Bind(() => order.UpdatePrice(dto.Price))
                .Map(() => order);
        }
    }
}
