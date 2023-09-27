using Microsoft.Data.Sqlite;
using СalculationOfUtilities.Interpreters;

namespace СalculationOfUtilities.Applications
{
    public abstract class ApplicationContext
    {
        private const string ConnectionStringFormat = "Data Source={0}.db";
        private const string SelectExpression = "SELECT * FROM {0}";
        private const string InsertExpression = "INSERT INTO {0} ({1}) VALUES ({2})";
        private const string DeleteExpression = "DELETE FROM {0} WHERE {1}=@{2}";

        private readonly string _connectionString;

        protected ApplicationContext(string databaseName)
        {
            _connectionString = string.Format(ConnectionStringFormat, databaseName);
        }

        protected bool TryRead<T>(string tableName, out T interpreter) where T : ISqliteDataInterpreter, new()
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string expression = string.Format(SelectExpression, tableName);
            SqliteCommand command = new SqliteCommand(expression, connection);
            using SqliteDataReader reader = command.ExecuteReader();

            interpreter = new T();
            return interpreter.Interpreter(reader);
        }

        protected void Add(string tableName, string names, string values, params SqliteParameter[] parameters)
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string expression = string.Format(InsertExpression, tableName, names, values);
            SqliteCommand command = new SqliteCommand(expression, connection);

            foreach (SqliteParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            command.ExecuteNonQuery();
        }

        protected void Delete(string tableName, string name, SqliteParameter parameter)
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string expression = string.Format(DeleteExpression, tableName, name, name);
            SqliteCommand command = new SqliteCommand(expression, connection);

            command.Parameters.Add(parameter);
            command.ExecuteNonQuery();
        }
    }
}
