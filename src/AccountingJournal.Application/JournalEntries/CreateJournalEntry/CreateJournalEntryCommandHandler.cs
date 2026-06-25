using AccountingJournal.Application.Abstractions.Messaging;
using AccountingJournal.Application.Abstractions.Persistence;
using AccountingJournal.Domain.JournalEntries;
using AccountingJournal.Domain.ValueObjects;

namespace AccountingJournal.Application.JournalEntries.CreateJournalEntry;

public sealed class CreateJournalEntryCommandHandler
    : ICommandHandler<CreateJournalEntryCommand, Guid>
{
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateJournalEntryCommandHandler(
        IJournalEntryRepository journalEntryRepository,
        IUnitOfWork unitOfWork)
    {
        _journalEntryRepository = journalEntryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateJournalEntryCommand command,
        CancellationToken cancellationToken)
    {
        var journalEntry = JournalEntry.Create(
            command.JournalDate,
            command.Memo,
            command.Currency);

        foreach (var line in command.Lines)
        {
            var amount = Money.Of(line.Amount, command.Currency);

            if (line.Side == JournalLineSide.Debit)
            {
                journalEntry.AddDebitLine(line.AccountCode, amount);
            }
            else if (line.Side == JournalLineSide.Credit)
            {
                journalEntry.AddCreditLine(line.AccountCode, amount);
            }
            else
            {
                throw new InvalidOperationException("Journal line side is invalid.");
            }
        }

        await _journalEntryRepository.AddAsync(
            journalEntry,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return journalEntry.Id;
    }
}