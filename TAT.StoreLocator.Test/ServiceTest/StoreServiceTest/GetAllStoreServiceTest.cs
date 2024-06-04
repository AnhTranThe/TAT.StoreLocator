
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
{
    public class GetAllStoreServiceTest
    {
        [Fact]
        public async Task GetAllStore_Returns_Valid_Result()
        {
            // Arrange
            List<StoreResponseModel> expectedStores = new()
            {
                new StoreResponseModel
                {
                    Id = "store1",
                    Name = "Store 1",
                },
                new StoreResponseModel
                {
                    Id = "store2",
                    Name = "Store 2",
                }
            };

            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetListStore_TestDatabase")
                .Options;

            using AppDbContext dbContextMock = new(dbContextOptions);

            // Add mock data to the in-memory database
            foreach (StoreResponseModel store in expectedStores)
            {
                _ = dbContextMock.Stores.Add(new Store
                {
                    Id = store.Id!,
                    Name = store.Name,
                    Email = "example@example.com", // Thêm giá trị cho Email và các cột khác tương ứng
                    PhoneNumber = "123456789",
                    Address = new Address(), // Khởi tạo một đối tượng Address hoặc sử dụng dữ liệu thực tế
                    IsActive = true, // Thêm giá trị cho các cột khác nếu cần
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }
            _ = await dbContextMock.SaveChangesAsync();

            // Mock IMapper
            MapperConfiguration mapperConfig = new(cfg =>
            {
                _ = cfg.CreateMap<Store, StoreResponseModel>();
            });
            IMapper mapper = mapperConfig.CreateMapper();

            // Mock IPhotoService
            Mock<IPhotoService> photoServiceMock = new();

            // Mock ILogger
            Mock<ILogger<StoreService>> loggerMock = new();

            StoreService storeService = new(dbContextMock, mapper, photoServiceMock.Object, loggerMock.Object);

            BasePaginationRequest paginationRequest = new()
            {
                PageSize = 10,
                PageIndex = 1,
                SearchString = "Store 1" // Chuỗi tìm kiếm
            };

            // Act
            BasePaginationResult<StoreResponseModel> result = await storeService.GetAllStoreAsync(paginationRequest);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);

            // Kiểm tra kết quả trả về chỉ chứa các cửa hàng có tên chứa chuỗi tìm kiếm
            Assert.True(result.Data.TrueForAll(store => store.Name!.Contains(paginationRequest.SearchString)));
        }
    }
}
