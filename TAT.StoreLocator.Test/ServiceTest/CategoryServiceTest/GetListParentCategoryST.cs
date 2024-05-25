using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.CategoryServiceTest
{
    public class GetListParentCategoryST
    {
        [Fact]
        public async Task GetListParentCategory_ReturnsPagedList()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetListParentCategory_ReturnsPagedListTestDatabase")
                .Options;
            using var dbContextMock = new AppDbContext(dbContextOptions);
            dbContextMock.Categories.AddRange(new List<Category>
            {
                new Category { Id = "1", Name = "Category 1" },
                new Category { Id = "2", Name = "Category 2" },
            });
            await dbContextMock.SaveChangesAsync(); // Chờ cho tác vụ lưu trữ hoàn thành

            var service = new CategoryService(loggerMock.Object, dbContextMock);
            var request = new BasePaginationRequest { PageIndex = 1, PageSize = 10 };

            // Act
            var result = await service.GetListParentCategoryAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Data.Count);
        }
    }
}