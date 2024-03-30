using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.User
{
    public class DeleteUserResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public UserResponseModel DeleteUserResponse { get; set; } = new UserResponseModel();

    }
}
