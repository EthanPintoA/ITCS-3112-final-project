using Database;
using Microsoft.Data.Sqlite;
using Models;

namespace Test
{
    public class TestClass
    {
        public static void Program()
        {
            const string databaseFileName = "test";

            // "." is the current directory
            // var databasePath = KanbanDatabase.GetDatabasePath(".");

            // Reset the database so we can start fresh
            if (File.Exists($"{databaseFileName}.kanban.db"))
            {
                File.Delete($"{databaseFileName}.kanban.db");
            }

            // create a new database file
            using var connection = new SqliteConnection(
                $"Data Source={databaseFileName}.kanban.db"
            );
            connection.Open();
            KanbanDatabase.ApplySchema(connection);

            // Test CRUD on cards
            CardTable.CreateCard(connection, new("Test Card", "This is a test card", new("To Do")));
            Console.WriteLine(CardTable.GetCard(connection, 1));
            CardTable.UpdateCard(
                connection,
                new(1, "Test Update Card", "This is a test update card", new("In Progress"))
            );
            Console.WriteLine(CardTable.GetCard(connection, 1));
            CardTable.DeleteCard(connection, 1);

            // Test CRUD on priority cards
            PriorityCardTable.CreateCard(
                connection,
                new("Test Priority Card", "This is a test priority card", new("To Do"), 1)
            );
            Console.WriteLine(PriorityCardTable.GetCard(connection, 1));
            PriorityCardTable.UpdateCard(
                connection,
                new(
                    1,
                    "Test Priority Update Card",
                    "This is a test priority update card",
                    new("In Progress"),
                    2
                )
            );
            Console.WriteLine(PriorityCardTable.GetCard(connection, 1));
            PriorityCardTable.DeleteCard(connection, 1);

            // Test CRUD on columns (Without the read part)
            ColumnTable.CreateColumn(connection, new Column("Archived"));

            CardTable.CreateCard(
                connection,
                new("Test Card", "This is a test card", new("Archived"))
            );

            ColumnTable.RenameColumn(connection, "Archived", "Archive");

            ColumnTable.DeleteColumn(connection, "Archive");

            // Test Querying of cards
            CardTable.CreateCard(connection, new("Test Card", "This is a test card", new("To Do")));
            CardTable.CreateCard(
                connection,
                new("Test Card 2", "This is a test card 2", new("In Progress"))
            );
            CardTable.CreateCard(
                connection,
                new("Test Card 3", "This is a test card 3", new("In Progress"))
            );
            PriorityCardTable.CreateCard(
                connection,
                new("Test Priority Card", "This is a test priority card", new("To Do"), 1)
            );
            PriorityCardTable.CreateCard(
                connection,
                new("Test Priority Card 2", "This is a test priority card 2", new("To Do"), 2)
            );
            PriorityCardTable.CreateCard(
                connection,
                new("Test Priority Card 3", "This is a test priority card 3", new("To Do"), 3)
            );

            
            Console.WriteLine("\nCards with '3':");
            Console.WriteLine(string.Join("\n", CardTable.QueryCards(connection, "3")));
            Console.WriteLine("\nCards in 'To Do':");
            Console.WriteLine(string.Join("\n", CardTable.QueryCards(connection, "To Do")));
        }
    }
}
