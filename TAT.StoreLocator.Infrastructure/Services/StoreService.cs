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

                // Create new address entity if address details are provided
                Address? newAddressEntity = null;
                if (request.Address != null)
                {
                    newAddressEntity = new Address
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoadName = request.Address.RoadName,
                        Province = request.Address.Province,
                        District = request.Address.District,
                        Ward = request.Address.Ward,
                        PostalCode = request.Address.PostalCode,
                        latitude = request.Address.Latitude,
                        longitude = request.Address.Longitude
                    };
                    _ = _appDbContext.Addresses.Add(newAddressEntity);
                }

                // Create new store entity
                string newStoreId = Guid.NewGuid().ToString();
                Store newStoreEntity = new()
                {
                    Id = newStoreId,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    AddressId = newAddressEntity?.Id,
                    IsActive = true
                };
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
                    Address = newAddressEntity == null ? null : new AddressResponseModel
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
                                                           on store.Id equals mapGalleryStore.StoreId
                                                       join gallery in _appDbContext.Galleries
                                                           on mapGalleryStore.GalleryId equals gallery.Id
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
                                                               .Select(mgs => new MapGalleryStoreResponse
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
                    .Include(s => s.MapGalleryStores!)
                        .ThenInclude(mgs => mgs.Gallery)  // Ensure Galleries are loaded
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (store != null)
                {
                    StoreResponseModel storeResponseModel = _mapper.Map<StoreResponseModel>(store);
                    // Map galleries
                    storeResponseModel.MapGalleryStores = store.MapGalleryStores != null ? store.MapGalleryStores
                        .Select(mgs => new MapGalleryStoreResponse
                        {
                            GalleryId = mgs.GalleryId,
                            Key = mgs.Gallery?.PublicId,
                            FileName = mgs.Gallery?.FileName,
                            Url = mgs.Gallery?.Url,
                            IsThumbnail = mgs.Gallery?.IsThumbnail ?? false
                        })
                        .ToList() : new List<MapGalleryStoreResponse>();

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

        public async Task<BaseResponseResult<StoreResponseModel>> UpdateStoreAsync(string storeId, UpdateStoreRequestModel request)
        {
            BaseResponseResult<StoreResponseModel> response = new();
            try
            {
                Store? store = await _appDbContext.Stores.FindAsync(storeId);
                if (store == null)
                {
                    response.Success = false;
                    response.Message = "Store not found";
                    return response;
                }

                store.Name = request.Name;
                store.Email = request.Email;
                store.PhoneNumber = request.PhoneNumber;
                store.IsActive = request.IsActive;

                _ = await _appDbContext.SaveChangesAsync();

                StoreResponseModel updateStoreResponse = new()
                {
                    Id = store.Id,
                    Name = store.Name,
                    Email = store.Email,
                    PhoneNumber = store.PhoneNumber,
                };
                response.Success = true;
                response.Data = updateStoreResponse;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
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

            if (getNearStoreRequest.District.IsNullOrEmpty()
                && getNearStoreRequest.Province.IsNullOrEmpty()
                && getNearStoreRequest.Ward.IsNullOrEmpty()
                && getNearStoreRequest.keyWord.IsNullOrEmpty())

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
                               select new
                               {
                                   Product = product,
                                   GalleryUrl = gallery != null ? gallery.Url : string.Empty
                               };

            // Execute queries and load data into memory
            var storesWithAddresses = await storeQuery.ToListAsync();
            var productsWithGalleries = await productQuery.ToListAsync();

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
                            }).ToList()
            }).ToList();

            // Apply filters
            if (!string.IsNullOrEmpty(getNearStoreRequest.Province))
            {


                string province = CommonUtils.RemoveDiacritics(getNearStoreRequest.Province
      .Replace("city", "", StringComparison.OrdinalIgnoreCase)
      .Replace("thanh pho", "", StringComparison.OrdinalIgnoreCase)
      .Replace("tinh", "", StringComparison.OrdinalIgnoreCase)).ToUpper().Trim();

                query = query.Where(x => !string.IsNullOrEmpty(x.Address?.Province) &&
                               CommonUtils.RemoveDiacritics(x.Address.Province).ToUpper().Contains(province)).ToList();
            }

            if (!string.IsNullOrEmpty(getNearStoreRequest.District))
            {


                string district = CommonUtils.RemoveDiacritics(getNearStoreRequest.District
      .Replace("quan", "", StringComparison.OrdinalIgnoreCase)
      .Replace("district", "", StringComparison.OrdinalIgnoreCase)
      .Replace("huyen", "", StringComparison.OrdinalIgnoreCase)).ToUpper().Trim();


                //List<string> nearbyDistricts = await GetNearDistrict(district);
                //query = query.Where(x => !string.IsNullOrEmpty(x.Address?.District) &&
                //            CommonUtils.RemoveDiacritics(x.Address.District).ToUpper().Contains(province)).ToList();
                //query = query.Where(x => nearbyDistricts.Contains(x.Address?.District ?? string.Empty)).ToList();
            }
            if (!string.IsNullOrEmpty(getNearStoreRequest.Ward))
            {
                string ward = getNearStoreRequest.Ward.Replace("Quận", "Q.").Replace("Huyện", "H.");
                List<string> nearbyWards = await GetNearDistrict(ward);
                query = query.Where(x => nearbyWards.Contains(x.Address?.Ward ?? string.Empty)).ToList();
            }

            if (!string.IsNullOrEmpty(getNearStoreRequest.keyWord))
            {
                query = query.Where(store => store.Products.Exists(product => !string.IsNullOrEmpty(product.Name) && product.Name.Contains(getNearStoreRequest.keyWord))).ToList();
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

            return response;
        }

        private async Task<List<string>> GetNearDistrict(string district)
        {
            district = CommonUtils.RemoveDiacritics(district).ToUpper(); // Chuyển đổi chuỗi đầu vào thành chữ hoa

            return district switch
            {
                "1" => await Task.FromResult(new List<string> { "1", "3", "4", "5", "Binh Thanh", "Phu Nhuan" }),
                "2" => await Task.FromResult(new List<string> { "2", "Binh Thanh", "9", "Thu Duc", "4", "7" }),
                "3" => await Task.FromResult(new List<string> { "1", "3", "10", "Phu Nhuan", "Tan Binh" }),
                "4" => await Task.FromResult(new List<string> { "1", "7", "8", "2" }),
                "5" => await Task.FromResult(new List<string> { "1", "5", "6", "10", "11", "8" }),
                "6" => await Task.FromResult(new List<string> { "5", "6", "11", "Binh Tan", "8" }),
                "7" => await Task.FromResult(new List<string> { "2", "4", "8", "Nha Be", "Binh Chanh" }),
                "8" => await Task.FromResult(new List<string> { "4", "5", "6", "7", "Binh Tan", "Binh Chanh" }),
                "9" => await Task.FromResult(new List<string> { "2", "Thu Duc" }),
                "10" => await Task.FromResult(new List<string> { "3", "5", "10", "11", "Tan Binh" }),
                "11" => await Task.FromResult(new List<string> { "5", "6", "10", "11", "Tan Binh", "Binh Tan" }),
                "12" => await Task.FromResult(new List<string> { "Go Vap", "Binh Thanh", "Thu Duc", "Tan Binh", "Hoc Mon" }),
                "BINH THANH" => await Task.FromResult(new List<string> { "1", "2", "Go Vap", "Phu Nhuan", "Thu Duc" }),
                "TAN BINH" => await Task.FromResult(new List<string> { "3", "10", "11", "Phu Nhuan", "Tan Phu" }),
                "TAN PHU" => await Task.FromResult(new List<string> { "Tan Binh", "Binh Tan", "11" }),
                "PHU NHUAN" => await Task.FromResult(new List<string> { "1", "3", "Binh Thanh", "Tan Binh" }),
                "THU DUC" => await Task.FromResult(new List<string> { "2", "9", "Binh Thanh", "12" }),
                "BINH TAN" => await Task.FromResult(new List<string> { "6", "8", "Tan Phu", "Binh Chanh" }),
                "HOC MON" => await Task.FromResult(new List<string> { "12", "Cu Chi", "Binh Tan" }),
                "CU CHI" => await Task.FromResult(new List<string> { "Hoc Mon", "Binh Chanh" }),
                "BINH CHANH" => await Task.FromResult(new List<string> { "7", "8", "Binh Tan", "Nha Be", "Cu Chi" }),
                "NHA BE" => await Task.FromResult(new List<string> { "7", "Binh Chanh" }),
                _ => await Task.FromResult(new List<string>())
            };
        }
    }
}