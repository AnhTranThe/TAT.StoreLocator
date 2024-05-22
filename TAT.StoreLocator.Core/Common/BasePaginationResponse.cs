namespace TAT.StoreLocator.Core.Common
{
    public class BasePaginationResponse
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount { get; set; }

        public string SearchString { get; set; } = string.Empty;

    }

}
