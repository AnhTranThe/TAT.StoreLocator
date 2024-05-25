using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Review;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

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

        public async Task<BaseResponse> CreateReviewAsync(CreateReviewRequestModel request)
        {
            IQueryable<Review> existingReviewQuery = _appDbContext.Reviews.AsQueryable();

            BaseResponse response = new() { Success = false };

            if (string.IsNullOrEmpty(request.Type) || string.IsNullOrEmpty(request.TypeId) || string.IsNullOrEmpty(request.UserId))
            {
                response.Message = GlobalConstants.REQUEST_INVALID;
                return response;

            }

            existingReviewQuery = existingReviewQuery.Where(r => r.UserId == request.UserId);

            if (request.Type == "store")
            {
                existingReviewQuery = existingReviewQuery.Where(g => g.StoreId == request.TypeId);
            }
            if (request.Type == "product")
            {
                existingReviewQuery = existingReviewQuery.Where(g => g.ProductId == request.TypeId);
            }


            Review? existingReview = await existingReviewQuery.FirstOrDefaultAsync();
            if (existingReview != null)
            {
                response.Message = GlobalConstants.USER_HAVE_REVIEW_ALREADY;
                return response;
            }

            Review? review = _mapper.Map<Review>(request);
            if (request.Type == "store")
            {
                review.StoreId = request.TypeId;
            }
            if (request.Type == "product")
            {
                review.ProductId = request.TypeId;
            }

            _ = _appDbContext.Reviews.Add(review);
            if (await _appDbContext.SaveChangesAsync() <= 0)
            {
                response.Message = GlobalConstants.FAIL;
                return response;

            }
            response.Success = true;
            response.Message = GlobalConstants.SUCCESSFULL;
            return response;
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

        public async Task<BasePaginationResult<ReviewResponseModel>> GetReviewsByUserIdAsync(string userId, BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest)
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

        public async Task<BasePaginationResult<ReviewResponseModel>> GetReviewsByStoreIdAsync(BaseReviewFilterRequest filterRequest, BasePaginationRequest paginationRequest)
        {
            IQueryable<Review> query = _appDbContext.Reviews
                .Include(r => r.Product)
                .Where(r => r.StoreId == filterRequest.TypeId);

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

        public async Task<BaseResponseResult<ReviewResponseModel>> GetReviewByUserAndStoreAsync(GetReviewByUserAndStoreRequestModel request)
        {
            BaseResponseResult<ReviewResponseModel> response = new() { Success = false };
            try
            {
                if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.StoreId))
                {

                    response.Message = GlobalConstants.REQUEST_INVALID;
                    return response;

                }

                Review? review = await _appDbContext.Reviews
          .Include(r => r.Product)
          .Include(r => r.User)
          .Include(r => r.Store)
          .FirstOrDefaultAsync(r => r.UserId == request.UserId && r.StoreId == request.StoreId);
                if (review == null)
                {
                    response.Message = "User didn't review store";
                    return response;
                }
                response.Success = true;
                response.Data = _mapper.Map<ReviewResponseModel>(review);
                return response;

            }
            catch (Exception ex)
            {
                response.Message += ex.ToString();
                return response;

            }




        }
    }
}