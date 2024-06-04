using Microsoft.EntityFrameworkCore;
using Moq;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
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
            string Id = "product1";
            ProductResponseModel expectedProduct = new()
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
                Category = new Core.Models.Response.Category.CategoryResponseModel
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
                    new() {
                        Id = "gallery1",
                        FileName = "image1.jpg",
                        Url = "http://example.com/image1.jpg",
                        FileBelongsTo = "product",
                        IsThumbnail = true
                    }
                },
                Reviews = new List<ReviewResponseModel>
                {
                    new() {
                        Id = "review1",
                        Content = "Great product!",
                        RatingValue = 5,
                        UserId = "user1"
                    }
                }
            };

            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetById_ReturnsCorrectProduct_TestDatabase")
                .Options;

            using AppDbContext dbContextMock = new(dbContextOptions);
            _ = dbContextMock.Products.Add(new Product
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
                    new() {
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
                    new() {
                        Id = "review1",
                        Content = "Great product!",
                        RatingValue = 5,
                        UserId = "user1"
                    }
                }
            });

            _ = dbContextMock.Categories.Add(new Category
            {
                Id = "category1",
                Name = "Category 1",
                Description = "Description of Category 1"
            });

            _ = dbContextMock.Stores.Add(new Store
            {
                Id = "store1",
                Name = "Store 1",
                Email = "store1@example.com"
            });

            _ = await dbContextMock.SaveChangesAsync(); // Wait for the data to be saved

            ProductService productService = new(dbContextMock, Mock.Of<IPhotoService>());

            // Act
            Core.Common.BaseResponseResult<ProductResponseModel> result = await productService.GetById(Id);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedProduct.Id, result.Data!.Id);
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
            Assert.Equal(expectedProduct.Category.Id, result.Data.Category!.Id);
            Assert.Equal(expectedProduct.Category.Name, result.Data.Category.Name);
            Assert.Equal(expectedProduct.Category.Description, result.Data.Category.Description);
            Assert.NotNull(result.Data.Store);
            Assert.Equal(expectedProduct.Store.Id, result.Data.Store!.Id);
            Assert.Equal(expectedProduct.Store.Name, result.Data.Store.Name);
            Assert.Equal(expectedProduct.Store.Email, result.Data.Store.Email);
            Assert.NotNull(result.Data.GalleryResponseModels);
            Assert.Equal(expectedProduct.GalleryResponseModels.FirstOrDefault()!.Id, result.Data.GalleryResponseModels!.FirstOrDefault()!.Id);
            Assert.Equal(expectedProduct.GalleryResponseModels.FirstOrDefault()!.FileName, result.Data.GalleryResponseModels!.FirstOrDefault()!.FileName);
            Assert.Equal(expectedProduct.GalleryResponseModels.FirstOrDefault()!.Url, result.Data.GalleryResponseModels!.FirstOrDefault()!.Url);
            Assert.Equal(expectedProduct.GalleryResponseModels.FirstOrDefault()!.FileBelongsTo, result.Data.GalleryResponseModels!.FirstOrDefault()!.FileBelongsTo);
            Assert.Equal(expectedProduct.GalleryResponseModels.FirstOrDefault()!.IsThumbnail, result.Data.GalleryResponseModels!.FirstOrDefault()!.IsThumbnail);
            Assert.NotNull(result.Data.Reviews);
            Assert.Equal(expectedProduct.Reviews.FirstOrDefault()!.Id, result.Data.Reviews!.FirstOrDefault()!.Id);
            Assert.Equal(expectedProduct.Reviews.FirstOrDefault()!.Content, result.Data.Reviews!.FirstOrDefault()!.Content);
            Assert.Equal(expectedProduct.Reviews.FirstOrDefault()!.RatingValue, result.Data.Reviews!.FirstOrDefault()!.RatingValue);
            Assert.Equal(expectedProduct.Reviews.FirstOrDefault()!.Status, result.Data.Reviews!.FirstOrDefault()!.Status);
            Assert.Equal(expectedProduct.Reviews.FirstOrDefault()!.UserId, result.Data.Reviews!.FirstOrDefault()!.UserId);
        }
    }
}