using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Apartments.SearchApartments;

public record SearchApartmentQuery(
    DateOnly StartDate,
    DateOnly EndDate
) : IRequest<Result<IReadOnlyList<ApartmentResponse>>>;