using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Galleries")]
    public class Gallery : BaseEntity
    {
        public string? FileName { get; set; }
        public string? Url { get; set; }
        public string? PublicId { get; set; }
        public bool IsThumbnail { get; set; } = false;
        public string? FileBelongsTo { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public Category? Category { get; set; }

        public virtual ICollection<MapGalleryProduct>? MapGalleryProducts { get; set; }
        public virtual ICollection<MapGalleryStore>? MapGalleryStores { get; set; }
    }
}