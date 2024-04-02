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
        private readonly IFileHistoryRepository _fileHistoryRepository = fileHistoryRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Create(CreateFileHistoryDTO fileHistory, Guid userId)
        {
            // Validate the input
            if (fileHistory == null)
                return Result.Fail(new Error("The file history is required."));
            else if (userId == Guid.Empty)
                return Result.Fail(new Error("The user ID is required."));

            // Create the file history
            var result = await _fileHistoryRepository.Create(_mapper.Map<FileHistory>(fileHistory), userId);
            if (result.IsFailed)
                return Result.Fail(result.Errors[0]);

            return Result.Ok();
        }
    }
}