namespace TAT.StoreLocator.Core.Common
{
    /// <summary>
    /// BaseResponse
    /// </summary>
    public class BaseResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

    }
}
