namespace TAT.StoreLocator.Core.Models.Request.Photo
{
    public class PhotoRequestModel
    {
        public bool IsThumbnail { get; set; } = false;
        public string Type { get; set; } = string.Empty;
        public string StoreId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        //public string FileName { get; set; } = string.Empty;
        //public string? Url { get; set; } = string.Empty;
        //public string? PublicId { get; set; } = string.Empty;
        //public string? FileBelongsTo { get; set; } = string.Empty;
    }
}
