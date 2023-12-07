using Microsoft.Data.Sqlite;
using Models;

namespace Database
{
    public class PriorityCardTable : CardTable
    {
        public static new readonly string CreateTableCommandText =
            @"
            CREATE TABLE priority_cards (
                id INTEGER PRIMARY KEY,
                priority INTEGER NOT NULL,
                FOREIGN KEY (id) REFERENCES cards (id) ON DELETE CASCADE
            );";

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
