using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.ReviewServiceTest
{
    public class GetReviewByStoreIdServiceTest
    {
        [Fact]
        public async Task GetReviewByStoreIdAsync_ReturnsPaginatedReviews()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateReviewRequestModel, Review>();
                cfg.CreateMap<Review, ReviewResponseModel>()
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product == null ? null : new BaseProductResponseModel
                    {
                        Id = src.Product.Id,
                        Name = src.Product.Name,
                        Description = src.Product.Description,
                        Content = src.Product.Content,
                        Price = src.Product.Price,
                        Discount = src.Product.Discount,
                        Quantity = src.Product.Quantity
                    }));
            });
            var mapper = config.CreateMapper();

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseInMemoryDatabase(databaseName: "GetReviewByStoreIdAsync_TestDatabase")
                                    .Options;

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var storeId = "store1";
                var productId = "product1";

                dbContext.Products.Add(new Product { Id = productId, Name = "Product 1", StoreId = storeId });
                dbContext.Reviews.AddRange(
                    new Review { Id = "review1", StoreId = storeId, ProductId = productId, Content = "Review 1", RatingValue = 5 },
                    new Review { Id = "review2", StoreId = storeId, ProductId = productId, Content = "Review 2", RatingValue = 4 },
                    new Review { Id = "review3", StoreId = storeId, ProductId = productId, Content = "Review 3", RatingValue = 3 },
                    new Review { Id = "review4", StoreId = storeId, ProductId = productId, Content = "Review 4", RatingValue = 4 }
                );
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var reviewService = new ReviewService(dbContext, mapper);

                var filterRequest = new BaseReviewFilterRequest
                {
                    SearchRatingKey = 4
                };
                var paginationRequest = new BasePaginationRequest
                {
                    PageIndex = 1,
                    PageSize = 10,
                    SearchString = ""
                };

                // Act
                var result = await reviewService.GetReviewByStoreIdAsync("store1", filterRequest, paginationRequest);

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Data);
                Assert.True(result.Data.Any());
                Assert.Equal(1, result.PageIndex);
                Assert.Equal(10, result.PageSize);
                Assert.Equal(2, result.Data.Count(x => x.RatingValue == 4)); // Kiểm tra số lượng các phần tử có RatingValue bằng 4
            }
        }
    }
}
