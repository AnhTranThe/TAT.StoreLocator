using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.WishList;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IWishlistService
    {
        /// <summary>
        /// Get Status whislist of product
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true if not null</returns>
        Task<BaseResponseResult<bool>> GetStatusProduct(WishListRequestProduct request);
        Task<BaseResponseResult<bool>> ChangeStatusProduct(WishListRequestProduct request, bool status);
        /// <summary>
        /// Get Status Whislist Store
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true if not null</returns>
        Task<BaseResponseResult<bool>> GetStatusStore(WishListRequestStore request);
        Task<BaseResponseResult<bool>> ChangeStatusStore(WishListRequestStore request, bool status);

    }
}
