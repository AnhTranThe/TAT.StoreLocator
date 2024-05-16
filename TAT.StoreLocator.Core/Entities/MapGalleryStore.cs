using System.ComponentModel.DataAnnotations.Schema;

namespace TAT.StoreLocator.Core.Entities
{
    [Table("MapGalleryStores")]
    public class MapGalleryStore
    {

        public string? GalleryId { get; set; }

        public Gallery? Gallery { get; set; }

        public string? StoreId { get; set; }

        public Store? Store { get; set; }
    }
}
