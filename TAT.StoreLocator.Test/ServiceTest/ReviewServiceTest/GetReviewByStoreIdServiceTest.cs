using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
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
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetReviewsByStoreIdAsync_ReturnsPaginatedReviews_TestDatabase")
                .Options;

            // Create and seed the in-memory database
            using (AppDbContext dbContext = new(options))
            {
                dbContext.Reviews.AddRange(
                    new Core.Entities.Review { Id = "review1", StoreId = "store1", Content = "Review 1", RatingValue = 4 },
                    new Core.Entities.Review { Id = "review2", StoreId = "store1", Content = "Review 2", RatingValue = 4 },
                    new Core.Entities.Review { Id = "review3", StoreId = "store2", Content = "Review 3", RatingValue = 5 }
                );
                _ = await dbContext.SaveChangesAsync();
            }

            BaseReviewFilterRequest filterRequest = new()
            {
                TypeId = "store1",
                SearchRatingKey = 4
            };
            BasePaginationRequest paginationRequest = new()
            {
                PageIndex = 1,
                PageSize = 10,
                SearchString = ""
            };

            // Mock IMapper
            Mock<IMapper> mapperMock = new();
            _ = mapperMock.Setup(m => m.Map<List<ReviewResponseModel>>(It.IsAny<List<Review>>()))
                .Returns((List<Review> reviews) => reviews.Select(r => new ReviewResponseModel
                {
                    Id = r.Id,
                    StoreId = r.StoreId,
                    Content = r.Content,
                    RatingValue = r.RatingValue,
                    // Map other properties as needed
                }).ToList());

            using (AppDbContext dbContext = new(options))
            {
                ReviewService reviewService = new(dbContext, mapperMock.Object);

                // Act
                BasePaginationResult<ReviewResponseModel> result = await reviewService.GetReviewsByStoreIdAsync(filterRequest, paginationRequest);

                // Print debug information
                Console.WriteLine($"Total Count: {result.TotalCount}");
                if (result.Data != null)
                {
                    foreach (ReviewResponseModel review in result.Data)
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
                Assert.Equal(2, result.Data!.Count(x => x.RatingValue == 4));
            }
        }
    }
}