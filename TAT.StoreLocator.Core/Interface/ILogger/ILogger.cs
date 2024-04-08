namespace TAT.StoreLocator.Core.Interface.ILogger
{
    public interface ILogger
    {
        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="exception"></param>
        void LogError(string message);
        /// <summary>
        /// LogInfo
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(string message);

    }
}
