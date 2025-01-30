using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstraction.Messaging
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
    {
        // Define members of ICommandHandler here
    }

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
    {
        // Define members of ICommandHandler<TCommand, TResponse> here
    }
}