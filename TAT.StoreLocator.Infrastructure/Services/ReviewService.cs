using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IPaging;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Utils;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace TAT.StoreLocator.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ReviewService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<BaseResponseResult<ReviewResponseModel>> CreateReviewAsync(CreateReviewRequestModel request)
        {
            Review? existingReview = await _appDbContext.Reviews
                  .Include(r => r.Product)
                      .ThenInclude(p => p!.Store) // Thêm dấu ! để chỉ định rằng nó k null 
               .FirstOrDefaultAsync(r => r.UserId == request.UserId && r.Product != null && r.Product.StoreId == request.StoreId);


            if (existingReview != null)
            {
                return new BaseResponseResult<ReviewResponseModel>()
                {
                    Success = false,
                    Message = "User has already reviewed this store",
                    Errors = new List<string> { "User has already reviewed this store" }
                };
            }
            Review? review = _mapper.Map<Review>(request);
            _ = _appDbContext.Reviews.Add(review);
            _ = await _appDbContext.SaveChangesAsync();

            review = await _appDbContext.Reviews
               .Include(r => r.Product)
               .ThenInclude(p => p!.Store)
               .FirstOrDefaultAsync(r => r.Id == review.Id);


            ReviewResponseModel responseModel = _mapper.Map<ReviewResponseModel>(review);
            return new BaseResponseResult<ReviewResponseModel>
            {
                Success = true,
                Data = responseModel
            };
        }

        public async Task<BaseResponseResult<ReviewResponseModel>> UpdateReviewAsync(string reviewId, UpdateReviewRequestModel request)
        {
            BaseResponseResult<ReviewResponseModel> response = new();
            try
            {
                Review? review = await _appDbContext.Reviews
                    .Include(r => r.Product)
                        .ThenInclude(p => p!.Store)
                      .FirstOrDefaultAsync(r => r.Id == reviewId);

                if (review == null)
                {
                    response.Success = false;
                    response.Message = "Can not found review";
                    return response;
                }

                // Sử dụng AutoMapper để map các thuộc tính từ request đến thực thể review
                _ = _mapper.Map(request, review);
                //review.UpdatedBy = request.UpdatedBy;

                _ = await _appDbContext.SaveChangesAsync();
                ReviewResponseModel updateReviewResponse = new()
                {
                    Id = reviewId,
                    Content = review.Content,
                    RatingValue = review.RatingValue,
                    Status = review.Status,
                    UserId = request.UserId,
                    StoreId = review.StoreId,
                    Product = review.Product == null ? null : new BaseProductResponseModel
                    {
                        Id = review.Product.Id,
                        Name = review.Product.Name,
                        Description = review.Product.Description,
                        Content = review.Product.Content,
                        Price = review.Product.Price,
                        Discount = review.Product.Discount,
                        Quantity = review.Product.Quantity,
                    },
                    CreatedAt = review.CreatedAt.ToString(),
                    CreatedBy = review.CreatedBy,
                    UpdatedAt = review.UpdatedAt.ToString(),
                    //UpdatedBy = review.UpdatedBy
                };
                response.Success = true;
                response.Data = updateReviewResponse;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BasePaginationResult<ReviewResponseModel>> GetReviewByUserIdAsync(string userId, BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest)
        {
            IQueryable<Review> query = _appDbContext.Reviews
                .Include(r => r.Product)
                .ThenInclude(p => p!.Store)
                .Where(r => r.UserId == userId);

            // Lọc theo rating nếu có giá trị được chỉ định
            if (filterRequest.SearchRatingKey.HasValue)
            {
                query = query.Where(r => r.RatingValue == filterRequest.SearchRatingKey.Value);
            }

            int totalCount = await query.CountAsync();

            // Thực hiện phân trang
            List<Review> reviewList = await query
                .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            // Map danh sách đánh giá sang danh sách đáp ứng
            List<ReviewResponseModel> reviewResponseModel = _mapper.Map<List<ReviewResponseModel>>(reviewList);

            // Tạo kết quả trả về
            BasePaginationResult<ReviewResponseModel> paginationResult = new BasePaginationResult<ReviewResponseModel>
            {
                Data = reviewResponseModel,
                PageSize = paginationRequest.PageSize,
                PageIndex = paginationRequest.PageIndex,
                TotalCount = totalCount,
                SearchString = paginationRequest.SearchString
            };
            return paginationResult;
        }

        public async Task<BasePaginationResult<ReviewResponseModel>> GetReviewByStoreIdAsync(string storeId, BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest)
        {
            IQueryable<Review> query = _appDbContext.Reviews
                .Include(r => r.Product)
                .Where(r => r.StoreId == storeId);

            if (filterRequest.SearchRatingKey.HasValue)
            {
                query = query.Where(r => r.RatingValue == filterRequest.SearchRatingKey.Value);
            }

            int totalCount = await query.CountAsync();

            List<Review> reviewList = await query
                .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            List<ReviewResponseModel> reviewResponseModel = _mapper.Map<List<ReviewResponseModel>>(reviewList);
            BasePaginationResult<ReviewResponseModel> paginationResult = new()
            {
                Data = reviewResponseModel,
                PageSize = paginationRequest.PageSize,
                PageIndex = paginationRequest.PageIndex,
                TotalCount = totalCount,
                SearchString = paginationRequest.SearchString
            };
            return paginationResult;
        }
    }
}


