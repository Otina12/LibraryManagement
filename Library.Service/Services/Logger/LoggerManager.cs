using Library.Model.Abstractions;
using NLog;

namespace Library.Service.Services.Logger;

public class LoggerManager : ILoggerManager
{
    private static ILogger logger = LogManager.GetCurrentClassLogger();

    public LoggerManager()
    {
    }

    public void LogInfo(string message) => logger.Info(message);
    public void LogWarning(string message) => logger.Warn(message);
    public void LogDebug(string message) => logger.Debug(message);
    public void LogError(string message, Error error) => logger.Error(message, error);
}
