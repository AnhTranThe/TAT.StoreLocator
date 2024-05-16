using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadImage(IFormFile formFile, bool profile);
        Task<DeletionResult?> DeleteImage(string publicId);
    }
}
