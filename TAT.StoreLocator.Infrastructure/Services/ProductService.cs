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



        public async Task<List<string>> GetNearDistrict(string district)
        {
            return district switch
            {
                "Q.1" => await Task.FromResult(new List<string>
            {
                "Quận 3",
                "Quận 4",
                "Quận 5",
                "Quận Bình Thạnh",
                "Quận Phú Nhuận"
            }),

                "Q.2" => await Task.FromResult(new List<string>
            {
                "Quận Bình Thạnh",
                "Quận 9",
                "Quận Thủ Đức",
                "Quận 4",
                "Quận 7"
            }),

                "Q.3" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 10",
                "Quận Phú Nhuận",
                "Quận Tân Bình"
            }),

                "Q.4" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 7",
                "Quận 8",
                "Quận 2"
            }),

                "Q.5" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 6",
                "Quận 10",
                "Quận 11",
                "Quận 8"
            }),

                "Q.6" => await Task.FromResult(new List<string>
            {
                "Quận 5",
                "Quận 11",
                "Quận Bình Tân",
                "Quận 8"
            }),

                "Q.7" => await Task.FromResult(new List<string>
            {
                "Quận 2",
                "Quận 4",
                "Quận 8",
                "Huyện Nhà Bè",
                "Huyện Bình Chánh"
            }),

                "Q.8" => await Task.FromResult(new List<string>
            {
                "Quận 4",
                "Quận 5",
                "Quận 6",
                "Quận 7",
                "Quận Bình Tân",
                "Huyện Bình Chánh"
            }),

                "Q.9" => await Task.FromResult(new List<string>
            {
                "Quận 2",
                "Quận Thủ Đức"
            }),

                "Q.10" => await Task.FromResult(new List<string>
            {
                "Quận 3",
                "Quận 5",
                "Quận 11",
                "Quận Tân Bình"
            }),

                "Q.11" => await Task.FromResult(new List<string>
            {
                "Quận 5",
                "Quận 6",
                "Quận 10",
                "Quận Tân Bình",
                "Quận Bình Tân"
            }),

                "Q.12" => await Task.FromResult(new List<string>
            {
                "Quận Gò Vấp",
                "Quận Bình Thạnh",
                "Quận Thủ Đức",
                "Quận Tân Bình",
                "Huyện Hóc Môn"
            }),

                "Q.Bình Thạnh" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 2",
                "Quận Gò Vấp",
                "Quận Phú Nhuận",
                "Quận Thủ Đức"
            }),

                "Q.Tân Bình" => await Task.FromResult(new List<string>
            {
                "Quận 3",
                "Quận 10",
                "Quận 11",
                "Quận Phú Nhuận",
                "Quận Tân Phú"
            }),

                "Q.Tân Phú" => await Task.FromResult(new List<string>
            {
                "Quận Tân Bình",
                "Quận Bình Tân",
                "Quận 11"
            }),

                "Q.Phú Nhuận" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 3",
                "Quận Bình Thạnh",
                "Quận Tân Bình"
            }),

                "Q.Thủ Đức" => await Task.FromResult(new List<string>
            {
                "Quận 2",
                "Quận 9",
                "Quận Bình Thạnh",
                "Quận 12"
            }),

                "Q.Bình Tân" => await Task.FromResult(new List<string>
            {
                "Quận 6",
                "Quận 8",
                "Quận Tân Phú",
                "Quận Bình Chánh"
            }),

                "H.Hóc Môn" => await Task.FromResult(new List<string>
            {
                "Quận 12",
                "Huyện Củ Chi",
                "Quận Bình Tân"
            }),

                "H.Củ Chi" => await Task.FromResult(new List<string>
            {
                "Huyện Hóc Môn",
                "Huyện Bình Chánh"
            }),

                "H.Bình Chánh" => await Task.FromResult(new List<string>
            {
                "Quận 7",
                "Quận 8",
                "Quận Bình Tân",
                "Huyện Nhà Bè",
                "Huyện Củ Chi"
            }),

                "H.Nhà Bè" => await Task.FromResult(new List<string>
            {
                "Quận 7",
                "Huyện Bình Chánh"
            }),

                _ => await Task.FromResult(new List<string>())
            };
        }


    }
}
