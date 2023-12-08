# ITCS 3112 Final Project / Kanban Board CLI

## Description

This is a CLI Kanban Board that allows users to:

- Create a kanban board (if one does not already exist).
- Add, query, update, and delete cards and priority cards.
- Add, rename, and delete columns.
- Move cards between columns.

## Notes

- I'm using a `PriorityCard` class to represent cards with a priority. This is a subclass of the `Card` class.
- Because of query limitations, using the `query-cards` will print priority cards as regular cards if a specific priority is not specified in the query.
- Using `help` and `--help` should provide enough information on how to use the program. Error messages should also be descriptive enough to help the user understand what went wrong.

## How to Run

### Prerequisites

- I'm using .NET 7.0.404. Other versions may work, but I haven't tested them.
- I'm using libraries specified in the `KanbanBoard.csproj` file. They should be installed automatically when you build the project.

### Commands

```
dotnet run -- [args]
dotnet run --no-build -- [args]
```

Note that the `--no-build` flag is faster after you already built the program. Also note that the `--` is necessary to separate the `dotnet run` command from the arguments.

#### Example:

```
dotnet run -- help
dotnet run -- create-board
dotnet run -- add-column --name "My Column"
dotnet run -- add-card -t "My Card" -d "This is my card" -p 1 -c "Done"
dotnet run --no-build -- query-cards
```
