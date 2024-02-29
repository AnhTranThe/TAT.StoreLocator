using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Domain.Common
{
    /// <summary>
    /// BaseEntity
    /// </summary>
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }


    }
}
