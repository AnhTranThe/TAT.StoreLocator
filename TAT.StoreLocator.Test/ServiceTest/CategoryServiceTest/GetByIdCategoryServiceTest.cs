using Xunit;
using Moq;
using TAT.StoreLocator.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Core.Models.Request.Category;
using TAT.StoreLocator.Core.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace TAT.StoreLocator.Test.ServiceTests
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetById_ReturnsCorrectResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseInMemoryDatabase(databaseName: "GetById_ReturnsCorrectResult_Database")
                                    .Options;

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var category = new Category
                {
                    Id = "1",
                    Name = "Category 1",
                    Description = "Description of category 1",
                    Slug = "category-1",
                    IsActive = true,
                    ParentCategoryId = null
                };
                dbContext.Categories.Add(category);
                dbContext.SaveChanges();
            }

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var service = new CategoryService(loggerMock.Object, dbContext);

                // Act
                var result = await service.GetById("1");

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.NotNull(result.Data);
                Assert.Equal("1", result.Data?.Id);
                Assert.Equal("Category 1", result.Data?.Name);
            }
        }

        [Fact]
        public async Task GetById_ReturnsNullWhenIdNotFound()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseInMemoryDatabase(databaseName: "TestDatabase1")
                                    .Options;

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var service = new CategoryService(loggerMock.Object, dbContext);

                // Act
                var result = await service.GetById("1");

                // Assert
                Assert.NotNull(result);
                Assert.False(result.Success);
                Assert.Null(result.Data);
            }
        }

        [Fact]
        public async Task GetById_ReturnsNullWhenIdIsNull()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseInMemoryDatabase(databaseName: "TestDatabase2")
                                    .Options;

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var service = new CategoryService(loggerMock.Object, dbContext);

                // Act
                var result = await service.GetById(null);

                // Assert
                Assert.NotNull(result);
                Assert.False(result.Success);
                Assert.Null(result.Data);
            }
        }

    }
}