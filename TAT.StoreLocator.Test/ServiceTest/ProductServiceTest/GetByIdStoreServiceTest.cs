using Microsoft.EntityFrameworkCore;
using Moq;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.ProductServiceTest
{
    public class GetByIdStoreServiceTest
    {
        [Fact]
        public async Task GetByIdStore_ReturnsActiveProducts()
        {
            // Setup

            Mock<IPhotoService> photoServiceMock = new();
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            await using AppDbContext dbContext = new(dbContextOptions);

            string storeId = "test-store-id";
            List<Product> products = new()
            {
                new Product { Id = "1", Name = "Product 1", StoreId = storeId, IsActive = true },
                new Product { Id = "2", Name = "Product 2", StoreId = storeId, IsActive = true },
                new Product { Id = "3", Name = "Product 3", StoreId = "other-store-id", IsActive = true },
                new Product { Id = "4", Name = "Product 4", StoreId = storeId, IsActive = false }
            };

            await dbContext.Products.AddRangeAsync(products);
            _ = await dbContext.SaveChangesAsync();

            ProductService productService = new(dbContext, photoServiceMock.Object);

            BasePaginationRequest request = new() { PageIndex = 1, PageSize = 10 };

            // Act
            BasePaginationResult<Core.Models.Response.Product.ProductResponseModel> result = await productService.GetByIdStore(storeId, request);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Contains(result.Data, p => p.Name == "Product 1");
            Assert.Contains(result.Data, p => p.Name == "Product 2");
            Assert.DoesNotContain(result.Data, p => p.Name == "Product 3");
            Assert.DoesNotContain(result.Data, p => p.Name == "Product 4");
        }
    }

}