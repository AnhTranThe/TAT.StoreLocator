using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapStoreWishlists")]
    public class MapStoreWishlist
    {

        public string? WishlistId { get; set; }

        public Wishlist? Wishlist { get; set; }

        public string? StoreId { get; set; }

        public Store? Store { get; set; }

    }
}
