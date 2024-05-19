using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAT.StoreLocator.Core.Models.Request.Product
{
    public class PhotoProductRequestModel
    {
        public IFormFile? FileUpload { get; set; }
        public bool IsThumbnail { get; set; } = false;
    }
}
