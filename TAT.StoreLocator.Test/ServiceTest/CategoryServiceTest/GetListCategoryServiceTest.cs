using Xunit;
using Moq;
using TAT.StoreLocator.Infrastructure.Services;
using TAT.StoreLocator.Core.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;

namespace TAT.StoreLocator.Test.ServiceTest.CategoryServiceTest
{
    public class GetListCategoryServiceTest
    {
        [Fact]
        public async Task GetList_ReturnsPagedList()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetList_ReturnsPagedListTestDatabase")
                .Options;
            using var dbContextMock = new AppDbContext(dbContextOptions);
            dbContextMock.Categories.AddRange(new List<Category>
            {
                new Category { Id = "1", Name = "Category 1" },
                new Category { Id = "2", Name = "Category 2" },
                new Category { Id = "3", Name = "Category 3" }
            });
            await dbContextMock.SaveChangesAsync(); // Chờ cho tác vụ lưu trữ hoàn thành

            var service = new CategoryService(loggerMock.Object, dbContextMock);
            var request = new BasePaginationRequest { PageIndex = 1, PageSize = 10 };

            // Act
            var result = await service.GetListAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(3, result.Data.Count);
        }

    }
}