using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("MapStoreWishlists")]
    public class MapStoreWishlist
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WishlistId { get; set; }

        public Wishlist Wishlist { get; set; } = new Wishlist();

        public Guid StoreId { get; set; }

        public Store Store { get; set; } = new Store();

    }
}
