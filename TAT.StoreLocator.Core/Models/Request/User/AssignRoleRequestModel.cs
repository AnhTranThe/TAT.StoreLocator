using System.ComponentModel.DataAnnotations;
using TAT.StoreLocator.Core.Common;

namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class AssignRoleRequestModel
    {
        [Required]
        public string? UserId { get; set; }

        public List<BaseSelectItem> Roles { get; set; } = new List<BaseSelectItem>();
    }
}