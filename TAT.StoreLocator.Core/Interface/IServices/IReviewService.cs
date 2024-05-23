using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Models.Response.Review;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IReviewService
    {
        // Create
        Task<BaseResponseResult<ReviewResponseModel>> CreateReviewAsync(CreateReviewRequestModel request);

        //Update
        Task<BaseResponseResult<ReviewResponseModel>> UpdateReviewAsync(string reviewId, UpdateReviewRequestModel request);

        // GetReviewByUserId
        Task<BasePaginationResult<ReviewResponseModel>> GetReviewByUserIdAsync(string userId, BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest);

        //GetReviewByStoreId
        Task<BasePaginationResult<ReviewResponseModel>> GetReviewByStoreIdAsync(string storeId, BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest);
    }
}