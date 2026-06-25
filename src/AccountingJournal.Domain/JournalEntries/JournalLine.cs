using AccountingJournal.Domain.Exceptions;
using AccountingJournal.Domain.ValueObjects;

namespace AccountingJournal.Domain.JournalEntries;

public sealed class JournalLine
{
    public Guid Id { get; private set; }
    public string AccountCode { get; private set; }
    public JournalLineSide Side { get; private set; }
    public Money Amount { get; private set; }

    private JournalLine(
        Guid id,
        string accountCode,
        JournalLineSide side,
        Money amount)
    {
        if (id == Guid.Empty)
            throw new DomainException("Journal line id is required.");

        if (string.IsNullOrWhiteSpace(accountCode))
            throw new DomainException("Account code is required.");

        if (!Enum.IsDefined(typeof(JournalLineSide), side))
            throw new DomainException("Journal line side is invalid.");

        if (amount is null)
            throw new DomainException("Journal line amount is required.");

        if (!amount.IsPositive())
            throw new DomainException("Journal line amount must be greater than zero.");

        Id = id;
        AccountCode = accountCode.Trim();
        Side = side;
        Amount = amount;
    }

    public static JournalLine Debit(string accountCode, Money amount)
    {
        return new JournalLine(
            Guid.NewGuid(),
            accountCode,
            JournalLineSide.Debit,
            amount);
    }

    public static JournalLine Credit(string accountCode, Money amount)
    {
        return new JournalLine(
            Guid.NewGuid(),
            accountCode,
            JournalLineSide.Credit,
            amount);
    }

    public bool IsDebit()
    {
        return Side == JournalLineSide.Debit;
    }

    public bool IsCredit()
    {
        return Side == JournalLineSide.Credit;
    }
}