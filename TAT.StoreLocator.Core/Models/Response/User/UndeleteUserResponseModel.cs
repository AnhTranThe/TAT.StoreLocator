﻿using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.User
{
    public class UnDeleteUserResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public UserResponseModel userResponseModel { get; set; } = new UserResponseModel();
    }
}