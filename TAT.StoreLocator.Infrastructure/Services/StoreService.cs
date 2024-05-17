//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using TAT.StoreLocator.Infrastructure.Persistence.EF;
//using TAT.StoreLocator.Core.Models.Response.Store;
//using TAT.StoreLocator.Core.Interface.IServices;
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
                var newStoreId = Guid.NewGuid().ToString();
                var newStoreEntity = new Store
                {
                    Id = newStoreId,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                };
                _appDbContext.Stores.Add(newStoreEntity);

                await _appDbContext.SaveChangesAsync();

                var newStoreResponse = new CreateStoreResponseModel
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
            var response = new BaseResponseResult<List<StoreResponseModel>>();

            try
            {
                var query = (from store in _appDbContext.Stores
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
            var response = new BaseResponseResult<StoreResponseModel>();
            try
            {
                var store = await _appDbContext.Stores
                    .Include(s => s.Address)
                    .Include(s => s.MapGalleryStores)
                        .ThenInclude(mgs => mgs.Gallery)  // Ensure Galleries are loaded
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (store != null)
                {
                    var storeResponseModel = _mapper.Map<StoreResponseModel>(store);
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

        public async Task<BaseResponseResult<StoreResponseModel>>UpdateStoreAsync ( string storeId,UpdateStoreRequestModel request)
        {
            var response = new BaseResponseResult<StoreResponseModel>();
            try
            {
                var store = await _appDbContext.Stores.FindAsync(storeId);
                if (store == null) 
                {
                    response.Success = false;
                    response.Message = "Store not found";
                    return response;
                }

                store.Name = request.Name;
                store.Email = request.Email;
                store.PhoneNumber = request.PhoneNumber;

                await _appDbContext.SaveChangesAsync();

                var updateStoreResponse = new StoreResponseModel
                {
                    Id = store.Id,
                    Name = store.Name,
                    Email = store.Email,
                    PhoneNumber = store.PhoneNumber,
                };
                response.Success = true;
                response.Data = updateStoreResponse;
            }
            catch(Exception ex) 
            { 
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponse> DeleteStoreAsync(string storeId)
        {
            var response = new BaseResponse();
            try
            {
                var store = await _appDbContext.Stores.FindAsync(storeId);
                if (store == null)
                {
                    response.Success = false;
                    response.Message = "Store not found";
                    return response;
                }


                store.IsDeleted = true;
                _appDbContext.Stores.Update(store);

                await _appDbContext.SaveChangesAsync();

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
    }
}










