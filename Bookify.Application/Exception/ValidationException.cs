using Bookify.Application.Abstraction.Behavious;

namespace Bookify.Application.Exception;

public class ValidationException :System.Exception
{
    public   ValidationException(IEnumerable<ValidationError> validationErrors )
    {
        Errors = validationErrors;
    }

    public static IEnumerable<ValidationError> Errors { get; set; }


}