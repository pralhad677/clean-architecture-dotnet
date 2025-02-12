 using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;
using MediatR;
using Bookify.Domain.Apartments; // Ensure you have the Money and Currency classes in this namespace

namespace Bookify.Application.Apartments.SearchApartments
{
    internal sealed class SearchApartmentQueryHandler : IQueryHandler<SearchApartmentQuery, Result<IReadOnlyList<ApartmentResponse>>>
    {
        private static readonly int[] ActiveBookingStatuses =
        {
            (int)BookingStatus.Reserved,
            (int)BookingStatus.Confirmed,
            (int)BookingStatus.Completed
        };

        private readonly ISqlConnectionFactory SqlConnectionFactory;

        public SearchApartmentQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            SqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                return Result.Failure<IReadOnlyList<ApartmentResponse>>(Error.NullValue);
            }

            using var connection = SqlConnectionFactory.CreateConnection();
            const string sql = """
                               SELECT
                                   a.Id AS Id,  -- Use 'Id' for the apartment ID
                                   a.name AS Name,
                                   a.description AS Description,
                                   a.price AS Price,  
                                   a.address_country AS AddressCountry,
                                   a.address_state AS AddressState,
                                   a.Address_ZipCode AS AddressZipCode,
                                   a.address_city AS AddressCity,
                                   a.address_street AS AddressStreet
                               FROM apartments AS a
                               WHERE NOT EXISTS
                               (
                                   SELECT 1
                                   FROM bookings AS b
                                   WHERE
                                       b.ApartmentId = a.Id AND  -- Correct reference to 'Id' as 'ApartmentId'
                                       b.duration_start <= @EndDate AND
                                       b.duration_end >= @StartDate AND
                                       b.status IN @ActiveBookingStatuses
                               )
            """;

            var apartments = await connection.QueryAsync<ApartmentResponse, AddressResponse, decimal, ApartmentResponse>(
                sql,
                (apartment, address, price) =>
                {
                    // Set the address property of the apartment
                    apartment.Address = address;

                    // Convert the decimal price to Money (assuming Currency is EUR by default)
                    apartment.Price = new Money(price, Currency.Eur);

                    return apartment;
                },
                new
                {
                    StartDate = request.StartDate.ToDateTime(new TimeOnly(0, 0)),  // Convert StartDate to DateTime
                    EndDate = request.EndDate.ToDateTime(new TimeOnly(23, 59)),    // Convert EndDate to DateTime
                    ActiveBookingStatuses
                },
                splitOn: "AddressCountry"  // Split on AddressCountry to indicate where the AddressResponse starts
            );

            return Result.Success<IReadOnlyList<ApartmentResponse>>(apartments.ToList());
        }
    }
}
