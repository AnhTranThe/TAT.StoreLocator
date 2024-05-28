using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.Category;
using TAT.StoreLocator.Core.Models.Response.Gallery;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.ProductServiceTest
{
    public class GetByProductIdServiceTest
    {
        [Fact]
        public async Task GetById_ReturnsCorrectProduct()
        {
            // Arrange
            var Id = "product1";
            var expectedProduct = new ProductResponseModel
            {
                Id = Id,
                Name = "Product 1",
                Description = "Description of Product 1",
                Content = "Content of Product 1",
                Note = "Note of Product 1",
                Slug = "product-1",
                Price = 100000,
                Discount = 5,
                MetaTitle = "Meta Title of Product 1",
                MetaDescription = "Meta Description of Product 1",
                Quantity = 100,
                Rating = 4,
                SKU = "SKU-001",
                IsActive = true,
                ProductViewCount = 500,
                CategoryId = "category1",
                Category = new CategoryProductResponseModel
                {
                    Id = "category1",
                    Name = "Category 1",
                    Description = "Description of Category 1"
                },
                StoreId = "store1",
                Store = new StoreOfProductResponseModel
                {
                    Id = "store1",
                    Name = "Store 1",
                    Email = "store1@example.com"
                },
                GalleryResponseModels = new List<GalleryResponseModel>
                {
                    new GalleryResponseModel
                    {
                        Id = "gallery1",
                        FileName = "image1.jpg",
                        Url = "http://example.com/image1.jpg",
                        FileBelongsTo = "product",
                        IsThumbnail = true
                    }
                },
                Reviews = new List<ReviewResponseModel>
                {
                    new ReviewResponseModel
                    {
                        Id = "review1",
                        Content = "Great product!",
                        RatingValue = 5,
                        UserId = "user1"
                    }
                }
            };

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetById_ReturnsCorrectProduct_TestDatabase")
                .Options;

            using var dbContextMock = new AppDbContext(dbContextOptions);
            dbContextMock.Products.Add(new Product
            {
                Id = Id,
                Name = "Product 1",
                Description = "Description of Product 1",
                Content = "Content of Product 1",
                Note = "Note of Product 1",
                Slug = "product-1",
                Price = 100000,
                Discount = 5,
                MetaTitle = "Meta Title of Product 1",
                MetaDescription = "Meta Description of Product 1",
                Quantity = 100,
                Rating = 4,
                SKU = "SKU-001",
                IsActive = true,
                ProductViewCount = 500,
                CategoryId = "category1",
                StoreId = "store1",
                MapGalleryProducts = new List<MapGalleryProduct>
                {
                    new MapGalleryProduct
                    {
                        Gallery = new Gallery
                        {
                            Id = "gallery1",
                            FileName = "image1.jpg",
                            Url = "http://example.com/image1.jpg",
                            FileBelongsTo = "product",
                            IsThumbnail = true
                        }
                    }
                },
                Reviews = new List<Review>
                {
                    new Review
                    {
                        Id = "review1",
                        Content = "Great product!",
                        RatingValue = 5,
                        UserId = "user1"
                    }
                }
            });

            dbContextMock.Categories.Add(new Category
            {
                Id = "category1",
                Name = "Category 1",
                Description = "Description of Category 1"
            });

            dbContextMock.Stores.Add(new Store
            {
                Id = "store1",
                Name = "Store 1",
                Email = "store1@example.com"
            });

            await dbContextMock.SaveChangesAsync(); // Wait for the data to be saved

            var productService = new ProductService(Mock.Of<ILogger>(), dbContextMock, Mock.Of<IPhotoService>());

            // Act
            var result = await productService.GetById(Id);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedProduct.Id, result.Data.Id);
            Assert.Equal(expectedProduct.Name, result.Data.Name);
            Assert.Equal(expectedProduct.Description, result.Data.Description);
            Assert.Equal(expectedProduct.Content, result.Data.Content);
            Assert.Equal(expectedProduct.Note, result.Data.Note);
            Assert.Equal(expectedProduct.Slug, result.Data.Slug);
            Assert.Equal(expectedProduct.Price, result.Data.Price);
            Assert.Equal(expectedProduct.Discount, result.Data.Discount);
            Assert.Equal(expectedProduct.MetaTitle, result.Data.MetaTitle);
            Assert.Equal(expectedProduct.MetaDescription, result.Data.MetaDescription);
            Assert.Equal(expectedProduct.Quantity, result.Data.Quantity);
            Assert.Equal(expectedProduct.Rating, result.Data.Rating);
            Assert.Equal(expectedProduct.SKU, result.Data.SKU);
            Assert.Equal(expectedProduct.IsActive, result.Data.IsActive);
            Assert.Equal(expectedProduct.ProductViewCount, result.Data.ProductViewCount);
            Assert.NotNull(result.Data.Category);
            Assert.Equal(expectedProduct.Category.Id, result.Data.Category.Id);
            Assert.Equal(expectedProduct.Category.Name, result.Data.Category.Name);
            Assert.Equal(expectedProduct.Category.Description, result.Data.Category.Description);
            Assert.NotNull(result.Data.Store);
            Assert.Equal(expectedProduct.Store.Id, result.Data.Store.Id);
            Assert.Equal(expectedProduct.Store.Name, result.Data.Store.Name);
            Assert.Equal(expectedProduct.Store.Email, result.Data.Store.Email);
            Assert.NotNull(result.Data.GalleryResponseModels);
            Assert.Equal(expectedProduct.GalleryResponseModels.First().Id, result.Data.GalleryResponseModels.First().Id);
            Assert.Equal(expectedProduct.GalleryResponseModels.First().FileName, result.Data.GalleryResponseModels.First().FileName);
            Assert.Equal(expectedProduct.GalleryResponseModels.First().Url, result.Data.GalleryResponseModels.First().Url);
            Assert.Equal(expectedProduct.GalleryResponseModels.First().FileBelongsTo, result.Data.GalleryResponseModels.First().FileBelongsTo);
            Assert.Equal(expectedProduct.GalleryResponseModels.First().IsThumbnail, result.Data.GalleryResponseModels.First().IsThumbnail);
            Assert.NotNull(result.Data.Reviews);
            Assert.Equal(expectedProduct.Reviews.First().Id, result.Data.Reviews.First().Id);
            Assert.Equal(expectedProduct.Reviews.First().Content, result.Data.Reviews.First().Content);
            Assert.Equal(expectedProduct.Reviews.First().RatingValue, result.Data.Reviews.First().RatingValue);
            Assert.Equal(expectedProduct.Reviews.First().Status, result.Data.Reviews.First().Status);
            Assert.Equal(expectedProduct.Reviews.First().UserId, result.Data.Reviews.First().UserId);
        }
    }
}