using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Bookify.Application.Apartments.SearchApartments;
namespace Bookify.Infrastructure.Repositories;

internal sealed class BookingRepository:Repository<Booking>, IBookingRepository
{
    private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }


        public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration, CancellationToken cancellationToken)
        {
            return await Context.Set<Booking>()
                .AnyAsync(
                    booking =>
                        booking.ApartmentId == apartment.Id &&
                        booking.Duration.Start < duration.End &&  // Overlap check
                        booking.Duration.End > duration.Start
                        &&  // Overlap check
                         ActiveBookingStatuses.Contains((int)booking.Status)
                    , cancellationToken);
        }


}