using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapStoreWishlists")]
    public class MapStoreWishlist
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string? WishlistId { get; set; }

        public Wishlist Wishlist { get; set; } = new Wishlist();
        [Required]
        public string? StoreId { get; set; }

        public Store Store { get; set; } = new Store();

    }
}
