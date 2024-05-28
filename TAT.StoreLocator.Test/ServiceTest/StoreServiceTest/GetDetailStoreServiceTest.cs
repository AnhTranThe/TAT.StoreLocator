using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Entities;
using log4net.Repository.Hierarchy;
using TAT.StoreLocator.Infrastructure.Services;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Store;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
{
    public  class GetDetailStoreServiceTest
    {
        [Fact]
        public async Task GetDetailStoreAsync_ReturnsStoreResponseModel()
        {
            // Arrange
            var storeId = "store1";
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetDetailStoreAsync_TestDatabase")
                .Options;

            using ( var dbContext = new AppDbContext(dbContextOptions))
            {
                dbContext.Stores.Add(new Store { Id = storeId , Name = "Test Store" });
               await dbContext.SaveChangesAsync();
            }

            using ( var dbContext = new AppDbContext(dbContextOptions))
            {
                var store = new Store { Id = storeId, Name = "Test Store" };

                var mapperMock = new Mock<IMapper>();
                mapperMock.Setup(m => m.Map<StoreResponseModel>(It.IsAny<Store>()))
                        .Returns<Store>((s) => new StoreResponseModel { Id = s.Id , Name = s.Name });// Configure mapper to return a valid StoreRespo
                var loggerMock = new Mock<ILogger<StoreService>>();
                var service = new StoreService(dbContext, mapperMock.Object,null,loggerMock.Object);
               
                // Act
                var result = await service.GetDetailStoreAsync(storeId);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.NotNull(result.Data);
                Assert.Equal(store.Id, result.Data?.Id);
                Assert.Equal(store.Name, result.Data?.Name);
            }
        }
    }
}
