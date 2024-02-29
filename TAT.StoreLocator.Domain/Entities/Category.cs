using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HomeFlag { get; set; } = false;

        // relationship
        public Guid ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; } = new Category();
        public Guid GalleryId { get; set; }
        public Gallery Gallery { get; set; } = new Gallery();

        public virtual ICollection<Category>? ChildrenCategories { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

    }
}
