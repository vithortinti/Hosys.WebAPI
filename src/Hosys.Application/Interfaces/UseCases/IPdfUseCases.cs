using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Microsoft.AspNetCore.Http;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IPdfUseCases
    {
        Task<Result<FileOutput>> ConvertToImage(IFormFile pdfStream);
    }
}