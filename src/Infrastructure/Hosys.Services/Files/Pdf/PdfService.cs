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
                ProcessExecution.ExecuteShellCommand(command);
            });
        }
    }
}