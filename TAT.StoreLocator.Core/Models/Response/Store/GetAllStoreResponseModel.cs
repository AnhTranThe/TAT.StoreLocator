using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class GetAllStoreResponseModel
    {
        public BasePaginationRequest PaginationRequest { get; set; } = new BasePaginationRequest();
        public List<StoreResponseModel> StoreResponseList { get; set; } = new List<StoreResponseModel>();
        public int TotalCount { get; set; }
    }
}
