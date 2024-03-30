namespace TAT.StoreLocator.Core.Models.Pagination
{
    public class PaginationRequestModel
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }


        public PaginationRequestModel()
        {
            PageSize = 10;
            PageIndex = 1;
        }


        public PaginationRequestModel(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string? SearchTerm { get; set; } = "";


    }
}
