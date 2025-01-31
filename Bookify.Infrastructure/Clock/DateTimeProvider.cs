using Bookify.Application.Abstraction.Clock;

namespace Bookify.Infrastructure.Clock;

internal sealed  class DateTimeProvider :IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}