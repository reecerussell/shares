using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shares.Core.Entity;
using Shares.Orders.Domain.Models;

namespace Shares.Orders.Infrastructure.Configuration
{
    internal class InstrumentConfiguration : BaseConfiguration<Instrument>
    {
        public override void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.ToTable("instruments");

            base.Configure(builder);
        }
    }
}
