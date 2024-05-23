using System.ComponentModel.DataAnnotations;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Request.Product
{
    public class GetProductsByStoreRequestModel
    {
        [Required]
        public string? StoreId { get; set; }

        public BasePaginationRequest PaginationRequest { get; set; } = new BasePaginationRequest();
    }
}