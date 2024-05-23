using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Note { get; set; }
        public string? Slug { get; set; }

        public decimal Price { get; set; } = 0;
        public decimal Discount { get; set; } = 0; // in percent
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int Quantity { get; set; } = 0;
        public double Rating { get; set; } = 0;
        public string? SKU { get; set; }
        public bool IsActive { get; set; } = true;
        public int ProductViewCount { get; set; } = 0;

        // relationship

        public string? CategoryId { set; get; }
        public Category? Category { get; set; }

        public virtual ICollection<Gallery>? Galleries { get; set; }
        public virtual ICollection<MapProductWishlist>? MapProductWishlists { get; set; }

        public virtual ICollection<MapGalleryProduct>? MapGalleryProducts { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }

        public string? StoreId { get; set; }
        public Store? Store { get; set; }
    }
}