using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Models.Response.Authentication
{
    public class RegisterResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public UserResponseModel UserResponseModel { get; set; } = new UserResponseModel();
    }
}