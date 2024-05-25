using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Photo
{
    public class DeleteFileRequest
    {
        [Required(ErrorMessage = "Gallery ID is required to delete a file.")]
        public Guid GalleryId { get; set; }

        [Required(ErrorMessage = "FileBelongTo property is required.")]
        [RegularExpression(@"^(Store|Product)$", ErrorMessage = "FileBelongTo must be either 'Store' or 'Product'.")]
        public string FileBelongTo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Public ID is required to identify the file for deletion.")]
        public string PublicId { get; set; } = string.Empty;

    }
}
