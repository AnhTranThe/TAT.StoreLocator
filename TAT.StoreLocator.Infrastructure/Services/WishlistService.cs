using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Core.Models.Request.WishList;
using TAT.StoreLocator.Core.Models.Response.Wishlist;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _dbContext;

        public WishlistService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Change Status wishlist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="status"></param>
        /// if status is false
        /// <returns>added wislist</returns>
        public async Task<BaseResponseResult<bool>> ChangeStatusProduct(WishListRequestProduct request, bool status)
        {
         
            try
            {
                if (request.UserId == null || request.ProductId == null ){

                    return CreateErrorResponse("Request is null");
                }
                Wishlist? wishlist = await GetOrCreateWishlistAsync(request.UserId,!status);

                if (wishlist == null)
                {
                    return CreateErrorResponse("Wishlist not found.");
                }

                if (!status)
                {
                    AddProductToWishlist(wishlist, request.ProductId);
                    status = true;
                }
                else
                {
                    RemoveProductFromWishlist(wishlist, request.ProductId);
                    status = false;
                }

                return CreateSuccessResponse(status, status ? "Product added to wishlist." : "Product removed from wishlist.");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse("An error occurred while toggling product in wishlist. " + ex.Message);
            }
        }
        public async Task<BaseResponseResult<bool>> GetStatusProduct(WishListRequestProduct request)
        {
           
            try
            {
                if(request.UserId == null)
                {
                    return CreateErrorResponse("userId is null or emty");
                }
                Wishlist? wishlist = await GetWishlistAsync(request.UserId);

                if (wishlist == null)
                {
                    return CreateErrorResponse("Wishlist not found.");
                }

                if (string.IsNullOrEmpty(request.ProductId))
                {
                    return CreateErrorResponse("ProductId is null or empty.");
                }

                bool productExists = ProductExistsInWishlist(wishlist, request.ProductId);
                return CreateSuccessResponse(productExists, productExists ? "Product exist in wishlist." : "Product not found in wishlist.");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse("An error occurred while getting wishlist status. " + ex.Message);
            }
        }


        /// <summary>
        /// Change Status wishlist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="status"></param>
        /// if status is false
        /// <returns>added wislist</returns>
        public async Task<BaseResponseResult<bool>> ChangeStatusStore(WishListRequestStore request, bool status)
        {

            try
            {
                if (request.UserId == null || request.StoreId == null)
                {

                    return CreateErrorResponse("Request is null");
                }
                Wishlist? wishlist = await GetOrCreateWishlistAsync(request.UserId, !status);

                if (wishlist == null)
                {
                    return CreateErrorResponse("Wishlist not found.");
                }

                if (!status)
                {
                    AddStoreToWishlist(wishlist, request.StoreId);
                    status = true;
                }
                else
                {
                    RemoveStoreFromWishlist(wishlist, request.StoreId);
                    status = false;
                }

                return CreateSuccessResponse(status, status ? "Store added to wishlist." : "Store removed from wishlist.");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse("An error occurred while toggling Store in wishlist. " + ex.Message);
            }
        }

        public async Task<BaseResponseResult<bool>> GetStatusStore(WishListRequestStore request)
        {

            try
            {
                if (request.UserId == null || request.StoreId == null)
                {
                    return CreateErrorResponse("userId is null or emty");
                }

                Wishlist? wishlist = await GetWishlistAsync(request.StoreId);

                if (wishlist == null)
                {
                    return CreateErrorResponse("Wishlist not found.");
                }

                if (string.IsNullOrEmpty(request.StoreId))
                {
                    return CreateErrorResponse("StoreId is null or empty.");
                }

                bool productExists = StoreExistsInWishlist(wishlist, request.StoreId);
                return CreateSuccessResponse(productExists, productExists ? "Store exist in wishlist." : "Store not found in wishlist.");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse("An error occurred while getting wishlist status. " + ex.Message);
            }
        }




        private void RemoveStoreFromWishlist(Wishlist wishlist, string storeId)
        {
            if (wishlist.MapStoreWishlists == null)
            {
                throw new ArgumentNullException(nameof(wishlist), "MapStoreWishlists is null. Wishlist data may be corrupted.");
            }

            var mapStoreWishlist = wishlist.MapStoreWishlists.FirstOrDefault(mp => mp.StoreId == storeId);
            if (mapStoreWishlist != null)
            {
                _dbContext.mapStoreWishlists.Remove(mapStoreWishlist);
                _dbContext.SaveChanges();
            }
        }

        private void AddStoreToWishlist(Wishlist wishlist, string storeId)
        {
            if (wishlist.MapStoreWishlists == null)
            {
                throw new ArgumentNullException(nameof(wishlist), "MapStoreWishlists is null. Wishlist data may be not here.");
            }

            if (!wishlist.MapStoreWishlists.Any(mp => mp.StoreId == storeId))
            {
                var mapStoreWishlist = new MapStoreWishlist { WishlistId = wishlist.Id, StoreId = storeId };
                _dbContext.mapStoreWishlists.Add(mapStoreWishlist);
                _dbContext.SaveChanges();
            }
        }

        private bool StoreExistsInWishlist(Wishlist wishlist, string StoreId)
        {
            if (wishlist.MapStoreWishlists == null)
            {
                throw new ArgumentNullException(nameof(wishlist), "MapStoreWhislist is null. Wishlist data may be corrupted.");
            }
            return wishlist.MapStoreWishlists.Any(mp => mp.StoreId == StoreId);
        }
        private async Task<Wishlist?> GetWishlistAsync(string userId)
        {
            return await _dbContext.Wishlist
               .Include(w => w.MapProductWishlists)
               .Include(w => w.MapStoreWishlists)
               .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        private async Task<Wishlist?> GetOrCreateWishlistAsync(string userId, bool status)
        {
            Wishlist? wishlist = await GetWishlistAsync(userId);
            if (wishlist == null && status)
            {
                wishlist = new Wishlist { UserId = userId };
                _dbContext.Wishlist.Add(wishlist);
                await _dbContext.SaveChangesAsync();
            }
            return await GetWishlistAsync(userId);
        }

        private void AddProductToWishlist(Wishlist wishlist, string productId)
        {
            if (wishlist.MapProductWishlists == null)
            {
                throw new ArgumentNullException(nameof(wishlist), "MapProductWishlists is null. Wishlist data may be corrupted.");
            }

            if (!wishlist.MapProductWishlists.Any(mp => mp.ProductId == productId))
            {
                var mapProductWishlist = new MapProductWishlist { WishlistId = wishlist.Id, ProductId = productId };
                _dbContext.MapProductWishlists.Add(mapProductWishlist);
                _dbContext.SaveChanges();
            }
        }

        private void RemoveProductFromWishlist(Wishlist wishlist, string productId)
        {
            if (wishlist.MapProductWishlists == null)
            {
                throw new ArgumentNullException(nameof(wishlist), "MapProductWishlists is null. Wishlist data may be corrupted.");
            }

            var mapProductWishlist = wishlist.MapProductWishlists.FirstOrDefault(mp => mp.ProductId == productId);
            if (mapProductWishlist != null)
            {
                _dbContext.MapProductWishlists.Remove(mapProductWishlist);
                _dbContext.SaveChanges();
            }
        }

        private bool ProductExistsInWishlist(Wishlist wishlist, string productId)
        {
            if (wishlist.MapProductWishlists == null)
            {
                throw new ArgumentNullException(nameof(wishlist), "MapProductWishlists is null. Wishlist data may be corrupted.");
            }
            return wishlist.MapProductWishlists.Any(mp => mp.ProductId == productId);
        }


        private static BaseResponseResult<bool> CreateErrorResponse(string message)
        {
            return new BaseResponseResult<bool>
            {
                Success = false,
                Message = message,
                Data = false
            };
        }

        private static BaseResponseResult<bool> CreateSuccessResponse(bool data, string message)
        {
            return new BaseResponseResult<bool>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
    }
}
