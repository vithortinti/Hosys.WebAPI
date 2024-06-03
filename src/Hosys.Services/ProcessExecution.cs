using System.Diagnostics;

namespace Hosys.Services
{
    public static class ProcessExecution
    {
        public static string ExecuteShellCommand(string command)
        {
            string[] commands = command.Split(" ");

            using Process process = new();
            process.StartInfo.FileName = commands[0];
            process.StartInfo.Arguments = string.Join(" ", commands.Skip(1));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}