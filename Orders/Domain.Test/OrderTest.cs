using CSharpFunctionalExtensions;
using Shares.Core.Dtos;
using Shares.Orders.Domain.Models;
using System;
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

            var result = order.UpdateQuantity(price);
            Assert.Equal(expectSuccess, result.IsSuccess);
            Assert.Equal(expectSuccess ? price : dto.Price, order.Price);
        }
    }
}
