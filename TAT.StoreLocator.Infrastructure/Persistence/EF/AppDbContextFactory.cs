using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TAT.StoreLocator.Infrastructure.Persistence.EF
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("di_appsettings.json")
            .Build();
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
            string connStr = configuration.GetConnectionString("DefaultConnection");
            _ = optionsBuilder.UseNpgsql(connStr);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}