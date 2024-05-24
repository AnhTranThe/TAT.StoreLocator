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

        public async Task<BaseResponseResult<bool>> ChangeStatus(WishListRequestProduct request, bool status)
        {
         
            try
            {
                if (request.UserId == null || request.ProductId == null ){

                    return CreateErrorResponse("Request is null");
                }
                Wishlist? wishlist = await GetOrCreateWishlistAsync(request.UserId, status);

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

        public async Task<BaseResponseResult<bool>> GetStatus(WishListRequestProduct request)
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

        private async Task<Wishlist?> GetWishlistAsync(string userId)
        {
            return await _dbContext.Wishlist
                .Include(w => w.MapProductWishlists)
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
            return wishlist;
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
