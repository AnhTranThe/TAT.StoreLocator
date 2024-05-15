﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Response.Store
{
    public class GetDetailStoreResponseModel
    {
        //public BaseResponse BaseResponse { get; set; } = new BaseResponse();
        public StoreResponseModel StoreResponseModel { get; set; } = new StoreResponseModel();
        public MapGalleryStoreResponse MapGalleryStore { get; set; } = new MapGalleryStoreResponse();
    }
}
