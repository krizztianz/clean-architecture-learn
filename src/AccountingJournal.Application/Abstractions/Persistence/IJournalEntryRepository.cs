using AccountingJournal.Domain.JournalEntries;

namespace AccountingJournal.Application.Abstractions.Persistence;

public interface IJournalEntryRepository
{
    Task AddAsync(
        JournalEntry journalEntry,
        CancellationToken cancellationToken);

    Task<JournalEntry?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
}