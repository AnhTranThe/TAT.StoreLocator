using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Category
{
    public class DeleteCategoryRequestModel
    {
        [Required]
        public string? Id { get; set; }


    }
}