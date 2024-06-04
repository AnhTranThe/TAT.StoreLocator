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

namespace TAT.StoreLocator.Test.ServiceTest.ProductServiceTest
{
    public class GetListProductServiceTest
    {
        [Fact]
        public async Task GetListProduct_ReturnsCorrectProductList()
        {
            ////Arrange
            var expectedProduct = new List<ProductResponseModel>
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

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetListProduct_TestDatabase")
                .Options;

            using var dbContextMock = new AppDbContext(dbContextOptions);

            // Add mock data to the in-memory database
            foreach (var product in expectedProduct)
            {
                dbContextMock.Products.Add(new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                });
            }
            await dbContextMock.SaveChangesAsync();

            var productService = new ProductService(Mock.Of<ILogger>(), dbContextMock, Mock.Of<IPhotoService>());

            var paginationRequest = new BasePaginationRequest
            {
                PageSize = 10,
                PageIndex = 1,
            };

            // Act
            var result = await productService.GetListProductAsync(paginationRequest);

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
