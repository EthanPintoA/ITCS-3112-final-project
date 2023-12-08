using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Database
{
    public class KanbanDatabase
    {
        /// <summary>
        /// Apply the schema to the database.
        /// </summary>
        public static void ApplySchema(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = ColumnTable.CreateTableCommandText;
            command.CommandText += ColumnTable.InsertDefaultColumnsCommandText;

            command.CommandText += CardTable.CreateTableCommandText;
            command.CommandText += PriorityCardTable.CreateTableCommandText;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Get the path to the database file.
        /// </summary>
        /// <param name="dirPath">The directory to search for the database file.</param>
        /// <returns>The path to the database file, or null if it does not exist.</returns>
        public static string? GetDatabasePath(string dirPath)
        {
            Matcher matcher = new();
            matcher.AddInclude("*.kanban.db");
            var matches = matcher.GetResultsInFullPath(dirPath);
            return matches.Any() ? matches.First() : null;
        }
    }
}
