namespace TAT.StoreLocator.Core.Models.Request.User
{
    public class ChangeStatusUserRequestModel
    {
        public string? UserId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}