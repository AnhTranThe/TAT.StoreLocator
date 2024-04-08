using log4net;
using TAT.StoreLocator.Core.Interface.ILogger;

namespace TAT.StoreLocator.Infrastructure.Services
{
    public class LoggerService : ILogger
    {
        private readonly ILog _logger;
        /// <summary>
        /// LoggingService
        /// </summary>
        public LoggerService()
        {
            _logger = LogManager.GetLogger(typeof(LoggerService));
        }
        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="exception"></param>
        public void LogError(string message)
        {
            _logger.Error(message);
        }
        /// <summary>
        /// LogInfo
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
    }
}
