using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("Wishlists")]
    public class Wishlist : BaseEntity
    {

        public Guid UserId { get; set; }

        public User User { get; set; } = new User();

        public virtual ICollection<MapProductWishlist>? MapProductWishlists { get; set; }// this wishlist contains these products

        public virtual ICollection<MapStoreWishlist>? MapStoreWishlists { get; set; }// this wishlist contains these products
    }
}
