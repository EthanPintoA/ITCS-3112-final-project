using Database;
using KanbanCli.Api;
using Microsoft.Data.Sqlite;
using Models;

namespace KanbanCli.Actions
{
    public static class Actions
    {
        /// <summary>
        /// Print a message indicating that no board exists in the current directory.
        /// </summary>
        private static void PrintNoBoardMessage()
        {
            Console.WriteLine("No board exists in this directory.");
            Console.WriteLine("Create a board with the create-board command.");
        }

        /// <summary>
        /// Create a new Kanban board.
        /// </summary>
        public static void CreateBoard(CreateBoardOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                using var connection = new SqliteConnection(
                    $"Data Source={options.Name}.kanban.db"
                );
                connection.Open();

                KanbanDatabase.ApplySchema(connection);

                Console.WriteLine($"Created board: {options.Name}.kanban.db");
            }
            else
            {
                var fileName = Path.GetFileName(path);
                Console.WriteLine($"A board already exists in this directory: {fileName}");
                Console.WriteLine("Only one board is allowed per directory.");
            }
        }

        #region Column Actions
        /// <summary>
        /// Add a column to the Kanban board.
        /// </summary>
        public static void AddColumn(AddColumnOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            var columnId = ColumnTable.GetColumnId(connection, options.Name);

            if (columnId is not null)
            {
                Console.WriteLine($"A column with the name {options.Name} already exists.");
                return;
            }

            ColumnTable.CreateColumn(connection, new(options.Name));

            Console.WriteLine($"Created column {options.Name}.");
        }

        /// <summary>
        /// Rename a column on the Kanban board.
        /// </summary>
        public static void RenameColumn(RenameColumnOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            var columnId = ColumnTable.GetColumnId(connection, options.NewName);

            if (columnId is not null)
            {
                Console.WriteLine($"A column with the name {options.NewName} already exists.");
                return;
            }

            ColumnTable.RenameColumn(connection, options.OldName, options.NewName);

            Console.WriteLine($"Renamed column {options.OldName} to {options.NewName}.");
        }

        /// <summary>
        /// Delete a column from the Kanban board.
        /// </summary>
        public static void DeleteColumn(DeleteColumnOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            var columnId = ColumnTable.GetColumnId(connection, options.Name);

            if (columnId is null)
            {
                Console.WriteLine($"No column with the name {options.Name} exists.");
                return;
            }

            ColumnTable.DeleteColumn(connection, options.Name);

            Console.WriteLine($"Deleted column {options.Name}.");
        }
        #endregion

        #region Card Actions
        /// <summary>
        /// Add a card to the Kanban board.
        /// </summary>
        public static void AddCard(AddCardOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            var columnId = ColumnTable.GetColumnId(connection, options.Column);

            if (columnId is null)
            {
                Console.WriteLine($"No column with the name {options.Column} exists.");
                return;
            }

            options.Description ??= "";

            if (options.Priority is null)
            {
                CardTable.CreateCard(
                    connection,
                    new(options.Title, options.Description, new(options.Column))
                );
            }
            else
            {
                PriorityCardTable.CreateCard(
                    connection,
                    new(
                        options.Title,
                        options.Description,
                        new(options.Column),
                        options.Priority.Value
                    )
                );
            }

            Console.WriteLine($"Created card {options.Title}.");
        }

        /// <summary>
        /// Move a card on the Kanban board.
        /// </summary>
        public static void MoveCard(MoveCardOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            var card = CardTable.GetCard(connection, options.Id);

            if (card is null)
            {
                Console.WriteLine($"No card with ID {options.Id} exists.");
                return;
            }

            var columnId = ColumnTable.GetColumnId(connection, options.Column);

            if (columnId is null)
            {
                Console.WriteLine($"No column with the name {options.Column} exists.");
                return;
            }

            card.Column = new(options.Column);

            CardTable.UpdateCard(connection, card);

            Console.WriteLine($"Moved card {options.Id} to {options.Column}.");
        }

        /// <summary>
        /// Update a card on the Kanban board.
        /// </summary>
        public static void UpdateCard(UpdateCardOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            if (options.Priority is null)
            {
                var card = CardTable.GetCard(connection, options.Id);

                if (card is null)
                {
                    Console.WriteLine($"No card with ID {options.Id} exists.");
                    return;
                }

                if (options.Title is not null)
                {
                    card.Title = options.Title;
                }

                if (options.Description is not null)
                {
                    card.Description = options.Description;
                }

                CardTable.UpdateCard(connection, card);
            }
            else
            {
                var card = PriorityCardTable.GetCard(connection, options.Id);

                if (card is null)
                {
                    Console.WriteLine($"No priority card with ID {options.Id} exists.");
                    return;
                }

                if (options.Title is not null)
                {
                    card.Title = options.Title;
                }

                if (options.Description is not null)
                {
                    card.Description = options.Description;
                }

                card.Priority = options.Priority.Value;

                PriorityCardTable.UpdateCard(connection, card);
            }

            Console.WriteLine($"Updated card {options.Id}.");
        }

        public static void DeleteCard(DeleteCardOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            var card = CardTable.GetCard(connection, options.Id);

            if (card is null)
            {
                Console.WriteLine($"No card with ID {options.Id} exists.");
                return;
            }

            CardTable.DeleteCard(connection, options.Id);

            Console.WriteLine($"Deleted card {options.Id}.");
        }
        #endregion

        public static void QueryCards(QueryCardsOptions options)
        {
            var path = KanbanDatabase.GetDatabasePath(".");

            if (path is null)
            {
                PrintNoBoardMessage();
                return;
            }

            using var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            // No options specified, so query all cards.
            if (options.Text is null && options.Priority is null)
            {
                var cards = CardTable.GetCards(connection);

                foreach (var card in cards)
                {
                    Console.WriteLine(card);
                }
            }
            else if (options.Text is not null && options.Priority is not null)
            {
                var cards = PriorityCardTable.QueryCards(
                    connection,
                    options.Text,
                    options.Priority.Value
                );

                foreach (var card in cards)
                {
                    Console.WriteLine(card);
                }
            }
            else if (options.Text is not null)
            {
                var cards = CardTable.QueryCards(connection, options.Text);

                foreach (var card in cards)
                {
                    Console.WriteLine(card);
                }
            }
            else if (options.Priority is not null)
            {
                var cards = PriorityCardTable.QueryCards(connection, options.Priority.Value);

                foreach (var card in cards)
                {
                    Console.WriteLine(card);
                }
            }
        }
    }
}
