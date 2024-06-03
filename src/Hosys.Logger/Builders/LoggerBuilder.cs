using Hosys.Logger.Markers;

namespace Hosys.Logger.Builders;

public class LogConfiguration
{
    public LogConfiguration() {  }
    public LogType _minimumLogLevel { get; set; } = LogType.Information;
    public string MinimumLogLevel 
    {
        get => _minimumLogLevel.ToString();
        set => _minimumLogLevel = Enum.Parse<LogType>(value);
    }
    
    public string LogPath { get; set; } = "Logs";
    
    private string _logFileName = $"log{DateTime.Now:yyyyMMdd}.txt";
    public string LogFileName 
    {
        get => _logFileName;
        set
        {
            value = value.Replace("{date}", DateTime.Now.ToString("yyyyMMdd"));
            _logFileName = value;
        }
    }

    public LogType GetMinimumLogLevel() => _minimumLogLevel;
}

public delegate void LogConfigurationHandler(LogConfiguration configuration);

public enum LogType
{
    Information,
    Warning,
    Error
}

public abstract class LoggerBuilder<T>(LogConfiguration configuration) where T : class
{
    private FileLogWriter _fileLogWriter => new(Path.Combine(configuration.LogPath, configuration.LogFileName));
    
    public void Log(LogType type, string message)
    {
        switch (type)
        {
            case LogType.Information:
                if (configuration.GetMinimumLogLevel() <= LogType.Information)
                    LogInformation(message);
                break;
            case LogType.Warning:
                if (configuration.GetMinimumLogLevel() <= LogType.Warning)
                    LogWarning(message);
                break;
            case LogType.Error:
                if (configuration.GetMinimumLogLevel() <= LogType.Error)
                    LogError(message);
                break;
        }
    }

    private void LogError(string message)
    {
        ConsoleLogWriter.WriteLineError(typeof(T), message);
        _fileLogWriter.WriteError(typeof(T), message);
    }

    private void LogInformation(string message)
    {
        ConsoleLogWriter.WriteLineInformation(typeof(T), message);
        _fileLogWriter.WriteInformation(typeof(T), message);
    }

    private void LogWarning(string message)
    {
        ConsoleLogWriter.WriteLineWarning(typeof(T), message);
        _fileLogWriter.WriteWarning(typeof(T), message);
    }
}
