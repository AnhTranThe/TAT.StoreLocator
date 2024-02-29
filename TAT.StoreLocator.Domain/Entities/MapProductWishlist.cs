using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("MapProductWishlists")]
    public class MapProductWishlist
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid WishlistId { get; set; }

        public Wishlist Wishlist { get; set; } = new Wishlist();

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = new Product();
    }
}
