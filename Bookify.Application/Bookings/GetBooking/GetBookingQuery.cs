using Bookify.Application.Abstraction.Caching;
using Bookify.Application.Abstraction.Messaging;
using MediatR;

namespace Bookify.Application.Bookings.GetBooking;

public record GetBookingQuery(Guid BookingId):ICachedQuery<BookingResponse>, IRequest<BookingResponse>
{
    public string CacheKey => $"{nameof(GetBookingQuery)}-{BookingId}";
    public TimeSpan? Expiration => null;
}