using Hosys.Logger.Markers.Configuration;

namespace Hosys.Logger.Markers;

public class FileLogWriter
{
    private static readonly object _fileLock = new();
    private readonly string _file;

    public FileLogWriter(string file)
    {
        _file = file;
        if (!Directory.Exists(Path.GetDirectoryName(file)))
            Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        if (!File.Exists(file))
            File.Create(file).Close();
    }

    public void WriteError(Type type, string message)
    {
        Write(type, $"[{Marker.ERROR}] {message}");
    }

    public void WriteInformation(Type type, string message)
    {
        Write(type, $"[{Marker.INFORMATION}] {message}");
    }

    public void WriteWarning(Type type, string message)
    {
        Write(type, $"[{Marker.WARNING}] {message}");
    }

    public void Write(Type type, string message)
    {
        lock (_fileLock)
        {
            if (!File.Exists(_file))
                File.Create(_file).Close();
                
            using StreamWriter writer = new(_file, true);
            writer.WriteLine($"[{type.FullName}] [{DateTime.Now.ToString(Marker.DATE_FORMAT)}] {message}");
        }
    }
}
