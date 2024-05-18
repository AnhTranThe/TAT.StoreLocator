using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAT.StoreLocator.Core.Models.Request.Category
{
    public class CategoryRequestModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ParentCategoryId { get; set; }

    }
}
