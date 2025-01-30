namespace Bookify.Application.Abstraction.Behavious;

public record ValidationError(string PropertyName, string ErrorMessage);