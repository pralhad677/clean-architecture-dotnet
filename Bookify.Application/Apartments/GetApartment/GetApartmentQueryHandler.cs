using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;

namespace Bookify.Application.Apartments;

public class GetApartmentQueryHandler:IQueryHandler<GetApartmentQuery,Result< Guid>>
{
    private readonly IApartmentsRepository _repository;

    public GetApartmentQueryHandler(IApartmentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
    {
             var apartment = await _repository.GetByIdAsync(request.Id, cancellationToken);
             return apartment.Id;
    }
}