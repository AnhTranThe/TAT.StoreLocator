using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class UpdateStoreResponseModel
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public StoreResponseModel StoreResponseModel { get; set; }  = new StoreResponseModel();
    }
}
