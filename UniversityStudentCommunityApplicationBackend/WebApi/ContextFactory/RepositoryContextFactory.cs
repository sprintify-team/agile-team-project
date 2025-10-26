using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

namespace WebApi.ContextFactory
{
    // Bu sınıf, Add-Migration gibi tasarım zamanı (design-time) komutları için
    // RepositoryContext'in nasıl oluşturulacağını EF Core araçlarına söyler.
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            // 1. appsettings.json dosyasını oku
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 2. DbContextOptionsBuilder'ı oluştur
            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    // 3. EN ÖNEMLİ KISIM: Migration'ların hangi projede olacağını belirt
                    // "MiniShop.WebApi" -> Bu projenin Assembly adıdır.
                    opt => opt.MigrationsAssembly("WebApi")
                );

            // 4. Yeni RepositoryContext örneğini döndür
            return new RepositoryContext(optionsBuilder.Options);
        }
    }

}
