using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments;

public sealed class Apartment :Entity
{
    public Apartment(){}
    public Apartment(Guid Id):base(Id)
    {

    }

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public Money CleaningFee { get; private set; }


    public DateTime? LastBookedOnTuc {get; internal set;}
    public List<Amenity> Amenities { get; private set; } = new();







}