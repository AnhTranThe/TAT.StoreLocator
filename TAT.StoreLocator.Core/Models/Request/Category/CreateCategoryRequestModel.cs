using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Category
{
    public class CreateCategoryRequestModel
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ParentCategoryId { get; set; }



    }
}
