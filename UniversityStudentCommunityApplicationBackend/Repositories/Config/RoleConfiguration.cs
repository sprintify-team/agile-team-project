using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "ClubManager",
                    NormalizedName = "CLUBMANAGER"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                }
            );
        }
    }
}
