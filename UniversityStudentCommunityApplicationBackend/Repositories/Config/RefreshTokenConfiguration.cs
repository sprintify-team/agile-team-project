using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(r => r.Expires)
                   .IsRequired();

            builder.Property(r => r.IsRevoked)
                   .IsRequired();

            builder.HasIndex(r => r.Token)
                   .IsUnique();

            builder.HasOne(r => r.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
