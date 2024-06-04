using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Models.Response.Review;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IReviewService
    {
        // Create
        Task<BaseResponse> CreateReviewAsync(ReviewRequestModel request);

        //Update
        Task<BaseResponseResult<ReviewResponseModel>> UpdateReviewAsync(string reviewId, ReviewRequestModel request);

        // GetReviewByUserId
        Task<BasePaginationResult<ReviewResponseModel>> GetReviewsByUserIdAsync(BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest);

        //GetReviewByStoreId
        Task<BasePaginationResult<ReviewResponseModel>> GetReviewsByStoreIdAsync(BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest);

        Task<BaseResponseResult<ReviewResponseModel>> GetReviewByUserAndStoreAsync(GetReviewByUserAndStoreRequestModel request);
    }
}