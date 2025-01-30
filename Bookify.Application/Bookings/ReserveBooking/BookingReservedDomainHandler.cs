using Bookify.Application.Abstraction.Email;
using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using MediatR;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class BookingReservedDomainHandler :INotificationHandler<BookingReservedDomainEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public BookingReservedDomainHandler(
        IBookingRepository repository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        _bookingRepository = repository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(notification.BookingId,cancellationToken);
        if (booking == null)
        {
            return;
        }
        var user  = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);
        if (user is null)
        {
            return;
        }

        await _emailService.SendAsync(

            user.Email,
            "Booking Reserved",

            "You have 10 minutes to confirm this booking"
        );
    }
}