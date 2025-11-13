using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Config;

namespace Repositories.EFCore
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

        public DbSet<SystemMessage> SystemMessages => Set<SystemMessage>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new SystemMessageConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());

        }
    }
}
