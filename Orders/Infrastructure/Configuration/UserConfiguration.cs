using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shares.Core.Entity;
using Shares.Orders.Domain.Models;

namespace Shares.Orders.Infrastructure.Configuration
{
    internal class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            base.Configure(builder);
        }
    }
}
