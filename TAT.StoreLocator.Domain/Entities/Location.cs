using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("Locations")]
    public class Location : BaseEntity
    {
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public Address? Address { get; set; }
        public Guid AddressId { get; set; }

    }
}
