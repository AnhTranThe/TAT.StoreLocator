using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapStoreWishlists")]
    public class MapStoreWishlist
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? WishlistId { get; set; }

        public Wishlist? Wishlist { get; set; }

        public string? StoreId { get; set; }

        public Store? Store { get; set; }

    }
}
