using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Infrastructure.Persistence.EF;


namespace TAT.StoreLocator.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ReviewService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<BaseResponseResult<ReviewResponseModel>> CreateReviewAsync(CreateReviewRequestModel request)
        {
            Review? existingReview = await _appDbContext.Reviews
                  .Include(r => r.Product)
                      .ThenInclude(p => p.Store)
               .FirstOrDefaultAsync(r => r.UserId == request.UserId && r.Product.StoreId == request.StoreId);


            if (existingReview != null)
            {
                return new BaseResponseResult<ReviewResponseModel>()
                {
                    Success = false,
                    Message = "User has already reviewed this store",
                    Errors = new List<string> { "User has already reviewed this store" }
                };
            }
            Review? review = _mapper.Map<Review>(request);
            _ = _appDbContext.Reviews.Add(review);
            _ = await _appDbContext.SaveChangesAsync();

            review = await _appDbContext.Reviews
               .Include(r => r.Product)
               .ThenInclude(p => p.Store)
               .FirstOrDefaultAsync(r => r.Id == review.Id);


            ReviewResponseModel responseModel = _mapper.Map<ReviewResponseModel>(review);
            return new BaseResponseResult<ReviewResponseModel>
            {
                Success = true,
                Data = responseModel
            };
        }

        public async Task<BaseResponseResult<ReviewResponseModel>> UpdateReviewAsync(string reviewId, UpdateReviewRequestModel request)
        {
            BaseResponseResult<ReviewResponseModel> response = new();
            try
            {
                Review? review = await _appDbContext.Reviews
                    .Include(r => r.Product)
                        .ThenInclude(p => p.Store)
                      .FirstOrDefaultAsync(r => r.Id == reviewId);

                if (review == null)
                {
                    response.Success = false;
                    response.Message = "Can not found review";
                    return response;
                }

                // Sử dụng AutoMapper để map các thuộc tính từ request đến thực thể review
                _ = _mapper.Map(request, review);
                //review.UpdatedBy = request.UpdatedBy;

                _ = await _appDbContext.SaveChangesAsync();
                ReviewResponseModel updateReviewResponse = new()
                {
                    Id = reviewId,
                    Content = review.Content,
                    RatingValue = review.RatingValue,
                    Status = review.Status,
                    UserId = request.UserId,
                    //     ProductId = review.ProductId,
                    Store = new StoreResponse
                    {
                        Id = review.Product?.Store?.Id,
                        Name = review.Product?.Store?.Name,
                        Email = review.Product?.Store?.Email,
                        PhoneNumber = review.Product?.Store?.PhoneNumber
                    },
                    CreatedAt = review.CreatedAt.ToString(),
                    CreatedBy = review.CreatedBy,
                    UpdatedAt = review.UpdatedAt.ToString(),
                    //UpdatedBy = review.UpdatedBy
                };
                response.Success = true;
                response.Data = updateReviewResponse;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message += ex.Message;
            }
            return response;
        }
    }
}
