﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAT.StoreLocator.Core.Models.Request.Category;
using TAT.StoreLocator.Core.Models.Request.Photo;
using TAT.StoreLocator.Core.Models.Request.Store;

namespace TAT.StoreLocator.Core.Models.Request.Product
{
    public class ProductRequestModel
    {
            public string? ProductId { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Content { get; set; }
            public string? Note { get; set; }
            public string? Slug { get; set; }

            public decimal? Price { get; set; }
            public decimal? Discount { get; set; } // in percent
            public string? MetaTitle { get; set; }
            public string? MetaDescription { get; set; }
            public int? Quantity { get; set; }
            public double? Rating { get; set; }
            public string? SKU { get; set; }
            public bool? IsActive { get; set; }
            public int? ProductViewCount { get; set; }

            // Category
            public string? CategoryId { get; set; }
            public CategoryRequestModel? Category { get; set; }

            // Store
            public string? StoreId { get; set; }

            // Upload Photo
            public PhotoProductRequestModel? UploadPhoto { get; set; }
     

    }
}