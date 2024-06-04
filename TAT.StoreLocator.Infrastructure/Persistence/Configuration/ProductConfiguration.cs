using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            _ = builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
            _ = builder.HasOne(x => x.Store).WithMany(x => x.Products).HasForeignKey(x => x.StoreId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}