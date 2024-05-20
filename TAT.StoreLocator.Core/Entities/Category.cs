using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; } = true;
        // relationship
        public string? ParentCategoryId { get; set; } = string.Empty;
        public Category? ParentCategory { get; set; }
        public Gallery? Gallery { get; set; }
        public string? GalleryId { get; set; }
        public virtual ICollection<Category>? ChildrenCategories { get; set; }
        public virtual ICollection<Product>? Products { get; set; }


    }
}
