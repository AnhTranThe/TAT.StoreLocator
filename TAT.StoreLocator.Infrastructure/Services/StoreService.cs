using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Review;
using TAT.StoreLocator.Core.Models.Response.Store;
using TAT.StoreLocator.Core.Utils;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class StoreService : IStoreService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<StoreService> _logger;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public StoreService(AppDbContext appDbContext, IMapper mapper, IPhotoService photoService, ILogger<StoreService> logger)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _photoService = photoService;
            _logger = logger;
        }

        public async Task<CreateStoreResponseModel> CreateStoreAsync(StoreRequestModel request)
        {
            CreateStoreResponseModel response = new();

            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    response.BaseResponse = new BaseResponse { Success = false, Message = "Name and PhoneNumber are required." };
                    return response;
                }

                // Start a new transaction
                using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (!string.IsNullOrEmpty(request.Province))
                    {
                        request.Province = CommonUtils.ReplaceProvincePatterns(request.Province);



                    }
                    if (!string.IsNullOrEmpty(request.District))
                    {
                        request.District = CommonUtils.ReplaceDistrictPatterns(request.District);

                    }


                    if (!string.IsNullOrEmpty(request.Ward))
                    {
                        request.Ward = CommonUtils.ReplaceWardPatterns(request.Ward);
                    }

                    Address newAddressEntity = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoadName = request.RoadName,
                        Province = request.Province,
                        District = request.District,
                        Ward = request.Ward,
                        PostalCode = request.PostalCode,
                        latitude = request.Latitude,
                        longitude = request.Longitude
                    };
                    _ = _appDbContext.Addresses.Add(newAddressEntity);

                    // Create new store entity
                    string newStoreId = Guid.NewGuid().ToString();
                    Store newStoreEntity = new()
                    {
                        Id = newStoreId,
                        Name = request.Name,
                        PhoneNumber = request.PhoneNumber,
                        Email = request.Email,
                        AddressId = newAddressEntity.Id,
                        IsActive = true
                    };

                    // Handle file uploads if any
                    if (request.files != null)
                    {
                        foreach (IFormFile file in request.files)
                        {
                            await UpdateStorePhotoAsync(newStoreId, file);
                        }
                    }

                    // Add new store entity to context
                    _ = _appDbContext.Stores.Add(newStoreEntity);

                    // Save changes to the database
                    _ = await _appDbContext.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    // Populate response
                    response.Id = newStoreId;
                    response.BaseResponse = new BaseResponse { Success = true, Message = "Store created successfully." };
                    response.StoreResponseModel = new StoreResponseModel
                    {
                        Id = newStoreId,
                        Name = request.Name,
                        PhoneNumber = request.PhoneNumber,
                        Email = request.Email,
                        IsActive = request.IsActive,
                        Address = new AddressResponseModel
                        {
                            Id = newAddressEntity.Id,
                            RoadName = newAddressEntity.RoadName,
                            Province = newAddressEntity.Province,
                            District = newAddressEntity.District,
                            Ward = newAddressEntity.Ward,
                            PostalCode = newAddressEntity.PostalCode,
                            Latitude = newAddressEntity.latitude,
                            Longitude = newAddressEntity.longitude
                        }
                    };
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    await transaction.RollbackAsync();

                    // Handle any exceptions that occur during the process
                    response.BaseResponse = new BaseResponse { Success = false, Message = $"Error creating store: {ex.Message}" };
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                response.BaseResponse = new BaseResponse { Success = false, Message = $"Error creating store: {ex.Message}" };
            }
            return response;
        }

        private async Task UpdateStorePhotoAsync(string storeId, IFormFile file)
        {
            CloudinaryDotNet.Actions.ImageUploadResult uploadFileResult = await _photoService.UploadImage(file, true);
            Gallery gallery = new()
            {
                PublicId = uploadFileResult.PublicId,
                Url = uploadFileResult.Url.ToString(),
                FileBelongsTo = "Store",
                // IsThumbnail = uploadPhoto.IsThumbnail,
            };

            _ = _appDbContext.Galleries.Add(gallery);

            MapGalleryStore mapGalleryStore = new()
            {
                StoreId = storeId.ToString(),
                GalleryId = gallery.Id
            };

            _ = _appDbContext.MapGalleryStores.Add(mapGalleryStore);
        }

        public async Task<BasePaginationResult<StoreResponseModel>> GetAllStoreAsync(BasePaginationRequest paginationRequest)
        {
            BasePaginationResult<StoreResponseModel> response = new();

            try
            {
                response.SearchString = paginationRequest.SearchString;
                response.PageIndex = paginationRequest.PageIndex;
                response.PageSize = paginationRequest.PageSize;
                IQueryable<StoreResponseModel> query = from store in _appDbContext.Stores
                                                       join mapGalleryStore in _appDbContext.MapGalleryStores
                                                           on store.Id equals mapGalleryStore.StoreId into galleryStores
                                                       from galleryStore in galleryStores.DefaultIfEmpty() // Left join
                                                       join gallery in _appDbContext.Galleries
                                                           on galleryStore.GalleryId equals gallery.Id into galleries
                                                       from gallery in galleries.DefaultIfEmpty() // Left join
                                                       select new StoreResponseModel
                                                       {
                                                           Id = store.Id,
                                                           Name = store.Name,
                                                           Email = store.Email,
                                                           PhoneNumber = store.PhoneNumber,
                                                           Address = store.Address == null ? null : new AddressResponseModel
                                                           {
                                                               RoadName = store.Address.RoadName,
                                                               Province = store.Address.Province,
                                                               District = store.Address.District,
                                                               Ward = store.Address.Ward,
                                                               PostalCode = store.Address.PostalCode,
                                                               Latitude = store.Address.latitude,
                                                               Longitude = store.Address.longitude
                                                           },
                                                           IsActive = store.IsActive,
                                                           CreatedAt = store.CreatedAt,
                                                           CreatedBy = store.CreatedBy,
                                                           UpdatedAt = store.UpdatedAt,
                                                           UpdatedBy = store.UpdatedBy,
                                                           MapGalleryStores = _appDbContext.MapGalleryStores
                                                               .Where(mgs => mgs.StoreId == store.Id)
                                                               .Select(mgs => new MapGalleryStoreResponseModel
                                                               {
                                                                   GalleryId = mgs.GalleryId,
                                                                   FileName = gallery.FileName,
                                                                   Url = gallery.Url,
                                                                   IsThumbnail = gallery.IsThumbnail
                                                               }).ToList(),
                                                           RatingStore = new RatingStore()
                                                           {
                                                               NumberRating = store.Reviews == null ? 0 : store.Reviews.Count,
                                                               PointOfRating = store.Reviews == null || !store.Reviews.Any()
                                                                                               ? 0
                                                                                                : store.Reviews.Average(r => r.RatingValue)
                                                           }

                                                       };

                //Thêm điều kiện tìm kiếm theo tên
                if (!string.IsNullOrWhiteSpace(paginationRequest.SearchString))
                {
                    string normalizedSearchString = CommonUtils.vietnameseReplace(paginationRequest.SearchString);
                    query = query.Where(store => store.Name != null && store.Name.ToUpper().Contains(normalizedSearchString));
                }

                response.TotalCount = await query.CountAsync();
                IQueryable<StoreResponseModel> pagedQuery = query
                    .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                    .Take(paginationRequest.PageSize);

                response.Data = await pagedQuery.ToListAsync();
                response.SearchString = paginationRequest.SearchString;
                response.PageIndex = paginationRequest.PageIndex;
                response.PageSize = paginationRequest.PageSize;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category list");
            }
            await Task.Yield(); /*Thêm một câu lệnh await Task.Yield() để đảm bảo phương thức trả về một Task*/
            return response;
        }


        public async Task<BaseResponseResult<StoreResponseModel>> GetDetailStoreAsync(string storeId)
        {
            BaseResponseResult<StoreResponseModel> response = new();
            try
            {
                Store? store = await _appDbContext.Stores
                    .Include(s => s.Address)
                    .Include(s => s.Reviews!).ThenInclude(s => s.User)
                    .Include(s => s.MapGalleryStores!)
                        .ThenInclude(mgs => mgs.Gallery)  // Ensure Galleries are loaded
                    .FirstOrDefaultAsync(s => s.Id == storeId);


                if (store != null)
                {
                    StoreResponseModel storeResponseModel = _mapper.Map<StoreResponseModel>(store);
                    // Map galleries
                    storeResponseModel.MapGalleryStores = store.MapGalleryStores != null ? store.MapGalleryStores
                        .Select(mgs => new MapGalleryStoreResponseModel
                        {
                            GalleryId = mgs.GalleryId,
                            Key = mgs.Gallery?.PublicId,
                            FileName = mgs.Gallery?.FileName,
                            Url = mgs.Gallery?.Url,
                            IsThumbnail = mgs.Gallery?.IsThumbnail ?? false
                        })
                        .ToList() : new List<MapGalleryStoreResponseModel>();
                    storeResponseModel.Reviews = store.Reviews != null ? store.Reviews.Select(m => new ReviewResponseModel
                    {
                        Id = m.Id,
                        Content = m.Content,
                        RatingValue = m.RatingValue,
                        Status = m.Status,
                        UserId = m.UserId,
                        StoreId = m.StoreId,
                        UserEmail = m.User != null ? m.User.Email : string.Empty

                    }).ToList() : new List<ReviewResponseModel>();

                    response.Success = true;
                    response.Data = storeResponseModel;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Store not found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponseResult<StoreResponseModel>> UpdateStoreAsync(string storeId, StoreRequestModel request)
        {
            BaseResponseResult<StoreResponseModel> response = new();

            try
            {
                // Validate storeId and request
                if (string.IsNullOrEmpty(storeId) || request == null)
                {
                    response.Success = false;
                    response.Message = "Invalid storeId or request.";
                    return response;
                }

                // Retrieve the existing store entity from the database
                Store? existingStore = await _appDbContext.Stores
                    .Include(s => s.Address) // Include related address entity
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (existingStore == null)
                {
                    response.Success = false;
                    response.Message = "Store not found.";
                    return response;
                }

                // Update store properties
                _ = _mapper.Map(request, existingStore); // Map request to existing store entity

                // Update address properties
                _ = _mapper.Map(request, existingStore.Address); // Map request to existing address entity

                // Save the updated store entity back to the database
                _ = _appDbContext.Stores.Update(existingStore);
                _ = await _appDbContext.SaveChangesAsync();

                // Construct and return the response
                response.Success = true;
                response.Message = "Store updated successfully.";
                response.Data = _mapper.Map<StoreResponseModel>(existingStore);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                response.Success = false;
                response.Message = $"Error updating store: {ex.Message}";
            }

            return response;

        }

        public async Task<BaseResponse> DeleteStoreAsync(string storeId)
        {
            BaseResponse response = new();
            try
            {
                Store? store = await _appDbContext.Stores.FindAsync(storeId);
                if (store == null)
                {
                    response.Success = false;
                    response.Message = "Store not found";
                    return response;
                }

                store.IsActive = false;
                _ = _appDbContext.Stores.Update(store);

                _ = await _appDbContext.SaveChangesAsync();

                response.Success = true;
                response.Message = "Store deleted successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error deleting store: {ex.Message}";
            }
            return response;
        }

        public async Task<BaseResponseResult<List<SimpleStoreResponse>>> GetTheNearestStore(GetNearStoreRequestModel getNearStoreRequest, BasePaginationRequest paginationRequest)
        {



            BaseResponseResult<List<SimpleStoreResponse>> response = new() { Success = false };

            try
            {


                if (getNearStoreRequest.District.IsNullOrEmpty()
                    && getNearStoreRequest.Province.IsNullOrEmpty()
                    && getNearStoreRequest.Ward.IsNullOrEmpty()
                    && getNearStoreRequest.keyWord.IsNullOrEmpty()
                    && getNearStoreRequest.Categories.IsNullOrEmpty())

                {
                    response.Message = GlobalConstants.NOT_STORE_NEAR;
                    response.Data = null;
                    return response;
                }
                // Query to get stores and their addresses
                var storeQuery = from store in _appDbContext.Stores
                                 join address in _appDbContext.Addresses on store.AddressId equals address.Id

                                 select new
                                 {
                                     Store = store,
                                     Address = address
                                 };


                // Fetch products with galleries
                var productQuery = from product in _appDbContext.Products
                                   join gaProduct in _appDbContext.mapGalleryProducts on product.Id equals gaProduct.ProductId into prodGaProducts
                                   from gaProduct in prodGaProducts.DefaultIfEmpty()
                                   join gallery in _appDbContext.Galleries on gaProduct.GalleryId equals gallery.Id into gaProductGalleries
                                   from gallery in gaProductGalleries.DefaultIfEmpty()
                                   join childCategory in _appDbContext.Categories on product.CategoryId equals childCategory.Id into productCategories
                                   from childCategory in productCategories.DefaultIfEmpty()
                                   join parentCategory in _appDbContext.Categories on childCategory.ParentCategoryId equals parentCategory.Id into parentCategories
                                   from parentCategory in parentCategories.DefaultIfEmpty()

                                   select new
                                   {
                                       Product = product,
                                       GalleryUrl = gallery != null ? gallery.Url : string.Empty,
                                       ChildCategory = childCategory != null ? childCategory.Name : string.Empty,
                                       ParentCategory = parentCategory != null ? parentCategory.Name : string.Empty,
                                       ParentCategoryId = parentCategory != null ? parentCategory.ParentCategoryId : string.Empty
                                   };
                if (getNearStoreRequest.Categories != null)
                {
                    productQuery = productQuery.Where(x => getNearStoreRequest.Categories.Contains(x.ParentCategoryId));
                }


                // Fetch reviews for stores
                IQueryable<ReviewResponseModel> reviewQuery = from review in _appDbContext.Reviews
                                                              join user in _appDbContext.Users on review.UserId equals user.Id into reviewUsers
                                                              from user in reviewUsers.DefaultIfEmpty()

                                                              select new ReviewResponseModel
                                                              {
                                                                  Id = review.Id,
                                                                  StoreId = review.StoreId,
                                                                  Content = review.Content,
                                                                  RatingValue = review.RatingValue,
                                                                  Status = review.Status,
                                                                  UserId = review.UserId,
                                                                  UserEmail = user != null ? user.Email : string.Empty

                                                              };



                // Fetch Images for stores
                IQueryable<MapGalleryStoreResponseModel> imageQuery = from image in _appDbContext.Galleries
                                                                      join mapGalleryStore in _appDbContext.MapGalleryStores on image.Id equals mapGalleryStore.GalleryId

                                                                      select new MapGalleryStoreResponseModel
                                                                      {
                                                                          GalleryId = image.Id,
                                                                          Key = image.PublicId,
                                                                          FileName = image.FileName,
                                                                          Url = image.Url,
                                                                          IsThumbnail = image.IsThumbnail,
                                                                          StoreId = mapGalleryStore.StoreId

                                                                      };
                var storesWithAddresses = await storeQuery.ToListAsync();
                var productsWithGalleries = await productQuery.ToListAsync();
                List<MapGalleryStoreResponseModel> imageWithStores = await imageQuery.ToListAsync();


                List<ReviewResponseModel> reviews = await reviewQuery.ToListAsync();

                // Step 3: Combine results in-memory
                List<SimpleStoreResponse> query = storesWithAddresses.Select(s => new SimpleStoreResponse
                {
                    Id = s.Store.Id.ToString(), // Assuming store.Id is a Guid or similar type
                    Name = s.Store.Name,
                    Email = s.Store.Email,
                    PhoneNumber = s.Store.PhoneNumber,
                    Address = new AddressResponseModel
                    {
                        RoadName = s.Address.RoadName,
                        Ward = s.Address.Ward,
                        Province = s.Address.Province,
                        Latitude = s.Address.latitude,
                        Longitude = s.Address.longitude,
                        PostalCode = s.Address.PostalCode,
                        District = s.Address.District
                    },
                    Products = productsWithGalleries
                                .Where(p => p.Product.StoreId == s.Store.Id)
                                .Select(p => new SimpleProductResponse
                                {
                                    Id = p.Product.Id.ToString(),
                                    Name = p.Product.Name,
                                    Image = p.GalleryUrl
                                }).ToList(),
                    Reviews = reviews
                        .Where(r => r.StoreId == s.Store.Id)
                        .Select(r => new ReviewResponseModel
                        {
                            Id = r.Id,
                            StoreId = r.StoreId,
                            Content = r.Content,
                            RatingValue = r.RatingValue,
                            Status = r.Status,
                            UserId = r.UserId,
                            UserEmail = r.UserEmail
                        }).ToList(),
                    Images = imageWithStores.Where(r => r.StoreId == s.Store.Id)
                             .Select(i => new MapGalleryStoreResponseModel
                             {
                                 GalleryId = i.GalleryId,
                                 StoreId = i.StoreId,
                                 Url = i.Url,
                                 FileName = i.FileName,
                                 Key = i.Key,


                             }).ToList()
                }).ToList();

                // Apply filters
                if (!string.IsNullOrEmpty(getNearStoreRequest.Province))
                {


                    string province = CommonUtils.vietnameseReplace(getNearStoreRequest.Province
          .Replace("city", "", StringComparison.OrdinalIgnoreCase)
          .Replace("thanh pho", "", StringComparison.OrdinalIgnoreCase)
          .Replace("tinh", "", StringComparison.OrdinalIgnoreCase)).ToUpper().Trim();

                    query = query.Where(x => !string.IsNullOrEmpty(x.Address?.Province) &&
                                   CommonUtils.vietnameseReplace(x.Address.Province).ToUpper().Contains(province)).ToList();
                }

                if (!string.IsNullOrEmpty(getNearStoreRequest.District))
                {


                    string district = CommonUtils.vietnameseReplace(getNearStoreRequest.District
                          .Replace("quan", "", StringComparison.OrdinalIgnoreCase)
                          .Replace("q.", "", StringComparison.OrdinalIgnoreCase)
                          .Replace("district", "", StringComparison.OrdinalIgnoreCase)
                          .Replace("h.", "", StringComparison.OrdinalIgnoreCase)
                          .Replace("huyen", "", StringComparison.OrdinalIgnoreCase))
                          .Replace("thanh pho", "", StringComparison.OrdinalIgnoreCase).ToUpper().Trim();


                    List<string> nearbyDistricts = await GetNearDistrict(district);
                    if (!nearbyDistricts.Contains(district))
                    {
                        nearbyDistricts.Add(district);
                    }
                    query = query.Where(x => nearbyDistricts.Exists(nearbyDistrict =>
                                     CommonUtils.vietnameseReplace(x.Address?.District ?? "").ToUpper() == nearbyDistrict.ToUpper())).ToList();
                }
                if (!string.IsNullOrEmpty(getNearStoreRequest.Ward))
                {

                    string ward = CommonUtils.vietnameseReplace(getNearStoreRequest.Ward
                                  .Replace("phuong", "", StringComparison.OrdinalIgnoreCase)
                                  .Replace("p.", "", StringComparison.OrdinalIgnoreCase)
                                  .Replace("ward", "", StringComparison.OrdinalIgnoreCase))
                                 .ToUpper().Trim();

                    query = query.Where(x => !string.IsNullOrEmpty(x.Address?.Ward) &&
                                 CommonUtils.vietnameseReplace(x.Address.Ward).ToUpper().Contains(ward)).ToList();
                }


                if (!string.IsNullOrEmpty(getNearStoreRequest.keyWord))
                {
                    string keyword = CommonUtils.vietnameseReplace(getNearStoreRequest.keyWord).ToUpper();
                    List<SimpleStoreResponse> filteredStores = query.Where(store => store.Products.Exists(product => !string.IsNullOrEmpty(product.Name) && CommonUtils.vietnameseReplace(product.Name).ToUpper().Contains(keyword))).ToList();

                    // If no products match the keyword, search by store name
                    if (!filteredStores.Any())
                    {
                        filteredStores = query
                            .Where(store => !string.IsNullOrEmpty(store.Name)
                            && CommonUtils.vietnameseReplace(store.Name).ToUpper().Contains(keyword)).ToList();
                    }

                    query = filteredStores;
                }

                List<SimpleStoreResponse> nearestStores = query
                     .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                     .Take(paginationRequest.PageSize).ToList();

                //List<SimpleStoreResponse> nearestStores = query.ToList(); // Make sure it's a List


                if (nearestStores.Any())
                {
                    response.Code = System.Net.HttpStatusCode.OK.ToString();
                    response.Message = GlobalConstants.SUCCESSFULL;
                    response.Data = nearestStores;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = GlobalConstants.NOT_STORE_NEAR;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                response.Success = false;
                response.Message = $"Error retrieving nearest stores: {ex.Message}";
            }

            return response;




        }

        private async Task<List<string>> GetNearDistrict(string district)
        {
            district = CommonUtils.vietnameseReplace(district).ToUpper(); // Chuyển đổi chuỗi đầu vào thành chữ hoa

            return district switch
            {
                "1" => await Task.FromResult(new List<string> { "1", "3", "4", "5", "Binh Thanh", "Phu Nhuan" }),
                "2" => await Task.FromResult(new List<string> { "2", "Binh Thanh", "9", "Thu Duc", "4", "7" }),
                "3" => await Task.FromResult(new List<string> { "1", "3", "10", "Phu Nhuan", "Tan Binh" }),
                "4" => await Task.FromResult(new List<string> { "1", "7", "8", "2", "4" }),
                "5" => await Task.FromResult(new List<string> { "1", "5", "6", "10", "11", "8" }),
                "6" => await Task.FromResult(new List<string> { "5", "6", "11", "Binh Tan", "8" }),
                "7" => await Task.FromResult(new List<string> { "2", "4", "8", "Nha Be", "Binh Chanh" }),
                "8" => await Task.FromResult(new List<string> { "4", "5", "6", "7", "Binh Tan", "Binh Chanh", "8" }),
                "9" => await Task.FromResult(new List<string> { "2", "Thu Duc", "9" }),
                "10" => await Task.FromResult(new List<string> { "3", "5", "10", "11", "Tan Binh" }),
                "11" => await Task.FromResult(new List<string> { "5", "6", "10", "11", "Tan Binh", "Binh Tan" }),
                "12" => await Task.FromResult(new List<string> { "Go Vap", "Binh Thanh", "Thu Duc", "Tan Binh", "Hoc Mon", "12" }),
                "BINH THANH" => await Task.FromResult(new List<string> { "1", "2", "Go Vap", "Phu Nhuan", "Thu Duc", "BINH THANH" }),
                "TAN BINH" => await Task.FromResult(new List<string> { "3", "10", "11", "Phu Nhuan", "Tan Phu", "TAN BINH" }),
                "TAN PHU" => await Task.FromResult(new List<string> { "Tan Binh", "Binh Tan", "11", "TAN PHU" }),
                "PHU NHUAN" => await Task.FromResult(new List<string> { "1", "3", "Binh Thanh", "Tan Binh", "PHU NHUAN" }),
                "THU DUC" => await Task.FromResult(new List<string> { "2", "9", "Binh Thanh", "12" }),
                "BINH TAN" => await Task.FromResult(new List<string> { "6", "8", "Tan Phu", "Binh Chanh", "BINH TAN" }),
                "HOC MON" => await Task.FromResult(new List<string> { "12", "Cu Chi", "Binh Tan" }),
                "CU CHI" => await Task.FromResult(new List<string> { "Hoc Mon", "Binh Chanh", "CU CHI" }),
                "BINH CHANH" => await Task.FromResult(new List<string> { "7", "8", "Binh Tan", "Nha Be", "Cu Chi", "BINH CHANH" }),
                "NHA BE" => await Task.FromResult(new List<string> { "7", "Binh Chanh", "NHA BE" }),
                _ => await Task.FromResult(new List<string>())
            };
        }
    }
}