using Bookify.Domain.Apartments;
using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories;

internal sealed class ApartmentRepository: Repository<Apartment>,IApartmentsRepository
{
    public ApartmentRepository(ApplicationDbContext context) : base(context)
    {
    }

}