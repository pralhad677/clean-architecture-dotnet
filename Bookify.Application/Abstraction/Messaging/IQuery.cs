using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstraction.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}