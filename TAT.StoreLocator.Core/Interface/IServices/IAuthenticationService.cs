using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.User;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IAuthenticationService
    {
        Task<LoginUserResponseModel> ValidatePasswordAsync(LoginUserRequestModel model);

        Task<ChangePasswordUserResponseModel> ChangePasswordAsync(ChangePasswordRequestModel model);

        Task<RegisterUserResponseModel> RegisterUserAsync(RegisterUserRequestModel model);



    }
}
