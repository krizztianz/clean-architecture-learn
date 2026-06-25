namespace AccountingJournal.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.", nameof(currency));

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }

    public static Money Of(decimal amount, string currency)
    {
        return new Money(amount, currency);
    }

    public static Money Idr(decimal amount)
    {
        return new Money(amount, "IDR");
    }

    public static Money Zero(string currency)
    {
        return new Money(0m, currency);
    }

    public static Money ZeroIdr()
    {
        return new Money(0m, "IDR");
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);

        return new Money(Amount - other.Amount, Currency);
    }

    public bool IsZero()
    {
        return Amount == 0m;
    }

    public bool IsPositive()
    {
        return Amount > 0m;
    }

    public bool IsNegative()
    {
        return Amount < 0m;
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot operate money with different currencies: {Currency} and {other.Currency}.");
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }
}