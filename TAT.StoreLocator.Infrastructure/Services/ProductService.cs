using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Product;
using TAT.StoreLocator.Core.Models.Response.Category;
using TAT.StoreLocator.Core.Models.Response.Gallery;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Models.Response.User;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<BaseResponse> AddProduct(ProductRequestModel request)
        {
            BaseResponse response = new BaseResponse()
            {
                Success = false,
            };
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Category category = new Category()
                    {   Id = request.Category.Id,
                        Name = request.Category.Name,
                        Description = request.Category.Description,
                        Slug = request.Category.Slug,
                        IsActive = request.Category.IsActive,
                        ParentCategoryId = request.Category.Id
                    };
                    _dbContext.Categories.Add(category);
                    await _dbContext.SaveChangesAsync();

                    Product product = new Product
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Content = request.Content,
                        Note = request.Note,
                        Slug = request.Slug,
                        Price = request.Price,
                        Discount = request.Discount,
                        MetaTitle = request.MetaTitle,
                        MetaDescription = request.MetaDescription,
                        Quantity = request.Quantity,
                        Rating = request.Rating,
                        SKU = request.SKU,
                        IsActive = request.IsActive,
                        ProductViewCount = request.ProductViewCount,
                        StoreId = request.StoreId,
                        CategoryId = request.Category.Id,
                    };

                    _dbContext.Products.Add(product);
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = $"An error occurred: {ex.Message}";
                }
            }

            return response;
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
                // Other properties
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
                Content = product.Content,
                Note = product.Note,
                Slug = product.Slug,
                Price = product.Price,
                Discount = product.Discount,
                MetaTitle = product.MetaTitle,
                MetaDescription = product.MetaDescription,
                Quantity = product.Quantity,
                Rating = product.Rating,
                SKU = product.SKU,
                IsActive = product.IsActive,
                ProductViewCount = product.ProductViewCount,
                CategoryId = product.CategoryId,
                Category = categoryResponse,
                galleryResponseModels = galleryResponseModels,
                StoreId = product.StoreId,

            };
            return new BaseResponseResult<ProductResponseModel>
            {
                Success = true,
                Data = productResponse
            };
        }
        public async Task<BasePaginationResult<ProductResponseModel>> GetListProductAsync(BasePaginationRequest request)
        {
            IQueryable<Product> productQuery = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.MapGalleryProducts)
                .ThenInclude(m => m.Gallery);

            int totalRow = await productQuery.CountAsync();
            List<ProductResponseModel> data = await productQuery.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(product => new ProductResponseModel()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Content = product.Content,
                    Note = product.Note,
                    Slug = product.Slug,
                    Price = product.Price,
                    Discount = product.Discount,
                    MetaTitle = product.MetaTitle,
                    MetaDescription = product.MetaDescription,
                    Quantity = product.Quantity,
                    Rating = product.Rating,
                    SKU = product.SKU,
                    IsActive = product.IsActive,
                    ProductViewCount = product.ProductViewCount,
                    CategoryId = product.CategoryId,
                    StoreId = product.StoreId,            
                    Category = product.Category != null ? new CategoryResponseModel
                    {
                        Name = product.Category.Name,
                        Description = product.Category.Description,
                        // Other properties

                    } : null,
                    galleryResponseModels = product.MapGalleryProducts != null ? product.MapGalleryProducts.Select(g => new GalleryResponseModel
                    {
                        Id = g.GalleryId,
                        FileName = g.Gallery.FileName,
                        Url = g.Gallery.Url,
                        FileBelongsTo = g.Gallery.FileBelongsTo,
                        IsThumbnail = g.Gallery.IsThumbnail
                        // Other properties
                    }).ToList() : new List<GalleryResponseModel>()


                }).ToListAsync();

            BasePaginationResult<ProductResponseModel> response = new()
            {
                TotalCount = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Data = data
            };

            return response;
        }

        public Task<BasePaginationResult<ProductResponseModel>> SearchProductAsync(SearchProductPagingRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}
