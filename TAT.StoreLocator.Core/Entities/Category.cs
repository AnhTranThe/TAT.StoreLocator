using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; } = new Category();
        [Required]
        public string? GalleryId { get; set; }
        public Gallery Gallery { get; set; } = new Gallery();

        public virtual ICollection<Category>? ChildrenCategories { get; set; }

        public virtual ICollection<Product>? Products { get; set; }



    }
}
