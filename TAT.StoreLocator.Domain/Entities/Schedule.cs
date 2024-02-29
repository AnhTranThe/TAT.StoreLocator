using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;
using static TAT.StoreLocator.Domain.Helpers.Enums;

namespace TAT.StoreLocator.Domain.Entities
{

    [Table("Schedules")]
    public class Schedule : BaseEntity
    {
        public Store? Store { get; set; }
        public Guid StoreId { get; set; }
        public EDayOfWeek DayOfWeek { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

    }
}
