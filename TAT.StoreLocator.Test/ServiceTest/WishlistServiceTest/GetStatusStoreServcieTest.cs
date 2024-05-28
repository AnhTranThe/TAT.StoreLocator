using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.WishList;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.WishlistServiceTest
{
    public class GetStatusStoreServcieTest
    {
        private static Wishlist GetSampleWishlist()
        {
            return new Wishlist { UserId = "user1", MapStoreWishlists = GetSampleMapStoreWishlists() };
        }

        private static List<MapStoreWishlist> GetSampleMapStoreWishlists()
        {
            return new List<MapStoreWishlist>
            {
                new MapStoreWishlist { WishlistId = "1", StoreId = "store1" },
                new MapStoreWishlist { WishlistId = "1", StoreId = "store2" }
            };
        }


        [Fact]
        public async Task GetStatusStore_ReturnsNotFoundResult()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetStatusStore_NotFound_TestDatabase")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            dbContext.Wishlist.Add(GetSampleWishlist());
            await dbContext.SaveChangesAsync();

            var wishlistService = new WishlistService(dbContext);
            var request = new WishListRequestStore { UserId = "user1", StoreId = "nonexistent_store" };

            // Act
            var result = await wishlistService.GetStatusStore(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success, $"Expected failure but got {result.Message}");
            Assert.False(result.Data, "Expected store not to exist in wishlist.");
        }
    }
}
