using Hosys.Logger.Builders;

namespace Hosys.Logger;

public interface IAppLogger<T>
{
    void LogError(string message);
    void LogError(string message, Exception exception);
    void LogInformation(string message);
    void LogWarning(string message);
    void LogWarning(string message, IList<string> errors);
}

public class AppLogger<T>(LogConfiguration handler) : LoggerBuilder<T>(handler), IAppLogger<T>
    where T : class
{
    public void LogError(string message)
    {
        Log(LogType.Error, message);
    }

    public void LogError(string message, Exception exception)
    {
        Log(LogType.Error, $"{message}\n{exception.StackTrace}");
    }

    public void LogInformation(string message)
    {
        Log(LogType.Information, message);
    }

    public void LogWarning(string message)
    {
        Log(LogType.Warning, message);
    }

    public void LogWarning(string message, IList<string> errors)
    {
        Log(LogType.Warning, $"{message}\n\t{string.Join("\n\t", errors)}");
    }
}
