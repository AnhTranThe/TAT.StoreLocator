using Xunit;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Models.Request.WishList;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Test.ServiceTest.WishlistServiceTest
{
    public class GetStatusProductServiceTest
    {
        [Fact]
        public async Task GetStatusProduct_ReturnsCorrectStatus()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetStatusProduct_ReturnsCorrectStatus_TestDatabase")
                .Options;

            using var dbContextMock = new AppDbContext(dbContextOptions);
            dbContextMock.Wishlist.Add(new Wishlist
            {
                UserId = "user1",
                MapProductWishlists = new List<MapProductWishlist>
                {
                    new MapProductWishlist { ProductId = "product1" }
                }
            });
            await dbContextMock.SaveChangesAsync(); // Wait for the data to be saved

            var wishlistService = new WishlistService(dbContextMock);

            // Act - product exists in the wishlist
            var requestProductExists = new WishListRequestProduct
            {
                UserId = "user1",
                ProductId = "product1"
            };
            var resultProductExists = await wishlistService.GetStatusProduct(requestProductExists);

            // Act - product does not exist in the wishlist
            var requestProductNotExists = new WishListRequestProduct
            {
                UserId = "user1",
                ProductId = "product2"
            };
            var resultProductNotExists = await wishlistService.GetStatusProduct(requestProductNotExists);

            // Act - wishlist does not exist
            var requestWishlistNotExists = new WishListRequestProduct
            {
                UserId = "user2",
                ProductId = "product1"
            };
            var resultWishlistNotExists = await wishlistService.GetStatusProduct(requestWishlistNotExists);

            // Assert - product exists in the wishlist
            Assert.True(resultProductExists.Success);
            Assert.True(resultProductExists.Data);
            Assert.Equal("Product exist in wishlist.", resultProductExists.Message);

            // Assert - product does not exist in the wishlist
            Assert.True(resultProductNotExists.Success);
            Assert.False(resultProductNotExists.Data);
            Assert.Equal("Product not found in wishlist.", resultProductNotExists.Message);

            // Assert - wishlist does not exist
            Assert.False(resultWishlistNotExists.Success);
            Assert.False(resultWishlistNotExists.Data);
            Assert.Equal("Wishlist not found.", resultWishlistNotExists.Message);
        }
    }
}