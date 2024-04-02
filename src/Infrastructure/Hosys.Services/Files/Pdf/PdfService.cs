using System.Diagnostics;
using System.Reflection;

namespace Hosys.Services.Files.Pdf
{
    public class PdfService
    {
        private readonly string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        public async Task ConvertoToImage(string pdfFile, string directory)
        {
            await Task.Run(() =>
            {
                string command = $"python {Path.Combine(_path, "processes", "python", "pdf_converter.py")} {pdfFile} {directory} image";
                ExecuteCommand(command);
            });
        }

        private string ExecuteCommand(string command)
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