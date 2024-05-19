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
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IPhotoService _photoService;
        private readonly ILogger _logger;
        private readonly AppDbContext _dbContext;
        public ProductService(ILogger logger, AppDbContext dbContext, IPhotoService photoService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _photoService = photoService;
        }
        public async Task<BaseResponseResult<ProductResponseModel>> GetById(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId));
            }
            BaseResponseResult<ProductResponseModel> reponse = new();

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
            CategoryProductResponseModel? categoryResponse = product.Category != null ? new CategoryProductResponseModel
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
                Description = product.Category.Description,
            } : null;
            StoreOfProductResponseModel? storeResponse = new();

            Store? store = await _dbContext.Stores.FindAsync(product.StoreId);
            if (store is null)
            {
                reponse.Message = $"Store is not found";
                store = null;
            }
            else
            {
                storeResponse = new StoreOfProductResponseModel
                {
                    Id = store.Id,
                    Name = store.Name,
                    Email = store.Email,
                    PhoneNumber = store.PhoneNumber,
                };

            }

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
                Id = product.Id,
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
                StoreId = product.StoreId,
                Store = storeResponse,
                CategoryId = product.CategoryId,
                Category = categoryResponse,
                GalleryResponseModels = galleryResponseModels
            };

            reponse.Data = productResponse;
            reponse.Success = true;

            return reponse;
        }

        public Task<BasePaginationResult<ProductResponseModel>> SearchProductAsync(SearchProductPagingRequestModel request)
        {
            throw new NotImplementedException();
        }

        public async Task<BasePaginationResult<ProductResponseModel>> GetListProductAsync(BasePaginationRequest request)
        {
            IQueryable<Product> productQuery = _dbContext.Products
                 .Include(p => p.Category)
                 .Include(p => p.MapGalleryProducts)
                     .ThenInclude(m => m.Gallery)
                 .Include(p => p.Store);
            int totalRow = await productQuery.CountAsync();
            List<ProductResponseModel> data = await productQuery.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(product => new ProductResponseModel()
                {
                    Id = product.Id,
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
                    Category = product.Category != null ? new CategoryProductResponseModel
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name,
                        Description = product.Category.Description,
                        // Other properties
                    } : null,
                    Store = new StoreOfProductResponseModel
                    {
                        Id = product.Store.Id,
                        Name = product.Store.Name,
                        Email = product.Store.Email
                        // Other properties
                    },
                    GalleryResponseModels = product.MapGalleryProducts
                        .Select(m => new GalleryResponseModel
                        {
                            Id = m.Gallery.Id,
                            FileName = m.Gallery.FileName,
                            Url = m.Gallery.Url,
                            FileBelongsTo = m.Gallery.FileBelongsTo,
                            IsThumbnail = m.Gallery.IsThumbnail
                        }).ToList()
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

        public async Task<BaseResponse> UpdateProduct(ProductRequestModel request)
        {
            BaseResponse response = new() { Success = false };

            if (request == null || string.IsNullOrEmpty(request.ProductId))
            {
                response.Message = "Invalid request data.";
                return response;
            }

            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Product? product = await _dbContext.Products.FindAsync(request.ProductId);
                    if (product == null)
                    {
                        response.Message = "Product not found.";
                        return response;
                    }

                    UpdateProductProperties(product, request);

                    await UpdateOrAddCategoryAsync(product, request);

                    if (request.UploadPhoto?.FileUpload != null && request.UploadPhoto.FileUpload.Length > 0)
                    {
                        BaseResponse photoResponse = await UpdateProductPhotoAsync(product, request.UploadPhoto);
                        if (!photoResponse.Success)
                        {
                            return photoResponse;
                        }
                    }

                    _ = await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    response.Success = true;
                    response.Message = "Product updated successfully.";
                }
                catch (DbUpdateException dbEx)
                {
                    await transaction.RollbackAsync();
                    response.Message = $"Database update error: {dbEx.Message}";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response.Message = $"An error occurred: {ex.Message}";
                }
            }

            return response;
        }

        private void UpdateProductProperties(Product product, ProductRequestModel request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                product.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                product.Description = request.Description;
            }

            if (!string.IsNullOrEmpty(request.Content))
            {
                product.Content = request.Content;
            }

            if (!string.IsNullOrEmpty(request.Note))
            {
                product.Note = request.Note;
            }

            if (!string.IsNullOrEmpty(request.Slug))
            {
                product.Slug = request.Slug;
            }

            if (request.Price.HasValue)
            {
                product.Price = request.Price.Value;
            }

            if (request.Discount.HasValue)
            {
                product.Discount = request.Discount.Value;
            }

            if (!string.IsNullOrEmpty(request.MetaTitle))
            {
                product.MetaTitle = request.MetaTitle;
            }

            if (!string.IsNullOrEmpty(request.MetaDescription))
            {
                product.MetaDescription = request.MetaDescription;
            }

            if (request.Quantity.HasValue)
            {
                product.Quantity = request.Quantity.Value;
            }

            if (request.Rating.HasValue)
            {
                product.Rating = request.Rating.Value;
            }

            if (!string.IsNullOrEmpty(request.SKU))
            {
                product.SKU = request.SKU;
            }

            if (request.IsActive.HasValue)
            {
                product.IsActive = request.IsActive.Value;
            }

            if (request.ProductViewCount.HasValue)
            {
                product.ProductViewCount = request.ProductViewCount.Value;
            }
        }

        private async Task UpdateOrAddCategoryAsync(Product product, ProductRequestModel request)
        {
            if (!string.IsNullOrEmpty(request.CategoryId))
            {
                Category? existingCategory = await _dbContext.Categories.FindAsync(request.CategoryId);
                if (existingCategory == null)
                {
                    throw new Exception("Invalid CategoryId.");
                }
                product.CategoryId = request.CategoryId;
            }
            else if (request.Category != null)
            {
                Category newCategory = new()
                {
                    Name = request.Category.Name,
                    Description = request.Category.Description,
                    Slug = request.Category.Slug,
                    IsActive = request.Category.IsActive
                };
                _ = _dbContext.Categories.Add(newCategory);
                _ = await _dbContext.SaveChangesAsync();
                product.CategoryId = newCategory.Id;
            }
        }

        private async Task<BaseResponse> UpdateProductPhotoAsync(Product product, PhotoProductRequestModel uploadPhoto)
        {
            BaseResponse response = new() { Success = false };

            CloudinaryDotNet.Actions.ImageUploadResult uploadFileResult = await _photoService.UploadImage(uploadPhoto.FileUpload, true);
            if (uploadFileResult.Error != null)
            {
                response.Message = uploadFileResult.Error.Message;
                return response;
            }

            Gallery gallery = new()
            {
                PublicId = uploadFileResult.PublicId,
                Url = uploadFileResult.Url.ToString(),
                FileBelongsTo = "Product",
                IsThumbnail = uploadPhoto.IsThumbnail,
            };

            _ = _dbContext.Galleries.Add(gallery);
            _ = await _dbContext.SaveChangesAsync();

            MapGalleryProduct mapGalleryProduct = new()
            {
                ProductId = product.Id,
                GalleryId = gallery.Id
            };

            _ = _dbContext.mapGalleryProducts.Add(mapGalleryProduct);
            _ = await _dbContext.SaveChangesAsync();

            response.Success = true;
            return response;
        }

        public async Task<BaseResponse> AddProduct(ProductRequestModel request)
        {
            BaseResponse response = new() { Success = false };

            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                response.Message = "Invalid request data.";
                return response;
            }

            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Product product = new()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Content = request.Content,
                        Note = request.Note,
                        Slug = request.Slug,
                        Price = request.Price ?? 0,
                        Discount = request.Discount ?? 0,
                        MetaTitle = request.MetaTitle,
                        MetaDescription = request.MetaDescription,
                        Quantity = request.Quantity ?? 0,
                        Rating = request.Rating ?? 0,
                        SKU = request.SKU,
                        IsActive = request.IsActive ?? true,
                        ProductViewCount = request.ProductViewCount ?? 0,
                        StoreId = request.StoreId,
                        CategoryId = request.CategoryId
                    };

                    _ = _dbContext.Products.Add(product);
                    await UpdateOrAddCategoryAsync(product, request);

                    if (request.UploadPhoto?.FileUpload != null && request.UploadPhoto.FileUpload.Length > 0)
                    {
                        BaseResponse photoResponse = await UpdateProductPhotoAsync(product, request.UploadPhoto);
                        if (!photoResponse.Success)
                        {
                            return photoResponse;
                        }
                    }

                    _ = await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    response.Success = true;
                    response.Message = "Product added successfully.";
                }
                catch (DbUpdateException dbEx)
                {
                    await transaction.RollbackAsync();
                    response.Message = $"Database update error: {dbEx.Message}";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response.Message = $"An error occurred: {ex.Message}";
                }
            }

            return response;
        }






    }
}
