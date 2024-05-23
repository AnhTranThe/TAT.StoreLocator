using Microsoft.AspNetCore.Http;

namespace TAT.StoreLocator.Core.Models.Request.Photo
{
    public class UploadPhotoRequestModel
    {
        public IFormFile? FileUpload { get; set; }
        public string? UserId { get; set; }
    }
}