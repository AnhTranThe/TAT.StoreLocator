using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class MapProductGalleryConfiguration : IEntityTypeConfiguration<MapProductGallery>
    {
        public void Configure(EntityTypeBuilder<MapProductGallery> builder)
        {

            _ = builder.HasOne(x => x.Product).WithMany(x => x.MapProductGalleries).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(x => x.Gallery).WithMany(x => x.MapProductGalleries).HasForeignKey(x => x.GalleryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
