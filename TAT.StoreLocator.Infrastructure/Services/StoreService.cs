using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.Store;
using TAT.StoreLocator.Core.Models.Response.Store;
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

                CreateStoreResponseModel newStoreResponse = new()
                {
                    Id = newStoreId,
                    BaseResponse = new BaseResponse { Success = true, Message = "Store created successfully." },
                    StoreResponseModel = new StoreResponseModel
                    {
                        Id = newStoreId,
                        Name = request.Name,
                        PhoneNumber = request.PhoneNumber,
                    }
                };
                return newStoreResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating store : " + ex.Message);
            }
        }

        public async Task<BaseResponseResult<List<StoreResponseModel>>> GetAllStoreAsync()
        {
            BaseResponseResult<List<StoreResponseModel>> response = new();

            try
            {
                List<StoreResponseModel> query = (from store in _appDbContext.Stores
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


                                                  }).ToList();
                if (!query.IsNullOrEmpty())
                {
                    response.Code = GlobalConstants.SUCCESSFULL;
                    response.Message = HttpStatusCode.OK.ToString();
                    response.Data = query;
                    response.Success = true;
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

        public async Task<BaseResponseResult<StoreResponseModel>> GetDetailStoreAsync(string storeId)
        {
            BaseResponseResult<StoreResponseModel> response = new();
            try
            {
                Store? store = await _appDbContext.Stores
                    .Include(s => s.Address)
                    .Include(s => s.MapGalleryStores)
                        .ThenInclude(mgs => mgs.Gallery)  // Ensure Galleries are loaded
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (store != null)
                {
                    StoreResponseModel storeResponseModel = _mapper.Map<StoreResponseModel>(store);
                    // Map galleries
                    storeResponseModel.MapGalleryStores = store.MapGalleryStores
                        .Select(mgs => new MapGalleryStoreResponse
                        {
                            GalleryId = mgs.GalleryId,
                            FileName = mgs.Gallery?.FileName,
                            Url = mgs.Gallery?.Url,
                            IsThumbnail = mgs.Gallery?.IsThumbnail ?? false
                        })
                        .ToList();

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

        public async Task<BaseResponseResult<List<SimpleStoreResponse>>> GetTheNearestStore(string district)
        {
            BaseResponseResult<List<SimpleStoreResponse>> response = new();
            // Chuyển đổi chuỗi "Quận" thành "Q." để so sánh dễ dàng hơn
            if (district.StartsWith("Quận"))
            {
                district = district.Replace("Quận", "Q.");
            }
            if (district.StartsWith("Huyện"))
            {
                district = district.Replace("Huyện", "H.");
            }
            List<string> list = await GetNearDistrict(district);

            // Ensure that there is at least one nearby district
            if (list == null || !list.Any())
            {
                response.Success = false;
                response.Message = GlobalConstants.DISTRICT_NOT_FOUND;
                response.Data = null;
                return response; // Return an empty response if no nearby districts are found
            }

            // Query the stores whose addresses are in the nearby districts
            IQueryable<SimpleStoreResponse> query = from store in _appDbContext.Stores
                                                    join address in _appDbContext.Addresses
                                                        on store.AddressId equals address.Id
                                                    where address.District != null && list.Contains(address.District)
                                                    select new SimpleStoreResponse
                                                    {
                                                        Id = store.Id.ToString(), // Assuming store.Id is a Guid or similar type
                                                        Name = store.Name,
                                                        Email = store.Email,
                                                        PhoneNumber = store.PhoneNumber,
                                                        Address = new AddressResponseModel
                                                        {
                                                            RoadName = address.RoadName,
                                                            Ward = address.Ward,
                                                            Province = address.Province,
                                                            Latitude = address.latitude,
                                                            Longitude = address.longitude,
                                                            PostalCode = address.PostalCode,
                                                            District = address.District
                                                        }
                                                    };

            // Execute the query and get the list of stores
            List<SimpleStoreResponse> nearestStores = await query.ToListAsync();

            if (nearestStores != null && nearestStores.Any())
            {
                response.Code = HttpStatusCode.OK.ToString();
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
            district = district.ToUpper(); // Chuyển đổi chuỗi đầu vào thành chữ hoa

            return district switch
            {
                "Q.1" => await Task.FromResult(new List<string>
            {
                "Quận 3",
                "Quận 4",
                "Quận 5",
                "Quận Bình Thạnh",
                "Quận Phú Nhuận"
            }),

                "Q.2" => await Task.FromResult(new List<string>
            {
                "Quận Bình Thạnh",
                "Quận 9",
                "Quận Thủ Đức",
                "Quận 4",
                "Quận 7"
            }),

                "Q.3" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 10",
                "Quận Phú Nhuận",
                "Quận Tân Bình"
            }),

                "Q.4" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 7",
                "Quận 8",
                "Quận 2"
            }),

                "Q.5" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 6",
                "Quận 10",
                "Quận 11",
                "Quận 8"
            }),

                "Q.6" => await Task.FromResult(new List<string>
            {
                "Quận 5",
                "Quận 11",
                "Quận Bình Tân",
                "Quận 8"
            }),

                "Q.7" => await Task.FromResult(new List<string>
            {
                "Quận 2",
                "Quận 4",
                "Quận 8",
                "Huyện Nhà Bè",
                "Huyện Bình Chánh"
            }),

                "Q.8" => await Task.FromResult(new List<string>
            {
                "Quận 4",
                "Quận 5",
                "Quận 6",
                "Quận 7",
                "Quận Bình Tân",
                "Huyện Bình Chánh"
            }),

                "Q.9" => await Task.FromResult(new List<string>
            {
                "Quận 2",
                "Quận Thủ Đức"
            }),

                "Q.10" => await Task.FromResult(new List<string>
            {
                "Quận 3",
                "Quận 5",
                "Quận 11",
                "Quận Tân Bình"
            }),

                "Q.11" => await Task.FromResult(new List<string>
            {
                "Quận 5",
                "Quận 6",
                "Quận 10",
                "Quận Tân Bình",
                "Quận Bình Tân"
            }),

                "Q.12" => await Task.FromResult(new List<string>
            {
                "Quận Gò Vấp",
                "Quận Bình Thạnh",
                "Quận Thủ Đức",
                "Quận Tân Bình",
                "Huyện Hóc Môn"
            }),

                "Q.BÌNH THẠNH" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 2",
                "Quận Gò Vấp",
                "Quận Phú Nhuận",
                "Quận Thủ Đức"
            }),

                "Q.TÂN BÌNH" => await Task.FromResult(new List<string>
            {
                "Quận 3",
                "Quận 10",
                "Quận 11",
                "Quận Phú Nhuận",
                "Quận Tân Phú"
            }),

                "Q.TÂN PHÚ" => await Task.FromResult(new List<string>
            {
                "Quận Tân Bình",
                "Quận Bình Tân",
                "Quận 11"
            }),

                "Q.PHÚ NHUẬN" => await Task.FromResult(new List<string>
            {
                "Quận 1",
                "Quận 3",
                "Quận Bình Thạnh",
                "Quận Tân Bình"
            }),

                "Q.THỦ ĐỨC" => await Task.FromResult(new List<string>
            {
                "Quận 2",
                "Quận 9",
                "Quận Bình Thạnh",
                "Quận 12"
            }),

                "Q.BÌNH TÂN" => await Task.FromResult(new List<string>
            {
                "Quận 6",
                "Quận 8",
                "Quận Tân Phú",
                "Quận Bình Chánh"
            }),

                "H.HÓC MÔN" => await Task.FromResult(new List<string>
            {
                "Quận 12",
                "Huyện Củ Chi",
                "Quận Bình Tân"
            }),

                "H.CỦ CHI" => await Task.FromResult(new List<string>
            {
                "Huyện Hóc Môn",
                "Huyện Bình Chánh"
            }),

                "H.BÌNH CHÁNH" => await Task.FromResult(new List<string>
            {
                "Quận 7",
                "Quận 8",
                "Quận Bình Tân",
                "Huyện Nhà Bè",
                "Huyện Củ Chi"
            }),

                "H.NHÀ BÈ" => await Task.FromResult(new List<string>
            {
                "Quận 7",
                "Huyện Bình Chánh"
            }),

                _ => await Task.FromResult(new List<string>())
            };
        }
    }


}











