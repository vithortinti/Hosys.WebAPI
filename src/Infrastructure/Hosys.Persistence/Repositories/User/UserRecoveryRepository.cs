using System.Data.Common;
using System.Data.SqlClient;
using FluentResults;
using Hosys.Domain.Interfaces.User;
using Hosys.Domain.Models.User;
using MySql.Data.MySqlClient;

namespace Hosys.Persistence.Repositories.User
{
    public class UserRecoveryRepository(Database database) : IUserRecoveryRepository
    {
        private readonly Database _database = database;

        public async Task<Result<UserRecovery>> Get(Guid id)
        {
            try
            {
                string sql = @"SELECT `USER_ID`
                    `RECOVERY_KEY`,
                    `CHANGE_PASSWORD_CODE`,
                    `CHANGE_PASSWORD_CODE_EXPIRATION`
                    FROM `USER_RECOVERY`
                    WHERE `USER_ID` = @ID";

                DbDataReader reader = await _database.ExecuteReaderAsync(sql, new MySqlParameter("@ID", id));
                if (!reader.HasRows)
                    return Result.Fail<UserRecovery>("The user recovery was not found.");
                reader.Read();

                return Result.Ok(new UserRecovery
                {
                    UserId = reader.GetGuid(0),
                    RecoveryKey = reader.GetString(1),
                    ChangePasswordCode = reader.GetString(2),
                    ChangePasswordCodeExpiration = reader.GetDateTime(3)
                });
            }
            catch
            {
                return Result.Fail<UserRecovery>("An error occurred while trying to get the user recovery.");
            }
        }

        public Task<Result> Update(UserRecovery user)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}