using Microsoft.AspNetCore.Http;

namespace TAT.StoreLocator.Core.Models.Request.Product
{
    public class UploadPhotoRequestModel
    {
        public List<IFormFile>? FileUpload { get; set; }
        public bool IsThumbnail { get; set; } = false;
        public string Type { get; set; } = string.Empty;
        public string TypeId { get; set; } = string.Empty;
    }
}