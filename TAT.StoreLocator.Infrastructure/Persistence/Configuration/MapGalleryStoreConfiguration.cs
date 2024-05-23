using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class MapGalleryStoreConfiguration : IEntityTypeConfiguration<MapGalleryStore>
    {
        public void Configure(EntityTypeBuilder<MapGalleryStore> builder)
        {
            _ = builder.HasKey(x => new { x.StoreId, x.GalleryId });
            _ = builder.HasOne(x => x.Store).WithMany(x => x.MapGalleryStores).HasForeignKey(x => x.StoreId).OnDelete(DeleteBehavior.Cascade);
            _ = builder.HasOne(x => x.Gallery).WithMany(x => x.MapGalleryStores).HasForeignKey(x => x.GalleryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}