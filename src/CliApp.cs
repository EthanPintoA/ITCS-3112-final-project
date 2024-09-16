// using System.Diagnostics;

// using Microsoft.Data.Sqlite;

// using Command;
// using Database;
// using Models;

// namespace Itcs3112FinalProject
// {
//     class CliApp
//     {
//         public static void Run(string[] args)
//         {
//             if (args[0] == "new")
//             {
//                 var DbName = args[1] + ".sqlite3";
//                 // if file exists write error message and exit
//                 if (File.Exists(DbName))
//                 {
//                     Console.Error.WriteLine("File already exists");
//                     return;
//                 }
//                 else
//                 {
//                     // create new file
//                     using var connection = new SqliteConnection($"Data Source={DbName}.sqlite3");
//                     connection.Open();
//                     CardTable.CreateTable(connection);
//                 }
//             }
//             else
//             {
//                 var DbName = "kanban.sqlite3";

//                 using var connection = new SqliteConnection($"Data Source={DbName}");
//                 connection.Open();

//                 // Parse and execute command
//                 {
//                     var command = Parser.ParseCommand(args);

//                     if (command == null)
//                     {
//                         Console.Error.WriteLine("Unknown command");
//                     }
//                     else if (command is Create create)
//                     {
//                         var card = new Card(create.Title, create.Description, KanbanColumn.Todo);
//                         CardTable.AddCard(connection, card);
//                     }
//                     else if (command is Get get)
//                     {
//                         var card = CardTable.GetCard(connection, get.Id);
//                         Console.WriteLine(card);
//                     }
//                     else if (command is Delete delete)
//                     {
//                         CardTable.DeleteCard(connection, delete.Id);
//                     }
//                     else if (command is Update update)
//                     {
//                         var card = new Card(update.Id, update.Title, update.Description, update.Column);

//                         CardTable.UpdateCard(connection, card);
//                     }
//                     else
//                     {
//                         throw new UnreachableException($"Action for {command.GetType()} not implemented");
//                     }
//                 }

//                 // Print all cards
//                 {
//                     var command = connection.CreateCommand();
//                     command.CommandText = @"
//                         SELECT Id, Title, Description, Column
//                         FROM Cards
//                     ";
//                     using var reader = command.ExecuteReader();
//                     Console.WriteLine("All cards:");
//                     while (reader.Read())
//                     {
//                         var id = reader.GetInt32(0);
//                         var title = reader.GetString(1);
//                         var description = reader.GetString(2);
//                         var column = reader.GetString(3);
//                         Console.WriteLine(new Card(id, title, description, Enum.Parse<KanbanColumn>(column)));
//                     }
//                 }
//             }

//         }
//     }
// }
