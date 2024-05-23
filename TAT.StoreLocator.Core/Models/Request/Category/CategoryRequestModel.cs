namespace TAT.StoreLocator.Core.Models.Request.Category
{
    public class CategoryRequestModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ParentCategoryId { get; set; } = string.Empty;
    }
}