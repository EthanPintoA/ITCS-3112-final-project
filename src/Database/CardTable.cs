using Microsoft.Data.Sqlite;
using Models;

namespace Database
{
    public class CardTable
    {
        /// <summary>
        /// The SQL command text to create the cards table.
        /// </summary>
        public static readonly string CreateTableCommandText =
            @"
            CREATE TABLE cards (
                id INTEGER PRIMARY KEY,
                title TEXT NOT NULL,
                description TEXT NOT NULL,
                column_id INTEGER NOT NULL,
                FOREIGN KEY (column_id) REFERENCES columns (id) ON DELETE CASCADE
            );";

        /// <summary>
        /// Create a card in the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="card"> The card to create. </param>
        /// <returns> The id of the card that was created. </returns>
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

        /// <summary>
        /// Get a card from the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="id"> The id of the card to get. </param>
        /// <returns> The card with the given id, or null if it does not exist. </returns>
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

        /// <summary>
        /// Get all cards from the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <returns> All cards in the database. </returns>
        public static List<Card> GetCards(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                SELECT cards.id, cards.title, description, columns.title
                FROM cards
                JOIN columns ON cards.column_id = columns.id
                ORDER BY columns.id ASC;
                ";

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

        /// <summary>
        /// Get all cards from the database that match the given text.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="text"> The text to search for. </param>
        /// <returns> All cards in the database that match the given text. </returns>
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
                OR columns.title LIKE $text
                ORDER BY columns.id ASC;
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

        /// <summary>
        /// Update a card in the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="card"> The card to update. </param>
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

        /// <summary>
        /// Delete a card from the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="id"> The id of the card to delete. </param>
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
