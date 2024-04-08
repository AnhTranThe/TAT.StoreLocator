using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class GetUserPagingRequestModel : BasePaginationRequest
    {
        public string? keyword { get; set; }
    }
}
