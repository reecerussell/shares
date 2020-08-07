using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shares.Users.Models;

namespace Shares.Users.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder
                .Property(x => x.Id)
                .HasColumnName("id");

            builder
                .OwnsOne(x => x.Name)
                .Property(x => x.Firstname)
                .HasColumnName("first_name");

            builder
                .OwnsOne(x => x.Name)
                .Property(x => x.Lastname)
                .HasColumnName("last_name");

            builder
                .OwnsOne(x => x.Name)
                .Ignore(x => x.Fullname);

            builder
                .Property(x => x.Email)
                .HasColumnName("email");

            builder
                .Property(x => x.NormalizedEmail)
                .HasColumnName("normalized_email");

            builder
                .Property(x => x.PasswordHash)
                .HasColumnName("password_hash");
        }
    }
}
