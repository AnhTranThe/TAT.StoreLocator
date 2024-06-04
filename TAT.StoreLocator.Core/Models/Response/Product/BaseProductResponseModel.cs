namespace TAT.StoreLocator.Core.Models.Response.Product
{
    public class BaseProductResponseModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
}