using FluentResults;
using Hosys.Domain.Interfaces.Files;
using Hosys.Domain.Models.Files;
using MySql.Data.MySqlClient;

namespace Hosys.Persistence.Repositories.Files
{
    public class FileHistoryRepository(Database database) : IFileHistoryRepository
    {
        private readonly Database _database = database;

        public async Task<Result> Create(FileHistory fileHistory, Guid userId)
        {
            try
            {
                string sql = @"INSERT INTO `FILE_HISTORY`
                    (`ID`, `USER_ID`, `FILE_NAME`, `FILE_EXTENSION`, `CONTENT_TYPE`, `FILE_PATH`, `CREATED_AT`)
                    VALUES 
                    (@Id, @UserId, @FileName, @FileExtension, @ContentType, @FilePath, @CreatedAt);";

                MySqlParameter[] parameters = [
                    new("@Id", fileHistory.Id != Guid.Empty ? fileHistory.Id : Guid.NewGuid()),
                    new("@UserId", userId),
                    new("@FileName", fileHistory.FileName),
                    new("@FileExtension", fileHistory.FileExtension),
                    new("@ContentType", fileHistory.ContentType),
                    new("@FilePath", fileHistory.FilePath),
                    new("@CreatedAt", fileHistory.CreatedAt)
                ];

                _ = await _database.ExecuteCommandAsync(sql, parameters);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occured while creating the file history."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public Task<Result<FileHistory>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<FileHistory>>> GetByUserId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}