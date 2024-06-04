using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
{
    public class GetDetailStoreServiceTest
    {
        [Fact]
        public async Task GetDetailStoreAsync_ReturnsStoreResponseModel()
        {
            // Arrange
            string storeId = "store1";
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetDetailStoreAsync_TestDatabase")
                .Options;

            using (AppDbContext dbContext = new(dbContextOptions))
            {
                _ = dbContext.Stores.Add(new Store { Id = storeId, Name = "Test Store" });
                _ = await dbContext.SaveChangesAsync();
            }

            using (AppDbContext dbContext = new(dbContextOptions))
            {
                Store store = new() { Id = storeId, Name = "Test Store" };

                Mock<IMapper> mapperMock = new();
                _ = mapperMock.Setup(m => m.Map<StoreResponseModel>(It.IsAny<Store>()))
                        .Returns<Store>((s) => new StoreResponseModel { Id = s.Id, Name = s.Name });// Configure mapper to return a valid StoreRespo
                Mock<ILogger<StoreService>> loggerMock = new();
                StoreService service = new(dbContext, mapperMock.Object, null, loggerMock.Object);

                // Act
                Core.Common.BaseResponseResult<StoreResponseModel> result = await service.GetDetailStoreAsync(storeId);

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
