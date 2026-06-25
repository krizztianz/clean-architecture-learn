using AccountingJournal.Application.Abstractions.Messaging;
using AccountingJournal.Application.Abstractions.Persistence;
using AccountingJournal.Application.Exceptions;

namespace AccountingJournal.Application.JournalEntries.PostJournalEntry;

public sealed class PostJournalEntryCommandHandler
    : ICommandHandler<PostJournalEntryCommand, bool>
{
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PostJournalEntryCommandHandler(
        IJournalEntryRepository journalEntryRepository,
        IUnitOfWork unitOfWork)
    {
        _journalEntryRepository = journalEntryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        PostJournalEntryCommand command,
        CancellationToken cancellationToken)
    {
        var journalEntry = await _journalEntryRepository.GetByIdAsync(
            command.JournalEntryId,
            cancellationToken);

        if (journalEntry is null)
            throw new NotFoundException("Journal entry was not found.");

        journalEntry.Post();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}