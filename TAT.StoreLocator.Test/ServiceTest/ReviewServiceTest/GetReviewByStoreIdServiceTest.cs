using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
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
        public async Task GetReviewsByStoreIdAsync_ReturnsPaginatedReviews()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetReviewsByStoreIdAsync_ReturnsPaginatedReviews_TestDatabase")
                .Options;

            // Create and seed the in-memory database
            using (var dbContext = new AppDbContext(options))
            {
                dbContext.Reviews.AddRange(
                    new Core.Entities.Review { Id = "review1", StoreId = "store1", Content = "Review 1", RatingValue = 4 },
                    new Core.Entities.Review { Id = "review2", StoreId = "store1", Content = "Review 2", RatingValue = 4 },
                    new Core.Entities.Review { Id = "review3", StoreId = "store2", Content = "Review 3", RatingValue = 5 }
                );
                await dbContext.SaveChangesAsync();
            }

            var filterRequest = new BaseReviewFilterRequest
            {
                TypeId = "store1", 
                SearchRatingKey = 4
            };
            var paginationRequest = new BasePaginationRequest
            {
                PageIndex = 1,
                PageSize = 10,
                SearchString = ""
            };

            // Mock IMapper
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<List<ReviewResponseModel>>(It.IsAny<List<Review>>()))
                .Returns((List<Review> reviews) => reviews.Select(r => new ReviewResponseModel
                {
                    Id = r.Id,
                    StoreId = r.StoreId,
                    Content = r.Content,
                    RatingValue = r.RatingValue,
                    // Map other properties as needed
                }).ToList());

            using (var dbContext = new AppDbContext(options))
            {
                var reviewService = new ReviewService(dbContext, mapperMock.Object);

                // Act
                var result = await reviewService.GetReviewsByStoreIdAsync(filterRequest, paginationRequest);

                // Print debug information
                Console.WriteLine($"Total Count: {result.TotalCount}");
                if (result.Data != null)
                {
                    foreach (var review in result.Data)
                    {
                        Console.WriteLine($"Review Id: {review.Id}, Store Id: {review.StoreId}, Rating Value: {review.RatingValue}");
                    }
                }
                else
                {
                    Console.WriteLine("Result data is null.");
                }

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Data);
                Assert.Equal(1, result.PageIndex);
                Assert.Equal(10, result.PageSize);
                Assert.Equal(2, result.Data.Count(x => x.RatingValue == 4));
            }
        }
    }
}