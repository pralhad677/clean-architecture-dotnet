using Bookify.Application.Abstraction.Messaging;
using MediatR;

namespace Bookify.Application.Bookings.GetBooking;

public record GetBookingQuery(Guid BookingId):IQuery<BookingResponse>, IRequest<BookingResponse>;