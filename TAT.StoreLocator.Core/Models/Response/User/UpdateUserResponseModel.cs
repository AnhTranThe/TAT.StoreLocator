using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.User
{
    public class UpdateUserResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public UserResponseModel OldUserResponseModel { get; set; } = new UserResponseModel();
        public UserResponseModel UpdatedUserResponseModel { get; set; } = new UserResponseModel();
    }
}