using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.Photo;
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Response.Gallery;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadImage(IFormFile formFile, bool profile);
        Task<DeletionResult?> DeleteImageCloudinary(string publicId);
        Task<DeletionResult?> DeleteDbAndCloudAsyncResult(Guid galleryId, string fileBelongTo, string publicId);

        Task<BasePaginationResult<GalleryResponseModel>> GetListImagesAsync(BasePaginationRequest request);
        Task<BasePaginationResult<GalleryResponseModel>> GetListImagesById(GetListPhotoByIdRequestModel request);

        Task<BaseResponse> CreateImage(UploadPhotoRequestModel request);
        Task<BaseResponse> UpdateImage(string Id, UpdatePhotoRequestModel request);
        Task<BaseResponse> RemoveImage(string Id, DeletePhotoRequestModel request);

    }
}