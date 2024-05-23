using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            _ = builder.HasOne(x => x.User).WithOne(x => x.Wishlist).HasForeignKey<Wishlist>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}