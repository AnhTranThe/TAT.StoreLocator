using System.ComponentModel.DataAnnotations;
using TAT.StoreLocator.Core.Models.Response.Product;

namespace TAT.StoreLocator.Core.Models.Response.Category
{
    public class CategoryResponseModel
    {
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public string? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public IList<ProductResponseModel> productResponseModels { get; set; } = new List<ProductResponseModel>();


    }
}
