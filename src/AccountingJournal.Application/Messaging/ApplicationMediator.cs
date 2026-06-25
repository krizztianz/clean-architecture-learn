using AccountingJournal.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingJournal.Application.Messaging;

public sealed class ApplicationMediator : IApplicationMediator
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> Send<TCommand, TResult>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>
    {
        var handler = _serviceProvider
            .GetRequiredService<ICommandHandler<TCommand, TResult>>();

        return handler.Handle(command, cancellationToken);
    }

    public Task<TResult> Query<TQuery, TResult>(
        TQuery query,
        CancellationToken cancellationToken)
        where TQuery : IQuery<TResult>
    {
        var handler = _serviceProvider
            .GetRequiredService<IQueryHandler<TQuery, TResult>>();

        return handler.Handle(query, cancellationToken);
    }
}