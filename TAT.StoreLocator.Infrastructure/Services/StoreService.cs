﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using TAT.StoreLocator.Infrastructure.Persistence.EF;
//using TAT.StoreLocator.Core.Models.Response.Store;
//using TAT.StoreLocator.Core.Interface.IServices;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
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

        //private string GenerateNewStoreId()
        //{
        //    return Guid.NewGuid().ToString();
        //}

        public async Task<BaseResponseResult<List<StoreResponseModel>>> GetAllStoreAsync()
        {
            var response = new BaseResponseResult<List<StoreResponseModel>>();

            try
            {
                var store = await _appDbContext.Stores.ToListAsync();
                if (store != null)
                {
                    var storeResponseModel = _mapper.Map<List<StoreResponseModel>>(store);
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
    }
}









