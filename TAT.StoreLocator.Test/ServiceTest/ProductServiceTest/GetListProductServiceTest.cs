using Microsoft.EntityFrameworkCore;
using Moq;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.ProductServiceTest
{
    public class GetListProductServiceTest
    {
        [Fact]
        public async Task GetListProduct_ReturnsCorrectProductList()
        {
            ////Arrange
            List<ProductResponseModel> expectedProduct = new()
            {
                new ProductResponseModel
                {
                    Id = "product1",
                    Name = "Product 1",
                    Description = "Description of Product 1",
                },
                new ProductResponseModel
                {
                    Id = "product2",
                    Name = "Product 2",
                    Description = "Description of Product 2",
                 }
         };

            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetListProduct_TestDatabase")
                .Options;

            using AppDbContext dbContextMock = new(dbContextOptions);

            // Add mock data to the in-memory database
            foreach (ProductResponseModel product in expectedProduct)
            {
                _ = dbContextMock.Products.Add(new Product
                {
                    Id = product.Id!,
                    Name = product.Name,
                    Description = product.Description,
                });
            }
            _ = await dbContextMock.SaveChangesAsync();

            ProductService productService = new(dbContextMock, Mock.Of<IPhotoService>());

            BasePaginationRequest paginationRequest = new()
            {
                PageSize = 10,
                PageIndex = 1,
            };

            // Act
            BasePaginationResult<ProductResponseModel> result = await productService.GetListProductAsync(paginationRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Count, result.Data.Count);

            // Assert each product in the result matches the expected products
            for (int i = 0; i < expectedProduct.Count; i++)
            {
                Assert.Equal(expectedProduct[i].Id, result.Data[i].Id);
                Assert.Equal(expectedProduct[i].Name, result.Data[i].Name);
                Assert.Equal(expectedProduct[i].Description, result.Data[i].Description);
            }
        }
    }
}
