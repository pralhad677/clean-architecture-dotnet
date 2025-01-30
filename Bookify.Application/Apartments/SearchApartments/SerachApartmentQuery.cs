using Bookify.Application.Abstraction.Messaging;
using MediatR;

namespace Bookify.Application.Apartments.SearchApartments;

public record SerachApartmentQuery(
        DateOnly StartDate,
        DateOnly EndDate
    ):IQuery<IReadOnlyList<ApartmentResponse>>, IRequest<IReadOnlyList<ApartmentResponse>>;