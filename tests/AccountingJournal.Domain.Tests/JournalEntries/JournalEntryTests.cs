using AccountingJournal.Domain.Exceptions;
using AccountingJournal.Domain.JournalEntries;
using AccountingJournal.Domain.ValueObjects;

namespace AccountingJournal.Domain.Tests.JournalEntries;

public sealed class JournalEntryTests
{
    [Fact]
    public void Create_ShouldCreateDraftJournalEntry()
    {
        var journal = JournalEntry.Create(
            new DateOnly(2026, 6, 25),
            "Pembayaran sewa kantor");

        Assert.NotEqual(Guid.Empty, journal.Id);
        Assert.Equal(new DateOnly(2026, 6, 25), journal.JournalDate);
        Assert.Equal("Pembayaran sewa kantor", journal.Memo);
        Assert.Equal("IDR", journal.Currency);
        Assert.Equal(JournalStatus.Draft, journal.Status);
        Assert.Empty(journal.Lines);
    }

    [Fact]
    public void Post_WhenJournalIsBalanced_ShouldChangeStatusToPosted()
    {
        var journal = JournalEntry.Create(
            new DateOnly(2026, 6, 25),
            "Pembayaran sewa kantor");

        journal.AddDebitLine("6100", Money.Idr(5_000_000));
        journal.AddCreditLine("1100", Money.Idr(5_000_000));

        journal.Post();

        Assert.Equal(JournalStatus.Posted, journal.Status);
    }

    [Fact]
    public void Post_WhenJournalIsNotBalanced_ShouldThrowDomainException()
    {
        var journal = JournalEntry.Create(
            new DateOnly(2026, 6, 25),
            "Pembayaran sewa kantor");

        journal.AddDebitLine("6100", Money.Idr(5_000_000));
        journal.AddCreditLine("1100", Money.Idr(4_000_000));

        var exception = Assert.Throws<DomainException>(() => journal.Post());

        Assert.Equal("Journal entry is not balanced.", exception.Message);
        Assert.Equal(JournalStatus.Draft, journal.Status);
    }

    [Fact]
    public void Post_WhenJournalHasLessThanTwoLines_ShouldThrowDomainException()
    {
        var journal = JournalEntry.Create(
            new DateOnly(2026, 6, 25),
            "Incomplete journal");

        journal.AddDebitLine("6100", Money.Idr(5_000_000));

        var exception = Assert.Throws<DomainException>(() => journal.Post());

        Assert.Equal("Journal entry must have at least two lines.", exception.Message);
        Assert.Equal(JournalStatus.Draft, journal.Status);
    }

    [Fact]
    public void AddLine_WhenJournalAlreadyPosted_ShouldThrowDomainException()
    {
        var journal = JournalEntry.Create(
            new DateOnly(2026, 6, 25),
            "Pembayaran sewa kantor");

        journal.AddDebitLine("6100", Money.Idr(5_000_000));
        journal.AddCreditLine("1100", Money.Idr(5_000_000));
        journal.Post();

        var exception = Assert.Throws<DomainException>(
            () => journal.AddDebitLine("6200", Money.Idr(1_000_000)));

        Assert.Equal("Only draft journal entry can be modified.", exception.Message);
    }

    [Fact]
    public void Reverse_WhenJournalIsPosted_ShouldChangeStatusToReversed()
    {
        var journal = JournalEntry.Create(
            new DateOnly(2026, 6, 25),
            "Pembayaran sewa kantor");

        journal.AddDebitLine("6100", Money.Idr(5_000_000));
        journal.AddCreditLine("1100", Money.Idr(5_000_000));
        journal.Post();

        journal.Reverse();

        Assert.Equal(JournalStatus.Reversed, journal.Status);
    }

    [Fact]
    public void Reverse_WhenJournalIsStillDraft_ShouldThrowDomainException()
    {
        var journal = JournalEntry.Create(
            new DateOnly(2026, 6, 25),
            "Draft journal");

        var exception = Assert.Throws<DomainException>(() => journal.Reverse());

        Assert.Equal("Only posted journal entry can be reversed.", exception.Message);
        Assert.Equal(JournalStatus.Draft, journal.Status);
    }
}