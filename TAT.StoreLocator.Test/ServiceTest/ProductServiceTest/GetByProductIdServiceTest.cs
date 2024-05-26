//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TAT.StoreLocator.Core.Entities;
//using TAT.StoreLocator.Core.Interface.ILogger;
//using TAT.StoreLocator.Core.Interface.IServices;
//using TAT.StoreLocator.Core.Models.Response.Category;
//using TAT.StoreLocator.Core.Models.Response.Product;
//using TAT.StoreLocator.Infrastructure.Persistence.EF;
//using TAT.StoreLocator.Infrastructure.Services;
//using Xunit;

//namespace TAT.StoreLocator.Test.ServiceTest.ProductServiceTest
//{
//    public class GetByProductIdServiceTest
//    {
//        [Fact]
//        public async Task GetById_ReturnsCorrectProduct()
//        {
//            // Arrange
//            var productId = "product1";
//            var expectedProduct = new ProductResponseModel
//            {
//                Id = productId,
//                Name = "Product 1",
//                Description = "Description of Product 1",
//                Content = "Content of Product 1",
//                Note = "Note of Product 1",
//                Slug = "product-1",
//                Price = 100000,
//                Discount = 5,
//                MetaTitle = "Meta Title of Product 1",
//                MetaDescription = "Meta Description of Product 1",
//                Quantity = 100,
//                Rating = 4,
//                SKU = "SKU-001",
//                IsActive = true,
//                ProductViewCount = 500,
//                CategoryId = "category1",
//                Category = new CategoryProductResponseModel
//                {
//                    Id = "category1",
//                    Name = "Category 1",
//                    Description = "Description of Category 1"
//                }
//            };

//            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: "GetById_ReturnsCorrectProduct_TestDatabase")
//                .Options;

//            using var dbContextMock = new AppDbContext(dbContextOptions);
//            dbContextMock.Products.Add(new Product
//            {
//                Id = productId,
//                Name = "Product 1",
//                Description = "Description of Product 1",
//                Content = "Content of Product 1",
//                Note = "Note of Product 1",
//                Slug = "product-1",
//                Price = 100000,
//                Discount = 5,
//                MetaTitle = "Meta Title of Product 1",
//                MetaDescription = "Meta Description of Product 1",
//                Quantity = 100,
//                Rating = 4,
//                SKU = "SKU-001",
//                IsActive = true,
//                ProductViewCount = 500,
//                CategoryId = "category1"
//            });
//            await dbContextMock.SaveChangesAsync(); // Wait for the data to be saved

//            var productService = new ProductService(Mock.Of<ILogger>(), dbContextMock, Mock.Of<IPhotoService>());

//            // Act
//            var result = await productService.GetById(productId);

//            // Assert
//            Assert.True(result.Success);
//            Assert.NotNull(result.Data);
//            Assert.Equal(expectedProduct.Id, result.Data.Id);
//            Assert.Equal(expectedProduct.Name, result.Data.Name);
//            Assert.Equal(expectedProduct.Description, result.Data.Description);
//            Assert.Equal(expectedProduct.Content, result.Data.Content);
//            Assert.Equal(expectedProduct.Note, result.Data.Note);
//            Assert.Equal(expectedProduct.Slug, result.Data.Slug);
//            Assert.Equal(expectedProduct.Price, result.Data.Price);
//            Assert.Equal(expectedProduct.Discount, result.Data.Discount);
//            Assert.Equal(expectedProduct.MetaTitle, result.Data.MetaTitle);
//            Assert.Equal(expectedProduct.MetaDescription, result.Data.MetaDescription);
//            Assert.Equal(expectedProduct.Quantity, result.Data.Quantity);
//            Assert.Equal(expectedProduct.Rating, result.Data.Rating);
//            Assert.Equal(expectedProduct.SKU, result.Data.SKU);
//            Assert.Equal(expectedProduct.IsActive, result.Data.IsActive);
//            Assert.Equal(expectedProduct.ProductViewCount, result.Data.ProductViewCount);
//            Assert.NotNull(result.Data.Category);
//            Assert.Equal(expectedProduct.Category.Id, result.Data.Category.Id);
//            Assert.Equal(expectedProduct.Category.Name, result.Data.Category.Name);
//            Assert.Equal(expectedProduct.Category.Description, result.Data.Category.Description);
//        }
//    }
//}
