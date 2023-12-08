using CommandLine;

namespace KanbanCli.Api
{
    [Verb("create-board", HelpText = "Create a new Kanban board.")]
    public struct CreateBoardOptions
    {
        [Option('n', "name", Required = true, HelpText = "Name of the board.")]
        public required string Name { get; set; }
    }

    #region Column Verbs
    [Verb("add-column", HelpText = "Add a column to the Kanban board.")]
    public struct AddColumnOptions
    {
        [Option('n', "name", Required = true, HelpText = "Name of the column.")]
        public required string Name { get; set; }
    }

    [Verb("rename-column", HelpText = "Rename a column on the Kanban board.")]
    public struct RenameColumnOptions
    {
        [Option('o', "old-name", Required = true, HelpText = "Old name of the column.")]
        public required string OldName { get; set; }

        [Option('n', "new-name", Required = true, HelpText = "New name of the column.")]
        public required string NewName { get; set; }
    }

    [Verb("delete-column", HelpText = "Delete a column from the Kanban board.")]
    public struct DeleteColumnOptions
    {
        [Option('n', "name", Required = true, HelpText = "Name of the column.")]
        public required string Name { get; set; }
    }
    #endregion

    #region Card Verbs
    [Verb("add-card", HelpText = "Add to the Kanban board.")]
    public struct AddCardOptions
    {
        [Option('t', "title", Required = true, HelpText = "Title of the card.")]
        public required string Title { get; set; }

        [Option('d', "description", Required = false, HelpText = "Description of the card.")]
        public string? Description { get; set; }

        [Option('c', "column", Required = true, HelpText = "Column to add the card to.")]
        public required string Column { get; set; }

        [Option('p', "priority", Required = false, HelpText = "Priority of the card.")]
        public int? Priority { get; set; }
    }

    [Verb("move-card", HelpText = "Move a card on the Kanban board.")]
    public struct MoveCardOptions
    {
        [Option('i', "id", Required = true, HelpText = "ID of the card.")]
        public required int Id { get; set; }

        [Option('c', "column", Required = true, HelpText = "Column to move the card to.")]
        public required string Column { get; set; }
    }

    [Verb("update-card", HelpText = "Update a card on the Kanban board.")]
    public struct UpdateCardOptions
    {
        [Option('i', "id", Required = true, HelpText = "ID of the card.")]
        public required int Id { get; set; }

        [Option('t', "title", Required = false, HelpText = "Title of the card.")]
        public string? Title { get; set; }

        [Option('d', "description", Required = false, HelpText = "Description of the card.")]
        public string? Description { get; set; }
        [Option('p', "priority", Required = false, HelpText = "Priority of the card.")]
        public int? Priority { get; set; }
    }

    [Verb("delete-card", HelpText = "Delete a card from the Kanban board.")]
    public struct DeleteCardOptions
    {
        [Option('i', "id", Required = true, HelpText = "ID of the card.")]
        public required int Id { get; set; }
    }
    #endregion

    [Verb("query-cards", HelpText = "Query cards on the Kanban board. If no options are provided, all cards will be returned.")]
    public struct QueryCardsOptions
    {
        [Option(
            't',
            "text",
            Required = false,
            HelpText = "Text to query cards from, e.g. title, description, or column."
        )]
        public string? Text { get; set; }

        [Option('p', "priority", Required = false, HelpText = "Priority to query cards from.")]
        public int? Priority { get; set; }
    }
}
