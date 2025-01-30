using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments;

public class ApartmentErrors
{
    public static Error NotFound = new(
        "Apartment.Found",


        "The Apartment with the specified Identified was not found. "
    );
}