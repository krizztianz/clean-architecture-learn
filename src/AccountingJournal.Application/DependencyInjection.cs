using AccountingJournal.Application.Abstractions.Messaging;
using AccountingJournal.Application.JournalEntries.CreateJournalEntry;
using AccountingJournal.Application.JournalEntries.PostJournalEntry;
using AccountingJournal.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingJournal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IApplicationMediator, ApplicationMediator>();

        services.AddScoped<
            ICommandHandler<CreateJournalEntryCommand, Guid>,
            CreateJournalEntryCommandHandler>();

        services.AddScoped<
            ICommandHandler<PostJournalEntryCommand, bool>,
            PostJournalEntryCommandHandler>();

        return services;
    }
}