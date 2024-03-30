using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IMapper;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Role;
using TAT.StoreLocator.Core.Models.Request.User;
using TAT.StoreLocator.Core.Models.Response.Role;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Core.Models.Response.UserRole;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseMapper<User, UserResponseModel> _userModelMapper;
        private readonly IBaseMapper<UserResponseModel, User> _userMapper;


        public UserService(UserManager<User> userManager,
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IBaseMapper<User, UserResponseModel> userModelMapper,
            IBaseMapper<UserResponseModel, User> userMapper)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userModelMapper = userModelMapper;
            _userMapper = userMapper;

        }

        public async Task<AssignRoleResponseModel> AssignRoleAsync(AssignRoleRequestModel model)
        {
            AssignRoleResponseModel response = new();
            BaseResponse baseResponse = response.BaseResponse;
            baseResponse.Success = false;
            try
            {

                bool isUserInThisRole = await _context.UserRoles
             .AnyAsync(x => x.RoleId == model.RoleId && x.UserId == model.UserId);

                if (isUserInThisRole)
                {
                    baseResponse.Message = "User already in this role";
                    return response;
                }
                BaseRequest baseRequest = new()
                {
                    RequestId = model.UserId
                };

                GetUserResponseModel user = await GetUserAsync(baseRequest);

                if (user == null)
                {
                    baseResponse.Message = "User not found";
                    return response;
                }

                Role? role = await _context.Roles
                    .FirstOrDefaultAsync(x => x.Id == model.RoleId);

                if (role == null)
                {
                    baseResponse.Message = "Role not found";
                    return response;

                }

                IdentityUserRole<string> userRole = new()
                {
                    RoleId = model.RoleId ?? "",
                    UserId = model.UserId ?? ""
                };

                _ = await _context.UserRoles.AddAsync(userRole);

                _ = await _context.SaveChangesAsync();

                baseRequest.RequestId = model.UserId;

                UserRoleResponseModel userModel = await GetUserWithRolesAsync(baseRequest);



                baseResponse.Success = true;


            }
            catch (Exception ex)
            {
                baseResponse.Message = $"An error occurred while check existed the entity: {ex.Message}";
            }

            return response;


        }

        public Task<DeleteUserResponseModel> DeleteUserAsync(BaseRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<EditUserResponseModel> EditUserAsync(EditUserRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationUserResponseModel> GetAllAsync(PaginationUserRequestModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<GetUserResponseModel> GetUserAsync(BaseRequest model)
        {

            GetUserResponseModel response = new();
            BaseResponse baseResponse = response.BaseResponse;
            baseResponse.Success = false;
            try
            {

                User? user = await _context.Users
              .FirstOrDefaultAsync(x => x.Id == model.RequestId && x.IsActive);

                if (user == null)
                {
                    baseResponse.Message = "User not found";
                }
                else
                {
                    baseResponse.Success = true;
                    response.userResponseModel = _mapper.Map<UserResponseModel>(user);

                }

            }
            catch (Exception ex)
            {
                baseResponse.Message = $"An error occurred while check existed the entity: {ex.Message}";
            }

            return response;



        }

        public UserResponseModel GetUserInfoFromJwt()
        {
            throw new NotImplementedException();
        }

        public Task<UserRoleResponseModel> GetUserWithRolesAsync(BaseRequest model)
        {
            throw new NotImplementedException();
        }

        public bool IsUserAdmin()
        {
            throw new NotImplementedException();
        }

        public Task<RemoveFromRoleResponseModel> RemoveFromRoleAsync(RemoveFromRoleRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<UndeleteUserResponseModel> UndeleteUserAsync(BaseRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
