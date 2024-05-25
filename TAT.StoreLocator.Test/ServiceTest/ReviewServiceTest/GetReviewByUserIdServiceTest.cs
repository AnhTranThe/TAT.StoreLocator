using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class GetReviewByUserIdServiceTest
    {
        [Fact]
        public async Task GetReviewByUserIdAsync_ReturnsPaginatedReviews()
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
             var _mapper = config.CreateMapper();

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseInMemoryDatabase(databaseName: "GetReviewByUserIdAsync_TestDatabase")
                                    .Options;

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var userId = "user1";
                var productId = "product1";

                dbContext.Products.Add(new Product { Id = productId, Name = "Product 1" });
                dbContext.Reviews.AddRange(
                    new Review { Id = "review1", UserId = userId, ProductId = productId, Content = "Review 1", RatingValue = 5 },
                    new Review { Id = "review2", UserId = userId, ProductId = productId, Content = "Review 2", RatingValue = 4 },
                    new Review { Id = "review3", UserId = userId, ProductId = productId, Content = "Review 3", RatingValue = 3 },
                    new Review { Id = "review4", UserId = userId, ProductId = productId, Content = "Review 5", RatingValue = 5 }
                );
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var reviewService = new ReviewService(dbContext, _mapper);

                var filterRequest = new BaseReviewFilterRequest
                {
                    SearchRatingKey = 5
                };
                var paginationRequest = new BasePaginationRequest
                {
                    PageIndex = 1,
                    PageSize = 10,
                    SearchString = ""
                };

                // Act
                var result = await reviewService.GetReviewByUserIdAsync("user1", filterRequest, paginationRequest);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Data.Any());
                Assert.Equal(1, result.PageIndex);
                Assert.Equal(10, result.PageSize);
                Assert.Equal(2, result.Data.Count(x => x.RatingValue == 5)); // Kiểm tra số lượng các phần tử có RatingValue bằng 5
            }
        }
    }
}
