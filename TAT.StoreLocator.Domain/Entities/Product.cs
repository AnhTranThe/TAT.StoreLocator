using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Note { get; set; }
        public string? Slug { get; set; }
        public string? Brand { get; set; }
        public string? Thumb { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal Discount { get; set; } // in percent
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; } = 0;
        public string? SKU { get; set; }
        [NotMapped]
        public bool IsAvailable => Quantity > 0;
        public bool IsActive { get; set; } = true;
        public bool IsDiscountAllowed { get; set; } = true;
        public int ProductViewCount { get; set; } = 0;


        // relationship
        public Guid? CategoryId { set; get; }
        public Category? Category { get; set; }

        public virtual ICollection<MapProductGallery>? MapProductGalleries { get; set; }

        public virtual ICollection<MapProductWishlist>? MapProductWishlists { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }

        public Guid StoreId { get; set; }
        public Store Store { get; set; } = new Store();



    }
}
