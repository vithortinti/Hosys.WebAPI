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

        public async Task<Result> DeleteFile(Guid userId, Guid fileId)
        {
            if (userId == Guid.Empty)
                return Result.Fail("The user id is required or the Guid isn't valid.");
            if (fileId == Guid.Empty)
                return Result.Fail("The file id is required or the Guid isn't valid.");

            // First, check if the fileId belongs to the user
            Result<FileHistory> fileHistory = 
                await fileHistoryRepository.GetById(fileId);
            if (fileHistory.IsFailed)
                return Result.Fail(fileHistory.Errors);
            if (fileHistory.Value.UserId != userId)
                return Result.Fail("File not found.");

            // Second, delete the file history data from the database
            Result deleteResult = await fileHistoryRepository.Delete(fileId);
            if (deleteResult.IsFailed)
                return Result.Fail(deleteResult.Errors);
            
            // Finally, delete the physical file
            File.Delete(fileHistory.Value.FilePath!);

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

        public async Task<Result<FileOutput>> GetFileStream(Guid userId, Guid fileId)
        {
            // Validate the input
            if (userId == Guid.Empty)
                return Result.Fail("The user id is required or the Guid isn't valid.");
            if (fileId == Guid.Empty)
                return Result.Fail("The file id is required or the Guid isn't valid.");

            Result<FileHistory> fileHistory = 
                await fileHistoryRepository.GetById(fileId);
            if (fileHistory.IsFailed)
                return Result.Fail(fileHistory.Errors);

            // Check if the file belongs to the user
            if (fileHistory.Value.UserId != userId)
                return Result.Fail("File not found.");

            return Result.Ok(new FileOutput
            {
                Name = fileHistory.Value.FileName!,
                ContentType = fileHistory.Value.ContentType!,
                Extension = fileHistory.Value.FileExtension!,
                Path = fileHistory.Value.FilePath!,
                FileStream = new FileStream(fileHistory.Value.FilePath!, FileMode.Open, FileAccess.Read)
            });
        }

        public async Task<Result> UpdateFileName(Guid userId, Guid fileId, UpdateFileHistoryDTO updateFileHistoryDTO)
        {
            // Validate the input
            if (userId == Guid.Empty)
                return Result.Fail("The user id is required or the Guid isn't valid.");
            if (fileId == Guid.Empty)
                return Result.Fail("The file id is required or the Guid isn't valid.");
            if (updateFileHistoryDTO == null)
                return Result.Fail("The update file history is required.");

            // Check if the file belongs to the user
            Result<FileHistory> file = 
                await fileHistoryRepository.GetById(fileId);
            if (file.IsFailed)
                return Result.Fail(file.Errors);
            if (file.Value.UserId != userId) 
                return Result.Fail("File not found.");

            // Update the file history
            var fileHistory = file.Value;
            fileHistory.FileName = updateFileHistoryDTO.FileName;
            Result updateResult = await fileHistoryRepository.Update(fileHistory);
            if (updateResult.IsFailed)
                return Result.Fail(updateResult.Errors);

            return Result.Ok();
        }
    }
}