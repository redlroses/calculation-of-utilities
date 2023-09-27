using Microsoft.Data.Sqlite;

namespace СalculationOfUtilities.Interpreters
{
    public interface ISqliteDataInterpreter
    {
        public bool Interpreter(SqliteDataReader reader);
    }
}