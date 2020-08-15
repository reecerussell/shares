using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using System;

namespace Shares.Orders.Domain.Models
{
    public class SellOrder
    {
        public string Id { get; protected set; }
        public string UserId { get; protected set; }
        public string OrderId { get; protected set; }
        public double Quantity { get; private set; }
        public double Price { get; private set; }

        private SellOrder(string userId, string orderId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrEmpty(orderId))
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            Id = Guid.NewGuid().ToString();
            UserId = userId;
            OrderId = orderId;
        }

        internal Result SetQuantity(double quantity)
        {
            if (quantity <= 0)
            {
                return Result.Failure("Quantity must be greater than zero.");
            }

            Quantity = quantity;

            return Result.Success();
        }

        internal Result SetPrice(double price)
        {
            if (price <= 0)
            {
                return Result.Failure("Price must be greater than zero.");
            }

            Price = price;

            return Result.Success();
        }

        public static Result<SellOrder> Create(CreateSellOrderDto dto)
        {
            var sell = new SellOrder(dto.UserId, dto.OrderId);
            return sell.SetQuantity(dto.Quantity)
                .Bind(() => sell.SetPrice(dto.Price))
                .Map(() => sell);
        }
    }
}
