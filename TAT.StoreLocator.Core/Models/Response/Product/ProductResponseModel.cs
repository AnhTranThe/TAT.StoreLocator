using TAT.StoreLocator.Core.Models.Response.Category;
using TAT.StoreLocator.Core.Models.Response.Gallery;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.Store;

namespace TAT.StoreLocator.Core.Models.Response.Product
{
    public class ProductResponseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Note { get; set; }
        public string? Slug { get; set; }

        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; }
        public string? SKU { get; set; }
        public bool IsActive { get; set; } = true;
        public int ProductViewCount { get; set; }

        public string? StoreId { get; set; }
        public StoreOfProductResponseModel? Store { get; set; }
        public string? CategoryId { set; get; }
        public CategoryProductResponseModel? Category { get; set; }
        public List<GalleryResponseModel>? GalleryResponseModels { get; set; }
        public List<ReviewResponseModel>? Reviews { get; set; }
    }
}