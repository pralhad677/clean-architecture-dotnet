using Serilog.Context;

namespace Bookify.API.Middleware;

public class RequestContextLoggingMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        using ( LogContext.PushProperty( "CorrelationId  ",GetCorrelationId(context)))
        {
            return _next(context);
        }
    }

    public static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);
        return correlationId.FirstOrDefault()??context.TraceIdentifier;
    }
}