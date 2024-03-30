using System.ComponentModel.DataAnnotations;
using static TAT.StoreLocator.Core.Helpers.Enums;

namespace TAT.StoreLocator.Core.Models.Response.Gallery
{
    public class GalleryResponseModel
    {
        [Required]
        public string? FileName { get; set; }
        [Required]
        public string? Url { get; set; }
        public bool IsThumbnail { get; set; }
        public EUploadFileStatus? FileStatus { get; set; }
        public string? FileBelongsTo { get; set; }



    }
}
