using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class SearchUserPagingRequestModel : BasePaginationRequest
    {
        public string? searchValue { get; set; }
    }
}