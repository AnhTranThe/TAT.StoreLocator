using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Domain.Entities
{
    [Table("MapProductGalleries")]
    public class MapProductGallery
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid ProductId { get; set; }

        public Product? Product { get; set; }

        [Required]
        public Guid GalleryId { get; set; }

        public Gallery? Gallery { get; set; }

        public bool IsThumbnail { get; set; }
    }

}
