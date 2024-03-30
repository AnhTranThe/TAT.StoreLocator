using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Locations")]
    public class Location : BaseEntity
    {
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public Address? Address { get; set; }
        [Required]
        public string? AddressId { get; set; }

    }
}
