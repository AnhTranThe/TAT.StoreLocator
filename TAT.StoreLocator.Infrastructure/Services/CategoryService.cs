using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Category;
using TAT.StoreLocator.Core.Models.Response.Category;
using TAT.StoreLocator.Core.Utils;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly AppDbContext _dbContext;

        public CategoryService(ILogger<CategoryService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> Add(CategoryRequestModel request)
        {
            BaseResponse response = new();

            try
            {
                Category category = MapToCategory(request);
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    category.Slug = CommonUtils.UrlFriendly(request.Name);
                }
                _ = await _dbContext.Categories.AddAsync(category);
                _ = await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Message = "Category added successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponseResult<CategoryResponseModel>> GetById(string Id)
        {
            BaseResponseResult<CategoryResponseModel> response = new();

            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    response.Success = false;
                    response.Message = "Category ID is null or empty.";
                    return response;
                }

                var category = await FindCategoryByIdAsync(Id);

                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category not found.";
                    return response;
                }

                response.Data = MapToCategoryResponse(category);
                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by ID");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<BasePaginationResult<CategoryResponseModel>> GetListAsync(BasePaginationRequest request)
        {
            BasePaginationResult<CategoryResponseModel> response = new();

            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "Pagination request is null.");

                var query = _dbContext.Categories.AsQueryable();

                response.TotalCount = await query.CountAsync();

                List<Category> categories = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                response.Data = categories.Select(MapToCategoryResponse).ToList();
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category list");
            }

            return response;
        }

        public async Task<BaseResponse> Update(string Id, CategoryRequestModel request)
        {
            BaseResponse response = new();

            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    response.Success = false;
                    response.Message = "Category ID is null or empty.";
                    return response;
                }

                if (request == null)
                    throw new ArgumentNullException(nameof(request), "Category request is null.");

                var category = await FindCategoryByIdAsync(Id);

                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category not found.";
                    return response;
                }

                UpdateCategory(category, request);
                _ = _dbContext.Categories.Update(category);
                _ = await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Message = "Category updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        private static Category MapToCategory(CategoryRequestModel request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                request.Slug = CommonUtils.UrlFriendly(request.Name);
            }
            return new Category
            {
                Name = request.Name,
                Description = request.Description,
                Slug = request.Slug,
                IsActive = request.IsActive,
                ParentCategoryId = request.ParentCategoryId,
            };
        }

        private static void UpdateCategory(Category category, CategoryRequestModel request)
        {
            category.Name = request.Name;
            category.Description = request.Description;
            category.Slug = request.Slug;
            category.IsActive = request.IsActive;
            category.ParentCategoryId = request.ParentCategoryId;
        }

        private async Task<Category?> FindCategoryByIdAsync(string Id)
        {
            if (_dbContext == null)
                throw new InvalidOperationException("Database context is null.");

            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == Id);
        }

        private CategoryResponseModel MapToCategoryResponse(Category category)
        {
            return new CategoryResponseModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
                IsActive = category.IsActive,
                ParentCategoryId = category.ParentCategoryId,
                GalleryId = category.GalleryId
            };
        }

        private async Task<Category> FindCategoryByIdAsync(string id)
        {
            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<BasePaginationResult<CategoryResponseModel>> GetListParentCategoryAsync(BasePaginationRequest request)
        {
            BasePaginationResult<CategoryResponseModel> response = new();

            try
            {
                IQueryable<Category> query = _dbContext.Categories.Where(c => c.ParentCategoryId == string.Empty); // Assuming ParentCategoryId is null for parent categories


                response.TotalCount = await query.CountAsync();

                List<Category> categories = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                response.Data = categories.Select(MapToCategoryResponse).ToList();
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category list");
            }

            return response;
        }

        public async Task<BasePaginationResult<CategoryResponseModel>> GetListSubCategoryAsync(BasePaginationRequest request)
        {
            BasePaginationResult<CategoryResponseModel> response = new();

            try
            {
                IQueryable<Category> query = _dbContext.Categories.Where(c => c.ParentCategoryId != string.Empty); // Assuming ParentCategoryId is null for parent categories


                response.TotalCount = await query.CountAsync();

                List<Category> categories = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                response.Data = categories.Select(MapToCategoryResponse).ToList();
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category list");
            }

            return response;
        }
    }
}
