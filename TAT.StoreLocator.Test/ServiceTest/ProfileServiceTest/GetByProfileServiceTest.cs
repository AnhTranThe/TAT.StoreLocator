using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using Xunit;

namespace TAT.StoreLocator.Test.ServiceTest.ProfileServiceTest
{
    public class GetProfileServiceTest
    {

        [Fact]
        public void GetGuidUserIdLogin_WhenHttpContextUserIsNotNull_ReturnsUserId()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(UserClaims.Id, "1"), // assuming UserClaims.Id is the claim type for user id
            }));

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var profileService = new ProfileService(null, null, null, httpContextAccessorMock.Object, null);

            // Act
            var userId = profileService.GetGuidUserIdLogin();

            // Assert
            Assert.Equal("1", userId);
        }


    }
}


