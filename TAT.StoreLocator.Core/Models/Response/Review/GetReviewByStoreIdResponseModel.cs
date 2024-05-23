using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Review
{
    public class GetReviewByStoreIdResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public ReviewResponseModel ReviewResponseModel { get; set; } = new ReviewResponseModel();   
        public BasePaginationRequest PaginationRequest { get; set; }    = new BasePaginationRequest();
        public int TotalCount { get; set; } 
    }
}
