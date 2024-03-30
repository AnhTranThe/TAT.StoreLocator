using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Response.Role
{
    public class RoleResponseModel
    {
        [Required]
        public string? RoleId { get; set; }
        [Required]
        public string? RoleName { get; set; }
    }
}
