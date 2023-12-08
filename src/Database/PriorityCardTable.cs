using Microsoft.Data.Sqlite;
using Models;

namespace Database
{
    public class PriorityCardTable : CardTable
    {
        /// <summary>
        /// The SQL command text to create the priority_cards table.
        /// </summary>
        public static new readonly string CreateTableCommandText =
            @"
            CREATE TABLE priority_cards (
                id INTEGER PRIMARY KEY,
                priority INTEGER NOT NULL,
                FOREIGN KEY (id) REFERENCES cards (id) ON DELETE CASCADE
            );";

        /// <summary>
        /// Create a priority card in the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="card"> The priority card to create. </param>
        public static void CreateCard(SqliteConnection connection, PriorityCard card)
        {
            CreateCard(connection, (Card)card);

            var command = connection.CreateCommand();

            // Get the id of the card we just created. It's the last one in the table.
            command.CommandText =
                @"
                SELECT id
                FROM cards
                ORDER BY id DESC
                LIMIT 1;
                ";

            var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return;
            }

            var id = reader.GetInt32(0);

            reader.Close();

            command = connection.CreateCommand();

            command.CommandText =
                @"
                INSERT INTO priority_cards (id, priority)
                VALUES ($id, $priority);
                ";
            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$priority", card.Priority);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Get a priority card from the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="id"> The id of the card to get. </param>
        /// <returns> The priority card with the given id, or null if no such card exists. </returns>
        public static new PriorityCard? GetCard(SqliteConnection connection, int id)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                SELECT cards.title, description, columns.title, priority
                FROM cards
                JOIN priority_cards ON cards.id = priority_cards.id
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
            var priority = reader.GetInt32(3);

            return new PriorityCard(id, title, description, column, priority);
        }

        /// <summary>
        /// Get all priority cards from the database ordered by column.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="text"> The text to search for. </param>
        /// <param name="priority"> The priority to search for. </param>
        /// <returns> All priority cards in the database. </returns>
        public static List<PriorityCard> QueryCards(
            SqliteConnection connection,
            string text,
            int priority
        )
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                SELECT cards.id, cards.title, description, columns.title, priority
                FROM cards
                JOIN priority_cards ON cards.id = priority_cards.id
                JOIN columns ON cards.column_id = columns.id
                WHERE cards.title LIKE $text
                OR cards.description LIKE $text
                OR columns.title LIKE $text
                OR priority_cards.priority = $priority
                ORDER BY columns.id ASC;
                ";
            command.Parameters.AddWithValue("$text", $"%{text}%");
            command.Parameters.AddWithValue("$priority", priority);

            var reader = command.ExecuteReader();

            var cards = new List<PriorityCard>();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var title = reader.GetString(1);
                var description = reader.GetString(2);
                var column = new Column(reader.GetString(3));

                cards.Add(new(id, title, description, column, priority));
            }

            return cards;
        }

        /// <summary>
        /// Get all priority cards from the database ordered by column.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="priority"> The priority to search for. </param>
        /// <returns> All priority cards in the database. </returns>
        public static List<PriorityCard> QueryCards(SqliteConnection connection, int priority)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
                SELECT cards.id, cards.title, description, columns.title, priority
                FROM cards
                JOIN priority_cards ON cards.id = priority_cards.id
                JOIN columns ON cards.column_id = columns.id
                WHERE priority_cards.priority = $priority
                ORDER BY columns.id ASC;
                ";
            command.Parameters.AddWithValue("$priority", priority);

            var reader = command.ExecuteReader();

            var cards = new List<PriorityCard>();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var title = reader.GetString(1);
                var description = reader.GetString(2);
                var column = new Column(reader.GetString(3));

                cards.Add(new(id, title, description, column, priority));
            }

            return cards;
        }

        /// <summary>
        /// Update a priority card in the database.
        /// </summary>
        /// <param name="connection"> The database connection. </param>
        /// <param name="card"> The priority card to update. </param>
        public static void UpdateCard(SqliteConnection connection, PriorityCard card)
        {
            UpdateCard(connection, (Card)card);

            var command = connection.CreateCommand();

            command.CommandText =
                @"
                UPDATE priority_cards
                SET priority = $priority
                WHERE id = $card_id;
                ";
            command.Parameters.AddWithValue("$priority", card.Priority);
            command.Parameters.AddWithValue("$card_id", card.Id);

            command.ExecuteNonQuery();
        }

        // DeleteCard Override works because of the ON DELETE CASCADE clause.
    }
}
