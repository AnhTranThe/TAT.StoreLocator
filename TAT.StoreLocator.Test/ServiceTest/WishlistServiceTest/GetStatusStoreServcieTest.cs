//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TAT.StoreLocator.Core.Entities;
//using TAT.StoreLocator.Core.Models.Request.WishList;
//using TAT.StoreLocator.Infrastructure.Persistence.EF;
//using TAT.StoreLocator.Infrastructure.Services;
//using Xunit;

//namespace TAT.StoreLocator.Test.ServiceTest.WishlistServiceTest
//{
//    public class GetStatusStoreServcieTest
//    {
//        [Fact]
//        public async Task GetStatusStore_ReturnsCorrectStatus()
//        {
//            // Arrange
//            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetStatusStore_ReturnsCorrectStatus_TestDatabase")
//                .Options;

//            using var dbContextMock = new AppDbContext(dbContextOptions);
//            dbContextMock.Wishlist.Add(new Wishlist
//            {
//                UserId = "user1",
//                MapStoreWishlists = new List<MapStoreWishlist>
//                {
//                    new MapStoreWishlist { StoreId = "store1" }
//                }
//            });
//            await dbContextMock.SaveChangesAsync(); // Wait for the data to be saved

//            var wishlistService = new WishlistService(dbContextMock);

//            // Act - store exists in the wishlist
//            var requestStoreExists = new WishListRequestStore
//            {
//                UserId = "user1",
//                StoreId = "store1"
//            };
//            var resultStoreExists = await wishlistService.GetStatusStore(requestStoreExists);

//            // Act - store does not exist in the wishlist
//            var requestStoreNotExists = new WishListRequestStore
//            {
//                UserId = "user1",
//                StoreId = "store2"
//            };
//            var resultStoreNotExists = await wishlistService.GetStatusStore(requestStoreNotExists);

//            // Act - wishlist does not exist
//            var requestWishlistNotExists = new WishListRequestStore
//            {
//                UserId = "user2",
//                StoreId = "store1"
//            };
//            var resultWishlistNotExists = await wishlistService.GetStatusStore(requestWishlistNotExists);

//            // Assert - store exists in the wishlist
//            Assert.True(resultStoreExists.Success);
//            Assert.True(resultStoreExists.Data);
//            Assert.Equal("Store exist in wishlist.", resultStoreExists.Message);

//            // Assert - store does not exist in the wishlist
//            Assert.True(resultStoreNotExists.Success);
//            Assert.False(resultStoreNotExists.Data);
//            Assert.Equal("Store not found in wishlist.", resultStoreNotExists.Message);

//            // Assert - wishlist does not exist
//            Assert.False(resultWishlistNotExists.Success);
//            Assert.False(resultWishlistNotExists.Data);
//            Assert.Equal("Wishlist not found.", resultWishlistNotExists.Message);
//        }
//    }
//}


