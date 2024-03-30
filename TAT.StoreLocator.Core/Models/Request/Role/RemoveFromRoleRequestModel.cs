using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Models.Request.Role
{
    public class RemoveFromRoleRequestModel
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? RoleId { get; set; }

    }
}
