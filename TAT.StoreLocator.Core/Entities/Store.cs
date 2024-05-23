using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Stores")]
    public class Store : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public Address? Address { get; set; }
        public string? AddressId { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<MapStoreWishlist>? MapStoreWishlists { get; set; }
        public virtual ICollection<MapGalleryStore>? MapGalleryStores { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}