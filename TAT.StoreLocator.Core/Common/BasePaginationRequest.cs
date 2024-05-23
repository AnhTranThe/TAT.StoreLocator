using TAT.StoreLocator.Core.Interface.IPaging;

namespace TAT.StoreLocator.Core.Common
{
    public class BasePaginationRequest : IPaginationRequest
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageIndex = 1;

        private int _pageSize = DefaultPageSize;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 ? value : DefaultPageSize;
        }

        private int _pageIndex = DefaultPageIndex;

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value > 0 ? value : DefaultPageIndex;
        }

        public string SearchString { get; set; } = string.Empty;
    }
}