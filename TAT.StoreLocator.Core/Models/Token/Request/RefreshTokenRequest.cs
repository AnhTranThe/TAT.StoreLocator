namespace TAT.StoreLocator.Core.Models.Token.Request
{
    public class RefreshTokenRequest
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
