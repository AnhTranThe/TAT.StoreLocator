namespace TAT.StoreLocator.Core.Interface.ILogger
{
    public interface ILoggerService
    {
        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="exception"></param>
        void LogError(Exception ex);

        /// <summary>
        /// LogInfo
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(string message);
    }
}