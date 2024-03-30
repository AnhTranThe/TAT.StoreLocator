using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Response.Product
{
    public class ProductResponseModel
    {
        [Required]
        public string? Id { get; set; }
    }
}
