using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Photo;
using TAT.StoreLocator.Core.Models.Response.Gallery;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly AppDbContext _appDbContext;


        public PhotoService(IOptions<CloudinarySettings> config, AppDbContext appDbContext)
        {
            Account account = new(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _appDbContext = appDbContext;
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile formFile, bool profile)
        {
            ImageUploadResult uploadResult = new();
            CloudinaryDotNet.Transformation transformation = profile
                ? new CloudinaryDotNet.Transformation().Width(500).Height(500).Crop("fill").Gravity(Gravity.Face)
                : new CloudinaryDotNet.Transformation().Height(512).Crop("fit");
            if (formFile.Length > 0)
            {
                await using Stream stream = formFile.OpenReadStream();
                ImageUploadParams uploadParams = new()
                {
                    File = new FileDescription(formFile.FileName, stream),
                    Transformation = transformation,
                    Folder = "TAT.StoreLocator"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult?> DeleteImageCloudinary(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                return null;
            }

            DeletionParams deleteParams = new(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task DeleteDbAndCloudAsync(Guid galleryId, string fileBelongTo, string url)
        {
            switch (fileBelongTo)
            {
                case "Product":
                    MapGalleryProduct? mapGalleryProduct = _appDbContext.mapGalleryProducts
                        .FirstOrDefault(x => x.GalleryId == galleryId.ToString());
                    if (mapGalleryProduct != null)
                    {
                        _ = _appDbContext.mapGalleryProducts.Remove(mapGalleryProduct);
                    }
                    break;

                case "Store":
                    MapGalleryStore? mapGalleryStore = _appDbContext.MapGalleryStores
                        .FirstOrDefault(x => x.GalleryId == galleryId.ToString());
                    if (mapGalleryStore != null)
                    {
                        _ = _appDbContext.MapGalleryStores.Remove(mapGalleryStore);
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid value for FilebeLongto");
            }

            Gallery? gallery = _appDbContext.Galleries
                .FirstOrDefault(x => x.Id == galleryId.ToString());
            if (gallery != null)
            {
                _ = _appDbContext.Galleries.Remove(gallery);
            }

            _ = await _appDbContext.SaveChangesAsync();
            if (url != null)
            {
                _ = await DeleteImageCloudinary(url);
            }
        }

        public async Task<BasePaginationResult<GalleryResponseModel>> GetListImagesAsync(BasePaginationRequest request)
        {

            IQueryable<Gallery> query = _appDbContext.Galleries;

            int totalRow = await query.CountAsync();


            List<GalleryResponseModel> data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new GalleryResponseModel()
                {
                    Id = e.Id,
                    FileName = e.FileName,
                    Url = e.Url,
                    FileBelongsTo = e.FileBelongsTo,
                    IsThumbnail = e.IsThumbnail

                }).ToListAsync();

            BasePaginationResult<GalleryResponseModel> response = new()
            {
                TotalCount = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Data = data
            };

            return response;
        }

        public async Task<BasePaginationResult<GalleryResponseModel>> GetListImagesById(GetListPhotoByIdRequestModel request)
        {

            IQueryable<Gallery> query = _appDbContext.Galleries;
            if (request.Type == "store" && !string.IsNullOrEmpty(request.Id))
            {
                query = query.Include(g => g.MapGalleryStores)
                             .Where(g => g.MapGalleryStores != null && g.MapGalleryStores.Any(mgs => mgs.StoreId == request.Id));
            }
            else if (request.Type == "product" && !string.IsNullOrEmpty(request.Id))
            {
                query = query.Include(g => g.MapGalleryProducts)
                             .Where(g => g.MapGalleryProducts != null && g.MapGalleryProducts.Any(mgp => mgp.ProductId == request.Id));
            }

            int totalRow = await query.CountAsync();


            List<GalleryResponseModel> data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new GalleryResponseModel()
                {
                    Id = e.Id,
                    FileName = e.FileName,
                    Url = e.Url,
                    FileBelongsTo = e.FileBelongsTo,
                    IsThumbnail = e.IsThumbnail

                }).ToListAsync();

            BasePaginationResult<GalleryResponseModel> response = new()
            {
                TotalCount = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Data = data
            };

            return response;
        }

        public async Task<BaseResponse> UpdateImage(string Id, PhotoRequestModel request)
        {
            BaseResponse response = new() { Success = false };
            try
            {
                // Retrieve the image from the database based on the provided Id
                if (string.IsNullOrWhiteSpace(Id))
                {
                    response.Message = GlobalConstants.ID_NOT_FOUND;
                    return response;
                }
                if (_appDbContext == null)
                {
                    response.Message = GlobalConstants.DB_NOT_FOUND;
                    return response;
                }

                Gallery? image = await _appDbContext.Galleries.FindAsync(Id);

                if (image == null)
                {

                    response.Message = "Image not found";
                    return response;
                }

                // Update the IsThumbnail property of the retrieved image if necessary
                if (request.IsThumbnail != image.IsThumbnail)
                {
                    image.IsThumbnail = request.IsThumbnail;
                }

                // Check if Name or Url has changed
                if (request.FileName != null && request.FileName != image.FileName)
                {
                    image.FileName = request.FileName;
                }

                if (request.Url != null && request.Url != image.Url)
                {
                    image.Url = request.Url;
                }

                // Save the changes to the database
                _ = await _appDbContext.SaveChangesAsync();

                // Return a success response
                response.Message = GlobalConstants.UPDATE_SUCCESSFULL;
                response.Success = true;
                return response;
            }
            catch (Exception)
            {
                // Log the exception
                // You can log the exception details for debugging purposes

                // Return an error response
                response.Message = GlobalConstants.UPDATE_FAIL;
                return response;
            }

        }

        public async Task<BaseResponse> RemoveImage(string Id)
        {
            BaseResponse response = new() { Success = false };
            try
            {
                // Retrieve the image from the database based on the provided Id
                if (string.IsNullOrWhiteSpace(Id))
                {
                    response.Message = GlobalConstants.ID_NOT_FOUND;
                    return response;
                }
                if (_appDbContext == null)
                {
                    response.Message = GlobalConstants.DB_NOT_FOUND;
                    return response;
                }

                Gallery? image = await _appDbContext.Galleries.FindAsync(Id);

                if (image == null)
                {

                    response.Message = "Image not found";
                    return response;
                }
                // Delete the image from the database

                DeletionResult? result = await DeleteImageCloudinary(image.PublicId ?? "");
                if (result == null || result.Result != "ok")
                {
                    response.Message = "Failed to delete image from Cloudinary";
                    return response;
                }
                // Remove the image from the database
                _ = _appDbContext.Galleries.Remove(image);
                _ = await _appDbContext.SaveChangesAsync();

                response.Message = "Image deleted successfully";
                response.Success = true;
                return response;

            }
            catch (Exception)
            {
                response.Message = GlobalConstants.DELETE_FAIL;
                return response;
            }
        }
    }
}