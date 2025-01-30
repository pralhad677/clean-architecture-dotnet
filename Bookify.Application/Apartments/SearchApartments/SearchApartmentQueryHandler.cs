using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class SearchApartmentQueryHandler:IQueryHandler<SerachApartmentQuery,IReadOnlyList<ApartmentResponse>>
{
    private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };

    private readonly  ISqlConnectionFactory SqlConnectionFactory;

    public SearchApartmentQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        SqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IReadOnlyList<ApartmentResponse>> Handle(SerachApartmentQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return new  List<ApartmentResponse>().AsReadOnly();
        }

        using var connection = SqlConnectionFactory.CreateConnection();
        const string sql = """
                            SELECT
                            a.id AS Id,
                            a.name AS Name,
                            a.description AS Description,
                            a.price AS Price,
                            a.price_currency AS PriceCurrency,
                            a.address_country AS AddressCountry,
                            a.address_state AS AddressState,
                            a.address_zip_code  AS AddressZipCode,
                            a.address_city  AS AddressCity,
                            a.address_street AS AddressStreet 
                            FROM apartments as a
                            WHERE NOT EXISTS
                                (select  1
                                 FROM bookings as b 
                                 WHERE 
                                     b.apartment_id = a.Id AND
                                     
                                     b.duration_start <= @EndDate AND
                                     b.duration_end >= @StartDate AND
                                     b.status= Any(@ActiveBookingStatuses)
                                    )
                           """;
        var apartments = await connection.QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
            sql,
            (apartment, address) =>
            {
                apartment.Address = address;
                return apartment;
            },
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses
            },
            splitOn: "Country"
        );

        return apartments.ToList();
    }
}