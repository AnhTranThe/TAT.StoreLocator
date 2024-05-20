using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Category;
using TAT.StoreLocator.Core.Models.Response.Category;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IPhotoService _photoService;
        private readonly ILogger<CategoryService> _logger;
        private readonly AppDbContext _dbContext;

        public CategoryService(ILogger<CategoryService> logger, AppDbContext dbContext, IPhotoService photoService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _photoService = photoService;
        }

        public async Task<BaseResponse> Add(CategoryRequestModel request)
        {
            var response = new BaseResponse();

            try
            {
                var category = MapToCategory(request);
                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();

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

        public async Task<BaseResponseResult<CategoryResponseModel>> GetById(string id)
        {
            var response = new BaseResponseResult<CategoryResponseModel>();

            try
            {
                var category = await FindCategoryByIdAsync(id);

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

                var query = _dbContext.Categories.AsQueryable();
            }

            return response;
        }

        public async Task<BasePaginationResult<CategoryResponseModel>> GetListAsync(BasePaginationRequest request)
        {
            var response = new BasePaginationResult<CategoryResponseModel>();

            try
            {
                var query = _dbContext.Categories.AsQueryable();

                response.TotalCount = await query.CountAsync();

                var categories = await query
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
            var response = new BaseResponse();

            try
            {
                var category = await FindCategoryByIdAsync(Id);

                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category not found.";
                    return response;
                }

                UpdateCategory(category, request);
                _dbContext.Categories.Update(category);
                await _dbContext.SaveChangesAsync();

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

        private Category MapToCategory(CategoryRequestModel request)
        {
            return new Category
            {
                Name = request.Name,
                Description = request.Description,
                Slug = request.Slug,
                IsActive = request.IsActive,
                ParentCategoryId = request.ParentCategoryId,
            };
        }

        private void UpdateCategory(Category category, CategoryRequestModel request)
        {
            category.Name = request.Name;
            category.Description = request.Description;
            category.Slug = request.Slug;
            category.IsActive = request.IsActive;
            category.ParentCategoryId = request.ParentCategoryId;
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
    }
}
