using FluentResults;
using Hosys.Application.Data.Outputs.File;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IFileHistoryUseCases
    {
        Task<Result> Create(CreateFileHistoryDTO fileHistory, Guid userId);
    }
}