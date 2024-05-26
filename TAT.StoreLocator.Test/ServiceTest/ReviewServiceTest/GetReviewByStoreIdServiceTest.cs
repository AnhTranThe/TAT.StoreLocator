using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            MapperConfiguration config = new(cfg =>
            {
                _ = cfg.CreateMap<CreateReviewRequestModel, Review>();
                _ = cfg.CreateMap<Review, ReviewResponseModel>()
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
            IMapper mapper = config.CreateMapper();

            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseInMemoryDatabase(databaseName: "GetReviewByStoreIdAsync_TestDatabase")
                                    .Options;

            using (AppDbContext dbContext = new(dbContextOptions))
            {
                string storeId = "store1";
                string productId = "product1";

                _ = dbContext.Products.Add(new Product { Id = productId, Name = "Product 1", StoreId = storeId });
                dbContext.Reviews.AddRange(
                    new Review { Id = "review1", StoreId = storeId, ProductId = productId, Content = "Review 1", RatingValue = 5 },
                    new Review { Id = "review2", StoreId = storeId, ProductId = productId, Content = "Review 2", RatingValue = 4 },
                    new Review { Id = "review3", StoreId = storeId, ProductId = productId, Content = "Review 3", RatingValue = 3 },
                    new Review { Id = "review4", StoreId = storeId, ProductId = productId, Content = "Review 4", RatingValue = 4 }
                );
                _ = await dbContext.SaveChangesAsync();
            }

            using (AppDbContext dbContext = new(dbContextOptions))
            {
                ReviewService reviewService = new(dbContext, mapper);

                BaseReviewFilterRequest filterRequest = new()
                {
                    SearchRatingKey = 4
                };
                BasePaginationRequest paginationRequest = new()
                {
                    PageIndex = 1,
                    PageSize = 10,
                    SearchString = ""
                };

                // Act
                BasePaginationResult<ReviewResponseModel> result = await reviewService.GetReviewsByStoreIdAsync(filterRequest, paginationRequest);

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
