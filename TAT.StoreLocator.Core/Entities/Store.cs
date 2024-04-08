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
        public Address? Address { get; set; }
        public virtual ICollection<Gallery>? Galleries { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<MapStoreWishlist>? MapStoreWishlists { get; set; }
    }
}
