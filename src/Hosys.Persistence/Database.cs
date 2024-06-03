using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Hosys.Persistence
{
    public class Database
    {
        private readonly MySqlConnection _connection;

        public Database(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public async Task<int> ExecuteCommandAsync(string command, params MySqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                var sqlCommand = new MySqlCommand(command, _connection);
                if (parameters?.Length > 0)
                    sqlCommand.Parameters.AddRange(parameters);
                return await sqlCommand.ExecuteNonQueryAsync();
            }
            catch
            {
                _connection.Close();
                throw;
            }
        }

        public async Task<DbDataReader> ExecuteReaderAsync(string query, params MySqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
                var sqlCommand = new MySqlCommand(query, _connection);
                if (parameters?.Length > 0)
                    sqlCommand.Parameters.AddRange(parameters);

                return await sqlCommand.ExecuteReaderAsync();
            }
            catch
            {
                _connection.Close();
                throw;
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
    }
}