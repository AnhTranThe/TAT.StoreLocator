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
            ///////////////////////////////
            _ = builder.ApplyConfiguration(new AddressConfiguration());
            _ = builder.ApplyConfiguration(new CategoryConfiguration());
            _ = builder.ApplyConfiguration(new GalleryConfiguration());
            _ = builder.ApplyConfiguration(new LocationConfiguration());
            _ = builder.ApplyConfiguration(new MapProductGalleryConfiguration());
            _ = builder.ApplyConfiguration(new MapProductWishlistConfiguration());
            _ = builder.ApplyConfiguration(new MapStoreWishlistConfiguration());
            _ = builder.ApplyConfiguration(new ProductConfiguration());
            _ = builder.ApplyConfiguration(new ReviewConfiguration());
            _ = builder.ApplyConfiguration(new ScheduleConfiguration());
            _ = builder.ApplyConfiguration(new StoreConfiguration());
            _ = builder.ApplyConfiguration(new WishlistConfiguration());
        }
        public virtual Task<int> SaveChangesAsync(string username = "")
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
            {

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = username;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = username;
                }
            }
            return base.SaveChangesAsync();
        }


        public DbSet<Address>? Addresses { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Gallery>? Galleries { get; set; }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<MapProductGallery>? MapProductGalleries { get; set; }
        public DbSet<MapProductWishlist>? MapProductWishlists { get; set; }
        public DbSet<MapStoreWishlist>? mapStoreWishlists { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<Schedule>? Schedules { get; set; }
        public DbSet<Store>? Stores { get; set; }
        public DbSet<Wishlist>? Wishlists { get; set; }


    }



}
