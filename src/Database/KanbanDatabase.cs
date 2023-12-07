using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Database
{
    public class KanbanDatabase
    {
        public static void ApplySchema(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = ColumnTable.CreateTableCommandText;
            command.CommandText += ColumnTable.InsertDefaultColumnsCommandText;

            command.CommandText += CardTable.CreateTableCommandText;
            command.CommandText += PriorityCardTable.CreateTableCommandText;

            command.ExecuteNonQuery();
        }

        public static string? GetDatabasePath(string databaseFolderPath)
        {
            Matcher matcher = new();
            matcher.AddInclude("*.kanban.db");
            var matches = matcher.GetResultsInFullPath(databaseFolderPath);
            return matches.Any() ? matches.First() : null;
        }
    }
}
