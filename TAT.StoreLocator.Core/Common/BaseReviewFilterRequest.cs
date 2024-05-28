namespace TAT.StoreLocator.Core.Common
{
    public class BaseReviewFilterRequest
    {
        public string TypeId { get; set; } = string.Empty;
        public int? SearchRatingKey { get; set; }
    }
}