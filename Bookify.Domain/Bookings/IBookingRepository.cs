using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;

namespace Bookify.Domain.Users;

public interface IBookingRepository
{
 //   bool async IsOverlappingAsync();
 void Add(Booking book );
 Task<Booking> GetByIdAsync(Guid userId,CancellationToken cancellationToken);
 Task<bool>   IsOverlappingAsync(Apartment apartment, int duration, CancellationToken cancellationToken);
}