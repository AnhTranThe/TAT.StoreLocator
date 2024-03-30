using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapProductGalleries")]
    public class MapProductGallery
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string? ProductId { get; set; }

        public Product? Product { get; set; }

        [Required]
        public string? GalleryId { get; set; }

        public Gallery? Gallery { get; set; }


    }

}
