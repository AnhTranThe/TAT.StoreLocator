using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Domain.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class MapProductWishlistConfiguration : IEntityTypeConfiguration<MapProductWishlist>
    {
        public void Configure(EntityTypeBuilder<MapProductWishlist> builder)
        {

            _ = builder.HasOne(x => x.Product).WithMany(x => x.MapProductWishlists).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(x => x.Wishlist).WithMany(x => x.MapProductWishlists).HasForeignKey(x => x.WishlistId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
