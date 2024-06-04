
using Xunit;
using Moq;
using TAT.StoreLocator.Infrastructure.Services;
using TAT.StoreLocator.Core.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using AutoMapper;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Store;

namespace TAT.StoreLocator.Test.ServiceTest.StoreServiceTest
{
    public class GetAllStoreServiceTest
    {
        [Fact]
        public async Task GetAllStore_Returns_Valid_Result()
        {
            // Arrange
            var expectedStores = new List<StoreResponseModel>
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

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetListStore_TestDatabase")
                .Options;

            using var dbContextMock = new AppDbContext(dbContextOptions);

            // Add mock data to the in-memory database
            foreach (var store in expectedStores)
            {
                dbContextMock.Stores.Add(new Store
                {
                    Id = store.Id,
                    Name = store.Name,
                    Email = "example@example.com", // Thêm giá trị cho Email và các cột khác tương ứng
                    PhoneNumber = "123456789",
                    Address = new Address(), // Khởi tạo một đối tượng Address hoặc sử dụng dữ liệu thực tế
                    IsActive = true, // Thêm giá trị cho các cột khác nếu cần
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }
            await dbContextMock.SaveChangesAsync();

            // Mock IMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Store, StoreResponseModel>();
            });
            var mapper = mapperConfig.CreateMapper();

            // Mock IPhotoService
            var photoServiceMock = new Mock<IPhotoService>();

            // Mock ILogger
            var loggerMock = new Mock<ILogger<StoreService>>();

            var storeService = new StoreService(dbContextMock, mapper, photoServiceMock.Object, loggerMock.Object);

            var paginationRequest = new BasePaginationRequest
            {
                PageSize = 10,
                PageIndex = 1,
                SearchString = "Store 1" // Chuỗi tìm kiếm
            };

            // Act
            var result = await storeService.GetAllStoreAsync(paginationRequest);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);

            // Kiểm tra kết quả trả về chỉ chứa các cửa hàng có tên chứa chuỗi tìm kiếm
            Assert.True(result.Data.All(store => store.Name.Contains(paginationRequest.SearchString)));
        }
    }
}
    