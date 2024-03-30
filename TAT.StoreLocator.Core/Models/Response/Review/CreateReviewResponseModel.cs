using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Review
{
    public class CreateReviewResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();

        public ReviewResponseModel ReviewResponseModel { get; set; } = new ReviewResponseModel();

    }
}
