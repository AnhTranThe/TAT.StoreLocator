namespace TAT.StoreLocator.Core.Models.Request.Store
{
    public class GetNearStoreRequestModel
    {
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string keyWord { get; set; } = string.Empty;
    }
}