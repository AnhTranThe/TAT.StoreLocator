using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class MapProductWishlistConfiguration : IEntityTypeConfiguration<MapProductWishlist>
    {
        public void Configure(EntityTypeBuilder<MapProductWishlist> builder)
        {
            _ = builder.HasKey(x => new { x.ProductId, x.WishlistId });
            _ = builder.HasOne(x => x.Product).WithMany(x => x.MapProductWishlists).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
            _ = builder.HasOne(x => x.Wishlist).WithMany(x => x.MapProductWishlists).HasForeignKey(x => x.WishlistId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}