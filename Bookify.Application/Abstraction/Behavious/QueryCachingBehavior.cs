using Bookify.Application.Abstraction.Caching;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstraction.Behaviours
{
    internal sealed class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : Result
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;

        public QueryCachingBehavior(ICacheService cacheService, ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var cacheKey = request.CacheKey;
            var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey, cancellationToken);
            string name =typeof(TRequest).Name;
            if (cachedResponse is not null)
            {
                _logger.LogInformation("Cache hit for   {Query}", name);
                return cachedResponse;
            }

            _logger.LogInformation("Cache miss for key {Query} ", name);
            var response = await next();

            if (response.IsSuccess)
            {
                // var expiration = request.Expiration ?? TimeSpan.FromMinutes(5);
                await _cacheService.SetAsync(cacheKey, response, request.Expiration, cancellationToken);
                _logger.LogInformation("Cached response for key {CacheKey} with expiration {Expiration}", cacheKey,  request.Expiration);
            }

            return response;
        }
    }
}