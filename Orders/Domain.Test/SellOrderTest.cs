using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using Shares.Orders.Domain.Models;
using System;
using Xunit;

namespace Domain.Test
{
    public class SellOrderTest
    {
        [Fact]
        public void TestCreate()
        {
            var dto = new CreateSellOrderDto
            {
                UserId = "292323",
                OrderId = "082783",
                Price = 25,
                Quantity = 1000
            };

            var (success, _, sellOrder, error) = SellOrder.Create(dto);
            Assert.True(success, error);
            Assert.NotNull(sellOrder.Id);
            Assert.Equal(dto.UserId, sellOrder.UserId);
            Assert.Equal(dto.OrderId, sellOrder.OrderId);
            Assert.Equal(dto.Price, sellOrder.Price);
            Assert.Equal(dto.Quantity, sellOrder.Quantity);
        }

        [Fact]
        public void TestCreateWithNullUserId()
        {
            var dto = new CreateSellOrderDto
            {
                UserId = null,
                OrderId = "082783",
                Price = 25,
                Quantity = 1000
            };

            Assert.Throws<ArgumentNullException>(() => SellOrder.Create(dto));
        }

        [Fact]
        public void TestCreateWithNullOrderId()
        {
            var dto = new CreateSellOrderDto
            {
                UserId = "7234",
                OrderId = null,
                Price = 25,
                Quantity = 1000
            };

            Assert.Throws<ArgumentNullException>(() => SellOrder.Create(dto));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(4.5, true)]
        [InlineData(-4.6, false)]
        [InlineData(4, true)]
        public void TestUpdateQuantity(double quantity, bool expectSuccess)
        {
            var dto = new CreateSellOrderDto
            {
                UserId = "7234",
                OrderId = "3947",
                Price = 25,
                Quantity = 1000
            };

            var (success, _, order, error) = SellOrder.Create(dto);
            Assert.True(success, error);

            var result = order.SetQuantity(quantity);
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
            var dto = new CreateSellOrderDto
            {
                UserId = "7234",
                OrderId = "3748",
                Price = 25,
                Quantity = 1000
            };

            var (success, _, order, error) = SellOrder.Create(dto);
            Assert.True(success, error);

            var result = order.SetPrice(price);
            Assert.Equal(expectSuccess, result.IsSuccess);
            Assert.Equal(expectSuccess ? price : dto.Price, order.Price);
        }
    }
}
