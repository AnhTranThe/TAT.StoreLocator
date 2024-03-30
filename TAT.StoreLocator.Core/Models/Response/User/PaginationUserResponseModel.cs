using TAT.StoreLocator.Core.Models.Pagination;

namespace TAT.StoreLocator.Core.Models.Response.User
{
    public class PaginationUserResponseModel : PaginationResponseModel
    {
        public List<UserResponseModel>? userResponseModels { get; set; } = new List<UserResponseModel>();
    }
}
