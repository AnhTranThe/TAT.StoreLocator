using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Authentication
{
    public class JwtTokenResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset TokenCreatedDate { get; set; }
        public DateTimeOffset TokenExpiredDate { get; set; }
    }
}
