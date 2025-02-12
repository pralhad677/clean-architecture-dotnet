using Bookify.Application.Abstraction.Messaging;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

namespace Bookify.Application.Abstraction.Behavious;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly ILogger<  LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
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
        _logger.LogInformation("Executing request {Request}",name);
        var result = await next();
        if (result.IsSuccess)
        {
            _logger.LogInformation("  Request {Request} processed successfully",name);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {


                _logger.LogError("  Request {Request} failed with error {@Error}", name, result.Error);
            }
        }

        return result;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, " Request {Request} processing failed}",name);
            throw;
        }
    }
}