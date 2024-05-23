namespace TAT.StoreLocator.Core.Models.Request.Store
{
    public class UpdateStoreRequestModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsDelete { get; set; }
        public AddressStoreRequestModel? Address { get; set; }
    }
}
