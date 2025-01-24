namespace Bookify.Domain.Apartments;

public record Address(
    string Country,
    string State,
    string City,
    string ZipCode,
    string Street

    )
    {}