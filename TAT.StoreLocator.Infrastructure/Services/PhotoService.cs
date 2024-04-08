using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            Account account = new(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
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


    }
}
