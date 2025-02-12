using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Abstraction.Caching;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;


public interface ICachedQuery //use this in place of query of cqrs  instead of IQuery<BookingResponse> ::time 13.09
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}

