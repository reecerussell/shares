using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shares.Orders.Domain.Models;

namespace Shares.Orders.Infrastructure.Configuration
{
    internal class SellOrderConfiguration : IEntityTypeConfiguration<SellOrder>
    {
        public void Configure(EntityTypeBuilder<SellOrder> builder)
        {
            builder.ToTable("sell_orders");

            builder
                .Property(x => x.Id)
                .HasColumnName("id");

            builder
                .Property(x => x.OrderId)
                .HasColumnName("order_id");

            builder
                .Property(x => x.UserId)
                .HasColumnName("user_id");

            builder
                .Property(x => x.Price)
                .HasColumnName("price");

            builder
                .Property(x => x.Quantity)
                .HasColumnName("quantity");
        }
    }
}
