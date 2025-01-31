using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Bookify.Application.Exception;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bookify.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred:{Message}",ex.Message);
                var exceptionDetails = GetExceptionDetails(ex);
                var problemDetails = new ProblemDetails
                {
                    Status = exceptionDetails.Status,
                    Type = exceptionDetails.Type,
                    Title = exceptionDetails.Title,
                    Detail = exceptionDetails.Detail
                };

                if (exceptionDetails.Errors is not null)
                {
                    problemDetails.Extensions["errors"] = exceptionDetails.Errors;
                }
                 context.Response.StatusCode= exceptionDetails.Status;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
        }

        private static ExceptionDetails GetExceptionDetails(Exception ex)
        {
            return ex switch
            {


                ValidationException validationExcxeption => new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "ValidationFailure",
                    "Validation error",
                    "One or more validation errors has occurred.",
                    ValidationException.Errors
                ),
                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "ServerError",
                    "Server error",
                    "An unexpected error occurred",
                    null

                    )
            };
        }

    }

    internal record ExceptionDetails(
            int Status,
            string Type,
            string Title,
            string Detail,
            IEnumerable<object> Errors
        );

}