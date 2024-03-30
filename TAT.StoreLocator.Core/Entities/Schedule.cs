using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Entities
{

    [Table("Schedules")]
    public class Schedule : BaseEntity
    {
        public Store? Store { get; set; }
        [Required]
        public string? StoreId { get; set; }
        public EDayOfWeek DayOfWeek { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

    }
}
