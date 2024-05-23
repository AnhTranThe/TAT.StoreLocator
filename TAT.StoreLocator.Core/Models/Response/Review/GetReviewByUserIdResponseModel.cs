using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Review
{
    public class GetReviewByUserIdResponseModel
    {
        public BasePaginationRequest PaginationRequest { get; set; } = new BasePaginationRequest();
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public List <ReviewResponseModel> ReviewResponseModel { get; set; } = new  List <ReviewResponseModel>();
        //public ReviewResponseModel ReviewResponseModel { get; set; }   = new ReviewResponseModel();
        public int TotalCount { get; set; }
    }
}
