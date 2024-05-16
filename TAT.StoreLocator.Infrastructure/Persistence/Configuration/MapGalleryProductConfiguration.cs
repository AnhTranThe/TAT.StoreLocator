using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class MapGalleryProductConfiguration : IEntityTypeConfiguration<MapGalleryProduct>
    {
        public void Configure(EntityTypeBuilder<MapGalleryProduct> builder)
        {
            _ = builder.HasKey(x => new { x.ProductId, x.GalleryId });
            _ = builder.HasOne(x => x.Product).WithMany(x => x.MapGalleryProducts).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
            _ = builder.HasOne(x => x.Gallery).WithMany(x => x.MapGalleryProducts).HasForeignKey(x => x.GalleryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
