using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shares.Auth.Domain.Models;
using Shares.Core.Entity;

namespace Shares.Auth.Infrastructure.Configuration
{
    internal class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("users");

            builder
                .Property(x => x.NormalizedEmail)
                .HasColumnName("normalized_email");

            builder
                .Property(x => x.Hash)
                .HasColumnName("password_hash");
        }
    }
}
