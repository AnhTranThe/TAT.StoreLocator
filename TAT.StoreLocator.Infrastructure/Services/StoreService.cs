﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        public StoreService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<CreateStoreResponseModel> CreateStoreAsync(CreateStoreRequestModel request)
        {
            CreateStoreResponseModel response = new();
            try
            {
                string newStoreId = Guid.NewGuid().ToString();
                Store newStoreEntity = new()
                {
                    Id = newStoreId,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                };
                _ = _appDbContext.Stores.Add(newStoreEntity);

                _ = await _appDbContext.SaveChangesAsync();

                response.Id = newStoreId;
                response.BaseResponse = new BaseResponse { Success = true, Message = "Store created successfully." };
                response.StoreResponseModel = new StoreResponseModel
                {
                    Id = newStoreId,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                };
            }
            catch (Exception ex)
            {
                response.BaseResponse = new BaseResponse { Success = false, Message = "Error creating store: " + ex.Message };
            }
            return response;
        }

        public async Task<BaseResponseResult<GetAllStoreResponseModel>> GetAllStoreAsync(BasePaginationRequest paginationRequest)
        {
            BaseResponseResult<GetAllStoreResponseModel> response = new();

            try
            {
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
                                                               }).ToList()
                                                       };

                int totalCount = await query.CountAsync();

                IQueryable<StoreResponseModel> pagedQuery = query
                    .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                    .Take(paginationRequest.PageSize);

                List<StoreResponseModel> storeList = await pagedQuery.ToListAsync();

                if (storeList.Any())
                {
                    response.Code = GlobalConstants.SUCCESSFULL;
                    response.Message = System.Net.HttpStatusCode.OK.ToString();
                    response.Data = new GetAllStoreResponseModel
                    {
                        PaginationRequest = paginationRequest,
                        StoreResponseList = storeList,
                        TotalCount = totalCount
                    };
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Stores not found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
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


                store.IsDeleted = true;
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
        public async Task<BaseResponseResult<List<SimpleStoreResponse>>> GetTheNearestStore(string district, string ward, string province, string keyWord)
        {
            BaseResponseResult<List<SimpleStoreResponse>> response = new();

            if (district.IsNullOrEmpty() && province.IsNullOrEmpty() && ward.IsNullOrEmpty() && keyWord.IsNullOrEmpty())
            {
                response.Success = false;
                response.Message = GlobalConstants.NOT_STORE_NEAR;
                response.Data = null;
                return response;
            }

            // Query the stores whose addresses are in the nearby districts
            var storeQuery = from store in _appDbContext.Stores
                             join address in _appDbContext.Addresses
                                 on store.AddressId equals address.Id
                             select new
                             {
                                 Store = store,
                                 Address = address
                             };

            var storesWithAddresses = storeQuery.ToList();

            // Step 2: Fetch products with galleries
            var productQuery = from product in _appDbContext.Products
                               join gaProduct in _appDbContext.mapGalleryProducts
                                   on product.Id equals gaProduct.ProductId into prodGaProducts
                               from gaProduct in prodGaProducts.DefaultIfEmpty()
                               join gallery in _appDbContext.Galleries
                                   on gaProduct.GalleryId equals gallery.Id into gaProductGalleries
                               from gallery in gaProductGalleries.DefaultIfEmpty()
                               select new
                               {
                                   Product = product,
                                   GalleryUrl = gallery != null ? gallery.Url : string.Empty
                               };

            var productsWithGalleries = productQuery.ToList();

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

            if (!province.IsNullOrEmpty())
            {
                province = CommonUtils.RemoveDiacritics(province).ToUpper();
                query = query.Where(x => province.Contains(x.Address != null ? x.Address.Province ?? "" : "")).ToList();
            }

            if (!district.IsNullOrEmpty())
            {
                if (district.StartsWith("Quận"))
                {
                    district = district.Replace("Quận", "Q.");
                }

                if (district.StartsWith("Huyện"))
                {
                    district = district.Replace("Huyện", "H.");
                }

                List<string> list = await GetNearDistrict(district);
                query = query.Where(x => list.Contains(x.Address != null ? x.Address.District ?? "" : "")).ToList();
            }

            if (!ward.IsNullOrEmpty())
            {
                if (ward.StartsWith("Quận"))
                {
                    ward = ward.Replace("Quận", "Q.");
                }

                if (ward.StartsWith("Huyện"))
                {
                    ward = ward.Replace("Huyện", "H.");
                }

                List<string> list = await GetNearDistrict(ward);
                query = query.Where(x => list.Contains(x.Address != null ? x.Address.Ward ?? "" : "")).ToList();
            }

            if (!keyWord.IsNullOrEmpty())
            {
                query = query.Where(store => store.Products.Any(product => product.Name != null && product.Name.Contains(keyWord))).ToList();
            }

            List<SimpleStoreResponse> nearestStores = query.ToList(); // Make sure it's a List

            if (nearestStores != null && nearestStores.Any())
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











