namespace TAT.StoreLocator.Core.Models.Pagination
{
    public class PaginationResponseModel<T>
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount { get; set; }

        public string SearchTerm { get; set; } = "";


        public IEnumerable<T>? Data { get; set; }

        public PaginationResponseModel(IEnumerable<T> data, int totalCount, int pageSize, int pageIndex)
        {
            Data = data;
            TotalCount = totalCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

    }
}
