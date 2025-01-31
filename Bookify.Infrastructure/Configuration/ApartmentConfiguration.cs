using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configuration
{
    internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            // Configure the table name (if different from the entity name)
            builder.ToTable("apartments");

            // Configure the primary key
            builder.HasKey(a => a.Id);

            // Configure properties
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasConversion(name=>name.value,value=>new Name(value));  // Adjust the max length as needed

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(1000)
                .HasConversion(description=>description.value,value=>new Description(value));  // Adjust the max length as needed

            builder.OwnsOne(a => a.Price, pricebuilder =>
            {
                pricebuilder.Property( money => money.Currency).HasConversion(currency=>currency.Code,code=>Currency.FromCode(code));
            });
            builder.OwnsOne(a => a.CleaningFee, pricebuilder =>
            {
                pricebuilder.Property(money => money.Currency).HasConversion(currency=>currency.Code,code=>Currency.FromCode(code));
            });

            builder.Property<uint>("Version").IsRowVersion();


            // Configure any other specific rules based on the domain requirements
            // e.g. value conversions or unique constraints

            // Optionally, use value conversion if "Money" is a complex type and needs conversion
            // Example:
            // builder.Property(a => a.Price)
            //    .HasConversion(v => v.ToString(), v => new Money(decimal.Parse(v)));
        }
    }
}
