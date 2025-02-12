using System.Diagnostics.CodeAnalysis;

namespace Bookify.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        // Validate the combination of isSuccess and error
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("A successful result cannot have an error.");
        }
        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("A failure result must have an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    // Factory method for success
    public static Result Success() => new(true, Error.None);

    // Factory method for failure
    public static Result Failure(Error error) => new(false, error);

    // Factory method for success with a value
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    // Factory method for failure with a value
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    // Factory method to create a result based on a nullable value
    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    // Implicit conversion from TValue to Result<TValue>
    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}