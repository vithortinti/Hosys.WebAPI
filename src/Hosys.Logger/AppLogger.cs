using Hosys.Identity.Interfaces;
using Hosys.Logger.Builders;

namespace Hosys.Logger;

public interface IAppLogger<T>
{
    void LogError(string message);
    void LogError(string message, Exception exception);
    Task LogError(string message, Guid userId);
    Task LogError(string message, Exception exception, Guid userId);
    void LogInformation(string message);
    Task LogInformation(string message, Guid userId);
    void LogWarning(string message);
    void LogWarning(string message, IList<string> errors);
    Task LogWarning(string message, Guid userId);
    Task LogWarning(string message, IList<string> errors, Guid userId);
}

public class AppLogger<T>(LogConfiguration handler, IIdentityManager identityManager) : LoggerBuilder<T>(handler), IAppLogger<T>
    where T : class
{
    public async Task LogError(string message, Guid userId)
    {
        await LogUser(LogType.Error, message, userId);
    }

    public async Task LogError(string message, Exception exception, Guid userId)
    {
        await LogUser(LogType.Error, message + $"\nException message: {exception.Message}\n{exception.StackTrace}", userId);
    }

    public void LogError(string message)
    {
        Log(LogType.Error, message);
    }

    public void LogError(string message, Exception exception)
    {
        Log(LogType.Error, message + $"\nException message: {exception.Message}\n{exception.StackTrace}");
    }

    public async Task LogInformation(string message, Guid userId)
    {
        await LogUser(LogType.Information, message, userId);
    }

    public void LogInformation(string message)
    {
        Log(LogType.Information, message);
    }

    public async Task LogWarning(string message, Guid userId)
    {
        await LogUser(LogType.Warning, message, userId);
    }

    public async Task LogWarning(string message, IList<string> errors, Guid userId)
    {
        await LogUser(LogType.Warning, $"{message}\n\t{string.Join("\n\t", errors)}", userId);
    }

    public void LogWarning(string message)
    {
        Log(LogType.Warning, message);
    }

    public void LogWarning(string message, IList<string> errors)
    {
        Log(LogType.Warning, $"{message}\n\t{string.Join("\n\t", errors)}");
    }

    private async Task LogUser(LogType logType, string message, Guid userId)
    {
        var user = await identityManager.GetUserById(userId);
        if (user.IsFailed)
            Log(logType, message);
        else
            Log(logType, message + $"\n\tRequester: {user.Value.NickName}");
    }
}
