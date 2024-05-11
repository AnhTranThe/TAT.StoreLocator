using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Response.Category;
using TAT.StoreLocator.Core.Models.Response.Gallery;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _dbContext;
        public ProductService(ILogger logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }
        public async Task<BaseResponseResult<ProductResponseModel>> GetById(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId));
            }            
            Product? product = await _dbContext.Products
                 .Include(p => p.Category)
                 .Include(p => p.MapGalleryProducts)
                 .ThenInclude(m => m.Gallery) // Nạp thông tin các hình ảnh của sản phẩm
                 .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return new BaseResponseResult<ProductResponseModel>
                {
                    Success = false,
                    Message = $"Product with ID {productId} not found."
                };
            }
            // Check null
            CategoryResponseModel? categoryResponse = product.Category != null ? new CategoryResponseModel
            {
                Name = product.Category.Name,
                // Các thuộc tính khác của danh mục nếu cần
            } : null;

            // check null product.MapGalleryProducts 
            List<GalleryResponseModel> galleryResponseModels = product.MapGalleryProducts != null ? product.MapGalleryProducts.Select(g => new GalleryResponseModel
            {
                Id = g.GalleryId,
                FileName = g.Gallery.FileName,
                Url = g.Gallery.Url,
                FileBelongsTo = g.Gallery.FileBelongsTo,
                IsThumbnail = g.Gallery.IsThumbnail
                // Các thuộc tính khác của hình ảnh
            }).ToList() : new List<GalleryResponseModel>();

            ProductResponseModel productResponse = new()
            {
                Name = product.Name,
                Description = product.Description,
                // Các thuộc tính khác của sản phẩm
                Category = categoryResponse,
                galleryResponseModels = galleryResponseModels
            };

            return new BaseResponseResult<ProductResponseModel>
            {
                Success = true,
                Data = productResponse
            };
        }

        public Task<BasePaginationResult<ProductResponseModel>> SearchUserAsync(SearchProductPagingRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}
