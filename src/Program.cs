using CommandLine;
using KanbanCli.Actions;
using KanbanCli.Api;

Parser
    .Default
    .ParseArguments<
        CreateBoardOptions,
        // Column Verbs
        AddColumnOptions,
        DeleteColumnOptions,
        RenameColumnOptions,
        // Card Verbs
        AddCardOptions,
        MoveCardOptions,
        UpdateCardOptions,
        DeleteCardOptions,
        // Query Cards
        QueryCardsOptions
    >(args)
    .WithParsed<CreateBoardOptions>(Actions.CreateBoard)
    // Column Verbs and Actions
    .WithParsed<AddColumnOptions>(Actions.AddColumn)
    .WithParsed<DeleteColumnOptions>(Actions.DeleteColumn)
    .WithParsed<RenameColumnOptions>(Actions.RenameColumn)
    // Card Verbs and Actions
    .WithParsed<AddCardOptions>(Actions.AddCard)
    .WithParsed<MoveCardOptions>(Actions.MoveCard)
    .WithParsed<UpdateCardOptions>(Actions.UpdateCard)
    .WithParsed<DeleteCardOptions>(Actions.DeleteCard)
    // Query Cards
    .WithParsed<QueryCardsOptions>(Actions.QueryCards);
