using FluentResults;
using Hosys.Domain.Models.Files;

namespace Hosys.Domain.Interfaces.Files
{
    public interface IFileHistoryRepository
    {
        Task<Result> Create(FileHistory fileHistory, Guid userId);
        Task<Result<FileHistory>> GetById(Guid id);
        Task<Result<List<FileHistory>>> GetByUserId(Guid id);
    }
}