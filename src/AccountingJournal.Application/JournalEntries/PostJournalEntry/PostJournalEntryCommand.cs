using AccountingJournal.Application.Abstractions.Messaging;

namespace AccountingJournal.Application.JournalEntries.PostJournalEntry;

public sealed record PostJournalEntryCommand(Guid JournalEntryId)
    : ICommand<bool>;