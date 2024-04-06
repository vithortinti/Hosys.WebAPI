using System.IO.Compression;
using System.Reflection;
using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Services.Files.Pdf;
using Microsoft.AspNetCore.Http;

namespace Hosys.Application.UseCases
{
    public class PdfUseCases(PdfService pdfService) : IPdfUseCases
    {
        private readonly PdfService _pdfService = pdfService;

        public async Task<Result<FileOutput>> ConvertToImage(IFormFile formFile)
        {
            // Check the file size
            if (formFile.Length > 50000000)
                return Result.Fail("The file is too large. The maximum size is 50MB.");
            else if (!formFile.FileName.EndsWith(".pdf"))
                return Result.Fail("The file must be a PDF.");

            // Open the file stream
            var pdfStream = formFile.OpenReadStream();

            // Select the local path
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            // Create a temporary folder
            string filesFolder = Path.Combine(path, "externals", "pdf_convert", "images", Guid.NewGuid().ToString().Replace("-", ""));
            Directory.CreateDirectory(filesFolder);

            // Save the pdf file
            string file = SaveFile(pdfStream, filesFolder);

            // Create the python command
            Result convertResult = await _pdfService.ConvertoToImage(file, filesFolder);
            if (convertResult.IsFailed)
                return Result.Fail(convertResult.Errors[0].Message);

            // Delete the pdf file
            File.Delete(file);

            // Create the zip file
            string zipFile = $"{filesFolder}.zip";
            ZipFile.CreateFromDirectory(filesFolder, zipFile);
            FileStream zipStream = File.OpenRead(zipFile);

            // Delete the images folder
            Directory.Delete(filesFolder, true);
            
            return Result.Ok(new FileOutput
            {
                Name = Path.GetFileNameWithoutExtension(zipFile),
                Path = zipFile,
                Extension = ".zip",
                FileStream = zipStream,
                ContentType = "application/zip"
            });
        }

        private string SaveFile(Stream pdfStream, string path)
        {
            string filePath = Path.Combine(path, $"{Guid.NewGuid().ToString().Replace("-", "")}.pdf");
            using var file = File.Create(filePath, (int)pdfStream.Length);
            byte[] bytes = new byte[pdfStream.Length];
            pdfStream.Read(bytes, 0, (int)pdfStream.Length);
            file.Write(bytes, 0, (int)pdfStream.Length);
            file.Close();

            return filePath;
        }
    }
}