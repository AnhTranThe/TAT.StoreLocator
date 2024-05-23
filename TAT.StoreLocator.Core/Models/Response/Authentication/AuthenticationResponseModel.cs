namespace TAT.StoreLocator.Core.Models.Response.Authentication
{
    public class AuthenticationResponseModel
    {
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
    }
}