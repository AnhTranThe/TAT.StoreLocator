using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.API.Controllers;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Response.User;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.UserServiceTest
{
    public class GetByUserIdServiceTest
    {
        [Fact]
        public async Task GetUserById_Returns_OkResult_With_UserData()
        {
            // Arrange
            var userId = "1"; // Đây là ID của người dùng mà bạn muốn kiểm tra
            var expectedUserResponse = new BaseResponseResult<UserResponseModel>
            {
                Success = true,
                Data = new UserResponseModel
                {
                    Id = userId,
                    UserName = "testuser",
                    Email = "testuser@example.com",
                    Roles = new[] { "Role1", "Role2" } // Các vai trò của người dùng
                }
            };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetById(userId)).ReturnsAsync(expectedUserResponse);

            var userController = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await userController.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var resultData = Assert.IsAssignableFrom<BaseResponseResult<UserResponseModel>>(okResult.Value);
            Assert.True(resultData.Success);
            Assert.NotNull(resultData.Data);
            Assert.Equal(expectedUserResponse.Data.Id, resultData.Data.Id);
            // Kiểm tra các thuộc tính khác nếu cần
        }

        [Fact]
        public async Task GetUserById_Returns_BadRequest_When_UserNotFound()
        {
            // Arrange
            var userId = "nonexistentId"; // ID không tồn tại
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetById(userId)).ReturnsAsync(new BaseResponseResult<UserResponseModel> { Success = false, Message = "User not found" });
            var userController = new UserController(userServiceMock.Object);

            // Act
            var actionResult = await userController.GetUserById(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            var resultData = Assert.IsAssignableFrom<string>(badRequestResult.Value);
            Assert.Equal("User not found", resultData);
        }
    }
}
