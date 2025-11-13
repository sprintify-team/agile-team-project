using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers"); // Identity varsayılan tablo ismiyle aynı kalır

            // FirstName & LastName
            builder.Property(u => u.FirstName)
                   .HasMaxLength(100)
                   .IsRequired(false); // nullable

            builder.Property(u => u.LastName)
                   .HasMaxLength(100)
                   .IsRequired(false); // nullable

            // Email & Username benzersiz olmalı
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.UserName).IsUnique();

            // RefreshToken ilişkisi
            builder.HasMany(u => u.RefreshTokens)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
