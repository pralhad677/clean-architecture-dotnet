using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configuration;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        // Configure the primary key for the Booking entity
        builder.HasKey(b => b.Id);

        // Configure the relationships (foreign keys)
        builder.HasOne<User>()  // Booking has one User (Foreign Key)
            .WithMany()  // Assumes a User can have many Bookings
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Defines cascading delete behavior (optional)

        builder.HasOne<Apartment>()  // Booking has one Apartment (Foreign Key)
            .WithMany()  // Assumes an Apartment can have many Bookings
            .HasForeignKey(b => b.ApartmentId)
            .OnDelete(DeleteBehavior.Restrict);  // Defines cascading delete behavior (optional)

        // Configure properties
        builder.Property(b => b.ApartmentId)
            .IsRequired()
            ;

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.Duration)
            .IsRequired();  // Assuming DateRange is a complex type that is correctly mapped

        builder.Property(b => b.PriceForPeriod)
            .IsRequired();  // Assuming Money is a complex type or value object

        builder.OwnsOne(booking => booking.CleaningFee, pricebuilder =>
        {
            pricebuilder.Property(money => money.Currency).HasConversion(currency=>currency.Code,code=>Currency.FromCode(code));
        });

        builder.OwnsOne(booking => booking.AmenitiesUpCharge, pricebuilder =>
        {
            pricebuilder.Property(money => money.Currency).HasConversion(currency=>currency.Code,code=>Currency.FromCode(code));
        });

        builder.OwnsOne(booking => booking.TotalPrice, pricebuilder =>
        {
            pricebuilder.Property(money => money.Currency).HasConversion(currency=>currency.Code,code=>Currency.FromCode(code));
        });

        // builder.OwnsOne(booking => booking.AmenitiesUpCharge, pricebuilder =>
        // {
        //     pricebuilder.Property(money => money.Currency).HasConversion(currency=>currency.Code,code=>Currency.FromCode(code));
        // });


        builder.HasOne(booking => booking.Duration);
        builder.Property(b => b.Status)
            .IsRequired();  // Assuming BookingStatus is an enum

        builder.Property(b => b.CreatedOnUtc)
            .IsRequired();

        builder.Property(b => b.ConfirmedOnUtc)
            .IsRequired(false);  // Nullable property

        builder.Property(b => b.RejectedOnUtc)
            .IsRequired(false);  // Nullable property

        builder.Property(b => b.CompletedOnUtc)
            .IsRequired(false);  // Nullable property

        builder.Property(b => b.CanceledOnUtc)
            .IsRequired(false);  // Nullable property

        // Optionally, add indexes on frequently queried fields
        builder.HasIndex(b => b.UserId);  // Index for UserId
        builder.HasIndex(b => b.ApartmentId);  // Index for ApartmentId
    }
}
