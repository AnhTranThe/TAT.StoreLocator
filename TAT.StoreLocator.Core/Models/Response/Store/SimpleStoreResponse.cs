namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class SimpleStoreResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressResponseModel? Address { get; set; }
    }
}
