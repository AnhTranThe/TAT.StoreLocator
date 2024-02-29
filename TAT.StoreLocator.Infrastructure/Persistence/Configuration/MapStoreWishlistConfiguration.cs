﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAT.StoreLocator.Domain.Entities;

namespace TAT.StoreLocator.Infrastructure.Persistence.Configuration
{
    public class MapStoreWishlistConfiguration : IEntityTypeConfiguration<MapStoreWishlist>
    {
        public void Configure(EntityTypeBuilder<MapStoreWishlist> builder)
        {

            _ = builder.HasOne(x => x.Store).WithMany(x => x.MapStoreWishlists).HasForeignKey(x => x.StoreId).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(x => x.Wishlist).WithMany(x => x.MapStoreWishlists).HasForeignKey(x => x.WishlistId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
