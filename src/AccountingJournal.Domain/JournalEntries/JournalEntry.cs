using AccountingJournal.Domain.Exceptions;
using AccountingJournal.Domain.ValueObjects;

namespace AccountingJournal.Domain.JournalEntries;

public sealed class JournalEntry
{
    private readonly List<JournalLine> _lines = new();

    public Guid Id { get; private set; }
    public DateOnly JournalDate { get; private set; }
    public string Memo { get; private set; }
    public string Currency { get; private set; }
    public JournalStatus Status { get; private set; }

    public IReadOnlyCollection<JournalLine> Lines => _lines.AsReadOnly();

    private JournalEntry(
        Guid id,
        DateOnly journalDate,
        string memo,
        string currency)
    {
        if (id == Guid.Empty)
            throw new DomainException("Journal entry id is required.");

        if (journalDate == default)
            throw new DomainException("Journal date is required.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Journal currency is required.");

        Id = id;
        JournalDate = journalDate;
        Memo = memo?.Trim() ?? string.Empty;
        Currency = currency.Trim().ToUpperInvariant();
        Status = JournalStatus.Draft;
    }

    public static JournalEntry Create(
        DateOnly journalDate,
        string memo,
        string currency = "IDR")
    {
        return new JournalEntry(
            Guid.NewGuid(),
            journalDate,
            memo,
            currency);
    }

    public void AddDebitLine(string accountCode, Money amount)
    {
        AddLine(JournalLine.Debit(accountCode, amount));
    }

    public void AddCreditLine(string accountCode, Money amount)
    {
        AddLine(JournalLine.Credit(accountCode, amount));
    }

    public void AddLine(JournalLine line)
    {
        EnsureDraft();
        EnsureLineIsValidForThisJournal(line);

        _lines.Add(line);
    }

    public void RemoveLine(Guid lineId)
    {
        EnsureDraft();

        var line = _lines.FirstOrDefault(x => x.Id == lineId);

        if (line is null)
            throw new DomainException("Journal line was not found.");

        _lines.Remove(line);
    }

    public Money GetTotalDebit()
    {
        return SumLinesBySide(JournalLineSide.Debit);
    }

    public Money GetTotalCredit()
    {
        return SumLinesBySide(JournalLineSide.Credit);
    }

    public bool IsBalanced()
    {
        return GetTotalDebit() == GetTotalCredit();
    }

    public void Post()
    {
        EnsureDraft();

        if (_lines.Count < 2)
            throw new DomainException("Journal entry must have at least two lines.");

        if (!IsBalanced())
            throw new DomainException("Journal entry is not balanced.");

        Status = JournalStatus.Posted;
    }

    public void Reverse()
    {
        if (Status != JournalStatus.Posted)
            throw new DomainException("Only posted journal entry can be reversed.");

        Status = JournalStatus.Reversed;
    }

    private void EnsureDraft()
    {
        if (Status != JournalStatus.Draft)
            throw new DomainException("Only draft journal entry can be modified.");
    }

    private void EnsureLineIsValidForThisJournal(JournalLine line)
    {
        if (line is null)
            throw new DomainException("Journal line is required.");

        if (line.Amount.Currency != Currency)
            throw new DomainException(
                $"Journal line currency must be {Currency}.");
    }

    private Money SumLinesBySide(JournalLineSide side)
    {
        var total = Money.Zero(Currency);

        foreach (var line in _lines.Where(x => x.Side == side))
        {
            total = total.Add(line.Amount);
        }

        return total;
    }
}