using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Core.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{

    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {

            _ = builder.HasOne(x => x.User).WithMany(x => x.Wishlists).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
