using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
{
    public class GetTheNearestStoreServiceTest
    {
        [Fact] 
        public async Task GetTheNearestStore_ReturnsExpectedStores() 
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTheNearestStoreAsync_TestDatabase")
                .Options;

            using ( var dbContext = new AppDbContext(dbContextOptions) )
            {
                var address1 = new Address { Id = Guid.NewGuid().ToString(), District = "1", Province = "Ho Chi Minh" };
                var address2 = new Address { Id = Guid.NewGuid().ToString(), District = "1", Province = "Ho Chi Minh" };
                dbContext.Addresses.AddRange(address1, address2);

                var store1 = new Store { Id = Guid.NewGuid().ToString(), Name = "Store1", AddressId = address1.Id };
                var store2 = new Store { Id = Guid.NewGuid().ToString(), Name = "Store2", AddressId = address2.Id };
                dbContext.Stores.AddRange( store1, store2 );

                var product1 = new Product { Id = Guid.NewGuid().ToString(), Name = "Product1", StoreId = store1.Id };
                var product2 = new Product { Id = Guid.NewGuid().ToString(), Name = "Product2", StoreId = store2.Id };
                dbContext.Products.AddRange(product1, product2);

                var gallery1 = new Gallery { Id = Guid.NewGuid().ToString(), Url ="url1" };
                var gallery2 = new Gallery {Id = Guid.NewGuid().ToString(), Url = "url2" };
                dbContext.Galleries.AddRange(gallery1, gallery2);

                var mapGalleryProdcut1 = new MapGalleryProduct { ProductId = product1.Id, GalleryId = gallery1.Id };
                var mapGalleryProdcut2 = new MapGalleryProduct { ProductId = product2.Id, GalleryId = gallery2.Id };
                dbContext.mapGalleryProducts.AddRange(mapGalleryProdcut1, mapGalleryProdcut2);

                await dbContext.SaveChangesAsync();
            }

            using ( var dbContext = new AppDbContext(dbContextOptions))
            {
                var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<SimpleStoreResponse>(It.IsAny<Store>()))
                          .Returns((Store store) => new SimpleStoreResponse
                          {
                              Id = store.Id,
                              Name = store.Name,
                              Address = new AddressResponseModel
                              {
                                  District = store.Address?.District,
                                  Province = store.Address?.Province,
                              }
                          });
                var storeService = new StoreService(dbContext,mapperMock.Object,null,null);

                // Act
                var result = await storeService.GetTheNearestStore("1", null, null, null);

                //Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.NotEmpty(result.Data);
                Assert.Contains(result.Data, s => s.Address?.District == "1");
            }
        }
    }
}
