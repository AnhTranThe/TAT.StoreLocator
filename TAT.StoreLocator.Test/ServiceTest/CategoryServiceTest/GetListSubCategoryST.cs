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
    public class GetListSubCategoryST
    {
        [Fact]
        public async Task GetListSubCategory_ReturnsPagedList()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetListSubCategory_ReturnsPagedListTestDatabase")
                .Options;
            using var dbContextMock = new AppDbContext(dbContextOptions);
            dbContextMock.Categories.AddRange(new List<Category>
            {
              new Category { Id = "2", Name = "Category 2", ParentCategoryId = "1" },
              new Category { Id = "3", Name = "Category 3", ParentCategoryId = "1" },
              new Category { Id = "4", Name = "Category 4", ParentCategoryId = "" } // Danh mục cha
            });
            await dbContextMock.SaveChangesAsync(); // Chờ cho tác vụ lưu trữ hoàn thành

            var service = new CategoryService(loggerMock.Object, dbContextMock);
            var request = new BasePaginationRequest { PageIndex = 1, PageSize = 10 };

            // Act
            var result = await service.GetListSubCategoryAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount); // Chỉ cần 2 danh mục con được trả về
        }
    }
}