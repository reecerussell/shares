using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shares.Core.Dtos;
using Shares.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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
            SellOrders = new List<SellOrder>();
        }

        private List<SellOrder> _sellOrders;

        public IReadOnlyList<SellOrder> SellOrders
        {
            get => _lazyLoader == null ? 
                _sellOrders : 
                _lazyLoader.Load(this, ref _sellOrders);
            internal set => _sellOrders = (List<SellOrder>) value;
        }

        private readonly ILazyLoader _lazyLoader;

        private Order(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
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

        public Result<SellOrder> CreateSellOrder(CreateSellOrderDto dto)
        {
            var quantitySold = SellOrders.Sum(x => x.Quantity);
            var availableToSell = Quantity - quantitySold;
            if (availableToSell <= 0)
            {
                return Result.Failure<SellOrder>("All shares from this order have already been sold.");
            }

            if (availableToSell - dto.Quantity < 0)
            {
                return Result.Failure<SellOrder>($"There are only {availableToSell} shares available to sell.");
            }

            return SellOrder.Create(dto)
                .Tap(_sellOrders.Add);
        }
    }
}
