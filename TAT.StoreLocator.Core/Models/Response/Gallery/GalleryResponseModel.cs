﻿using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Response.Gallery
{
    public class GalleryResponseModel
    {
        [Required]
        public string? FileName { get; set; }
        [Required]
        public string? Url { get; set; }
        public bool IsThumbnail { get; set; }
        public string? FileBelongsTo { get; set; }


    }
}
