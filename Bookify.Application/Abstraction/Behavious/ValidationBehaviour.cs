using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Exception;
using FluentValidation;
using MediatR;
using ValidationException = Bookify.Application.Exception.ValidationException;

namespace Bookify.Application.Abstraction.Behavious;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>>  _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validator)
    {
        _validators = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
       var validationErrors = _validators.
           Select(validator => validator.Validate(context)).
           Where(result => result.Errors.Any())
           .SelectMany(result => result.Errors)
           .Select(failure =>new ValidationError(failure.PropertyName, failure.ErrorMessage) ).ToList();
        if (validationErrors.Any())
        {
            // Throw a custom validation exception or return the errors
            // throw new ValidationException()
            throw new ValidationException(validationErrors);
        }

        return await next();
    }
}