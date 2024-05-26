//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TAT.StoreLocator.Core.Common;
//using TAT.StoreLocator.Core.Entities;
//using TAT.StoreLocator.Core.Models.Response.Store;
//using TAT.StoreLocator.Infrastructure.Persistence.EF;
//using TAT.StoreLocator.Infrastructure.Services;
//using Xunit;

//namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
//{
//    public class GetAllStoreServiceTest
//    {
//        [Fact]
//        public async Task GetAllStore_ReturnsExpectedStores()
//        {
//            // Arrange
//            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetAllStore_TestDatabase")
//                .Options;

//            var paginationRequest = new BasePaginationRequest
//            {
//                PageIndex = 1,
//                PageSize = 10,
//                SearchString = "Test"
//            };

//            using (var dbContext = new AppDbContext(dbContextOptions))
//            {
//                var stores = new List<Store>
//                {
//                    new Store { Id = Guid.NewGuid().ToString(), Name = "Store1", IsActive = true, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
//                    new Store { Id = Guid.NewGuid().ToString(), Name = "Store2", IsActive = true, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow }
//                };

//                dbContext.Stores.AddRange();
//                await dbContext.SaveChangesAsync();
//            }

//            using (var dbContext = new AppDbContext(dbContextOptions))
//            {
//                var mapperMock = new Mock<IMapper>();
//                mapperMock.Setup(m => m.Map<List<StoreResponseModel>>(It.IsAny<List<Store>>()))
//                    .Returns((List<Store> stores) => stores.Select(store => new StoreResponseModel
//                    {
//                        Id = store.Id,
//                        Name = store.Name,
//                        IsActive = store.IsActive,
//                        CreatedAt = store.CreatedAt,
//                        UpdatedAt = store.UpdatedAt
//                    }).ToList());

//                var storeService = new StoreService(dbContext, mapperMock.Object, null, null);

//                // Act
//                var result = await storeService.GetAllStoreAsync(paginationRequest);

//                //Assert
//                Assert.NotNull(result);
//                Assert.True(result.Success);
//                Assert.NotNull(result.Data);
//                Assert.Equal(2, result.Data.Count); // Adjust the count based on the number of stores you've mocked
//            }
//        }
//    }
//}


////test 
//using Xunit;
//using Moq;
//using TAT.StoreLocator.Infrastructure.Services;
//using TAT.StoreLocator.Core.Entities;
//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;
//using TAT.StoreLocator.Core.Common;
//using TAT.StoreLocator.Infrastructure.Persistence.EF;
//using Microsoft.EntityFrameworkCore;
//using System.Xml.Linq;

//namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
//{
//    public class GetAllStoreServiceTest
//    {
//        [Fact]  
//        public async Task GetAllStore_Returns_Valid_Result()
//        {
//            //Arrange
//            var loggerMock = new Mock<ILogger<StoreService>>();
//            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: " GetAllStoreAsync_TestDatabase")
//                .Options;

//            using var dbContextMock = new AppDbContext(dbContextOptions);
//            dbContextMock.Stores.AddRange(new List<Store>
//            {
//                new Store  { Id = "1", Name = "Store1"},
//                new Store  { Id = "2", Name = "Store2"}
//            });
//            await dbContextMock.SaveChangesAsync();

//            var service = new StoreService(dbContextMock,null,null,loggerMock.Object);
//            var request = new BasePaginationRequest 
//            { 
//                PageIndex = 1 ,
//                PageSize = 10
//             };

//            //Act
//            var result = await service.GetAllStoreAsync(request);

//            //Assert
//            Assert.NotNull(result);
//            Assert.Equal(2,result.Data.Count);
//        }
//    }
//}