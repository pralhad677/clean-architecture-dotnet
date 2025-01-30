using Bookify.Application.Abstraction.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstraction.Behavious;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly ILogger< TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).Name;
        try
        {
        _logger.LogInformation("Executing command {Command}",name);
        var result = await next();
        _logger.LogInformation("  command {Command} processed successfully",name);
        return result;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, " Command {Command} processing failed}");
            throw;
        }
    }
}