namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class SimpleStoreResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressResponseModel? Address { get; set; }
        public List<SimpleProductResponse> Products { get; set; } = new List<SimpleProductResponse>();
    }

    public class SimpleProductResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
    }
}