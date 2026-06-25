namespace AccountingJournal.Application.Abstractions.Messaging;

public interface IApplicationMediator
{
    Task<TResult> Send<TCommand, TResult>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>;

    Task<TResult> Query<TQuery, TResult>(
        TQuery query,
        CancellationToken cancellationToken)
        where TQuery : IQuery<TResult>;
}