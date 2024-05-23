﻿using Microsoft.AspNetCore.Http;

namespace TAT.StoreLocator.Core.Models.Request.Product
{
    public class PhotoProductRequestModel
    {
        public IFormFile? FileUpload { get; set; }
        public bool IsThumbnail { get; set; } = false;
    }
}