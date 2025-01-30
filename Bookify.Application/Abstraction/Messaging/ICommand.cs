using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstraction.Messaging
{
    public interface ICommand : IRequest<Result>, IBaseCommand
    {
        // Define members of ICommand here
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {
        // Define members of ICommand<TResponse> here
    }

    public interface IBaseCommand
    {
        // Define members of IBaseCommand here
    }
}