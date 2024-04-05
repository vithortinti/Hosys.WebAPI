using System.Security.Cryptography;
using System.Text;
using FluentResults;
using Hosys.Domain.Interfaces.User;
using MySql.Data.MySqlClient;

namespace Hosys.Persistence.Repositories.User
{
    public class UserRepository(Database database) : IUserRepository
    {
        private readonly Database _database = database;

        public async Task<Result<Domain.Models.User.User>> Create(Domain.Models.User.User user, string password)
        {
            try
            {
                string sql = @"INSERT INTO `USER` 
                    (`ID`, `NAME`, `LAST_NAME`, `NICKNAME`, `E_MAIL`, `PASSWORD`, `ROLE`, `CREATED_AT`)
                    VALUES
                    (@ID, @NAME, @LAST_NAME, @NICKNAME, @E_MAIL, @PASSWORD, @ROLE, @CREATED_AT);";

                user.Id = user.Id != Guid.Empty ? user.Id : Guid.NewGuid();
                MySqlParameter[] parameters =
                [
                    new MySqlParameter("@ID", user.Id),
                    new MySqlParameter("@NAME", user.Name),
                    new MySqlParameter("@LAST_NAME", user.LastName),
                    new MySqlParameter("@NICKNAME", user.NickName),
                    new MySqlParameter("@E_MAIL", user.Email),
                    new MySqlParameter("@PASSWORD", password),
                    new MySqlParameter("@ROLE", user.Role),
                    new MySqlParameter("@CREATED_AT", user.CreatedAt)
                ];
                _ = await _database.ExecuteCommandAsync(sql, parameters);
                
                return Result.Ok(user);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when creating the user."),
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
                string sql = "DELETE FROM `USER` WHERE `ID` = @ID";

                int rowsAffected = await _database.ExecuteCommandAsync(sql, new MySqlParameter("@ID", id));

                if (rowsAffected == 0)
                    return Result.Fail("The user was not found.");

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when deleting the user."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result<Domain.Models.User.User>> Get(Guid id)
        {
            try
            {
                string sql = @"SELECT `ID`,
                    `PUBLIC_ID`,
                    `NAME`,
                    `LAST_NAME`,
                    `NICKNAME`,
                    `E_MAIL`,
                    `ROLE`,
                    `CREATED_AT`
                    FROM `USER` WHERE `ID` = @ID";

                using var reader = await _database.ExecuteReaderAsync(sql, new MySqlParameter("@ID", id));
                if (!reader.HasRows)
                    return Result.Fail<Domain.Models.User.User>("The user was not found.");
                reader.Read();

                return Result.Ok(new Domain.Models.User.User
                {
                    Id = reader.GetGuid(0),
                    PublicId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    LastName = reader.GetString(3),
                    NickName = reader.GetString(4),
                    Email = reader.GetString(5),
                    Role = reader.GetString(6),
                    CreatedAt = reader.GetDateTime(7)
                });
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when getting the user."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result<Domain.Models.User.User>> GetByEmail(string email)
        {
            try
            {
                string sql = @"SELECT `ID`,
                    `PUBLIC_ID`,
                    `NAME`,
                    `LAST_NAME`,
                    `NICKNAME`,
                    `E_MAIL`,
                    `ROLE`,
                    `CREATED_AT`
                    FROM `USER` WHERE `E_MAIL` = @E_MAIL";

                using var reader = await _database.ExecuteReaderAsync(sql, new MySqlParameter("@E_MAIL", email));
                if (!reader.HasRows)
                    return Result.Fail<Domain.Models.User.User>("The user was not found.");
                reader.Read();

                return Result.Ok(new Domain.Models.User.User
                {
                    Id = reader.GetGuid(0),
                    PublicId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    LastName = reader.GetString(3),
                    NickName = reader.GetString(4),
                    Email = reader.GetString(5),
                    Role = reader.GetString(6),
                    CreatedAt = reader.GetDateTime(7)
                });
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when getting the user by email."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result<Domain.Models.User.User>> GetByNickname(string nickname)
        {
            try
            {
                string sql = @"SELECT `ID`,
                    `PUBLIC_ID`,
                    `NAME`,
                    `LAST_NAME`,
                    `NICKNAME`,
                    `E_MAIL`,
                    `ROLE`,
                    `CREATED_AT`
                    FROM `USER` WHERE `NICKNAME` = @NICKNAME";

                using var reader = await _database.ExecuteReaderAsync(sql, new MySqlParameter("@NICKNAME", nickname));
                if (!reader.HasRows)
                    return Result.Fail<Domain.Models.User.User>("The user was not found.");
                reader.Read();

                return Result.Ok(new Domain.Models.User.User
                {
                    Id = reader.GetGuid(0),
                    PublicId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    LastName = reader.GetString(3),
                    NickName = reader.GetString(4),
                    Email = reader.GetString(5),
                    Role = reader.GetString(6),
                    CreatedAt = reader.GetDateTime(7)
                });
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when getting the user by email."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result> Update(Domain.Models.User.User user)
        {
            try
            {
                string sql = @"UPDATE `USER`
                    SET NAME = @NAME,
                    LAST_NAME = @LAST_NAME,
                    NICKNAME = @NICKNAME,
                    ROLE = @ROLE
                    WHERE ID = @ID";

                MySqlParameter[] parameters =
                [
                    new MySqlParameter("@ID", user.Id),
                    new MySqlParameter("@NAME", user.Name),
                    new MySqlParameter("@LAST_NAME", user.LastName),
                    new MySqlParameter("@NICKNAME", user.NickName),
                    new MySqlParameter("@ROLE", user.Role)
                ];

                int rowsAffected = await _database.ExecuteCommandAsync(sql, parameters);
                if (rowsAffected == 0)
                    return Result.Fail("The user was not found.");

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when updating the user."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result> UpdatePassword(Guid id, string newPassword)
        {
            try
            {
                string sql = "UPDATE `USER` SET PASSWORD = @PASSWORD WHERE ID = @ID";

                MySqlParameter[] parameters =
                [
                    new MySqlParameter("@ID", id),
                    new MySqlParameter("@PASSWORD", newPassword)
                ];

                int rowsAffected = await _database.ExecuteCommandAsync(sql, parameters);
                if (rowsAffected == 0)
                    return Result.Fail("The user was not found.");

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when updating the user's password."),
                    new(ex.Message)
                    });
            }
            finally
            {
                _database.CloseConnection();
            }
        }

        public async Task<Result<bool>> CheckPassword(Domain.Models.User.User user, string password)
        {
            try
            {
                string sql = "SELECT PASSWORD FROM `USER` WHERE ID = @ID";

                MySqlParameter id = new("@ID", user.Id);

                var reader = await _database.ExecuteReaderAsync(sql, id);
                if (!reader.HasRows)
                    return Result.Fail("The user was not found.");
                reader.Read();

                if (reader.GetString(0) != password)
                    return Result.Fail("The password is incorrect.");
                
                return Result.Ok(true);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error[] { 
                    new("An error occurred when checking the user's password."),
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