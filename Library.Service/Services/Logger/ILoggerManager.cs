using Library.Model.Abstractions;

namespace Library.Service.Services.Logger;

public interface ILoggerManager
{
    void LogInfo(string message);
    void LogInfo(string message, params object[] args);
    void LogWarning(string message);
    void LogDebug(string message);
    void LogError(string message);
}
