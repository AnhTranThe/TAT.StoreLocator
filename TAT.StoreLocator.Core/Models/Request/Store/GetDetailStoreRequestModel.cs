using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Store
{
    public class GetDetailStoreRequestModel
    {
        [Required]
        public string? Id { get; set; }
    }
}