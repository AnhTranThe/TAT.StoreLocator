﻿using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Review
{
    public class GetReviewByUserIdResponseModel
    {
        public BasePaginationRequest PaginationRequest { get; set; } = new BasePaginationRequest();
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public List<ReviewResponseModel> ReviewResponseModel { get; set; } = new List<ReviewResponseModel>();
        public int TotalCount { get; set; }
    }
}