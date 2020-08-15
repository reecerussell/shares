using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using Shares.Orders.Domain.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Domain.Test
{
    public class OrderTest
    {
        [Fact]
        public void TestCreate()
        {
            var dto = new CreateOrderDto
            {
                UserId = "323343",
                InstrumentId = "36492374",
                Quantity = 1000,
                Price = 24.7,
            };

            var (success, _, order, error) = Order.Create(dto);
            Assert.True(success, error);
            Assert.NotNull(order.Id);
            Assert.Equal(dto.UserId, order.UserId);
            Assert.Equal(dto.InstrumentId, order.InstrumentId);
            Assert.Equal(dto.Price, order.Price);
            Assert.Equal(dto.Quantity, order.Quantity);
        }

        [Fact]
        public void TestCreateWithNullUserId()
        {
            var dto = new CreateOrderDto
            {
                UserId = null,
                InstrumentId = "36492374",
                Quantity = 1000,
                Price = 24.7,
            };

            Assert.Throws<ArgumentNullException>(() => Order.Create(dto));
        }

        [Fact]
        public void TestCreateWithNullInstrumentId()
        {
            var dto = new CreateOrderDto
            {
                UserId = "3y94234",
                InstrumentId = null,
                Quantity = 1000,
                Price = 24.7,
            };

            Assert.Throws<ArgumentNullException>(() => Order.Create(dto));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(4.5, true)]
        [InlineData(-4.6, false)]
        [InlineData(4, true)]
        public void TestUpdateQuantity(double quantity, bool expectSuccess)
        {
            var dto = new CreateOrderDto
            {
                UserId = "323343",
                InstrumentId = "36492374",
                Quantity = 1000,
                Price = 24.7,
            };

            var (success, _, order, error) = Order.Create(dto);
            Assert.True(success, error);

            var result = order.UpdateQuantity(quantity);
            Assert.Equal(expectSuccess, result.IsSuccess);
            Assert.Equal(expectSuccess ? quantity : dto.Quantity, order.Quantity);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(4.5, true)]
        [InlineData(-4.6, false)]
        [InlineData(4, true)]
        public void TestUpdatePrice(double price, bool expectSuccess)
        {
            var dto = new CreateOrderDto
            {
                UserId = "323343",
                InstrumentId = "36492374",
                Quantity = 1000,
                Price = 24.7,
            };

            var (success, _, order, error) = Order.Create(dto);
            Assert.True(success, error);

            var result = order.UpdatePrice(price);
            Assert.Equal(expectSuccess, result.IsSuccess);
            Assert.Equal(expectSuccess ? price : dto.Price, order.Price);
        }

        [Fact]
        public void TestCreateSellOrder()
        {
            var order = CreateOrder();

            var dto = new CreateSellOrderDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Price = 25,
                Quantity = 1000
            };

            var (success, _, sellOrder, error) = order.CreateSellOrder(dto);
            Assert.True(success, error);
            Assert.Contains(sellOrder, order.SellOrders);
        }

        [Fact]
        public void TestCreateSellOrderWithNonLeftToSell()
        {
            // Creating an order, then adding a sell order to mark
            // all of its shares as sold.
            var order = CreateOrder();
            AddSellOrder(order, 1000);

            var dto = new CreateSellOrderDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Price = 25,
                Quantity = 1000
            };

            var (success, _, _, error) = order.CreateSellOrder(dto);
            Assert.False(success);
            Assert.Contains("All shares from this order have already been sold.", error);
        }

        [Fact]
        public void TestCreateSellOrderForTooMany()
        {
            // Creating an order, then adding a sell order to mark
            // all of its shares as sold.
            var order = CreateOrder();
            AddSellOrder(order, 900);

            var dto = new CreateSellOrderDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Price = 25,
                Quantity = 200
            };

            var result = order.CreateSellOrder(dto);
            Assert.False(result.IsSuccess);
        }

        private static Order CreateOrder()
        {
            var dto = new CreateOrderDto
            {
                UserId = "323343",
                InstrumentId = "36492374",
                Quantity = 1000,
                Price = 24.7,
            };

            return Order.Create(dto).Value;
        }

        private static void AddSellOrder(Order order, int quantity)
        {
            var dto = new CreateSellOrderDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Price = 23,
                Quantity = quantity
            };

            var sellOrders = new List<SellOrder>();
            sellOrders.AddRange(order.SellOrders);
            sellOrders.Add(SellOrder.Create(dto).Value);
            order.SellOrders = sellOrders;
        }
    }
}
