using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Common
{
    public class BaseRequest
    {
        [Required]
        public string? RequestId { get; set; }

    }
}
