using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Management
{
    public class PhotoManagement : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly AppDbContext _appDbContext;

        public PhotoManagement(IOptions<CloudinarySettings> config, AppDbContext appDbContext)
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

        public async Task<DeletionResult?> DeleteImage(string publicId)
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
                    var mapGalleryProduct = _appDbContext.mapGalleryProducts
                        .FirstOrDefault(x => x.GalleryId == galleryId.ToString());
                    if (mapGalleryProduct != null)
                    {
                        _appDbContext.mapGalleryProducts.Remove(mapGalleryProduct);
                    }
                    break;

                case "Store":
                    var mapGalleryStore = _appDbContext.MapGalleryStores
                        .FirstOrDefault(x => x.GalleryId == galleryId.ToString());
                    if (mapGalleryStore != null)
                    {
                        _appDbContext.MapGalleryStores.Remove(mapGalleryStore);
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid value for FilebeLongto");
            }

            var gallery = _appDbContext.Galleries
                .FirstOrDefault(x => x.Id == galleryId.ToString());
            if (gallery != null)
            {
                _appDbContext.Galleries.Remove(gallery);
            }

            await _appDbContext.SaveChangesAsync();
            if (url != null)
            {
                await DeleteImage(url);
            }
        }
    }
}