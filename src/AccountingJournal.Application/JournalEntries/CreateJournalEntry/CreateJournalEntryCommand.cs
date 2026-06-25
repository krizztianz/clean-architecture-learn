using AccountingJournal.Application.Abstractions.Messaging;
using AccountingJournal.Domain.JournalEntries;

namespace AccountingJournal.Application.JournalEntries.CreateJournalEntry;

public sealed record CreateJournalEntryCommand(
    DateOnly JournalDate,
    string Memo,
    string Currency,
    IReadOnlyList<CreateJournalEntryLineCommand> Lines
) : ICommand<Guid>;

public sealed record CreateJournalEntryLineCommand(
    string AccountCode,
    JournalLineSide Side,
    decimal Amount
);