using Hosys.Logger.Markers.Configuration;

namespace Hosys.Logger.Markers;

static class ConsoleLogWriter
{
    private static ConsoleColor _defaultColor = Console.ForegroundColor;

    public static void WriteLineError(Type classType, string message)
    {
        WriteData();
        Console.Write($"[{classType.FullName}] ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"[{Marker.ERROR}] ");
        Console.ForegroundColor = _defaultColor;
        Console.WriteLine(message);
    }

    public static void WriteLineInformation(Type classType, string message)
    {
        WriteData();
        Console.Write($"[{classType.FullName}] ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"[{Marker.INFORMATION}] ");
        Console.ForegroundColor = _defaultColor;
        Console.WriteLine(message);
    }

    public static void WriteLineWarning(Type classType, string message)
    {
        WriteData();
        Console.Write($"[{classType.FullName}] ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"[{Marker.WARNING}] ");
        Console.ForegroundColor = _defaultColor;
        Console.WriteLine(message);
    }

    private static void WriteData()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write($"[{DateTime.Now.ToString(Marker.DATE_FORMAT)}] ");
        Console.ForegroundColor = _defaultColor;
    }
}
