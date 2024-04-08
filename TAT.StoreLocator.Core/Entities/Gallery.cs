using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Core.Common;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("Galleries")]
    public class Gallery : BaseEntity
    {
        [Required]
        public string? FileName { get; set; }
        [Required]
        public string? Url { get; set; }
        public bool IsThumbnail { get; set; } = false;
        public string? FileBelongsTo { get; set; }
        public EUploadFileStatus? FileStatus { get; set; } = EUploadFileStatus.Active;
        public Store? Store { get; set; }
        public string? StoreId { get; set; }

        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
        public Product? Product { get; set; }
        public string? ProductId { get; set; }

    }
}
