namespace TAT.StoreLocator.Core.Common
{
    public class BasePaginationRequest
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public BasePaginationRequest()
        {
            PageSize = 10;
            PageIndex = 1;
        }


        public BasePaginationRequest(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }




    }
}
