using Microsoft.AspNetCore.Http;
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
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Utils;
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

        /// <summary>
        /// getproductbyid Last
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<BaseResponseResult<ProductResponseModel>> GetById(string Id)
        {
            BaseResponseResult<ProductResponseModel> response = new();

            ProductResponseModel? product = await (from p in _dbContext.Products
                                                   where p.Id == Id
                                                   select new ProductResponseModel
                                                   {
                                                       Id = p.Id,
                                                       Name = p.Name,
                                                       Description = p.Description,
                                                       Content = p.Content,
                                                       Note = p.Note,
                                                       Slug = p.Slug,
                                                       Price = p.Price,
                                                       Discount = p.Discount,
                                                       MetaTitle = p.MetaTitle,
                                                       MetaDescription = p.MetaDescription,
                                                       Quantity = p.Quantity,
                                                       Rating = p.Rating,
                                                       SKU = p.SKU,
                                                       IsActive = p.IsActive,
                                                       ProductViewCount = p.ProductViewCount,
                                                       CategoryId = p.CategoryId,
                                                       Category = p.Category != null ? new CategoryProductResponseModel
                                                       {
                                                           Id = p.Category.Id,
                                                           Name = p.Category.Name,
                                                           Description = p.Category.Description
                                                       } : null,
                                                       StoreId = p.StoreId,
                                                       Store = p.Store != null ? new StoreOfProductResponseModel
                                                       {
                                                           Id = p.Store.Id,
                                                           Name = p.Store.Name,
                                                           Email = p.Store.Email
                                                       } : null,
                                                       GalleryResponseModels = p.MapGalleryProducts != null ? p.MapGalleryProducts
                                                           .Select(m => new GalleryResponseModel
                                                           {
                                                               Id = m.Gallery!.Id,
                                                               FileName = m.Gallery.FileName,
                                                               Url = m.Gallery.Url,
                                                               FileBelongsTo = m.Gallery.FileBelongsTo,
                                                               IsThumbnail = m.Gallery.IsThumbnail
                                                           }).ToList() : null,
                                                       Reviews = p.Reviews != null ? p.Reviews
                                                           .Select(r => new ReviewResponseModel
                                                           {
                                                               Id = r.Id,
                                                               Content = r.Content,
                                                               RatingValue = r.RatingValue,
                                                               Status = r.Status,
                                                               UserId = r.UserId
                                                           }).ToList() : null
                                                   }).FirstOrDefaultAsync();

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found.";
            }
            else
            {
                response.Success = true;
                response.Data = product;
            }
            return response;
        }

        public Task<BasePaginationResult<ProductResponseModel>> SearchProductAsync(SearchProductPagingRequestModel request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// getProductList
        /// </summary>
        /// <param name="request"></param>
        /// <returns> List Product </returns>
        public async Task<BasePaginationResult<ProductResponseModel>> GetListProductAsync(BasePaginationRequest request)
        {
            IQueryable<Product> productQuery = _dbContext.Products
                 .Include(p => p.Category)
                 .Include(p => p.MapGalleryProducts)
                 .Include(p => p.Store);
            //Thêm điều kiện tìm kiếm theo tên
            if (!string.IsNullOrWhiteSpace(request.SearchString))
            {
                string normalizedSearchString = CommonUtils.vietnameseReplace(request.SearchString);
                productQuery = productQuery.Where(store => store.Name != null && store.Name.ToUpper().Contains(normalizedSearchString));
            }

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
                    Store = product.Store != null ? new StoreOfProductResponseModel
                    {
                        Id = product.Store.Id,
                        Name = product.Store.Name,
                        Email = product.Store.Email
                    } : null,
                    GalleryResponseModels = product.MapGalleryProducts != null ? product.MapGalleryProducts
                        .Select(m => new GalleryResponseModel
                        {
                            Id = m.Gallery!.Id,
                            FileName = m.Gallery.FileName,
                            Url = m.Gallery.Url,
                            FileBelongsTo = m.Gallery.FileBelongsTo,
                            IsThumbnail = m.Gallery.IsThumbnail
                        }).ToList() : null,
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

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="request"></param>
        /// <returns> BaseResponse </returns>
        public async Task<BaseResponse> UpdateProduct(string Id, ProductRequestModel request)
        {
            BaseResponse response = new() { Success = false };

            if (request == null || string.IsNullOrEmpty(Id))
            {
                response.Message = "Invalid request data.";
                return response;
            }

            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Product? product = await _dbContext.Products.FindAsync(Id);
                    if (product == null)
                    {
                        response.Message = "Product not found.";
                        return response;
                    }

                    UpdateProductProperties(product, request);


                    /*if (request.UploadPhoto?.FileUpload != null)
                    {
                        foreach (IFormFile file in request.UploadPhoto.FileUpload)
                        {
                            await AddPhotoProductAsync(product.Id, file);
                        }
                    }
                    _ = await _dbContext.SaveChangesAsync();*/
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

        private static void UpdateProductProperties(Product product, ProductRequestModel request)
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


        public async Task AddPhotoProductAsync(string productId, IFormFile file)
        {
            CloudinaryDotNet.Actions.ImageUploadResult uploadFileResult = await _photoService.UploadImage(file, true);
            Gallery gallery = new()
            {
                PublicId = uploadFileResult.PublicId,
                Url = uploadFileResult.Url.ToString(),
                FileBelongsTo = "Product",
                // IsThumbnail = uploadPhoto.IsThumbnail,
            };

            _ = _dbContext.Galleries.Add(gallery);

            MapGalleryStore mapGalleryStore = new()
            {
                StoreId = productId.ToString(),
                GalleryId = gallery.Id
            };

            _ = _dbContext.MapGalleryStores.Add(mapGalleryStore);
        }

        /// <summary>
        /// add new product
        /// </summary>
        /// <param name="request"></param>
        /// <returns>BaseResponse</returns>
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


                    //if (request.UploadPhoto?.ListFilesUpload != null)
                    //{
                    //    foreach (IFormFile file in request.UploadPhoto.ListFilesUpload)
                    //    {
                    //        await AddPhotoProductAsync(product.Id, file);
                    //    }
                    //}

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

        /// <summary>
        /// get Product By StoreId
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="request"></param>
        /// <returns>List Product of StoreId </returns>
        public async Task<BasePaginationResult<ProductResponseModel>> GetByIdStore(string StoreId, BasePaginationRequest request)
        {
            IQueryable<ProductResponseModel> query = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.MapGalleryProducts)
                .Where(p => p.StoreId == StoreId && p.IsActive)
                .Select(product => new ProductResponseModel
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
                });

            int totalCount = await query.CountAsync();

            List<ProductResponseModel> paginatedProducts = await query
                .OrderBy(p => p.Name)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            BasePaginationResult<ProductResponseModel> paginationResult = new()
            {
                Data = paginatedProducts,
                TotalCount = totalCount
            };

            return paginationResult;
        }
    }
}