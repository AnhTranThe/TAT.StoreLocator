using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.Test.ServiceTest.UserServiceTest
{
    public class GetListUserServiceTest
    {
        [Fact]
        public async Task GetListUserAsync_Returns_PaginationResult_With_UserList()
        {
            // Arrange
            var expectedUserList = new BasePaginationResult<UserResponseModel>
            {
                TotalCount = 2,
                PageIndex = 1,
                PageSize = 10,
                Data = new List<UserResponseModel>
                {
                    new UserResponseModel {Id = "1", UserName = "user1", Email = "user1@example.com"},
                    new UserResponseModel {Id = "2", UserName = "user2", Email = "user2@example.com"}
                }
            };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetListUserAsync(It.IsAny<BasePaginationRequest>())).ReturnsAsync(expectedUserList);

            var userService = userServiceMock.Object;

            // Act
            var request = new BasePaginationRequest();
            var result = await userService.GetListUserAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserList.TotalCount, result.TotalCount);
            Assert.Equal(expectedUserList.PageIndex, result.PageIndex);
            Assert.Equal(expectedUserList.PageSize, result.PageSize);
            Assert.Equal(expectedUserList.Data.Count, result.Data.Count);
        }
    }
}