using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings;

public static class BookingErrors
{
    public static Error NotFound = new(
        "Booking.Found",


        "The Booking with the specified Identified was not found. "
    );

    public static Error Overlap = new(
        "Booking.Overlap",


        "The current booking  is overlapping with existing one "
    );
    public static Error NotReserved = new(
        "Booking.NotReserved",


        "The current booking  is not pending "
    );
    public static Error NotConfirmed = new(
        "Booking.NotConfirmed",


        "The current booking  is NotConfirmed "
    );
    public static Error AlreadyStarted = new(
        "Booking.AlreadyStarted",


        "The current booking  has AlreadyStarted "
    );
}