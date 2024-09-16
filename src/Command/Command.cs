// using Models;

// namespace Command
// {
//     public class Parser
//     {
//         public static ICommand? ParseCommand(string[] args)
//         {
//             return args[0] switch
//             {
//                 "create" => new Create(args[1], args[2]),
//                 "get" => new Get(int.Parse(args[1])),
//                 "delete" => new Delete(int.Parse(args[1])),
//                 "update" => new Update(int.Parse(args[1]), args[2], args[3], Enum.Parse<KanbanColumn>(args[4])),
//                 _ => null
//             };
//         }
//     }

//     public interface ICommand { }

//     public class Create : ICommand
//     {
//         public string Title { get; set; }
//         public string Description { get; set; }

//         public Create(string title, string description)
//         {
//             Title = title;
//             Description = description;
//         }
//     }

//     public class Get : ICommand
//     {
//         public int Id { get; set; }

//         public Get(int id)
//         {
//             Id = id;
//         }
//     }

//     public class Delete : ICommand
//     {
//         public int Id { get; set; }

//         public Delete(int id)
//         {
//             Id = id;
//         }
//     }

//     public class Update : ICommand
//     {
//         public int Id { get; set; }
//         public string Title { get; set; }
//         public string Description { get; set; }
//         public KanbanColumn Column { get; set; }

//         public Update(int id, string title, string description, KanbanColumn column)
//         {
//             Id = id;
//             Title = title;
//             Description = description;
//             Column = column;
//         }
//     }
// }
