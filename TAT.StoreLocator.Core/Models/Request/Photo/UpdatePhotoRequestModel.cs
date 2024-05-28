namespace TAT.StoreLocator.Core.Models.Request.Photo
{
    public class UpdatePhotoRequestModel
    {
        public bool IsThumbnail { get; set; } = false;
        public string Type { get; set; } = string.Empty;
        public string TypeId { get; set; } = string.Empty;

    }
}
