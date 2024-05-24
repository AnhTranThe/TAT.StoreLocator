using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAT.StoreLocator.Core.Models.Request.WishList
{
    public class WishListRequestStore
    {
        public string? UserId { get; set; }
        public string? StoreId { get; set; }
    }
}