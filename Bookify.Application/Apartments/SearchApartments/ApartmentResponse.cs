using Bookify.Domain.Apartments;

namespace Bookify.Application.Apartments.SearchApartments;

public sealed class ApartmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Money Price { get; set; }
    public string Currency { get; set; }
    public AddressResponse Address { get; set; }
}