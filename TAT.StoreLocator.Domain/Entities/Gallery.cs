using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TAT.StoreLocator.Domain.Common;
using static TAT.StoreLocator.Domain.Helpers.Enums;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("Galleries")]
    public class Gallery : BaseEntity
    {


        [Required]
        public string? Url { get; set; }
        public EUploadFileStatus? FileStatus { get; set; } = EUploadFileStatus.Active;
        public ICollection<Category>? Categories { get; set; } //Categories which has this image as cover
        public ICollection<MapProductGallery>? MapProductGalleries { get; set; } //Categories which has this image as cover


    }
}
