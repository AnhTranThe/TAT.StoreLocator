using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Addresses")]
    public class Address : BaseEntity
    {
        public string? RoadName { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? PostalCode { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string? StoreId { get; set; }
        public Store? Store { get; set; }


    }
}
