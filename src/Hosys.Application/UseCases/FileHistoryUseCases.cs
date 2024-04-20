using AutoMapper;
using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Domain.Interfaces.Files;
using Hosys.Domain.Models.Files;

namespace Hosys.Application.UseCases
{
    public class FileHistoryUseCases(
        IFileHistoryRepository fileHistoryRepository,
        IMapper mapper
        ) : IFileHistoryUseCases
    {
        public async Task<Result> Create(CreateFileHistoryDTO fileHistory, Guid userId)
        {
            // Validate the input
            if (fileHistory == null)
                return Result.Fail(new Error("The file history is required."));
            else if (userId == Guid.Empty)
                return Result.Fail(new Error("The user ID is required."));

            // Create the file history
            var result = await fileHistoryRepository.Create(mapper.Map<FileHistory>(fileHistory), userId);
            if (result.IsFailed)
                return Result.Fail(result.Errors);

            return Result.Ok();
        }

        public async Task<Result<IEnumerable<ReadFileHistoryDTO>>> GetByUserId(Guid userId, int skip = 0, int take = 5)
        {
            // Vaida a entrada
            if (userId == Guid.Empty)
                return Result.Fail("The user id is required or the Guid isn't valid.");

            Result<List<FileHistory>> userFiles = 
                await fileHistoryRepository.GetByUserId(userId, skip, take);

            if (userFiles.IsFailed)
                return Result.Fail(userFiles.Errors);

            return Result.Ok(mapper.Map<IEnumerable<ReadFileHistoryDTO>>(userFiles.Value));
        }
    }
}