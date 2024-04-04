using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Hosys.Application.Ports;
using Microsoft.AspNetCore.Http;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IFileUseCases
    {
        Task<Result<FileOutput>> CorruptFile(IFormFile file);
    }
}