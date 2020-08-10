using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shares.Core.Entity
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Aggregate
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id");
        }
    }
}
