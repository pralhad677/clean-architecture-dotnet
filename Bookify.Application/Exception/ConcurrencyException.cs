namespace Bookify.Application.Exception;

public sealed class ConcurrencyException:System.Exception
{
    public ConcurrencyException(string message,System.Exception innerException )
    : base(message, innerException)
    {

    }

}