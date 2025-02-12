using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Apartments;

public record GetApartmentQuery(Guid Id):IRequest<Result< Guid>>
{

}