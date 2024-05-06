using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Infrastructure.Persistence.Configuration;

namespace TAT.StoreLocator.Infrastructure.Persistence.EF
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // identity 
            _ = builder.ApplyConfiguration(new AppRoleClaimConfiguration());
            _ = builder.ApplyConfiguration(new AppRoleConfiguration());
            _ = builder.ApplyConfiguration(new AppUserClaimConfiguration());
            _ = builder.ApplyConfiguration(new AppUserConfiguration());
            _ = builder.ApplyConfiguration(new AppUserLoginConfiguration());
            _ = builder.ApplyConfiguration(new AppUserRoleConfiguration());
            _ = builder.ApplyConfiguration(new AppUserTokenConfiguration());
            _ = builder.ApplyConfiguration(new AddressConfiguration());
            _ = builder.ApplyConfiguration(new CategoryConfiguration());
            _ = builder.ApplyConfiguration(new GalleryConfiguration());

            _ = builder.ApplyConfiguration(new MapProductWishlistConfiguration());
            _ = builder.ApplyConfiguration(new MapStoreWishlistConfiguration());
            _ = builder.ApplyConfiguration(new MapGalleryStoreConfiguration());
            _ = builder.ApplyConfiguration(new MapGalleryProductConfiguration());
            _ = builder.ApplyConfiguration(new ProductConfiguration());
            _ = builder.ApplyConfiguration(new ReviewConfiguration());

            _ = builder.ApplyConfiguration(new StoreConfiguration());
            _ = builder.ApplyConfiguration(new WishlistConfiguration());
        }


        public virtual async Task<int> SaveChangesAsync(string userId = "")
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
            {

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.Now;
                    entry.Entity.UpdatedBy = userId;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.CreatedBy = userId;
                }
            }


            return await base.SaveChangesAsync();

        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gallery> Galleries { get; set; }

        public DbSet<MapProductWishlist> MapProductWishlists { get; set; }
        public DbSet<MapStoreWishlist> mapStoreWishlists { get; set; }

        public DbSet<MapGalleryProduct> mapGalleryProducts { get; set; }
        public DbSet<MapGalleryStore> mapGalleryStores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

    }



}
