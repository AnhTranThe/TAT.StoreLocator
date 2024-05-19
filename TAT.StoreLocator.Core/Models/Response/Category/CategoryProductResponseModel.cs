using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Response.Category
{
    public class CategoryProductResponseModel
    {
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public string? ParentCategoryId { get; set; }
        public string? GalleryId { get; set; }
    }
}