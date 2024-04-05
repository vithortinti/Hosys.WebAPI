using System.Reflection;
using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Security.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hosys.Application.UseCases
{
    public class FileUseCases(ITextSecurityAnalyzer textSecurityAnalyzer) : IFileUseCases
    {
        private readonly ITextSecurityAnalyzer _textSecurityAnalyzer = textSecurityAnalyzer;

        public async Task<Result<FileOutput>> CorruptFile(IFormFile formFile)
        {
            // Check the file size
            if (formFile.Length > 50000000)
                return Result.Fail("The file is too large. The maximum size is 50MB.");
            else if (_textSecurityAnalyzer.HasPathTraversal(formFile.FileName))
                return Result.Fail("The file name contains invalid characters.");
            
            // Open the file stream
            var fileStream = formFile.OpenReadStream();

            // Select the local path
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            // Create a temporary folder
            string filesFolder = Path.Combine(path, "externals", "corrupt");
            if (!Directory.Exists(filesFolder))
                Directory.CreateDirectory(filesFolder);

            // Corrupt the file
            var file = await CorruptAndSaveFile(fileStream, filesFolder, formFile.FileName);

            // Return the file
            return Result.Ok(new FileOutput
            {
                Name = formFile.FileName,
                Extension = Path.GetExtension(formFile.FileName),
                ContentType = formFile.ContentType,
                Path = file.Name,
                FileStream = file
            });
        }

        private async Task<FileStream> CorruptAndSaveFile(Stream fileStream, string path, string fileName)
        {
            // Create a temporary file
            FileStream result = null!;
            await Task.Run(() =>
            {
                string file = Path.Combine(path, fileName);
                using (FileStream fs = new(file, FileMode.Create))
                {
                    fileStream.CopyTo(fs);
                }

                // Corrupt the file
                byte[] fileBytes = File.ReadAllBytes(file);
                fileBytes[0] = 0x00;
                File.WriteAllBytes(file, fileBytes);
                result = new FileStream(file, FileMode.Open, FileAccess.Read);
            });

            return result;
        }
    }
}