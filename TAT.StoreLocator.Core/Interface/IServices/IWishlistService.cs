using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;
using TAT.StoreLocator.Core.Models.Request.WishList;
using TAT.StoreLocator.Core.Models.Response.Product;
using TAT.StoreLocator.Core.Models.Response.Wishlist;

namespace TAT.StoreLocator.Core.Interface.IServices
{
    public interface IWishlistService
    {
        Task<BaseResponseResult<bool>> GetStatus(WishListRequestProduct request);

        Task<BaseResponseResult<bool>> ChangeStatus (WishListRequestProduct request, bool status);
    }
}
