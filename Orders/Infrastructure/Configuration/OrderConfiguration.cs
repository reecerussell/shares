using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shares.Core.Entity;
using Shares.Orders.Domain.Models;

namespace Shares.Orders.Infrastructure.Configuration
{
    internal class OrderConfiguration : BaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder
                .Property(x => x.InstrumentId)
                .HasColumnName("instrument_id");

            builder
                .Property(x => x.UserId)
                .HasColumnName("user_id");

            builder
                .Property(x => x.Price)
                .HasColumnName("price");

            builder
                .Property(x => x.Quantity)
                .HasColumnName("quantity");

            builder
                .HasMany(x => x.SellOrders)
                .WithOne()
                .HasForeignKey(x => x.OrderId);

            base.Configure(builder);
        }
    }
}
