using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
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
            CreateStoreResponseModel response = new CreateStoreResponseModel();
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

                response.Id =newStoreId;
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

        public async Task<BaseResponseResult<List<StoreResponseModel>>> GetAllStoreAsync()
        {
            BaseResponseResult<List<StoreResponseModel>> response = new BaseResponseResult<List<StoreResponseModel>>();

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
                    response.Message = System.Net.HttpStatusCode.OK.ToString();
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
            var response = new BaseResponseResult<List<SimpleStoreResponse>>();


            // Ensure that there is at least one nearby district
            if (district.IsNullOrEmpty() && province.IsNullOrEmpty() && ward.IsNullOrEmpty() && keyWord.IsNullOrEmpty())
            {
                response.Success = false;
                response.Message = GlobalConstants.NOT_STORE_NEAR;
                response.Data = null;
                return response; // Return an empty response if no nearby districts are found
            }

            // Query the stores whose addresses are in the nearby districts
            var query = from store in _appDbContext.Stores
                        join address in _appDbContext.Addresses
                            on store.AddressId equals address.Id
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
            if (!province.IsNullOrEmpty())
            {
                province = RemoveDiacritics(province).ToUpper(); // Convert input string to uppercase
                query = query.Where(x => province.Contains(x.Address.Province));
            }
            if (!district.IsNullOrEmpty())
            {
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
                query = query.Where(x => list.Contains(x.Address.District));
            }
            if (!keyWord.IsNullOrEmpty())
            {
                query = query.Where(x => x.Name.Contains(keyWord));
            }
            // Execute the query and get the list of stores
            List<SimpleStoreResponse> nearestStores = await query.ToListAsync();

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
            district = RemoveDiacritics(district).ToUpper(); // Chuyển đổi chuỗi đầu vào thành chữ hoa

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

        private Task<List<string>> GetDistrictInProvision(string provision)
        {
            provision = RemoveDiacritics(provision).ToUpper(); // Convert input string to uppercase
            return Task.FromResult(provision switch
            {
                "HO CHI MINH" => new List<string>
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12",
            "Binh Thanh", "Tan Binh", "Tan Phu", "Phu Nhuan", "Thu Duc",
            "Binh Tan", "Go Vap", "Hoc Mon", "Cu Chi", "Binh Chanh", "Nha Be", "Can Gio"
        },

                "HANOI" => new List<string>
        {
            "Ba Dinh", "Hoan Kiem", "Tay Ho", "Long Bien", "Cau Giay", "Dong Da",
            "Hai Ba Trung", "Hoang Mai", "Thanh Xuan", "Soc Son", "Dong Anh",
            "Gia Lam", "Nam Tu Liem", "Thanh Tri", "Bac Tu Liem", "Me Linh",
            "Ha Dong", "Son Tay", "Ba Vi", "Phuc Tho", "Dan Phuong",
            "Hoai Duc", "Quoc Oai", "Thach That", "Chuong My", "Thanh Oai",
            "Thuong Tin", "Phu Xuyen", "Ung Hoa", "My Duc"
        },

                "DA NANG" => new List<string>
        {
            "Hai Chau", "Thanh Khe", "Son Tra", "Ngu Hanh Son", "Lien Chieu",
            "Cam Le", "Hoa Vang", "Hoang Sa"
        },

                "CAN THO" => new List<string>
        {
            "Ninh Kieu", "Binh Thuy", "Cai Rang", "O Mon", "Thot Not",
            "Phong Dien", "Co Do", "Vinh Thanh", "Thoi Lai"
        },

                "HAI PHONG" => new List<string>
        {
            "Hong Bang", "Le Chan", "Ngo Quyen", "Kien An", "Hai An",
            "Duong Kinh", "Do Son", "Thuy Nguyen", "An Duong", "An Lao",
            "Tien Lang", "Vinh Bao", "Cat Hai", "Bach Long Vi"
        },

                // Add remaining provinces and their districts as needed

                _ => new List<string>() // Default case if the provision does not match any case
            });
        }




        protected string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }


}











