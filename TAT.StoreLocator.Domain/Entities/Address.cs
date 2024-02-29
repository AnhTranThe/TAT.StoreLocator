using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("Addresses")]
    public class Address : BaseEntity
    {
        public string? RoadName { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? PostalCode { get; set; }
        public User? User { get; set; }
        public Store? Store { get; set; }
        public Location? Location { get; set; }


    }
}
