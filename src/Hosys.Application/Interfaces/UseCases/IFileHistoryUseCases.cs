using FluentResults;
using Hosys.Application.Data.Outputs.File;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IFileHistoryUseCases
    {
        Task<Result> Create(CreateFileHistoryDTO fileHistory, Guid userId);
        Task<Result<IEnumerable<ReadFileHistoryDTO>>> GetByUserId(Guid userId, int skip = 0, int take = 5);
        Task<Result<FileOutput>> GetFileStream(Guid userId, Guid fileId);
    }
}