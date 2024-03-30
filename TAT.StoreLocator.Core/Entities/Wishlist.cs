using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Wishlists")]
    public class Wishlist : BaseEntity
    {
        [Required]
        public string? UserId { get; set; }

        public User User { get; set; } = new User();

        public virtual ICollection<MapProductWishlist>? MapProductWishlists { get; set; }// this wishlist contains these products

        public virtual ICollection<MapStoreWishlist>? MapStoreWishlists { get; set; }// this wishlist contains these products
    }
}
