using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bogus;
using Bookify.Application.Abstraction.Data;
using Dapper;

namespace Bookify.API.Extensions
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var sqlConnectionFactory = serviceScope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using var connection = sqlConnectionFactory.CreateConnection();

            var currencies = new[] { Currency.Usd, Currency.Eur };

            var faker = new Faker<Apartment>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Name, f => new Name(f.Company.CompanyName()))
                .RuleFor(a => a.Description, f => new Description(f.Lorem.Sentence()))
                .RuleFor(a => a.Address, f => new Address(
                    f.Address.Country(),
                    f.Address.State(),
                    f.Address.City(),
                    f.Address.ZipCode(),
                    f.Address.StreetAddress()
                ))
                .RuleFor(a => a.Price, f => new Money(f.Finance.Amount(500, 5000), f.PickRandom(currencies)))
                .RuleFor(a => a.CleaningFee, f => new Money(f.Finance.Amount(50, 200), f.PickRandom(currencies)))
                .RuleFor(a => a.LastBookedOnTuc, f => f.Date.Past())
                .RuleFor(a => a.Amenities, f => new List<Amenity>
                {
                    f.PickRandom<Amenity>(),
                    f.PickRandom<Amenity>()
                });

            List<Apartment> apartments = new();
            for (var i = 1; i <= 100; i++)
            {
                apartments.Add(faker.Generate());
            }

            const string sql = """
                                  INSERT INTO [dbo].[Apartments]
                                  (id, name, description, address_country, address_state, address_city, address_zipcode, address_street, price, cleaningfee, lastbookedontuc, amenities, version)
                                  VALUES (@Id, @Name, @Description, @Country, @State, @City, @ZipCode, @Street, @Price, @CleaningFee, @LastBookedOnTuc, @Amenities, @Version)
                               """;

            var flattenedApartments = apartments.Select(apartment => new
            {
                Id = apartment.Id,
                Name = apartment.Name.value,
                Description = apartment.Description.value,
                Country = apartment.Address.Country,
                State = apartment.Address.State,
                City = apartment.Address.City,
                ZipCode = apartment.Address.ZipCode,
                Street = apartment.Address.Street,
                Price = System.Text.Json.JsonSerializer.Serialize(new
                {
                    Amount = apartment.Price.Amount,
                    Currency = apartment.Price.Currency.Code
                }),
                CleaningFee = System.Text.Json.JsonSerializer.Serialize(new
                {
                    Amount = apartment.CleaningFee.Amount,
                    Currency = apartment.CleaningFee.Currency.Code
                }),
                LastBookedOnTuc = apartment.LastBookedOnTuc,
                Amenities = string.Join(",", apartment.Amenities.Select(a => a.ToString())),
                Version = 1 // Assuming you have a Version property
            });

            connection.Execute(sql, flattenedApartments);
        }
    }
}
