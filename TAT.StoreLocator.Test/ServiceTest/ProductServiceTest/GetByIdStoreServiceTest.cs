using log4net.Core;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;
using ILogger = TAT.StoreLocator.Core.Interface.ILogger.ILogger;

namespace TAT.StoreLocator.Test.ServiceTest.ProductServiceTest
{
    public class GetByIdStoreServiceTest
    {
        [Fact]
        public async Task GetByIdStore_ReturnsActiveProducts()
        {
            // Setup
            var loggerMock = new Mock<ILogger>();
            var photoServiceMock = new Mock<IPhotoService>();
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            await using var dbContext = new AppDbContext(dbContextOptions);

            var storeId = "test-store-id";
            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1", StoreId = storeId, IsActive = true },
                new Product { Id = "2", Name = "Product 2", StoreId = storeId, IsActive = true },
                new Product { Id = "3", Name = "Product 3", StoreId = "other-store-id", IsActive = true },
                new Product { Id = "4", Name = "Product 4", StoreId = storeId, IsActive = false }
            };

            await dbContext.Products.AddRangeAsync(products);
            await dbContext.SaveChangesAsync();

            var productService = new ProductService(loggerMock.Object, dbContext, photoServiceMock.Object);

            var request = new BasePaginationRequest { PageIndex = 1, PageSize = 10 };

            // Act
            var result = await productService.GetByIdStore(storeId, request);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Contains(result.Data, p => p.Name == "Product 1");
            Assert.Contains(result.Data, p => p.Name == "Product 2");
            Assert.DoesNotContain(result.Data, p => p.Name == "Product 3");
            Assert.DoesNotContain(result.Data, p => p.Name == "Product 4");
        }
    }

}