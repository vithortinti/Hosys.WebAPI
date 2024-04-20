using FluentResults;
using Hosys.Domain.Models.Files;

namespace Hosys.Domain.Interfaces.Files
{
    public interface IFileHistoryRepository
    {
        Task<Result> Create(FileHistory fileHistory, Guid userId);
        Task<Result<FileHistory>> GetById(Guid id);
        Task<Result<List<FileHistory>>> GetByUserId(Guid id);
        Task<Result<List<FileHistory>>> GetByUserId(Guid id, int skip = 0, int take = 5);
        Task<Result> Delete(Guid id);
        Task<Result> DeleteAllFromUser(Guid userId);
        Task<Result> Update(FileHistory fileHistory);
    }
}