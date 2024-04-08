namespace TAT.StoreLocator.Core.Common
{
    public class BaseResponseResult<T> : BaseResponse
    {
        public T? Data { get; set; }
    }
}
