using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAT.StoreLocator.Core.Models.Request.Gallery
{
    public class GalleryRequestModel
    {
        public string? FileName { get; set; }
        public string? Url { get; set; }
        public string? PublicId { get; set; }
        public bool IsThumbnail { get; set; }
        public string? FileBelongsTo { get; set; }

        public string? UserId { get; set; }
    }
}
