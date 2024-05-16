using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapProductWishlists")]
    public class MapProductWishlist
    {

        public string? WishlistId { get; set; }

        public Wishlist? Wishlist { get; set; }

        public string? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
