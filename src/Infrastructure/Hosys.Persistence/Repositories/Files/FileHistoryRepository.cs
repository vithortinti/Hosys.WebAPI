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

        public async Task<Result> Delete(Guid id)
        {
            try
            {
                string sql = "DELETE FROM `FILE_HISTORY` WHERE `ID` = @Id;";
                MySqlParameter parameter = new("@Id", id);

                _ = await _database.ExecuteCommandAsync(sql, parameter);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occured while deleting the file history."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result> DeleteAllFromUser(Guid userId)
        {
            try
            {
                string sql = "DELETE FROM `FILE_HISTORY` WHERE `USER_ID` = @UserId;";
                MySqlParameter parameter = new("@UserId", userId);

                _ = await _database.ExecuteCommandAsync(sql, parameter);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occured while deleting the file history."),
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

        public async Task<Result<List<FileHistory>>> GetByUserId(Guid id)
        {
            try
            {
                string sql = "SELECT * FROM `FILE_HISTORY` WHERE `USER_ID` = @UserId;";
                MySqlParameter parameter = new("@UserId", id);

                var reader = await _database.ExecuteReaderAsync(sql, parameter);
                if (!reader.HasRows)
                    return Result.Fail<List<FileHistory>>("No files found for the user.");

                List<FileHistory> files = new();
                while (reader.Read())
                {
                    files.Add(new FileHistory
                    {
                        Id = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        FileName = reader.GetString(2),
                        FileExtension = reader.GetString(3),
                        ContentType = reader.GetString(4),
                        FilePath = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    });
                }

                return Result.Ok(files);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occured while deleting the file history."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result<List<FileHistory>>> GetByUserId(Guid id, int skip = 0, int take = 5)
        {
            try
            {
                string sql = "SELECT * FROM `FILE_HISTORY` WHERE `USER_ID` = @UserId LIMIT @Skip, @Take;";

                MySqlParameter[] parameters = [
                    new("@UserId", id),
                    new("@Skip", skip),
                    new("@Take", take)
                ];

                var reader = await _database.ExecuteReaderAsync(sql, parameters);
                if (!reader.HasRows)
                    return Result.Fail<List<FileHistory>>("No files found for the user.");

                List<FileHistory> files = new();
                while (reader.Read())
                {
                    files.Add(new FileHistory
                    {
                        Id = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        FileName = reader.GetString(2),
                        FileExtension = reader.GetString(3),
                        ContentType = reader.GetString(4),
                        FilePath = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    });
                }

                return Result.Ok(files);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occured while deleting the file history."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }
    }
}