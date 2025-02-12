using Bookify.Domain.Apartments;

namespace Bookify.Domain.Users;

public interface IApartmentsRepository
{
    Task<Apartment> GetByIdAsync(Guid apartmentId, CancellationToken cancellationToken=default);

}