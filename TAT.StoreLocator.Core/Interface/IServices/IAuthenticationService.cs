using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Authentication;
using TAT.StoreLocator.Core.Models.Response.Authentication;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IAuthenticationService
    {
        Task<RegisterResponseModel> RegisterUserAsync(RegisterRequestModel model);
        Task<LoginResponseModel> LoginUserAsync(LoginRequestModel model);
        Task<BaseResponse> LogoutUserAsync(string UserId);


    }
}
