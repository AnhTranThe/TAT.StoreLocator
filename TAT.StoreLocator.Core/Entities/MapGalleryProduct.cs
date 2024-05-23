using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapGalleryProducts")]
    public class MapGalleryProduct
    {
        public string? GalleryId { get; set; }

        public Gallery? Gallery { get; set; }

        public string? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}