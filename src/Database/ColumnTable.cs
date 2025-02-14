using Microsoft.Data.Sqlite;
using Models;

namespace Database
{
    static class ColumnTable
    {
        /// <summary>
        /// The SQL command text to create the columns table.
        /// </summary>
        public static readonly string CreateTableCommandText =
            @"
            CREATE TABLE columns (
                id INTEGER,
                title TEXT NOT NULL,
                PRIMARY KEY (id),
                UNIQUE (title)
            );
            CREATE INDEX title_index ON columns (title);
            ";

        /// <summary>
        /// The SQL command text to insert the default columns.
        /// </summary>
        public static readonly string InsertDefaultColumnsCommandText =
            @"
            INSERT INTO columns (title)
            VALUES ('To Do'),
            ('In Progress'),
            ('Done');
            ";

        /// <summary>
        /// Create a column in the database.
        /// </summary>
        public static void CreateColumn(SqliteConnection connection, Column column)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                INSERT INTO columns (title)
                VALUES ($title)
                ";
            command.Parameters.AddWithValue("$title", column.Title);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Returns the id of the column with the given title, or null if no such column exists.
        /// </summary>
        public static int? GetColumnId(SqliteConnection connection, string title)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                SELECT id
                FROM columns
                WHERE title = $title;
                ";
            command.Parameters.AddWithValue("$title", title);

            var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            var id = reader.GetInt32(0);

            return id;
        }

        /// <summary>
        /// Rename a column in the database.
        /// </summary>
        public static void RenameColumn(
            SqliteConnection connection,
            string oldTitle,
            string newTitle
        )
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                UPDATE columns
                SET title = $newTitle
                WHERE title = $oldTitle;
                ";
            command.Parameters.AddWithValue("$newTitle", newTitle);
            command.Parameters.AddWithValue("$oldTitle", oldTitle);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Delete a column from the database.
        /// </summary>
        public static void DeleteColumn(SqliteConnection connection, string title)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                DELETE FROM columns
                WHERE title = $title;
                ";
            command.Parameters.AddWithValue("$title", title);

            command.ExecuteNonQuery();
        }
    }
}
