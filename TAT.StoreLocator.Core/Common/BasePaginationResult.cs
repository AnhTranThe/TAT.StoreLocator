namespace TAT.StoreLocator.Core.Common
{
    public class BasePaginationResult<T> : BasePaginationResponse
    {
        public List<T> Data { get; set; } = new List<T>();
    }
}
