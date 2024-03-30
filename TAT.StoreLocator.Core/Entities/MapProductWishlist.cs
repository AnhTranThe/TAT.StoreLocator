using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapProductWishlists")]
    public class MapProductWishlist
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? WishlistId { get; set; }

        public Wishlist Wishlist { get; set; } = new Wishlist();

        public string? ProductId { get; set; }

        public Product Product { get; set; } = new Product();
    }
}
