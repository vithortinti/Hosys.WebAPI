using System.Reflection;
using FluentResults;

namespace Hosys.Services.Files.Pdf
{
    public class PdfService
    {
        private readonly string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        public async Task<Result> ConvertoToImage(string pdfFile, string directory)
        {
            string output = string.Empty;
            await Task.Run(() =>
            {
                string command = $"python {Path.Combine(_path, "processes", "python", "pdf_converter.py")} {pdfFile} {directory} image";
                output = ProcessExecution.ExecuteShellCommand(command);
            });

            if (output.Contains("Error") || output.Contains("Traceback"))
                return Result.Fail([
                    new Error("An error occurred while converting the PDF file to images."),
                    new Error(output)
                ]);
            
            return Result.Ok();
        }
    }
}