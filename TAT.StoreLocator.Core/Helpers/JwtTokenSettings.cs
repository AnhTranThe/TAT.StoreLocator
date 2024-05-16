namespace TAT.StoreLocator.Core.Helpers
{
    public class JwtTokenSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public int ExpireInMinutes { get; set; }
    }
}
