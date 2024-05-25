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
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Response.Gallery;
using TAT.StoreLocator.Core.Utils;
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

        public async Task<DeletionResult?> DeleteDbAndCloudAsyncResultt(Guid galleryId, string fileBelongTo, string publicId)
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
                    else
                    {
                        return null;
                    }
                    break;

                case "Store":
                    MapGalleryStore? mapGalleryStore = _appDbContext.MapGalleryStores
                        .FirstOrDefault(x => x.GalleryId == galleryId.ToString());
                    if (mapGalleryStore != null)
                    {
                        _ = _appDbContext.MapGalleryStores.Remove(mapGalleryStore);
                    }
                    else
                    {
                        return null;
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
            if (publicId != null)
            {
                DeletionParams deleteParams = new(publicId);
                return await _cloudinary.DestroyAsync(deleteParams);
            }
            return null;
        }



        public async Task DeleteDbAndCloudAsync(Guid galleryId, string fileBelongTo, string PublicId)
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
            if (PublicId != null)
            {
                await DeleteImageCloudinary(PublicId);
            }
        }

        public async Task<BasePaginationResult<GalleryResponseModel>> GetListImagesAsync(BasePaginationRequest request)
        {

            IQueryable<Gallery> query = _appDbContext.Galleries.AsQueryable();

            int totalRow = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(request.SearchString))
            {
                string normalizedSearchString = CommonUtils.vietnameseReplace(request.SearchString);
                query = query.
              Where(item => item.FileName != null && item.FileName.ToUpper().Contains(normalizedSearchString))
              .AsQueryable();
            }

            List<GalleryResponseModel> data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new GalleryResponseModel()
                {
                    Id = e.Id,
                    Key = e.PublicId,
                    FileName = e.FileName,
                    Url = e.Url,
                    FileBelongsTo = e.FileBelongsTo,
                    IsThumbnail = e.IsThumbnail,
                    PublicId = e.PublicId


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

            IQueryable<Gallery> query = _appDbContext.Galleries.AsQueryable();
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
            if (!string.IsNullOrWhiteSpace(request.SearchString))
            {
                string normalizedSearchString = CommonUtils.vietnameseReplace(request.SearchString);
                query = query.
              Where(item => item.FileName != null && item.FileName.ToUpper().Contains(normalizedSearchString))
              .AsQueryable();
            }


            List<GalleryResponseModel> data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new GalleryResponseModel()
                {
                    Id = e.Id,
                    FileName = e.FileName,
                    Url = e.Url,
                    FileBelongsTo = e.FileBelongsTo,
                    IsThumbnail = e.IsThumbnail,
                    PublicId = e.PublicId

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

        public async Task<BaseResponse> UpdateImage(string Id, UpdatePhotoRequestModel request)
        {
            BaseResponse response = new() { Success = false };
            try
            {
                // Check if the provided Id is null or empty
                if (string.IsNullOrWhiteSpace(Id))
                {
                    response.Message = GlobalConstants.ID_NOT_FOUND;
                    return response;
                }

                // Check if the DbContext is null
                if (_appDbContext == null)
                {
                    response.Message = GlobalConstants.DB_NOT_FOUND;
                    return response;
                }

                // Retrieve the image from the database based on the provided Id
                Gallery? image = await _appDbContext.Galleries.FindAsync(Id);
                if (image == null)
                {
                    response.Message = "Image not found";
                    return response;
                }

                // If IsThumbnail is being set to true, ensure no other image is a thumbnail
                if (request.IsThumbnail)
                {
                    IQueryable<Gallery> query = _appDbContext.Galleries.AsQueryable();
                    if (request.Type == "store" && !string.IsNullOrEmpty(request.TypeId))
                    {
                        query = query.Include(g => g.MapGalleryStores)
                                     .Where(g => g.MapGalleryStores != null && g.MapGalleryStores.Any(mgs => mgs.StoreId == request.TypeId));
                    }
                    else if (request.Type == "product" && !string.IsNullOrEmpty(request.TypeId))
                    {
                        query = query.Include(g => g.MapGalleryProducts)
                                     .Where(g => g.MapGalleryProducts != null && g.MapGalleryProducts.Any(mgp => mgp.ProductId == request.TypeId));
                    }

                    // Set IsThumbnail to false for all other images
                    IQueryable<Gallery> otherThumbnails = query.Where(g => g.IsThumbnail);
                    foreach (Gallery? otherThumbnail in otherThumbnails)
                    {
                        otherThumbnail.IsThumbnail = false;
                    }
                }

                // Update the specified image
                image.IsThumbnail = request.IsThumbnail;




                // Save the changes to the database
                _ = await _appDbContext.SaveChangesAsync();

                // Return a success response
                response.Message = GlobalConstants.UPDATE_SUCCESSFULL;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                // Log the exception
                // You can log the exception details for debugging purposes
                Console.WriteLine(ex);

                // Return an error response
                response.Message = GlobalConstants.UPDATE_FAIL;
                return response;
            }

        }

        public async Task<BaseResponse> RemoveImage(string Id, DeletePhotoRequestModel request)
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


                if (request.Type == "store" && !string.IsNullOrEmpty(request.TypeId))
                {
                    IQueryable<MapGalleryStore> query = _appDbContext.MapGalleryStores.AsQueryable();
                    query = query.Where(g => g.StoreId == request.TypeId);
                    foreach (MapGalleryStore item in query)
                    {
                        _ = _appDbContext.MapGalleryStores.Remove(item);
                    }
                }
                else if (request.Type == "product" && !string.IsNullOrEmpty(request.TypeId))
                {
                    IQueryable<MapGalleryProduct> query = _appDbContext.mapGalleryProducts.AsQueryable();
                    query = query.Where(g => g.ProductId == request.TypeId);
                    foreach (MapGalleryProduct item in query)
                    {
                        _ = _appDbContext.mapGalleryProducts.Remove(item);
                    }
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

        public async Task<BaseResponse> CreateImage(UploadPhotoRequestModel request)
        {
            BaseResponse response = new() { Success = false };

            // Check if the file upload is null
            if (request.FileUpload == null)
            {
                response.Message = GlobalConstants.FILE_UPLOAD_NOT_FOUND;
                return response;
            }

            // Start a database transaction
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                // Upload the image to Cloudinary
                CloudinaryDotNet.Actions.ImageUploadResult uploadFileResult = await UploadImage(request.FileUpload, true);
                if (uploadFileResult.Error != null)
                {
                    response.Message = uploadFileResult.Error.Message;
                    return response;
                }

                // Create a new Gallery entity
                Gallery gallery = new()
                {
                    PublicId = uploadFileResult.PublicId,
                    Url = uploadFileResult.Url.ToString(),
                    FileBelongsTo = request.Type,
                    IsThumbnail = request.IsThumbnail,
                };

                // Add the new gallery entity to the database
                _ = _appDbContext.Galleries.Add(gallery);
                _ = await _appDbContext.SaveChangesAsync();

                // Create and add a new MapGalleryProduct entity if the type is "product"
                if (!string.IsNullOrEmpty(request.Type) && request.Type == "product")
                {
                    MapGalleryProduct mapGalleryProduct = new()
                    {
                        ProductId = request.TypeId,
                        GalleryId = gallery.Id
                    };
                    _ = _appDbContext.mapGalleryProducts.Add(mapGalleryProduct);
                    _ = await _appDbContext.SaveChangesAsync();
                }

                // Create and add a new MapGalleryStore entity if the type is "store"
                if (!string.IsNullOrEmpty(request.Type) && request.Type == "store")
                {
                    MapGalleryStore mapGalleryStore = new()
                    {
                        StoreId = request.TypeId,
                        GalleryId = gallery.Id
                    };
                    _ = _appDbContext.MapGalleryStores.Add(mapGalleryStore);
                    _ = await _appDbContext.SaveChangesAsync();
                }

                // Commit the transaction
                await transaction.CommitAsync();

                // Return a success response
                response.Message = GlobalConstants.UPLOAD_SUCCESSFULL;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();

                // Log the exception (optional)
                Console.WriteLine(ex);

                // Return an error response
                response.Message = GlobalConstants.UPLOAD_FAIL;
                return response;
            }

        }


    }
}