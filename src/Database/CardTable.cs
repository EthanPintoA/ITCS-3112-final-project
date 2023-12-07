using Microsoft.Data.Sqlite;
using Models;

namespace Database
{
    public class CardTable
    {
        public static readonly string CreateTableCommandText =
            @"
            CREATE TABLE cards (
                id INTEGER PRIMARY KEY,
                title TEXT NOT NULL,
                description TEXT NOT NULL,
                column_id INTEGER NOT NULL,
                FOREIGN KEY (column_id) REFERENCES columns (id) ON DELETE CASCADE
            );";

        public static void CreateCard(SqliteConnection connection, Card card)
        {
            var command = connection.CreateCommand();

            var columnId = ColumnTable.GetColumnId(connection, card.Column.Title);

            command.CommandText =
                @"
                INSERT INTO cards (title, description, column_id)
                VALUES ($title, $description, $column_id);
                ";
            command.Parameters.AddWithValue("$title", card.Title);
            command.Parameters.AddWithValue("$description", card.Description);
            command.Parameters.AddWithValue("$column_id", columnId);

            command.ExecuteNonQuery();
        }

        public static Card? GetCard(SqliteConnection connection, int id)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                SELECT cards.title, description, columns.title
                FROM cards
                JOIN columns ON cards.column_id = columns.id
                WHERE cards.id = $id;
                ";
            command.Parameters.AddWithValue("$id", id);

            var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            var title = reader.GetString(0);
            var description = reader.GetString(1);
            var column = new Column(reader.GetString(2));

            return new Card(id, title, description, column);
        }

        public static List<Card> QueryCards(SqliteConnection connection, string text)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                SELECT cards.id, cards.title, description, columns.title
                FROM cards
                JOIN columns ON cards.column_id = columns.id
                WHERE cards.title LIKE $text
                OR cards.description LIKE $text
                OR columns.title LIKE $text;
                ";
            command.Parameters.AddWithValue("$text", $"%{text}%");

            var reader = command.ExecuteReader();

            var cards = new List<Card>();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var title = reader.GetString(1);
                var description = reader.GetString(2);
                var column = new Column(reader.GetString(3));

                cards.Add(new(id, title, description, column));
            }

            return cards;
        }

        public static void UpdateCard(SqliteConnection connection, Card card)
        {
            var command = connection.CreateCommand();

            var columnId = ColumnTable.GetColumnId(connection, card.Column.Title);

            command.CommandText =
                @"
                UPDATE cards
                SET title = $title,
                    description = $description,
                    column_id = $column_id
                WHERE id = $card_id;
                ";
            command.Parameters.AddWithValue("$title", card.Title);
            command.Parameters.AddWithValue("$description", card.Description);
            command.Parameters.AddWithValue("$column_id", columnId);
            command.Parameters.AddWithValue("$card_id", card.Id);

            command.ExecuteNonQuery();
        }

        public static void DeleteCard(SqliteConnection connection, int id)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                DELETE FROM cards
                WHERE id = $id;
                ";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }
    }
}
