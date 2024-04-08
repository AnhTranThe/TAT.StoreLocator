namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class UpdateJwtUserInfoRequestModel
    {
        public string? UserId { get; set; }
        public string? RefreshToken { get; set; }

    }
}
